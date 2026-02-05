
''' <summary>
''' Définition d'une Unité administrative(UA) associé à un employé.
''' </summary>
''' <remarks></remarks>
Friend Class TsDtSourceUaAssociee

    Private mTitre As String
    ''' <summary>
    ''' Titre de l'Unité Administrative(UA) associée.
    ''' </summary>
    Public Property Titre() As String
        Get
            Return mTitre
        End Get
        Set(ByVal value As String)
            mTitre = value
        End Set
    End Property

    Private mValeurAssociee As String
    ''' <summary>
    ''' Si le rôle est associé sans contexte, il vaut "X", sinon la valeur sera la valeur du contexte.
    ''' </summary>
    Public Property ValeurAssociee() As String
        Get
            Return mValeurAssociee
        End Get
        Set(ByVal value As String)
            mValeurAssociee = value
        End Set
    End Property

    ''' <summary>
    ''' Permet de savoir si l'association est étrangère à l'unité administrative de l'employé.
    ''' </summary>
    ''' <remarks></remarks>
    Public Property ValeurAssocieeEtrangere As Boolean = False

End Class
