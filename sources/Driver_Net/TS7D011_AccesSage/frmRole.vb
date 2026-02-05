Imports Rrq.Securite.GestionAcces

Public Class frmRole

    Public oRole As TsCdSageRole

    Public Sub envoyerInformation(ByVal _oRole As TsCdSageRole)
        oRole = _oRole
        remplirChampTexte()
        construitRelationURo()
        construitRelationRoRe()
        construitRelationParent()
        construitRelationEnfant()
    End Sub

    Private Sub remplirChampTexte()
        txtCodeApproval.Text = oRole.ApproveCode
        txtDate.Text = oRole.CreateDate.ToString
        txtDescription.Text = oRole.Description
        txtFilter.Text = oRole.Filter
        txtID.Text = oRole.RoleID.ToString
        txtName.Text = oRole.Name
        txtOrganisation.Text = construireTextOrganisations()
        txtOwner.Text = oRole.Owner
        txtReview.Text = oRole.Reviewer
        txtType.Text = oRole.Type
        txtDateApprove.Text = oRole.ApproveDate.ToString
        txtDateExpiration.Text = oRole.ExpirationDate.ToString
    End Sub

    Private Function construireTextOrganisations() As String
        Dim _sRet As String
        _sRet = ""
        _sRet = _sRet + oRole.Organization + vbCrLf
        _sRet = _sRet + oRole.Organization2 + vbCrLf
        _sRet = _sRet + oRole.Organization3 + vbCrLf
        Return _sRet
    End Function

    ''' <summary>
    ''' Ce procécus va faire le lien entre la liste des Relations Usager/Role et le Role en cour
    ''' </summary>
    ''' <remarks>
    ''' Le lien est identifier par le "PersonID" et le "RoleName"
    ''' </remarks>
    Private Sub construitRelationURo()
        lstbRelationURo.Items.Clear()

        Dim _oUsagerCollection As TsCdSageUserCollection = frmMain.oService.construitRelationRoU(oRole.Name)
        For Each c As TsCdSageUser In _oUsagerCollection
            lstbRelationURo.Items.Add(c)
        Next
    End Sub

    ''' <summary>
    ''' Ce procécus va faire le lien entre la liste des RelationsRole /Ressource et le Role en cour
    ''' </summary>
    ''' <remarks>
    ''' Le lien est identifier par le "RoleName" et le "ResName1"
    ''' </remarks>
    Private Sub construitRelationRoRe()
        lstbRelationRoRe.Items.Clear()

        Dim _oRessourceCollection As TsCdSageResourceCollection = frmMain.oService.construitRelationRoRe(oRole.Name)
        For Each c As TsCdSageResource In _oRessourceCollection
            lstbRelationRoRe.Items.Add(c)
        Next
    End Sub

    ''' <summary>
    ''' Ce procécus va faire le lien Parent entre la liste des Relations Role/Role et le Role en cour
    ''' </summary>
    ''' <remarks>
    ''' Le lien est identifier par le "RoleName" et le "RoleName"
    ''' </remarks>
    Private Sub construitRelationParent()
        lstbRelationParent.Items.Clear()

        Dim _oRoleCollection As TsCdSageRoleCollection = frmMain.oService.construitRelationRoRoParent(oRole.Name)
        For Each c As TsCdSageRole In _oRoleCollection
            lstbRelationParent.Items.Add(c)
        Next
    End Sub
    ''' <summary>
    ''' Ce procécus va faire le lien Fils entre la liste des Relations Role/Role et le Role en cour
    ''' </summary>
    ''' <remarks>
    ''' Le lien est identifier par le "RoleName" et le "RoleName"
    ''' </remarks>
    Private Sub construitRelationEnfant()
        lstbRelationFils.Items.Clear()

        Dim _oRoleCollection As TsCdSageRoleCollection = frmMain.oService.construitRelationRoRoEnfant(oRole.Name)
        For Each c As TsCdSageRole In _oRoleCollection
            lstbRelationFils.Items.Add(c)
        Next
    End Sub

    Private Sub lstbRelationURo_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstbRelationURo.DoubleClick
        If (lstbRelationURo.SelectedIndex = -1) Then Return

        Dim _oInfoUsager As TsCdSageUser = CType(lstbRelationURo.SelectedItem, TsCdSageUser)

        frmUsager.Show()
        frmUsager.Focus()
        frmUsager.envoyerInformation(_oInfoUsager)
    End Sub

    Private Sub lstbRelationParent_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstbRelationParent.DoubleClick
        If (lstbRelationParent.SelectedIndex = -1) Then Return

        Dim _oInfoRole As TsCdSageRole = CType(lstbRelationParent.SelectedItem, TsCdSageRole)

        Me.envoyerInformation(_oInfoRole)
    End Sub

    Private Sub lstbRelationFils_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstbRelationFils.DoubleClick
        If (lstbRelationFils.SelectedIndex = -1) Then Return

        Dim _oInfoRole As TsCdSageRole = CType(lstbRelationFils.SelectedItem, TsCdSageRole)

        Me.envoyerInformation(_oInfoRole)
    End Sub

    Private Sub lstbRelationRoRe_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstbRelationRoRe.DoubleClick
        If (lstbRelationRoRe.SelectedIndex = -1) Then Return

        Dim _oInfoRessource As TsCdSageResource = CType(lstbRelationRoRe.SelectedItem, TsCdSageResource)

        frmRessource.Show()
        frmRessource.Focus()
        frmRessource.envoyerInformation(_oInfoRessource)
    End Sub
End Class