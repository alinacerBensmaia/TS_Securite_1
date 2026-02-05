using System.IO;

namespace Rq.Infrastructure.Securite.SignatureDigital.Internes
{
    internal class Utilisateur
    {
        public static Utilisateur Invalide = new Utilisateur();

        private readonly string _identifiant;        
        private readonly string _prenom;
        private readonly string _nom;
        private readonly string _numeroEmploye;
        private readonly string _ville;
        private readonly string _identifiantAvecDomaine;

        private Utilisateur()
        {
            Valide = false;
        }
        public Utilisateur(string identifiant, string nom, string prenom, string numeroEmploye, string ville)
        {
            Valide = true;
            _identifiant = identifiant;
            _nom = nom;
            _prenom = prenom;
            _numeroEmploye = numeroEmploye.PadLeft(5, '0'); ;
            _ville = ville;

            var u = Rrq.Securite.tsCuObtnrInfoAD.ObtenirUtilisateur(identifiant);
            _identifiantAvecDomaine = string.Format("{0}@{1}", Identifiant, u.ServeurActiveDirectory);
        }

        public bool Valide { get; }

        public string Identifiant
        {
            get
            {
                if (!Valide) throw new InvalidDataException();
                return _identifiant;
            }
        }

        public string IdentifiantAvecDomaine
        {
            get
            {
                if (!Valide) throw new InvalidDataException();
                return _identifiantAvecDomaine;
            }
        }
        public string Prenom
        {
            get
            {
                if (!Valide) throw new InvalidDataException();
                return _prenom;
            }
        }
        public string Nom
        {
            get
            {
                if (!Valide) throw new InvalidDataException();
                return _nom;
            }
        }
        public string NumeroEmploye
        {
            get
            {
                if (!Valide) throw new InvalidDataException();
                return _numeroEmploye;
            }
        }
        public string Ville
        {
            get
            {
                if (!Valide) throw new InvalidDataException();
                return _ville;
            }
        }
    }
}
