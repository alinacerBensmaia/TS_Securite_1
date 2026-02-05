' Classe de base pour toutes les collections d'objets Sage
Public Class TsCdCollectionSage(Of T)
    Implements IEnumerable(Of T)

    Protected _list As New List(Of T)

    Public Sub Add(ByVal cfg As T)
        _list.Add(cfg)
    End Sub

    Public Sub AddRange(ByVal collections As IEnumerable(Of T))
        _list.AddRange(collections)
    End Sub

    Public Function GetEnumerator() As System.Collections.Generic.IEnumerator(Of T) Implements System.Collections.Generic.IEnumerable(Of T).GetEnumerator
        Return _list.GetEnumerator()
    End Function

    Private Function GetEnumerator1() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return Me.GetEnumerator
    End Function

    Public ReadOnly Property List() As List(Of T)
        Get
            Return _list
        End Get
    End Property

End Class

