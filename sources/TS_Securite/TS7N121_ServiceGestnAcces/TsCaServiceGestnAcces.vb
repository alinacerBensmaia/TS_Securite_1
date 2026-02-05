Imports System.Text.RegularExpressions

''' <summary>
''' Classe d'accès principale pour le module Web. 
''' </summary>
''' <remarks>Un buffer mémorise les données Sage pour des lectures rapides.</remarks>
Public Module TsCaServiceGestnAcces

    Private context As String

#Region "Constructeur"

#End Region

#Region "Méthodes"

    ''' <summary>
    ''' Méthode de recherche d'un utilisateur
    ''' Recherche par mot clé dans le nom complet ou à defaut dans le nom ou prénom.
    ''' Tous les mots clé doivent être présent dans l'utilisateur pour que celui-ci soit valide.
    ''' Les mot clés sont séparés par un espace.
    ''' </summary>
    ''' <returns>Une liste de TsCdUtilisateur.</returns>
    ''' <remarks>Pattern = "" => Tous les utilisateurs. Les accents et la casse sont ignorés (é = e = É = E)</remarks>
    Public Function RechercherUtilisateur(ByVal Pattern As String) As List(Of TsCdUtilisateur)
        Dim lstDeTravail As New List(Of TsCdUtilisateur)
        Dim listeSeparateur() As Char = {" "c}
        Dim clesRecherche As List(Of String)
        Dim lstDeValide As List(Of TsCdUtilisateur)

        For Each us As TsCdSageUser In TsCuAccesSage.Utilisateurs
            lstDeTravail.Add(TsCdUtilisateur.TraductionUtilisateur(us))
        Next
        'Si on demande tous les utilisateurs, on les retourne dès maintenant
        If Pattern = Nothing Then Return lstDeTravail

        'On enleve les accents pour faire une recherche
        Pattern = Rrq.InfrastructureCommune.UtilitairesCommuns.XuCuTraiterChaine.EliminerAccent(Pattern)
        clesRecherche = New List(Of String)(Pattern.Split(listeSeparateur))

        For Each cr As String In clesRecherche
            lstDeValide = New List(Of TsCdUtilisateur)

            For Each u As TsCdUtilisateur In lstDeTravail
                If u.NomComplet = Nothing Then
                    If VerifierSiChampCorrespond(u.Nom, cr) Or VerifierSiChampCorrespond(u.Prenom, cr) Then lstDeValide.Add(u)
                Else
                    If VerifierSiChampCorrespond(u.NomComplet, cr) Then lstDeValide.Add(u)
                End If
            Next

            lstDeTravail = lstDeValide
        Next

        Return lstDeTravail
    End Function

    ''' <summary>
    ''' Méthode de recherche.
    ''' Recherche un rôle dans Sage, basé sur le nom partiel ou complet en entrée.
    ''' </summary>
    ''' <returns>Une liste de TsCdRole.</returns>
    ''' <remarks>Pattern = "" => Tous les rôles.</remarks> 
    Public Function RechercherRole(ByVal pattern As String) As ArrayList
        Dim paramRetour As ArrayList = New ArrayList()

        Dim roleCollection As TsCdSageRoleCollection = TsCuAccesSage.Roles

        For Each c As TsCdSageRole In roleCollection
            Dim tmpRoleName As String = c.Rule.ToLower()
            If Not (tmpRoleName.IndexOf(pattern.ToLower) = -1) Then
                Dim role As TsCdRole = TsCdRole.TraductionRole(c)
                paramRetour.Add(role)
            End If
        Next
        Return paramRetour
    End Function

    ''' <summary>
    ''' Méthode de recherche.
    ''' Recherche un rôle dans Sage, basé sur le nom partiel ou complet en entrée.
    ''' </summary>
    ''' <returns>Une liste de TsCdRole.</returns>
    ''' <remarks>Pattern = "" => Tous les rôles.</remarks> 
    Public Function RechercherRolePolyvalent() As List(Of TsCdRole)
        Dim paramRetour As New List(Of TsCdRole)

        Dim roleCollection As TsCdSageRoleCollection = TsCuAccesSage.Roles
        Dim regex As New Regex("^ *[*] *$")

        For Each c As TsCdSageRole In roleCollection
            If regex.IsMatch(c.Owner) Then
                paramRetour.Add(TsCdRole.TraductionRole(c))
            End If
        Next

        Return paramRetour
    End Function

    ''' <summary>
    ''' Cette méthode va chercher dans la configuration Sage toutes les unités administratives.
    ''' </summary>
    ''' <returns>Une liste de TsCdUniteAdministrative.</returns>
    Public Function ObtenirListeUnitesAdmin() As List(Of TsCdUniteAdministrative)
        Dim ParamRetour As List(Of TsCdUniteAdministrative) = New List(Of TsCdUniteAdministrative)

        For Each c As TsCdSageRole In TsCuAccesSage.Roles
            If c.Type = TsCuAccesSage.TYPEROLE_REO Then
                ParamRetour.Add(TsCdUniteAdministrative.TraductionUnitAdmin(c))
            End If
        Next

        Return ParamRetour
    End Function

    ''' <summary>
    ''' Cette méthode va chercher dans la configuration Sage tous les rôles directement associés à l'utilisateur.
    ''' Les rôles apparenté à ces rôles ne seront pas retournés.
    ''' </summary>
    ''' <returns>Une liste de TsCdAssignationRole.</returns>
    ''' <remarks>Provoque une exception si l'utilisateur n'est pas existant.</remarks>
    Public Function ObtenirAssignationsRole(ByVal idUtilisateur As String) As List(Of TsCdAssignationRole)
        Dim paramRetour As List(Of TsCdAssignationRole) = New List(Of TsCdAssignationRole)

        If TsCuAccesSage.ObtenirUtilisateur(idUtilisateur) Is Nothing Then Throw New TsExcErreurGeneral("L'utilisateur est inexistant.")

        For Each c As TsCdSageRole In TsCuAccesSage.ObtenirRelationURo(idUtilisateur)
            Dim associationRole As TsCdAssignationRole = TsCdAssignationRole.TraductionAssignationRole(c)
            paramRetour.Add(associationRole)
        Next

        Return paramRetour
    End Function

    ''' <summary>
    ''' Cette méthode va chercher dans la configuration Sage toutes les unités administratives de l'utilisateur.
    ''' </summary>
    ''' <returns>Une liste de TsCdUniteAdministrative.</returns>
    ''' <remarks>Provoque une exception si l'utilisateur n'est pas existant.</remarks>
    Public Function ObtenirUnitesAdmin(ByVal idUtilisateur As String) As List(Of TsCdUniteAdministrative)
        Dim paramRetour As New Dictionary(Of String, TsCdUniteAdministrative)

        If TsCuAccesSage.ObtenirUtilisateur(idUtilisateur) Is Nothing Then Throw New TsExcErreurGeneral("L'utilisateur est inexistant.")

        For Each r As TsCdSageRole In TsCuAccesSage.ObtenirRelationURo(idUtilisateur)
            If r.Type = TsCuAccesSage.TYPEROLE_REO_E Then
                For Each p As TsCdSageRole In TsCuAccesSage.ObtenirRelationRoRoEnfant(r.Name)
                    If p.Type = TsCuAccesSage.TYPEROLE_REO Then
                        If paramRetour.ContainsKey(p.Name) = False Then
                            Dim uniteAdmin As TsCdUniteAdministrative = TsCdUniteAdministrative.TraductionUnitAdmin(p)
                            paramRetour.Add(p.Name, uniteAdmin)
                        End If
                    End If
                Next
            End If
            If r.Type = TsCuAccesSage.TYPEROLE_REO Then
                If paramRetour.ContainsKey(r.Name) = False Then
                    Dim uniteAdmin As TsCdUniteAdministrative = TsCdUniteAdministrative.TraductionUnitAdmin(r)
                    paramRetour.Add(r.Name, uniteAdmin)
                End If
            End If
        Next

        Return New List(Of TsCdUniteAdministrative)(paramRetour.Values)
    End Function

    ''' <summary>
    ''' Cette fonction retourne les équipes lié a l'unité administrative.
    ''' </summary>
    ''' <param name="uniteAdminID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ObtenirEquipesUniteAdmin(ByVal uniteAdminID As String) As List(Of TsCdEquipe)
        Dim paramRetour As New List(Of TsCdEquipe)
        Dim rolesSage As TsCdSageRoleCollection = TsCuAccesSage.ObtenirRelationRoRoParent(uniteAdminID)

        Dim lstRoles As New List(Of TsCdSageRole)(rolesSage)

        For Each r As TsCdSageRole In lstRoles
            If r.Type = TsCuAccesSage.TYPEROLE_REO_E Then
                paramRetour.Add(TsCdEquipe.TraductionEquipe(r))
            End If
        Next
        Return paramRetour
    End Function

    ''' <summary>
    ''' Cette méthode va chercher dans la configuration Sage les rôles liés de l'unité administrative.
    ''' Un Rôle lier à l'unité administrative est un rôle équipe qui hérite de l'unité administrative.
    ''' </summary>
    ''' <returns>Une liste de TsCdRole.</returns>
    ''' <remarks>Si l'unite administrative est inexistant une exception sera produite.</remarks>
    Public Function ObtenirRolesUniteAdmin(ByVal uniteAdminID As String) As List(Of TsCdRole)
        Dim lstRoles As New List(Of TsCdRole)()

        Dim roleCollection As TsCdSageRoleCollection = TsCuAccesSage.Roles

        For Each c As TsCdSageRole In roleCollection
            Dim role As TsCdRole = TsCdRole.TraductionRole(c)
            If role.ListeUniteAdministrativeResponsable.Contains(uniteAdminID) Then
                lstRoles.Add(role)
            End If
        Next

        Return lstRoles
    End Function

    ''' <summary>
    ''' Cette méthode va chercher dans la configuration Sage toutes les équipes reliées à l'utilisateur.
    ''' </summary>
    ''' <returns>Une liste de TsCdRole.</returns>
    Public Function ObtenirEquipesUtilisateur(ByVal idUtilisateur As String) As List(Of TsCdEquipe)
        Dim paramRetour As List(Of TsCdEquipe) = New List(Of TsCdEquipe)

        If TsCuAccesSage.ObtenirUtilisateur(idUtilisateur) Is Nothing Then Throw New TsExcErreurGeneral("L'utilisateur est inexistant.")

        '! Trouver les rôles liés à l'utilisateur.
        Dim roles As TsCdSageRoleCollection = TsCuAccesSage.ObtenirRelationURo(idUtilisateur)

        '! Sont-t'ils des équipes? Si oui les mettre dans la liste de retour.
        For Each r As TsCdSageRole In roles
            If r.Type = TsCuAccesSage.TYPEROLE_REO_E Then
                paramRetour.Add(TsCdEquipe.TraductionEquipe(r))
            End If
        Next

        Return paramRetour
    End Function

    ''' <summary>
    ''' Transforme l'équipe en format rôle.
    ''' </summary>
    ''' <returns>Retourne un TsCdRole.</returns>
    Public Function ObtenirRoleEquipe(ByVal idEquipe As String) As TsCdRole
        Dim paramRetour As TsCdRole
        Dim roleSage As TsCdSageRole = TsCuAccesSage.ObtenirRole(idEquipe)
        If roleSage Is Nothing Then
            Throw New TsExcErreurGeneral("Équipe inexistante.")
        Else
            paramRetour = TsCdRole.TraductionRole(roleSage)
        End If
        Return paramRetour
    End Function

    ''' <summary>
    ''' Obtenir un utilisateur de la configuration Sage selon l'identifiant.
    ''' </summary>
    Public Function ObtenirUtilisateur(ByVal idUtilisateur As String) As TsCdUtilisateur
        Dim utilisateur As TsCdSageUser = TsCuAccesSage.ObtenirUtilisateur(idUtilisateur)

        If utilisateur Is Nothing Then Throw New TsExcErreurGeneral("L'utilisateur n'est pas présent dans Sage.")

        Return TsCdUtilisateur.TraductionUtilisateur(utilisateur)
    End Function

    ''' <summary>
    ''' Obtenir un utilisateur selon l'identifiant et dans une configuration particuliere.
    ''' </summary>
    Public Function ObtenirUtilisateur(ByVal config As String, ByVal idUtilisateur As String) As TsCdUtilisateur
        TsCuAccesSage.Config = config
        Return ObtenirUtilisateur(idUtilisateur)
    End Function

    ''' <summary>
    ''' Cette méthode permet d'obtenir les utilisateurs d'une unité administrative.
    ''' </summary>
    ''' <param name="pUniteAdminID">Le numéro ou la partiel(*) de l'unité administrative.</param>
    ''' <returns>Une liste d'utilisateur relier à l'unité administrative.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirUtilisateursUniteAdmin(ByVal pUniteAdminID As String) As List(Of TsCdUtilisateur)
        Dim lstUtilisateurSage As TsCdSageUserCollection = TsCuAccesSage.Utilisateurs()

        Dim uniteAdminIDCorriger As String = pUniteAdminID.Replace("*", ".*")
        Dim regex As New Regex("^" & uniteAdminIDCorriger & "$")

        Return (
                From u In lstUtilisateurSage
                Where regex.IsMatch(u.OrganizationType)
                Select TsCdUtilisateur.TraductionUtilisateur(u)
               ).ToList

        '' Sélectionnez d'abord les rôles qui sont de l'unité administrative.
        'Dim dicRoleSelectionne As New Dictionary(Of String, TsCdRole)

        'Dim roleCollection As TsCdSageRoleCollection = TsCuAccesSage.Roles

        'For Each c As TsCdSageRole In roleCollection
        '    Dim role As TsCdRole = TsCdRole.TraductionRole(c)

        '    For Each uar As String In role.ListeUniteAdministrativeResponsable
        '        Dim uniteAdminIDCorriger As String = pUniteAdminID.Replace("*", ".*")
        '        Dim regex As New Regex("^" & uniteAdminIDCorriger & "$")

        '        If regex.IsMatch(uar) Then
        '            If Not dicRoleSelectionne.ContainsKey(role.ID) Then
        '                dicRoleSelectionne.Add(role.ID, role)
        '            End If
        '        End If
        '    Next
        'Next

        '' --- Seclectionnez ensuite les utilisateurs basé sur les rôles retenu précédament.

        '' Dictionnaire des utilisateurs à retenir.
        'Dim dicUtilisateurs As New Dictionary(Of String, TsCdUtilisateur)

        '' Liste de toutes les liens rôle/utilisateur.
        'Dim lstLiens As List(Of TsCdLienRoleUtilisateur) = TsCuAccesSage.ObtenirRelationsURo()

        '' Dictionnaire des utilisateurs de sage.
        'Dim dicSageUtilisateurs As Dictionary(Of String, TsCdSageUser) = _
        '    TsCuAccesSage.Utilisateurs.ToDictionary(Function(u As TsCdSageUser) u.PersonID)


        'For Each l As TsCdLienRoleUtilisateur In lstLiens
        '    If dicRoleSelectionne.ContainsKey(l.IDRole) AndAlso dicSageUtilisateurs.ContainsKey(l.IDUtilisateur) Then
        '        If Not dicUtilisateurs.ContainsKey(l.IDUtilisateur) Then
        '            Dim utilisateur As TsCdUtilisateur = TsCdUtilisateur.TraductionUtilisateur(dicSageUtilisateurs(l.IDUtilisateur))

        '            dicUtilisateurs.Add(utilisateur.ID, utilisateur)
        '        End If
        '    End If
        'Next

        'Return dicUtilisateurs.Values.ToList
    End Function

    ''' <summary>
    ''' Permet d'obtenir un dictionnaire de tous les utilisateurs et les roles associés à chaque utilisateur.
    ''' </summary>
    ''' <returns>Un dictionnaire d'utilisateur avec leur ensemble de rôles associés.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirRolesUtilisateurs() As Dictionary(Of String, HashSet(Of String))
        Dim dctUtilisateurRole As New Dictionary(Of String, HashSet(Of String))
        Dim lstLiens As TsCdSageUserRoleLinkCollection

        lstLiens = TsCuAccesSage.ObtenirLiensUtilisateurRole()

        For Each l As TsCdSageUserRoleLink In lstLiens
            If dctUtilisateurRole.ContainsKey(l.PersonID) Then
                dctUtilisateurRole(l.PersonID).Add(l.RoleName)
            Else
                dctUtilisateurRole.Add(l.PersonID, New HashSet(Of String)({l.RoleName}))
            End If
        Next

        Return dctUtilisateurRole
    End Function

    ''' <summary>
    ''' Permet d'obtenir un dictionnaire de tous les liens rôle\rôle.
    ''' </summary>
    ''' <returns>Un dictionnaire de rôles avec leur ensemble de rôles associés.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirRolesRoles() As Dictionary(Of String, HashSet(Of String))
        Dim dctRoleRole As New Dictionary(Of String, HashSet(Of String))
        Dim lstLiens As TsCdSageRoleRoleLinkCollection

        lstLiens = TsCuAccesSage.ObtenirLiensRoleRole()

        For Each l As TsCdSageRoleRoleLink In lstLiens
            If dctRoleRole.ContainsKey(l.ParentRole) Then
                dctRoleRole(l.ParentRole).Add(l.ChildRole)
            Else
                dctRoleRole.Add(l.ParentRole, New HashSet(Of String)({l.ChildRole}))
            End If
        Next

        Return dctRoleRole
    End Function


