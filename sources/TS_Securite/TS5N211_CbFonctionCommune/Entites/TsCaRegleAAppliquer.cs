using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using TS5N211_CbFonctionCommune.AccessBD;

using System.Security.AccessControl;

namespace TS5N211_CbFonctionCommune
{
    public class TsCaRegleAAppliquer
    {
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

        public List<FileSystemRights> ListePermissions { get; internal set; }

        public string MessageErreur { get; set; }

        #endregion
        public TsCaRegleAAppliquer(DataRow dr)
        {
            if (!(dr == null))
            {
                ListePermissions=new List<FileSystemRights>();
                AssignerValeur(dr);    
            }
        }

        private void AssignerValeur(DataRow dr)
        {
           AssignerValeur(dr[TsCaTablesChamps.VL_TYP_SEC_NTF].ToString(),
            dr[TsCaTablesChamps.VL_CHE_NTF].ToString(),
            dr[TsCaTablesChamps.VL_IDN_NTF].ToString(),
            dr[TsCaTablesChamps.VL_TYP_ACC_NTF].ToString(),
            dr[TsCaTablesChamps.IN_DRO_ACC_HET_NTF].ToString(),
            dr[TsCaTablesChamps.VL_PER_NTF].ToString(),
            DateTime.Parse(dr[TsCaTablesChamps.DH_ANL_NTF].ToString()),
            dr[TsCaTablesChamps.NM_PRO_NTF].ToString(),
            dr[TsCaTablesChamps.VL_TYP_PRO_ENF_NTF].ToString(),
            dr[TsCaTablesChamps.VL_TYP_HER_NTF].ToString(),
            dr[TsCaTablesChamps.CO_TYP_APP_REG_NTF].ToString(),
            dr[TsCaTablesChamps.CO_TYP_REG_ACC_NTF].ToString());
        }
        private void AssignerValeur(string typeSecurite, string chemin, string nomIdentite, string typeAcces, string droitAccesHerite, string permission, DateTime dateAnalyse, string proprietaire, string typePropagationEnfants, string typeHeritage, string codeTypeApplication, string codeTypeRegle)
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
            CodeTypeRegle = codeTypeRegle.ToUpper();
            if(!string.IsNullOrEmpty(permission))
            {
                PeuplerListePermission(permission, chemin, nomIdentite);
            }

            MessageErreur = "";
        }
        private void PeuplerListePermission(string permission, string chemin, string nomIdentite)
        {
            string[] listePermissionTemp = Array.ConvertAll(permission.Split(','), p => p.Trim());
            foreach (var permissionTemp in listePermissionTemp)
            {
                //TODO
                try
                {
                    FileSystemRights enumPerm;
                    bool reussi = Enum.TryParse(permissionTemp, out enumPerm);                   
                    if (reussi)
                    {
                        ListePermissions.Add(enumPerm);
                    }
                    else
                    {
                        MessageErreur = "ERREUR --> La permission "+ permissionTemp + "n'a pu être ajouter à la liste du répertoire "+ chemin + " groupe: "+ nomIdentite +". Veuillez revérifier votre fichier d'entrée" + Environment.NewLine; 
                    }
                    
                }
                catch (Exception)
                {
                    MessageErreur = "Impossible de peupler la liste de permission du répertoire cible: "+ chemin + " groupe: " + nomIdentite + Environment.NewLine; 
                }

            }
        }
    }

}