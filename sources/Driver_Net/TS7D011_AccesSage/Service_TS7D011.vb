Imports Rrq.Securite.GestionAcces
Imports Rrq.InfrastructureCommune.Parametres

Public Class Service_TS7D011

#Region "CONSTANTE"
    Public Const sDEFAULT_CONFIG As String = "Roles SSDS 4340"
    Public Const sVIEUX_CONFIG As String = "AD_TSS_ 43xx 4 juin 2008 avec roles"
#End Region

#Region "Public Vars"
    Public config As String
#End Region

#Region "Private Vars"
    Private oUsagerCollection As TsCdSageUserCollection
    Private oRoleCollection As TsCdSageRoleCollection
    Private oRessourceCollection As TsCdSageResourceCollection
    Private oRelationURoCollection As TsCdSageUserRoleLinkCollection
    Private oRelationUReCollection As TsCdSageUserResLinkCollection
    Private oRelationRoReCollection As TsCdSageRoleResLinkCollection
    Private oRelationRoRoCollection As TsCdSageRoleRoleLinkCollection
#End Region

#Region "Constructeur"


    Public Sub New()
        config = XuCuConfiguration.ValeurSysteme("TS7", "TS7D011\ConfigDefaut")
    End Sub
#End Region

#Region "Property ReadOnly"
    Public ReadOnly Property getUsagerCollection() As TsCdSageUserCollection
        Get
            load_UsagerCollection()

            Return oUsagerCollection
        End Get
    End Property

    Public ReadOnly Property getRoleCollection() As TsCdSageRoleCollection
        Get
            load_RoleCollection()

            Return oRoleCollection
        End Get
    End Property

    Public ReadOnly Property getRessourceCollection() As TsCdSageResourceCollection
        Get
            load_RessourceCollection()

            Return oRessourceCollection
        End Get
    End Property
#End Region

#Region "Fonctions Loadeur"
    Private Sub load_UsagerCollection()
        If oUsagerCollection Is Nothing Then
            oUsagerCollection = cfg_get_configuration_users(config)
        End If
    End Sub

    Private Sub load_RoleCollection()
        If oRoleCollection Is Nothing Then
            oRoleCollection = cfg_get_roles(config)
        End If
    End Sub

    Private Sub load_RessourceCollection()
        If oRessourceCollection Is Nothing Then
            oRessourceCollection = cfg_get_configuration_Ressource(config)
        End If
    End Sub

    Private Sub load_RelationURoCollection()
        If oRelationURoCollection Is Nothing Then
            oRelationURoCollection = cfg_get_user_role_links(config)
        End If
    End Sub

    Private Sub load_RelationUReCollection()
        If oRelationUReCollection Is Nothing Then
            oRelationUReCollection = cfg_get_user_resource_links(config)
        End If
    End Sub

    Private Sub load_RelationRoReCollection()
        If oRelationRoReCollection Is Nothing Then
            oRelationRoReCollection = cfg_get_role_resource_links(config)
        End If
    End Sub

    Private Sub load_RelationRoRoCollection()
        If oRelationRoRoCollection Is Nothing Then
            oRelationRoRoCollection = cfg_get_role_role_links(config)
        End If
    End Sub
#End Region

#Region "Méthode Get Usager/Role/Ressource"
    Public Function getRole(ByVal _sRoleName As String) As TsCdSageRole
        Dim _oRole As TsCdSageRole
        _oRole = Nothing

        load_RoleCollection()

        For Each c As TsCdSageRole In oRoleCollection
            If _sRoleName = c.Name Then
                _oRole = c
            End If
        Next

        Return _oRole

    End Function

    Public Function getRessource(ByVal _sResName1 As String) As TsCdSageResource
        Dim _oRessource As TsCdSageResource
        _oRessource = Nothing

        load_RessourceCollection()

        For Each c As TsCdSageResource In oRessourceCollection
            If _sResName1 = c.ResName1 Then
                _oRessource = c
            End If
        Next

        Return _oRessource

    End Function

    Public Function getUsager(ByVal _sPersonID As String) As TsCdSageUser
        Dim _oUsager As TsCdSageUser
        _oUsager = Nothing

        load_UsagerCollection()

        For Each c As TsCdSageUser In oUsagerCollection
            If _sPersonID = c.PersonID Then
                _oUsager = c
            End If
        Next

        Return _oUsager

    End Function
#End Region

