Imports System.Runtime.CompilerServices

Friend Class Curseur
    Implements IDisposable
    Private _ctrl As Control
    Private _curseurPrecedent As Cursor

    Public Sub New(ctrl As Control, nouveauCurseur As Cursor)
        _ctrl = ctrl
        _curseurPrecedent = ctrl.Cursor
        ctrl.Cursor = nouveauCurseur
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        _ctrl.Cursor = _curseurPrecedent
    End Sub
End Class

Friend Module CursorExtensions

    <Extension>
    Public Function ChangerCurseur(source As Control, nouveauCurseur As Cursor) As IDisposable
        Return New Curseur(source, nouveauCurseur)
    End Function

End Module