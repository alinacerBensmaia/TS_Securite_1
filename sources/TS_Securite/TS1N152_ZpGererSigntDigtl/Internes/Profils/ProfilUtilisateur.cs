using Rq.Infrastructure.Securite.SignatureDigital.Internes.Commandes;
using Rq.Infrastructure.Securite.SignatureDigital.Internes.Enumerations;
using Rq.Infrastructure.Securite.SignatureDigital.Internes.Extensions;
using System;
using System.IO;

namespace Rq.Infrastructure.Securite.SignatureDigital.Internes.Profils
{
    internal class ProfilUtilisateur : Profil
    {
        private readonly Utilisateur _utilisateur;
        private string _racineChemin;

        public ProfilUtilisateur(Utilisateur utilisateur)
        {
            _utilisateur = utilisateur;

            var ville = Villes.Parse(utilisateur.Ville);
            _racineChemin = ville.RepertoireProfil;
            var chemin = determinerChemin(utilisateur);

            if (ville.Est(Villes.Aucune))
                throw new ArgumentException("ville");

            //si valeur valide et répertoire existe
            if (!string.IsNullOrWhiteSpace(chemin) && Directory.Exists(chemin))
            {

                RafraichirPourAffichage(true, chemin);
            }
            else
            {
                RafraichirPourAffichage(false, chemin);
            }
        }

        public override void Creer()
        {


            string proprietaire = _utilisateur.IdentifiantAvecDomaine;
            SecuriteRepertoire securite;
            string repertoire;                                  

            //--- Répertoire racine
            repertoire = _racineChemin;
            securite = new SecuriteRepertoire(repertoire, true);
            securite.AssignerProprietaire(proprietaire);


            //--- --- Mes Documents
            repertoire = Path.Combine(_racineChemin, @"Mes documents");
            securite = new SecuriteRepertoire(repertoire, true);
            securite.BriserHeritage();
            securite.AjouterAccesModification(_utilisateur.IdentifiantAvecDomaine);
            securite.AjouterAccesControlTotal(new string[] { ".\\Administrators", "ROD_PFT_Gestion Mes Documents".AtRQ() });
            securite.AjouterJournalisationNTFS(new string[] { ".\\Administrators", "ROD_PFT_Gestion Mes Documents".AtRQ() });
            securite.AssignerProprietaire(proprietaire);

            //--- --- Mes Documents/Mes Images
            repertoire = Path.Combine(_racineChemin, @"Mes documents\Mes Images");
            securite = new SecuriteRepertoire(repertoire, true);
            securite.AssignerProprietaire(proprietaire);

            RafraichirPourAffichage(true, Chemin);
        }
        
        public override void Supprimer()
        {
            if (Directory.Exists(_racineChemin))
            {

                var securite = new SecuriteRepertoire(_racineChemin);
                securite.AssignerProprietaireGenerique();
                Directory.Delete(_racineChemin, true);

                var journal = new JournalEvenements();
                journal.JournaliserSuppressionRepertoiresProfil(_utilisateur);
            }

            //on doit redeterminer le chemin au cas ou le chemin supprimé était d'une ancienne nomenclature
            var chemin = determinerChemin(_utilisateur);
            RafraichirPourAffichage(false, chemin);
        }
                 
        private string determinerChemin(Utilisateur utilisateur)
        {
            var ville = Villes.Parse(utilisateur.Ville);
            _racineChemin = Path.Combine(ville.RepertoireProfil, utilisateur.Identifiant);

            return _racineChemin;
        }
    }
}
