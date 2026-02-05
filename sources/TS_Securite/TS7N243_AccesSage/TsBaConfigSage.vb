Imports System.ServiceModel
Imports Rrq.InfrastructureCommune.Parametres
Imports System.ServiceModel.Channels
Imports Rrq.Securite.GestionAcces.TsCdConstanteNomChampNorm

Public Class TsBaConfigSage

    Shared _sageBrowsingService As SageBrowsingService.SageBrowsingServicePortTypeClient = Nothing
    Shared _sageModificationService As SageModificationsService.SageModificationsServicePortTypeClient = Nothing
    Shared _sageDNAService As SageDNAService.SageDNAServicePortTypeClient = Nothing

#Region "Constantes"
    Public Const DATE_MIN_SAGE As Date = #1/1/1901#
    Public Const DATE_MAX_SAGE As Date = #1/1/2075#

    ' Correspond à la propriété "format.date.display" situé dans le fichier "eurekify.properties" qui gère le format des
    '       dates retournés par les services webs de SAGE.
    ' Correspond aussi a la constante TsCdConnxAttrb.FORMAT_DATE_TOSTRING du module TS7N311_ConnexionCibles
    Public Const FORMAT_DATE_TOSTRING As String = "yyyy-MM-dd"
#End Region

#Region "Propriétés"
    Private Shared ReadOnly Property UrlSageBrowsingService() As String
        Get
            Return XuCuConfiguration.ValeurSysteme("TS7", "TS7\TS7N243\UrlSageBrowsingService")
        End Get
    End Property
    Private Shared ReadOnly Property UrlSageModificationsService() As String
        Get
            Return XuCuConfiguration.ValeurSysteme("TS7", "TS7\TS7N243\UrlSageModificationsService")
        End Get
    End Property
    Private Shared ReadOnly Property UrlSageDNAService() As String
        Get
            Return XuCuConfiguration.ValeurSysteme("TS7", "TS7\TS7N243\UrlSageDNAService")
        End Get
    End Property
#End Region

