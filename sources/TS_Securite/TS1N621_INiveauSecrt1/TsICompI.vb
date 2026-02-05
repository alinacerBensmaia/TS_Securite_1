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
<ServiceContract(Namespace:="TS1N621_INiveauSecrt1.TsICompI")> _
Public Interface TsICompI

	''' <summary>
	''' 
	''' </summary>
	''' <param name="ChaineContexte">La chaîne de contexte nécessaire pour effectuer un appel.</param>
	''' <param name="InformationsCreation">Informations pour la création des comptes</param>
	<RRQTypeTransaction(XuBiModeTransactionnel.XuBiMtAucun)> _
	<TransactionFlow(TransactionFlowOption.NotAllowed)> _
	<OperationContract(name:="CreerCompteFtp1")> _
	Function CreerCompteFtp(ByRef ChaineContexte as String, ByVal InformationsCreation as TsDtCreationComptes) as Generic.IList(Of TsDtInfoCleSymbolique)

	''' <summary>
	''' Obtenir la liste de tous les comptes FTP
	''' </summary>
	''' <param name="ChaineContexte">La chaîne de contexte nécessaire pour effectuer un appel.</param>
	''' <returns>Liste des comptes FTP</returns>
	<RRQTypeTransaction(XuBiModeTransactionnel.XuBiMtAucun)> _
	<TransactionFlow(TransactionFlowOption.NotAllowed)> _
	<OperationContract(name:="ObtenirListeComptes2")> _
	Function ObtenirListeComptes(ByRef ChaineContexte as String) as Generic.IList(Of String)

	''' <summary>
	''' 
	''' </summary>
	''' <param name="ChaineContexte">La chaîne de contexte nécessaire pour effectuer un appel.</param>
	''' <param name="NomCompte">Nom complet du compte à supprimer</param>
	<RRQTypeTransaction(XuBiModeTransactionnel.XuBiMtAucun)> _
	<TransactionFlow(TransactionFlowOption.NotAllowed)> _
	<OperationContract(name:="SupprimerCompte3")> _
	Sub SupprimerCompte(ByRef ChaineContexte as String, ByVal NomCompte as String)

	''' <summary>
	''' Déverrouiller un compte FTP donné
	''' </summary>
	''' <param name="ChaineContexte">La chaîne de contexte nécessaire pour effectuer un appel.</param>
	''' <param name="NomCompte">Nom complet du compte à déverrouiller</param>
	<RRQTypeTransaction(XuBiModeTransactionnel.XuBiMtAucun)> _
	<TransactionFlow(TransactionFlowOption.NotAllowed)> _
	<OperationContract(name:="DeverrouillerCompte4")> _
	Sub DeverrouillerCompte(ByRef ChaineContexte as String, ByVal NomCompte as String)

End Interface

#End Region

#Region "--- XuCuInterface ---"

<EditorBrowsable(EditorBrowsableState.Never)> _
Public Class XuCuInterface
    Inherits ClassesBaseIntegration.XuCuBaseInterfaceV2

    Protected Overrides ReadOnly Property NomAssemblyComposant() As String
        Get
            Return "TS1N621_CiGererUtilSecFtpSvr"
        End Get
    End Property

    Protected Overrides ReadOnly Property NomClasseComposant() As String
        Get
            Return "TS1N621_CiGererUtilSecFtpSvr.TsCaNiveauSecrt1"
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

    Private Function CreerCompteFtp(ByRef ChaineContexte as String, ByVal InformationsCreation as TsDtCreationComptes) as Generic.IList(Of TsDtInfoCleSymbolique) Implements TsICompI.CreerCompteFtp
		Using p As New XuCuProxyHelper(Of TsICompI)(ChaineContexte, MyBase.Environnement)
			Return p.Proxy.CreerCompteFtp(ChaineContexte, InformationsCreation)
		End Using
	End Function

	
	Private Function ObtenirListeComptes(ByRef ChaineContexte as String) as Generic.IList(Of String) Implements TsICompI.ObtenirListeComptes
		Using p As New XuCuProxyHelper(Of TsICompI)(ChaineContexte, MyBase.Environnement)
			Return p.Proxy.ObtenirListeComptes(ChaineContexte)
		End Using
	End Function

	
	Private Sub SupprimerCompte(ByRef ChaineContexte as String, ByVal NomCompte as String) Implements TsICompI.SupprimerCompte
		Using p As New XuCuProxyHelper(Of TsICompI)(ChaineContexte, MyBase.Environnement)
			p.Proxy.SupprimerCompte(ChaineContexte, NomCompte)
		End Using
	End Sub

	
	Private Sub DeverrouillerCompte(ByRef ChaineContexte as String, ByVal NomCompte as String) Implements TsICompI.DeverrouillerCompte
		Using p As New XuCuProxyHelper(Of TsICompI)(ChaineContexte, MyBase.Environnement)
			p.Proxy.DeverrouillerCompte(ChaineContexte, NomCompte)
		End Using
	End Sub


End Class

#End Region

