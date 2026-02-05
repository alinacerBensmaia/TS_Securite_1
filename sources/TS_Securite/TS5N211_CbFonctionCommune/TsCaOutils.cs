using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using Rrq.InfrastructureCommune.Parametres;
using Rrq.InfrastructureCommune.UtilitairesCommuns;


namespace TS5N211_CbFonctionCommune
{
    public class TsCaOutils
    {

        //private static readonly string CleSymbolique = XuCuConfiguration.get_ObtenirValeurSystemeOptionnelle("TS5", @"TS5N212\ConnexionSQL\CleSymbolique");
        private static readonly string NomDatasource = XuCuConfiguration.get_ObtenirValeurSystemeOptionnelle("TS5", @"TS5N212\ConnexionSQL\NomDatasource");
        private static readonly string NomBaseDonnees = XuCuConfiguration.get_ObtenirValeurSystemeOptionnelle("TS5", @"TS5N212\ConnexionSQL\NomBaseDonnees");

        private static readonly string RepertoiresSource = XuCuConfiguration.get_ObtenirValeurSystemeOptionnelle("TS5", @"TS5N212\RepertoiresSource");
        private static readonly string RepertoiresModele = XuCuConfiguration.get_ObtenirValeurSystemeOptionnelle("TS5", @"TS5N212\RepertoiresModele");

        private static readonly string RepertoiresSourceModele = XuCuConfiguration.get_ObtenirValeurSystemeOptionnelle("TS5", @"TS5N212\Repertoires_Source_Modele");

        private static readonly string _EstBavard = XuCuConfiguration.get_ObtenirValeurSystemeOptionnelle("TS5", @"TS5N212\EstBavard");

        public static readonly string CleSymbolique = XuCuConfiguration.get_ObtenirValeurSystemeOptionnelle("TS5", @"CleSymboliqueNTFS");

        //Constante indiquant la taille d'un travail SQL en batch
        public const int TailleBatchSQL = 5000;

        public static SqlConnection ConnexionSql
        {

            get
            {
                try
                {
                    XuCuAccesBd objAccesBd = new XuCuAccesBd();
                    SqlConnection scConn;

                    scConn = objAccesBd.ObtenirConnexionSqlAuthentifiee(NomDatasource, NomBaseDonnees);

                    return scConn;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }
        }

        internal static void ExecuterTraitementBulk(SqlConnection connection, DataTable dt)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
            {
                bulkCopy.BulkCopyTimeout = 50;
                bulkCopy.DestinationTableName = dt.TableName;

                bulkCopy.ColumnMappings.Clear();

                foreach (DataColumn col in dt.Columns)
                    bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                //System.Diagnostics.Debugger.Launch();
                try
                {
                    bulkCopy.WriteToServer(dt);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }
        }

        public static bool EstBavard()
        {
            return Convert.ToBoolean(_EstBavard);
        }

        internal static void RemplirDataTable(SqlCommand commandeSql, DataTable dt, ref string requetePourErreur)
        {
            using (SqlCommand commande = commandeSql)
            {
                requetePourErreur = ObtenirRequeteTextePourErreur(commande);

                using (SqlDataAdapter da = new SqlDataAdapter(commande))
                {
                    da.Fill(dt);
                }
            }
        }

        internal static DataTable ExecuterRequeteDataTable(SqlConnection connexionSql, string requete)
        {
            return ExecuterRequeteDataTable(connexionSql, requete, null/* TODO Change to default(_) if this is not a reference type */);
        }

        internal static DataTable ExecuterRequeteDataTable(SqlConnection connexionSql, string requete, List<SqlParameter> listeParametres)
        {
            DataTable dtRetour = new DataTable();
            
            try
            {
                
                using (TransactionScope tScope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(30)))
                {
                    using (SqlConnection connexion = connexionSql)
                    {
                        //Connexion.Open();

                        SqlCommand commande = ObtenirCommande(connexion, requete, listeParametres);

                        RemplirDataTable(commande, dtRetour, ref requete);
                    }

                    tScope.Complete();
                }
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrWhiteSpace(requete))
                    EcrireEvenementErreur(ex, requete);
                else
                    EcrireEvenementErreur(ex);
                throw;
            }

            return dtRetour;
        }

        internal static SqlCommand ObtenirCommande(SqlConnection connexion, string texte)
        {
            SqlCommand command = new SqlCommand(texte, connexion) {CommandType = CommandType.Text};
            return command;
        }

        internal static SqlCommand ObtenirCommande(SqlConnection connexion, string texte, List<SqlParameter> listeParametres)
        {
            SqlCommand command = ObtenirCommande(connexion, texte);
            if (listeParametres != null)
            {
                List<string> listeParamAjoutes = new List<string>();
                foreach (SqlParameter param in listeParametres)
                {
                    if (!listeParamAjoutes.Contains(param.ParameterName))
                    {
                        command.Parameters.Add(param);
                        listeParamAjoutes.Add(param.ParameterName);
                    }
                }
            }
            return command;
        }
        internal static void EcrireEvenementErreur(Exception ex)
        {
            EcrireEvenementErreur(ex, "Erreur lors de l'éxécution de la requête de l'analyse des composants");
        }

