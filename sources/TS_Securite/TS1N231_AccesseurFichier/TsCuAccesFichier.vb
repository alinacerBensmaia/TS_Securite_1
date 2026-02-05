Imports System.DirectoryServices.AccountManagement
Imports Rrq.InfrastructureCommune.Parametres
Imports System.DirectoryServices
Imports System.Text
Imports System.Runtime.CompilerServices
Imports Rrq.InfrastructureCommune.UtilitairesCommuns

Public Class TsCuAccesFichier
    Implements TsIObtnrCompteGenerique


#Region "Constructeur"

    Public Sub New()
    End Sub

#End Region

#Region "Méthodes publiques"

    Public Sub ObtenirCodeAccesMotDePasse(strCle As String, strRaison As String, ByRef strCompte As String, ByRef strMDP As String) Implements TsIObtnrCompteGenerique.ObtenirCodeAccesMotDePasse
        Dim cleSymbolique As New tsCuObtCdAccGen()
        cleSymbolique.ObtenirCodeAccesMotDePasse(strCle, strRaison, strCompte, strMDP)
    End Sub

    Public Sub ObtenirCodeAccesMotDePasseLibraire(strCle As String, strRaison As String, ByRef strCompte As String, ByRef strMDP As String) Implements TsIObtnrCompteGenerique.ObtenirCodeAccesMotDePasseLibraire
        Dim cleSymbolique As New tsCuObtCdAccGen()
        cleSymbolique.ObtenirCodeAccesMotDePasseLibraire(strCle, strRaison, strCompte, strMDP)
    End Sub


#End Region



End Class