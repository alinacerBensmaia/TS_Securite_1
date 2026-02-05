
using System.Net;
using System.Security;
using Rrq.Securite;
///-----------------------------------------------------------------------------
/// Project		: TS5N221_CiSecuriserDepotNTFS
/// Class		: TsCuGererSecurite
///
///-----------------------------------------------------------------------------
/// <summary>
/// Cette classe permet de gérer les info de connexion lors de l'emprunt d'identité
/// </summary>
/// <remarks></remarks>
///-----------------------------------------------------------------------------
///

public class TsCuGererSecurite 
{

    #region "--- Variables ---"

    private SecureString mMotDePasseSecurString = new SecureString();

    #endregion

    #region "--- Propriétés ---"
    public string CleSymbolique { get; set; }

    public string NomUsager { get; set; }

    public string MotDePasse { get { return new NetworkCredential(string.Empty, mMotDePasseSecurString).Password; } }


    #endregion

    #region "--- Constructeurs ---"
    public TsCuGererSecurite(string pCleSymbolique)
    {
        string mdp = null;
        string codeUsager = null;
        CleSymbolique = pCleSymbolique;
       
        //optenir mot de passe a partir de la clé symbolique
        tsCuObtCdAccGen obtCdAccGen = new tsCuObtCdAccGen();
        obtCdAccGen.ObtenirCodeAccesMotDePasse(CleSymbolique, "Appliquer sécurité NTFS", ref codeUsager, ref mdp);

        NomUsager = codeUsager;

        //obtenir le mot de passe encrypté?
        ObtenirMdpCleSymbolique(mdp);
    }
    #endregion

    #region "--- Méthodes privées ---"
    /// <summary>
    /// Cette méthode permet de stocker le mot de passe dans un SecurString
    /// </summary>
    /// <param name="pMotDePasseRecupere"></param>
    private void ObtenirMdpCleSymbolique(string pMotDePasseRecupere)
    {
        string chars = pMotDePasseRecupere;

        foreach (var caractere in chars)
        {
            mMotDePasseSecurString.AppendChar(caractere);
        }
    }
    #endregion


}
