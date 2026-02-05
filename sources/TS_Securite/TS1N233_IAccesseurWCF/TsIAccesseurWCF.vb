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
    <OperationContract(Name:="ObtenirCodeAccesMotDePasse1")>
    Sub ObtenirCodeAccesMotDePasse(ByVal strCle As String, ByVal strRaison As String, ByRef strCompte As String, ByRef strMDP As String)

    <OperationContract(Name:="ObtenirCodeAccesMotDePasse2")>
    Sub ObtenirCodeAccesMotDePasse(ByVal strCle As String, ByVal strRaison As String, ByVal strUsagerZE As String, ByRef strCompte As String, ByRef strMDP As String)

    <OperationContract(Name:="ObtenirCodeAccesMotDePasseLibraire3")>
    Sub ObtenirCodeAccesMotDePasseLibraire(ByVal strCle As String, ByVal strRaison As String, ByRef strCompte As String, ByRef strMDP As String)

End Interface

#End Region

#Region "--- XuCuInterface ---"

<EditorBrowsable(EditorBrowsableState.Never)> _
Public Class XuCuInterface
    Inherits Rrq.InfrastructureCommune.ScenarioTransactionnel.XuCuInterfaceInfra

    Protected Overrides ReadOnly Property NomAssemblyComposant() As String
        Get
            Return "TS1N233_CiAccesseurWCF"
        End Get
    End Property

    Protected Overrides Function GetNomClasseComposant(ByVal TypeInterface As System.Type) As String

        Select Case True
            Case TypeInterface Is GetType(TsIAccesseurWCF)
                Return "TS1N233_CiAccesseurWCF.TsCaAccesseurWCF"
            Case Else
                Throw New Exception("Le type """ + TypeInterface.FullName + """ n'a pas été géré par la méthode NomClasseComposant(ByVal typeInterface As System.Type).")
        End Select

    End Function


End Class

#End Region


