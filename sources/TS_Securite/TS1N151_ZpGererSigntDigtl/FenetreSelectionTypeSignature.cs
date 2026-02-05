using System.Windows.Forms;
using Rq.Infrastructure.Securite.SignatureDigital.Internes.Enumerations;
using Rq.Infrastructure.Securite.SignatureDigital.Internes.Extensions;

namespace Rq.Infrastructure.Securite.SignatureDigital
{
    public partial class FenetreSelectionTypeSignature : Form
    {
        public FenetreSelectionTypeSignature()
        {
            InitializeComponent();
            cboTypes.PopulerSignatureTypes();
        }

        internal SignatureTypes Selection
        {
            get { return cboTypes.Selection<SignatureTypes>(); }
        }
    }
}
