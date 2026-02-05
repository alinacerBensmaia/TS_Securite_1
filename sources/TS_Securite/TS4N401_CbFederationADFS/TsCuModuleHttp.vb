Imports System.Threading
Imports System.Web
Imports Rrq.InfrastructureCommune
''' <summary>
''' Cette classe sera inscrite comme HTTP module dans le web.config 
''' Pour le site des partenaires sera le «fail safe» d'authentification
''' Pour les cssaa le httpmodule sera aussi en place.
''' Si la dll n'est pas sur le serveur le site plantera
''' On ajoute aussi le ménage des headers
''' </summary>
Public Class TsCuModuleHttp
    Implements IHttpModule 'Implémentation de l'interface HttpModule

    Private _app As HttpApplication
    Private _html As New Text.StringBuilder()
    Private _ex As Exception
    Private mExclureCache As Boolean
    Private mDelaiExpirationCache As Integer
    Private _StrictTransport As Boolean = True

    ''' <summary>
    ''' Implémentation des méthodes INIT et DISPOSE du HttpModule
    ''' </summary>
    ''' <param name="app">HttpApplication</param>
    Public Sub Init(ByVal app As HttpApplication) Implements IHttpModule.Init
        _app = app
        Dim ObjTS As New TsCuCommun()
        Boolean.TryParse(ObjTS.ObtenirCleConfigRacine("SansCacheClient"), mExclureCache)
        Integer.TryParse(ObjTS.ObtenirCleConfigRacine("DelaiExpirationCacheClient"), mDelaiExpirationCache)
        ObjTS = Nothing
        Dim ParametreStrict As String = Parametres.XuCuConfiguration.ObtenirValeurSystemeOptionnelle("TS4", "TS4\TS4N401\Strict-Transport-Security")
        If (Not String.IsNullOrEmpty(ParametreStrict) AndAlso ParametreStrict.ToUpper() = "FALSE") Then
            _StrictTransport = False
        End If
        AddHandler _app.PreSendRequestHeaders, AddressOf Me.PreSendRequestHeaders
    End Sub
    Public Sub Dispose() Implements IHttpModule.Dispose
        If Not _app Is Nothing Then
            Try
                RemoveHandler _app.PreSendRequestHeaders, AddressOf Me.PreSendRequestHeaders
            Catch ex As Exception
                'rien faire dans ce cas...
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Définir les options pour la mise en cache des pages et des ressources
    ''' </summary>
    Private Sub MAJCache()
        If HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.ToLower.Contains(".aspx") Or mExclureCache Then
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
            HttpContext.Current.Response.Cache.SetNoStore()
        Else
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Private)
            HttpContext.Current.Response.Cache.SetMaxAge(New TimeSpan(0, mDelaiExpirationCache, 0)) 'Délai expiration cache selon la valeur dans le config
            HttpContext.Current.Response.Cache.SetETagFromFileDependencies()
            HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches)
        End If
    End Sub
    ''' <summary>
    ''' Ajout des headers de sécurité
    ''' </summary>
    Private Sub AjoutHeaders()
        HttpContext.Current.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN")
        HttpContext.Current.Response.Headers.Add("X-XSS-Protection", "1; mode=block")
        HttpContext.Current.Response.Headers.Add("X-Robots-Tag", "noindex, nofollow")

        'Entêtes de politique de sécurité 
        'Les entêtes doivent être inscrites sur les pages et les ressources (Suite a une recommandation de la sécurité)
        HttpContext.Current.Response.Headers.Add("Content-Security-Policy", "default-src 'self';script-src 'self' 'unsafe-inline';style-src 'self' 'unsafe-inline'")
        'Entête dépréciée mais utilisée par IE11 on doit donc la garder
        HttpContext.Current.Response.Headers.Add("X-Content-Security-Policy", "default-src 'self';script-src 'self' 'unsafe-inline';style-src 'self' 'unsafe-inline'")
        'Entête dépréciée mais utilisée par anciennes versions de chrome on doit donc la garder
        HttpContext.Current.Response.Headers.Add("X-WebKit-CSP", "default-src 'self';script-src 'self' 'unsafe-inline';style-src 'self' 'unsafe-inline'")

        HttpContext.Current.Response.Headers.Add("X-Content-Type-Options", "nosniff")
        HttpContext.Current.Response.Headers.Add("X-Download-Options", "noopen")

        HttpContext.Current.Response.Headers.Add("Referrer-Policy", "strict-origin")

        'Ce paramètre ne doit être poussé que dans les environnements autres que unitaire
        If _StrictTransport Then
            HttpContext.Current.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains")
        End If

    End Sub
    ''' <summary>
    ''' Méthode appelés juste avant que le client reçoive sa requête
    ''' On ajuste les headers et la cache
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub PreSendRequestHeaders(ByVal sender As Object, ByVal e As EventArgs)
        Dim NomsRetrait As New List(Of String) From {
            "X-Powered-By",
            "X-AspNet-Version",
            "X-AspNetMvc-Version",
            "Server",
            "CODE_LANGUAGE"
        }
        RetirerHeader(NomsRetrait)
        AjoutHeaders()
        MAJCache()
    End Sub
    ''' <summary>
    ''' Retirer les headers de la liste selon la liste recue en entrée
    ''' </summary>
    ''' <param name="Noms"></param>
    Private Sub RetirerHeader(Noms As List(Of String))
        Dim Cles As New List(Of String)
        For Each elm As String In HttpContext.Current.Response.Headers.AllKeys
            Cles.Add(elm)
        Next
        For Each elm As String In Cles
            Dim retour = From unelm In Noms Where unelm.ToLower = elm.ToLower
            If retour.Count > 0 Then
                HttpContext.Current.Response.Headers.Remove(elm)
            End If
        Next
    End Sub
End Class
