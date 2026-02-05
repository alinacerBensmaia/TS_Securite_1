using Rq.Infrastructure.Securite.SignatureDigital.Internes.Commandes;
using Rq.Infrastructure.Securite.SignatureDigital.Internes.Enumerations;
using Rq.Infrastructure.Securite.SignatureDigital.Internes.Extensions;
using System;
using System.IO;

namespace Rq.Infrastructure.Securite.SignatureDigital.Internes.Profils
{
    internal class Profil2000 : Profil
    {
        private readonly Utilisateur _utilisateur;
        private string _racineChemin;

        public Profil2000(Utilisateur utilisateur)
        {
            _utilisateur = utilisateur;

            var ville = Villes.Parse(utilisateur.Ville);
            var chemin = utilisateur.CheminProfil;

            if (ville.Est(Villes.Aucune))
                throw new ArgumentException("ville");

            //si valeur valide et répertoire existe
            if (!string.IsNullOrWhiteSpace(chemin) && Directory.Exists(chemin))
            {
                //la racine du répertoire de profile ce termine avec l'itentifiant
                var startIndex = chemin.IndexOf(utilisateur.Identifiant, StringComparison.InvariantCultureIgnoreCase);
                startIndex += utilisateur.Identifiant.Length;
                _racineChemin = chemin.Remove(startIndex);

                RafraichirPourAffichage(true, chemin);
            }
            else
            {
                chemin = determinerChemin(utilisateur);
                RafraichirPourAffichage(false, chemin);
            }
        }

        public override void Creer()
        {
            var ad = new ActiveDirectory();
            ad.AssignerProfilePath(_utilisateur, Chemin);


            string proprietaire = _utilisateur.IdentifiantAvecDomaine;
            SecuriteRepertoire securite;
            string repertoire;                                  

            //--- Répertoire racine
            repertoire = _racineChemin;
            securite = new SecuriteRepertoire(repertoire, true);
            securite.BriserHeritage();
            securite.AjouterAccesLectureEtExecution(new string[] { "AdmSgr CS_Corpo".AtRQ() });
            securite.AjouterAccesModification(_utilisateur.IdentifiantAvecDomaine);
            securite.AjouterAccesControlTotal(new string[] { "Administrators", "AdmResp InfoBoutique".AtRQ(), "AdmResp Securite".AtRQ() });
            securite.AssignerProprietaire(proprietaire);


            //--- --- Mes Documents
            repertoire = Path.Combine(_racineChemin, @"Mes documents");
            securite = new SecuriteRepertoire(repertoire, true);
            securite.BriserHeritage();
            securite.AjouterAccesModification(_utilisateur.IdentifiantAvecDomaine);
            securite.AjouterAccesControlTotal(new string[] { "Administrators", "AdmResp InfoBoutique".AtRQ(), "AdmResp Securite".AtRQ() });
            securite.AssignerProprietaire(proprietaire);

            //--- --- Mes Documents/Mes Images
            repertoire = Path.Combine(_racineChemin, @"Mes documents\Mes Images");
            securite = new SecuriteRepertoire(repertoire, true);
            securite.AssignerProprietaire(proprietaire);


            //--- Profil
            repertoire = Path.Combine(_racineChemin, @"Profil");
            securite = new SecuriteRepertoire(repertoire, true);
            securite.Masquer();
            securite.BriserHeritage();
            securite.AjouterAccesModification(new string[] { _utilisateur.IdentifiantAvecDomaine, "AdmSgr CS_Corpo".AtRQ() });
            securite.AjouterAccesControlTotal(new string[] { "Administrators", "AdmResp InfoBoutique".AtRQ(), "AdmResp Securite".AtRQ() });
            securite.AssignerProprietaire(proprietaire);

            //--- --- Profil/Prof2000
            repertoire = Path.Combine(_racineChemin, @"Profil\Prof2000");
            securite = new SecuriteRepertoire(repertoire, true);
            securite.Masquer();
            securite.AssignerProprietaire(proprietaire);

            //--- --- Profil/Prof2000.V2
            repertoire = Path.Combine(_racineChemin, @"Profil\Prof2000.V2");
            securite = new SecuriteRepertoire(repertoire, true);
            securite.Masquer();
            securite.AssignerProprietaire(proprietaire);

            RafraichirPourAffichage(true, Chemin);
        }
        
        public override void Supprimer()
        {
            if (Directory.Exists(_racineChemin))
            {
                var ad = new ActiveDirectory();
                ad.AssignerProfilePath(_utilisateur, null);

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

            return Path.Combine(_racineChemin, @"profil\prof2000");
        }
    }
}
