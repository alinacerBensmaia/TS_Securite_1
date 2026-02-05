
''' <summary>
''' Permet de définir les options d'impression.
''' </summary>
''' <remarks></remarks>
Public Class TsCuPageSetup

#Region "--- Propriétés ---"

    'x:Footer
    Private mFooter As TsCuFooter
    ''' <summary>
    ''' Spécifie le bas de page de l'impression.
    ''' </summary>
    Public Property Footer() As TsCuFooter
        Get
            Return mFooter
        End Get
        Set(ByVal value As TsCuFooter)
            mFooter = value
        End Set
    End Property

    'x:Header
    Private mHeader As TsCuHeader
    ''' <summary>
    ''' Spécifie le l'entête de page de l'impression.
    ''' </summary>
    Public Property Header() As TsCuHeader
        Get
            Return mHeader
        End Get
        Set(ByVal value As TsCuHeader)
            mHeader = value
        End Set
    End Property

    'x:Layout
    Private mLayout As TsCuLayout
    ''' <summary>
    ''' Spécifie la mise en page de l'impression.
    ''' </summary>
    Public Property Layout() As TsCuLayout
        Get
            Return mLayout
        End Get
        Set(ByVal value As TsCuLayout)
            mLayout = value
        End Set
    End Property

    'x:PageMargins
    Private mPageMargins As TsCuPageMargins
    ''' <summary>
    ''' Définit les marge d'impression.
    ''' </summary>
    Public Property PageMargins() As TsCuPageMargins
        Get
            Return mPageMargins
        End Get
        Set(ByVal value As TsCuPageMargins)
            mPageMargins = value
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
        balise &= "<PageSetup"
        If Footer Is Nothing And Header Is Nothing And Layout Is Nothing And PageMargins Is Nothing Then
            balise &= "/>"
        Else
            balise &= ">"
        End If

        If Footer IsNot Nothing Then balise &= Footer.ObtenirXML()
        If Header IsNot Nothing Then balise &= Header.ObtenirXML()
        If Layout IsNot Nothing Then balise &= Layout.ObtenirXML()
        If PageMargins IsNot Nothing Then balise &= PageMargins.ObtenirXML()

        If Footer IsNot Nothing Or Header IsNot Nothing Or Layout IsNot Nothing Or PageMargins IsNot Nothing Then
            balise &= "</PageSetup>"
        End If

        Return balise
    End Function

#End Region

End Class