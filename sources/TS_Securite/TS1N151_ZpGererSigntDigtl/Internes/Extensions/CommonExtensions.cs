using System;

namespace Rq.Infrastructure.Securite.SignatureDigital.Internes.Extensions
{
    internal static class CommonExtensions
    {
        public static bool Est(this string source, string valeur)
        {
            return string.Equals(source, valeur, StringComparison.InvariantCultureIgnoreCase);
        }

        public static string AtRQ(this string source)
        {
            return string.Concat(source, "@rq.retraitequebec.gouv.qc.ca");
        }
    }
}
