

''' <summary>
''' Permet de définir une table dans une Worksheet.
''' </summary>
''' <remarks></remarks>
Public Class TsCuTable

#Region "--- Propriétés ---"

    'ss:DefaultColumnWidth
    Private mDefaultColumnWidth As Double?
    ''' <summary>
    ''' Valeur par défaut de la largeur d'une colonne.
    ''' </summary>
    Public Property DefaultColumnWidth() As Double?
        Get
            Return mDefaultColumnWidth
        End Get
        Set(ByVal value As Double?)
            mDefaultColumnWidth = value
        End Set
    End Property

    'ss:DefaultRowHeight
    Private mDefaultRowHeight As Double?
    ''' <summary>
    ''' Valeur par défaut de la hauteur d'une ligne.
    ''' </summary>
    Public Property DefaultRowHeight() As Double?
        Get
            Return mDefaultRowHeight
        End Get
        Set(ByVal value As Double?)
            mDefaultRowHeight = value
        End Set
    End Property

    'ss:ExpandedColumnCount
    Private mExpandedColumnCount As ULong?
    ''' <summary>
    ''' Indique le nombre de colonne utilisé par la table.
    ''' </summary>
    Public Property ExpandedColumnCount() As ULong?
        Get
            Return mExpandedColumnCount
        End Get
        Set(ByVal value As ULong?)
            mExpandedColumnCount = value
        End Set
    End Property

    'ss:ExpandedRowCount
    Private mExpandedRowCount As ULong?
    ''' <summary>
    ''' Indique le nombre de ligne que la table utilise.
    ''' </summary>
    Public Property ExpandedRowCount() As ULong?
        Get
            Return mExpandedRowCount
        End Get
        Set(ByVal value As ULong?)
            mExpandedRowCount = value
        End Set
    End Property

    'ss:LeftCell
    Private mLeftCell As ULong?
    ''' <summary>
    ''' Spécifie l'index de la colonne à laquelle la table devrait commencer.
    ''' </summary>
    Public Property LeftCell() As ULong?
        Get
            Return mLeftCell
        End Get
        Set(ByVal value As ULong?)
            mLeftCell = value
        End Set
    End Property

    'ss:StyleID
    Private mStyleID As String = Nothing
    ''' <summary>
    ''' Indique quel style la table obtera par défaut.
    ''' </summary>
    Public Property StyleID() As String
        Get
            Return mStyleID
        End Get
        Set(ByVal value As String)
            mStyleID = value
        End Set
    End Property

    'ss:TopCell
    Private mTopCell As String = Nothing
    ''' <summary>
    ''' Spécifie l'index de la ligne à laquelle la table devrait commencer.
    ''' </summary>
    Public Property TopCell() As String
        Get
            Return mTopCell
        End Get
        Set(ByVal value As String)
            mTopCell = value
        End Set
    End Property

    'x:FullColumns
    Private mFullColumns As Boolean?
    ''' <summary>
    ''' Indique si les informations présentées sont la totalité des données dans les colonnes.
    ''' </summary>
    Public Property FullColumns() As Boolean?
        Get
            Return mFullColumns
        End Get
        Set(ByVal value As Boolean?)
            mFullColumns = value
        End Set
    End Property

    'x:FullRows 
    Private mFullRows As Boolean?
    ''' <summary>
    ''' Indique si les informations présentées sont la totalité des données dans les ligne.
    ''' </summary>
    Public Property FullRows() As Boolean?
        Get
            Return mFullRows
        End Get
        Set(ByVal value As Boolean?)
            mFullRows = value
        End Set
    End Property

    'ss:Column
    Private mColumns As New List(Of TsCuColumn)
    ''' <summary>
    ''' Contient les colonnes de la table.
    ''' </summary>
    Public Property Columns() As List(Of TsCuColumn)
        Get
            Return mColumns
        End Get
        Set(ByVal value As List(Of TsCuColumn))
            mColumns = value
        End Set
    End Property

    Private mRows As New List(Of TsCuRow)
    ''' <summary>
    ''' Contient les lignes de la table.
    ''' </summary>
    Public Property Rows() As List(Of TsCuRow)
        Get
            Return mRows
        End Get
        Set(ByVal value As List(Of TsCuRow))
            mRows = value
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
        balise &= "<Table"
        balise &= TsCuOutilsExcel.ConstruireAttributOptionnel("ss:DefaultColumnWidth", DefaultColumnWidth)
        balise &= TsCuOutilsExcel.ConstruireAttributOptionnel("ss:DefaultRowHeight", DefaultRowHeight)
        balise &= TsCuOutilsExcel.ConstruireAttributOptionnel("ss:ExpandedColumnCount", ExpandedColumnCount)
        balise &= TsCuOutilsExcel.ConstruireAttributOptionnel("ss:ExpandedRowCount", ExpandedRowCount)
        balise &= TsCuOutilsExcel.ConstruireAttributOptionnel("ss:LeftCell", LeftCell)
        balise &= TsCuOutilsExcel.ConstruireAttributOptionnel("ss:StyleID", StyleID)
        balise &= TsCuOutilsExcel.ConstruireAttributOptionnel("ss:TopCell", TopCell)
        balise &= TsCuOutilsExcel.ConstruireAttributOptionnel("x:FullColumns", FullColumns)
        balise &= TsCuOutilsExcel.ConstruireAttributOptionnel("x:FullRows", FullRows)
        If Columns.Count = 0 And Rows.Count = 0 Then
            balise &= "/>"
        Else
            balise &= ">"
        End If

        For Each c In Columns
            balise &= c.ObtenirXML()
        Next

        For Each r In Rows
            balise &= r.ObtenirXML()
        Next

        If Columns.Count <> 0 Or Rows.Count <> 0 Then
            balise &= "</Table>"
        End If

        Return balise
    End Function

#End Region

End Class
