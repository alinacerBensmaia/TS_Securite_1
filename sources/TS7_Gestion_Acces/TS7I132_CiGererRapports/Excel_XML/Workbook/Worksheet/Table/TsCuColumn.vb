Imports TS7I132_CiGererRapports.TsCuOutilsExcel

''' <summary>
''' Permet de définir une colonne dans la table.
''' </summary>
''' <remarks></remarks>
Public Class TsCuColumn

#Region "--- Propriétés ---"

    'c:Caption
    Private mCaption As String = Nothing
    ''' <summary>
    ''' Spécifie le libelé qui devrait apparaître quand le l'entête 
    ''' des colonnes et des lignes du composant par défaut affiche.
    ''' </summary>
    Public Property Caption() As String
        Get
            Return mCaption
        End Get
        Set(ByVal value As String)
            mCaption = value
        End Set
    End Property

    'ss:AutoFitWidth
    Private mAutoFitWidth As Boolean?
    ''' <summary>
    ''' Indique si la colonne devrait s'ajuster automatiquement sur la largeur de la date ou du numérique.
    ''' </summary>
    Public Property AutoFitWidth() As Boolean?
        Get
            Return mAutoFitWidth
        End Get
        Set(ByVal value As Boolean?)
            mAutoFitWidth = value
        End Set
    End Property

    'ss:Hidden
    Private mHidden As Boolean?
    ''' <summary>
    ''' Indique si la colonne doit être caché.
    ''' </summary>
    Public Property Hidden() As Boolean?
        Get
            Return mHidden
        End Get
        Set(ByVal value As Boolean?)
            mHidden = value
        End Set
    End Property

    'ss:Index
    Private mIndex As ULong?
    ''' <summary>
    ''' Spécifie la position de la colonne dans la table.
    ''' </summary>
    Public Property Index() As ULong?
        Get
            Return mIndex
        End Get
        Set(ByVal value As ULong?)
            mIndex = value
        End Set
    End Property

    'ss:Span
    Private mSpan As ULong?
    ''' <summary>
    ''' Indique sur combien de colonne adjacente le format de cette colonne s'étend.
    ''' </summary>
    Public Property Span() As ULong?
        Get
            Return mSpan
        End Get
        Set(ByVal value As ULong?)
            mSpan = value
        End Set
    End Property

    'ss:StyleID
    Private mStyleID As String = Nothing
    ''' <summary>
    ''' Indique quel style la colonne obtera par défaut.
    ''' </summary>
    Public Property StyleID() As String
        Get
            Return mStyleID
        End Get
        Set(ByVal value As String)
            mStyleID = value
        End Set
    End Property

    'ss:Width
    Private mWidth As Double?
    ''' <summary>
    ''' La largeur de la Colonne.
    ''' </summary>
    Public Property Width() As Double?
        Get
            Return mWidth
        End Get
        Set(ByVal value As Double?)
            mWidth = value
        End Set
    End Property

#End Region

#Region "--- Méthodes ---"

    ''' <summary>
    ''' Permet d'obtenir la version XML valide SpreadSheet.
    ''' </summary>
    ''' <returns>Un XML.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirXML() As String
        Dim balise As String = ""
        balise &= "<Column"
        balise &= ConstruireAttributOptionnel("c:Caption", Caption)
        balise &= ConstruireAttributOptionnel("ss:AutoFitWidth", AutoFitWidth)
        balise &= ConstruireAttributOptionnel("ss:Hidden", Hidden)
        balise &= ConstruireAttributOptionnel("ss:Index", Index)
        balise &= ConstruireAttributOptionnel("ss:Span", Span)
        balise &= ConstruireAttributOptionnel("ss:StyleID", StyleID)
        balise &= ConstruireAttributOptionnel("ss:Width", Width)
        balise &= "/>"

        Return balise
    End Function

#End Region

End Class