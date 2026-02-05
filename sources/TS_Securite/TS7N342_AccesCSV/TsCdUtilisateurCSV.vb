''' <summary>
''' Cette classe sert à conserver les informations sur les utilisateurs de TSS.
''' </summary>
''' <remarks></remarks>
Public Class TsCdUtilisateurCSV
    Public Id As String
    Public Groupes As HashSet(Of String)

    Public Sub New(ByVal uid As String)
        Id = uid
        Groupes = New HashSet(Of String)()
    End Sub
End Class
