using System;
using System.Data;

namespace Rrq.Securite.ParametreChiffrement
{
    public class TsCuAccesseurFichier : TsIAccesseur
    {

        public void InscrireFichierChiffre(string strNomFichier, string strNomTypeDataSet, string strContenuFichier, bool blnChiffrementDechiffrementPermis)
        {

            if (blnChiffrementDechiffrementPermis)
            {
                TsCuGererFichrParamChiffr objFichier = new TsCuGererFichrParamChiffr();
                objFichier.ChiffrerFichier(strNomFichier, strNomTypeDataSet, strContenuFichier);
            }
        }

        public string ObtenirFichierChiffre(string strNomFichier, string strNomTypeDataSet, bool blnChiffrementDechiffrementPermis)
        {
            if (blnChiffrementDechiffrementPermis)
            {
                TsCuGererFichrParamChiffr objFichier = new TsCuGererFichrParamChiffr();
                return objFichier.DechiffrerFichier(strNomFichier, strNomTypeDataSet);
            }
            else
            {
                return null;
            }
        }

    }
}
