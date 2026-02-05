Imports System.Windows.Forms
Imports System.IO

''' <summary>
''' Connecteur pour les fichiers CSV, compatible avec la cible agtdos-complete-sample de IDM Suite (Hitachi ID)
''' </summary>
''' <remarks>
''' On l'utilise pour simuler TSS.
''' ATTENTION, ce connecteur n'est pas testé, on n'en a pas besoin pour le moment.
''' </remarks>
<TsAtCibleRBACAttribute(False)> _
Public Class TsCdConnecteurCSV
    Inherits TsCuConnecteurCibleIgnorable

    Private _accesCSV As TsCdAccountGroupCSV

    Private _disposedValue As Boolean = False        ' Pour détecter les appels redondants

    ''' <summary>
    ''' Description du système cible compréhensible par l'utilisateur.
    ''' </summary>
    Public Overrides ReadOnly Property DescrCible() As String
        Get
            Return "CSV (simulation TSS)"
        End Get
    End Property

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    Public Sub New(ByVal idCible As String, ByVal cheminAccounts As String, ByVal cheminGroups As String)
        MyBase.New(idCible)

        _accesCSV = New TsCdAccountGroupCSV(cheminAccounts, cheminGroups)
    End Sub


    ''' <summary>
    ''' Constructeur pour formulaire Windows.
    ''' </summary>
    Public Sub New(ByVal idCible As String)
        MyBase.New(idCible)

        _accesCSV = Nothing
    End Sub


    ''' <summary>
    ''' Gère les ajouts/suppressions de groupes à un utilisateur dans un fichier CSV.
    ''' </summary>
    ''' <param name="ajouts">Liste de liens Utilisateur/Ressource à ajouter.</param>
    ''' <param name="suppr">Liste de liens Utilisateur/Ressource à supprimer.</param>
    ''' <param name="contnErreur">
    ''' True: si une erreur est rencontrée, l'erreur est prise en compte, mais le traitement continue.
    ''' False: si une erreur est rencontrée, l'erreur est prise en compte et le traitement s'arrête immédiatement.
    ''' </param>
    ''' <returns>Si la méthode a rencontrée au moins une erreur, elle retourne <c>False</c>, sinon elle retourne <c>True</c>.</returns>
    Public Overrides Function AppliquerLiensUserRessr(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRessr), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRessr), Optional ByVal contnErreur As Boolean = False) As Boolean
        If _accesCSV Is Nothing Then
            Return False
        End If

        Dim toutOk As Boolean = True

        For Each elementAjout As TsCdConnxUserRessr In ajouts
            If elementAjout.CibleAJour < TsECcCibleAJour.ExtractionAJour Then
                If _accesCSV.AjouterGroupeUtilisateur(elementAjout.NomRessource, elementAjout.CodeUtilisateur) Then
                    elementAjout.CibleAJour = TsECcCibleAJour.AJour
                Else
                    If contnErreur Then
                        toutOk = False
                    Else
                        Return False
                    End If
                End If
            End If
        Next

        For Each elementSupp As TsCdConnxUserRessr In suppr
            If elementSupp.CibleAJour < TsECcCibleAJour.ExtractionAJour Then
                If _accesCSV.EnleverGroupeUtilisateur(elementSupp.NomRessource, elementSupp.CodeUtilisateur) Then
                    elementSupp.CibleAJour = TsECcCibleAJour.AJour
                End If
            Else
                If contnErreur Then
                    toutOk = False
                Else
                    Return False
                End If
            End If
        Next

        ' On pourrait attendre le Dispose, mais pourquoi ne pas le faire ici...
        _accesCSV.Flush()

        Return toutOk
    End Function

    ''' <summary>
    ''' Vérifie, dans le fichier CSV, les liens entre les rôles et les ressources qui sont déjà existants ou inexistants.
    ''' </summary>
    ''' <param name="ajouts">Liens dont l'inexistance est à vérifier dans le fichier CSV.</param>
    ''' <param name="suppr">Liens dont l'existance est à vérifier dans le fichier CSV.</param>
    ''' <remarks>
    ''' Pour chaque liens à ajouter, si dans le fichier d'extraction TSS le lien n'est pas fait, le lien verra sa variable
    ''' <see cref="TsCdConnxUserRessr.CibleAJour"/> changé à la valeurs <see cref="TsECcCibleAJour.AJour" />,
    ''' sinon elle sera changé à <see cref="TsECcCibleAJour.PasAJour" />.
    ''' Pour chaque liens à supprimer, si dans le fichier d'extraction TSS le lien est fait, alors le lien verra sa variable
    ''' <see cref="TsCdConnxUserRessr.CibleAJour"/> changé à la valeurs <see cref="TsECcCibleAJour.AJour" />,
    ''' sinon elle sera changé à <see cref="TsECcCibleAJour.PasAJour" />.
    ''' </remarks>
    Public Overrides Sub VerifierLiensUserRessr(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRessr), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRessr))
        If _accesCSV Is Nothing Then
            Exit Sub
        End If

        For Each elementAjout As TsCdConnxUserRessr In ajouts

            If _accesCSV.UtilisateurMembre(elementAjout.NomRessource, elementAjout.CodeUtilisateur) Then
                elementAjout.CibleAJour = TsECcCibleAJour.ExtractionAJour
            Else
                elementAjout.CibleAJour = TsECcCibleAJour.ExtractionPasAJour
            End If
        Next

        For Each elementSupp As TsCdConnxUserRessr In suppr
            If Not _accesCSV.UtilisateurMembre(elementSupp.NomRessource, elementSupp.CodeUtilisateur) Then
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
                If _accesCSV IsNot Nothing Then
                    _accesCSV.Dispose()
                End If
            End If
        End If
        _disposedValue = True
    End Sub

    ''' <summary>
    ''' Méthode d'initialisation. Cette fonction prépare l'utilisation du connecteur.
    ''' </summary>
    Public Overrides Function Initialiser() As Boolean
        Return InitialiserAccesCSV()
    End Function


    ''' <summary>
    ''' Fonction de service. Sous fonction d'Initialiser().
    ''' Créer un nouveau TsBaAccesTSS et demande des spécifications à l'utilisateur.
    ''' </summary>
    Private Function InitialiserAccesCSV() As Boolean
        _accesCSV = Nothing

        Dim cheminAccounts As String
        Using dlgOpenFile As New OpenFileDialog()

            dlgOpenFile.Filter = "Fichier CSV (*.csv)|*.csv|Tous les fichier (*.*)|*.*"
            dlgOpenFile.Title = IdCible + " - Ouvrir le fichier «Accounts»"
            dlgOpenFile.Multiselect = False
            If dlgOpenFile.ShowDialog() <> DialogResult.OK Then
                Return False
            End If
            cheminAccounts = dlgOpenFile.FileName
        End Using

        Dim cheminGroups As String
        Using dlgOpenFile As New OpenFileDialog()
            dlgOpenFile.Filter = "Fichier CSV (*.csv)|*.csv|Tous les fichier (*.*)|*.*"
            dlgOpenFile.Title = IdCible + " - Ouvrir le fichier «Groups»"
            If dlgOpenFile.ShowDialog() <> Windows.Forms.DialogResult.OK Then
                Return False
            End If
            cheminGroups = dlgOpenFile.FileName
        End Using

        _accesCSV = New TsCdAccountGroupCSV(cheminAccounts, cheminGroups)

        Return True
    End Function


End Class
