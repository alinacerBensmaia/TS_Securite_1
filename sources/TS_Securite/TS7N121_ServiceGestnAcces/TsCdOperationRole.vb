''' <summary>
''' Classe de donées. Élément de base du module.
''' Indiques les modification etre le rôle et l'utilisateur.
''' </summary>
''' <remarks>
''' Cette classe possède des niveaux d'écritures.
''' Si les données de cette objet proviennent du serveur Sage, ils seront totalements barrées.
''' </remarks>
Public Class TsCdOperationRole

#Region "CONSTANTE"
    Enum TsSgaOperation
        Aucun
        Ajout
        Modification
        Suppression
    End Enum
#End Region

#Region "Private Vars"

    Private mIdRole As String
    Private mOperation As TsSgaOperation
    Private mFinPrevue As Boolean
    Private mDateFin As Date

    Private mOrganisation As String
    Private mOrganisation2 As String
    Private mOrganisation3 As String

    Private mType As String

    Private mDescription As String
    Private mNom As String

    Friend lectureSeule As Boolean

#End Region

#Region "Property"

    ''' <summary>
    ''' L'identifiant du rôle dont l'operation est appliqué. Lecture seule.
    ''' </summary>
    ''' <remarks>Donnée en lecture seule.</remarks>
    Public Property IdRole() As String
        Get
            Return mIdRole
        End Get
        Set(ByVal nouvelValeur As String)
            If lectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est en lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mIdRole = nouvelValeur
            End If
        End Set
    End Property

    ''' <summary>
    ''' Le type d'opération appliqué. Lecture seule.
    ''' </summary>
    ''' <remarks>Donnée en lecture seule.</remarks>
    Public Property Operation() As TsSgaOperation
        Get
            Return mOperation
        End Get
        Set(ByVal nouvelValeur As TsSgaOperation)
            If lectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est en lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mOperation = nouvelValeur
            End If
        End Set
    End Property

    ''' <summary>
    ''' Date à laquel l'assignation du rôle n'est plus valide. Lecture seule.
    ''' </summary>
    ''' <remarks>Donnée en lecture seule.</remarks>
    Public Property DateFin() As Date
        Get
            Return mDateFin
        End Get
        Set(ByVal nouvelValeur As Date)
            If lectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est en lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mDateFin = nouvelValeur
            End If
        End Set
    End Property

    ''' <summary>
    ''' Il ya une date de fin d'assigantion? Lecture seule.
    ''' </summary>
    ''' <remarks>Donnée en lecture seule.</remarks>
    Public Property FinPrevue() As Boolean
        Get
            Return mFinPrevue
        End Get
        Set(ByVal nouvelValeur As Boolean)
            If lectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est en lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mFinPrevue = nouvelValeur
            End If
        End Set
    End Property

    ''' <summary>
    ''' La descritpion du rôle. Lecture seule.
    ''' </summary>
    ''' <remarks>Donnée en lecture seule.</remarks>
    Public Property Description() As String
        Get
            Return mDescription
        End Get
        Set(ByVal nouvelValeur As String)
            If lectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est en lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mDescription = nouvelValeur
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
            Return mIdRole.StartsWith(TsCuAccesSage.TYPEROLE_REO)
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
    ''' Nom du rôle. Lecture seule.
    ''' </summary>
    ''' <remarks>Donnée en lecture seule.</remarks>
    Public Property Nom() As String
        Get
            Return mNom
        End Get
        Set(ByVal nouvelValeur As String)
            If lectureSeule = True Then
                Throw New TsExcErreurGeneral("L'élément est en lecture seule. Vous ne pouvez l'accéder en écriture")
            Else
                mNom = nouvelValeur
            End If
        End Set
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
    ''' <remarks></remarks>
    Public Sub New()
        lectureSeule = False
    End Sub

#End Region

#Region "Méthodes"
    ''' <summary>
    ''' Crée une parfaite copie de l'opération. Aucun lien avec l'original.
    ''' </summary>
    Public Function Clone() As TsCdOperationRole
        Dim paramRetour As New TsCdOperationRole

        With paramRetour
            .mDateFin = mDateFin
            .mDescription = mDescription
            .mFinPrevue = mFinPrevue
            .mIdRole = mIdRole
            .mNom = mNom
            .mOperation = mOperation
            .mOrganisation = mOrganisation
            .mOrganisation2 = mOrganisation2
            .mOrganisation3 = mOrganisation3
            .lectureSeule = lectureSeule
        End With

        Return paramRetour
    End Function
#End Region

#Region "Fonctions de services"

    ''' <summary>
    ''' Sert à transformer les assignations en opérations sur les rôles.
    ''' </summary>
    Friend Shared Function ConversionAssignation(ByVal assigantionRole As TsCdAssignationRole) As TsCdOperationRole
        Dim paramRetour As New TsCdOperationRole()

        With paramRetour
            .mDateFin = assigantionRole.DateFin
            .Description = assigantionRole.Description
            .mFinPrevue = assigantionRole.FinPrevu
            .mIdRole = assigantionRole.ID
            .Nom = assigantionRole.Nom
            .mOrganisation = assigantionRole.Organisation
            .mOrganisation2 = assigantionRole.Organisation2
            .mOrganisation3 = assigantionRole.Organisation3
            .mType = assigantionRole.Type

            paramRetour.lectureSeule = assigantionRole.lectureSeule
        End With

        Return paramRetour
    End Function

#End Region


End Class
