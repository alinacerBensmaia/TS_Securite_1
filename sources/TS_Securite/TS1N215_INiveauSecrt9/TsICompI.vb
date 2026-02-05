Imports System.ComponentModel
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.ServiceModel
Imports Rrq.InfrastructureCommune.ScenarioTransactionnel
Imports Rrq.InfrastructureCommune.ScenarioTransactionnel.ClassesBaseIntegration

#Region "--- TsICompI ---"

''' <summary>
''' Interface d'intégration INiveauSecrt.
''' </summary>
<XuCuRRQComposantIntegration()> _
<ServiceContract(Namespace:="TS1N215_INiveauSecrt9.TsICompI")> _
Public Interface TsICompI

	''' <summary>
	''' Obtenir la liste de toues les cles
	''' </summary>
	''' <param name="ChaineContexte">La chaîne de contexte nécessaire pour effectuer un appel.</param>
	<RRQTypeTransaction(XuBiModeTransactionnel.XuBiMtAucun)> _
	<TransactionFlow(TransactionFlowOption.NotAllowed)> _
	<OperationContract(name:="ObtenirListeCle1")> _
	Function ObtenirListeCle(ByRef ChaineContexte as String) as Generic.IList(Of TS1N201_DtCdAccGenV1.TsDtCleSym)

	''' <summary>
	''' Obtenir une clé
	''' </summary>
	''' <param name="ChaineContexte">La chaîne de contexte nécessaire pour effectuer un appel.</param>
	''' <param name="IdCle">Code identifiant de la clé</param>
	<RRQTypeTransaction(XuBiModeTransactionnel.XuBiMtAucun)> _
	<TransactionFlow(TransactionFlowOption.NotAllowed)> _
	<OperationContract(name:="ObtenirCle2")> _
	Function ObtenirCle(ByRef ChaineContexte as String, ByVal IdCle as String) as TS1N201_DtCdAccGenV1.TsDtCleSym

	''' <summary>
	''' Obtenir les cles du parent
	''' </summary>
	''' <param name="ChaineContexte">La chaîne de contexte nécessaire pour effectuer un appel.</param>
	''' <param name="IdCleParent">Code identifiant de la clé parent</param>
	<RRQTypeTransaction(XuBiModeTransactionnel.XuBiMtAucun)> _
	<TransactionFlow(TransactionFlowOption.NotAllowed)> _
	<OperationContract(name:="ObtenirCles3")> _
	Function ObtenirCles(ByRef ChaineContexte as String, ByVal IdCleParent as String) as Generic.IList(Of TS1N201_DtCdAccGenV1.TsDtCleSym)

	''' <summary>
	''' Obtenir les cles recherche
	''' </summary>
	''' <param name="ChaineContexte">La chaîne de contexte nécessaire pour effectuer un appel.</param>
	''' <param name="CoTypCle">Code de type cle</param>
	''' <param name="CoTypEnv">Code de type environnement</param>
	''' <param name="GroupeAd">Profil</param>
	''' <param name="IdCle">Partie de l'identifiant de la clé à rechercher</param>
	''' <param name="UsagerAd">Code</param>
	<RRQTypeTransaction(XuBiModeTransactionnel.XuBiMtAucun)> _
	<TransactionFlow(TransactionFlowOption.NotAllowed)> _
	<OperationContract(name:="ObtenirCleRecherche4")> _
	Function ObtenirCleRecherche(ByRef ChaineContexte as String, ByVal CoTypCle as String, ByVal CoTypEnv as String, ByVal GroupeAd as String, ByVal IdCle as String, ByVal UsagerAd as String) as DataTable

	''' <summary>
	''' Obtenir l'état des indicateurs de création de compte TSS et AD
	''' </summary>
	''' <param name="ChaineContexte">La chaîne de contexte nécessaire pour effectuer un appel.</param>
	''' <returns>Indicateurs de création de compte</returns>
	<RRQTypeTransaction(XuBiModeTransactionnel.XuBiMtAucun)> _
	<TransactionFlow(TransactionFlowOption.NotAllowed)> _
	<OperationContract(name:="ObtenirIndicateursCreationCompte5")> _
	Function ObtenirIndicateursCreationCompte(ByRef ChaineContexte as String) as TS1N201_DtCdAccGenV1.TsDtIndCreCpt

	''' <summary>
	''' Permet d'obtenir l'état du fichier d'exportation pour savoir si il est à jour ou non
	''' </summary>
	''' <param name="ChaineContexte">La chaîne de contexte nécessaire pour effectuer un appel.</param>
	''' <returns>Informations sur l'état du fichier d'exportation</returns>
	<RRQTypeTransaction(XuBiModeTransactionnel.XuBiMtAucun)> _
	<TransactionFlow(TransactionFlowOption.NotAllowed)> _
	<OperationContract(name:="ObtenirEtatFichierExportation6")> _
	Function ObtenirEtatFichierExportation(ByRef ChaineContexte as String) as TS1N201_DtCdAccGenV1.TsDtEtaFicExp

