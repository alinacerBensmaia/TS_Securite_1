Imports Rrq.InfrastructureCommune.ScenarioTransactionnel

Public Class TsCuProxyAccesseurADWCF
    Inherits XuCuProxyInfra(Of TsIAccesseurADWCF)
    Implements IDisposable

    Public Sub New()
        MyBase.New()

        _Url = "TS/exec/TS4N215_IObtnrInfoAD.TS4N215_IObtnrInfoAD.TsIAccesseurADWCF.svc/TS4N215_IObtnrInfoAD.TsIAccesseurADWCF"

    End Sub

    Public Function RechercheActiveDirectory(ByVal NomServeurAD As String, ByVal pTypeRequete As TsIAccesseurADWCF.TsIadTypeRequete, ByVal strCritereRecherche As String, ByVal strCritereRechercheSecondaire As String, ByVal pObjectCategory As TsIAccesseurADWCF.TsIadObjectCategory) As DataTable
        CreateChannel()
        Return Channel.RechercheActiveDirectory(NomServeurAD, pTypeRequete, strCritereRecherche, strCritereRechercheSecondaire, pObjectCategory)
    End Function

    Public Function RechercheGroupeAD(ByVal NomServeurAD As String, ByVal strGroupe As String, ByVal blnRechRecursive As Boolean) As DataTable
        CreateChannel()
        Return Channel.RechercheGroupeAD(NomServeurAD, strGroupe, blnRechRecursive)
    End Function

    Public Function ChercheDansGroupes(ByVal NomServeurAD As String, ByVal strACID As String, ByVal strGroupeRecherche As String) As Boolean
        CreateChannel()
        Return Channel.ChercheDansGroupes(NomServeurAD, strACID, strGroupeRecherche)
    End Function

    Public Function ObtenirMembresGroupe(ByVal NomServeurAD As String, ByVal NomGroupe As String) As String()
        CreateChannel()
        Return Channel.ObtenirMembresGroupe(NomServeurAD, NomGroupe)
    End Function

    Public Function VerifierGroupeExiste(ByVal NomServeurAD As String, ByVal strGroupe As String) As Boolean
        CreateChannel()
        Return Channel.VerifierGroupeExiste(NomServeurAD, strGroupe)
    End Function

    Public Function DomaineNT(ByVal NomServeurAD As String) As String
        CreateChannel()
        Return Channel.DomaineNT(NomServeurAD)
    End Function

End Class
