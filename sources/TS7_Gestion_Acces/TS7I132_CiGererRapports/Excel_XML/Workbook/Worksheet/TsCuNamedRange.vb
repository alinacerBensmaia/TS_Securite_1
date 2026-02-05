Imports TS7I132_CiGererRapports.TsCuOutilsExcel

''' <summary>
''' Définit une zone nommée.
''' </summary>
''' <remarks></remarks>
Public Class TsCuNamedRange

    'ss:Name
    Private mName As String
    ''' <summary>
    ''' Le nom de la zone nommée.
    ''' </summary>
    Public Property Name() As String
        Get
            Return mName
        End Get
        Set(ByVal value As String)
            mName = value
        End Set
    End Property

    'ss:RefersTo
    Private mRefersTo As String
    ''' <summary>
    ''' À quoi réfère la zone.
    ''' </summary>
    Public Property RefersTo() As String
        Get
            Return mRefersTo
        End Get
        Set(ByVal value As String)
            mRefersTo = value
        End Set
    End Property

    'ss:Hidden
    Private mHidden As Boolean?
    ''' <summary>
    ''' Indique si la zone nommée est caché à l'utilisateur.
    ''' </summary>
    Public Property Hidden() As Boolean?
        Get
            Return mHidden
        End Get
        Set(ByVal value As Boolean?)
            mHidden = value
        End Set
    End Property

    ''' <summary>
    ''' Permet d'obtenir la version XML valide SpreadSheet.
    ''' </summary>
    ''' <returns>Un XML.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirXML() As String
        Dim balise As String = ""
        balise &= "<NamedRange"
        balise &= ConstruireAttribut("ss:Name", Name)
        balise &= ConstruireAttribut("ss:RefersTo", RefersTo)
        balise &= ConstruireAttributOptionnel("ss:Hidden", Hidden)
        balise &= "/>"

        Return balise
    End Function


End Class