End Interface

#End Region

#Region "--- XuCuInterface ---"

<EditorBrowsable(EditorBrowsableState.Never)> _
Public Class XuCuInterface
    Inherits ClassesBaseIntegration.XuCuBaseInterfaceV2

    Protected Overrides ReadOnly Property NomAssemblyComposant() As String
        Get
            Return "TS1N215_CiCodAccGen"
        End Get
    End Property

    Protected Overrides ReadOnly Property NomClasseComposant() As String
        Get
            Return "TS1N215_CiCodAccGen.TsCaNiveauSecrt9"
        End Get
    End Property

End Class

#End Region

#Region "--- Proxy ---"

<EditorBrowsable(EditorBrowsableState.Never)> _
Public Class XuCuProxy
    Inherits ClassesBaseIntegration.XuCuBaseProxy
    Implements TsICompI

    Public Overrides ReadOnly Property NomAssembly() As String
        Get
            Dim interfCompi As New XuCuInterface()
            Return interfCompi.ObtenirNomAssembly()
        End Get
    End Property

    Private Function ObtenirListeCle(ByRef ChaineContexte as String) as Generic.IList(Of TS1N201_DtCdAccGenV1.TsDtCleSym) Implements TsICompI.ObtenirListeCle
		Using p As New XuCuProxyHelper(Of TsICompI)(ChaineContexte, MyBase.Environnement)
			Return p.Proxy.ObtenirListeCle(ChaineContexte)
		End Using
	End Function

	
	Private Function ObtenirCle(ByRef ChaineContexte as String, ByVal IdCle as String) as TS1N201_DtCdAccGenV1.TsDtCleSym Implements TsICompI.ObtenirCle
		Using p As New XuCuProxyHelper(Of TsICompI)(ChaineContexte, MyBase.Environnement)
			Return p.Proxy.ObtenirCle(ChaineContexte, IdCle)
		End Using
	End Function

	
	Private Function ObtenirCles(ByRef ChaineContexte as String, ByVal IdCleParent as String) as Generic.IList(Of TS1N201_DtCdAccGenV1.TsDtCleSym) Implements TsICompI.ObtenirCles
		Using p As New XuCuProxyHelper(Of TsICompI)(ChaineContexte, MyBase.Environnement)
			Return p.Proxy.ObtenirCles(ChaineContexte, IdCleParent)
		End Using
	End Function

	
	Private Function ObtenirCleRecherche(ByRef ChaineContexte as String, ByVal CoTypCle as String, ByVal CoTypEnv as String, ByVal GroupeAd as String, ByVal IdCle as String, ByVal UsagerAd as String) as DataTable Implements TsICompI.ObtenirCleRecherche
		Using p As New XuCuProxyHelper(Of TsICompI)(ChaineContexte, MyBase.Environnement)
			Return p.Proxy.ObtenirCleRecherche(ChaineContexte, CoTypCle, CoTypEnv, GroupeAd, IdCle, UsagerAd)
		End Using
	End Function

	
	Private Function ObtenirIndicateursCreationCompte(ByRef ChaineContexte as String) as TS1N201_DtCdAccGenV1.TsDtIndCreCpt Implements TsICompI.ObtenirIndicateursCreationCompte
		Using p As New XuCuProxyHelper(Of TsICompI)(ChaineContexte, MyBase.Environnement)
			Return p.Proxy.ObtenirIndicateursCreationCompte(ChaineContexte)
		End Using
	End Function

	
	Private Function ObtenirEtatFichierExportation(ByRef ChaineContexte as String) as TS1N201_DtCdAccGenV1.TsDtEtaFicExp Implements TsICompI.ObtenirEtatFichierExportation
		Using p As New XuCuProxyHelper(Of TsICompI)(ChaineContexte, MyBase.Environnement)
			Return p.Proxy.ObtenirEtatFichierExportation(ChaineContexte)
		End Using
	End Function


End Class

#End Region

