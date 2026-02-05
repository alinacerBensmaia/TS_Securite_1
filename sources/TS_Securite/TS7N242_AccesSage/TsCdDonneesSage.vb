' On a ici dans un même fichier toutes les classes de données de base pour jouer avec une configuration de Sage
' (autant les classes de collection que les objets de dernier niveau: utilisateurs, rôles, ressources, liens)
' Les listes d'attributs de ces classes sont probablement incomplètes, on pourra en ajouter

Imports System.Xml.Serialization
Imports Rrq.Securite.GestionAcces.TsCdConstanteNomChampNorm

#Region "Utilisateurs"
<XmlRoot("Users")> _
Public Class TsCdSageUserCollection
    Inherits TsCdCollectionSage(Of TsCdSageUser)

    <XmlIgnore()> _
    Public ReadOnly Property Users() As List(Of TsCdSageUser)
        Get
            Return _list
        End Get
    End Property

End Class

<XmlType("User")> _
Public Class TsCdSageUser
    <TsAtNomChampGen(USER.ID)> _
    Public UserID As Integer
    <TsAtNomChampGen(USER.PERSON_ID)> _
    Public PersonID As String
    <TsAtNomChampGen(USER.NAME)> _
    Public UserName As String
    <TsAtNomChampGen(USER.ORGANIZATION)> _
    Public Organization As String
    <TsAtNomChampGen(USER.ORGANIZATION_TYPE)> _
    Public OrganizationType As String
    <XmlElement("FieldValue1"), TsAtNomChampGen(USER.VILLE)> _
    Public Ville As String
    <XmlElement("FieldValue2"), TsAtNomChampGen(USER.COURRIEL)> _
    Public Courriel As String
    <XmlElement("FieldValue3"), TsAtNomChampGen(USER.DATE_FIN)> _
    Public DateFin As Date
    <XmlElement("FieldValue4"), TsAtNomChampGen(USER.PRENOM)> _
    Public Prenom As String
    <XmlElement("FieldValue5"), TsAtNomChampGen(USER.NOM)> _
    Public Nom As String
    <XmlElement("FieldValue6"), TsAtNomChampGen(USER.DATE_APPROBATION)> _
    Public DateApprobation As Date
    <XmlElement("FieldValue7"), TsAtNomChampGen(USER.CN)> _
    Public CN As String
    <XmlElement("FieldValue8"), TsAtNomChampGen(USER.NOM_UNITE)> _
    Public nomUnite As String


    Public Overrides Function ToString() As String
        Return UserName
    End Function
End Class
#End Region

#Region "Roles"
<XmlRoot("Roles")> _
Public Class TsCdSageRoleCollection
    Inherits TsCdCollectionSage(Of TsCdSageRole)

    Public ReadOnly Property Roles() As List(Of TsCdSageRole)
        Get
            Return _list
        End Get
    End Property

End Class

<XmlType("Role")> _
Public Class TsCdSageRole
    <TsAtNomChampGen(ROLE.ID)> _
    Public RoleID As Integer
    <XmlElement("RoleName"), TsAtNomChampGen(ROLE.NAME)> _
    Public Name As String
    <XmlElement("RoleDescription"), TsAtNomChampGen(ROLE.DESCRIPTION)> _
    Public Description As String
    <XmlElement("RoleOrganization"), TsAtNomChampGen(ROLE.ORGANIZATION)> _
    Public Organization As String
    <XmlElement("RoleOwner"), TsAtNomChampGen(ROLE.OWNER)> _
    Public Owner As String
    <XmlElement("RoleCreateDate"), TsAtNomChampGen(ROLE.CREATE_DATE)> _
    Public CreateDate As Date
    <XmlElement("RoleApproveCode"), TsAtNomChampGen(ROLE.APPROVE_CODE)> _
    Public ApproveCode As String
    <XmlElement("RoleApprovedDate"), TsAtNomChampGen(ROLE.APPROVED_DATE)> _
    Public ApproveDate As Date
    <XmlElement("RoleExpirationDate"), TsAtNomChampGen(ROLE.EXPIRATION_DATE)> _
    Public ExpirationDate As Date
    <XmlElement("RoleOrganization2"), TsAtNomChampGen(ROLE.ORGANIZATION2)> _
    Public Organization2 As String
    <XmlElement("RoleOrganization3"), TsAtNomChampGen(ROLE.ORGANIZATION3)> _
    Public Organization3 As String
    <XmlElement("RoleType"), TsAtNomChampGen(ROLE.TYPE)> _
    Public Type As String
    <XmlElement("RoleReviewer"), TsAtNomChampGen(ROLE.REVIEWER)> _
    Public Reviewer As String
    <XmlElement("RoleFilter"), TsAtNomChampGen(ROLE.FILTER)> _
    Public Filter As String

    Public Overrides Function ToString() As String
        Return Name
    End Function
