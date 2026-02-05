using System;
using System.Data;

namespace Rrq.Securite.ParametreChiffrement
{
    public interface TsIAccesseur
    {
        void InscrireFichierChiffre(string strNomFichier, string strNomTypeDataSet, string strContenuFichier, bool blnChiffrementDechiffrementPermis);

        string  ObtenirFichierChiffre(string strNomFichier, string strNomTypeDataSet, bool blnChiffrementDechiffrementPermis);
    }

    [Flags]
    public enum TsRessourceParamChiffr
    {
        //Fichier sur le réseau
        FICHIER,
        //Appelle un composant d'intégration de la Logique d'affaire.
        CIWCF,
        //Appelle un composant de service.
        CS
    }
}