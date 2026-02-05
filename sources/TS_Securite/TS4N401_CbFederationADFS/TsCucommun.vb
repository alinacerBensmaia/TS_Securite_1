Imports System.Web
Imports Rrq.InfrastructureCommune.Parametres

Friend Class TsCuCommun
    ''' <summary>
    ''' Obtenir les informations de configuration de la fédération ADFS dans un fichier de configuration.
    ''' Le tout sera par phase dans le fichier de conifg. Prod sera phase = 1 
    ''' </summary>
    ''' <returns></returns>
    Friend Function ObtenirInformationConfig() As Proprietes
        Dim elm As New Proprietes
        Dim SysCle As String = "TS4"
        Dim AppPath As String = HttpContext.Current.Request.ApplicationPath.Replace("/", "")
        Dim BaseCle As String = String.Format("{0}\TS4N401\{1}", SysCle, AppPath)
        Dim CleTS As String = String.Format("{0}\TS4N401", SysCle)

        With elm
            .ClientID = XuCuConfiguration.ValeurSysteme(SysCle, String.Format("{0}\{1}", BaseCle, "ClientId"))
            .Authority = XuCuConfiguration.ValeurSysteme(SysCle, String.Format("{0}\{1}", BaseCle, "Authority"))
            .RedirectUriBase = XuCuConfiguration.ValeurSysteme(SysCle, String.Format("{0}\{1}", BaseCle, "RedirectUri"))
            .DomaineBase = XuCuConfiguration.ValeurSysteme(SysCle, String.Format("{0}\{1}", BaseCle, "Domaine"))
            Boolean.TryParse(XuCuConfiguration.ValeurSysteme(SysCle, String.Format("{0}\{1}", CleTS, "ShowPII")), elm.ShowPII)
            Boolean.TryParse(XuCuConfiguration.ValeurSysteme(SysCle, String.Format("{0}\{1}", CleTS, "HttpsRequis")), elm.RequiereHttps)
            .RegExExclusion = XuCuConfiguration.ValeurSysteme(SysCle, String.Format("{0}\{1}", BaseCle, "RegExExlusion"))
            .DelaiCookie = Integer.Parse(XuCuConfiguration.ValeurSysteme(SysCle, String.Format("{0}\{1}", BaseCle, "ExpirationCookies")))
        End With
        Return elm
    End Function
    Friend Function ObtenirCleConfigRacine(NomCle As String) As String
        Dim SysCle As String = "TS4"
        Dim CleTS As String = String.Format("{0}\TS4N401", SysCle)
        Return XuCuConfiguration.ValeurSysteme(SysCle, String.Format("{0}\{1}", CleTS, NomCle))
    End Function


    'Inspiré de XW2I012
    Friend Shared Function GenererInfrmServrClient() As String
        ' Ajouter dans le détail de l'erreur des informations sur le serveur et sur le client
        Dim infrmServrClient As String = Environment.NewLine
        Try
            Dim app As System.Web.HttpContext = HttpContext.Current()
            infrmServrClient &= "-------------------------------------------" & Environment.NewLine
            infrmServrClient &= "Nom de la machine : " & app.Server.MachineName & Environment.NewLine
            infrmServrClient &= "Adresse IP du client : " & app.Request.UserHostAddress & Environment.NewLine
            infrmServrClient &= "URL demandé : " & HttpContext.Current.Server.UrlDecode(app.Request.Url.AbsoluteUri) & Environment.NewLine
            infrmServrClient &= "UserAgent : " & app.Request.UserAgent & Environment.NewLine
            infrmServrClient &= "Instance de fureteur : " & app.Request.Browser.Type & Environment.NewLine
            infrmServrClient &= "Numéro de version du fureteur : " & app.Request.Browser.Version & Environment.NewLine
            infrmServrClient &= "Système d'exploitation : " & app.Request.Browser.Platform & Environment.NewLine
            infrmServrClient &= "-------------------------------------------" & Environment.NewLine
        Catch ex As Exception
            infrmServrClient &= "Exception : " & ex.Message
            'Ne pas planter dans ce cas.
        End Try

        Return infrmServrClient
    End Function


End Class
