Imports Rrq.InfrastructureCommune.ScenarioTransactionnel



''' <summary>
''' Cette classe permet d'obtenir le rapport.
''' </summary>
''' <remarks></remarks>
Friend Class TsCaComposerRapport

#Region "--- Variables ---"

    ''' <summary>Source du rapports.</summary>
    Private mSources As Dictionary(Of String, TsDtSourceRapport)

    ''' <summary>Date de production.</summary>
    Private mDateProduction As Date

#End Region

#Region "--- Constructeurs ---"

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="pSources">Les sources d'informtaions pour produire le rapport.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal pSources As Dictionary(Of String, TsDtSourceRapport))
        mSources = pSources

        mDateProduction = Date.Now
    End Sub

#End Region

#Region "--- Méthodes ---"

    ''' <summary>
    ''' Permet d'obtenir le rapport en format XML.
    ''' </summary>
    ''' <returns>Le rapport en XML.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirRapport() As String

        Dim lstCompositeur As New List(Of TsICompositeurFeuille)

        For Each skv In mSources
            Dim compositeur As TsICompositeurFeuille

            compositeur = FabriquerCompositeur(skv.Key, skv.Value)
            If compositeur IsNot Nothing Then
                lstCompositeur.Add(compositeur)
            End If
        Next

        If lstCompositeur.Count = 0 Then
            Throw New Rrq.InfrastructureCommune.ScenarioTransactionnel.XuExcEErrFatale("Erreur lors de la composition du rapport. Aucun compositeur disponible pour les sources fournis.")
        End If

        Dim lstFeuille As New List(Of TsCuWorksheet)

        For Each c In lstCompositeur
            lstFeuille.Add(c.ProduireFeuilleTravail())
        Next

        Dim livreDeTravail As New TsCuWorkbook(lstFeuille)
        Dim documentExcel As New TsCuExcelXml(livreDeTravail)

        AjouterProprietesDocument(livreDeTravail)
        AjouterStyle(livreDeTravail)

        Return documentExcel.ObtenirXML()
    End Function

#End Region

#Region "--- Sous méthodes ---"

    ''' <summary>
    ''' Permet d'ajouté au livre de travail, les informations sur le documents.
    ''' </summary>
    ''' <param name="pLivreDeTravail">Un livre de travail.</param>
    ''' <remarks></remarks>
    Private Sub AjouterProprietesDocument(ByVal pLivreDeTravail As TsCuWorkbook)
        Dim proprietesDocuments As New TsCuDocumentProperties()

        With proprietesDocuments
            .Author = "Traitement TS71010"
            .Company = "Régie des Rentes du Québec"
            .Created = mDateProduction.ToString("yyyy-MM-ddThh:mm:ssZ")
            .Version = "1.00"
        End With

        pLivreDeTravail.DocumentProperties = proprietesDocuments
    End Sub

    ''' <summary>
    ''' Permet d'ajouter les Styles qui sont associé au livre de travail.
    ''' </summary>
    ''' <param name="pLivreDeTravail">Un livre de travail Excel.</param>
    ''' <remarks></remarks>
    Private Sub AjouterStyle(ByVal pLivreDeTravail As TsCuWorkbook)
        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirParDefaut())

        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirHyperLien())

        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleEnteteRapport())
        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleEnteteTableau())
        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleSousEnteteTableau())
        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleEnteteCoupe())
        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleSousEnteteCoupe())

        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleUA())

        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleNom())
        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleNomBas())
        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleNomHaut())

        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleNoUA())
        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleNoUABas())
        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleNoUAHaut())

        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleSeparateurFonce())
        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleSeparateurPale())
        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleVideGauche())
        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleVideDroite())
        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleVideHaut())
        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleVideCoinHautGauche())
        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleVideCoinHautDroit())
        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleVideBas())

        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleGrille())
        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleGrilleMarque())

        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleTotal())
        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleTotalGauche())

        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleSousTotal())
        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleSousTotalGauche())

        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleSousTotalBarre())
        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleSousTotalBarreGauche())

        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleContexteGauche())
        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleContexteDroite())

        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleLegendeBarree())
        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleLegendeJaune())
        pLivreDeTravail.Styles.Add(TsCaStylesRapport.ObtenirCelluleLegendeTexte())
    End Sub

#End Region

#Region "--- Fonctions privées ---"

    ''' <summary>
    ''' Fabrique de compositeur de feuille pour le rapport.
    ''' </summary>
    ''' <param name="pIdentifiant">L'identifiant de la feuille.</param>
    ''' <param name="pSource">La source à fournir au compositeur.</param>
    ''' <returns>Un compositeur de feuille ou nulle si aucun compositeur n'est trouvé.</returns>
    ''' <remarks></remarks>
    Private Function FabriquerCompositeur(pIdentifiant As String, pSource As TsDtSourceRapport) As TsICompositeurFeuille
        Dim retour As TsICompositeurFeuille = Nothing

        Select Case pIdentifiant
            Case TsCuConstantesRapport.ID_FEUILLE_1
                retour = New TsCaFeuillePrincipale(pSource)

        End Select

        Return retour
    End Function

#End Region

End Class

