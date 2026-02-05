Imports System
Imports System.Collections
Imports System.IO
Imports System.Net
Imports System.Text
Imports Rrq.InfrastructureCommune.Parametres
Imports Rrq.InfrastructureCommune.UtilitairesCommuns
Imports System.Web.Script.Serialization

Public Class TsCuDemndMajMotPasse


#Region "Variables"

    Private _DeviceVoute As String = String.Empty
    Private _ApplicationVoute As String = String.Empty

#End Region

#Region "Constantes"

    'URL_SERVICES_REST_VOUTE, TARGET_ALIAS_SERVICES_REST_VOUTE et PASSWORD_VIEW_POLICY sont, fonctionnellement parlant, des constantes.
    'Toutefois, on ne peut les déclarer avec le mot réservé 'const' puisqu'une valeur constante est défini à la
    'compilation alors que l'obtention des paramètres du fichier de config se fait plus tard (lors de l'éxécution de l'application).
    Private Shared URL_SERVICES_REST_VOUTE As String = XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N227\UrlServicesRESTVoute")
    Private Shared TARGET_ALIAS_SERVICES_REST_VOUTE As String = XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N227\TargetAliasServicesRESTVoute")
    Private Shared PASSWORD_VIEW_POLICY As String = XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N227\PasswordViewPolicy")

    Public Property DeviceVoute As String
        Get
            Return _DeviceVoute
        End Get
        Set(value As String)
            _DeviceVoute = value
        End Set
    End Property

    Public Property ApplicationVoute As String
        Get
            Return _ApplicationVoute
        End Get
        Set(value As String)
            _ApplicationVoute = value
        End Set
    End Property
#End Region

#Region "constructeurs"

    Public Sub New(ByVal DeviceVoute As String, ByVal ApplicationVoute As String)
        Me.DeviceVoute = DeviceVoute
        Me.ApplicationVoute = ApplicationVoute
    End Sub

#End Region

#Region "Méthodes publiques"


    Public Sub InscrireUsagerMotPasse(ByVal codeUsager As String, ByVal description As String, ByVal accessType As String, ByVal motPass As String)

        Dim url As String = String.Format("{0}devices.json/{1}/targetApplications/{2}/targetAccounts", URL_SERVICES_REST_VOUTE, DeviceVoute, ApplicationVoute)

        Dim verbe As String = String.Empty

        Dim parametres As String

        Dim postData As String

        verbe = "POST"

        parametres = "{{""accountName"":""{0}"",""password"":""{1}"",""attributes"":{{""descriptor1"":""{2}""}},""accessType"":""{3}"",""privileged"":""t"",""passwordViewPolicyId"":""{4}""}}"

        postData = String.Format(parametres, codeUsager, motPass, description, accessType, PASSWORD_VIEW_POLICY)

        ExecuterRequeteREST(url, verbe, postData).Close()

    End Sub

    Public Sub SupprimerUsagerMotPasse(ByVal codeUsager As String)

        'Trouver le accountID du code d'usager à supprimer
        Dim AccountID As String = ObtenirAccountID(codeUsager)

        If Not String.IsNullOrWhiteSpace(AccountID) Then
            Dim url As String = String.Format("{0}devices.json/{1}/targetApplications/{2}/targetAccounts/{3}", URL_SERVICES_REST_VOUTE, DeviceVoute, ApplicationVoute, AccountID)

            ExecuterRequeteREST(url, "DELETE").Close()
        Else
            Throw New ApplicationException(String.Format("L'utilisateur {0} à supprimer n'existe pas dans la voute de mot de passe.", codeUsager))
        End If

    End Sub

#End Region

#Region "Méthodes priéves"

    Private Function ObtenirAccountID(ByVal codeUsager As String) As String
        Dim AccountID As String = String.Empty

        'Sort by AccountName, Limit = 0 pour retourner tous les accountNames et un filtre par AccountName qui presentement ne fonctionne pas.
        Dim url As String = String.Format("{0}devices.json/{1}/targetApplications/{2}/targetAccounts?sortBy=%2BaccountName&limit=0&accountName={3}", URL_SERVICES_REST_VOUTE, DeviceVoute, ApplicationVoute, codeUsager)

        Using reponse As HttpWebResponse = ExecuterRequeteREST(url, "GET")
            Using reader As New StreamReader(reponse.GetResponseStream)

                Dim retour As String = reader.ReadToEnd

                Dim jss As New JavaScriptSerializer()
                Dim dict() As Dictionary(Of String, String) = jss.Deserialize(Of Dictionary(Of String, String)())(retour)

                For Each infoCount As Dictionary(Of String, String) In dict
                    If infoCount("accountName") = codeUsager Then
                        AccountID = infoCount("accountId")
                        Exit For
                    End If
                Next

            End Using
        End Using

        Return AccountID
    End Function

    Private Function ObtenirAPIKey(ByVal targetAlias As String) As String

        Dim demMotPasse As New TsCuDemndRecprMotPasse
        Dim codeUsagerMotPasse As TsDtCodeUsageMotPasse = demMotPasse.ObtenirCodeUsagerMotPasse(targetAlias)

        Return String.Format("{0}:{1}", codeUsagerMotPasse.CodeUsager, codeUsagerMotPasse.MotPasse)

    End Function

    Private Function ExecuterRequeteREST(ByVal url As String, ByVal httpMethod As String) As HttpWebResponse
        Return ExecuterRequeteREST(url, httpMethod, Nothing)
    End Function

    Private Function ExecuterRequeteREST(ByVal url As String, ByVal httpMethod As String, ByVal postData As String) As HttpWebResponse

        Dim requete As HttpWebRequest = HttpWebRequest.CreateHttp(url)

        Dim authInfo As String = ObtenirAPIKey(TARGET_ALIAS_SERVICES_REST_VOUTE)

        authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo))
        requete.Headers("Authorization") = "Basic " + authInfo
        requete.Method = httpMethod

        Dim dataStream As Stream = Nothing

        If Not (String.IsNullOrEmpty(postData)) Then

            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)

            requete.ContentLength = byteArray.Length

            dataStream = requete.GetRequestStream()

            dataStream.Write(byteArray, 0, byteArray.Length)
        End If

        Dim reponse As HttpWebResponse = Nothing

        Try
            reponse = CType(requete.GetResponse(), HttpWebResponse)
        Catch ex As WebException
            If ex.Response IsNot Nothing Then
                If ex.Response.ContentLength > 0 Then
                    Using streamReponse As Stream = ex.Response.GetResponseStream
                        Using reader As StreamReader = New StreamReader(streamReponse)
                            Throw New ApplicationException(reader.ReadToEnd, ex)
                        End Using
                    End Using
                End If
            Else
                Throw ex
            End If
        Finally
            If (dataStream IsNot Nothing) Then
                dataStream.Close()
                dataStream.Dispose()
            End If
        End Try

        Return reponse

    End Function


#End Region

End Class
