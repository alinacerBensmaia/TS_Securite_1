Imports TS7I132_CiGererRapports.TsCuOutilsExcel

''' <summary>
''' Permet de définir les marges de l'impression.
''' </summary>
''' <remarks></remarks>
Public Class TsCuPageMargins

#Region "--- Propriétés ---"

    'x:Bottom
    Private mBottom As Double?
    ''' <summary>
    ''' Définition de la marge du bas.
    ''' </summary>
    Public Property Bottom() As Double?
        Get
            Return mBottom
        End Get
        Set(ByVal value As Double?)
            mBottom = value
        End Set
    End Property

    'x:Left
    Private mLeft As Double?
    ''' <summary>
    ''' Définition de la marge de gauche.
    ''' </summary>
    Public Property Left() As Double?
        Get
            Return mLeft
        End Get
        Set(ByVal value As Double?)
            mLeft = value
        End Set
    End Property

    'x:Right
    Private mRight As Double?
    ''' <summary>
    ''' Définition de la marge de droite.
    ''' </summary>
    Public Property Right() As Double?
        Get
            Return mRight
        End Get
        Set(ByVal value As Double?)
            mRight = value
        End Set
    End Property

    'x:Top
    Private mTop As Double?
    ''' <summary>
    ''' Définition de la marge du haut.
    ''' </summary>
    Public Property Top() As Double?
        Get
            Return mTop
        End Get
        Set(ByVal value As Double?)
            mTop = value
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
        balise &= "<PageMargins"
        balise &= ConstruireAttributOptionnel("x:Bottom", Bottom)
        balise &= ConstruireAttributOptionnel("x:Left", Left)
        balise &= ConstruireAttributOptionnel("x:Right", Right)
        balise &= ConstruireAttributOptionnel("x:Top", Top)
        balise &= "/>"

        Return balise
    End Function

#End Region

End Class