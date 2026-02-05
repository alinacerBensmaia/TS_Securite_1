Imports System.ServiceModel
Imports System.ComponentModel
Imports Rrq.InfrastructureCommune.ScenarioTransactionnel

#Region "--- TsIAccesseurWCF ---"

''' <summary>
''' 
''' </summary>
<ServiceContract(Namespace:="TsIAccesseurWCF")> _
Public Interface TsIAccesseurWCF

    ''' <summary>
    ''' 
    ''' </summary>
    <OperationContract(name:="EstMembreGroupe1")> _
    Function EstMembreGroupe(ByVal NomGroupe As String, ByVal CodeUsager As String) As Boolean

    ''' <summary>
    ''' 
    ''' </summary>
    <OperationContract(Name:="EstMembreGroupeV2")>
    Function EstMembreGroupeV2(ByVal CodeUsager As String, ByVal NomGroupes As Generic.IList(Of String)) As Generic.IDictionary(Of String, Boolean)

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="NomGroupe"></param>
    ''' <param name="Recursif"></param>
    <OperationContract(name:="ObtenirUtilisateurGroupe2")> _
    Function ObtenirUtilisateurGroupe(ByVal NomGroupe As Generic.IList(Of String), ByVal Recursif As Boolean) As Generic.IList(Of Rrq.Securite.Applicative.TsDtUtilisateur)

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="CodeUsager"></param>
    <OperationContract(name:="ObtenirGroupeUtilisateur3")> _
    Function ObtenirGroupeUtilisateur(ByVal CodeUsager As String) As Generic.IList(Of Rrq.Securite.Applicative.TsDtGroupe)

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="NomGroupe"></param>
    ''' <param name="Recursif"></param>
    <OperationContract(name:="ObtenirGroupeMembreDe4")> _
    Function ObtenirGroupeMembreDe(ByVal NomGroupe As String, ByVal Recursif As Boolean) As Generic.IList(Of Rrq.Securite.Applicative.TsDtGroupe)

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="NomGroupe"></param>
    <OperationContract(name:="ObtenirGroupe5")> _
    Function ObtenirGroupe(ByVal NomGroupe As String) As Rrq.Securite.Applicative.TsDtGroupe

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Filtre"></param>
    <OperationContract(name:="RechercherGroupes6")> _
    Function RechercherGroupes(ByVal Filtre As String) As Generic.IList(Of String)

End Interface

#End Region

#Region "--- XuCuInterface ---"

<EditorBrowsable(EditorBrowsableState.Never)> _
Public Class XuCuInterface
    Inherits Rrq.InfrastructureCommune.ScenarioTransactionnel.XuCuInterfaceInfra

    Protected Overrides ReadOnly Property NomAssemblyComposant() As String
        Get
            Return "TS4N323_CiAccesseurWCF"
        End Get
    End Property

    Protected Overrides Function GetNomClasseComposant(ByVal TypeInterface As System.Type) As String

        Select Case True
            Case TypeInterface Is GetType(TsIAccesseurWCF)
                Return "TS4N323_CiAccesseurWCF.TsCaAccesseurWCF"
            Case Else
                Throw New Exception("Le type """ + TypeInterface.FullName + """ n'a pas été géré par la méthode NomClasseComposant(ByVal typeInterface As System.Type).")
        End Select

    End Function

End Class

#End Region


