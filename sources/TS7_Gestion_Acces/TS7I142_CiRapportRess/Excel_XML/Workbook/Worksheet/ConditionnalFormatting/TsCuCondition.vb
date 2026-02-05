
''' <summary>
''' Condition pour d'une format conditionné.
''' </summary>
''' <remarks></remarks>
Public Class TsCuCondition

    Private mValues As New List(Of String)
    ''' <summary>
    ''' Les valeurs pour évaluer la conditon.
    ''' On s'attend à avoir des formules Excel.
    ''' </summary>
    Public ReadOnly Property Values() As List(Of String)
        Get
            Return mValues
        End Get
    End Property

    Private mFormat As TsCuFormat
    ''' <summary>
    ''' Format à appliquer.
    ''' </summary>
    Public ReadOnly Property Format() As TsCuFormat
        Get
            Return mFormat
        End Get
    End Property

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="pValue">Première valeur de la condition.</param>
    ''' <param name="pFormat">Format à appliquer.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal pValue As String, ByVal pFormat As TsCuFormat)
        mValues.Add(pValue)
        mFormat = pFormat
    End Sub

    ''' <summary>
    ''' Constructeur de base avec valeur multiple.
    ''' </summary>
    ''' <param name="pValues">Multiple valeurs de la condition.</param>
    ''' <param name="pFormat">Format à appliquer.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal pValues As List(Of String), ByVal pFormat As TsCuFormat)
        mValues.AddRange(pValues)
        mFormat = pFormat
    End Sub

    ''' <summary>
    ''' Permet d'obtenir la version XML.
    ''' </summary>
    ''' <returns>Un XML.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirXML() As String
        Dim xmlns As String = "urn:schemas-microsoft-com:office:excel"

        Dim balise As String = ""
        balise &= "<Condition>"

        Dim i As Integer = 1
        For Each v In Values
            balise &= String.Format("<Value{0}>", i)
            balise &= v
            balise &= String.Format("</Value{0}>", i)
            i += 1
        Next

        balise &= Format.ObtenirXML()

        balise &= "</Condition>"

        Return balise
    End Function

End Class