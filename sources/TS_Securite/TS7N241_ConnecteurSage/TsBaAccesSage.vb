Imports Configuration = Rrq.InfrastructureCommune.Parametres.XuCuConfiguration
Imports Rrq.Securite.GestionAcces.TsCdConstanteNomChampNorm

''' <summary>
''' Classe de base pour communiquer avec Sage à partir des connecteurs.
''' </summary>
Public Class TsBaAccesSage


#Region "Variables Privées"

    '! Liste des champs de chaque entité.(Utilisateur, Rôle, Ressource)
    Private Shared lstFieldInfoUser As Reflection.FieldInfo() = GetType(TsCdSageUser).GetFields
    Private Shared lstFieldInfoRole As Reflection.FieldInfo() = GetType(TsCdSageRole).GetFields
    Private Shared lstFieldInfoRessr As Reflection.FieldInfo() = GetType(TsCdSageResource).GetFields

    '! La configuration lié à l'objet.
    Private configSageCible As String

    '! Informations sur la configuration lié à l'objet.
    Private sageSBInfo As TsCdSageConfiguration = Nothing

    ''! Mémoire tampon pour les entités.
    Private lstRoles As List(Of TsCdSageRole) = Nothing
    Private lstUsers As List(Of TsCdSageUser) = Nothing
    Private lstRessr As List(Of TsCdSageResource) = Nothing

    '! Mémoire Tampon pour les attributs des entités.
    Private lstChampUser As List(Of TsCdChampSage) = Nothing
    Private lstChampRessr As List(Of TsCdChampSage) = Nothing

    '! Mémoire tampon pour les relations.
    Private lstLiensRoleRole As List(Of TsCdSageRoleRoleLink) = Nothing
    Private lstLiensUserRole As List(Of TsCdSageUserRoleLink) = Nothing
    Private lstLiensRoleRessr As List(Of TsCdSageRoleResLink) = Nothing
    Private lstLiensUserRessr As List(Of TsCdSageUserResLink) = Nothing

#End Region

#Region "Propriétés"
    Public Property Config() As String
        Get
            If String.IsNullOrEmpty(configSageCible) Then
                configSageCible = Configuration.ValeurSysteme("TS7", "TS7\TS7N241\ConfigurationCible")
            End If
            Return configSageCible
        End Get
        Set(ByVal value As String)
            configSageCible = value
        End Set
    End Property
#End Region

#Region "Constructeurs"

#End Region

#Region "Méthodes"

    ''' <summary>
    ''' Méthode permettant de créer un rôle dans la configuration.
    ''' </summary>
    ''' <param name="nomRole">Le nom désirer pour le nouveau rôle.</param>
    ''' <returns>
    ''' Retour de type erreur. 
    ''' Une chaine de caractères vide("") indique qu'il n'y a pas eu d'erreur.
    ''' Dans le cas contraire l'erreur sera décrit dans le paramètre de retour.
    ''' </returns>
    Public Function AjouterRole(ByVal nomRole As String) As String
        Dim erreur As String = ""

        Try
            TsBaConfigSage.AddRoleToConfiguration(Config, nomRole, " ", " ", " ", " ", Nothing, "", "", Nothing, " ", " ", " ", Nothing)
        Catch ex As ApplicationException
            erreur = "Le rôle n'a pas pu être ajouté."
        Catch ex As TsExcSageRoleDejaExistant
            erreur = "Le rôle existe déja dans la config sage."
        End Try

        Return erreur
    End Function

    ''' <summary>
    ''' Méthode permettant de créer un utilisateur dans la configuration.
    ''' </summary>
    ''' <param name="codeUtilisateur">Le code unique de l'utilisateur.</param>
    ''' <returns>
    ''' Retour de type erreur. 
    ''' Une chaine de caractères vide("") indique qu'il n'y a pas eu d'erreur.
    ''' Dans le cas contraire l'erreur sera décrit dans le paramètre de retour.
    ''' </returns>
    Public Function AjouterUtilisateur(ByVal codeUtilisateur As String) As String
        Dim erreur As String = ""
        Dim erreurUDB As Boolean = False
        Try
            TsBaConfigSage.AddUserToDatabase(Config, codeUtilisateur, " ", " ", " ")
        Catch ex As ApplicationException
            '! L'utilisateur existe déja.
            erreurUDB = True
        End Try
        Try
            TsBaConfigSage.AddUserToConfiguration(Config, codeUtilisateur)
        Catch ex As ApplicationException
            If erreurUDB = True Then
                erreur = "L'utilisateur n'a pas pus être ajouter dans la UDB de Sage. L'utilisateur n'a pas pus être ajouté à la configuration de Sage."
            Else
                erreur = "L'utilisateur n'a pas pus être ajouté à la configuration de Sage."
            End If
        End Try

        Return erreur
    End Function

    ''' <summary>
    ''' Méthode permettant de créer une nouvelle ressource dans la configuration.
    ''' </summary>
    ''' <param name="resname1">Premier tier de la clé unique de la ressource.</param>
    ''' <param name="resname2">Deuxième tier de la clé unique de la ressource.</param>
    ''' <param name="resname3">Troisème tier de la clé unique de la ressource.</param>
    ''' <returns>
    ''' Retour de type erreur. 
    ''' Une chaine de caractères vide("") indique qu'il n'y a pas eu d'erreur.
    ''' Dans le cas contraire l'erreur sera décrit dans le paramètre de retour.
    ''' </returns>
    Public Function AjouterRessr(ByVal resname1 As String, ByVal resname2 As String, ByVal resname3 As String) As String
        Dim erreur As String = ""
        Dim erreurRDB As Boolean = False

        Try
            TsBaConfigSage.AddResourceToDatabase(Config, resname1, resname2, resname3)
        Catch ex As ApplicationException
            '! La ressource existe déja.
            erreurRDB = True
        End Try
        Try
            TsBaConfigSage.AddResourceToConfiguration(Config, resname1, resname2, resname3)
        Catch ex As ApplicationException
            If erreurRDB = True Then
                erreur = "La ressource n'a pas pus être ajouter dans la RDB de Sage. La ressource n'a pas pus être ajoutée à la configuration de Sage."
            Else
                erreur = "L'utilisateur n'a pas pus être ajouté à la configuration de Sage."
            End If
        End Try

        Return erreur
    End Function

    ''' <summary>
    ''' Méthode permettant d'effacer un rôle de la configuration.
    ''' </summary>
    ''' <param name="nomRole">Le nom unique du rôle que vous voulez effacer.</param>
    ''' <returns>
    ''' Retour de type erreur. 
    ''' Une chaine de caractères vide("") indique qu'il n'y a pas eu d'erreur.
    ''' Dans le cas contraire l'erreur sera décrit dans le paramètre de retour.
    ''' </returns>
    Public Function EffacerRole(ByVal nomRole As String) As String
        Dim erreur As String = ""

        Try
            TsBaConfigSage.RemoveConfigurationRole(Config, nomRole)
        Catch ex As ApplicationException
            erreur = "Le rôle n'a pu être effaeé de la configuration."
        End Try

        Return erreur
    End Function

    ''' <summary>
    ''' Méthode permettant d'effacer un utilisateur de la configuration.
    ''' </summary>
    ''' <param name="codeUtilisateur">Le code d'identification unique de l'utilisateur que vous voulez effacer.</param>
    ''' <returns>
    ''' Retour de type erreur. 
    ''' Une chaine de caractères vide("") indique qu'il n'y a pas eu d'erreur.
    ''' Dans le cas contraire l'erreur sera décrit dans le paramètre de retour.
    ''' </returns>
    Public Function EffacerUser(ByVal codeUtilisateur As String) As String
        Dim erreur As String = ""

        Try
            TsBaConfigSage.RemoveConfigurationUser(Config, codeUtilisateur)
        Catch ex As ApplicationException
            erreur = "L'utilisateur n'a pu être effacé de la configuration."
        End Try

        Return erreur
    End Function

    ''' <summary>
    ''' Méthode permettant d'effacer la ressource de la configuration.
    ''' </summary>
    ''' <param name="resname1">Premier tier de la clé unique de la ressource.</param>
    ''' <param name="resname2">Deuxième tier de la clé unique de la ressource.</param>
    ''' <param name="resname3">Troisème tier de la clé unique de la ressource.</param>
    ''' <returns>
    ''' Retour de type erreur. 
    ''' Une chaine de caractères vide("") indique qu'il n'y a pas eu d'erreur.
    ''' Dans le cas contraire l'erreur sera décrit dans le paramètre de retour.
    ''' </returns>
    Public Function EffacerRessr(ByVal resname1 As String, ByVal resname2 As String, ByVal resname3 As String) As String
        Dim erreur As String = ""

        Try
            TsBaConfigSage.RemoveConfigurationResource(Config, resname1, resname2, resname3)
        Catch ex As ApplicationException
            erreur = "La ressource n'a pu être effacé de la configuration."
        End Try

        Return erreur
    End Function

    ''' <summary>
    ''' Méthode permettant d'ajouter de l'informations à d'un champ attribut d'un rôle.
    ''' </summary>
    ''' <param name="nomRole">Le nom unique du rôle.</param>
    ''' <param name="nomNormaliserAttrb">Le nom normaliser du champs attribut à changer.</param>
    ''' <param name="valeur">La valeur qui sera ajoutée au champ attribut.</param>
    ''' <returns>
    ''' Retour de type erreur. 
    ''' Une chaine de caractères vide("") indique qu'il n'y a pas eu d'erreur.
    ''' Dans le cas contraire l'erreur sera décrit dans le paramètre de retour.
    ''' </returns>
    Public Function AjouterAttrbRole(ByVal nomRole As String, ByVal nomNormaliserAttrb As String, ByVal valeur As String) As String
        Dim erreur As String = ""

        Dim nomChamp As String = TrouverNomChamp(nomNormaliserAttrb)

        Try
            TsBaConfigSage.UpdateRoleField(Config, nomRole, nomChamp, valeur)
        Catch ex As ApplicationException
            erreur = "Le champ du rôle n'a pas pu être changé."
        End Try

        Return erreur
    End Function

    ''' <summary>
    ''' Méthode permettant d'ajouter de l'informations à d'un champ attribut d'un utilisateur.
    ''' </summary>
    ''' <param name="codeUtilisateur">Le code unique de l'utilisateur.</param>
    ''' <param name="nomNormaliserAttrb">Le nom normaliser du champs attribut à changer.</param>
    ''' <param name="valeur">La valeur qui sera ajoutée au champ attribut.</param>
    ''' <returns>
    ''' Retour de type erreur. 
    ''' Une chaine de caractères vide("") indique qu'il n'y a pas eu d'erreur.
    ''' Dans le cas contraire l'erreur sera décrit dans le paramètre de retour.
    ''' </returns>
    Public Function AjouterAttrbUser(ByVal codeUtilisateur As String, ByVal nomNormaliserAttrb As String, ByVal valeur As String) As String
        Dim erreur As String = ""

        Dim nomChamp As String = TrouverNomChamp(nomNormaliserAttrb)

        Try
            TsBaConfigSage.UpdateUserField(Config, codeUtilisateur, nomChamp, valeur)
        Catch ex As ApplicationException
            erreur = "Le champ de l'utilisateur n'a pas pu être changé."
        End Try

        Return erreur
    End Function

    ''' <summary>
    ''' Méthode permettant d'ajouter de l'informations à d'un champ attribut d'une ressource.
    ''' </summary>
    ''' <param name="resname1">Premier tier de la clé unique de la ressource.</param>
    ''' <param name="resname2">Deuxième tier de la clé unique de la ressource.</param>
    ''' <param name="resname3">Troisème tier de la clé unique de la ressource.</param>
    ''' <param name="nomNormaliserAttrb">Le nom normaliser du champs attribut à changer.</param>
    ''' <param name="valeur">La valeur qui sera ajoutée au champ attribut.</param>>
    ''' <returns>
    ''' Retour de type erreur. 
    ''' Une chaine de caractères vide("") indique qu'il n'y a pas eu d'erreur.
    ''' Dans le cas contraire l'erreur sera décrit dans le paramètre de retour.
    ''' </returns>
    Public Function AjouterAttrbRessr(ByVal resName1 As String, ByVal resName2 As String, ByVal resName3 As String, ByVal nomNormaliserAttrb As String, ByVal valeur As String) As String
        Dim erreur As String = ""

        Dim numeroChamp As Integer = TrouverNumeroChamp(nomNormaliserAttrb)
        Dim nomChamp As String = TrouverNomChamp(nomNormaliserAttrb)

        Try
            TsBaConfigSage.UpdateResourceField(Config, resName1, resName2, resName3, nomChamp, valeur)
        Catch ex As ApplicationException
            erreur = "Le champ de la ressource n'a pas pu être changé."
        End Try

        Return erreur
    End Function

    ''' <summary>
    ''' Méthode permettant d'effacer l'informations du champs attribut d'un rôle.
    ''' </summary>
    ''' <param name="nomRole">Le nom du rôle.</param>
    ''' <param name="nomNormaliserAttrb">Le nom normaliser du champs attribut à effacer.</param>
    ''' <returns>
    ''' Retour de type erreur. 
    ''' Une chaine de caractères vide("") indique qu'il n'y a pas eu d'erreur.
    ''' Dans le cas contraire l'erreur sera décrit dans le paramètre de retour.
    ''' </returns>
    Public Function EffacerAttrbRole(ByVal nomRole As String, ByVal nomNormaliserAttrb As String) As String
        Dim erreur As String = ""

        Dim nomChamp As String = TrouverNomChamp(nomNormaliserAttrb)

        Try
            TsBaConfigSage.UpdateRoleField(Config, nomRole, nomChamp, "")
        Catch ex As ApplicationException
            erreur = "Le champ du rôle n'a pu être effacé."
        End Try

        Return erreur
    End Function

    ''' <summary>
    ''' Méthode permettant d'effacer l'informations du champs attribut d'un utilisateur.
    ''' </summary>
    ''' <param name="codeUtilisateur">le code identifiant l'utilisateur.</param>
    ''' <param name="nomNormaliserAttrb">Le nom normaliser du champs attribut à effacer.</param>
    ''' <returns>
    ''' Retour de type erreur. 
    ''' Une chaine de caractères vide("") indique qu'il n'y a pas eu d'erreur.
    ''' Dans le cas contraire l'erreur sera décrit dans le paramètre de retour.
    ''' </returns>
    Public Function EffacerAttrbUser(ByVal codeUtilisateur As String, ByVal nomNormaliserAttrb As String) As String
        Dim erreur As String = ""

        Dim numeroChamp As Integer = TrouverNumeroChamp(nomNormaliserAttrb)
        Dim nomChamp As String = TrouverNomChamp(nomNormaliserAttrb)

        Try
            TsBaConfigSage.UpdateUserField(Config, codeUtilisateur, nomChamp, "")
        Catch ex As ApplicationException
            erreur = "Le champ de l'utilisateur n'a pu être effacé."
        End Try

        Return erreur
    End Function

    ''' <summary>
    ''' Méthode permettant d'effacer l'informations du champs attribut d'un utilisateur.
    ''' </summary>
    ''' <param name="resName1">Premier tier de la identifiant de la ressource.</param>
    ''' <param name="resName2">Deuxième tier de la identifiant de la ressource.</param> 
    ''' <param name="resName3">Troisième tier de la identifiant de la ressource.</param> 
    ''' <param name="nomNormaliserAttrb">Le nom normaliser du champs attribut à effacer.</param>
    ''' <returns>
    ''' Retour de type erreur. 
    ''' Une chaine de caractères vide("") indique qu'il n'y a pas eu d'erreur.
    ''' Dans le cas contraire l'erreur sera décrit dans le paramètre de retour.
    ''' </returns>
    Public Function EffacerAttrbRessr(ByVal resName1 As String, ByVal resName2 As String, ByVal resName3 As String, ByVal nomNormaliserAttrb As String) As String
        Dim erreur As String = ""

        Dim numeroChamp As Integer = TrouverNumeroChamp(nomNormaliserAttrb)
        Dim nomChamp As String = TrouverNomChamp(nomNormaliserAttrb)

        Try
            TsBaConfigSage.UpdateResourceField(Config, resName1, resName2, resName3, nomChamp, "")
        Catch ex As ApplicationException
            erreur = "Le champ de la ressource n'a pu être effacé."
        End Try

        Return erreur
    End Function

    ''' <summary>
    ''' Méthode permettant de créer un lien entre deux rôle.
    ''' </summary>
    ''' <param name="nomRoleSup">Le nom du rôle supérieur. Hérite des autorisation du sous rôle.</param>
    ''' <param name="nomSousRole">Le nom du sous-rôle. Le sous rôle donne ses autorisation au rôle supérieur.</param>
    ''' <returns>
    ''' Retour de type erreur. 
    ''' Une chaine de caractères vide("") indique qu'il n'y a pas eu d'erreur.
    ''' Dans le cas contraire l'erreur sera décrit dans le paramètre de retour.
    ''' </returns>
    Public Function AjouerLienRoleRole(ByVal nomRoleSup As String, ByVal nomSousRole As String) As String
        Dim erreur As String = ""

        Try
            TsBaConfigSage.AddRoleRoleLink(Config, nomRoleSup, nomSousRole)
        Catch ex As ApplicationException
            erreur = "Le lien rôle-rôle n'a pas pu être créé."
        End Try

        Return erreur
    End Function

    ''' <summary>
    ''' Méthode permettant de supprimer un lien entre deux rôle.
    ''' </summary>
    ''' <param name="nomRoleSup">Le nom du rôle supérieur. Hérite des autorisation du sous rôle.</param>
    ''' <param name="nomSousRole">Le nom du sous-rôle. Le sous rôle donne ses autorisation au rôle supérieur.</param>
    ''' <returns>
    ''' Retour de type erreur. 
    ''' Une chaine de caractères vide("") indique qu'il n'y a pas eu d'erreur.
    ''' Dans le cas contraire l'erreur sera décrit dans le paramètre de retour.
    ''' </returns>
    Public Function EffacerLienRoleRole(ByVal nomRoleSup As String, ByVal nomSousRole As String) As String
        Dim erreur As String = ""

        Try
            TsBaConfigSage.RemoveRoleRoleLink(Config, nomRoleSup, nomSousRole)
        Catch ex As ApplicationException
            erreur = "Le lien rôle-rôle n'a pas pu être effacé."
        End Try

        Return erreur
    End Function

    ''' <summary>
    ''' Méthode permettant de lier un utilisateur à un rôle.
    ''' </summary>
    ''' <param name="codeUtilisateur">Le code d'identification de l'utilisateur.</param>
    ''' <param name="nomRole">Le nom du rôle.</param>
    ''' <returns>
    ''' Retour de type erreur. 
    ''' Une chaine de caractères vide("") indique qu'il n'y a pas eu d'erreur.
    ''' Dans le cas contraire l'erreur sera décrit dans le paramètre de retour.
    ''' </returns>
    Public Function AjouerLienUserRole(ByVal codeUtilisateur As String, ByVal nomRole As String) As String
        Dim erreur As String = ""

        Try
            TsBaConfigSage.AddLinkUserRole(Config, codeUtilisateur, nomRole)
        Catch ex As ApplicationException
            erreur = "Le lien utilisateur-rôle n'a pas pu être créé."
        End Try

        Return erreur
    End Function

    ''' <summary>
    ''' Méthode permettant d'effacer un lien entre un utilisateur et un rôle.
    ''' </summary>
    ''' <param name="codeUtilisateur">Le code d'identification de l'utilisateur.</param>
    ''' <param name="nomRole">Le nom du rôle.</param>
    ''' <returns>
    ''' Retour de type erreur. 
    ''' Une chaine de caractères vide("") indique qu'il n'y a pas eu d'erreur.
    ''' Dans le cas contraire l'erreur sera décrit dans le paramètre de retour.
    ''' </returns>
    Public Function EffacerLienUserRole(ByVal codeUtilisateur As String, ByVal nomRole As String) As String
        Dim erreur As String = ""

        Try
            TsBaConfigSage.RemoverUserRoleLink(Config, codeUtilisateur, nomRole)
        Catch ex As ApplicationException
            erreur = "Le lien utilisateur-rôle n'a pas pus être effacé."
        End Try

        Return erreur
    End Function

    ''' <summary>
    ''' Méthode permettant de lier un rôle et une ressource.
    ''' </summary>
    ''' <param name="resName1">Premier tier de la identifiant de la ressource.</param>
    ''' <param name="resName2">Deuxième tier de la identifiant de la ressource.</param>
    ''' <param name="resName3">Troisième tier de la identifiant de la ressource.</param>
    ''' <param name="nomRole">Le nom du rôle.</param>
    ''' <returns>
    ''' Retour de type erreur. 
    ''' Une chaine de caractères vide("") indique qu'il n'y a pas eu d'erreur.
    ''' Dans le cas contraire l'erreur sera décrit dans le paramètre de retour.
    ''' </returns>
    Public Function AjouterLienRoleRessr(ByVal resName1 As String, ByVal resName2 As String, ByVal resName3 As String, ByVal nomRole As String) As String
        Dim erreur As String = ""

        Try
            TsBaConfigSage.AddResourceRoleLink(Config, resName1, resName2, resName3, nomRole)
        Catch ex As ApplicationException
            erreur = "Le lien rôle-ressource n'a pas pu être créé."
        End Try

        Return erreur
    End Function

    ''' <summary>
    ''' Méthode permettant d'effacer un lien entre un rôle et une ressource.
    ''' </summary>
    ''' <param name="resName1">Premier tier de la identifiant de la ressource.</param>
    ''' <param name="resName2">Deuxième tier de la identifiant de la ressource.</param>
    ''' <param name="resName3">Troisième tier de la identifiant de la ressource.</param>
    ''' <param name="nomRole">Le nom du rôle.</param>
    ''' <returns>
    ''' Retour de type erreur. 
    ''' Une chaine de caractères vide("") indique qu'il n'y a pas eu d'erreur.
    ''' Dans le cas contraire l'erreur sera décrit dans le paramètre de retour.
    ''' </returns>
    Public Function EffacerLienRoleRessr(ByVal resName1 As String, ByVal resName2 As String, ByVal resName3 As String, ByVal nomRole As String) As String
        Dim erreur As String = ""

        Try
            TsBaConfigSage.RemoveResourceRoleLink(Config, resName1, resName2, resName3, nomRole)
        Catch ex As ApplicationException
            erreur = "Le lien rôle-ressource n'a pas pu être effacé."
        End Try

        Return erreur
    End Function

    ''' <summary>
    ''' Méthode permettant de créer un lien entre un utilisateur et une ressource.
    ''' </summary>
    ''' <param name="resName1">Premier tier de la identifiant de la ressource.</param>
    ''' <param name="resName2">Deuxième tier de la identifiant de la ressource.</param>
    ''' <param name="resName3">Troisième tier de la identifiant de la ressource.</param>
    ''' <param name="codeUtilisateur">Le code d'identification de l'utilisateur.</param>
    ''' <returns>
    ''' Retour de type erreur. 
    ''' Une chaine de caractères vide("") indique qu'il n'y a pas eu d'erreur.
    ''' Dans le cas contraire l'erreur sera décrit dans le paramètre de retour.
    ''' </returns>
    Public Function AjouterLienUserRessr(ByVal resName1 As String, ByVal resName2 As String, ByVal resName3 As String, ByVal codeUtilisateur As String) As String
        Dim erreur As String = ""

        Try
            TsBaConfigSage.AddUserResourceLink(Config, codeUtilisateur, resName1, resName2, resName3)
        Catch ex As ApplicationException
            erreur = "Le lien utilisateur-ressource n'a pas pu être créé."
        End Try

        Return erreur
    End Function

    ''' <summary>
    ''' Méthode permettant d'effacer un lien entre un utilisateur et une ressource.
    ''' </summary>
    ''' <param name="resName1">Premier tier de la identifiant de la ressource.</param>
    ''' <param name="resName2">Deuxième tier de la identifiant de la ressource.</param>
    ''' <param name="resName3">Troisième tier de la identifiant de la ressource.</param>
    ''' <param name="codeUtilisateur">Le code d'identification de l'utilisateur.</param>
    ''' <returns>
    ''' Retour de type erreur. 
    ''' Une chaine de caractères vide("") indique qu'il n'y a pas eu d'erreur.
    ''' Dans le cas contraire l'erreur sera décrit dans le paramètre de retour.
    ''' </returns>
    Public Function EffacerLienUserRessr(ByVal resName1 As String, ByVal resName2 As String, ByVal resName3 As String, ByVal codeUtilisateur As String) As String
        Dim erreur As String = ""

        Try
            TsBaConfigSage.RemoveUserResourceLink(Config, codeUtilisateur, resName1, resName2, resName3)
        Catch ex As ApplicationException
            erreur = "Le lien utilisateur-ressource n'a pas pu être effacé."
        End Try

        Return erreur
    End Function

    ''' <summary>
    ''' Méthode détectant si le rôle est présent dans la configuration.
    ''' </summary>
    ''' <param name="nomRole">Le nom du rôle.</param>
    ''' <returns>True, si le rôle est présent, False sinon.</returns>
    Public Function PresentRole(ByVal nomRole As String) As Boolean
        Dim tousOk As Boolean = False

        For Each role As TsCdSageRole In ObtenirListeRoles()
            If role.Name = nomRole Then
                tousOk = True
                Exit For
            End If
        Next

        Return tousOk
    End Function

    ''' <summary>
    ''' Méthode détectant si le rôle est absent dans la configuration.
    ''' </summary>
    ''' <param name="nomRole">Le nom du rôle.</param>
    ''' <returns>True, si le rôle est absent, False sinon.</returns>
    Public Function AbsentRole(ByVal nomRole As String) As Boolean
        Return Not PresentRole(nomRole)
    End Function

    ''' <summary>
    ''' Méthode détectant si l'utilisateur est présent dans la configuration.
    ''' </summary>
    ''' <param name="codeUtilisateur">Le code d'identification de l'utilisateur.</param>
    ''' <returns>True, si l'utilisateur est présent, False sinon.</returns>
    Public Function PresentUser(ByVal codeUtilisateur As String) As Boolean
        Dim tousOk As Boolean = False

        For Each utilisateur As TsCdSageUser In ObtenirListeUsers()
            If utilisateur.PersonID = codeUtilisateur Then
                tousOk = True
                Exit For
            End If
        Next

        Return tousOk
    End Function

    ''' <summary>
    ''' Méthode détectant si l'utilisateur est absent dans la configuration.
    ''' </summary>
    ''' <param name="codeUtilisateur">Le code d'identification de l'utilisateur.</param>
    ''' <returns>True, si l'utilisateur est absent, False sinon.</returns>
    Public Function AbsentUser(ByVal codeUtilisateur As String) As Boolean
        Return Not PresentUser(codeUtilisateur)
    End Function

    ''' <summary>
    ''' Méthode détectant si la ressource est présent dans la configuration.
    ''' </summary>
    ''' <param name="nomRessource">Premier tier de la identifiant de la ressource.</param>
    ''' <param name="catgrRessource">Deuxième tier de la identifiant de la ressource.</param>
    ''' <param name="cible">Troisième tier de la identifiant de la ressource.</param>
    ''' <returns>True, si la ressource est présent, False sinon.</returns>
    Public Function PresentRessr(ByVal nomRessource As String, ByVal catgrRessource As String, ByVal cible As String) As Boolean
        Dim tousOk As Boolean = False

        For Each ressource As TsCdSageResource In ObtenirListeRessr()
            If ressource.ResName1 = nomRessource And ressource.ResName2 = catgrRessource And ressource.ResName3 = cible Then
                tousOk = True
                Exit For
            End If
        Next

        Return tousOk
    End Function

    ''' <summary>
    ''' Méthode détectant si la ressource est absent dans la configuration.
    ''' </summary>
    ''' <param name="nomRessource">Premier tier de la identifiant de la ressource.</param>
    ''' <param name="catgrRessource">Deuxième tier de la identifiant de la ressource.</param>
    ''' <param name="cible">Troisième tier de la identifiant de la ressource.</param>
    ''' <returns>True, si la ressource est absent, False sinon.</returns>
    Public Function AbsentRessr(ByVal nomRessource As String, ByVal catgrRessource As String, ByVal cible As String) As Boolean
        Return Not PresentRessr(nomRessource, catgrRessource, cible)
    End Function

    ''' <summary>
    ''' Méthode détectant si le lien rôle rôle est présent dans la configuration.
    ''' </summary>
    ''' <param name="roleSup">Le nom du rôle supérieur. Rôle supérieur hérite des autorisations du sous rôle.</param>
    ''' <param name="sousRole">Le nom du sous rôle. Le sous rôle donne ses autorisations au rôle supérieur.</param>
    ''' <returns>True, si le lien est présent, False sinon.</returns>
    Public Function PresentLienRoleRole(ByVal roleSup As String, ByVal sousRole As String) As Boolean
        Dim tousOk As Boolean = False

        For Each lienRoleRole As TsCdSageRoleRoleLink In ObtenirListeLiensRoleRole()
            If lienRoleRole.ParentRole = roleSup And lienRoleRole.ChildRole = sousRole Then
                tousOk = True
                Exit For
            End If
        Next

        Return tousOk
    End Function

    ''' <summary>
    ''' Méthode détectant si le lien rôle/rôle est absent dans la configuration.
    ''' </summary>
    ''' <param name="roleSup">Le nom du rôle supérieur. Rôle supérieur hérite des autorisations du sous rôle.</param>
    ''' <param name="sousRole">Le nom du sous rôle. Le sous rôle donne ses autorisations au rôle supérieur.</param>
    ''' <returns>True, si le lien est absent, False sinon.</returns>
    Public Function AbsentLienRoleRole(ByVal roleSup As String, ByVal sousRole As String) As Boolean
        Return Not PresentLienRoleRole(roleSup, sousRole)
    End Function

    ''' <summary>
    ''' Méthode détectant si le lien utilisateur/rôle est présent dans la configuration.
    ''' </summary>
    ''' <param name="codeUtilisateur">Le code d'identification de l'utilisateur.</param>
    ''' <param name="nomRole">Le nom du rôle.</param>
    ''' <returns>True, si le lien est présent, False sinon.</returns>
    Public Function PresentLienUserRole(ByVal codeUtilisateur As String, ByVal nomRole As String) As Boolean
        Dim tousOk As Boolean = False

        For Each lienUserRole As TsCdSageUserRoleLink In ObtenirListeLiensUserRole()
            If lienUserRole.PersonID = codeUtilisateur And lienUserRole.RoleName = nomRole Then
                tousOk = True
                Exit For
            End If
        Next

        Return tousOk
    End Function

    ''' <summary>
    ''' Méthode détectant si le lien utilisateur/rôle est absent dans la configuration.
    ''' </summary>
    ''' <param name="codeUtilisateur">Le code d'identification de l'utilisateur.</param>
    ''' <param name="nomRole">Le nom du rôle.</param>
    ''' <returns>True, si le lien est absent, False sinon.</returns>
    Public Function AbsentLienUserRole(ByVal codeUtilisateur As String, ByVal nomRole As String) As Boolean
        Return Not PresentLienUserRole(codeUtilisateur, nomRole)
    End Function

    ''' <summary>
    ''' Méthode détectant si le lien rôle/ressource est présent dans la configuration.
    ''' </summary>
    ''' <param name="nomRessource">Premier tier de la identifiant de la ressource.</param>
    ''' <param name="catgrRessource">Deuxième tier de la identifiant de la ressource.</param>
    ''' <param name="cible">Troisième tier de la identifiant de la ressource.</param>
    ''' <param name="nomRole">Le nom du rôle.</param>
    ''' <returns>True, si le lien est présent, False sinon.</returns>
    Public Function PresentLienRoleRessr(ByVal nomRessource As String, ByVal catgrRessource As String, ByVal cible As String, ByVal nomRole As String) As Boolean
        Dim tousOk As Boolean = False

        For Each lienRoleRessr As TsCdSageRoleResLink In ObtenirListeLiensRoleRessr()
            If lienRoleRessr.ResName1 = nomRessource And lienRoleRessr.ResName2 = catgrRessource And lienRoleRessr.ResName3 = cible And lienRoleRessr.RoleName = nomRole Then
                tousOk = True
                Exit For
            End If
        Next

        Return tousOk
    End Function

    ''' <summary>
    ''' Méthode détectant si le lien rôle/ressource est absent dans la configuration.
    ''' </summary>
    ''' <param name="nomRessource">Premier tier de la identifiant de la ressource.</param>
    ''' <param name="catgrRessource">Deuxième tier de la identifiant de la ressource.</param>
    ''' <param name="cible">Troisième tier de la identifiant de la ressource.</param>
    ''' <param name="nomRole">Le nom du rôle.</param>
    ''' <returns>True, si le lien est absent, False sinon.</returns>
    Public Function AbsentLienRoleRessr(ByVal nomRessource As String, ByVal catgrRessource As String, ByVal cible As String, ByVal nomRole As String) As Boolean
        Return Not PresentLienRoleRessr(nomRessource, catgrRessource, cible, nomRole)
    End Function

    ''' <summary>
    ''' Méthode détectant si le lien rôle/ressource est présent dans la configuration.
    ''' </summary>
    ''' <param name="nomRessource">Premier tier de la identifiant de la ressource.</param>
    ''' <param name="catgrRessource">Deuxième tier de la identifiant de la ressource.</param>
    ''' <param name="cible">Troisième tier de la identifiant de la ressource.</param>
    ''' <param name="codeUtilisateur">Le code d'identification de l'utilisateur.</param>
    ''' <returns>True, si le lien est présent, False sinon.</returns>
    Public Function PresentLienUserRessr(ByVal nomRessource As String, ByVal catgrRessource As String, ByVal cible As String, ByVal codeUtilisateur As String) As Boolean
        Dim tousOk As Boolean = False

        For Each lienUserRessr As TsCdSageUserResLink In ObtenirListeLiensUserRessr()
            If lienUserRessr.ResName1 = nomRessource And lienUserRessr.ResName2 = catgrRessource And lienUserRessr.ResName3 = cible And lienUserRessr.PersonID = codeUtilisateur Then
                tousOk = True
                Exit For
            End If
        Next

        Return tousOk
    End Function

    ''' <summary>
    ''' Méthode détectant si le lien rôle/ressource est absent dans la configuration.
    ''' </summary>
    ''' <param name="nomRessource">Premier tier de la identifiant de la ressource.</param>
    ''' <param name="catgrRessource">Deuxième tier de la identifiant de la ressource.</param>
    ''' <param name="cible">Troisième tier de la identifiant de la ressource.</param>
    ''' <param name="codeUtilisateur">Le code d'identification de l'utilisateur.</param>
    ''' <returns>True, si le lien est absent, False sinon.</returns>
    Public Function AbsentLienUserRessr(ByVal nomRessource As String, ByVal catgrRessource As String, ByVal cible As String, ByVal codeUtilisateur As String) As Boolean
        Return Not PresentLienUserRessr(nomRessource, catgrRessource, cible, codeUtilisateur)
    End Function

    ''' <summary>
    ''' Méthode permettant d'identifier si le champ attribut d'un rôle est égale au changement désiré.
    ''' </summary>
    ''' <param name="nomRole">Le nom du rôle.</param>
    ''' <param name="nomChampAttrbNorm">Le nom du champ d'attribut.</param>
    ''' <param name="valeur">La valeur à comparer.</param>
    ''' <returns>True, si les valeurs sont égale, False sinon.</returns>
    Public Function EgaleAttrbRole(ByVal nomRole As String, ByVal nomChampAttrbNorm As String, ByVal valeur As String) As Boolean
        Dim egale As Boolean = False

        For Each role As TsCdSageRole In ObtenirListeRoles() '! Liste des rôles.
            If role.Name = nomRole Then '! Trouver le bon rôle.
                For Each f As System.Reflection.FieldInfo In lstFieldInfoRole '! Liste des champs du rôle.
                    Dim nomChampNormaliser As String = CType(f.GetCustomAttributes(GetType(TsAtNomChampGen), True)(0), TsAtNomChampGen).NomChamp
                    If nomChampAttrbNorm = nomChampNormaliser Then '! Trouver le bon champ correspondant.
                        If valeur.Trim() = CType(f.GetValue(role), String).Trim() Then '! Est-ce que les valeurs s'égales.
                            egale = True
                        Else
                            egale = False
                        End If
                        Exit For
                    End If
                Next
                Exit For
            End If
        Next

        Return egale
    End Function

    ''' <summary>
    ''' Méthode permettant d'identifier si le champ attribut d'un utilisateur est égale au changement désiré.
    ''' </summary>
    ''' <param name="codeUtilisateur">Le nom du rôle.</param>
    ''' <param name="nomChampAttrbNorm">Le nom du champ d'attribut.</param>
    ''' <param name="valeur">La valeur à comparer.</param>
    ''' <returns>True, si les valeurs sont égale, False sinon.</returns>
    Public Function EgaleAttrbUser(ByVal codeUtilisateur As String, ByVal nomChampAttrbNorm As String, ByVal valeur As String) As Boolean
        Dim egale As Boolean = False

        For Each user As TsCdSageUser In ObtenirListeUsers() '! Liste des utilisateurs.
            If user.PersonID = codeUtilisateur Then '! Trouver le bon utilisateur.
                For Each f As System.Reflection.FieldInfo In lstFieldInfoUser '! Liste des champs de l'utilisateur.
                    Dim nomChampNormaliser As String = CType(f.GetCustomAttributes(GetType(TsAtNomChampGen), True)(0), TsAtNomChampGen).NomChamp
                    If nomChampAttrbNorm = nomChampNormaliser Then '! Trouver le bon champ correspondant.
                        If valeur.Trim() = CType(f.GetValue(user), String).Trim() Then '! Est-ce que les valeurs s'égales.
                            egale = True
                        Else
                            egale = False
                        End If
                        Exit For
                    End If
                Next
                Exit For
            End If
        Next

        Return egale
    End Function

    ''' <summary>
    ''' Méthode permettant d'identifier si le champ attribut d'un utilisateur est égale au changement désiré.
    ''' </summary>
    ''' <param name="resname1">Premier tier de l'identifiant de la ressource.</param>
    ''' <param name="resname2">Deuxième tier de l'identifiant de la ressource.</param>
    ''' <param name="resname3">Troisème tier de l'identifiant de la ressource.</param>
    ''' <param name="nomChampAttrbNorm">Le nom du champ d'attribut.</param>
    ''' <param name="valeur">La valeur à comparer.</param>
    ''' <returns>True, si les valeurs sont égale, False sinon.</returns>
    Public Function EgaleAttrbRessr(ByVal resname1 As String, ByVal resname2 As String, ByVal resname3 As String, ByVal nomChampAttrbNorm As String, ByVal valeur As String) As Boolean
        Dim egale As Boolean = False

        For Each ressource As TsCdSageResource In ObtenirListeRessr() '! Liste des ressources.
            If ressource.ResName1 = resname1 And ressource.ResName2 = resname2 And ressource.ResName3 = resname3 Then '! Trouver le bon utilisateur.
                For Each f As System.Reflection.FieldInfo In lstFieldInfoRessr '! Liste des champs de l'utilisateur.
                    Dim nomChampNormaliser As String = CType(f.GetCustomAttributes(GetType(TsAtNomChampGen), True)(0), TsAtNomChampGen).NomChamp
                    If nomChampAttrbNorm = nomChampNormaliser Then '! Trouver le bon champ correspondant.
                        If valeur.Trim() = CType(f.GetValue(ressource), String).Trim() Then '! Est-ce que les valeurs s'égales.
                            egale = True
                        Else
                            egale = False
                        End If
                        Exit For
                    End If
                Next
                Exit For
            End If
        Next

        Return egale
    End Function

    ''' <summary>
    ''' Méthode qui permette de vider la cache de lecture.
    ''' Utile après une modification.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ViderCache()
        lstRoles = Nothing
        lstUsers = Nothing
        lstRessr = Nothing

        lstLiensRoleRole = Nothing
        lstLiensUserRole = Nothing
        lstLiensRoleRessr = Nothing
        lstLiensUserRessr = Nothing

        lstChampUser = Nothing
        lstChampRessr = Nothing

        TsBaConfigSage.ClearCache()
    End Sub

    ''' <summary>
    ''' Méthode. Fait une série de test pour précisé l'erreur.
    ''' </summary>
    ''' <param name="utilisateur">L'utilisateur, dont l'erreur est à préciser.</param>
    ''' <param name="modeAjout">Est-ce dans le cadre d'un ajout.</param>
    ''' <returns>L'erreur précisé.</returns>
    ''' <remarks>Si l'utilisateur dans la UDB n'est pas associé dans une configuration, il sera invisible dans la UDB. On ne peut savoir s'il existe.</remarks>
    Public Function DeffnErreurUser(ByVal utilisateur As TsCdConnxUser, ByVal modeAjout As Boolean) As String
        Dim paramRetour As String = utilisateur.DescriptionErreur

        Dim nomUtilisateur As String = utilisateur.CodeUtilisateur

        Select Case modeAjout
            Case True
                If PresentUser(utilisateur.CodeUtilisateur) = True Then
                    paramRetour = "L'utilisateur: """ + nomUtilisateur + """ est déja présent dans la config cible: """ + Config + """"
                    Exit Select
                End If
            Case False
                If PresentUser(utilisateur.CodeUtilisateur) = False Then
                    paramRetour = "L'utilisateur: """ + nomUtilisateur + """ n'est pas présent dans la config cible: """ + Config + """"
                End If
        End Select

        Return paramRetour
    End Function

    ''' <summary>
    ''' Méthode. Fait une série de test pour précisé l'erreur.
    ''' </summary>
    ''' <param name="role">Le rôle, dont l'erreur est à préciser.</param>
    ''' <param name="modeAjout">Est-ce dans le cadre d'un ajout.</param>
    ''' <returns>L'erreur précisé.</returns>
    Public Function DeffnErreurRole(ByVal role As TsCdConnxRole, ByVal modeAjout As Boolean) As String
        Dim paramRetour As String = role.DescriptionErreur

        Dim nomRole As String = role.NomRole

        Select Case modeAjout
            Case True
                If PresentRole(role.NomRole) = True Then
                    paramRetour = "Le rôle: """ + nomRole + """ est déja présent dans la base de données cible: """ + Config + """"
                    Exit Select
                End If
            Case False
                If PresentRole(role.NomRole) = False Then
                    paramRetour = "Le rôle: """ + nomRole + """ n'est pas présent dans la base de données cible: """ + Config + """"
                End If
        End Select

        Return paramRetour
    End Function

    ''' <summary>
    ''' Méthode. Fait une série de test pour précisé l'erreur.
    ''' </summary>
    ''' <param name="ressource">La ressource, dont l'erreur est à préciser.</param>
    ''' <param name="modeAjout">Est-ce dans le cadre d'un ajout.</param>
    ''' <returns>L'erreur précisé.</returns>
    ''' <remarks>Si la ressource dans la RDB n'est pas associé dans une configuration, elle sera invisible dans la RDB. On ne peut savoir si elle existe.</remarks>
    Public Function DeffnErreurRessr(ByVal ressource As TsCdConnxRessr, ByVal modeAjout As Boolean, ByVal idCible As String) As String
        Dim paramRetour As String = ressource.DescriptionErreur

        Dim nomRessource As String = ressource.NomRessource + ", " + ressource.CatgrRessource + ", " + idCible

        Select Case modeAjout
            Case True
                If PresentRessr(ressource.NomRessource, ressource.CatgrRessource, idCible) = True Then
                    paramRetour = "La ressource: """ + nomRessource + """ est déja présent dans la config cible: """ + Config + """"
                    Exit Select
                End If
            Case False
                If PresentRessr(ressource.NomRessource, ressource.CatgrRessource, idCible) = False Then
                    paramRetour = "La ressource: """ + nomRessource + """ n'est pas présent dans la config cible: """ + Config + """"
                End If
        End Select

        Return paramRetour
    End Function

    ''' <summary>
    ''' Méthode. Fait une série de test pour précisé l'erreur.
    ''' </summary>
    ''' <param name="Attribut">L'attibut d'un utilisateur, dont l'erreur est à préciser.</param>
    ''' <param name="modeAjout">Est-ce dans le cadre d'un ajout.</param>
    ''' <returns>L'erreur précisé.</returns>
    Public Function DeffnErreurUserAttbr(ByVal attribut As TsCdConnxUserAttrb, ByVal modeAjout As Boolean) As String
        Dim paramRetour As String = attribut.DescriptionErreur

        Dim nomUtilisateur As String = attribut.CodeUtilisateur

        Select Case modeAjout
            Case True, False
                If PresentUser(attribut.CodeUtilisateur) = False Then
                    paramRetour = "L'utilisateur: """ + nomUtilisateur + """ n'est pas présent dans la config cible: """ + Config + """"
                ElseIf ValiderNomChampUser(attribut) Then
                    paramRetour = "Le champ attribut: """ + attribut.NomAttrb + """ n'est pas conforme avec l'enrichissement de la config cible: """ + Config + """"
                End If
        End Select

        Return paramRetour
    End Function

    ''' <summary>
    ''' Méthode. Fait une série de test pour précisé l'erreur.
    ''' </summary>
    ''' <param name="Attribut">L'attibut d'un rôle, dont l'erreur est à préciser.</param>
    ''' <param name="modeAjout">Est-ce dans le cadre d'un ajout.</param>
    ''' <returns>L'erreur précisé.</returns>
    Public Function DeffnErreurRoleAttbr(ByVal attribut As TsCdConnxRoleAttrb, ByVal modeAjout As Boolean) As String
        Dim paramRetour As String = attribut.DescriptionErreur

        Dim nomRole As String = attribut.NomRole

        Select Case modeAjout
            Case True, False
                If PresentRole(attribut.NomRole) = False Then
                    paramRetour = "Le rôle: """ + nomRole + """ n'est pas présent dans la config cible: """ + Config + """"
                End If
        End Select

        Return paramRetour
    End Function

    ''' <summary>
    ''' Méthode. Fait une série de test pour précisé l'erreur.
    ''' </summary>
    ''' <param name="Attribut">L'attibut d'une ressource, dont l'erreur est à préciser.</param>
    ''' <param name="modeAjout">Est-ce dans le cadre d'un ajout.</param>
    ''' <returns>L'erreur précisé.</returns>
    Public Function DeffnErreurRessrAttbr(ByVal attribut As TsCdConnxRessrAttrb, ByVal modeAjout As Boolean, ByVal idCible As String) As String
        Dim paramRetour As String = attribut.DescriptionErreur

        Dim nomRessource As String = attribut.NomRessource + ", " + attribut.CatgrRessource + ", " + idCible

        Select Case modeAjout
            Case True, False
                If PresentRessr(attribut.NomRessource, attribut.CatgrRessource, idCible) = False Then
                    paramRetour = "La ressource: """ + nomRessource + """ n'est pas présent dans la config cible: """ + Config + """"
                ElseIf ValiderNomChampRessr(attribut) Then
                    paramRetour = "Le champ attribut: """ + attribut.NomAttrb + """ n'est pas conforme avec l'enrichissement de la config cible: """ + Config + """"
                End If
        End Select

        Return paramRetour
    End Function

    ''' <summary>
    ''' Méthode. Fait une série de test pour précisé l'erreur.
    ''' </summary>
    ''' <param name="lienUtilisateurRole">Le lien utilisateur/rôle, dont l'erreur est à préciser.</param>
    ''' <param name="modeAjout">Est-ce dans le cadre d'un ajout.</param>
    ''' <returns>L'erreur précisé.</returns>
    Public Function DeffnErreurLienUserRole(ByVal lienUtilisateurRole As TsCdConnxUserRole, ByVal modeAjout As Boolean) As String
        Dim paramRetour As String = lienUtilisateurRole.DescriptionErreur

        Dim nomUtilisateur As String = lienUtilisateurRole.CodeUtilisateur
        Dim nomRole As String = lienUtilisateurRole.NomRole
        Dim nomCombiner As String = nomUtilisateur + "/" + nomRole

        If PresentUser(lienUtilisateurRole.CodeUtilisateur) = False Then
            paramRetour = "L'utilisateur: """ + nomUtilisateur + """ n'est pas présent dans la config cible: """ + Config + """"
        ElseIf PresentRole(lienUtilisateurRole.NomRole) = False Then
            paramRetour = "Le rôle: """ + nomRole + """ n'est pas présent dans la config cible: """ + Config + """"
        Else
            Select Case modeAjout
                Case True
                    If PresentLienUserRole(lienUtilisateurRole.CodeUtilisateur, lienUtilisateurRole.NomRole) = True Then
                        paramRetour = "Le lien utilisateur/rôle: """ + nomCombiner + """ est déja présent dans la config cible: """ + Config + """"
                        Exit Select
                    End If
                Case False
                    If PresentLienUserRole(lienUtilisateurRole.CodeUtilisateur, lienUtilisateurRole.NomRole) Then
                        paramRetour = "Le lien utilisateur/rôle: """ + nomCombiner + """ n'est pas présent dans la config cible: """ + Config + """"
                    End If
            End Select
        End If

        Return paramRetour
    End Function

    ''' <summary>
    ''' Méthode. Fait une série de test pour précisé l'erreur.
    ''' </summary>
    ''' <param name="lienUtilisateurRessource">Le lien utilisateur/ressource, dont l'erreur est à préciser.</param>
    ''' <param name="modeAjout">Est-ce dans le cadre d'un ajout.</param>
    ''' <returns>L'erreur précisé.</returns>
    Public Function DeffnErreurLienUserRessr(ByVal lienUtilisateurRessource As TsCdConnxUserRessr, ByVal modeAjout As Boolean, ByVal idCible As String) As String
        Dim paramRetour As String = lienUtilisateurRessource.DescriptionErreur

        Dim nomUtilisateur As String = lienUtilisateurRessource.CodeUtilisateur
        Dim nomRessource As String = lienUtilisateurRessource.NomRessource + ", " + lienUtilisateurRessource.CatgrRessource + ", " + idCible
        Dim nomCombiner As String = nomUtilisateur + "/" + nomRessource

        If PresentUser(lienUtilisateurRessource.CodeUtilisateur) = False Then
            paramRetour = "L'utilisateur: """ + nomUtilisateur + """ n'est pas présent dans la config cible: """ + Config + """"
        ElseIf PresentRessr(lienUtilisateurRessource.NomRessource, lienUtilisateurRessource.CatgrRessource, idCible) = False Then
            paramRetour = "La ressource: """ + nomRessource + """ n'est pas présent dans la config cible: """ + Config + """"
        Else
            Select Case modeAjout
                Case True
                    If PresentLienUserRessr(lienUtilisateurRessource.NomRessource, lienUtilisateurRessource.CatgrRessource, idCible, lienUtilisateurRessource.CodeUtilisateur) = True Then
                        paramRetour = "Le lien utilisateur/ressource: """ + nomCombiner + """ est déja présent dans la config cible: """ + Config + """"
                        Exit Select
                    End If
                Case False
                    If PresentLienUserRessr(lienUtilisateurRessource.NomRessource, lienUtilisateurRessource.CatgrRessource, idCible, lienUtilisateurRessource.CodeUtilisateur) Then
                        paramRetour = "Le lien utilisateur/ressource: """ + nomCombiner + """ n'est pas présent dans la config cible:""" + Config + """"
                    End If
            End Select
        End If

        Return paramRetour
    End Function

    ''' <summary>
    ''' Méthode. Fait une série de test pour précisé l'erreur.
    ''' </summary>
    ''' <param name="lienRoleRole">Le lien rôle supérieur/sous-rôle, dont l'erreur est à préciser.</param>
    ''' <param name="modeAjout">Est-ce dans le cadre d'un ajout.</param>
    ''' <returns>L'erreur précisé.</returns>
    Public Function DeffnErreurLienRoleRole(ByVal lienRoleRole As TsCdConnxRoleRole, ByVal modeAjout As Boolean) As String
        Dim paramRetour As String = lienRoleRole.DescriptionErreur

        Dim nomRoleSup As String = lienRoleRole.NomRoleSup
        Dim nomSousRole As String = lienRoleRole.NomSousRole
        Dim nomCombiner As String = nomRoleSup + "/" + nomSousRole

        If PresentRole(lienRoleRole.NomRoleSup) = False Then
            paramRetour = "Le rôle supérieur: """ + nomRoleSup + """ n'est pas présent dans la config cible: """ + Config + """"
        ElseIf PresentRole(lienRoleRole.NomSousRole) = False Then
            paramRetour = "Le sous-rôle: """ + nomSousRole + """ n'est pas présent dans la config cible: """ + Config + """"
        Else
            Select Case modeAjout
                Case True
                    If PresentLienRoleRole(lienRoleRole.NomRoleSup, lienRoleRole.NomSousRole) = True Then
                        paramRetour = "Le lien rôle supérieur/sous-rôle: """ + nomCombiner + """ est déja présent dans la config cible: """ + Config + """"
                        Exit Select
                    End If
                Case False
                    If PresentLienRoleRole(lienRoleRole.NomRoleSup, lienRoleRole.NomSousRole) Then
                        paramRetour = "Le lien rôle supérieur/sous-rôle: """ + nomCombiner + """ n'est pas présent dans la config cible: """ + Config + """"
                    End If
            End Select
        End If

        Return paramRetour
    End Function

    ''' <summary>
    ''' Méthode. Fait une série de test pour précisé l'erreur.
    ''' </summary>
    ''' <param name="lienRoleRessource">Le lien rôle/ressource, dont l'erreur est à préciser.</param>
    ''' <param name="modeAjout">Est-ce dans le cadre d'un ajout.</param>
    ''' <returns>L'erreur précisé.</returns>
    Public Function DeffnErreurLienRoleRessr(ByVal lienRoleRessource As TsCdConnxRoleRessr, ByVal modeAjout As Boolean, ByVal idCible As String) As String
        Dim paramRetour As String = lienRoleRessource.DescriptionErreur

        Dim nomRessource As String = lienRoleRessource.NomRessource + ", " + lienRoleRessource.CatgrRessource + ", " + idCible
        Dim nomRole As String = lienRoleRessource.NomRole
        Dim nomCombiner As String = nomRole + "/" + nomRessource

        If PresentRole(lienRoleRessource.NomRole) = False Then
            paramRetour = "Le rôle: """ + nomRole + """ n'est pas présent dans la config cible: """ + Config + """"
        ElseIf PresentRessr(lienRoleRessource.NomRessource, lienRoleRessource.CatgrRessource, idCible) = False Then
            paramRetour = "La ressource: """ + nomRessource + """ n'est pas présent dans la config cible: """ + Config + """"
        Else

            Select Case modeAjout
                Case True
                    If PresentLienRoleRessr(lienRoleRessource.NomRessource, lienRoleRessource.CatgrRessource, idCible, lienRoleRessource.NomRole) = True Then
                        paramRetour = "Le lien rôle/ressource: """ + nomCombiner + """ est déja présent dans la config cible: """ + Config + """"
                        Exit Select
                    End If
                Case False
                    If PresentLienRoleRessr(lienRoleRessource.NomRessource, lienRoleRessource.CatgrRessource, idCible, lienRoleRessource.NomRole) Then
                        paramRetour = "Le lien rôle/ressource: """ + nomCombiner + """ n'est pas présent dans la config cible: """ + Config + """"
                    End If
            End Select
        End If

        Return paramRetour
    End Function

#End Region

#Region "Fonctions de services"


    ''' <summary>
    ''' Fonction de service. Teste si le nom du champ est valide dans sage.
    ''' </summary>
    ''' <param name="attribut">Élément de connextion d'un attribut.</param>
    ''' <returns>Vrai si le champ est valide avec l'enrichissement.</returns>
    Private Function ValiderNomChampUser(ByVal attribut As TsCdConnxUserAttrb) As Boolean
        Dim lstUserNorm As String() = {USER.NAME, USER.ORGANIZATION, USER.ORGANIZATION_TYPE, _
                                      USER.CN, USER.COURRIEL, USER.DATE_APPROBATION, USER.DATE_FIN, _
                                      USER.NOM, USER.NOM_UNITE, USER.PRENOM, USER.VILLE}

        Dim infoConfig As TsCdSageConfiguration = TsBaConfigSage.GetConfiguration(Config)
        Dim lstUserSage As String() = ObtenirListeChampUser(infoConfig.UserDatabaseName)

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
                Exit For
            End If
        Next

        If flagUserIntegrite = False Then
            Return False
        End If

        Return True
    End Function

    ''' <summary>
    ''' Fonction de service. Teste si le nom du champ est valide dans sage.
    ''' </summary>
    ''' <param name="attribut">Élément de connextion d'un attribut.</param>
    ''' <returns>Vrai si le champ est valide avec l'enrichissement.</returns>
    Private Function ValiderNomChampRessr(ByVal attribut As TsCdConnxRessrAttrb) As Boolean
        Dim paramRetour As Boolean = False
        Dim lstRessrNorm As String() = {RESSOURCE.RESNAME_1, RESSOURCE.RESNAME_2, RESSOURCE.RESNAME_3}

        Dim infoConfig As TsCdSageConfiguration = TsBaConfigSage.GetConfiguration(Config)
        Dim lstRessrSage As String() = ObtenirListeChampRessr(infoConfig.ResourceDatabaseName)

        Dim flagRessrIntegrite As Boolean = False
        For Each champSage As String In lstRessrSage
            For Each champNorm As String In lstRessrNorm
                If TsCdNomChampRef.MapChampRef().Item(champNorm).NomChamp = champSage Then
                    flagRessrIntegrite = True
                    Exit For
                End If
                If flagRessrIntegrite = False Then
                    Exit For
                End If
            Next
        Next

        If flagRessrIntegrite = False Then
            Return False
        End If

        Return True
    End Function

    ''' <summary>
    ''' Fonction de service permettant de retourner un liste de liens rôle/rôle en mémoire tampon.
    ''' </summary>
    ''' <returns>Liste de lien TsCdSageRoleRoleLink.</returns>
    Private Function ObtenirListeLiensRoleRole() As List(Of TsCdSageRoleRoleLink)
        If lstLiensRoleRole Is Nothing Then
            lstLiensRoleRole = New List(Of TsCdSageRoleRoleLink)(TsBaConfigSage.GetRoleSubRolesLinks(Config))
        End If
        Return lstLiensRoleRole
    End Function

    ''' <summary>
    ''' Fonction de service permettant de retourner un liste de liens rôle/ressource en mémoire tampon.
    ''' </summary>
    ''' <returns>Liste de lien TsCdSageRoleResLink.</returns>
    Private Function ObtenirListeLiensRoleRessr() As List(Of TsCdSageRoleResLink)
        If lstLiensRoleRessr Is Nothing Then
            lstLiensRoleRessr = New List(Of TsCdSageRoleResLink)(TsBaConfigSage.GetResourceRolesLinks(Config))
        End If
        Return lstLiensRoleRessr
    End Function

    ''' <summary>
    ''' Fonction de service permettant de retourner un liste de liens utilisateur/ressource en mémoire tampon.
    ''' </summary>
    ''' <returns>Liste de lien TsCdSageUserResLink.</returns>
    Private Function ObtenirListeLiensUserRessr() As List(Of TsCdSageUserResLink)
        If lstLiensUserRessr Is Nothing Then
            lstLiensUserRessr = New List(Of TsCdSageUserResLink)(TsBaConfigSage.GetUserResourcesLinks(Config))
        End If
        Return lstLiensUserRessr
    End Function

    ''' <summary>
    ''' Fonction de service permettant de retourner un liste de liens utilisateur/rôle en mémoire tampon.
    ''' </summary>
    ''' <returns>Liste de lien TsCdSageUserRoleLink.</returns>
    Private Function ObtenirListeLiensUserRole() As List(Of TsCdSageUserRoleLink)
        If lstLiensUserRole Is Nothing Then
            lstLiensUserRole = New List(Of TsCdSageUserRoleLink)(TsBaConfigSage.GetUserRolesLinks(Config))
        End If
        Return lstLiensUserRole
    End Function

    ''' <summary>
    ''' Fonction de service permettant de retourner un liste de rôle en mémoire tampon.
    ''' </summary>
    ''' <returns>Liste de TsCdSageRole.</returns>
    Private Function ObtenirListeRoles() As List(Of TsCdSageRole)
        If lstRoles Is Nothing Then
            lstRoles = New List(Of TsCdSageRole)(TsBaConfigSage.GetConfigurationRoles(Config))
        End If
        Return lstRoles
    End Function

    ''' <summary>
    ''' Fonction de service permettant de retourner un liste d'utilisateur de la configuration en mémoire tampon.
    ''' </summary>
    ''' <returns>Liste de TsCdSageUser.</returns>
    Private Function ObtenirListeUsers() As List(Of TsCdSageUser)
        If lstUsers Is Nothing Then
            lstUsers = New List(Of TsCdSageUser)(TsBaConfigSage.GetConfigurationUsers(Config))
        End If
        Return lstUsers
    End Function

    ''' <summary>
    ''' Fonction de service permettant de retourner un liste de ressource de la configuration en mémoire tampon.
    ''' </summary>
    ''' <returns>Liste de TsCdSageResource.</returns>
    Private Function ObtenirListeRessr() As List(Of TsCdSageResource)
        If lstRessr Is Nothing Then
            lstRessr = New List(Of TsCdSageResource)(TsBaConfigSage.GetConfigurationResources(Config))
        End If
        Return lstRessr
    End Function

    ''' <summary>
    ''' Fonction de service permettant de retourner un liste de ressource de la configuration en mémoire tampon.
    ''' </summary>
    ''' <param name="RDB">Nom de la RDB.</param>
    ''' <returns>Liste de TsCdSageResource.</returns>
    Private Function ObtenirListeChampRessr(ByVal RDB As String) As String()
        Return TsBaConfigSage.GetResourceFields(RDB)
    End Function

    ''' <summary>
    ''' Fonction de service permettant de retourner un liste de ressource de la configuration en mémoire tampon.
    ''' </summary>
    ''' <param name="UDB">Nom de la UDB.</param>
    ''' <returns>Liste de TsCdSageResource.</returns>
    Private Function ObtenirListeChampUser(ByVal UDB As String) As String()
        Return TsBaConfigSage.GetUserFields(UDB)
    End Function

    ''' <summary>
    ''' Fonction de services. 
    ''' Cette fonction fait appel aux définitions des champs normalisés définit dans <see cref="TsCdNomChampRef.MapChampRef"/>.
    ''' Cherche dans un dictionnaire indexer sur le nom des champs normalisés, le numéro de champ de Sage qui leur est associé.
    ''' </summary>
    ''' <param name="nomChampNorm">Le nom du champ normalisé.</param>
    ''' <returns>Le numéro du champ de Sage.</returns>
    Private Function TrouverNumeroChamp(ByVal nomChampNorm As String) As Integer
        Dim paramRetour As Integer = -2
        If TsCdNomChampRef.MapChampRef.ContainsKey(nomChampNorm) Then
            paramRetour = TsCdNomChampRef.MapChampRef(nomChampNorm).NumeroChamp
        Else
            Throw New ApplicationException("Erreur inattendu. Erreur de conception. Le champ normaliser n'existe pas.")
        End If

        Return paramRetour
    End Function

    ''' <summary>
    ''' Fonction de services. 
    ''' Cette fonction fait appel aux définitions des champs normalisés définit dans <see cref="TsCdNomChampRef.MapChampRef"/>.
    ''' Cherche dans un dictionnaire indexer sur le nom des champs normalisés, le nom de champ de Sage qui leur est associé.
    ''' </summary>
    ''' <param name="nomChampNorm">Le nom du champ normalisé</param>
    ''' <returns>Le nom du champ de Sage.</returns>
    Private Function TrouverNomChamp(ByVal nomChampNorm As String) As String
        Dim paramRetour As String = ""
        If TsCdNomChampRef.MapChampRef.ContainsKey(nomChampNorm) Then
            paramRetour = TsCdNomChampRef.MapChampRef(nomChampNorm).NomChamp
        Else
            Throw New ApplicationException("Erreur inattendu. Erreur de conception. Le champ normaliser n'existe pas.")
        End If

        Return paramRetour
    End Function

    ''' <summary>
    ''' Revois les informations de de la configuration en elle est mit en mémoir tampon.
    ''' </summary>
    ''' <returns>Un objet(TsCdSageDBInfrm) renfermant les informations de la configuration.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirConfigInfo() As TsCdSageConfiguration
        If sageSBInfo Is Nothing Then
            sageSBInfo = TsBaConfigSage.GetConfiguration(Config)
        End If
        Return sageSBInfo
    End Function

#End Region

End Class
