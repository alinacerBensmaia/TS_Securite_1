Imports System.Runtime.CompilerServices
Imports TS4N323_IAccesseurWCF

Public Class TsCuAccesWCF
    Implements TsISecrtApplicative

    Public Function EstMembreGroupe(NomGroupe As String, CodeUsager As String) As Boolean Implements TsISecrtApplicative.EstMembreGroupe

        Using securiteApplicative As New TsCuProxyAccesseurWCF()
            Return securiteApplicative.EstMembreGroupe(NomGroupe, CodeUsager)
        End Using

    End Function

    Public Function EstMembreGroupeV2(CodeUsager As String, NomGroupes As IList(Of String)) As IDictionary(Of String, Boolean) Implements TsISecrtApplicative.EstMembreGroupeV2

        Using securiteApplicative As New TsCuProxyAccesseurWCF()
            Return securiteApplicative.EstMembreGroupeV2(CodeUsager, NomGroupes)
        End Using

    End Function

    Public Function ObtenirGroupe(NomGroupe As String) As TsDtGroupe Implements TsISecrtApplicative.ObtenirGroupe

        Using securiteApplicative As New TsCuProxyAccesseurWCF()
            Return securiteApplicative.ObtenirGroupe(NomGroupe)
        End Using


    End Function

    Public Function ObtenirGroupeMembreDe(NomGroupe As String, Recursif As Boolean) As IList(Of TsDtGroupe) Implements TsISecrtApplicative.ObtenirGroupeMembreDe

        Using securiteApplicative As New TsCuProxyAccesseurWCF()
            Return securiteApplicative.ObtenirGroupeMembreDe(NomGroupe, Recursif)
        End Using

    End Function

    Public Function ObtenirGroupeUtilisateur(CodeUsager As String) As IList(Of TsDtGroupe) Implements TsISecrtApplicative.ObtenirGroupeUtilisateur

        Using securiteApplicative As New TsCuProxyAccesseurWCF()
            Return securiteApplicative.ObtenirGroupeUtilisateur(CodeUsager)
        End Using

    End Function

    Public Function ObtenirUtilisateurGroupe(LsNomGroupe As IList(Of String), Recursif As Boolean) As IList(Of TsDtUtilisateur) Implements TsISecrtApplicative.ObtenirUtilisateurGroupe

        Using securiteApplicative As New TsCuProxyAccesseurWCF()
            Return securiteApplicative.ObtenirUtilisateurGroupe(LsNomGroupe, Recursif)
        End Using

    End Function

    Public Function RechercherGroupes(Filtre As String) As IList(Of String) Implements TsISecrtApplicative.RechercherGroupes

        Using securiteApplicative As New TsCuProxyAccesseurWCF()
            Return securiteApplicative.RechercherGroupes(Filtre)
        End Using

    End Function

End Class
