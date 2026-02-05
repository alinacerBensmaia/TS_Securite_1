Imports Configuration = Rrq.InfrastructureCommune.Parametres.XuCuConfiguration

''' <summary>
''' Classe de Donée. Élément de base du module.
''' Renferme les informations sur le rôle.
''' </summary>
''' <remarks>
''' Cette classe possède des niveaux d'écritures.
''' Si les données de cette objet proviennent du serveur Sage, ils seront totalements barrées.
''' </remarks>
Public Class TsCdRole

#Region "Private Vars"

    Private mID As String
    Private mNom As String
    Private mDescription As String
    Private mListeUniteAdministrative As List(Of String) = New List(Of String)
    Private mOrganisation As String
    Private mOrganisation2 As String
    Private mOrganisation3 As String

    Private mType As String

    Friend lectureSeule As Boolean

#End Region

#Region "Property"

    ''' <summary>
    ''' L'identifiant du rôle. Lecture seule.
    ''' </summary>
    ''' <remarks>Données en lecture seule.</remarks>
    Public Property ID() As String
        Get
            Return mID
        End Get
        Set(ByVal value As String)
            If lectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mID = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Le nom du rôle. Lecture seule.
    ''' </summary>
    ''' <remarks>Données en lecture seule.</remarks>
    Public Property Nom() As String
        Get
            Return mNom
        End Get
        Set(ByVal value As String)
            If lectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mNom = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' La description du rôle. Lecture seule.
    ''' </summary>
    ''' <remarks>Données en lecture seule.</remarks>
    Public Property Description() As String
        Get
            Return mDescription
        End Get
        Set(ByVal value As String)
            If lectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mDescription = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' L'organisation 1. Lecture seule.
    ''' </summary>
    ''' <remarks>Données en lecture seule.</remarks>
    Public Property Organisation() As String
        Get
            Return mOrganisation
        End Get
        Set(ByVal value As String)
            If lectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mOrganisation = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' L'organisation 2. Lecture seule.
    ''' </summary>
    ''' <remarks>Données en lecture seule.</remarks>
    Public Property Organisation2() As String
        Get
            Return mOrganisation2
        End Get
        Set(ByVal value As String)
            If lectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mOrganisation2 = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' L'organisation 3. Lecture seule.
    ''' </summary>
    ''' <remarks>Données en lecture seule.</remarks>
    Public Property Organisation3() As String
        Get
            Return mOrganisation3
        End Get
        Set(ByVal value As String)
            If lectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mOrganisation3 = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Est-ce que c'est un rôle Organisationnel. Lecture seule.
    ''' </summary>
    ''' <remarks>Données en lecture seule.</remarks>
    Public ReadOnly Property Organisationnel() As Boolean
        Get
            Return mID.StartsWith(TsCuAccesSage.TYPEROLE_REO)
        End Get
    End Property

    ''' <summary>
    ''' Est-ce que c'est un rôle métier. Lecture seule.
    ''' </summary>
    ''' <remarks>Données en lecture seule.</remarks>
    Public ReadOnly Property Particulier() As Boolean
        Get
            Return False
            'Return mID.StartsWith(TsCuAccesSage.TYPEROLE_RO))
        End Get
    End Property

    ''' <summary>
    ''' La liste des unités administratives responsable. Lecture seule.
    ''' </summary>
    ''' <remarks>Données en lecture seule.</remarks>
    Public ReadOnly Property ListeUniteAdministrativeResponsable() As List(Of String)
        Get
            Return mListeUniteAdministrative
        End Get
    End Property

    ''' <summary>
    ''' Le type de rôle. Lecture seule.
    ''' </summary>
    ''' <remarks>Données en lecture seule.</remarks>
    Public Property Type() As String
        Get
            Return mType
        End Get
        Set(ByVal nouvelValeur As String)
            If lectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mType = nouvelValeur
            End If
        End Set
    End Property

#End Region

#Region "Constructeur"

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    Public Sub New()
        lectureSeule = False
    End Sub

#End Region

#Region "Méthodes"

    ''' <summary>
    ''' Retourne une version string des informations de l'instance.
    ''' </summary>
    Public Overrides Function ToString() As String
        Dim paramRetour As String

        paramRetour = "-----------------" + vbCrLf
        paramRetour = paramRetour + "Rôle Id: " & ID & vbCrLf
        paramRetour = paramRetour + "Nom du Rôle: " & Nom & vbCrLf
        paramRetour = paramRetour + "sDescritpion du Rôle: " & Description & vbCrLf
        paramRetour = paramRetour + "Rôle particulier ? " & Particulier & vbCrLf
        paramRetour = paramRetour + "-----------------" + vbCrLf

        Return paramRetour
    End Function

#End Region

#Region "Fonctions de services"

    ''' <summary>
    ''' Transforme les informations d'un rôle du module sage en un rôle module web.
    ''' </summary>
    Friend Shared Function TraductionRole(ByVal sageRole As TsCdSageRole) As TsCdRole
        Dim paramRetour As TsCdRole = New TsCdRole

        With paramRetour
            .Nom = sageRole.Rule
            .ID = sageRole.Name
            .Description = sageRole.Description
            .Organisation = sageRole.Organization
            .Organisation2 = sageRole.Organization2
            .Organisation3 = sageRole.Organization3
            .Type = sageRole.Type

            If Not String.IsNullOrEmpty(sageRole.Owner) Then
                For Each uniteAdminID As String In sageRole.Owner.Split(";"c)
                    .ListeUniteAdministrativeResponsable.AddRange(TsCuOutils.TraiterUniteAdministrativeResponsable(uniteAdminID))
                Next
            End If
            .lectureSeule = True
        End With

        Return paramRetour
    End Function

#End Region

#Region "Operateur"

    ''' <summary>
    ''' Comparateur d'objets. Compare deux objets de même nature, pour savoir s'ils sont pareils.
    ''' </summary>
    Public Shared Operator =(ByVal premierRole As TsCdRole, ByVal deuxiemeRole As TsCdRole) As Boolean

        With premierRole
            If .Description <> deuxiemeRole.Description Then Return False
            If .ID <> deuxiemeRole.ID Then Return False
            If .Nom <> deuxiemeRole.Nom Then Return False
            If .Particulier <> deuxiemeRole.Particulier Then Return False
            If .Organisation <> deuxiemeRole.Organisation Then Return False
            If .Organisation2 <> deuxiemeRole.Organisation2 Then Return False
            If .Organisation3 <> deuxiemeRole.Organisation3 Then Return False
        End With

        Return True
    End Operator

    ''' <summary>
    ''' Comparateur d'objets. Compare deux objets de même nature, pour savoir s'ils sont différents.
    ''' </summary>
    Public Shared Operator <>(ByVal premierRole As TsCdRole, ByVal deuxiemeRole As TsCdRole) As Boolean
        Return Not (premierRole = deuxiemeRole)
    End Operator

#End Region

End Class
