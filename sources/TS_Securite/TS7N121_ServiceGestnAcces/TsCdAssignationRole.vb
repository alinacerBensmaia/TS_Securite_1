''' <summary>
''' Classe de données. 
''' Une assignation de rôle.
''' </summary>
''' <remarks>
''' Cette classe possède des niveaux d'écriture.
''' Si les données de cette objet proviennent du serveur Sage, ils seront totalements barrées.
''' </remarks>
Public Class TsCdAssignationRole

#Region "Private Vars"

    Private mID As String
    Private mNom As String
    Private mDescription As String
    Private mOrganisation As String
    Private mOrganisation2 As String
    Private mOrganisation3 As String
    Private mDateFin As Date
    Private mFinPrevu As Boolean
    Private mListeUniteAdministrative As List(Of String) = New List(Of String)

    Private mType As String

    Friend lectureSeule As Boolean

#End Region

#Region "Propriétés"

    ''' <summary>
    ''' L'identifiant du rôle assigné. Lecture seule.
    ''' </summary>
    ''' <remarks>Donnée en lecture seule.</remarks>
    Public Property ID() As String
        Get
            Return mID
        End Get
        Set(ByVal nouvelValeur As String)
            If lectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mID = nouvelValeur
            End If
        End Set
    End Property

    ''' <summary>
    ''' Le nom du rôle assigné. Lecture seule.
    ''' </summary>
    ''' <remarks>Donnée en lecture seule.</remarks>
    Public Property Nom() As String
        Get
            Return mNom
        End Get
        Set(ByVal nouvelValeur As String)
            If lectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mNom = nouvelValeur
            End If
        End Set
    End Property

    ''' <summary>
    ''' La description du rôle assigné.  Lecture seule.
    ''' </summary>
    ''' <remarks>Donnée en lecture seule.</remarks>
    Public Property Description() As String
        Get
            Return mDescription
        End Get
        Set(ByVal nouvelValeur As String)
            If lectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mDescription = nouvelValeur
            End If
        End Set
    End Property

    ''' <summary>
    ''' La date de fin de l'assignation du role. Lecture seule.
    ''' </summary>
    ''' <remarks>Donnée en lecture seule.</remarks>
    Public Property DateFin() As Date
        Get
            Return mDateFin
        End Get
        Set(ByVal nouvelValeur As Date)
            If lectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mDateFin = nouvelValeur
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
    ''' <remarks>Donnée en lecture seule.</remarks>
    Public ReadOnly Property Organisationnel() As Boolean
        Get
            Return mID.StartsWith(TsCuAccesSage.TYPEROLE_REO)
        End Get
    End Property

    ''' <summary>
    '''Est-ce que c'est un rôle métier. Lecture seule.
    ''' </summary>
    ''' <remarks>Donnée en lecture seule.</remarks>
    Public ReadOnly Property Particulier() As Boolean
        Get
            Return False
            'Return mID.StartsWith(TsCuAccesSage.TYPEROLE_RO)
        End Get
    End Property

    ''' <summary>
    ''' Y a t'il une date de fin pour cette assignation ?
    ''' Lecture seule.
    ''' </summary>
    ''' <remarks>Donnée en lecture seule.</remarks>
    Public Property FinPrevu() As Boolean
        Get
            Return mFinPrevu
        End Get
        Set(ByVal nouvelValeur As Boolean)
            If lectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est en Lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mFinPrevu = nouvelValeur
            End If
        End Set
    End Property

    ''' <summary>
    ''' Liste des unités administratives responsables.. Lecture seule.
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

#Region "Constructeurs"

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        lectureSeule = False
    End Sub

#End Region