#Region "Méthodes privées"
    Private Shared Sub AddProperty(ByRef ListeProprietes As SageModificationsService.Property(), ByVal NomPropriete As String, ByVal Valeur As String)
        If Valeur <> String.Empty Then
            If ListeProprietes Is Nothing Then
                ReDim Preserve ListeProprietes(0)
            Else
                ReDim Preserve ListeProprietes(ListeProprietes.Length)
            End If

            ListeProprietes(ListeProprietes.Length - 1) = DefineSageProperty(NomPropriete, Valeur)
        End If
    End Sub

    Private Shared Function ConvertConfigurationVOToSageConfiguration(ByVal ConfigurationVO As SageBrowsingService.ConfigurationVO) As TsCdSageConfiguration
        Dim SageConfiguration As TsCdSageConfiguration

        SageConfiguration = New TsCdSageConfiguration

        SageConfiguration.ConfigurationID = ConfigurationVO.id.ToString
        SageConfiguration.ConfigurationName = ConfigurationVO.name
        SageConfiguration.IsCompleted = ConfigurationVO.completed
        SageConfiguration.IsLogged = ConfigurationVO.logged
        SageConfiguration.IsReadOnly = ConfigurationVO.readOnly
        SageConfiguration.ModifyDate = ConfigurationVO.modifyDate
        SageConfiguration.Operation1 = ConfigurationVO.operation1
        SageConfiguration.Owner1 = ConfigurationVO.owner1
        SageConfiguration.ParentConfigName = ConfigurationVO.parentConfigurationName
        SageConfiguration.ResourceDatabaseID = ConfigurationVO.rdbId.ToString
        SageConfiguration.ResourceDatabaseName = ConfigurationVO.rdbName
        SageConfiguration.UserDatabaseID = ConfigurationVO.udbId.ToString
        SageConfiguration.UserDatabaseName = ConfigurationVO.udbName

        Return SageConfiguration
    End Function

    Private Shared Function ConvertUserVOToSageUser(ByVal UserVO As SageBrowsingService.UserVO) As TsCdSageUser
        Dim SageUser As TsCdSageUser

        SageUser = New TsCdSageUser
        SageUser.CN = GetStringProperty(UserVO.fields, TsCdNomChampRef.MapChampRef.Item(USER.CN).NomChamp)
        SageUser.Courriel = GetStringProperty(UserVO.fields, TsCdNomChampRef.MapChampRef.Item(USER.COURRIEL).NomChamp)
        SageUser.DateApprobation = GetDateProperty(UserVO.fields, TsCdNomChampRef.MapChampRef.Item(USER.DATE_APPROBATION).NomChamp)
        SageUser.DateFin = GetDateProperty(UserVO.fields, TsCdNomChampRef.MapChampRef.Item(USER.DATE_FIN).NomChamp)
        SageUser.Nom = GetStringProperty(UserVO.fields, TsCdNomChampRef.MapChampRef.Item(USER.NOM).NomChamp)
        SageUser.NomUnite = GetStringProperty(UserVO.fields, TsCdNomChampRef.MapChampRef.Item(USER.NOM_UNITE).NomChamp)
        SageUser.Organization = GetStringProperty(UserVO.fields, TsCdNomChampRef.MapChampRef.Item(USER.ORGANIZATION).NomChamp)
        SageUser.OrganizationType = GetStringProperty(UserVO.fields, TsCdNomChampRef.MapChampRef.Item(USER.ORGANIZATION_TYPE).NomChamp)
        SageUser.Champ9 = GetStringProperty(UserVO.fields, TsCdNomChampRef.MapChampRef.Item(USER.CHAMP_9).NomChamp)
        SageUser.PersonID = UserVO.name.name
        SageUser.Prenom = GetStringProperty(UserVO.fields, TsCdNomChampRef.MapChampRef.Item(USER.PRENOM).NomChamp)
        SageUser.UserName = GetStringProperty(UserVO.fields, TsCdNomChampRef.MapChampRef.Item(USER.NAME).NomChamp)
        SageUser.Ville = GetStringProperty(UserVO.fields, TsCdNomChampRef.MapChampRef.Item(USER.VILLE).NomChamp)

        Return SageUser
    End Function

    Private Shared Function ConvertSageUserToUserVO(ByVal SageUser As TsCdSageUser) As SageModificationsService.UserVO
        Dim UserVO As SageModificationsService.UserVO

        UserVO = New SageModificationsService.UserVO
        SetStringProperty(UserVO.fields, TsCdNomChampRef.MapChampRef.Item(USER.CN).NomChamp, SageUser.CN)
        SetStringProperty(UserVO.fields, TsCdNomChampRef.MapChampRef.Item(USER.COURRIEL).NomChamp, SageUser.Courriel)
        SetStringProperty(UserVO.fields, TsCdNomChampRef.MapChampRef.Item(USER.DATE_APPROBATION).NomChamp, SageUser.DateApprobation.ToString(FORMAT_DATE_TOSTRING))
        SetStringProperty(UserVO.fields, TsCdNomChampRef.MapChampRef.Item(USER.DATE_FIN).NomChamp, SageUser.DateFin.ToString(FORMAT_DATE_TOSTRING))
        SetStringProperty(UserVO.fields, TsCdNomChampRef.MapChampRef.Item(USER.NOM).NomChamp, SageUser.Nom)
        SetStringProperty(UserVO.fields, TsCdNomChampRef.MapChampRef.Item(USER.NOM_UNITE).NomChamp, SageUser.NomUnite)
        SetStringProperty(UserVO.fields, TsCdNomChampRef.MapChampRef.Item(USER.ORGANIZATION).NomChamp, SageUser.Organization)
        SetStringProperty(UserVO.fields, TsCdNomChampRef.MapChampRef.Item(USER.CN).NomChamp, SageUser.OrganizationType)
        UserVO.name.name = SageUser.PersonID
        SetStringProperty(UserVO.fields, TsCdNomChampRef.MapChampRef.Item(USER.PRENOM).NomChamp, SageUser.Prenom)
        SetStringProperty(UserVO.fields, TsCdNomChampRef.MapChampRef.Item(USER.NOM_UNITE).NomChamp, SageUser.NomUnite)
        SetStringProperty(UserVO.fields, TsCdNomChampRef.MapChampRef.Item(USER.ORGANIZATION).NomChamp, SageUser.Organization)

        Return UserVO
    End Function

    Private Shared Function ConvertRoleVOToSageRole(ByVal RoleVO As SageBrowsingService.RoleVO) As TsCdSageRole
        Dim SageRole As New TsCdSageRole

        With SageRole
            .Name = RoleVO.name.name
            .Type = GetStringProperty(RoleVO.fields, TsCdNomChampRef.MapChampRef.Item(ROLE.TYPE).NomChamp)
            .Owner = GetStringProperty(RoleVO.fields, TsCdNomChampRef.MapChampRef.Item(ROLE.OWNER).NomChamp)
            .Rule = GetStringProperty(RoleVO.fields, TsCdNomChampRef.MapChampRef.Item(ROLE.RULE).NomChamp)
            .Description = GetStringProperty(RoleVO.fields, TsCdNomChampRef.MapChampRef.Item(ROLE.DESCRIPTION).NomChamp)
            .Organization = GetStringProperty(RoleVO.fields, TsCdNomChampRef.MapChampRef.Item(ROLE.ORGANIZATION).NomChamp)
            .Organization2 = GetStringProperty(RoleVO.fields, TsCdNomChampRef.MapChampRef.Item(ROLE.ORGANIZATION2).NomChamp)
            .Organization3 = GetStringProperty(RoleVO.fields, TsCdNomChampRef.MapChampRef.Item(ROLE.ORGANIZATION3).NomChamp)

            '.Reviewer = GetStringProperty(RoleVO.fields, TsCdNomChampRef.MapChampRef.Item(ROLE.REVIEWER).NomChamp)
            '.CreateDate = GetDateProperty(RoleVO.fields, TsCdNomChampRef.MapChampRef.Item(ROLE.CREATE_DATE).NomChamp)
            '.ApproveDate = GetDateProperty(RoleVO.fields, TsCdNomChampRef.MapChampRef.Item(ROLE.APPROVED_DATE).NomChamp)
            '.ApprovalStatus = GetStringProperty(RoleVO.fields, TsCdNomChampRef.MapChampRef.Item(ROLE.APPROVE_CODE).NomChamp)
            '.ExpirationDate = GetDateProperty(RoleVO.fields, TsCdNomChampRef.MapChampRef.Item(ROLE.EXPIRATION_DATE).NomChamp)
        End With

        Return SageRole
    End Function

    Private Shared Function ConvertResourceVOToSageResource(ByVal ResourceVO As SageBrowsingService.ResourceVO) As TsCdSageResource
        Dim SageResource As TsCdSageResource

        SageResource = New TsCdSageResource

        SageResource.ResName1 = ResourceVO.name.name1
        SageResource.ResName2 = ResourceVO.name.name2
        SageResource.ResName3 = ResourceVO.name.name3

        SageResource.CN = GetStringProperty(ResourceVO.fields, TsCdNomChampRef.MapChampRef.Item(RESSOURCE.CN).NomChamp)
        SageResource.DerniereModification = GetStringProperty(ResourceVO.fields, TsCdNomChampRef.MapChampRef.Item(RESSOURCE.DERN_MODIF).NomChamp)
        SageResource.NomFonctionnelOuDescription = GetStringProperty(ResourceVO.fields, TsCdNomChampRef.MapChampRef.Item(RESSOURCE.DESCRIPTION).NomChamp)
        SageResource.Details = GetStringProperty(ResourceVO.fields, TsCdNomChampRef.MapChampRef.Item(RESSOURCE.DETAILS).NomChamp)
        SageResource.DateCreation = GetDateProperty(ResourceVO.fields, TsCdNomChampRef.MapChampRef.Item(RESSOURCE.DATE_CREATION).NomChamp)
        SageResource.Detenteur = GetStringProperty(ResourceVO.fields, TsCdNomChampRef.MapChampRef.Item(RESSOURCE.DETENTEUR).NomChamp)

        Return SageResource
    End Function

    Private Shared Function CreateBindingUserName(ByVal isHttps As Boolean) As System.ServiceModel.Channels.Binding
        Dim bec As New BindingElementCollection()

        ' Definis la securite de la communication
        Dim MySecurityBindingElement As TransportSecurityBindingElement
        MySecurityBindingElement = SecurityBindingElement.CreateUserNameOverTransportBindingElement
        MySecurityBindingElement.IncludeTimestamp = False

        ' Encode les message en UTF-8
        Dim MyTextMessageEncodingElement As New TextMessageEncodingBindingElement
        MyTextMessageEncodingElement.WriteEncoding = New Text.UTF8Encoding()
        MyTextMessageEncodingElement.MessageVersion = MessageVersion.Soap11

        Dim transportBindingElement As TransportBindingElement = Nothing
        If isHttps Then
            transportBindingElement = New HttpsTransportBindingElement
        Else
            transportBindingElement = New HttpTransportBindingElementQuiFaitSemblantDEtreSecuritaire
        End If
        transportBindingElement.MaxReceivedMessageSize = Convert.ToInt64(XuCuConfiguration.ValeurSysteme("TS7", "TS7N243\MaxReceivedMessageSize"))


        bec.Add(MySecurityBindingElement)
        bec.Add(MyTextMessageEncodingElement)
        bec.Add(transportBindingElement)

        Dim customBinding As New CustomBinding(bec)
        Dim timeout = Convert.ToInt32(XuCuConfiguration.ValeurSysteme("TS7", "TS7N243\Timeout"))
        customBinding.SendTimeout = New TimeSpan(0, 0, timeout)
        customBinding.ReceiveTimeout = New TimeSpan(0, 0, timeout)
        Return customBinding
    End Function

    Private Shared Function CreateResourceVO(ByVal ResName1 As String, ByVal ResName2 As String, ByVal ResName3 As String) As SageModificationsService.ResourceVO
        Dim ResourceVO As SageModificationsService.ResourceVO

        ResourceVO = New SageModificationsService.ResourceVO
        ResourceVO.name = TsCuSageModificationUtils.DefineResourceEntityName(ResName1, ResName2, ResName3)

        Return ResourceVO
    End Function

    Private Shared Sub ObtenirCodeAccesMotPasseCleSymbolique(ByVal clientCredentials As System.ServiceModel.Description.ClientCredentials)
        Dim cleSymbolique As String
        Dim masterOfTheKey As New Rrq.Securite.tsCuObtCdAccGen
        Dim CodeUtilisateur As String = Nothing
        Dim MotPasse As String = Nothing

        cleSymbolique = XuCuConfiguration.ValeurSysteme("TS7", "TS7N243\CleSymbolique")

        masterOfTheKey.ObtenirCodeAccesMotDePasse(cleSymbolique, _
                                                  "TS7N243_AccesSage - Obtenir " & _
                                                  "le mot de passe pour " & _
                                                  "accéder au service CACRM.", _
                                                  CodeUtilisateur, _
                                                  MotPasse)

        clientCredentials.UserName.UserName = CodeUtilisateur
        clientCredentials.UserName.Password = MotPasse
    End Sub

    Private Shared Function CreateSageBrowsingService() As SageBrowsingService.SageBrowsingServicePortTypeClient
        If _sageBrowsingService Is Nothing OrElse _sageBrowsingService.State <> CommunicationState.Opened Then
            Dim isHttps As Boolean = UrlSageBrowsingService.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase)
            Dim service = New SageBrowsingService.SageBrowsingServicePortTypeClient(CreateBindingUserName(isHttps), New EndpointAddress(UrlSageBrowsingService))
            ObtenirCodeAccesMotPasseCleSymbolique(service.ClientCredentials)

            _sageBrowsingService = service
        End If

        Return _sageBrowsingService
    End Function

    Private Shared Function CreerSageModificationService() As SageModificationsService.SageModificationsServicePortType
        If _sageModificationService Is Nothing OrElse _sageModificationService.State <> CommunicationState.Opened Then
            Dim isHttps As Boolean = UrlSageBrowsingService.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase)
            Dim service = New SageModificationsService.SageModificationsServicePortTypeClient(CreateBindingUserName(isHttps), New EndpointAddress(UrlSageModificationsService))
            ObtenirCodeAccesMotPasseCleSymbolique(service.ClientCredentials)

            _sageModificationService = service
        End If

        Return _sageModificationService
    End Function

    Private Shared Function CreerSageDNAService() As SageDNAService.SageDNAServicePortType
        If _sageDNAService Is Nothing OrElse _sageDNAService.State <> CommunicationState.Opened Then
            Dim isHttps As Boolean = UrlSageBrowsingService.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase)
            Dim service = New SageDNAService.SageDNAServicePortTypeClient(CreateBindingUserName(isHttps), New EndpointAddress(UrlSageDNAService))
            ObtenirCodeAccesMotPasseCleSymbolique(service.ClientCredentials)

            _sageDNAService = service
        End If

        Return _sageDNAService
    End Function

    Private Shared Function DefineSageProperty(ByVal Name As String, ByVal Value As String) As SageModificationsService.Property
        Dim SageProperty As SageModificationsService.Property

        SageProperty = New SageModificationsService.Property
        SageProperty.name = Name
        SageProperty.value = Value

        Return SageProperty
    End Function

    Private Shared Function GetStringProperty(ByVal Fields() As SageBrowsingService.Property, ByVal Name As String) As String
        Dim Propriete As List(Of SageBrowsingService.Property) = (From Prop In Fields Where Prop.name = Name).ToList
        Dim Valeur As String = Nothing

        If Propriete IsNot Nothing AndAlso Propriete.Count > 0 Then
            Valeur = Propriete(0).value
        End If

        Return Valeur
    End Function

    Private Shared Sub SetStringProperty(ByVal Fields() As SageModificationsService.Property, ByVal Name As String, ByVal NewValue As String)
        Dim Propriete As List(Of SageModificationsService.Property) = (From Prop In Fields Where Prop.name = Name).ToList

        If Propriete IsNot Nothing AndAlso Propriete.Count > 0 Then
            Propriete(0).value = NewValue
        End If
    End Sub

    Private Shared Function GetDateProperty(ByVal Fields() As SageBrowsingService.Property, ByVal Name As String) As Date
        Dim Propriete As List(Of SageBrowsingService.Property) = (From Prop In Fields Where Prop.name = Name).ToList
        Dim Valeur As Date = Nothing

        If Propriete IsNot Nothing AndAlso Propriete.Count > 0 Then
            If Propriete(0).value <> String.Empty Then
                If Not Date.TryParseExact(Propriete(0).value, FORMAT_DATE_TOSTRING, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, Valeur) Then
                    Valeur = Nothing
                End If
            End If
        End If

        Return Valeur
    End Function
