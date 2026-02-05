using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.ServiceModel;
using Rrq.InfrastructureCommune.ScenarioTransactionnel;
using Rrq.InfrastructureCommune.ScenarioTransactionnel.ClassesBaseAOS;
using Rrq.Securite.CleSymbolique;
using Rrq.TS.AccesseurServiceAOSV1;

namespace TS1N235_CsAccesseurServiceAOS
{	
	///-----------------------------------------------------------------------------
	/// Project : TS1N235_CsAccesseurServiceAOS
	/// Class   : TsCaAccesseurServiceAOS
	///-----------------------------------------------------------------------------
	/// <summary>
	/// Classe d'affaire.
	/// </summary>
	///-----------------------------------------------------------------------------
	[ServiceBehavior(ConcurrencyMode=ConcurrencyMode.Single, InstanceContextMode=InstanceContextMode.PerCall, AddressFilterMode=AddressFilterMode.Any)]
	public class TsCaAccesseurServiceAOS : Rrq.InfrastructureCommune.ScenarioTransactionnel.ClassesBaseAOS.XuCaBaseAffaireV2, TsIAccesseurServiceAOS
	{
		[OperationBehavior(TransactionScopeRequired=false, ReleaseInstanceMode=ReleaseInstanceMode.BeforeAndAfterCall)]
		void TsIAccesseurServiceAOS.ObtenirCodeAccesMotDePasse(ref string ChaineContexte, string strCle, string strRaison, ref string strCompte, ref string strMDP) 
		{
			try
			{
				tsCuObtCdAccGen cleSymbolique = new tsCuObtCdAccGen();
				cleSymbolique.AssemblyCaller = System.Reflection.Assembly.GetAssembly(this.GetType());
				cleSymbolique.ObtenirCodeAccesMotDePasse(strCle, strRaison, ref strCompte, ref strMDP);

			}
			catch (XuExcEErrFonctionnelle ex)
			{
				base.CreerRetourErrFonctionnelle(ref ChaineContexte, ex);
			}
			catch (XuExcEErrValidation ex)
			{
				base.CreerRetourErrValidation(ref ChaineContexte, ex);
			}
		}

	}

}