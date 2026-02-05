Imports TS7I132_CiGererRapports.TsCuOutilsExcel

''' <summary>
''' Permet de définir une bordure d'un style.
''' </summary>
''' <remarks></remarks>
Public Class TsCuStyleBorder

#Region "--- Énumérations ---"

    ''' <summary>
    ''' Position disponible.
    ''' </summary>
    Enum PositionType
        Bottom
        Left
        Right
        Top
        DiagonalLeft
        DiagonalRight
    End Enum

    ''' <summary>
    ''' Définit le style de la ligne.
    ''' </summary>
    Enum LineStyleType
        None
        Continuous
        Dash
        Dot
        DashDot
        DashDotDot
        SlantDashDot
        [Double]
    End Enum

#End Region

#Region "--- Propriétés ---"

    'ss:Position
    Private mPosition As PositionType
    ''' <summary>
    ''' Position de la bordure.
    ''' </summary>
    Public ReadOnly Property Position() As PositionType
        Get
            Return mPosition
        End Get
    End Property

    'ss:Color
    Private mColor As String = Nothing
    ''' <summary>
    ''' Couleur de la bordure.
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

    'ss:LineStyle
    Private mLineStyle As LineStyleType?
    ''' <summary>
    ''' Le style de la ligne.
    ''' </summary>
    Public Property LineStyle() As LineStyleType?
        Get
            Return mLineStyle
        End Get
        Set(ByVal value As LineStyleType?)
            mLineStyle = value
        End Set
    End Property

    'ss:Weight
    Private mWeight As Double?
    ''' <summary>
    ''' L'épaisseur de la ligne.
    ''' </summary>
    Public Property Weight() As Double?
        Get
            Return mWeight
        End Get
        Set(ByVal value As Double?)
            mWeight = value
        End Set
    End Property

#End Region

#Region "--- Constructeurs ---"

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="pPosition">La position de la bordure.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal pPosition As PositionType)
        mPosition = pPosition
    End Sub

#End Region

#Region "--- Méthodes ---"

    ''' <summary>
    ''' Permet d'obtenir la version XML valide SpreadSheet de la bordure.
    ''' </summary>
    ''' <returns>Un XML.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirXML() As String
        Dim balise As String = ""
        balise &= "<Border"
        balise &= ConstruireAttribut("ss:Position", Position)
        balise &= ConstruireAttributOptionnel("ss:Color", Color)
        balise &= ConstruireAttributOptionnel("ss:LineStyle", LineStyle)
        balise &= ConstruireAttributOptionnel("ss:Weight", Weight)
        balise &= "/>"

        Return balise
    End Function

#End Region

End Class