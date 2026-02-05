using System;
using System.ComponentModel;
using System.Data;
using System.ServiceModel;
using Rrq.InfrastructureCommune.ScenarioTransactionnel.ClassesBaseIntegration;
using Rrq.Securite.ParametreChiffrement;

namespace TS6N627_IGererFichierChiffr
{
    #region TsIGererFichierChiffr
    /// <summary>
    ///
    /// </summary>

    [XuCuRRQComposantIntegration()]
    [ServiceContract(Namespace = "TS6N627_IGererFichierChiffr")]
    public interface TsIGererFichierChiffr
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="ChaineContexte">La chaîne de contexte nécessaire pour effectuer un appel.</param>
        /// <param name="strNomFichier">Chemin du fichier</param>
        /// <param name="strNomTypeDataset">Type d'objet DataSet à retourner</param>
        /// <param name="strContenuFichier">Contenu à mettre dans le fichier</param>
        [RRQTypeTransaction(XuBiModeTransactionnel.XuBiMtAucun)]
        [TransactionFlow(TransactionFlowOption.NotAllowed)]
        [OperationContract(Name = "InscrireFichierChiffre1")]
        void InscrireFichierChiffre(ref string ChaineContexte, string strNomFichier, string strNomTypeDataset, string strContenuFichier);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ChaineContexte">La chaîne de contexte nécessaire pour effectuer un appel.</param>
        /// <param name="strNomFichier">Chemin du fichier</param>
        /// <param name="strNomTypeDataset">Type d'objet DataSet à retourner</param>
        [RRQTypeTransaction(XuBiModeTransactionnel.XuBiMtAucun)]
        [TransactionFlow(TransactionFlowOption.NotAllowed)]
        [OperationContract(Name = "ObtenirFichierChiffre2")]
        string ObtenirFichierChiffre(ref string ChaineContexte, string strNomFichier, string strNomTypeDataset);

    }
    #endregion



    #region XuCuInterface
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class XuCuInterface : XuCuBaseInterfaceV2
    {
        protected override string NomAssemblyComposant
        {
            get { return "TS6N627_CsGererFichierChiffr"; }
        }

        protected override string NomClasseComposant
        {
            get { return "TS6N627_CsGererFichierChiffr.TsCaCsGererFichierChiffr"; }
        }
    }
    #endregion

    #region Proxy
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class XuCuProxy : XuCuBaseProxy, TsIGererFichierChiffr
    {
        public override string NomAssembly
        {
            get
            {
                XuCuInterface interfCompi = new XuCuInterface();
                return interfCompi.ObtenirNomAssembly();
            }
        }

        void TsIGererFichierChiffr.InscrireFichierChiffre(ref string ChaineContexte, string strNomFichier, string nomTypeDataset, string contenuFichier)
        {
            using (XuCuProxyHelper<TsIGererFichierChiffr> p = new XuCuProxyHelper<TsIGererFichierChiffr>(ref ChaineContexte, Environnement))
            {
                p.Proxy.InscrireFichierChiffre(ref ChaineContexte, strNomFichier, nomTypeDataset, contenuFichier);
            }
        }

        string TsIGererFichierChiffr.ObtenirFichierChiffre(ref string ChaineContexte, string strNomFichier, string nomTypeDataset)
        {
            using (XuCuProxyHelper<TsIGererFichierChiffr> p = new XuCuProxyHelper<TsIGererFichierChiffr>(ref ChaineContexte, Environnement))
            {
                return p.Proxy.ObtenirFichierChiffre(ref ChaineContexte, strNomFichier, nomTypeDataset);
            }
        }

    }
    #endregion
}