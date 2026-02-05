Imports TS7I132_CiGererRapports.TsCuOutilsExcel

''' <summary>
''' Permet à un NamedRange de s'associé à un NamedCell.
''' </summary>
''' <remarks></remarks>
Public Class TsCuNamedCell

    'ss:Name
    Private mName As String
    ''' <summary>
    ''' Nom associé à un NamedRange.
    ''' </summary>
    Public ReadOnly Property Name() As String
        Get
            Return mName
        End Get
    End Property

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="pName">Nom associé à un NamedRange.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal pName As String)
        mName = pName
    End Sub

    ''' <summary>
    ''' Permet d'obtenir la version XML valide SpreadSheet.
    ''' </summary>
    ''' <returns>Un XML.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirXML() As String
        Dim balise As String = ""
        balise &= "<NamedCell"
        balise &= ConstruireAttribut("ss:Name", Name)
        balise &= "/>"

        Return balise
    End Function


End Class