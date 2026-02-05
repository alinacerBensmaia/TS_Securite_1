Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Data
Imports System.Data.SqlClient
Imports System.Transactions
Imports Rrq.InfrastructureCommune.UtilitairesCommuns
Imports Rrq.InfrastructureCommune.Parametres
Imports Rrq.InfrastructureCommune.ScenarioTransactionnel

Public Class TsCdAutoriserAOS

#Region "--- Constantes ---"
    Protected Const CHEMIN_CONFIG_TYPE_CONNEXION As String = "XU5\ConnexionsBD\{0}\{1}"

    Protected Const SQL_OBTENIR_SYSTEMES As String = "SELECT CO_UTL_SEC_INT_TS, CO_APP_SEC_INT_TS  FROM TS2.SIAOSTS WHERE CO_APP_SEC_INT_TS like @CodeApp  "

    Protected Const SQL_OBTENIR_TRACE As String = "SELECT CO_UTL_SEC_INT_TS, CO_APP_SEC_INT_TS FROM TS2.TRAOSTS WHERE CO_APP_SEC_INT_TS like @CodeApp  "

    Private Const SQL_INSERT As String = "BEGIN TRAN " &
        "If Not EXISTS(Select * FROM TS2.TRAOSTS WHERE CO_UTL_SEC_INT_TS = @CodUtil And CO_APP_SEC_INT_TS = @CodeApp) " &
        "BEGIN " &
        "    INSERT INTO [TS2].[TRAOSTS] ( " &
        "    [CO_UTL_SEC_INT_TS]" &
        "    ,[CO_APP_SEC_INT_TS]" &
        "    ) VALUES (" &
        "    @CodUtil" &
        "    ,@CodeApp" &
        "    )" &
        "End " &
        "COMMIT TRAN"

#End Region

#Region "--- Variables ---"
    Private Shared _HtCodesSystemes As Hashtable
    Private Shared _DateExpiration As DateTime
    Private Shared _Verrou As New Object

    Private Shared _TsDtTraceAOS As New List(Of TsDtTraceAOS)
