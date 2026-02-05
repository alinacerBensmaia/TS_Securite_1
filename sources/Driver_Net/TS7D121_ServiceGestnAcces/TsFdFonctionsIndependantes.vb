'TODO ASSOCIÉ LE TOOL TIPS AU BOUTONS
'TODO FAIRE LES INTÉRACTIONS DES BOUTONS
Public Class TsFdFonctionsIndependantes
#Region "Constantes"
    Private Const FORMAT_DATE As String = "yyyy-MM-dd"
#End Region

    Private Sub btnLienUserRole_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLienUserRole.Click
        Try
            CurseurFlip()
            Dim lstAssignation As List(Of TsCdAssignationRole) = TsCaServiceGestnAcces.ObtenirAssignationsRole(txtParam1.Text)
            CurseurFlip()

            txtResultat.Text = "Rôle(s) de l'utilisateur trouvé(s):"
            txtResultat.AppendText(vbCrLf)

            For Each a As TsCdAssignationRole In lstAssignation
                txtResultat.AppendText("- " + a.ID)
                txtResultat.AppendText(vbCrLf)
            Next
        Catch ex As TsExcErreurGeneral
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Erreur")
            ReinitialiserCurseur()
        End Try
    End Sub

    Private Sub btnEquipeAdmin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEquipeAdmin.Click
        Try
            CurseurFlip()
            Dim lstEquipe As List(Of TsCdEquipe) = TsCaServiceGestnAcces.ObtenirEquipesUniteAdmin(txtParam1.Text)
            CurseurFlip()

            txtResultat.Text = "Équipe(s) de l'unité administrative trouvée(s):"
            txtResultat.AppendText(vbCrLf)

            For Each equipe As TsCdEquipe In lstEquipe
                txtResultat.AppendText("- " + equipe.IDRole)
                txtResultat.AppendText(vbCrLf)
            Next
        Catch ex As TsExcErreurGeneral
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Erreur")
            ReinitialiserCurseur()
        End Try

    End Sub

    Private Sub btnEquipesUtilsateur_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEquipesUtilsateur.Click
        Try
            CurseurFlip()
            Dim lstEquipe As List(Of TsCdEquipe) = TsCaServiceGestnAcces.ObtenirEquipesUtilisateur(txtParam1.Text)
            CurseurFlip()

            txtResultat.Text = "Équipe(s) de l'utilisateur trouvé(s):"
            txtResultat.AppendText(vbCrLf)

            For Each equipe As TsCdEquipe In lstEquipe
                txtResultat.AppendText("- " + equipe.IDRole)
                txtResultat.AppendText(vbCrLf)
            Next
        Catch ex As TsExcErreurGeneral
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Erreur")
            ReinitialiserCurseur()
        End Try
    End Sub

    Private Sub btnListeUA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnListeUA.Click
        Try
            CurseurFlip()
            Dim lstUA As List(Of TsCdUniteAdministrative) = TsCaServiceGestnAcces.ObtenirListeUnitesAdmin()
            CurseurFlip()

            txtResultat.Text = "Liste des unités administratives:"
            txtResultat.AppendText(vbCrLf)

            For Each UA As TsCdUniteAdministrative In lstUA
                txtResultat.AppendText("- " + UA.IDRole)
                txtResultat.AppendText(vbCrLf)
            Next
        Catch ex As TsExcErreurGeneral
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Erreur")
            ReinitialiserCurseur()
        End Try
    End Sub

    Private Sub btnRoleEquipe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRoleEquipe.Click
        Try
            CurseurFlip()
            Dim role As TsCdRole = TsCaServiceGestnAcces.ObtenirRoleEquipe(txtParam1.Text)
            CurseurFlip()

            txtResultat.Text = "Informations de l'équipe:"
            txtResultat.AppendText(vbCrLf)

            txtResultat.AppendText("   ID: " + role.ID)
            txtResultat.AppendText(vbCrLf)
            txtResultat.AppendText("   Nom: " + role.Nom)
            txtResultat.AppendText(vbCrLf)
            txtResultat.AppendText("   Description: " + role.Description)
            txtResultat.AppendText(vbCrLf)
            txtResultat.AppendText("   Est organisationnel?: " + role.Organisationnel.ToString)
            txtResultat.AppendText(vbCrLf)
            txtResultat.AppendText("   Équipe particulière: " + role.Particulier.ToString)
            txtResultat.AppendText(vbCrLf)

        Catch ex As TsExcErreurGeneral
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Erreur")
            ReinitialiserCurseur()
        End Try
    End Sub

    Private Sub btnRolesUA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRolesUA.Click
        Try
            CurseurFlip()
            Dim lstRole As List(Of TsCdRole) = TsCaServiceGestnAcces.ObtenirRolesUniteAdmin(txtParam1.Text)
            CurseurFlip()

            txtResultat.Text = "Rôle(s) de l'unité administrative trouvé(s):"
            txtResultat.AppendText(vbCrLf)

            For Each r As TsCdRole In lstRole
                txtResultat.AppendText("- " + r.ID)
                txtResultat.AppendText(vbCrLf)
            Next
        Catch ex As TsExcErreurGeneral
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Erreur")
            ReinitialiserCurseur()
        End Try
    End Sub

    Private Sub btnUniteAdministratives_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUniteAdministratives.Click
        Try
            CurseurFlip()
            Dim lstUA As List(Of TsCdUniteAdministrative) = TsCaServiceGestnAcces.ObtenirUnitesAdmin(txtParam1.Text)
            CurseurFlip()

            txtResultat.Text = "Liste des unités administratives:"
            txtResultat.AppendText(vbCrLf)

            For Each ua As TsCdUniteAdministrative In lstUA
                txtResultat.AppendText("- " + ua.Nom)
                txtResultat.AppendText(vbCrLf)
            Next
        Catch ex As TsExcErreurGeneral
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Erreur")
            ReinitialiserCurseur()
        End Try

    End Sub

    Private Sub btnUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUser.Click

        Try
            CurseurFlip()
            Dim user As TsCdUtilisateur = TsCaServiceGestnAcces.ObtenirUtilisateur(txtParam1.Text)
            CurseurFlip()

            txtResultat.Text = "Les informations de l'utilisateur:"
            txtResultat.AppendText(vbCrLf)

            txtResultat.AppendText("   ID: " + user.ID)
            txtResultat.AppendText(vbCrLf)
            txtResultat.AppendText("   Nom complet: " + user.NomComplet)
            txtResultat.AppendText(vbCrLf)
            txtResultat.AppendText("   Nom: " + user.Nom)
            txtResultat.AppendText(vbCrLf)
            txtResultat.AppendText("   Prénom: " + user.Prenom)
            txtResultat.AppendText(vbCrLf)
            txtResultat.AppendText("   Unité administrative: " + user.NoUniteAdmin)
            txtResultat.AppendText(vbCrLf)
            txtResultat.AppendText("   Courriel: " + user.Courriel)
            txtResultat.AppendText(vbCrLf)
            txtResultat.AppendText("   Ville: " + user.Ville)
            txtResultat.AppendText(vbCrLf)
            If user.ApprobationAccepter = True Then
                txtResultat.AppendText("   Date d'approbation: " + user.DateApprobation.ToString(FORMAT_DATE))
                txtResultat.AppendText(vbCrLf)
            End If
            If user.FinPrevue = True Then
                txtResultat.AppendText("   Date de fin de contrat: " + user.DateFin.ToString(FORMAT_DATE))
                txtResultat.AppendText(vbCrLf)
            End If
        Catch ex As TsExcErreurGeneral
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Erreur")
            ReinitialiserCurseur()
        End Try
    End Sub

    Private Sub btnRoles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRoles.Click
        Try
            CurseurFlip()
            Dim lstRole As New ArrayList(TsCaServiceGestnAcces.RechercherRole(txtParam1.Text))
            CurseurFlip()

            txtResultat.Text = "Rôle(s) trouvé(s):"
            txtResultat.AppendText(vbCrLf)

            For Each r As TsCdRole In lstRole
                txtResultat.AppendText("- " + r.ID)
                txtResultat.AppendText(vbCrLf)
            Next
        Catch ex As TsExcErreurGeneral
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Erreur")
            ReinitialiserCurseur()
        End Try
    End Sub

    Private Sub btnUsers_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUsers.Click
        Try
            CurseurFlip()
            Dim lstUser As List(Of TsCdUtilisateur) = TsCaServiceGestnAcces.RechercherUtilisateur(txtParam1.Text)
            CurseurFlip()

            txtResultat.Text = "Utilisateur(s) trouvé(s):"
            txtResultat.AppendText(vbCrLf)

            For Each u As TsCdUtilisateur In lstUser
                Dim nom As String
                If u.NomComplet <> "" Then
                    nom = u.NomComplet
                Else
                    nom = u.Prenom + " " + u.Nom
                End If

                txtResultat.AppendText("- (" + u.ID + ") " + nom)
                txtResultat.AppendText(vbCrLf)
            Next
        Catch ex As TsExcErreurGeneral
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Erreur")
            ReinitialiserCurseur()
        End Try
    End Sub
