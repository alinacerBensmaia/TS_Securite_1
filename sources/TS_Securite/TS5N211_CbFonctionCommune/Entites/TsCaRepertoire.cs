using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;

using Rrq.Securite;
using static System.IO.File;

namespace TS5N211_CbFonctionCommune.Entites
{

    public static class TypeSecuriteNtfs
    {
        public const string Audit = "Audit";
        public const string Autorisation = "Autorisation";
    }

    public class TsCaRepertoire
    {
        public string Racine { get; set; } = string.Empty;
        public string Chemin { get; set; } = string.Empty;
        public string CheminCourt { get; set; } = string.Empty;
        public string Proprietaire { get; set; } = string.Empty; //Ex: BUILTIN\Administrators

        public DateTime DateAnalyse { get; set; }

        public readonly List<TsCaRepertoireAcces> ListAcces = new List<TsCaRepertoireAcces>();
        public readonly List<TsCaRepertoireAudit> ListAudit = new List<TsCaRepertoireAudit>();

        public TsCaRepertoire()
        {
        }

        public TsCaRepertoire(string pRacine, string pChemin, DateTime pDateAnalyse)
        {
            Racine = pRacine;
            Chemin = pChemin;

            DateAnalyse = pDateAnalyse;

            CheminCourt = Racine != pChemin ? pChemin.Substring(pRacine.Length + 1) : "";

            Proprietaire = RecupererProprietaire();

            //Lancer la récupération de la sécurité NTFS et de l'audit en paralèlle

            DirectorySecurity fsAcces = new DirectorySecurity(pChemin, AccessControlSections.Access);
            DirectorySecurity fsAudit = new DirectorySecurity(pChemin, AccessControlSections.Audit);

            TraiterReglesAcces(fsAcces);
            TraiterReglesAudit(fsAudit);

            //var tacheAcces = Task.Factory.StartNew(() => TraiterReglesAcces(fsAcces));
            //var tacheAudit = Task.Factory.StartNew(() => TraiterReglesAudit(fsAudit));

            //Task.WaitAll(tacheAcces, tacheAudit);

            //TODO à retirer
            #region Test

            //DirectoryInfo d = new DirectoryInfo(pChemin);
            //DirectorySecurity acl = d.GetAccessControl();

            //if (acl.GetAccessRules(false, true, typeof(System.Security.Principal.SecurityIdentifier)).Count > 0)
            //{
            //    // -- has inherited permissions
            //    AuthorizationRuleCollection securityTest = acl.GetAccessRules(false, true, typeof(System.Security.Principal.SecurityIdentifier));

            //    // -- has no inherited permissions
            //    AuthorizationRuleCollection securityTest2 = acl.GetAccessRules(false, false, typeof(System.Security.Principal.SecurityIdentifier));

            //    AuthorizationRuleCollection securityTest22 = acl.GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));

            //    //Toute les permissions
            //    AuthorizationRuleCollection securityTest3 = acl.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));

            //    FileSecurity fs1 = new FileSecurity(pChemin, AccessControlSections.Access);
            //    IEnumerable<FileSystemAccessRule> securityTest33 = fs1.GetAccessRules(true, true, typeof(NTAccount)).OfType<FileSystemAccessRule>();

            //    AuthorizationRuleCollection securityTest4 = acl.GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));

            //}
            //else
            //{
            //    AuthorizationRuleCollection securityTest = acl.GetAccessRules(false, true, typeof(System.Security.Principal.SecurityIdentifier));
            //    AuthorizationRuleCollection securityTest2 = acl.GetAccessRules(false, false, typeof(System.Security.Principal.SecurityIdentifier));
            //    // -- has no inherited permissions
            //}

            #endregion

        }

        private string RecupererProprietaire()
        {
            //Récupérer le propriétaire
            try
            {
                FileSecurity security = GetAccessControl(Chemin);
                return security.GetOwner(typeof(NTAccount)).ToString();
            }
            catch (Exception)
            {
                return "Inconnu";
            }
        }

