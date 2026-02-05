Imports TS7I132_CiGererRapports.TsCuOutilsExcel

''' <summary>
''' Définit une ligne dans la table.
''' </summary>
''' <remarks></remarks>
Public Class TsCuRow

#Region "--- Propriétés ---"

    'c:Caption
    Private mCaption As String = Nothing
    ''' <summary>
    ''' Spécifie le libelé qui devrait apparaître quand le l'entête 
    ''' des colonnes et des lignes du composant par défaut affiche.
    ''' </summary>
    Public Property Caption() As String
        Get
            Return mCaption
        End Get
        Set(ByVal value As String)
            mCaption = value
        End Set
    End Property

    'ss:AutoFitWidth
    Private mAutoFitHeight As Boolean?
    ''' <summary>
    ''' Indique si la ligne devrait s'ajuster automatiquement sur la hauteur de la date ou du numérique.
    ''' </summary>
    Public Property AutoFitHeight() As Boolean?
        Get
            Return mAutoFitHeight
        End Get
        Set(ByVal value As Boolean?)
            mAutoFitHeight = value
        End Set
    End Property

    'ss:Hidden
    Private mHidden As Boolean?
    ''' <summary>
    ''' Indique si la colonne doit être caché.
    ''' </summary>
    Public Property Hidden() As Boolean?
        Get
            Return mHidden
        End Get
        Set(ByVal value As Boolean?)
            mHidden = value
        End Set
    End Property

    'ss:Index
    Private mIndex As ULong?
    ''' <summary>
    ''' Spécifie la position de la colonne dans la table.
    ''' </summary>
    Public Property Index() As ULong?
        Get
            Return mIndex
        End Get
        Set(ByVal value As ULong?)
            mIndex = value
        End Set
    End Property

    'ss:Span
    Private mSpan As ULong?
    ''' <summary>
    ''' Indique sur combien de ligne adjacente le format de cette ligne s'étend.
    ''' </summary>
    Public Property Span() As ULong?
        Get
            Return mSpan
        End Get
        Set(ByVal value As ULong?)
            mSpan = value
        End Set
    End Property

    'ss:StyleID
    Private mStyleID As String = Nothing
    ''' <summary>
    ''' Indique quel style la ligne obtera par défaut.
    ''' </summary>
    Public Property StyleID() As String
        Get
            Return mStyleID
        End Get
        Set(ByVal value As String)
            mStyleID = value
        End Set
    End Property

    'ss:Width
    Private mHeight As Double?
    ''' <summary>
    ''' La largeur de la ligne.
    ''' </summary>
    Public Property Height() As Double?
        Get
            Return mHeight
        End Get
        Set(ByVal value As Double?)
            mHeight = value
        End Set
    End Property

    Private mCells As New List(Of TsCuCell)
    ''' <summary>
    ''' Liste de cellules.
    ''' </summary>
    Public Property Cells() As List(Of TsCuCell)
        Get
            Return mCells
        End Get
        Set(ByVal value As List(Of TsCuCell))
            mCells = value
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
        balise &= "<Row"
        balise &= ConstruireAttributOptionnel("c:Caption", Caption)
        balise &= ConstruireAttributOptionnel("ss:AutoFitHeight", AutoFitHeight)
        balise &= ConstruireAttributOptionnel("ss:Hidden", Hidden)
        balise &= ConstruireAttributOptionnel("ss:Index", Index)
        balise &= ConstruireAttributOptionnel("ss:Span", Span)
        balise &= ConstruireAttributOptionnel("ss:StyleID", StyleID)
        balise &= ConstruireAttributOptionnel("ss:Height", Height)
        If Cells.Count = 0 Then
            balise &= "/>"
        Else
            balise &= ">"
        End If

        For Each c In Cells
            balise &= c.ObtenirXML()
        Next

        If Cells.Count <> 0 Then
            balise &= "</Row>"
        End If
        Return balise
    End Function

#End Region

End Class