
using Rrq.InfrastructureCommune.UtilitairesCommuns;
using System;


namespace TS5N211_CbFonctionCommune
{
    internal class TsCuGestnEvent
    {

        private const string NOM_JOURN_EVENM_APPLICATION_RRQ = "ApplicationRRQ";
        private const string SOURCE_JOURN_EVENM_APPLICATION_RRQ = "ApplicationCS";
        private const int TAILLE_MAX_MESSAGE = 30000;

        internal static void EcrireEventLog(XuGeTypeEvenement type, string message)
        {
            if (message.Length > TAILLE_MAX_MESSAGE)
            {
                int nombrePage = System.Convert.ToInt32(Math.Round(message.Length / (double)TAILLE_MAX_MESSAGE, System.MidpointRounding.AwayFromZero));

                for (int page = 1; page <= nombrePage; page++)
                {
                    string messagePage;
                    if (message.Length > TAILLE_MAX_MESSAGE)
                    {
                        messagePage = message.Substring(0, TAILLE_MAX_MESSAGE);
                        message = message.Substring(TAILLE_MAX_MESSAGE);
                    }
                    else
                        messagePage = message;

                    _EcrireEventLog(type, string.Concat(page, "/", nombrePage, " ", messagePage));
                }
            }
            else
                _EcrireEventLog(type, message);
        }

        private static void _EcrireEventLog(XuGeTypeEvenement type, string message)
        {
            XuCuGestionEvent.AjouterEvenmSpecifique(XuGeJournalEvenement.XuGeJeApplicationCS, type, 0, message);
        }
    }
}
