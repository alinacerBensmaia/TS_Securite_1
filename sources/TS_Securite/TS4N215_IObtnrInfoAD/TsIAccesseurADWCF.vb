Imports System.ServiceModel
Imports System.ComponentModel
Imports Rrq.InfrastructureCommune.ScenarioTransactionnel

#Region "--- TsIAccesseurWCF ---"

''' <summary>
''' 
''' </summary>
<ServiceContract(Namespace:="TsIAccesseurADWCF")>
Public Interface TsIAccesseurADWCF

    ''' <summary>
    ''' 
    ''' </summary>
    <OperationContract(Name:="RechercheActiveDirectory1")>
    Function RechercheActiveDirectory(ByVal NomServeurAD As String, ByVal pTypeRequete As TsIadTypeRequete, ByVal strCritereRecherche As String,
                                      ByVal strCritereRechercheSecondaire As String,
                                      ByVal pObjectCategory As TsIadObjectCategory) As DataTable

    ''' <summary>
    ''' 
    ''' </summary>
    <OperationContract(Name:="RechercheGroupeAD2")>
    Function RechercheGroupeAD(ByVal NomServeurAD As String, ByVal strGroupe As String, ByVal blnRechRecursive As Boolean) As DataTable

    ''' <summary>
    ''' 
    ''' </summary>
    <OperationContract(Name:="ChercheDansGroupes3")>
    Function ChercheDansGroupes(ByVal NomServeurAD As String, ByVal strACID As String, ByVal strGroupeRecherche As String) As Boolean

    ''' <summary>
    ''' 
    ''' </summary>

    <OperationContract(Name:="ObtenirMembresGroupe4")>
    Function ObtenirMembresGroupe(ByVal NomServeurAD As String, ByVal NomGroupe As String) As String()

    ''' <summary>
    ''' 
    ''' </summary>

    <OperationContract(Name:="VerifierGroupeExiste5")>
    Function VerifierGroupeExiste(ByVal NomServeurAD As String, ByVal strGroupe As String) As Boolean


    <OperationContract(Name:="DomaineNT6")>
    Function DomaineNT(ByVal NomServeurAD As String) As String


    ''' <summary>
    '''   Possibilité de Champs sur lequel on peut effectuer la recherche dans l'active directory.
    ''' </summary>

    Enum TsIadTypeRequete
        TsIadTrCodeUtilisateur
        TsIadTrNom
        TsIadTrPrenom
        TsIadTrNomComplet
        TsIadTrCourriel
        TsIadTrUniteAdmn
        TsIadTrFonction
        TsIadTrMembreDe
        TsIadTrSid
        TsIadTrSociete
        TsIadTrDescription
        TsIadTrNoEmploye
        TsIadTrNomEtPrenom
        TsIadTrNoTelephone
    End Enum

    ''' <summary>
    '''   Type de catégorie possible pour la recherche dans l'active directory.
    ''' </summary>
    Enum TsIadObjectCategory
        TsIadOcTous
        TsIadOcPerson
        TsIadOcGroup
    End Enum
End Interface

#End Region

#Region "--- XuCuInterface ---"

<EditorBrowsable(EditorBrowsableState.Never)> _
Public Class XuCuInterface
    Inherits Rrq.InfrastructureCommune.ScenarioTransactionnel.XuCuInterfaceInfra

    Protected Overrides ReadOnly Property NomAssemblyComposant() As String
        Get
            Return "TS4N215_CiObtnrInfoAD"
        End Get
    End Property

    Protected Overrides Function GetNomClasseComposant(ByVal TypeInterface As System.Type) As String

        Select Case True
            Case TypeInterface Is GetType(TsIAccesseurADWCF)
                Return "TS4N215_CiObtnrInfoAD.TsCaAccesseurADWCF"
            Case Else
                Throw New Exception("Le type """ + TypeInterface.FullName + """ n'a pas été géré par la méthode NomClasseComposant(ByVal typeInterface As System.Type).")
        End Select

    End Function

End Class



#End Region


