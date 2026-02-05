using System;
using System.Windows.Forms;

using RRQ.Infrastructure.Securite.SignatureDigital.Internes.Extensions;

namespace RRQ.Infrastructure.Securite
{
    public partial class FenetrePrincipal : Form
    {
        public FenetrePrincipal()
        {
            InitializeComponent();
        }

        private void afficherExceptionInnatendu(Exception ex)
        {
            MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnChargerSrce_Click(object sender, EventArgs e)
        {
            try
            {
                using (this.CurseurWait())
                {
                    


                }
            }
            catch (Exception ex)
            {
                afficherExceptionInnatendu(ex);
            }
        }
    }
}
