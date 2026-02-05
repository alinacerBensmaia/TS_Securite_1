Imports System
Imports System.IO
Imports Cspmclientc64
Imports Rrq.InfrastructureCommune.Parametres

Public Class TsCuDemndRecprMotPasse

#Region "constructeurs"

    Public Sub New()
    End Sub

#End Region

#Region "Méthodes publiques"

    Public Function ObtenirMotPasse(ByVal targetAlias As String) As String

        Return ObtenirCodeUsagerMotPasse(targetAlias).MotPasse

    End Function

    Public Function ObtenirCodeUsager(ByVal targetAlias As String) As String

        Return ObtenirCodeUsagerMotPasse(targetAlias).CodeUsager

    End Function

    Public Function ObtenirCodeUsagerMotPasse(ByVal targetAlias As String) As TsDtCodeUsageMotPasse
        Dim codeUsagerMotPasse As New TsDtCodeUsageMotPasse
        Dim bypassCache As String = True.ToString
        Dim cliOpt As String = String.Empty

        Dim proxyA2A As ccspmclientc64 = New ccspmclientc64

        Dim retourDuProxy As Int32

        retourDuProxy = proxyA2A.retrieveCredentials(targetAlias, bypassCache, cliOpt)

        If (retourDuProxy = 400) Then
            codeUsagerMotPasse.CodeUsager = proxyA2A.getUtf16UserId()
            codeUsagerMotPasse.MotPasse = proxyA2A.getPassword()
        Else
            Throw New ApplicationException(String.Format("Erreur lors de l'obtention du compte et mot de passe associés au targetAlias {0}. Code de Rerour : {1} {2}", targetAlias, retourDuProxy, proxyA2A.getMessage()))
        End If

        Return codeUsagerMotPasse

    End Function


#End Region

End Class
