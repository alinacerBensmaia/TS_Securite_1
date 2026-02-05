Imports System.Collections.Generic

Public Class ListOfFiltreConformite
    Inherits List(Of IFiltreConformite)
    Implements IFiltreConformite

    Public Overloads Sub AddRange(regles As IEnumerable(Of String))
        For Each regle As String In regles
            Add(New FiltreConformite(regle))
        Next
    End Sub

    Public Function Correspond(valeur As String) As Boolean Implements IFiltreConformite.Correspond
        'si aucun filtre n'à été configuré, on retourne automatiquement 'vrai'
        If Count = 0 Then Return True

        'si la valeur correspond à au moins une des règles, nous retournons 'vrai'
        For Each r As FiltreConformite In Me
            If r.Correspond(valeur) Then Return True
        Next

        'la liste n'est pas vide et la valeur ne correspond à aucune des règles configuré, on retourne 'faux'
        Return False
    End Function

End Class
