''' <summary>
''' Source spécifique à la feuille principale.
''' </summary>
''' <remarks></remarks>
Friend Class TsDtSourcFeuilPrinc
    Inherits TsDtSourceRapport


#Region "--- Propriétés ---"

    ''' <summary>
    ''' La feuille principale est dépendante de la présence de la deuxième feuille.
    ''' </summary>
    ''' <remarks></remarks>
    Public Property PresenceFeuille2 As Boolean

#End Region

#Region "--- Constructeurs ---"

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="pDate">Date de production.</param>
    ''' <param name="pLstUaDemander">Liste des unitées administrative demandées.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal pDate As Date, ByVal pLstUaDemander As List(Of String))
        MyBase.New(pDate, pLstUaDemander)
    End Sub

#End Region

End Class
