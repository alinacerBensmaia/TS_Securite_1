using Rrq.InfrastructureCommune.Parametres;
using Rrq.InfrastructureCommune.UtilitairesCommuns;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;


namespace Rrq.Securite.Certificat
{
    internal class TsCdCertificatSecurite 
    {

        const string REQUETE_SQL_CERSETS = "SELECT CER.VL_PAS_CER_SEC_TS " +
                                           "FROM   TS3.CERSETS CER " +
                                           "WHERE  CER.CO_ENV_CER_SEC_TS = @CO_ENV_CER_SEC_TS " +
                                           "AND    CER.CO_CER_SEC_TS = @CO_CER_SEC_TS";



        public string RecupererMotPasseCertificat(string codeCertificat) 
        {

            string NomDatasource = XuCuPolitiqueConfig.ConfigDomaine.get_ValeurSysteme("TS1", @"TS1N241\NomDatasource");
            string NomBaseDonnees = XuCuPolitiqueConfig.ConfigDomaine.get_ValeurSysteme("TS1", @"TS1N241\NomBaseDonnees");

            SqlConnection connexion = ConnexionSql(NomDatasource, NomBaseDonnees);

            List<SqlParameter> listeParametres = new List<SqlParameter>();
            listeParametres.Add(new SqlParameter("CO_ENV_CER_SEC_TS", XuCuPolitiqueConfig.ConfigDomaine.get_ValeurSysteme("General", "Environnement").Trim().ToUpper()));
            listeParametres.Add(new SqlParameter("CO_CER_SEC_TS", codeCertificat));

            DataTable DT_CERSERTS = ExecuterRequeteDataTable(connexion, REQUETE_SQL_CERSETS, listeParametres);

            if (DT_CERSERTS.Rows.Count > 0)
            {
                return TsCuUtilitaire.Dechiffrer(DT_CERSERTS.Rows[0].Field<string>("VL_PAS_CER_SEC_TS"), codeCertificat);
            }
            else
            {
                return string.Empty;
            }
        }

        internal static DataTable ExecuterRequeteDataTable(SqlConnection connexionSql, string requete, List<SqlParameter> listeParametres)
        {
            DataTable dtRetour = new DataTable();


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

            return dtRetour;
        }


        public static SqlConnection ConnexionSql(string NomDatasource, string NomBaseDonnees)
        {

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

               internal static SqlCommand ObtenirCommande(SqlConnection connexion, string texte)
       {
           SqlCommand command = new SqlCommand(texte, connexion) {CommandType = CommandType.Text};
           return command;
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

        internal static string ObtenirRequeteTextePourErreur(SqlCommand commande)
        {
            StringBuilder retour = new StringBuilder();
            foreach (SqlParameter parametre in commande.Parameters)
            {
                retour.AppendLine(string.Concat("declare ", parametre.ParameterName, " as varchar(50)"));
                retour.AppendLine(string.Concat("set ", parametre.ParameterName, " = ", DoubleQuote(parametre.Value.ToString()), ";"));
            }
            retour.AppendLine();
            retour.AppendLine();
            retour.AppendLine(commande.CommandText);
            return retour.ToString();
        }

        public static string DoubleQuote(string valeur)
        {
            return string.Concat("'", valeur.Replace("'", "''"), "'");
        }

    }
}
