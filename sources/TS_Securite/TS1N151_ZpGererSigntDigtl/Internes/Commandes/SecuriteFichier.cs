using System.IO;
using System.Security.AccessControl;

namespace Rq.Infrastructure.Securite.SignatureDigital.Internes.Commandes
{
    internal class SecuriteFichier
    {
        private readonly string _fichier;

        public SecuriteFichier(string fichier)
        {
            _fichier = fichier;
        }

        public void AjouterAccesLectureEtExecution(string codeUtilisateur)
        {
            ajouterSecurite(_fichier, codeUtilisateur, FileSystemRights.ReadAndExecute, AccessControlType.Allow);
        }

        private void ajouterSecurite(string cible, string account, FileSystemRights rights, AccessControlType controlType)
        {
            //https://docs.microsoft.com/en-us/dotnet/api/system.io.file.setaccesscontrol?view=netframework-4.7.2

            // Get a FileSecurity object that represents the
            // current security settings.
            FileSecurity accessControl = File.GetAccessControl(cible);

            // Add the FileSystemAccessRule to the security settings.
            accessControl.AddAccessRule(new FileSystemAccessRule(account, rights, controlType));

            // Set the new access settings.
            File.SetAccessControl(cible, accessControl);
        }
    }
}
