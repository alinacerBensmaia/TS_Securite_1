
''' <summary>
''' La classe Workbook contient tous les éléments d'un livre de travail.
''' </summary>
''' <remarks></remarks>
Public Class TsCuWorkbook

#Region "--- Propriétés ---"

    Private mDocumentProperties As TsCuDocumentProperties
    ''' <summary>
    ''' Contient les informations reliés au document.
    ''' </summary>
    Public Property DocumentProperties() As TsCuDocumentProperties
        Get
            Return mDocumentProperties
        End Get
        Set(ByVal value As TsCuDocumentProperties)
            mDocumentProperties = value
        End Set
    End Property

    Private mExcelWorkbook As TsCuExcelWorkbook
    ''' <summary>
    ''' Informations pour Excel.
    ''' </summary>
    Public Property ExcelWorkbook() As TsCuExcelWorkbook
        Get
            Return mExcelWorkbook
        End Get
        Set(ByVal value As TsCuExcelWorkbook)
            mExcelWorkbook = Value
        End Set
    End Property

    Private mStyles As New List(Of TsCuStyle)
    ''' <summary>
    ''' Liste des styles reliés aux WorkSheet.
    ''' </summary>
    Public Property Styles() As List(Of TsCuStyle)
        Get
            Return mStyles
        End Get
        Set(ByVal value As List(Of TsCuStyle))
            mStyles = value
        End Set
    End Property

    Private mWorksheets As New List(Of TsCuWorksheet)
    ''' <summary>
    ''' Les feuilles de travail.
    ''' </summary>
    Public ReadOnly Property Worksheets() As List(Of TsCuWorksheet)
        Get
            Return mWorksheets
        End Get
    End Property

#End Region

#Region "--- Constructeurs ---"

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="pWorksheet">Une feuille de travail.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal pWorksheet As TsCuWorksheet)
        Worksheets.Add(pWorksheet)
    End Sub

    ''' <summary>
    ''' Constructeur avec plusieurs feuilles de travail.
    ''' </summary>
    ''' <param name="pWorksheets">Plusieurs feuilles de travail.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal pWorksheets As IEnumerable(Of TsCuWorksheet))
        Worksheets.AddRange(pWorksheets)
    End Sub

#End Region

#Region "--- Méthodes ---"

    ''' <summary>
    ''' Permet d'obtenir la valeur de l'objet en xml.
    ''' </summary>
    ''' <returns>Les xml de l'objet.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirXML() As String
        Dim balise = "<Workbook " & ObtenirSchema() & ">"
        If DocumentProperties IsNot Nothing Then balise &= DocumentProperties.ObtenirXML
        If ExcelWorkbook IsNot Nothing Then balise &= ExcelWorkbook.ObtenirXML()

        If Styles.Count = 0 Then
            balise &= "<Styles/>"
        Else
            balise &= "<Styles>"
            For Each s In Styles
                balise &= s.ObtenirXML()
            Next
            balise &= "</Styles>"
        End If

        For Each ws In Worksheets
            balise &= ws.ObtenirXML()
        Next

        balise &= "</Workbook>"
        Return balise
    End Function

#End Region

#Region "--- Fonctions privées ---"

    ''' <summary>
    ''' Permet d'obtenir les schémas relier à la balise. 
    ''' </summary>
    ''' <returns>Les schéma en version texte.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirSchema() As String
        Dim q As String = """"
        Dim schema As String = String.Format( _
            "xmlns={0}urn:schemas-microsoft-com:office:spreadsheet{0} " & _
            "xmlns:o={0}urn:schemas-microsoft-com:office:office{0} " & _
            "xmlns:x={0}urn:schemas-microsoft-com:office:excel{0} " & _
            "xmlns:ss={0}urn:schemas-microsoft-com:office:spreadsheet{0} " & _
            "xmlns:html={0}http://www.w3.org/TR/REC-html40{0}" _
            , q)

        Return schema
    End Function

#End Region

End Class
