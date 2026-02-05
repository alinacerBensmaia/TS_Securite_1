Imports TS7I132_CiGererRapports.TsCuOutilsExcel

''' <summary>
''' Permet de définir le niveau de protection des cellules d'un Style.
''' </summary>
''' <remarks></remarks>
Public Class TsCuStyleProtection

    'ss:Protected
    Private mProtected As Boolean?
    ''' <summary>
    ''' Permet de bloqué les cellules d'un Style.
    ''' </summary>
    Public Property [Protected]() As Boolean?
        Get
            Return mProtected
        End Get
        Set(ByVal value As Boolean?)
            mProtected = value
        End Set
    End Property

    ''' <summary>
    ''' Permet d'obtenir la version XML valide SpreadSheet du format de la cellule.
    ''' </summary>
    ''' <returns>Un XML.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirXML() As String
        Dim balise As String = ""
        balise &= "<Protection"
        balise &= ConstruireAttributOptionnel("ss:Protected", [Protected])
        balise &= "/>"

        Return balise
    End Function

End Class