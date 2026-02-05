using System;
using System.Data;
using System.ServiceModel;
using Rrq.InfrastructureCommune.UtilitairesCommuns;
using Rrq.Securite.ParametreChiffrement;
using TS6N626_IGererFichierChiffr;

namespace TS6N626_CiGererFichierChiffr
{
    ///-----------------------------------------------------------------------------
    /// Project		: TS6N626_CiGererFichierChiffr
    /// Class		: TsCaGererFichierChiffr
    ///
    ///-----------------------------------------------------------------------------
    /// <summary>
    /// Classe d'affaire.
    /// </summary>
    ///-----------------------------------------------------------------------------
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerCall, AddressFilterMode = AddressFilterMode.Any)]
    public class TsCaGererFichierChiffr : TsIGererFichierChiffr
    {
        [OperationBehavior(TransactionScopeRequired = false, ReleaseInstanceMode = ReleaseInstanceMode.BeforeAndAfterCall)]
        public void InscrireFichierChiffre(string strNomFichier, string nomTypeDataset, string contenuFichier)
        {
            XuCuChargementAssembly.CreerHandlerAssemblyResolve();
            TsCuGererFichrParamChiffr compBase = new TsCuGererFichrParamChiffr();
            compBase.ChiffrerFichier(strNomFichier, nomTypeDataset, contenuFichier);
        }

        [OperationBehavior(TransactionScopeRequired = false, ReleaseInstanceMode = ReleaseInstanceMode.BeforeAndAfterCall)]
        public string ObtenirFichierChiffre(string strNomFichier, string nomTypeDataset)
        {
            XuCuChargementAssembly.CreerHandlerAssemblyResolve();
            TsCuGererFichrParamChiffr compBase = new TsCuGererFichrParamChiffr();
            return compBase.DechiffrerFichier(strNomFichier, nomTypeDataset);
        }

    }


}