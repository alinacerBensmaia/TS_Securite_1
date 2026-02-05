Public Class TsFdTestDiffSage

#Region "Variables privées"

    Private diffSage As TsCaDiffSage

#End Region

#Region "Fonctions de services"
    Private Sub CurseurFlip()
        If Cursor = Cursors.WaitCursor Then
            Cursor = Cursors.Arrow
        Else
            Cursor = Cursors.WaitCursor
        End If
    End Sub

    Private Sub VerifierObjetCreer()
        Dim lstBouton() As Windows.Forms.Button = {btnAttrbRessr, btnAttrbRole, btnAttrbUser, btnDiffRessource, _
            btnDiffRole, btnDiffUser, btnLienRoleRole, btnLienRoRe, btnLiensUReRec, btnLienUReDir, btnLienURo}
        Dim etat As Boolean

        If diffSage Is Nothing Then
            etat = False
        Else
            etat = True
        End If

        For Each b As Button In lstBouton
            b.Enabled = etat
        Next
    End Sub
#End Region

#Region "Fonctions Évènements"

    Private Sub btnLiensUReRec_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLiensUReRec.Click
        CurseurFlip()
        Dim lstAjout As New List(Of TsCdConnxUserRessr)
        Dim lstSuppr As New List(Of TsCdConnxUserRessr)

        diffSage.ObtnrDiffrUilisateurRessourceRecurcif(txtParam1.Text, lstAjout, lstSuppr)

        txtResultat.Text = "Liste des différences des liens Utilisateur/Ressource récurcif"
        txtResultat.AppendText(vbCrLf)
        txtResultat.AppendText("Voici les ajouts:")
        txtResultat.AppendText(vbCrLf)

        For Each a As TsCdConnxUserRessr In lstAjout
            txtResultat.AppendText("- " + a.CodeUtilisateur + " -> (" + a.NomRessource + ", " + a.CatgrRessource + ")")
            txtResultat.AppendText(vbCrLf)
        Next

        txtResultat.AppendText("Voici les suppressions:")
        txtResultat.AppendText(vbCrLf)

        For Each s As TsCdConnxUserRessr In lstSuppr
            txtResultat.AppendText("- " + s.CodeUtilisateur + " -> (" + s.NomRessource + ", " + s.CatgrRessource + ")")
            txtResultat.AppendText(vbCrLf)
        Next
        CurseurFlip()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        CurseurFlip()
        Try
            diffSage = New TsCaDiffSage(txtParam1.Text, txtParam2.Text)
            txtResultat.Text = "Création de l'objet interactif a réussi."
        Catch ex As TsExcConfigurationInexistante
            txtResultat.Text = "Échec. L'une des deux configurations n'a pas été trouvée dans sage."
            CurseurFlip()
        End Try
        VerifierObjetCreer()
        CurseurFlip()
    End Sub

    Private Sub btnValiderExistence_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnValiderExistence.Click
        CurseurFlip()
        If TsCaDiffSage.ValiderExistenceConfig(txtParam1.Text) = False Then
            txtResultat.Text = "La configuration n'existe pas."
        Else
            txtResultat.Text = "La configuration existe."
        End If
        CurseurFlip()
    End Sub

    Private Sub btnValiderIntegriter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnValiderIntegriter.Click
        CurseurFlip()
        If TsCaDiffSage.ValiderIntegriteConfig(txtParam1.Text) = False Then
            txtResultat.Text = "L'enrichissement de la configuration n'est pas conforme avec le programme."
        Else
            txtResultat.Text = "L'enrichissement de la configuration est conforme."
        End If
        CurseurFlip()
    End Sub

    Private Sub btnLienUReDir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLienUReDir.Click
        CurseurFlip()
        Dim lstAjout As New List(Of TsCdConnxUserRessr)
        Dim lstSuppr As New List(Of TsCdConnxUserRessr)

        diffSage.ObtnrDiffrUtilisateurRessourceDirect(txtParam1.Text, lstAjout, lstSuppr)

        txtResultat.Text = "Liste des différences des liens Utilisateur/Ressource direct"
        txtResultat.AppendText(vbCrLf)
        txtResultat.AppendText("Voici les ajouts:")
        txtResultat.AppendText(vbCrLf)

        For Each a As TsCdConnxUserRessr In lstAjout
            txtResultat.AppendText("- " + a.CodeUtilisateur + " -> (" + a.NomRessource + ", " + a.CatgrRessource + ")")
            txtResultat.AppendText(vbCrLf)
        Next

        txtResultat.AppendText("Voici les suppressions:")
        txtResultat.AppendText(vbCrLf)

        For Each s As TsCdConnxUserRessr In lstSuppr
            txtResultat.AppendText("- " + s.CodeUtilisateur + " -> (" + s.NomRessource + ", " + s.CatgrRessource + ")")
            txtResultat.AppendText(vbCrLf)
        Next
        CurseurFlip()
    End Sub

    Private Sub btnLienURo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLienURo.Click
        CurseurFlip()
        Dim lstAjout As New List(Of TsCdConnxUserRole)
        Dim lstSuppr As New List(Of TsCdConnxUserRole)

        diffSage.ObtnrDiffrUtilisateurRole(lstAjout, lstSuppr)

        txtResultat.Text = "Liste des différences des liens Utilisateur/Rôle"
        txtResultat.AppendText(vbCrLf)
        txtResultat.AppendText("Voici les ajouts:")
        txtResultat.AppendText(vbCrLf)

        For Each a As TsCdConnxUserRole In lstAjout
            txtResultat.AppendText("- " + a.CodeUtilisateur + " -> " + a.NomRole)
            txtResultat.AppendText(vbCrLf)
        Next

        txtResultat.AppendText("Voici les suppressions:")
        txtResultat.AppendText(vbCrLf)

        For Each s As TsCdConnxUserRole In lstSuppr
            txtResultat.AppendText("- " + s.CodeUtilisateur + " -> " + s.NomRole)
            txtResultat.AppendText(vbCrLf)
        Next
        CurseurFlip()
    End Sub

    Private Sub btnLienRoleRole_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLienRoleRole.Click
        CurseurFlip()
        Dim lstAjout As New List(Of TsCdConnxRoleRole)
        Dim lstSuppr As New List(Of TsCdConnxRoleRole)

        diffSage.ObtnrDiffrRoleRole(lstAjout, lstSuppr)

        txtResultat.Text = "Liste des différences des liens Rôle supérieur/Sous rôle"
        txtResultat.AppendText(vbCrLf)
        txtResultat.AppendText("Voici les ajouts:")
        txtResultat.AppendText(vbCrLf)

        For Each a As TsCdConnxRoleRole In lstAjout
            txtResultat.AppendText("- " + a.NomRoleSup + " -> " + a.NomSousRole)
            txtResultat.AppendText(vbCrLf)
        Next

        txtResultat.AppendText("Voici les suppressions:")
        txtResultat.AppendText(vbCrLf)

        For Each s As TsCdConnxRoleRole In lstSuppr
            txtResultat.AppendText("- " + s.NomRoleSup + " -> " + s.NomSousRole)
            txtResultat.AppendText(vbCrLf)
        Next
        CurseurFlip()
    End Sub

    Private Sub BtnLienRoRe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLienRoRe.Click
        CurseurFlip()
        Dim lstAjout As New List(Of TsCdConnxRoleRessr)
        Dim lstSuppr As New List(Of TsCdConnxRoleRessr)

        diffSage.ObtnrDiffrRoleRessource(txtParam1.Text, lstAjout, lstSuppr)

        txtResultat.Text = "Liste des différences des liens Rôle/Ressource"
        txtResultat.AppendText(vbCrLf)
        txtResultat.AppendText("Voici les ajouts:")
        txtResultat.AppendText(vbCrLf)

        For Each a As TsCdConnxRoleRessr In lstAjout
            txtResultat.AppendText("- " + a.NomRole + " -> (" + a.NomRessource + ", " + a.CatgrRessource + ")")
            txtResultat.AppendText(vbCrLf)
        Next

        txtResultat.AppendText("Voici les suppressions:")
        txtResultat.AppendText(vbCrLf)

        For Each s As TsCdConnxRoleRessr In lstSuppr
            txtResultat.AppendText("- " + s.NomRole + " -> (" + s.NomRessource + ", " + s.CatgrRessource + ")")
            txtResultat.AppendText(vbCrLf)
        Next
        CurseurFlip()
    End Sub

    Private Sub btnDiffUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDiffUser.Click
        CurseurFlip()
        Dim lstAjout As New List(Of TsCdConnxUser)
        Dim lstSuppr As New List(Of TsCdConnxUser)

        diffSage.ObtnrDiffrUtilisateur(lstAjout, lstSuppr)

        txtResultat.Text = "Liste des différences des utilisateurs"
        txtResultat.AppendText(vbCrLf)
        txtResultat.AppendText("Voici les ajouts:")
        txtResultat.AppendText(vbCrLf)

        For Each a As TsCdConnxUser In lstAjout
            txtResultat.AppendText("- " + a.CodeUtilisateur)
            txtResultat.AppendText(vbCrLf)
        Next

        txtResultat.AppendText("Voici les suppressions:")
        txtResultat.AppendText(vbCrLf)

        For Each s As TsCdConnxUser In lstSuppr
            txtResultat.AppendText("- " + s.CodeUtilisateur)
            txtResultat.AppendText(vbCrLf)
        Next
        CurseurFlip()
    End Sub

    Private Sub btnDiffRole_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDiffRole.Click
        CurseurFlip()
        Dim lstAjout As New List(Of TsCdConnxRole)
        Dim lstSuppr As New List(Of TsCdConnxRole)

        diffSage.ObtnrDiffrRole(lstAjout, lstSuppr)

        txtResultat.Text = "Liste des différences des rôles"
        txtResultat.AppendText(vbCrLf)
        txtResultat.AppendText("Voici les ajouts:")
        txtResultat.AppendText(vbCrLf)

        For Each a As TsCdConnxRole In lstAjout
            txtResultat.AppendText("- " + a.NomRole)
            txtResultat.AppendText(vbCrLf)
        Next

        txtResultat.AppendText("Voici les suppressions:")
        txtResultat.AppendText(vbCrLf)

        For Each s As TsCdConnxRole In lstSuppr
            txtResultat.AppendText("- " + s.NomRole)
            txtResultat.AppendText(vbCrLf)
        Next
        CurseurFlip()
    End Sub

    Private Sub btnDiffRessource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDiffRessource.Click
        CurseurFlip()
        Dim lstAjout As New List(Of TsCdConnxRessr)
        Dim lstSuppr As New List(Of TsCdConnxRessr)

        diffSage.ObtnrDiffrRessource(txtParam1.Text, lstAjout, lstSuppr)

        txtResultat.Text = "Liste des différences des ressources"
        txtResultat.AppendText(vbCrLf)
        txtResultat.AppendText("Voici les ajouts:")
        txtResultat.AppendText(vbCrLf)

        For Each a As TsCdConnxRessr In lstAjout
            txtResultat.AppendText("- " + a.NomRessource + ", " + a.CatgrRessource)
            txtResultat.AppendText(vbCrLf)
        Next

        txtResultat.AppendText("Voici les suppressions:")
        txtResultat.AppendText(vbCrLf)

        For Each s As TsCdConnxRessr In lstSuppr
            txtResultat.AppendText("- " + s.NomRessource + ", " + s.CatgrRessource)
            txtResultat.AppendText(vbCrLf)
        Next
        CurseurFlip()
    End Sub

    Private Sub btnAttrbUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAttrbUser.Click
        CurseurFlip()
        Dim lstAjout As New List(Of TsCdConnxUserAttrb)
        Dim lstSuppr As New List(Of TsCdConnxUserAttrb)

        diffSage.ObtnrDiffrAttrbUser(lstAjout, lstSuppr)

        txtResultat.Text = "Liste des différences des attributs des utilisateurs"
        txtResultat.AppendText(vbCrLf)
        txtResultat.AppendText("Voici les ajouts:")
        txtResultat.AppendText(vbCrLf)

        For Each a As TsCdConnxUserAttrb In lstAjout
            txtResultat.AppendText("- """ + a.CodeUtilisateur + """" + vbCrLf + "  Champ: """ + a.NomAttrb + """" + vbCrLf + "  Valeur: """ + a.Valeur + """")
            txtResultat.AppendText(vbCrLf)
        Next

        txtResultat.AppendText("Voici les suppressions:")
        txtResultat.AppendText(vbCrLf)

        For Each s As TsCdConnxUserAttrb In lstSuppr
            txtResultat.AppendText("- """ + s.CodeUtilisateur + """" + vbCrLf + "  Champ: """ + s.NomAttrb + """" + vbCrLf + "  Valeur: """ + s.Valeur + """")
            txtResultat.AppendText(vbCrLf)
        Next
        CurseurFlip()
    End Sub

    Private Sub btnAttrbRole_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAttrbRole.Click
        CurseurFlip()
        Dim lstAjout As New List(Of TsCdConnxRoleAttrb)
        Dim lstSuppr As New List(Of TsCdConnxRoleAttrb)

        diffSage.ObtnrDiffrAttrbRole(lstAjout, lstSuppr)

        txtResultat.Text = "Liste des différences des attributs des rôles"
        txtResultat.AppendText(vbCrLf)
        txtResultat.AppendText("Voici les ajouts:")
        txtResultat.AppendText(vbCrLf)

        For Each a As TsCdConnxRoleAttrb In lstAjout
            txtResultat.AppendText("- """ + a.NomRole + """" + vbCrLf + "  Champ: """ + a.NomAttrb + """" + vbCrLf + "  Valeur: """ + a.Valeur + """")
            txtResultat.AppendText(vbCrLf)
        Next

        txtResultat.AppendText("Voici les suppressions:")
        txtResultat.AppendText(vbCrLf)

        For Each s As TsCdConnxRoleAttrb In lstSuppr
            txtResultat.AppendText("- """ + s.NomRole + """" + vbCrLf + "  Champ: """ + s.NomAttrb + """" + vbCrLf + "  Valeur: """ + s.Valeur + """")
            txtResultat.AppendText(vbCrLf)
        Next
        CurseurFlip()
    End Sub

    Private Sub btnAttrbRessr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAttrbRessr.Click
        CurseurFlip()
        Dim lstAjout As New List(Of TsCdConnxRessrAttrb)
        Dim lstSuppr As New List(Of TsCdConnxRessrAttrb)

        diffSage.ObtnrDiffrAttrbRessr(txtParam1.Text, lstAjout, lstSuppr)

        txtResultat.Text = "Liste des différences des attributs des ressources"
        txtResultat.AppendText(vbCrLf)
        txtResultat.AppendText("Voici les ajouts:")
        txtResultat.AppendText(vbCrLf)

        For Each a As TsCdConnxRessrAttrb In lstAjout
            txtResultat.AppendText("- """ + a.NomRessource + ", " + a.CatgrRessource + """" + vbCrLf + "  Champ: """ + a.NomAttrb + """" + vbCrLf + "  Valeur: """ + a.Valeur + """")
            txtResultat.AppendText(vbCrLf)
        Next

        txtResultat.AppendText("Voici les suppressions:")
        txtResultat.AppendText(vbCrLf)

        For Each s As TsCdConnxRessrAttrb In lstSuppr
            txtResultat.AppendText("- """ + s.NomRessource + ", " + s.CatgrRessource + """" + vbCrLf + "  Champ: """ + s.NomAttrb + """" + vbCrLf + "  Valeur: """ + s.Valeur + """")
            txtResultat.AppendText(vbCrLf)
        Next
        CurseurFlip()
    End Sub

    Private Sub btnListeConfig_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnListeConfig.Click
        CurseurFlip()
        Dim lstConfig As List(Of String) = TsCaDiffSage.ObtenirListeConfig()

        txtResultat.Text = "Voici les configurations disponibles:"
        txtResultat.AppendText(vbCrLf)

        For Each c As String In lstConfig
            txtResultat.AppendText("- " + c)
            txtResultat.AppendText(vbCrLf)
        Next
        CurseurFlip()
    End Sub

    Private Sub TsFdTestDiffSage_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        VerifierObjetCreer()
    End Sub

#End Region

End Class
