using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;

namespace TS6N621_ZgObtParmChif
{
    public abstract class TsCuObtnrParmt
    {
        private readonly byte[] Cle = { 47, 139, 1, 46, 248, 163, 248, 134, 48, 93, 99, 57, 241, 160, 148, 141, 17, 186, 151, 23, 58, 110, 141, 152, 249, 253, 173, 95, 90, 24, 106, 161 };
        private readonly byte[] Vecteur = { 38, 55, 72, 244, 225, 125, 22, 45, 231, 213, 251, 49, 129, 90, 241, 178 };

        private bool blnChiffrementDechiffrementPermis;

        public enum objTypeCle
        {
            Interne = 0,
            SQAG = 1,
            Externe = 2
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ChiffrementDechiffrementPermis
        {
            get
            {
                return blnChiffrementDechiffrementPermis;
            }
            set
            {
                blnChiffrementDechiffrementPermis = value;
            }
        }

        ///--------------------------------------------------------------------------------
        /// Class.Method:	tsCuObtParmChif.ChiffrerFichier
        /// <summary>
        /// 
        /// </summary>
        /// <param name = "strNomFichier" >
        ///  Variable de type string indiquant le nom du fichier à chiffrer.On doit mettre le chemin complet et non seulement le nom du fichier. 
        /// 	Value Type: <see cref = "String" /> (System.String)
        /// </ param >
        ///  Cette exception est lancée si...
        /// </exception>
        /// <returns><see cref = "Security.Cryptography.CryptoStream" /> (System.Security.Cryptography.CryptoStream) </ returns >
        /// < remarks >< para >< pre >
        /// Historique des modifications: 
        /// 
        /// --------------------------------------------------------------------------------
        /// Date Nom         Description
        /// 
        /// --------------------------------------------------------------------------------
        /// 2005-02-22	t209376 Création initiale
        /// 
        /// </pre></para>
        /// </remarks>
        /// --------------------------------------------------------------------------------
        public CryptoStream ChiffrerFichier(string strNomFichier)
        {
            var objChiffreur = new FileStream(strNomFichier, FileMode.Create);
            var objRijndael = new RijndaelManaged();

            if (blnChiffrementDechiffrementPermis)
            {
                ICryptoTransform objChiffre = objRijndael.CreateEncryptor(Cle, Vecteur);
                var objChifStrm = new CryptoStream(objChiffreur, objChiffre, CryptoStreamMode.Write);

                return objChifStrm;
            }
            return null;
        }

        /// --------------------------------------------------------------------------------
        /// Class.Method:	tsCuObtParmChif.DechiffrerFichier
        /// <summary>
        /// 
        /// </summary>
        /// <param name = "strNomFichier" >
        ///  Variable de type string indiquant le nom du fichier à déchiffrer.On doit mettre le chemin complet et non seulement le nom du fichier. 
        /// 	Value Type: <see cref = "String" /> (System.String)
        /// </ param >
        /// 
        /// 	Cette exception est lancée si...
        /// </exception>
        /// <returns><see cref = "Security.Cryptography.CryptoStream" /> (System.Security.Cryptography.CryptoStream) </ returns >
        /// < remarks >< para >< pre >
        /// Historique des modifications: 
        /// 
        /// --------------------------------------------------------------------------------
        /// Date Nom         Description
        /// 
        /// --------------------------------------------------------------------------------
        /// 2005-02-22	t209376 Création initiale
        /// 
        /// </pre></para>
        /// </remarks>
        /// --------------------------------------------------------------------------------
        public CryptoStream DechiffrerFichier(string strNomFichier)
        {
            var objDechiffreur = new FileStream(strNomFichier, FileMode.Open, FileAccess.Read, FileShare.Read);
            var objRijndael = new RijndaelManaged();

            if (blnChiffrementDechiffrementPermis)
            {
                ICryptoTransform objDechiffre = objRijndael.CreateDecryptor(Cle, Vecteur);
                var objDechifStrm = new CryptoStream(objDechiffreur, objDechiffre, CryptoStreamMode.Read);

                return objDechifStrm;
            }
            return null;
        }

        /// --------------------------------------------------------------------------------
        /// Class.Method:	tsCuObtParmChif.StringToByteArray
        /// <summary>
        /// 
        /// </summary>
        /// <param name = "tabChaine" >
        ///  Tableau de string pour qu'ils soit convertis en tableau de byte. 
        /// 	Value Type: <see cref="String" />	(System.String)
        /// </param>
        /// 
        /// 	Cette exception est lancée si...
        /// </exception>
        /// <returns><see cref = "Byte" /> (System.Byte) </ returns >
        /// < remarks >< para >< pre >
        /// Historique des modifications: 
        /// 
        /// --------------------------------------------------------------------------------
        /// Date		Nom			Description
        /// 
        /// --------------------------------------------------------------------------------
        /// 2005-02-22	t209376 Création initiale
        /// 
        /// </pre></para>
        /// </remarks>
        /// --------------------------------------------------------------------------------
        internal byte[] StringToByteArray(string[] tabChaine)
        {
            var bytBuffer = new List<byte>();
            foreach (var item in tabChaine)
            {
                bytBuffer.Add(Convert.ToByte(item));
            }
            return bytBuffer.ToArray();
        }
    }
}
