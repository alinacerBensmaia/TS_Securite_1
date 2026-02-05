Imports Rrq.Securite.GestionAcces

Public Class frmRessource
    Public oRessource As TsCdSageResource

    Public Sub envoyerInformation(ByVal _oRessource As TsCdSageResource)
        oRessource = _oRessource
        remplirChampTexte()
        construitRelationReU()
        construitRelationReRo()
    End Sub

    Private Sub remplirChampTexte()
        txtResName1.Text = oRessource.ResName1
        txtResName2.Text = oRessource.ResName2
        txtResName3.Text = oRessource.ResName3
        txtResName4.Text = oRessource.ResName4
        txtFieldValue1.Text = oRessource.FieldValue1
        txtFieldValue2.Text = oRessource.FieldValue2
        txtFieldValue3.Text = oRessource.FieldValue3
        txtFieldValue4.Text = oRessource.FieldValue4
        txtFieldValue5.Text = oRessource.FieldValue5
    End Sub

    ''' <summary>
    ''' Ce procécus va faire le lien entre la liste des Relations Usager/Ressource et la Ressource en cour
    ''' </summary>
    ''' <remarks>
    ''' Le lien est identifier par le "ResName1" et le "PersonID"
    ''' </remarks>
    Private Sub construitRelationReU()
        lstbRelationURe.Items.Clear()

        Dim _oUsagerCollection As TsCdSageUserCollection = frmMain.oService.construitRelationReU(oRessource.ResName1)
        For Each c As TsCdSageUser In _oUsagerCollection
            lstbRelationURe.Items.Add(c)
        Next
    End Sub

    ''' <summary>
    ''' Ce procécus va faire le lien entre la liste des Relations Usager/Ressource et la Ressource en cour
    ''' </summary>
    ''' <remarks>
    ''' Le lien est identifier par le "ResName1" et le "PersonID"
    ''' </remarks>
    Private Sub construitRelationReRo()
        lstbRelationRoRe.Items.Clear()

        Dim _oRoleCollection As TsCdSageRoleCollection = frmMain.oService.construitRelationReRo(oRessource.ResName1)
        For Each c As TsCdSageRole In _oRoleCollection
            lstbRelationRoRe.Items.Add(c)
        Next
    End Sub

    Private Sub lstbRelationURe_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstbRelationURe.DoubleClick
        If (lstbRelationURe.SelectedIndex = -1) Then Return

        Dim _oInfoUsager As TsCdSageUser = CType(lstbRelationURe.SelectedItem, TsCdSageUser)

        frmUsager.Show()
        frmUsager.Focus()
        frmUsager.envoyerInformation(_oInfoUsager)
    End Sub

    Private Sub lstbRelationRoRe_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstbRelationRoRe.DoubleClick
        If (lstbRelationRoRe.SelectedIndex = -1) Then Return

        Dim _oInfoRole As TsCdSageRole = CType(lstbRelationRoRe.SelectedItem, TsCdSageRole)

        frmRole.Show()
        frmRole.Focus()
        frmRole.envoyerInformation(_oInfoRole)
    End Sub
End Class