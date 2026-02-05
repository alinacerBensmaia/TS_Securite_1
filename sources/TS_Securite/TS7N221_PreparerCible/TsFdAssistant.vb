Imports System.Windows.Forms
Imports Rrq.CS.ServicesCommuns.ScenarioTransactionnel
Imports Rrq.CS.ServicesCommuns.UtilitairesCommuns
Imports Rrq.InfrastructureCommune.Parametres
Imports System.Reflection

'''-----------------------------------------------------------------------------
''' Project		: TS7N221_PreparerCible
''' Class		: TsFdAssistant
'''
'''-----------------------------------------------------------------------------
''' <summary>
''' Fenêtre principale de l'application.
''' Cette fenêtre donne la possibilité de faire des modifiactions aux systèmes d'accès et de sécurité de la RRQ.
''' </summary>
''' <remarks></remarks>
'''-----------------------------------------------------------------------------
Public Class TsFdAssistant

#Region "Variables FormAutonome"

    'Doit implementer XzIBesoinValid si on désire forcer la validation sur ce formulaire
    Implements XzIBesoinValid

    'Cette interface permet d'utiliser le XzBinding automatique des contrôles.
    Implements XzIBindingForm

    '$RRQ_SUGGESTION : Ajouter cette interface pour gérer focus sur contrôle avec plusieurs colonnes.
    'Cette interface permet de controler le curseur de facon générique
    'Implements XzIFocusMulti

    '$RRQ_SUGGESTION : Ajouter cette interface pour gérer focus dans ce formulaire.
    'Cette interface permet de controler le curseur de facon générique
    'Implements XzIFocus

#End Region

#Region "Constantes"

    Private Const SYSTEME_CIBLE_AD As String = "Active Directory interne"
    Private Const SYSTEME_CIBLE_TSS As String = "TopSecret Profile"
    Private Const SYSTEME_CIBLE_IDM As String = "ID Manager"

    Private Const CHANGEMENT_CREER_ROLE As String = "Créer Rôles"
    Private Const CHANGEMENT_CREER_USER As String = "Créer Utilisateurs"
    Private Const CHANGEMENT_CREER_RESSR As String = "Créer Ressources"

    Private Const CHANGEMENT_UTILISATEUR_ATTRB As String = "Changer Attributs Utilisateur"
    Private Const CHANGEMENT_ROLE_ATTRB As String = "Changer Attributs Rôle"
    Private Const CHANGEMENT_RESSOURCE_ATTRB As String = "Changer Attributs Ressource"

    Private Const CHANGEMENT_DETRUIRE_USER As String = "Détruire Utilisateurs"
    Private Const CHANGEMENT_DETRUIRE_ROLE As String = "Détruire Rôles"
    Private Const CHANGEMENT_DETRUIRE_RESSR As String = "Détruire Ressources"

    Private Const CHANGEMENT_UTILISATEUR_RESSOURCE As String = "Réorganiser les liens Utilisateur-Ressource"
    Private Const CHANGEMENT_UTILISATEUR_ROLE As String = "Réorganiser les liens Utilisateur-Rôle"
    Private Const CHANGEMENT_ROLE_RESSOURCE As String = "Réorganiser les liens Rôle-Ressource"
    Private Const CHANGEMENT_ROLE_ROLE As String = "Réorganiser les liens Rôle-Rôle"

    Private Const ACTION_CALCULE_AFFICHAGE As String = "Calcule et Affichage"
    Private Const ACTION_CALCULE_APPLICATION As String = "Calcule et application"
    Private Const ACTION_APPLICATION As String = "Application"
    Private Const ACTION_AFFICHAGE As String = "Affichage"
    Private Const ACTION_SYNCHRONISATION As String = "Synchronisation"

#End Region

#Region "Variables privées"

    ''' <summary>
    ''' Cette variable est utilisé pour bloqué les événements provoquer par la programmation interne du programme et 
    ''' permettre au action de l'utilisateur de primer sur certain évènement.
    ''' </summary>
    Private evenementUtilisateur As Boolean = True

    Private WithEvents fenetreProgression As New XzCuMessageProgression()

    ''' <summary>
    ''' Pour utilisé une fenêtre de progression, il faut utilisé une fonction qui sera appelé par la fenêtre de traitement.
    ''' Cette variable fait un pont entre la fonction et le rest du fonctionnment normal de la fenêtre assistant.
    ''' </summary>
    Private infoFenetreProgression As strFenetreProgression

    ''' <summary>Mémoire tempon des configuration existante.</summary>
    Private listeConfig As List(Of String)

    ''' <summary>Mémoire tempon des configurations déja confirmé intègre.</summary>
    Private dictioIntegrite As New Dictionary(Of String, Boolean)

#End Region

#Region "Fonctions déléguées"

    ''' <summary>
    ''' Une fonction qui prend un objet et renvois le dans un autre format.
    ''' </summary>
    ''' <typeparam name="E">Élément d'entrée.</typeparam>
    ''' <typeparam name="S">Format de sortis.</typeparam>
    ''' <param name="element">L'élément qui sera transformé.</param>
    ''' <returns>L'élément transformé.</returns>
    ''' <remarks></remarks>
    Delegate Function DelegateToGenerique(Of E, S)(ByVal element As E) As S

#End Region

#Region "--- Variables FormAutonome ---"

    ''''-----------------------------------------------------------------------------
    '''' <summary>
    '''' Instance de la classe de communication assoiciée à ce formulaire
    '''' </summary>
    ''''-----------------------------------------------------------------------------
    'Private mCcComm As $RRQWIZ_OBJCCTYPENAME

    '''-----------------------------------------------------------------------------
    ''' <summary>
    ''' Spécifie si la valeur d'un champ est modifié (Événement Validate)
    ''' </summary>
    '''-----------------------------------------------------------------------------
    Private mChange As Boolean

    '''-----------------------------------------------------------------------------
    ''' <summary>
    ''' Spécifie si le focus est sortie d'un contrôle.
    ''' </summary>
    '''-----------------------------------------------------------------------------
    Private mSortie As Boolean

    ''' --------------------------------------------------------------------------------
    ''' <summary>
    ''' Membre privé à chaque formulaire qui contient une référence à toutes 
    ''' les sources de données (Datatable, etc) nécessaire pour le XzBinding
    ''' automatique.
    ''' </summary>
    ''' --------------------------------------------------------------------------------
    Private mColSourceDonnee As Hashtable

    ''' --------------------------------------------------------------------------------
    ''' <summary>
    ''' Déclaration de l'événément à déclencher pour initialiser le XzBinding.
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    Public Event XzBindingInit(ByVal e As XzBindingEventArg) Implements XzIBindingForm.XzBindingInit

#End Region

#Region "--- Substitutions ---"

    '''-----------------------------------------------------------------------------
    ''' <summary>
    ''' Traitement declenché lorsque l'utilisateur demande de l'aide.
    ''' </summary>
    ''' <remarks></remarks>
    '''-----------------------------------------------------------------------------
    Protected Overrides Sub TraiterAide()

        XzCaAfficherAide.AfficherAide("")

    End Sub

#End Region

