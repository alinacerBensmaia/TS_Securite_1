''' <summary>
''' Représente un lien entre deux rôles pour les connecteurs.
''' </summary>
Public Class TsCdConnxRoleRole
    Inherits TsCdElementConnexion

    ''' <summary>
    ''' Sous-rôle, celui auquel sont associés des privilèges communs.
    ''' </summary>
    Public NomSousRole As String

    ''' <summary>
    ''' Rôle supérieur, qui hérite des privilèges du sous-rôle.
    ''' </summary>
    Public NomRoleSup As String

End Class
