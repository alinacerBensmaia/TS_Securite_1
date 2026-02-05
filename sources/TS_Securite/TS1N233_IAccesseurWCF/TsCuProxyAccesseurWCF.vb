Imports Rrq.InfrastructureCommune.ScenarioTransactionnel

Public Class TsCuProxyAccesseurWCF
    Inherits XuCuProxyInfra(Of TsIAccesseurWCF)
    Implements IDisposable

    Public Sub New()
        MyBase.New()

        _Url = "TS/exec/TS1N233_IAccesseurWCF.TS1N233_IAccesseurWCF.TsIAccesseurWCF.svc/TS1N233_IAccesseurWCF.TsIAccesseurWCF"

    End Sub

    Public Sub ObtenirCodeAccesMotDePasse(ByVal strCle As String, ByVal strRaison As String, ByRef strCompte As String, ByRef strMDP As String)
        CreateChannel()
        Channel.ObtenirCodeAccesMotDePasse(strCle, strRaison, strCompte, strMDP)
    End Sub

    Public Sub ObtenirCodeAccesMotDePasse(ByVal strCle As String, ByVal strRaison As String, ByVal strUsagerZE As String, ByRef strCompte As String, ByRef strMDP As String)
        CreateChannel()
        Channel.ObtenirCodeAccesMotDePasse(strCle, strRaison, strUsagerZE, strCompte, strMDP)
    End Sub

    Public Sub ObtenirCodeAccesMotDePasseLibraire(ByVal strCle As String, ByVal strRaison As String, ByRef strCompte As String, ByRef strMDP As String)
        CreateChannel()
        Channel.ObtenirCodeAccesMotDePasseLibraire(strCle, strRaison, strCompte, strMDP)
    End Sub

End Class
