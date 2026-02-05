Imports System.Collections.Generic

Friend Class DistinctList
    Inherits List(Of String)

    Public Function Contient(valeur As String) As Boolean
        Return Contains(valeur, StringComparer.InvariantCultureIgnoreCase)
    End Function

    Public Overloads Sub Ajouter(valeur As String)
        If Not Contient(valeur) Then
            Add(valeur)
        End If
    End Sub

    Public Overloads Sub AjouterValeurs(valeurs As IEnumerable(Of String))
        For Each item As String In valeurs
            Ajouter(item)
        Next
    End Sub

End Class