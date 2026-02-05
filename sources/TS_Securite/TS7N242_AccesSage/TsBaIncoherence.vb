Imports System.Reflection
Imports System.Text.RegularExpressions
Imports System.Xml
Imports System.Xml.Serialization

' Petit module pour fixer les incoh้rences de Sage
Public Module TsBaIncoherence

    ' Copie chacun des attributs qui ont le m๊me nom de source เ une nouvelle objet du type qui sera retourn้
    Public Function Copier(Of T)(ByVal source As Object) As T
        Dim dest As T = DirectCast(GetType(T).GetConstructor(New Type() {}).Invoke(New Object() {}), T)

        ' Pour chaque champ de l'objet source...
        For Each sfi As FieldInfo In source.GetType().GetFields(BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.Public)
            ' Il faut 
            ' On obtient le champ destination du m๊me nom
            Dim dfi As FieldInfo = GetType(T).GetField(sfi.Name, BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.Public)
            If dfi IsNot Nothing Then
                dfi.SetValue(dest, sfi.GetValue(source))
            End If
        Next

        Return dest
    End Function

    Public Function diff_get_DB(ByVal vieilleInfo As TsCdSageDBInfrm, ByVal nouvelleInfo As TsCdSageDBInfrm) As TsCdSageDifferenceDB
        Dim paramRetour As New TsCdSageDifferenceDB()

        Dim lstUtilisateurs1 As TsCdSageUserCollection = TsBaConfigSage.udb_get_users(vieilleInfo.Configuration.UserDBName)
        Dim lstUtilisateurs2 As TsCdSageUserCollection = TsBaConfigSage.udb_get_users(nouvelleInfo.Configuration.UserDBName)
        Dim lstRessources1 As TsCdSageResourceCollection = TsBaConfigSage.rdb_get_resources(vieilleInfo.Configuration.DatabaseName)
        Dim lstRessources2 As TsCdSageResourceCollection = TsBaConfigSage.rdb_get_resources(nouvelleInfo.Configuration.DatabaseName)

        With paramRetour

            'ญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญ
            '- Section Utilisateurs
            '---------------------------------------------
            .AddedUsers = New TsCdCollectionSage(Of TsCdSageUser)
            .RemovedUsers = New TsCdCollectionSage(Of TsCdSageUser)
            Decomposition(Of TsCdSageUser, TsCdCollectionSage(Of TsCdSageUser))(lstUtilisateurs1, lstUtilisateurs2, .AddedUsers, .RemovedUsers)

            'ญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญ
            '- Section Ressources
            '---------------------------------------------
            .AddedResources = New TsCdCollectionSage(Of TsCdSageResource)
            .RemovedResources = New TsCdCollectionSage(Of TsCdSageResource)
            Decomposition(Of TsCdSageResource, TsCdCollectionSage(Of TsCdSageResource))(lstRessources1, lstRessources2, .AddedResources, .RemovedResources)


        End With
        Return paramRetour
    End Function

    ''' <summary>
    ''' Fonction de remplacement du service Sage. Ommet certaines comparaisons que l'on ne peut pas control้, comme le num้ro ID des UtilisateursBD et RessourcesDB.
    ''' </summary>
    ''' <remarks></remarks>
    Public Function diff_get_all(ByVal oldCfg As String, ByVal updatedCfg As String) As TsCdSageDifferenceConfig
        Dim paramRetour As New TsCdSageDifferenceConfig()

        Dim lstUtilisateurs1 As TsCdSageUserCollection = TsBaConfigSage.cfg_get_configuration_users(oldCfg)
        Dim lstUtilisateurs2 As TsCdSageUserCollection = TsBaConfigSage.cfg_get_configuration_users(updatedCfg)
        Dim lstRoles1 As TsCdSageRoleCollection = TsBaConfigSage.cfg_get_roles(oldCfg)
        Dim lstRoles2 As TsCdSageRoleCollection = TsBaConfigSage.cfg_get_roles(updatedCfg)
        Dim lstRessources1 As TsCdSageResourceCollection = TsBaConfigSage.cfg_get_configuration_Ressource(oldCfg)
        Dim lstRessources2 As TsCdSageResourceCollection = TsBaConfigSage.cfg_get_configuration_Ressource(updatedCfg)
        Dim lstURe1 As TsCdSageUserResLinkCollection = TsBaConfigSage.cfg_get_user_resource_links(oldCfg)
        Dim lstURe2 As TsCdSageUserResLinkCollection = TsBaConfigSage.cfg_get_user_resource_links(updatedCfg)
        Dim lstURo1 As TsCdSageUserRoleLinkCollection = TsBaConfigSage.cfg_get_user_role_links(oldCfg)
        Dim lstURo2 As TsCdSageUserRoleLinkCollection = TsBaConfigSage.cfg_get_user_role_links(updatedCfg)
        Dim lstRoRe1 As TsCdSageRoleResLinkCollection = TsBaConfigSage.cfg_get_role_resource_links(oldCfg)
        Dim lstRoRe2 As TsCdSageRoleResLinkCollection = TsBaConfigSage.cfg_get_role_resource_links(updatedCfg)
        Dim lstRoRo1 As TsCdSageRoleRoleLinkCollection = TsBaConfigSage.cfg_get_role_role_links(oldCfg)
        Dim lstRoRo2 As TsCdSageRoleRoleLinkCollection = TsBaConfigSage.cfg_get_role_role_links(updatedCfg)
        'ญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญ
        '- Section cr้ation des listes de diff้rences
        '---------------------------------------------
        With paramRetour

            'ญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญ
            '- Section Utilisateurs
            '---------------------------------------------
            .AddedUsers = New TsCdCollectionSage(Of TsCdSageUser)
            .RemovedUsers = New TsCdCollectionSage(Of TsCdSageUser)
            Decomposition(Of TsCdSageUser, TsCdCollectionSage(Of TsCdSageUser))(lstUtilisateurs1, lstUtilisateurs2, .AddedUsers, .RemovedUsers)

            'ญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญ
            '- Section Roles
            '---------------------------------------------
            .AddedRoles = New TsCdCollectionSage(Of TsCdSageRole)
            .RemovedRoles = New TsCdCollectionSage(Of TsCdSageRole)
            Decomposition(Of TsCdSageRole, TsCdCollectionSage(Of TsCdSageRole))(lstRoles1, lstRoles2, .AddedRoles, .RemovedRoles)

            'ญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญ
            '- Section Ressources
            '---------------------------------------------
            .AddedResources = New TsCdCollectionSage(Of TsCdSageResource)
            .RemovedResources = New TsCdCollectionSage(Of TsCdSageResource)
            Decomposition(Of TsCdSageResource, TsCdCollectionSage(Of TsCdSageResource))(lstRessources1, lstRessources2, .AddedResources, .RemovedResources)

            'ญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญ
            '- Section liens Utilisteurs/Ressources
            '---------------------------------------------
            .AddedUserResourceLinks = New TsCdCollectionSage(Of TsCdSageUserResLink)
            .RemovedUserResourceLinks = New TsCdCollectionSage(Of TsCdSageUserResLink)
            Decomposition(Of TsCdSageUserResLink, TsCdCollectionSage(Of TsCdSageUserResLink))(lstURe1, lstURe2, .AddedUserResourceLinks, .RemovedUserResourceLinks)

            'ญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญ
            '- Section liens Utilisateurs/Roles
            '---------------------------------------------
            .AddedUserRoleLinks = New TsCdCollectionSage(Of TsCdSageUserRoleLink)
            .RemovedUserRoleLinks = New TsCdCollectionSage(Of TsCdSageUserRoleLink)
            Decomposition(Of TsCdSageUserRoleLink, TsCdCollectionSage(Of TsCdSageUserRoleLink))(lstURo1, lstURo2, .AddedUserRoleLinks, .RemovedUserRoleLinks)

            'ญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญ
            '- Section liens Roles/Ressources
            '---------------------------------------------
            .AddedRoleResourceLinks = New TsCdCollectionSage(Of TsCdSageRoleResLink)
            .RemovedRoleResourceLinks = New TsCdCollectionSage(Of TsCdSageRoleResLink)
            Decomposition(Of TsCdSageRoleResLink, TsCdCollectionSage(Of TsCdSageRoleResLink))(lstRoRe1, lstRoRe2, .AddedRoleResourceLinks, .RemovedRoleResourceLinks)

            'ญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญญ
            '- Section lien Roles/Roles
            '---------------------------------------------
            .AddedRoleRoleLinks = New TsCdCollectionSage(Of TsCdSageRoleRoleLink)
            .RemovedRoleRoleLinks = New TsCdCollectionSage(Of TsCdSageRoleRoleLink)
            Decomposition(Of TsCdSageRoleRoleLink, TsCdCollectionSage(Of TsCdSageRoleRoleLink))(lstRoRo1, lstRoRo2, .AddedRoleRoleLinks, .RemovedRoleRoleLinks)

        End With
        Return paramRetour
    End Function

    ''' <summary>
    ''' Fonction g้n้rique. Revois 2 listes d'objet tri้.
    ''' </summary>
    ''' <typeparam name="U">Type de base.</typeparam>
    ''' <typeparam name="T">Type Collection du type de base.</typeparam>
    ''' <param name="liste1">Collection contenant le type de base.</param>
    ''' <param name="liste2">Collection contenant le type de base.</param>
    ''' <param name="lstAjouter">
    ''' Collection contenant le type de base. Liste de retour.
    ''' Un des r้sultat sera retourner par cette varaible.
    ''' Contient le ้l้ment qui sont ajouter เ la nouvelle configuration.
    ''' </param>
    ''' <param name="lstRetirer">
    ''' Collection contenant le type de base. Liste de retour.
    ''' Un des r้sultat sera retourner par cette varaible.
    ''' Contient le ้l้ment qui sont retirer เ la nouvelle configuration.
    ''' </param>
    ''' <remarks></remarks>
    Private Sub Decomposition(Of U, T As TsCdCollectionSage(Of U))(ByVal liste1 As T, ByVal liste2 As T, ByVal lstAjouter As T, ByVal lstRetirer As T)
        Dim i As Integer = 0
        While i < liste1.List.Count
            Dim u1 As U = liste1.List(i)
            Dim flagAbsent As Boolean = True

            Dim j As Integer = 0
            While j < liste2.List.Count
                Dim u2 As U = liste2.List(j)

                If Comparaison(u1, u2) = True Then
                    liste2.List.RemoveAt(j)
                    flagAbsent = False
                    Exit While
                End If
                j = j + 1
            End While

            If flagAbsent = False Then
                liste1.List.RemoveAt(i)
                i = i - 1
            End If
            i = i + 1
        End While
        For Each u1 As U In liste1.List '! Les Objets retir้s de la nouvelle config
            lstRetirer.Add(u1)
        Next
        For Each u2 As U In liste2.List '! Les Objets ajout้s เ la nouvelle config
            lstAjouter.Add(u2)
        Next
    End Sub

    ''' <summary>
    ''' Fonction de compraison. 
    ''' Toute les objets/liens possibles de la configuration ont un cas de comparaion dasn cette fonction.
    ''' </summary>
    ''' <param name="object1">Doit ๊tre du m๊me type que Objet2.</param>
    ''' <param name="object2">Doit ๊tre du m๊me type que Objet1.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function Comparaison(ByVal object1 As Object, ByVal object2 As Object) As Boolean

        Select Case True
            Case TypeOf object1 Is TsCdSageUser
                Dim utilisateur1 As TsCdSageUser = CType(object1, TsCdSageUser)
                Dim utilisateur2 As TsCdSageUser = CType(object2, TsCdSageUser)

                If utilisateur1.Courriel <> utilisateur2.Courriel Then Return False
                If utilisateur1.DateFin <> utilisateur2.DateFin Then Return False
                If utilisateur1.Nom <> utilisateur2.Nom Then Return False
                If utilisateur1.Organization <> utilisateur2.Organization Then Return False
                If utilisateur1.OrganizationType <> utilisateur2.OrganizationType Then Return False
                If utilisateur1.PersonID <> utilisateur2.PersonID Then Return False
                If utilisateur1.Prenom <> utilisateur2.Prenom Then Return False
                If utilisateur1.UserID <> utilisateur2.UserID Then Return False
                If utilisateur1.UserName <> utilisateur2.UserName Then Return False
                If utilisateur1.Ville <> utilisateur2.Ville Then Return False

                Return True
            Case TypeOf object1 Is TsCdSageRole
                Dim role1 As TsCdSageRole = CType(object1, TsCdSageRole)
                Dim role2 As TsCdSageRole = CType(object2, TsCdSageRole)

                If role1.ApproveCode <> role2.ApproveCode Then Return False
                If role1.ApproveDate <> role2.ApproveDate Then Return False
                If role1.CreateDate <> role2.CreateDate Then Return False
                If role1.Description <> role2.Description Then Return False
                If role1.ExpirationDate <> role2.ExpirationDate Then Return False
                If role1.Filter <> role2.Filter Then Return False
                If role1.Name <> role2.Name Then Return False
                If role1.Organization <> role2.Organization Then Return False
                If role1.Organization2 <> role2.Organization2 Then Return False
                If role1.Organization3 <> role2.Organization3 Then Return False
                If role1.Owner <> role2.Owner Then Return False
                If role1.Reviewer <> role2.Reviewer Then Return False
                If role1.RoleID <> role2.RoleID Then Return False
                If role1.Type <> role2.Type Then Return False

                Return True
            Case TypeOf object1 Is TsCdSageResource
                Dim ressource1 As TsCdSageResource = CType(object1, TsCdSageResource)
                Dim ressource2 As TsCdSageResource = CType(object2, TsCdSageResource)

                If ressource1.FieldValue1 <> ressource2.FieldValue1 Then Return False
                If ressource1.FieldValue2 <> ressource2.FieldValue2 Then Return False
                If ressource1.FieldValue3 <> ressource2.FieldValue3 Then Return False
                If ressource1.FieldValue4 <> ressource2.FieldValue4 Then Return False
                If ressource1.FieldValue5 <> ressource2.FieldValue5 Then Return False
                If ressource1.ResName1 <> ressource2.ResName1 Then Return False
                If ressource1.ResName2 <> ressource2.ResName2 Then Return False
                If ressource1.ResName3 <> ressource2.ResName3 Then Return False
                If ressource1.ResName4 <> ressource2.ResName4 Then Return False

                Return True
            Case TypeOf object1 Is TsCdSageUserResLink
                Dim lienURe1 As TsCdSageUserResLink = CType(object1, TsCdSageUserResLink)
                Dim lienURe2 As TsCdSageUserResLink = CType(object2, TsCdSageUserResLink)

                If lienURe1.PersonID <> lienURe2.PersonID Then Return False
                If lienURe1.ResName1 <> lienURe2.ResName1 Then Return False
                If lienURe1.ResName2 <> lienURe2.ResName2 Then Return False
                If lienURe1.ResName3 <> lienURe2.ResName3 Then Return False

                Return True
            Case TypeOf object1 Is TsCdSageUserRoleLink
                Dim lienURo1 As TsCdSageUserRoleLink = CType(object1, TsCdSageUserRoleLink)
                Dim lienURo2 As TsCdSageUserRoleLink = CType(object2, TsCdSageUserRoleLink)

                If lienURo1.PersonID <> lienURo2.PersonID Then Return False
                If lienURo1.RoleName <> lienURo2.RoleName Then Return False

                Return True
            Case TypeOf object1 Is TsCdSageRoleResLink
                Dim lienRoRe1 As TsCdSageRoleResLink = CType(object1, TsCdSageRoleResLink)
                Dim lienRoRe2 As TsCdSageRoleResLink = CType(object2, TsCdSageRoleResLink)

                If lienRoRe1.RoleName <> lienRoRe2.RoleName Then Return False
                If lienRoRe1.ResName1 <> lienRoRe2.ResName1 Then Return False
                If lienRoRe1.ResName2 <> lienRoRe2.ResName2 Then Return False
                If lienRoRe1.ResName3 <> lienRoRe2.ResName3 Then Return False

                Return True
            Case TypeOf object1 Is TsCdSageRoleRoleLink
                Dim lienRoRo1 As TsCdSageRoleRoleLink = CType(object1, TsCdSageRoleRoleLink)
                Dim lienRoRo2 As TsCdSageRoleRoleLink = CType(object2, TsCdSageRoleRoleLink)

                If lienRoRo1.ChildRole <> lienRoRo2.ChildRole Then Return False
                If lienRoRo1.ParentRole <> lienRoRo2.ParentRole Then Return False

                Return True
        End Select
    End Function

