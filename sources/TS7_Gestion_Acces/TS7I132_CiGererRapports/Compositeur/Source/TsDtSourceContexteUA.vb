
''' <summary>
''' Définition d'un contexte d'unité administrative de la source du rapport.
''' </summary>
''' <remarks></remarks>
Public Class TsDtSourceContexteUA

    Private mTitre As String
    ''' <summary>
    ''' Titre du contexte d'Unité Administrative.
    ''' </summary>
    Public Property Titre() As String
        Get
            Return mTitre
        End Get
        Set(ByVal value As String)
            mTitre = value
        End Set
    End Property

    Private mSymbole As String
    ''' <summary>
    ''' Symbole du contexte d'Unité Administrative.
    ''' </summary>
    Public Property Symbole() As String
        Get
            Return mSymbole
        End Get
        Set(ByVal value As String)
            mSymbole = value
        End Set
    End Property

End Class