        private void TraiterReglesAcces(DirectorySecurity aclAcces)
        {

            TsCuAccesAD ad = new TsCuAccesAD();

            var listeReglesAcces = aclAcces.GetAccessRules(true, true, typeof(NTAccount)).OfType<FileSystemAccessRule>().ToArray();
            var listeReglesSid = aclAcces.GetAccessRules(true, true, typeof(SecurityIdentifier)).OfType<FileSystemAccessRule>().ToArray();

            //Si aucun accès n'est présent, ajouter un element vide dans la BD avec l'héritage à faux
            if (!listeReglesAcces.Any())
                ListAcces.Add(new TsCaRepertoireAcces {DroitAccesHerite = false});
            else
            {
                //Si tous les accès proviennent du répertoire parent, ajouter un élément vide dans la BD avec l'héritage à vrai'
                if (listeReglesAcces.All(r => r.IsInherited))
                    ListAcces.Add(new TsCaRepertoireAcces {DroitAccesHerite = true});
                else
                {
                    for (int i = 0; i < listeReglesAcces.Length; i++)
                    {
                        var nomGroupe = listeReglesAcces[i].IdentityReference.ToString();

                        //Ne pas ajouter lorsque c'est un accès hérité du parent 
                        if (listeReglesAcces[i].IsInherited) continue;

                        TsCaRepertoireAcces tsRa = new TsCaRepertoireAcces(listeReglesAcces[i], nomGroupe);

                        //Vérifier si un accès est déja présent dans la liste 
                        if (ListAcces.Contains(tsRa) == false) ListAcces.Add(tsRa);
                    }
                }
            }

        }

        private void TraiterReglesAudit(DirectorySecurity aclAcces)
        {
            TsCuAccesAD ad = new TsCuAccesAD();

            var listeReglesAudit = aclAcces.GetAuditRules(true,true,typeof(NTAccount)).OfType<FileSystemAuditRule>().ToArray();
            var listeReglesAuditSid = aclAcces.GetAuditRules(true,true,typeof(SecurityIdentifier)).OfType<FileSystemAuditRule>().ToArray();

            //Si aucun audit n'est présent, ajouter un élément vide dans la BD
            if (!listeReglesAudit.Any())
                ListAudit.Add(new TsCaRepertoireAudit {DroitAccesHerite = false});
            else
            {
                //Si tous les audit proviennent du répertoire parent, ajouter un audit vide dans la BD
                if (listeReglesAudit.All(r => r.IsInherited))
                    ListAudit.Add(new TsCaRepertoireAudit {DroitAccesHerite = true});
                else
                {
                    for (int i = 0; i < listeReglesAudit.Count(); i++)
                    {
                        //var nomGroupe = DeterminerNomGroupe(listeReglesAudit[i].IdentityReference.ToString(),
                        //    listeReglesAuditSid[i].IdentityReference.ToString(), ad);
                        var nomGroupe = listeReglesAudit[i].IdentityReference.ToString();

                        //Ne pas ajouter lorsque c'est un accès hérité du parent 
                        if (listeReglesAudit[i].IsInherited) continue;

                        var tsRa = new TsCaRepertoireAudit(TypeSecuriteNtfs.Audit, listeReglesAudit[i], nomGroupe);

                        //Vérifier si un accès est déja présent dans la liste 
                        if (ListAudit.Contains(tsRa) == false) ListAudit.Add(tsRa);
                    }
                }
            }
        }

        

