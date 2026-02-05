

''' <summary>
''' Permet de déifinir l'alignement d'une style.
''' </summary>
''' <remarks></remarks>
Public Class TsCuStyleAlignment

#Region "--- Énumérations ---"

    ''' <summary>
    ''' Définit les types d'alignement vertical disponibles.
    ''' </summary>
    Enum VerticalType
        Automatic
        Top
        Bottom
        Center
        Justify
        Distributed
        JustifyDistributed
    End Enum

    ''' <summary>
    ''' Définit les types d'alignement horizontal disponibles.
    ''' </summary>
    Enum HorizontalType
        Center
        CenterAcrossSelection
        Fill
        Justify
        Distributed
        JustifyDistributed
    End Enum

    ''' <summary>
    ''' Définit les types d'ordre de lecture disponibles.
    ''' </summary>
    Enum ReadingOrderType
        RightToLeft
        LeftToRight
        Context
    End Enum

#End Region

#Region "--- Propriétés ---"

    'ss:Horizontal
    Private mHorizontal As HorizontalType?
    ''' <summary>
    ''' Définit le style d'alignement horizontal.
    ''' </summary>
    Public Property Horizontal() As HorizontalType?
        Get
            Return mHorizontal
        End Get
        Set(ByVal value As HorizontalType?)
            mHorizontal = value
        End Set
    End Property

    'ss:Indent
    Private mIndent As ULong?
    ''' <summary>
    ''' Définit l'indentation d'une cellule.
    ''' </summary>
    Public Property Indent() As ULong?
        Get
            Return mIndent
        End Get
        Set(ByVal value As ULong?)
            mIndent = value
        End Set
    End Property

    'ss:ReadingOrder
    Private mReadingOrder As ReadingOrderType?
    ''' <summary>
    ''' Permet de définir le sens de la lecture d'une texte.
    ''' </summary>
    Public Property ReadingOrder() As ReadingOrderType?
        Get
            Return mReadingOrder
        End Get
        Set(ByVal value As ReadingOrderType?)
            mReadingOrder = value
        End Set
    End Property

    'ss:Rotate
    Private mRotate As Double?
    ''' <summary>
    ''' Définit la rotation à appliquer à au texte.
    ''' </summary>
    Public Property Rotate() As Double?
        Get
            Return mRotate
        End Get
        Set(ByVal value As Double?)
            mRotate = value
        End Set
    End Property

    'ss:ShrinkToFit
    Private mShrinkToFit As Boolean?
    ''' <summary>
    ''' Détermine si le texte doit rétrécir pour entrer dans la cellule.
    ''' </summary>
    Public Property ShrinkToFit() As Boolean?
        Get
            Return mShrinkToFit
        End Get
        Set(ByVal value As Boolean?)
            mShrinkToFit = value
        End Set
    End Property

    'ss:Vertical
    Private mVertical As VerticalType?
    ''' <summary>
    ''' Définit le style d'alignement vertical.
    ''' </summary>
    Public Property Vertical() As VerticalType?
        Get
            Return mVertical
        End Get
        Set(ByVal value As VerticalType?)
            mVertical = value
        End Set
    End Property

    'ss:VerticalText
    Private mVerticalText As Boolean?
    ''' <summary>
    ''' Définit si le texte est vertical.
    ''' </summary>
    Public Property VerticalText() As Boolean?
        Get
            Return mVerticalText
        End Get
        Set(ByVal value As Boolean?)
            mVerticalText = value
        End Set
    End Property

    'ss:WrapText
    Private mWrapText As Boolean?
    ''' <summary>
    ''' Définit si le texte d'une cellule doit resté dans sa cellule,
    ''' ou si faux, s'étendre sur les cellules vides adjacentes.
    ''' </summary>
    Public Property WrapText() As Boolean?
        Get
            Return mWrapText
        End Get
        Set(ByVal value As Boolean?)
            mWrapText = value
        End Set
    End Property

#End Region

#Region "--- Méthodes ---"

    ''' <summary>
    ''' Permet d'obtenir la version XML valide SpreadSheet de l'Alignement.
    ''' </summary>
    ''' <returns>Un XML.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirXML() As String
        Dim balise As String = ""
        balise &= "<Alignment"
        balise &= TsCuOutilsExcel.ConstruireAttributOptionnel("ss:Horizontal", Horizontal)
        balise &= TsCuOutilsExcel.ConstruireAttributOptionnel("ss:Indent", Indent)
        balise &= TsCuOutilsExcel.ConstruireAttributOptionnel("ss:ReadingOrder", ReadingOrder)
        balise &= TsCuOutilsExcel.ConstruireAttributOptionnel("ss:Rotate", Rotate)
        balise &= TsCuOutilsExcel.ConstruireAttributOptionnel("ss:ShrinkToFit", ShrinkToFit)
        balise &= TsCuOutilsExcel.ConstruireAttributOptionnel("ss:Vertical", Vertical)
        balise &= TsCuOutilsExcel.ConstruireAttributOptionnel("ss:VerticalText", VerticalText)
        balise &= TsCuOutilsExcel.ConstruireAttributOptionnel("ss:WrapText", WrapText)
        balise &= "/>"

        Return balise
    End Function

#End Region

End Class