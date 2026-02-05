Imports System.Web
Imports Microsoft.Owin
Imports Microsoft.Owin.Infrastructure

''' <summary>
''' Classe ajoutée pour corriger un bug connu de cookies qui n'Est réglé que dans .net core
''' https://github.com/aspnet/AspNetKatana/wiki/System.Web-response-cookie-integration-issues
''' </summary>
Friend Class TsCuGestCookies
    Implements ICookieManager

    Friend Function GetRequestCookie(ByVal context As IOwinContext, ByVal key As String) As String Implements ICookieManager.GetRequestCookie
        If context Is Nothing Then
            Throw New ArgumentNullException("context")
        End If

        Dim webContext = context.[Get](Of HttpContextBase)(GetType(HttpContextBase).FullName)
        Dim cookie = webContext.Request.Cookies(key)
        Return If(cookie Is Nothing, Nothing, cookie.Value)
    End Function

    Friend Sub AppendResponseCookie(ByVal context As IOwinContext, ByVal key As String, ByVal value As String, ByVal options As CookieOptions) Implements ICookieManager.AppendResponseCookie
        If context Is Nothing Then
            Throw New ArgumentNullException("context")
        End If

        If options Is Nothing Then
            Throw New ArgumentNullException("options")
        End If

        Dim webContext = context.[Get](Of HttpContextBase)(GetType(HttpContextBase).FullName)
        Dim domainHasValue As Boolean = Not String.IsNullOrEmpty(options.Domain)
        Dim pathHasValue As Boolean = Not String.IsNullOrEmpty(options.Path)
        Dim expiresHasValue As Boolean = options.Expires.HasValue
        Dim cookie = New HttpCookie(key, value)
        If domainHasValue Then
            cookie.Domain = options.Domain
        End If

        If pathHasValue Then
            cookie.Path = options.Path
        End If

        If expiresHasValue Then
            cookie.Expires = options.Expires.Value
        End If

        If options.Secure Then
            cookie.Secure = True
        End If

        If options.HttpOnly Then
            cookie.HttpOnly = True
        End If

        webContext.Response.AppendCookie(cookie)
    End Sub

    Friend Sub DeleteCookie(ByVal context As IOwinContext, ByVal key As String, ByVal options As CookieOptions) Implements ICookieManager.DeleteCookie
        If context Is Nothing Then
            Throw New ArgumentNullException("context")
        End If

        If options Is Nothing Then
            Throw New ArgumentNullException("options")
        End If

        AppendResponseCookie(context, key, String.Empty, New CookieOptions With {
        .Path = options.Path,
        .Domain = options.Domain,
        .Expires = New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
    })
    End Sub

End Class
