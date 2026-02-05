using Rq.Infrastructure.Securite.SignatureDigital.Internes.Signatures;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Rq.Infrastructure.Securite.SignatureDigital
{
    public partial class FenetreChoixSignature : Form
    {
        public FenetreChoixSignature()
        {
            InitializeComponent();
        }

        internal void Populer(List<Signature> signatures)
        {
            foreach (var signature in signatures)
            {
                var item = new ListViewItem();
                item.Tag = signature;
                item.Text = signature.Fichier;
                item.SubItems.Add(signature.Emplacement);

                lstSignatures.Items.Add(item);
            }
            lstSignatures.SelectedIndices.Clear();
        }

        internal Signature Selection
        {
            get { return (Signature)lstSignatures.SelectedItems[0].Tag; }
        }

        private void lstSignatures_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            bool enable = (lstSignatures.SelectedIndices.Count > 0);
            btnVisualiser.Enabled = enable;
            btnSelectionner.Enabled = enable;
        }

        private void btnVisualiser_Click(object sender, System.EventArgs e)
        {
            new Signatures().Visualiser(Selection);
        }
    }
}
