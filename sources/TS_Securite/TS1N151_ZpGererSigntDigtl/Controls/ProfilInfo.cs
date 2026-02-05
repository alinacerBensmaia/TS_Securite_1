using System;
using System.ComponentModel;
using System.Windows.Forms;
using Rq.Infrastructure.Securite.SignatureDigital.Internes.Profils;

namespace Rq.Infrastructure.Securite.SignatureDigital.Controls
{
    public partial class ProfilInfo : UserControl
    {
        private Profil _profil;

        public ProfilInfo()
        {
            InitializeComponent();
        }

        [Browsable(true)]
        [DefaultValue("<undefined>")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get { return lblProfil.Text; }
            set { lblProfil.Text = value; }
        }

        [Browsable(true)]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string MessageCreation { get; set; }

        [Browsable(true)]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string MessageSuppression { get; set; }

        internal void Nettoyer()
        {
            _profil = null;
            txtProfil.Clear();
            btnCreer.Enabled = false;
            btnSupprimer.Enabled = false;
        }

        internal void Afficher(Profil profil)
        {
            Nettoyer();
            _profil = profil;
            rafraichir();            
        }

        private void rafraichir()
        {
            txtProfil.Text = _profil.Chemin;
            if (!string.IsNullOrWhiteSpace(_profil.Chemin))
            {
                btnCreer.Enabled = !_profil.Existe;
                btnSupprimer.Enabled = _profil.Existe;
            }
        }

        private void btnCreer_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(MessageCreation, "Attention", MessageBoxButtons.OKCancel) == DialogResult.OK)
                _profil.Creer();
            rafraichir();
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(MessageSuppression, "Attention", MessageBoxButtons.OKCancel) == DialogResult.OK)
                _profil.Supprimer();
            rafraichir();
        }
    }
}
