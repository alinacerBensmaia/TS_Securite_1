Imports TS7I132_CiGererRapports.TsCuOutilsExcel

''' <summary>
''' Format à appliquer.
''' </summary>
''' <remarks></remarks>
Public Class TsCuFormat

    Private mStyle As String
    ''' <summary>
    ''' Le style à appliquer.
    ''' </summary>
    Public ReadOnly Property Style() As String
        Get
            Return mStyle
        End Get
    End Property

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="pStyle">Le style à appliquer</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal pStyle As String)
        mStyle = pStyle
    End Sub

    ''' <summary>
    ''' Permet d'obtenir la version XML.
    ''' </summary>
    ''' <returns>Un XML.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirXML() As String
        Dim xmlns As String = "urn:schemas-microsoft-com:office:excel"

        Dim balise As String = ""
        balise &= "<Format "
        balise &= ConstruireAttribut("Style", Style)
        balise &= "/>"

        Return balise
    End Function

End Class