#End Region

#Region "Méthodes publiques"
    Public Shared Sub AddLinkUserRole(ByVal Config As String, ByVal UserName As String, ByVal RoleName As String)
        Dim Reponse As SageModificationsService.addLinkUserRoleResponse
        Dim Request As SageModificationsService.addLinkUserRoleRequest
        Dim SageModificationsService As SageModificationsService.SageModificationsServicePortType
        Dim UserEntityName As SageModificationsService.UserEntityName
        Dim RoleEntityName As SageModificationsService.RoleEntityName

        SageModificationsService = CreerSageModificationService()

        UserEntityName = TsCuSageModificationUtils.DefineUserEntityName(UserName)

        RoleEntityName = TsCuSageModificationUtils.DefineRoleEntityName(RoleName)

        Request = New SageModificationsService.addLinkUserRoleRequest
        Request.in0 = Config
        Request.in1 = UserEntityName
        Request.in2 = RoleEntityName

        Reponse = SageModificationsService.addLinkUserRole(Request)

        If Reponse.out <> True Then
            Throw New ApplicationException("Code de retour différent de true dans l'appel de AddLinkUserRole")
        End If
    End Sub

    Public Shared Sub AddResourceRoleLink(ByVal Config As String, ByVal ResName1 As String, ByVal ResName2 As String, ByVal ResName3 As String, ByVal RoleName As String)
        Dim Request As SageModificationsService.addLinkRoleResourceRequest
        Dim Reponse As SageModificationsService.addLinkRoleResourceResponse
        Dim SageModificationsService As SageModificationsService.SageModificationsServicePortType

        SageModificationsService = CreerSageModificationService()

        Request = New SageModificationsService.addLinkRoleResourceRequest
        Request.in0 = Config
        Request.in1 = TsCuSageModificationUtils.DefineRoleEntityName(RoleName)
        Request.in2 = TsCuSageModificationUtils.DefineResourceEntityName(ResName1, ResName2, ResName3)

        Reponse = SageModificationsService.addLinkRoleResource(Request)

        If Reponse.out <> True Then
            Throw New ApplicationException("Code de retour différent de true dans l'appel de AddResourceRoleLink")
        End If
    End Sub

    Public Shared Sub AddResourceToConfiguration(ByVal Config As String, ByVal ResName1 As String, ByVal ResName2 As String, ByVal ResName3 As String)
        Dim Reponse As SageModificationsService.addResourceToConfigurationResponse
        Dim Request As SageModificationsService.addResourceToConfigurationRequest
        Dim SageModificationsService As SageModificationsService.SageModificationsServicePortType

        SageModificationsService = CreerSageModificationService()

        Request = New SageModificationsService.addResourceToConfigurationRequest
        Request.in0 = Config
        Request.in1 = TsCuSageModificationUtils.DefineResourceEntityName(ResName1, ResName2, ResName3)

        Reponse = SageModificationsService.addResourceToConfiguration(Request)

        If Reponse.out <> True Then
            Throw New ApplicationException("Code de retour différent de true dans l'appel de AddResourceToConfiguration")
        End If
    End Sub

    Public Shared Sub AddResourceToDatabase(ByVal Config As String, ByVal ResName1 As String, ByVal ResName2 As String, ByVal ResName3 As String)
        Dim Reponse As SageModificationsService.addResourceToDatabaseResponse
        Dim Request As SageModificationsService.addResourceToDatabaseRequest
        Dim SageModificationsService As SageModificationsService.SageModificationsServicePortType

        SageModificationsService = CreerSageModificationService()

        Request = New SageModificationsService.addResourceToDatabaseRequest
        Request.in0 = Config
        Request.in1 = CreateResourceVO(ResName1, ResName2, ResName3)

        Reponse = SageModificationsService.addResourceToDatabase(Request)

        If Reponse.out <> True Then
            Throw New ApplicationException("Code de retour différent de true dans l'appel de AddResourceToDatabase")
        End If
    End Sub

    Public Shared Sub RemoveResourceFromDatabase(ByVal Config As String, ByVal ResName1 As String, ByVal ResName2 As String, ByVal ResName3 As String)
        Dim Reponse As SageModificationsService.deleteResourceFromDatabaseResponse
        Dim Request As SageModificationsService.deleteResourceFromDatabaseRequest
        Dim SageModificationsService As SageModificationsService.SageModificationsServicePortType
        Dim ResourceEntityName As SageModificationsService.ResourceEntityName

        SageModificationsService = CreerSageModificationService()

        ResourceEntityName = TsCuSageModificationUtils.DefineResourceEntityName(ResName1, ResName2, ResName3)

        Request = New SageModificationsService.deleteResourceFromDatabaseRequest
        Request.in0 = Config
        Request.in1 = ResourceEntityName

        Reponse = SageModificationsService.deleteResourceFromDatabase(Request)

        If Reponse.out <> True Then
            Throw New ApplicationException("Code de retour diffÃ©rent de true dans l'appel de RemoveResourceFromDatabase")
        End If
    End Sub

    Public Shared Sub AddRoleRoleLink(ByVal Config As String, ByVal RoleSup As String, ByVal RoleSub As String)
        Dim Reponse As SageModificationsService.addLinkRoleRoleResponse
        Dim Request As SageModificationsService.addLinkRoleRoleRequest
        Dim SageModificationsService As SageModificationsService.SageModificationsServicePortType

        SageModificationsService = CreerSageModificationService()

        Request = New SageModificationsService.addLinkRoleRoleRequest
        Request.in0 = Config
        Request.in1 = TsCuSageModificationUtils.DefineRoleEntityName(RoleSup)
        Request.in2 = TsCuSageModificationUtils.DefineRoleEntityName(RoleSub)

        Reponse = SageModificationsService.addLinkRoleRole(Request)

        If Reponse.out <> True Then
            Throw New ApplicationException("Code de retour différent de true dans l'appel de AddRoleRoleLink")
        End If
    End Sub

    Public Shared Sub AddRoleToConfiguration(ByVal Config As String, ByVal Rolename As String)
        AddRoleToConfiguration(Config, Rolename, "", "", "", "", Nothing, "", "", Nothing, "", "", "", Nothing)
    End Sub

    Public Shared Sub AddRoleToConfiguration(ByVal Config As String, _
                                            ByVal RoleName As String, _
                                            ByVal Description As String, _
                                            ByVal Organization As String, _
                                            ByVal Owner As String, _
                                            ByVal Type As String, _
                                            ByVal CreateDate As Date, _
                                            ByVal Reviewer As String, _
                                            ByVal ApprovalStatus As String, _
                                            ByVal ApprovalDate As Date, _
                                            ByVal Rule As String, _
                                            ByVal Organization2 As String, _
                                            ByVal Organization3 As String, _
                                            ByVal ExpirationDate As Date)

        Dim SageModificationsService As SageModificationsService.SageModificationsServicePortType
        Dim Reponse As SageModificationsService.addRoleToConfigurationResponse
        Dim Request As SageModificationsService.addRoleToConfigurationRequest
        Dim RoleVO As SageModificationsService.RoleVO

        RoleVO = New SageModificationsService.RoleVO
        RoleVO.name = TsCuSageModificationUtils.DefineRoleEntityName(RoleName)

        AddProperty(RoleVO.fields, TsCdNomChampRef.MapChampRef.Item(ROLE.DESCRIPTION).NomChamp, Description)
        'AddProperty(RoleVO.fields, TsCdNomChampRef.MapChampRef.Item(ROLE.ORGANIZATION).NomChamp, Organization)
        'AddProperty(RoleVO.fields, TsCdNomChampRef.MapChampRef.Item(ROLE.OWNER).NomChamp, Owner)
        AddProperty(RoleVO.fields, TsCdNomChampRef.MapChampRef.Item(ROLE.TYPE).NomChamp, Type)
        'AddProperty(RoleVO.fields, TsCdNomChampRef.MapChampRef.Item(ROLE.CREATE_DATE).NomChamp, CreateDate)
        'AddProperty(RoleVO.fields, TsCdNomChampRef.MapChampRef.Item(ROLE.REVIEWER).NomChamp, Reviewer)
        'AddProperty(RoleVO.fields, TsCdNomChampRef.MapChampRef.Item(ROLE.APPROVED_DATE).NomChamp, ApprovalDate)
        'AddProperty(RoleVO.fields, TsCdNomChampRef.MapChampRef.Item(ROLE.APPROVE_CODE).NomChamp, ApprovalStatus)
        AddProperty(RoleVO.fields, TsCdNomChampRef.MapChampRef.Item(ROLE.RULE).NomChamp, Rule)
        'AddProperty(RoleVO.fields, TsCdNomChampRef.MapChampRef.Item(ROLE.EXPIRATION_DATE).NomChamp, ExpirationDate)
        'AddProperty(RoleVO.fields, TsCdNomChampRef.MapChampRef.Item(ROLE.ORGANIZATION2).NomChamp, Organization2)
        'AddProperty(RoleVO.fields, TsCdNomChampRef.MapChampRef.Item(ROLE.ORGANIZATION3).NomChamp, Organization3)

        Request = New SageModificationsService.addRoleToConfigurationRequest
        Request.in0 = Config
        Request.in1 = RoleVO

        SageModificationsService = CreerSageModificationService()
        Reponse = SageModificationsService.addRoleToConfiguration(Request)

        If Reponse.out <> True Then
            Throw New ApplicationException("Code de retour différent de true dans l'appel de AddRoleToConfig")
        End If
    End Sub

    Public Shared Sub AddUserResourceLink(ByVal Config As String, ByVal UserName As String, ByVal ResName1 As String, ByVal ResName2 As String, ByVal ResName3 As String)
        Dim Reponse As SageModificationsService.addLinkUserResourceResponse
        Dim Request As SageModificationsService.addLinkUserResourceRequest
        Dim SageModificationsService As SageModificationsService.SageModificationsServicePortType

        SageModificationsService = CreerSageModificationService()

        Request = New SageModificationsService.addLinkUserResourceRequest
        Request.in0 = Config
        Request.in1 = TsCuSageModificationUtils.DefineUserEntityName(UserName)
        Request.in2 = TsCuSageModificationUtils.DefineResourceEntityName(ResName1, ResName2, ResName3)

        Reponse = SageModificationsService.addLinkUserResource(Request)

        If Reponse.out <> True Then
            Throw New ApplicationException("Code de retour différent de true dans l'appel de AddLinkUserResource")
        End If
    End Sub

    Public Shared Sub AddUserToConfiguration(ByVal Config As String, ByVal UserName As String)
        Dim Reponse As SageModificationsService.addUserToConfigurationResponse
        Dim Request As SageModificationsService.addUserToConfigurationRequest
        Dim SageModificationsService As SageModificationsService.SageModificationsServicePortType
        Dim UserEntityName As SageModificationsService.UserEntityName

        SageModificationsService = CreerSageModificationService()

        UserEntityName = TsCuSageModificationUtils.DefineUserEntityName(UserName)

        Request = New SageModificationsService.addUserToConfigurationRequest
        Request.in0 = Config
        Request.in1 = UserEntityName

        Reponse = SageModificationsService.addUserToConfiguration(Request)

        If Reponse.out <> True Then
            Throw New ApplicationException("Code de retour différent de true dans l'appel de AddUserToConfiguration")
        End If
    End Sub

    Public Shared Sub AddUserToDatabase(ByVal DatabaseName As String, ByVal UserID As String, ByVal UserName As String, ByVal Organization As String, ByVal OrganizationType As String)
        Dim Reponse As SageModificationsService.addUserToDatabaseResponse
        Dim Request As SageModificationsService.addUserToDatabaseRequest
        Dim SageModificationsService As SageModificationsService.SageModificationsServicePortType
        Dim UserEntityName As SageModificationsService.UserEntityName
        Dim UserVO As SageModificationsService.UserVO

        SageModificationsService = CreerSageModificationService()

        UserEntityName = TsCuSageModificationUtils.DefineUserEntityName(UserID)

        UserVO = New SageModificationsService.UserVO
        UserVO.name = UserEntityName

        AddProperty(UserVO.fields, TsCdNomChampRef.MapChampRef.Item(USER.NAME).NomChamp, UserName)
        AddProperty(UserVO.fields, TsCdNomChampRef.MapChampRef.Item(USER.ORGANIZATION).NomChamp, Organization)
        AddProperty(UserVO.fields, TsCdNomChampRef.MapChampRef.Item(USER.ORGANIZATION_TYPE).NomChamp, OrganizationType)

        Request = New SageModificationsService.addUserToDatabaseRequest
        Request.in0 = DatabaseName
        Request.in1 = UserVO

        Reponse = SageModificationsService.addUserToDatabase(Request)

        If Reponse.out <> True Then
            Throw New ApplicationException("Code de retour différent de true dans l'appel de AddUserToDatabase")
        End If
    End Sub

    Public Shared Sub RemoveUserFromDatabase(ByVal Config As String, ByVal UserId As String)
        Dim Reponse As SageModificationsService.deleteUserFromDatabaseResponse
        Dim Request As SageModificationsService.deleteUserFromDatabaseRequest
        Dim SageModificationsService As SageModificationsService.SageModificationsServicePortType
        Dim UserEntityName As SageModificationsService.UserEntityName

        SageModificationsService = CreerSageModificationService()

        UserEntityName = TsCuSageModificationUtils.DefineUserEntityName(UserId)

        Request = New SageModificationsService.deleteUserFromDatabaseRequest
        Request.in0 = Config
        Request.in1 = UserEntityName

        Reponse = SageModificationsService.deleteUserFromDatabase(Request)

        If Reponse.out <> True Then
            Throw New ApplicationException("Code de retour diffÃ©rent de true dans l'appel de RemoveUserFromDatabase")
        End If
    End Sub

    Public Shared Function GetConfiguration(ByVal Config As String) As TsCdSageConfiguration
        Dim SageBrowsingService As SageBrowsingService.SageBrowsingServicePortTypeClient
        Dim ConfigurationVO As SageBrowsingService.ConfigurationVO
        Dim SageConfiguration As TsCdSageConfiguration = Nothing

        SageBrowsingService = CreateSageBrowsingService()

        ConfigurationVO = SageBrowsingService.getConfiguration(Config)

        If ConfigurationVO IsNot Nothing Then
            SageConfiguration = ConvertConfigurationVOToSageConfiguration(ConfigurationVO)
        End If

        Return SageConfiguration
    End Function

    Public Shared Function GetConfigurationResources(ByVal Config As String) As TsCdSageResourceCollection
        Dim SageBrowsingService As SageBrowsingService.SageBrowsingServicePortTypeClient
        Dim SageResourceCollection As New TsCdSageResourceCollection
        Dim ListeRessourcesVO As SageBrowsingService.ResourceVO()

        SageBrowsingService = CreateSageBrowsingService()

        ListeRessourcesVO = SageBrowsingService.findConfigurationResources(Config)

        If ListeRessourcesVO IsNot Nothing AndAlso ListeRessourcesVO.Length > 0 Then
            For Each ResourceVO As SageBrowsingService.ResourceVO In ListeRessourcesVO
                Dim SageResource As TsCdSageResource
                SageResource = ConvertResourceVOToSageResource(ResourceVO)
                SageResourceCollection.Add(SageResource)
            Next
        End If

        Return SageResourceCollection
    End Function

    ''' <summary>
    ''' Obtenir une ressource de Sage en indiquant le ressource name 1, 2 et 3 (ResName1, ResName2, ResName3).
    ''' </summary>
    ''' <param name="Config">La configuration a utiliser</param>
    ''' <param name="resName1"></param>
    ''' <param name="resName2"></param>
    ''' <param name="resName3"></param>
    ''' <returns>un object TsCdSageResource ou Nothing si la ressource n'existe pas</returns>
    Public Shared Function FindConfigurationResource(ByVal Config As String, ByVal resName1 As String, ByVal resName2 As String, ByVal resName3 As String) As TsCdSageResource
        Dim SageBrowsingService As SageBrowsingService.SageBrowsingServicePortTypeClient
        Dim ResourceVO As SageBrowsingService.ResourceVO

        SageBrowsingService = CreateSageBrowsingService()

        ResourceVO = SageBrowsingService.findConfigurationResource1(Config, resName1, resName2, resName3)
        If ResourceVO Is Nothing Then Return Nothing

        Return ConvertResourceVOToSageResource(ResourceVO)
    End Function

    ''' <summary>
    ''' Obtienir la liste de rôe
    ''' </summary>
    ''' <param name="Config">La configuration a utiliser</param>
    ''' <returns>Une collection de TsCdSageRole</returns>
    Public Shared Function GetConfigurationRoles(ByVal Config As String) As TsCdSageRoleCollection
        Dim SageBrowsingService As SageBrowsingService.SageBrowsingServicePortTypeClient
        Dim ListeRolesVO As SageBrowsingService.RoleVO()
        Dim SageRoleCollection As New TsCdSageRoleCollection

        SageBrowsingService = CreateSageBrowsingService()

        ListeRolesVO = SageBrowsingService.findConfigurationRoles(Config)

        If ListeRolesVO IsNot Nothing AndAlso ListeRolesVO.Length > 0 Then
            For Each RoleVO As SageBrowsingService.RoleVO In ListeRolesVO
                Dim SageRole As TsCdSageRole
                SageRole = ConvertRoleVOToSageRole(RoleVO)
                SageRoleCollection.Add(SageRole)
            Next
        End If

        Return SageRoleCollection
    End Function

    ''' <summary>
    ''' Obtenir un rôle de Sage en indiquant le nom du rôle.
    ''' </summary>
    ''' <param name="Config">La configuration a utiliser</param>
    ''' <param name="roleName">L'identifiant du rôle</param>
    ''' <returns>un object TsCdSageRole ou Nothing si le rôle n'existe pas</returns>
    Public Shared Function FindConfigurationRole(ByVal Config As String, ByVal roleName As String) As TsCdSageRole
        Dim SageBrowsingService As SageBrowsingService.SageBrowsingServicePortTypeClient
        Dim RoleVO As SageBrowsingService.RoleVO

        SageBrowsingService = CreateSageBrowsingService()

        RoleVO = SageBrowsingService.findConfigurationRole1(Config, roleName)
        If RoleVO Is Nothing Then Return Nothing

        Return ConvertRoleVOToSageRole(RoleVO)
    End Function

    Public Shared Function GetConfigurations() As TsCdSageConfigurationCollection
        Dim SageBrowsingService As SageBrowsingService.SageBrowsingServicePortTypeClient
        Dim ConfigurationsVOList() As SageBrowsingService.ConfigurationVO
        Dim SageConfigurationCollection As TsCdSageConfigurationCollection

        SageBrowsingService = CreateSageBrowsingService()

        ConfigurationsVOList = SageBrowsingService.getConfigurations()

        SageConfigurationCollection = New TsCdSageConfigurationCollection

        For Each ConfigurationsVO In ConfigurationsVOList
            SageConfigurationCollection.Add(ConvertConfigurationVOToSageConfiguration(ConfigurationsVO))
        Next

        Return SageConfigurationCollection
    End Function

    Public Shared Function GetConfigurationUserRoleLinks(ByVal Config As String, ByVal UserName As String) As TsCdSageUserRoleLinkCollection
        Dim SageBrowsingService As SageBrowsingService.SageBrowsingServicePortTypeClient
        Dim UserEntityName(0) As SageBrowsingService.UserEntityName
        Dim ListeLiens As SageBrowsingService.RoleEntityName()
        Dim SageUserRoleLinksCollection As New TsCdSageUserRoleLinkCollection

        SageBrowsingService = CreateSageBrowsingService()

        UserEntityName(0) = TsCuSageBrowsingUtils.DefineUserEntityName(UserName)

        ' Dans sage, quand tu demandes les liens directs, cela exclu les liens "dual"
        ' Pour obtenir tous les liens directs (c.a.d les direct et les dual dans sage), il faut appeler les methodes finissant par "Links"
        ListeLiens = SageBrowsingService.findUserRolesLinks(Config, UserEntityName)

        For Each Lien As SageBrowsingService.RoleEntityName In ListeLiens
            Dim SageUserRoleLink As TsCdSageUserRoleLink
            SageUserRoleLink = New TsCdSageUserRoleLink

            SageUserRoleLink.RoleName = Lien.name
            SageUserRoleLink.PersonID = UserName

            SageUserRoleLinksCollection.Add(SageUserRoleLink)
        Next

        Return SageUserRoleLinksCollection
    End Function

    ''' <summary>
    ''' Retourne la liste de tous les utilisateurs
    ''' </summary>
    ''' <param name="Config">La configuration a utiliser</param>
    ''' <returns>Une collection de TsCdSageUser</returns>
    Public Shared Function GetConfigurationUsers(ByVal Config As String) As TsCdSageUserCollection
        Dim SageBrowsingService As SageBrowsingService.SageBrowsingServicePortTypeClient
        Dim SageUserCollection As New TsCdSageUserCollection
        Dim ListeUsersVO As SageBrowsingService.UserVO()

        SageBrowsingService = CreateSageBrowsingService()

        ListeUsersVO = SageBrowsingService.findConfigurationUsers(Config)

        If ListeUsersVO IsNot Nothing AndAlso ListeUsersVO.Length > 0 Then
            For Each UserVO As SageBrowsingService.UserVO In ListeUsersVO
                SageUserCollection.Add(ConvertUserVOToSageUser(UserVO))
            Next
        End If

        Return SageUserCollection
    End Function

    ''' <summary>
    ''' Retourne un utilisateur par son identifiant
    ''' </summary>
    ''' <param name="Config">La configuration a utiliser</param>
    ''' <param name="personId">L'identifiant de l'utilisateur</param>
    ''' <returns>un object TsCdSageUser ou Nothing si l'utilisateur n'existe pas</returns>
    Public Shared Function FindConfigurationUser(ByVal Config As String, ByVal personId As String) As TsCdSageUser
        Dim SageBrowsingService As SageBrowsingService.SageBrowsingServicePortTypeClient
        Dim UserVO As SageBrowsingService.UserVO

        SageBrowsingService = CreateSageBrowsingService()

        UserVO = SageBrowsingService.findConfigurationUser1(Config, personId)
        If UserVO Is Nothing Then Return Nothing

        Return ConvertUserVOToSageUser(UserVO)
    End Function

    Public Shared Function GetResourceFields(ByVal Config As String) As String()
        Dim FieldList As String()
        Dim SageBrowsingService As SageBrowsingService.SageBrowsingServicePortTypeClient

        SageBrowsingService = CreateSageBrowsingService()

        FieldList = SageBrowsingService.getResourceFields(Config)

        Return FieldList
    End Function

    Public Shared Function GetResourceRolesLinks(ByVal Config As String, ByVal resName1 As String, ByVal resName2 As String, ByVal resName3 As String) As TsCdSageRoleResLinkCollection
        Dim SageBrowsingService As SageBrowsingService.SageBrowsingServicePortTypeClient
        Dim ResourceEntityName(0) As SageBrowsingService.ResourceEntityName
        Dim ListeRoles As SageBrowsingService.RoleEntityName()
        Dim SageRoleResLinkCollection As New TsCdSageRoleResLinkCollection

        SageBrowsingService = CreateSageBrowsingService()

        ResourceEntityName(0) = TsCuSageBrowsingUtils.DefineResourceEntityName(resName1, resName2, resName3)

        ' Dans sage, quand tu demandes les liens directs, cela exclu les liens "dual"
        ' Pour obtenir tous les liens directs (c.a.d les direct et les dual dans sage), il faut appeler les methodes finissant par "Links"
        ListeRoles = SageBrowsingService.findResourceRolesLinks(Config, ResourceEntityName)

        For Each Role As SageBrowsingService.RoleEntityName In ListeRoles
            Dim SageRoleResLink As TsCdSageRoleResLink
            SageRoleResLink = New TsCdSageRoleResLink

            SageRoleResLink.ResName1 = resName1
            SageRoleResLink.ResName2 = resName2
            SageRoleResLink.ResName3 = resName3

            SageRoleResLink.RoleName = Role.name

            SageRoleResLinkCollection.Add(SageRoleResLink)
        Next

        Return SageRoleResLinkCollection
    End Function

    Public Shared Function GetResourceRolesLinks(ByVal Config As String) As TsCdSageRoleResLinkCollection
        Return GetRoleResourcesLinks(Config)
    End Function

    Public Shared Function GetRoleResourcesLinks(ByVal Config As String) As TsCdSageRoleResLinkCollection
        Dim RolesList As TsCdSageRoleCollection
        Dim RolesResourcesList As New TsCdSageRoleResLinkCollection

        RolesList = GetConfigurationRoles(Config)

        For Each Role As TsCdSageRole In RolesList
            Dim RoleRessourcesList As TsCdSageRoleResLinkCollection

            RoleRessourcesList = GetRoleResourcesLinks(Config, Role.Name)

            RolesResourcesList.AddRange(RoleRessourcesList)
        Next

        Return RolesResourcesList
    End Function

    Public Shared Function GetRoleResourcesLinks(ByVal Config As String, ByVal RoleName As String) As TsCdSageRoleResLinkCollection
        Dim SageBrowsingService As SageBrowsingService.SageBrowsingServicePortTypeClient
        Dim RoleEntityName(0) As SageBrowsingService.RoleEntityName
        Dim ListeLiens As SageBrowsingService.ResourceEntityName()
        Dim SageRoleResLinkCollection As New TsCdSageRoleResLinkCollection

        SageBrowsingService = CreateSageBrowsingService()

        RoleEntityName(0) = TsCuSageBrowsingUtils.DefineRoleEntityName(RoleName)

        ' Dans sage, quand tu demandes les liens directs, cela exclu les liens "dual"
        ' Pour obtenir tous les liens directs (c.a.d les direct et les dual dans sage), il faut appeler les methodes finissant par "Links"
        ListeLiens = SageBrowsingService.findRoleResourcesLinks(Config, RoleEntityName)

        For Each Lien As SageBrowsingService.ResourceEntityName In ListeLiens
            Dim SageRoleResLink As TsCdSageRoleResLink
            SageRoleResLink = New TsCdSageRoleResLink

            SageRoleResLink.ResName1 = Lien.name1
            SageRoleResLink.ResName2 = Lien.name2
            SageRoleResLink.ResName3 = Lien.name3

            SageRoleResLink.RoleName = RoleName

            SageRoleResLinkCollection.Add(SageRoleResLink)
        Next

        Return SageRoleResLinkCollection
    End Function

    Public Shared Function GetRoleSubRolesLinks(ByVal Config As String) As TsCdSageRoleRoleLinkCollection
        Dim RolesList As TsCdSageRoleCollection
        Dim RolesSubRolesList As New TsCdSageRoleRoleLinkCollection

        RolesList = GetConfigurationRoles(Config)

        For Each Role As TsCdSageRole In RolesList
            Dim RoleSubRolesList As TsCdSageRoleRoleLinkCollection

            RoleSubRolesList = GetRoleSubRolesLinks(Config, Role.Name)

            RolesSubRolesList.AddRange(RoleSubRolesList)
        Next

        Return RolesSubRolesList
    End Function

    Public Shared Function GetRoleSubRolesLinks(ByVal Config As String, ByVal RoleName As String) As TsCdSageRoleRoleLinkCollection
        Dim SageBrowsingService As SageBrowsingService.SageBrowsingServicePortTypeClient
        Dim RoleEntityName(0) As SageBrowsingService.RoleEntityName
        Dim ListeLiens As SageBrowsingService.RoleEntityName()
        Dim SageRoleRoleLinkCollection As New TsCdSageRoleRoleLinkCollection

        SageBrowsingService = CreateSageBrowsingService()

        RoleEntityName(0) = TsCuSageBrowsingUtils.DefineRoleEntityName(RoleName)

        ' Dans sage, quand tu demandes les liens directs, cela exclu les liens "dual"
        ' Pour obtenir tous les liens directs (c.a.d les direct et les dual dans sage), il faut appeler les methodes finissant par "Links"
        ListeLiens = SageBrowsingService.findRoleChildRolesLinks(Config, RoleEntityName)

        For Each Lien As SageBrowsingService.RoleEntityName In ListeLiens
            Dim SageRoleRoleLink As TsCdSageRoleRoleLink
            SageRoleRoleLink = New TsCdSageRoleRoleLink

            SageRoleRoleLink.ChildRole = Lien.name
            SageRoleRoleLink.ParentRole = RoleName

            SageRoleRoleLinkCollection.Add(SageRoleRoleLink)
        Next

        Return SageRoleRoleLinkCollection
    End Function

    Public Shared Function GetRoleSupRolesLinks(ByVal Config As String, ByVal RoleName As String) As TsCdSageRoleRoleLinkCollection
        Dim SageBrowsingService As SageBrowsingService.SageBrowsingServicePortTypeClient
        Dim RoleEntityName(0) As SageBrowsingService.RoleEntityName
        Dim ListeLiens As SageBrowsingService.RoleEntityName()
        Dim SageRoleRoleLinkCollection As New TsCdSageRoleRoleLinkCollection

        SageBrowsingService = CreateSageBrowsingService()

        RoleEntityName(0) = TsCuSageBrowsingUtils.DefineRoleEntityName(RoleName)

        ' Dans sage, quand tu demandes les liens directs, cela exclu les liens "dual"
        ' Pour obtenir tous les liens directs (c.a.d les direct et les dual dans sage), il faut appeler les methodes finissant par "Links"
        ListeLiens = SageBrowsingService.findRoleParentRolesLinks(Config, RoleEntityName)

        For Each Lien As SageBrowsingService.RoleEntityName In ListeLiens
            Dim SageRoleRoleLink As TsCdSageRoleRoleLink
            SageRoleRoleLink = New TsCdSageRoleRoleLink

            SageRoleRoleLink.ChildRole = RoleName
            SageRoleRoleLink.ParentRole = Lien.name

            SageRoleRoleLinkCollection.Add(SageRoleRoleLink)
        Next

        Return SageRoleRoleLinkCollection
    End Function

    Public Shared Function GetUserFields(ByVal Config As String) As String()
        Dim FieldList As String()
        Dim SageBrowsingService As SageBrowsingService.SageBrowsingServicePortTypeClient

        SageBrowsingService = CreateSageBrowsingService()

        FieldList = SageBrowsingService.getUserFields(Config)

        Return FieldList
    End Function

    Public Shared Function GetUserResourcesLinks(ByVal Config As String) As TsCdSageUserResLinkCollection
        Dim UserList As TsCdSageUserCollection
        Dim UsersResourcesList As New TsCdSageUserResLinkCollection

        UserList = GetConfigurationUsers(Config)

        For Each User As TsCdSageUser In UserList
            Dim UserResourcesList As TsCdSageUserResLinkCollection

            UserResourcesList = GetUserResourcesLinks(Config, User.PersonID)

            UsersResourcesList.AddRange(UserResourcesList)
        Next

        Return UsersResourcesList
    End Function

    Public Shared Function GetUserResourcesLinks(ByVal Config As String, ByVal UserName As String) As TsCdSageUserResLinkCollection
        Dim SageBrowsingService As SageBrowsingService.SageBrowsingServicePortTypeClient
        Dim UserEntityName(0) As SageBrowsingService.UserEntityName
        Dim ListeLiens As SageBrowsingService.ResourceEntityName()
        Dim SageUserResLinkCollection As New TsCdSageUserResLinkCollection

        SageBrowsingService = CreateSageBrowsingService()

        UserEntityName(0) = TsCuSageBrowsingUtils.DefineUserEntityName(UserName)

        ListeLiens = SageBrowsingService.findUserResourcesLinks(Config, UserEntityName)

        For Each Lien As SageBrowsingService.ResourceEntityName In ListeLiens
            Dim SageUserResLink As TsCdSageUserResLink
            SageUserResLink = New TsCdSageUserResLink

            SageUserResLink.ResName1 = Lien.name1
            SageUserResLink.ResName2 = Lien.name2
            SageUserResLink.ResName3 = Lien.name3

            SageUserResLink.PersonID = UserName

            SageUserResLinkCollection.Add(SageUserResLink)
        Next

        Return SageUserResLinkCollection
    End Function

    Public Shared Function GetUserRolesLinks(ByVal Config As String) As TsCdSageUserRoleLinkCollection
        Dim UserList As TsCdSageUserCollection
        Dim UsersRoleList As New TsCdSageUserRoleLinkCollection

        UserList = GetConfigurationUsers(Config)

        For Each User As TsCdSageUser In UserList
            Dim UserRoleList As TsCdSageUserRoleLinkCollection

            UserRoleList = GetUserRolesLinks(Config, User.PersonID)

            UsersRoleList.AddRange(UserRoleList)
        Next

        Return UsersRoleList
    End Function

    Public Shared Function GetUserRolesLinks(ByVal Config As String, ByVal UserName As String) As TsCdSageUserRoleLinkCollection
        Dim SageBrowsingService As SageBrowsingService.SageBrowsingServicePortTypeClient
        Dim UserEntityName(0) As SageBrowsingService.UserEntityName
        Dim ListeLiens As SageBrowsingService.RoleEntityName()
        Dim SageUserRoleLinksCollection As New TsCdSageUserRoleLinkCollection

        SageBrowsingService = CreateSageBrowsingService()

        UserEntityName(0) = TsCuSageBrowsingUtils.DefineUserEntityName(UserName)

        ' Dans sage, quand tu demandes les liens directs, cela exclu les liens "dual"
        ' Pour obtenir tous les liens directs (c.a.d les direct et les dual dans sage), il faut appeler les methodes finissant par "Links"
        ListeLiens = SageBrowsingService.findUserRolesLinks(Config, UserEntityName)

        For Each Lien As SageBrowsingService.RoleEntityName In ListeLiens
            Dim SageUserRoleLink As TsCdSageUserRoleLink
            SageUserRoleLink = New TsCdSageUserRoleLink

            SageUserRoleLink.RoleName = Lien.name
            SageUserRoleLink.PersonID = UserName

            SageUserRoleLinksCollection.Add(SageUserRoleLink)
        Next

        Return SageUserRoleLinksCollection
    End Function

    Public Shared Function GetResourceUsersLinks(ByVal Config As String, ByVal resName1 As String, ByVal resName2 As String, ByVal resName3 As String) As TsCdSageUserResLinkCollection
        Dim SageBrowsingService As SageBrowsingService.SageBrowsingServicePortTypeClient
        Dim ResourceEntityName(0) As SageBrowsingService.ResourceEntityName
        Dim ListeUsers As SageBrowsingService.UserEntityName()
        Dim SageUserResLinkCollection As New TsCdSageUserResLinkCollection

        SageBrowsingService = CreateSageBrowsingService()

        ResourceEntityName(0) = TsCuSageBrowsingUtils.DefineResourceEntityName(resName1, resName2, resName3)

        ' Dans sage, quand tu demandes les liens directs, cela exclu les liens "dual"
        ' Pour obtenir tous les liens directs (c.a.d les direct et les dual dans sage), il faut appeler les methodes finissant par "Links"
        ListeUsers = SageBrowsingService.findResourceUsersLinks(Config, ResourceEntityName)

        For Each User As SageBrowsingService.UserEntityName In ListeUsers
            Dim SageUserResLink As TsCdSageUserResLink
            SageUserResLink = New TsCdSageUserResLink

            SageUserResLink.ResName1 = resName1
            SageUserResLink.ResName2 = resName2
            SageUserResLink.ResName3 = resName3

            SageUserResLink.PersonID = User.name

            SageUserResLinkCollection.Add(SageUserResLink)
        Next

        Return SageUserResLinkCollection
    End Function

    Public Shared Function GetRoleUsersLinks(ByVal Config As String, ByVal RoleName As String) As TsCdSageUserRoleLinkCollection
        Dim SageBrowsingService As SageBrowsingService.SageBrowsingServicePortTypeClient
        Dim RoleEntityName(0) As SageBrowsingService.RoleEntityName
        Dim ListeUsers As SageBrowsingService.UserEntityName()
        Dim SageUserRoleLinksCollection As New TsCdSageUserRoleLinkCollection

        SageBrowsingService = CreateSageBrowsingService()

        RoleEntityName(0) = TsCuSageBrowsingUtils.DefineRoleEntityName(RoleName)

        ' Dans sage, quand tu demandes les liens directs, cela exclu les liens "dual"
        ' Pour obtenir tous les liens directs (c.a.d les direct et les dual dans sage), il faut appeler les methodes finissant par "Links"
        ListeUsers = SageBrowsingService.findRoleUsersLinks(Config, RoleEntityName)

        For Each User As SageBrowsingService.UserEntityName In ListeUsers
            Dim SageUserRoleLink As TsCdSageUserRoleLink
            SageUserRoleLink = New TsCdSageUserRoleLink

            SageUserRoleLink.RoleName = RoleName
            SageUserRoleLink.PersonID = User.name

            SageUserRoleLinksCollection.Add(SageUserRoleLink)
        Next

        Return SageUserRoleLinksCollection
    End Function

    Public Shared Sub RemoveConfigurationResource(ByVal Config As String, ByVal ResName1 As String, ByVal ResName2 As String, ByVal ResName3 As String)
        Dim Reponse As SageModificationsService.deleteResourceFromConfigurationResponse
        Dim Request As SageModificationsService.deleteResourceFromConfigurationRequest
        Dim SageModificationsService As SageModificationsService.SageModificationsServicePortType
        Dim ResourceEntityName As SageModificationsService.ResourceEntityName

        SageModificationsService = CreerSageModificationService()

        ResourceEntityName = TsCuSageModificationUtils.DefineResourceEntityName(ResName1, ResName2, ResName3)

        Request = New SageModificationsService.deleteResourceFromConfigurationRequest
        Request.in0 = Config
        Request.in1 = ResourceEntityName

        Reponse = SageModificationsService.deleteResourceFromConfiguration(Request)

        If Reponse.out <> True Then
            Throw New ApplicationException("Code de retour différent de true dans l'appel de RemoveConfigurationResource")
        End If
    End Sub

    Public Shared Sub RemoveConfigurationRole(ByVal Config As String, ByVal RoleName As String)
        Dim Reponse As SageModificationsService.deleteRoleFromConfigurationResponse
        Dim Request As SageModificationsService.deleteRoleFromConfigurationRequest
        Dim SageModificationsService As SageModificationsService.SageModificationsServicePortType
        Dim RoleEntityName As SageModificationsService.RoleEntityName

        SageModificationsService = CreerSageModificationService()

        RoleEntityName = TsCuSageModificationUtils.DefineRoleEntityName(RoleName)

        Request = New SageModificationsService.deleteRoleFromConfigurationRequest
        Request.in0 = Config
        Request.in1 = RoleEntityName

        Reponse = SageModificationsService.deleteRoleFromConfiguration(Request)

        If Reponse.out <> True Then
            Throw New ApplicationException("Code de retour différent de true dans l'appel de RemoveConfigurationRole")
        End If
    End Sub

    Public Shared Sub RemoveConfigurationUser(ByVal Config As String, ByVal UserName As String)
        Dim Reponse As SageModificationsService.deleteUserFromConfigurationResponse
        Dim Request As SageModificationsService.deleteUserFromConfigurationRequest
        Dim SageModificationsService As SageModificationsService.SageModificationsServicePortType
        Dim UserEntityName As SageModificationsService.UserEntityName

        SageModificationsService = CreerSageModificationService()

        UserEntityName = TsCuSageModificationUtils.DefineUserEntityName(UserName)

        Request = New SageModificationsService.deleteUserFromConfigurationRequest
        Request.in0 = Config
        Request.in1 = UserEntityName

        Reponse = SageModificationsService.deleteUserFromConfiguration(Request)

        If Reponse.out <> True Then
            Throw New ApplicationException("Code de retour différent de true dans l'appel de RemoveConfigurationUser")
        End If
    End Sub

    Public Shared Sub RemoveResourceRoleLink(ByVal Config As String, ByVal ResName1 As String, ByVal ResName2 As String, ByVal ResName3 As String, ByVal RoleName As String)
        Dim Reponse As SageModificationsService.deleteLinkRoleResourceResponse
        Dim Request As SageModificationsService.deleteLinkRoleResourceRequest
        Dim SageModificationsService As SageModificationsService.SageModificationsServicePortType

        SageModificationsService = CreerSageModificationService()

        Request = New SageModificationsService.deleteLinkRoleResourceRequest
        Request.in0 = Config
        Request.in1 = TsCuSageModificationUtils.DefineRoleEntityName(RoleName)
        Request.in2 = TsCuSageModificationUtils.DefineResourceEntityName(ResName1, ResName2, ResName3)

        Reponse = SageModificationsService.deleteLinkRoleResource(Request)

        If Reponse.out <> True Then
            Throw New ApplicationException("Code de retour différent de true dans l'appel de RemoveRoleRoleLink")
        End If
    End Sub

    Public Shared Sub RemoveRoleRoleLink(ByVal Config As String, ByVal RoleSup As String, ByVal RoleSub As String)
        Dim Reponse As SageModificationsService.deleteLinkRoleRoleResponse
        Dim Request As SageModificationsService.deleteLinkRoleRoleRequest
        Dim SageModificationsService As SageModificationsService.SageModificationsServicePortType

        SageModificationsService = CreerSageModificationService()

        Request = New SageModificationsService.deleteLinkRoleRoleRequest
        Request.in0 = Config
        Request.in1 = TsCuSageModificationUtils.DefineRoleEntityName(RoleSup)
        Request.in2 = TsCuSageModificationUtils.DefineRoleEntityName(RoleSub)

        Reponse = SageModificationsService.deleteLinkRoleRole(Request)

        If Reponse.out <> True Then
            Throw New ApplicationException("Code de retour différent de true dans l'appel de RemoveRoleRoleLink")
        End If
    End Sub

    Public Shared Sub RemoveUserResourceLink(ByVal Config As String, ByVal UserName As String, ByVal ResName1 As String, ByVal ResName2 As String, ByVal ResName3 As String)
        Dim SageModificationsService As SageModificationsService.SageModificationsServicePortType
        Dim Reponse As SageModificationsService.deleteLinkUserResourceResponse
        Dim Request As SageModificationsService.deleteLinkUserResourceRequest

        SageModificationsService = CreerSageModificationService()

        Request = New SageModificationsService.deleteLinkUserResourceRequest
        Request.in0 = Config
        Request.in1 = TsCuSageModificationUtils.DefineUserEntityName(UserName)
        Request.in2 = TsCuSageModificationUtils.DefineResourceEntityName(ResName1, ResName2, ResName3)

        Reponse = SageModificationsService.deleteLinkUserResource(Request)

        If Reponse.out <> True Then
            Throw New ApplicationException("Code de retour différent de true dans l'appel de RemoveUserResourceLink")
        End If
    End Sub

    Public Shared Sub RemoverUserRoleLink(ByVal Config As String, ByVal UserName As String, ByVal RoleName As String)
        Dim Reponse As SageModificationsService.deleteLinkUserRoleResponse
        Dim Request As SageModificationsService.deleteLinkUserRoleRequest
        Dim SageModificationsService As SageModificationsService.SageModificationsServicePortType
        Dim UserEntityName As SageModificationsService.UserEntityName
        Dim RoleEntityName As SageModificationsService.RoleEntityName

        SageModificationsService = CreerSageModificationService()

        UserEntityName = TsCuSageModificationUtils.DefineUserEntityName(UserName)

        RoleEntityName = TsCuSageModificationUtils.DefineRoleEntityName(RoleName)

        Request = New SageModificationsService.deleteLinkUserRoleRequest
        Request.in0 = Config
        Request.in1 = UserEntityName
        Request.in2 = RoleEntityName

        Reponse = SageModificationsService.deleteLinkUserRole(Request)

        If Reponse.out <> True Then
            Throw New ApplicationException("Code de retour différent de true dans l'appel de RemoverUserRoleLink")
        End If
    End Sub

    Public Shared Sub UpdateResourceField(ByVal Config As String, ByVal ResName1 As String, ByVal ResName2 As String, ByVal ResName3 As String, ByVal FieldName As String, ByVal FieldValue As String)
        Dim Reponse As SageModificationsService.updateResourceResponse
        Dim Request As SageModificationsService.updateResourceRequest
        Dim SageModificationsService As SageModificationsService.SageModificationsServicePortType
        Dim Resource As SageModificationsService.ResourceVO

        SageModificationsService = CreerSageModificationService()

        Resource = New SageModificationsService.ResourceVO
        Resource.name = TsCuSageModificationUtils.DefineResourceEntityName(ResName1, ResName2, ResName3)

        AddProperty(Resource.fields, FieldName, FieldValue)

        Request = New SageModificationsService.updateResourceRequest
        Request.in0 = Config
        Request.in1 = Resource

        Reponse = SageModificationsService.updateResource(Request)

        If Reponse.out <> True Then
            Throw New ApplicationException("Code de retour différent de true dans l'appel de UpdateResourceField")
        End If
    End Sub

    Public Shared Sub UpdateRoleField(ByVal Config As String, ByVal RoleName As String, ByVal FieldName As String, ByVal FieldValue As String)
        Dim Reponse As SageModificationsService.updateRoleResponse
        Dim Request As SageModificationsService.updateRoleRequest
        Dim SageModificationsService As SageModificationsService.SageModificationsServicePortType
        Dim Role As SageModificationsService.RoleVO

        SageModificationsService = CreerSageModificationService()

        Role = New SageModificationsService.RoleVO
        Role.name = TsCuSageModificationUtils.DefineRoleEntityName(RoleName)

        AddProperty(Role.fields, FieldName, FieldValue)

        Request = New SageModificationsService.updateRoleRequest
        Request.in0 = Config
        Request.in1 = Role

        Reponse = SageModificationsService.updateRole(Request)

        If Reponse.out <> True Then
            Throw New ApplicationException("Code de retour différent de true dans l'appel de UpdateRoleField")
        End If
    End Sub

    Public Shared Sub UpdateUser(ByVal Config As String, ByVal SageUser As TsCdSageUser)
        Dim Reponse As SageModificationsService.updateUserResponse
        Dim Request As SageModificationsService.updateUserRequest
        Dim SageModificationsService As SageModificationsService.SageModificationsServicePortType

        SageModificationsService = CreerSageModificationService()

        Request = New SageModificationsService.updateUserRequest
        Request.in0 = Config
        Request.in1 = ConvertSageUserToUserVO(SageUser)

        Reponse = SageModificationsService.updateUser(Request)

        If Reponse.out <> True Then
            Throw New ApplicationException("Code de retour différent de true dans l'appel de UpdateUser")
        End If
    End Sub

    Public Shared Sub UpdateUserField(ByVal Config As String, ByVal UserName As String, ByVal FieldName As String, ByVal FieldValue As String)
        Dim Reponse As SageModificationsService.updateUserResponse
        Dim Request As SageModificationsService.updateUserRequest
        Dim SageModificationsService As SageModificationsService.SageModificationsServicePortType
        Dim User As SageModificationsService.UserVO
        Dim UserEntityName As SageModificationsService.UserEntityName

        SageModificationsService = CreerSageModificationService()

        UserEntityName = TsCuSageModificationUtils.DefineUserEntityName(UserName)

        User = New SageModificationsService.UserVO
        User.name = UserEntityName

        AddProperty(User.fields, FieldName, FieldValue)

        Request = New SageModificationsService.updateUserRequest
        Request.in0 = Config
        Request.in1 = User

        Reponse = SageModificationsService.updateUser(Request)

        If Reponse.out <> True Then
            Throw New ApplicationException("Code de retour différent de true dans l'appel de UpdateUserField")
        End If
    End Sub

    Public Shared Sub ClearCache()
        Dim SageDNAService As SageDNAService.SageDNAServicePortType

        SageDNAService = CreerSageDNAService()
        SageDNAService.clearCaches()
    End Sub

    Public Shared Function AreLinkedUserRole(ByVal Config As String, ByVal codeUtilisateur As String, ByVal nomRole As String) As Boolean
        Dim SageBrowsingService As SageBrowsingService.SageBrowsingServicePortType

        SageBrowsingService = CreateSageBrowsingService()

        Dim request As New SageBrowsingService.areLinkedUserRoleRequest

        request.in0 = Config
        request.in1 = TsCuSageBrowsingUtils.DefineUserEntityName(codeUtilisateur)
        request.in2 = TsCuSageBrowsingUtils.DefineRoleEntityName(nomRole)

        Dim response As SageBrowsingService.areLinkedUserRoleResponse = SageBrowsingService.areLinkedUserRole(request)
        Return response.out
    End Function

    Public Shared Function AreLinkedRoleRole(ByVal Config As String, ByVal nomRoleSup As String, ByVal nomSousRole As String) As Boolean
        Dim SageBrowsingService As SageBrowsingService.SageBrowsingServicePortType

        SageBrowsingService = CreateSageBrowsingService()

        Dim request As New SageBrowsingService.areLinkedRoleRoleRequest

        request.in0 = Config
        request.in1 = TsCuSageBrowsingUtils.DefineRoleEntityName(nomRoleSup)
        request.in2 = TsCuSageBrowsingUtils.DefineRoleEntityName(nomSousRole)

        Dim response As SageBrowsingService.areLinkedRoleRoleResponse = SageBrowsingService.areLinkedRoleRole(request)
        Return response.out
    End Function

    Public Shared Function AreLinkedRoleResource(ByVal Config As String, ByVal nomRole As String, ByVal resName1 As String, ByVal resName2 As String, ByVal resName3 As String) As Boolean
        Dim SageBrowsingService As SageBrowsingService.SageBrowsingServicePortType

        SageBrowsingService = CreateSageBrowsingService()

        Dim request As New SageBrowsingService.areLinkedRoleResourceRequest

        request.in0 = Config
        request.in1 = TsCuSageBrowsingUtils.DefineRoleEntityName(nomRole)
        request.in2 = TsCuSageBrowsingUtils.DefineResourceEntityName(resName1, resName2, resName3)

        Dim response As SageBrowsingService.areLinkedRoleResourceResponse = SageBrowsingService.areLinkedRoleResource(request)
        Return response.out
    End Function

    Public Shared Function HasSageRole(ByVal Config As String, ByVal nomRole As String) As Boolean
        Dim SageBrowsingService As SageBrowsingService.SageBrowsingServicePortType

        SageBrowsingService = CreateSageBrowsingService()

        Dim request As New SageBrowsingService.hasSageRoleRequest

        request.in0 = Config
        request.in1 = nomRole

        Dim response As SageBrowsingService.hasSageRoleResponse = SageBrowsingService.hasSageRole(request)
        Return response.out
    End Function
