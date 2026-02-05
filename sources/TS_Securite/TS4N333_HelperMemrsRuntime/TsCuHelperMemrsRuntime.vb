Imports System.Runtime.Caching
Imports System.Data
Imports System.Diagnostics
Imports System.Collections.Specialized
Imports Rrq.InfrastructureCommune.Parametres

Public Class TsCuHelperMemrsRuntime
    Implements TsIHelperMemrsSecurite


    Private Shared mCache As MemoryCache = Nothing
    Private Shared mexpire As Integer = 60
    Private Shared mpath As String = String.Empty
    Private Shared mNrbItemMax As Integer = 0
    Private Shared mPrctTrim As Integer = 10
    Private Const separateur As String = "_"c
    Private mlogActif As Boolean = False

    Public Sub New()
        If mCache Is Nothing Then
            ObtenirCache()
        End If
    End Sub


    Public Function ObtenirObjetMemoire(ByVal pCleObjetMemoire As String) As Object Implements TsIHelperMemrsSecurite.ObtenirObjetMemoire

        Return mCache.Get(pCleObjetMemoire)

    End Function

    Public Sub Memoriser(ByVal pCleObjetMemoire As String, ByVal pObjetSecurite As Object) Implements TsIHelperMemrsSecurite.Memoriser

        Dim policy As CacheItemPolicy = New CacheItemPolicy()
        policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(mexpire)
        policy.RemovedCallback = New CacheEntryRemovedCallback(AddressOf SuppressionObjMemoire)
        Dim monChangeMonitor As New TsCuChangeMonitor()
        policy.ChangeMonitors.Add(monChangeMonitor)

        ' On l'ajoute à la cache
        mCache.Set(pCleObjetMemoire, pObjetSecurite, policy)

        ' On évince un pourcentage de groupes selon le nombre maximum de groupes que l'on veut maintenir dans la cache
        ' Par défaut, on ne le fait pas.
        If mNrbItemMax > 0 AndAlso mCache.GetCount > mNrbItemMax Then
            mCache.Trim(mPrctTrim)
        End If

        ' Et on log
        Logging(pCleObjetMemoire & " n'était pas dans la cache de " & mpath & ", on l'ajoute.")
        
    End Sub


    Public Sub Dememoriser(ByVal pCleObjetMemoire As String) Implements TsIHelperMemrsSecurite.Dememoriser
        If mCache.Remove(pCleObjetMemoire) Is Nothing Then
            Logging(pCleObjetMemoire & " n'était pas mémorisé dans " & mpath)
        End If
    End Sub

    Public Sub DememoriserTout() Implements TsIHelperMemrsSecurite.DememoriserTout
        Logging("DememoriseTout les " & mCache.GetCount() & " objets dans la cache de " & mpath)
        ' Appele le changeMonitor de tous les éléments en cache qui force un changement et donc une suppression de l'élément
        TsCuChangeMonitor.DememoriseTout()
    End Sub

    Private Sub ObtenirCache()
        ' Si la config existe et n'est pas vide alors, on ne logue pas
        mlogActif = String.IsNullOrEmpty(XuCuPolitiqueConfig.ConfigDomaine.ObtenirValeurSystemeOptionnelle("TS4", "TS4\TS4N333\DesactiveLog"))

        ' Adresse si dans IIS, sinon juste nom machine
        mpath = Environment.MachineName
        If String.IsNullOrEmpty(System.Web.HttpRuntime.AppDomainAppVirtualPath) Then
            mpath &= String.Join(" ", Environment.GetCommandLineArgs)
            mpath &= " " & System.AppDomain.CurrentDomain.FriendlyName
        Else
            Dim port As String = XuCuConfiguration.ObtenirValeurSystemeOptionnelle("XU5", "XU5\XU5N151\Port")
            If Not String.IsNullOrEmpty(port) Then
                port = ":" & port
            End If
            mpath &= port & System.Web.HttpRuntime.AppDomainAppVirtualPath
        End If

        Dim maxObj As String = XuCuPolitiqueConfig.ConfigDomaine.ObtenirValeurSystemeOptionnelle("TS4", "TS4\TS4N333\NrbMaxObjet")

        ' Nombre maximum d'objet dans la cache
        If Not String.IsNullOrEmpty(maxObj) Then
            mNrbItemMax = CInt(maxObj)
        End If

        Dim prctItem As String = XuCuPolitiqueConfig.ConfigDomaine.ObtenirValeurSystemeOptionnelle("TS4", "TS4\TS4N333\PrctTrim")

        ' Pourcentage à retirer de la cache, defaut 10
        If Not String.IsNullOrEmpty(prctItem) Then
            mPrctTrim = CInt(prctItem)
        End If

        Dim expiration As String = XuCuPolitiqueConfig.ConfigDomaine.ObtenirValeurSystemeOptionnelle("TS4", "TS4\TS4N333\ExpirationCacheLocal")

        ' Temps expiration (minutes) du cache client par défaut
        If Not String.IsNullOrEmpty(expiration) Then
            mexpire = CInt(expiration)
        End If

        Dim limiteMem As String = XuCuPolitiqueConfig.ConfigDomaine.ObtenirValeurSystemeOptionnelle("TS4", "TS4\TS4N333\LimitMem")

        ' Limite de mémoire à utiliser, 0 = se gère tout seul
        If Not String.IsNullOrEmpty(limiteMem) Then
            limiteMem = "0"
        End If

        Dim pourcentMemPhys As String = XuCuPolitiqueConfig.ConfigDomaine.ObtenirValeurSystemeOptionnelle("TS4", "TS4\TS4N333\PrcntMemPhys")

        ' Pourcentage de mémoire physique à utiliser, 0 = se gère tout seul, sinon de 1 à 100
        If Not String.IsNullOrEmpty(pourcentMemPhys) Then
            pourcentMemPhys = "0"
        End If

        Dim polling As String = XuCuPolitiqueConfig.ConfigDomaine.ObtenirValeurSystemeOptionnelle("TS4", "TS4\TS4N333\Polling")

        ' Intervalle qu'il check les statistiques de mémoire (2 configs précédentes)
        ' Par défaut 2 minutes
        If Not String.IsNullOrEmpty(polling) Then
            polling = "00:02:00"
        End If

        Dim memconfigs As New NameValueCollection()
        memconfigs.Add("cacheMemoryLimitMegabytes", limiteMem)
        memconfigs.Add("physicalMemoryLimitPercentage", pourcentMemPhys)
        memconfigs.Add("pollingInterval", polling)

        ' Get a cache client for the cache
        Dim nomcache As String = XuCuConfiguration.ObtenirValeurSystemeOptionnelle("TS4", "TS4\TS4N333\NomCache")
        If String.IsNullOrEmpty(nomcache) Then
            nomcache = "TS4CacheSecrtApplicative"
        End If

        mCache = New MemoryCache(nomcache, memconfigs)
        Logging("Création de la cache " & nomcache & " dans " & mpath)
    End Sub

    Private Sub SuppressionObjMemoire(ByVal args As CacheEntryRemovedArguments)
        ' On log qui et la raison 
        Logging(args.CacheItem.Key & " est retiré de la cache " & mpath & " car " & args.RemovedReason.ToString())
    End Sub

    Private Sub Logging(ByVal message As String)
        Logging(message, EventLogEntryType.Information)
    End Sub

    Private Sub Logging(ByVal message As String, ByVal type As EventLogEntryType)
        If mlogActif Then
            System.Diagnostics.Trace.WriteLine(DateTime.Now.ToString() & " " & mpath & " " & message, "ApplicationRRQ")
            System.Diagnostics.EventLog.WriteEntry("ApplicationRRQ", message, type)
        End If
    End Sub

    Private Sub AjoutChangeMonitor(state As Object)
        Logging("On vient d'ajouter notre TsCuChangeMonitor et appeler NotifyOnChanged.")
    End Sub


End Class