#Region "Fonctions de services"

    Private Sub CurseurFlip()
        If Cursor = Cursors.WaitCursor Then
            Cursor = Cursors.Arrow
        Else
            Cursor = Cursors.WaitCursor
        End If
    End Sub

    Private Sub ReinitialiserCurseur()
        If Cursor = Cursors.WaitCursor Then
            Cursor = Cursors.Arrow
        End If
    End Sub

#End Region

    Private Sub btnRafraichirBuffer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRafraichirBuffer.Click
        TsBaConfigSage.ClearCache()
    End Sub

    Private Sub btnDemandes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDemandes.Click
        TsFdTestServiceGestnAcces.Show()
        Me.Close()
    End Sub

    Private Sub btnErreurs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnErreurs.Click
        TsFdTestErreurGerer.Show()
        Me.Close()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            CurseurFlip()
            
            Dim maxIter As Integer = 700
            Dim t0 As DateTime = DateTime.Now
            Dim t1 As TimeSpan
            Dim tmin As TimeSpan = TimeSpan.MaxValue
            Dim tmax As TimeSpan = TimeSpan.MinValue

            Dim dictioEtape As Dictionary(Of Integer, List(Of TimeSpan)) = New Dictionary(Of Integer, List(Of TimeSpan))
            Dim nomEtape As String() = {"RechercherUtilisateur", "ObtenirUtilisateur", "ObtenirUnitesAdmin", _
                                        "ObtenirAssignationsRole", "ObtenirEquipesUtilisateur", "RechercherRole", _
                                        "ObtenirListeUnitesAdmin", "ObtenirRolesUniteAdmin", "ObtenirEquipesUniteAdmin", _
                                        "ObtenirRoleEquipe", "AreLinkedUserRole"}

            Dim lstUser As List(Of TsCdUtilisateur) = Nothing
            Dim lstRole As ArrayList = Nothing
            Dim lstUniteAdmin As List(Of TsCdUniteAdministrative) = Nothing
            Dim lstEquipe As List(Of TsCdEquipe) = Nothing
            Dim lstIndex As Integer = 0
            Dim numEtape As Integer = 0
            Dim i As Integer = 0

            While i < maxIter
                Dim tdebiteration As DateTime = DateTime.Now

                Select Case numEtape
                    Case 0
                        If lstUser Is Nothing Then
                            lstUser = TsCaServiceGestnAcces.RechercherUtilisateur(txtParam1.Text)
                        Else
                            numEtape += 1
                            lstIndex = 0
                            Continue While
                        End If
                    Case 1
                        If lstIndex < lstUser.Count Then
                            TsCaServiceGestnAcces.ObtenirUtilisateur(lstUser(lstIndex).ID)
                            lstIndex += 1
                        Else
                            numEtape += 1
                            lstIndex = 0
                            Continue While
                        End If
                    Case 2
                        If lstIndex < lstUser.Count Then
                            TsCaServiceGestnAcces.ObtenirUnitesAdmin(lstUser(lstIndex).ID)
                            lstIndex += 1
                        Else
                            numEtape += 1
                            lstIndex = 0
                            Continue While
                        End If
                    Case 3
                        If lstIndex < lstUser.Count Then
                            TsCaServiceGestnAcces.ObtenirAssignationsRole(lstUser(lstIndex).ID)
                            lstIndex += 1
                        Else
                            numEtape += 1
                            lstIndex = 0
                            Continue While
                        End If
                    Case 4
                        If lstIndex < lstUser.Count Then
                            TsCaServiceGestnAcces.ObtenirEquipesUtilisateur(lstUser(lstIndex).ID)
                            lstIndex += 1
                        Else
                            numEtape += 1
                            lstIndex = 0
                            Continue While
                        End If
                    Case 5
                        If lstRole Is Nothing Then
                            lstRole = TsCaServiceGestnAcces.RechercherRole(txtParam1.Text)
                        Else
                            numEtape += 1
                            lstIndex = 0
                            Continue While
                        End If
                    Case 6
                        If lstUniteAdmin Is Nothing Then
                            lstUniteAdmin = TsCaServiceGestnAcces.ObtenirListeUnitesAdmin
                        Else
                            numEtape += 1
                            lstIndex = 0
                            Continue While
                        End If
                    Case 7
                        If lstIndex < lstUniteAdmin.Count Then
                            TsCaServiceGestnAcces.ObtenirRolesUniteAdmin(lstUniteAdmin(lstIndex).IDRole)
                            lstIndex += 1
                        Else
                            numEtape += 1
                            lstIndex = 0
                            Continue While
                        End If
                    Case 8
                        If lstIndex < lstUniteAdmin.Count Then
                            lstEquipe = TsCaServiceGestnAcces.ObtenirEquipesUniteAdmin(lstUniteAdmin(lstIndex).IDRole)
                            lstIndex += 1
                        Else
                            numEtape += 1
                            lstIndex = 0
                            Continue While
                        End If
                    Case 9
                        If lstIndex < lstEquipe.Count Then
                            TsCaServiceGestnAcces.ObtenirRoleEquipe(lstEquipe(lstIndex).IDRole)
                            lstIndex += 1
                        Else
                            numEtape += 1
                            lstIndex = 0
                            Continue While
                        End If
                    Case 10
                        If lstIndex < lstUser.Count AndAlso lstIndex < lstRole.Count Then
                            TsBaConfigSage.AreLinkedUserRole("Test volume1-2-3", lstUser(lstIndex).Nom, DirectCast(lstRole(lstIndex), TsCdRole).Nom)
                            lstIndex += 1
                        Else
                            numEtape += 1
                            lstIndex = 0
                            Continue While
                        End If
                    Case Else
                        'on recommence
                        numEtape = 0
                        lstIndex = 0
                        lstUser = Nothing
                        lstRole = Nothing
                        lstUniteAdmin = Nothing
                        lstEquipe = Nothing
                        Continue While
                End Select

                ' Calcule les temps moyens par iteration
                Dim tfiniteration As TimeSpan = DateTime.Now - tdebiteration
                If tfiniteration < tmin Then
                    tmin = tfiniteration
                End If
                If tfiniteration > tmax Then
                    tmax = tfiniteration
                End If

                If Not dictioEtape.ContainsKey(numEtape) Then
                    dictioEtape(numEtape) = New List(Of TimeSpan)
                End If
                dictioEtape(numEtape).Add(tfiniteration)

                i += 1
            End While

            t1 = DateTime.Now - t0
            txtResultat.Text = "Nombres d'itérations : " & i
            txtResultat.AppendText(vbCrLf)
            txtResultat.AppendText("Temps total : " & t1.TotalSeconds & "s")
            txtResultat.AppendText(vbCrLf)
            txtResultat.AppendText("Temps moyen par iteration : " & t1.TotalMilliseconds / i & "ms")
            txtResultat.AppendText(vbCrLf)
            txtResultat.AppendText("Temps maximun par iteration : " & tmax.TotalMilliseconds & "ms")
            txtResultat.AppendText(vbCrLf)
            txtResultat.AppendText("Temps minimun par iteration : " & tmin.TotalMilliseconds & "ms")
            txtResultat.AppendText(vbCrLf)

            txtResultat.AppendText("Temps par méthode : ")
            txtResultat.AppendText(vbCrLf)
            For Each item As KeyValuePair(Of Integer, List(Of TimeSpan)) In dictioEtape
                txtResultat.AppendText("Méthode testée : " & nomEtape(item.Key))
                txtResultat.AppendText(vbCrLf)
                Dim moy As Double = Aggregate x In item.Value Into Average(x.TotalMilliseconds)
                Dim min_iter As Double = Aggregate x In item.Value Into Min(x.TotalMilliseconds)
                Dim max_iter As Double = Aggregate x In item.Value Into Max(x.TotalMilliseconds)
                txtResultat.AppendText(vbTab & "Nombre d'iteration : " & item.Value.Count)
                txtResultat.AppendText(vbCrLf)
                txtResultat.AppendText(vbTab & "Temps moyen par iteration : " & moy & "ms")
                txtResultat.AppendText(vbCrLf)
                txtResultat.AppendText(vbTab & "Temps minimum par iteration : " & min_iter & "ms")
                txtResultat.AppendText(vbCrLf)
                txtResultat.AppendText(vbTab & "Temps maximun par iteration : " & max_iter & "ms")
                txtResultat.AppendText(vbCrLf)
            Next

            CurseurFlip()
        Catch ex As TsExcErreurGeneral
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Erreur")
            ReinitialiserCurseur()
        End Try
    End Sub
End Class