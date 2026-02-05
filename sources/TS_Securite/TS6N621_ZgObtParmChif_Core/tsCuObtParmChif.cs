using Rrq.InfrastructureCommune.Parametres;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace TS6N621_ZgObtParmChif
{
    /// --------------------------------------------------------------------------------
    /// Project:	TS6N621_ZgObtParmChif
    /// Class:	tsCuObtParmChif
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
    /// 2005-04-06	t209376		Création initiale
    /// 2005-07-15	t209428		Ajustement pour le SQAG
    /// 
    /// </pre></para>
    /// </remarks>
    /// --------------------------------------------------------------------------------
    public class tsCuObtParmChif : TsCuObtnrParmt
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
        /// 	Variable de type string dans laquelle est retournée le code de chiffrement 
        ///     sélectionné.Cette valeur est nécessaire lors de la demande d'obtention du 
        ///     code pour le déchiffrement.
        /// 	Reference Type: <see cref="String" />	(System.String)
        /// </param>
        /// <param name = "bytCle" >
        /// 	Variable de type tableau de bytes dans laquelle est retournée la clé de 
        ///     chiffrement sélectionnée. 
        /// 	Reference Type: <see cref = "Byte" /> (System.Byte)
        /// </param>
        /// <param name = "bytVecteur" >
        ///  Variable de type tableau de bytes dans laquelle est retournée le vecteur 
        ///     de chiffrement sélectionné. 
        /// 	Reference Type: <see cref = "Byte" /> (System.Byte)
        /// </param>
        /// 
        /// 	Cette exception est lancée si...
        /// </exception>
        /// <returns><see cref = "Boolean" /> (System.Boolean) </ returns >
        /// <remarks><para><pre>
        /// Historique des modifications: 
        /// 
        /// --------------------------------------------------------------------------------
        /// Date		Nom			Description
        /// 
        /// --------------------------------------------------------------------------------
        /// 2005-02-21	t209376 Création initiale
        /// 
        /// </pre></para>
        /// </remarks>
        /// --------------------------------------------------------------------------------
        public bool ObtenirParamChiffrement(objTypeCle objType, ref string strCode, ref byte[] bytCle, ref byte[] bytVecteur)
        {
            try
            {
                var objDsCleVecteurLoc = new tsDsObtParmChif();
                Stream objStrm;
                DataRow[] tabLignes;
                var objRnd = new Random(Environment.TickCount);
                int intRnd;
                string strCheminFichierChiffrement;
                string strChaine;
                var strType = string.Empty;

                strCheminFichierChiffrement = XuCuConfiguration.ValeurSysteme("TS6", @"TS6\TS6N621\CheminFichierChiffrement");

                try
                {
                    ChiffrementDechiffrementPermis = true;
                    objStrm = DechiffrerFichier(strCheminFichierChiffrement);
                    objDsCleVecteurLoc.ReadXml(objStrm);
                    ChiffrementDechiffrementPermis = false;
                    objStrm.Close();
                }
                catch (FileNotFoundException)
                {
                    throw new Exception("Vérifier l'emplacement et la présence du fichier de clés/vecteurs.");
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

                var liste = new List<DataRow>();
                liste.AddRange(objDsCleVecteurLoc.CleVecteur.Select());

                if (string.IsNullOrEmpty(strCode))
                {
                    // Si on ne nous a pas fourni de code, on va chercher de façon aléatoire dans toutes les clés numériques ( génériques )
                    tabLignes = liste.FindAll(x => x["Actif"].ToString().Equals("True") &&
                                                      x["Type"].ToString().Equals(strType) &&
                                                      Regex.IsMatch(x["Code"].ToString(), "^[0-9]*$")).ToArray();
                }
                else
                {
                    string code = strCode;


                    // Si on nous a fourni un code, on recherche cette donnée en particulier
                    tabLignes = liste.FindAll(x => x["Actif"].ToString().Equals("True") &&
                                                      x["Type"].ToString().Equals(strType) &&
                                                      String.Equals(x["Code"].ToString(), code, StringComparison.CurrentCultureIgnoreCase)).ToArray();
                }


                if (tabLignes.Length > 0)
                {
                    intRnd = objRnd.Next(0, tabLignes.Length);

                    strCode = tabLignes[intRnd]["Code"].ToString();
                    strChaine = tabLignes[intRnd]["Cle"].ToString();
                    bytCle = StringToByteArray(strChaine.Split(' '));
                    strChaine = tabLignes[intRnd]["Vecteur"].ToString();
                    bytVecteur = StringToByteArray(strChaine.Split(' '));
                    return true;
                }
                else
                {
                    throw new Exception("Il n'y a aucun code de disponible pour le chiffrement. Vérifier également l'emplacement et la présence du fichier de clés/vecteurs.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// --------------------------------------------------------------------------------
        /// Class.Method:	tsCuObtParmChif.ObtenirParamDechiffrement
        /// <summary>
        /// 
        /// </summary>
        /// <param name = "strCode" >
        ///  Variable de type string indiquant le code de déchiffrement désiré.
        /// 	Value Type: <see cref = "Int32" /> (System.Int32)
        /// </ param >
        /// <param name="bytCle">
        /// 	Variable de type tableau de bytes dans laquelle est retournée la clé de déchiffrement désirée.
        /// 	Reference Type: <see cref = "Byte" /> (System.Byte)
        /// </ param >
        /// < param name="bytVecteur">
        /// 	Variable de type tableau de bytes dans laquelle est retournée le vecteur de déchiffrement désiré.
        /// 	Reference Type: <see cref = "Byte" /> (System.Byte)
        /// </ param >
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
        /// 2005-02-21	t209376 Création initiale
        /// 
        /// </pre></para>
        /// </remarks>
        /// --------------------------------------------------------------------------------
        public bool ObtenirParamDechiffrement(objTypeCle objType, string strCode, ref byte[] bytCle, ref byte[] bytVecteur)
        {
            try
            {
                var objDsCleVecteurLoc = new tsDsObtParmChif();
                Stream objStrm;
                DataRow[] tabLignes;
                string strCheminFichierChiffrement;
                string strChaine;
                var strType = string.Empty;

                strCheminFichierChiffrement = XuCuConfiguration.ValeurSysteme("TS6", @"TS6\TS6N621\CheminFichierChiffrement");

                try
                {
                    ChiffrementDechiffrementPermis = true;
                    objStrm = DechiffrerFichier(strCheminFichierChiffrement);
                    objDsCleVecteurLoc.ReadXml(objStrm);
                    ChiffrementDechiffrementPermis = false;
                    objStrm.Close();
                }
                catch (FileNotFoundException)
                {
                    throw new Exception("Vérifier l'emplacement et la présence du fichier de clés/vecteurs.");
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

                tabLignes = objDsCleVecteurLoc.CleVecteur.Select("Code='" + strCode + "' and Type='" + strType + "'");

                if (tabLignes.Length > 0)
                {
                    strChaine = tabLignes[0]["Cle"].ToString();
                    bytCle = StringToByteArray(strChaine.Split(' '));
                    strChaine = tabLignes[0]["Vecteur"].ToString();
                    bytVecteur = StringToByteArray(strChaine.Split(' '));
                    return true;
                }
                else
                {
                    throw new Exception("Il n'y a aucun code de disponible pour le chiffrement. Vérifier également l'emplacement et la présence du fichier de clés/vecteurs.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