End Module

Public Class TsBaIncoherenceC

#Region "Private Vars"

    Private Shared nbrTrouvee As Integer = 0

#End Region

#Region "Fix"

    Private Shared Function evalFix(ByVal m As Match) As String
        Dim regexp As New Regex("<AddedRoleRoleLinks>")
        Dim paramRetour As String = m.ToString

        nbrTrouvee += 1
        If (nbrTrouvee >= 3 And nbrTrouvee <= 4) Then
            Select Case regexp.IsMatch(paramRetour)
                Case True
                    paramRetour = "<RemovedRoleRoleLinks>"
                Case False
                    paramRetour = "</RemovedRoleRoleLinks>"
            End Select
        End If
        Return paramRetour
    End Function

    Public Shared Function Fix_Diff_get_all(ByVal xmlString As String) As String
        Dim regexp As New Regex("(<AddedRoleRoleLinks>)|(</AddedRoleRoleLinks>)")

        nbrTrouvee = 0

        xmlString = regexp.Replace(xmlString, AddressOf evalFix)

        Return xmlString
    End Function

    Public Shared Sub Fix_Balise_Vide(ByVal xmlDoc As XmlDocument)
        VisiteurNode(xmlDoc)
    End Sub

#End Region

#Region "Fonctions de services"

    Private Shared Sub VisiteurNode(ByVal xmlNode As XmlNode)
        Dim i As Integer = 0
        While i < xmlNode.ChildNodes.Count
            Dim node As XmlNode = xmlNode.ChildNodes(i)
            If node.ChildNodes.Count = 0 Then
                If node.NodeType = XmlNodeType.Element Then
                    xmlNode.RemoveChild(node)
                    i = i - 1
                End If
            Else
                VisiteurNode(node)
            End If
            i = i + 1
        End While
    End Sub

#End Region

End Class

''' <summary>
''' Classe d้s้rialisable. Existe pour palier เ l'incoh้rence des nom de balise que sage Renvois de cfg_get_configuration_resources().
''' </summary>
<XmlRoot("Roles")> _
Public Class TsCdSageResourceConfigCollection
    Inherits TsCdCollectionSage(Of TsCdSageResourceConfig)

    <XmlIgnore()> _
    Public ReadOnly Property Resources() As List(Of TsCdSageResourceConfig)
        Get
            Return _list
        End Get
    End Property

End Class

''' <summary>
''' Classe d้s้rialisable. Existe pour palier เ l'incoh้rence des nom de balise que sage Renvois de cfg_get_configuration_resources().
''' </summary>
<XmlType("Role")> _
Public Class TsCdSageResourceConfig
    Public ResName1 As String
    Public ResName2 As String
    Public ResName3 As String
    Public ResName4 As String
    Public FieldValue1 As String
    Public FieldValue2 As String
    Public FieldValue3 As String
    Public FieldValue4 As String
    Public FieldValue5 As String
    Public Overrides Function ToString() As String
        Return ResName1
    End Function
End Class


