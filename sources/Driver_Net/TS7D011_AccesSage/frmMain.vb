Imports Rrq.Securite.GestionAcces



Public Class frmMain

#Region "Public Vars"
    Public oService As Service_TS7D011
#End Region

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        oService = New Service_TS7D011
    End Sub

#Region "Evenement"
    '''<summary>
    ''' Évenement Click btnAction1. Demande les usagers à sage et les inscrit dans une lstBox
    ''' </summary>
    Private Sub btnAction1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAction1.Click
        Dim _oUsagers As TsCdSageUserCollection = TsCuAccesSage.Utilisateurs

        blstUsager.Items.Clear()
        For Each c As TsCdSageUser In _oUsagers
            blstUsager.Items.Add(c)
        Next
    End Sub
    ''' <summary>
    ''' Évenement Click btnAction2. Demande les roles à sage et les inscrit dans une lstBox
    ''' </summary>
    Private Sub btnAction2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAction2.Click
        Dim _oRoles As TsCdSageRoleCollection = oService.getRoleCollection

        blstRole.Items.Clear()
        For Each c As TsCdSageRole In _oRoles
            blstRole.Items.Add(c)
        Next
    End Sub
    ''' <summary>
    ''' Évenement Click btnAction3. Demande les ressources à sage et les inscrit dans une lstBox
    ''' </summary>
    Private Sub btnAction3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAction3.Click
        Dim _oRessources As TsCdSageResourceCollection = oService.getRessourceCollection

        blstRessource.Items.Clear()
        For Each c As TsCdSageResource In _oRessources
            blstRessource.Items.Add(c)
        Next
    End Sub
    ''' <summary>
    ''' Évenement Click btnXmlReader1. Demande les Ressources à sage en format XML dans une string
    ''' </summary>
    Private Sub btnXmlReader1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnXmlReader1.Click
        Dim _sRessource As String = cfg_get_configuration_Ressource_string(oService.config)
        txtXML1.Text = _sRessource
    End Sub

    ''' <summary>
    ''' Évenement Click btnXmlReader2. Demande les Roles à sage en format XML dans une string
    ''' </summary>
    Private Sub btnXmlReader2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnXMLReader2.Click
        Dim _sRoles As String = cfg_get_roles_string(oService.config)
        txtXML1.Text = _sRoles
    End Sub
    ''' <summary>
    ''' Évenement Click btnXmlReader3. Demande les Usagers à sage en format XML dans une string
    ''' </summary>
    Private Sub btnXLMReader3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnXLMReader3.Click
        Dim _sUsager As String = cfg_get_configuration_users_string(oService.config)
        txtXML1.Text = _sUsager
    End Sub
    ''' <summary>
    ''' Évenement DoubleClick blstUsager. Affiche les informations sur les Usagers
    ''' </summary>
    Private Sub blstUsager_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles blstUsager.DoubleClick
        If (blstUsager.SelectedIndex = -1) Then Return

        Dim _oInfoUsager As TsCdSageUser = CType(blstUsager.SelectedItem, TsCdSageUser)

        frmUsager.Show()
        frmUsager.Focus()
        frmUsager.envoyerInformation(_oInfoUsager)
    End Sub
    ''' <summary>
    ''' Évenement DoubleClick blstRole. Affiche les informations sur les Rôles
    ''' </summary>
    Private Sub blstRole_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles blstRole.DoubleClick
        If (blstRole.SelectedIndex = -1) Then Return

        Dim _oInfoRole As TsCdSageRole = CType(blstRole.SelectedItem, TsCdSageRole)

        frmRole.Show()
        frmRole.Focus()
        frmRole.envoyerInformation(_oInfoRole)
    End Sub
    ''' <summary>
    ''' Évenement DoubleClick blstRole. Affiche les informations sur les Ressources
    ''' </summary>
    Private Sub blstRessource_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles blstRessource.DoubleClick
        If (blstRessource.SelectedIndex = -1) Then Return

        Dim _oInfoRessource As TsCdSageResource = CType(blstRessource.SelectedItem, TsCdSageResource)

        frmRessource.Show()
        frmRessource.Focus()
        frmRessource.envoyerInformation(_oInfoRessource)
    End Sub

    Private Sub btnRelationURo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRelationURo.Click
        Dim _sRelationURo As String = cfg_get_user_role_links_string(Service_TS7D011.sDEFAULT_CONFIG)
        txtXML1.Text = _sRelationURo
    End Sub

    Private Sub btnRelationURe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRelationURe.Click
        Dim _sRelationURe As String = cfg_get_user_resource_links_string(Service_TS7D011.sDEFAULT_CONFIG)
        txtXML1.Text = _sRelationURe
    End Sub

    Private Sub btnRelationRoRe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRelationRoRe.Click
        Dim _sRelationRoRe As String = cfg_get_role_resource_links_string(Service_TS7D011.sDEFAULT_CONFIG)
        txtXML1.Text = _sRelationRoRe
    End Sub

    Private Sub btnRelationRoRo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRelationRoRo.Click
        Dim _sRelationRoRo As String = cfg_get_role_role_links_string(Service_TS7D011.sDEFAULT_CONFIG)
        txtXML1.Text = _sRelationRoRo
    End Sub

#End Region

#Region "Vieux Code"
    '    Private Sub load_UsagerCollection()
    '        If oUsagerCollection Is Nothing Then
    '            txtXML1.AppendText("Collection Usager Loaded" + vbCrLf)
    '            oUsagerCollection = cfg_get_configuration_users("AD_TSS_ 43xx 4 juin 2008 avec roles")
    '        End If
    '    End Sub

    '    Private Sub load_RoleCollection()
    '        If oRoleCollection Is Nothing Then
    '            txtXML1.AppendText("Collection Role Loaded" + vbCrLf)
    '            oRoleCollection = cfg_get_roles("AD_TSS_ 43xx 4 juin 2008 avec roles")
    '        End If
    '    End Sub

    '    Private Sub load_RessourceCollection()
    '        If oRessourceCollection Is Nothing Then
    '            txtXML1.AppendText("Collection Ressource Loaded" + vbCrLf)
    '            oRessourceCollection = cfg_get_configuration_Ressource("AD_TSS_ 43xx 4 juin 2008 avec roles")
    '        End If
    '    End Sub

    '    Public Sub load_RelationURoCollection()
    '        If oRelationURoCollection Is Nothing Then
    '            txtXML1.AppendText("Collection Relation Usager/Role Loaded" + vbCrLf)
    '            oRelationURoCollection = cfg_get_user_role_links("AD_TSS_ 43xx 4 juin 2008 avec roles")
    '        End If
    '    End Sub

    '    Public Sub load_RelationUReCollection()
    '        If oRelationUReCollection Is Nothing Then
    '            txtXML1.AppendText("Collection Relation Usager/Ressource Loaded" + vbCrLf)
    '            oRelationUReCollection = cfg_get_user_resource_links("AD_TSS_ 43xx 4 juin 2008 avec roles")
    '        End If
    '    End Sub

    '    Public Sub load_RelationRoReCollection()
    '        If oRelationRoReCollection Is Nothing Then
    '            txtXML1.AppendText("Collection Relation Role/Ressource Loaded" + vbCrLf)
    '            oRelationRoReCollection = cfg_get_role_resource_links("AD_TSS_ 43xx 4 juin 2008 avec roles")
    '        End If
    '    End Sub

    '    Public Sub load_RelationRoRoCollection()
    '        If oRelationRoRoCollection Is Nothing Then
    '            txtXML1.AppendText("Collection Relation Role/Ressource Loaded" + vbCrLf)
    '            oRelationRoRoCollection = cfg_get_role_role_links("AD_TSS_ 43xx 4 juin 2008 avec roles")
    '        End If
    '    End Sub
    '#End Region

    '#Region "Méthode Public"
    '    Public Function getRoleParRolename(ByVal _sRoleName As String) As TsCdSageRole
    '        Dim _oRole As TsCdSageRole
    '        _oRole = Nothing

    '        load_RoleCollection()

    '        For Each c As TsCdSageRole In oRoleCollection
    '            If _sRoleName = c.Name Then
    '                _oRole = c
    '            End If
    '        Next

    '        Return _oRole

    '    End Function

    '    Public Function getRessourceParResname1(ByVal _sResName1 As String) As TsCdSageResource
    '        Dim _oRessource As TsCdSageResource
    '        _oRessource = Nothing

    '        load_RessourceCollection()

    '        For Each c As TsCdSageResource In oRessourceCollection
    '            If _sResName1 = c.ResName1 Then
    '                _oRessource = c
    '            End If
    '        Next

    '        Return _oRessource

    '    End Function
    '    Public Function getUsagerParPersonID(ByVal _sPersonID As String) As TsCdSageUser
    '        Dim _oUsager As TsCdSageUser
    '        _oUsager = Nothing

    '        load_UsagerCollection()

    '        For Each c As TsCdSageUser In oUsagerCollection
    '            If _sPersonID = c.PersonID Then
    '                _oUsager = c
    '            End If
    '        Next

    '        Return _oUsager

    '    End Function
#End Region

#Region "Test"
    Private Sub btnGenerique_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerique.Click
        ''RO/RE
        'Dim _oRessourceCollection As TsCdSageResourceCollection
        '_oRessourceCollection = oService.construitRelationRoRe("AAA AccesDistance Citrix")
        'For Each c As TsCdSageResource In _oRessourceCollection
        '    txtXML1.AppendText(c.ToString + vbCrLf)
        'Next

        ''RE/RO
        'Dim _oRoleCollection As TsCdSageRoleCollection
        '_oRoleCollection = oService.construitRelationReRo("NTAZRSAPW")
        'For Each c As TsCdSageRole In _oRoleCollection
        '    txtXML1.AppendText(c.ToString + vbCrLf)
        'Next

        ''RE/U
        'Dim _oUsagerCollection As TsCdSageUserCollection
        '_oUsagerCollection = oService.construitRelationReU("0000 Tableau de bord de gestion Secur")
        'For Each c As TsCdSageUser In _oUsagerCollection
        '    txtXML1.AppendText(c.ToString + vbCrLf)
        'Next

        ''U/RE
        'Dim _oRessourceCollection As TsCdSageResourceCollection
        '_oRessourceCollection = oService.construitRelationURe("T203981")
        'For Each c As TsCdSageResource In _oRessourceCollection
        '    txtXML1.AppendText(c.ToString + vbCrLf)
        'Next

        ''U/RO
        'Dim _oRoleCollection As TsCdSageRoleCollection
        '_oRoleCollection = oService.construitRelationURo("T20841A")
        'For Each c As TsCdSageRole In _oRoleCollection
        '    txtXML1.AppendText(c.ToString + vbCrLf)
        'Next

        ''U/RO
        'Dim _oUsagerCollection As TsCdSageUserCollection
        '_oUsagerCollection = oService.construitRelationRoU("4300 Comptes Administration")
        'For Each c As TsCdSageUser In _oUsagerCollection
        '    txtXML1.AppendText(c.ToString + vbCrLf)
        'Next

        ''RO/RO Enfant
        'Dim _oParentCollection As TsCdSageRoleCollection
        '_oParentCollection = oService.construitRelationRoRoEnfant("AAA AccesDistance Citrix")
        'For Each c As TsCdSageRole In _oParentCollection
        '    txtXML1.AppendText(c.ToString + vbCrLf)
        'Next

        ''RO/RO Parent
        'Dim _oEnfantCollection As TsCdSageRoleCollection
        '_oEnfantCollection = oService.construitRelationRoRoParent("AAA RAS Citrix")
        'For Each c As TsCdSageRole In _oEnfantCollection
        '    txtXML1.AppendText(c.ToString + vbCrLf)
        'Next

        'TsBaConfigSage.fake()
    End Sub
#End Region

End Class
