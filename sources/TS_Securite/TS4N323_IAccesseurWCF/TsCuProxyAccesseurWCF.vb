Imports Rrq.InfrastructureCommune.ScenarioTransactionnel

Public Class TsCuProxyAccesseurWCF
    Inherits XuCuProxyInfra(Of TsIAccesseurWCF)
    Implements IDisposable

    Public Sub New()
        MyBase.New()

        _Url = "TS/exec/TS4N323_IAccesseurWCF.TS4N323_IAccesseurWCF.TsIAccesseurWCF.svc/TS4N323_IAccesseurWCF.TsIAccesseurWCF"

    End Sub

    Public Function EstMembreGroupe(ByVal NomGroupe As String, ByVal CodeUsager As String) As Boolean
        CreateChannel()
        Return Channel.EstMembreGroupe(NomGroupe, CodeUsager)
    End Function

    Public Function EstMembreGroupeV2(ByVal CodeUsager As String, ByVal NomGroupes As Generic.IList(Of String)) As Generic.IDictionary(Of String, Boolean)
        CreateChannel()
        Return Channel.EstMembreGroupeV2(CodeUsager, NomGroupes)
    End Function

    Public Function ObtenirUtilisateurGroupe(ByVal NomGroupe As Generic.IList(Of String), ByVal Recursif As Boolean) As Generic.IList(Of Rrq.Securite.Applicative.TsDtUtilisateur)
        CreateChannel()
        Return Channel.ObtenirUtilisateurGroupe(NomGroupe, Recursif)
    End Function

    Public Function ObtenirGroupeUtilisateur(ByVal CodeUsager As String) As Generic.IList(Of Rrq.Securite.Applicative.TsDtGroupe)
        CreateChannel()
        Return Channel.ObtenirGroupeUtilisateur(CodeUsager)
    End Function

    Public Function ObtenirGroupeMembreDe(ByVal NomGroupe As String, ByVal Recursif As Boolean) As Generic.IList(Of Rrq.Securite.Applicative.TsDtGroupe)
        CreateChannel()
        Return Channel.ObtenirGroupeMembreDe(NomGroupe, Recursif)
    End Function

    Public Function ObtenirGroupe(ByVal NomGroupe As String) As Rrq.Securite.Applicative.TsDtGroupe
        CreateChannel()
        Return Channel.ObtenirGroupe(NomGroupe)
    End Function

    Public Function RechercherGroupes(ByVal Filtre As String) As Generic.IList(Of String)
        CreateChannel()
        Return Channel.RechercherGroupes(Filtre)
    End Function

End Class
