Imports System.Text
Imports Rrq.Securite.GestionAcces


''' <summary>
''' Cette Class sert d'accès rapide et facile à Sage. 
''' Elle possède un buffeur de 5 minutes pour chaque collections d'instance.
''' </summary>
''' <remarks></remarks>
Friend Class TsCuConfiguration

#Region "CONSTANTE"

    '! Définition de valeur dans Sage pour les Type de Rôles
    Public Const TYPEROLE_REO As String = "REO"
    Public Const TYPEROLE_REO_E As String = "REO-E"

#End Region

#Region "Private Vars"

    Private Shared mConfig As String = ""

#End Region

#Region "Propriétés"
    Public Shared Property Config() As String
        Get
            If String.IsNullOrEmpty(mConfig) Then
                mConfig = "ConfigUtilRess"
            End If
            Return mConfig
        End Get
        Set(ByVal value As String)
            mConfig = value
        End Set
    End Property
#End Region

#Region "Property ReadOnly"

    ''' <summary>
    ''' Donne accès à la collection d'utilisateurs. Se charge d'être à jour aux 5 minutes.
    ''' </summary>
    Public Shared ReadOnly Property Utilisateurs() As TsCdSageUserCollection
        Get
            Return TsBaConfigSage.GetConfigurationUsers(Config)
        End Get
    End Property

    ''' <summary>
    ''' Donne accès à la collection des rôles. Se charge d'être à jour aux 5 minutes.
    ''' </summary>
    Public Shared ReadOnly Property Roles() As TsCdSageRoleCollection
        Get
            Return TsBaConfigSage.GetConfigurationRoles(Config)
        End Get
    End Property

    ''' <summary>
    ''' Donne accès à la collection des ressources. Se charge d'être à jour aux 5 minutes.
    ''' </summary>
    Public Shared ReadOnly Property Ressources() As TsCdSageResourceCollection
        Get
            Return TsBaConfigSage.GetConfigurationResources(Config)
        End Get
    End Property

#End Region

#Region "Méthodes: Obtenir Utilisateur/Role/Ressource"

    ''' <summary>
    ''' Obtenir un rôle de Sage en indiquant le nom du rôle.
    ''' </summary>
    ''' <returns>Retourne le premier rôle remplissant la condition, sinon retourne Nothing.</returns>
    Public Shared Function ObtenirRole(ByVal roleName As String) As TsCdSageRole
        Return TsBaConfigSage.FindConfigurationRole(Config, roleName)
    End Function

    ''' <summary>
    ''' Obtenir une ressource de Sage en indiquant le ressource name 1, 2 et 3 (ResName1, ResName2, ResName3).
    ''' </summary>
    ''' <returns>Retourne la première ressource remplissant la condition, sinon retourne Nothing.</returns>
    Public Shared Function ObtenirRessource(ByVal resName1 As String, ByVal resName2 As String, ByVal resName3 As String) As TsCdSageResource
        Return TsBaConfigSage.FindConfigurationResource(Config, resName1, resName2, resName3)
    End Function

    ''' <summary>
    ''' Obtenir un utilisateur de Sage en indiquant le ID de l'utilisateur.
    ''' </summary>
    ''' <returns>Retourne le premier utilisateur remplissant la condition, sinon retourne Nothing.</returns>
    Public Shared Function ObtenirUtilisateur(ByVal personID As String) As TsCdSageUser
        Return TsBaConfigSage.FindConfigurationUser(Config, personID)
    End Function

#End Region