        public static string DeterminerNomGroupe(string listeRegles, string listeReglesSid, TsCuAccesAD ad)
        {
            if (listeRegles.Contains(@"RRQ_QC\") || listeRegles.Contains(@"RQ\"))
            {
                try
                {
                    //C'est un groupe '
                    return ad.ObtenirGroupeParSID(listeReglesSid);
                }
                catch (TsCuSIdInexistantException)
                {
                    //C'est un usagé 
                    var lstUtl = tsCuObtnrInfoAD.ObtenirUtilisateurs(TsIadTypeRequete.TsIadTrSid, listeReglesSid);

                    if (lstUtl.Count > 0)
                    {
                        return lstUtl[0].DomaineNT + @"\" + lstUtl[0].CodeUtilisateur;
                    }
                    else
                    {
                        return "«SID introuvable :" + listeReglesSid;
                    }
                }
            }
            else
            {
                return listeRegles;
            }
        }

        /// <summary>Détermine si l'objet spécifié est identique à l'objet actuel.</summary>
        /// <returns>true si l'objet spécifié est égal à l'objet actuel ; sinon, false.</returns>
        /// <param name="obj">Objet à comparer à l'objet actuel. </param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            TsCaRepertoire objAsComposantDeploiement = (TsCaRepertoire) obj;

            return Equals(objAsComposantDeploiement);

        }

        protected bool Equals(TsCaRepertoire other)
        {
            return (ListAcces.Equals(other.ListAcces)) && CheminCourt == other.CheminCourt && Proprietaire == other.Proprietaire;

            //TODO
            //listAcces.Equals(other.listAcces) ne fonctionne pas retourne toujours false, à corriger.
        }

        public override int GetHashCode()
        {

            //On valide seulement les champs clé primaire pour ne pas insérer de doublons et faire planter l'insertion.

            unchecked
            {
                var hashCode = (ListAcces != null ? ListAcces.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CheminCourt != null ? CheminCourt.GetHashCode() : 0);
                //hashCode = (hashCode * 397) ^ (Proprietaire != null ? Proprietaire.GetHashCode() : 0);
                return hashCode;
            }
        }

        public TsCaRepertoire Clone()
        {
            TsCaRepertoire clone = new TsCaRepertoire
            {
                Proprietaire = this.Proprietaire, Chemin = this.Chemin, CheminCourt = this.CheminCourt
            };

            clone.Proprietaire = this.Proprietaire;
            clone.Racine = this.Racine;

            foreach (TsCaRepertoireAcces r in this.ListAcces)
            {
                clone.ListAcces.Add(r);
            }

            return clone;

        }

        /// <summary>Permet d'ajouter une règle d'audit</summary>
        /// <param name="account">Groupe ou utilisateur à ajouter à l'audit de sécurité. ex: "MYDOMAIN\MyAccount" </param>
        /// <param name="rights">Type d'accès à surveiller' ex: "FileSystemRights.ReadData," </param>
        /// <param name="auditRule">Règle de l'audit ex: "AuditFlags.Failure" </param>
        public void AjouterUneRegleAudit(string account, FileSystemRights rights, AuditFlags auditRule)
        {
            // Récupérer les sécurité du dossier. 
            var fSecurity = File.GetAccessControl(Chemin);

            // Ajouter au FileSystemAuditRule à la sécurité. 
            // Exemple:  AddFileAuditRule(FileName, @"MYDOMAIN\MyAccount", FileSystemRights.ReadData, AuditFlags.Failure);
            
            fSecurity.AddAuditRule(new FileSystemAuditRule(account,
                                                            rights,
                                                            auditRule));

            // Mettre en place le changement
            File.SetAccessControl(Chemin, fSecurity);

        }

        /// <summary>Permet de retirer une règle d'audit</summary>
        /// <param name="account">Groupe ou utilisateur à ajouter à l'audit de sécurité. ex: "MYDOMAIN\MyAccount" </param>
        /// <param name="rights">Type d'accès à surveiller' ex: "FileSystemRights.ReadData," </param>
        /// <param name="auditRule">Règle de l'audit ex: "AuditFlags.Failure" </param>
        public void RetirerUneRegleAudit(string account,FileSystemRights rights, AuditFlags auditRule)
        {

            // Get a FileSecurity object that represents the current security settings.
            FileSecurity fSecurity = File.GetAccessControl(Chemin);
            
            // Add the FileSystemAuditRule to the security settings. 
            fSecurity.RemoveAuditRule(new FileSystemAuditRule(account,
                                                            rights,
                                                            auditRule));

            // Set the new access settings.
            File.SetAccessControl(Chemin, fSecurity);

        }

      

    }
}
