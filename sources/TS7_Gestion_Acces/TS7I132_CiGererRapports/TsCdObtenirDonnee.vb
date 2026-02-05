Imports Rrq.Securite.GestionAcces
Imports System.Text.RegularExpressions

''' <summary>
''' Cette classe permet d'allez chercher les informations nécessaires à la composition de la source du rapport.
''' </summary>
''' <remarks></remarks>
Friend Class TsCdObtenirDonnee

#Region "--- Propriétés ---"

    ''' <summary>
    ''' Dictionnaire des utilisateurs. 
    ''' </summary>
    Private mDctUtilisateurs As New Dictionary(Of String, TsCdUtilisateur)

    ''' <summary>
    ''' Dictionnaire des rôles. 
    ''' </summary>
    Private mDctRoles As New Dictionary(Of String, TsCdRole)

    ''' <summary>
    ''' Dictionnaire des contextes.
    ''' </summary>
    Private mDctContextes As New Dictionary(Of String, TsDtSourceContexteUA)

    ''' <summary>
    ''' Dictionnaire des utilisateurs et de leurs rôles associés.
    ''' </summary>
    Private mDctUtilisateurRoles As Dictionary(Of String, HashSet(Of String))

    ''' <summary>
    ''' Dictionnaire des utilisateurs et de leurs rôles associés directs et indirects.
    ''' </summary>
    Private mDctUtilisateurRolesIndirect As New Dictionary(Of String, HashSet(Of String))

    ''' <summary>
    ''' Dictionnaire des rôles et de leurs sous-rôles.
    ''' </summary>
    Private mDctRoleRoles As Dictionary(Of String, HashSet(Of String))

    ''' <summary>
    ''' Dictionnaire des contextes de rôles.
    ''' </summary>
    Private mDctRoleContextes As New Dictionary(Of String, HashSet(Of String))

#End Region

#Region "--- Constructeurs ---"

    ''' <summary>
    ''' Constructeur de base
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        For Each u In TsCaServiceGestnAcces.RechercherUtilisateur("")
            mDctUtilisateurs.Add(u.ID, u)
        Next

        'T208704 : patch pour afficher les roles RET, REM et REO seulement
        For Each r As TsCdRole In TsCaServiceGestnAcces.RechercherRole("")
            If r.ID.StartsWith("RET_") Or r.ID.StartsWith("REO_") Or r.ID.StartsWith("REM_") Then
                mDctRoles.Add(r.ID, r)
            End If

        Next

        mDctUtilisateurRoles = TsCaServiceGestnAcces.ObtenirRolesUtilisateurs()
        mDctRoleRoles = TsCaServiceGestnAcces.ObtenirRolesRoles()

        EnleverRolesNonVoulus() ' FAit l'élimination des roles qui ne sont pas rET, rem et reo pour dctRolesroles

        CompleterDctUtilisateurRolesIndirect() ' fera l'élimination des roles qui ne commence pas par RET, REM et REO pour dctUtilisateurRolesIndirects
        CompleterContextes()
    End Sub

#End Region


#Region "--- Méthodes ---"
   
    ''' <summary>
    ''' Permet d'obtenir la source du rapport.
    ''' </summary>
    ''' <param name="pLstUnitesAdministrative">Liste des unités administratives.</param>
    ''' <returns>Une source destinée au rapport.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirSourcePrincipale(ByVal pLstUnitesAdministrative As List(Of String)) As TsDtSourcFeuilPrinc
        Dim dateProd As Date = Date.Now

        ' Besoin de la source:
        '   - Sélectionner tous les utilisateurs des Unités administrative.
        '   - Sélectionner tous les rôle des unité administrative.
        '   - Sélectionner tous les autres rôles des utilisateurs.

        Dim dctRole As Dictionary(Of String, TsCdRole)
        Dim lstUaSlect As List(Of String)
        Dim dctUtilisateurs As Dictionary(Of String, TsCdUtilisateur)
        Dim dctUtilisateurAssgnRole As Dictionary(Of String, List(Of TsCdAssignationRole))
        Dim dctRoleContexte As Dictionary(Of String, List(Of TsDtSourceContexteUA))
        Dim dctDistinctContexte As Dictionary(Of String, TsDtSourceContexteUA)
        Dim dctUtilisateurUaAssoc As Dictionary(Of String, List(Of TsDtSourceUaAssociee))
        Dim lstEmployes As List(Of TsDtSourceEmploye)
        Dim dctUaDisponible As Dictionary(Of String, TsDtSourceUa)

        dctRole = ObtenirRoles(pLstUnitesAdministrative)
        lstUaSlect = (From r In dctRole.Values Select r.Nom).ToList()
        dctUtilisateurs = ObtenirUtilisateurs(pLstUnitesAdministrative)
        dctUtilisateurAssgnRole = ObtenirAssignationsRoles(dctUtilisateurs)

        AjouterRoleUtilisateur(dctUtilisateurAssgnRole, dctRole)

        dctRoleContexte = ObtenirRoleContexte(dctRole)
        dctDistinctContexte = ObtenirRolesContextesPlat(dctRoleContexte)
        dctUtilisateurUaAssoc = ObtenirUaAssocEmploye(dctUtilisateurAssgnRole, dctRole, dctDistinctContexte)
        lstEmployes = ObtenirEmployes(dctUtilisateurs, dctUtilisateurUaAssoc)
        dctUaDisponible = ObtenirUnitesAdministrative(dctUtilisateurAssgnRole, dctRole, dctRoleContexte)

        ' --- Trier les listes et filtrer ---
        lstEmployes = (From e In lstEmployes Order By e.NoUA, e.Nom).ToList
        Dim lstUaDisponible = (From ua In dctUaDisponible.Values.ToList Where ua.Type <> TsDtSourceUa.TypeRoleUA.REO Order By ua.Type Descending, ua.Nom).ToList

        ' --- Remplir la source du rapport ---
        Dim source As New TsDtSourcFeuilPrinc(dateProd, pLstUnitesAdministrative)
        With source
            .Contextes = (From c In dctDistinctContexte.Values Select c Order By c.Titre).ToList
            .Employes = lstEmployes
            .RoleUaSelectionnes = lstUaSlect
            .RoleUaDisponibles = lstUaDisponible
        End With

        Return source
    End Function

    ''' <summary>
    ''' Permet d'obtenir la source du rapport.
    ''' </summary>
    ''' <param name="pLstUnitesAdministrative">Liste des unités administratives.</param>
    ''' <returns>Une source destinée au rapport.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirSourceSecondaire(ByVal pLstUnitesAdministrative As List(Of String)) As TsDtSourceRapport
        Dim lstRoleUa As List(Of TsCdRole)
        Dim lstRoleUaNonRejeter As List(Of TsCdRole)
        Dim dctRoleUaPolyvalent As Dictionary(Of String, TsCdRole)

        Dim lstUtilisateurUa As List(Of TsCdUtilisateur)
        Dim lstUtilisateurUaCible As List(Of TsCdUtilisateur)

        Dim lstContexteCible As List(Of TsDtSourceContexteUA)
        Dim lstEmploye As List(Of TsDtSourceEmploye)
        Dim lstSourcesUA As List(Of TsDtSourceUa)

        ' Certain rôle sont attribué à tous les utilisateur
        dctRoleUaPolyvalent = ObtenirRolesPolyvalent()

        ' Trouver les rôles des unités administratives.
        lstRoleUa = ObtenirRoles2(pLstUnitesAdministrative)
        lstRoleUaNonRejeter = lstRoleUa.Where(Function(r) Not dctRoleUaPolyvalent.ContainsKey(r.ID)).ToList

        ' Touver les utilisateurs qui consomme les rôles ciblés.
        lstUtilisateurUa = ObtenirUtilisateursRoles(lstRoleUaNonRejeter)

        ' Filtrer pour ne garder que les utilisateurs qui ne font pas partis des unités administratives.
        lstUtilisateurUaCible = (From u In lstUtilisateurUa Where pLstUnitesAdministrative.Contains(u.NoUniteAdmin) = False).ToList

        ' Transformer les résultats dans les structures sources.
        lstEmploye = ObtenirEmployes(lstUtilisateurUaCible)

        ' --- Trier les employés ---
        lstEmploye = (From e In lstEmploye Order By e.NoUA, e.Nom).ToList

        ' Obtenir les contextes concerné.
        lstContexteCible = ObtenirContextes(lstRoleUa)

        ' Obtenir les rôles UA disponible.
        lstSourcesUA = ObtenirRoleUnitesAdministrative(lstRoleUa)

        ' Trier et filtrer les roles UA disponible.
        lstSourcesUA = (From ua In lstSourcesUA Where ua.Type <> TsDtSourceUa.TypeRoleUA.REO Order By ua.Type Descending, ua.Nom).ToList

        Dim source As New TsDtSourceRapport(Date.Now, pLstUnitesAdministrative)
        With source
            .Contextes = lstContexteCible
            .Employes = lstEmploye
            .RoleUaSelectionnes = (From r In lstRoleUa Select r.Nom).ToList
            .RoleUaDisponibles = lstSourcesUA
        End With

        If source.Employes.Count = 0 Then
            Return Nothing
        Else
            Return source
        End If
    End Function

#End Region

#Region "--- Sous-fonctions feuille principale ---"

    ''' <summary>
    ''' Permet d'obtenir tous les rôles.
    ''' </summary>
    ''' <returns>Un dictionnaire de rôle, indexé sur le ID.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirRolesPolyvalent() As Dictionary(Of String, TsCdRole)
        Dim dctRole As New Dictionary(Of String, TsCdRole)

        For Each r As TsCdRole In TsCaServiceGestnAcces.RechercherRolePolyvalent()
            If dctRole.ContainsKey(r.ID) = False Then
                'Ne plus afficher les Roles qui ne sont pas des REM, RET et REO
                If r.ID.StartsWith("RET_") Or r.ID.StartsWith("REM_") Or r.ID.StartsWith("REO_") Then
                    dctRole.Add(r.ID, r)
                End If
            End If
        Next

        Return dctRole
    End Function


    ''' <summary>
    ''' Permet d'obtenir tous les rôles reliés aux unités administratives.
    ''' </summary>
    ''' <param name="pLstUnitesAdministrative">La liste des unités administratives</param>
    ''' <returns>Un dictionnaire de rôle, indexé sur le ID.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirRoles(ByVal pLstUnitesAdministrative As List(Of String)) As Dictionary(Of String, TsCdRole)
        Dim dctRole As New Dictionary(Of String, TsCdRole)
        Dim lstRoleParAdm As New List(Of TsCdRole)


        For Each uaID In pLstUnitesAdministrative
            lstRoleParAdm = TsCaServiceGestnAcces.ObtenirRolesUniteAdmin(uaID)

            For Each r In lstRoleParAdm
                If dctRole.ContainsKey(r.ID) = False Then
                    'T208704 : Ne plus afficher les Roles qui ne sont pas des REM, RET et REO
                    If r.ID.StartsWith("RET_") Or r.ID.StartsWith("REO_") Or r.ID.StartsWith("REM_") Then
                        dctRole.Add(r.ID, r)
                    End If
                End If

            Next
        Next

        Return dctRole
    End Function

    ''' <summary>
    ''' Permet d'obtenir tous les utilisateurs reliés aux unités administratives.
    ''' </summary>
    ''' <param name="pLstUnitesAdministrative">La liste des unités administratives</param>
    ''' <returns>Un dictionnaire d'utilisateur, indexé su le ID.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirUtilisateurs(ByVal pLstUnitesAdministrative As List(Of String)) As Dictionary(Of String, TsCdUtilisateur)
        Dim dctUtilisateurs As New Dictionary(Of String, TsCdUtilisateur)

        For Each uaID In pLstUnitesAdministrative
            For Each u In TsCaServiceGestnAcces.ObtenirUtilisateursUniteAdmin(uaID)
                If dctUtilisateurs.ContainsKey(u.ID) = False Then dctUtilisateurs.Add(u.ID, u)
            Next
        Next

        Return dctUtilisateurs
    End Function


    ''' <summary>
    ''' Permet d'obtenir les assignations de rôles de chaque utilisateur.
    ''' </summary>
    ''' <param name="pDctUtilisateurs">Un dictionnaire d'utilisateur, indexé su le ID.</param>
    ''' <returns>Un dictionnaire de liste d'assignations de rôles, indexé sur le Id de l'utilisateur assigné.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirAssignationsRoles(ByVal pDctUtilisateurs As Dictionary(Of String, TsCdUtilisateur)) As Dictionary(Of String, List(Of TsCdAssignationRole))
        Dim dctUtilisateurAssgnRole As New Dictionary(Of String, List(Of TsCdAssignationRole))

        For Each u In pDctUtilisateurs.Values

            Dim lstAssignationRole As New List(Of TsCdAssignationRole)

            lstAssignationRole = TsCaServiceGestnAcces.ObtenirAssignationsRole(u.ID).Where(Function(x) x.ID.StartsWith("RET_") Or x.ID.StartsWith("REM_") Or x.ID.StartsWith("REO_")).ToList
            dctUtilisateurAssgnRole.Add(u.ID, lstAssignationRole)

        Next

        Return dctUtilisateurAssgnRole
    End Function

    ''' <summary>
    ''' Permet d'obtenir le contexte d'un rôle.
    ''' </summary>
    ''' <param name="pDctRole">Un dictionnaire de rôles, indexé sur le ID du rôle.</param>
    ''' <returns>Un dictionnaire de listes de contextes de rôles, indexé sur le ID du rôle.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirRoleContexte(ByVal pDctRole As Dictionary(Of String, TsCdRole)) As Dictionary(Of String, List(Of TsDtSourceContexteUA))
        Dim dctRoleContexte As New Dictionary(Of String, List(Of TsDtSourceContexteUA))
        For Each r In pDctRole.Values
            If String.IsNullOrEmpty(r.Organisation) = False Then
                Dim lst As List(Of TsDtSourceContexteUA) = ConstruireContextes(r)

                dctRoleContexte.Add(r.ID, lst)
            End If
        Next

        Return dctRoleContexte
    End Function

    ''' <summary>
    ''' Permet d'obtenir tous les contextes rôles de tous les rôles, sans doublons.
    ''' </summary>
    ''' <param name="pDctRoleContexte">Un dictionnaire de contextes de rôles.</param>
    ''' <returns>Un dictionnaire sans doublons de contextes de rôle, indexé sur le titre du contexte.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirRolesContextesPlat(ByVal pDctRoleContexte As Dictionary(Of String, List(Of TsDtSourceContexteUA))) As Dictionary(Of String, TsDtSourceContexteUA)
        Dim dctDistinctContexte As New Dictionary(Of String, TsDtSourceContexteUA)

        For Each rc In pDctRoleContexte.Values
            For Each c In rc
                If dctDistinctContexte.ContainsKey(c.Titre) = False Then dctDistinctContexte.Add(c.Titre, c)
            Next
        Next

        Return dctDistinctContexte
    End Function

    ''' <summary>
    ''' Permet d'obtenir les unités administratives associées à un utilisateur.
    ''' </summary>
    ''' <param name="pDctUtilisateurAssgnRole">Un dictionnaire d'assignations de rôles, indexé sur l'ID de l'utilisateur.</param>
    ''' <param name="pDctRole">Un dictionnaire de rôle, indexé sur le ID du rôle.</param>
    ''' <param name="pDctDistinctContexte">Un dictionnaire de contextes, indexé sur le Titre du contexte.</param>
    ''' <returns>Un dictionnaire d'associations d'unités administratives, indexé sur le ID de l'utilisateur.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirUaAssocEmploye(ByVal pDctUtilisateurAssgnRole As Dictionary(Of String, List(Of TsCdAssignationRole)), _
                                               ByVal pDctRole As Dictionary(Of String, TsCdRole), _
                                               ByVal pDctDistinctContexte As Dictionary(Of String, TsDtSourceContexteUA)) As Dictionary(Of String, List(Of TsDtSourceUaAssociee))
        Return ObtenirUaAssocEmploye(pDctUtilisateurAssgnRole, pDctRole, pDctDistinctContexte, False, Nothing)
    End Function

    ''' <summary>
    ''' Permet d'obtenir les unités administratives associées à un utilisateur.
    ''' </summary>
    ''' <param name="pDctUtilisateurAssgnRole">Un dictionnaire d'assignations de rôles, indexé sur l'ID de l'utilisateur.</param>
    ''' <param name="pDctRole">Un dictionnaire de rôle, indexé sur le ID du rôle.</param>
    ''' <param name="pDctDistinctContexte">Un dictionnaire de contextes, indexé sur le Titre du contexte.</param>
    ''' <param name="pAssociationEtrangerePermi">Inquie si les associations étrangères sont permi d'être identifiées.</param>
    ''' <param name="pDctUtilisateur">Le dictionnaire des utilisateurs.</param>
    ''' <returns>Un dictionnaire d'associations d'unités administratives, indexé sur le ID de l'utilisateur.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirUaAssocEmploye(ByVal pDctUtilisateurAssgnRole As Dictionary(Of String, List(Of TsCdAssignationRole)), _
                                               ByVal pDctRole As Dictionary(Of String, TsCdRole), _
                                               ByVal pDctDistinctContexte As Dictionary(Of String, TsDtSourceContexteUA), _
                                               ByVal pAssociationEtrangerePermi As Boolean, _
                                               ByVal pDctUtilisateur As Dictionary(Of String, TsCdUtilisateur)) As Dictionary(Of String, List(Of TsDtSourceUaAssociee))

        Dim dctUtilisateurUaAssoc As New Dictionary(Of String, List(Of TsDtSourceUaAssociee))

        For Each pair In pDctUtilisateurAssgnRole
            For Each ua In pair.Value

                Dim nom As String = ua.Nom
                Dim valeur As String = "X"

                ' Si le type d'unité administrative est un contexte.
                If ua.Type.ToUpper = "RET_C_CONTEXTE" Then
                    ' Découper son identifiant pour trouvé sont nom de base.
                    Dim lstSections = ua.ID.Split("_"c)
                    Dim IdRole As String = ""
                    For i = 0 To lstSections.Count - 2
                        If IdRole <> "" Then IdRole &= "_"
                        IdRole &= lstSections(i)
                    Next

                    If pDctRole.ContainsKey(IdRole) Then
                        ' Touver le rôle de base.
                        nom = pDctRole(IdRole).Nom

                        ' Associer le symbole du lien entre l'utilisateur et l'unité administrative.
                        valeur = pDctDistinctContexte(lstSections.Last).Symbole
                    End If
                End If

                ' Construire l'association.
                Dim uaAssoc As New TsDtSourceUaAssociee
                With uaAssoc
                    .Titre = nom
                    .ValeurAssociee = valeur

                    If pAssociationEtrangerePermi Then
                        .ValeurAssocieeEtrangere = Not ua.ListeUniteAdministrativeResponsable.Contains(pDctUtilisateur(pair.Key).NoUniteAdmin)
                    End If
                End With

                ' L'ajouté de facon unique dans le dictionnaire, indexé sur le ID de l'utilisateur.
                If dctUtilisateurUaAssoc.ContainsKey(pair.Key) Then
                    dctUtilisateurUaAssoc(pair.Key).Add(uaAssoc)
                Else
                    Dim lst As New List(Of TsDtSourceUaAssociee)
                    lst.Add(uaAssoc)
                    dctUtilisateurUaAssoc.Add(pair.Key, lst)
                End If

            Next
        Next

        Return dctUtilisateurUaAssoc
    End Function

    ''' <summary>
    ''' Permet d'obtenir la liste des employés source et leur liste d'Ua associé.
    ''' </summary>
    ''' <param name="pDctUtilisateurs">Un dictionnaire d'utilisateur, indexé sur le ID de l'utilisateur.</param>
    ''' <param name="pDctUtilisateurUaAssoc">Un dictionnaire d'associations d'unités administratives, indexé sur le ID de l'utilisateur.</param>
    ''' <returns>Une liste d'employés.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirEmployes(ByVal pDctUtilisateurs As Dictionary(Of String, TsCdUtilisateur), _
                                     ByVal pDctUtilisateurUaAssoc As Dictionary(Of String, List(Of TsDtSourceUaAssociee)) _
                                     ) As List(Of TsDtSourceEmploye)

        Dim lstEmployes As New List(Of TsDtSourceEmploye)

        For Each u In pDctUtilisateurs.Values
            Dim employe As New TsDtSourceEmploye
            With employe
                .Nom = u.NomComplet ' String.Format("{0}, {1}", u.Nom, u.Prenom)
                .NoUA = CInt(u.NoUniteAdmin)

                If pDctUtilisateurUaAssoc.ContainsKey(u.ID) Then .UniteAdmnsAssociees = pDctUtilisateurUaAssoc(u.ID).ToList()
            End With
            lstEmployes.Add(employe)
        Next

        Return lstEmployes
    End Function

    ''' <summary>
    ''' Permet d'obtenir la liste des unités administratives.
    ''' </summary>
    ''' <param name="pDctUtilisateurAssgnRole">Dictionnaire d'assignations de rôles, indexé sur l'ID de l'utilisateur.</param>
    ''' <param name="pDctRole">Dictionnaire de rôles, indexé sur le ID du rôle.</param>
    ''' <param name="pDctRoleContexte">Dictionnaire de contextes de rôles, indexé sur l'ID du rôle.</param>
    ''' <returns>Un dictionnaire d'unités administratives, indexé sur le Nom de l'unité.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirUnitesAdministrative(ByVal pDctUtilisateurAssgnRole As Dictionary(Of String, List(Of TsCdAssignationRole)), _
                                                 ByVal pDctRole As Dictionary(Of String, TsCdRole), _
                                                 ByVal pDctRoleContexte As Dictionary(Of String, List(Of TsDtSourceContexteUA)) _
                                                 ) As Dictionary(Of String, TsDtSourceUa)

        Dim dctUaDisponible As New Dictionary(Of String, TsDtSourceUa)

        For Each r In pDctRole.Values
            Dim uaDisponible As New TsDtSourceUa()
            With uaDisponible
                .Nom = r.Nom
                .Type = TsDtSourceUa.TypeRoleUA.Metier
                If r.ID.StartsWith("RET") Then .Type = TsDtSourceUa.TypeRoleUA.Tache
                If r.ID.StartsWith("REO") Then .Type = TsDtSourceUa.TypeRoleUA.REO
                .ContextesPermis = If(pDctRoleContexte.ContainsKey(r.ID), (From c In pDctRoleContexte(r.ID) Select c.Titre).ToList, New List(Of String))
            End With

            If dctUaDisponible.ContainsKey(uaDisponible.Nom) = False Then dctUaDisponible.Add(uaDisponible.Nom, uaDisponible)
        Next

        Return dctUaDisponible
    End Function

    ''' <summary>
    ''' Permet d'ajouter les rôles qui ne font pas partis de l'UA, mais dont les utilisateurs ont des liens.
    ''' </summary>
    ''' <param name="pDctUtilisateurAssgnRole">Un dictionnaire d'assigantions de rôles.</param>
    ''' <param name="pDctRole">Le dictionnaire de rôle.</param>
    ''' <remarks></remarks>
    Private Sub AjouterRoleUtilisateur(ByVal pDctUtilisateurAssgnRole As Dictionary(Of String, List(Of TsCdAssignationRole)), _
                                       ByVal pDctRole As Dictionary(Of String, TsCdRole))
        Dim dctRoleTotal As New Dictionary(Of String, TsCdRole)
        For Each r As TsCdRole In TsCaServiceGestnAcces.RechercherRole("")
            If dctRoleTotal.ContainsKey(r.ID) = False Then
                'T208704 : patch pour garder les roles qui commence par RET, rem et reo seulement
                If r.ID.StartsWith("RET_") Or r.ID.StartsWith("REM_") Or r.ID.StartsWith("REO_") Then
                    dctRoleTotal.Add(r.ID, r)
                End If
            End If

        Next

        For Each uar In pDctUtilisateurAssgnRole
            For Each ar In uar.Value
                Dim id = ar.ID
                If ar.Type.ToUpper = "RET_C_CONTEXTE" Then
                    id = ObtenirNomTacheContexte(ar.ID)
                End If
                If pDctRole.ContainsKey(id) = False Then
                    pDctRole.Add(id, dctRoleTotal(id))
                End If
            Next
        Next
    End Sub

#End Region

#Region "--- Sous-fonctions feuille secondaire ---"

    ''' <summary>
    ''' Obtenir les rôles des unités administratives.
    ''' </summary>
    ''' <param name="pLstUnitesAdministrative">Liste des unités administratives.</param>
    ''' <returns>Une liste de rôles.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirRoles2(pLstUnitesAdministrative As List(Of String)) As List(Of TsCdRole)
        'Return (From r In mDctRoles.Values _
        '        Where r.ListeUniteAdministrativeResponsable.Intersect(pLstUnitesAdministrative).Count > 0 _
        '        Select r).ToList

        Return (From r In mDctRoles.Values _
                Where (r.ID.StartsWith("RET_") Or r.ID.StartsWith("REM_") Or r.ID.StartsWith("REO_")) And r.ListeUniteAdministrativeResponsable.Intersect(pLstUnitesAdministrative).Count > 0 _
                Select r).ToList

    End Function

    ''' <summary>
    ''' Obtenir les utilisateurs relié aux rôles.
    ''' </summary>
    ''' <param name="pLstRole">Liste des rôles.</param>
    ''' <returns>Une liste de rôles.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirUtilisateursRoles(pLstRole As List(Of TsCdRole)) As List(Of TsCdUtilisateur)
        Dim lstRoleId As List(Of String) = (From r In pLstRole Select r.ID).ToList
        Return (From u In mDctUtilisateurRolesIndirect _
                Where u.Value.Intersect(lstRoleId).Count > 0 _
                Select mDctUtilisateurs(u.Key)).ToList
    End Function

    ''' <summary>
    ''' Permet d'obtenir la liste des employés source et leur liste d'Ua associé.
    ''' </summary>
    ''' <param name="pLstUtilisateurs">Une liste d'utilisateurs.</param>
    ''' <returns>Une liste d'employés.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirEmployes(ByVal pLstUtilisateurs As List(Of TsCdUtilisateur)) As List(Of TsDtSourceEmploye)

        Dim lstEmployes As New List(Of TsDtSourceEmploye)

        For Each u In pLstUtilisateurs
            Dim employe As New TsDtSourceEmploye
            With employe
                .Nom = u.NomComplet ' String.Format("{0}, {1}", u.Nom, u.Prenom)
                .NoUA = CInt(u.NoUniteAdmin)

                .UniteAdmnsAssociees = ObtenirUaAssocEmploye(u, True)
            End With
            lstEmployes.Add(employe)
        Next

        Return lstEmployes
    End Function

    ''' <summary>
    ''' Permet d'obtenir les unités administratives associées à un utilisateur.
    ''' </summary>
    ''' <param name="pUtilisateur">L'utilisateur concerné.</param>
    ''' <param name="pAssociationEtrangerePermi">Inquie si les associations étrangères sont permi d'être identifiées.</param>
    ''' <returns>Une liste d'associations d'unités administratives.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirUaAssocEmploye(pUtilisateur As TsCdUtilisateur, pAssociationEtrangerePermi As Boolean) As List(Of TsDtSourceUaAssociee)
        Dim lstSourceUaAssociee As New List(Of TsDtSourceUaAssociee)

        For Each idRole In mDctUtilisateurRolesIndirect(pUtilisateur.ID)
            Dim role As TsCdRole = mDctRoles(idRole)

            Dim nom As String = role.Nom
            Dim valeur As String = "X"

            ' Si le type d'unité administrative est un contexte.
            If role.Type.ToUpper = "RET_C_CONTEXTE" Then
                ' Découper son identifiant pour trouvé sont nom de base.
                Dim lstSections = role.ID.Split("_"c)
                Dim idRoleBase As String = ""
                For i = 0 To lstSections.Count - 2
                    If idRoleBase <> "" Then idRoleBase &= "_"
                    idRoleBase &= lstSections(i)
                Next

                If mDctRoles.ContainsKey(idRoleBase) Then
                    ' Touver le rôle de base.
                    nom = mDctRoles(idRoleBase).Nom

                    ' Associer le symbole du lien entre l'utilisateur et l'unité administrative.
                    valeur = mDctContextes(lstSections.Last).Symbole
                End If
            End If

            ' Construire l'association.
            If role.Type.ToUpper <> "RET_C_GENERIQUE" Then

                Dim uaAssoc As New TsDtSourceUaAssociee
                With uaAssoc
                    .Titre = nom
                    .ValeurAssociee = valeur

                    If pAssociationEtrangerePermi Then
                        .ValeurAssocieeEtrangere = Not EstAssociationNonEtrangere(pUtilisateur, role)
                    End If
                End With

                lstSourceUaAssociee.Add(uaAssoc)

            End If
        Next

        Return lstSourceUaAssociee
    End Function

    ''' <summary>
    ''' Permet de définir si un rôle est une association non étrangère à l'utilisateur.
    ''' Vérifie aussi les rôles fils du rôle.
    ''' </summary>
    ''' <param name="pUtilisateur">L'utilisateur à vérifier.</param>
    ''' <param name="pRole">Le rôle vérifier.</param>
    ''' <returns>Vrai si l'association utilisateur\rôle est non étrangère.</returns>
    ''' <remarks></remarks>
    Private Function EstAssociationNonEtrangere(pUtilisateur As TsCdUtilisateur, pRole As TsCdRole) As Boolean
        If pRole.ListeUniteAdministrativeResponsable.Contains(pUtilisateur.NoUniteAdmin) = True Then
            ' Condition d'arrêt.
            Return True
        End If

        ' Trouver tous les rôles fils du rôle.
        Dim ensembleRolefils As New HashSet(Of String)({pRole.ID})
        Dim ensembleCourant As HashSet(Of String)
        Do
            ensembleCourant = New HashSet(Of String)(ensembleRolefils)

            For Each j In ensembleCourant
                If mDctRoleRoles.ContainsKey(j) Then
                    For Each i In mDctRoleRoles(j)
                        ensembleRolefils.Add(i)
                    Next
                End If
            Next
        Loop While ensembleCourant.Count <> ensembleRolefils.Count

        ' Vérifier si dans les rôles parents l'association est non étrangère.
        For Each r In ensembleRolefils
            If mDctRoles(r).ListeUniteAdministrativeResponsable.Contains(pUtilisateur.NoUniteAdmin) = True Then
                Return True
            End If
        Next

        Return False
    End Function

    ''' <summary>
    ''' Obtenir les contextes des rôles.
    ''' </summary>
    ''' <param name="pLstRole">Les rôles conscernés.</param>
    ''' <returns>Liste de contextes.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirContextes(pLstRole As List(Of TsCdRole)) As List(Of TsDtSourceContexteUA)
        Dim dctContexte As New Dictionary(Of String, TsDtSourceContexteUA)

        For Each r In pLstRole
            If mDctRoleContextes.ContainsKey(r.ID) Then
                Dim hsTitresContextes As HashSet(Of String) = mDctRoleContextes(r.ID)

                For Each tc In hsTitresContextes
                    If dctContexte.ContainsKey(tc) = False Then
                        dctContexte.Add(tc, mDctContextes(tc))
                    End If
                Next
            End If
        Next

        Return dctContexte.Values.ToList
    End Function

    ''' <summary>
    ''' Permet d'obtenir la liste des sources des rôles unités administratives.
    ''' </summary>
    ''' <param name="pLstRole">Liste des rôles concernés.</param>
    ''' <returns>Un dictionnaire d'unités administratives, indexé sur le Nom de l'unité.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirRoleUnitesAdministrative(ByVal pLstRole As List(Of TsCdRole)) As List(Of TsDtSourceUa)
        Dim lstRoleUaDisponible As New List(Of TsDtSourceUa)

        For Each r In pLstRole
            Dim uaDisponible As New TsDtSourceUa()

            With uaDisponible
                .Nom = r.Nom
                .Type = TsDtSourceUa.TypeRoleUA.Metier
                If r.ID.StartsWith("RET") Then .Type = TsDtSourceUa.TypeRoleUA.Tache
                If r.ID.StartsWith("REO") Then .Type = TsDtSourceUa.TypeRoleUA.REO

                If mDctRoleContextes.ContainsKey(r.ID) Then
                    .ContextesPermis = mDctRoleContextes(r.ID).ToList
                Else
                    .ContextesPermis = New List(Of String)
                End If
            End With

            lstRoleUaDisponible.Add(uaDisponible)

        Next

        Return lstRoleUaDisponible
    End Function


