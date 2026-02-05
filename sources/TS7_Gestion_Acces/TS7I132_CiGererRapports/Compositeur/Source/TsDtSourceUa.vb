
''' <summary>
''' Cette classe contient la définition d'une unité administrative de la source du rapport.
''' </summary>
''' <remarks></remarks>
Friend Class TsDtSourceUa

    ''' <summary>
    ''' Type d'Unité Administrative.
    ''' </summary>
    Enum TypeRoleUA
        Tache
        Metier
        REO
    End Enum

    Private mNom As String
    ''' <summary>
    ''' Nom de l'Unité Administrative(UA).
    ''' </summary>
    Public Property Nom() As String
        Get
            Return mNom
        End Get
        Set(ByVal value As String)
            mNom = value
        End Set
    End Property

    Private mType As TypeRoleUA
    ''' <summary>
    ''' Type de l'Unité Administrative.
    ''' </summary>
    Public Property Type() As TypeRoleUA
        Get
            Return mType
        End Get
        Set(ByVal value As TypeRoleUA)
            mType = value
        End Set
    End Property

    Private mContextesPermis As New List(Of String)
    ''' <summary>
    ''' Liste contenant les contexte permis pour cette unité administrative.
    ''' </summary>
    Public Property ContextesPermis() As List(Of String)
        Get
            Return mContextesPermis
        End Get
        Set(ByVal value As List(Of String))
            mContextesPermis = value
        End Set
    End Property

    ''' <summary>
    ''' Permet de dire si un contexte est permt pour cette unité administrative.
    ''' </summary>
    ''' <param name="pContexte">Le contexte à vérifier.</param>
    ''' <returns>Vrai si le contexte est permit, faux sinon.</returns>
    ''' <remarks></remarks>
    Public Function EstPermitContexte(ByVal pContexte As String) As Boolean
        Dim requete = (From cp In ContextesPermis _
                       Where cp = pContexte).FirstOrDefault
        Return Not String.IsNullOrEmpty(requete)
    End Function

End Class
