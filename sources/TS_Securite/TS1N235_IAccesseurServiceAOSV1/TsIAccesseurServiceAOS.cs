using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.ServiceModel;
using Rrq.InfrastructureCommune.ScenarioTransactionnel;
using Rrq.InfrastructureCommune.ScenarioTransactionnel.ClassesBaseAOS;

namespace Rrq.TS.AccesseurServiceAOSV1
{
	#region TsIAccesseurServiceAOS

	/// <summary>
	/// Service AOS pour les récupération de code usager et  mot de passe des clés symboliques
	/// </summary>

	[XuCuRRQComposantService()]
	[ServiceContract(Namespace = "Rrq.TS.AccesseurServiceAOSV1")]
	public interface TsIAccesseurServiceAOS
	{
		/// <summary>
		/// Permet d'obtenir le code usager et mot de passe associés à une clé symbolique
		/// </summary>
		/// <param name="ChaineContexte">La chaîne de contexte nécessaire pour effectuer un appel.</param>
		/// <param name="strCle">Nom de la clé symbolique</param>
		/// <param name="strRaison">Raison de la demande de code usager et mot de passe</param>
		/// <param name="strCompte">Compte associé à la clé symbolique</param>
		/// <param name="strMDP">mot de passe associé à la clé symbolique</param>
		[RRQTypeTransaction(XuCaBaseAffaire.XuBsTransactionService.XuBsMtsSupporte)]
		[TransactionFlow(TransactionFlowOption.Allowed)]
		[OperationContract(Name="ObtenirCodeAccesMotDePasse1")]
		void ObtenirCodeAccesMotDePasse(ref string ChaineContexte, string strCle, string strRaison, ref string strCompte, ref string strMDP);
	}

	#endregion

}


#region XuCuInterface
namespace Rrq.TS.AccesseurServiceAOSV1
{
	[EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	public class XuCuInterface : XuCuBaseInterface
	{
		protected override string NomAssemblyComposant 
		{
			get
			{
				return "TS1N235_CsAccesseurServiceAOS";   //Nom du DLL du composant d'intégration
			}
		}

		protected override string NomClasseComposant 
		{
			get
			{
				return null; //'Cette propriété n'est plus utilisée.Elle existe encore par souci de rétro - compatibilité seulement.
			} 
		}
		
		protected override string get_NomClasseComposant(Type typeInterface)
		{
			if (typeInterface == typeof(TsIAccesseurServiceAOS))
			{
				return "TS1N235_CsAccesseurServiceAOS.TsCaAccesseurServiceAOS";
			}
			else
			{
				throw new XuExcEErrFatale("Le type " + typeInterface.FullName + " n'a pas été géré par la méthode NomClasseComposant(ByVal typeInterface As System.Type).");
			}
		}
	}
}
#endregion
