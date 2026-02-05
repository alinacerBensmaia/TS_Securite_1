Friend Class TsCuTypeRequeteVsAttributAD

    Private _DicTypeRequeteVsAttribut As New Generic.Dictionary(Of TsIadTypeRequete, String)

    Public Sub AjouterCombinaison(ByVal TypeRequete As TsIadTypeRequete, ByVal NomAttributAD As String)
        _DicTypeRequeteVsAttribut.Add(TypeRequete, NomAttributAD)
    End Sub

    Public Function ObtenirCombinaison(ByVal TypeRequete As TsIadTypeRequete) As String
        Return _DicTypeRequeteVsAttribut.Item(TypeRequete)
    End Function

End Class
