using System;
using System.Windows.Forms;

namespace RRQ.Infrastructure.Securite.SignatureDigital.Internes.Extensions
{
    internal static class ControlExtensions
    {
        public static IDisposable CurseurWait(this Control source)
        {
            return new Curseur(source, Cursors.WaitCursor);
        }

        
        public static void Selectionner<T>(this ComboBox source, T valeur)
        {
            source.SelectedItem = valeur;
        }

        public static T Selection<T>(this ComboBox source)
        {
            return (T)source.SelectedItem;
        }
    }
}