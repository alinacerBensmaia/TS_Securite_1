using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using TS5N211_CbFonctionCommune.AccessBD;

namespace TS5N211_CbFonctionCommune.Entites
{
    public class TsCaJournalisation
    {
       ///CETTE CLASSE N'EST PAS UTILISÉE EN CE MOMENT. DÉVELOPPÉE EN VUE DE FAIRE LA JOURNALISATION SEULEMENT A LA FIN DU TRAITEMENT.

        #region Propriétés
        /// <summary>
        /// VL_TYP_SEC_NTF
        /// </summary>
        public string TypeSecurite { get; internal set; }
        /// <summary>
        /// VL_CHE_NTF
        /// </summary>
        public string Chemin { get; internal set; }
        /// <summary>
        /// VL_IDN_NTF
        /// </summary>
        public string NomIdentite { get; internal set; }
        /// <summary>
        /// VL_TYP_ACC_NTF
        /// </summary>
        public string TypeAcces { get; internal set; }
        /// <summary>
        /// IN_DRO_ACC_HET_NTF
        /// </summary>
        public string DroitAccesHerite { get; internal set; }
        /// <summary>
        /// VL_PER_NTF
        /// </summary>
        public string Permission { get; internal set; }
        /// <summary>
        /// DH_ANL_NTF
        /// </summary>
        public DateTime DateAnalyse { get; internal set; }
        /// <summary>
        /// NM_PRO_NTF
        /// </summary>
        public string Proprietaire { get; internal set; }
        /// <summary>
        /// VL_TYP_PRO_ENF_NTF
        /// </summary>
        public string TypePropagationEnfants { get; internal set; }
        /// <summary>
        /// VL_TYP_HER_NTF
        /// </summary>
        public string TypeHeritage { get; internal set; }
        /// <summary>
        /// CO_TYP_APP_REG_ACC_NTF
        /// </summary>
        public string CodeTypeApplication { get; internal set; }
        /// <summary>
        /// CO_TYP_REG_ACC_NTF
        /// </summary>
        public string CodeTypeRegle { get; internal set; }

        public List<System.Security.AccessControl.FileSystemRights> ListePermissions { get; internal set; }
        public string CodeDemandeur { get; internal set; }
        #endregion
        public TsCaJournalisation()
        {
        }
        public DataTable CreerDataTableJournalisation()
        {
            DataTable dt = new DataTable(TsCaTablesChamps.JOUNTFS);
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
            dt.Columns.Add(TsCaTablesChamps.DH_APP_REG_ACC_NTF, typeof(DateTime));
            dt.Columns.Add(TsCaTablesChamps.CO_TYP_APP_REG_NTF, typeof(string));
            dt.Columns.Add(TsCaTablesChamps.CM_APP_REG_ACC_NTF, typeof(string));
            dt.Columns.Add(TsCaTablesChamps.CO_TYP_REG_ACC_NTF, typeof(string));
            dt.Columns.Add(TsCaTablesChamps.CO_UTL_DEM_REG_NTF, typeof(string));


            return dt;
        }
        private void AssignerValeur(string typeSecurite, string chemin, string nomIdentite, string typeAcces, string droitAccesHerite, string permission, DateTime dateAnalyse, string proprietaire, string typePropagationEnfants, string typeHeritage, string codeTypeApplication, string codeTypeRegle, string codeDemandeur)
        {

            TypeSecurite = typeSecurite;
            Chemin = chemin;
            NomIdentite = nomIdentite;
            TypeAcces = typeAcces;
            DroitAccesHerite = droitAccesHerite;
            Permission = permission;
            DateAnalyse = dateAnalyse;
            Proprietaire = proprietaire;
            TypePropagationEnfants = typePropagationEnfants;
            TypeHeritage = typeHeritage;
            CodeTypeApplication = codeTypeApplication;
            CodeTypeRegle = codeTypeRegle;
            CodeDemandeur = CodeDemandeur;
            
        }
    }
}

