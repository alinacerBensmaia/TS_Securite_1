using Rq.Infrastructure.Securite.SignatureDigital.Internes.Commandes;
using System.IO;

namespace Rq.Infrastructure.Securite.SignatureDigital.Internes.Profils
{
    internal class ProfilCoba : Profil
    {
        private readonly Utilisateur _utilisateur;

        public ProfilCoba(Utilisateur utilisateur)
        {
            _utilisateur = utilisateur;

            var chemin = Path.Combine(AppConfiguration.RepertoireCoba, utilisateur.Identifiant.ToUpper());
            RafraichirPourAffichage(Directory.Exists(chemin), chemin);
        }

        public override void Creer()
        {
            if (Directory.Exists(Chemin)) return;

            Directory.CreateDirectory(Chemin);

            var securite = new SecuriteRepertoire(Chemin);
            securite.AssignerProprietaire(_utilisateur.IdentifiantAvecDomaine);

            RafraichirPourAffichage(true, Chemin);
        }

        public override void Supprimer()
        {
            if (Directory.Exists(Chemin))
            {
                var securite = new SecuriteRepertoire(Chemin);
                securite.AssignerProprietaireGenerique();

                Directory.Delete(Chemin, true);
            }

            RafraichirPourAffichage(false, Chemin);
        }
    }
}
