

''' <summary>
''' Permet de définir le format des cellules d'un Style.
''' </summary>
''' <remarks></remarks>
Public Class TsCuStyleNumberFormat

#Region "--- Énumérations ---"

    ''' <summary>
    ''' Format disponible.
    ''' </summary>
    Enum FormatType
        General
        GeneralNumber
        GeneralDate
        LongDate
        MediumDate
        ShortDate
        LongTime
        MediumTime
        ShortTime
        Currency
        EuroCurrency
        Fixed
        Standard
        Percent
        Scientific
        Yes_No
        True_False
        On_Off
    End Enum

#End Region

#Region "--- Propriétés ---"

    'ss:Format
    Private mFormat As FormatType?
    ''' <summary>
    ''' Définit le format des cellules d'un style.
    ''' </summary>
    Public Property Format() As FormatType?
        Get
            Return mFormat
        End Get
        Set(ByVal value As FormatType?)
            mFormat = value
        End Set
    End Property

#End Region

#Region "--- Méthodes ---"

    ''' <summary>
    ''' Permet d'obtenir la version XML valide SpreadSheet du format de la cellule.
    ''' </summary>
    ''' <returns>Un XML.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirXML() As String
        Dim balise As String = ""
        balise &= "<NumberFormat"
        balise &= TsCuOutilsExcel.ConstruireAttributOptionnel("ss:Format", Format)
        balise &= "/>"

        Return balise
    End Function

#End Region

#Region "--- Fonctions privées---"

    ''' <summary>
    ''' Permet d'obtenir les valeurs texte du Enum FormatType.
    ''' </summary>
    ''' <param name="pFormat">Le format du Enum.</param>
    ''' <returns>La valeur texte du format type.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirValeurFormatType(ByVal pFormat As FormatType?) As String
        If pFormat.HasValue = False Then Return Nothing

        Select Case pFormat
            Case FormatType.EuroCurrency
                Return "Euro Currency"
            Case FormatType.GeneralDate
                Return "General Date"
            Case FormatType.GeneralNumber
                Return "General Number"
            Case FormatType.LongDate
                Return "Long Date"
            Case FormatType.LongTime
                Return "Long Time"
            Case FormatType.MediumDate
                Return "Medium Date"
            Case FormatType.MediumTime
                Return "Medium Time"
            Case FormatType.On_Off
                Return "On/Off"
            Case FormatType.ShortDate
                Return "Short Date"
            Case FormatType.ShortTime
                Return "Short Time"
            Case FormatType.True_False
                Return "True/False"
            Case FormatType.Yes_No
                Return "Yes/No"
            Case Else
                Return pFormat.ToString()
        End Select
    End Function

#End Region

End Class
