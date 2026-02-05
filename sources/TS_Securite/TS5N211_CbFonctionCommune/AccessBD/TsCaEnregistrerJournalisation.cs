using Rrq.InfrastructureCommune.ScenarioTransactionnel;
using System;
using System.Data.SqlClient;
using System.Transactions;

namespace TS5N211_CbFonctionCommune.AccessBD
{
    public class TsCaEnregistrerJournalisation
    {
        private string mMessage;

        //public string Message { get => mMessage; set => mMessage = value; }
        public TsCaEnregistrerJournalisation()
        {
        }

        public string Message
        {
            get
            {
                return mMessage;
            }

            set
            {
                mMessage = value;
            }
        }

        public string Executer(TsCaRegleAAppliquer regle, string codeDemandeur, string commentaire)
        {
            try
            {
                string requete = RequeteJournalisationInsert(regle, codeDemandeur, commentaire);
                using (TransactionScope tScope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(120)))
                {
                    using (SqlConnection connection = TsCaOutils.ConnexionSql)
                    {
                        SqlCommand commande = TsCaOutils.ObtenirCommande(connection, requete);
                        commande.ExecuteNonQuery();
                    }
                    tScope.Complete();
                }
            }
            catch (Exception ex)
            {
                return "ERREUR --> Erreur dans EnregistrerJournalisation.ExecuterRequete(): " + ex.Message + Environment.NewLine; 
            }
            return "";
        }
        private string RequeteJournalisationInsert(TsCaRegleAAppliquer regle, string codeDemandeur, string commentaire)
        {
            return string.Concat("insert into ", TsCaTablesChamps.JOUNTFS, "(", TsCaTablesChamps.VL_TYP_SEC_NTF, ", ",
                                                                                TsCaTablesChamps.VL_CHE_NTF, ", ",
                                                                                TsCaTablesChamps.VL_IDN_NTF, ", ",
                                                                                TsCaTablesChamps.VL_TYP_ACC_NTF, ", ",
                                                                                TsCaTablesChamps.IN_DRO_ACC_HET_NTF, ", ",
                                                                                TsCaTablesChamps.VL_PER_NTF, ", ",
                                                                                TsCaTablesChamps.DH_ANL_NTF, ", ",
                                                                                TsCaTablesChamps.NM_PRO_NTF, ", ",
                                                                                TsCaTablesChamps.VL_TYP_PRO_ENF_NTF, ", ",
                                                                                TsCaTablesChamps.VL_TYP_HER_NTF, ", ",
                                                                                TsCaTablesChamps.DH_APP_REG_ACC_NTF, ", ",
                                                                                TsCaTablesChamps.CO_TYP_APP_REG_NTF, ", ",
                                                                                TsCaTablesChamps.CM_APP_REG_ACC_NTF, ", ",
                                                                                TsCaTablesChamps.CO_TYP_REG_ACC_NTF, ", ",
                                                                                TsCaTablesChamps.CO_UTL_DEM_REG_NTF,
                                                                                ") values (",
                                                                                TsCaOutils.DoubleQuote(regle.TypeSecurite), ",",
                                                                                TsCaOutils.DoubleQuote(regle.Chemin), ",",
                                                                                TsCaOutils.DoubleQuote(regle.NomIdentite), ",",
                                                                                TsCaOutils.DoubleQuote(regle.TypeAcces), ",",
                                                                                TsCaOutils.DoubleQuote(regle.DroitAccesHerite), ",",
                                                                                TsCaOutils.DoubleQuote(regle.Permission), ",",
                                                                                TsCaOutils.DoubleQuote(regle.DateAnalyse.ToString()), ",",
                                                                                TsCaOutils.DoubleQuote(regle.Proprietaire), ",",
                                                                                TsCaOutils.DoubleQuote(regle.TypePropagationEnfants), ",",
                                                                                TsCaOutils.DoubleQuote(regle.TypeHeritage), ",",
                                                                                TsCaOutils.DoubleQuote(DateTime.Now.ToString()), ",",
                                                                                TsCaOutils.DoubleQuote(regle.CodeTypeApplication), ",",
                                                                                TsCaOutils.DoubleQuote(commentaire), ",",
                                                                                TsCaOutils.DoubleQuote(regle.CodeTypeRegle), ",",
                                                                                TsCaOutils.DoubleQuote(codeDemandeur), ")");
        }
    }
}
