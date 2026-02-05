using System.Security.AccessControl;
using System.Security.Principal;


namespace TS5N211_CbFonctionCommune.Entites
{
    public class TsCaRepertoireAudit
    {
        //Permission ou Audit
        public string TypeSecurite = TypeSecuriteNtfs.Audit;

        //AccessControlType
        //private AccessControlType _accessControlType;

        //AccessControlType
        private AuditFlags _auditFlags;

        //Type d’opération associé à la règle d’audit
        private FileSystemRights _fileSystemRights;

        //Compte d’utilisateur.
        private IdentityReference _identityReference;

        //Valeurs qui spécifie comment les masques d’accès sont propagés aux objets enfants.
        private InheritanceFlags _inheritanceFlags;

        //EstHérité
        private bool _isInherited;

        //Valeurs qui spécifie comment les entrées de contrôle d’accès (ACE) sont propagées aux objets enfants.
        private PropagationFlags _propagationFlags;

        public string Domaine; //Ex: RQ
        public string GroupeUtilisateur { get; set; } //Ex: AdmResp Graduation Inforoute

        //VL_TYP_AUDIT_FIC_NTF
        //Ex: Echec, Succès, Tout
        public string TypeAudit
        {
            get
            {
                //Retourne vide si c'est hérité du parent
                if (_isInherited && _auditFlags == AuditFlags.None)
                {
                    return "";
                }

                return _auditFlags.ToString();
            }
        }

        //VL_TYP_INH_ENF_NTF
        public string TypeHeritage
        {
            get
            {
                //Retourne vide si c'est hérité du parent'
                if (_isInherited && _inheritanceFlags == InheritanceFlags.None)
                {
                    return "";
                }

                return _inheritanceFlags.ToString();
                
            }
        }

        //VL_TYP_PRG_ENF_NTF
        public string TypePropagationEnfants
        {
            get
            {
                //Retourne vide si c'est hérité du parent
                if (_isInherited && _propagationFlags == PropagationFlags.None)
                {
                    return "";
                }

                switch (_propagationFlags)
                {
                    case PropagationFlags.None:
                        return _propagationFlags.ToString();
                    case PropagationFlags.NoPropagateInherit:
                        return _propagationFlags.ToString();
                    case PropagationFlags.InheritOnly:
                        return _propagationFlags.ToString();
                    default:
                        return _propagationFlags.ToString();
                }
            }

        }

        //IN_DRO_ACC_HET_NTF
        //public bool DroitAccesHerite
        //{
        //    get => _isInherited;
        //    set => _isInherited = value;
        //}

        //VL_TYP_IDN_FIC_NTF //Ex: RQ\T202275 ou CREATOR OWNER 
        public string NomIdentite;

        //VL_PER_FIC_NTF
        //Ex: Lecture, écriture et Exécution
        public string Permission => _fileSystemRights.ToString() == "0" ? "" : _fileSystemRights.ToString();

        public bool DroitAccesHerite
        {
            get
            {
                return _isInherited;
            }

            set
            {
                _isInherited = value;
            }
        }

        public TsCaRepertoireAudit()
        {
            DroitAccesHerite = true;
            NomIdentite = "";
            GroupeUtilisateur = "";
            Domaine = "";
        }

        public TsCaRepertoireAudit(string typeAccesAudit, FileSystemAuditRule ace, string nomGroupe)
        {
            AssignerValeurs(ace, nomGroupe);
        }

        public TsCaRepertoireAudit(string typeAccesAudit, FileSystemAuditRule ace)
        {
            AssignerValeurs(ace, "");
        }

        private void AssignerValeurs(FileSystemAuditRule ace, string nomGroupe)
        {
            _fileSystemRights = ace.FileSystemRights;
            _identityReference = ace.IdentityReference;
            _inheritanceFlags = ace.InheritanceFlags;
            _isInherited = ace.IsInherited;
            _propagationFlags = ace.PropagationFlags;

            _auditFlags = ace.AuditFlags;

            //NM_GRO_UTL_FIC_NTF,VL_DOM_FIC_NTF et VL_TYP_IDN_FIC_NTF
            if (nomGroupe.Length == 0)
            {
                GroupeUtilisateur = TsCaOutils.RecupererGroupeUtilisateur(ace.IdentityReference.Value);
                Domaine = TsCaOutils.RecupererDomaine(ace.IdentityReference.Value);
                NomIdentite = ace.IdentityReference.Value;
            }
            else
            {
                GroupeUtilisateur = TsCaOutils.RecupererGroupeUtilisateur(nomGroupe);
                if (GroupeUtilisateur.Length == 0) GroupeUtilisateur = nomGroupe;
                Domaine = TsCaOutils.RecupererDomaine(nomGroupe);
                NomIdentite = Domaine + @"\" + nomGroupe;
            }
           
        }

    }

    

}
