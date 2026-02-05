Imports TS7I132_CiGererRapports.TsCuOutilsExcel

''' <summary>
''' Permet de définir le style d'un texte.
''' </summary>
''' <remarks></remarks>
Public Class TsCuStyleFont

#Region "--- Énumérations ---"

    ''' <summary>
    ''' Définit l'épaisseur du soulignement.
    ''' </summary>
    Enum UnderlineType
        None
        [Single]
        [Double]
        SingleAccounting
        DoubleAccounting
    End Enum

    ''' <summary>
    ''' Définit le type d'alignement vertical.
    ''' </summary>
    Enum VerticalAlignType
        None
        Subscript
        Superscript
    End Enum

    ''' <summary>
    ''' Définit le type de famille.
    ''' </summary>
    Enum FamilyType
        Automatic
        Decorative
        Modern
        Roman
        Script
        Swiss
    End Enum

#End Region

#Region "--- Propriétés ---"

    'ss:Bold
    Private mBold As Boolean?
    ''' <summary>
    ''' Définit si le texte est en gras ou pas.
    ''' </summary>
    Public Property Bold() As Boolean?
        Get
            Return mBold
        End Get
        Set(ByVal value As Boolean?)
            mBold = value
        End Set
    End Property

    'ss:Color
    Private mColor As String = Nothing
    ''' <summary>
    ''' Définit la couleur du texte.
    ''' Code de couleur RVB: "#rrvvbb".
    ''' La valeur "Automatic" est accepté.
    ''' </summary>
    Public Property Color() As String
        Get
            Return mColor
        End Get
        Set(ByVal value As String)
            mColor = value
        End Set
    End Property

    'ss:FontName
    Private mFontName As String = Nothing
    ''' <summary>
    ''' Nom de la police de caractère.
    ''' </summary>
    Public Property FontName() As String
        Get
            Return mFontName
        End Get
        Set(ByVal value As String)
            mFontName = value
        End Set
    End Property

    'ss:Italic
    Private mItalic As Boolean?
    ''' <summary>
    ''' Définit si le texte est en italique.
    ''' </summary>
    Public Property Italic() As Boolean?
        Get
            Return mItalic
        End Get
        Set(ByVal value As Boolean?)
            mItalic = value
        End Set
    End Property

    'ss:Outline
    Private mOutline As Boolean?
    ''' <summary>
    ''' Définit si le texte a un contour.
    ''' </summary>
    ''' <remarks>Spécifique à Mac.</remarks>
    Public Property Outline() As Boolean?
        Get
            Return mOutline
        End Get
        Set(ByVal value As Boolean?)
            mOutline = value
        End Set
    End Property

    'ss:Shadow
    Private mShadow As Boolean?
    ''' <summary>
    ''' Définit si le texte à une ombre.
    ''' </summary>
    ''' <remarks>Spécifique à Mac.</remarks>
    Public Property Shadow() As Boolean?
        Get
            Return mShadow
        End Get
        Set(ByVal value As Boolean?)
            mShadow = value
        End Set
    End Property

    'ss:Size
    Private mSize As Double?
    ''' <summary>
    ''' Définit la grosseur du texte.
    ''' </summary>
    Public Property Size() As Double?
        Get
            Return mSize
        End Get
        Set(ByVal value As Double?)
            mSize = value
        End Set
    End Property

    'ss:StrikeThrough
    Private mStrikeThrough As Boolean?
    ''' <summary>
    ''' Permet de barrer un texte.
    ''' </summary>
    Public Property StrikeThrough() As Boolean?
        Get
            Return mStrikeThrough
        End Get
        Set(ByVal value As Boolean?)
            mStrikeThrough = value
        End Set
    End Property

    'ss:Underline
    Private mUnderline As UnderlineType?
    ''' <summary>
    ''' Permet de souligner un texte.
    ''' </summary>
    Public Property Underline() As UnderlineType?
        Get
            Return mUnderline
        End Get
        Set(ByVal value As UnderlineType?)
            mUnderline = value
        End Set
    End Property

    'ss:VerticalAlign
    Private mVerticalAlign As VerticalAlignType?
    ''' <summary>
    ''' Permet d'aligner le texte dans le sens de la vertical.
    ''' </summary>
    Public Property VerticalAlign() As VerticalAlignType?
        Get
            Return mVerticalAlign
        End Get
        Set(ByVal value As VerticalAlignType?)
            mVerticalAlign = value
        End Set
    End Property

    'x:CharSet
    Private mCharSet As ULong?
    ''' <summary>
    ''' Définit le jeu de caractère de Win32.
    ''' </summary>
    Public Property CharSet() As ULong?
        Get
            Return mCharSet
        End Get
        Set(ByVal value As ULong?)
            mCharSet = value
        End Set
    End Property

    'x:Family
    Private mFamily As FamilyType?
    ''' <summary>
    ''' Indique un style dépendant Win32 d'écriture.
    ''' </summary>
    Public Property Family() As FamilyType?
        Get
            Return mFamily
        End Get
        Set(ByVal value As FamilyType?)
            mFamily = value
        End Set
    End Property

#End Region

#Region "--- Méthodes ---"

    ''' <summary>
    ''' Permet d'obtenir la version XML valide SpreadSheet de la police.
    ''' </summary>
    ''' <returns>Un XML.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirXML() As String
        Dim balise As String = ""
        balise &= "<Font"
        balise &= ConstruireAttributOptionnel("ss:Bold", Bold)
        balise &= ConstruireAttributOptionnel("ss:Color", Color)
        balise &= ConstruireAttributOptionnel("ss:FontName", FontName)
        balise &= ConstruireAttributOptionnel("ss:Italic", Italic)
        balise &= ConstruireAttributOptionnel("ss:Outline", Outline)
        balise &= ConstruireAttributOptionnel("ss:Outline", Outline)
        balise &= ConstruireAttributOptionnel("ss:Shadow", Shadow)
        balise &= ConstruireAttributOptionnel("ss:Size", Size)
        balise &= ConstruireAttributOptionnel("ss:StrikeThrough", StrikeThrough)
        balise &= ConstruireAttributOptionnel("ss:Underline", Underline)
        balise &= ConstruireAttributOptionnel("ss:VerticalAlign", VerticalAlign)
        balise &= ConstruireAttributOptionnel("x:CharSet", CharSet)
        balise &= ConstruireAttributOptionnel("x:Family", Family)
        balise &= "/>"

        Return balise
    End Function

#End Region

End Class