#End Region

End Class

Friend Class TsCuSageModificationUtils

#Region "Méthodes publiques"
    Public Shared Function DefineResourceEntityName(ByVal ResName1 As String, ByVal ResName2 As String, ByVal ResName3 As String) As SageModificationsService.ResourceEntityName
        Dim RessourceEntityName As SageModificationsService.ResourceEntityName

        RessourceEntityName = New SageModificationsService.ResourceEntityName
        RessourceEntityName.name1 = ResName1
        RessourceEntityName.name2 = ResName2
        RessourceEntityName.name3 = ResName3
        RessourceEntityName.dataType = SageModificationsService.DataType.RESOURCE
        RessourceEntityName.dataTypeSpecified = True

        Return RessourceEntityName
    End Function

    Public Shared Function DefineRoleEntityName(ByVal RoleName As String) As SageModificationsService.RoleEntityName
        Dim RoleEntityName As SageModificationsService.RoleEntityName

        RoleEntityName = New SageModificationsService.RoleEntityName
        RoleEntityName.name = RoleName
        RoleEntityName.dataType = SageModificationsService.DataType.ROLE
        RoleEntityName.dataTypeSpecified = True

        Return RoleEntityName
    End Function

    Public Shared Function DefineUserEntityName(ByVal UserName As String) As SageModificationsService.UserEntityName
        Dim UserEntityName As SageModificationsService.UserEntityName

        UserEntityName = New SageModificationsService.UserEntityName
        UserEntityName.name = UserName
        UserEntityName.dataType = SageModificationsService.DataType.USER
        UserEntityName.dataTypeSpecified = True

        Return UserEntityName
    End Function