End Class
#End Region

#Region "Ressources"
<XmlRoot("Resources")> _
Public Class TsCdSageResourceCollection
    Inherits TsCdCollectionSage(Of TsCdSageResource)

    <XmlIgnore()> _
    Public ReadOnly Property Resources() As List(Of TsCdSageResource)
        Get
            Return _list
        End Get
    End Property

End Class

<XmlType("Resource")> _
Public Class TsCdSageResource
    <TsAtNomChampGen(RESSOURCE.RESNAME_1)> _
    Public ResName1 As String
    <TsAtNomChampGen(RESSOURCE.RESNAME_2)> _
    Public ResName2 As String
    <TsAtNomChampGen(RESSOURCE.RESNAME_3)> _
    Public ResName3 As String
    <TsAtNomChampGen(RESSOURCE.RESNAME_4)> _
    Public ResName4 As String
    '! "Nom" ou Description
    <TsAtNomChampGen(RESSOURCE.FIELD_VALUE_1)> _
    Public FieldValue1 As String
    '! "CN"
    <TsAtNomChampGen(RESSOURCE.FIELD_VALUE_2)> _
    Public FieldValue2 As String
    '! "Détails"
    <TsAtNomChampGen(RESSOURCE.FIELD_VALUE_3)> _
    Public FieldValue3 As String
    '! "Détenteur"
    <TsAtNomChampGen(RESSOURCE.FIELD_VALUE_4)> _
    Public FieldValue4 As String
    <TsAtNomChampGen(RESSOURCE.FIELD_VALUE_5)> _
    Public FieldValue5 As String


    Public Overrides Function ToString() As String
        Return ResName1
    End Function
End Class
#End Region

#Region "Liens utilisateur-rôle"
<XmlRoot("Links")> _
Public Class TsCdSageUserRoleLinkCollection
    Inherits TsCdCollectionSage(Of TsCdSageUserRoleLink)

    <XmlIgnore()> _
    Public ReadOnly Property Links() As List(Of TsCdSageUserRoleLink)
        Get
            Return _list
        End Get
    End Property

End Class

<XmlType("Link")> _
Public Class TsCdSageUserRoleLink
    Public PersonID As String
    Public RoleName As String
End Class
#End Region

#Region "Liens rôle-rôle"
<XmlRoot("Links")> _
Public Class TsCdSageRoleRoleLinkCollection
    Inherits TsCdCollectionSage(Of TsCdSageRoleRoleLink)

    <XmlIgnore()> _
    Public ReadOnly Property Links() As List(Of TsCdSageRoleRoleLink)
        Get
            Return _list
        End Get
    End Property

End Class

<XmlType("Link")> _
Public Class TsCdSageRoleRoleLink
    '! Considérer comme le rôle supérieur.
    Public ParentRole As String
    '! Considérer comme le sous rôle.
    Public ChildRole As String
End Class
#End Region

#Region "Liens rôle-ressource"
<XmlRoot("Links")> _
Public Class TsCdSageRoleResLinkCollection
    Inherits TsCdCollectionSage(Of TsCdSageRoleResLink)

    <XmlIgnore()> _
    Public ReadOnly Property Links() As List(Of TsCdSageRoleResLink)
        Get
            Return _list
        End Get
    End Property

End Class

<XmlType("Link")> _
Public Class TsCdSageRoleResLink
    Public RoleName As String
    Public ResName1 As String
    Public ResName2 As String
    Public ResName3 As String
    Public ResName4 As String
End Class
#End Region

#Region "Liens usager-ressource"
<XmlRoot("Links")> _
Public Class TsCdSageUserResLinkCollection
    Inherits TsCdCollectionSage(Of TsCdSageUserResLink)

    <XmlIgnore()> _
    Public ReadOnly Property Links() As List(Of TsCdSageUserResLink)
        Get
            Return _list
        End Get
    End Property

