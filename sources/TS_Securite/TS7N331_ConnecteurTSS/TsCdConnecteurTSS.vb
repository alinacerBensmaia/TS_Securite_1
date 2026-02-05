Imports System.Windows.Forms
Imports System.IO

<TsAtCibleRBACAttribute(False)> _
Public Class TsCdConnecteurTSS
    Inherits TsCuConnecteurCibleIgnorable

#Region "Variable privé"

    Private accesTSS As TsBaAccesTSS

    Private _disposedValue As Boolean = False        ' Pour détecter les appels redondants

#End Region

#Region "Property"

    ''' <summary>
    ''' Description du système cible compréhensible par l'utilisateur.
    ''' </summary>
    Public Overrides ReadOnly Property DescrCible() As String
        Get
            Return "Top Secret"
        End Get
    End Property

#End Region

#Region "Constructeurs"

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    Public Sub New(ByVal idCible As String, ByVal fichierExtraction As String, ByVal fichierCommande As String)
        MyBase.New(idCible)

        accesTSS = New TsBaAccesTSS(fichierExtraction, fichierCommande)
    End Sub

    ''' <summary>
    ''' Constructeur pour formulaire Windows.
    ''' </summary>
    ''' <param name="idCible">Identification de la cible.</param>
    ''' <remarks>Est appeler par un invoke lors d'un cération d'objet.</remarks>
    Public Sub New(ByVal idCible As String)
        MyBase.New(idCible)

        accesTSS = Nothing
    End Sub

#End Region

