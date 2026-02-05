Imports System.DirectoryServices
Imports System.Runtime.CompilerServices


Namespace Internal

    Friend Module DirectoryEntryExtensions
        Private Const AD_PROP_description As String = "description"
        Private Const AD_PROP_distinguishedName As String = "distinguishedName"
        Private Const AD_PROP_member As String = "member"

        <Extension>
        Public Function SelectWherePropertyIs(source As DirectoryEntry, propertyName As String, propertyValue As String) As DirectoryEntry
            Using searcher As New DirectorySearcher(source, String.Format("{0}={1}", propertyName, propertyValue))
                Dim result As SearchResult = searcher.FindOne()
                If result Is Nothing Then Return Nothing
                Return result.GetDirectoryEntry()
            End Using
        End Function

        <Extension>
        Public Sub AjouterMembre(source As DirectoryEntry, membre As DirectoryEntry)
            source.Properties(AD_PROP_member).Add(membre.Properties(AD_PROP_distinguishedName).Value)
        End Sub

        <Extension>
        Public Sub AssignerDescription(source As DirectoryEntry, valeur As String)
            source.Properties(AD_PROP_description).Value = valeur
        End Sub

        <Extension>
        Public Function ObtenirDescription(source As DirectoryEntry) As String
            If Not source.Properties.Contains(AD_PROP_description) Then Return String.Empty
            If source.Properties(AD_PROP_description) Is Nothing Then Return String.Empty
            If source.Properties(AD_PROP_description).Value Is Nothing Then Return String.Empty

            Return source.Properties(AD_PROP_description).Value.ToString()
        End Function

        ''' <summary>
        ''' Retourne une valeur qui indique si le membre spécifié apparaît dans le liste de membre de ce groupe.
        ''' </summary>
        ''' <param name="source">Ce groupe.</param>
        ''' <param name="membre">Le membre à rechercher.</param>
        ''' <returns>true si le paramètre membre apparaît dans la liste de membre de ce groupe, sinon false.</returns>
        <Extension>
        Public Function Contient(source As DirectoryEntry, membre As DirectoryEntry) As Boolean
            Return source.Properties(AD_PROP_member).Contains(membre.Properties(AD_PROP_distinguishedName).Value)
        End Function

    End Module

End Namespace