#Region "--- Implémentations de l'interface  ""XzIBindingForm""  ---"

    ''' --------------------------------------------------------------------------------
    ''' <summary>
    ''' Propriété qui doit contenir une référence à toutes les sources de données nécessaires
    ''' pour le XzBinding automatique.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    ''' --------------------------------------------------------------------------------
    Public ReadOnly Property XzBindingSources() As System.Collections.Hashtable Implements XzIBindingForm.XzBindingSources
        Get
            If mColSourceDonnee Is Nothing Then
                '$RRQ-SUGGESTION : mettre en paramètre du constructeur la nombre
                'de sources de données, ceci est plus optimale...
                'mColSourceDonnee = New Hashtable(2)
                mColSourceDonnee = New Hashtable
            End If
            Return mColSourceDonnee
        End Get
    End Property

#End Region

#Region "--- Implémentation de l'interface  ""XzIBesoinValid""  ---"

    ''' --------------------------------------------------------------------------------
    ''' <summary>
    ''' Spécifie si les contrôles du formulaire requiert par défaut d'être validé.
    ''' </summary>
    ''' <value></value>
    ''' <remarks>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    Public ReadOnly Property NecessiteValidation() As Boolean Implements XzIBesoinValid.NecessiteValidation
        Get
            '$RRQ_ACTION : Doit spécifier si les contrôles du formulaire requiert par défaut de la validation

            '$RRQ_EXEMPLE_DEBUT :
            'Return True
            '$RRQ_EXEMPLE_FIN
        End Get
    End Property

#End Region

#Region "Fonctions événements"

    ''' <summary>
    ''' Fonction événement. Appelée lors du clique de <see cref="btnAnnuler_Page1" /> et <see cref="btnAnnuler_Page2" />.
    ''' Ferme l'assisant.
    ''' </summary>
    Private Sub btnAnnuler_Pages_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnnuler_Page2.Click, btnAnnuler_Page1.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' Fonction événement. Appelée lors du clique de <see cref="btnSuivant_Page1" />.
    ''' Fait avancer l'assisant à la seconde étape.
    ''' </summary>
    Private Sub btnSuivant_Page1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuivant_Page1.Click
        Cursor = Cursors.WaitCursor

        Dim descriptionErreur As String = Nothing

        If optChoix3Page1.Checked = True Then '! --- Option 3 ---
            If txtChoix3Page1NomConfigAjout.Text = "" Then
                Cursor = Cursors.Arrow
                MsgBox("Le nom de la configuration n'a pas été entrée.", MsgBoxStyle.Exclamation, "Indication manquante")
                Exit Sub
            End If
            If ValiderExistanceConfig(txtChoix3Page1NomConfigAjout.Text) = False Then
                Cursor = Cursors.Arrow
                MsgBox("Le nom de la configuration est inexistante dans Sage.", MsgBoxStyle.Exclamation, "Nom invalide")
                Exit Sub
            End If
            If ValiderIntegriteConfig(txtChoix3Page1NomConfigAjout.Text, descriptionErreur) = False Then
                Cursor = Cursors.Arrow
                MsgBox("L'intégrité de la configuration est brisé. " + vbCrLf + "L'enrichissement des champs de la configuration n'est pas valide avec le programme." & vbCrLf & descriptionErreur, MsgBoxStyle.Exclamation, "Intégrité invalide")
                Exit Sub
            End If

        ElseIf optChoix4Page1.Checked = True Then '! --- Option 4 ---
            If txtChoix4Page1NomConfigSupp.Text = "" Then
                Cursor = Cursors.Arrow
                MsgBox("Le nom de la configuration n'a pas été entrée.", MsgBoxStyle.Exclamation, "Indication manquante")
                Exit Sub
            End If
            If ValiderExistanceConfig(txtChoix4Page1NomConfigSupp.Text) = False Then
                Cursor = Cursors.Arrow
                MsgBox("Le nom de la configuration est inexistante dans Sage.", MsgBoxStyle.Exclamation, "Nom invalide")
                Exit Sub
            End If
            If ValiderIntegriteConfig(txtChoix4Page1NomConfigSupp.Text, descriptionErreur) = False Then
                Cursor = Cursors.Arrow
                MsgBox("L'intégrité de la configuration est brisé. " + vbCrLf + "L'enrichissement des champs de la configuration n'est pas valide avec le programme." & vbCrLf & descriptionErreur, MsgBoxStyle.Exclamation, "Intégrité invalide")
                Exit Sub
            End If

        ElseIf optChoix1Page1.Checked = True Then '! --- Option 1 ---
            If txtChoix1Page1VieilleConfig.Text = "" Then
                Cursor = Cursors.Arrow
                MsgBox("Le nom de la vieille configuration n'a pas été entrée.", MsgBoxStyle.Exclamation, "Indication manquante")
                Exit Sub
            End If
            If txtChoix1Page1ConfigAJour.Text = "" Then
                Cursor = Cursors.Arrow
                MsgBox("Le nom de la configuration à jour n'a pas été entrée.", MsgBoxStyle.Exclamation, "Indication manquante")
                Exit Sub
            End If
            If ValiderExistanceConfig(txtChoix1Page1VieilleConfig.Text) = False Then
                Cursor = Cursors.Arrow
                MsgBox("Le nom de la vieille configuration est inexistante dans Sage.", MsgBoxStyle.Exclamation, "Nom invalide")
                Exit Sub
            End If
            If ValiderExistanceConfig(txtChoix1Page1ConfigAJour.Text) = False Then
                Cursor = Cursors.Arrow
                MsgBox("Le nom de la configuration à jour est inexistante dans Sage.", MsgBoxStyle.Exclamation, "Nom invalide")
                Exit Sub
            End If
            If ValiderIntegriteConfig(txtChoix1Page1VieilleConfig.Text, descriptionErreur) = False Then
                Cursor = Cursors.Arrow
                MsgBox("L'intégrité de la configuration '" + txtChoix1Page1VieilleConfig.Text + "' est brisé. " + vbCrLf + "L'enrichissement des champs de la configuration n'est pas valide avec le programme." & vbCrLf & descriptionErreur, MsgBoxStyle.Exclamation, "Intégrité invalide")
                Exit Sub
            End If
            If ValiderIntegriteConfig(txtChoix1Page1ConfigAJour.Text, descriptionErreur) = False Then
                Cursor = Cursors.Arrow
                MsgBox("L'intégrité de la configuration '" + txtChoix1Page1ConfigAJour.Text + "' est brisé. " + vbCrLf + "L'enrichissement des champs de la configuration n'est pas valide avec le programme." & vbCrLf & descriptionErreur, MsgBoxStyle.Exclamation, "Intégrité invalide")
                Exit Sub
            End If

        ElseIf optChoix2Page1.Checked = True Then '! --- Option 2 ---
            If txtChoix2Page1VieilleConfig.Text = "" Then
                Cursor = Cursors.Arrow
                MsgBox("Le nom de la vieille configuration n'a pas été entrée.", MsgBoxStyle.Exclamation, "Indication manquante")
                Exit Sub
            End If
            If txtChoix2Page1ConfigAJour.Text = "" Then
                Cursor = Cursors.Arrow
                MsgBox("Le nom de la configuration à jour n'a pas été entrée.", MsgBoxStyle.Exclamation, "Indication manquante")
                Exit Sub
            End If
            If ValiderExistanceConfig(txtChoix2Page1VieilleConfig.Text) = False Then
                Cursor = Cursors.Arrow
                MsgBox("Le nom de la vieille configuration est inexistante dans Sage.", MsgBoxStyle.Exclamation, "Nom invalide")
                Exit Sub
            End If
            If ValiderExistanceConfig(txtChoix2Page1ConfigAJour.Text) = False Then
                Cursor = Cursors.Arrow
                MsgBox("Le nom de la configuration à jour est inexistante dans Sage.", MsgBoxStyle.Exclamation, "Nom invalide")
                Exit Sub
            End If
            If ValiderIntegriteConfig(txtChoix2Page1VieilleConfig.Text, descriptionErreur) = False Then
                Cursor = Cursors.Arrow
                MsgBox("L'intégrité de la configuration '" + txtChoix2Page1VieilleConfig.Text + "' est brisé. " + vbCrLf + "L'enrichissement des champs de la configuration n'est pas valide avec le programme." & vbCrLf & descriptionErreur, MsgBoxStyle.Exclamation, "Intégrité invalide")
                Exit Sub
            End If
            If ValiderIntegriteConfig(txtChoix2Page1ConfigAJour.Text, descriptionErreur) = False Then
                Cursor = Cursors.Arrow
                MsgBox("L'intégrité de la configuration '" + txtChoix2Page1ConfigAJour.Text + "' est brisé. " + vbCrLf + "L'enrichissement des champs de la configuration n'est pas valide avec le programme." & vbCrLf & descriptionErreur, MsgBoxStyle.Exclamation, "Intégrité invalide")
                Exit Sub
            End If

        End If

        '! Cération des objets différences.
        Dim sourceDiffPourConnecteur As TsCaDiffSage = Nothing
        Dim sourceDiffPourSource As TsCaDiffSage = Nothing

        If optChoix3Page1.Checked = True Then
            EcrireEntree("Demande d'ajout de la configuration : '" + txtChoix3Page1NomConfigAjout.Text + "'")
            sourceDiffPourConnecteur = New TsCaDiffSage("", txtChoix3Page1NomConfigAjout.Text)
            sourceDiffPourSource = New TsCaDiffSage("", txtChoix3Page1NomConfigAjout.Text)
        ElseIf optChoix4Page1.Checked = True Then
            EcrireEntree("Demande de suppression à partir de la configuration : '" + txtChoix4Page1NomConfigSupp.Text + "'")
            sourceDiffPourConnecteur = New TsCaDiffSage(txtChoix4Page1NomConfigSupp.Text, "")
            sourceDiffPourSource = New TsCaDiffSage(txtChoix4Page1NomConfigSupp.Text, "")
        ElseIf optChoix1Page1.Checked = True Then
            EcrireEntree("Demande de traitement initiale à partir des configurations Original : '" + txtChoix1Page1VieilleConfig.Text + "' et Cible : '" + txtChoix1Page1ConfigAJour.Text + "'")
            sourceDiffPourConnecteur = New TsCaDiffSage(txtChoix1Page1VieilleConfig.Text, txtChoix1Page1ConfigAJour.Text)
            sourceDiffPourSource = New TsCaDiffSage("", txtChoix1Page1ConfigAJour.Text)
        ElseIf optChoix2Page1.Checked = True Then
            EcrireEntree("Demande de mise à jour à partir de la vieille configuration: '" + txtChoix2Page1VieilleConfig.Text + "' et la configuration à jour : '" + txtChoix2Page1ConfigAJour.Text + "'")
            sourceDiffPourConnecteur = New TsCaDiffSage(txtChoix2Page1VieilleConfig.Text, txtChoix2Page1ConfigAJour.Text)
            sourceDiffPourSource = New TsCaDiffSage(txtChoix2Page1VieilleConfig.Text, txtChoix2Page1ConfigAJour.Text)
        End If

        ChargerPage2(sourceDiffPourConnecteur, sourceDiffPourSource)

        Cursor = Cursors.Arrow
    End Sub

    ''' <summary>
    ''' Fonction événement. Appelée lors du clique de <see cref="btnPrecedant_Page2" />
    ''' Ramène l'assistant à l'étape 1.
    ''' </summary>
    Private Sub btnPrecedant_Page2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrecedant_Page2.Click
        EcrireEntree("Demande de retour à l'étape 1 de l'assistant.")

        pnlPage1.Visible = True
        pnlPage2.Visible = False

        Me.Text = "Assistant - Sources du changement (1 de 2)"
    End Sub

    ''' <summary>
    ''' Fonction événement. Appelée lors de la fermeture du programme.
    ''' Ferme la journalisation.
    ''' </summary>
    Private Sub TsFdAssistant_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        TerminerJournalisation()
    End Sub

    ''' <summary>
    ''' Fonction événement. Appelée au démarrage de l'assisant.
    ''' Initialise les pages dynamique de l'assistant.
    ''' </summary>
    Private Sub TsFdAssistant_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DemarerJournalisation()

        EcrireEntree("Démarrage du programme TS7N221_PreparerCible")
        TsCaDiffSage.ViderCache()
        EcrireEntree("Le cache du service sage a été vidé.")
        ChargerPage1()
    End Sub

    ''' <summary>
    ''' Fonction événement. Déclenchée quand l'état d'une boîte à cocher de l'arbre a été changé.
    ''' Enclanchement en cascade. Se répercute sur les boîtes filles de la boîte cochée.
    ''' </summary>
    ''' <remarks>Cette fonction n'est activable que par l'utilisateur. Voir la variable <see cref="TsFdAssistant.evenementUtilisateur" />.</remarks>
    Private Sub trvCible_AfterCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles trvCibles.AfterCheck
        '! Identifie si l'événement est déclanché par l'utilisateur.
        If evenementUtilisateur = True Then
            evenementUtilisateur = False
        Else
            Exit Sub
        End If

        ChangerEtatEnfantNodes(e.Node.Nodes, e.Node.Checked)
        If e.Node.Parent IsNot Nothing Then
            If e.Node.Checked = False Then
                ChangerEtatParentNodes(e.Node.Parent, False)
            Else
                ChangerEtatParentNodes(e.Node.Parent, True)
            End If
        End If

        verifierBtnAppliquer()

        '! Remet le contrôle des événements à l'utilisateur.
        evenementUtilisateur = True
    End Sub

    ''' <summary>
    ''' Fonction événement. Déclenchée par le clic du bouton <see cref="btnAppliquer_Page2" />.
    ''' Enclenche la mécanique de modification.
    ''' </summary>
    Private Sub btnAppliquer_Page2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAppliquer_Page2.Click
        Dim codeRetour As TsFdAfficherApplq.CodeRetour
        infoFenetreProgression.LstInfoDiff = New List(Of TsCdInformationsDiff)

        If chkAfficher_Page2.Checked = False Then
            If MsgBox("Désirez-vous vraiment appliquer les changements ?", MsgBoxStyle.YesNo, "Avertissement") = MsgBoxResult.No Then
                Exit Sub
            End If
        End If

        infoFenetreProgression.lstConnecteur = TrouverConnecteur(trvCibles.Nodes)

        Dim lstEtapes As New List(Of String)
        For Each noeudConnecteur As TreeNode In infoFenetreProgression.lstConnecteur
            Dim infoTag As TsCdConncSourceDiff = CType(noeudConnecteur.Tag, TsCdConncSourceDiff)
            Dim resultatInit As Boolean = infoTag.Connecteur.Initialiser()

            If resultatInit = False Then
                MsgBox("L'initialisation du connecteur: " + infoTag.Connecteur.IdCible + ", n'a pu être effectuée correctement. Aucun traitement ne sera fait.", MsgBoxStyle.Information, "Arrêt du traitement.")
                Exit Sub
            End If
            lstEtapes.AddRange(ConstruireListeProgressionDiff(noeudConnecteur))
        Next

        fenetreProgression.LargeurFenetre = 600
        fenetreProgression.Titre = "Traitement en cours"

        If chkAfficher_Page2.Checked = True Then
            EcrireEntree("Demande de différences pour les connecteurs suivants")
        Else
            EcrireEntree("Application des modifications pour les connecteurs suivants")
        End If
        For Each treeNode As TreeNode In infoFenetreProgression.lstConnecteur
            EcrireEntree(vbTab & treeNode.FullPath)
            For Each nd As TreeNode In treeNode.Nodes
                If nd.Checked Then
                    EcrireEntree(vbTab & vbTab & nd.FullPath)
                End If
            Next
        Next

        '! Affichage des modifications avant de faire les modifications.   
        If chkAfficher_Page2.Checked = True Then
            fenetreProgression.Action = ACTION_CALCULE_AFFICHAGE
            Dim lstEtapesClone As New List(Of String)(lstEtapes)
            lstEtapesClone.Add("Vérification des changements à appliquer.")
            lstEtapesClone.Add("Affichage.")
            fenetreProgression.Demarrer(Me, lstEtapesClone.ToArray)

            codeRetour = infoFenetreProgression.FenetreAffichage.DemarrerFenetre()

            If codeRetour = TsFdAfficherApplq.CodeRetour.Appliquer Then
                fenetreProgression.Action = ACTION_APPLICATION
                fenetreProgression.Demarrer(Me, New String() {"Application des modifications."})
            Else
                Exit Sub
            End If
        Else
            Dim lstEtapesClone As New List(Of String)(lstEtapes)
            lstEtapesClone.Add("Application des modifications.")
            lstEtapesClone.Add("Affichage.")
            fenetreProgression.Action = ACTION_CALCULE_APPLICATION
            fenetreProgression.Demarrer(Me, lstEtapesClone.ToArray)
        End If

        '! Si une erreur a été rentcontrée, un affichage des modifications non effectuées seront présentées.
        If infoFenetreProgression.ErreursRencontrer = True Then
            MsgBox("La modification a rencontré une erreur. Voici la liste des modifications qui n'ont pas été appliquées.", MsgBoxStyle.Information, "Erreurs")
            Dim nouvelleFernetre As New TsFdAfficherApplq()

            fenetreProgression.Titre = "Traitement en cours"

            fenetreProgression.Action = ACTION_AFFICHAGE
            fenetreProgression.Demarrer(Me, New String() {"Création de l'affichage."})

            infoFenetreProgression.FenetreAffichage.DemarrerFenetre()
        Else
            MsgBox("Les modifications ont été éffectuées sans problème.", MsgBoxStyle.Information, "Opération complétée")
        End If

        btnTerminer_Page2.Enabled = True
        btnAnnuler_Page2.Enabled = False

    End Sub

    ''' <summary>
    ''' Fonction événement. Déclenchée par le clic du bouton <see cref="btnTerminer_Page2" />.
    ''' Enclenche la mécanique de synchronisation et termine l'assistant.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnTerminer_Page2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTerminer_Page2.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' Fonction événement.
    ''' Appelée par la fenêtre de progression qui fait patienté l'utilisateur.
    ''' Les calcules sont éffectués à partir de cette fonction. 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub fenetreProgression_ExecuterAction() Handles fenetreProgression.ExecuterAction
        Try '! Renvois l'erreur avec une pile d'appel plus complète.
            infoFenetreProgression.FenetreAffichage = New TsFdAfficherApplq()

            '! Regroupe travail commun.
            Select Case fenetreProgression.Action
                Case ACTION_CALCULE_AFFICHAGE, ACTION_CALCULE_APPLICATION

                    For Each connecteur As TreeNode In infoFenetreProgression.lstConnecteur
                        Dim infoDiff As TsCdInformationsDiff

                        infoDiff = ObtenirDiffAncNouv(connecteur, fenetreProgression)
                        infoFenetreProgression.LstInfoDiff.Add(infoDiff)
                    Next

                    fenetreProgression.MettreMessageSuivantEnCours()
            End Select

            '! Éffectue l'action dépendament de quel fenêtre progression a été appelée.
            Select Case fenetreProgression.Action
                Case ACTION_CALCULE_AFFICHAGE
                    infoFenetreProgression.FenetreAffichage.InitialiserFenetre(infoFenetreProgression.LstInfoDiff, TsFdAfficherApplq.ModeAffichage.AppliquerAnnuler)
                Case ACTION_AFFICHAGE
                    infoFenetreProgression.FenetreAffichage.InitialiserFenetre(infoFenetreProgression.LstInfoDiff, TsFdAfficherApplq.ModeAffichage.Afficher)
                Case ACTION_CALCULE_APPLICATION
                    AppliquerChangement(infoFenetreProgression.LstInfoDiff)
                Case ACTION_APPLICATION
                    AppliquerChangement(infoFenetreProgression.LstInfoDiff)
            End Select
        Catch ex As Exception
            Throw New ApplicationException("Scenario Transactionnel", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Fonction évènement. Change le texte du bouton afficher, quand l'utilisateur ne veut pas affciher avant d'appliquer les changements.
    ''' </summary>
    Private Sub chkAfficher_Page2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAfficher_Page2.CheckedChanged
        If chkAfficher_Page2.Checked = True Then
            btnAppliquer_Page2.Text = "Afficher"
        Else
            btnAppliquer_Page2.Text = "Appliquer"
        End If
    End Sub

    ''' <summary>
    ''' Fonction évènement. Active/désactive les régions et leurs objets.
    ''' </summary>
    Private Sub optPage1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optChoix1Page1.CheckedChanged, optChoix3Page1.CheckedChanged, optChoix4Page1.CheckedChanged, optChoix2Page1.CheckedChanged
        If optChoix1Page1.Checked = True Then
            txtChoix1Page1VieilleConfig.Enabled = True
            txtChoix1Page1ConfigAJour.Enabled = True
            lblChoix1Page1VieilleConfig.Font = New Font(lblChoix1Page1VieilleConfig.Font, FontStyle.Bold)
            lblChoix1Page1ConfigAJour.Font = New Font(lblChoix1Page1VieilleConfig.Font, FontStyle.Bold)
            btnChoix1Page1VieilleConfig.Enabled = True
            btnChoix1Page1ConfigAJour.Enabled = True
        Else
            txtChoix1Page1VieilleConfig.Enabled = False
            txtChoix1Page1ConfigAJour.Enabled = False
            lblChoix1Page1VieilleConfig.Font = New Font(lblChoix1Page1VieilleConfig.Font, FontStyle.Regular)
            lblChoix1Page1ConfigAJour.Font = New Font(lblChoix1Page1VieilleConfig.Font, FontStyle.Regular)
            btnChoix1Page1VieilleConfig.Enabled = False
            btnChoix1Page1ConfigAJour.Enabled = False
        End If
        If optChoix2Page1.Checked = True Then
            txtChoix2Page1VieilleConfig.Enabled = True
            txtChoix2Page1ConfigAJour.Enabled = True
            lblChoix2Page1VieilleConfig.Font = New Font(lblChoix2Page1VieilleConfig.Font, FontStyle.Bold)
            lblChoix2Page1ConfigAJour.Font = New Font(lblChoix2Page1VieilleConfig.Font, FontStyle.Bold)
            btnChoix2Page1VieilleConfig.Enabled = True
            btnChoix2Page1ConfigAJour.Enabled = True
        Else
            txtChoix2Page1VieilleConfig.Enabled = False
            txtChoix2Page1ConfigAJour.Enabled = False
            lblChoix2Page1VieilleConfig.Font = New Font(lblChoix2Page1VieilleConfig.Font, FontStyle.Regular)
            lblChoix2Page1ConfigAJour.Font = New Font(lblChoix2Page1VieilleConfig.Font, FontStyle.Regular)
            btnChoix2Page1VieilleConfig.Enabled = False
            btnChoix2Page1ConfigAJour.Enabled = False
        End If
        If optChoix3Page1.Checked = True Then
            txtChoix3Page1NomConfigAjout.Enabled = True
            lblchoix3Page1NomConfigAjout.Font = New Font(lblChoix1Page1VieilleConfig.Font, FontStyle.Bold)
            btnChoix3Page1NomConfigAjout.Enabled = True
        Else
            txtChoix3Page1NomConfigAjout.Enabled = False
            lblchoix3Page1NomConfigAjout.Font = New Font(lblChoix1Page1VieilleConfig.Font, FontStyle.Regular)
            btnChoix3Page1NomConfigAjout.Enabled = False
        End If
        If optChoix4Page1.Checked = True Then
            txtChoix4Page1NomConfigSupp.Enabled = True
            lblChoix4Page1NomConfigSupp.Font = New Font(lblChoix1Page1VieilleConfig.Font, FontStyle.Bold)
            btnChoix4Page1NomConfigSupp.Enabled = True
        Else
            txtChoix4Page1NomConfigSupp.Enabled = False
            lblChoix4Page1NomConfigSupp.Font = New Font(lblChoix1Page1VieilleConfig.Font, FontStyle.Regular)
            btnChoix4Page1NomConfigSupp.Enabled = False
        End If
    End Sub

    ''' <summary>
    ''' Affiche les configurations disponible de sage.
    ''' </summary>
    Private Sub AfficherListeConfigs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChoix1Page1VieilleConfig.Click, btnChoix1Page1ConfigAJour.Click, btnChoix3Page1NomConfigAjout.Click, btnChoix4Page1NomConfigSupp.Click, btnChoix2Page1VieilleConfig.Click, btnChoix2Page1ConfigAJour.Click
        Dim bouton As ButtonBase = CType(sender, ButtonBase)

        Cursor = Cursors.WaitCursor

        If listeConfig Is Nothing Then
            listeConfig = TsCaDiffSage.ObtenirListeConfig()
        End If

        Dim boiteSelection As New TsFdBoiteSelection()
        boiteSelection.InitialiserListe(listeConfig)

        Dim selection As String = boiteSelection.OuvrirDialogue()

        Cursor = Cursors.Default
        If selection <> "" Then
            Select Case True
                Case sender Is btnChoix1Page1VieilleConfig
                    txtChoix1Page1VieilleConfig.Text = selection
                Case sender Is btnChoix1Page1ConfigAJour
                    txtChoix1Page1ConfigAJour.Text = selection
                Case sender Is btnChoix2Page1VieilleConfig
                    txtChoix2Page1VieilleConfig.Text = selection
                Case sender Is btnChoix2Page1ConfigAJour
                    txtChoix2Page1ConfigAJour.Text = selection
                Case sender Is btnChoix3Page1NomConfigAjout
                    txtChoix3Page1NomConfigAjout.Text = selection
                Case sender Is btnChoix4Page1NomConfigSupp
                    txtChoix4Page1NomConfigSupp.Text = selection
            End Select
        End If
    End Sub

