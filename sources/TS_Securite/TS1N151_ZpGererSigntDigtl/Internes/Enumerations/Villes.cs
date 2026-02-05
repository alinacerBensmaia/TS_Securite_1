using Rq.Infrastructure.Securite.SignatureDigital.Internes.Extensions;
using System;

namespace Rq.Infrastructure.Securite.SignatureDigital.Internes.Enumerations
{
    internal class Villes
    {
        public static readonly Villes Aucune = new Villes(string.Empty, string.Empty);
        public static readonly Villes Quebec = new Villes("Québec", AppConfiguration.RepertoireProfilQuebec);
        public static readonly Villes Montreal = new Villes("Montréal", AppConfiguration.RepertoireProfilMontreal);
        public static readonly Villes TroisRivieres = new Villes("Trois-Rivières", AppConfiguration.RepertoireProfilTroisRivieres);
        public static readonly Villes Regions = new Villes("Régions", AppConfiguration.RepertoireProfilQuebec);

        public static readonly Villes[] All = { Quebec, Montreal, TroisRivieres, Regions };

        private readonly string _nom;
        private readonly string _code;
        private readonly string _repertoireProfil;

        private Villes(string affichage, string repertoireProfil) : this(affichage, affichage, repertoireProfil)
        { }

        private Villes(string nom, string code, string repertoireProfil)
        {
            _nom = nom;
            _code = code;
            _repertoireProfil = repertoireProfil;
        }

        public bool Est(Villes valeur)
        {
            return _code.Est(valeur._code);
        }
        public bool Est(string nom)
        {
            return _nom.Est(nom);
        }

        public string Nom
        {
            get { return _nom; }
        }

        public string Code
        {
            get { return _code; }
        }

        public string RepertoireProfil
        {
            get { return _repertoireProfil; }
        }

        public override string ToString()
        {
            return Nom;
        }

        public static Villes Parse(string nom)
        {            
            foreach (var item in All)
                if (item.Est(nom)) return item;
            return Aucune;
        }
    }
}
