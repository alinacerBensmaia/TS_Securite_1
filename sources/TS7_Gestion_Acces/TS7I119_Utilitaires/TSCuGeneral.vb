Imports Rrq.Securite.GestionAcces
Imports Rrq.Web.GabaritsPetitsSystemes.Utilitaires
Imports System.Collections.Generic
Imports System.Linq


Public Class TSCuGeneral

    Public Shared Sub EnvoyerCourriel(ByVal strObjet As String, _
                                             ByVal strMessage As String, _
                                             ByVal strExpediteur As String, _
                                             ByVal strDestinataire As String, _
                                    Optional ByVal blnFormatHTML As Boolean = False)

        Dim objCourriel As New Rrq.InfrastructureCommune.UtilitairesCommuns.XuCuEnvoiCourriel

        objCourriel.Destinataire = strDestinataire
        objCourriel.Expediteur = strExpediteur

        objCourriel.Objet = strObjet
        objCourriel.Message = strMessage
        If blnFormatHTML Then
            objCourriel.FormatMessage = InfrastructureCommune.UtilitairesCommuns.XuCuEnvoiCourriel.XuEcFormatMessage.XuEcFmHTML
        End If
        objCourriel.EnvoyerCourriel()

        objCourriel = Nothing

    End Sub

    Public Shared Sub EnvoyerCourriel(ByVal strObjet As String, _
                                             ByVal strMessage As String, _
                                             ByVal strExpediteur As String, _
                                             ByVal strDestinataire As String, _
                                             ByVal CopieConforme As String, _
                                    Optional ByVal blnFormatHTML As Boolean = False)

        Dim objCourriel As New Rrq.InfrastructureCommune.UtilitairesCommuns.XuCuEnvoiCourriel

        objCourriel.Destinataire = strDestinataire
        objCourriel.Expediteur = strExpediteur
        If Not String.IsNullOrEmpty(CopieConforme) Then
            objCourriel.CopieConforme = CopieConforme
        End If


        objCourriel.Objet = strObjet
        objCourriel.Message = strMessage
        If blnFormatHTML Then
            objCourriel.FormatMessage = InfrastructureCommune.UtilitairesCommuns.XuCuEnvoiCourriel.XuEcFormatMessage.XuEcFmHTML
        End If
        objCourriel.EnvoyerCourriel()

        objCourriel = Nothing

    End Sub
    ''' <summary>
    ''' Fonction de traduction.
    ''' </summary>
    ''' <param name="utilisateurs"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' listeColonnes()=        {"ID", "NomComplet",   "Prenom", "Nom", "NoUniteAdm", "FinPrevue","DateFin","Courriel","Ville", "NomPrenom"}
    ''' listeInformations()=    {u.ID, u.NomComplet,u.Prenom, u.Nom, u.NoUniteAdmin,  u.FinPrevue.ToString,   u.DateFin.ToString, u.Courriel,u.Ville, u.NomPrenom}
    ''' Si vous changé quelque chose dans une des 2 listes, il faut le changé dans l'autre pour que la logique suive.

    Public Shared Function UtilisateurUniqueDataTable(ByVal utilisateurs As TsCdUtilisateur) As DataTable
        Dim paramRetour As New DataTable()
        NiCuADO.AjouterDtColonne(paramRetour, "ID", System.Type.GetType("System.String"), 7, "", True)
        NiCuADO.AjouterDtColonne(paramRetour, "NomComplet", System.Type.GetType("System.String"), 120, "", True)
        NiCuADO.AjouterDtColonne(paramRetour, "Prenom", System.Type.GetType("System.String"), 60, "", True)
        NiCuADO.AjouterDtColonne(paramRetour, "Nom", System.Type.GetType("System.String"), 60, "", True)
        NiCuADO.AjouterDtColonne(paramRetour, "NoUniteAdm", System.Type.GetType("System.String"), 20, "", True)
        NiCuADO.AjouterDtColonne(paramRetour, "FinPrevue", System.Type.GetType("System.String"), 20, "", True)
        NiCuADO.AjouterDtColonne(paramRetour, "DateFin", System.Type.GetType("System.String"), 20, "", True)
        NiCuADO.AjouterDtColonne(paramRetour, "Courriel", System.Type.GetType("System.String"), 100, "", True)
        NiCuADO.AjouterDtColonne(paramRetour, "Ville", System.Type.GetType("System.String"), 60, "", True)
        NiCuADO.AjouterDtColonne(paramRetour, "NomPrenom", System.Type.GetType("System.String"), 120, "", True)

        Dim drParamRetour As DataRow = paramRetour.NewRow
        drParamRetour("ID") = utilisateurs.ID
        drParamRetour("NomComplet") = utilisateurs.NomComplet
        drParamRetour("Prenom") = utilisateurs.Prenom
        drParamRetour("Nom") = utilisateurs.Nom
        drParamRetour("NoUniteAdm") = utilisateurs.NoUniteAdmin
        drParamRetour("FinPrevue") = utilisateurs.FinPrevue
        drParamRetour("DateFin") = IIf(utilisateurs.DateFin.Equals(Nothing), "", utilisateurs.DateFin.ToShortDateString)
        'drParamRetour("DateFin") = utilisateurs.DateFin
        drParamRetour("Courriel") = utilisateurs.Courriel
        drParamRetour("Ville") = utilisateurs.Ville
        drParamRetour("NomPrenom") = String.Concat(utilisateurs.Nom, " ", utilisateurs.Prenom)
        paramRetour.Rows.Add(drParamRetour)


        Return paramRetour
    End Function



    ''' <summary>
    ''' Fonction de traduction.
    ''' </summary>
    ''' <param name="utilisateurs"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' listeColonnes()=        {"ID", "NomComplet",   "Prenom", "Nom", "NoUniteAdm", "FinPrevue","DateFin","Courriel","Ville", "NomPrenom"}
    ''' listeInformations()=    {u.ID, u.NomComplet,u.Prenom, u.Nom, u.NoUniteAdmin,  u.FinPrevue.ToString,   u.DateFin.ToString, u.Courriel,u.Ville, u.NomPrenom}
    ''' Si vous changé quelque chose dans une des 2 listes, il faut le changé dans l'autre pour que la logique suive.

    Public Shared Function UtilisateurDataTable(ByVal utilisateurs As List(Of TsCdUtilisateur)) As DataTable
        Dim paramRetour As New DataTable()

        ' C'est une liste de noms des colonnes
        Dim listeColonnes() As String = {"ID", "NomComplet", "Prenom", "Nom", "NoUniteAdm",
        "FinPrevue", "DateFin", "Courriel", "Ville", "NomPrenom"}

        For Each c As String In listeColonnes
            Dim colonne As New DataColumn()
            colonne.ColumnName = c
            colonne.DataType = Type.GetType("System.String")
            paramRetour.Columns.Add(colonne)
        Next


        For Each u As TsCdUtilisateur In utilisateurs
            ' C'est une liste de données aussi longue et trié comme la liste des noms des colonnes. L'ordre est très important.
            Dim listeInformations() As String = {u.ID, u.NomComplet, u.Prenom, u.Nom, _
            u.NoUniteAdmin, u.FinPrevue.ToString, IIf(u.DateFin.Equals(Nothing), "", u.DateFin.ToShortDateString).ToString, u.Courriel, u.Ville, u.Nom & " " & u.Prenom}

            Dim ligne As DataRow = paramRetour.NewRow()

            For i As Integer = 0 To listeColonnes.Length - 1
                ligne(listeColonnes(i)) = listeInformations(i)
            Next
            paramRetour.Rows.Add(ligne)
        Next

        Return paramRetour
    End Function

    ''' <summary>
    ''' Fonction de traduction.
    ''' </summary>
    ''' <param name="UniteAdmin"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' listeColonnes()=        {"Abbreviation", "IDRole",   "No", "Nom",   "NoAbbreviation"}
    ''' listeInformations()=    {u.Abbreviation, u.IDRole,    u.No, u.Nom,   "u.NoAbbreviation"}
    ''' Si vous changé quelque chose dans une des 2 listes, il faut le changé dans l'autre pour que la logique suive.

    Public Shared Function UniteAdminDataTable(ByVal UniteAdmin As List(Of TsCdUniteAdministrative)) As DataTable
        Dim paramRetour As New DataTable()

        ' C'est une liste de noms des colonnes
        Dim listeColonnes() As String = {"Abbreviation", "IDRole", "No", "Nom", "NoAbbreviation"}

        For Each c As String In listeColonnes
            Dim colonne As New DataColumn()
            colonne.ColumnName = c
            colonne.DataType = Type.GetType("System.String")
            paramRetour.Columns.Add(colonne)
        Next


        For Each u As TsCdUniteAdministrative In UniteAdmin
            ' C'est une liste de données aussi longue et trié comme la liste des noms des colonnes. L'ordre est très important.
            Dim listeInformations() As String = {u.Abbreviation, u.IDRole, u.No, u.Nom, u.No & "-" & u.Abbreviation}

            Dim ligne As DataRow = paramRetour.NewRow()

            For i As Integer = 0 To listeColonnes.Length - 1
                ligne(listeColonnes(i)) = listeInformations(i)
            Next
            paramRetour.Rows.Add(ligne)
        Next

        Return paramRetour
    End Function

    Public Shared Function UniteAdminDataTable(ByVal UniteAdmin As List(Of String)) As DataTable
        Dim paramRetour As New DataTable()

        ' C'est une liste de noms des colonnes
        Dim listeColonnes() As String = {"No"}

        For Each c As String In listeColonnes
            Dim colonne As New DataColumn()
            colonne.ColumnName = c
            colonne.DataType = Type.GetType("System.String")
            paramRetour.Columns.Add(colonne)
        Next


        For Each u As String In UniteAdmin

            Dim listeInformations() As String = {u}

            Dim ligne As DataRow = paramRetour.NewRow()

            For i As Integer = 0 To listeColonnes.Length - 1
                ligne(listeColonnes(i)) = listeInformations(i)
            Next
            paramRetour.Rows.Add(ligne)
        Next

        Return paramRetour
    End Function

    ''' <summary>
    ''' Fonction de traduction.
    ''' </summary>
    ''' <param name="Equip"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' listeColonnes()=        {"IDRole", "Nom", "NoUniteAdmin"}
    ''' listeInformations()=    {u.IDRole, u.Nom, u.NoUniteAdmin}
    ''' Si vous changé quelque chose dans une des 2 listes, il faut le changé dans l'autre pour que la logique suive.

    Public Shared Function EquipDataTable(ByVal Equip As List(Of TsCdEquipe)) As DataTable
        Dim paramRetour As New DataTable()

        ' C'est une liste de noms des colonnes
        Dim listeColonnes() As String = {"IDRole", "Nom", "NoUniteAdmin"}

        For Each c As String In listeColonnes
            Dim colonne As New DataColumn()
            colonne.ColumnName = c
            colonne.DataType = Type.GetType("System.String")
            paramRetour.Columns.Add(colonne)
        Next


        For Each u As TsCdEquipe In Equip
            ' C'est une liste de données aussi longue et trié comme la liste des noms des colonnes. L'ordre est très important.
            Dim listeInformations() As String = {u.IDRole, u.Nom, u.NoUniteAdmin}

            Dim ligne As DataRow = paramRetour.NewRow()

            For i As Integer = 0 To listeColonnes.Length - 1
                ligne(listeColonnes(i)) = listeInformations(i)
            Next
            paramRetour.Rows.Add(ligne)
        Next

        Return paramRetour
    End Function


    
    Public Shared Function RoleDataTable(ByVal Role As List(Of TsCdRole)) As DataTable
        Dim paramRetour As New DataTable()
        Dim strUAResponsable As String = String.Empty
       
        ' C'est une liste de noms des colonnes
        'Contexte : Vide à ce niveau mais à l'assignation du role contexte, contiendra la valeur du contexte choisi.
        'LienTachesMetiers : Correspondance Metier/Taches.  Pour validation si on ajoute un role d'une catégorie, il faut ajouter le role de l'autre catégorie.
        'DomValContexte: Contient tous les contexte possible pour un role.  Sert a afficher dans une liste de roles assignés dans "GererRole"
        'ContexteOrigine : Pour un role assigné, on lui met la valeur de son contexte au départ.  A la confirmation, si le contexte d'origine est différente
        '                   du contexte, c'est que l'utilisateur a changé le contexte d'un role.
        'NomAAfficher : Dans la page confirmation, il faut afficher le nom du role générique.  Si c'est pas un role avec contexte,
        '               cette valeur est la même que le nom.
        Dim listeColonnes() As String = {"Description", "ID", "Nom", "Particulier", "Organisationnel", "ListeUniteAdministrativeResponsable", "Contexte", "LienTachesMetiers", "DomValContexte", "ContexteOrigine", "NomAAfficher"}

        For Each c As String In listeColonnes
            Dim colonne As New DataColumn()
            colonne.ColumnName = c
            colonne.DataType = Type.GetType("System.String")
            paramRetour.Columns.Add(colonne)
        Next

        Dim blnAAjouter As Boolean = False
        For Each u As TsCdRole In Role
            blnAAjouter = False
            'Si c'est un role avec contexte, on garde seulement le générique.
            If InStr(u.ID, "RET_C_") > 0 Then
                'AVEC CONTEXTE
                'C'est un role générique quand il n'y a plus de "_" dans l'id apres le RET_C_"
                Dim strRoleTemp As String = u.ID.ToString.Replace("RET_C_", "")
                If Not InStr(strRoleTemp, "_") > 0 Then
                    'C'est le générique, on l'ajoute à la liste des roles
                    blnAAjouter = True
                   
                    For Each UniteAdm As String In u.ListeUniteAdministrativeResponsable
                        If strUAResponsable = String.Empty Then
                            strUAResponsable = UniteAdm
                        Else
                            strUAResponsable = strUAResponsable & ";" & UniteAdm
                        End If

                    Next

                End If
            Else
                'SANS CONTEXTE
                blnAAjouter = True
                strUAResponsable = String.Empty

                For Each UniteAdm As String In u.ListeUniteAdministrativeResponsable
                    If strUAResponsable = String.Empty Then
                        strUAResponsable = UniteAdm
                    Else
                        strUAResponsable = strUAResponsable & ";" & UniteAdm
                    End If

                Next
            End If

            If blnAAjouter Then

                ' C'est une liste de données aussi longue et trié comme la liste des noms des colonnes. L'ordre est très important.
                Dim listeInformations() As String = {u.Description, u.ID, u.Nom, u.Particulier.ToString, u.Organisationnel.ToString, strUAResponsable, "", u.Organisation2, u.Organisation, "", u.Nom}

                Dim ligne As DataRow = paramRetour.NewRow()

                For i As Integer = 0 To listeColonnes.Length - 1
                    ligne(listeColonnes(i)) = listeInformations(i)
                Next
                paramRetour.Rows.Add(ligne)
            End If

        Next

        Return paramRetour
    End Function

    ''' <summary>
    ''' Fonction de traduction.
    ''' </summary>
    ''' <param name="AssignationRole"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' listeColonnes()=        {"Description", "ID", "Nom" , "Particulier", "Organisationnel"}
    ''' listeInformations()=    {u.Description, u.ID,    u.Nom, u.Particulier, U.Organisationnel}
    ''' Si vous changé quelque chose dans une des 2 listes, il faut le changé dans l'autre pour que la logique suive.

    Public Shared Function AssignationRoleDataTable(ByVal AssignationRole As List(Of TsCdAssignationRole)) As DataTable
        Dim paramRetour As New DataTable()

        Dim strDescription As String = String.Empty
        Dim strDateFin As String = String.Empty
        Dim strFinPrevu As String = String.Empty
        Dim strID As String = String.Empty
        Dim strParticulier As String = String.Empty
        Dim strOrganisationnel As String = String.Empty
        Dim strUAResponsable As String = String.Empty
        Dim strContexte As String = String.Empty
        Dim strLien As String = String.Empty
        Dim strDomVal As String = String.Empty
        Dim strContexteOrigine As String = String.Empty
        Dim strNom As String = String.Empty

        ' C'est une liste de noms des colonnes
        Dim listeColonnes() As String = {"DateFin", "Description", "FinPrevu", "ID", "Nom", "Particulier", "Organisationnel", "ListeUniteAdministrativeResponsable", "Contexte", "LienTachesMetiers", "DomValContexte", "ContexteOrigine", "NomAAfficher"}

        For Each c As String In listeColonnes
            Dim colonne As New DataColumn()
            colonne.ColumnName = c
            colonne.DataType = Type.GetType("System.String")
            paramRetour.Columns.Add(colonne)
        Next

        Dim blnAAjouter As Boolean = False
        For Each u As TsCdAssignationRole In AssignationRole
            blnAAjouter = False
            strUAResponsable = String.Empty
            'Pour les roles avec contexte, prendre le role générique et garder le contexte dans "Contexte" et "Contexteorigine"

            If InStr(u.ID, "RET_C_") > 0 Then
                'AVEC CONTEXTE
                'C'est un role générique quand il n'y a plus de "_" dans l'id apres le RET_C_"
                Dim strRoleTemp As String = u.ID.ToString.Replace("RET_C_", "")
                If InStr(strRoleTemp, "_") > 0 Then
                    blnAAjouter = True
                    'C'est un role contextuel, trouver le générique

                    Dim strIDRoleGenerique As String = Left(u.ID, InStrRev(u.ID, "_") - 1)

                    Dim RoleGenerique As New TsCdRole
                    RoleGenerique = RechercherRoleParID(strIDRoleGenerique)

                    If Not RoleGenerique Is Nothing Then
                        strDescription = RoleGenerique.Description
                        strDateFin = IIf(u.DateFin.Equals(Nothing), "", u.DateFin.ToShortDateString).ToString
                        strFinPrevu = u.FinPrevu.ToString
                        strID = RoleGenerique.ID
                        strNom = RoleGenerique.Nom
                        strParticulier = RoleGenerique.Particulier.ToString()
                        strOrganisationnel = RoleGenerique.Organisationnel.ToString()
                        strContexte = Mid(u.ID, InStrRev(u.ID, "_") + 1)
                        strLien = RoleGenerique.Organisation2
                        strDomVal = RoleGenerique.Organisation
                        strContexteOrigine = strContexte

                        strUAResponsable = String.Empty
                        For Each UniteAdm As String In RoleGenerique.ListeUniteAdministrativeResponsable
                            If strUAResponsable = String.Empty Then
                                strUAResponsable = UniteAdm
                            Else
                                strUAResponsable = strUAResponsable & ";" & UniteAdm
                            End If
                            'strUAResponsable &= String.Concat(UniteAdm, ";")
                        Next
                    End If
                    
                End If
            Else
                'SANS CONTEXTE
                blnAAjouter = True
                strDescription = u.Description
                strDateFin = IIf(u.DateFin.Equals(Nothing), "", u.DateFin.ToShortDateString).ToString
                strFinPrevu = u.FinPrevu.ToString
                strID = u.ID
                strNom = u.Nom
                strParticulier = u.Particulier.ToString()
                strOrganisationnel = u.Organisationnel.ToString()
                strContexte = String.Empty
                strLien = u.Organisation2
                strDomVal = u.Organisation
                strContexteOrigine = strContexte
                For Each UniteAdm As String In u.ListeUniteAdministrativeResponsable
                    If strUAResponsable = String.Empty Then
                        strUAResponsable = UniteAdm
                    Else
                        strUAResponsable = strUAResponsable & ";" & UniteAdm
                    End If
                    'strUAResponsable &= String.Concat(UniteAdm, ";")
                Next
            End If

            If blnAAjouter Then

                Dim listeInformations() As String = {strDateFin, strDescription, strFinPrevu, strID, strNom, strParticulier, strOrganisationnel, strUAResponsable, strContexte, strLien, strDomVal, strContexteOrigine, strNom}

                Dim ligne As DataRow = paramRetour.NewRow()

                For i As Integer = 0 To listeColonnes.Length - 1
                    ligne(listeColonnes(i)) = listeInformations(i)
                Next
                paramRetour.Rows.Add(ligne)
            End If

        Next

        Return paramRetour
    End Function

    
    ''' <summary>
    ''' ObtenirListeUnitAdminParRoles : Obtient toutes les unités administratives de l'utilisateur en fonction 
    ''' des roles de sécurité.
    ''' </summary>
    ''' <param name="pstrNomUtilisateur"></param>
    ''' <returns>Liste d'unité administratives</returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenirListeUnitAdminParRoles(ByVal pUtilisateur As Rrq.Web.GabaritsPetitsSystemes.Utilitaires.NiCdUtilisateurAD) As List(Of String)
        Dim lstUnitAdmin As New List(Of String)
        Dim lstRoles As String()


        lstRoles = pUtilisateur.GroupesMembreDe

        For Each roles As String In lstRoles
            lstUnitAdmin.Add(roles)

        Next

        Return lstUnitAdmin

    End Function
    ''' <summary>
    ''' Permet de savoir si le demandeur est le responsable du role.
    ''' </summary>
    ''' <param name="strUARole"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function boolResponsableRole(ByVal strUARole As String, ByVal dtListeUADemandeur As DataTable) As Boolean

        If Not String.IsNullOrEmpty(strUARole) Then

            Dim tableauUA() As String = strUARole.Split(CChar(";"))

            For cmptUARole As Integer = 0 To tableauUA.Length - 1
                For cmptUAResp As Integer = 0 To dtListeUADemandeur.Rows.Count - 1
                    If tableauUA(cmptUARole) = dtListeUADemandeur.Rows(cmptUAResp)(2).ToString() Then
                        Return True
                    End If
                Next

            Next
        End If
        Return False


    End Function

    Public Shared Function RechercherRoleParID(ByVal pID As String) As TsCdRole
        Dim RoleRetour As New TsCdRole
        Dim lstRole As New ArrayList

        lstRole = TsCaServiceGestnAcces.RechercherRole("")

        For i As Integer = 0 To lstRole.Count - 1
            Dim Role As New TsCdRole
            Role = CType(lstRole(i), TsCdRole)
            
            If Trim(Role.ID.ToUpper) = Trim(pID.ToUpper) Then
                RoleRetour = Role
            End If
        Next
        Return RoleRetour



    End Function
    ' ''' <summary>
    ' '''  'VALIDATION CORRESPONDANCE METIER/TACHES
    ' '''  Pour tous les roles ajoutés et assignés, s'assurer que tous les roles ont une correspondance Metier/Taches.
    ' ''' </summary>
    ' ''' <param name="dtRoleAssigne">DataTable des roles assignés</param>
    ' ''' <param name="dtAjoutRole">DataTable des roles ajoutés, si vide = Nothing</param>
    ' ''' <param name="strCorrespondanceErreur">(ByRef)Valeur du champs "LiensMetierTaches" en erreur</param>
    ' ''' <param name="strCorrespondanceIDRoleErreur">(ByRef)Id du role qui n'a pas de correspondance dans l'autre catégorie</param>
    ' ''' <param name="strNomRoleCorrespondanceErreur">(ByRef)Nom du role qui n'a pas de correspondance dans l'autre catégorie</param>
    ' ''' <param name="strTypeCorrespondanceErreur">(ByRef) Catégorie du role en erreur (Taches ou Metier)</param>
    ' ''' <returns>False si erreur dans les cohérences</returns>
    ' ''' <remarks></remarks>
    'Public Shared Function ValiderCoherenceMetierTaches(ByVal dtRoleAssigne As DataTable, ByVal dtAjoutRole As DataTable, ByRef strCorrespondanceErreur As String, ByRef strCorrespondanceIDRoleErreur As String, ByRef strTypeCorrespondanceErreur As String, ByRef strNomRoleCorrespondanceErreur As String) As Boolean


    '    strCorrespondanceErreur = String.Empty
    '    strCorrespondanceIDRoleErreur = String.Empty
    '    strTypeCorrespondanceErreur = String.Empty
    '    strNomRoleCorrespondanceErreur = String.Empty

    '    Dim dtCorrespondance As IEnumerable(Of DataRow) = Nothing

    '    'Obtenir tous les roles qui ont des correspondances Metier/Taches dans les datatables recues en parametre

    '    '1- les 2 datatable sont vides, on ne valide pas la cohérence.
    '    If (dtRoleAssigne Is Nothing OrElse dtRoleAssigne.Rows.Count = 0) And _
    '        (dtAjoutRole Is Nothing OrElse dtAjoutRole.Rows.Count = 0) Then
    '        Return True
    '    End If

    '    '2- Les 2 datatable ne sont pas vides, on met les roles des 2 tables dans la liste des roles à valider.
    '    If (Not dtRoleAssigne Is Nothing AndAlso dtRoleAssigne.Rows.Count > 0) And _
    '        (Not dtAjoutRole Is Nothing AndAlso dtAjoutRole.Rows.Count > 0) Then

    '        dtCorrespondance = dtAjoutRole.Select("SELECT LIKE 'O' AND LienTachesMetiers <> ''") _
    '        .Union(dtRoleAssigne.Select("strSupprimer like 'False' AND LienTachesMetiers <> ''"))

    '    End If

    '    '3- 1 des 2 datatables n'est pas vide
    '    'Role assigné vide
    '    If dtRoleAssigne Is Nothing OrElse dtRoleAssigne.Rows.Count = 0 Then
    '        dtCorrespondance = dtAjoutRole.Select("SELECT LIKE 'O' AND LienTachesMetiers <> ''")
    '    End If

    '    'role ajouté vide
    '    If dtAjoutRole Is Nothing OrElse dtAjoutRole.Rows.Count = 0 Then
    '        dtCorrespondance = dtRoleAssigne.Select("strSupprimer like 'False' AND LienTachesMetiers <> ''")
    '    End If

    '    'Verification de la cohérence pour chaque role dans la liste.
    '    For Each Correspondance As DataRow In dtCorrespondance
    '        Dim dtAComparer As IEnumerable(Of DataRow)

    '        Dim strLienCompare As String = Correspondance("LienTachesMetiers").ToString.ToUpper 'Valeur a comparer
    '        Dim strIDCompare As String = Correspondance("ID").ToString.ToUpper() 'Garde le ID pour s'assurer que la recherche
    '        Dim strNomCompare As String = Correspondance("NomAAfficher").ToString
    '        'ne retrouve pas le meme role   
    '        Dim strCategorieRole As String = String.Empty 'Pour rechercher dans l'autre catégorie de role que celui en cours. Ex. Taches = RET_ et Metier = "REM_

    '        If Not String.IsNullOrEmpty(strIDCompare) Then
    '            strCategorieRole = Left(strIDCompare, InStr(strIDCompare, "_"))
    '        End If

    '        'requete pour trouver un role dans l'autre catégorie qui a la meme correspondance.
    '        dtAComparer = dtCorrespondance.Where(Function(x) x("LienTachesMetiers").ToString.ToUpper = strLienCompare And _
    '         x("ID").ToString.ToUpper <> strIDCompare And InStr(x("ID").ToString.ToUpper, strCategorieRole) = 0)

    '        'Si le dtAComparer est vide, c'est qu'il manque une correspondance
    '        'on rempli les variable recues (Byref) en parametre pour aider au surlignage des suggestion
    '        ' de roles cohérent : 
    '        '       - strCorrespondanceErreur = Contient la valeur du champs servant à valider la cohérence
    '        '       - strCorrespondanceIDRoleErreur = Contient le id du role à surligner qui n'a pas de correspondance dans l'autre catégorie 
    '        '       - strTypeCorrespondanceErreur = Contient la catégorie du role qui n'a pas de correspondance, pour surligner les roles de l'autre catégorie.

    '        'Surligner les suggestions qui ont la même correspondance dans l'autre liste et surligner le role
    '        'qui n'a pas de correspondance.
    '        If dtAComparer Is Nothing OrElse dtAComparer.Count = 0 Then
    '            strCorrespondanceErreur = strLienCompare
    '            strCorrespondanceIDRoleErreur = strIDCompare
    '            strNomRoleCorrespondanceErreur = strNomCompare


    '            'Déterminer dans quelle liste surligner les suggestion grace au ID du role qui n'a pas de correspondance.
    '            If InStr(strIDCompare, "RET_") > 0 Then
    '                'Role Tache en problème, on surligne dans la grille Métier
    '                strTypeCorrespondanceErreur = "Taches"
    '            Else
    '                'Role métier en problème, on surligne dans la grille taches
    '                strTypeCorrespondanceErreur = "Metiers"
    '            End If

    '            Return False
    '        End If

    '    Next
    '    'aucune erreur de cohérence
    '    Return True

    'End Function

    ''' <summary>
    ''' Retourne une balise de style pour mettre du texte en rouge dans les grilles.
    ''' </summary>
    ''' <param name="pIDRole">Le id du role à vérifier</param>
    ''' <param name="pNomGrille">Taches ou métiers : sert pour l'ancienne validation</param>
    ''' <param name="pstrChaineAComparer">La valeur du champs "organisation2" ou "LiensMetiersTaches" du role</param>
    ''' <param name="pPosition">Doit retourner le texte html de la balise différent si au début ou fin du texte</param>
    ''' <param name="pstrCorrespondanceErreur">Tache ou Métiers</param>
    ''' <param name="pstrCorrespondanceIDRoleErreur">Le role en erreur</param>
    ''' <param name="pstrTypeCorrespondanceErreur">La valeurs du champs "liens metiertaches" qui n'a pas de correspondance dans l'autre grille</param>
    ''' <param name="pIDRoleNonValideCoherence">Nouvelle validations : le role qui a généré une erreur de cohérence.</param>
    ''' <param name="pRegleCoherenceEnErreur">incoherent, obligatoire, choix multiples, choix unique</param>
    ''' <param name="plstReglesCoherences">Liste des règles</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function AfficherSuggestionCoherence(ByVal pIDRole As String, ByVal pNomGrille As String, ByVal pstrChaineAComparer As String, ByVal pPosition As String, _
                                                       ByVal pIDRoleNonValideCoherence As String, ByVal pRegleCoherenceEnErreur As String, ByVal plstReglesCoherences As List(Of TsCdRegleCoherence)) As String
        

        If Not String.IsNullOrEmpty(pIDRoleNonValideCoherence) Then

            'SL 2015-02-02 Le role en erreur doit être en rouge aussi.
            If pIDRoleNonValideCoherence = pIDRole Then
                If pPosition = "Debut" Then
                    Return "<span style=""color:Red;font-weight:bold;"">"
                Else
                    Return "</span>"
                End If
            End If

            Dim lstSuggestions As New List(Of String)

            Select Case pRegleCoherenceEnErreur
                Case "I"

                    If plstReglesCoherences.Where(Function(x) x.IDRole = pIDRoleNonValideCoherence).Count > 0 Then

                        lstSuggestions = plstReglesCoherences.Where(Function(x) x.IDRole = pIDRoleNonValideCoherence).Single.LstIncoherent
                        If lstSuggestions.Contains(pIDRole) Then
                            If pPosition = "Debut" Then
                                Return "<span style=""color:Red;font-weight:bold;"">"
                            Else
                                Return "</span>"
                            End If
                        End If
                    End If
                Case "O"
                    If plstReglesCoherences.Where(Function(x) x.IDRole = pIDRoleNonValideCoherence).Count > 0 Then

                        lstSuggestions = plstReglesCoherences.Where(Function(x) x.IDRole = pIDRoleNonValideCoherence).Single.LstObligatoire
                        If lstSuggestions.Contains(pIDRole) Then
                            If pPosition = "Debut" Then
                                Return "<span style=""color:Red;font-weight:bold;"">"
                            Else
                                Return "</span>"
                            End If
                        End If
                    End If
                Case "CM"
                    If plstReglesCoherences.Where(Function(x) x.IDRole = pIDRoleNonValideCoherence).Count > 0 Then

                        lstSuggestions = plstReglesCoherences.Where(Function(x) x.IDRole = pIDRoleNonValideCoherence).Single.LstChoixMultiples
                        If lstSuggestions.Contains(pIDRole) Then
                            If pPosition = "Debut" Then
                                Return "<span style=""color:Red;font-weight:bold;"">"
                            Else
                                Return "</span>"
                            End If
                        End If
                    End If
                Case "CU"
                    If plstReglesCoherences.Where(Function(x) x.IDRole = pIDRoleNonValideCoherence).Count > 0 Then

                        lstSuggestions = plstReglesCoherences.Where(Function(x) x.IDRole = pIDRoleNonValideCoherence).Single.LstChoixUnique
                        If lstSuggestions.Contains(pIDRole) Then
                            If pPosition = "Debut" Then
                                Return "<span style=""color:Red;font-weight:bold;"">"
                            Else
                                Return "</span>"
                            End If
                        End If
                    End If
            End Select
        End If
        'End If

        Return String.Empty
    End Function
End Class
