''' <summary>
''' Permet de définir des paramètre d'impression.
''' </summary>
''' <remarks>
''' Attention! Cette classe n'est pas stadardisé par Excel ou 
''' par les standards SpreadSheet. 
''' Peut ne pas fonctionné avec toutes les versions d'Excel.
''' Ne possède pas toutes les variables disponibles pour la classe.
''' </remarks>
Public Class TsCuPrint

#Region "--- Propriétés ---"

    Private mPaperSizeIndex As Integer?
    ''' <summary>
    ''' Définit le style d'impression de la page.
    ''' </summary>
    Public Property PaperSizeIndex() As Integer?
        Get
            Return mPaperSizeIndex
        End Get
        Set(ByVal value As Integer?)
            mPaperSizeIndex = value
        End Set
    End Property

    Private mFitWidth As Integer?
    ''' <summary>
    ''' La page d'impresion doit s'ajusté à la largeur de X pages.
    ''' </summary>
    Public Property FitWidth() As Integer?
        Get
            Return mFitWidth
        End Get
        Set(ByVal value As Integer?)
            mFitWidth = value
        End Set
    End Property

    Private mFitHeight As Integer?
    ''' <summary>
    ''' La page d'impresion doit s'ajusté à la hauteur de X pages.
    ''' </summary>
    Public Property FitHeight() As Integer?
        Get
            Return mFitHeight
        End Get
        Set(ByVal value As Integer?)
            mFitHeight = value
        End Set
    End Property

    Private mLeftToRight As Boolean
    ''' <summary>
    ''' Ajuste l'ordre des page à imprimer. De gauche à droite ou de bas en haut.
    ''' </summary>
    Public Property LeftToRight() As Boolean
        Get
            Return mLeftToRight
        End Get
        Set(ByVal value As Boolean)
            mLeftToRight = value
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

        If mPaperSizeIndex.HasValue _
           Or mFitHeight.HasValue _
           Or mFitWidth.HasValue _
           Or LeftToRight = True _
        Then
            balise &= "<Print>"
            If mPaperSizeIndex.HasValue Then balise &= String.Format("<PaperSizeIndex>{0}</PaperSizeIndex>", mPaperSizeIndex)
            If mFitWidth.HasValue Then balise &= String.Format("<FitWidth>{0}</FitWidth>", mFitWidth)
            If mFitHeight.HasValue Then balise &= String.Format("<FitHeight>{0}</FitHeight>", mFitHeight)
            If mLeftToRight = True Then balise &= "<LeftToRight/>"
            balise &= "</Print>"
        Else
            balise &= "<Print/>"
        End If

        Return balise
    End Function

#End Region

End Class
