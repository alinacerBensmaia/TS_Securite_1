using Rq.Infrastructure.Securite.SignatureDigital.Internes.Enumerations;
using System;
using System.Windows.Forms;

namespace Rq.Infrastructure.Securite.SignatureDigital.Internes.Extensions
{
    internal static class ControlExtensions
    {
        public static IDisposable CurseurWait(this Control source)
        {
            return new Curseur(source, Cursors.WaitCursor);
        }

        public static void PopulerLois(this ComboBox source)
        {
            source.Items.Clear();
            source.Items.Add(Lois.Toutes);
            foreach (var item in Lois.All)
                source.Items.Add(item);
            source.SelectedIndex = 0;
        }
        public static void PopulerEnvironnements(this ComboBox source)
        {
            source.Items.Clear();
            foreach (var item in Environnements.All)
            {
                source.Items.Add(item);
            }
            source.SelectedIndex = 0;
        }
        public static void PopulerSignatureTypes(this ComboBox source)
        {
            source.Items.Clear();
            foreach (var item in SignatureTypes.All)
            {
                source.Items.Add(item);
            }
            source.SelectedIndex = 0;
        }
        public static void PopulerVilles(this ComboBox source)
        {
            source.Items.Clear();
            source.Items.Add(Villes.Aucune);
            foreach (var item in Villes.All)
                source.Items.Add(item);
            source.SelectedIndex = 0;
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
