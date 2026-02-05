namespace Rq.Infrastructure.Securite.SignatureDigital.Internes.Profils
{
    internal abstract class Profil
    {
        private bool _existe;
        private string _chemin;

        protected void RafraichirPourAffichage(bool existe, string chemin)
        {
            _existe = existe;
            _chemin = chemin;
        }

        public bool Existe { get { return _existe; } }

        public string Chemin { get { return _chemin; } }

        public abstract void Creer();

        public abstract void Supprimer();
    }
}
