''' <summary>
''' Classe de données. Cette classe sert à entreposé des informations sur les utilisateurs de TSS.
''' </summary>
''' <remarks></remarks>
Public Class TsCdUtilisateurTSS
    Public AccessorID As String
    Public Profiles As List(Of String)

    Public Sub New()
        AccessorID = ""
        Profiles = New List(Of String)()
    End Sub
End Class
