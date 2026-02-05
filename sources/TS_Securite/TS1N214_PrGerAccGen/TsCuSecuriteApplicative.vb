Imports System.Collections.Generic
Imports System.Security.Principal

Friend Class TsCuSecuriteApplicative
    Private ReadOnly _prefixeProfils As String
    Private ReadOnly _securite As Rrq.Securite.Applicative.TsCaVerfrSecrtApplicative

    Public Sub New()
        _prefixeProfils = Config.PrefixeRechercheProfils
        _securite = New Rrq.Securite.Applicative.TsCaVerfrSecrtApplicative()
    End Sub

    Public Function ObtenirTousLesProfils() As IList(Of String)
        Dim prefix As String = _prefixeProfils
        If Not prefix.EndsWith("*") Then prefix &= "*"

        Return _securite.RechercherGroupes(prefix)
    End Function

    Public Function EstMembreROI(nomGroupe As String, userToken As WindowsIdentity) As Boolean
        Return _securite.EstMembreGroupe(nomGroupe, userToken)
    End Function

    Public Function GroupesExistent(nomGroupes() As String) As Boolean
        For Each nomGroupe As String In nomGroupes
            If Not _securite.GroupeExiste(nomGroupe) Then Return False
        Next
        Return True
    End Function

End Class
