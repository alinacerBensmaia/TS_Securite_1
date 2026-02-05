''' <summary>
''' Représente un lien entre un rôle et une ressource pour les connecteurs.
''' </summary>
Public Class TsCdConnxRoleRessr
    Inherits TsCdElementConnexion

    ''' <summary>
    ''' Nom du rôle auquel la ressource doit (ou pas) être associée
    ''' </summary>
    Public NomRole As String

    ''' <summary>
    ''' Catégorie de la ressource, correspond à ResName2 dans Sage
    ''' </summary>
    Public CatgrRessource As String

    ''' <summary>
    ''' Nom de la ressource, correspond à ResName1 dans Sage
    ''' </summary>
    Public NomRessource As String

End Class
