using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Rq.Infrastructure.Securite.SignatureDigital.Internes.Commandes
{
    internal class SecuriteRepertoire
    {
        private readonly string _repertoire;

        public SecuriteRepertoire(string repertoire) : this(repertoire, false)
        { }
        public SecuriteRepertoire(string repertoire, bool creer)
        {
            if (!Directory.Exists(repertoire))
            {
                if (!creer)
                    throw new ArgumentException("repertoire");
                else
                    Directory.CreateDirectory(repertoire);
            }

            _repertoire = repertoire;
        }

        public void BriserHeritage()
        {
            BriserHeritage(false);
        }
        public void BriserHeritage(bool conserverPermissions)
        {
            DirectorySecurity accessControl = Directory.GetAccessControl(_repertoire);
            accessControl.SetAccessRuleProtection(true, conserverPermissions);
            Directory.SetAccessControl(_repertoire, accessControl);
        }

        public void RetablirHeritage()
        {
            var parametreIgnore = true;

            DirectorySecurity accessControl = Directory.GetAccessControl(_repertoire);
            accessControl.SetAccessRuleProtection(false, parametreIgnore);
            Directory.SetAccessControl(_repertoire, accessControl);
        }

        public void Masquer()
        {
            var info = Directory.CreateDirectory(_repertoire);
            info.Attributes = FileAttributes.Hidden;
        }

        public void AssignerProprietaireGenerique()
        {
            AssignerProprietaire(AppConfiguration.ProprietaireGenerique);
        }

        public void AssignerProprietaire(string proprietaire)
        {
            var winId = new WindowsIdentity(proprietaire);
            IdentityReference owner = winId.User;

            var directory = new DirectoryInfo(_repertoire);
            var directorySecurity = directory.GetAccessControl();

            directorySecurity.SetOwner(owner);
            directory.SetAccessControl(directorySecurity);
        }


        public void AjouterAccesLectureEtExecution(string codeUtilisateur)
        {
            AjouterAccesLectureEtExecution(new string[] { codeUtilisateur });
        }
        public void AjouterAccesLectureEtExecution(string[] comptes)
        {
            foreach (var compte in comptes)
                ajouterSecurite(_repertoire, compte, FileSystemRights.ReadAndExecute, AccessControlType.Allow);
        }


        public void AjouterAccesModification(string codeUtilisateur)
        {
            AjouterAccesModification(new string[] { codeUtilisateur });
        }
        public void AjouterAccesModification(string[] comptes)
        {
            foreach (var compte in comptes)
                ajouterSecurite(_repertoire, compte, FileSystemRights.Modify, AccessControlType.Allow);
        }


        public void AjouterAccesControlTotal(string codeUtilisateur)
        {
            AjouterAccesControlTotal(new string[] { codeUtilisateur });
        }
        public void AjouterAccesControlTotal(string[] comptes)
        {
            foreach (var compte in comptes)
                ajouterSecurite(_repertoire, compte, FileSystemRights.FullControl, AccessControlType.Allow);
        }
                            


        private void ajouterSecurite(string cible, string compte, FileSystemRights droits, AccessControlType permissions)
        {
            ajouterSecurite(cible, compte, droits, permissions, false);
        }
        private void ajouterSecurite(string cible, string compte, FileSystemRights droits, AccessControlType permissions, bool ceRepertoireSeulement)
        {
            //https://docs.microsoft.com/en-us/dotnet/api/system.io.file.setaccesscontrol?view=netframework-4.7.2

            // Obtient un objet FileSecurity qui represente les parametre de securite actuels
            var accessControl = Directory.GetAccessControl(cible);

            if (ceRepertoireSeulement)
                accessControl.AddAccessRule(new FileSystemAccessRule(compte, droits, permissions));
            else
            {
                // Ajouter les paramèetres de securite pour : ce répertoire, les sous-répertoires et les fichiers
                accessControl.AddAccessRule(new FileSystemAccessRule(compte, droits, InheritanceFlags.ContainerInherit, PropagationFlags.None, permissions));
                accessControl.AddAccessRule(new FileSystemAccessRule(compte, droits, InheritanceFlags.ObjectInherit, PropagationFlags.None, permissions));
            }

            // Set the new access settings.
            Directory.SetAccessControl(cible, accessControl);
        }
    }
}
