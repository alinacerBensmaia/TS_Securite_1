using Rrq.InfrastructureCommune.ScenarioTransactionnel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace TS5N211_CbFonctionCommune.AccessBD
{
    public class TsCaObtenirInformation
    {
       public TsCaObtenirInformation()
       { 
       }

        public List<TsCaRegleAAppliquer> ObtenirListeReglesAAppliquerBD()
        {
            List<TsCaRegleAAppliquer> listeTsCaReglesAAppliquer = new List<TsCaRegleAAppliquer>();

            using (DataTable dt_TsCaReglesAAppliquer = TsCaOutils.ExecuterRequeteDataTable(TsCaOutils.ConnexionSql, RequeteObtenirListeRegles()))
            {
                foreach (DataRow dr in dt_TsCaReglesAAppliquer.Rows)
                {
                    listeTsCaReglesAAppliquer.Add(new TsCaRegleAAppliquer(dr));
                }
                    
            }
            return listeTsCaReglesAAppliquer;
        }
        public string RequeteObtenirListeRegles()
        {
            return string.Concat("SELECT ", TsCaTablesChamps.VL_TYP_SEC_NTF, ", ",
                                                    TsCaTablesChamps.VL_CHE_NTF, ", ",
                                                    TsCaTablesChamps.VL_IDN_NTF, ", ",
                                                    TsCaTablesChamps.VL_TYP_ACC_NTF, ", ",
                                                    TsCaTablesChamps.IN_DRO_ACC_HET_NTF, ", ",
                                                    TsCaTablesChamps.VL_PER_NTF, ", ",
                                                    TsCaTablesChamps.DH_ANL_NTF, ", ",
                                                    TsCaTablesChamps.NM_PRO_NTF, ", ",
                                                    TsCaTablesChamps.VL_TYP_PRO_ENF_NTF, ", ",
                                                    TsCaTablesChamps.VL_TYP_HER_NTF, ", ",
                                                    TsCaTablesChamps.CO_TYP_APP_REG_NTF, ", ",
                                                    TsCaTablesChamps.CO_TYP_REG_ACC_NTF,
                                            " FROM ", TsCaTablesChamps.REGNTFS);
        }
        public string RequeteObtenirRegle(TsCaRegleAAppliquer regle)
        { 
            return string.Concat("SELECT ", TsCaTablesChamps.VL_TYP_SEC_NTF, ", ",
                                                    TsCaTablesChamps.VL_CHE_NTF, ", ",
                                                    TsCaTablesChamps.VL_IDN_NTF, ", ",
                                                    TsCaTablesChamps.VL_TYP_ACC_NTF, ", ",
                                                    TsCaTablesChamps.IN_DRO_ACC_HET_NTF, ", ",
                                                    TsCaTablesChamps.VL_PER_NTF, ", ",
                                                    TsCaTablesChamps.DH_ANL_NTF, ", ",
                                                    TsCaTablesChamps.NM_PRO_NTF, ", ",
                                                    TsCaTablesChamps.VL_TYP_PRO_ENF_NTF, ", ",
                                                    TsCaTablesChamps.VL_TYP_HER_NTF, ", ",
                                                    TsCaTablesChamps.CO_TYP_APP_REG_NTF, ", ",
                                                    TsCaTablesChamps.CO_TYP_REG_ACC_NTF,
                                            " FROM ", TsCaTablesChamps.REGNTFS,
                                           " WHERE ", TsCaTablesChamps.VL_TYP_SEC_NTF, "=", regle.TypeSecurite,
                                                    TsCaTablesChamps.VL_CHE_NTF, " = ", regle.Chemin,
                                                    TsCaTablesChamps.VL_IDN_NTF, " = ", regle.NomIdentite,
                                                    TsCaTablesChamps.VL_TYP_ACC_NTF, " = ", regle.TypeAcces,
                                                    TsCaTablesChamps.IN_DRO_ACC_HET_NTF, " = ", regle.DroitAccesHerite,
                                                    TsCaTablesChamps.VL_PER_NTF, " = ", regle.Permission,
                                                    TsCaTablesChamps.DH_ANL_NTF, " = ", regle.DateAnalyse,
                                                    TsCaTablesChamps.NM_PRO_NTF, " = ", regle.Proprietaire,
                                                    TsCaTablesChamps.VL_TYP_PRO_ENF_NTF, " = ", regle.TypePropagationEnfants,
                                                    TsCaTablesChamps.VL_TYP_HER_NTF, " = ", regle.TypeHeritage,
                                                    TsCaTablesChamps.CO_TYP_APP_REG_NTF, " = ",regle.CodeTypeApplication,
                                                    TsCaTablesChamps.CO_TYP_REG_ACC_NTF, " = ", regle.CodeTypeRegle);
            }

    }
}