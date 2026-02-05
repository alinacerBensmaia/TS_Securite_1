Imports Rrq.Securite.GestionAcces.TsCdConstanteNomChampNorm
'''-----------------------------------------------------------------------------
''' Project		: TS7N211_DiffSage
''' Class		: TsCaDiffSage
'''
'''-----------------------------------------------------------------------------
''' <summary>
''' Classe objet contenant les méthodes permettant de faire la différence entre deux configurations.
''' </summary>
''' <remarks></remarks>
'''-----------------------------------------------------------------------------
Public Class TsCaDiffSage
    Implements TsISourceDiff


#Region "--- Variables privées ---"

    Private Shared lstFileInfoUser As Reflection.FieldInfo() = GetType(TsCdSageUser).GetFields
    Private Shared lstFileInfoRole As Reflection.FieldInfo() = GetType(TsCdSageRole).GetFields
    Private Shared lstFileInfoRessr As Reflection.FieldInfo() = GetType(TsCdSageResource).GetFields

    Private _vieilleConfig As String
    Private _configAjour As String

#End Region

#Region "--- Constructeurs ---"

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="vielleConfig">Nom de la vieille configuration.</param>
    ''' <param name="configAjour">Nom de la configuration à jour</param>
    ''' <remarks>
    ''' Les configurations peuvent être égale à 'Nothing'. 
    ''' Cela a pour conséquence de faire des différences d'ajout complet ou d'effacement complet.
    ''' </remarks>
    Public Sub New(ByVal vielleConfig As String, ByVal configAjour As String)
        ValiderExistencesConfigs(vielleConfig, configAjour)

        _vieilleConfig = vielleConfig
        _configAjour = configAjour

    End Sub

#End Region

#Region "--- Méthodes ---"

    ''' <summary>
    ''' Vérifie si la configuration est existante dans Sage.
    ''' </summary>
    ''' <param name="nomConfig">Le nom de la configuration.</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' !!!! Attention !!!!!, si la configuration est inexistance le call string(cfg_get_databases_string) renvois "".
    ''' Si le retour change avec une mise à jour de Sage, la fonction doit être changé aussi.
    ''' </remarks>
    Public Shared Function ValiderExistenceConfig(ByVal nomConfig As String) As Boolean
        Dim paramRetour As Boolean = True

        If TsBaConfigSage.GetConfiguration(nomConfig) Is Nothing Then
            paramRetour = False
        End If

        Return paramRetour
    End Function

    ''' <summary>
    ''' Méthode servant à vérifier si les champs enrichit de la configuration sont toujours conforme à ceux définie dans ce programme.
    ''' </summary>
    ''' <param name="nomConfig">Nom de la configuration à vérifier.</param>
    ''' <returns>Vrai si la configuration est valide, sinon Faux.</returns>
    Public Shared Function ValiderIntegriteConfig(ByVal nomConfig As String, Optional ByRef descriptionErreur As String = Nothing) As Boolean
        Dim lstUserNorm As String() = {USER.NAME, USER.CN, USER.COURRIEL, USER.DATE_APPROBATION, USER.DATE_FIN, _
                                       USER.ORGANIZATION, USER.ORGANIZATION_TYPE, _
                                       USER.NOM, USER.NOM_UNITE, USER.PRENOM, USER.VILLE, _
                                       USER.GESTIONNAIRE, USER.TITRE, _
                                        USER.CHAMP_9, USER.CHAMP_10, USER.CHAMP_11, USER.CHAMP_12}
        Dim lstRessrNorm As String() = {RESSOURCE.CN, RESSOURCE.DATE_CREATION, RESSOURCE.DERN_MODIF, _
                                        RESSOURCE.DESCRIPTION, RESSOURCE.DETAILS, RESSOURCE.DETENTEUR}

        Dim infoConfig As TsCdSageConfiguration = TsBaConfigSage.GetConfiguration(nomConfig)
        Dim lstUserSage As String() = TsBaConfigSage.GetUserFields(nomConfig)
        Dim lstRessrSage As String() = TsBaConfigSage.GetResourceFields(nomConfig)

        Dim flagUserIntegrite As Boolean
        For Each champSage As String In lstUserSage
            flagUserIntegrite = False
            For Each champNorm As String In lstUserNorm
                If TsCdNomChampRef.MapChampRef().Item(champNorm).NomChamp = champSage Then
                    flagUserIntegrite = True
                    Exit For
                End If
            Next
            If flagUserIntegrite = False Then
                descriptionErreur = String.Format("Le champ «{0}» présent dans l'UDB de sage n'est pas connu de l'application.", champSage)
                Exit For
            End If
        Next

        If flagUserIntegrite = False Then
            Return False
        End If

        Dim flagRessrIntegrite As Boolean = False
        For Each champSage As String In lstRessrSage
            For Each champNorm As String In lstRessrNorm
                If TsCdNomChampRef.MapChampRef().Item(champNorm).NomChamp = champSage Then
                    flagRessrIntegrite = True
                    Exit For
                End If
            Next
            If flagRessrIntegrite = False Then
                descriptionErreur = String.Format("Le champ «{0}» présent dans la RDB de sage n'est pas connu de l'application.", champSage)
                Exit For
            End If
        Next

        If flagRessrIntegrite = False Then
            Return False
        End If

        Return True
    End Function

    ''' <summary>
    ''' Cette méthode obtient la différence des liens utilisateurs/ressources direct entre la vieille config et la configuration mise à jour.
    ''' </summary>
    ''' <param name="cible">Filtre définie sur resname3 de la ressource. Définie de quel système vous désirer la différence</param>
    ''' <param name="lstAjouter">Une liste préinitialisé pour contenir les ajouts de la différence.</param>
    ''' <param name="lstSupprimer">Une liste préinitialisé pour contenir les suppresions de la différence.</param>
    Public Sub ObtnrDiffrUtilisateurRessourceDirect(ByVal cible As String, ByRef lstAjouter As List(Of TsCdConnxUserRessr), _
    ByRef lstSupprimer As List(Of TsCdConnxUserRessr)) Implements TsISourceDiff.ObtnrDiffrUserRessrDirect
        Dim lstUtilisateurRessourceAncn As TsCdSageUserResLinkCollection = ObtenirListe(Of TsCdSageUserResLinkCollection)(_vieilleConfig, AddressOf TsBaConfigSage.GetUserResourcesLinks)
        Dim lstUtilisateurRessourceAjour As TsCdSageUserResLinkCollection = ObtenirListe(Of TsCdSageUserResLinkCollection)(_configAjour, AddressOf TsBaConfigSage.GetUserResourcesLinks)

        Dim liensUtilisateurRessourceAjouter As New TsCdCollectionSage(Of TsCdSageUserResLink)
        Dim liensUtilisateurRessourceSupprimer As New TsCdCollectionSage(Of TsCdSageUserResLink)

        Dim lstUtilisateurRessourceFiltrerAncn As New TsCdSageUserResLinkCollection
        Dim lstUtilisateurRessourceFiltrerAjour As New TsCdSageUserResLinkCollection

        For Each lienRoleRessource As TsCdSageUserResLink In lstUtilisateurRessourceAncn
            If String.Compare(lienRoleRessource.ResName3, cible, True) = 0 Then
                lstUtilisateurRessourceFiltrerAncn.Add(lienRoleRessource)
            End If
        Next

        For Each lienRoleRessource As TsCdSageUserResLink In lstUtilisateurRessourceAjour
            If String.Compare(lienRoleRessource.ResName3, cible, True) = 0 Then
                lstUtilisateurRessourceFiltrerAjour.Add(lienRoleRessource)
            End If
        Next

        Decomposition(Of TsCdSageUserResLink, TsCdCollectionSage(Of TsCdSageUserResLink))(lstUtilisateurRessourceFiltrerAncn, lstUtilisateurRessourceFiltrerAjour, _
        liensUtilisateurRessourceAjouter, liensUtilisateurRessourceSupprimer, New Comparateur(Of TsCdSageUserResLink)(AddressOf Comparaison))

        '! Taduire les données en format TS7N311_ConnecxionCibles
        lstAjouter = liensUtilisateurRessourceAjouter.List.ConvertAll(Of TsCdConnxUserRessr) _
            (New Converter(Of TsCdSageUserResLink, TsCdConnxUserRessr)(AddressOf Conversion))
        lstSupprimer = liensUtilisateurRessourceSupprimer.List.ConvertAll(Of TsCdConnxUserRessr) _
            (New Converter(Of TsCdSageUserResLink, TsCdConnxUserRessr)(AddressOf Conversion))

    End Sub

    ''' <summary>
    ''' Cette méthode obtient la différence des liens utilisateurs/ressources en passant par les rôles et les liens directs.
    ''' </summary>
    ''' <param name="cible">Filtre définie sur resname3 de la ressource. Définie de quel système vous désirer la différence</param>
    ''' <param name="lstAjouter">Une liste préinitialisé pour contenir les ajouts de la différence.</param>
    ''' <param name="lstSupprimer">Une liste préinitialisé pour contenir les suppresions de la différence.</param>
    Public Sub ObtnrDiffrUilisateurRessourceRecurcif(ByVal cible As String, ByRef lstAjouter As List(Of TsCdConnxUserRessr), _
    ByRef lstSupprimer As List(Of TsCdConnxUserRessr)) Implements TsISourceDiff.ObtnrDiffrUserRessrRecurcif
        Dim lstUtilisateurRessourceAncn As TsCdSageUserResLinkCollection
        Dim lstUtilisateurRessourceAjour As TsCdSageUserResLinkCollection

        If _vieilleConfig <> Nothing Then
            lstUtilisateurRessourceAncn = ObtnrLiensRecurcifUtilisateurRessource(_vieilleConfig, cible)
        Else
            lstUtilisateurRessourceAncn = New TsCdSageUserResLinkCollection
        End If

        If _configAjour <> Nothing Then
            lstUtilisateurRessourceAjour = ObtnrLiensRecurcifUtilisateurRessource(_configAjour, cible)
        Else
            lstUtilisateurRessourceAjour = New TsCdSageUserResLinkCollection

        End If

        Dim liensUtilisateurRessourceAjouter As New TsCdCollectionSage(Of TsCdSageUserResLink)
        Dim liensUtilisateurRessourceSupprimer As New TsCdCollectionSage(Of TsCdSageUserResLink)

        Dim lstUtilisateurRessourceFiltrerAncn As New TsCdSageUserResLinkCollection
        Dim lstUtilisateurRessourceFiltrerAjour As New TsCdSageUserResLinkCollection

        Decomposition(Of TsCdSageUserResLink, TsCdCollectionSage(Of TsCdSageUserResLink))(lstUtilisateurRessourceAncn, lstUtilisateurRessourceAjour, _
        liensUtilisateurRessourceAjouter, liensUtilisateurRessourceSupprimer, New Comparateur(Of TsCdSageUserResLink)(AddressOf Comparaison))

        '! Taduire les données en format TS7N311_ConnecxionCibles
        lstAjouter = liensUtilisateurRessourceAjouter.List.ConvertAll(Of TsCdConnxUserRessr) _
            (New Converter(Of TsCdSageUserResLink, TsCdConnxUserRessr)(AddressOf Conversion))
        lstSupprimer = liensUtilisateurRessourceSupprimer.List.ConvertAll(Of TsCdConnxUserRessr) _
            (New Converter(Of TsCdSageUserResLink, TsCdConnxUserRessr)(AddressOf Conversion))

    End Sub

    ''' <summary>
    ''' Cette méthode obtient la différence des liens utilisateurs/rôles entre la vieille config et la configuration mise à jour.
    ''' </summary>
    ''' <param name="lstAjouter">Une liste préinitialisé pour contenir les ajouts de la différence.</param>
    ''' <param name="lstSupprimer">Une liste préinitialisé pour contenir les suppresions de la différence.</param>
    Public Sub ObtnrDiffrUtilisateurRole(ByRef lstAjouter As List(Of TsCdConnxUserRole), _
    ByRef lstSupprimer As List(Of TsCdConnxUserRole)) Implements TsISourceDiff.ObtnrDiffrUserRole
        Dim lstUtilisateurRoleAncn As TsCdSageUserRoleLinkCollection = ObtenirListe(Of TsCdSageUserRoleLinkCollection)(_vieilleConfig, AddressOf TsBaConfigSage.GetUserRolesLinks)
        Dim lstUtilisateurRoleAjour As TsCdSageUserRoleLinkCollection = ObtenirListe(Of TsCdSageUserRoleLinkCollection)(_configAjour, AddressOf TsBaConfigSage.GetUserRolesLinks)

        Dim liensUitilisateurRoleAjouter As New TsCdCollectionSage(Of TsCdSageUserRoleLink)
        Dim liensUitilisateurRoleSupprimer As New TsCdCollectionSage(Of TsCdSageUserRoleLink)

        Decomposition(Of TsCdSageUserRoleLink, TsCdCollectionSage(Of TsCdSageUserRoleLink))(lstUtilisateurRoleAncn, lstUtilisateurRoleAjour, _
        liensUitilisateurRoleAjouter, liensUitilisateurRoleSupprimer, New Comparateur(Of TsCdSageUserRoleLink)(AddressOf Comparaison))

        '! Traduire les données en format TS7N311_ConnecxionCibles
        lstAjouter = liensUitilisateurRoleAjouter.List.ConvertAll(Of TsCdConnxUserRole) _
            (New Converter(Of TsCdSageUserRoleLink, TsCdConnxUserRole)(AddressOf Conversion))
        lstSupprimer = liensUitilisateurRoleSupprimer.List.ConvertAll(Of TsCdConnxUserRole) _
            (New Converter(Of TsCdSageUserRoleLink, TsCdConnxUserRole)(AddressOf Conversion))
    End Sub

    ''' <summary>
    ''' Cette méthode obtient la différence des liens rôles/rôles entre la vieille config et la configuration mise à jour.
    ''' </summary>
    ''' <param name="lstAjouter">Une liste préinitialisé pour contenir les ajouts de la différence.</param>
    ''' <param name="lstSupprimer">Une liste préinitialisé pour contenir les suppresions de la différence.</param>
    Public Sub ObtnrDiffrRoleRole(ByRef lstAjouter As List(Of TsCdConnxRoleRole), _
    ByRef lstSupprimer As List(Of TsCdConnxRoleRole)) Implements TsISourceDiff.ObtnrDiffrRoleRole
        Dim lstRoleRoleAncn As TsCdSageRoleRoleLinkCollection = ObtenirListe(Of TsCdSageRoleRoleLinkCollection)(_vieilleConfig, AddressOf TsBaConfigSage.GetRoleSubRolesLinks)
        Dim lstRoleRoleAjour As TsCdSageRoleRoleLinkCollection = ObtenirListe(Of TsCdSageRoleRoleLinkCollection)(_configAjour, AddressOf TsBaConfigSage.GetRoleSubRolesLinks)

        Dim liensRoleRoleAjouter As New TsCdCollectionSage(Of TsCdSageRoleRoleLink)
        Dim liensRoleRoleSupprimer As New TsCdCollectionSage(Of TsCdSageRoleRoleLink)

        Decomposition(Of TsCdSageRoleRoleLink, TsCdCollectionSage(Of TsCdSageRoleRoleLink))(lstRoleRoleAncn, lstRoleRoleAjour, _
        liensRoleRoleAjouter, liensRoleRoleSupprimer, New Comparateur(Of TsCdSageRoleRoleLink)(AddressOf Comparaison))

        '! Traduire les données en format TS7N311_ConnecxionCibles.
        lstAjouter = liensRoleRoleAjouter.List.ConvertAll(Of TsCdConnxRoleRole) _
            (New Converter(Of TsCdSageRoleRoleLink, TsCdConnxRoleRole)(AddressOf Conversion))
        lstSupprimer = liensRoleRoleSupprimer.List.ConvertAll(Of TsCdConnxRoleRole) _
            (New Converter(Of TsCdSageRoleRoleLink, TsCdConnxRoleRole)(AddressOf Conversion))
    End Sub

    ''' <summary>
    ''' Cette méthode obtient la différence des liens rôles/ressources entre la vieille config et la configuration mise à jour.
    ''' </summary>
    ''' <param name="cible">Filtre définie sur resname3 de la ressource. Définie de quel système vous désirer la différence.</param>
    ''' <param name="lstAjouter">Une liste préinitialisé pour contenir les ajouts de la différence.</param>
    ''' <param name="lstSupprimer">Une liste préinitialisé pour contenir les suppresions de la différence.</param>
    Public Sub ObtnrDiffrRoleRessource(ByVal cible As String, ByRef lstAjouter As List(Of TsCdConnxRoleRessr), _
    ByRef lstSupprimer As List(Of TsCdConnxRoleRessr)) Implements TsISourceDiff.ObtnrDiffrRoleRessr

        Dim lstRoleRessourceAncn As TsCdSageRoleResLinkCollection = ObtenirListe(Of TsCdSageRoleResLinkCollection)(_vieilleConfig, AddressOf TsBaConfigSage.GetRoleResourcesLinks)
        Dim lstRoleRessourceAjour As TsCdSageRoleResLinkCollection = ObtenirListe(Of TsCdSageRoleResLinkCollection)(_configAjour, AddressOf TsBaConfigSage.GetRoleResourcesLinks)

        Dim lstRoleRessourceFiltrerAncn As New TsCdSageRoleResLinkCollection
        Dim lstRoleRessourceFiltrerAjour As New TsCdSageRoleResLinkCollection

        For Each lienRoleRessource As TsCdSageRoleResLink In lstRoleRessourceAncn
            If String.Compare(lienRoleRessource.ResName3, cible, True) = 0 Then
                lstRoleRessourceFiltrerAncn.Add(lienRoleRessource)
            End If
        Next

        For Each lienRoleRessource As TsCdSageRoleResLink In lstRoleRessourceAjour
            If String.Compare(lienRoleRessource.ResName3, cible, True) = 0 Then
                lstRoleRessourceFiltrerAjour.Add(lienRoleRessource)
            End If
        Next

        Dim liensRoleRessourceAjouter As New TsCdCollectionSage(Of TsCdSageRoleResLink)
        Dim liensRoleRessourceSupprimer As New TsCdCollectionSage(Of TsCdSageRoleResLink)

        Decomposition(Of TsCdSageRoleResLink, TsCdCollectionSage(Of TsCdSageRoleResLink))(lstRoleRessourceFiltrerAncn, lstRoleRessourceFiltrerAjour, _
        liensRoleRessourceAjouter, liensRoleRessourceSupprimer, New Comparateur(Of TsCdSageRoleResLink)(AddressOf Comparaison))

        '! Taduire les données en format TS7N311_ConnecxionCibles.
        lstAjouter = liensRoleRessourceAjouter.List.ConvertAll(Of TsCdConnxRoleRessr) _
            (New Converter(Of TsCdSageRoleResLink, TsCdConnxRoleRessr)(AddressOf Conversion))
        lstSupprimer = liensRoleRessourceSupprimer.List.ConvertAll(Of TsCdConnxRoleRessr) _
            (New Converter(Of TsCdSageRoleResLink, TsCdConnxRoleRessr)(AddressOf Conversion))
    End Sub

    ''' <summary>
    ''' Cette méthode obtient la différence de la liste d'utilisateurs de la vieille config et celle de la configuration mise à jour.
    ''' </summary>
    ''' <param name="lstAjouter">Une liste préinitialisé pour contenir les ajouts de la différence.</param>
    ''' <param name="lstSupprimer">Une liste préinitialisé pour contenir les suppresions de la différence.</param>
    Public Sub ObtnrDiffrUtilisateur(ByRef lstAjouter As List(Of TsCdConnxUser), _
    ByRef lstSupprimer As List(Of TsCdConnxUser)) Implements TsISourceDiff.ObtnrDiffrUser

        Dim lstUtilisateursAncn As TsCdSageUserCollection = ObtenirListe(Of TsCdSageUserCollection)(_vieilleConfig, AddressOf TsBaConfigSage.GetConfigurationUsers)
        Dim lstUtilisateursAjour As TsCdSageUserCollection = ObtenirListe(Of TsCdSageUserCollection)(_configAjour, AddressOf TsBaConfigSage.GetConfigurationUsers)

        Dim utilisateursAjouter As New TsCdCollectionSage(Of TsCdSageUser)
        Dim utilisateursSupprimer As New TsCdCollectionSage(Of TsCdSageUser)

        Decomposition(Of TsCdSageUser, TsCdCollectionSage(Of TsCdSageUser))(lstUtilisateursAncn, lstUtilisateursAjour, _
        utilisateursAjouter, utilisateursSupprimer, New Comparateur(Of TsCdSageUser)(AddressOf Comparaison))

        '! Taduire les données en format TS7N311_ConnecxionCibles.
        lstAjouter = utilisateursAjouter.List.ConvertAll(Of TsCdConnxUser) _
            (New Converter(Of TsCdSageUser, TsCdConnxUser)(AddressOf Conversion))
        lstSupprimer = utilisateursSupprimer.List.ConvertAll(Of TsCdConnxUser) _
            (New Converter(Of TsCdSageUser, TsCdConnxUser)(AddressOf Conversion))
    End Sub

    ''' <summary>
    ''' Cette méthode obtient la différence de la liste d'utilisateurs de la vieille config et celle de la configuration mise à jour.
    ''' </summary>
    ''' <param name="lstAjouter">Une liste préinitialisé pour contenir les ajouts de la différence.</param>
    ''' <param name="lstSupprimer">Une liste préinitialisé pour contenir les suppresions de la différence.</param>
    Public Sub ObtnrDiffrRole(ByRef lstAjouter As List(Of TsCdConnxRole), _
    ByRef lstSupprimer As List(Of TsCdConnxRole)) Implements TsISourceDiff.ObtnrDiffrRole

        Dim lstRolesAncn As TsCdSageRoleCollection = ObtenirListe(Of TsCdSageRoleCollection)(_vieilleConfig, AddressOf TsBaConfigSage.GetConfigurationRoles)
        Dim lstRolesAjour As TsCdSageRoleCollection = ObtenirListe(Of TsCdSageRoleCollection)(_configAjour, AddressOf TsBaConfigSage.GetConfigurationRoles)

        Dim rolesAjouter As New TsCdCollectionSage(Of TsCdSageRole)
        Dim rolesSupprimer As New TsCdCollectionSage(Of TsCdSageRole)

        Decomposition(Of TsCdSageRole, TsCdCollectionSage(Of TsCdSageRole))(lstRolesAncn, lstRolesAjour, _
        rolesAjouter, rolesSupprimer, New Comparateur(Of TsCdSageRole)(AddressOf Comparaison))

        '! Taduire les données en format TS7N311_ConnecxionCibles.
        lstAjouter = rolesAjouter.List.ConvertAll(Of TsCdConnxRole) _
            (New Converter(Of TsCdSageRole, TsCdConnxRole)(AddressOf Conversion))
        lstSupprimer = rolesSupprimer.List.ConvertAll(Of TsCdConnxRole) _
            (New Converter(Of TsCdSageRole, TsCdConnxRole)(AddressOf Conversion))
    End Sub

    ''' <summary>
    ''' Cette méthode obtient la différence de la liste d'utilisateurs de la vieille config et celle de la configuration mise à jour.
    ''' </summary>
    ''' <param name="lstAjouter">Une liste préinitialisé pour contenir les ajouts de la différence.</param>
    ''' <param name="lstSupprimer">Une liste préinitialisé pour contenir les suppresions de la différence.</param>
    Public Sub ObtnrDiffrRessource(ByVal cible As String, ByRef lstAjouter As List(Of TsCdConnxRessr), _
    ByRef lstSupprimer As List(Of TsCdConnxRessr)) Implements TsISourceDiff.ObtnrDiffrRessource

        Dim lstRessourcesAncn As TsCdSageResourceCollection = ObtenirListe(Of TsCdSageResourceCollection)(_vieilleConfig, AddressOf TsBaConfigSage.GetConfigurationResources)
        Dim lstRessourcesAjour As TsCdSageResourceCollection = ObtenirListe(Of TsCdSageResourceCollection)(_configAjour, AddressOf TsBaConfigSage.GetConfigurationResources)

        Dim lstRessourceFiltrerAncn As New TsCdSageResourceCollection
        Dim lstRessourceFiltrerAjour As New TsCdSageResourceCollection

        For Each ressource As TsCdSageResource In lstRessourcesAncn
            If String.Compare(ressource.ResName3, cible, True) = 0 Then
                lstRessourceFiltrerAncn.Add(ressource)
            End If
        Next

        For Each ressource As TsCdSageResource In lstRessourcesAjour
            If String.Compare(ressource.ResName3, cible, True) = 0 Then
                lstRessourceFiltrerAjour.Add(ressource)
            End If
        Next

        Dim ressourcesAjouter As New TsCdCollectionSage(Of TsCdSageResource)
        Dim ressourcesSupprimer As New TsCdCollectionSage(Of TsCdSageResource)

        Decomposition(Of TsCdSageResource, TsCdCollectionSage(Of TsCdSageResource))(lstRessourceFiltrerAncn, lstRessourceFiltrerAjour, _
        ressourcesAjouter, ressourcesSupprimer, New Comparateur(Of TsCdSageResource)(AddressOf Comparaison))

        '! Taduire les données en format TS7N311_ConnecxionCibles.
        lstAjouter = ressourcesAjouter.List.ConvertAll(Of TsCdConnxRessr) _
            (New Converter(Of TsCdSageResource, TsCdConnxRessr)(AddressOf Conversion))
        lstSupprimer = ressourcesSupprimer.List.ConvertAll(Of TsCdConnxRessr) _
            (New Converter(Of TsCdSageResource, TsCdConnxRessr)(AddressOf Conversion))
    End Sub

    ''' <summary>
    ''' Cette méthode définit les différences d'attributs des utilisateurs entre les deux configurations.
    ''' </summary>
    ''' <param name="lstAjouter">Une liste préinitialisé pour contenir les ajouts d'attributs des utilisateurs.</param>
    ''' <param name="lstSupprimer">Une liste préinitialisé pour contenir les suppresions d'attributs des utilisateurs.</param>
    Public Sub ObtnrDiffrAttrbUser(ByRef lstAjouter As List(Of TsCdConnxUserAttrb), _
    ByRef lstSupprimer As List(Of TsCdConnxUserAttrb)) Implements TsISourceDiff.ObtnrDiffrAttrbUser
        '!/////////////////////////////////////
        '!// Préparation des dictionnaires et des listes de traitements.
        '!/////////////////////////////////////
        Dim lstUsersAncn As New TsCdSageUserCollection
        Dim lstUsersAjour As New TsCdSageUserCollection

        If Not String.IsNullOrEmpty(_vieilleConfig) Then
            lstUsersAncn = ObtenirListe(Of TsCdSageUserCollection)(_vieilleConfig, AddressOf TsBaConfigSage.GetConfigurationUsers)
        End If
        If Not String.IsNullOrEmpty(_configAjour) Then
            lstUsersAjour = ObtenirListe(Of TsCdSageUserCollection)(_configAjour, AddressOf TsBaConfigSage.GetConfigurationUsers)
        End If

        Dim DictioAnc As New Dictionary(Of String, TsCdSageUser)
        Dim DictioAjour As New Dictionary(Of String, TsCdSageUser)

        RemplirDictio(Of TsCdSageUser)(DictioAnc, lstUsersAncn, Function(o As TsCdSageUser) o.PersonID)
        RemplirDictio(Of TsCdSageUser)(DictioAjour, lstUsersAjour, Function(o As TsCdSageUser) o.PersonID)

        '!/////////////////////////////////////

        Dim lstUsersAncnConfig As TsCdSageUserCollection = ObtenirListe(Of TsCdSageUserCollection)(_vieilleConfig, AddressOf TsBaConfigSage.GetConfigurationUsers)
        Dim lstUsersAjourConfig As TsCdSageUserCollection = ObtenirListe(Of TsCdSageUserCollection)(_configAjour, AddressOf TsBaConfigSage.GetConfigurationUsers)

        For Each utilisateur As TsCdSageUser In lstUsersAjourConfig
            If DictioAnc.ContainsKey(utilisateur.PersonID) = False Then
                If DictioAjour.ContainsKey(utilisateur.PersonID) = True Then
                    Dim sageUser As New TsCdSageUser
                    sageUser.PersonID = utilisateur.PersonID
                    ComparerUserAttrbs(sageUser, DictioAjour.Item(utilisateur.PersonID), lstAjouter, lstSupprimer)
                End If
            Else
                ComparerUserAttrbs(DictioAnc.Item(utilisateur.PersonID), DictioAjour.Item(utilisateur.PersonID), lstAjouter, lstSupprimer)
            End If

            '! Nous ne feront pas de suppresion d'attributs des éléments effacés, car pour le moment 
            '! seulement SAGE est concerné par cette fonction et la supression d'attributs est inutile. 
            ' For Each utilisateur As TsCdSageUser In lstUsersAncConfig
            ' ...
        Next

    End Sub

    ''' <summary>
    ''' Cette méthode définit les différences d'attributs des rôles entre les deux configurations.
    ''' </summary>
    ''' <param name="lstAjouter">Une liste préinitialisé pour contenir les ajouts d'attributs des rôles.</param>
    ''' <param name="lstSupprimer">Une liste préinitialisé pour contenir les suppresions d'attributs des rôles.</param>
    Public Sub ObtnrDiffrAttrbRole(ByRef lstAjouter As List(Of TsCdConnxRoleAttrb), _
    ByRef lstSupprimer As List(Of TsCdConnxRoleAttrb)) Implements TsISourceDiff.ObtnrDiffrAttrbRole
        '!/////////////////////////////////////
        '!// Préparation des dictionnaires et des listes de traitements.
        '!/////////////////////////////////////

        Dim lstRolesAncn As TsCdSageRoleCollection = ObtenirListe(Of TsCdSageRoleCollection)(_vieilleConfig, AddressOf TsBaConfigSage.GetConfigurationRoles)
        Dim lstRolesAjour As TsCdSageRoleCollection = ObtenirListe(Of TsCdSageRoleCollection)(_configAjour, AddressOf TsBaConfigSage.GetConfigurationRoles)

        Dim DictioAnc As New Dictionary(Of String, TsCdSageRole)

        RemplirDictio(Of TsCdSageRole)(DictioAnc, lstRolesAncn, Function(o As TsCdSageRole) o.Name)

        '!/////////////////////////////////////

        For Each role As TsCdSageRole In lstRolesAjour
            If DictioAnc.ContainsKey(role.Name) = False Then
                Dim sageRole As New TsCdSageRole
                sageRole.Name = role.Name
                ComparerRolesAttrbs(sageRole, role, lstAjouter, lstSupprimer)
            Else
                ComparerRolesAttrbs(DictioAnc.Item(role.Name), role, lstAjouter, lstSupprimer)
            End If
        Next
        '! Nous ne feront pas de suppresion d'attributs des éléments effacés, car pour le moment 
        '! seulement SAGE est concerné par cette fonction et la supression d'attributs est inutile. 
        'For Each role As TsCdSageRole In lstRolesAjour
        ' ...

    End Sub

    ''' <summary>
    ''' Cette méthode définit les différences d'attributs des ressources entre les deux configurations.
    ''' </summary>
    ''' <param name="lstAjouter">Une liste préinitialisé pour contenir les ajouts d'attributs des ressources.</param>
    ''' <param name="lstSupprimer">Une liste préinitialisé pour contenir les suppresions d'attributs des ressources.</param>
    Public Sub ObtnrDiffrAttrbRessr(ByVal cible As String, ByRef lstAjouter As List(Of TsCdConnxRessrAttrb), _
    ByRef lstSupprimer As List(Of TsCdConnxRessrAttrb)) Implements TsISourceDiff.ObtnrDiffrAttrbRessr
        '!/////////////////////////////////////
        '!// Préparation des dictionnaires et des listes de traitements.
        '!/////////////////////////////////////
        Dim lstRessrsAncn As New TsCdSageResourceCollection
        Dim lstRessrsAjour As New TsCdSageResourceCollection

        If Not String.IsNullOrEmpty(_vieilleConfig) Then
            lstRessrsAncn = ObtenirListe(Of TsCdSageResourceCollection)(_vieilleConfig, AddressOf TsBaConfigSage.GetConfigurationResources)
        End If
        If Not String.IsNullOrEmpty(_configAjour) Then
            lstRessrsAjour = ObtenirListe(Of TsCdSageResourceCollection)(_configAjour, AddressOf TsBaConfigSage.GetConfigurationResources)
        End If

        Dim DictioAnc As New Dictionary(Of String, TsCdSageResource)
        Dim DictioAjour As New Dictionary(Of String, TsCdSageResource)

        RemplirDictio(Of TsCdSageResource)(DictioAnc, lstRessrsAncn, Function(o As TsCdSageResource) o.ResName1 + o.ResName2 + o.ResName3)
        RemplirDictio(Of TsCdSageResource)(DictioAjour, lstRessrsAjour, Function(o As TsCdSageResource) o.ResName1 + o.ResName2 + o.ResName3)

        '!/////////////////////////////////////

        Dim lstRessrsAncnConfig As TsCdSageResourceCollection = ObtenirListe(Of TsCdSageResourceCollection)(_vieilleConfig, AddressOf TsBaConfigSage.GetConfigurationResources)
        Dim lstRessrsAjourConfig As TsCdSageResourceCollection = ObtenirListe(Of TsCdSageResourceCollection)(_configAjour, AddressOf TsBaConfigSage.GetConfigurationResources)

        For Each ressource As TsCdSageResource In lstRessrsAjourConfig
            If ressource.ResName3 = cible Then
                Dim cleDictio As String = ressource.ResName1 + ressource.ResName2 + ressource.ResName3
                If DictioAnc.ContainsKey(cleDictio) = False Then
                    If DictioAjour.ContainsKey(cleDictio) = True Then
                        Dim elementDictionRessr As TsCdSageResource = DictioAjour.Item(cleDictio)

                        Dim ressrSage As New TsCdSageResource()
                        ressrSage.ResName1 = elementDictionRessr.ResName1
                        ressrSage.ResName2 = elementDictionRessr.ResName2
                        ressrSage.ResName3 = elementDictionRessr.ResName3

                        ComparerRessrsAttrbs(ressrSage, elementDictionRessr, lstAjouter, lstSupprimer)
                    End If
                Else
                    ComparerRessrsAttrbs(DictioAnc.Item(cleDictio), DictioAjour.Item(cleDictio), lstAjouter, lstSupprimer)
                End If
            End If
        Next

        '! Nous ne feront pas de suppresion d'attributs des éléments effacés, car pour le moment 
        '! seulement SAGE est concerné par cette fonction et la supression d'attributs est inutile. 
        ' For Each ressource As TsCdSageResource In lstRessrsAncConfig
        ' ...

    End Sub

    ''' <summary>
    ''' Revois une liste des configurations disponibles dans Sage.
    ''' </summary>
    ''' <returns>Liste de configurations Sage.</returns>
    Public Shared Function ObtenirListeConfig() As List(Of String)
        Dim paramRetour As New List(Of String)
        Dim configs As TsCdSageConfigurationCollection = TsBaConfigSage.GetConfigurations()

        For Each config As TsCdSageConfiguration In configs
            paramRetour.Add(config.ConfigurationName)
        Next
        Return paramRetour
    End Function

    ''' <summary>
    ''' Vide le cache du service sage
    ''' </summary>
    Public Shared Sub ViderCache()
        TsBaConfigSage.ClearCache()
    End Sub

#End Region

#Region "--- Fonctions de services ---"

    ''' <summary>
    ''' Fonction de service. Permette de comparer les attributs de l'utilisateur de la vieille configuration et la configuration à jour. 
    ''' Les différences seront ajoutées aux listes d'entrée.
    ''' </summary>
    ''' <param name="vieuxUser">L'utilisateur de la vieille configuration.</param>
    ''' <param name="majUser">L'utilisateur de la configuration à jour.</param>
    ''' <param name="lstAjout">La liste qui recevera les ajouts d'attributs.</param>
    ''' <param name="lstSupp">La liste qui recevera les supressions d'attributs.</param>
    ''' <remarks></remarks>
    Private Shared Sub ComparerUserAttrbs(ByVal vieuxUser As TsCdSageUser, ByVal majUser As TsCdSageUser, ByVal lstAjout As List(Of TsCdConnxUserAttrb), ByVal lstSupp As List(Of TsCdConnxUserAttrb))
        If vieuxUser.PersonID <> majUser.PersonID Then
            Throw New ApplicationException("Les utilisateurs ne possèdent pas les même ID.")
        End If

        For Each f As System.Reflection.FieldInfo In lstFileInfoUser
            If f.Name = "UserID" Or f.Name = "PersonID" Then Continue For

            ComparerUserAttrb(vieuxUser, majUser, f, vieuxUser.PersonID, lstAjout, lstSupp)
        Next
    End Sub

    ''' <summary>
    ''' Fonction de service. Permette de comparer les attributs du rôle de la vieille configuration et la configuration à jour. 
    ''' Les différences seront ajoutées aux listes d'entrée.
    ''' </summary>
    ''' <param name="vieuxRole">Le rôle de la vieille configuration.</param>
    ''' <param name="majRole">Le rôle de la configuration à jour.</param>
    ''' <param name="lstAjout">La liste qui recevera les ajouts d'attributs.</param>
    ''' <param name="lstSupp">La liste qui recevera les supressions d'attributs.</param>
    ''' <remarks></remarks>
    Private Shared Sub ComparerRolesAttrbs(ByVal vieuxRole As TsCdSageRole, ByVal majRole As TsCdSageRole, ByVal lstAjout As List(Of TsCdConnxRoleAttrb), ByVal lstSupp As List(Of TsCdConnxRoleAttrb))
        If vieuxRole.Name <> majRole.Name Then
            Throw New ApplicationException("Les rôles ne possèdent pas la même clé d'identification.")
        End If

        For Each f As System.Reflection.FieldInfo In lstFileInfoRole
            If f.Name = "Name" Or f.Name = "RoleID" Then Continue For
            ComparerRoleAttrb(vieuxRole, majRole, f, vieuxRole.Name, lstAjout, lstSupp)
        Next
    End Sub

    ''' <summary>
    ''' Fonction de service. Permette de comparer les attributs de la ressource de la vieille configuration et la configuration à jour. 
    ''' Les différences seront ajoutées aux listes d'entrée.
    ''' </summary>
    ''' <param name="vieilleRessr">La ressource de la vieille configuration.</param>
    ''' <param name="majRessr">La ressource de la configuration à jour.</param>
    ''' <param name="lstAjout">La liste qui recevera les ajouts d'attributs.</param>
    ''' <param name="lstSupp">La liste qui recevera les supressions d'attributs.</param>
    ''' <remarks></remarks>
    Private Shared Sub ComparerRessrsAttrbs(ByVal vieilleRessr As TsCdSageResource, ByVal majRessr As TsCdSageResource, ByVal lstAjout As List(Of TsCdConnxRessrAttrb), ByVal lstSupp As List(Of TsCdConnxRessrAttrb))
        Dim cleVieille As String = vieilleRessr.ResName1 + vieilleRessr.ResName2 + vieilleRessr.ResName3
        Dim cleMaj As String = majRessr.ResName1 + majRessr.ResName2 + majRessr.ResName3
        If cleVieille <> cleMaj Then
            Throw New ApplicationException("Les ressources ne possèdent pas les même clées d'identifications.")
        End If

        For Each f As System.Reflection.FieldInfo In lstFileInfoRessr
            If f.Name = "ResName1" Or f.Name = "ResName2" Then Continue For
            ComparerRessrAttrb(vieilleRessr, majRessr, f, vieilleRessr.ResName1, vieilleRessr.ResName2, lstAjout, lstSupp)
        Next
    End Sub

    ''' <summary>
    ''' Fonction de service. Compare un attribut d'un utilisateur.
    ''' </summary>
    ''' <param name="vieuxUser">L'utilisateur de la vieille configuration.</param>
    ''' <param name="majUser">L'utilisateur de la configuration à jour.</param>
    ''' <param name="infoChamp">Les informations sur le format des objets utilisateurs.</param>
    ''' <param name="userID">L'identifiant de l'utilisateur.</param>
    ''' <param name="lstAjout">La liste qui recevera les ajouts d'attributs.</param>
    ''' <param name="lstSupp">La liste qui recevera les supressions d'attributs.</param>
    ''' <remarks></remarks>
    Private Shared Sub ComparerUserAttrb(ByVal vieuxUser As TsCdSageUser, ByVal majUser As TsCdSageUser, ByVal infoChamp As Reflection.FieldInfo, ByVal userID As String, ByVal lstAjout As List(Of TsCdConnxUserAttrb), ByVal lstSupp As List(Of TsCdConnxUserAttrb))
        Dim vieilleValeur As Object = infoChamp.GetValue(vieuxUser)
        Dim nouvelleValeur As Object = infoChamp.GetValue(majUser)

        If vieilleValeur Is Nothing Then vieilleValeur = ""
        If nouvelleValeur Is Nothing Then nouvelleValeur = ""

        '! Quand ont créer un nouveau rôle, Soap ne prend pas les dates nulles, donc nous devons
        '! passé une date minimum définit par nous. Lors de la différence entre un role créé
        '! dans DNA et un rôle créé par nous, il y a une différence entre les deux,
        '! ce bout de code rend cette différence invisible au processu de différence.
        If (TypeOf vieilleValeur Is Date) And (TypeOf nouvelleValeur Is Date) Then
            Dim vieilleValeurConvert As Date = CType(vieilleValeur, Date)
            Dim nouvelleValeurConvert As Date = CType(nouvelleValeur, Date)

            If vieilleValeurConvert <> nouvelleValeurConvert Then
                If vieilleValeurConvert = Date.MinValue Then
                    '! Pour que la veille valeur ne soit pas détecter elle sera définit à rien. 
                    vieilleValeur = ""
                End If
                If nouvelleValeurConvert = Date.MinValue Then
                    nouvelleValeur = TsBaConfigSage.DATE_MIN_SAGE
                End If
            End If
        End If

        If TypeOf vieilleValeur Is Date Then
            vieilleValeur = CType(vieilleValeur, Date).ToString(TsCdConnxAttrb.FORMAT_DATE_TOSTRING)
        End If
        If TypeOf nouvelleValeur Is Date Then
            nouvelleValeur = CType(nouvelleValeur, Date).ToString(TsCdConnxAttrb.FORMAT_DATE_TOSTRING)
        End If

        Dim userAttrb As New TsCdConnxUserAttrb()
        userAttrb.CodeUtilisateur = userID

        Dim attrbVide As New TsCdConnxUserAttrb()
        attrbVide.CodeUtilisateur = userAttrb.CodeUtilisateur

        '! Ici nous appelons le première argument .net lier au champ de type TsAtNomChampGen.
        Dim nomChampNormaliser As String = CType(infoChamp.GetCustomAttributes(GetType(TsAtNomChampGen), True)(0), TsAtNomChampGen).NomChamp

        ComparerAttrb(Of TsCdConnxUserAttrb)(vieilleValeur.ToString, nouvelleValeur.ToString, nomChampNormaliser, userAttrb, attrbVide, lstAjout, lstSupp)
    End Sub

    ''' <summary>
    ''' Fonction de service. Compare un attribut d'un rôle.
    ''' </summary>
    ''' <param name="vieuxRole">Le rôle de la vieille configuration.</param>
    ''' <param name="majRole">Le rôle de la configuration à jour.</param>
    ''' <param name="infoChamp">Les information sur le format des objets rôles.</param>
    ''' <param name="roleName">L'identifiant du rôle.</param>
    ''' <param name="lstAjout">La liste qui recevera les ajouts d'attributs.</param>
    ''' <param name="lstSupp">La liste qui recevera les supressions d'attributs.</param>
    ''' <remarks></remarks>
    Private Shared Sub ComparerRoleAttrb(ByVal vieuxRole As TsCdSageRole, ByVal majRole As TsCdSageRole, ByVal infoChamp As Reflection.FieldInfo, ByVal roleName As String, ByVal lstAjout As List(Of TsCdConnxRoleAttrb), ByVal lstSupp As List(Of TsCdConnxRoleAttrb))
        Dim vieilleValeur As Object = infoChamp.GetValue(vieuxRole)
        Dim nouvelleValeur As Object = infoChamp.GetValue(majRole)

        If vieilleValeur Is Nothing Then vieilleValeur = ""
        If nouvelleValeur Is Nothing Then nouvelleValeur = ""

        '! Quand ont créé un nouveau rôle, Soap ne prend pas les dates nulles, donc nous devons
        '! passé une date minimum définit pas nous. Lors de la différence entre un role créé
        '! dans DNA et un rôle créé par nous, il y a une différence entre les deux,
        '! ce bout de code rend cette différence invisible au processu de différence.
        If (TypeOf vieilleValeur Is Date) And (TypeOf nouvelleValeur Is Date) Then
            Dim vieilleValeurConvert As Date = CType(vieilleValeur, Date)
            Dim nouvelleValeurConvert As Date = CType(nouvelleValeur, Date)

            If vieilleValeurConvert <> nouvelleValeurConvert Then
                If vieilleValeurConvert = Date.MinValue Then
                    '! Pour que la veille valeur ne soit pas détecter elle sera définit à rien. 
                    vieilleValeur = ""
                End If
                If nouvelleValeurConvert = Date.MinValue Then
                    nouvelleValeur = TsBaConfigSage.DATE_MIN_SAGE
                End If
            End If
        End If

        If TypeOf vieilleValeur Is Date Then
            vieilleValeur = CType(vieilleValeur, Date).ToString(TsCdConnxAttrb.FORMAT_DATE_TOSTRING)
        End If
        If TypeOf nouvelleValeur Is Date Then
            nouvelleValeur = CType(nouvelleValeur, Date).ToString(TsCdConnxAttrb.FORMAT_DATE_TOSTRING)
        End If

        Dim roleAttrb As New TsCdConnxRoleAttrb()
        roleAttrb.NomRole = roleName

        Dim attrbVide As New TsCdConnxRoleAttrb()
        attrbVide.NomRole = roleAttrb.NomRole

        '! Ici nous appelons le première argument .net lier au champ de type TsAtNomChampGen.
        Dim nomChampNormaliser As String = CType(infoChamp.GetCustomAttributes(GetType(TsAtNomChampGen), True)(0), TsAtNomChampGen).NomChamp

        ComparerAttrb(Of TsCdConnxRoleAttrb)(vieilleValeur.ToString, nouvelleValeur.ToString, nomChampNormaliser, roleAttrb, attrbVide, lstAjout, lstSupp)
    End Sub

    ''' <summary>
    ''' Fonction de service. Compare un attribut d'une ressource.
    ''' </summary>
    ''' <param name="vieilleRessr">La ressource de la vieille configuration.</param>
    ''' <param name="majRessr">La ressource de la configuration à jour.</param>
    ''' <param name="infoChamp">Les information sur le format des objets ressources.</param>
    ''' <param name="NomRessource">L'identifiant du rôle.(1/2 de la clé)</param>
    ''' <param name="CatgrRessource">L'identifiant du rôle.(2/2 de la clé)</param>
    ''' <param name="lstAjout">La liste qui recevera les ajouts d'attributs.</param>
    ''' <param name="lstSupp">La liste qui recevera les supressions d'attributs.</param>
    ''' <remarks></remarks>
    Private Shared Sub ComparerRessrAttrb(ByVal vieilleRessr As TsCdSageResource, ByVal majRessr As TsCdSageResource, ByVal infoChamp As Reflection.FieldInfo, ByVal NomRessource As String, ByVal CatgrRessource As String, ByVal lstAjout As List(Of TsCdConnxRessrAttrb), ByVal lstSupp As List(Of TsCdConnxRessrAttrb))
        Dim vieilleValeur As Object = infoChamp.GetValue(vieilleRessr)
        Dim nouvelleValeur As Object = infoChamp.GetValue(majRessr)

        If vieilleValeur Is Nothing Then vieilleValeur = ""
        If nouvelleValeur Is Nothing Then nouvelleValeur = ""

        '! Quand ont créé un nouveau rôle, Soap ne prend pas les dates nulles, donc nous devons
        '! passé une date minimum définit pas nous. Lors de la différence entre un role créé
        '! dans DNA et un rôle créé par nous, il y a une différence entre les deux,
        '! ce bout de code rend cette différence invisible au processu de différence.
        If (TypeOf vieilleValeur Is Date) And (TypeOf nouvelleValeur Is Date) Then
            Dim vieilleValeurConvert As Date = CType(vieilleValeur, Date)
            Dim nouvelleValeurConvert As Date = CType(nouvelleValeur, Date)

            If vieilleValeurConvert <> nouvelleValeurConvert Then
                If vieilleValeurConvert = Date.MinValue Then
                    '! Pour que la veille valeur ne soit pas détecter elle sera définit à rien. 
                    vieilleValeur = ""
                End If
                If nouvelleValeurConvert = Date.MinValue Then
                    nouvelleValeur = TsBaConfigSage.DATE_MIN_SAGE
                End If
            End If
        End If

        If TypeOf vieilleValeur Is Date Then
            vieilleValeur = CType(vieilleValeur, Date).ToString(TsCdConnxAttrb.FORMAT_DATE_TOSTRING)
        End If
        If TypeOf nouvelleValeur Is Date Then
            nouvelleValeur = CType(nouvelleValeur, Date).ToString(TsCdConnxAttrb.FORMAT_DATE_TOSTRING)
        End If

        Dim ressrAttrb As New TsCdConnxRessrAttrb()
        ressrAttrb.CatgrRessource = CatgrRessource
        ressrAttrb.NomRessource = NomRessource

        Dim attrbVide As New TsCdConnxRessrAttrb()
        attrbVide.CatgrRessource = ressrAttrb.CatgrRessource
        attrbVide.NomRessource = ressrAttrb.NomRessource

        '! Ici nous appelons le première argument .net lier au champ de type TsAtNomChampGen.
        Dim nomChampNormaliser As String = CType(infoChamp.GetCustomAttributes(GetType(TsAtNomChampGen), True)(0), TsAtNomChampGen).NomChamp

        ComparerAttrb(Of TsCdConnxRessrAttrb)(vieilleValeur.ToString, nouvelleValeur.ToString, nomChampNormaliser, ressrAttrb, attrbVide, lstAjout, lstSupp)
    End Sub

    ''' <summary>
    ''' Fonction de service. Permet de comparer deux attributs. Met les différences dans les listes appropriées.
    ''' </summary>
    ''' <typeparam name="T">Type générique. Doit être une classe descendante de <see cref="TsCdConnxAttrb"/>.</typeparam>
    ''' <param name="attrbVieux">L'attribut de la vieille configuration.</param>
    ''' <param name="attrbMaj">L'attribut de la configuration à jour.</param>
    ''' <param name="nomAttrb">Nom de l'attribut comparé.</param>
    ''' <param name="connexAttrb">L'attribut partiellement complèté par les fonctions appelantes. Qui sera ajouté à la liste ajout.</param>
    ''' <param name="attrbVide">L'attribut partiellement complèté par les fonctions appelantes. Qui sera ajouté à la liste supprimer.</param>
    ''' <param name="lstAjout">La liste qui recevera les ajouts d'attributs.</param>
    ''' <param name="lstSupp">La liste qui recevera les supressions d'attributs.</param>
    ''' <remarks></remarks>
    Private Shared Sub ComparerAttrb(Of T As TsCdConnxAttrb)(ByVal attrbVieux As String, ByVal attrbMaj As String, ByVal nomAttrb As String, ByVal connexAttrb As T, ByVal attrbVide As T, ByVal lstAjout As List(Of T), ByVal lstSupp As List(Of T))
        connexAttrb.CibleAJour = TsECcCibleAJour.Inconnu
        connexAttrb.NomAttrb = nomAttrb
        connexAttrb.Valeur = attrbMaj

        If attrbVieux <> attrbMaj Then
            If attrbVieux = Nothing Then
                lstAjout.Add(connexAttrb)
            ElseIf attrbMaj <> Nothing Then
                attrbVide.CibleAJour = TsECcCibleAJour.Inconnu
                attrbVide.NomAttrb = connexAttrb.NomAttrb
                attrbVide.Valeur = Nothing

                lstSupp.Add(attrbVide)
                lstAjout.Add(connexAttrb)
            Else
                lstSupp.Add(connexAttrb)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Fonction de service. Permet de remplir un dictionnaire à partir d'une liste.
    ''' </summary>
    ''' <typeparam name="T">Type générique.</typeparam>
    ''' <param name="dictionnaire">Le dictionnaire qui sera remplis par la liste.</param>
    ''' <param name="listElement">La liste qui remplira le dictionnaire.</param>
    ''' <param name="CreerCle">Fonction qui traite un élément de la liste pour récuperer une clé unique pour le dictionnaire.</param>
    ''' <remarks></remarks>
    Private Shared Sub RemplirDictio(Of T)(ByVal dictionnaire As Dictionary(Of String, T), ByVal listElement As IEnumerable(Of T), ByVal CreerCle As DeleguerCreateurCle(Of T))
        For Each element As T In listElement
            If dictionnaire.ContainsKey(CreerCle(element)) = False Then
                dictionnaire.Add(CreerCle(element), element)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Fonction générique. Revois deux listes d'objets triés par ajout et trié par supression.
    ''' </summary>
    ''' <typeparam name="T">Type de base.</typeparam>
    ''' <typeparam name="C">Type Collection du type de base.</typeparam>
    ''' <param name="listeAncienne">Collection acienne contenant le type de base.</param>
    ''' <param name="listeAjour">Collection à jour contenant le type de base.</param>
    ''' <param name="lstAjouter">
    ''' Collection contenant le type de base. Liste de retour.
    ''' Les résultats seront retournés par cette variable.
    ''' Contient les éléments qui sont ajoutés à la nouvelle configuration.
    ''' </param>
    ''' <param name="lstRetirer">
    ''' Collection contenant le type de base. Liste de retour.
    ''' Les résultats seront retournés par cette variable.
    ''' Contient les éléments qui sont retirés à la nouvelle configuration.
    ''' </param>
    ''' <remarks></remarks>
    Private Shared Sub Decomposition(Of T, C As TsCdCollectionSage(Of T))(ByVal listeAncienne As C, ByVal listeAjour As C, ByVal lstAjouter As C, ByVal lstRetirer As C, ByVal comparateur As Comparateur(Of T))
        For Each ancienElement As T In listeAncienne
            Dim drapeauPresent As Boolean = False
            For Each nouvelElement As T In listeAjour
                If comparateur(ancienElement, nouvelElement) = True Then
                    drapeauPresent = True
                    Exit For
                End If
            Next

            If drapeauPresent = False Then
                lstRetirer.Add(ancienElement)
            End If
        Next

        For Each nouvelElement As T In listeAjour
            Dim drapeauPresent As Boolean = False
            For Each ancienElement As T In listeAncienne
                If comparateur(ancienElement, nouvelElement) = True Then
                    drapeauPresent = True
                    Exit For
                End If
            Next

            If drapeauPresent = False Then
                lstAjouter.Add(nouvelElement)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Fonction de service. Comparateur de <see cref="TsCdSageUser" />.
    ''' Compare les éléments utiles des objets d'entrés.
    ''' </summary>
    ''' <param name="utilisateur1">Un utilisateur provenant de sage.</param>
    ''' <param name="utilisateur2">Un autre utilisateur provenant de sage.</param>
    ''' <returns>Si les objets sont comparables, la fonction retourne true.</returns>
    ''' <remarks></remarks>
    Private Shared Function Comparaison(ByVal utilisateur1 As TsCdSageUser, ByVal utilisateur2 As TsCdSageUser) As Boolean
        If utilisateur1.PersonID <> utilisateur2.PersonID Then
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' Fonction de service. Comparateur de <see cref="TsCdSageRole" />.
    ''' Compare les éléments utiles des objets d'entrés.
    ''' </summary>
    ''' <param name="role1">Un rôle provenant de sage.</param>
    ''' <param name="role2">Un autre rôle provenant de sage.</param>
    ''' <returns>Si les objets sont comparables, la fonction retourne true.</returns>
    ''' <remarks></remarks>
    Private Shared Function Comparaison(ByVal role1 As TsCdSageRole, ByVal role2 As TsCdSageRole) As Boolean
        If role1.Name <> role2.Name Then
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' Fonction de service. Comparateur de <see cref="TsCdSageResource" />.
    ''' Compare les éléments utiles des objets d'entrés.
    ''' </summary>
    ''' <param name="ressource1">Une ressource provenant de sage.</param>
    ''' <param name="ressource2">Une autre ressource provenant de sage.</param>
    ''' <returns>Si les objets sont comparables, la fonction retourne true.</returns>
    ''' <remarks></remarks>
    Private Shared Function Comparaison(ByVal ressource1 As TsCdSageResource, ByVal ressource2 As TsCdSageResource) As Boolean
        If ressource1.ResName1 <> ressource2.ResName1 Then
            Return False
        End If
        If ressource1.ResName2 <> ressource2.ResName2 Then
            Return False
        End If
        If ressource1.ResName3 <> ressource2.ResName3 Then
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' Fonction de service. Comparateur de <see cref="TsCdSageRoleRoleLink" />.
    ''' Compare les éléments utiles des objets d'entrés.
    ''' </summary>
    ''' <param name="lienRoleRole1">Un lien rôle/rôle provenant de sage.</param>
    ''' <param name="lienRoleRole2">Un autre lien rôle/rôle provenant de sage.</param>
    ''' <returns>Si les objets sont comparables, la fonction retourne true.</returns>
    ''' <remarks></remarks>
    Private Shared Function Comparaison(ByVal lienRoleRole1 As TsCdSageRoleRoleLink, ByVal lienRoleRole2 As TsCdSageRoleRoleLink) As Boolean
        If lienRoleRole1.ChildRole <> lienRoleRole2.ChildRole Then
            Return False
        End If
        If lienRoleRole1.ParentRole <> lienRoleRole2.ParentRole Then
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' Fonction de service. Comparateur de <see cref="TsCdSageRoleResLink" />.
    ''' Compare les éléments utiles des objets d'entrés.
    ''' </summary>
    ''' <param name="lienRoleRessource1">Un lien rôle/ressource provenant de sage.</param>
    ''' <param name="lienRoleRessource2">Un autre lien rôle/ressource provenant de sage.</param>
    ''' <returns>Si les objets sont comparables, la fonction retourne true.</returns>
    ''' <remarks></remarks>
    Private Shared Function Comparaison(ByVal lienRoleRessource1 As TsCdSageRoleResLink, ByVal lienRoleRessource2 As TsCdSageRoleResLink) As Boolean
        If lienRoleRessource1.RoleName <> lienRoleRessource2.RoleName Then
            Return False
        End If
        If lienRoleRessource1.ResName1 <> lienRoleRessource2.ResName1 Then
            Return False
        End If
        If lienRoleRessource1.ResName2 <> lienRoleRessource2.ResName2 Then
            Return False
        End If
        If lienRoleRessource1.ResName3 <> lienRoleRessource2.ResName3 Then
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' Fonction de service. Comparateur de <see cref="TsCdSageRoleResLink" />.
    ''' Compare les éléments utiles des objets d'entrés.
    ''' </summary>
    ''' <param name="lienUtilisateurRole1">Un lien utilisateur/rôle provenant de sage.</param>
    ''' <param name="lienUtilisateurRole2">Un autre lien utilisateur/rôle provenant de sage.</param>
    ''' <returns>Si les objets sont comparables, la fonction retourne true.</returns>
    ''' <remarks></remarks>
    Private Shared Function Comparaison(ByVal lienUtilisateurRole1 As TsCdSageUserRoleLink, ByVal lienUtilisateurRole2 As TsCdSageUserRoleLink) As Boolean
        If lienUtilisateurRole1.PersonID <> lienUtilisateurRole2.PersonID Then
            Return False
        End If
        If lienUtilisateurRole1.RoleName <> lienUtilisateurRole2.RoleName Then
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' Fonction de service. Comparateur de <see cref="TsCdSageRoleResLink" />.
    ''' Compare les éléments utiles des objets d'entrés.
    ''' </summary>
    ''' <param name="lienUtilisateurRessource1">Un lien utilisateur/ressource provenant de sage.</param>
    ''' <param name="lienUtilisateurRessource2">Un autre lien utilisateur/ressource provenant de sage.</param>
    ''' <returns>Si les objets sont comparables, la fonction retourne true.</returns>
    ''' <remarks></remarks>
    Private Shared Function Comparaison(ByVal lienUtilisateurRessource1 As TsCdSageUserResLink, ByVal lienUtilisateurRessource2 As TsCdSageUserResLink) As Boolean
        If lienUtilisateurRessource1.PersonID <> lienUtilisateurRessource2.PersonID Then
            Return False
        End If
        If lienUtilisateurRessource1.ResName1 <> lienUtilisateurRessource2.ResName1 Then
            Return False
        End If
        If lienUtilisateurRessource1.ResName2 <> lienUtilisateurRessource2.ResName2 Then
            Return False
        End If
        If lienUtilisateurRessource1.ResName3 <> lienUtilisateurRessource2.ResName3 Then
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' Fonction de service fait la résolution des liens hiérarchiques des rôles et 
    ''' fait les lien entre les utilisateur et les ressource.
    ''' </summary>
    ''' <param name="cible">Pour filtrer les ressource pour la cible désiré.</param>
    ''' <param name="config">La configuration à visiter.</param>
    Private Shared Function ObtnrLiensRecurcifUtilisateurRessource(ByVal config As String, ByVal cible As String) As TsCdSageUserResLinkCollection
        Dim dictnrLiensUtilisateurRessource As New Dictionary(Of String, HashSet(Of RessourceHaset))
        Dim dictnrLiensRoleRessource As New Dictionary(Of String, HashSet(Of RessourceHaset))
        Dim dictnrLienRoleRole As New Dictionary(Of String, HashSet(Of String))

        Dim lstUtilisateurRessource As TsCdSageUserResLinkCollection = TsBaConfigSage.GetUserResourcesLinks(config)
        Dim lstUtilisateurRole As TsCdSageUserRoleLinkCollection = TsBaConfigSage.GetUserRolesLinks(config)
        Dim lstRoleRole As TsCdSageRoleRoleLinkCollection = TsBaConfigSage.GetRoleSubRolesLinks(config)
        Dim lstRoleRessource As TsCdSageRoleResLinkCollection = TsBaConfigSage.GetRoleResourcesLinks(config)

        '! ///////////////////////////////////////////////
        '! // BOUCLE UTILISATEUR -> RESSOURCE
        '! ///////////////////////////////////////////////

        For Each lienUtilisateurRessource As TsCdSageUserResLink In lstUtilisateurRessource
            With lienUtilisateurRessource

                If String.Compare(.ResName3, cible, True) = 0 Then
                    If dictnrLiensUtilisateurRessource.ContainsKey(.PersonID) = False Then
                        dictnrLiensUtilisateurRessource.Add(.PersonID, New HashSet(Of RessourceHaset))
                    End If
                    dictnrLiensUtilisateurRessource.Item(.PersonID).Add(New RessourceHaset(.ResName1, .ResName2, .ResName3))
                End If

            End With
        Next

        '! ///////////////////////////////////////////////
        '! // BOUCLE ROLE -> ROLE -> ROLE ->...
        '! ///////////////////////////////////////////////
        '! // Attention les liens Parentaux sont inversés à ceux définis dans Sage DNA. 
        '! // Car malgré que les liens entres rôles semble bien forgés dans Sage DNA, les liens entres les rôles et les ressources ne le sont pas.
        '! // Donc dans sage les responsables de la sécurités sont forcés d'inverser les "Parents" avec les "Enfants" pour que les ressources soient bien attribuées.
        '! // Ici nous reconstruisont à l'envers ce procédé pour revenir à une forme plus conventionnel d'héritage. 
        '! ///////////////////////////////////////////////
        For Each lienRoleRole As TsCdSageRoleRoleLink In lstRoleRole
            If dictnrLienRoleRole.ContainsKey(lienRoleRole.ParentRole) = False Then
                dictnrLienRoleRole.Add(lienRoleRole.ParentRole, New HashSet(Of String)())
            End If
            If dictnrLienRoleRole.ContainsKey(lienRoleRole.ChildRole) = False Then
                dictnrLienRoleRole.Add(lienRoleRole.ChildRole, New HashSet(Of String)())
                dictnrLienRoleRole.Item(lienRoleRole.ChildRole).Add(lienRoleRole.ChildRole)
            End If
            dictnrLienRoleRole.Item(lienRoleRole.ParentRole).Add(lienRoleRole.ParentRole)
            dictnrLienRoleRole.Item(lienRoleRole.ParentRole).Add(lienRoleRole.ChildRole)
        Next

        Dim changement As Boolean = True
        While changement
            changement = False

            For Each pair1 As KeyValuePair(Of String, HashSet(Of String)) In dictnrLienRoleRole
                For Each pair2 As KeyValuePair(Of String, HashSet(Of String)) In dictnrLienRoleRole
                    If pair1.Value.Contains(pair2.Key) Then
                        Dim nbEnssemble As Integer = pair1.Value.Count
                        pair1.Value.UnionWith(pair2.Value)
                        If pair1.Value.Count <> nbEnssemble Then
                            changement = True
                        End If
                    End If
                Next
            Next
        End While

        '! ///////////////////////////////////////////////
        '! // BOUCLE ROLE -> RESSOURCE
        '! ///////////////////////////////////////////////

        For Each lienRoleRessource As TsCdSageRoleResLink In lstRoleRessource
            With lienRoleRessource

                If String.Compare(.ResName3, cible, True) = 0 Then
                    If dictnrLienRoleRole.ContainsKey(.RoleName) = False Then
                        dictnrLienRoleRole.Add(.RoleName, New HashSet(Of String))
                    End If
                    dictnrLienRoleRole.Item(.RoleName).Add(.RoleName)
                    If dictnrLiensRoleRessource.ContainsKey(.RoleName) = False Then
                        dictnrLiensRoleRessource.Add(.RoleName, New HashSet(Of RessourceHaset))
                    End If
                    dictnrLiensRoleRessource.Item(.RoleName).Add(New RessourceHaset(.ResName1, .ResName2, .ResName3))
                End If

            End With
        Next

        '! ///////////////////////////////////////////////
        '! // BOUCLE UTILISATEUR -> ROLE -> RESSOURCE
        '! ///////////////////////////////////////////////

        For Each lienUtilisateurRole As TsCdSageUserRoleLink In lstUtilisateurRole
            With lienUtilisateurRole
                If dictnrLiensUtilisateurRessource.ContainsKey(.PersonID) = False Then
                    dictnrLiensUtilisateurRessource.Add(.PersonID, New HashSet(Of RessourceHaset)())
                End If

                If dictnrLienRoleRole.ContainsKey(.RoleName) Then
                    For Each element As String In dictnrLienRoleRole.Item(.RoleName)
                        If dictnrLiensRoleRessource.ContainsKey(element) Then
                            dictnrLiensUtilisateurRessource.Item(.PersonID).UnionWith(dictnrLiensRoleRessource.Item(element))
                        End If
                    Next
                End If
            End With
        Next

        '! ///////////////////////////////////////////////
        '! // TRANSFORMER LES INFORMATIONS EN LISTE PLUS SIMPLE
        '! ///////////////////////////////////////////////
        Dim paramRetour As New TsCdSageUserResLinkCollection
        For Each element As KeyValuePair(Of String, HashSet(Of RessourceHaset)) In dictnrLiensUtilisateurRessource

            For Each sousElement As RessourceHaset In element.Value
                Dim lienUtilstrRessrc As New TsCdSageUserResLink
                lienUtilstrRessrc.PersonID = element.Key
                lienUtilstrRessrc.ResName1 = sousElement.resName1
                lienUtilstrRessrc.ResName2 = sousElement.resName2
                lienUtilstrRessrc.ResName3 = sousElement.resName3
                paramRetour.Add(lienUtilstrRessrc)
            Next

        Next

        Return paramRetour
    End Function

    ''' <summary>
    ''' Vérifie si les configurations existe.
    ''' </summary>
    ''' <param name="MAJConfig">Nom de la configuration à jour.</param>
    ''' <param name="vieilleConfig">Nom de la vieille configuration.</param>
    ''' <remarks>Si les noms de configurations sont 'nothing' les différences fonctionnent quand même.</remarks>
    Private Shared Sub ValiderExistencesConfigs(ByVal vieilleConfig As String, ByVal MAJConfig As String)
        If vieilleConfig <> Nothing Then
            If ValiderExistenceConfig(vieilleConfig) = False Then
                Throw New TsExcConfigurationInexistante("La vieille configuration n'existe pas dans Sage.")
            End If
        End If

        If MAJConfig <> Nothing Then
            If ValiderExistenceConfig(MAJConfig) = False Then
                Throw New TsExcConfigurationInexistante("La configuration à jour n'existe pas dans Sage.")
            End If
        End If
    End Sub

    ''' <summary>
    ''' Function de Conversion. Utilisé dans la converstion d'élément d'une liste.
    ''' </summary>
    ''' <param name="lienUtilRessr">Lien utilisateur/ressource à convertir.</param>
    Private Shared Function Conversion(ByVal lienUtilRessr As TsCdSageUserResLink) As TsCdConnxUserRessr
        Dim nouvelElement As New TsCdConnxUserRessr()

        nouvelElement.NomRessource = lienUtilRessr.ResName1
        nouvelElement.CatgrRessource = lienUtilRessr.ResName2
        nouvelElement.CodeUtilisateur = lienUtilRessr.PersonID

        nouvelElement.CibleAJour = TsECcCibleAJour.Inconnu

        Return nouvelElement
    End Function

    ''' <summary>
    ''' Function de Conversion. Utilisé dans la converstion d'élément d'une liste.
    ''' </summary>
    ''' <param name="lienUtilRole">Lien utilisateur/rôle à convertir.</param>
    Private Shared Function Conversion(ByVal lienUtilRole As TsCdSageUserRoleLink) As TsCdConnxUserRole
        Dim nouvelElement As New TsCdConnxUserRole()

        nouvelElement.NomRole = lienUtilRole.RoleName
        nouvelElement.CodeUtilisateur = lienUtilRole.PersonID

        nouvelElement.CibleAJour = TsECcCibleAJour.Inconnu

        Return nouvelElement
    End Function

    ''' <summary>
    ''' Function de Conversion. Utilisé dans la converstion d'élément d'une liste.
    ''' </summary>
    ''' <param name="lienRoleRole">Lien rôle/rôle à convertir.</param>
    ''' <remarks>
    ''' Ici nous inverson le définition de Sage dans les rapports d'héritage.
    ''' Car dans sage les parents héritent des liens des enfants.
    ''' </remarks>
    Private Shared Function Conversion(ByVal lienRoleRole As TsCdSageRoleRoleLink) As TsCdConnxRoleRole
        Dim nouvelElement As New TsCdConnxRoleRole()

        nouvelElement.NomSousRole = lienRoleRole.ChildRole
        nouvelElement.NomRoleSup = lienRoleRole.ParentRole

        nouvelElement.CibleAJour = TsECcCibleAJour.Inconnu

        Return nouvelElement
    End Function

    ''' <summary>
    ''' Function de Conversion. Utilisé dans la converstion d'élément d'une liste.
    ''' </summary>
    ''' <param name="lienRoleRessr">Lien rôle/ressource à convertir.</param>
    Private Shared Function Conversion(ByVal lienRoleRessr As TsCdSageRoleResLink) As TsCdConnxRoleRessr
        Dim nouvelElement As New TsCdConnxRoleRessr()

        nouvelElement.NomRole = lienRoleRessr.RoleName
        nouvelElement.NomRessource = lienRoleRessr.ResName1
        nouvelElement.CatgrRessource = lienRoleRessr.ResName2

        nouvelElement.CibleAJour = TsECcCibleAJour.Inconnu

        Return nouvelElement
    End Function

    ''' <summary>
    ''' Function de Conversion. Utilisé dans la converstion d'élément d'une liste.
    ''' </summary>
    ''' <param name="role">Rôle à convertir.</param>
    Private Shared Function Conversion(ByVal role As TsCdSageRole) As TsCdConnxRole
        Dim nouvelElement As New TsCdConnxRole()

        nouvelElement.NomRole = role.Name

        nouvelElement.CibleAJour = TsECcCibleAJour.Inconnu

        Return nouvelElement
    End Function

    ''' <summary>
    ''' Function de Conversion. Utilisé dans la converstion d'élément d'une liste.
    ''' </summary>
    ''' <param name="utilisateur">Utilisateur à convertir.</param>
    Private Shared Function Conversion(ByVal utilisateur As TsCdSageUser) As TsCdConnxUser
        Dim nouvelElement As New TsCdConnxUser()

        nouvelElement.CodeUtilisateur = utilisateur.PersonID

        nouvelElement.CibleAJour = TsECcCibleAJour.Inconnu

        Return nouvelElement
    End Function

    ''' <summary>
    ''' Function de Conversion. Utilisé dans la converstion d'élément d'une liste.
    ''' </summary>
    ''' <param name="ressource">Ressource à convertir.</param>
    Private Shared Function Conversion(ByVal ressource As TsCdSageResource) As TsCdConnxRessr
        Dim nouvelElement As New TsCdConnxRessr()

        nouvelElement.NomRessource = ressource.ResName1

        nouvelElement.CatgrRessource = ressource.ResName2

        nouvelElement.CibleAJour = TsECcCibleAJour.Inconnu

        Return nouvelElement
    End Function

    ''' <summary>
    ''' Fonction de service. Renvois une liste à partir de la fonction passée en argument.
    ''' </summary>
    ''' <typeparam name="T">Type générique possédant au minimum un contructeur.</typeparam>
    ''' <param name="nomConfig">Nom de la configuration</param>
    ''' <param name="fctObtentionListe">Fonction d'appel qui renvoira la liste de Sage.</param>
    ''' <returns>
    ''' Si la configue est 'nothing', une liste vide sera retournée.
    ''' Si la configue existe une liste de sage sera returnée.
    ''' </returns>
    Private Shared Function ObtenirListe(Of T As New)(ByVal nomConfig As String, ByVal fctObtentionListe As DeleguerObtentionListe(Of T)) As T
        If nomConfig <> Nothing Then
            Return fctObtentionListe(nomConfig)
        Else
            Return New T
        End If
    End Function

#End Region

#Region "Fonctions déléguées"

    ''' <summary>
    ''' Définition de Fonction de délégation. Sert à définir les fonctions de renvois des listes Sage.
    ''' </summary>
    ''' <typeparam name="T">Type Générique.</typeparam>
    ''' <param name="nomConfig">Le nom de la configuration.</param>
    ''' <returns>Le type générique.</returns>
    Private Delegate Function DeleguerObtentionListe(Of T)(ByVal nomConfig As String) As T

    ''' <summary>
    ''' Fonction Délégué. Format de fonction pour créer une clé.
    ''' </summary>
    ''' <typeparam name="T">Type générique.</typeparam>
    ''' <param name="objet">Objet du type générique.</param>
    ''' <returns>Une clé sous format string.</returns>
    Delegate Function DeleguerCreateurCle(Of T)(ByVal objet As T) As String

    ''' <summary>
    ''' Fonction délégué. Est utilisé comme gabarit pour des fonctions de comparaison.
    ''' </summary>
    ''' <typeparam name="TInput">Type d'entrée.</typeparam>
    ''' <param name="input1">Premier objet comparé.</param>
    ''' <param name="input2">Deuxième objet comparé.</param>
    ''' <returns>
    ''' VRAI si les deux objet sont juger semblable selon les critères de comparaison des fonctions,
    ''' FAUX sinon
    ''' </returns>
    Delegate Function Comparateur(Of TInput)(ByVal input1 As TInput, ByVal input2 As TInput) As Boolean

#End Region

#Region "Classe privée"

    ''' <summary>
    ''' Classe privée. Cette classe est utilisé pour conserver les informations 
    ''' d'une ressource dans un ensemble <see cref="HashSet(Of Ressource)" />.
    ''' </summary>
    ''' <remarks></remarks>
    Private Class RessourceHaset

