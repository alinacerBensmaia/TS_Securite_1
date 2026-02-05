''' <summary>
''' Représente un attribut d'élément pour les connecteurs.
''' </summary>
Public MustInherit Class TsCdConnxAttrb
    Inherits TsCdElementConnexion

    ' Correspond à la propriété "format.date.display" situé dans le fichier "eurekify.properties" qui gère le format des
    '       dates retournés par les services webs de SAGE.
    ' Correspond aussi a la constante TsBaConfigSage.FORMAT_DATE_TOSTRING du module TS7N243_AccesSage
    Public Const FORMAT_DATE_TOSTRING As String = "yyyy-MM-dd"

    ''' <summary>
    ''' Nom de l'attribut.
    ''' </summary>
    Public NomAttrb As String

    ''' <summary>
    ''' La valeur de l'attribut.
    ''' </summary>
    Public Valeur As String

End Class