#End Region



    ''' <summary>
    ''' Cette fonction permet de vérifier si l'Utilisateur est autorisé à ce code de système
    ''' </summary>
    ''' <param name="Utilisateur">Compte de l'Utilisateur</param>
    ''' <param name="CodeSysteme">Code de système ex: XY3</param>
    ''' <returns>True si l'utilisateur (compte du pool appelant) peut accéder au code système en cours</returns>
    ''' <remarks></remarks>
    Public Shared Function EstAutoriseA(ByVal Utilisateur As String, ByVal CodeSysteme As String) As Boolean

        'On valide que la table existe ou que la date d'expiration n'est pas atteinte
        If _HtCodesSystemes Is Nothing OrElse _DateExpiration < DateTime.Now Then
            ObtenirSystemeEtSousSystemes(CodeSysteme)
        End If

        Try
            AjouterTrace(Utilisateur.Split("\"c).Last(), CodeSysteme)

        Catch ex As XuExcEErrValidation
            XuCuGestionEvent.AjouterEvenmSpecifique(XuGeJournalEvenement.XuGeJeInstallationSTCM,
                                               XuGeTypeEvenement.XuGeTeErreur, 1,
                                               ex.MsgErreur.NumMessage, ex.Source, ex.StackTrace, "")
        End Try


        Return _HtCodesSystemes.ContainsKey(Utilisateur & CodeSysteme) OrElse _HtCodesSystemes.ContainsKey(Utilisateur & CodeSysteme.Substring(0, 2) & "*")

    End Function

    ''' <summary>
    ''' Cette méthode permet d'obtenir tous les sous systèmes pour un code de système
    ''' </summary>
    ''' <param name="CodeSysteme">Code de système ex: XY3</param>
    ''' <remarks></remarks>
    Private Shared Sub ObtenirSystemeEtSousSystemes(ByVal CodeSysteme As String)

        Dim paramCodeApp As SqlParameter = New SqlParameter("@CodeApp", SqlDbType.VarChar)
        paramCodeApp.Direction = ParameterDirection.Input
        paramCodeApp.Value = CodeSysteme.Substring(0, 2) & "%"

        Dim htTempo As New Hashtable

        Using scope As TransactionScope = New TransactionScope(TransactionScopeOption.Suppress)
            Dim nomDatasource As String = XuCuConfiguration.ObtenirValeurSystemeOptionnelle("XU5", String.Format(CHEMIN_CONFIG_TYPE_CONNEXION, "SQL_Securite", "NomDatasource"))
            Dim nomBaseDonnees As String = XuCuConfiguration.ObtenirValeurSystemeOptionnelle("XU5", String.Format(CHEMIN_CONFIG_TYPE_CONNEXION, "SQL_Securite", "NomBaseDonnees"))

            Dim connXu As XuCuAccesBd = New XuCuAccesBd()

            Using connection As SqlConnection = connXu.ObtenirConnexionSqlAuthentifiee(nomDatasource, nomBaseDonnees)
                Dim command As SqlCommand = connection.CreateCommand()
                command.Parameters.Add(paramCodeApp)
                command.CommandText = SQL_OBTENIR_SYSTEMES
                Using reader As SqlDataReader = command.ExecuteReader()
                    While reader.Read()
                        'Inscrire les comptes + les systèmes
                        htTempo.Add(Convert.ToString(reader(0)) & Convert.ToString(reader(1)), Nothing)
                    End While

                    reader.Close()
                End Using

            End Using

            scope.Complete()
        End Using

        SyncLock _Verrou
            If _HtCodesSystemes Is Nothing Then
                _HtCodesSystemes = New Hashtable
            Else
                _HtCodesSystemes.Clear()
            End If

            _HtCodesSystemes = htTempo
        End SyncLock

        ' On peut aussi bypasser le délai en faisant un recycle du pool appelé
        Dim tempsExpiration = XuCuConfiguration.ObtenirValeurSystemeOptionnelle("XU5", "XU5N150\TempsExpiration")

        If tempsExpiration Is Nothing Then
            _DateExpiration = DateTime.Now.AddMinutes(60)
        Else
            _DateExpiration = DateTime.Now.AddMinutes(Convert.ToInt32(tempsExpiration))
        End If

    End Sub

    Private Shared Sub AjouterTrace(ByVal Utilisateur As String, ByVal CodeSysteme As String)

        If (_TsDtTraceAOS Is Nothing) Then
            ChargerCache(CodeSysteme)
        End If

        If Not (_TsDtTraceAOS.Any(Function(x) x.CodUtlSecIntTs = Utilisateur And x.CodAppSecIntTs = CodeSysteme)) Then
            SyncLock _Verrou
                _TsDtTraceAOS.Add(New TsDtTraceAOS() With {
                .CodUtlSecIntTs = Utilisateur,
                .CodAppSecIntTs = CodeSysteme
            })
            End SyncLock

            EnregistrerTraces(Utilisateur, CodeSysteme)
        End If


    End Sub

    Private Shared Sub ChargerCache(ByVal CodeSysteme As String)

        Dim htTempo As New List(Of TsDtTraceAOS)

        Dim paramCodeApp As SqlParameter = New SqlParameter("@CodeApp", SqlDbType.VarChar)
        paramCodeApp.Direction = ParameterDirection.Input
        paramCodeApp.Value = CodeSysteme
        paramCodeApp.Value = CodeSysteme.Substring(0, 2) & "%"

        Using scope As TransactionScope = New TransactionScope(TransactionScopeOption.Suppress)
            Dim nomDatasource As String = XuCuConfiguration.ObtenirValeurSystemeOptionnelle("XU5", String.Format(CHEMIN_CONFIG_TYPE_CONNEXION, "SQL_Securite", "NomDatasource"))
            Dim nomBaseDonnees As String = XuCuConfiguration.ObtenirValeurSystemeOptionnelle("XU5", String.Format(CHEMIN_CONFIG_TYPE_CONNEXION, "SQL_Securite", "NomBaseDonnees"))

            Dim connXu As XuCuAccesBd = New XuCuAccesBd()

            Using connection As SqlConnection = connXu.ObtenirConnexionSqlAuthentifiee(nomDatasource, nomBaseDonnees)
                Dim command As SqlCommand = connection.CreateCommand()
                command.Parameters.Add(paramCodeApp)
                command.CommandText = SQL_OBTENIR_TRACE
                Using reader As SqlDataReader = command.ExecuteReader()
                    While reader.Read()
                        'Inscrire les comptes + les systèmes
                        htTempo.Add(New TsDtTraceAOS() With {
                            .CodUtlSecIntTs = Convert.ToString(reader(0)),
                            .CodAppSecIntTs = Convert.ToString(reader(1))
                        })

                    End While

                    reader.Close()
                End Using
            End Using
            scope.Complete()
        End Using

        SyncLock _Verrou
            If _TsDtTraceAOS Is Nothing Then
                _TsDtTraceAOS = New List(Of TsDtTraceAOS)
            Else
                _TsDtTraceAOS.Clear()
            End If

            _TsDtTraceAOS = htTempo
        End SyncLock

    End Sub

    Private Shared Sub EnregistrerTraces(ByVal Utilisateur As String, ByVal CodeSysteme As String)


        Dim paramCodeUtil As SqlParameter = New SqlParameter("@CodUtil", SqlDbType.VarChar)
        paramCodeUtil.Direction = ParameterDirection.Input
        paramCodeUtil.Value = Utilisateur

        Dim paramCodeApp As SqlParameter = New SqlParameter("@CodeApp", SqlDbType.VarChar)
        paramCodeApp.Direction = ParameterDirection.Input
        paramCodeApp.Value = CodeSysteme

        Using scope As TransactionScope = New TransactionScope(TransactionScopeOption.Suppress)
            Dim nomDatasource As String = XuCuConfiguration.ObtenirValeurSystemeOptionnelle("XU5", String.Format(CHEMIN_CONFIG_TYPE_CONNEXION, "SQL_Securite", "NomDatasource"))
            Dim nomBaseDonnees As String = XuCuConfiguration.ObtenirValeurSystemeOptionnelle("XU5", String.Format(CHEMIN_CONFIG_TYPE_CONNEXION, "SQL_Securite", "NomBaseDonnees"))

            Dim connXu As XuCuAccesBd = New XuCuAccesBd()

            Using connection As SqlConnection = connXu.ObtenirConnexionSqlAuthentifiee(nomDatasource, nomBaseDonnees)
                Dim command As SqlCommand = connection.CreateCommand()
                command.Parameters.Add(paramCodeUtil)
                command.Parameters.Add(paramCodeApp)
                command.CommandText = SQL_INSERT
                command.ExecuteNonQuery()
            End Using
            scope.Complete()
        End Using


    End Sub

End Class
