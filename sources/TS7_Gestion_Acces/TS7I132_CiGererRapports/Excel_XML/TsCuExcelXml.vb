
''' <summary>
''' Classe de têtes. Cette classe permet d'écrire un XML d'un Excel.
''' </summary>
''' <remarks></remarks>
Public Class TsCuExcelXml

    Private mWorkBook As TsCuWorkbook
    ''' <summary>
    ''' Le livre de travail.
    ''' </summary>
    Public ReadOnly Property WorkBook() As TsCuWorkbook
        Get
            Return mWorkBook
        End Get
    End Property

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="pWorkBook">Le livre de travail.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal pWorkBook As TsCuWorkbook)
        mWorkBook = pWorkBook
    End Sub

    ''' <summary>
    ''' Permet de récupérer un fichier Excel en XML prêt à être utilisé.
    ''' </summary>
    ''' <returns>Un XML prêt à l'utilisation.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirXML() As String
        Dim q As String = """"
        Dim balise As String = ""

        balise &= String.Format("<?xml version={0}1.0{0}?>", q)
        balise &= String.Format("<?mso-application progid={0}Excel.Sheet{0}?>", q)

        balise &= WorkBook.ObtenirXML()

        Return balise
    End Function

End Class
