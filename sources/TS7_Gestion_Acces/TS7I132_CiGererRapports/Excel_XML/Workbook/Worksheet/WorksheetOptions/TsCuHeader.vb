Imports TS7I132_CiGererRapports.TsCuOutilsExcel

''' <summary>
''' Permet de définit l'entête de page d'une impression.
''' </summary>
''' <remarks></remarks>
Public Class TsCuHeader

    'x:Margin
    Private mMargin As Double
    ''' <summary>
    ''' Définit la marge d'une entête de page.
    ''' </summary>
    Public ReadOnly Property Margin() As Double
        Get
            Return mMargin
        End Get
    End Property

    'x:Data
    Private mData As String
    ''' <summary>
    ''' Texte d'une entête de page.
    ''' </summary>
    Public Property Data() As String
        Get
            Return mData
        End Get
        Set(ByVal value As String)
            mData = value
        End Set
    End Property

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="pMargin">La marge de l'entête de page.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal pMargin As Double)
        mMargin = pMargin
    End Sub

    ''' <summary>
    ''' Permet d'obtenir la version XML valide SpreadSheet.
    ''' </summary>
    ''' <returns>Un XML.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirXML() As String
        Dim balise As String = ""
        balise &= "<Header"
        balise &= ConstruireAttribut("x:Margin", Margin)
        balise &= ConstruireAttributOptionnel("x:Data", Data)
        balise &= "/>"

        Return balise
    End Function

End Class