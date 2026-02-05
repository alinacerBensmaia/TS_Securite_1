using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace RRQ.Infrastructure.Securite.SignatureDigital.Internes.Commandes
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

        //public void AssignerProprietaireGenerique()
        //{
        //    AssignerProprietaire(AppConfiguration.ProprietaireGenerique);
        //}

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
                AjouterSecurite(_repertoire, compte, FileSystemRights.ReadAndExecute, AccessControlType.Allow);
        }


        public void AjouterAccesModification(string codeUtilisateur)
        {
            AjouterAccesModification(new string[] { codeUtilisateur });
        }
        public void AjouterAccesModification(string[] comptes)
        {
            foreach (var compte in comptes)
                AjouterSecurite(_repertoire, compte, FileSystemRights.Modify, AccessControlType.Allow);
        }


        public void AjouterAccesControlTotal(string codeUtilisateur)
        {
            AjouterAccesControlTotal(new string[] { codeUtilisateur });
        }
        public void AjouterAccesControlTotal(string[] comptes)
        {
            foreach (var compte in comptes)
                AjouterSecurite(_repertoire, compte, FileSystemRights.FullControl, AccessControlType.Allow);
        }


        //public void AjouterJournalisationNTFS(string[] comptes)
        //{

        //    if (AppConfiguration.AjouterAuditNTFS.Equals("O", StringComparison.InvariantCultureIgnoreCase))
        //    {
        //        AddFileAuditRule(_repertoire, comptes);
        //    }

        //}


        private void AjouterSecurite(string cible, string compte, FileSystemRights droits, AccessControlType permissions)
        {
            AjouterSecurite(cible, compte, droits, permissions, false);
        }
        private void AjouterSecurite(string cible, string compte, FileSystemRights droits, AccessControlType permissions, bool ceRepertoireSeulement)
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

        // Adds an ACL entry on the specified file for the specified account.
        private void AddFileAuditRule(string FileName, string[] Comptes)
        {

            //FileSecurity fSecurity = File.GetAccessControl(FileName);

            //// Add the FileSystemAuditRule to the security settings. 
            //foreach (var Account in Comptes)
            //{
            //    fSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.ListDirectory, AuditFlags.Failure));
            //    fSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.ListDirectory, AuditFlags.Success));
            //    fSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.CreateFiles, AuditFlags.Failure));
            //    fSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.CreateFiles, AuditFlags.Success));
            //    fSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.Delete, AuditFlags.Failure));
            //    fSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.Delete, AuditFlags.Success));
            //    fSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.TakeOwnership, AuditFlags.Failure));
            //    fSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.TakeOwnership, AuditFlags.Success));
            //    fSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.TakeOwnership, AuditFlags.Failure));
            //    fSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.TakeOwnership, AuditFlags.Success));
            //}

            //// Set the new access settings.
            //File.SetAccessControl(FileName, fSecurity);


            // Get a FileSecurity object that represents the 
            // current security settings.

            DirectorySecurity dSecurity = Directory.GetAccessControl(FileName);

            // Add the FileSystemAuditRule to the security settings. 
            foreach (var Account in Comptes)
            {
                dSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.ListDirectory, InheritanceFlags.ObjectInherit, PropagationFlags.None, AuditFlags.Failure));
                dSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.ListDirectory, InheritanceFlags.ObjectInherit, PropagationFlags.None, AuditFlags.Success));
                dSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.ListDirectory, InheritanceFlags.ContainerInherit, PropagationFlags.None, AuditFlags.Failure));
                dSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.ListDirectory, InheritanceFlags.ContainerInherit, PropagationFlags.None, AuditFlags.Success));

                dSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.CreateFiles, InheritanceFlags.ObjectInherit, PropagationFlags.None, AuditFlags.Failure));
                dSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.CreateFiles, InheritanceFlags.ObjectInherit, PropagationFlags.None, AuditFlags.Success));
                dSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.CreateFiles, InheritanceFlags.ContainerInherit, PropagationFlags.None, AuditFlags.Failure));
                dSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.CreateFiles, InheritanceFlags.ContainerInherit, PropagationFlags.None, AuditFlags.Success));


                dSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.Delete, InheritanceFlags.ObjectInherit, PropagationFlags.None, AuditFlags.Failure));
                dSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.Delete, InheritanceFlags.ObjectInherit, PropagationFlags.None, AuditFlags.Success));
                dSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.Delete, InheritanceFlags.ContainerInherit, PropagationFlags.None, AuditFlags.Failure));
                dSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.Delete, InheritanceFlags.ContainerInherit, PropagationFlags.None, AuditFlags.Success));

                dSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.ChangePermissions, InheritanceFlags.ObjectInherit, PropagationFlags.None, AuditFlags.Failure));
                dSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.ChangePermissions, InheritanceFlags.ObjectInherit, PropagationFlags.None, AuditFlags.Success));
                dSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.ChangePermissions, InheritanceFlags.ContainerInherit, PropagationFlags.None, AuditFlags.Failure));
                dSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.ChangePermissions, InheritanceFlags.ContainerInherit, PropagationFlags.None, AuditFlags.Success));


                dSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.CreateDirectories, InheritanceFlags.ObjectInherit, PropagationFlags.None, AuditFlags.Failure));
                dSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.CreateDirectories, InheritanceFlags.ObjectInherit, PropagationFlags.None, AuditFlags.Success));
                dSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.CreateDirectories, InheritanceFlags.ContainerInherit, PropagationFlags.None, AuditFlags.Failure));
                dSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.CreateDirectories, InheritanceFlags.ContainerInherit, PropagationFlags.None, AuditFlags.Success));

                dSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.TakeOwnership, InheritanceFlags.ContainerInherit, PropagationFlags.None, AuditFlags.Failure));
                dSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.TakeOwnership, InheritanceFlags.ContainerInherit, PropagationFlags.None, AuditFlags.Success));
                dSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.TakeOwnership, InheritanceFlags.ObjectInherit, PropagationFlags.None, AuditFlags.Failure));
                dSecurity.AddAuditRule(new FileSystemAuditRule(Account, FileSystemRights.TakeOwnership, InheritanceFlags.ObjectInherit, PropagationFlags.None, AuditFlags.Success));
            }

            // Set the new access settings.
            Directory.SetAccessControl(FileName, dSecurity);



        }

    }
}
