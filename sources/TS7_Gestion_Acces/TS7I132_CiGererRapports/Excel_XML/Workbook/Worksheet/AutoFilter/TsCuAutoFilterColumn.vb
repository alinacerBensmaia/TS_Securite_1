Imports TS7I132_CiGererRapports.TsCuOutilsExcel

''' <summary>
''' Filtre sur une colonne.
''' </summary>
''' <remarks></remarks>
Public Class TsCuAutoFilterColumn

#Region "--- Énumérations ---"

    ''' <summary>
    ''' Type de type.
    ''' </summary>
    Enum TypeType
        All
        Blanks
        NonBlanks
        Top
        TopPercent
        Bottom
        BottomPercent
        Custom
    End Enum

#End Region

#Region "--- Propriétés ---"

    'x:Hidden
    Private mHidden As Boolean?
    ''' <summary>
    ''' Définit si la zone colonne filtrer est caché.
    ''' </summary>
    Public Property Hidden() As Boolean?
        Get
            Return mHidden
        End Get
        Set(ByVal value As Boolean?)
            mHidden = value
        End Set
    End Property

    'x:Index
    Private mIndex As ULong?
    ''' <summary>
    ''' Index dans les colonnes filtrées.
    ''' </summary>
    Public Property Index() As ULong?
        Get
            Return mIndex
        End Get
        Set(ByVal value As ULong?)
            mIndex = value
        End Set
    End Property

    'x:Type
    Private mType As TypeType?
    ''' <summary>
    ''' Définit le type de filtre de colonne.
    ''' </summary>
    Public Property Type() As TypeType?
        Get
            Return mType
        End Get
        Set(ByVal value As TypeType?)
            mType = value
        End Set
    End Property

    'x:Value
    Private mValue As Double?
    ''' <summary>
    ''' En conjonction avec le Type, si le Type requière une valeur.
    ''' </summary>
    Public Property Value() As Double?
        Get
            Return mValue
        End Get
        Set(ByVal value As Double?)
            mValue = value
        End Set
    End Property

    'x:AutoFilterAnd
    Private mAutoFilterAnd As TsCuAutoFilterAnd
    ''' <summary>
    ''' Contient une condition Et sur le filtre de colonne.
    ''' </summary>
    Public Property AutoFilterAnd() As TsCuAutoFilterAnd
        Get
            Return mAutoFilterAnd
        End Get
        Set(ByVal value As TsCuAutoFilterAnd)
            mAutoFilterAnd = value
        End Set
    End Property

    'x:AutoFilterCondition
    Private mAutoFilterCondition As TsCuAutoFilterCondition
    ''' <summary>
    ''' Permet d'émettre une condition sur le filtre de la colonne.
    ''' </summary>
    Public Property AutoFilterCondition() As TsCuAutoFilterCondition
        Get
            Return mAutoFilterCondition
        End Get
        Set(ByVal value As TsCuAutoFilterCondition)
            mAutoFilterCondition = value
        End Set
    End Property

    'x:AutoFilterAnd
    Private mAutoFilterOr As TsCuAutoFilterOr
    ''' <summary>
    ''' Contient une condition Ou sur le filtre de colonne.
    ''' </summary>
    Public Property AutoFilterOr() As TsCuAutoFilterOr
        Get
            Return mAutoFilterOr
        End Get
        Set(ByVal value As TsCuAutoFilterOr)
            mAutoFilterOr = value
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
        balise &= "<AutoFilterColumn"
        balise &= ConstruireAttributOptionnel("x:Hidden", Hidden)
        balise &= ConstruireAttributOptionnel("x:Index", Index)
        balise &= ConstruireAttributOptionnel("x:Type", Type)
        balise &= ConstruireAttributOptionnel("x:Value", Value)
        If AutoFilterAnd Is Nothing And AutoFilterOr Is Nothing And AutoFilterCondition Is Nothing Then
            balise &= "/>"
        Else
            balise &= ">"
        End If

        If AutoFilterAnd IsNot Nothing Then AutoFilterAnd.ObtenirXML()
        If AutoFilterCondition IsNot Nothing Then AutoFilterCondition.ObtenirXML()
        If AutoFilterOr IsNot Nothing Then AutoFilterOr.ObtenirXML()

        If AutoFilterAnd IsNot Nothing Or AutoFilterOr IsNot Nothing Or AutoFilterCondition IsNot Nothing Then
            balise &= "</AutoFilterColumn>"
        End If

        Return balise
    End Function

#End Region

End Class
