Imports Rrq.CS.ServicesCommuns.ScenarioTransactionnel
Imports Rrq.CS.ServicesCommuns.UtilitairesCommuns
Imports Rrq.InfrastructureCommune.Parametres

Imports System.Collections.Generic
Imports System.IO
Imports Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ

'''-----------------------------------------------------------------------------
''' Project		: TS7N323_VerifierGroupeAD
''' Class		: TsFdVerifierGroupeAD
'''
'''-----------------------------------------------------------------------------
''' <summary>
''' Fenêtre principale de l'application.
''' </summary>
''' <remarks></remarks>
'''-----------------------------------------------------------------------------
''' 
Public Class TsFdVerifierGroupeAD
    Implements XzIBesoinValid 'Doit implementer XzIBesoinValid si on désire forcer la validation sur ce formulaire
    Implements XzIBindingForm 'Cette interface permet d'utiliser le XzBinding automatique des contrôles.

    '$RRQ_SUGGESTION : Ajouter cette interface pour gérer focus sur contrôle avec plusieurs colonnes.
    'Cette interface permet de controler le curseur de facon générique
    'Implements XzIFocusMulti

    '$RRQ_SUGGESTION : Ajouter cette interface pour gérer focus dans ce formulaire.
    'Cette interface permet de controler le curseur de facon générique
    'Implements XzIFocus

    Private Const cst_TITLE As String = "Enregistrement et comparaison AD pour une unité administrative"

#Region "Enums"
#End Region

#Region "--- Variables ---"

    ''''-----------------------------------------------------------------------------
    '''' <summary>
    '''' Instance de la classe de communication assoiciée à ce formulaire
    '''' </summary>
    ''''-----------------------------------------------------------------------------
    ''Private mCcComm As $RRQWIZ_OBJCCTYPENAME

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
    Private mNomFichier1 As String
    Private mNomFichier2 As String
    Private mWatcher As New FileSystemWatcher()
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