#End Region

#Region "--- Fonctions privées ---"
    Private Sub EnleverRolesNonVoulus()
        Dim TempdctRoleRoles As New System.Collections.Generic.Dictionary(Of String, System.Collections.Generic.HashSet(Of String))

        'T2028704 : patch pour enlever tous les roles qui ne commence pas par : REM RET et REO
        For Each role In mDctRoleRoles
            If role.Key.StartsWith("RET_") Or role.Key.StartsWith("REO_") Or role.Key.StartsWith("REM_") Then
                TempdctRoleRoles.Add(role.Key, role.Value)
            End If
        Next

        mDctRoleRoles = TempdctRoleRoles
    End Sub

    ''' <summary>
    ''' Permet de complété le dictionnaire des associations utilisateur\rôle\sour-rôle.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CompleterDctUtilisateurRolesIndirect()
        For Each u In mDctUtilisateurRoles.Keys
            Dim ensembleTemporaire As New HashSet(Of String)
            Dim indicateur As Integer = 0
            mDctUtilisateurRolesIndirect.Add(u, New HashSet(Of String)(mDctUtilisateurRoles(u)))

            While mDctUtilisateurRolesIndirect(u).Count <> indicateur
                ensembleTemporaire = New HashSet(Of String)
                indicateur = mDctUtilisateurRolesIndirect(u).Count

                For Each r In mDctUtilisateurRolesIndirect(u)
                    'T208704 : patch pour n'afficher que les REM, RET et REO dans le rapport
                    If r.StartsWith("RET_") Or r.StartsWith("REM_") Or r.StartsWith("REO_") Then
                        ensembleTemporaire.Add(r)
                        If mDctRoleRoles.ContainsKey(r) Then
                            For Each sr In mDctRoleRoles(r)
                                ensembleTemporaire.Add(sr)
                            Next
                        End If
                    End If

                Next

                mDctUtilisateurRolesIndirect(u) = ensembleTemporaire
            End While
        Next
    End Sub
    ''' <summary>
    ''' Permet de complété le dictionnaire des contexte et des role\contextes
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CompleterContextes()
        For Each r In mDctRoles.Values
            If String.IsNullOrEmpty(r.Organisation) = False Then
                Dim lstContextes As List(Of TsDtSourceContexteUA) = ConstruireContextes(r)

                mDctRoleContextes.Add(r.ID, New HashSet(Of String)(From c In lstContextes Select c.Titre))

                For Each c In lstContextes
                    If mDctContextes.ContainsKey(c.Titre) = False Then
                        mDctContextes.Add(c.Titre, c)
                    End If
                Next
            End If
        Next
    End Sub

    ''' <summary>
    ''' Permet de construire les contexte à partir d'une assigantion de rôle.
    ''' </summary>
    ''' <param name="pAssngRole">Une assignation de rôle.</param>
    ''' <returns>Liste de contextes.</returns>
    ''' <remarks></remarks>
    Private Function ConstruireContextes(ByVal pAssngRole As TsCdRole) As List(Of TsDtSourceContexteUA)
        If String.IsNullOrEmpty(pAssngRole.Organisation) Then Return New List(Of TsDtSourceContexteUA)

        Dim resultat As New List(Of TsDtSourceContexteUA)

        Dim contextesBrute = pAssngRole.Organisation.Split(";"c)

        Dim regex As New Regex("^(.*) \((.*)\)$")
        For Each cb In contextesBrute
            Dim reslt = regex.Matches(cb)
            resultat.Add(New TsDtSourceContexteUA() With {.Titre = Trim(reslt(0).Groups(1).Value), .Symbole = reslt(0).Groups(2).Value})
        Next

        Return resultat
    End Function

    ''' <summary>
    ''' Permer d'obtenir le nom d'une tâche contexté.
    ''' </summary>
    ''' <param name="pNomComplet">Le nom complet de la tâche contextée.</param>
    ''' <returns>Le nom de la tâche contexté.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirNomTacheContexte(ByVal pNomComplet As String) As String
        Dim lstSections = pNomComplet.Split("_"c)

        Dim IdRole As String = ""
        For i = 0 To lstSections.Count - 2
            If IdRole <> "" Then IdRole &= "_"
            IdRole &= lstSections(i)
        Next

        Return IdRole
    End Function

#End Region

End Class
