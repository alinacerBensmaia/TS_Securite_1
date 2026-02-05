using Rrq.InfrastructureCommune.ScenarioTransactionnel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace TS5N211_CbFonctionCommune.AccessBD
{
    /// <summary>
    /// Les Sens possibles de la Confrontation de données
    /// </summary>
    public static class SensConfrontation
    {
        public const string SituationDeReference = "Reference";
        public const string SituationCourante = "Courante";
    }
    /// <summary>
    /// Les Type de Rapports générés par la Confrontation de données
    /// </summary>
    public static class TypeRapportConfrontation
    {
        public const string Audit = "Audit";
        public const string Autorisation = "Autorisation";
        public const string Proprietaire = "Proprietaire";
    }
    public class TsCaSituationDeReference
    {
        private string _restrictionConfrontation;

        /// <summary>
        /// Fonction générant la clause SQL restraignant la confrontation des données, entre les situations Courante et de Référence, aux seuls chemins passés en paramètre de la chaine TS5HA pour Analyse
        /// </summary>
        /// <param name="listeDesRepertoiresDAnalyse">Tableau des chemins à analyser passés en paramètre</param>
        private void GenererLesRestrictionsDeConfrontation(string[] listeDesRepertoiresDAnalyse)
        {
            StringBuilder lesRestrictions = new StringBuilder();
            lesRestrictions.Append(" (");

            for (int i = 0; i < listeDesRepertoiresDAnalyse.Length; i++)
            {
                if (i == 0)
                {
                    lesRestrictions.Append($"UPPER(a.{TsCaTablesChamps.VL_CHE_NTF}) like UPPER({TsCaOutils.DoubleQuote(listeDesRepertoiresDAnalyse[i] + "%")})");
                }
                else
                    lesRestrictions.Append($" OR UPPER(a.{TsCaTablesChamps.VL_CHE_NTF}) like UPPER({TsCaOutils.DoubleQuote(listeDesRepertoiresDAnalyse[i] + "%")})");
            }

            lesRestrictions.Append(") ");

            _restrictionConfrontation = lesRestrictions.ToString();
        }

        /// <summary>
        /// Fonction de constitution de la situation de Référence par transfert de données ciblées à partir
        /// </summary>
        public void ReconstituerSituationDeReference(List<string> lesRacinesCiblees)
        {
            StringBuilder sRequete = new StringBuilder();

            try
            {
                //Commencer par faire Table Rase
                TableRase();

                //Transférer les données raçine par raçine
                using (TransactionScope tScope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(180)))
                {
                    using (SqlConnection connection = TsCaOutils.ConnexionSql)
                    {
                        foreach (var racineEnCours in lesRacinesCiblees)
                        {
                            if (racineEnCours.Trim().Length != 0)
                            {
                                SqlCommand commandeSql = connection.CreateCommand();
                                commandeSql.CommandType = CommandType.Text;
                                //INSERT INTO
                                sRequete.Append($"INSERT INTO {TsCaTablesChamps.TableDeReference} ");
                                sRequete.Append("(");
                                sRequete.Append($"{TsCaTablesChamps.VL_TYP_SEC_NTF}, ");
                                sRequete.Append($"{TsCaTablesChamps.VL_CHE_NTF}, ");
                                sRequete.Append($"{TsCaTablesChamps.VL_IDN_NTF}, ");
                                sRequete.Append($"{TsCaTablesChamps.VL_TYP_ACC_NTF}, ");
                                sRequete.Append($"{TsCaTablesChamps.IN_DRO_ACC_HET_NTF}, ");
                                sRequete.Append($"{TsCaTablesChamps.VL_PER_NTF}, ");
                                sRequete.Append($"{TsCaTablesChamps.DH_ANL_NTF}, ");
                                sRequete.Append($"{TsCaTablesChamps.NM_PRO_NTF}, ");
                                sRequete.Append($"{TsCaTablesChamps.VL_TYP_PRO_ENF_NTF}, ");
                                sRequete.Append($"{TsCaTablesChamps.VL_TYP_HER_NTF}");
                                sRequete.Append(")");

                                //SELECT 
                                sRequete.Append(" select ");
                                sRequete.Append($"{TsCaTablesChamps.VL_TYP_SEC_NTF}, ");
                                sRequete.Append($"{TsCaTablesChamps.VL_CHE_NTF}, ");
                                sRequete.Append($"{TsCaTablesChamps.VL_IDN_NTF}, ");
                                sRequete.Append($"{TsCaTablesChamps.VL_TYP_ACC_NTF}, ");
                                sRequete.Append($"{TsCaTablesChamps.IN_DRO_ACC_HET_NTF}, ");
                                sRequete.Append($"{TsCaTablesChamps.VL_PER_NTF}, ");
                                sRequete.Append($"{TsCaTablesChamps.DH_ANL_NTF}, ");
                                sRequete.Append($"{TsCaTablesChamps.NM_PRO_NTF}, ");
                                sRequete.Append($"{TsCaTablesChamps.VL_TYP_PRO_ENF_NTF}, ");
                                sRequete.Append($"{TsCaTablesChamps.VL_TYP_HER_NTF} ");
                                sRequete.Append($"from {TsCaTablesChamps.SECNTFS} ");

                                //WHERE
                                sRequete.Append($" WHERE UPPER({TsCaTablesChamps.VL_CHE_NTF}) like UPPER({TsCaOutils.DoubleQuote(racineEnCours + "%")})");

                                commandeSql.CommandText = sRequete.ToString();

                                sRequete.Clear();

                                commandeSql.ExecuteNonQuery();
                                commandeSql.Dispose();
                            }
                        }
                    }

                    tScope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw new XuExcEErrFatale("ERREUR --> ConstituerSituationDeReference(): " +
                                          ex.Message +
                                          Environment.NewLine);
            }

        }

        /// <summary>
        /// Fonction de remise à blanc de la Table de Référence en vue de sa reconstitution 
        /// </summary>
        private void TableRase()
        {
            StringBuilder sRequete = new StringBuilder();
            sRequete.Append($"DELETE TOP({TsCaOutils.TailleBatchSQL}) FROM {TsCaTablesChamps.TableDeReference} ");

            try
            {
                using (TransactionScope tScope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(180)))
                {
                    using (SqlConnection connection = TsCaOutils.ConnexionSql)
                    {
                        int retourSuppression = TsCaOutils.TailleBatchSQL;

                        while (retourSuppression == TsCaOutils.TailleBatchSQL)
                        {
                            SqlCommand commandeSql = connection.CreateCommand();
                            commandeSql.CommandType = CommandType.Text;

                            commandeSql.CommandText = sRequete.ToString();

                            retourSuppression = commandeSql.ExecuteNonQuery();
                            commandeSql.Dispose();
                        }
                        
                    }

                    tScope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw new XuExcEErrFatale("ERREUR --> ConstituerSituationDeReference(): " +
                                          ex.Message +
                                          Environment.NewLine);
            }

        }

        /// <summary>
        /// Fonction invoquant les divers Rapports de Confrontation entre la situation de Référence et celle Courante.
        /// </summary>
        public void CollecterLesDonneesDesRapportsDeConfrontation(string[] listeCheminAAnalyser)
        {
            try
            {
                var lesRapports = CreerDataTableDesRapports();

                //Générer la clause de restriction
                GenererLesRestrictionsDeConfrontation(listeCheminAAnalyser);

                //Récupération des résultats pour le Rapport Autorisation, sens Situation Courante
                lesRapports.Merge(GenererRapportAutorisation(SensConfrontation.SituationCourante));
                //Récupération des résultats pour le Rapport Autorisation, sens Situation de Référence
                lesRapports.Merge(GenererRapportAutorisation(SensConfrontation.SituationDeReference));

                //Récupération des résultats pour le Rapport Audit, sens Situation Courante
                lesRapports.Merge(GenererRapportAudit(SensConfrontation.SituationCourante));
                //Récupération des résultats pour le Rapport Audit, sens Situation de Référence
                lesRapports.Merge(GenererRapportAudit(SensConfrontation.SituationDeReference));

                //Récupération des résultats pour le Rapport Proprietaire, sens Situation Courante
                lesRapports.Merge(GenererRapportProprietaire(SensConfrontation.SituationCourante));
                //Récupération des résultats pour le Rapport Proprietaire, sens Situation de Référence
                lesRapports.Merge(GenererRapportProprietaire(SensConfrontation.SituationDeReference));

                //Sauvegarde des enregistrements collectés
                EnregistrerDonneesRapportsCollectes(lesRapports);
            }
            catch (Exception ex)
            {
                throw new XuExcEErrFatale("ERREUR --> CollecterLesDonneesDesRapportsDeConfrontation(): " +
                                         ex.Message +
                                         Environment.NewLine);
            }
        }

        /// <summary>
        /// Génération des données du Rapport des Autorisation
        /// </summary>
        private DataTable GenererRapportAutorisation(string sensConfrontation)
        {
            try
            {
                string tableVisee, tableDeConfrontation;

                //Selon le sens attendu, fixer les tables
                switch (sensConfrontation)
                {
                    case SensConfrontation.SituationCourante:
                        tableVisee = TsCaTablesChamps.SECNTFS;
                        tableDeConfrontation = TsCaTablesChamps.TableDeReference;
                        break;
                    case SensConfrontation.SituationDeReference:
                        tableVisee = TsCaTablesChamps.TableDeReference;
                        tableDeConfrontation = TsCaTablesChamps.SECNTFS;
                        break;
                    default:
                        throw new Exception("Sens de confrontation inconnu.");
                }

                //Construction de la requête de confrontation du type rapport Autorisation
                //confrontation sur un nombre de champs bien précis
                //sélection de l'ensemble des champs de la table visée
                StringBuilder sRequete = new StringBuilder();
                sRequete.Append("select VL_TYP_SEC_NTF, VL_CHE_NTF, VL_IDN_NTF, VL_TYP_ACC_NTF, IN_DRO_ACC_HET_NTF, VL_PER_NTF, NM_PRO_NTF, VL_TYP_PRO_ENF_NTF, VL_TYP_HER_NTF, ");
                sRequete.Append($"'{sensConfrontation}' as VL_SEN_CON_NTF, '{TypeRapportConfrontation.Autorisation}' as VL_TYP_RAP_NTF, GETDATE() as DH_CON_NTF, DH_ANL_NTF ");
                sRequete.Append($"from {tableVisee} as a ");
                sRequete.Append("where ");
                if (!string.IsNullOrWhiteSpace(_restrictionConfrontation))
                    sRequete.Append($"{_restrictionConfrontation} and ");
                sRequete.Append($"a.VL_TYP_SEC_NTF = '{TypeRapportConfrontation.Autorisation}' and not exists(");
                sRequete.Append("select b.VL_TYP_SEC_NTF, b.VL_CHE_NTF, b.VL_IDN_NTF, b.VL_TYP_ACC_NTF, b.IN_DRO_ACC_HET_NTF, b.VL_PER_NTF, b.VL_TYP_PRO_ENF_NTF, b.VL_TYP_HER_NTF ");
                sRequete.Append($"from {tableDeConfrontation} as b ");
                sRequete.Append("where b.VL_TYP_SEC_NTF = a.VL_TYP_SEC_NTF and ");
                sRequete.Append("b.VL_CHE_NTF = a.VL_CHE_NTF and b.VL_IDN_NTF = a.VL_IDN_NTF and ");
                sRequete.Append("b.VL_TYP_ACC_NTF = a.VL_TYP_ACC_NTF and b.IN_DRO_ACC_HET_NTF = a.IN_DRO_ACC_HET_NTF and ");
                sRequete.Append("b.VL_PER_NTF = a.VL_PER_NTF and b.VL_TYP_PRO_ENF_NTF = a.VL_TYP_PRO_ENF_NTF and ");
                sRequete.Append("b.VL_TYP_HER_NTF = a.VL_TYP_HER_NTF)");

                //Exécution et retour du résultat
                var res = TsCaOutils.ExecuterRequeteDataTable(TsCaOutils.ConnexionSql, sRequete.ToString());
                res.TableName = TsCaTablesChamps.TableRapportsConfrontation;
                return res;
            }
            catch (Exception ex)
            {
                throw new XuExcEErrFatale("ERREUR --> GenererRapportAutorisation(): " +
                                         ex.Message +
                                         Environment.NewLine);
            }
        }

        /// <summary>
        /// Génération des données du Rapport des Audits
        /// </summary>
        private DataTable GenererRapportAudit(string sensConfrontation)
        {
            try
            {
                string tableVisee, tableDeConfrontation;

                //Selon le sens attendu, fixer les tables
                switch (sensConfrontation)
                {
                    case SensConfrontation.SituationCourante:
                        tableVisee = TsCaTablesChamps.SECNTFS;
                        tableDeConfrontation = TsCaTablesChamps.TableDeReference;
                        break;
                    case SensConfrontation.SituationDeReference:
                        tableVisee = TsCaTablesChamps.TableDeReference;
                        tableDeConfrontation = TsCaTablesChamps.SECNTFS;
                        break;
                    default:
                        throw new Exception("Sens de confrontation inconnu.");
                }

                //Construction de la requête de confrontation du type rapport Audit
                //confrontation sur un nombre de champs bien précis
                //sélection de l'ensemble des champs de la table visée
                StringBuilder sRequete = new StringBuilder();
                sRequete.Append("select VL_TYP_SEC_NTF, VL_CHE_NTF, VL_IDN_NTF, VL_TYP_ACC_NTF, IN_DRO_ACC_HET_NTF, VL_PER_NTF, NM_PRO_NTF, VL_TYP_PRO_ENF_NTF, VL_TYP_HER_NTF, ");
                sRequete.Append($"'{sensConfrontation}' as VL_SEN_CON_NTF, '{TypeRapportConfrontation.Audit}' as VL_TYP_RAP_NTF, GETDATE() as DH_CON_NTF, DH_ANL_NTF  ");
                sRequete.Append($"from {tableVisee} as a ");
                sRequete.Append("where ");
                if (!string.IsNullOrWhiteSpace(_restrictionConfrontation))
                    sRequete.Append($"{_restrictionConfrontation} and ");
                sRequete.Append($"a.VL_TYP_SEC_NTF = '{TypeRapportConfrontation.Audit}' and not exists(");
                sRequete.Append("select b.VL_TYP_SEC_NTF, b.VL_CHE_NTF, b.VL_IDN_NTF, b.VL_TYP_ACC_NTF, b.IN_DRO_ACC_HET_NTF, b.VL_PER_NTF, b.VL_TYP_PRO_ENF_NTF, b.VL_TYP_HER_NTF ");
                sRequete.Append($"from {tableDeConfrontation} as b ");
                sRequete.Append("where b.VL_TYP_SEC_NTF = a.VL_TYP_SEC_NTF and ");
                sRequete.Append("b.VL_CHE_NTF = a.VL_CHE_NTF and b.VL_IDN_NTF = a.VL_IDN_NTF and ");
                sRequete.Append("b.VL_TYP_ACC_NTF = a.VL_TYP_ACC_NTF and b.IN_DRO_ACC_HET_NTF = a.IN_DRO_ACC_HET_NTF and ");
                sRequete.Append("b.VL_PER_NTF = a.VL_PER_NTF and b.VL_TYP_PRO_ENF_NTF = a.VL_TYP_PRO_ENF_NTF and ");
                sRequete.Append("b.VL_TYP_HER_NTF = a.VL_TYP_HER_NTF)");

                //Exécution et retour du résultat
                var res = TsCaOutils.ExecuterRequeteDataTable(TsCaOutils.ConnexionSql, sRequete.ToString());
                res.TableName = TsCaTablesChamps.TableRapportsConfrontation;
                return res;
            }
            catch (Exception ex)
            {
                throw new XuExcEErrFatale("ERREUR --> GenererRapportAudit(): " +
                                         ex.Message +
                                         Environment.NewLine);
            }
        }

        /// <summary>
        /// Génération des données du Rapport des Audits
        /// </summary>
        private DataTable GenererRapportProprietaire(string sensConfrontation)
        {
            try
            {
                string tableVisee, tableDeConfrontation;

                //Selon le sens attendu, fixer les tables
                switch (sensConfrontation)
                {
                    case SensConfrontation.SituationCourante:
                        tableVisee = TsCaTablesChamps.SECNTFS;
                        tableDeConfrontation = TsCaTablesChamps.TableDeReference;
                        break;
                    case SensConfrontation.SituationDeReference:
                        tableVisee = TsCaTablesChamps.TableDeReference;
                        tableDeConfrontation = TsCaTablesChamps.SECNTFS;
                        break;
                    default:
                        throw new Exception("Sens de confrontation inconnu.");
                }

                //Construction de la requête de confrontation du type rapport Proprietaire
                StringBuilder sRequete = new StringBuilder();

                sRequete.Append("select a.VL_CHE_NTF, a.NM_PRO_NTF from ");
                sRequete.Append($"(select VL_CHE_NTF, NM_PRO_NTF, count(*) as nbr from {tableVisee} where {_restrictionConfrontation.Replace("a.", "")} group by VL_CHE_NTF, NM_PRO_NTF) as a, ");
                sRequete.Append($"(select VL_CHE_NTF, NM_PRO_NTF, count(*) as nbr from {tableDeConfrontation} where {_restrictionConfrontation.Replace("a.", "")} group by VL_CHE_NTF, NM_PRO_NTF) as b ");
                sRequete.Append("where b.VL_CHE_NTF = a.VL_CHE_NTF and b.NM_PRO_NTF <> a.NM_PRO_NTF");

                //Exécution et retour du résultat
                var differencesRapportProprietaire = TsCaOutils.ExecuterRequeteDataTable(TsCaOutils.ConnexionSql, sRequete.ToString());

                //Ajuster le format à celui de la table des rapports de confrontation
                var res = CreerDataTableDesRapports();
                for (int i = 0; i < differencesRapportProprietaire.Rows.Count; i++)
                {
                    var enregistrementRapport = res.NewRow();
                    enregistrementRapport["VL_CHE_NTF"] = differencesRapportProprietaire.Rows[i]["VL_CHE_NTF"];
                    enregistrementRapport["NM_PRO_NTF"] = differencesRapportProprietaire.Rows[i]["NM_PRO_NTF"];
                    enregistrementRapport["VL_TYP_RAP_NTF"] = TypeRapportConfrontation.Proprietaire;
                    enregistrementRapport["VL_SEN_CON_NTF"] = sensConfrontation;
                    enregistrementRapport["DH_CON_NTF"] = DateTime.Now;
                    res.Rows.Add(enregistrementRapport);
                }
                return res;
            }
            catch (Exception ex)
            {
                throw new XuExcEErrFatale("ERREUR --> GenererRapportAudit(): " +
                                         ex.Message +
                                         Environment.NewLine);
            }
        }

        /// <summary>
        /// Génération des données du Rapport des Autorisation
        /// </summary>
        private DataTable CreerDataTableDesRapports()
        {
            DataTable dt = new DataTable(TsCaTablesChamps.TableRapportsConfrontation);

            dt.Columns.Add(TsCaTablesChamps.VL_TYP_SEC_NTF, typeof(string));
            dt.Columns.Add(TsCaTablesChamps.VL_CHE_NTF, typeof(string));
            dt.Columns.Add(TsCaTablesChamps.VL_IDN_NTF, typeof(string));
            dt.Columns.Add(TsCaTablesChamps.VL_TYP_ACC_NTF, typeof(string));
            dt.Columns.Add(TsCaTablesChamps.IN_DRO_ACC_HET_NTF, typeof(string));
            dt.Columns.Add(TsCaTablesChamps.VL_PER_NTF, typeof(string));
            

            dt.Columns.Add(TsCaTablesChamps.NM_PRO_NTF, typeof(string));
            dt.Columns.Add(TsCaTablesChamps.VL_TYP_PRO_ENF_NTF, typeof(string));
            dt.Columns.Add(TsCaTablesChamps.VL_TYP_HER_NTF, typeof(string));

            dt.Columns.Add(TsCaTablesChamps.SensConfrontation, typeof(string));
            dt.Columns.Add(TsCaTablesChamps.TypeRapport, typeof(string));

            dt.Columns.Add(TsCaTablesChamps.DateConfrontation, typeof(DateTime));
            dt.Columns.Add(TsCaTablesChamps.DH_ANL_NTF, typeof(DateTime));

            return dt;
        }

        /// <summary>
        /// Enregistrer les données des divers rapports collectés
        /// </summary>
        /// <param name="rapports">DataTable renfermant les divers données collectés</param>
        /// <returns></returns>
        private bool EnregistrerDonneesRapportsCollectes(DataTable rapports)
        {
            try
            {
                rapports.TableName = TsCaTablesChamps.TableRapportsConfrontation;

                using (TransactionScope tScope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(180)))
                {
                    using (SqlConnection connection = TsCaOutils.ConnexionSql)
                    {
                        //Partitionner les données collectées en plusieurs sous-datatables afin que l'enregistrement ne souffre pas d'un dépassement de délais
                        DataTable[] partitions = rapports.AsEnumerable()
                                                    .Select((row, index) => new { row, index })
                                                    .GroupBy(x => x.index / TsCaOutils.TailleBatchSQL)
                                                    .Select(g => g.Select(x => x.row).CopyToDataTable())
                                                    .ToArray();
                        //Envoyer les sous-datatables à l'enregistrement une à une
                        for (int i = 0; i < partitions.Length; i++)
                        {
                            partitions[i].TableName = TsCaTablesChamps.TableRapportsConfrontation;
                            TsCaOutils.ExecuterTraitementBulk(connection, partitions[i]);
                        }
                    }
                    tScope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw new XuExcEErrFatale("ERREUR --> EnregistrerDonneesRapportsCollectes(): " +
                                         ex.Message +
                                         Environment.NewLine);
            }
            return true;
        }
    }
}
