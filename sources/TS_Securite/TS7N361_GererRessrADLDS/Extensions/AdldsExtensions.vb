Imports System.DirectoryServices
Imports System.Runtime.CompilerServices

Namespace Internal

    Friend Module AdldsExtensions
        Private Const AD_PROP_name As String = "name"
        Private Const AD_PROP_userPrincipalName As String = "userPrincipalName"

        <Extension>
        Public Function ObtenirGroupe(source As ActiveDirectoryLightweightDirectoryServices, name As String) As DirectoryEntry
            Return source.DirectoryEntry.SelectWherePropertyIs(AD_PROP_name, name)
        End Function

        <Extension>
        Public Function ObtenirUtilisateur(source As ActiveDirectoryLightweightDirectoryServices, userPrincipalName As String) As DirectoryEntry
            Return source.DirectoryEntry.SelectWherePropertyIs(AD_PROP_userPrincipalName, userPrincipalName)
        End Function

        <Extension>
        Public Function GroupeExiste(source As ActiveDirectoryLightweightDirectoryServices, nomGroupe As String) As Boolean
            Using groupe As DirectoryEntry = source.ObtenirGroupe(nomGroupe)
                Return Not (groupe Is Nothing)
            End Using
        End Function

    End Module

End Namespace