        internal static void EcrireEvenementErreur(Exception ex, string erreur)
        {
            TsCuGestnEvent.EcrireEventLog(XuGeTypeEvenement.XuGeTeErreur, string.Concat(erreur, Environment.NewLine, ex.Message, Environment.NewLine, ex.StackTrace));
        }
        internal static string ObtenirRequeteTextePourErreur(SqlCommand commande)
        {
            StringBuilder retour = new StringBuilder();
            foreach (SqlParameter parametre in commande.Parameters)
            {
                retour.AppendLine(string.Concat("declare ", parametre.ParameterName, " as varchar(5000)"));
                retour.AppendLine(string.Concat("set ", parametre.ParameterName, " = ", DoubleQuote(parametre.Value.ToString()), ";"));
            }
            retour.AppendLine();
            retour.AppendLine();
            retour.AppendLine(commande.CommandText);
            return retour.ToString();
        }
        
        public static List<string> ListeRepertoiresSource()
        {
            return RepertoiresSource.Trim().Split(';').ToList();
        }

        public static List<string> ListeRepertoiresModele()
        {
            return RepertoiresModele.Trim().Split(';').ToList();
        }

        public static List<string> ListeRepertoires_Source_Modele()
        {
            return RepertoiresSourceModele.Trim().Split(';').ToList();
        }

        public static string Valeur(string valeur, bool quoteDouble)
        {
            string oui = "O";
            string non = "N";
            string retour = "0";
            if (valeur.Equals(oui))
            {
                retour = "0";
            }
            else if (valeur.Equals(non))
            { 
                retour = "1";
            }
            return retour;
         }

        public static string ValeurBoolean(bool valeur, bool quoteDouble)
        {
            string retour = valeur ? "O" : "N";

            if (quoteDouble)
                return DoubleQuote(retour);

            return retour;
        }

        public static string ValeurBit(bool valeur, bool quoteDouble)
        {
            string retour = valeur ? "1" : "0";

            if (quoteDouble)
                return DoubleQuote(retour);

            return retour;

        }
        public static string DoubleQuote(string valeur)
        {
            return string.Concat("'", valeur.Replace("'", "''"), "'");
        }

        public static string RecupererGroupeUtilisateur(string pIdentityReference)
        {
            string retour = string.Empty;
            int index = pIdentityReference.IndexOf(@"\", StringComparison.Ordinal);
            if (index > 0)
            {
                retour = pIdentityReference.Substring(index + 1, pIdentityReference.Length - index - 1);
            }

            return retour;
        }

        public static string RecupererDomaine(string pIdentityReference)
        {
            string retour = string.Empty;
            int index = pIdentityReference.IndexOf(@"\", StringComparison.Ordinal);
            if (index > 0)
            {
                retour = pIdentityReference.Substring(0, index);
            }

            return retour;
        }

        public static IEnumerable<int> DistribuerListeEnInteger(int total, int diviseur)
        {
            if (diviseur == 0)
            {
                yield return 0;
            }
            else
            {
                int rest = total % diviseur;
                double result = total / (double)diviseur;

                for (int i = 0; i < diviseur; i++)
                {
                    if (rest-- > 0)
                        yield return (int)Math.Ceiling(result);
                    else
                        yield return (int)Math.Floor(result);
                }
            }
        }

        /// <summary>
        /// À partir d'un répertoire, permet de récupérer les sous-dossiers selon la profondeur souhaité.
        /// </summary>
        /// <param name="chemin">Chemin du répertoire racine</param>
        /// <param name="debut">Niveau du début</param>
        /// <param name="fin">Nombre de niveau à récupérer</param>
        /// <param name="listRpp">Liste contenant les reépertoires trouvés</param>
        public static void RecupererNiveauRep(string chemin, int debut, int fin, List<string> listRpp)
        {
            var dirInfo = new DirectoryInfo(chemin);
            var folders = dirInfo.GetDirectories().ToList().Where(d => !d.FullName.Contains("$RECYCLE.BIN") && !d.FullName.Contains("System Volume Information"));

            foreach (var item in folders)
            {
                if (debut < fin)
                {
                    listRpp.Add(item.FullName);
                    RecupererNiveauRep(item.FullName, debut + 1, fin, listRpp);
                }
            }
        }

        // '' <summary>
        // '' Permet d'envoyer un courriel à la fin du traitement de la chaine'
        // '' </summary>
        // '' <param name="ChaineContexte"></param>
        // '' <param name="titre"></param>
        // '' <param name="contenu"></param>
        // '' <param name="usager"></param>
        // '' <param name="environnement"></param>
        public static void TransmettreCourrielChargement(ref string chaineContexte, string objet, string contenu, string destinataire, string environnement)
        {

            StringBuilder message = new StringBuilder();
            message.AppendLine(string.Format("{0}<br/>", objet));
            message.AppendLine("<br/>");
            message.AppendLine(contenu);
            message.AppendLine("<br/>");
            message.AppendLine(("Date/heure de fin de traitement: " + (DateTime.Now + "<br/>")));
            message.AppendLine("<br/>");
            message.AppendLine(("Environnement du navigateur de support : "+ (XuCuConfiguration.get_ValeurSysteme("General", "General\\Environnement") + "<br/>")));
            message.AppendLine("<br/>");

            XuCuEnvoiCourriel cuEnvoiCourriel = new XuCuEnvoiCourriel
            {
                Destinataire = destinataire,
                CopieConforme = XuCuConfiguration.get_ValeurSysteme("TS5", "TS5\\CopieConforme"),
                Expediteur = XuCuConfiguration.get_ValeurSysteme("TS5", "TS5\\ExpediteurTraitementNTFS"),
                FormatMessage = XuCuEnvoiCourriel.XuEcFormatMessage.XuEcFmHTML,
                Message = message.ToString(),
                Objet = objet
            };
            cuEnvoiCourriel.EnvoyerCourriel();


        }
    }
    }
