Imports TS7I132_CiGererRapports.TsCuOutilsExcel

''' <summary>
''' Condition sur un filtre conditionnel.
''' </summary>
''' <remarks></remarks>
Public Class TsCuAutoFilterCondition

    ''' <summary>
    ''' Type d'opérateur conditionnel.
    ''' </summary>
    Enum OperatorType
        Equals
        DoesNotEqual
        GreaterThan
        GreaterThanOrEqual
        LessThan
        LessThanOrEqual
    End Enum

    'x:Operator
    Private mOperator As OperatorType
    ''' <summary>
    ''' L'opérateur de la condition.
    ''' </summary>
    Public ReadOnly Property [Operator]() As OperatorType
        Get
            Return mOperator
        End Get
    End Property

    'X:Value
    Private mValue As String
    ''' <summary>
    ''' La valeur évalué par l'opérateur.
    ''' </summary>
    Public ReadOnly Property Value() As String
        Get
            Return mValue
        End Get
    End Property

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="pOperator">Type d'opérateur.</param>
    ''' <param name="pValue">Valeur à évaluer.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal pOperator As OperatorType, ByVal pValue As String)
        mOperator = pOperator
        mValue = pValue
    End Sub

    ''' <summary>
    ''' Permet d'obtenir la version XML valide SpreadSheet.
    ''' </summary>
    ''' <returns>Un XML.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirXML() As String
        Dim balise As String = ""
        balise &= "<AutoFilterCondition"
        balise &= ConstruireAttributOptionnel("x:Operator", [Operator])
        balise &= ConstruireAttributOptionnel("x:Value", Value)
        balise &= "/>"

        Return balise
    End Function

End Class
