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
    Private mDctRessources As New Dictionary(Of String, TsCdSageResource)

    ''' <summary>
    ''' Dictionnaire des utilisateurs et de leurs rôles associés.
    ''' </summary>
    Private mDctUtilisateurRoles As Dictionary(Of String, HashSet(Of String))

    Private mDctUtilisateurRessources As Dictionary(Of String, HashSet(Of String))

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

        '   - Sélectionner tous les utilisateurs des Unités administrative. (dctUtilisateurs)
        '   - sélectionner toutes les ressources de la configuration (lstRessources)
        '   - sélectionner toutes les ressources associées aux utilisateurs (lstemployes)

        Dim dctUtilisateurs As Dictionary(Of String, TsCdUtilisateur)
        Dim lstRessources As New List(Of TsDtSourceRessources)
        Dim lstEmployes As List(Of TsDtSourceEmploye)
        Dim dctUtilisateursParUA As New Dictionary(Of String, Integer)



        'lstRessources = ObtenirRessources()
        dctUtilisateurs = ObtenirUtilisateurs(pLstUnitesAdministrative)

        lstEmployes = ObtenirEmployes(dctUtilisateurs)

        Dim NomUA As Integer = lstEmployes(0).NoUA
        Dim cmpt As Integer = 0
        'Remplir le dictionnaire du nombre d'employé par UA pour les entetes de tableau.
        'For i = 0 To lstEmployes.Count - 1

        '    Dim empl As TsDtSourceEmploye = lstEmployes(i)
        '    cmpt = cmpt + 1
        '    If NomUA = empl.NoUA Then
        '        dernier
        '        If i = lstEmployes.Count - 1 Then
        '            dctUtilisateursParUA.Add(NomUA.ToString, cmpt)
        '        End If
        '    Else
        '        mettre la valeur dans le dictionnaire
        '        dctUtilisateursParUA.Add(NomUA.ToString, cmpt)
        '        NomUA = empl.NoUA
        '        cmpt = 0
        '    End If
        'Next

        For i = 0 To lstEmployes.Count - 1

            Dim empl As TsDtSourceEmploye = lstEmployes(i)

            If NomUA <> empl.NoUA Then

                'mettre la valeur dans le dictionnaire
                dctUtilisateursParUA.Add(NomUA.ToString, cmpt)
                NomUA = empl.NoUA
                cmpt = 0
            End If
            cmpt = cmpt + 1
        Next
        dctUtilisateursParUA.Add(NomUA.ToString, cmpt)



        '--- Trier les listes et filtrer ---
        lstEmployes = (From e In lstEmployes Order By e.NoUA, e.Nom).ToList


        ' --- Remplir la source du rapport ---
        Dim source As New TsDtSourcFeuilPrinc(dateProd, pLstUnitesAdministrative)
        With source
            .Employes = lstEmployes
            .LstRessources = ObtenirRessources(lstEmployes)
            .dctUtilisateursParUA = dctUtilisateursParUA
        End With

        Return source
    End Function


#End Region

#Region "--- Sous-fonctions feuille principale ---"


    ''' <summary>
    ''' Permet d'obtenir tous les utilisateurs reliés aux unités administratives.
    ''' </summary>
    ''' <param name="pLstUnitesAdministrative">La liste des unités administratives</param>
    ''' <returns>Un dictionnaire d'utilisateur, indexé su le ID.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirUtilisateurs(ByVal pLstUnitesAdministrative As List(Of String)) As Dictionary(Of String, TsCdUtilisateur)
        Dim dctUtilisateurs As New Dictionary(Of String, TsCdUtilisateur)

        For Each uaID In pLstUnitesAdministrative
            For Each u In ObtenirUtilisateursUniteAdmin(uaID)
                If dctUtilisateurs.ContainsKey(u.ID) = False Then dctUtilisateurs.Add(u.ID, u)
            Next
        Next

        Return dctUtilisateurs
    End Function

    Public Function ObtenirUtilisateursUniteAdmin(ByVal pUniteAdminID As String) As List(Of TsCdUtilisateur)
        Dim lstUtilisateurSage As TsCdSageUserCollection = TsCuConfiguration.Utilisateurs()


        Dim uniteAdminIDCorriger As String = pUniteAdminID.Replace("*", ".*")
        Dim regex As New Regex("^" & uniteAdminIDCorriger & "$")

        Return (
                From u In lstUtilisateurSage
                Where regex.IsMatch(u.OrganizationType)
                Select TraductionUtilisateur(u)
               ).ToList


    End Function

    Friend Shared Function TraductionUtilisateur(ByVal sageUtilisateur As TsCdSageUser) As TsCdUtilisateur
        Dim paramRetour As TsCdUtilisateur = New TsCdUtilisateur()

        paramRetour.ID = sageUtilisateur.PersonID
        paramRetour.NoUniteAdmin = sageUtilisateur.OrganizationType

        paramRetour.Nom = sageUtilisateur.Nom
        paramRetour.Prenom = sageUtilisateur.Prenom
        paramRetour.NomComplet = sageUtilisateur.UserName
        paramRetour.Courriel = sageUtilisateur.Courriel
        paramRetour.Ville = sageUtilisateur.Ville

        paramRetour.DateFin = sageUtilisateur.DateFin
        If sageUtilisateur.DateFin = Nothing Then
            paramRetour.FinPrevue = False
        Else
            paramRetour.FinPrevue = True
        End If

        paramRetour.DateApprobation = sageUtilisateur.DateApprobation
        If sageUtilisateur.DateApprobation = Nothing Then
            paramRetour.ApprobationAccepter = False
        Else
            paramRetour.ApprobationAccepter = True
        End If
        ' paramRetour.protection = NiveauProtection.Totale

        Return paramRetour
    End Function


    ''' <summary>
    ''' Permet d'obtenir la liste des employés source et leur liste d'Ua associé.
    ''' </summary>
    ''' <param name="pDctUtilisateurs">Un dictionnaire d'utilisateur, indexé sur le ID de l'utilisateur.</param>
    ''' <returns>Une liste d'employés.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirEmployes(ByVal pDctUtilisateurs As Dictionary(Of String, TsCdUtilisateur)) As List(Of TsDtSourceEmploye)

        Dim lstEmployes As New List(Of TsDtSourceEmploye)

        For Each u In pDctUtilisateurs.Values
            Dim employe As New TsDtSourceEmploye
            With employe
                .Nom = u.NomComplet 'String.Format("{0}, {1}", u.Nom, u.Prenom)
                .NoUA = CInt(u.NoUniteAdmin)

                'If pDctUtilisateurUaAssoc.ContainsKey(u.ID) Then .UniteAdmnsAssociees = pDctUtilisateurUaAssoc(u.ID).ToList()

                .lstRessourcesAssociees = ObtenirRessourcesUtilisateurs(u.ID)
            End With
            lstEmployes.Add(employe)
        Next

        Return lstEmployes
    End Function


#End Region

    Public Function ObtenirRessourcesUtilisateurs(ByVal UtilisateurId As String) As List(Of TsDtSourceRessources)
        Dim lstLiens As TsCdSageResourceCollection
        Dim lstRessourcesAssociees As New List(Of TsDtSourceRessources)
        lstLiens = TsCuConfiguration.ObtenirRelationURe(UtilisateurId)

        For Each Res As TsCdSageResource In lstLiens
            lstRessourcesAssociees.Add(New TsDtSourceRessources(Res.ResName1, Res.NomFonctionnelOuDescription))
        Next

        Return lstRessourcesAssociees
    End Function






    Public Function ObtenirRessources(ByVal pLstEmployes As List(Of TsDtSourceEmploye)) As List(Of TsDtSourceRessources)
        Dim lstRes As New List(Of TsDtSourceRessources)
        Dim lstResEmpl As New List(Of TsDtSourceRessources)

        'construire la liste des ressources des utilisateurs
        For Each Employes As TsDtSourceEmploye In pLstEmployes
            lstResEmpl.AddRange(Employes.lstRessourcesAssociees)
        Next

        For Each Ressource As TsCdSageResource In TsCuConfiguration.Ressources()
            Dim Res As IEnumerable(Of TsDtSourceRessources) = lstResEmpl.Where(Function(x) x.Nom = Ressource.ResName1)
            If Not (Res Is Nothing OrElse Res.Count = 0) Then
                lstRes.Add(New TsDtSourceRessources(Ressource.ResName1, Ressource.NomFonctionnelOuDescription))
            End If

        Next


        lstRes = (From e In lstRes Order By e.Nom, e.Nom).ToList




        Return lstRes
    End Function

End Class
