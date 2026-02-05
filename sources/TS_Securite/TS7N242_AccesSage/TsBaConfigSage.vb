Imports Rrq.Securite.GestionAcces.portailsage
Imports Rrq.Securite.GestionAcces.portailsage1
Imports Rrq.Securite.GestionAcces.portailsage2
Imports System.Xml

' Méthode en lien avec une «configuration» au sens Sage

''' <summary>
''' Classe Pont. Interface d'Accès sage.
''' </summary>
''' <remarks></remarks>
Public Module TsBaConfigSage

#Region "Enums"

    Public Enum TypeBD
        User
        Resource
    End Enum

#End Region

#Region "Constantes"

    Public Const DATE_MIN_SAGE As Date = #1/1/1901#
    Public Const DATE_MAX_SAGE As Date = #1/1/2075#

#End Region

#Region "Méthodes d'obtention des données d'une configuration Sage (utilisateurs, rôles, resources, liens)"
    ''' <summary>
    ''' Obtient tous les utilisateurs définis dans une configuration Sage.
    ''' </summary>
    Public Function cfg_get_configuration_users(ByVal config As String) As TsCdSageUserCollection
        Dim retour As TsCdSageUserCollection = call_sage(Of SageDataService, TsCdSageUserCollection)("cfg_get_configuration_users", config)

        With retour
            If .List.Count = 1 Then
                If .List(0).Courriel = Nothing And .List(0).DateApprobation = Nothing And .List(0).DateFin = Nothing And _
                .List(0).Nom = Nothing And .List(0).Organization = Nothing And .List(0).OrganizationType = Nothing And _
                .List(0).PersonID = Nothing And .List(0).Prenom = Nothing And .List(0).UserID = Nothing And _
                .List(0).UserName = Nothing And .List(0).Ville = Nothing Then
                    retour.List.RemoveAt(0)
                End If
            End If
        End With

        Return retour
    End Function

    ''' <summary>
    ''' Obtien le XML d'une demande de Usagers.
    ''' </summary>
    Public Function cfg_get_configuration_users_string(ByVal config As String) As String
        Return call_sage_string(Of SageDataService, TsCdSageUserCollection)("cfg_get_configuration_users", config)
    End Function

    ''' <summary>
    ''' Obtient tous les rôles définis dans une configuration Sage.
    ''' </summary>
    Public Function cfg_get_roles(ByVal config As String) As TsCdSageRoleCollection
        Dim retour As TsCdSageRoleCollection = call_sage(Of SageDataService, TsCdSageRoleCollection)("cfg_get_roles", config)
        With retour

            '! Suprimme une liste remplis d'un seul élément et qui est vide. 
            '! Le XML renvoyé avec une réponse négative provoque une liste d'un seul élément vide.
            If .List.Count = 1 Then
                If .List(0).ApproveCode = Nothing And .List(0).ApproveDate = Nothing And .List(0).CreateDate = Nothing And _
                .List(0).Description = Nothing And .List(0).ExpirationDate = Nothing And .List(0).Filter = Nothing And _
                .List(0).Name = Nothing And .List(0).Organization = Nothing And .List(0).Organization2 = Nothing And _
                .List(0).Organization3 = Nothing And .List(0).Owner = Nothing And .List(0).Reviewer = Nothing And _
                .List(0).RoleID = Nothing And .List(0).Type = Nothing Then
                    retour.List.RemoveAt(0)
                End If
            End If
        End With

        Return retour
    End Function

    ''' <summary>
    ''' Obtien le XML d'une demande de Roles.
    ''' </summary>
    Public Function cfg_get_roles_string(ByVal config As String) As String
        Return call_sage_string(Of SageDataService, String)("cfg_get_roles", config)
    End Function

    ''' <summary>
    ''' Obtient tous les configurations définis dans Sage.
    ''' </summary>
    Public Function data_source_get_configurations() As TsCdSageConfigurationCollc
        Return call_sage(Of SageDataService, TsCdSageConfigurationCollc)("data_source_get_configurations")
    End Function

    ''' <summary>
    ''' Obtient tous les rôles définis dans une configuration Sage.
    ''' </summary>
    ''' <remarks>Il doit y avoir un tranfert à faire, car Sage retourne des balises avec des noms incohérents.</remarks>
    Public Function cfg_get_configuration_Ressource(ByVal config As String) As TsCdSageResourceCollection
        Dim retour As TsCdSageResourceConfigCollection = call_sage(Of SageDataService, TsCdSageResourceConfigCollection)("cfg_get_configuration_resources", config)
        Dim retour2 As New TsCdSageResourceCollection()

        For Each s As TsCdSageResourceConfig In retour
            retour2.Resources.Add(TsBaIncoherence.Copier(Of TsCdSageResource)(s))
        Next

        Return retour2
    End Function

    ''' <summary>
    ''' Obtien le XML d'une demande de Resources.
    ''' </summary>
    Public Function cfg_get_configuration_Ressource_string(ByVal config As String) As String
        Return call_sage_string(Of SageDataService, String)("cfg_get_configuration_resources", config)
    End Function

    ''' <summary>
    ''' Obtient tous les liens Usager/Role définis dans une configuration Sage.
    ''' </summary>
    Public Function cfg_get_user_role_links(ByVal config As String) As TsCdSageUserRoleLinkCollection
        Dim retour As TsCdSageUserRoleLinkCollection = call_sage(Of SageDataService, TsCdSageUserRoleLinkCollection)("cfg_get_user_role_links", config)
        With retour

            If .List.Count = 1 Then
                If .List(0).PersonID = Nothing And .List(0).RoleName = Nothing Then
                    retour.List.RemoveAt(0)
                End If
            End If
        End With
        Return retour
    End Function

    ''' <summary>
    ''' Obtien le XML d'un demande de Liens (Usager/Rôle).
    ''' </summary>
    Public Function cfg_get_user_role_links_string(ByVal config As String) As String
        Return call_sage_string(Of SageDataService, String)("cfg_get_user_role_links", config)
    End Function

    ''' <summary>
    ''' Obtient tous les Liens Usager/Ressource définis dans une configuration Sage.
    ''' </summary>
    Public Function cfg_get_user_resource_links(ByVal config As String) As TsCdSageUserResLinkCollection
        Dim retour As TsCdSageUserResLinkCollection = call_sage(Of SageDataService, TsCdSageUserResLinkCollection)("cfg_get_user_resource_links", config)
        With retour

            If .List.Count = 1 Then
                If .List(0).PersonID = Nothing And .List(0).ResName1 = Nothing And .List(0).ResName2 = Nothing And _
                .List(0).ResName3 = Nothing Then
                    retour.List.RemoveAt(0)
                End If
            End If
        End With
        Return retour
    End Function

    ''' <summary>
    ''' Obtien le XML d'un demande de Liens (Usager/Ressource).
    ''' </summary>
    Public Function cfg_get_user_resource_links_string(ByVal config As String) As String
        Return call_sage_string(Of SageDataService, String)("cfg_get_user_resource_links", config)
    End Function

    ''' <summary>
    ''' Obtient tous les Liens Role/Ressource définis dans une configuration Sage.
    ''' </summary>
    Public Function cfg_get_role_resource_links(ByVal config As String) As TsCdSageRoleResLinkCollection
        Dim retour As TsCdSageRoleResLinkCollection = call_sage(Of SageDataService, TsCdSageRoleResLinkCollection)("cfg_get_role_resource_links", config)
        With retour
            If .List.Count = 1 Then
                If .List(0).ResName1 = Nothing And .List(0).ResName2 = Nothing And .List(0).ResName3 = Nothing And _
                .List(0).ResName4 = Nothing And .List(0).RoleName = Nothing Then
                    retour.List.RemoveAt(0)
                End If
            End If
        End With
        Return retour
    End Function

    ''' <summary>
    ''' Obtien le XML d'un demande de Liens (Role/Ressource).
    ''' </summary>
    Public Function cfg_get_role_resource_links_string(ByVal config As String) As String
        Return call_sage_string(Of SageDataService, String)("cfg_get_role_resource_links", config)
    End Function

    ''' <summary>
    ''' Obtient tous les liens Role/Role définis dans une configuration Sage.
    ''' </summary>
    Public Function cfg_get_role_role_links(ByVal config As String) As TsCdSageRoleRoleLinkCollection
        Dim retour As TsCdSageRoleRoleLinkCollection = call_sage(Of SageDataService, TsCdSageRoleRoleLinkCollection)("cfg_get_role_role_links", config)
        If retour.List.Count = 1 Then
            If retour.List(0).ChildRole = Nothing And retour.List(0).ParentRole = Nothing Then
                retour.List.RemoveAt(0)
            End If
        End If

        Return retour
    End Function

    ''' <summary>
    ''' Obtien le XML d'un demande de Liens (Role/Role).
    ''' </summary>
    Public Function cfg_get_role_role_links_string(ByVal config As String) As String
        Return call_sage_string(Of SageDataService, String)("cfg_get_role_role_links", config)
    End Function

    ''' <summary>
    ''' Va chercher toute les information sur la cofig.
    ''' </summary>
    Public Function cfg_get_databases(ByVal cfgName As String) As TsCdSageDBInfrm
        Dim xml As String = cfg_get_databases_string(cfgName)
        If xml = "" Then
            Dim paramRetour As TsCdSageDBInfrm = New TsCdSageDBInfrm()
            paramRetour.Configuration = New TsCdSageConfigurationFull()
            Return paramRetour
        Else
            Return Deserialize(Of TsCdSageDBInfrm)(xml)
        End If

    End Function

    ''' <summary>
    ''' Cette fonction fait appel à sage et va chercher toutes les informations sur la cofig.
    ''' </summary>
    Public Function cfg_get_databases_string(ByVal cfgName As String) As String
        Return call_sage_string(Of SageDataService, String)("cfg_get_databases", cfgName)
    End Function

    ''' <summary>
    ''' Retourne une listes d'utilisateur de Sage.
    ''' </summary>
    Public Function udb_get_users(ByVal udbName As String) As TsCdSageUserCollection
        Dim stringXML As String

        stringXML = DataService.udb_get_users(ServeurSage, BDSage, udbName)

        Dim xmlDoc As New Xml.XmlDocument()
        xmlDoc.PreserveWhitespace = False
        xmlDoc.LoadXml(stringXML)
        TsBaIncoherenceC.Fix_Balise_Vide(xmlDoc)

        Dim lstUser As TsCdSageUserCollection = Deserialize(Of TsCdSageUserCollection)(xmlDoc.InnerXml)
        Dim objComparateur As New Comparateur(Of TsCdSageUser)( _
                 Function(e1 As TsCdSageUser, e2 As TsCdSageUser) e1.PersonID = e2.PersonID, _
                 Function(e As TsCdSageUser) e.PersonID.GetHashCode)
        Dim ensemble As New HashSet(Of TsCdSageUser)(lstUser, objComparateur)
        Dim paramRetour As New TsCdSageUserCollection
        paramRetour.AddRange(ensemble)

        Return paramRetour

    End Function

    ''' <summary>
    ''' Retourne une listes des ressources de Sage.
    ''' </summary>
    Public Function rdb_get_resources(ByVal rdbName As String) As TsCdSageResourceCollection
        Dim stringXML As String

        stringXML = DataService.rdb_get_resources(ServeurSage, BDSage, rdbName)

        Dim xmlDoc As New Xml.XmlDocument()
        xmlDoc.PreserveWhitespace = False
        xmlDoc.LoadXml(stringXML)
        TsBaIncoherenceC.Fix_Balise_Vide(xmlDoc)

        Dim lstRessource As TsCdSageResourceCollection = Deserialize(Of TsCdSageResourceCollection)(xmlDoc.InnerXml)
        Dim objComparateur As New Comparateur(Of TsCdSageResource)( _
                Function(e1 As TsCdSageResource, e2 As TsCdSageResource) e1.ResName1 = e2.ResName1 And e1.ResName2 = e2.ResName2 And e2.ResName3 = e2.ResName3, _
                Function(e As TsCdSageResource) (e.ResName1 + e.ResName2 + e.ResName3).GetHashCode)
        Dim ensemble As New HashSet(Of TsCdSageResource)(lstRessource, objComparateur)
        Dim paramRetour As New TsCdSageResourceCollection

        paramRetour.AddRange(ensemble)

        Return paramRetour
    End Function

    ''' <summary>
    ''' Cette fonction va appliqué les données sur sage. Creer un nouvel utilisateur.
    ''' </summary>
    Public Sub rdb_new_resource(ByVal rdbName As String, ByVal resName1 As String, _
    ByVal resName2 As String, ByVal resName3 As String)
        Dim ret As Integer

        ret = BasicService.rdb_new_resource(ServeurSage, BDSage, rdbName, resName1, resName2, resName3)
        If ret <> 0 Then
            Throw New ApplicationException("Code de retour différent de 0 dans l'appel de rdb_new_resource")
        End If
    End Sub

    ''' <summary>
    ''' Cette fonction, modifie le champ d'information pour la ressource donnée.
    ''' </summary>
    ''' <param name="fieldName">Nom du champ à modifier.</param>
    ''' <param name="fieldValue">Valeur du champ à modifier.</param>
    ''' <param name="fieldNumber">Numéro du champ à modifier.</param>
    Public Sub rdb_change_resource_field(ByVal rdbName As String, ByVal resName1 As String, ByVal resName2 As String, ByVal resName3 As String, _
    ByVal fieldName As String, ByVal fieldValue As String, ByVal fieldNumber As Integer)
        Dim ret As Integer
        '!BUG Promblème d'apostrophe: La solution de Eurikify est de doubler nous même les apostrophes.
        Dim fieldValueDoubler As String = doubleurApostrophe(fieldValue)
        
        If fieldNumber = -1 Then
            ret = BasicService.rdb_change_resource_field(ServeurSage, BDSage, rdbName, resName1, resName2, resName3, fieldName, fieldValueDoubler)
        Else
            '!BUG Promblème d'apostrophe: La solution de Eurikify est de doubler nous même les apostrophes.
            resName1 = doubleurApostrophe(resName1)
            resName2 = doubleurApostrophe(resName2)
            resName3 = doubleurApostrophe(resName3)

            '! Pour la création du champ, doubler le field value n'est pas nécessaire.
            ret = BasicService.rdb_new_resource_field(ServeurSage, BDSage, rdbName, resName1, resName2, resName3, fieldNumber, fieldValue)
            If ret = -1 Then
                ret = BasicService.rdb_change_resource_field(ServeurSage, BDSage, rdbName, resName1, resName2, resName3, fieldName, fieldValueDoubler)
            End If
        End If

        If ret < 0 Then
            Throw New ApplicationException("Code de retour est inférieur à 0 dans l'appel de rdb_change_resource_field")
        End If
    End Sub

    ''' <summary>
    ''' Cette fonction modifie un champ de données du rôle.
    ''' </summary>
    ''' <param name="cfgName">Nom de la configuration.</param>
    ''' <param name="nomRole">Nom du rôle à modifer.</param>
    ''' <param name="nomChamp">Nom du champ à modifier.</param>
    ''' <param name="valeur">La valeur qui sera attribuer au champ de données.</param>
    ''' <remarks></remarks>
    Public Sub cfg_change_role_field(ByVal cfgName As String, ByVal nomRole As String, ByVal nomChamp As String, ByVal valeur As String)
        Dim ret As Integer

        '!BUG Promblème d'apostrophe: La solution de Eurikify est de doubler nous même les apostrophes.
        nomRole = doubleurApostrophe(nomRole)
        valeur = doubleurApostrophe(valeur)

        ret = BasicService.cfg_change_role_field(ServeurSage, BDSage, cfgName, nomRole, nomChamp, valeur)
        'Retourne 1 quand la modification a été complèté
        If ret < 0 Then
            Throw New ApplicationException("Code de retour différent de 0 dans l'appel de cfg_change_role_field")
        End If
    End Sub

    ''' <summary>
    ''' Cette fonction va appliqué les données sur sage. Retire un utilisateur le la config.
    ''' </summary>
    Public Function diff_get_all(ByVal oldCfg As String, ByVal updatedCfg As String) As TsCdSageDifferenceConfig
        '! Si 2 configurations avec des udb et des rdb différent sont comparé le temps de calcule dépasse la limite normal.
        DiffService.Timeout = 3600000

        'BUG: Les balises sont mal définit. Cette function sert à changé la dernière balise de niveau 2.
        Dim stringTmp As String = TsBaIncoherenceC.Fix_Diff_get_all(DiffService.diff_get_all(ServeurSage, BDSage, oldCfg, updatedCfg))

        '! Fait un nétoyage des éléments vide.
        Dim xmlDoc As New Xml.XmlDocument()
        xmlDoc.PreserveWhitespace = False
        xmlDoc.LoadXml(stringTmp)
        TsBaIncoherenceC.Fix_Balise_Vide(xmlDoc)

        Dim paramRetour As TsCdSageDifferenceConfig = Deserialize(Of TsCdSageDifferenceConfig)(xmlDoc.InnerXml)
        paramRetour.VieilleConfig = oldCfg
        paramRetour.NouvelleConfig = updatedCfg
        Return paramRetour
    End Function

    ''' <summary>
    ''' Cette fonction va appliqué les données sur sage. Retire un utilisateur le la config.
    ''' </summary>
    Public Function diff_get_all_string(ByVal oldCfg As String, ByVal updatedCfg As String) As String
        '! Si 2 configurations avec des udb et des rdb différent sont comparé le temps de calcule dépasse la limite normal.
        DiffService.Timeout = 3600000

        'BUG: Les balises sont mal définit. Cette function sert à changé la dernière balise de niveau 2.
        Dim xml As String = TsBaIncoherenceC.Fix_Diff_get_all(DiffService.diff_get_all(ServeurSage, BDSage, oldCfg, updatedCfg))

        Return xml
    End Function

    ''' <summary>
    ''' Cette fonction revois les noms et numéros des champs de données de la configuration des utilisateur ou des ressources.
    ''' </summary>
    ''' <param name="dbName">Nom de la base de donnée.</param>
    ''' <param name="typeBD">Quel type la base de données est t-elle. 'User' ou 'Resource'.</param>
    ''' <returns>Une liste des noms et titres des champs de données.</returns>
    Public Function database_get_fields(ByVal dbName As String, ByVal typeBD As TypeBD) As TsCdListeChampsSage
        Dim stringXML As String

        stringXML = DataService.database_get_fields(ServeurSage, BDSage, dbName, typeBD.ToString())

        Dim xmlDoc As New Xml.XmlDocument()
        xmlDoc.PreserveWhitespace = False
        xmlDoc.LoadXml(stringXML)
        TsBaIncoherenceC.Fix_Balise_Vide(xmlDoc)

        Dim paramRetour As TsCdListeChampsSage = Deserialize(Of TsCdListeChampsSage)(xmlDoc.InnerXml)

        Return paramRetour
    End Function

    
#End Region

#Region "Méthodes pour créer et mettre à jour des configurations"

    ''' <summary>
    ''' Créer une nouvelle configuration.
    ''' </summary>
    ''' <param name="cfgName">Clé indexé. Le nom de la configuration.</param>
    ''' <param name="udbName">Nom de la base de données dans laquel la configuration va puiser ses usagers.</param>
    ''' <param name="rdbName">Nom de la base de données dans laquel la configuration va puiser ses ressources.</param>
    ''' <remarks></remarks>
    Public Sub new_cfg(ByVal cfgName As String, ByVal udbName As String, ByVal rdbName As String, _
                            ByVal modifyDate As Date, ByVal statusDate As Date, _
                            ByVal owner1 As String, ByVal owner2 As String, _
                            ByVal org1 As String, ByVal org2 As String, _
                            ByVal op1 As String, ByVal op2 As String, ByVal op3 As String, _
                            ByVal status As String, ByVal parentCfg As String)
        Dim ret As Integer
        ret = BasicService.new_cfg(ServeurSage, BDSage, cfgName, udbName, rdbName, _
                                    modifyDate, statusDate, owner1, owner2, _
                                    org1, org2, op1, op2, op3, _
                                    status, parentCfg)
        If ret <> 0 Then
            Throw New ApplicationException("Code de retour différent de 0 dans l'appel de new_cfg")
        End If
    End Sub

    ''' <summary>
    ''' Créer un nouveau rôle dans la configuration.
    ''' </summary>
    ''' <param name="cfgName">La configuration dans sage.</param>
    ''' <remarks></remarks>
    Public Sub cfg_new_configuration_role(ByVal cfgName As String, _
                            ByVal roleName As String, ByVal roleDescription As String, _
                            ByVal roleOrganization As String, ByVal roleOwner As String, _
                            ByVal roleType As String, _
                            ByVal roleCreateDate As Date, ByVal roleReviewer As String, _
                            ByVal roleApproveCode As String, ByVal roleApprovedDate As Date, _
                            ByVal roleFilter As String, _
                            ByVal roleOrganization2 As String, ByVal roleOrganization3 As String, _
                            ByVal roleExpirationDate As Date)
        '! Sage créer n Rôle pour n appel, même si dans l'interface DNA ne le permet pas. 
        '! Pour éviter cette faille d'intégrité, nous allon vérifier si elle n'est pas déja présente dans Sage.
        For Each role As TsCdSageRole In cfg_get_roles(cfgName)
            If role.Name = roleName Then
                Throw New TsExcSageRoleDejaExistant("Le rôle est déja présent dans Sage.")
            End If
        Next


        ' Sage supporte des dates NULL dans sa BD, mais ce n'est pas possible de donner une valeur nulle
        ' avec son interface SOAP... Pas fort... On approximationne avec des dates qui ne feront pas trop mal
        If roleCreateDate = Date.MinValue Then
            roleCreateDate = DATE_MIN_SAGE
        End If
        If roleApprovedDate = Date.MinValue Then
            roleApprovedDate = DATE_MIN_SAGE
        End If
        If roleExpirationDate = Date.MinValue Then
            roleExpirationDate = DATE_MIN_SAGE
        End If

        Dim ret As Integer
        ret = BasicService.cfg_new_configuration_role(ServeurSage, BDSage, cfgName, _
                                    roleName, roleDescription, roleOrganization, roleOwner, _
                                    roleType, roleCreateDate, roleReviewer, _
                                    roleApproveCode, roleApprovedDate, roleFilter, _
                                    roleOrganization2, roleOrganization3, roleExpirationDate)
        If ret <> 0 Then

            Throw New ApplicationException("Code de retour différent de 0 dans l'appel de cfg_new_configuration_role")
        End If
    End Sub

    ''' <summary>
    ''' Ajoute un usager à la configuration à partir de la udb lier à la configuration.
    ''' </summary>
    ''' <param name="cfgName">La configuration dans sage.</param>
    ''' <param name="userID">Clé de l'usager. Numéro de l'usager</param>
    ''' <remarks></remarks>
    Public Sub cfg_new_configuration_user(ByVal cfgName As String, ByVal userID As String)
        Dim ret As Integer

        ret = BasicService.cfg_new_configuration_user(ServeurSage, BDSage, userID, cfgName)

        If ret <> 0 Then
            Throw New ApplicationException("Code de retour différent de 0 dans l'appel de cfg_new_configuration_user")
        End If
    End Sub

    ''' <summary>
    ''' Ajoute une ressource dans la configuration à partir de la rdb lier à la configuration.
    ''' </summary>
    ''' <param name="cfgName">La configuration dans sage.</param>
    ''' <param name="resName1">Clé partiel(1/3) de la ressource. Nom de la ressource 1</param>
    ''' <param name="resName2">Clé partiel(1/3) de la ressource. Nom de la ressource 2</param>
    ''' <param name="resName3">Clé partiel(1/3) de la ressource. Nom de la ressource 3</param>
    ''' <remarks></remarks>
    Public Sub cfg_new_configuration_resource(ByVal cfgName As String, ByVal resName1 As String, ByVal resName2 As String, ByVal resName3 As String)
        Dim ret As Integer
        '!BUG Promblème d'apostrophe: La solution de Eurikify est de doubler nous même les apostrophes.
        resName1 = doubleurApostrophe(resName1)
        resName2 = doubleurApostrophe(resName2)
        resName3 = doubleurApostrophe(resName3)

        ret = BasicService.cfg_new_configuration_resource(ServeurSage, BDSage, resName1, resName2, resName3, cfgName)

        If ret <> 0 Then
            Throw New ApplicationException("Code de retour différent de 0 dans l'appel de cfg_new_configuration_resource")
        End If
    End Sub

    ''' <summary>
    ''' Créer un nouveau lien Rôle/Rôle dans la configuration.
    ''' </summary>
    ''' <param name="cfgName">La configuration dans sage.</param>
    ''' <param name="parentRoleName">Le rôle parent.</param>
    ''' <param name="childRoleName">Le rôle enfant.</param>
    ''' <remarks>
    ''' !!!! ATTETNION FAIRE DES BOUCLES DE RELATION EST POSSIBLE AVEC CETTE APPEL SOAP !!!!!
    ''' Les boucles 'a->a' et 'a->b, b->a'  cella ne dérange pas les futures appels SOAP,
    ''' mais le programme Sage DNA ne pourra plus ouvrir la configuration.
    ''' </remarks>
    Public Sub cfg_new_role_role_link(ByVal cfgName As String, ByVal parentRoleName As String, ByVal childRoleName As String)
        Dim ret As Integer

        '!BUG Promblème d'apostrophe: La solution de Eurikify est de doubler nous même les apostrophes.
        parentRoleName = doubleurApostrophe(parentRoleName)
        childRoleName = doubleurApostrophe(childRoleName)


        ret = BasicService.cfg_new_role_role_link(ServeurSage, BDSage, cfgName, parentRoleName, childRoleName)
        If ret <> 0 Then
            Throw New ApplicationException("Code de retour différent de 0 dans l'appel de cfg_new_role_role_link")
        End If
    End Sub

    ''' <summary>
    ''' Créer un nouveau lien entre une ressource et un Role.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub cfg_new_resource_role_link(ByVal cfgName As String, ByVal resName1 As String, ByVal resName2 As String, _
    ByVal resName3 As String, ByVal roleName As String)
        Dim ret As Integer

        '!BUG Promblème d'apostrophe: La solution de Eurikify est de doubler nous même les apostrophes.
        resName1 = doubleurApostrophe(resName1)
        resName2 = doubleurApostrophe(resName2)
        resName3 = doubleurApostrophe(resName3)
        roleName = doubleurApostrophe(roleName)


        ret = BasicService.cfg_new_resource_role_link(ServeurSage, BDSage, cfgName, resName1, resName2, resName3, roleName)
        If ret <> 0 Then
            Throw New ApplicationException("Code de retour différent de 0 dans l'appel de cfg_new_resource_role_link")
        End If
    End Sub

    ''' <summary>
    ''' Cette fonction va appliqué les données sur sage. Creer un nouveau lien Usager/Rôle.
    ''' </summary>
    Public Sub cfg_new_user_role_link(ByVal cfgName As String, ByVal personId As String, ByVal roleName As String)
        Dim ret As Integer

        roleName = doubleurApostrophe(roleName)

        ret = BasicService.cfg_new_user_role_link(ServeurSage, BDSage, cfgName, personId, roleName)
        If ret <> 0 Then
            Throw New ApplicationException("Code de retour différent de 0 dans l'appel de cfg_new_user_role_link")
        End If
    End Sub

    ''' <summary>
    ''' Cette fonction va appliqué les données sur sage. Creer un nouveau lien Usager/Ressource.
    ''' </summary>
    Public Sub cfg_new_user_resource_link(ByVal cfgName As String, ByVal personId As String, ByVal resname1 As String, ByVal resname2 As String, ByVal resname3 As String)
        Dim ret As Integer

        '!BUG Promblème d'apostrophe: La solution de Eurikify est de doubler nous même les apostrophes.
        resname1 = doubleurApostrophe(resname1)
        resname2 = doubleurApostrophe(resname2)
        resname3 = doubleurApostrophe(resname3)

        ret = BasicService.cfg_new_user_resource_link(ServeurSage, BDSage, cfgName, personId, resname1, resname2, resname3)
        If ret <> 0 Then
            Throw New ApplicationException("Code de retour différent de 0 dans l'appel de cfg_new_user_resource_link")
        End If
    End Sub

    ''' <summary>
    ''' Cette fonction va appliqué les données sur sage. Supprime le lien Usager/Rôle existant.
    ''' </summary>
    Public Sub cfg_remove_user_role_link(ByVal cfgName As String, ByVal userID As String, ByVal roleName As String)
        Dim ret As Integer

        roleName = doubleurApostrophe(roleName)

        ret = BasicService.cfg_remove_user_role_link(ServeurSage, BDSage, userID, roleName, cfgName)
        If ret <> 0 Then
            Throw New ApplicationException("Code de retour différent de 0 dans l'appel de cfg_remove_user_role_link")
        End If
    End Sub

    ''' <summary>
    ''' Cette fonction va appliqué les données sur sage. Supprime le lien Usager/Ressource existant.
    ''' </summary>
    Public Sub cfg_remove_user_resource_link(ByVal cfgName As String, ByVal userID As String, ByVal resname1 As String, ByVal resname2 As String, ByVal resname3 As String)
        Dim ret As Integer
        '!BUG Promblème d'apostrophe: La solution de Eurikify est de doubler nous même les apostrophes.
        resname1 = doubleurApostrophe(resname1)
        resname2 = doubleurApostrophe(resname2)
        resname3 = doubleurApostrophe(resname3)

        ret = BasicService.cfg_remove_user_resource_link(ServeurSage, BDSage, userID, resname1, resname2, resname3, cfgName)
        If ret <> 0 Then
            Throw New ApplicationException("Code de retour différent de 0 dans l'appel de cfg_remove_user_resource_link")
        End If
    End Sub

    ''' <summary>
    ''' Cette fonction va appliqué les données sur sage. Supprime le lien Ressource/Rôle existant.
    ''' </summary>
    Public Sub cfg_remove_resource_role_link(ByVal cfgName As String, ByVal resname1 As String, ByVal resname2 As String, ByVal resname3 As String, ByVal roleName As String)
        Dim ret As Integer

        '!BUG Promblème d'apostrophe: La solution de Eurikify est de doubler nous même les apostrophes.
        resname1 = doubleurApostrophe(resname1)
        resname2 = doubleurApostrophe(resname2)
        resname3 = doubleurApostrophe(resname3)
        roleName = doubleurApostrophe(roleName)

        ret = BasicService.cfg_remove_resource_role_link(ServeurSage, BDSage, resname1, resname2, resname3, roleName, cfgName)
        If ret <> 0 Then
            Throw New ApplicationException("Code de retour différent de 0 dans l'appel de cfg_remove_resource_role_link")
        End If
    End Sub

    ''' <summary>
    ''' Cette fonction va appliqué les données sur sage. Supprime le lien Rôle/Rôle existant.
    ''' </summary>
    Public Sub cfg_remove_role_role_link(ByVal cfgName As String, ByVal parentName As String, ByVal childName As String)
        Dim ret As Integer

        parentName = doubleurApostrophe(parentName)
        childName = doubleurApostrophe(childName)

        ret = BasicService.cfg_remove_role_role_link(ServeurSage, BDSage, parentName, childName, cfgName)
        If ret <> 0 Then
            Throw New ApplicationException("Code de retour différent de 0 dans l'appel de cfg_remove_role_role_link")
        End If
    End Sub

    
    ''' <summary>
    ''' Cette fonction va appliqué les données sur sage. Creer un nouvel utilisateur.
    ''' </summary>
    Public Sub udb_new_user(ByVal udbName As String, ByVal userID As String, _
    ByVal userName As String, ByVal org As String, ByVal orgType As String)
        Dim ret As Integer

        '! TODO Les champs de base ne peuvent être vide, minimum un espacement.
        userName = RemplirChampsVide(userName)
        org = RemplirChampsVide(org)
        orgType = RemplirChampsVide(orgType)

        ret = BasicService.udb_new_user(ServeurSage, BDSage, udbName, userID, userName, org, orgType)
        If ret <> 0 Then
            Throw New ApplicationException("Code de retour différent de 0 dans l'appel de udb_new_user")
        End If
    End Sub

    ''' <summary>
    ''' Cette fonction, créer le champ d'information pour l'utilisateur donnée.
    ''' </summary>
    Public Sub udb_new_user_field(ByVal udbName As String, ByVal personID As String, _
    ByVal fieldNum As Integer, ByVal FieldValue As String)
        Dim ret As Integer

        ret = BasicService.udb_new_user_field(ServeurSage, BDSage, udbName, personID, fieldNum, FieldValue)
        If ret <> 0 Then
            Throw New ApplicationException("Code de retour différent de 0 dans l'appel de udb_new_user_field")
        End If
    End Sub

    ''' <summary>
    ''' Cette fonction, modifier le champ d'information pour l'utilisateur donnée.
    ''' </summary>
    Public Sub udb_change_user_field(ByVal udbName As String, ByVal personID As String, _
    ByVal fieldName As String, ByVal fieldValue As String, ByVal fieldNumber As Integer)
        Dim ret As Integer
        '!ATTENTION Comprendre le ret = 1, est-ce bon ou pas bon, comment savoir si le champ est vide ou est plein?
        '!BUG Promblème d'apostrophe: La solution de Eurikify est de doubler nous même les apostrophes.
        Dim fieldValueDoubler As String = doubleurApostrophe(fieldValue)

        If FieldNumber = -1 Then
            ret = BasicService.udb_change_user_field(ServeurSage, BDSage, udbName, personID, fieldName, fieldValueDoubler)
        Else
            '! Pour la création du champ, doubler le field value n'est pas nécessaire.
            ret = BasicService.udb_new_user_field(ServeurSage, BDSage, udbName, personID, fieldNumber, fieldValue)
            If ret = -1 Then
                ret = BasicService.udb_change_user_field(ServeurSage, BDSage, udbName, personID, fieldName, fieldValueDoubler)
            End If
        End If

        If ret < 0 Then
            Throw New ApplicationException("Code de retour est inférieur à 0 dans l'appel de udb_change_user_field")
        End If
    End Sub


    ''' <summary>
    ''' Cette fonction va appliqué les données sur sage. Retire un utilisateur de la config.
    ''' </summary>
    Public Sub cfg_remove_configuration_user(ByVal cfgName As String, ByVal userID As String)
        Dim ret As Integer

        ret = BasicService.cfg_remove_configuration_user(ServeurSage, BDSage, userID, cfgName)
        If ret <> 0 Then
            Throw New ApplicationException("Code de retour différent de 0 dans l'appel de cfg_remove_configuration_user")
        End If
    End Sub

    ''' <summary>
    ''' Cette fonction va appliqué les données sur sage. Retire un rôle de la config.
    ''' </summary>
    Public Sub cfg_remove_configuration_role(ByVal cfgName As String, ByVal roleName As String)
        Dim ret As Integer

        roleName = doubleurApostrophe(roleName)

        ret = BasicService.cfg_remove_configuration_role(ServeurSage, BDSage, roleName, cfgName)
        If ret <> 0 Then
            Throw New ApplicationException("Code de retour différent de 0 dans l'appel de cfg_remove_configuration_role")
        End If
    End Sub

    ''' <summary>
    ''' Cette fonction va appliqué les données sur sage. Retire une ressource de la config.
    ''' </summary>
    Public Sub cfg_remove_configuration_resource(ByVal cfgName As String, ByVal resname1 As String, ByVal resname2 As String, ByVal resname3 As String)
        Dim ret As Integer

        '!BUG Promblème d'apostrophe: La solution de Eurikify est de doubler nous même les apostrophes.
        resname1 = doubleurApostrophe(resname1)
        resname2 = doubleurApostrophe(resname2)
        resname3 = doubleurApostrophe(resname3)

        ret = BasicService.cfg_remove_configuration_resource(ServeurSage, BDSage, resname1, resname2, resname3, cfgName)
        If ret <> 0 Then
            Throw New ApplicationException("Code de retour différent de 0 dans l'appel de cfg_remove_configuration_resource")
        End If
    End Sub
    'TODO: à compléter avec les autres méthodes (lorsqu'on aura le besoin)

