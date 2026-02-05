namespace TS5N211_CbFonctionCommune.AccessBD
{
    // Cette class permet d'éviter de faire des erreurs sur les noms de table et de champ lors des différentes requëtes
    public class TsCaTablesChamps
    {
        /// <summary>
        /// Table contenant l'Information des permissions NTFS par répertoire. Considérée également comme Table de la situation Courante (Mécanique de Confrontation)
        /// </summary>
        public const string SECNTFS = "TS5.SECNTFS";

        /// <summary>
        /// Table contenant la journalisation de l'application des règles d'accès NTFS
        /// </summary>
        public const string JOUNTFS = "TS5.JOUNTFS";

        /// <summary>
        /// Table contenant les règles NTFS à appliquer
        /// </summary>      
        public const string REGNTFS = "TS5.REGNTFS";

        /// <summary>
        /// Table de la Situation de Référence
        /// </summary>      
        public const string TableDeReference = "TS5.REFNTFS";

        /// <summary>
        /// Table des Rapports de Confrontation
        /// </summary>      
        public const string TableRapportsConfrontation = "TS5.RAPNTFS";

        /// <summary>
        /// Valeur type sécurité NTFS (Permission ou Audit)
        /// </summary>
        public const string VL_TYP_SEC_NTF = "VL_TYP_SEC_NTF"; //Clé

        /// <summary>
        /// Chemin dossier
        /// </summary>
        public const string VL_CHE_NTF = "VL_CHE_NTF"; //Clé

        /// <summary>
        ///  Valeur identité
        /// </summary>
        public const string VL_IDN_NTF = "VL_IDN_NTF"; //Clé

        /// <summary>
        /// Type d'accès
        /// </summary>
        public const string VL_TYP_ACC_NTF = "VL_TYP_ACC_NTF"; //Clé

        /// <summary>
        /// Droit d'accès hérité'
        /// </summary>
        public const string IN_DRO_ACC_HET_NTF = "IN_DRO_ACC_HET_NTF"; //Clé

        /// <summary>
        /// Permission
        /// </summary>
        public const string VL_PER_NTF = "VL_PER_NTF"; //Clé

        /// <summary>
        /// Date de mise à jour de l'analyse'
        /// </summary>
        public const string DH_ANL_NTF = "DH_ANL_NTF"; //Clé

        /// <summary>
        /// Nom Groupe\Utilisateur
        /// </summary>
        public const string NM_GRO_UTL_NTF = "NM_GRO_UTL_NTF"; //Clé

        /// <summary>
        /// Domaine
        /// </summary>
        public const string VL_DOM_NTF = "VL_DOM_NTF"; //Clé

        /// <summary>
        /// Propriétaire
        /// </summary>
        public const string NM_PRO_NTF = "NM_PRO_NTF";

        /// <summary>
        /// Étendu
        /// </summary>
        public const string VL_ETN_NTF = "VL_ETN_NTF";

        /// <summary>
        /// Type de propagation
        /// </summary>
        public const string VL_TYP_PRO_ENF_NTF = "VL_TYP_PRO_ENF_NTF";

        /// <summary>
        /// Type de d'héritage'
        /// </summary>
        public const string VL_TYP_HER_NTF = "VL_TYP_HER_NTF";

        /// <summary>
        /// date heure application règle accès NTFS
        /// <summary>
        public const string DH_APP_REG_ACC_NTF = "DH_APP_REG_ACC_NTF";
        
        /// <summary>
        /// code type application règle accès NTFS
        /// <summary>
        public const string CO_TYP_APP_REG_NTF = "CO_TYP_APP_REG_NTF";
        
        /// <summary>
        /// commentaire application règle accès NTFS
        /// <summary>
        public const string CM_APP_REG_ACC_NTF = "CM_APP_REG_ACC_NTF";

        /// <summary>
        /// code type règle accès NTFS (type de règle reg = régulière ou exc = exception)
        /// </summary>
        public const string CO_TYP_REG_ACC_NTF= "CO_TYP_REG_ACC_NTF";
        /// <summary>
        /// code du demandeur
        /// </summary>
        public const string CO_UTL_DEM_REG_NTF = "CO_UTL_DEM_REG_NTF";
        /// <summary>
        /// Sens de Confrontation
        /// </summary>
        public const string SensConfrontation = "VL_SEN_CON_NTF";
        /// <summary>
        /// Type de Rapport
        /// </summary>
        public const string TypeRapport = "VL_TYP_RAP_NTF";
        /// <summary>
        /// Date de Confrontation
        /// </summary>
        public const string DateConfrontation = "DH_CON_NTF";
    }
}
