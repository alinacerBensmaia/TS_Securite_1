''' <summary>
''' Cette classe sert d'entrepot pour conservé des calcules qui prennent beaucoup de temps 
''' et qui sont lourd à gèrer indépendament.
''' </summary>
''' <remarks></remarks>
Public Class TsCdInformationsDiff

#Region "Public Vars"

    Public AjoutRoles As New List(Of TsCdConnxRole)
    Public SupprimerRoles As New List(Of TsCdConnxRole)

    Public AjoutUsers As New List(Of TsCdConnxUser)
    Public SupprimerUsers As New List(Of TsCdConnxUser)

    Public AjoutRessr As New List(Of TsCdConnxRessr)
    Public SupprimerRessr As New List(Of TsCdConnxRessr)

    Public AjoutAttrbRole As New List(Of TsCdConnxRoleAttrb)
    Public SupprimerAttrbRole As New List(Of TsCdConnxRoleAttrb)

    Public AjoutAttrbUser As New List(Of TsCdConnxUserAttrb)
    Public SupprimerAttrbUser As New List(Of TsCdConnxUserAttrb)

    Public AjoutAttrbRessr As New List(Of TsCdConnxRessrAttrb)
    Public SupprimerAttrbRessr As New List(Of TsCdConnxRessrAttrb)

    Public AjoutRoleRole As New List(Of TsCdConnxRoleRole)
    Public SupprimerRoleRole As New List(Of TsCdConnxRoleRole)

    Public AjoutRoleRessource As New List(Of TsCdConnxRoleRessr)
    Public SupprimerRoleRessource As New List(Of TsCdConnxRoleRessr)

    Public AjoutUtilisateurRole As New List(Of TsCdConnxUserRole)
    Public SupprimerUtilisateurRole As New List(Of TsCdConnxUserRole)

    Public AjoutUtilisateurRessource As New List(Of TsCdConnxUserRessr)
    Public SupprimerUtilisateurRessource As New List(Of TsCdConnxUserRessr)

    Public Connecteur As TsCuConnecteurCible = Nothing

#End Region

End Class
