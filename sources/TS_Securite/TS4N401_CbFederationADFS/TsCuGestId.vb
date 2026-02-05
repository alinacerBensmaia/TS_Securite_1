Imports System.Web
Imports Microsoft.IdentityModel.Logging
Imports Microsoft.IdentityModel.Protocols.OpenIdConnect
Imports Microsoft.Owin.Extensions
Imports Microsoft.Owin.Security
Imports Microsoft.Owin.Security.Cookies
Imports Microsoft.Owin.Security.Notifications
Imports Microsoft.Owin.Security.OpenIdConnect
Imports Rrq.InfrastructureCommune.UtilitairesCommuns
Imports Rrq.InfrastructureCommune.UtilitairesCommuns.XuGeJournalEvenement
Imports Rrq.InfrastructureCommune.UtilitairesCommuns.XuCuGestionEvent
Imports Owin
Imports Microsoft.Owin
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Security.Claims
Imports Rrq.InfrastructureCommune.Parametres
Imports System.Security.Cryptography.X509Certificates
Imports Rrq.InfrastructureCommune

''' <summary>
''' Classe gérant l'authentification avec Owin
''' Le web.config du site doit avoir 
''' <add key="owin:AppStartup" value="TS4N401_CbFederationADFS.TsCuGestId"/>
''' dans la section appSettings du web,config
''' </summary>
Public Class TsCuGestId

    Private mInfCompl As String = String.Empty
    Private mInf As Proprietes = Nothing

    ''' <summary>
    ''' Cette méthode sera appelés lors du startup du site l'objet app sera reçu d'owin
    ''' Cette méthode a un try catch qui log l'erreur puis la relance pour l'avoir dans les logs RRQ..
    ''' </summary>
    ''' <param name="app"></param>
    Public Sub Configuration(app As IAppBuilder)
        Dim InfCompl As String = String.Empty
        Try
            'Si nous sommes sur un serveur du bon type : WESAA
            Dim AccepPartenaire As String = System.Configuration.ConfigurationManager.AppSettings.Item("TS_SERVEUR_ACCES_PARTENAIRES")
            If AccepPartenaire.ToUpper = "O" Then
                mInf = New TsCuCommun().ObtenirInformationConfig()
                mInfCompl = mInf.ToString()
                IdentityModelEventSource.ShowPII = mInf.ShowPII 'Mettre la valeur dans un config pour permettre d'activer ou non..
                app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType)
                'On doit passer notre propre cookie manager pour contrer un bug connu corrigé uniquement en .net core.
                'https://github.com/aspnet/AspNetKatana/wiki/System.Web-response-cookie-integration-issues
                app.UseCookieAuthentication(New CookieAuthenticationOptions() _
                                            With {.AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
                                                  .ExpireTimeSpan = TimeSpan.FromMinutes(mInf.DelaiCookie),
                                                  .CookieManager = New TsCuGestCookies(),
                                                  .CookieName = "Authentification",
                                                  .SlidingExpiration = True,
                                                  .Provider = New CookieAuthenticationProvider With {
                                                                .OnResponseSignIn = AddressOf GestionParmJetonApp,
                                                                .OnValidateIdentity = AddressOf ValiderJournliserJetonApp
                                                                }
                                                   })
                'Mise en place du lien de sécurité avec l’application
                'ClientID : Le numéro unique qui est présent dans ADFS pour l'application créé dans ce produit
                'Authority : L'emplacement de l'ADFS qui gère la sécurité de cette application
                app.UseOpenIdConnectAuthentication(New OpenIdConnectAuthenticationOptions With {
                    .ClientId = mInf.ClientID,
                    .Authority = mInf.Authority,
                    .Notifications = GestionNotifications(),
                    .RequireHttpsMetadata = mInf.RequiereHttps,
                    .TokenValidationParameters = New TsCuValidCert().ObtenirValidationCertificat()
                })
                'https://sts1.retraitequebec.gouv.qc.ca/federationmetadata/2007-06/federationmetadata.xml
                'https://sts1.retraitequebec.gouv.qc.ca/.well-known/openid-configuration
                'Définir la fonction d'authentification dans le pipeline
                app.Use(AddressOf GererAuthentificationADFS)
                'Se situer dans le pileline
                app.UseStageMarker(PipelineStage.Authenticate)
            Else
                'Le traitement ne sera pas inscrit dans IIS et ne gèrera pas l'authentification
            End If
        Catch ExceptionRecue As Exception 'Journaliser l'Erreur c'Est notre seul point pour intercepter dans ce cas.
            AjouterEvenmNormalise(XuGeJeApplicationCS, XuGeTypeEvenement.XuGeTeErreur, ExceptionRecue.Message, ExceptionRecue.Source, ExceptionRecue.StackTrace, mInfCompl)
            Throw ExceptionRecue
        End Try

    End Sub

    Private Sub GestionParmJetonApp(Context As CookieResponseSignInContext)
        Context.Properties.AllowRefresh = True
        Context.Properties.ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(mInf.DelaiCookie)
    End Sub

    Private Function ValiderJournliserJetonApp(context As Microsoft.Owin.Security.Cookies.CookieValidateIdentityContext) As Task
        Dim Infcontexte As String = String.Format("Contexte jeton application :  ExpireTimeSpan {0} Renouvelable {1} Date émission {2} Date Fin {3} Refresh {4}", context.Options.ExpireTimeSpan, context.Options.SlidingExpiration, context.Properties.IssuedUtc, context.Properties.ExpiresUtc, context.Properties.AllowRefresh)
        Dim Texte As String = String.Format("Authentification réussie pour {0} ({4}) sur le site {1} {2} {5} {2} Jeton : {3}", context.Identity.Name, HttpContext.Current.Request.Url.OriginalString, Environment.NewLine, TsCuJeton.ObtenirInfClaims(context.Identity.Claims), TsCuCommun.GenererInfrmServrClient, Infcontexte)
        PourDynatrace(Texte)
        Return Task.FromResult(0)
    End Function

    Private Sub PourDynatrace(Texte As String)
        'Cette méthode ne fait absolument rien. Mais si elle est active dans dynatrace avec l'options de voir le détail des paramètres d'entrés alors ça devient un log...
    End Sub

    Public Sub Deconnexion()
        Dim Contexte As IOwinContext = HttpContext.Current.Request.GetOwinContext()
        If Not Contexte.Authentication Is Nothing Then
            PourDynatrace($"Déconnexion de {Contexte.Authentication.User.Identity.Name}")
            Contexte.Authentication.SignOut()
        End If
    End Sub
    ''' <summary>
    ''' Préparation de l'objet des notifications open id et définition des méthodes réceptrices
    ''' </summary>
    ''' <returns></returns>
    Private Function GestionNotifications() As OpenIdConnectAuthenticationNotifications
        'ces deux lignes sont optionnelles.
        Dim ObjGestionNotifications As New OpenIdConnectAuthenticationNotifications With {
            .SecurityTokenValidated = AddressOf GereSecurityTokenValidated,
            .RedirectToIdentityProvider = AddressOf GereRedirectToIdentityProvider,
            .AuthenticationFailed = AddressOf AuthenticationFailedNotification
        }
        Return ObjGestionNotifications
    End Function


    ''' <summary>
    ''' Permet de définir l'url sur laquelle on sera redirigé une fois l'authentification réussie
    ''' </summary>
    Private Function GereRedirectToIdentityProvider(ByVal context As RedirectToIdentityProviderNotification(Of OpenIdConnectMessage, OpenIdConnectAuthenticationOptions)) As Task


        context.ProtocolMessage.RedirectUri = ObtenirUrl()
        'context.ProtocolMessage.PostLogoutRedirectUri = context.ProtocolMessage.RedirectUri
        'context.ProtocolMessage.ErrorUri = UrlRetour
        Return Task.FromResult(0)

    End Function


    Private Function ObtenirUrl() As String
        Dim UrlRetour As String = String.Empty

        UrlRetour = String.Format("{0}{1}", mInf.DomaineBase, mInf.RedirectUriBase)

        Return UrlRetour
    End Function

    ''' <summary>
    ''' Fonction qui sera utilisée par un AddressOF dans le app.Use afin de d'inscrire dans le pipeline
    ''' Gestion de l'authentification. Si l'utilisateur n'est pas authentifier forcer la connexion
    ''' Si on détecte déjà une erreur de connexion on lancera une exception 401 pour terminer l'exécution du site
    ''' </summary>
    Private Function GererAuthentificationADFS(ContexteOwinRecu As Microsoft.Owin.IOwinContext, ProchainParticipant As Func(Of Task)) As Task
        If Not HttpContext.Current Is Nothing Then
            If (Not HttpContext.Current.Request.IsAuthenticated) Then
                If HttpContext.Current.Request.RawUrl.ToLower().Contains("warmup") Then
                    CreeJetonWarmup(ContexteOwinRecu)
                Else
                    If Not EstErreurOpenIdConnect() Then
                        If EstPageExclue() Then
                            'Page sans authentification
                        Else
                            'IsPersistent permet de conserver le cookie même après la fermeture de la page et à la réouverture la session continue.
                            Dim ExpUtc As DateTime = DateTime.Now.Add(New TimeSpan(0, mInf.DelaiCookie, 0))
                            Dim prop As New AuthenticationProperties With {
                                .IsPersistent = False,
                                .AllowRefresh = True
                            }
                            ContexteOwinRecu.Authentication.Challenge(prop, OpenIdConnectAuthenticationDefaults.AuthenticationType)
                        End If
                    End If
                End If
            End If
        End If
        Return ProchainParticipant.Invoke()
    End Function

    ''' <summary>
    ''' Méthode ajouté afin de créer un jeton au traitement qui fait le warmup des pages apsx. Sinon il est refusé et ne peux pré charger les pages
    ''' </summary>
    ''' <param name="ContexteOwinRecu"></param>
    Private Sub CreeJetonWarmup(ContexteOwinRecu As Microsoft.Owin.IOwinContext)
        'Valider qu'on a reçu un certificat dans la requête
        If CertificatValide() Then
            'Dim identity = New System.Security.Principal.GenericIdentity("Warmup")
            Dim claims As New List(Of Claim) From {New Claim(IdentityModel.Claims.ClaimTypes.Name, "Warmup"), New Claim(IdentityModel.Claims.ClaimTypes.Upn, "Warmup@warmup.com")}
            Dim identity As New ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationType)
            Dim principal = New System.Security.Principal.GenericPrincipal(identity, Nothing)
            Thread.CurrentPrincipal = principal
            HttpContext.Current.User = principal
            ContexteOwinRecu.Authentication.SignIn(New AuthenticationProperties With {.IsPersistent = False}, identity)
        Else
            HttpContext.Current.Response.StatusDescription = "Vous n'avez pas les accès requis pour utiliser l'application Warmup"
            HttpContext.Current.Response.StatusCode = 403 'On refuse l'accès
        End If
    End Sub

    Private Function CertificatValide() As Boolean
        Return True
        Dim retour As Boolean = False
        If HttpContext.Current.Request.ClientCertificate.IsPresent() Then
            Dim cle As String = XuCuConfiguration.ObtenirValeurSystemeOptionnelle("XU5", "XU5N154\ThumbprintCertificatIISMappingCertifClient")
            Dim x509 = New X509Certificate2(HttpContext.Current.Request.ClientCertificate.Certificate)
            Dim chain = New X509Chain(True)
            chain.ChainPolicy.RevocationMode = X509RevocationMode.Offline
            chain.Build(x509)
            If x509.Thumbprint = cle Then
                retour = True
            End If
        Else
            Dim environnement As String = Parametres.XuCuConfiguration.ValeurSysteme("General", "Environnement")
            If environnement.ToUpper() = "UNIT" Then
                retour = True
            End If
        End If
        Return retour

    End Function

    Private Function EstErreurOpenIdConnect() As Boolean
        Dim retour = False
        Dim ContenuErreur As String = String.Empty
        If Not String.IsNullOrEmpty(HttpContext.Current.Request.QueryString("error")) Then
            If Not String.IsNullOrEmpty(HttpContext.Current.Request.QueryString("error_description")) Then
                If Not String.IsNullOrEmpty(HttpContext.Current.Request.QueryString("state")) Then
                    If Not String.IsNullOrEmpty(HttpContext.Current.Request.QueryString("client-request-id")) Then
                        ContenuErreur = HttpContext.Current.Request.QueryString.ToString()
                        AjouterEvenmNormalise(XuGeJeApplicationCS, XuGeTypeEvenement.XuGeTeErreur, ContenuErreur, "TS4N401_CbFederationADFS", "EstErreurOpenIdConnect", "")
                        retour = True
                    End If
                End If
            End If
        End If
        Return retour
    End Function

    Private Function GereSecurityTokenValidated(ByVal context As SecurityTokenValidatedNotification(Of OpenIdConnectMessage, OpenIdConnectAuthenticationOptions)) As Task
        'Journaliser l'authentification réussie
        Dim Infcontexte As String = String.Format("Contexte jeton adfs : ExpiresUtc {0} IssuedUtc {1}", context.AuthenticationTicket.Properties.ExpiresUtc, context.AuthenticationTicket.Properties.IssuedUtc)
        Dim Texte As String = String.Format("Authentification réussie pour {0} ({4}) sur le site {1} {2} {5} {2} Jeton : {3}", context.AuthenticationTicket.Identity.Name, HttpContext.Current.Request.Url.OriginalString, Environment.NewLine, TsCuJeton.ObtenirInfClaims(context.AuthenticationTicket.Identity.Claims), TsCuCommun.GenererInfrmServrClient, Infcontexte)
        AjouterEvenmNormalise(XuGeJournalEvenement.XuGeJeSecuriteRRQ, XuGeTypeEvenement.XuGeTeSuccesAudit, Texte, "", "", mInfCompl)
        Return Task.FromResult(0)
    End Function

    Private Function AuthenticationFailedNotification(ByVal context As AuthenticationFailedNotification(Of OpenIdConnectMessage, OpenIdConnectAuthenticationOptions)) As Task
        Dim Erreur As String = String.Empty
        If Not context Is Nothing Then
            context.HandleResponse()
            If Not String.IsNullOrEmpty(context.ProtocolMessage.ErrorDescription) Then
                Erreur = String.Concat(Erreur, "-- ProtocolMessage : ", context.ProtocolMessage.ErrorDescription)
            End If
            If Not String.IsNullOrEmpty(context.Exception.Message) Then
                Erreur = String.Concat(Erreur, "-- Message Exception : ", context.Exception.Message)
            End If
        End If

        Dim url As String = String.Empty
        If Not HttpContext.Current Is Nothing Then
            url = HttpContext.Current.Request.Url.OriginalString
        End If
        Dim Informations As String = TsCuCommun.GenererInfrmServrClient
        AjouterEvenmNormalise(XuGeJournalEvenement.XuGeJeApplicationCS, XuGeTypeEvenement.XuGeTeErreur, String.Format("Authentification en erreur pour {0} sur le site {1} {2} Erreur : {3}", Informations, url, Environment.NewLine, Erreur), "", "", mInfCompl)
        AjouterEvenmNormalise(XuGeJournalEvenement.XuGeJeSecuriteRRQ, XuGeTypeEvenement.XuGeTeEchecAudit, String.Format("Authentification en erreur pour {0} sur le site {1} {2} Erreur : {3}", Informations, url, Environment.NewLine, Erreur), "", "", mInfCompl)
        'créer un retour pour le client
        HTMLAccesRefuse()
        Return Task.FromResult(0)
    End Function

    Private Sub HTMLAccesRefuse()
        If HttpContext.Current IsNot Nothing Then
            Dim Contexte As HttpContext = HttpContext.Current
            Dim html As String = "<hmtl>"
            Dim Apos As String = """"
            html = String.Concat(html, $"<head><meta http-equiv={Apos}Content-Type{Apos} content={Apos}text/html;charset=utf8{Apos}/></head>")
            html = String.Concat(html, $"<body>")
            html = String.Concat(html, "<img style='display:block; width:211px;height:72px;' id='base64image'  src='data:image/png;base64,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' />")
            html = String.Concat(html, "<br/>")
            html = String.Concat(html, "<h2>Vous n'avez pas les accès requis pour accéder à cette application</h2>")
            html = String.Concat(html, "</body></html>")
            Contexte.Response.StatusCode = 403
            Contexte.Response.ContentType = "text/html"
            Contexte.Response.Write(html)
        End If
    End Sub

    Private Function EstPageExclue() As Boolean
        Dim retour As Boolean = False
        If Not String.IsNullOrEmpty(mInf.RegExExclusion) AndAlso Regex.Match(HttpContext.Current.Request.Url.OriginalString, mInf.RegExExclusion, RegexOptions.IgnoreCase).Success Then
            retour = True
        End If
        Return retour
    End Function

End Class