using Rq.Infrastructure.Securite.SignatureDigital.Internes.Extensions;
using Rq.Infrastructure.Securite.SignatureDigital.Internes.Signatures;

namespace Rq.Infrastructure.Securite.SignatureDigital.Internes.Enumerations
{
    internal class SignatureTypes
    {
        public static readonly SignatureTypes Normale = new SignatureTypes("Normal", 
            new[] { new EmployeeNumberSignatureFormatter() });

        public static readonly SignatureTypes GestionnaireMedecin = new SignatureTypes("Gestionnaire et médecin", 
            new[] { new EmployeeNameSignatureFormatter() });

        public static readonly SignatureTypes InfirmiereSEM = new SignatureTypes("Infirmière SEM", 
            new ISignatureFormatter[] { new EmployeeNameSignatureFormatter(), new EmployeeNumberSignatureFormatter() });


        public static SignatureTypes[] All = { Normale, GestionnaireMedecin, InfirmiereSEM };


        private readonly string _nom;
        private readonly ISignatureFormatter[] _formatters;

        private SignatureTypes(string nom, ISignatureFormatter[] formatter)
        {
            _nom = nom;
            _formatters = formatter;
        }

        public string Nom
        {
            get { return _nom; }
        }

        public bool Est(SignatureTypes valeur)
        {
            return _nom.Est(valeur._nom);
        }

        public ISignatureFormatter[] Formatters
        {
            get { return _formatters; }
        }

        public override string ToString()
        {
            return Nom;
        }
    }
}