End Class

<XmlType("Link")> _
Public Class TsCdSageUserResLink
    Public PersonID As String
    Public ResName1 As String
    Public ResName2 As String
    Public ResName3 As String
End Class
#End Region

#Region "Information configuration"
<XmlRoot("SageConfigurations")> _
Public Class TsCdSageConfigurationCollc
    Inherits TsCdCollectionSage(Of TsCdSageConfigurationFull)

    Public ReadOnly Property Configurations() As List(Of TsCdSageConfigurationFull)
        Get
            Return _list
        End Get
    End Property

End Class

<XmlRoot("NewDataSet")> _
Public Class TsCdSageDBInfrm
    Public Configuration As TsCdSageConfigurationFull
End Class

<XmlType("Configuration")> _
Public Class TsCdSageConfigurationFull
    Public ConfigurationID As String
    Public ConfigurationName As String
    Public UserDatabaseID As String
    ''' <summary>
    ''' Nom de la UDB.
    ''' </summary>
    Public UserDBName As String
    Public ResourceDatabaseID As String
    ''' <summary>
    ''' Nom de la RDB.
    ''' </summary>
    Public DatabaseName As String
    Public CreateDate As Date
    Public ModifyDate As Date
    Public Owner1 As String
    Public Operation1 As String
    Public ParentConfigName As String
    Public IsReadOnly As Boolean
    Public IsLogged As Boolean
    Public IsCompleted As Boolean
End Class

<XmlRoot("Diff")> _
Public Class TsCdSageDifferenceConfig

    <XmlIgnore()> _
    Public VieilleConfig As String
    <XmlIgnore()> _
    Public NouvelleConfig As String

    Public AddedUsers As TsCdCollectionSage(Of TsCdSageUser)
    Public RemovedUsers As TsCdCollectionSage(Of TsCdSageUser)

    Public AddedRoles As TsCdCollectionSage(Of TsCdSageRole)
    Public RemovedRoles As TsCdCollectionSage(Of TsCdSageRole)

    Public AddedResources As TsCdCollectionSage(Of TsCdSageResource)
    Public RemovedResources As TsCdCollectionSage(Of TsCdSageResource)

    Public AddedUserRoleLinks As TsCdCollectionSage(Of TsCdSageUserRoleLink)
    Public RemovedUserRoleLinks As TsCdCollectionSage(Of TsCdSageUserRoleLink)

    Public AddedUserResourceLinks As TsCdCollectionSage(Of TsCdSageUserResLink)
    Public RemovedUserResourceLinks As TsCdCollectionSage(Of TsCdSageUserResLink)

    Public AddedRoleResourceLinks As TsCdCollectionSage(Of TsCdSageRoleResLink)
    Public RemovedRoleResourceLinks As TsCdCollectionSage(Of TsCdSageRoleResLink)

    Public AddedRoleRoleLinks As TsCdCollectionSage(Of TsCdSageRoleRoleLink)
    Public RemovedRoleRoleLinks As TsCdCollectionSage(Of TsCdSageRoleRoleLink)

End Class

Public Class TsCdSageDifferenceDB

    Public AddedUsers As TsCdCollectionSage(Of TsCdSageUser)
    Public RemovedUsers As TsCdCollectionSage(Of TsCdSageUser)

    Public AddedResources As TsCdCollectionSage(Of TsCdSageResource)
    Public RemovedResources As TsCdCollectionSage(Of TsCdSageResource)

End Class
#End Region

#Region "Champs de données"

<XmlRoot("Fields")> _
Public Class TsCdListeChampsSage
    Inherits TsCdCollectionSage(Of TsCdChampSage)

    Public ReadOnly Property ListeChamp() As List(Of TsCdChampSage)
        Get
            Return _list
        End Get
    End Property
    'Public lstChampsSage As TsCdCollectionSage(Of TsCdChampSage)
End Class

<XmlType("Field")> _
Public Class TsCdChampSage

    <XmlElement("FieldNumber")> _
    Public NumeroChamp As Integer
    <XmlElement("FieldName")> _
    Public NomChamp As String

    Public Sub New()
    End Sub

    Public Sub New(ByVal numero As Integer, ByVal nom As String)
        NumeroChamp = numero
        NomChamp = nom
    End Sub

End Class

#End Region


