using System;
using System.ComponentModel;
using System.Data;
using System.ServiceModel;
using Rrq.InfrastructureCommune.ScenarioTransactionnel;

namespace TS6N626_IGererFichierChiffr
{
    #region TsIGererFichierChiffr
    /// <summary>
    ///
    /// </summary>

    [ServiceContract(Namespace = "TsIGererFichierChiffr")]
    public interface TsIGererFichierChiffr
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="strNomFichier"></param>
        /// <param name="contenuFichier"></param>
        [OperationContract(Name = "InscrireFichierChiffre1")]
        void InscrireFichierChiffre(string strNomFichier, string nomTypeDataset, string contenuFichier);

        /// <summary>
        ///
        /// </summary>
        /// <param name="strNomFichier"></param>
        /// <param name="NomTypeDataset"></param>
        [OperationContract(Name = "ObtenirFichierChiffre2")]
        string ObtenirFichierChiffre(string strNomFichier, string nomTypeDataset);

    }
    #endregion



    #region XuCuInterface
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class XuCuInterface : XuCuInterfaceInfra
    {
        protected override string NomAssemblyComposant
        {
            get { return "TS6N626_CiGererFichierChiffr"; }
        }

        protected override string GetNomClasseComposant(Type TypeInterface)
        {
            switch (true)
            {
                case object _ when TypeInterface == typeof(TsIGererFichierChiffr):
                    {
                        return "TS6N626_CiGererFichierChiffr.TsCaGererFichierChiffr";
                    }

                default:
                    {
                        throw new Exception("Le type \"" + TypeInterface.FullName + "\" n'a pas été géré par la méthode NomClasseComposant(ByVal typeInterface As System.Type).");
                    }
            }
        }
    }
    #endregion

}