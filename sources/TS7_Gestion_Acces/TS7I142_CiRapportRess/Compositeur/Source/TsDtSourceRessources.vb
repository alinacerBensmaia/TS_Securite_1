''' <summary>
''' Définition d'un employé de la source du rapport.
''' </summary>
''' <remarks></remarks>
Friend Class TsDtSourceRessources

#Region "--- Propriétés ---"

    Private mNom As String
    ''' <summary>
    ''' Nom de la ressource
    ''' </summary>
    Public Property Nom() As String
        Get
            Return mNom
        End Get
        Set(ByVal value As String)
            mNom = value
        End Set
    End Property

    Private mNomFonctionnel As String
    ''' <summary>
    ''' Nom de la ressource
    ''' </summary>
    Public Property NomFonctionnel() As String
        Get
            Return mNomFonctionnel
        End Get
        Set(ByVal value As String)
            mNomFonctionnel = value
        End Set
    End Property

#End Region
    Public Sub New(ByVal pNom As String, ByVal pNomFonctionnel As String)
        Nom = pNom
        NomFonctionnel = pNomFonctionnel
    End Sub

End Class
