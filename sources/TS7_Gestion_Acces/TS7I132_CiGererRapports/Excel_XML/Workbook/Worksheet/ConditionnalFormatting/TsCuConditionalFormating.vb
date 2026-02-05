Imports TS7I132_CiGererRapports.TsCuOutilsExcel

''' <summary>
''' Permet de changé le format de célules dépendament de la valeur de cellules.
''' </summary>
''' <remarks></remarks>
Public Class TsCuConditionalFormatting

    ' Format RxCy:RxCy, RxCy:RxCy
    Private mRange As String
    ''' <summary>
    ''' Indque l'étendu de la zone affecté.
    ''' Format attendu: RxCy[:RxCy][,RxCy[:RxCy]][...]
    ''' </summary>
    Public ReadOnly Property Range() As String
        Get
            Return mRange
        End Get
    End Property

    Private mCondition As TsCuCondition
    ''' <summary>
    ''' Condition pour appliquer le formatage conditionné.
    ''' </summary>
    Public ReadOnly Property Condition() As TsCuCondition
        Get
            Return mCondition
        End Get
    End Property

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="pRange">La zone affecté.</param>
    ''' <param name="pCondition">La condition pour affecté la zone.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal pRange As String, ByVal pCondition As TsCuCondition)
        mRange = pRange
        mCondition = pCondition
    End Sub

    ''' <summary>
    ''' Permet d'obtenir la version XML.
    ''' </summary>
    ''' <returns>Un XML.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirXML() As String
        Dim xmlns As String = "urn:schemas-microsoft-com:office:excel"

        Dim balise As String = ""
        balise &= "<ConditionalFormatting"
        balise &= ConstruireAttribut("xmlns", xmlns)
        balise &= ">"

        balise &= "<Range>"
        balise &= Range
        balise &= "</Range>"

        balise &= Condition.ObtenirXML()

        balise &= "</ConditionalFormatting>"

        Return balise
    End Function

End Class