Imports System.Windows.Forms
Imports Rrq.CS.ServicesCommuns.ScenarioTransactionnel
Imports Rrq.CS.ServicesCommuns.UtilitairesCommuns
Imports System.Collections.Generic

'''-----------------------------------------------------------------------------
''' Project		: $safeprojectname$
''' Class		: TsFdDifferences
'''
'''-----------------------------------------------------------------------------
''' <summary>
''' Fenêtre principale de l'application.
''' </summary>
''' <remarks></remarks>
'''-----------------------------------------------------------------------------
Public Class TsFdDifferences

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

#Region "--- Énumérations ---"

#End Region

#Region "--- Constantes ---"

#End Region

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


#Region "--- Publiques ---"

#Region "--- Propriétés ---"

#End Region

#Region "--- Méthodes ---"
    Public Sub AfficherDifferences(ByVal NomFichier1 As String, ByVal NomFichier2 As String, ByVal nodeCollection As TreeNodeCollection)

        Me.Text = "Différences entre " + NomFichier1 + " et " + NomFichier2

        Me.XzCrTreeView1.Nodes.Clear()
        For Each node As TreeNode In nodeCollection
            Me.XzCrTreeView1.Nodes.Add(DirectCast(node.Clone(), TreeNode))
        Next

        Me.XzCrTreeView1.ExpandAll()
    End Sub
#End Region

#End Region


#Region "--- Privées ---"

#Region "--- Propriétés ---"

#End Region

#Region "--- Méthodes ---"

#End Region

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

End Class