#Region "Fonctions Événements"
    Private Sub TsFdVerifierGroupeAD_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.XzCrTextboxRepertoireSauvegarde.Text = XuCuConfiguration.ValeurSysteme("TS7", "TS7N323\RepertoireSauvegarde")
        If Directory.Exists(Me.XzCrTextboxRepertoireSauvegarde.Text) = False Then
            MessageBox.Show("Le repertoire " + Me.XzCrTextboxRepertoireSauvegarde.Text + " n'existe pas", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.XzCrTextboxRepertoireSauvegarde.Text = ""
        Else
            ModifierRepertoireDeTravail(Me.XzCrTextboxRepertoireSauvegarde.Text)
        End If

        Me.XzCrComboBoxFiltreUniteAdministrative.Items.Add("1724")
        Me.XzCrCboServeurActiveDirectory.SelectedIndex = 0
        Me.ActiverComparaison(False)
    End Sub

    Private Sub XzCrTextboxRepertoireSauvegarde_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles XzCrTextboxRepertoireSauvegarde.Validating
        If Directory.Exists(Me.XzCrTextboxRepertoireSauvegarde.Text) = False Then
            MessageBox.Show("Le repertoire " + Me.XzCrTextboxRepertoireSauvegarde.Text + " n'existe pas", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            e.Cancel = True
        Else
            e.Cancel = False
        End If
    End Sub

    Private Sub XzCrTextboxRepertoireSauvegarde_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XzCrTextboxRepertoireSauvegarde.Validated
        ModifierRepertoireDeTravail(Me.XzCrTextboxRepertoireSauvegarde.Text)
    End Sub

    Private Sub XzCrButtonEnregistrer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XzCrButtonEnregistrer.Click
        Dim validations As List(Of String) = validationEnregistrer()
        If validations.Count > 0 Then
            Dim message As String = String.Format("Vous devez fournir:{0}{1}", Environment.NewLine, String.Join(Environment.NewLine, validations))
            MessageBox.Show(message, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)

        Else
            Using ChangerCurseur(Cursors.WaitCursor)
                EnregistrerAD(Me.XzCrComboBoxFiltreUniteAdministrative.Text)
            End Using
            MessageBox.Show("L'enregistrement de l'unité s'est terminée avec succès", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Function validationEnregistrer() As List(Of String)
        Dim validations As New List(Of String)()
        If String.IsNullOrWhiteSpace(XzCrTextboxRepertoireSauvegarde.Text) Then validations.Add("- Un répertoire de sauvegarde")
        If String.IsNullOrWhiteSpace(XzCrComboBoxFiltreUniteAdministrative.Text) Then validations.Add("- Un numéro d'unité administrative")
        If String.IsNullOrWhiteSpace(XzCrCboServeurActiveDirectory.Text) Then validations.Add("- Le serveur Active Directory à rechercher")
        If String.IsNullOrWhiteSpace(XzCrTxtAttributDeRecherche.Text) Then validations.Add("- Le nom de l'attribut contenant le numéro de l'unité administrative")
        Return validations
    End Function

    Private Sub XzCrListView1_ItemCheck(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles XzCrListView1.ItemCheck
        If e.NewValue = CheckState.Checked And Me.XzCrListView1.CheckedItems.Count = 2 Then
            MessageBox.Show("Vous pouvez sélectionner deux fichiers au maximum !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            e.NewValue = CheckState.Unchecked
        End If
        If Me.XzCrListView1.CheckedItems.Count = 1 And e.NewValue = CheckState.Checked Then
            ' Autorise la comparaison
            Me.ActiverComparaison(True)
        Else
            Me.ActiverComparaison(False)
        End If
    End Sub


    Private Sub XzCrListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XzCrListView1.SelectedIndexChanged
        For Each Index As Integer In Me.XzCrListView1.SelectedIndices
            Me.AfficherDetail(Me.XzCrListView1.Items(Index).Text)
        Next
    End Sub

    Private Sub XzCrButtonComparer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XzCrButtonComparer.Click
        nettoyerAffichageComparaison()

        If Me.XzCrListView1.CheckedItems.Count <> 2 Then
            MessageBox.Show("Vous devez cocher deux fichiers pour faire une comparaison !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            Using ChangerCurseur(Cursors.WaitCursor)
                Me.Comparer(Me.XzCrListView1.CheckedItems(0).Text, Me.XzCrListView1.CheckedItems(1).Text)
            End Using
        End If
    End Sub

    Private Sub XzCrButtonAfficherDifferences_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XzCrButtonAfficherDifferences.Click
        Using fdDifference As New TsFdDifferences()
            fdDifference.AfficherDifferences(Me.mNomFichier1, Me.mNomFichier2, Me.XzCrTreeView1.Nodes)
            fdDifference.ShowDialog()
        End Using
    End Sub

#End Region

#Region "Fonctions de services"
    ' This delegate enables asynchronous calls for RafraichirListeDeFichier
    Delegate Sub RafraichirListeDeFichierCallback(ByVal path As String)

    Private Sub RafraichirListeDeFichier(ByVal path As String)
        ' InvokeRequired required compares the thread ID of the
        ' calling thread to the thread ID of the creating thread.
        ' If these threads are different, it returns true.
        If Me.XzCrListView1.InvokeRequired Then
            Dim d As New RafraichirListeDeFichierCallback(AddressOf RafraichirListeDeFichier)
            Me.Invoke(d, New Object() {path})
        Else
            Me.XzCrListView1.Items.Clear()
            Dim newCurrentDirectory As DirectoryInfo = New DirectoryInfo(path)

            Dim fileArray As FileInfo() = newCurrentDirectory.GetFiles("*.xml")
            Dim file As FileInfo
            For Each file In fileArray
                Me.XzCrListView1.Items.Add(file.Name)
            Next
        End If
    End Sub

    Private Sub EnregistrerAD(ByVal NumeroUniteAdministrative As String)
        Dim ad As String = String.Format("LDAP://{0}", XzCrCboServeurActiveDirectory.Text)
        Dim att As String = XzCrTxtAttributDeRecherche.Text

        Dim recherche As New Chercheur(ad, att)
        Dim uniteAdministrative As TsCdUniteAdministrative = recherche.ObtenirUniteAdministrative(NumeroUniteAdministrative)

        Dim nomFichier As String = String.Format("{0}-{1}.xml", uniteAdministrative.NumeroUniteAdministrative, uniteAdministrative.DateDeSauvegarde.ToString("yyyyMMdd'-'HHmmss"))
        uniteAdministrative.Serialize(Path.Combine(Me.XzCrTextboxRepertoireSauvegarde.Text, nomFichier))
    End Sub

    Private Sub AfficherDetail(ByVal nomFichier As String)
        Try
            Dim filename As String = Path.Combine(Me.XzCrTextboxRepertoireSauvegarde.Text, nomFichier)
            Dim uniteAdministrative As TsCdUniteAdministrative = filename.Deserialize()

            Dim message As String =
                "Unité : {1}{0}" &
                "Date de sauvegarde : {2}{0}" &
                "Nombre d'utilisateurs : {3}{0}" &
                "Nombre de groupes totales : {4}"

            message = String.Format(message,
                                    Environment.NewLine,
                                    uniteAdministrative.NumeroUniteAdministrative,
                                    uniteAdministrative.DateDeSauvegarde,
                                    uniteAdministrative.NombreUtilisateurs,
                                    uniteAdministrative.NombreGroupes)

            Me.XzCrGroupBoxDetail.Text = String.Concat("Détail sur ", nomFichier)
            Me.XzCrLabelDetails.Text = message

        Catch ex As InvalidOperationException
            MessageBox.Show(String.Format("Le fichier xml '{0}' est non valide", nomFichier), "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Comparer(ByVal nomFichier1 As String, ByVal nomFichier2 As String)
        Dim uniteAdministrative1 As TsCdUniteAdministrative = chargerUniteAdministrative(nomFichier1)
        If uniteAdministrative1 Is Nothing Then Return

        Dim uniteAdministrative2 As TsCdUniteAdministrative = chargerUniteAdministrative(nomFichier2)
        If uniteAdministrative2 Is Nothing Then Return

        Me.mNomFichier1 = nomFichier1
        Me.mNomFichier2 = nomFichier2

        Dim utilisateursAjoutesAuFichier2 As New List(Of String)()
        Dim utilisateursAbscentsDuFichier2 As New List(Of String)()
        Dim groupesAjoutesPourUtilisateur2 As New Dictionary(Of String, List(Of String))()
        Dim groupesAbscentPourUtilisateur2 As New Dictionary(Of String, List(Of String))()

        comparerUnitesAdministratives(uniteAdministrative1, uniteAdministrative2, utilisateursAbscentsDuFichier2, groupesAbscentPourUtilisateur2)
        comparerUnitesAdministratives(uniteAdministrative2, uniteAdministrative1, utilisateursAjoutesAuFichier2, groupesAjoutesPourUtilisateur2)

        afficherComparaison(utilisateursAjoutesAuFichier2, utilisateursAbscentsDuFichier2, groupesAjoutesPourUtilisateur2, groupesAbscentPourUtilisateur2)
    End Sub

    Private Function chargerUniteAdministrative(fichier As String) As TsCdUniteAdministrative
        Const GABARIT_FICHIER_INVALIDE As String = "Le fichier xml '{0}' est non valide"

        Try
            Dim filename As String = Path.Combine(Me.XzCrTextboxRepertoireSauvegarde.Text, fichier)
            Return filename.Deserialize()
        Catch ex As InvalidOperationException
            MessageBox.Show(String.Format(GABARIT_FICHIER_INVALIDE, fichier), "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        End Try
    End Function

    Private Sub comparerUnitesAdministratives(unAdmA As TsCdUniteAdministrative, unAdmB As TsCdUniteAdministrative, ByRef diffUsers As List(Of String), ByRef diffGroupes As Dictionary(Of String, List(Of String)))
        ' On compare les utilisateurs de l'unité administrative 1 avec l'unite administrative 2
        For Each utilisateurA As TsCdUtilisateur In unAdmA.ListeUtilisateurs
            Dim clefUtilisateurA As String = String.Format("{0} - {1}", utilisateurA.CodeUtilisateur, utilisateurA.NomComplet)

            Dim utilisateurB As TsCdUtilisateur = unAdmB.ListeUtilisateurs.Find(Function(x) x.CodeUtilisateur = utilisateurA.CodeUtilisateur)
            If utilisateurB Is Nothing Then
                diffUsers.Add(clefUtilisateurA)
            Else
                Dim clefUtilisateurB As String = String.Format("{0} - {1}", utilisateurB.CodeUtilisateur, utilisateurB.NomComplet)

                ' On verifie les groupes entre les deux fichiers
                diffGroupes(clefUtilisateurB) = New List(Of String)
                For Each groupe As String In utilisateurA.ListeGroupes
                    If Not utilisateurB.ListeGroupes.Contains(groupe) Then
                        diffGroupes(clefUtilisateurB).Add(groupe)
                    End If
                Next
            End If
        Next
    End Sub

    Private Sub nettoyerAffichageComparaison()
        Me.Text = cst_TITLE
        Me.XzCrLabelDifferences.Text = String.Empty
        Me.XzCrLabelDetails.Text = String.Empty
        Me.XzCrTreeView1.Nodes.Clear()
        Me.Refresh()
    End Sub

    Private Sub afficherComparaison(utilisateursAjoutesAuFichier2 As List(Of String), utilisateursAbscentsDuFichier2 As List(Of String), groupesAjoutesPourUtilisateur2 As Dictionary(Of String, List(Of String)), groupesAbscentPourUtilisateur2 As Dictionary(Of String, List(Of String)))
        ' Afficher le résumé des différences
        Dim message As String =
            "1er fichier : {1}{0}" &
            "2ème fichier : {2}{0}{0}" &
            "Nombre d'utilisateurs ajoutées : {3}{0}" &
            "Nombre d'utilisateurs supprimées : {4}{0}" &
            "Nombre de groupes ajoutées : {5}{0}" &
            "Nombre de groupes supprimées : {6}{0}"

        message = String.Format(message,
                                Environment.NewLine,
                                mNomFichier1,
                                mNomFichier2,
                                utilisateursAjoutesAuFichier2.Count,
                                utilisateursAbscentsDuFichier2.Count,
                                groupesAjoutesPourUtilisateur2.Sum(Function(x) x.Value.Count),
                                groupesAbscentPourUtilisateur2.Sum(Function(x) x.Value.Count))

        Me.XzCrLabelDifferences.Text = message
        AfficherDetailDifferences(mNomFichier1, mNomFichier2, utilisateursAjoutesAuFichier2, utilisateursAbscentsDuFichier2, groupesAjoutesPourUtilisateur2, groupesAbscentPourUtilisateur2)
    End Sub

    Public Sub AfficherDetailDifferences(ByVal fichier1 As String, ByVal fichier2 As String,
                                         ByVal utilisateursAjoutes As List(Of String), ByVal utilisateursSupprimes As List(Of String),
                                         ByVal groupesAjoutes As Dictionary(Of String, List(Of String)), ByVal groupesSupprimes As Dictionary(Of String, List(Of String)))

        Me.Text = String.Format("Différences entre {0} et {1}", fichier1, fichier2)
        Me.XzCrTreeView1.Nodes.Clear()

        ajouterDifferenceUtilisateurs(XzCrTreeView1, "Utilisateurs ajoutés", utilisateursAjoutes)
        ajouterDifferenceUtilisateurs(XzCrTreeView1, "Utilisateurs supprimés", utilisateursSupprimes)

        ajouterDifferencesGroupes(XzCrTreeView1, "Groupes ajoutés", groupesAjoutes)
        ajouterDifferencesGroupes(XzCrTreeView1, "Groupes supprimés", groupesSupprimes)

        Me.XzCrTreeView1.ExpandAll()
    End Sub

    Private Sub ajouterDifferenceUtilisateurs(tree As XZCrTreeView, nomNode As String, utilisateurs As List(Of String))
        Dim node As TreeNode = tree.Nodes.Add(nomNode)
        For Each utilisateur As String In utilisateurs
            node.Nodes.Add(utilisateur)
        Next
    End Sub

    Private Sub ajouterDifferencesGroupes(tree As XZCrTreeView, nomNode As String, groupes As Dictionary(Of String, List(Of String)))
        Dim node As TreeNode = tree.Nodes.Add(nomNode)
        For Each utilisateur As KeyValuePair(Of String, List(Of String)) In groupes
            If utilisateur.Value.Count > 0 Then
                Dim utilisateurNode As TreeNode = node.Nodes.Add(utilisateur.Key)
                For Each groupe As String In utilisateur.Value
                    utilisateurNode.Nodes.Add(groupe)
                Next
            End If
        Next
    End Sub

    Private Sub InitialiserWatcher(ByVal path As String)
        ' Remove handlers if exists
        RemoveHandler mWatcher.Created, AddressOf OnRepertoireDeTravailChanged
        RemoveHandler mWatcher.Deleted, AddressOf OnRepertoireDeTravailChanged
        RemoveHandler mWatcher.Renamed, AddressOf OnRepertoireDeTravailChanged

        mWatcher.Path = path
        mWatcher.NotifyFilter = (NotifyFilters.LastAccess Or NotifyFilters.LastWrite Or NotifyFilters.FileName)
        mWatcher.Filter = "*.xml"

        AddHandler mWatcher.Created, AddressOf OnRepertoireDeTravailChanged
        AddHandler mWatcher.Deleted, AddressOf OnRepertoireDeTravailChanged
        AddHandler mWatcher.Renamed, AddressOf OnRepertoireDeTravailChanged

        mWatcher.EnableRaisingEvents = True
    End Sub

    Private Sub OnRepertoireDeTravailChanged(ByVal source As Object, ByVal e As FileSystemEventArgs)
        Me.RafraichirListeDeFichier(Path.GetDirectoryName(e.FullPath))
    End Sub

    Private Sub ModifierRepertoireDeTravail(ByVal path As String)
        InitialiserWatcher(path)
        RafraichirListeDeFichier(path)
    End Sub

    Public Sub ActiverComparaison(ByVal activer As Boolean)
        Me.XzCrButtonComparer.Enabled = activer
        Me.XzCrGroupBoxDifferences.Enabled = activer
        nettoyerAffichageComparaison()
        Me.XzCrLabelDifferences.Text = "Vous devez cocher deux fichiers pour faire une comparaison !"
    End Sub

#End Region
End Class