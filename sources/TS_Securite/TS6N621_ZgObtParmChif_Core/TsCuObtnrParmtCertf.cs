using Rrq.InfrastructureCommune.Parametres;
using System;
using System.Data;
using System.IO;

namespace TS6N621_ZgObtParmChif
{
    /// --------------------------------------------------------------------------------
    /// Project:	TS6N621_ZgObtParmChif
    /// Class:	TsCuObtnrParmtCertf
    /// <summary>
    /// 
    /// </summary>
    /// <remarks><para><pre>
    /// Historique des modifications: 
    /// 
    /// --------------------------------------------------------------------------------
    /// Date		Nom			Description
    /// 
    /// --------------------------------------------------------------------------------
    /// 2005-07-15	T209428		Création initiale
    /// 
    /// </pre></para>
    /// </remarks>
    /// --------------------------------------------------------------------------------
    public class TsCuObtnrParmtCertf : TsCuObtnrParmt
    {
        /// --------------------------------------------------------------------------------
        /// Class.Method:	tsCuObtParmChif.ObtenirParamChiffrement
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objType">
        /// 	Variable de type objTypeCle dans laquelle on reçoit le type de clé demandée.
        /// 	Reference Type: <see cref = "objTypeCle" /> (objTypeCle)
        /// </ param >
        /// < param name="strCode">
        /// 	Variable de type string dans laquelle est reçu et retournée le code de  
        ///     chiffrement sélectionné.Cette valeur est nécessaire lors de la demande
        ///     d'obtention du code pour le déchiffrement. 
        /// 	Reference Type: <see cref = "String" /> (System.String)
        /// </ param >
        /// < param name= "strIdCertificat" >
        /// 	Variable de type string laquelle est retournée le Id du certificat de 
        ///     chiffrement sélectionné. 
        /// 	Reference Type: <see cref = "String" /> (System.String)
        /// </param>
        /// 
        /// 	Cette exception est lancée si...
        /// </exception>
        /// <returns><see cref = "Boolean" /> (System.Boolean) </ returns >
        /// < remarks >< para >< pre >
        /// Historique des modifications: 
        /// 
        /// --------------------------------------------------------------------------------
        /// Date Nom         Description
        ///
        /// --------------------------------------------------------------------------------
        /// 2005-07-15	T209428		Création initiale
        /// 
        /// </pre></para>
        /// </remarks>
        /// --------------------------------------------------------------------------------
        public bool ObtenirParamCertficat(objTypeCle objType, ref string strCode,
                                              ref string strIdCertificat, ref string strNomMagasin)
        {
            try
            {
                var objDsCertfLoc = new TsDsObtnrParmtCertf();
                Stream objStrm;
                DataRow[] tabLignes;
                var objRnd = new Random(Environment.TickCount);
                int intRnd;
                string strCheminFichierChiffrement;
                var strType = string.Empty;

                strCheminFichierChiffrement = XuCuConfiguration.ValeurSysteme("TS6", @"TS6\TS6N621\CheminFichierCertificat");

                try
                {
                    ChiffrementDechiffrementPermis = true;
                    objStrm = DechiffrerFichier(strCheminFichierChiffrement);
                    objDsCertfLoc.ReadXml(objStrm);
                    ChiffrementDechiffrementPermis = false;
                    objStrm.Close();
                }
                catch (FileNotFoundException)
                {
                    throw new Exception("Vérifier l'emplacement et la présence du fichier des ID de certificats.");
                }

                switch (objType)
                {
                    case TsCuObtnrParmt.objTypeCle.Interne:
                        strType = "Interne";
                        break;
                    case TsCuObtnrParmt.objTypeCle.SQAG:
                        strType = "SQAG";
                        break;
                    case TsCuObtnrParmt.objTypeCle.Externe:
                        strType = "Externe";
                        break;
                }

                if (strCode.Length > 0)
                {
                    // Si on nous a fourni un code, on recherche cette donnée en particulier
                    tabLignes = objDsCertfLoc.Certificat.Select("Actif=true and Type='" + strType + "' and Code='" + strCode + "'");
                }
                else
                {
                    // Si on ne nous a pas fourni un code, on recherche toutes les données actives 
                    // et pour un type de données
                    tabLignes = objDsCertfLoc.Certificat.Select("Actif=true and Type='" + strType + "'");
                }

                if (tabLignes.Length > 0)
                {
                    intRnd = objRnd.Next(0, tabLignes.Length);
                    strCode = tabLignes[intRnd]["Code"].ToString();
                    strIdCertificat = tabLignes[intRnd]["IdCertificat"].ToString();
                    strNomMagasin = tabLignes[intRnd]["NomMagasin"].ToString();
                    return true;
                }
                else
                {
                    throw new Exception("Il n'y a aucun code de disponible pour le chiffrement. Vérifier également l'emplacement et la présence du fichier des ID de certificats.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
