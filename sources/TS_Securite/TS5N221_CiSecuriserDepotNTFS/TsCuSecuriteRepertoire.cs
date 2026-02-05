using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using TS5N211_CbFonctionCommune;

namespace TS5N221_CiSecuriserDepotNTFS
{
    class TsCuSecuriteRepertoire
    {
        private string _repertoire;
        private DirectoryInfo _infoRepertoire;
        private DirectorySecurity _securiteRepertoire;
        
      
       
        //public DirectoryInfo InfoRepertoire { get => _infoRepertoire; set => _infoRepertoire = value; }
        public DirectoryInfo InfoRepertoire
        {
            get
            {
                return _infoRepertoire;
            }

            set
            {
                _infoRepertoire = value;
            }
        }
        //public DirectorySecurity SecuriteRepertoire { get => _securiteRepertoire; set => _securiteRepertoire = value; }
        public DirectorySecurity SecuriteRepertoire
        {
            get
            {
                return _securiteRepertoire;
            }

            set
            {
                _securiteRepertoire = value;
            }
        }
        //public string Repertoire { get => _repertoire; internal set =>_repertoire = value; }
        public string Repertoire
        {
            get
            {
                return _repertoire;
            }

            set
            {
                _repertoire = value;
            }
        }


        //public TsCuSecuriteRepertoire(string repertoire) : this(repertoire, false)
        //{ }
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="repertoire"></param>
        /// <param name="creer"></param>
        public TsCuSecuriteRepertoire(string repertoire, bool creer)
        {
            if (!Directory.Exists(repertoire))
            {
                if (creer)
                    Directory.CreateDirectory(repertoire);
                else
                    throw new ArgumentException("repertoire");
            }

            _infoRepertoire = new DirectoryInfo(repertoire);
            _securiteRepertoire = _infoRepertoire.GetAccessControl();
            _repertoire = repertoire;
           
        }
        #region --- Règle audit ---
      
        
        public void AjouterRegleAuditHerite(string cible)
        {
            _securiteRepertoire.SetAuditRuleProtection(false, true);
        }
        #endregion


        public string SupprimerRegleAccesHerite(TsCaRegleAAppliquer regle)
        {
            string message;
            try
            {
                _securiteRepertoire.SetAccessRuleProtection(false, false);
                message = "SUCCÈS -->La règle d'accès hérité a été supprimée avec succès au répertoire" + regle.Chemin + Environment.NewLine;
            }
            catch (Exception ex)
            {
                return "ERREUR --> cible: " + regle.Chemin + " groupe: " + regle.NomIdentite + "-->" +
                       "Erreur dans SecuriteRepertoire.SupprimerRegleAccesHerite(): " + ex.Message + Environment.NewLine;
            }
            return message;
        }