#Region "Méthode Get liste de Usagers/Roles/Ressources"

    ''' <summary>
    ''' Cette méthode va regrouper les usagers fesant partis de la relation ressource/utilisateurs à partir 
    ''' des noms de la ressource 1, 2 et 3 (resName1, resName2, resName3) en entrée.
    ''' </summary>
    Public Shared Function ObtenirRelationReU(ByVal resName1 As String, ByVal resName2 As String, ByVal resName3 As String) As TsCdSageUserCollection
        Dim paramRetour As TsCdSageUserCollection
        Dim Liste As TsCdSageUserResLinkCollection

        Liste = TsBaConfigSage.GetResourceUsersLinks(Config, resName1, resName2, resName3)

        paramRetour = New TsCdSageUserCollection()

        For Each c As TsCdSageUserResLink In Liste
            Dim utilisateur As TsCdSageUser = ObtenirUtilisateur(c.PersonID)
            If Not (utilisateur Is Nothing) Then
                paramRetour.Add(utilisateur)
            End If
        Next
        Return paramRetour
    End Function

    ''' <summary>
    ''' Cette méthode va regrouper les ressources
    ''' fesant partis de la relation utilisateur/ressources à partir du personID d'entrée.
    ''' </summary>
    Public Shared Function ObtenirRelationURe(ByVal personID As String) As TsCdSageResourceCollection
        Dim paramRetour As TsCdSageResourceCollection
        Dim Liste As TsCdSageUserResLinkCollection

        Liste = TsBaConfigSage.GetUserResourcesLinks(Config, personID)

        paramRetour = New TsCdSageResourceCollection()

        For Each c As TsCdSageUserResLink In Liste
            Dim ressource As TsCdSageResource = ObtenirRessource(c.ResName1, c.ResName2, c.ResName3)
            If Not (ressource Is Nothing) Then
                paramRetour.Add(ressource)
            End If
        Next
        Return paramRetour
    End Function

    ''' <summary>
    ''' Cette méthode va regrouper les utilisateurs
    ''' fesant partis de la relation role/utilisateurs à partir du roleName d'entrée.
    ''' </summary>
    Public Shared Function ObtenirRelationRoU(ByVal roleName As String) As TsCdSageUserCollection
        Dim paramRetour As TsCdSageUserCollection
        Dim Liste As TsCdSageUserRoleLinkCollection

        Liste = TsBaConfigSage.GetRoleUsersLinks(Config, roleName)

        paramRetour = New TsCdSageUserCollection()

        For Each c As TsCdSageUserRoleLink In Liste
            Dim utilisateur As TsCdSageUser = ObtenirUtilisateur(c.PersonID)
            If Not (utilisateur Is Nothing) Then
                paramRetour.Add(utilisateur)
            End If
        Next
        Return paramRetour
    End Function

    ''' <summary>
    ''' Cette méthode va regrouper les utilisateurs
    ''' fesant partis des relations role/utilisateurs à partir de plusieurs roleName d'entrée.
    ''' </summary>
    Public Shared Function ObtenirRelationRoU(ByVal pLstRoleName As List(Of String)) As TsCdSageUserCollection
        Dim paramRetour As New TsCdSageUserCollection
        Dim liste As TsCdSageUserRoleLinkCollection
        Dim dctRetour As New Dictionary(Of String, TsCdSageUser)

        Dim dctUtilisateur As Dictionary(Of String, TsCdSageUser) = Utilisateurs.ToDictionary(Function(u) u.PersonID)

        For Each roleName As String In pLstRoleName
            liste = TsBaConfigSage.GetRoleUsersLinks(Config, roleName)

            paramRetour = New TsCdSageUserCollection()

            For Each c As TsCdSageUserRoleLink In liste
                If Not dctRetour.ContainsKey(c.PersonID) Then
                    dctRetour.Add(c.PersonID, dctUtilisateur(c.PersonID))
                End If
            Next
        Next

        paramRetour.AddRange(dctRetour.Values)

        Return paramRetour
    End Function

    ''' <summary>
    ''' Cette méthode va regrouper les rôles
    ''' fesant partis de la relation utilisateur/rôles à partir du personID d'entrée.
    ''' </summary>
    Public Shared Function ObtenirRelationURo(ByVal personID As String) As TsCdSageRoleCollection
        Dim paramRetour As TsCdSageRoleCollection
        Dim Liste As TsCdSageUserRoleLinkCollection

        Liste = TsBaConfigSage.GetUserRolesLinks(Config, personID)

        paramRetour = New TsCdSageRoleCollection()

        For Each c As TsCdSageUserRoleLink In Liste
            Dim role As TsCdSageRole = ObtenirRole(c.RoleName)
            If Not (role Is Nothing) Then
                paramRetour.Add(role)
            End If
        Next
        Return paramRetour
    End Function

    ''' <summary>
    ''' Cette méthode va regrouper les rôles fesant partis de la relation ressource/rôles 
    ''' à partir des noms de la ressource 1, 2 et 3 (resName1, resName2, resName3) en entrée.
    ''' </summary>
    Public Shared Function ObtenirRelationReRo(ByVal resName1 As String, ByVal resName2 As String, ByVal resName3 As String) As TsCdSageRoleCollection
        Dim paramRetour As TsCdSageRoleCollection
        Dim Liste As TsCdSageRoleResLinkCollection

        Liste = TsBaConfigSage.GetResourceRolesLinks(Config, resName1, resName2, resName3)

        paramRetour = New TsCdSageRoleCollection()

        For Each c As TsCdSageRoleResLink In Liste
            Dim role As TsCdSageRole = ObtenirRole(c.RoleName)
            If Not (role Is Nothing) Then
                paramRetour.Add(role)
            End If
        Next
        Return paramRetour
    End Function

    ''' <summary>
    ''' Cette méthode va regrouper les ressources
    ''' fesant partis de la relation rôle/ressources à partir du roleName d'entrée.
    ''' </summary>
    Public Shared Function ObtenirRelationRoRe(ByVal roleName As String) As TsCdSageResourceCollection
        Dim paramRetour As TsCdSageResourceCollection
        Dim Liste As TsCdSageRoleResLinkCollection

        Liste = TsBaConfigSage.GetRoleResourcesLinks(Config, roleName)

        paramRetour = New TsCdSageResourceCollection()

        For Each c As TsCdSageRoleResLink In Liste
            Dim ressource As TsCdSageResource = ObtenirRessource(c.ResName1, c.ResName2, c.ResName3)
            If Not (ressource Is Nothing) Then
                paramRetour.Add(ressource)
            End If
        Next
        Return paramRetour
    End Function

    ''' <summary>
    ''' Cette méthode va regrouper les rôles enfants 
    ''' fesant partis de la relation rôle/rôle à partir du roleName d'entrée.
    ''' </summary> 
    ''' <remarks>Relation Enfant/Parent définit par Sage. Les parents héritent des enfants.</remarks>
    Public Shared Function ObtenirRelationRoRoEnfant(ByVal roleName As String) As TsCdSageRoleCollection
        Dim paramRetour As TsCdSageRoleCollection
        Dim Liste As TsCdSageRoleRoleLinkCollection

        Liste = TsBaConfigSage.GetRoleSubRolesLinks(Config, roleName)

        paramRetour = New TsCdSageRoleCollection()

        For Each c As TsCdSageRoleRoleLink In Liste
            Dim role As TsCdSageRole = ObtenirRole(c.ChildRole)
            If Not (role Is Nothing) Then
                paramRetour.Add(role)
            End If
        Next
        Return paramRetour
    End Function

    ''' <summary>
    ''' Cette méthode va regrouper les rôles parents
    ''' fesant partis de la relation rôle/rôle à partir du roleName d'entée.
    ''' </summary> 
    ''' <remarks>Relation Enfant/Parent définit par Sage. Les parents héritent des enfants.</remarks>
    Public Shared Function ObtenirRelationRoRoParent(ByVal roleName As String) As TsCdSageRoleCollection
        Dim paramRetour As TsCdSageRoleCollection
        Dim Liste As TsCdSageRoleRoleLinkCollection

        Liste = TsBaConfigSage.GetRoleSupRolesLinks(Config, roleName)

        paramRetour = New TsCdSageRoleCollection()

        For Each c As TsCdSageRoleRoleLink In Liste
            Dim role As TsCdSageRole = ObtenirRole(c.ParentRole)
            If Not (role Is Nothing) Then
                paramRetour.Add(role)
            End If
        Next
        Return paramRetour
    End Function

    ''' <summary>
    ''' Obtien tous les liens utilisateur\rôle.
    ''' </summary>
    ''' <returns>Collection de liens.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirLiensUtilisateurRole() As TsCdSageUserRoleLinkCollection
        Return TsBaConfigSage.GetUserRolesLinks(Config)
    End Function

    ''' <summary>
    ''' Obtien tous les liens rôle\rôle.
    ''' </summary>
    ''' <returns>Collection de liens.</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirLiensRoleRole() As TsCdSageRoleRoleLinkCollection
        Return TsBaConfigSage.GetRoleSubRolesLinks(Config)
    End Function

#End Region

#Region "Méthodes Partagées"

    ''' <summary>
    ''' Cette fonction permet de vider la cache.
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub RafraichirBuffer()
        TsBaConfigSage.ClearCache()
    End Sub

#End Region

End Class