#End Region

#Region "Méthodes globales"
    ''' <summary>
    ''' Cette méthode va prendre la demande de création en entrée et la formater en version Courriel.
    ''' Elle sera ensuite expédié au service de sécurité.
    ''' </summary>
    ''' <param name="demndCreation">Ce paramettre doit être en mode création.</param>
    ''' <param name="dateEffective">Cette information sera directement et uniquement envoyé dans le courriel.</param>
    ''' <remarks>Des validations sont faites sur plusieurs données, si les données sont invalides une exception est provoquée.</remarks>
    Public Sub DemanderCreation(ByVal demndCreation As TsCdDemndCreationModif, ByVal dateEffective As Date)
        TsCaDemandes.DemanderCreation(demndCreation, dateEffective)
    End Sub

    ''' <summary>
    ''' Cette méthode va prendre la demande de création en entrée et la formater en version Courriel.
    ''' Elle sera ensuite expédié au service de sécurité.
    ''' </summary>
    ''' <param name="demndCreation">Ce paramettre doit être en mode création.</param>
    ''' <param name="dateEffective">Cette information sera directement et uniquement envoyé dans le courriel.</param>
    ''' <param name="guid">Indique l'entrée heat à mettre à jour.</param>
    ''' <remarks>Des validations sont faites sur plusieurs données, si les données sont invalides une exception est provoquée.</remarks>
    Public Sub DemanderCreation(ByVal demndCreation As TsCdDemndCreationModif, ByVal dateEffective As Date, ByVal guid As String)
        TsCaDemandes.DemanderCreation(demndCreation, dateEffective, guid)
    End Sub

    ''' <summary>
    ''' Cette méthode va prendre la demande de modification en entrée et la formater en version courriel.
    ''' Le courriel sera ensuite expédié au service de sécurité.
    ''' </summary>
    ''' <param name="demndModification">Ce paramettre doit être en mode création.</param>
    ''' <param name="dateEffective">Cette information sera directement et uniquement envoyé dans le courriel.</param>
    ''' <remarks>
    ''' Des validations sont faites sur plusieurs données, si les données sont invalides une exception est provoquée.
    ''' </remarks>
    Public Sub DemanderModification(ByVal demndModification As TsCdDemndCreationModif, ByVal dateEffective As Date)
        TsCaDemandes.DemanderModification(demndModification, dateEffective)
    End Sub

    ''' <summary>
    ''' Cette méthode va prendre la demande de modification en entrée et la formater en version courriel.
    ''' Le courriel sera ensuite expédié au service de sécurité.
    ''' </summary>
    ''' <param name="demndModification">Ce paramettre doit être en mode création.</param>
    ''' <param name="dateEffective">Cette information sera directement et uniquement envoyé dans le courriel.</param>
    ''' <param name="guid">Indique l'entrée heat à mettre à jour.</param>
    ''' <remarks>
    ''' Des validations sont faites sur plusieurs données, si les données sont invalides une exception est provoquée.
    ''' </remarks>
    Public Sub DemanderModification(ByVal demndModification As TsCdDemndCreationModif, ByVal dateEffective As Date, ByVal guid As String)
        TsCaDemandes.DemanderModification(demndModification, dateEffective, guid)
    End Sub

    ''' <summary>
    ''' Cette Méthode va prendre la demande de Supression en entrée et la formater en version Courriel.
    ''' Elle sera ensuite expédié au service de sécurité.
    ''' </summary>
    ''' <param name="demndDestruction">Ce paramettre doit avoir un utilisateur déja existant dans Sage</param>
    ''' <param name="dateEffective">Cette information sera directement et uniquement envoyé dans le courriel.</param>
    ''' <remarks></remarks>
    Public Sub DemanderDestruction(ByVal demndDestruction As TsCdDemandeDestruction, ByVal dateEffective As Date)
        TsCaDemandes.DemanderDestruction(demndDestruction, dateEffective)
    End Sub

    ''' <summary>
    ''' Cette Méthode va prendre la demande de Supression en entrée et la formater en version Courriel.
    ''' Elle sera ensuite expédié au service de sécurité.
    ''' </summary>
    ''' <param name="demndDestruction">Ce paramettre doit avoir un utilisateur déja existant dans Sage</param>
    ''' <param name="dateEffective">Cette information sera directement et uniquement envoyé dans le courriel.</param>
    ''' <param name="guid">Indique l'entrée heat à mettre à jour.</param>
    ''' <remarks></remarks>
    Public Sub DemanderDestruction(ByVal demndDestruction As TsCdDemandeDestruction, ByVal dateEffective As Date, ByVal guid As String)
        TsCaDemandes.DemanderDestruction(demndDestruction, dateEffective, guid)
    End Sub

    ''' <summary>
    ''' Fonction cachée.
    ''' Cet function raffraichi les buffers. À utiliser si une modification est effectuée dans sage.
    ''' </summary>
    ''' <remarks>N'est pas supposée etre affcihé au développeur web. Utilisation strictement interne.</remarks>
    <System.ComponentModel.Browsable(False)>
    <System.ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Advanced)>
    Public Sub RafraichirBuffer()
        TsCuAccesSage.RafraichirBuffer()
    End Sub
#End Region

#Region "Méthodes privées"
    ''' <summary>
    ''' Verifier si le champs contient le pattern en ignorant la casse et les accents
    ''' </summary>
    ''' <param name="champ">Le champs a tester</param>
    ''' <param name="patternSansAccent">Le pattern à chercher</param>
    ''' <returns>Vrai si le champs contient le pattern</returns>
    ''' <remarks>Les accents du pattern doivent être enlevés avant l'appel de cette fonction pour éviter de le faire plusieurs fois</remarks>
    Private Function VerifierSiChampCorrespond(ByVal champ As String, ByVal patternSansAccent As String) As Boolean
        champ = Rrq.InfrastructureCommune.UtilitairesCommuns.XuCuTraiterChaine.EliminerAccent(champ)
        Return champ.IndexOf(patternSansAccent, System.StringComparison.CurrentCultureIgnoreCase) <> -1
    End Function
#End Region

End Module