        //
        public string AjouterRegleAccesHerite(TsCaRegleAAppliquer regle)
        {
            string message;
            try
            {
                _securiteRepertoire.SetAccessRuleProtection(false, true);
               message = "SUCCÈS -->La règle d'accès hérité a été ajouté avec succès au répertoire" + regle.Chemin + Environment.NewLine;
            }
            catch (Exception ex)
            {
                return "ERREUR --> cible: " + regle.Chemin + " groupe: " + regle.NomIdentite + "-->" +
                       "Erreur dans SecuriteRepertoire.AjouterRegleAccesHerite(): " + ex.Message + Environment.NewLine;
            }
            return message;
                   
        }
        public string AjouterRegleAcces(TsCaRegleAAppliquer regle)
        {
            try
            {
                string message;

                if (regle.MessageErreur.Contains("ERREUR"))
                {
                    message=regle.MessageErreur;
                }
                else if (ValiderRegle(regle))
                {
                  
                        foreach (var permission in regle.ListePermissions)
                        {
                            if (permission == FileSystemRights.Synchronize) continue;

                            FileSystemAccessRule regleAcces = new FileSystemAccessRule(regle.NomIdentite, permission, (InheritanceFlags)Enum.Parse(typeof(InheritanceFlags), regle.TypeHeritage), (PropagationFlags)Enum.Parse(typeof(PropagationFlags), regle.TypePropagationEnfants), (AccessControlType)Enum.Parse(typeof(AccessControlType), regle.TypeAcces));
                            _securiteRepertoire.AddAccessRule(regleAcces);
                            
                        }
                        message = "SUCCÈS -->La règle d'autorisation a été ajoutée avec succès au répertoire" + regle.Chemin + Environment.NewLine;               
                }
                else
                    message = "ERREUR --> cible: " + regle.Chemin + " groupe: " + regle.NomIdentite + "-->La règle d'autorisation  contient des erreurs en entrées" + Environment.NewLine;

                return message;

            }
            catch (Exception ex)
            {
               return "ERREUR --> cible: " + regle.Chemin + " groupe: " + regle.NomIdentite + "-->" + "Erreur dans SecuriteRepertoire.AjouterRegleAcces(): " + ex.Message + Environment.NewLine;
            }
        }
        public string SupprimerRegleAcces(TsCaRegleAAppliquer regle)
        {
            //System.Diagnostics.Debugger.Launch();
            string message;
            try
            {
                if (ValiderRegle(regle))
                {
                        foreach (var permission in regle.ListePermissions)
                        {
                            if (permission != FileSystemRights.Synchronize)
                            {
                                FileSystemAccessRule regleAcces = new FileSystemAccessRule(regle.NomIdentite, permission, (InheritanceFlags)Enum.Parse(typeof(InheritanceFlags), regle.TypeHeritage), (PropagationFlags)Enum.Parse(typeof(PropagationFlags), regle.TypePropagationEnfants), (AccessControlType)Enum.Parse(typeof(AccessControlType), regle.TypeAcces));
                                _securiteRepertoire.RemoveAccessRule(regleAcces);
                            }
                        }
                        message = "SUCCÈS -->La règle d'autorisation a été supprimée avec succès au répertoire" + regle.Chemin + Environment.NewLine;                    
                }
                else
                {
                    message = "ERREUR --> cible: " + regle.Chemin + " groupe: " + regle.NomIdentite + "-->La règle d'autorisation contient des erreurs en entrées"+Environment.NewLine;
                }

                return message;
            }
            catch (Exception ex)
            {
                return "ERREUR --> cible: " + regle.Chemin + " groupe: " + regle.NomIdentite + "-->" + "Erreur dans SecuriteRepertoire.SupprimerRegleAcces(): " + ex.Message + Environment.NewLine;
            }
        }
        public string SupprimerRegleAccesSID(TsCaRegleAAppliquer regle)
        {
            string message;
            try
            {
                if (ValiderRegle(regle))
                {
                    foreach (var permission in regle.ListePermissions)
                        {
                            if (permission != FileSystemRights.Synchronize)
                            {
                                SecurityIdentifier sid = new SecurityIdentifier(regle.NomIdentite);
                                _securiteRepertoire.PurgeAccessRules(sid);
                            }
                            
                        }
                    message = "SUCCÈS -->La règle d'autorisation du SID" +regle.NomIdentite +" a été supprimée avec succès au répertoire" + regle.Chemin + Environment.NewLine;
                }
                else
                {
                         message = "ERREUR --> cible: " + regle.Chemin + " groupe: " + regle.NomIdentite + "-->La règle d'autorisation contient des erreurs en entrées. SecuriteRepertoire.SupprimerRegleAccesSID():"+ Environment.NewLine;
                }
                return message;
            }

            catch (Exception ex)
            {
                return "ERREUR --> cible: " + regle.Chemin + " groupe: " + regle.NomIdentite + "-->" + "Erreur dans SecuriteRepertoire.SupprimerRegleAccesSID(): " + ex.Message + Environment.NewLine;
            }
            
            
        }
        
        private bool ValiderRegle(TsCaRegleAAppliquer regle)
        {
            InheritanceFlags res1;
            PropagationFlags res2;
            AccessControlType res3;
            return (Enum.TryParse(regle.TypeHeritage, out res1) &&
                    Enum.TryParse(regle.TypePropagationEnfants, out res2) &&
                    Enum.TryParse(regle.TypeAcces, out res3));    
        }
       
        public string AppliquerRegles()
        {
            string message;
            try
            {
                _infoRepertoire.SetAccessControl(_securiteRepertoire);
                message = "Succès --> repertoire:" + _infoRepertoire.FullName + " Règle d'accès:" + _securiteRepertoire.AccessRuleType + Environment.NewLine;
            }
            catch (Exception ex)
            {
                return "ERREUR --> repertoire:" +_infoRepertoire.FullName+ " Règle d'accès:" + _securiteRepertoire.AccessRuleType + Environment.NewLine + ex.Message;
            }
            return message;
        }

    }
}
    

