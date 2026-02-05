using System;
using System.Data;
using System.ServiceModel;
using Rrq.InfrastructureCommune.ScenarioTransactionnel;
using Rrq.InfrastructureCommune.UtilitairesCommuns;
using Rrq.Securite.ParametreChiffrement;

namespace TS6N627_CsGererFichierChiffr
{
    ///-----------------------------------------------------------------------------
    /// Project		: TS6N627_CsGererFichierChiffr
    /// Class		: TsCaCsGererFichierChiffr
    ///
    ///-----------------------------------------------------------------------------
    /// <summary>
    /// Classe d'affaire.
    /// </summary>
    ///-----------------------------------------------------------------------------
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerCall, AddressFilterMode = AddressFilterMode.Any)]
    public class TsCaCsGererFichierChiffr : Rrq.InfrastructureCommune.ScenarioTransactionnel.ClassesBaseIntegration.XuCaBaseComposantV2
    {
        [OperationBehavior(TransactionScopeRequired = false, ReleaseInstanceMode = ReleaseInstanceMode.BeforeAndAfterCall)]
        void InscrireFichierChiffre(ref string ChaineContexte, string strNomFichier, string strNomTypeDataset, string strContenuFichier)
        {
            try
            {
                XuCuChargementAssembly.CreerHandlerAssemblyResolve();
                TsCuGererFichrParamChiffr compBase = new TsCuGererFichrParamChiffr();
                compBase.ChiffrerFichier(strNomFichier, strNomTypeDataset, strContenuFichier);
            }
            catch (XuExcEErrFonctionnelle ex)
            {
                CreerRetourErrFonctionnelle(ref ChaineContexte, ex);
            }
            catch (XuExcEErrValidation ex)
            {
                CreerRetourErrValidation(ref ChaineContexte, ex);
            }
        }

        [OperationBehavior(TransactionScopeRequired = false, ReleaseInstanceMode = ReleaseInstanceMode.BeforeAndAfterCall)]
        string ObtenirFichierChiffre(ref string ChaineContexte, string strNomFichier, string strNomTypeDataset)
        {
            try
            {
                XuCuChargementAssembly.CreerHandlerAssemblyResolve();
                TsCuGererFichrParamChiffr compBase = new TsCuGererFichrParamChiffr();
                return compBase.DechiffrerFichier(strNomFichier, strNomTypeDataset);
            }
            catch (XuExcEErrFonctionnelle ex)
            {
                CreerRetourErrFonctionnelle(ref ChaineContexte, ex);
            }
            catch (XuExcEErrValidation ex)
            {
                CreerRetourErrValidation(ref ChaineContexte, ex);
            }

            return null;
        }
 
    }
}