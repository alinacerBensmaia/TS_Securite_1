

''' <summary>
''' Permet de définit le bas de page d'une impression.
''' </summary>
''' <remarks></remarks>
Public Class TsCuFooter

    'x:Margin
    Private mMargin As Double
    ''' <summary>
    ''' Définit la marge du bas de page.
    ''' </summary>
    Public ReadOnly Property Margin() As Double
        Get
            Return mMargin
        End Get
    End Property

    'x:Data
    Private mData As String = Nothing
    ''' <summary>
    ''' Texte de bas de page.
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
    ''' <param name="pMargin">La marge du bas de page.</param>
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
        balise &= "<Footer"
        balise &= TsCuOutilsExcel.ConstruireAttribut("x:Margin", Margin)
        balise &= TsCuOutilsExcel.ConstruireAttributOptionnel("x:Data", Data)
        balise &= "/>"

        Return balise
    End Function

End Class