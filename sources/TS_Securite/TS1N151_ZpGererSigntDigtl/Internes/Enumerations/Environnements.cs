using Rq.Infrastructure.Securite.SignatureDigital.Internes.Extensions;
using System;

namespace Rq.Infrastructure.Securite.SignatureDigital.Internes.Enumerations
{
    internal class Environnements
    {
        public static readonly Environnements Depot = new Environnements("Dépot");
        public static readonly Environnements Production = new Environnements("Production");
        public static readonly Environnements[] All = { Depot, Production };

        private readonly string _nom;
        private Environnements(string nom)
        {
            _nom = nom;
        }

        public string Nom { get { return _nom; } }

        public bool Est(Environnements valeur)
        {
            return _nom.Est(valeur._nom);
        }
        public bool Est(string valeur)
        {
            return _nom.Est(valeur);
        }

        public override string ToString()
        {
            return Nom;
        }

        public static Environnements Parse(string nom)
        {
            foreach (var env in All)
                if (env.Est(nom)) return env;
            throw new ArgumentException();
        }
    }
}