#End Region

#Region "Fonctions de services"

    ''' <summary>
    ''' Double les apostrophes pour éviter l'injection SQL avec des envois à sage défectueux.
    ''' </summary>
    ''' <param name="texte"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function doubleurApostrophe(ByVal texte As String) As String
        Dim paramRetour As String
        Dim pattern As String = "'"
        Dim remplacement As String = "''"
        Dim rgx As New System.Text.RegularExpressions.Regex(pattern)

        If texte = Nothing Then
            Return ""
        End If

        paramRetour = rgx.Replace(texte, remplacement)

        Return paramRetour
        Return texte
    End Function

    ''' <summary>
    ''' Fonction de service. Transforme les textes vide en texte remplis d'un espace.
    ''' </summary>
    ''' <param name="texte">Texte à changé.</param>
    ''' <returns>Renvois si c'estu ntexte vide, un texte avec un espace dedans, sinon le texte original.</returns>
    ''' <remarks></remarks>
    Private Function RemplirChampsVide(ByVal texte As String) As String
        If texte = "" Or texte = Nothing Then
            Return " "
        End If
        Return texte
    End Function

#End Region

#Region "Classes de services"

    ''' <summary>
    ''' Classe de service. Sert à comparer 2 éléments de même nature et savoir s'il sont semblable.
    ''' </summary>
    ''' <typeparam name="T">Type des éléments à comparer.</typeparam>
    ''' <remarks></remarks>
    Private Class Comparateur(Of T)
        Implements IEqualityComparer(Of T)

        Private mFonctionCle As ElementComparatif
        Private mFonctionHashageCode As FonctionHashage

        Delegate Function ElementComparatif(ByVal element1 As T, ByVal element2 As T) As Boolean
        Delegate Function FonctionHashage(ByVal element1 As T) As Integer

        ''' <summary>
        ''' Constructeur de base.
        ''' </summary>
        ''' <param name="fonctionCle">Function qui démarque ce qui est à comparer en format string.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal fonctionCle As ElementComparatif, ByVal fonctionHashage As FonctionHashage)
            mFonctionCle = fonctionCle
            mFonctionHashageCode = fonctionHashage
        End Sub

        Public Function Equals1(ByVal x As T, ByVal y As T) As Boolean Implements System.Collections.Generic.IEqualityComparer(Of T).Equals
            If mFonctionCle(x, y) Then
                Return True
            End If
            Return False
        End Function

        Public Function GetHashCode1(ByVal obj As T) As Integer Implements System.Collections.Generic.IEqualityComparer(Of T).GetHashCode
            Return mFonctionHashageCode(obj)
        End Function
    End Class
#End Region

End Module
