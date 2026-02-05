Imports System.DirectoryServices
Imports System.DirectoryServices.AccountManagement


Friend Class ActiveDirectoryLightweightDirectoryServices
    Implements IDisposable
    Private ReadOnly _directoryConnexion As String
    Private ReadOnly _principalConnexion As String
    Private _directoryEntry As DirectoryEntry = Nothing


    Public Sub New(serveur As String, racine As String)
        _directoryConnexion = String.Format("LDAP://{0}/{1}", serveur, racine)
        _principalConnexion = serveur
    End Sub

    Public ReadOnly Property DirectoryEntry As DirectoryEntry
        Get
            Return _directoryEntry
        End Get
    End Property

    Public ReadOnly Property Connexion() As ActiveDirectoryLightweightDirectoryServices
        Get
            If _directoryEntry Is Nothing Then _directoryEntry = New DirectoryEntry(_directoryConnexion)
            Return Me
        End Get
    End Property

    Public Sub CreerGroupe(ByVal nomGroupe As String, description As String, emplacement As String)
        Using pc As PrincipalContext = New PrincipalContext(ContextType.ApplicationDirectory, _principalConnexion, emplacement)
            Using gp As New GroupPrincipal(pc)
                gp.Name = nomGroupe
                If Not String.IsNullOrEmpty(description) Then gp.Description = description

                gp.Save()
            End Using
        End Using
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Using _directoryEntry
        End Using
    End Sub

End Class
