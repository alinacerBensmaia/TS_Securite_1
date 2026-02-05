

''' <summary>
''' Une feuille de travail. Une feuille égale un onglet dans Excel.
''' </summary>
''' <remarks></remarks>
Public Class TsCuWorksheet

#Region "--- Propriétés ---"

    'ss:Name
    Private mName As String
    ''' <summary>
    ''' Nom de la feuille de travail.
    ''' </summary>
    Public ReadOnly Property Name() As String
        Get
            Return mName
        End Get
    End Property

    'ss:Protected
    Private mProtected As Boolean?
    ''' <summary>
    ''' Définit si la feuille de travail est protégé. 
    ''' Doit être activé pour que les cellules soit protégé.
    ''' </summary>
    Public Property [Protected]() As Boolean?
        Get
            Return mProtected
        End Get
        Set(ByVal value As Boolean?)
            mProtected = value
        End Set
    End Property

    'ss:RightToLeft
    Private mRightToLeft As Boolean?
    ''' <summary>
    ''' Permet d'affichier la feuille de travail de droite à gauche.
    ''' </summary>
    Public Property RightToLeft() As Boolean?
        Get
            Return mRightToLeft
        End Get
        Set(ByVal value As Boolean?)
            mRightToLeft = value
        End Set
    End Property

    'ss:Names
    Private mNames As New List(Of TsCuNamedRange)
    ''' <summary>
    ''' Définit des noms de section.
    ''' </summary>
    Public Property Names() As List(Of TsCuNamedRange)
        Get
            Return mNames
        End Get
        Set(ByVal value As List(Of TsCuNamedRange))
            mNames = value
        End Set
    End Property

    'ss:Table
    Private mTable As TsCuTable
    ''' <summary>
    ''' Définition de la table des cellules.
    ''' </summary>
    Public Property Table() As TsCuTable
        Get
            Return mTable
        End Get
        Set(ByVal value As TsCuTable)
            mTable = value
        End Set
    End Property

    'x:AutoFiller
    Private mAutoFiller As TsCuAutoFilter
    ''' <summary>
    ''' Définit un filtre automatique dans la feuille de travail.
    ''' </summary>
    Public Property AutoFiller() As TsCuAutoFilter
        Get
            Return mAutoFiller
        End Get
        Set(ByVal value As TsCuAutoFilter)
            mAutoFiller = value
        End Set
    End Property

    'x:WorksheetOptions
    Private mWorksheetOptions As TsCuWorkSheetOptions
    ''' <summary>
    ''' Opiton de la feuille de travail spécifique à Excel.
    ''' </summary>
    Public Property WorksheetOptions() As TsCuWorkSheetOptions
        Get
            Return mWorksheetOptions
        End Get
        Set(ByVal value As TsCuWorkSheetOptions)
            mWorksheetOptions = value
        End Set
    End Property

    Private mConditionnalFormattings As New List(Of TsCuConditionalFormatting)
    ''' <summary>
    ''' Liste de format conditonnés.
    ''' </summary>
    Public Property ConditionnalFormattings() As List(Of TsCuConditionalFormatting)
        Get
            Return mConditionnalFormattings
        End Get
        Set(ByVal value As List(Of TsCuConditionalFormatting))
            mConditionnalFormattings = value
        End Set
    End Property

#End Region

#Region "--- Constrcuteurs ---"

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="pName">Le nom de la feuille de travail.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal pName As String)
        mName = pName
    End Sub

#End Region

#Region "--- Méthodes ---"

    ''' <summary>
    ''' Permet d'obtenir la version XML valide SpreadSheet.
    ''' </summary>
    ''' <returns>Un XML.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirXML() As String
        Dim balise As String = ""
        balise &= "<Worksheet"
        balise &= TsCuOutilsExcel.ConstruireAttribut("ss:Name", Name)
        balise &= TsCuOutilsExcel.ConstruireAttributOptionnel("ss:Protected", [Protected])
        balise &= TsCuOutilsExcel.ConstruireAttributOptionnel("ss:RightToLeft", RightToLeft)
        balise &= ">"

        If Names.Count = 0 Then
            balise &= "<Names/>"
        Else
            balise &= "<Names>"
            For Each n In Names
                balise &= n.ObtenirXML
            Next
            balise &= "</Names>"
        End If

        If Table Is Nothing Then
            balise &= "<Table/>"
        Else
            balise &= Table.ObtenirXML()
        End If

        If WorksheetOptions Is Nothing Then
            balise &= "<WorksheetOptions/>"
        Else
            balise &= WorksheetOptions.ObtenirXML()
        End If

        If AutoFiller Is Nothing Then
            balise &= "<AutoFiller/>"
        Else
            balise &= AutoFiller.ObtenirXML()
        End If

        For Each c In ConditionnalFormattings
            balise &= c.ObtenirXML()
        Next

        balise &= "</Worksheet>"

        Return balise
    End Function

#End Region

End Class
