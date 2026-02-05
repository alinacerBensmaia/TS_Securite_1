''' <summary>
''' Représente un lien entre un utilisateur et un rôle pour les connecteurs.
''' </summary>
Public Class TsCdConnxUserRole
    Inherits TsCdElementConnexion

    ''' <summary>
    ''' Code de l'utilisateur qui doit (ou pas) être associé au rôle.
    ''' </summary>
    Public CodeUtilisateur As String

    ''' <summary>
    ''' Rôle auquel doit (ou pas) être associé l'utilisateur.
    ''' </summary>
    Public NomRole As String

    ''' <summary>
    ''' Date à laquelle l'association rôle-utilisateur doit se terminer.
    ''' </summary>
    ''' <remarks>
    ''' Une date nulle signifie qu'il n'y a pas de fin prévue à l'association.
    ''' Un système cible peut généralement ignorer cette valeur s'il ne supporte pas la notion de fin.
    ''' </remarks>
    Public DateFin As Nullable(Of Date)

End Class
