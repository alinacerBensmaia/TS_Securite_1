Imports TS7I132_CiGererRapports.TsCuOutilsExcel
''' <summary>
''' Permet de définir la mise en page de l'impression.
''' </summary>
''' <remarks></remarks>
Public Class TsCuLayout

#Region "--- Énumérations ---"

    ''' <summary>
    ''' Définition destype d'orientation.
    ''' </summary>
    ''' <remarks></remarks>
    Enum OrientationType
        Portrait
        Landscape
    End Enum

#End Region

#Region "--- Propriétés ---"

    'x:CenterHorizontal
    Private mCenterHorizontal As Boolean?
    ''' <summary>
    ''' Centrer la page à l'horizontal.
    ''' </summary>
    Public Property CenterHorizontal() As Boolean?
        Get
            Return mCenterHorizontal
        End Get
        Set(ByVal value As Boolean?)
            mCenterHorizontal = value
        End Set
    End Property

    'x:CenterVertical
    Private mCenterVertical As Boolean?
    ''' <summary>
    ''' Centrer la page à la vertical.
    ''' </summary>
    Public Property CenterVertical() As Boolean?
        Get
            Return mCenterVertical
        End Get
        Set(ByVal value As Boolean?)
            mCenterVertical = value
        End Set
    End Property

    'x:Orientation
    Private mOrientation As OrientationType?
    ''' <summary>
    ''' Orientation de la page d'impression.
    ''' </summary>
    Public Property Orientation() As OrientationType?
        Get
            Return mOrientation
        End Get
        Set(ByVal value As OrientationType?)
            mOrientation = value
        End Set
    End Property

    'x:StartPageNumber
    Private mStartPageNumber As ULong?
    ''' <summary>
    ''' À quelle page doit commencer l'impression.
    ''' </summary>
    Public Property StartPageNumber() As ULong?
        Get
            Return mStartPageNumber
        End Get
        Set(ByVal value As ULong?)
            mStartPageNumber = value
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
        balise &= "<Layout"
        balise &= ConstruireAttributOptionnel("x:CenterHorizontal", CenterHorizontal)
        balise &= ConstruireAttributOptionnel("x:CenterVertical", CenterVertical)
        balise &= ConstruireAttributOptionnel("x:Orientation", Orientation)
        balise &= ConstruireAttributOptionnel("x:StartPageNumber", StartPageNumber)
        balise &= "/>"

        Return balise
    End Function

#End Region

End Class