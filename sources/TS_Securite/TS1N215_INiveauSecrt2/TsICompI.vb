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
<ServiceContract(Namespace:="TS1N215_INiveauSecrt2.TsICompI")> _
Public Interface TsICompI

	''' <summary>
	''' Méthode pour enregistrer une clé ou des clés (dans le cas d'une insertion)
	''' </summary>
	''' <param name="ChaineContexte">La chaîne de contexte nécessaire pour effectuer un appel.</param>
	''' <param name="CleSymbolique">Dt de la clé symbolique à sauvegarderDans le cas d'une insertion multiple, certains champs vont contenir l'information multiple</param>
	''' <param name="IndicMdpNouveau">Indicateur pour savoir si on doit encrypter le nouveau mot de passe.Si deja encrypter, il ne faut pas l'encrypter une deuxieme fois.</param>
	''' <param name="IndicMaj">Indicateur pour savoir si c'est une mise a jour Vrai = Mise a jour, Faux = Insertion</param>
	<RRQTypeTransaction(XuBiModeTransactionnel.XuBiMtDistribue)> _
	<TransactionFlow(TransactionFlowOption.NotAllowed)> _
	<OperationContract(name:="EnregistrerCle1")> _
	Function EnregistrerCle(ByRef ChaineContexte as String, ByVal CleSymbolique as TS1N201_DtCdAccGenV1.TsDtCleSym, ByVal IndicMdpNouveau as Boolean, ByVal IndicMaj as Boolean) as Boolean

	''' <summary>
	''' Méthode pour détruire une clé
	''' </summary>
	''' <param name="ChaineContexte">La chaîne de contexte nécessaire pour effectuer un appel.</param>
	''' <param name="CleSymbolique">Dt de la clé symbolique à détruire</param>
	<RRQTypeTransaction(XuBiModeTransactionnel.XuBiMtDistribue)> _
	<TransactionFlow(TransactionFlowOption.NotAllowed)> _
	<OperationContract(name:="DetruireCle2")> _
	Function DetruireCle(ByRef ChaineContexte as String, ByVal CleSymbolique as TS1N201_DtCdAccGenV1.TsDtCleSym) as Boolean

	''' <summary>
	''' Méthode pour importer des clés
	''' </summary>
	''' <param name="ChaineContexte">La chaîne de contexte nécessaire pour effectuer un appel.</param>
	<RRQTypeTransaction(XuBiModeTransactionnel.XuBiMtDistribue)> _
	<TransactionFlow(TransactionFlowOption.NotAllowed)> _
	<OperationContract(name:="ImporterCles3")> _
	Sub ImporterCles(ByRef ChaineContexte as String)

	''' <summary>
	''' Méthode pour exporter des clés
	''' </summary>
	''' <param name="ChaineContexte">La chaîne de contexte nécessaire pour effectuer un appel.</param>
	<RRQTypeTransaction(XuBiModeTransactionnel.XuBiMtDistribue)> _
	<TransactionFlow(TransactionFlowOption.NotAllowed)> _
	<OperationContract(name:="ExporterCles4")> _
	Sub ExporterCles(ByRef ChaineContexte as String)

	''' <summary>
	''' Tente d'obtenir un verrou sur l'édition pour éviter la concurrence
	''' </summary>
	''' <param name="ChaineContexte">La chaîne de contexte nécessaire pour effectuer un appel.</param>
	''' <returns>Indicateur de verrou obtenu</returns>
	<RRQTypeTransaction(XuBiModeTransactionnel.XuBiMtAucun)> _
	<TransactionFlow(TransactionFlowOption.NotAllowed)> _
	<OperationContract(name:="ObtenirVerrouEdition5")> _
	Function ObtenirVerrouEdition(ByRef ChaineContexte as String) as TsDtVerrou

	''' <summary>
	''' Effectue la libération du verrou d'édition
	''' </summary>
	''' <param name="ChaineContexte">La chaîne de contexte nécessaire pour effectuer un appel.</param>
	<RRQTypeTransaction(XuBiModeTransactionnel.XuBiMtAucun)> _
	<TransactionFlow(TransactionFlowOption.NotAllowed)> _
	<OperationContract(name:="RelacherVerrouEdition6")> _
	Sub RelacherVerrouEdition(ByRef ChaineContexte as String)

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
            Return "TS1N215_CiCodAccGen.TsCaNiveauSecrt2"
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

    Private Function EnregistrerCle(ByRef ChaineContexte as String, ByVal CleSymbolique as TS1N201_DtCdAccGenV1.TsDtCleSym, ByVal IndicMdpNouveau as Boolean, ByVal IndicMaj as Boolean) as Boolean Implements TsICompI.EnregistrerCle
		Using p As New XuCuProxyHelper(Of TsICompI)(ChaineContexte, MyBase.Environnement)
			Return p.Proxy.EnregistrerCle(ChaineContexte, CleSymbolique, IndicMdpNouveau, IndicMaj)
		End Using
	End Function

	
	Private Function DetruireCle(ByRef ChaineContexte as String, ByVal CleSymbolique as TS1N201_DtCdAccGenV1.TsDtCleSym) as Boolean Implements TsICompI.DetruireCle
		Using p As New XuCuProxyHelper(Of TsICompI)(ChaineContexte, MyBase.Environnement)
			Return p.Proxy.DetruireCle(ChaineContexte, CleSymbolique)
		End Using
	End Function

	
	Private Sub ImporterCles(ByRef ChaineContexte as String) Implements TsICompI.ImporterCles
		Using p As New XuCuProxyHelper(Of TsICompI)(ChaineContexte, MyBase.Environnement)
			p.Proxy.ImporterCles(ChaineContexte)
		End Using
	End Sub

	
	Private Sub ExporterCles(ByRef ChaineContexte as String) Implements TsICompI.ExporterCles
		Using p As New XuCuProxyHelper(Of TsICompI)(ChaineContexte, MyBase.Environnement)
			p.Proxy.ExporterCles(ChaineContexte)
		End Using
	End Sub

	
	Private Function ObtenirVerrouEdition(ByRef ChaineContexte as String) as TsDtVerrou Implements TsICompI.ObtenirVerrouEdition
		Using p As New XuCuProxyHelper(Of TsICompI)(ChaineContexte, MyBase.Environnement)
			Return p.Proxy.ObtenirVerrouEdition(ChaineContexte)
		End Using
	End Function

	
	Private Sub RelacherVerrouEdition(ByRef ChaineContexte as String) Implements TsICompI.RelacherVerrouEdition
		Using p As New XuCuProxyHelper(Of TsICompI)(ChaineContexte, MyBase.Environnement)
			p.Proxy.RelacherVerrouEdition(ChaineContexte)
		End Using
	End Sub


End Class

#End Region

