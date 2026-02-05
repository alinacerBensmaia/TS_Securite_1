using System;
using System.Windows.Forms;

namespace Rq.Infrastructure.Securite.SignatureDigital.Internes
{
    internal class Curseur : IDisposable
    {
        private readonly Control _ctrl;
        private readonly Cursor _precedent;

        public Curseur(Control ctrl, Cursor nouveau)
        {
            this._ctrl = ctrl;
            this._precedent = ctrl.Cursor;
            ctrl.Cursor = nouveau;
        }

        public void Dispose()
        {
            _ctrl.Cursor = _precedent;
        }
    }
}
