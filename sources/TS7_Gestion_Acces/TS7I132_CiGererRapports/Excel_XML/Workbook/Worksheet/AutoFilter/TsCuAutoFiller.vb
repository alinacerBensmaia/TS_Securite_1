Imports TS7I132_CiGererRapports.TsCuOutilsExcel

''' <summary>
''' Zone de filtre automatique.
''' </summary>
''' <remarks></remarks>
Public Class TsCuAutoFilter

    'x:Range
    Private mRange As String
    ''' <summary>
    ''' Spécifie la zone filtré. Doit être en format RxCy.
    ''' </summary>
    Public ReadOnly Property Range() As String
        Get
            Return mRange
        End Get
    End Property

    'x:AutoFilterColumn
    Private mAutoFilterColumns As New List(Of TsCuAutoFilterColumn)
    ''' <summary>
    ''' Filtre automatique sur une colonne.
    ''' </summary>
    Public Property AutoFilterColumns() As List(Of TsCuAutoFilterColumn)
        Get
            Return mAutoFilterColumns
        End Get
        Set(ByVal value As List(Of TsCuAutoFilterColumn))
            mAutoFilterColumns = value
        End Set
    End Property

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="pRange">Zone filtré. Format attendu: RxCy.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal pRange As String)
        mRange = pRange
    End Sub

#Region "--- Méthodes ---"

    ''' <summary>
    ''' Permet d'obtenir la version XML valide SpreadSheet.
    ''' </summary>
    ''' <returns>Un XML.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirXML() As String
        Dim balise As String = ""
        balise &= "<AutoFilter"
        balise &= ConstruireAttribut("x:Range", Range)
        If AutoFilterColumns.Count = 0 Then
            balise &= "/>"
        Else
            balise &= ">"
        End If

        If AutoFilterColumns.Count <> 0 Then
            For Each afc In AutoFilterColumns
                balise &= afc.ObtenirXML()
            Next
            balise &= "</AutoFilter>"
        End If

        Return balise
    End Function

#End Region

End Class
