using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

using Rrq.InfrastructureCommune.ScenarioTransactionnel;

using System.Text;
using TS5N211_CbFonctionCommune.Entites;

namespace TS5N211_CbFonctionCommune.AccessBD
{
    public class TsCaEnregistrerRepertoire
    {

        public bool Executer(List<TsCaRepertoire> listeRepertoire)
        {

            int diviseur = 10000;

            try
            {
                //Découper l'insertion en paquet de 5000 pour éviter un out of memory

                if (diviseur > listeRepertoire.Count) diviseur = listeRepertoire.Count;

                var listeInsertion = TsCaOutils.DistribuerListeEnInteger(listeRepertoire.Count, listeRepertoire.Count / diviseur);

                foreach (var i in listeInsertion)
                {
                    using (TransactionScope tScope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(120)))
                    {
                        using (SqlConnection connection = TsCaOutils.ConnexionSql)
                        {
                            var range = listeRepertoire.GetRange(0, i);
                            TsCaOutils.ExecuterTraitementBulk(connection, ObtenirListeRppPourBulk(range));
                            listeRepertoire.RemoveRange(0, i);
                        }
                        tScope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new XuExcEErrFatale("Erreur dans TsCaEnregistrerRepertoire.Executer(): " + ex.Message);
            }
            return true;
        }

        /// <summary>
        /// Vider la table des emplacements avant de recevoir les nouvelles valeurs
        /// </summary>
        public void ViderTableEmplacement(List<string> listeRrp)
        {
            StringBuilder sRequete = new StringBuilder();

            try
            {
                using (TransactionScope tScope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(180)))
                {
                    using (SqlConnection connection = TsCaOutils.ConnexionSql)
                    {
                        foreach (var nomRrrpCible in listeRrp)
                        {
                            SqlCommand commandeSql = connection.CreateCommand();
                            commandeSql.CommandType = CommandType.Text;

                            sRequete.Append($"DELETE FROM {TsCaTablesChamps.SECNTFS} ");

                            if (nomRrrpCible.Trim().Length != 0)
                            {
                                sRequete.Append($" WHERE {TsCaTablesChamps.VL_CHE_NTF} like {TsCaOutils.DoubleQuote(nomRrrpCible + "%")}");
                            }

                            commandeSql.CommandText = sRequete.ToString();

                            sRequete.Clear();

                            commandeSql.ExecuteNonQuery();
                        }
                    }

                    tScope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw new XuExcEErrFatale("ERREUR --> ViderTableEmplacement(): " +
                                          ex.Message +
                                          Environment.NewLine);
            }

        }

        /// <summary>
        /// Vider la table des emplacements avant de recevoir les nouvelles valeurs
        /// </summary>
        public void ViderTableEmplacement(string racine)
        {
            StringBuilder sRequete = new StringBuilder();
            sRequete.Append($"DELETE TOP({TsCaOutils.TailleBatchSQL}) FROM {TsCaTablesChamps.SECNTFS} WHERE UPPER({TsCaTablesChamps.VL_CHE_NTF}) like UPPER({TsCaOutils.DoubleQuote(racine + "%")})");

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
                throw new XuExcEErrFatale("ERREUR --> ViderTableEmplacement(): " +
                                          ex.Message +
                                          Environment.NewLine);
            }

        }

        internal bool ExecuterSansDecouper(List<TsCaRepertoire> listeRepertoire)
        {
            try
            {
                using (TransactionScope tScope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(120)))
                {
                    using (SqlConnection connection = TsCaOutils.ConnexionSql)
                    {
                        //connection.Open();
                        TsCaOutils.ExecuterTraitementBulk(connection, dt: ObtenirListeRppPourBulk(listeRepertoire));
                    }
                    tScope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw new XuExcEErrFatale("Erreur dans TsCaEnregistrerRepertoire.Executer(): " + ex.Message);
            }
            return true;
        }

