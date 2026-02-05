Imports System.Windows.Forms
Imports Rrq.CS.ServicesCommuns.ScenarioTransactionnel
Imports Rrq.CS.ServicesCommuns.UtilitairesCommuns

'''-----------------------------------------------------------------------------
''' Project		: $safeprojectname$
''' Class		: TsFdListBox
'''
'''-----------------------------------------------------------------------------
''' <summary>
''' Fenêtre principale de l'application.
''' </summary>
''' <remarks></remarks>
'''-----------------------------------------------------------------------------
Public Class TsFdBoiteSelection

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

#Region "--- Variables ---"

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

#Region "--- Privées ---"

    ''' <summary>
    ''' Si l'action est initié par les la croix de fermeture.
    ''' </summary>
    ''' <remarks></remarks>
    Dim CliqueCroixFermeture As Boolean = True

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

#Region "Méthodes"
    ''' <summary>
    ''' Initialise la liste de la boite de selection.
    ''' </summary>
    ''' <param name="listeSelections">Une liste de texte.</param>
    ''' <remarks></remarks>
    Public Sub InitialiserListe(ByVal listeSelections As List(Of String))
        lstSelection.Items.Clear()
        listeSelections.Sort()
        For Each selection As String In listeSelections
            lstSelection.Items.Add(selection)
        Next
    End Sub

    ''' <summary>
    ''' Méthode alternative au showDialogue qui retourne un string.
    ''' </summary>
    ''' <returns>La sélection.</returns>
    ''' <remarks></remarks>
    Public Function OuvrirDialogue() As String
        Me.ShowDialog()
        If lstSelection.SelectedItem Is Nothing Then
            Return ""
        Else
            Return lstSelection.SelectedItem.ToString
        End If

    End Function

#End Region

#Region "Evenements"
    ''' <summary>
    ''' Fonction évènement. Ferme la fênêtre.
    ''' </summary>
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        CliqueCroixFermeture = False
        Me.Close()
    End Sub

    ''' <summary>
    ''' Fonction évènement. Ferme la fênêtre.
    ''' </summary>
    Private Sub lstSelection_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstSelection.MouseDoubleClick
        CliqueCroixFermeture = False
        Me.Close()
    End Sub
#End Region

    ''' <summary>
    ''' Fonction évènement. Vérifie si l'utilisateur a cliqué sur la croix de fermeture ou fait échape.
    ''' </summary>
    Private Sub TsFdBoiteSelection_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If CliqueCroixFermeture = True Then
            lstSelection.SelectedItem = Nothing
        End If
    End Sub

End Class