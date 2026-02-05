''' <summary>
''' Représente un rôle pour les connecteurs.
''' </summary>
Public Class TsCdConnxRole
    Inherits TsCdElementConnexion

    ''' <summary>
    ''' Nom du rôle (nom technique, qui suit la nomenclature).
    ''' </summary>
    ''' <remarks>
    ''' Certains systèmes cibles peuvent avoir des raisons techniques qui les empêche de suivre la nomenclature des
    ''' rôle. Dans ce cas ils peuvent transformer les noms de rôles pour les adapter au système cible,
    ''' mais au niveau de l'interface publique ça doit toujours être le nom officiel qui est véhiculé.
    ''' </remarks>
    Public NomRole As String

End Class
