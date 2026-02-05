Imports Rrq.Securite.GestionAcces

Public Class frmUsager
    Public oUsager As TsCdSageUser

    Public Sub envoyerInformation(ByVal _oUsager As TsCdSageUser)
        oUsager = _oUsager
        remplirChampTexte()
        construitRelationURo()
        construitRelationURe()
    End Sub

    Private Sub remplirChampTexte()
        txtUserName.Text = oUsager.UserName
        txtPersonID.Text = oUsager.PersonID
        txtOrganisazion.Text = oUsager.Organization
        txtOrganizationType.Text = oUsager.OrganizationType
        txtUserID.Text = oUsager.UserID.ToString
    End Sub

    ''' <summary>
    ''' Ce procécus va faire le lien entre la liste des Relation Usager/Role et l'usager en cour
    ''' </summary>
    ''' <remarks>
    ''' Le lien est identifier par le "PersonID" et le "RoleName"
    ''' </remarks>
    Private Sub construitRelationURo()
        lstbRole.Items.Clear()

        Dim _oRoleCollection As TsCdSageRoleCollection = frmMain.oService.construitRelationURo(oUsager.PersonID)
        For Each c As TsCdSageRole In _oRoleCollection
            lstbRole.Items.Add(c)
        Next
    End Sub

    ''' <summary>
    ''' Ce procécus va faire le lien entre la liste des Relation Usager/Ressource et l'usager en cour
    ''' </summary>
    ''' <remarks>
    ''' Le lien est identifier par le "PersonID" et le "RessName1"
    ''' </remarks>
    Private Sub construitRelationURe()
        lstbRessource.Items.Clear()

        Dim _oRessourceCollection As TsCdSageResourceCollection = frmMain.oService.construitRelationURe(oUsager.PersonID)
        For Each c As TsCdSageResource In _oRessourceCollection
            lstbRessource.Items.Add(c)
        Next
    End Sub

    Private Sub lstbRole_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstbRole.DoubleClick
        If (lstbRole.SelectedIndex = -1) Then Return

        Dim _oInfoRole As TsCdSageRole = CType(lstbRole.SelectedItem, TsCdSageRole)

        frmRole.Show()
        frmRole.Focus()
        frmRole.envoyerInformation(_oInfoRole)

        'Me.Close()
    End Sub

    Private Sub lstbRessource_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstbRessource.DoubleClick
        If (lstbRessource.SelectedIndex = -1) Then Return

        Dim _oInfoRessource As TsCdSageResource = CType(lstbRessource.SelectedItem, TsCdSageResource)

        frmRessource.Show()
        frmRessource.Focus()
        frmRessource.envoyerInformation(_oInfoRessource)

        'Me.Close()
    End Sub
End Class
