using System;
using System.Data;
using System.Runtime.CompilerServices;
using TS6N626_IGererFichierChiffr;

namespace Rrq.Securite.ParametreChiffrement
{
    public class TsCuAccesseurCI : TsIAccesseur
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void InscrireFichierChiffre(string strNomFichier, string strNomTypeDataSet, string strContenuFichier, bool blnChiffrementDechiffrementPermis)
        {

            if (blnChiffrementDechiffrementPermis)
            {
                TsCuProxyGererFichierChiffr objFichier = new TsCuProxyGererFichierChiffr();
                objFichier.InscrireFichierChiffre(strNomFichier, strNomTypeDataSet, strContenuFichier);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string ObtenirFichierChiffre(string strNomFichier, string strNomTypeDataSet, bool blnChiffrementDechiffrementPermis)
        {
            if (blnChiffrementDechiffrementPermis)
            {
                TsCuProxyGererFichierChiffr objFichier = new TsCuProxyGererFichierChiffr();
                return objFichier.ObtenirFichierChiffre(strNomFichier, strNomTypeDataSet);
            }
            else
            {
                return null;
            }
        }

    }
}
