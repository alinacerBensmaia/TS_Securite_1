using System.Security.AccessControl;
using System.Security.Principal;


namespace TS5N211_CbFonctionCommune.Entites
{
    public class TsCaRepertoireAcces
    {
        //Permission ou Audit
        public string TypeSecurite = TypeSecuriteNtfs.Autorisation;

        //AccessControlType
        private AccessControlType _accessControlType;

        //Type d’opération associé à la règle à la sécurité
        private FileSystemRights _fileSystemRights;

        //Compte d’utilisateur.
        private IdentityReference _identityReference;

        //Valeurs qui spécifie comment les masques d’accès sont propagés aux objets enfants.
        private InheritanceFlags _inheritanceFlags;

        //EstHérité
        private bool _isInherited;

        //Valeurs qui spécifie comment les entrées de contrôle d’accès (ACE) sont propagées aux objets enfants.
        private PropagationFlags _propagationFlags;

        //VL_DOM_FIC_NTF
        public string Domaine; //Ex: RQ

        //NM_GRO_UTL_FIC_NTF
        public string GroupeUtilisateur { get; set; } //Ex: AdmResp Graduation Inforoute

        //VL_TYP_ACC_FIC_NTF
        //Ex: Permis, refusé
        public string TypeAcces
        {
            get
            {
                //Retourne vide si c'est hérité du parent
                if (GroupeUtilisateur.Length == 0 && Domaine.Length == 0)
                {
                    return "";
                }

                return _accessControlType.ToString();

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

                return _propagationFlags.ToString();
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

                return _inheritanceFlags.ToString(); ;
            }
        }


        //IN_DRO_ACC_HET_NTF
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

        //VL_TYP_IDN_FIC_NTF
        //Ex: RQ\T202275 ou CREATOR OWNER 
        public string NomIdentite; 

        //VL_PER_FIC_NTF
        //Ex: Lecture, écriture et Exécution
        public string Permission => _fileSystemRights.ToString() == "0" ? "" : _fileSystemRights.ToString();

        /// <summary>
        ///   Indique si les règles d’accès héritées sont propagées automatiquement.
        ///   Les indicateurs de propagation sont ignorés si "inheritanceFlags" a la valeur "InheritanceFlags.None".
        /// </summary>
        //private PropagationFlags PropagationFlag
        //{
        //    get => _propagationFlags;
        //    set =>_propagationFlags = value;
        //}
        public PropagationFlags PropagationFlags
        {
            get
            {
                return _propagationFlags;
            }

            set
            {
                _propagationFlags = value;
            }
        }

        public TsCaRepertoireAcces()
        {
            DroitAccesHerite = true;
            NomIdentite = "";
            GroupeUtilisateur = "";
            Domaine = "";
        }

        public TsCaRepertoireAcces(FileSystemAccessRule ace, string nomGroupe)
        {
            AssignerValeurs(ace,nomGroupe);
        }

        private void AssignerValeurs(FileSystemAccessRule ace, string nomGroupe)
        {

            _fileSystemRights = ace.FileSystemRights;
            _identityReference = ace.IdentityReference;
            _inheritanceFlags = ace.InheritanceFlags;
            _isInherited = ace.IsInherited;
            _propagationFlags = ace.PropagationFlags;

            _accessControlType = ace.AccessControlType;

            //NM_GRO_UTL_FIC_NTF,VL_DOM_FIC_NTF et VL_TYP_IDN_FIC_NTF
            if (nomGroupe.Length == 0){
                GroupeUtilisateur = TsCaOutils.RecupererGroupeUtilisateur(_identityReference.Value);
                Domaine = TsCaOutils.RecupererDomaine(_identityReference.Value);
                NomIdentite = _identityReference.Value;
            }
            else{
                GroupeUtilisateur = TsCaOutils.RecupererGroupeUtilisateur(nomGroupe);
                if (GroupeUtilisateur.Length == 0) GroupeUtilisateur = nomGroupe;
                Domaine = TsCaOutils.RecupererDomaine(nomGroupe);
                NomIdentite = nomGroupe;
            }
            
        }

        public override string ToString()
        {
            return $"{GroupeUtilisateur};{TypeAcces};";
        }

        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                TsCaRepertoireAcces objAsComposantDeploiement = (TsCaRepertoireAcces) obj;

                return Equals(objAsComposantDeploiement);
            }

            return false;
        }

        protected bool Equals(TsCaRepertoireAcces other)
        {
            //On valide seulement les champs clé primaire pour ne pas insérer de doublons et faire planter l'insertion.

            return TypeAcces == other.TypeAcces &&
                   Domaine == other.Domaine &&
                   Permission == other.Permission &&
                   DroitAccesHerite == other.DroitAccesHerite &&
                   GroupeUtilisateur == other.GroupeUtilisateur;

            //return _typeAccesControl == other._typeAccesControl && 
            //       _heritageFlag == other._heritageFlag && 
            //       _propagationFlag == other._propagationFlag && 
            //       Domaine == other.Domaine && 
            //       TypeIdentite == other.TypeIdentite && 
            //       Permission == other.Permission && 
            //       TypeAcces == other.TypeAcces && 
            //       GroupeUtilisateur == other.GroupeUtilisateur;

            //    //TODO
            //    //HeritageActiver == other.HeritageActiver &&
        }

        

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)_accessControlType;
                hashCode = (hashCode * 397) ^ (int)_inheritanceFlags;
                hashCode = (hashCode * 397) ^ (int) _propagationFlags;
                hashCode = (hashCode * 397) ^ (Domaine != null ? Domaine.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (NomIdentite != null ? NomIdentite.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Permission != null ? Permission.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (DroitAccesHerite.GetHashCode());
                hashCode = (hashCode * 397) ^ (TypeAcces != null ? TypeAcces.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (GroupeUtilisateur != null ? GroupeUtilisateur.GetHashCode() : 0);
                return hashCode;

                //hashCode = (hashCode * 397) ^ (HeritageActiver != null ? HeritageActiver.GetHashCode() : 0);
            }
        }

    }
}
