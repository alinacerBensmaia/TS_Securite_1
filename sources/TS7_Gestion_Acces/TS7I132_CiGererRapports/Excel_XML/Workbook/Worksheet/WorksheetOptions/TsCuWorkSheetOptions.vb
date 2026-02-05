
''' <summary>
''' Définit les opitons de la feuille de travail spécifique à Excel.
''' </summary>
''' <remarks></remarks>
Public Class TsCuWorkSheetOptions
    Private xmlns As String = "urn:schemas-microsoft-com:office:excel"

#Region "--- Propriétés ---"

    'x:PageSetup
    Private mPageSetup As TsCuPageSetup
    ''' <summary>
    ''' Contient les options d'impression du workBook.
    ''' </summary>
    Public Property PageSetup() As TsCuPageSetup
        Get
            Return mPageSetup
        End Get
        Set(ByVal value As TsCuPageSetup)
            mPageSetup = value
        End Set
    End Property

    Private mPrint As TsCuPrint
    ''' <summary>
    ''' Permet d'ajuster les paramètre d'impression.
    ''' </summary>
    ''' <remarks>Attention! Ne fait pas partis des standards, donc possiblement incompatible avec les vieilles versions d'Excel.</remarks>
    Public Property Print() As TsCuPrint
        Get
            Return mPrint
        End Get
        Set(ByVal value As TsCuPrint)
            mPrint = value
        End Set
    End Property

    Private mFreezePanes As Boolean
    ''' <summary>
    ''' Permet de geler les colonnes et les lignes. Dépendant de SplitHorizontal et SplitVertical.
    ''' </summary>
    ''' <remarks>Attention! Ne fait pas partis des standards, donc possiblement incompatible avec les vieilles versions d'Excel.</remarks>
    Public Property FreezePanes() As Boolean
        Get
            Return mFreezePanes
        End Get
        Set(ByVal value As Boolean)
            mFreezePanes = value
        End Set
    End Property

    Private mSplitHorizontal As Integer?
    ''' <summary>
    ''' Permet de séparer la fenêtre en deux à l'horizontal à la hauteur de la ligne X.
    ''' </summary>
    ''' <remarks>Attention! Ne fait pas partis des standards, donc possiblement incompatible avec les vieilles versions d'Excel.</remarks>
    Public Property SplitHorizontal() As Integer?
        Get
            Return mSplitHorizontal
        End Get
        Set(ByVal value As Integer?)
            mSplitHorizontal = value
        End Set
    End Property

    Private mSplitVertical As Integer?
    ''' <summary>
    ''' Permet de séparer la fenêtre en deux à la vertical à la distance de X colonnes.
    ''' </summary>
    ''' <remarks>Attention! Ne fait pas partis des standards, donc possiblement incompatible avec les vieilles versions d'Excel.</remarks>
    Public Property SlipVertical() As Integer?
        Get
            Return mSplitVertical
        End Get
        Set(ByVal value As Integer?)
            mSplitVertical = value
        End Set
    End Property

    Private mActivePane As Integer?
    ''' <summary>
    ''' Indique quel fraction de fenêtre est connecté au barres défilantes.
    ''' </summary>
    ''' <remarks>Attention! Ne fait pas partis des standards, donc possiblement incompatible avec les vieilles versions d'Excel.</remarks>
    Public Property ActivePane() As Integer?
        Get
            Return mActivePane
        End Get
        Set(ByVal value As Integer?)
            mActivePane = value
        End Set
    End Property

    Private mTopRowBottomPane As Integer?
    ''' <summary>
    ''' Permet d'ajuster les limites en haut de la fenêtre fractionné active.
    ''' </summary>
    ''' <remarks>Attention! Ne fait pas partis des standards, donc possiblement incompatible avec les vieilles versions d'Excel.</remarks>
    Public Property TopRowBottomPane() As Integer?
        Get
            Return mTopRowBottomPane
        End Get
        Set(ByVal value As Integer?)
            mTopRowBottomPane = value
        End Set
    End Property

    Private mLeftColumnRightPane As Integer?
    ''' <summary>
    ''' Permet d'ajuster les limites à gauche de la fenêtre fractionné active.
    ''' </summary>
    ''' <remarks>Attention! Ne fait pas partis des standards, donc possiblement incompatible avec les vieilles versions d'Excel.</remarks>
    Public Property LeftColumnRightPane() As Integer?
        Get
            Return mLeftColumnRightPane
        End Get
        Set(ByVal value As Integer?)
            mLeftColumnRightPane = value
        End Set
    End Property

    Private mFitToPage As Boolean
    ''' <summary>
    ''' Indique que les propriétés FitWidth et FitHeight doivent être pris en compte.
    ''' </summary>
    ''' <remarks>Attention! Ne fait pas partis des standards, donc possiblement incompatible avec les vieilles versions d'Excel.</remarks>
    Public Property FitToPage() As Boolean
        Get
            Return mFitToPage
        End Get
        Set(ByVal value As Boolean)
            mFitToPage = value
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
        If PageSetup IsNot Nothing _
           Or mPrint IsNot Nothing _
           Or mFreezePanes = True _
           Or mActivePane.HasValue _
           Or mSplitHorizontal.HasValue _
           Or mSplitVertical.HasValue _
           Or mLeftColumnRightPane.HasValue _
           Or mTopRowBottomPane.HasValue _
           Or mFitToPage = True _
        Then
            balise &= "<WorksheetOptions"
            balise &= " xmlns=""urn:schemas-microsoft-com:office:excel"""
            balise &= ">"

            If mPageSetup IsNot Nothing Then
                balise &= mPageSetup.ObtenirXML()
            End If

            If mPrint IsNot Nothing Then
                balise &= mPrint.ObtenirXML()
            End If

            If mFreezePanes = True Then
                balise &= "<FreezePanes/>"
            End If

            If mActivePane.HasValue Then
                balise &= String.Format("<ActivePane>{0}</ActivePane>", mActivePane)
            End If

            If mSplitHorizontal.HasValue Then
                balise &= String.Format("<SplitHorizontal>{0}</SplitHorizontal>", mSplitHorizontal)
            End If

            If mSplitVertical.HasValue Then
                balise &= String.Format("<SplitVertical>{0}</SplitVertical>", mSplitVertical)
            End If

            If mTopRowBottomPane.HasValue Then
                balise &= String.Format("<TopRowBottomPane>{0}</TopRowBottomPane>", mTopRowBottomPane)
            End If

            If mLeftColumnRightPane.HasValue Then
                balise &= String.Format("<LeftColumnRightPane>{0}</LeftColumnRightPane>", mLeftColumnRightPane)
            End If

            If mFitToPage = True Then
                balise &= "<FitToPage/>"
            End If

            balise &= "</WorksheetOptions>"
        Else
            balise &= "/>"
        End If

        Return balise
    End Function

#End Region

End Class