#Region "Méthodes"
    ''' <summary>
    ''' Écrit une verstion string des données contenus dans l'instance.
    ''' </summary>
    Public Overrides Function ToString() As String
        Dim paramRetour As String

        paramRetour = "-----------------" + vbCrLf
        paramRetour = paramRetour + "Id de l'association de Role: " & ID & vbCrLf
        paramRetour = paramRetour + "Nom de l'association de Role: " & Nom & vbCrLf
        paramRetour = paramRetour + "Descritpion de l'association de Role: " & Description & vbCrLf
        paramRetour = paramRetour + "Date de fin de l'association de Role: " & DateFin & vbCrLf
        paramRetour = paramRetour + "L'association de Role Particulier: " & Particulier & vbCrLf
        paramRetour = paramRetour + "-----------------" + vbCrLf

        Return paramRetour
    End Function
#End Region

#Region "Functions de service"

    ''' <summary>
    '''  Fait une converstion entre une opération sur un rôle et une assignation de rôle.
    ''' </summary>
    Friend Shared Function ConvertirAssignation(ByVal operationRole As TsCdOperationRole) As TsCdAssignationRole
        Dim paramRetour As New TsCdAssignationRole()

        With paramRetour
            .mDateFin = operationRole.DateFin
            .mDescription = operationRole.Description
            .mFinPrevu = operationRole.FinPrevue
            .mID = operationRole.IdRole
            .mNom = operationRole.Nom
            .mOrganisation = operationRole.Organisation
            .mOrganisation2 = operationRole.Organisation2
            .mOrganisation3 = operationRole.Organisation3
            .mType = operationRole.Type

            .lectureSeule = True
        End With

        Return paramRetour
    End Function

    ''' <summary>
    '''  Fait une converstion entre une opération sur un rôle et une assignation de rôle.
    ''' </summary>
    Friend Shared Function ConvertirAssignation(ByVal role As TsCdRole) As TsCdAssignationRole
        Dim paramRetour As New TsCdAssignationRole()

        With paramRetour
            .mDescription = role.Description
            .mFinPrevu = False
            .mID = role.ID
            .mNom = role.Nom
            .mOrganisation = role.Organisation
            .mOrganisation2 = role.Organisation2
            .mOrganisation3 = role.Organisation3
            .mType = role.Type

            .lectureSeule = True
        End With

        Return paramRetour
    End Function

    ''' <summary>
    ''' Transforme les informations d'un rôle du module sage en rôle du module web
    ''' </summary>
    Friend Shared Function TraductionAssignationRole(ByVal sageRole As TsCdSageRole) As TsCdAssignationRole
        Dim paramRetour As New TsCdAssignationRole()

        With paramRetour
            .DateFin = New Date()
            .Description = sageRole.Description
            .ID = sageRole.Name
            .Nom = sageRole.Rule
            .FinPrevu = False
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

#Region "Opérateurs"

    ''' <summary>
    ''' Comparateur d'objets. Compare deux objets de même nature, pour savoir s'ils sont pareils.
    ''' </summary>
    Public Shared Operator =(ByVal premiereAssignation As TsCdAssignationRole, ByVal deuxiemeAssignation As TsCdAssignationRole) As Boolean
        With premiereAssignation
            If Not .DateFin = deuxiemeAssignation.DateFin Then Return False
            If Not .Description = deuxiemeAssignation.Description Then Return False
            If Not .FinPrevu = deuxiemeAssignation.FinPrevu Then Return False
            If Not .ID = deuxiemeAssignation.ID Then Return False
            If Not .Nom = deuxiemeAssignation.Nom Then Return False
            If Not .Particulier = deuxiemeAssignation.Particulier Then Return False
            If Not .Organisation = deuxiemeAssignation.Organisation Then Return False
            If Not .Organisation2 = deuxiemeAssignation.Organisation2 Then Return False
            If Not .Organisation3 = deuxiemeAssignation.Organisation3 Then Return False
        End With

        Return True
    End Operator

    ''' <summary>
    ''' Comparateur d'objets. Compare deux objets de même nature, pour savoir s'ils sont différents.
    ''' </summary>
    Public Shared Operator <>(ByVal premiereAssignation As TsCdAssignationRole, ByVal deuxiemeAssignation As TsCdAssignationRole) As Boolean
        Return Not (premiereAssignation = deuxiemeAssignation)
    End Operator

#End Region


End Class
