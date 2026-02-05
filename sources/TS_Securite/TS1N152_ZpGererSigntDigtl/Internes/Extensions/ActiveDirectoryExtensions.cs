using System.DirectoryServices;

namespace Rq.Infrastructure.Securite.SignatureDigital.Internes.Extensions
{
    internal static class ActiveDirectoryExtensions
    {
        public static void AjouterProprietesUtilisateur(this DirectorySearcher source)
        {
            source.PropertiesToLoad.Add("sAMAccountName");
            source.PropertiesToLoad.Add("employeeNumber");
            source.PropertiesToLoad.Add("sn");
            source.PropertiesToLoad.Add("givenname");
            source.PropertiesToLoad.Add("l");
        }

        public static string Prenom(this DirectoryEntry source)
        {
            return source.EnChaine("givenname");
        }
        public static string Nom(this DirectoryEntry source)
        {
            return source.EnChaine("sn");
        }
        public static string NumeroEmploye(this DirectoryEntry source)
        {
            return source.EnChaine("employeeNumber");
        }
        public static string Identifiant(this DirectoryEntry source)
        {
            return source.EnChaine("sAMAccountName");
        }
        public static string Ville(this DirectoryEntry source)
        {
            return source.EnChaine("l");
        }
        
        private static string EnChaine(this DirectoryEntry source, string nom)
        {
            var valeur = source.Properties[nom];

            if (valeur == null) return string.Empty;
            if (valeur.Count < 1) return string.Empty;
            if (valeur[0] == null) return string.Empty;

            return valeur[0].ToString();
        }
    }
}
