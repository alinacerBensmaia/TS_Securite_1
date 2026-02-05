Imports System.Web.Services.Protocols
Imports System.Reflection
Imports Rrq.Securite.GestionAcces.portailsage
Imports Rrq.Securite.GestionAcces.portailsage1
Imports Rrq.Securite.GestionAcces.portailsage2
Imports Rrq.Securite.GestionAcces.portailsage3
Imports Configuration = Rrq.InfrastructureCommune.Parametres.XuCuConfiguration

' Attention, une seule instance, voir plus bas
Friend Module TsBaServicesSage

    ' Variables de cache
    Private _cfgGetMethods As New Generic.Dictionary(Of String, System.Reflection.MethodInfo)
    Private _services As New Generic.Dictionary(Of Type, SoapHttpClientProtocol)

#Region "Propriétés de configuration"
    ' Ces deux propriétés de configuration doivent être accessibles aux méthodes qui appels les services directement
    Friend ReadOnly Property ServeurSage() As String
        Get
            Return Configuration.ValeurSysteme("TS7", "TS7N242\ServeurSage")
        End Get
    End Property

    Friend ReadOnly Property BDSage() As String
        Get
            Return Configuration.ValeurSysteme("TS7", "TS7N242\BDSage")
        End Get
    End Property
#End Region

#Region "Propriétés et méthode pour obtenir les différents ""Services"" de Sage"
    Friend ReadOnly Property DataService() As SageDataService
        Get
            Return ObtenirService(Of SageDataService)()
        End Get
    End Property

    Friend ReadOnly Property BasicService() As SageBasicService
        Get
            Return ObtenirService(Of SageBasicService)()
        End Get
    End Property

    Friend ReadOnly Property EntityDataService() As SageEntityDataService
        Get
            Return ObtenirService(Of SageEntityDataService)()
        End Get
    End Property

    Friend ReadOnly Property DiffService() As SageDiffService
        Get
            Return ObtenirService(Of SageDiffService)()
        End Get
    End Property


    ' Attention, on retourne toujours la même instance, ça ne sera peut-être pas approprié
    ' quand on va être multiuser pour le Web (RevertToSelf?)
    Private Function ObtenirService(Of T As {New, SoapHttpClientProtocol})() As T
        Dim service As SoapHttpClientProtocol = Nothing
        If Not _services.TryGetValue(GetType(T), service) Then
            service = New T
            service.PreAuthenticate = True
            service.Credentials = System.Net.CredentialCache.DefaultCredentials
            '! Pour évité la surcharge des ports d'appel, nous pouvons demander de réutilisé la même connexion.
            '! Cette portion de code force le service à utilisé la même connection.
            '! Cette utilisation pourrait être problèmatique si le programme était multiutilisateur, donc par défaut on évite le partage
            service.UnsafeAuthenticatedConnectionSharing = partageUnsafePermis
            '!----------
            _services.Item(GetType(T)) = service
        End If
        Return DirectCast(service, T)
    End Function
#End Region

#Region "Méthodes génériques pour l'appel de services"
    ' Paramètres et retour As String
    Public Function call_sage_string(Of S As {New, SoapHttpClientProtocol})(ByVal op As String, ByVal ParamArray params() As String) As String
        Return call_sage_string(ObtenirService(Of S)(), op, params)
    End Function

    ' Paramètres et retour As String
    Public Function call_sage_string(ByVal srv As SoapHttpClientProtocol, ByVal op As String, ByVal ParamArray params() As String) As String
        Dim mi As MethodInfo = Nothing
        If Not _cfgGetMethods.TryGetValue(op, mi) Then
            mi = srv.GetType().GetMethod(op)
            _cfgGetMethods.Item(op) = mi
        End If
        Dim paramMethode(mi.GetParameters().Length - 1) As Object
        If paramMethode.Length > 0 Then paramMethode(0) = ServeurSage
        If paramMethode.Length >= 1 Then paramMethode(1) = BDSage
        For i As Integer = 2 To paramMethode.Length - 1
            Dim iParam As Integer = i - 2
            If iParam < params.Length Then paramMethode(i) = params(iParam)
        Next
        Dim res As String = DirectCast(mi.Invoke(srv, paramMethode), String)
        Return res
    End Function

    ' Pour les méthodes avec au moins 2 paramètres, version retour fortement typé
    Public Function call_sage(Of S As {New, SoapHttpClientProtocol}, T)(ByVal op As String, ByVal ParamArray params() As String) As T
        Return Deserialize(Of T)(call_sage_string(Of S)(op, params))
    End Function
    ' Pour les méthodes avec au moins 2 paramètres, version retour fortement typé
    Public Function call_sage_string(Of S As {New, SoapHttpClientProtocol}, T)(ByVal op As String, ByVal ParamArray params() As String) As String
        Return call_sage_string(Of S)(op, params)
    End Function
#End Region

End Module
