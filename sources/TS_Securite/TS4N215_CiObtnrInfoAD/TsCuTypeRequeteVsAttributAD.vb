Imports System.Collections.Generic

Friend Class TsCuTypeRequeteVsAttributAD
    Inherits Dictionary(Of TS4N215_IObtnrInfoAD.TsIAccesseurADWCF.TsIadTypeRequete, String)

    Public Sub AjouterCombinaison(ByVal TypeRequete As TS4N215_IObtnrInfoAD.TsIAccesseurADWCF.TsIadTypeRequete, ByVal NomAttributAD As String)
        Add(TypeRequete, NomAttributAD)
    End Sub

    Public Function ObtenirCombinaison(ByVal TypeRequete As TS4N215_IObtnrInfoAD.TsIAccesseurADWCF.TsIadTypeRequete) As String
        Return Item(TypeRequete)
    End Function

End Class
