Imports System.DirectoryServices.AccountManagement

Public Class TsCuValiderCredential

    Public Function ValiderUsagerMotPasse(ByVal CodeUsager As String, ByVal MotDePasse As String) As Boolean
        If Not tsCuObtnrInfoAD.UtilisateurExiste(CodeUsager) Then
            Return False
        End If

        Dim utilisateur As TsCuUtilisateurAD = tsCuObtnrInfoAD.ObtenirUtilisateur(CodeUsager)
        If utilisateur.UtilisateurDesactive Then
            Return False
        End If

        Return validerCredentials(CodeUsager, MotDePasse, utilisateur.DomaineNT)
    End Function


    Private Function validerCredentials(ByVal codeUsager As String, ByVal motDePasse As String, domaine As String) As Boolean
        'À cause du trust des domaines et parce que le code d'utilisateurs sont 
        Using context As PrincipalContext = New PrincipalContext(ContextType.Domain, domaine)
            Return context.ValidateCredentials(codeUsager, motDePasse)
        End Using
    End Function

End Class
