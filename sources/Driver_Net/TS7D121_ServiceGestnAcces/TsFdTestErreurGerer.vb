Imports Rrq.InfrastructureCommune.Parametres
Imports System.Data.SqlClient

Public Class TsFdTestErreurGerer

    Private Sub btnPage2Precedant_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrecedant.Click
        TsFdFonctionsIndependantes.Show()
        Me.Close()
    End Sub

    Private Sub btnUtilisateurManquant_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUtilisateurManquant.Click
        Try
            Dim demndCreation As New TsCdDemndCreationModif()

            TsCaServiceGestnAcces.DemanderCreation(demndCreation, DateAdd(DateInterval.Day, 1, Date.Now))
        Catch ex As ApplicationException
            txtResultat.Text = ex.Message
        End Try
    End Sub

    Private Sub btnDestrcutionSansUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDestrcutionSansUser.Click
        Try
            Dim demnd As New TsCdDemandeDestruction("100")

        Catch ex As ApplicationException
            txtResultat.Text = ex.Message
            Try
                Dim demnd2 As New TsCdDemandeDestruction()
                TsCaServiceGestnAcces.DemanderDestruction(demnd2, DateAdd(DateInterval.Day, 1, Date.Now))
            Catch ex1 As ApplicationException
                txtResultat.AppendText(vbCrLf)
                txtResultat.AppendText(ex1.Message)
            End Try
        End Try
    End Sub

    Private Sub btnModUserInexistant_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModUserInexistant.Click
        Try
            Dim demnd As New TsCdDemndCreationModif("100")
        Catch ex As ApplicationException
            txtResultat.Text = ex.Message
        End Try
    End Sub

    Private Sub btnAjoutRoleInexistant_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAjoutRoleInexistant.Click
        Try
            Dim demndCreation As New TsCdDemndCreationModif()

            demndCreation.AjouterRole("ah")
        Catch ex As ApplicationException
            txtResultat.Text = ex.Message
        End Try
    End Sub

    Private Sub btnMauvaiseModif_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMauvaiseModif.Click
        Try
            Dim demnd As New TsCdDemndCreationModif()

            demnd.AjouterRole("REO_4340_SSDS")
            demnd.ModifierRole("REO_4340_SSDS", True)
        Catch ex As ApplicationException
            txtResultat.Text = ex.Message
        End Try
    End Sub

    Private Sub btnLectureSeule_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLectureSeule.Click
        Try
            Dim demnd As New TsCdDemndCreationModif("T207185")

            demnd.Utilisateur.Nom = "Martin2"
        Catch ex As ApplicationException
            txtResultat.Text = ex.Message
        End Try
    End Sub

    Private Sub btnTropFichiers_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTropFichiers.Click
        Dim cheminDepot As String = Rrq.InfrastructureCommune.Parametres.XuCuConfiguration.ValeurSysteme("TS7", "TS7N121\CheminDepotHeat")
        Try

            If My.Computer.FileSystem.FileExists(cheminDepot) = False Then
                My.Computer.FileSystem.CreateDirectory(cheminDepot)
            End If
            For i As Integer = 1 To 100
                Dim fichierSortie As New IO.StreamWriter(cheminDepot + "\Auth" + i.ToString + ".txt")
                fichierSortie.Close()
            Next

            Dim demndCreation As New TsCdDemndCreationModif()

            Dim utililisateur As New TsCdUtilisateur
            With utililisateur
                .ApprobationAccepter = False
                .Courriel = "Martin@rrq.qc"
                .FinPrevue = False
                .NomComplet = "Martin Bellemare"
                .Nom = "Bellemare"
                .Prenom = "Martin"
                .NoUniteAdmin = "1724"
                .Ville = "Québec"
            End With
            demndCreation.Utilisateur = utililisateur

            DemanderCreation(demndCreation, DateAdd(DateInterval.Day, 1, Date.Now))
        Catch ex As ApplicationException
            txtResultat.Text = ex.Message

            For i As Integer = 1 To 100
                My.Computer.FileSystem.DeleteFile(cheminDepot + "\Auth" + i.ToString + ".txt")
            Next
        End Try
    End Sub

    Private Sub btnErreurBD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnErreurBD.Click
        Dim connectionXU As New Rrq.InfrastructureCommune.UtilitairesCommuns.XuCuAccesBd
        Dim connectionSQLServeur As String = XuCuConfiguration.ValeurSysteme("TS7", "TS7\TS7N121\ConnectionSQLServeur")
        Dim connectionSQLBaseDonnees As String = XuCuConfiguration.ValeurSysteme("TS7", "TS7\TS7N121\ConnectionSQLBaseDonnees")
        Dim connection As SqlClient.SqlConnection = connectionXU.ObtenirConnexionSqlAuthentifiee(connectionSQLServeur, connectionSQLBaseDonnees)
        Dim transaction As SqlTransaction = connection.BeginTransaction

        Try
            Dim sqlD1 As New SqlClient.SqlCommand("Delete from dbo.CallLog where CallID = '100' or CallID = '101'", connection, transaction)
            Dim sqlD2 As New SqlClient.SqlCommand("Delete from dbo.Detail where CallID = '100' or CallID = '101'", connection, transaction)

            Dim sql1 As New SqlClient.SqlCommand("Insert Into dbo.Detail (CallID) Values ('100')", connection, transaction)
            Dim sql2 As New SqlClient.SqlCommand("Insert Into dbo.Detail (CallID) Values ('101')", connection, transaction)
            Dim sql3 As New SqlClient.SqlCommand("Insert Into dbo.CallLog (CallID,GuidRRQ) Values ('101','f31b2a28-daae-4bff-8276-a350ac65adad')", connection, transaction)
            Dim sql4 As New SqlClient.SqlCommand("Insert Into dbo.CallLog (CallID,GuidRRQ) Values ('100','f31b2a28-daae-4bff-8276-a350ac65adad')", connection, transaction)

            sqlD1.ExecuteNonQuery()
            sqlD2.ExecuteNonQuery()
            sql1.ExecuteNonQuery()
            sql2.ExecuteNonQuery()
            sql3.ExecuteNonQuery()
            sql4.ExecuteNonQuery()
            transaction.Commit()

            Dim demndCreation As New TsCdDemndCreationModif()

            Dim utililisateur As New TsCdUtilisateur
            With utililisateur
                .ApprobationAccepter = False
                .Courriel = "Martin@rrq.qc"
                .FinPrevue = False
                .NomComplet = "Martin Bellemare"
                .Nom = "Bellemare"
                .Prenom = "Martin"
                .NoUniteAdmin = "1724"
                .Ville = "Québec"
            End With
            demndCreation.Utilisateur = utililisateur

            DemanderCreation(demndCreation, DateAdd(DateInterval.Day, 1, Date.Now), "f31b2a28-daae-4bff-8276-a350ac65adad")
        Catch ex As ApplicationException
            txtResultat.Text = ex.Message
        End Try
        connection.Close()
        connection.Dispose()
        connection = Nothing
    End Sub
End Class