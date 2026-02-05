using System.Diagnostics;

namespace Rq.Infrastructure.Securite.SignatureDigital.Internes.Commandes
{
    internal class Xcopy
    {
        private readonly string _source;
        private readonly string _cible;
        public Xcopy(string fichierSource, string fichierCible)
        {
            _source = fichierSource;
            _cible = fichierCible;
        }

        public void Executer()
        {
            var xCopy = string.Format("/C xcopy \"{0}\" \"{1}\"", _source, _cible);
            var cmd = "cmd.exe";

            var psi = new ProcessStartInfo(cmd, xCopy);
            psi.WindowStyle = ProcessWindowStyle.Maximized;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = false;

            using (var p = Process.Start(psi))
                p.WaitForExit();
        }
    }
}
