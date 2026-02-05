''' <summary>
''' Classe Fenêtre. Utiliser pour afficher les données qui seront changées dans les différents systèmes cibles.
''' </summary>
''' <remarks> </remarks>
Public Class TsFdAfficherApplq

#Region "Constantes"

    Private Const AJOUT As String = "Ajout"
    Private Const SUPPRIMER As String = "Supprimer"

#End Region

#Region "Enumerations"

    ''' <summary>
    ''' Action possible par l'utilisateur dans la fenêtre.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum CodeRetour
        Annuler
        Appliquer
    End Enum

    ''' <summary>
    ''' Mode d'affichage possible.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum ModeAffichage
        AppliquerAnnuler
        Afficher
    End Enum
#End Region

#Region "Variables Privées"
    Private codeDeRetour As CodeRetour
    Private modeAff As ModeAffichage

    Private listeCompareur As List(Of ComparateurItem)

    Private listeViewItemCollection As List(Of ListViewItem)
#End Region

#Region "Structures"

    ''' <summary>
    ''' Structure locale. 
    ''' Sert a passé des informations durant la <see cref="TsFdAfficherApplq.Decomposition" /> 
    ''' des listes jusqu'a <see cref="TsFdAfficherApplq.ActionTyper"/>
    ''' </summary>
    Private Structure StrSpecification
        Dim SystemeCible As String
        Dim Operation As String
        Dim TypeChangement As String
    End Structure

#End Region

#Region "Fonctions d'initialisations"

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    Friend Sub New()

        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().

        listeCompareur = New List(Of ComparateurItem)(New ComparateurItem() { _
                       New ComparateurItem(ComparateurItem.TypeColonne.SystemCible, viewChangement.Columns(ComparateurItem.TypeColonne.SystemCible)), _
                       New ComparateurItem(ComparateurItem.TypeColonne.Operation, viewChangement.Columns(ComparateurItem.TypeColonne.Operation)), _
                       New ComparateurItem(ComparateurItem.TypeColonne.TypeChangement, viewChangement.Columns(ComparateurItem.TypeColonne.TypeChangement)), _
                       New ComparateurItem(ComparateurItem.TypeColonne.Operande1, viewChangement.Columns(ComparateurItem.TypeColonne.Operande1)), _
                       New ComparateurItem(ComparateurItem.TypeColonne.Operande2, viewChangement.Columns(ComparateurItem.TypeColonne.Operande2)), _
                       New ComparateurItem(ComparateurItem.TypeColonne.Erreur, viewChangement.Columns(ComparateurItem.TypeColonne.Erreur)) _
                       })
        Dim listeImages As New ImageList()

        listeImages.Images.Add("ASC", My.Resources.xyTri_ascendant__liste_)
        listeImages.Images.Add("DESC", My.Resources.xyTri_descendant__liste_)

        viewChangement.SmallImageList = listeImages

        listeViewItemCollection = New List(Of ListViewItem)
    End Sub

    ''' <summary>
    ''' Fonction d'initialisation. 
    ''' Remplis la "list view" avec des <paramref name="informations"/> à afficher.
    ''' </summary>
    ''' <param name="informations">Liste d'objets d'informations essentielles pour accomplir l'affichage.</param>
    Friend Sub InitialiserFenetre(ByVal informations As List(Of TsCdInformationsDiff), ByVal modeAffichage As ModeAffichage)
        Dim specification As StrSpecification
        modeAff = modeAffichage
        Select Case modeAffichage
            Case TsFdAfficherApplq.ModeAffichage.AppliquerAnnuler
                viewChangement.Height = viewChangement.Height + txtErreur.Height
                txtErreur.Visible = False
            Case TsFdAfficherApplq.ModeAffichage.Afficher
                lblNbAjour.Visible = False
                lblNbPasAjour.Visible = False
                lblTotal.Visible = False
                txtNbAjour.Visible = False
                txtNbPasAjour.Visible = False
                txtTotal.Visible = False
        End Select

        specification.Operation = ""

        Dim faireVerification As Boolean = (modeAff = modeAffichage.AppliquerAnnuler)

        For Each info As TsCdInformationsDiff In informations
            specification.SystemeCible = info.Connecteur.DescrCible

            With info '! Affiche les éléments qui seront à modifier.
                specification.TypeChangement = "Utilisateurs"
                If faireVerification And MethodeIgnorable(AddressOf .Connecteur.CreerUsers) = False Then
                    .Connecteur.VerifierUsers(.AjoutUsers, New List(Of TsCdConnxUser))
                End If
                If MethodeIgnorable(AddressOf .Connecteur.CreerUsers) = False Then
                    Decomposition(.AjoutUsers, New List(Of TsCdConnxUser), specification)
                End If
                specification.TypeChangement = "Rôles"
                If faireVerification And MethodeIgnorable(AddressOf .Connecteur.CreerRoles) = False Then
                    .Connecteur.VerifierRoles(.AjoutRoles, New List(Of TsCdConnxRole))
                End If
                If MethodeIgnorable(AddressOf .Connecteur.CreerRoles) = False Then
                    Decomposition(.AjoutRoles, New List(Of TsCdConnxRole), specification)
                End If
                specification.TypeChangement = "Ressources"
                If faireVerification And MethodeIgnorable(AddressOf .Connecteur.CreerRessr) = False Then
                    .Connecteur.VerifierRessr(.AjoutRessr, New List(Of TsCdConnxRessr))
                End If
                If MethodeIgnorable(AddressOf .Connecteur.CreerRessr) = False Then
                    Decomposition(.AjoutRessr, New List(Of TsCdConnxRessr), specification)
                End If

                specification.TypeChangement = "Attributs Utilisateur"
                If faireVerification And MethodeIgnorable(AddressOf .Connecteur.AppliquerAttrbUsers) = False Then
                    .Connecteur.VerifierAttrbUsers(.AjoutAttrbUser, .SupprimerAttrbUser)
                End If
                Decomposition(.AjoutAttrbUser, .SupprimerAttrbUser, specification)
                specification.TypeChangement = "Attributs Rôle"
                If faireVerification And MethodeIgnorable(AddressOf .Connecteur.AppliquerAttrbRoles) = False Then
                    .Connecteur.VerifierAttrbRoles(.AjoutAttrbRole, .SupprimerAttrbRole)
                End If
                Decomposition(.AjoutAttrbRole, .SupprimerAttrbRole, specification)
                specification.TypeChangement = "Attributs Ressource"
                If faireVerification And MethodeIgnorable(AddressOf .Connecteur.AppliquerAttrbRessr) = False Then
                    .Connecteur.VerifierAttrbRessr(.AjoutAttrbRessr, .SupprimerAttrbRessr)
                End If
                Decomposition(.AjoutAttrbRessr, .SupprimerAttrbRessr, specification)

                specification.TypeChangement = "Utilisateur-Ressource"
                If faireVerification And MethodeIgnorable(AddressOf .Connecteur.AppliquerLiensUserRessr) = False Then
                    .Connecteur.VerifierLiensUserRessr(.AjoutUtilisateurRessource, .SupprimerUtilisateurRessource)
                End If
                Decomposition(.AjoutUtilisateurRessource, .SupprimerUtilisateurRessource, specification)
                specification.TypeChangement = "Utilisateur-Rôle"
                If faireVerification And MethodeIgnorable(AddressOf .Connecteur.AppliquerLiensUserRole) = False Then
                    .Connecteur.VerifierLiensUserRole(.AjoutUtilisateurRole, .SupprimerUtilisateurRole)
                End If
                Decomposition(.AjoutUtilisateurRole, .SupprimerUtilisateurRole, specification)
                specification.TypeChangement = "Rôle-Ressource"
                If faireVerification And MethodeIgnorable(AddressOf .Connecteur.AppliquerLiensRoleRessr) = False Then
                    .Connecteur.VerifierLiensRoleRessr(.AjoutRoleRessource, .SupprimerRoleRessource)
                End If
                Decomposition(.AjoutRoleRessource, .SupprimerRoleRessource, specification)
                specification.TypeChangement = "Rôle Supérieur-Sous Rôle"
                If faireVerification And MethodeIgnorable(AddressOf .Connecteur.AppliquerLiensRoleRole) = False Then
                    .Connecteur.VerifierLiensRoleRole(.AjoutRoleRole, .SupprimerRoleRole)
                End If
                Decomposition(.AjoutRoleRole, .SupprimerRoleRole, specification)

                specification.TypeChangement = "Utilisateurs"
                If faireVerification And MethodeIgnorable(AddressOf .Connecteur.DetruireUsers) = False Then
                    .Connecteur.VerifierUsers(New List(Of TsCdConnxUser), .SupprimerUsers)
                End If
                If MethodeIgnorable(AddressOf .Connecteur.DetruireUsers) = False Then
                    Decomposition(New List(Of TsCdConnxUser), .SupprimerUsers, specification)
                End If
                specification.TypeChangement = "Rôles"
                If faireVerification And MethodeIgnorable(AddressOf .Connecteur.DetruireRoles) = False Then
                    .Connecteur.VerifierRoles(New List(Of TsCdConnxRole), .SupprimerRoles)
                End If
                If MethodeIgnorable(AddressOf .Connecteur.DetruireRoles) = False Then
                    Decomposition(New List(Of TsCdConnxRole), .SupprimerRoles, specification)
                End If
                specification.TypeChangement = "Ressources"
                If faireVerification And MethodeIgnorable(AddressOf .Connecteur.DetruireRessr) = False Then
                    .Connecteur.VerifierRessr(New List(Of TsCdConnxRessr), .SupprimerRessr)
                End If
                If MethodeIgnorable(AddressOf .Connecteur.DetruireRessr) = False Then
                    Decomposition(New List(Of TsCdConnxRessr), .SupprimerRessr, specification)
                End If

            End With
        Next
        Select Case modeAffichage
            Case TsFdAfficherApplq.ModeAffichage.AppliquerAnnuler
                DenombrerAjour(informations)
            Case TsFdAfficherApplq.ModeAffichage.Afficher
                listeCompareur(5).ChangerAscendance()
                listeCompareur(5).ChangerAscendance()
                viewChangement.ListViewItemSorter = listeCompareur(5)
                viewChangement.Sort()
        End Select
    End Sub

    ''' <summary>
    ''' Ouvre la fenêtre en mode dialogue et renvois le résultat qui a fermé la fenêtre.
    ''' </summary>
    ''' <returns>Le résultat de la fenêtre, soit annuler ou appliquer</returns>
    ''' <remarks></remarks>
    Public Function DemarrerFenetre() As CodeRetour

        If modeAff = ModeAffichage.Afficher Then
            btnAnnuler.Text = "OK"
            btnAppliqer.Visible = False
        End If

        ShowDialog()

        Return Me.codeDeRetour
    End Function

#End Region

#Region "Fonctions Évenements"

    ''' <summary>
    ''' Fonction évènement. Appeler lorsque le listeView vois sa sélection changé.
    ''' Utilisé pour affiché la description des erreurs.
    ''' </summary>
    Private Sub viewChangement_ItemSelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.ListViewItemSelectionChangedEventArgs) Handles viewChangement.ItemSelectionChanged
        txtErreur.Text = ""
        If e.Item.Tag IsNot Nothing Then
            If TypeOf e.Item.Tag Is TsCdElementConnexion Then
                Dim elementConverti As TsCdElementConnexion = CType(e.Item.Tag, TsCdElementConnexion)
                If elementConverti.DescriptionErreur <> Nothing Then
                    txtErreur.Text = "Erreur: " + elementConverti.DescriptionErreur
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Fonction évènement. Appelé lorsque qu'une entête d'une colonne du listview <see cref="viewChangement"/>
    ''' est cliqué.
    ''' </summary>
    Private Sub viewChangement_ColumnClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles viewChangement.ColumnClick
        Dim objetComparateur As ComparateurItem = listeCompareur(e.Column)
        For Each c As ComparateurItem In listeCompareur
            c.ReinialiserEntete()
        Next
        objetComparateur.ChangerAscendance()

        viewChangement.ListViewItemSorter = objetComparateur
        viewChangement.Sort()
    End Sub

    ''' <summary>
    ''' Fonction Évenement. Provoqué par le clique du bouton btnAnnuler. Ferme la fenêtre en cour.
    ''' </summary>
    Private Sub btnAnnuler_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnnuler.Click
        If modeAff = ModeAffichage.Afficher Then
            codeDeRetour = CodeRetour.Appliquer
        Else
            codeDeRetour = CodeRetour.Annuler
        End If
        Me.Close()
    End Sub

    ''' <summary>
    ''' Fonction Évènement. Provoqué par le clique du bouton btnAppliqer.
    ''' Change le résultat de la fenêtre à Appliquer(<see cref="codeDeRetour" />)
    ''' et ferme la fenêtre.
    ''' </summary>
    Private Sub btnAppliqer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAppliqer.Click
        codeDeRetour = CodeRetour.Appliquer
        Me.Close()
    End Sub

#End Region

#Region "Fonctions de services"

    ''' <summary>
    ''' Fonction générique. Dépile les listes d'objets.
    ''' </summary>
    ''' <typeparam name="T">Type générique.</typeparam>
    ''' <param name="ajouts">Liste d'éléments ajoutés.</param>
    ''' <param name="supprs">Liste d'éléments supprimés.</param>
    ''' <param name="specification">Informations cumulatifs pour les fonctions qui seront appelées.</param>
    ''' <remarks></remarks>
    Private Sub Decomposition(Of T)(ByVal ajouts As List(Of T), ByVal supprs As List(Of T), ByVal specification As StrSpecification)
        For Each element As T In ajouts
            specification.Operation = AJOUT
            ActionTyper(element, specification)
        Next

        For Each element As T In supprs
            specification.Operation = SUPPRIMER
            ActionTyper(element, specification)
        Next
    End Sub

    ''' <summary>
    ''' Cette fonction connait chaques actions à prendre selon les types connus.
    ''' Contient aussi des actions communs à chaque type.
    ''' </summary>
    ''' <param name="element">Objet non typé.</param>
    ''' <param name="specification">Informations cumulatif utilisé pour appliquer les actions.</param>
    ''' <remarks></remarks>
    Private Sub ActionTyper(ByVal element As Object, ByVal specification As StrSpecification)
        Dim operant1 As String = ""
        Dim operant2 As String = ""
        Dim ajour As Boolean = True
        Dim erreur As Boolean = False

        Select Case True
            Case TypeOf element Is TsCdConnxUser
                Dim elementConverti As TsCdConnxUser = CType(element, TsCdConnxUser)
                operant1 = elementConverti.CodeUtilisateur
                ajour = (elementConverti.CibleAJour = TsECcCibleAJour.AJour OrElse elementConverti.CibleAJour = TsECcCibleAJour.ExtractionAJour)
                If elementConverti.DescriptionErreur <> Nothing Then
                    erreur = True
                End If

            Case TypeOf element Is TsCdConnxRole
                Dim elementConverti As TsCdConnxRole = CType(element, TsCdConnxRole)
                operant1 = elementConverti.NomRole
                ajour = (elementConverti.CibleAJour = TsECcCibleAJour.AJour OrElse elementConverti.CibleAJour = TsECcCibleAJour.ExtractionAJour)
                If elementConverti.DescriptionErreur <> Nothing Then
                    erreur = True
                End If

            Case TypeOf element Is TsCdConnxRessr
                Dim elementConverti As TsCdConnxRessr = CType(element, TsCdConnxRessr)
                operant1 = elementConverti.NomRessource + "." + elementConverti.CatgrRessource
                ajour = (elementConverti.CibleAJour = TsECcCibleAJour.AJour OrElse elementConverti.CibleAJour = TsECcCibleAJour.ExtractionAJour)
                If elementConverti.DescriptionErreur <> Nothing Then
                    erreur = True
                End If

            Case TypeOf element Is TsCdConnxRoleRessr
                Dim elementConverti As TsCdConnxRoleRessr = CType(element, TsCdConnxRoleRessr)
                operant1 = elementConverti.NomRole
                operant2 = elementConverti.NomRessource
                ajour = (elementConverti.CibleAJour = TsECcCibleAJour.AJour OrElse elementConverti.CibleAJour = TsECcCibleAJour.ExtractionAJour)
                If elementConverti.DescriptionErreur <> Nothing Then
                    erreur = True
                End If

            Case TypeOf element Is TsCdConnxRoleRole
                Dim elementConverti As TsCdConnxRoleRole = CType(element, TsCdConnxRoleRole)
                operant1 = elementConverti.NomRoleSup
                operant2 = elementConverti.NomSousRole
                ajour = (elementConverti.CibleAJour = TsECcCibleAJour.AJour OrElse elementConverti.CibleAJour = TsECcCibleAJour.ExtractionAJour)
                If elementConverti.DescriptionErreur <> Nothing Then
                    erreur = True
                End If

            Case TypeOf element Is TsCdConnxUserRessr
                Dim elementConverti As TsCdConnxUserRessr = CType(element, TsCdConnxUserRessr)
                operant1 = elementConverti.CodeUtilisateur
                operant2 = elementConverti.NomRessource
                ajour = (elementConverti.CibleAJour = TsECcCibleAJour.AJour OrElse elementConverti.CibleAJour = TsECcCibleAJour.ExtractionAJour)
                If elementConverti.DescriptionErreur <> Nothing Then
                    erreur = True
                End If

            Case TypeOf element Is TsCdConnxUserRole
                Dim elementConverti As TsCdConnxUserRole = CType(element, TsCdConnxUserRole)
                operant1 = elementConverti.CodeUtilisateur
                operant2 = elementConverti.NomRole
                ajour = (elementConverti.CibleAJour = TsECcCibleAJour.AJour OrElse elementConverti.CibleAJour = TsECcCibleAJour.ExtractionAJour)
                If elementConverti.DescriptionErreur <> Nothing Then
                    erreur = True
                End If

            Case TypeOf element Is TsCdConnxUserAttrb
                Dim elementConverti As TsCdConnxUserAttrb = CType(element, TsCdConnxUserAttrb)
                operant1 = elementConverti.CodeUtilisateur + "." + elementConverti.NomAttrb
                operant2 = elementConverti.Valeur
                ajour = (elementConverti.CibleAJour = TsECcCibleAJour.AJour OrElse elementConverti.CibleAJour = TsECcCibleAJour.ExtractionAJour)
                If elementConverti.DescriptionErreur <> Nothing Then
                    erreur = True
                End If

            Case TypeOf element Is TsCdConnxRoleAttrb
                Dim elementConverti As TsCdConnxRoleAttrb = CType(element, TsCdConnxRoleAttrb)
                operant1 = elementConverti.NomRole + "." + elementConverti.NomAttrb
                operant2 = elementConverti.Valeur
                ajour = (elementConverti.CibleAJour = TsECcCibleAJour.AJour OrElse elementConverti.CibleAJour = TsECcCibleAJour.ExtractionAJour)
                If elementConverti.DescriptionErreur <> Nothing Then
                    erreur = True
                End If

            Case TypeOf element Is TsCdConnxRessrAttrb
                Dim elementConverti As TsCdConnxRessrAttrb = CType(element, TsCdConnxRessrAttrb)
                operant1 = elementConverti.NomRessource + "/" + elementConverti.CatgrRessource + "." + elementConverti.NomAttrb
                operant2 = elementConverti.Valeur
                ajour = (elementConverti.CibleAJour = TsECcCibleAJour.AJour OrElse elementConverti.CibleAJour = TsECcCibleAJour.ExtractionAJour)
                If elementConverti.DescriptionErreur <> Nothing Then
                    erreur = True
                End If

        End Select


        Dim item As System.Windows.Forms.ListViewItem = New ListViewItem(specification.SystemeCible)
        item.Tag = element
        item.SubItems.Add(specification.Operation)
        item.SubItems.Add(specification.TypeChangement)
        item.SubItems.Add(operant1)
        item.SubItems.Add(operant2)
        item.SubItems.Add(ajour.ToString)
        If erreur = True Then
            item.ForeColor = Color.Red
            item.SubItems.Add("X")
        Else
            item.SubItems.Add("")
        End If
        If ajour = False Then
            viewChangement.Items.Add(item)
        End If
        listeViewItemCollection.Add(item)
    End Sub

    ''' <summary>
    ''' Function de service. Sert à dénombrer le nombre d'élément qui sont à jour.
    ''' Effet de bord: Afficher ses résultats dans la fenêtre directement.
    ''' </summary>
    ''' <param name="informations">Bloc d'information transmit à la fenêtre.</param>
    Private Sub DenombrerAjour(ByVal informations As List(Of TsCdInformationsDiff))
        Dim totalAjour As Integer = 0
        Dim totalAfficher As Integer = 0
        For Each info As TsCdInformationsDiff In informations
            With info
                EcrireEntree("Connecteur : " + .Connecteur.DescrCible)

                totalAfficher += DepileurCompteurAjour(.AjoutAttrbRessr, totalAjour)
                EcrireEntree(vbTab & "Nombre d'élément dans AjoutAttrbRessr = " & .AjoutAttrbRessr.Count)

                totalAfficher += DepileurCompteurAjour(.AjoutAttrbRole, totalAjour)
                EcrireEntree(vbTab & "Nombre d'élément dans AjoutAttrbRole = " & .AjoutAttrbRole.Count)

                totalAfficher += DepileurCompteurAjour(.AjoutAttrbUser, totalAjour)
                EcrireEntree(vbTab & "Nombre d'élément dans AjoutAttrbUser = " & .AjoutAttrbUser.Count)

                totalAfficher += DepileurCompteurAjour(.AjoutRessr, totalAjour)
                EcrireEntree(vbTab & "Nombre d'élément dans AjoutRessr = " & .AjoutRessr.Count)

                totalAfficher += DepileurCompteurAjour(.AjoutRoleRessource, totalAjour)
                EcrireEntree(vbTab & "Nombre d'élément dans AjoutRoleRessource = " & .AjoutRoleRessource.Count)

                totalAfficher += DepileurCompteurAjour(.AjoutRoleRole, totalAjour)
                EcrireEntree(vbTab & "Nombre d'élément dans AjoutRoleRole = " & .AjoutRoleRole.Count)

                totalAfficher += DepileurCompteurAjour(.AjoutRoles, totalAjour)
                EcrireEntree(vbTab & "Nombre d'élément dans AjoutRoles = " & .AjoutRoles.Count)

                totalAfficher += DepileurCompteurAjour(.AjoutUsers, totalAjour)
                EcrireEntree(vbTab & "Nombre d'élément dans AjoutUsers = " & .AjoutUsers.Count)

                totalAfficher += DepileurCompteurAjour(.AjoutUtilisateurRessource, totalAjour)
                EcrireEntree(vbTab & "Nombre d'élément dans AjoutUtilisateurRessource = " & .AjoutUtilisateurRessource.Count)

                totalAfficher += DepileurCompteurAjour(.AjoutUtilisateurRole, totalAjour)
                EcrireEntree(vbTab & "Nombre d'élément dans AjoutUtilisateurRole = " & .AjoutUtilisateurRole.Count)

                totalAfficher += DepileurCompteurAjour(.SupprimerAttrbRessr, totalAjour)
                EcrireEntree(vbTab & "Nombre d'élément dans SupprimerAttrbRessr = " & .SupprimerAttrbRessr.Count)

                totalAfficher += DepileurCompteurAjour(.SupprimerAttrbRole, totalAjour)
                EcrireEntree(vbTab & "Nombre d'élément dans SupprimerAttrbRole = " & .SupprimerAttrbRole.Count)

                totalAfficher += DepileurCompteurAjour(.SupprimerAttrbUser, totalAjour)
                EcrireEntree(vbTab & "Nombre d'élément dans SupprimerAttrbUser = " & .SupprimerAttrbUser.Count)

                totalAfficher += DepileurCompteurAjour(.SupprimerRessr, totalAjour)
                EcrireEntree(vbTab & "Nombre d'élément dans SupprimerRessr = " & .SupprimerRessr.Count)

                totalAfficher += DepileurCompteurAjour(.SupprimerRoleRessource, totalAjour)
                EcrireEntree(vbTab & "Nombre d'élément dans SupprimerRoleRessource = " & .SupprimerRoleRessource.Count)

                totalAfficher += DepileurCompteurAjour(.SupprimerRoleRole, totalAjour)
                EcrireEntree(vbTab & "Nombre d'élément dans SupprimerRoleRole = " & .SupprimerRoleRole.Count)

                totalAfficher += DepileurCompteurAjour(.SupprimerRoles, totalAjour)
                EcrireEntree(vbTab & "Nombre d'élément dans SupprimerRoles = " & .SupprimerRoles.Count)

                totalAfficher += DepileurCompteurAjour(.SupprimerUsers, totalAjour)
                EcrireEntree(vbTab & "Nombre d'élément dans SupprimerUsers = " & .SupprimerUsers.Count)

                totalAfficher += DepileurCompteurAjour(.SupprimerUtilisateurRessource, totalAjour)
                EcrireEntree(vbTab & "Nombre d'élément dans SupprimerUtilisateurRessource = " & .SupprimerUtilisateurRessource.Count)

                totalAfficher += DepileurCompteurAjour(.SupprimerUtilisateurRole, totalAjour)
                EcrireEntree(vbTab & "Nombre d'élément dans SupprimerUtilisateurRole = " & .SupprimerUtilisateurRole.Count)
            End With
        Next
        txtNbAjour.Text = totalAjour.ToString
        txtTotal.Text = totalAfficher.ToString
        txtNbPasAjour.Text = (totalAfficher - totalAjour).ToString

        EcrireEntree("Nombre d'éléments pas à jour / Nombre total d'éléments : " + txtNbPasAjour.Text + "/" + txtTotal.Text)
    End Sub

    ''' <summary>
    ''' Fonction de service générique.
    ''' Sert à dépiler une liste de <see cref="TsCdElementConnexion"/> pour déterminer combien d'élément sont à jour.
    ''' </summary>
    ''' <typeparam name="T">Type générique limité à TsCdElementConnexion</typeparam>
    ''' <param name="listeElements">Une liste d'élément T.</param>
    ''' <param name="ajour">Valeur par reférence. Revois le nombre d'élément ajour.</param>
    ''' <returns>Revois le nombre d'élément total de la liste.</returns>
    Function DepileurCompteurAjour(Of T As TsCdElementConnexion)(ByVal listeElements As List(Of T), ByRef ajour As Integer) As Integer
        For Each element As TsCdElementConnexion In listeElements
            If element.CibleAJour >= TsECcCibleAJour.ExtractionAJour Then
                ajour += 1
            End If
        Next
        Return listeElements.Count
    End Function

#End Region

#Region "Classes privées"

    ''' <summary>
    ''' Classe comparateur. Cert a faire des comparaisons de colonne du listview <see cref="viewChangement"/>.
    ''' </summary>
    ''' <remarks></remarks>
    Private Class ComparateurItem
        Implements IComparer

        Private mTypeColonne As TypeColonne
        Private mAscendance As Integer
        Private mEnteteColonne As ColumnHeader

        Private Shared mDernierTypeSelectionner As TypeColonne

        ''' <summary>
        ''' Nom des colonnes du listview.
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum TypeColonne
            SystemCible
            Operation
            TypeChangement
            Operande1
            Operande2
            Erreur
            Aucune
        End Enum

        ''' <summary>
        ''' Constructeur de base.
        ''' </summary>
        ''' <param name="colonne">Type de la colonne.</param>
        ''' <param name="listeVue">La colonne associée à ce comparateur.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal colonne As TypeColonne, ByVal listeVue As ColumnHeader)
            mTypeColonne = colonne
            mAscendance = 1
            mEnteteColonne = listeVue
            mDernierTypeSelectionner = TypeColonne.Aucune

        End Sub

        ''' <summary>
        ''' Fonction ICompare. Appeler lors des comparaison.
        ''' </summary>
        ''' <param name="x">Première objet.</param>
        ''' <param name="y">Deuxième objet</param>
        ''' <returns>Une valeur d'un String.Compare() modifier par l'ordre croissance ou non.</returns>
        Public Function Compare1(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            Dim item1 As ListViewItem = CType(x, ListViewItem)
            Dim item2 As ListViewItem = CType(y, ListViewItem)
            Dim paramRetour As Integer

            Select Case mTypeColonne
                Case TypeColonne.Erreur
                    paramRetour = String.Compare(item1.SubItems(5).Text.ToString, item2.SubItems(5).Text.ToString)
                Case TypeColonne.Operande1
                    paramRetour = String.Compare(item1.SubItems(3).ToString, item2.SubItems(3).ToString)
                Case TypeColonne.Operande2
                    paramRetour = String.Compare(item1.SubItems(4).ToString, item2.SubItems(4).ToString)
                Case TypeColonne.Operation
                    paramRetour = String.Compare(item1.SubItems(1).ToString, item2.SubItems(1).ToString)
                Case TypeColonne.SystemCible
                    paramRetour = String.Compare(item1.SubItems(0).ToString, item2.SubItems(0).ToString)
                Case TypeColonne.TypeChangement
                    paramRetour = String.Compare(item1.SubItems(2).ToString, item2.SubItems(2).ToString)
            End Select

            paramRetour *= mAscendance

            Return paramRetour
        End Function

        ''' <summary>
        ''' Changer l'ordre croissant/décroissant.
        ''' </summary>
        Public Sub ChangerAscendance()
            If mDernierTypeSelectionner = mTypeColonne Then
                mAscendance *= -1
            Else
                mDernierTypeSelectionner = mTypeColonne
                mAscendance = 1
            End If

            If mAscendance = 1 Then
                mEnteteColonne.ImageIndex = 0
            Else
                mEnteteColonne.ImageIndex = 1
            End If
        End Sub

        ''' <summary>
        ''' Remette l'entete de la colone à sont état original.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ReinialiserEntete()
            mEnteteColonne.ImageIndex = -1
        End Sub
    End Class

#End Region

    Private Sub XzCrCheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XzCrCheckBox1.CheckedChanged
        Me.Cursor = Cursors.WaitCursor
        viewChangement.Items.Clear()
        If Me.XzCrCheckBox1.Checked Then
            For Each item As ListViewItem In listeViewItemCollection
                If item.SubItems(clnAjour.Index).Text = Boolean.TrueString Then
                    viewChangement.Items.Add(item)
                End If
            Next
        Else
            For Each item As ListViewItem In listeViewItemCollection
                If item.SubItems(clnAjour.Index).Text = Boolean.FalseString Then
                    viewChangement.Items.Add(item)
                End If
            Next
        End If
        Me.Cursor = Cursors.Default
    End Sub
End Class