#Region "Méthodes"

    ''' <summary>
    ''' Fonction du Connecteur TSS. Cette fonction écrit des commandes d'ajouts de liens Utilisateur/Ressource de la liste <paramref name="ajouts"/>
    ''' et va écrire des commandes de suppressions de liens Utilisateur/Ressource à partir de la liste <paramref name="suppr"/>.
    ''' </summary>
    ''' <param name="ajouts">Liste de liens Utilisateur/Ressource à ajouter.</param>
    ''' <param name="suppr">Liste de liens Utilisateur/Ressource à supprimer.</param>
    ''' <param name="contnErreur">
    ''' True = Si une erreur est rencontrée, l'erreur est prise en compte, mais le traitement continue.
    ''' False =  Si une erreur est rencontrée, l'erreur est prise en compte et le traitement s'arrête immédiatement.
    ''' </param>
    ''' <returns>Si la méthode a rencontrée au moins une erreur, elle retourne <c>False</c>, sinon elle retourne <c>True</c>.</returns>
    Public Overrides Function AppliquerLiensUserRessr(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRessr), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRessr), Optional ByVal contnErreur As Boolean = False) As Boolean
        If accesTSS Is Nothing Then
            Return False
        End If

        Dim toutOk As Boolean = True

        For Each elementAjout As TsCdConnxUserRessr In ajouts
            If elementAjout.CibleAJour < TsECcCibleAJour.ExtractionAJour Then
                accesTSS.AjouterProfileUtilisateur(elementAjout.NomRessource, elementAjout.CodeUtilisateur)
                elementAjout.CibleAJour = TsECcCibleAJour.ExtractionAJour
            End If
        Next

        For Each elementSupp As TsCdConnxUserRessr In suppr
            If elementSupp.CibleAJour < TsECcCibleAJour.ExtractionAJour Then
                accesTSS.EnleverProfileUtilisateur(elementSupp.NomRessource, elementSupp.CodeUtilisateur)
                elementSupp.CibleAJour = TsECcCibleAJour.ExtractionAJour
            End If
        Next

        accesTSS.Dispose()

        Return toutOk
    End Function

    ''' <summary>
    ''' Vérifie dans le fichier d'extraction TSS, les liens entre les rôles et les ressources qui sont déjà existants ou inexistants.
    ''' </summary>
    ''' <param name="ajouts">Liens dont l'inexistance est à vérifier dans le fichier d'extraction TSS.</param>
    ''' <param name="suppr">Liens dont l'existance est à vérifier dans le fichier d'extraction TSS.</param>
    ''' <remarks>
    ''' Pour chaque liens à ajouter, si dans le fichier d'extraction TSS le lien n'est pas fait, le lien verra sa variable
    ''' <see cref="TsCdConnxUserRessr.CibleAJour"/> changé à la valeurs <see cref="TsECcCibleAJour.AJour" />,
    ''' sinon elle sera changé à <see cref="TsECcCibleAJour.PasAJour" />.
    ''' Pour chaque liens à supprimer, si dans le fichier d'extraction TSS le lien est fait, alors le lien verra sa variable
    ''' <see cref="TsCdConnxUserRessr.CibleAJour"/> changé à la valeurs <see cref="TsECcCibleAJour.AJour" />,
    ''' sinon elle sera changé à <see cref="TsECcCibleAJour.PasAJour" />.
    ''' </remarks>
    Public Overrides Sub VerifierLiensUserRessr(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRessr), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRessr))
        If accesTSS Is Nothing Then
            Exit Sub
        End If

        For Each elementAjout As TsCdConnxUserRessr In ajouts

            If accesTSS.ProfileDejaAssocier(elementAjout.NomRessource, elementAjout.CodeUtilisateur) = True Then
                elementAjout.CibleAJour = TsECcCibleAJour.ExtractionAJour
            Else
                elementAjout.CibleAJour = TsECcCibleAJour.ExtractionPasAJour
            End If
        Next

        For Each elementSupp As TsCdConnxUserRessr In suppr
            If accesTSS.ProfileDejaDesassocier(elementSupp.NomRessource, elementSupp.CodeUtilisateur) = True Then
                elementSupp.CibleAJour = TsECcCibleAJour.ExtractionAJour
            Else
                elementSupp.CibleAJour = TsECcCibleAJour.ExtractionPasAJour
            End If
        Next
    End Sub

    ''' <summary>
    ''' Méthode protégé. Cette méthode est appelée quand l'objet n'est plus utile, 
    ''' alors les ressources, qu'il gère, sont libérées.
    ''' </summary>
    ''' <param name="disposing"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If Not _disposedValue Then
            If disposing Then
                If accesTSS IsNot Nothing Then
                    accesTSS.Dispose()
                End If
            End If
        End If
        _disposedValue = True
    End Sub

    ''' <summary>
    ''' Méthode d'initialisation. Cette fonction prépare l'utilisation du connecteur.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function Initialiser() As Boolean
        Return InitialiserAccesTSS()
    End Function

#End Region

#Region "Fonctions de services"

    ''' <summary>
    ''' Fonction de service. Sous fonction d'Initialiser().
    ''' Créer un nouveau TsBaAccesTSS et demande des spécifications à l'utilisateur.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function InitialiserAccesTSS() As Boolean
        'force le dispose pour qu'il ferme le fichier en écriture
        If Not accesTSS Is Nothing Then
            accesTSS.Dispose()
        End If
        accesTSS = Nothing

        Dim dlgOpenFile As New OpenFileDialog()
        Dim dlgSaveFile As New SaveFileDialog()
        Dim dlgResultat As DialogResult

        dlgOpenFile.Filter = "Fichier Texte (*.Txt)|*.txt|Tous les fichier (*.*)|*.*"
        dlgOpenFile.Title = IdCible + " - Ouvrir le fichier d'extraction TSS "
        dlgOpenFile.Multiselect = False
        dlgResultat = dlgOpenFile.ShowDialog()

        dlgSaveFile.Filter = "Fichier Texte (*.Txt)|*.txt|Tous les fichier (*.*)|*.*"
        dlgSaveFile.Title = IdCible + " - Enregistrer le fichier de commande sous ..."
        dlgResultat = dlgSaveFile.ShowDialog()

        If dlgResultat <> Windows.Forms.DialogResult.OK Then
            Return False
        End If

        accesTSS = New TsBaAccesTSS(dlgOpenFile.FileName, dlgSaveFile.FileName)

        Return True
    End Function

#End Region

End Class
