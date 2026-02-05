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
<ServiceContract(Namespace:="TS1N215_INiveauSecrt1.TsICompI")> _
Public Interface TsICompI

	''' <summary>
	''' 
	''' </summary>
	''' <param name="ChaineContexte">La chaîne de contexte nécessaire pour effectuer un appel.</param>
	<RRQTypeTransaction(XuBiModeTransactionnel.XuBiMtAucun)> _
	<TransactionFlow(TransactionFlowOption.NotAllowed)> _
	<OperationContract(name:="ObtenirListeCle1")> _
	Function ObtenirListeCle(ByRef ChaineContexte as String) as Generic.IList(Of TS1N201_DtCdAccGenV1.TsDtCleSym)

	''' <summary>
	''' 
	''' </summary>
	''' <param name="chaineContexte">La chaîne de contexte nécessaire pour effectuer un appel.</param>
	''' <param name="cle"></param>
	<RRQTypeTransaction(XuBiModeTransactionnel.XuBiMtDistribue)> _
	<TransactionFlow(TransactionFlowOption.NotAllowed)> _
	<OperationContract(name:="AfficherMDP2")> _
	Function AfficherMDP(ByRef chaineContexte as String, ByVal cle as TS1N201_DtCdAccGenV1.TsDtCleSym) as String

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
            Return "TS1N215_CiCodAccGen.TsCaNiveauSecrt1"
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

	
	Private Function AfficherMDP(ByRef chaineContexte as String, ByVal cle as TS1N201_DtCdAccGenV1.TsDtCleSym) as String Implements TsICompI.AfficherMDP
		Using p As New XuCuProxyHelper(Of TsICompI)(chaineContexte, MyBase.Environnement)
			Return p.Proxy.AfficherMDP(chaineContexte, cle)
		End Using
	End Function


End Class

#End Region

