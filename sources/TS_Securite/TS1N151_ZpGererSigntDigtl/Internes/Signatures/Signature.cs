using Rq.Infrastructure.Securite.SignatureDigital.Internes.Enumerations;
using System.IO;

namespace Rq.Infrastructure.Securite.SignatureDigital.Internes.Signatures
{
    internal class Signature
    {
        public static Signature Invalide = new Signature();

        private readonly string _emplacement;
        private readonly string _fichier;
        private readonly Lois _loi;
                

        private Signature()
        {
            Valide = false;
        }

        public Signature(Lois loi, string emplacement, string fichier)
        {
            Valide = true;
            _emplacement = emplacement;
            _fichier = fichier;
            _loi = loi;
        }


        public bool Valide { get; }

        public string Emplacement
        {
            get
            {
                if (!Valide) throw new InvalidDataException();
                return _emplacement;
            }
        }
        public string Fichier
        {
            get
            {
                if (!Valide) throw new InvalidDataException();
                return _fichier;
            }
        }

        public Lois Loi
        {
            get
            {
                if (!Valide) throw new InvalidDataException();
                return _loi;
            }
        }

        public string FullPath
        {
            get
            {
                if (!Valide) throw new InvalidDataException();
                return Path.Combine(_emplacement, _fichier);
            }
        }
    }
}