#End Region

#Region "Fonctions de service"

    ''' <summary>
    ''' Fonction de service. Fait les modifications dans les cibles.
    ''' </summary>
    ''' <param name="lstInfoDiff">Liste d'informations sur les différences pour chaque connecteur.</param>
    Private Sub AppliquerChangement(ByVal lstInfoDiff As List(Of TsCdInformationsDiff))
        Dim contnrErreur As Boolean = chkContinuerErreur.Checked
        Dim sansErreur As Boolean = True

        EcrireEntree("Application des changements suivants")
        EcrireEntree(vbTab & "État des options de travail - Afficher : " + TraduireChecked(chkAfficher_Page2.Checked) + " Continuer avec erreur : " + TraduireChecked(chkAfficher_Page2.Checked))

        infoFenetreProgression.ErreursRencontrer = False

        For Each infoDiff As TsCdInformationsDiff In lstInfoDiff
            With infoDiff
                EcrireEntree(vbTab & "Travail effectué pour le connecteur: " + .Connecteur.DescrCible)
                '! Création des entités
                If MethodeIgnorable(AddressOf infoDiff.Connecteur.CreerUsers) = False Then
                    If sansErreur = True Then
                        sansErreur = .Connecteur.CreerUsers(.AjoutUsers, contnrErreur)
                    ElseIf contnrErreur = True Then
                        .Connecteur.CreerUsers(.AjoutUsers, contnrErreur)
                    End If
                    JournaliserListe("Création de l'utilisateur", .AjoutUsers, Function(e) e.CodeUtilisateur)
                End If
                If MethodeIgnorable(AddressOf infoDiff.Connecteur.CreerRoles) = False Then
                    If sansErreur = True Then
                        sansErreur = .Connecteur.CreerRoles(.AjoutRoles, contnrErreur)
                    ElseIf contnrErreur = True Then
                        .Connecteur.CreerRoles(.AjoutRoles, contnrErreur)
                    End If
                    JournaliserListe("Création du rôle", .AjoutRoles, Function(e) e.NomRole)
                End If
                If MethodeIgnorable(AddressOf infoDiff.Connecteur.CreerRessr) = False Then
                    If sansErreur = True Then
                        sansErreur = .Connecteur.CreerRessr(.AjoutRessr, contnrErreur)
                    ElseIf contnrErreur = True Then
                        .Connecteur.CreerRessr(.AjoutRessr, contnrErreur)
                    End If
                    JournaliserListe("Création de la ressource", .AjoutRessr, Function(e) e.NomRessource + ", " + e.CatgrRessource)
                End If
                '! Suppresion/Ajouts d'attributs.
                If MethodeIgnorable(AddressOf infoDiff.Connecteur.AppliquerAttrbUsers) = False Then
                    If sansErreur = True Then
                        sansErreur = .Connecteur.AppliquerAttrbUsers(.AjoutAttrbUser, .SupprimerAttrbUser, contnrErreur)
                    ElseIf contnrErreur = True Then
                        .Connecteur.AppliquerAttrbUsers(.AjoutAttrbUser, .SupprimerAttrbUser, contnrErreur)
                    End If
                    JournaliserListe("Ajout d'attribut d'utilisateur", .AjoutAttrbUser, Function(e) e.CodeUtilisateur + "| Attribut: " + e.NomAttrb + " Valeur: " + e.Valeur)
                    JournaliserListe("Suppression d'attribut d'utilisateur", .SupprimerAttrbUser, Function(e) e.CodeUtilisateur + "| Atribut: " + e.NomAttrb + " Valeur: " + e.Valeur)
                End If
                If MethodeIgnorable(AddressOf infoDiff.Connecteur.AppliquerAttrbRoles) = False Then
                    If sansErreur = True Then
                        sansErreur = .Connecteur.AppliquerAttrbRoles(.AjoutAttrbRole, .SupprimerAttrbRole, contnrErreur)
                    ElseIf contnrErreur = True Then
                        .Connecteur.AppliquerAttrbRoles(.AjoutAttrbRole, .SupprimerAttrbRole, contnrErreur)
                    End If
                    JournaliserListe("Ajout d'attribut de rôle", .AjoutAttrbRole, Function(e) e.NomRole + "| Attribut: " + e.NomAttrb + " Valeur: " + e.Valeur)
                    JournaliserListe("Suppression d'attribut de rôle", .AjoutAttrbRole, Function(e) e.NomRole + "| Attribut: " + e.NomAttrb + " Valeur: " + e.Valeur)
                End If
                If MethodeIgnorable(AddressOf infoDiff.Connecteur.AppliquerAttrbRessr) = False Then
                    If sansErreur = True Then
                        sansErreur = .Connecteur.AppliquerAttrbRessr(.AjoutAttrbRessr, .SupprimerAttrbRessr, contnrErreur)
                    ElseIf contnrErreur = True Then
                        .Connecteur.AppliquerAttrbRessr(.AjoutAttrbRessr, .SupprimerAttrbRessr, contnrErreur)
                    End If
                    JournaliserListe("Ajout d'attribut de ressource", .AjoutAttrbRessr, Function(e) e.NomRessource + ", " + e.CatgrRessource + "| Attribut: " + e.NomAttrb + " Valeur: " + e.Valeur)
                    JournaliserListe("Suppression d'attribut de ressource", .SupprimerAttrbRessr, Function(e) e.NomRessource + ", " + e.CatgrRessource + "| Attribut: " + e.NomAttrb + " Valeur: " + e.Valeur)

                End If
                '! Application des relations entre entités.
                If MethodeIgnorable(AddressOf infoDiff.Connecteur.AppliquerLiensUserRessr) = False Then
                    If sansErreur = True Then
                        sansErreur = .Connecteur.AppliquerLiensUserRessr(.AjoutUtilisateurRessource, .SupprimerUtilisateurRessource, contnrErreur)
                    ElseIf contnrErreur = True Then
                        .Connecteur.AppliquerLiensUserRessr(.AjoutUtilisateurRessource, .SupprimerUtilisateurRessource, contnrErreur)
                    End If
                    JournaliserListe("Ajout du lien Utilisateur/Ressource", .AjoutUtilisateurRessource, Function(e) e.CodeUtilisateur + "/" + e.NomRessource + ", " + e.CatgrRessource)
                    JournaliserListe("Suppression du lien Utilisateur/Ressource", .SupprimerUtilisateurRessource, Function(e) e.CodeUtilisateur + "/" + e.NomRessource + ", " + e.CatgrRessource)

                End If
                If MethodeIgnorable(AddressOf infoDiff.Connecteur.AppliquerLiensUserRole) = False Then
                    If sansErreur = True Then
                        sansErreur = .Connecteur.AppliquerLiensUserRole(.AjoutUtilisateurRole, .SupprimerUtilisateurRole, contnrErreur)
                    ElseIf contnrErreur = True Then
                        .Connecteur.AppliquerLiensUserRole(.AjoutUtilisateurRole, .SupprimerUtilisateurRole, contnrErreur)
                    End If
                    JournaliserListe("Ajout du lien Utilisateur/Rôle", .AjoutUtilisateurRole, Function(e) e.CodeUtilisateur + "/" + e.NomRole)
                    JournaliserListe("Supression du lien Utilisateur/Rôle", .SupprimerUtilisateurRole, Function(e) e.CodeUtilisateur + "/" + e.NomRole)

                End If
                If MethodeIgnorable(AddressOf infoDiff.Connecteur.AppliquerLiensRoleRessr) = False Then
                    If sansErreur = True Then
                        sansErreur = .Connecteur.AppliquerLiensRoleRessr(.AjoutRoleRessource, .SupprimerRoleRessource, contnrErreur)
                    ElseIf contnrErreur = True Then
                        .Connecteur.AppliquerLiensRoleRessr(.AjoutRoleRessource, .SupprimerRoleRessource, contnrErreur)
                    End If
                    JournaliserListe("Ajout du lien Rôle/Ressource", .AjoutRoleRessource, Function(e) e.NomRole + "/" + e.NomRessource + ", " + e.CatgrRessource)
                    JournaliserListe("Suppression du lien Rôle/Ressource", .SupprimerRoleRessource, Function(e) e.NomRole + "/" + e.NomRessource + ", " + e.CatgrRessource)

                End If
                If MethodeIgnorable(AddressOf infoDiff.Connecteur.AppliquerLiensRoleRole) = False Then
                    If sansErreur = True Then
                        sansErreur = .Connecteur.AppliquerLiensRoleRole(.AjoutRoleRole, .SupprimerRoleRole, contnrErreur)
                    ElseIf contnrErreur = True Then
                        .Connecteur.AppliquerLiensRoleRole(.AjoutRoleRole, .SupprimerRoleRole, contnrErreur)
                    End If
                    JournaliserListe("Ajout du lien Rôle Supérieur/Sous Rôle", .AjoutRoleRole, Function(e) e.NomRoleSup + "/" + e.NomSousRole)
                    JournaliserListe("Suppresion du lien Rôle Supérieur/Sous Rôle", .AjoutRoleRole, Function(e) e.NomRoleSup + "/" + e.NomSousRole)

                End If
                '! Suppresion d'entités.
                If MethodeIgnorable(AddressOf infoDiff.Connecteur.DetruireUsers) = False Then
                    If sansErreur = True Then
                        sansErreur = .Connecteur.DetruireUsers(.SupprimerUsers, contnrErreur)
                    ElseIf contnrErreur = True Then
                        .Connecteur.DetruireUsers(.SupprimerUsers, contnrErreur)
                    End If
                    JournaliserListe("Supprimer l'utilisateur", .SupprimerUsers, Function(e) e.CodeUtilisateur)

                End If
                If MethodeIgnorable(AddressOf infoDiff.Connecteur.DetruireRoles) = False Then
                    If sansErreur = True Then
                        sansErreur = .Connecteur.DetruireRoles(.SupprimerRoles, contnrErreur)
                    ElseIf contnrErreur = True Then
                        .Connecteur.DetruireRoles(.SupprimerRoles, contnrErreur)
                    End If
                    JournaliserListe("Supprimer le rôle", .SupprimerRoles, Function(e) e.NomRole)

                End If
                If MethodeIgnorable(AddressOf infoDiff.Connecteur.DetruireRessr) = False Then
                    If sansErreur = True Then
                        sansErreur = .Connecteur.DetruireRessr(.SupprimerRessr, contnrErreur)
                    ElseIf contnrErreur = True Then
                        .Connecteur.DetruireRessr(.SupprimerRessr, contnrErreur)
                    End If
                    JournaliserListe("Supprimer la ressource", .SupprimerRessr, Function(e) e.NomRessource + ", " + e.CatgrRessource)

                End If
            End With

            If sansErreur = False Then
                infoFenetreProgression.ErreursRencontrer = True
                If contnrErreur = False Then
                    Exit For
                End If
            End If
        Next

        If sansErreur = False Then
            EcrireEntree("[Erreur] Une ou des erreurs on été rencontrées durant le traitement. Les erreurs sont affichés à l'utilisateur.")
        End If

        EcrireEntree("Fin de l'application des changements")
    End Sub

    ''' <summary>
    ''' Fonction de service. Chercher les noeuds qui possède des connecteur dans un arbre.
    ''' </summary>
    ''' <param name="noeuds">Une collection de noeuds dans lequel chercher les connecteurs.</param>
    ''' <returns>Une liste de noeuds ayant un connecteur d'associé.</returns>
    Private Function TrouverConnecteur(ByVal noeuds As TreeNodeCollection) As List(Of TreeNode)
        Dim lstNoeud As New List(Of TreeNode)()

        For Each noeud As TreeNode In noeuds
            If TypeOf noeud.Tag Is TsCdConncSourceDiff Then
                If ConnecteurSoliciter(noeud) = True Then
                    lstNoeud.Add(noeud)
                End If
            Else
                lstNoeud.AddRange(TrouverConnecteur(noeud.Nodes))
            End If
        Next

        Return lstNoeud
    End Function

    ''' <summary>
    ''' Fonction de service. Fonction récursif. Sous fonction de TrouverConnecteur().
    ''' Chercher si l'utilisateur a demandé à taiter ce noeud, en vérifiant si lui ou ses fils ont été cochés.
    ''' </summary>
    ''' <param name="noeud">Noeud que l'on veux vérifier s'il est solicité.</param>
    ''' <returns>Vrai si le noeud ou un de ses fils est solicité, sinon Faux.</returns>
    Private Function ConnecteurSoliciter(ByVal noeud As TreeNode) As Boolean
        If noeud.Checked = True Then
            Return True
        Else
            Dim soliciter As Boolean = False
            For Each noeudFils As TreeNode In noeud.Nodes
                If ConnecteurSoliciter(noeudFils) = True Then
                    soliciter = True
                End If
            Next
            Return soliciter
        End If
    End Function

    ''' <summary>
    ''' Fonction de service.  Sous fonction de fenetreProgression_ExecuterAction.
    ''' Obtient les différences entre une vieille cofiguration et une nouvelle cofiguration.
    ''' </summary>
    ''' <param name="connecteur">Le noeud possédant le connecteur de cible à laquel la différence est demandé.</param>
    ''' <param name="fenetreProgrssn">La fenêtre de progression en cour.</param>
    ''' <returns>Un objet de données contenant les informations sur différances.</returns>
    Private Function ObtenirDiffAncNouv(ByVal connecteur As TreeNode, ByVal fenetreProgrssn As XzCuMessageProgression) As TsCdInformationsDiff
        Dim paramRetour As New TsCdInformationsDiff()
        Dim infoTag As TsCdConncSourceDiff = CType(connecteur.Tag, TsCdConncSourceDiff)
        Dim sourceDiff As TsISourceDiff = infoTag.SourceDiff

        paramRetour.Connecteur = infoTag.Connecteur
        Dim idCible As String = paramRetour.Connecteur.IdCible

        '! Remplissage des listes de différence.
        With paramRetour
            '! Entités Utilisateurs
            If ChercherEtatNode(connecteur, CHANGEMENT_CREER_USER) And ChercherEtatNode(connecteur, CHANGEMENT_DETRUIRE_USER) Then
                sourceDiff.ObtnrDiffrUser(.AjoutUsers, .SupprimerUsers)
                fenetreProgrssn.MettreMessageSuivantEnCours()
            ElseIf ChercherEtatNode(connecteur, CHANGEMENT_CREER_USER) Then
                sourceDiff.ObtnrDiffrUser(.AjoutUsers, New List(Of TsCdConnxUser))
                fenetreProgrssn.MettreMessageSuivantEnCours()
            End If
            '! Entités Rôles.
            If ChercherEtatNode(connecteur, CHANGEMENT_CREER_ROLE) And ChercherEtatNode(connecteur, CHANGEMENT_DETRUIRE_ROLE) Then
                sourceDiff.ObtnrDiffrRole(.AjoutRoles, .SupprimerRoles)
                fenetreProgrssn.MettreMessageSuivantEnCours()
            ElseIf ChercherEtatNode(connecteur, CHANGEMENT_CREER_ROLE) Then
                sourceDiff.ObtnrDiffrRole(.AjoutRoles, New List(Of TsCdConnxRole))
                fenetreProgrssn.MettreMessageSuivantEnCours()
            End If
            '! Entités Ressources
            If ChercherEtatNode(connecteur, CHANGEMENT_CREER_RESSR) And ChercherEtatNode(connecteur, CHANGEMENT_DETRUIRE_RESSR) Then
                sourceDiff.ObtnrDiffrRessource(idCible, .AjoutRessr, .SupprimerRessr)
                fenetreProgrssn.MettreMessageSuivantEnCours()
            ElseIf ChercherEtatNode(connecteur, CHANGEMENT_CREER_RESSR) Then
                sourceDiff.ObtnrDiffrRessource(idCible, .AjoutRessr, New List(Of TsCdConnxRessr))
                fenetreProgrssn.MettreMessageSuivantEnCours()
            End If
            '! Attributs
            If ChercherEtatNode(connecteur, CHANGEMENT_UTILISATEUR_ATTRB) Then
                sourceDiff.ObtnrDiffrAttrbUser(.AjoutAttrbUser, .SupprimerAttrbUser)
                fenetreProgrssn.MettreMessageSuivantEnCours()
            End If
            If ChercherEtatNode(connecteur, CHANGEMENT_ROLE_ATTRB) Then
                sourceDiff.ObtnrDiffrAttrbRole(.AjoutAttrbRole, .SupprimerAttrbRole)
                fenetreProgrssn.MettreMessageSuivantEnCours()
            End If
            If ChercherEtatNode(connecteur, CHANGEMENT_RESSOURCE_ATTRB) Then
                sourceDiff.ObtnrDiffrAttrbRessr(idCible, .AjoutAttrbRessr, .SupprimerAttrbRessr)
                fenetreProgrssn.MettreMessageSuivantEnCours()
            End If
            '! Liens
            If ChercherEtatNode(connecteur, CHANGEMENT_ROLE_ROLE) Then
                sourceDiff.ObtnrDiffrRoleRole(.AjoutRoleRole, .SupprimerRoleRole)
                fenetreProgrssn.MettreMessageSuivantEnCours()
            End If
            If ChercherEtatNode(connecteur, CHANGEMENT_ROLE_RESSOURCE) Then
                sourceDiff.ObtnrDiffrRoleRessr(idCible, .AjoutRoleRessource, .SupprimerRoleRessource)
                fenetreProgrssn.MettreMessageSuivantEnCours()
            End If
            If ChercherEtatNode(connecteur, CHANGEMENT_UTILISATEUR_ROLE) Then
                sourceDiff.ObtnrDiffrUserRole(.AjoutUtilisateurRole, .SupprimerUtilisateurRole)
                fenetreProgrssn.MettreMessageSuivantEnCours()
            End If
            If ChercherEtatNode(connecteur, CHANGEMENT_UTILISATEUR_RESSOURCE) Then
                If TsBaSupportConnecteur.SupporteRole(.Connecteur) Then
                    sourceDiff.ObtnrDiffrUserRessrDirect(idCible, .AjoutUtilisateurRessource, .SupprimerUtilisateurRessource)
                Else
                    sourceDiff.ObtnrDiffrUserRessrRecurcif(idCible, .AjoutUtilisateurRessource, .SupprimerUtilisateurRessource)
                End If
                fenetreProgrssn.MettreMessageSuivantEnCours()
            End If
            '! Supprimer entités.
            If Not ChercherEtatNode(connecteur, CHANGEMENT_CREER_USER) And ChercherEtatNode(connecteur, CHANGEMENT_DETRUIRE_USER) Then
                sourceDiff.ObtnrDiffrUser(New List(Of TsCdConnxUser), .SupprimerUsers)
                fenetreProgrssn.MettreMessageSuivantEnCours()
            End If
            If Not ChercherEtatNode(connecteur, CHANGEMENT_CREER_ROLE) And ChercherEtatNode(connecteur, CHANGEMENT_DETRUIRE_ROLE) Then
                sourceDiff.ObtnrDiffrRole(New List(Of TsCdConnxRole), .SupprimerRoles)
                fenetreProgrssn.MettreMessageSuivantEnCours()
            End If
            If Not ChercherEtatNode(connecteur, CHANGEMENT_CREER_RESSR) And ChercherEtatNode(connecteur, CHANGEMENT_DETRUIRE_RESSR) Then
                sourceDiff.ObtnrDiffrRessource(idCible, New List(Of TsCdConnxRessr), .SupprimerRessr)
                fenetreProgrssn.MettreMessageSuivantEnCours()
            End If
        End With

        Return paramRetour
    End Function

    ''' <summary>
    ''' Fonction de service. Construit une liste pour une fenêtre de progression.
    ''' </summary>
    ''' <param name="nouedConnecteur">Le connecteur à partir du quel la liste sera bâtie.</param>
    ''' <returns>Une liste d'étapes en texte.</returns>
    ''' <remarks>Cette fonction doit être batis sur la même structure que la fonction ObtenirDiffAncNouv()</remarks>
    Private Function ConstruireListeProgressionDiff(ByVal nouedConnecteur As TreeNode) As List(Of String)
        Dim lstEtapes As New List(Of String)()
        Dim infoDiff As TsCdConncSourceDiff = CType(nouedConnecteur.Tag, TsCdConncSourceDiff)
        Dim connecteur As TsCuConnecteurCible = infoDiff.Connecteur

        If ChercherEtatNode(nouedConnecteur, CHANGEMENT_CREER_USER) And ChercherEtatNode(nouedConnecteur, CHANGEMENT_DETRUIRE_USER) Then
            lstEtapes.Add(connecteur.DescrCible + " - Différence utilisateurs.")
        ElseIf ChercherEtatNode(nouedConnecteur, CHANGEMENT_CREER_USER) Then
            lstEtapes.Add(connecteur.DescrCible + " - Création utilisateurs.")
        End If
        If ChercherEtatNode(nouedConnecteur, CHANGEMENT_CREER_ROLE) And ChercherEtatNode(nouedConnecteur, CHANGEMENT_DETRUIRE_ROLE) Then
            lstEtapes.Add(connecteur.DescrCible + " - Différence rôles.")
        ElseIf ChercherEtatNode(nouedConnecteur, CHANGEMENT_CREER_ROLE) Then
            lstEtapes.Add(connecteur.DescrCible + " - Création rôles.")
        End If
        If ChercherEtatNode(nouedConnecteur, CHANGEMENT_CREER_RESSR) And ChercherEtatNode(nouedConnecteur, CHANGEMENT_DETRUIRE_RESSR) Then
            lstEtapes.Add(connecteur.DescrCible + " - Différence ressources.")
        ElseIf ChercherEtatNode(nouedConnecteur, CHANGEMENT_CREER_RESSR) Then
            lstEtapes.Add(connecteur.DescrCible + " - Création ressources.")
        End If
        '! Attributs
        If ChercherEtatNode(nouedConnecteur, CHANGEMENT_UTILISATEUR_ATTRB) Then
            lstEtapes.Add(connecteur.DescrCible + " - Différence entre attributs utilisateurs.")
        End If
        If ChercherEtatNode(nouedConnecteur, CHANGEMENT_ROLE_ATTRB) Then
            lstEtapes.Add(connecteur.DescrCible + " - Différence entre attributs rôles.")
        End If
        If ChercherEtatNode(nouedConnecteur, CHANGEMENT_RESSOURCE_ATTRB) Then
            lstEtapes.Add(connecteur.DescrCible + " - Différence entre attributs ressources.")
        End If
        '! Liens
        If ChercherEtatNode(nouedConnecteur, CHANGEMENT_ROLE_ROLE) Then
            lstEtapes.Add(connecteur.DescrCible + " - Différences entre lien rôle/rôle.")
        End If
        If ChercherEtatNode(nouedConnecteur, CHANGEMENT_ROLE_RESSOURCE) Then
            lstEtapes.Add(connecteur.DescrCible + " - Différences entre lien rôle/ressource.")
        End If
        If ChercherEtatNode(nouedConnecteur, CHANGEMENT_UTILISATEUR_ROLE) Then
            lstEtapes.Add(connecteur.DescrCible + " - Différences entre lien utilisateur/rôle.")
        End If
        If ChercherEtatNode(nouedConnecteur, CHANGEMENT_UTILISATEUR_RESSOURCE) Then
            lstEtapes.Add(connecteur.DescrCible + " - Différences entre lien utilisateur/ressource.")
        End If
        '! Supprimer entités.
        If Not ChercherEtatNode(nouedConnecteur, CHANGEMENT_CREER_USER) And ChercherEtatNode(nouedConnecteur, CHANGEMENT_DETRUIRE_USER) Then
            lstEtapes.Add(connecteur.DescrCible + " - Détruire utilisateurs.")
        End If
        If Not ChercherEtatNode(nouedConnecteur, CHANGEMENT_CREER_ROLE) And ChercherEtatNode(nouedConnecteur, CHANGEMENT_DETRUIRE_ROLE) Then
            lstEtapes.Add(connecteur.DescrCible + " - Détruire rôles.")
        End If
        If Not ChercherEtatNode(nouedConnecteur, CHANGEMENT_CREER_RESSR) And ChercherEtatNode(nouedConnecteur, CHANGEMENT_DETRUIRE_RESSR) Then
            lstEtapes.Add(connecteur.DescrCible + " - Détruire ressources.")
        End If
        Return lstEtapes

    End Function

    ''' <summary>
    ''' Fonction de service.
    ''' Cherche l'état coché ou non d'un noeud. L'élément de recherche est le texte d'affichage.
    ''' </summary>
    ''' <param name="noeud">Noeud dans laquel la recherche sera faite.</param>
    ''' <param name="nomDuFils">Le nom du texte d'affichage.</param>
    ''' <returns>L'état coché du noeud recherché.</returns>
    ''' <remarks>Si le noeud recherché n'existe pas, il est considéré comme non coché.</remarks>
    Private Function ChercherEtatNode(ByVal noeud As TreeNode, ByVal nomDuFils As String) As Boolean
        For Each nd As TreeNode In noeud.Nodes
            If nd.Text = nomDuFils Then
                Return nd.Checked
            End If
        Next

        Return False
    End Function

    ''' <summary>
    ''' Fonction d'initialisation. Fait les ajustements nécessaires pour initialiser le pnlPage2.
    ''' Construir l'arbre des systèmes cibles.
    ''' </summary>
    ''' <param name="diffConnecteur">Objet source des différences pour les connecteurs cibles.</param>
    ''' <param name="diffSource">Objet source des différence pour les connecteurs sources</param>
    Private Sub ChargerPage2(ByVal diffConnecteur As TsISourceDiff, ByVal diffSource As TsISourceDiff)
        btnAppliquer_Page2.Enabled = False
        pnlPage2.Visible = True
        pnlPage1.Visible = False

        Me.Text = "Assistant - Systèmes cibles (2 de 2)"
        trvCibles.Nodes(0).Nodes.Clear()
        trvCibles.Nodes(1).Nodes.Clear()
        trvCibles.Nodes(0).Checked = False
        trvCibles.Nodes(1).Checked = False
        ConstruireArbre(trvCibles.Nodes(0), "DifferenceRelatif", diffConnecteur)

        ' On desactive la difference avec le maitre en cas de suppresion
        ' Trop d'impact => on supprime des roles même si ils sont utilisé dans une autre configuration
        ' Du coup la suppression d'une configuration dans la config maitre modifie d'autre configuration 
        ' présent dans la config maitre...
        If optChoix4Page1.Checked = False Then
            ConstruireArbre(trvCibles.Nodes(1), "DifferenceMaitre", diffSource)
        End If
    End Sub

    ''' <summary>
    ''' Fonction d'initialisation. Fait les ajustements nécessaires pour initialiser le pnlPage1.
    ''' Construir l'arbre des systèmes cibles.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ChargerPage1()
        pnlPage1.Visible = True
        pnlPage2.Visible = False

        Me.Text = "Assistant - Sources du changement (1 de 2)"
    End Sub


    ''' <summary>
    ''' Fonction de service. Sous fonction de chargerPage2. 
    ''' Construit un arbre de à partir des informations d'un <see cref="TsCuConnecteurCible" />.
    ''' </summary>
    ''' <param name="noeud">Noeud principale du TreeView.</param>
    ''' <remarks>Methode récursive avec les sous-sections</remarks>
    Private Sub ConstruireArbre(ByVal noeud As TreeNode, ByVal nomSectionConnecteur As String, ByVal sourceDiff As TsISourceDiff)
        Dim lstSectionClef As String() = XuCuConfiguration.ClefsSysteme("TS7", "TS7\TS7N221\" + nomSectionConnecteur)
        Dim dictioTrieTsCuConnecteurCible As New SortedDictionary(Of Integer, TsCuConnecteurCible)
        Dim lstSousSection As New List(Of TreeNode)

        '! Parcourrir les sections
        For Each sectionClef As String In lstSectionClef
            Dim valeur As String = XuCuConfiguration.ValeurSysteme("TS7", sectionClef)
            Dim sectionClefSplit As String = sectionClef.Split("\"c)(3)
            If String.Compare(sectionClefSplit, "sous-sections", True) = 0 Then
                '! Récupérer la string contenant la liste des sous-sections existants. Les sous-sections sont ajoutées a la fin de la liste.
                Dim noeudSousSection As New TreeNode(valeur.Replace("_", " "))
                ConstruireArbre(noeudSousSection, nomSectionConnecteur + "\" + valeur, sourceDiff)
                lstSousSection.Add(noeudSousSection)
            Else
                Dim pattern As String = "^(\d+),([\w\.]+),([\w\.]+),([\w\. ]+)$"
                Dim regex As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex(pattern)
                Dim match As System.Text.RegularExpressions.Match = regex.Match(valeur)
                If Not match.Success Then
                    Throw New ApplicationException("Ligne de configuration mal formattée : " & valeur)
                End If

                Dim a As Assembly = Assembly.Load(match.Groups(2).Value)
                Dim t As Type = a.GetType(match.Groups(3).Value)
                Dim c As ConstructorInfo = t.GetConstructor(New Type() {GetType(String)})
                Dim o As Object = c.Invoke(New Object() {match.Groups(4).Value})
                Dim connecteurObject As TsCuConnecteurCible = CType(o, TsCuConnecteurCible)

                dictioTrieTsCuConnecteurCible.Add(Integer.Parse(match.Groups(1).Value), connecteurObject)
            End If
        Next

        For Each connecteurObject As TsCuConnecteurCible In dictioTrieTsCuConnecteurCible.Values
            'Dim noeudConnecteurAd As TreeNode = noeud.Nodes.Add(connecteurObject.DescrCible)
            ConstruireBranche(noeud, connecteurObject, sourceDiff)
        Next
        For Each sousSection As TreeNode In lstSousSection
            noeud.Nodes.Add(sousSection)
        Next

        'On ouvre seulement les noeud parent
        If noeud.Level = 0 AndAlso Not noeud.TreeView Is Nothing Then
            noeud.Expand()
        End If
    End Sub


    ''' <summary>
    ''' Fonction de service. Sous fonction de ConstruireArbre(). 
    ''' Construit une branche de l'arbre de à partir des informations d'un <see cref="TsCuConnecteurCible" />.
    ''' </summary>
    ''' <param name="noeud">Noeud du TreeView qui recevera la branche.</param>
    ''' <remarks></remarks>
    Private Sub ConstruireBranche(ByVal noeud As TreeNode, ByVal connecteur As TsCuConnecteurCible, ByVal sourceDiff As TsISourceDiff)
        Dim noeudConnecteurAd As TreeNode = noeud.Nodes.Add(connecteur.DescrCible)
        Dim infoTag As New TsCdConncSourceDiff
        infoTag.Connecteur = connecteur
        infoTag.SourceDiff = sourceDiff
        noeudConnecteurAd.Tag = infoTag

        If MethodeIgnorable(AddressOf connecteur.CreerUsers) = False Then
            noeudConnecteurAd.Nodes.Add(CHANGEMENT_CREER_USER)
        End If
        If MethodeIgnorable(AddressOf connecteur.CreerRoles) = False Then
            noeudConnecteurAd.Nodes.Add(CHANGEMENT_CREER_ROLE)
        End If
        If MethodeIgnorable(AddressOf connecteur.CreerRessr) = False Then
            noeudConnecteurAd.Nodes.Add(CHANGEMENT_CREER_RESSR)
        End If

        If MethodeIgnorable(AddressOf connecteur.AppliquerAttrbUsers) = False Then
            noeudConnecteurAd.Nodes.Add(CHANGEMENT_UTILISATEUR_ATTRB)
        End If
        If MethodeIgnorable(AddressOf connecteur.AppliquerAttrbRoles) = False Then
            noeudConnecteurAd.Nodes.Add(CHANGEMENT_ROLE_ATTRB)
        End If
        If MethodeIgnorable(AddressOf connecteur.AppliquerAttrbRessr) = False Then
            noeudConnecteurAd.Nodes.Add(CHANGEMENT_RESSOURCE_ATTRB)
        End If

        If MethodeIgnorable(AddressOf connecteur.AppliquerLiensRoleRole) = False Then
            noeudConnecteurAd.Nodes.Add(CHANGEMENT_ROLE_ROLE)
        End If
        If MethodeIgnorable(AddressOf connecteur.AppliquerLiensUserRessr) = False Then
            noeudConnecteurAd.Nodes.Add(CHANGEMENT_UTILISATEUR_RESSOURCE)
        End If
        If MethodeIgnorable(AddressOf connecteur.AppliquerLiensUserRole) = False Then
            noeudConnecteurAd.Nodes.Add(CHANGEMENT_UTILISATEUR_ROLE)
        End If
        If MethodeIgnorable(AddressOf connecteur.AppliquerLiensRoleRessr) = False Then
            noeudConnecteurAd.Nodes.Add(CHANGEMENT_ROLE_RESSOURCE)
        End If

        If MethodeIgnorable(AddressOf connecteur.DetruireUsers) = False Then
            noeudConnecteurAd.Nodes.Add(CHANGEMENT_DETRUIRE_USER)
        End If
        If MethodeIgnorable(AddressOf connecteur.DetruireRoles) = False Then
            noeudConnecteurAd.Nodes.Add(CHANGEMENT_DETRUIRE_ROLE)
        End If
        If MethodeIgnorable(AddressOf connecteur.DetruireRessr) = False Then
            noeudConnecteurAd.Nodes.Add(CHANGEMENT_DETRUIRE_RESSR)
        End If

        noeudConnecteurAd.Collapse()
    End Sub

    ''' <summary>
    ''' Fonction de service. Cette fonction va, en cascade, changer l'état de tous les décendants des noeuds.
    ''' </summary>
    ''' <param name="noeuds">Les noeuds dans lequel l'état des décendants sera changés.</param>
    ''' <param name="etat">L'état dans lequel les noeuds seront changés.</param>
    ''' <remarks></remarks>
    Private Sub ChangerEtatEnfantNodes(ByVal noeuds As TreeNodeCollection, ByVal etat As Boolean)
        For Each noeud As TreeNode In noeuds
            ChangerEtatEnfantNodes(noeud.Nodes, etat)
            noeud.Checked = etat
        Next
    End Sub

    ''' <summary>
    ''' Fonction de service. Cette fonction va, en cascade, changer l'état du parent et de ses ancètres.
    ''' Si tous les fils du parent sont cochés, alors le parent sera coché. 
    ''' Si un de ses fils n'est pas coché, le parent sera décoché.
    ''' </summary>
    ''' <param name="noeud">Noeud parent dont l'état sera changé.</param>
    ''' <param name="etat">L'état dans lequel le parent sera changé.</param>
    ''' <remarks></remarks>
    Private Sub ChangerEtatParentNodes(ByVal noeud As TreeNode, ByVal etat As Boolean)
        Dim enfantsTousCocher As Boolean = True
        If etat = True Then
            For Each ndEnfant As TreeNode In noeud.Nodes
                enfantsTousCocher = enfantsTousCocher = True And ndEnfant.Checked = True
            Next
            If enfantsTousCocher = True Then
                noeud.Checked = True
                If noeud.Parent IsNot Nothing Then
                    ChangerEtatParentNodes(noeud.Parent, True)
                End If
            End If
        Else
            noeud.Checked = False
            If noeud.Parent IsNot Nothing Then
                ChangerEtatParentNodes(noeud.Parent, False)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Fonction de service. 
    ''' Cette fonction active ou désactive le bouton <see cref="btnAppliquer_Page2" />,
    ''' si au moins une des cases à cocher de l'arbre est cochée.
    ''' </summary>
    ''' <remarks>Si au moins une case est cochée.</remarks>
    Private Sub verifierBtnAppliquer()
        If BoiteEstCocher(trvCibles.Nodes) = True Then
            btnAppliquer_Page2.Enabled = True
        Else
            btnAppliquer_Page2.Enabled = False
        End If
    End Sub

    ''' <summary>
    ''' Fonction de service. Sous-fonction de <see cref="verifierBtnAppliquer" />.
    ''' Fonction récursive qui fouille chaque étage de l'arbre.
    ''' </summary>
    ''' <param name="noeuds">Les noueds de l'étage à chercher.</param>
    ''' <returns>Si une case de l'étage ou de leurs fils est cochée.</returns>
    ''' <remarks></remarks>
    Private Function BoiteEstCocher(ByVal noeuds As TreeNodeCollection) As Boolean
        For Each nd As TreeNode In noeuds
            If nd.Checked = True Then
                Return True
            End If
            If BoiteEstCocher(nd.Nodes) = True Then
                Return True
            End If
        Next
        Return False
    End Function

    ''' <summary>
    ''' Fonction de service. Vérifie dans la liste des configurations déja chargées si la configuration en argument existe,
    ''' si non elle demande à Sage directement.
    ''' </summary>
    ''' <param name="nomConfig">Nom de la configuration.</param>
    ''' <returns>Si oui ou non la configuration existe dans sage.</returns>
    ''' <remarks></remarks>
    Private Function ValiderExistanceConfig(ByVal nomConfig As String) As Boolean
        If listeConfig Is Nothing Then
            listeConfig = TsCaDiffSage.ObtenirListeConfig()
        End If

        For Each config As String In listeConfig
            If config = nomConfig Then
                Return True
            End If
        Next

        Return TsCaDiffSage.ValiderExistenceConfig(nomConfig)
    End Function

    ''' <summary>
    ''' Fonction de service. Vérifie l'intégrité de la configuration. 
    ''' Si la configuration n'est enrichit correctement, il faut le savoir avant de manipuler ses données.
    ''' </summary>
    ''' <param name="nomConfig">Nom de la configuration à vérifier l'intégrité.</param>
    ''' <returns>Vrai si l'intégrité de la configuration est valide, sinon Faux.</returns>
    Private Function ValiderIntegriteConfig(ByVal nomConfig As String, ByRef descriptionErreur As String) As Boolean
        Dim paramRetour As Boolean = False
        If dictioIntegrite.ContainsKey(nomConfig) = True Then
            paramRetour = dictioIntegrite(nomConfig)
        Else
            paramRetour = TsCaDiffSage.ValiderIntegriteConfig(nomConfig, descriptionErreur)
            dictioIntegrite.Add(nomConfig, paramRetour)
        End If

        Return paramRetour
    End Function

#End Region

#Region "Fonctions de services pour journalisation"

    ''' <summary>
    ''' Fonctions de services. Permet de traduire l'état d'une case à coché en texte.
    ''' </summary>
    ''' <param name="entree">la valeur de la case à coché.</param>
    ''' <returns>Sa version texte.</returns>
    Private Function TraduireChecked(ByVal entree As Boolean) As String
        Select Case entree
            Case True
                Return "Coché"
            Case False
                Return "Décoché"
            Case Else
                Return "Décoché"
        End Select

    End Function

    ''' <summary>
    ''' Fonction de service. Fonction générique qui permet de faire une entrée dans le journal avec n'importe quel objet.
    ''' </summary>
    ''' <typeparam name="T">Type d'entrée. Type minimum TsCdElementConnexion.</typeparam>
    ''' <param name="titreEntrees">Titre de l'entrée dans le journal.</param>
    ''' <param name="liste">Liste d'élément du type d'entrée.</param>
    ''' <param name="functionToString">Fonction qui permet de transformer les objets du type d'entrée en texte.</param>
    Private Sub JournaliserListe(Of T As TsCdElementConnexion)(ByVal titreEntrees As String, ByVal liste As List(Of T), ByVal functionToString As DelegateToGenerique(Of T, String))

        For Each e As T In liste
            ' On affiche l'élément si il contient une erreur
            If e.DescriptionErreur <> "" Then
                EcrireEntree(vbTab + vbTab + "[Erreur] " + titreEntrees + " - " + functionToString(e) + " - " + "a rencontré l'erreur suivante : " + e.DescriptionErreur)
            Else
                ' On affiche que les vrai modifications sur le système
                ' c'est à dire les elements qui sont passé de l'etat "pas a jour" vers "a jour"
                If e.ModificationEffectuee Then
                    EcrireEntree(vbTab + vbTab + titreEntrees + " - " + functionToString(e) + " - " + " est maintenant dans l'état: " + e.CibleAJour.ToString)
                Else
                    EcrireEntree(vbTab + vbTab + titreEntrees + " - " + functionToString(e) + " - " + " était dèjà dans l'état: " + e.CibleAJour.ToString)
                End If
            End If
        Next

    End Sub

#End Region

#Region "Structures"
    ''' <summary>
    ''' Structure mémoire qui transfère les informations entre la fenêtre et le handler de la fenêtre de progression. 
    ''' </summary>
    ''' <remarks></remarks>
    Private Structure strFenetreProgression
        Dim LstInfoDiff As List(Of TsCdInformationsDiff)
        Dim FenetreAffichage As TsFdAfficherApplq
        Dim ErreursRencontrer As Boolean
        Dim lstConnecteur As List(Of TreeNode)
    End Structure
#End Region

End Class