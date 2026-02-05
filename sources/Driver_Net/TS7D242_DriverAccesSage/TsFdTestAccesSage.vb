Public Class TsFdTestAccesSage

#Region "Constantes"
    Const FORMAT_DATE As String = "yyyy-MM-dd"
#End Region

#Region "Fonctions de services"

    Private Sub Sablier()
        If Cursor = Cursors.Arrow Then
            Cursor = Cursors.WaitCursor
        Else
            Cursor = Cursors.Arrow
        End If
    End Sub

#End Region

#Region "Fonctions évènements"

    Private Sub btnConfigurations_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfigurations.Click
        Sablier()

        Dim configCollection As TsCdSageConfigurationCollc = TsBaConfigSage.data_source_get_configurations()
        txtResultat.Clear()

        For Each c As TsCdSageConfigurationFull In configCollection.Configurations
            txtResultat.AppendText(c.ConfigurationName + vbCrLf)
        Next

        Sablier()
    End Sub



    Private Sub btnRoles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRoles.Click
        Sablier()

        Dim collection As TsCdSageRoleCollection = TsBaConfigSage.cfg_get_roles(txtParam1.Text)
        txtResultat.Clear()

        txtResultat.AppendText("Voici les rôles de la configuration " + txtParam1.Text + " :" + vbCrLf)

        For Each c As TsCdSageRole In collection
            txtResultat.AppendText("- " + c.Name + vbCrLf)
        Next

        Sablier()
    End Sub

    Private Sub btnCfgUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCfgUser.Click
        Sablier()

        Dim collection As TsCdSageUserCollection = TsBaConfigSage.cfg_get_configuration_users(txtParam1.Text)
        txtResultat.Clear()

        txtResultat.AppendText("Voici les utilisateurs de la configuration " + txtParam1.Text + " :" + vbCrLf)

        For Each c As TsCdSageUser In collection
            txtResultat.AppendText("- " + c.PersonID + vbCrLf)
        Next

        Sablier()
    End Sub

    Private Sub btnCfgRessr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCfgRessr.Click
        Sablier()

        Dim collection As TsCdSageResourceCollection = TsBaConfigSage.cfg_get_configuration_Ressource(txtParam1.Text)
        txtResultat.Clear()

        txtResultat.AppendText("Voici les ressources de la configuration " + txtParam1.Text + " :" + vbCrLf)

        For Each c As TsCdSageResource In collection
            txtResultat.AppendText("- " + c.ResName1 + ", " + c.ResName2 + ", " + c.ResName3 + vbCrLf)
        Next

        Sablier()
    End Sub

    Private Sub btnDetailsConfig_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDetailsConfig.Click
        Sablier()

        Dim config As TsCdSageConfigurationFull = TsBaConfigSage.cfg_get_databases(txtParam1.Text).Configuration
        txtResultat.Clear()

        txtResultat.AppendText("Identifiant de la configuration: " + config.ConfigurationID + vbCrLf)
        txtResultat.AppendText("Nom de la configuration: " + config.ConfigurationName + vbCrLf)
        txtResultat.AppendText("Date de création: " + config.CreateDate.ToString + vbCrLf)
        txtResultat.AppendText("Date de modification: " + config.ModifyDate.ToString + vbCrLf)
        txtResultat.AppendText("Opération1: " + config.Operation1 + vbCrLf)
        txtResultat.AppendText("Propriétaire(Owner1): " + config.Owner1 + vbCrLf)
        txtResultat.AppendText("Nom de la base de données des utilisateurs(UDB): " + config.UserDBName + vbCrLf)
        txtResultat.AppendText("Nom de la base de données des ressources(RDB): " + config.DatabaseName + vbCrLf)

        Sablier()
    End Sub

    Private Sub btnLiensUserRole_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLiensUserRole.Click
        Sablier()

        Dim collection As TsCdSageUserRoleLinkCollection = TsBaConfigSage.cfg_get_user_role_links(txtParam1.Text)
        txtResultat.Clear()

        txtResultat.AppendText("Voici les liens utilisateur/rôle de la configuration " + txtParam1.Text + " (Utilisateur->Rôle):" + vbCrLf)

        For Each element As TsCdSageUserRoleLink In collection
            txtResultat.AppendText("- " + element.PersonID + " -> " + element.RoleName + vbCrLf)
        Next

        Sablier()
    End Sub

    Private Sub btnLiensRoleRole_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLiensRoleRole.Click
        Sablier()

        Dim collection As TsCdSageRoleRoleLinkCollection = TsBaConfigSage.cfg_get_role_role_links(txtParam1.Text)
        txtResultat.Clear()

        txtResultat.AppendText("Voici les liens rôle/rôle de la configuration " + txtParam1.Text + " (Rôle Enfant->Rôle Parent):" + vbCrLf)


        For Each element As TsCdSageRoleRoleLink In collection
            txtResultat.AppendText("- " + element.ChildRole + " -> " + element.ParentRole + vbCrLf)
        Next

        Sablier()
    End Sub

    Private Sub btnLiensRoleRessource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLiensRoleRessource.Click
        Sablier()

        Dim collection As TsCdSageRoleResLinkCollection = TsBaConfigSage.cfg_get_role_resource_links(txtParam1.Text)
        txtResultat.Clear()

        txtResultat.AppendText("Voici les liens rôle/ressource de la configuration " + txtParam1.Text + " (Rôle->R1/3, R2/3, R3/3):" + vbCrLf)


        For Each element As TsCdSageRoleResLink In collection
            txtResultat.AppendText("- " + element.RoleName + " -> " + element.ResName1 + ", " + element.ResName2 + ", " + element.ResName3 + vbCrLf)
        Next

        Sablier()
    End Sub

    Private Sub btnLiensUserRessr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLiensUserRessr.Click
        Sablier()

        Dim collection As TsCdSageUserResLinkCollection = TsBaConfigSage.cfg_get_user_resource_links(txtParam1.Text)
        txtResultat.Clear()

        txtResultat.AppendText("Voici les liens utilisateur/ressource de la configuration " + txtParam1.Text + " (Utilisateur->R1/3, R2/3, R3/3):" + vbCrLf)

        For Each element As TsCdSageUserResLink In collection
            txtResultat.AppendText("- " + element.PersonID + " -> " + element.ResName1 + ", " + element.ResName2 + ", " + element.ResName3 + vbCrLf)
        Next

        Sablier()
    End Sub

    Private Sub btnAjouterUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAjouterUser.Click
        Sablier()

        txtResultat.Clear()

        Try
            TsBaConfigSage.cfg_new_configuration_user(txtParam1.Text, txtParam2.Text)
            txtResultat.AppendText("L'appel a été complété sans problème.")
        Catch ex As ApplicationException
            txtResultat.AppendText("L'appel a rencontré une erreur.")
        End Try
        Sablier()
    End Sub

    Private Sub btnEffacerUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEffacerUser.Click
        Sablier()

        txtResultat.Clear()

        Try
            TsBaConfigSage.cfg_remove_configuration_user(txtParam1.Text, txtParam2.Text)
            txtResultat.AppendText("L'appel a été complété sans problème.")
        Catch ex As ApplicationException
            txtResultat.AppendText("L'appel a rencontré une erreur.")
        End Try
        Sablier()
    End Sub

    Private Sub btnEffacerRessource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEffacerRessource.Click
        Sablier()

        txtResultat.Clear()

        Try
            TsBaConfigSage.cfg_remove_configuration_resource(txtParam1.Text, txtParam2.Text, txtParam3.Text, txtParam4.Text)
            txtResultat.AppendText("L'appel a été complété sans problème.")
        Catch ex As ApplicationException
            txtResultat.AppendText("L'appel a rencontré une erreur.")
        End Try
        Sablier()
    End Sub

    Private Sub btnAjouterRessource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAjouterRessource.Click
        Sablier()

        txtResultat.Clear()

        Try
            TsBaConfigSage.cfg_new_configuration_resource(txtParam1.Text, txtParam2.Text, txtParam3.Text, txtParam4.Text)
            txtResultat.AppendText("L'appel a été complété sans problème.")
        Catch ex As ApplicationException
            txtResultat.AppendText("L'appel a rencontré une erreur.")
        End Try
        Sablier()
    End Sub

    Private Sub btnEffacerRole_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEffacerRole.Click
        Sablier()

        txtResultat.Clear()

        Try
            TsBaConfigSage.cfg_remove_configuration_role(txtParam1.Text, txtParam2.Text)
            txtResultat.AppendText("L'appel a été complété sans problème.")
        Catch ex As ApplicationException
            txtResultat.AppendText("L'appel a rencontré une erreur.")
        End Try
        Sablier()
    End Sub

    Private Sub Button19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button19.Click
        Sablier()

        txtResultat.Clear()

        Try
            TsBaConfigSage.cfg_new_configuration_role(txtParam1.Text, txtParam2.Text, txtParam3.Text, _
                txtParam4.Text, txtParam5.Text, txtParam6.Text, Date.Parse(txtParam7.Text, New System.Globalization.DateTimeFormatInfo()), txtParam8.Text, _
                txtParam9.Text, Date.Parse(txtParam10.Text), txtParam11.Text, txtParam12.Text, txtParam13.Text, Date.Parse(txtParam14.Text))
            txtResultat.AppendText("L'appel a été complété sans problème.")
        Catch ex As ApplicationException
            txtResultat.AppendText("L'appel a rencontré une erreur.")
        Catch ex As TsExcSageRoleDejaExistant
            txtResultat.AppendText("Le rôle existe déja dans la configuration.")
        Catch ex As FormatException
            txtResultat.AppendText("Le format de vos date n'est pas valide.")
        End Try
        Sablier()
    End Sub

    Private Sub btnAjoutLienUserRole_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAjoutLienUserRole.Click
        Sablier()

        txtResultat.Clear()

        Try
            TsBaConfigSage.cfg_new_user_role_link(txtParam1.Text, txtParam2.Text, txtParam3.Text)
            txtResultat.AppendText("L'appel a été complété sans problème.")
        Catch ex As ApplicationException
            txtResultat.AppendText("L'appel a rencontré une erreur.")
        End Try
        Sablier()
    End Sub

    Private Sub btnEffacerLienUserRole_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEffacerLienUserRole.Click
        Sablier()

        txtResultat.Clear()

        Try
            TsBaConfigSage.cfg_remove_user_role_link(txtParam1.Text, txtParam2.Text, txtParam3.Text)
            txtResultat.AppendText("L'appel a été complété sans problème.")
        Catch ex As ApplicationException
            txtResultat.AppendText("L'appel a rencontré une erreur.")
        End Try
        Sablier()
    End Sub

    Private Sub btnEffacerLienRoleRole_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEffacerLienRoleRole.Click
        Sablier()

        txtResultat.Clear()

        Try
            TsBaConfigSage.cfg_remove_role_role_link(txtParam1.Text, txtParam2.Text, txtParam3.Text)
            txtResultat.AppendText("L'appel a été complété sans problème.")
        Catch ex As ApplicationException
            txtResultat.AppendText("L'appel a rencontré une erreur.")
        End Try
        Sablier()
    End Sub

    Private Sub btnAjouterLienRoleRole_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAjouterLienRoleRole.Click
        Sablier()

        txtResultat.Clear()

        Try
            TsBaConfigSage.cfg_new_role_role_link(txtParam1.Text, txtParam2.Text, txtParam3.Text)
            txtResultat.AppendText("L'appel a été complété sans problème.")
        Catch ex As ApplicationException
            txtResultat.AppendText("L'appel a rencontré une erreur.")
        End Try
        Sablier()
    End Sub

    Private Sub btnEffacerLienRoleRessr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEffacerLienRoleRessr.Click
        Sablier()

        txtResultat.Clear()

        Try
            TsBaConfigSage.cfg_remove_resource_role_link(txtParam1.Text, txtParam2.Text, txtParam3.Text, txtParam4.Text, txtParam5.Text)
            txtResultat.AppendText("L'appel a été complété sans problème.")
        Catch ex As ApplicationException
            txtResultat.AppendText("L'appel a rencontré une erreur.")
        End Try
        Sablier()
    End Sub

    Private Sub btnAjouterLienRoleRessr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAjouterLienRoleRessr.Click
        Sablier()

        txtResultat.Clear()

        Try
            TsBaConfigSage.cfg_new_resource_role_link(txtParam1.Text, txtParam2.Text, txtParam3.Text, txtParam4.Text, txtParam5.Text)
            txtResultat.AppendText("L'appel a été complété sans problème.")
        Catch ex As ApplicationException
            txtResultat.AppendText("L'appel a rencontré une erreur.")
        End Try
        Sablier()
    End Sub

    Private Sub btnEffacerLienUserRessr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEffacerLienUserRessr.Click
        Sablier()

        txtResultat.Clear()

        Try
            TsBaConfigSage.cfg_remove_user_resource_link(txtParam1.Text, txtParam2.Text, txtParam3.Text, txtParam4.Text, txtParam5.Text)
            txtResultat.AppendText("L'appel a été complété sans problème.")
        Catch ex As ApplicationException
            txtResultat.AppendText("L'appel a rencontré une erreur.")
        End Try
        Sablier()
    End Sub

    Private Sub btnAjouterLienUserRessr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAjouterLienUserRessr.Click
        Sablier()

        txtResultat.Clear()

        Try
            TsBaConfigSage.cfg_new_user_resource_link(txtParam1.Text, txtParam2.Text, txtParam3.Text, txtParam4.Text, txtParam5.Text)
            txtResultat.AppendText("L'appel a été complété sans problème.")
        Catch ex As ApplicationException
            txtResultat.AppendText("L'appel a rencontré une erreur.")
        End Try
        Sablier()
    End Sub

    Private Sub btnChangeChampRole_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChangeChampRole.Click
        Sablier()

        txtResultat.Clear()
        Dim lstRole As TsCdSageRoleCollection = TsBaConfigSage.cfg_get_roles(txtParam1.Text)
        For Each r As TsCdSageRole In lstRole
            If r.Name = txtParam2.Text Then
                txtResultat.AppendText("Rôle avant transformation:" + vbCrLf)
                txtResultat.AppendText("- Id: " + r.RoleID.ToString + vbCrLf)
                txtResultat.AppendText("- Nom: " + r.Name + vbCrLf)
                txtResultat.AppendText("- Description: " + r.Description + vbCrLf)
                txtResultat.AppendText("- Organisation1: " + r.Organization + vbCrLf)
                txtResultat.AppendText("- Organisation2: " + r.Organization2 + vbCrLf)
                txtResultat.AppendText("- Organisation3: " + r.Organization3 + vbCrLf)
                txtResultat.AppendText("- Propriétaire: " + r.Owner + vbCrLf)
                txtResultat.AppendText("- Réviseur: " + r.Reviewer + vbCrLf)
                txtResultat.AppendText("- Filtre: " + r.Filter + vbCrLf)
                txtResultat.AppendText("- Type: " + r.Type + vbCrLf)
                txtResultat.AppendText("- Code d'approbation: " + r.ApproveCode + vbCrLf)
                txtResultat.AppendText("- Date d'approbation: " + r.ApproveDate.ToString("yyyy-MM-dd") + vbCrLf)
                txtResultat.AppendText("- Date de création: " + r.CreateDate.ToString("yyyy-MM-dd") + vbCrLf)
                txtResultat.AppendText("- Date d'expiration: " + r.ExpirationDate.ToString("yyyy-MM-dd") + vbCrLf)
                txtResultat.AppendText("----------------------" + vbCrLf)
                Exit For
            End If
        Next

        Try
            TsBaConfigSage.cfg_change_role_field(txtParam1.Text, txtParam2.Text, txtParam3.Text, txtParam4.Text)
            txtResultat.AppendText("L'appel a été complété sans problème." + vbCrLf)

            lstRole = TsBaConfigSage.cfg_get_roles(txtParam1.Text)
            For Each r As TsCdSageRole In lstRole
                If r.Name = txtParam2.Text Then
                    txtResultat.AppendText("----------------------" + vbCrLf)
                    txtResultat.AppendText("Rôle après transformation:" + vbCrLf)
                    txtResultat.AppendText("- Id: " + r.RoleID.ToString + vbCrLf)
                    txtResultat.AppendText("- Nom: " + r.Name + vbCrLf)
                    txtResultat.AppendText("- Description: " + r.Description + vbCrLf)
                    txtResultat.AppendText("- Organisation1: " + r.Organization + vbCrLf)
                    txtResultat.AppendText("- Organisation2: " + r.Organization2 + vbCrLf)
                    txtResultat.AppendText("- Organisation3: " + r.Organization3 + vbCrLf)
                    txtResultat.AppendText("- Propriétaire: " + r.Owner + vbCrLf)
                    txtResultat.AppendText("- Réviseur: " + r.Reviewer + vbCrLf)
                    txtResultat.AppendText("- Filtre: " + r.Filter + vbCrLf)
                    txtResultat.AppendText("- Type: " + r.Type + vbCrLf)
                    txtResultat.AppendText("- Code d'approbation: " + r.ApproveCode + vbCrLf)
                    txtResultat.AppendText("- Date d'approbation: " + r.ApproveDate.ToString("yyyy-MM-dd") + vbCrLf)
                    txtResultat.AppendText("- Date de création: " + r.CreateDate.ToString("yyyy-MM-dd") + vbCrLf)
                    txtResultat.AppendText("- Date d'expiration: " + r.ExpirationDate.ToString("yyyy-MM-dd") + vbCrLf)
                End If
            Next

        Catch ex As ApplicationException
            txtResultat.AppendText("L'appel a rencontré une erreur.")
        End Try


        Sablier()
    End Sub

    Private Sub btnUDBUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUDBUser.Click
        Sablier()

        txtResultat.Clear()

        Dim collection As TsCdSageUserCollection = TsBaConfigSage.udb_get_users(txtParam1.Text)
        txtResultat.Clear()

        txtResultat.AppendText("Voici les utilisateurs de la UDB " + txtParam1.Text + " :" + vbCrLf)

        For Each element As TsCdSageUser In collection
            txtResultat.AppendText("- " + element.PersonID + vbCrLf)
        Next

        Sablier()
    End Sub

    Private Sub btnNouveauChampUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNouveauChampUser.Click
        Sablier()

        txtResultat.Clear()
        Dim lstUser As TsCdSageUserCollection = TsBaConfigSage.udb_get_users(txtParam1.Text)
        For Each u As TsCdSageUser In lstUser
            If u.PersonID = txtParam2.Text Then
                txtResultat.AppendText("Utilisateur avant transformation:" + vbCrLf)
                txtResultat.AppendText("- Id: " + u.PersonID + vbCrLf)
                txtResultat.AppendText("- Nom: " + u.UserName + vbCrLf)
                txtResultat.AppendText("- Organisation: " + u.Organization + vbCrLf)
                txtResultat.AppendText("- Type d'organisation: " + u.OrganizationType + vbCrLf)
                txtResultat.AppendText("- Ville: " + u.Ville + vbCrLf)
                txtResultat.AppendText("- Courriel: " + u.Courriel + vbCrLf)
                txtResultat.AppendText("- Date de fin: " + u.DateFin.ToString(FORMAT_DATE) + vbCrLf)
                txtResultat.AppendText("- Prénom: " + u.Prenom + vbCrLf)
                txtResultat.AppendText("- Nom: " + u.Nom + vbCrLf)
                txtResultat.AppendText("- Date d'approbation: " + u.DateApprobation.ToString(FORMAT_DATE) + vbCrLf)
                txtResultat.AppendText("- CN: " + u.CN + vbCrLf)
                txtResultat.AppendText("- Nom de l'unité: " + u.nomUnite + vbCrLf)
                txtResultat.AppendText("----------------------" + vbCrLf)
                Exit For
            End If
        Next

        Try
            TsBaConfigSage.udb_new_user_field(txtParam1.Text, txtParam2.Text, Integer.Parse(txtParam3.Text), txtParam4.Text)
            txtResultat.AppendText("L'appel a été complété sans problème." + vbCrLf)

            lstUser = TsBaConfigSage.udb_get_users(txtParam1.Text)
            For Each u As TsCdSageUser In lstUser
                If u.PersonID = txtParam2.Text Then
                    txtResultat.AppendText("----------------------" + vbCrLf)
                    txtResultat.AppendText("Utilisateur après transformation:" + vbCrLf)
                    txtResultat.AppendText("- Id: " + u.PersonID + vbCrLf)
                    txtResultat.AppendText("- Nom: " + u.UserName + vbCrLf)
                    txtResultat.AppendText("- Organisation: " + u.Organization + vbCrLf)
                    txtResultat.AppendText("- Type d'organisation: " + u.OrganizationType + vbCrLf)
                    txtResultat.AppendText("- Ville: " + u.Ville + vbCrLf)
                    txtResultat.AppendText("- Courriel: " + u.Courriel + vbCrLf)
                    txtResultat.AppendText("- Date de fin: " + u.DateFin.ToString(FORMAT_DATE) + vbCrLf)
                    txtResultat.AppendText("- Prénom: " + u.Prenom + vbCrLf)
                    txtResultat.AppendText("- Nom: " + u.Nom + vbCrLf)
                    txtResultat.AppendText("- Date d'approbation: " + u.DateApprobation.ToString(FORMAT_DATE) + vbCrLf)
                    txtResultat.AppendText("- CN: " + u.CN + vbCrLf)
                    txtResultat.AppendText("- Nom de l'unité: " + u.nomUnite + vbCrLf)

                    Exit For
                End If
            Next

        Catch ex As ApplicationException
            txtResultat.AppendText("L'appel a rencontré une erreur.")
        End Try
        Sablier()
    End Sub

    Private Sub btnModifierChampUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModifierChampUser.Click
        Sablier()

        txtResultat.Clear()
        Dim lstUser As TsCdSageUserCollection = TsBaConfigSage.udb_get_users(txtParam1.Text)
        For Each u As TsCdSageUser In lstUser
            If u.PersonID = txtParam2.Text Then
                txtResultat.AppendText("Utilisateur avant transformation:" + vbCrLf)
                txtResultat.AppendText("- Id: " + u.PersonID + vbCrLf)
                txtResultat.AppendText("- Nom: " + u.UserName + vbCrLf)
                txtResultat.AppendText("- Organisation: " + u.Organization + vbCrLf)
                txtResultat.AppendText("- Type d'organisation: " + u.OrganizationType + vbCrLf)
                txtResultat.AppendText("- Ville: " + u.Ville + vbCrLf)
                txtResultat.AppendText("- Courriel: " + u.Courriel + vbCrLf)
                txtResultat.AppendText("- Date de fin: " + u.DateFin.ToString(FORMAT_DATE) + vbCrLf)
                txtResultat.AppendText("- Prénom: " + u.Prenom + vbCrLf)
                txtResultat.AppendText("- Nom: " + u.Nom + vbCrLf)
                txtResultat.AppendText("- Date d'approbation: " + u.DateApprobation.ToString(FORMAT_DATE) + vbCrLf)
                txtResultat.AppendText("- CN: " + u.CN + vbCrLf)
                txtResultat.AppendText("- Nom de l'unité: " + u.nomUnite + vbCrLf)
                txtResultat.AppendText("----------------------" + vbCrLf)
                Exit For
            End If
        Next

        Try
            TsBaConfigSage.udb_change_user_field(txtParam1.Text, txtParam2.Text, txtParam3.Text, txtParam4.Text, Integer.Parse(txtParam5.Text))
            txtResultat.AppendText("L'appel a été complété sans problème." + vbCrLf)

            lstUser = TsBaConfigSage.udb_get_users(txtParam1.Text)
            For Each u As TsCdSageUser In lstUser
                If u.PersonID = txtParam2.Text Then
                    txtResultat.AppendText("----------------------" + vbCrLf)
                    txtResultat.AppendText("Utilisateur après transformation:" + vbCrLf)
                    txtResultat.AppendText("- Id: " + u.PersonID + vbCrLf)
                    txtResultat.AppendText("- Nom: " + u.UserName + vbCrLf)
                    txtResultat.AppendText("- Organisation: " + u.Organization + vbCrLf)
                    txtResultat.AppendText("- Type d'organisation: " + u.OrganizationType + vbCrLf)
                    txtResultat.AppendText("- Ville: " + u.Ville + vbCrLf)
                    txtResultat.AppendText("- Courriel: " + u.Courriel + vbCrLf)
                    txtResultat.AppendText("- Date de fin: " + u.DateFin.ToString(FORMAT_DATE) + vbCrLf)
                    txtResultat.AppendText("- Prénom: " + u.Prenom + vbCrLf)
                    txtResultat.AppendText("- Nom: " + u.Nom + vbCrLf)
                    txtResultat.AppendText("- Date d'approbation: " + u.DateApprobation.ToString(FORMAT_DATE) + vbCrLf)
                    txtResultat.AppendText("- CN: " + u.CN + vbCrLf)
                    txtResultat.AppendText("- Nom de l'unité: " + u.nomUnite + vbCrLf)

                    Exit For
                End If
            Next
        Catch ex As ApplicationException
            txtResultat.AppendText("L'appel a rencontré une erreur.")
        End Try

        Sablier()
    End Sub

    Private Sub btnCreerUtilisateur_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreerUtilisateur.Click
        Sablier()

        txtResultat.Clear()

        Try
            TsBaConfigSage.udb_new_user(txtParam1.Text, txtParam2.Text, txtParam3.Text, txtParam4.Text, txtParam5.Text)
            txtResultat.AppendText("L'appel a été complété sans problème.")
        Catch ex As ApplicationException
            txtResultat.AppendText("L'appel a rencontré une erreur.")
        End Try
        Sablier()
    End Sub

    Private Sub btnRDBRessr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRDBRessr.Click
        Sablier()

        txtResultat.Clear()

        Dim collection As TsCdSageResourceCollection = TsBaConfigSage.rdb_get_resources(txtParam1.Text)
        txtResultat.Clear()

        txtResultat.AppendText("Voici les ressources de la RDB " + txtParam1.Text + " :" + vbCrLf)

        For Each element As TsCdSageResource In collection
            txtResultat.AppendText("- " + element.ResName1 + ", " + element.ResName2 + ", " + element.ResName3 + vbCrLf)
        Next

        Sablier()
    End Sub

    Private Sub btnModifierChampRessr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModifierChampRessr.Click
        Sablier()

        txtResultat.Clear()
        Dim lstRessource As TsCdSageResourceCollection = TsBaConfigSage.rdb_get_resources(txtParam1.Text)
        For Each r As TsCdSageResource In lstRessource
            If r.ResName1 = txtParam2.Text And r.ResName2 = txtParam3.Text And r.ResName3 = txtParam4.Text Then
                txtResultat.AppendText("Ressource avant transformation:" + vbCrLf)
                txtResultat.AppendText("- Nom ressource 1/3: " + r.ResName1 + vbCrLf)
                txtResultat.AppendText("- Nom ressource 2/3: " + r.ResName2 + vbCrLf)
                txtResultat.AppendText("- Nom ressource 3/3: " + r.ResName3 + vbCrLf)
                txtResultat.AppendText("- Champ 1: " + r.FieldValue1 + vbCrLf)
                txtResultat.AppendText("- Champ 2: " + r.FieldValue2 + vbCrLf)
                txtResultat.AppendText("- Champ 3: " + r.FieldValue3 + vbCrLf)
                txtResultat.AppendText("- Champ 4: " + r.FieldValue4 + vbCrLf)
                txtResultat.AppendText("- Champ 5: " + r.FieldValue5 + vbCrLf)
                txtResultat.AppendText("----------------------" + vbCrLf)
                Exit For
            End If
        Next

        Try
            TsBaConfigSage.rdb_change_resource_field(txtParam1.Text, txtParam2.Text, txtParam3.Text, txtParam4.Text, txtParam5.Text, txtParam6.Text, Integer.Parse(txtParam7.Text))
            txtResultat.AppendText("L'appel a été complété sans problème." + vbCrLf)

            lstRessource = TsBaConfigSage.rdb_get_resources(txtParam1.Text)
            For Each r As TsCdSageResource In lstRessource
                If r.ResName1 = txtParam2.Text And r.ResName2 = txtParam3.Text And r.ResName3 = txtParam4.Text Then
                    txtResultat.AppendText("----------------------" + vbCrLf)
                    txtResultat.AppendText("Ressource après transformation:" + vbCrLf)
                    txtResultat.AppendText("- Nom ressource 1/3: " + r.ResName1 + vbCrLf)
                    txtResultat.AppendText("- Nom ressource 2/3: " + r.ResName2 + vbCrLf)
                    txtResultat.AppendText("- Nom ressource 3/3: " + r.ResName3 + vbCrLf)
                    txtResultat.AppendText("- Champ 1: " + r.FieldValue1 + vbCrLf)
                    txtResultat.AppendText("- Champ 2: " + r.FieldValue2 + vbCrLf)
                    txtResultat.AppendText("- Champ 3: " + r.FieldValue3 + vbCrLf)
                    txtResultat.AppendText("- Champ 4: " + r.FieldValue4 + vbCrLf)
                    txtResultat.AppendText("- Champ 5: " + r.FieldValue5 + vbCrLf)
                    Exit For
                End If
            Next
        Catch ex As ApplicationException
            txtResultat.AppendText("L'appel a rencontré une erreur.")
        End Try

        Sablier()
    End Sub

    Private Sub btnCreerRessource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreerRessource.Click
        Sablier()

        txtResultat.Clear()

        Try
            TsBaConfigSage.rdb_new_resource(txtParam1.Text, txtParam2.Text, txtParam3.Text, txtParam4.Text)
            txtResultat.AppendText("L'appel a été complété sans problème.")
        Catch ex As ApplicationException
            txtResultat.AppendText("L'appel a rencontré une erreur.")
        End Try
        Sablier()
    End Sub

    Private Sub cmdPermettrePartageUnsafe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPermettrePartageUnsafe.Click
        TsBaSetupAccesSage.PermettrePartageUnsafe()
    End Sub

#End Region

End Class