#Region "Méthode Get liste de Usagers/Roles/Ressources"

    ''' <summary>
    ''' Cette méthode va regrouper les usagers dans un TsCdSageUserCollection
    ''' fesant partis dans la relation Usager/Ressource à partir de la ResName1 d'entée.
    ''' </summary>
    Public Function construitRelationReU(ByVal _sResName1 As String) As TsCdSageUserCollection
        Dim _oRet As TsCdSageUserCollection

        load_RelationUReCollection()

        _oRet = New TsCdSageUserCollection()

        For Each c As TsCdSageUserResLink In oRelationUReCollection
            If c.ResName1 = _sResName1 Then
                Dim _oRessource As TsCdSageUser = getUsager(c.PersonID)
                If Not (_oRessource Is Nothing) Then
                    _oRet.Add(_oRessource)
                End If
            End If
        Next
        Return _oRet
    End Function

    ''' <summary>
    ''' Cette méthode va regrouper les Ressource dans un TsCdSageRessourceCollection
    ''' fesant partis dans la relation Usager/Ressource à partir du PersonID d'entée.
    ''' </summary>
    Public Function construitRelationURe(ByVal _sPersonID As String) As TsCdSageResourceCollection
        Dim _oRet As TsCdSageResourceCollection

        load_RelationUReCollection()

        _oRet = New TsCdSageResourceCollection()

        For Each c As TsCdSageUserResLink In oRelationUReCollection
            If c.PersonID = _sPersonID Then
                Dim _oRessource As TsCdSageResource = getRessource(c.ResName1)
                If Not (_oRessource Is Nothing) Then
                    _oRet.Add(_oRessource)
                End If
            End If
        Next
        Return _oRet
    End Function

    ''' <summary>
    ''' Cette méthode va regrouper les Usagers dans un TsCdSageUserCollection
    ''' fesant partis dans la relation Usager/Role à partir du RoleName d'entée.
    ''' </summary>
    Public Function construitRelationRoU(ByVal _sRoleName As String) As TsCdSageUserCollection
        Dim _oRet As TsCdSageUserCollection

        load_RelationURoCollection()

        _oRet = New TsCdSageUserCollection()

        For Each c As TsCdSageUserRoleLink In oRelationURoCollection
            If c.RoleName = _sRoleName Then
                Dim _oRole As TsCdSageUser = getUsager(c.PersonID)
                If Not (_oRole Is Nothing) Then
                    _oRet.Add(_oRole)
                End If
            End If
        Next
        Return _oRet
    End Function

    ''' <summary>
    ''' Cette méthode va regrouper les Rôles dans un TsCdSageRoleCollection
    ''' fesant partis dans la relation Usager/Role à partir du PersonID d'entée.
    ''' </summary>
    Public Function construitRelationURo(ByVal _sPersonID As String) As TsCdSageRoleCollection
        Dim _oRet As TsCdSageRoleCollection

        load_RelationURoCollection()

        _oRet = New TsCdSageRoleCollection()

        For Each c As TsCdSageUserRoleLink In oRelationURoCollection
            If c.PersonID = _sPersonID Then
                Dim _oRole As TsCdSageRole = getRole(c.RoleName)
                If Not (_oRole Is Nothing) Then
                    _oRet.Add(_oRole)
                End If
            End If
        Next
        Return _oRet
    End Function

    ''' <summary>
    ''' Cette méthode va regrouper les Rôles dans un TsCdSageRoleCollection
    ''' fesant partis dans la relation Role/Ressource à partir du ResName1 d'entée.
    ''' </summary>
    Public Function construitRelationReRo(ByVal _sResName1 As String) As TsCdSageRoleCollection
        Dim _oRet As TsCdSageRoleCollection

        load_RelationRoReCollection()

        _oRet = New TsCdSageRoleCollection()

        For Each c As TsCdSageRoleResLink In oRelationRoReCollection
            If c.ResName1 = _sResName1 Then
                Dim _oRole As TsCdSageRole = getRole(c.RoleName)
                If Not (_oRole Is Nothing) Then
                    _oRet.Add(_oRole)
                End If
            End If
        Next
        Return _oRet
    End Function

    ''' <summary>
    ''' Cette méthode va regrouper les Ressources dans un TsCdSageResourceCollection
    ''' fesant partis dans la relation Role/Ressource à partir du Rolename d'entée.
    ''' </summary>
    Public Function construitRelationRoRe(ByVal _sRolename As String) As TsCdSageResourceCollection
        Dim _oRet As TsCdSageResourceCollection

        load_RelationRoReCollection()

        _oRet = New TsCdSageResourceCollection()

        For Each c As TsCdSageRoleResLink In oRelationRoReCollection
            If c.RoleName = _sRolename Then
                Dim _oRessource As TsCdSageResource = getRessource(c.ResName1)
                If Not (_oRessource Is Nothing) Then
                    _oRet.Add(_oRessource)
                End If
            End If
        Next
        Return _oRet
    End Function

    ''' <summary>
    ''' Cette méthode va regrouper les Rôles enfants dans un TsCdSageRoleCollection
    ''' fesant partis dans la relation Role/Role à partir du Rolename d'entée.
    ''' </summary> 
    Public Function construitRelationRoRoEnfant(ByVal _sRoleName As String) As TsCdSageRoleCollection
        Dim _oRet As TsCdSageRoleCollection

        load_RelationRoRoCollection()

        _oRet = New TsCdSageRoleCollection()

        For Each c As TsCdSageRoleRoleLink In oRelationRoRoCollection
            If c.ParentRole = _sRoleName Then
                Dim _oRole As TsCdSageRole = getRole(c.ChildRole)
                If Not (_oRole Is Nothing) Then
                    _oRet.Add(_oRole)
                End If
            End If
        Next
        Return _oRet
    End Function

    ''' <summary>
    ''' Cette méthode va regrouper les Rôles parents dans un TsCdSageRoleCollection
    ''' fesant partis dans la relation Role/Role à partir du Rolename d'entée.
    ''' </summary> 
    Public Function construitRelationRoRoParent(ByVal _sRoleName As String) As TsCdSageRoleCollection
        Dim _oRet As TsCdSageRoleCollection

        load_RelationRoRoCollection()

        _oRet = New TsCdSageRoleCollection()

        For Each c As TsCdSageRoleRoleLink In oRelationRoRoCollection
            If c.ChildRole = _sRoleName Then
                Dim _oRole As TsCdSageRole = getRole(c.ParentRole)
                If Not (_oRole Is Nothing) Then
                    _oRet.Add(_oRole)
                End If
            End If
        Next
        Return _oRet
    End Function
#End Region

End Class
