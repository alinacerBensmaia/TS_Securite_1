using System.Windows.Forms;
using Rq.Infrastructure.Securite.SignatureDigital.Internes;
using Rq.Infrastructure.Securite.SignatureDigital.Internes.Extensions;
using Rq.Infrastructure.Securite.SignatureDigital.Internes.Enumerations;

namespace Rq.Infrastructure.Securite.SignatureDigital.Controls
{
    public partial class UtilisateurInfo : UserControl
    {
        private Utilisateur _utilisateur;

        public UtilisateurInfo()
        {
            InitializeComponent();
            cboVille.PopulerVilles();
        }

        internal Utilisateur UtilisateurCourant
        {
            get { return _utilisateur; }
            private set { _utilisateur = value; }
        }

        internal void Afficher(Utilisateur utilisateur)
        {
            if (utilisateur == null || !utilisateur.Valide)
            {
                Nettoyer();
                return;
            }

            UtilisateurCourant = utilisateur;
            txtPrenom.Text = utilisateur.Prenom;
            txtNom.Text = utilisateur.Nom;
            txtNumeroEmploye.Text = utilisateur.NumeroEmploye;
            txtIdentifiant.Text = utilisateur.Identifiant;

            var ville = Villes.Parse(utilisateur.Ville);
            cboVille.Selectionner(ville);
        }

        internal void Nettoyer()
        {
            UtilisateurCourant = null;
            txtPrenom.Clear();
            txtNom.Clear();
            txtNumeroEmploye.Clear();
            txtIdentifiant.Clear();
            cboVille.Selectionner(Villes.Aucune);
        }
    }
}
