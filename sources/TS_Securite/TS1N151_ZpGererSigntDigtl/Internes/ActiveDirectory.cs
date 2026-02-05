using Rq.Infrastructure.Securite.SignatureDigital.Internes.Extensions;
using System;
using System.DirectoryServices;

namespace Rq.Infrastructure.Securite.SignatureDigital.Internes
{
    internal class ActiveDirectory
    {
        public Utilisateur ObtenirUtilisateur(string identifiant)
        {
            try
            {
                using (var de = new DirectoryEntry(AppConfiguration.UtilisateursOU))
                using (var ds = new DirectorySearcher(de))
                {
                    ds.AjouterProprietesUtilisateur();
                    ds.Filter = string.Format("sAMAccountName={0}", identifiant);

                    var resultat = ds.FindOne();
                    if (resultat == null) return Utilisateur.Invalide;

                    using (var utilisateur = resultat.GetDirectoryEntry())
                    {
                        return new Utilisateur(utilisateur.Identifiant(), 
                            utilisateur.Nom(), 
                            utilisateur.Prenom(), 
                            utilisateur.NumeroEmploye(), 
                            utilisateur.Ville(), 
                            utilisateur.CheminProfil());
                    }
                }
            }
            catch (Exception)
            {
                return Utilisateur.Invalide;
            }
        }

        internal void AssignerProfilePath(Utilisateur utilisateur, string chemin)
        {
            using (var de = new DirectoryEntry(AppConfiguration.UtilisateursOU))
            using (var ds = new DirectorySearcher(de))
            {
                ds.AjouterProprietesUtilisateur();
                ds.Filter = string.Format("sAMAccountName={0}", utilisateur.Identifiant);

                var resultat = ds.FindOne();
                using (var entry = resultat.GetDirectoryEntry())
                {
                    entry.Properties["profilePath"].Value = chemin;
                    entry.CommitChanges();
                }
            }
        }
    }
}
