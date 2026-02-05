using Rq.Infrastructure.Securite.SignatureDigital.Internes.Extensions;
using System;

namespace Rq.Infrastructure.Securite.SignatureDigital.Internes.Enumerations
{

    internal class Lois
    {
        public static readonly Lois Toutes = new Lois("Toutes les lois", string.Empty);
        public static readonly Lois Des = new Lois("DES");
        public static readonly Lois Pfa = new Lois("PFA");
        public static readonly Lois Rcr = new Lois("RCR");
        public static readonly Lois Rev = new Lois("REV");
        public static readonly Lois Rrq = new Lois("RRQ");

        public static readonly Lois[] All = { Des, Pfa, Rcr, Rev, Rrq };


        private readonly string _nom;
        private readonly string _dossier;

        private Lois(string nom) : this(nom, nom)
        { }

        private Lois(string nom, string dossier)
        {
            _nom = nom;
            _dossier = dossier;
        }

        public bool Est(Lois valeur)
        {
            return _dossier.Est(valeur._dossier);
        }
        public bool Est(string nom)
        {
            return _nom.Est(nom);
        }

        public string Nom
        {
            get { return _nom; }
        }

        public string Dossier
        {
            get { return _dossier; }
        }

        public override string ToString()
        {
            return Nom;
        }

        public static Lois Parse(string nom)
        {
            if (Toutes.Est(nom)) return Toutes;
            foreach (var loi in All)
                if (loi.Est(nom)) return loi;
            throw new ArgumentException();
        }
    }
}
