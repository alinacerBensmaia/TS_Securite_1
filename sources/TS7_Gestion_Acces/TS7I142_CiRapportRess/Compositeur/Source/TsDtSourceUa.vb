
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



End Class
