<Serializable>
Friend Class Proprietes
    Public Property ClientID As String
    Public Property Authority As String
    Public Property RedirectUriBase As String
    Public Property DomaineBase As String
    Public Property ShowPII As Boolean
    Public Property RegExExclusion As String
    Public Property RequiereHttps As Boolean
    Public Property DelaiCookie As Integer
    Public Overrides Function ToString() As String
        Dim retour As String = String.Empty
        If Not ClientID Is Nothing Then retour = String.Concat(retour, " ID client : ", ClientID)
        If Not Authority Is Nothing Then retour = String.Concat(retour, " Authority : ", Authority)
        If Not RedirectUriBase Is Nothing Then retour = String.Concat(retour, " RedirectUri : ", RedirectUriBase)
        If Not DomaineBase Is Nothing Then retour = String.Concat(retour, " Domaine : ", DomaineBase)
        retour = String.Concat(retour, " ShowPII : ", ShowPII)
        If Not RegExExclusion Is Nothing Then retour = String.Concat(retour, " RegExExclusion : ", RegExExclusion)
        retour = String.Concat(retour, " RequiereHttps : ", RequiereHttps)
        retour = String.Concat(retour, " DelaiCookies : ", DelaiCookie.ToString())
        Return retour
    End Function

End Class
