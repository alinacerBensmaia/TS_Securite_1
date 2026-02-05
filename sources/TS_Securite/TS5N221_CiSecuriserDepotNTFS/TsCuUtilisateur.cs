
using Rrq.Securite;
using System;

///-----------------------------------------------------------------------------
/// Project		: 
/// Class		: TsCuUtilisateur
///
///-----------------------------------------------------------------------------
/// <summary>
/// 
/// </summary>
/// <remarks></remarks>
///-----------------------------------------------------------------------------
///

public class TsCuUtilisateur 
{
   

    #region "--- Constantes ---"

    #endregion

    #region "--- Variables ---"

    private TsCuAccesAD mActiveDirectory;
    private string mCodeUtilisateur;

    #endregion

    #region "--- Constructeurs ---"
    public TsCuUtilisateur()
    {
        this.ActiveDirectory = new TsCuAccesAD();
       
    }

    #endregion


    #region "--- Publiques ---"

    #region "--- Propriété ---"

    //public string CodeUtilisateur { get => mCodeUtilisateur; set => mCodeUtilisateur = value; }
    public string CodeUtilisateur
    {
        get
        {
            return mCodeUtilisateur;
        }

        set
        {
            mCodeUtilisateur = value;
        }
    }
    #endregion

    #region "--- Méthodes ---"
    public bool EstMembreDe(String pGrougeSecurite)
    {
        if (!ActiveDirectory.GroupeExiste(pGrougeSecurite))
        {
            return false;
        }
        else
        {
            return ActiveDirectory.EstMembreGroupe2(pGrougeSecurite, new System.Security.Principal.WindowsIdentity(CodeUtilisateur));
        }

    }
    #endregion
    #endregion

    #region "--- Privées ---"
    #region "--- Propriété ---"
    //private TsCuAccesAD ActiveDirectory { get => mActiveDirectory; set => mActiveDirectory = value; }
    private TsCuAccesAD ActiveDirectory
    {
        get
        {
            return mActiveDirectory;
        }

        set
        {
            mActiveDirectory = value;
        }
    }

    #endregion
    #endregion

}

    
