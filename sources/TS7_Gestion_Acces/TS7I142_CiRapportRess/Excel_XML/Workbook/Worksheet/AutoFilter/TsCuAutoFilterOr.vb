
''' <summary>
''' Condition Ou d'un filtre conditionnel.
''' </summary>
''' <remarks></remarks>
Public Class TsCuAutoFilterOr

    'x:AutoFilterCondition
    Private mAutoFilterCondition As TsCuAutoFilterCondition
    ''' <summary>
    ''' Permet d'émettre une condition sur le filtre de la colonne.
    ''' </summary>
    Public ReadOnly Property AutoFilterCondition() As TsCuAutoFilterCondition
        Get
            Return mAutoFilterCondition
        End Get
    End Property

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="pAutoFilterCondition">Condition du filtre conditonnel Ou.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal pAutoFilterCondition As TsCuAutoFilterCondition)
        mAutoFilterCondition = pAutoFilterCondition
    End Sub

    ''' <summary>
    ''' Permet d'obtenir la version XML valide SpreadSheet.
    ''' </summary>
    ''' <returns>Un XML.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirXML() As String
        Dim balise As String = ""
        balise &= "<AutoFilterOr>"

        balise &= AutoFilterCondition.ObtenirXML()

        balise &= "</AutoFilterOr>"

        Return balise
    End Function

End Class
