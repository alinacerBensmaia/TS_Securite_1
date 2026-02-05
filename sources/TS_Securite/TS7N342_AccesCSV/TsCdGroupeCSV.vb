''' <summary>
''' Cette classe sert à conserver les informations sur les Groupes d'un fichier CSV.
''' </summary>
''' <remarks></remarks>
Public Class TsCdGroupeCSV
    Public Id As String
    Public Membres As HashSet(Of String)

    Public Sub New(ByVal gid As String)
        Id = gid
        Membres = New HashSet(Of String)()
    End Sub
End Class