#End Region

End Class

Friend Class TsCuSageBrowsingUtils

#Region "Méthodes publiques"
    Public Shared Function DefineResourceEntityName(ByVal ResName1 As String, ByVal ResName2 As String, ByVal ResName3 As String) As SageBrowsingService.ResourceEntityName
        Dim RessourceEntityName As SageBrowsingService.ResourceEntityName

        RessourceEntityName = New SageBrowsingService.ResourceEntityName
        RessourceEntityName.name1 = ResName1
        RessourceEntityName.name2 = ResName2
        RessourceEntityName.name3 = ResName3
        RessourceEntityName.dataType = SageBrowsingService.DataType.RESOURCE
        RessourceEntityName.dataTypeSpecified = True

        Return RessourceEntityName
    End Function

    Public Shared Function DefineRoleEntityName(ByVal RoleName As String) As SageBrowsingService.RoleEntityName
        Dim RoleEntityName As SageBrowsingService.RoleEntityName

        RoleEntityName = New SageBrowsingService.RoleEntityName
        RoleEntityName.name = RoleName
        RoleEntityName.dataType = SageBrowsingService.DataType.ROLE
        RoleEntityName.dataTypeSpecified = True

        Return RoleEntityName
    End Function

    Public Shared Function DefineUserEntityName(ByVal UserName As String) As SageBrowsingService.UserEntityName
        Dim UserEntityName As SageBrowsingService.UserEntityName

        UserEntityName = New SageBrowsingService.UserEntityName
        UserEntityName.name = UserName
        UserEntityName.dataType = SageBrowsingService.DataType.USER
        UserEntityName.dataTypeSpecified = True

        Return UserEntityName
    End Function
#End Region

End Class