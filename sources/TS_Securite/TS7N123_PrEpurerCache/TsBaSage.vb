Imports Rrq.InfrastructureCommune.Parametres
Imports System.ServiceModel
Imports System.ServiceModel.Channels

'''-----------------------------------------------------------------------------
''' Project		: $safeprojectname$
''' Class		: TsBaSage
'''
'''-----------------------------------------------------------------------------
''' <summary>
''' Classe permettant de stocker les paramètres utilisés par l'application. Elle
''' contient des propriétés partagées.
''' </summary>
''' <remarks></remarks>
'''-----------------------------------------------------------------------------
Friend NotInheritable Class TsBaSage

#Region "--- Variables ---"

    Private Shared _sageDNAService As SageDNAService.SageDNAServicePortTypeClient = Nothing

#End Region

#Region "--- Publiques ---"

#Region "--- Méthodes ---"

    ''' <summary>
    ''' 
    ''' </summary>
    Public Shared Sub ClearCache()
        Dim SageDNAService As SageDNAService.SageDNAServicePortType

        SageDNAService = CreerSageDNAService()
        SageDNAService.clearCaches()
    End Sub

#End Region

#End Region

#Region "--- Privées ---"

#Region "--- Propriétés ---"

    ''' <summary>
    ''' 
    ''' </summary>
    Private Shared ReadOnly Property UrlSageBrowsingService() As String
        Get
            Return XuCuConfiguration.ValeurSysteme("TS7", "TS7\TS7N243\UrlSageBrowsingService")
        End Get
    End Property
    Private Shared ReadOnly Property UrlSageDNAService() As String
        Get
            Return XuCuConfiguration.ValeurSysteme("TS7", "TS7\TS7N243\UrlSageDNAService")
        End Get
    End Property

#End Region

#Region "--- Méthodes ---"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Shared Function CreerSageDNAService() As SageDNAService.SageDNAServicePortType
        If _sageDNAService Is Nothing OrElse _sageDNAService.State <> CommunicationState.Opened Then
            Dim isHttps As Boolean = UrlSageBrowsingService.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase)
            Dim service As SageDNAService.SageDNAServicePortTypeClient = _
                New SageDNAService.SageDNAServicePortTypeClient(CreateBindingUserName(isHttps), New EndpointAddress(UrlSageDNAService))
            ObtenirCodeAccesMotPasseCleSymbolique(service.ClientCredentials)

            _sageDNAService = service
        End If

        Return _sageDNAService
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="clientCredentials"></param>
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

    Private Shared Function CreateBindingUserName(ByVal isHttps As Boolean) As System.ServiceModel.Channels.Binding
        Dim bec As New BindingElementCollection()

        ' Definis la securite de la communication
        Dim MySecurityBindingElement As TransportSecurityBindingElement
        MySecurityBindingElement = SecurityBindingElement.CreateUserNameOverTransportBindingElement
        MySecurityBindingElement.IncludeTimestamp = False

        ' Encode les message en UTF-8
        Dim MyTextMessageEncodingElement As New TextMessageEncodingBindingElement
        MyTextMessageEncodingElement.WriteEncoding = New System.Text.UTF8Encoding()
        MyTextMessageEncodingElement.MessageVersion = MessageVersion.Soap11

        Dim transportBindingElement As TransportBindingElement = Nothing
        If isHttps Then
            transportBindingElement = New HttpsTransportBindingElement
        Else
            transportBindingElement = New TsHttpTransportBindingEpuration
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

#End Region

#End Region

End Class