        private DataTable ObtenirListeRppPourBulk(List<TsCaRepertoire> listeRepertoires)
        {
            var dt = CreerDataTable();

            try
            {
                foreach (TsCaRepertoire r in listeRepertoires)
                {
                    foreach (TsCaRepertoireAcces a in r.ListAcces)
                    {
                        var dr = dt.NewRow();
                        dr[TsCaTablesChamps.VL_TYP_SEC_NTF] = a.TypeSecurite;
                        dr[TsCaTablesChamps.VL_CHE_NTF] = r.Chemin;
                        dr[TsCaTablesChamps.VL_IDN_NTF] = a.NomIdentite;
                        dr[TsCaTablesChamps.VL_TYP_ACC_NTF] = a.TypeAcces;
                        dr[TsCaTablesChamps.IN_DRO_ACC_HET_NTF] = TsCaOutils.ValeurBoolean(a.DroitAccesHerite, false);
                        dr[TsCaTablesChamps.VL_PER_NTF] = a.Permission;
                        dr[TsCaTablesChamps.DH_ANL_NTF] = r.DateAnalyse;
                        dr[TsCaTablesChamps.NM_PRO_NTF] = r.Proprietaire;
                        dr[TsCaTablesChamps.VL_TYP_PRO_ENF_NTF] = a.TypePropagationEnfants;
                        dr[TsCaTablesChamps.VL_TYP_HER_NTF] = a.TypeHeritage;
                        dt.Rows.Add(dr);
                    }

                    foreach (TsCaRepertoireAudit a in r.ListAudit)
                    {
                        var dr = dt.NewRow();
                        dr[TsCaTablesChamps.VL_TYP_SEC_NTF] = a.TypeSecurite;
                        dr[TsCaTablesChamps.VL_CHE_NTF] = r.Chemin;
                        dr[TsCaTablesChamps.VL_IDN_NTF] = a.NomIdentite;
                        dr[TsCaTablesChamps.VL_TYP_ACC_NTF] = a.TypeAudit;
                        dr[TsCaTablesChamps.IN_DRO_ACC_HET_NTF] = TsCaOutils.ValeurBoolean(a.DroitAccesHerite, false);
                        dr[TsCaTablesChamps.VL_PER_NTF] = a.Permission;
                        dr[TsCaTablesChamps.DH_ANL_NTF] = r.DateAnalyse;
                        dr[TsCaTablesChamps.NM_PRO_NTF] = r.Proprietaire;
                        dr[TsCaTablesChamps.VL_TYP_PRO_ENF_NTF] = a.TypePropagationEnfants;
                        dr[TsCaTablesChamps.VL_TYP_HER_NTF] = a.TypeHeritage;
                        dt.Rows.Add(dr);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return dt;
        }

        private static DataTable CreerDataTable()
        {
            DataTable dt = new DataTable(TsCaTablesChamps.SECNTFS);
            dt.Columns.Add(TsCaTablesChamps.VL_TYP_SEC_NTF, typeof(string));
            dt.Columns.Add(TsCaTablesChamps.VL_CHE_NTF, typeof(string));
            dt.Columns.Add(TsCaTablesChamps.VL_IDN_NTF, typeof(string));
            dt.Columns.Add(TsCaTablesChamps.VL_TYP_ACC_NTF, typeof(string));
            dt.Columns.Add(TsCaTablesChamps.IN_DRO_ACC_HET_NTF, typeof(string));
            dt.Columns.Add(TsCaTablesChamps.VL_PER_NTF, typeof(string));
            dt.Columns.Add(TsCaTablesChamps.DH_ANL_NTF, typeof(DateTime));

            dt.Columns.Add(TsCaTablesChamps.NM_PRO_NTF, typeof(string));
            dt.Columns.Add(TsCaTablesChamps.VL_TYP_PRO_ENF_NTF, typeof(string));
            dt.Columns.Add(TsCaTablesChamps.VL_TYP_HER_NTF, typeof(string));

            return dt;
        }

    }
}