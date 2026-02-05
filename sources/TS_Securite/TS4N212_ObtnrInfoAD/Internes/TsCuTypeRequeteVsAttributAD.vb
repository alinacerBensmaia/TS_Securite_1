Imports System.Collections.Generic

Friend Class TsCuTypeRequeteVsAttributAD
    Inherits Dictionary(Of TsIadTypeRequete, String)

    Public Sub AjouterCombinaison(ByVal TypeRequete As TsIadTypeRequete, ByVal NomAttributAD As String)
        Add(TypeRequete, NomAttributAD)
    End Sub

    Public Function ObtenirCombinaison(ByVal TypeRequete As TsIadTypeRequete) As String
        Return Item(TypeRequete)
    End Function

End Class