#Region "Variables Publics"

        Public resName1 As String
        Public resName2 As String
        Public resName3 As String

#End Region

#Region "Constructeur"

        ''' <summary>
        ''' Constructeur de base.
        ''' </summary>
        ''' <param name="resName1">Nom clé de la ressource (1/3).</param>
        ''' <param name="resName2">Nom clé de la ressource (2/3).</param>
        ''' <param name="resName3">Nom clé de la ressource (3/3).</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal resName1 As String, ByVal resName2 As String, ByVal resName3 As String)
            Me.resName1 = resName1
            Me.resName2 = resName2
            Me.resName3 = resName3
        End Sub

#End Region

#Region "Méthodes"

        ''' <summary>
        ''' Fonction de base surchargée.
        ''' Définits l'égalité entre deux objets.
        ''' </summary>
        ''' <param name="obj">L'objet à comparer</param>
        ''' <returns>Si l'objet entré est égale à l'objet courant alors retourne VRAI, sinon FAUX</returns>
        ''' <remarks></remarks>
        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            If TypeOf obj Is RessourceHaset Then
                Dim comparer As RessourceHaset = CType(obj, RessourceHaset)
                Dim paramRetour As Boolean = True

                paramRetour = paramRetour = True And Me.resName1 = comparer.resName1
                paramRetour = paramRetour = True And Me.resName2 = comparer.resName2
                paramRetour = paramRetour = True And Me.resName3 = comparer.resName3

                Return paramRetour
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Fonction de base surchargée.
        ''' Renvois un numéro "Hashé", pour les tables de hashages.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function GetHashCode() As Integer
            Return Me.resName1.GetHashCode Xor Me.resName2.GetHashCode Xor Me.resName3.GetHashCode
        End Function

#End Region

    End Class
#End Region

End Class
