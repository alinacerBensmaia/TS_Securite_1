Imports TS7I132_CiGererRapports.TsCuConstantesRapport
Imports TS7I132_CiGererRapports.TsCuConstantesRapport.ConstantesStyle

''' <summary>
''' Cette classe contient la définiton des lignes du rapport.
''' </summary>
''' <remarks></remarks>
Friend Class TsCaLignesRapport

#Region "--- Variables ---"
    ' Définition des variable utilisé par toutes les méthodes.

    ''' <summary>Nombre de métiers dans la section sélectionnées.</summary>
    Private mNbMetiersSelect As Integer

    ''' <summary>Nombre de tâches dans la section sélectionnées.</summary>
    Private mNbTachesSelect As Integer

    ''' <summary>Nombre de métiers dans la section autres.</summary>
    Private mNbMetiersAutre As Integer

    ''' <summary>Nombre de tâches dans la section autres.</summary>
    Private mNbTachesAutre As Integer

    ''' <summary>La liste des métiers dans la section sélectionnées.</summary>
    Private mLstMetiersSelect As List(Of TsDtSourceUa)

    ''' <summary>La liste des tâches dans la section sélectionnées.</summary>
    Private mLstTachesSelect As List(Of TsDtSourceUa)

    ''' <summary>La liste des métiers dans la section autres.</summary>
    Private mLstMetiersAutre As List(Of TsDtSourceUa)

    ''' <summary>La liste des tâches dans la section autres.</summary>
    Private mLstTachesAutre As List(Of TsDtSourceUa)

    ''' <summary>La largeur totale du tableau en nombre de cellules.</summary>
    Private mLargeurTableau As ULong

    ''' <summary>La largeur totale de la section des sélectionnées en nombre de cellules.</summary>
    Private mLargeurUaSelect As ULong

    ''' <summary>La largeur totale de la section autres en nombre de cellules.</summary>
    Private mLargeurUaAutre As ULong

    ''' <summary>Le nombre d'employés dans le rapport.</summary>
    Private mNbEmployes As Integer

    ''' <summary>Le nombre de contexte de tâche dansle rapport.</summary>
    Private mNbContextes As Integer

    ''' <summary>Date de production du rapport.</summary>
    Private mDate As Date

#End Region

#Region "--- Constructeurs ---"

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="pSource">La source du rapport.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal pSource As TsDtSourceRapport)
        mLstMetiersSelect = pSource.ObtenirMetiersSelectionnes
        mLstTachesSelect = pSource.ObtenirRolesTachesSelectionnes
        mLstMetiersAutre = pSource.ObtenirMetiersAutre
        mLstTachesAutre = pSource.ObtenirRolesTachesAutre

        mNbMetiersSelect = mLstMetiersSelect.Count
        mNbTachesSelect = mLstTachesSelect.Count
        mNbMetiersAutre = mLstMetiersAutre.Count
        mNbTachesAutre = mLstTachesAutre.Count

        mLargeurTableau = CULng(TsCuOutilsRapport.ObtenirLargeurTableau(mNbMetiersSelect, mNbTachesSelect, mNbMetiersAutre, mNbTachesAutre))
        mLargeurUaSelect = CULng(TsCuOutilsRapport.ObtenirLargeurSectionUaSelect(mNbMetiersSelect, mNbTachesSelect))
        mLargeurUaAutre = CULng(TsCuOutilsRapport.ObtenirLargeurSectionUaAutre(mNbMetiersAutre, mNbTachesAutre))

        mNbEmployes = pSource.Employes.Count
        mNbContextes = pSource.Contextes.Count

        mDate = pSource.DateProduction
    End Sub

#End Region

#Region "--- Méthodes ---"

    ''' <summary>
    ''' Permet d'obtenir une ligne d'espacement.
    ''' </summary>
    ''' <returns>Une ligne.</returns>
    ''' <remarks></remarks>
    Public Function Espacement() As TsCuRow
        Return New TsCuRow()
    End Function

    ''' <summary>
    ''' Permet d'obtenir l'en-tête du tableau.
    ''' </summary>
    ''' <returns>Une ligne.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirEnteteTableau() As TsCuRow
        Dim ligne As New TsCuRow()

        With ligne
            .Cells.Add(TsCaCellulesRapport.ObtenirVide(CELLULE_VIDE_COIN_HAUT_GAUCHE))
            .Cells.Add(TsCaCellulesRapport.ObtenirVide(CELLULE_VIDE_COIN_HAUT_DROIT))

            Dim style As String = TsCuConstantesRapport.ConstantesStyle.CELLULE_ENTETE_TABLEAU

            If mLargeurUaSelect > 0 Then
                .Cells.Add(TsCaCellulesRapport.ObtenirEnteteTableau(style, mLargeurUaSelect, "Unité(s) sélectionnée(s)", "Unité(s) sélectionnée(s)"))
            End If

            If mLargeurUaSelect > 0 And mLargeurUaAutre > 0 Then
                .Cells.Add(TsCaCellulesRapport.ObtenirEnteteTableau(style, 1, "", ""))
            End If

            If mLargeurUaAutre > 0 Then
                .Cells.Add(TsCaCellulesRapport.ObtenirEnteteTableau(style, mLargeurUaAutre, "Autre(s) unité(s)", "Autre(s) unité(s)"))
            End If

            .Cells.Add(TsCaCellulesRapport.ObtenirVide(CELLULE_VIDE_GAUCHE))
        End With

        Return ligne
    End Function

    ''' <summary>
    ''' Permet d'obtenir la ligne du sous en-tête du tableau.
    ''' </summary>
    ''' <returns>Une ligne.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirSousEnteteTableau() As TsCuRow
        Dim ligne As New TsCuRow()

        With ligne
            .Cells.Add(TsCaCellulesRapport.ObtenirVide(CELLULE_VIDE_GAUCHE))
            .Cells.Add(TsCaCellulesRapport.ObtenirVide(CELLULE_VIDE_DROITE))

            AjouterSeparateurPale(ligne, 1, 1)

            Dim style As String = TsCuConstantesRapport.ConstantesStyle.CELLULE_SOUS_ENTETE_TABLEAU
            If mNbMetiersSelect > 0 Then
                .Cells.Add(TsCaCellulesRapport.ObtenirSousEntete(style, CULng(mNbMetiersSelect), "Métier(s)", "Métier(s)"))
            End If

            AjouterSeparateurPale(ligne, mNbMetiersSelect, mNbTachesSelect)

            If mNbTachesSelect > 0 Then
                .Cells.Add(TsCaCellulesRapport.ObtenirSousEntete(style, CULng(mNbTachesSelect), "Tâche(s)", "Tâche(s)"))
            End If

            AjouterSeparateurFonce(ligne, mNbMetiersSelect + mNbTachesSelect, mNbMetiersAutre + mNbTachesAutre)

            If mNbMetiersAutre > 0 Then
                .Cells.Add(TsCaCellulesRapport.ObtenirSousEntete(style, CULng(mNbMetiersAutre), "Métier(s)", "Métier(s)"))
            End If

            AjouterSeparateurPale(ligne, mNbMetiersAutre, mNbTachesAutre)

            If mNbTachesAutre > 0 Then
                .Cells.Add(TsCaCellulesRapport.ObtenirSousEntete(style, CULng(mNbTachesAutre), "Tâche(s)", "Tâche(s)"))
            End If

            .Cells.Add(TsCaCellulesRapport.ObtenirVide(CELLULE_VIDE_GAUCHE))
        End With

        Return ligne
    End Function

    ''' <summary>
    ''' Permet d'obtenir la ligne des unité administrative.
    ''' </summary>
    ''' <returns>Une ligne.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirUA() As TsCuRow
        Dim ligne As New TsCuRow()

        With ligne
            .Height = 200
            .Cells.Add(TsCaCellulesRapport.ObtenirVide(CELLULE_VIDE_GAUCHE))
            .Cells.Add(TsCaCellulesRapport.ObtenirVide(CELLULE_VIDE_DROITE))

            AjouterSeparateurPale(ligne, 1, 1)

            '--- Section Métiers selectionnées
            Dim style As String = TsCuConstantesRapport.ConstantesStyle.CELLULE_UA
            For Each m In mLstMetiersSelect
                .Cells.Add(TsCaCellulesRapport.ObtenirUA(style, m.Nom))
            Next

            '--- Séparateur de la section sélectionnées
            AjouterSeparateurPale(ligne, mNbMetiersSelect, mNbTachesSelect)

            '--- Section Tâches selectionnées
            For Each ts In mLstTachesSelect
                .Cells.Add(TsCaCellulesRapport.ObtenirUA(style, ts.Nom))
            Next

            '--- Séparateur Sélectionnée/autre
            AjouterSeparateurFonce(ligne, mNbMetiersSelect + mNbTachesSelect, mNbMetiersAutre + mNbTachesAutre)

            '--- Section Métiers autres
            For Each ma In mLstMetiersAutre
                .Cells.Add(TsCaCellulesRapport.ObtenirUA(style, ma.Nom))
            Next

            '--- Séparateur de la section Autres
            AjouterSeparateurPale(ligne, mNbMetiersAutre, mNbTachesAutre)

            '--- Section Tâche autres
            For Each ta In mLstTachesAutre
                .Cells.Add(TsCaCellulesRapport.ObtenirUA(style, ta.Nom))
            Next

            .Cells.Add(TsCaCellulesRapport.ObtenirVide(CELLULE_VIDE_GAUCHE))
        End With

        Return ligne
    End Function

    ''' <summary>
    ''' Permet d'obtenir l'en-tête coupé.
    ''' </summary>
    ''' <returns>Une ligne.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirEnteteCoupe() As TsCuRow
        Dim ligne As New TsCuRow()

        With ligne
            .AutoFitHeight = True
            .Cells.Add(TsCaCellulesRapport.ObtenirEnteteCoupe("Employés des unités demandées"))
            .Cells.Add(TsCaCellulesRapport.ObtenirEnteteCoupe("UA"))

            AjouterSeparateurPale(ligne, 1, 1)

            For i = 1 To mNbMetiersSelect
                .Cells.Add(TsCaCellulesRapport.ObtenirSeparateurPale())
            Next

            AjouterSeparateurPale(ligne, mNbMetiersSelect, mNbTachesSelect)

            For i = 1 To mNbTachesSelect
                .Cells.Add(TsCaCellulesRapport.ObtenirSeparateurPale())
            Next

            AjouterSeparateurFonce(ligne, mNbMetiersSelect + mNbTachesSelect, mNbMetiersAutre + mNbTachesAutre)

            For i = 1 To mNbMetiersAutre
                .Cells.Add(TsCaCellulesRapport.ObtenirSeparateurPale())
            Next

            AjouterSeparateurPale(ligne, mNbMetiersAutre, mNbTachesAutre)

            For i = 1 To mNbTachesAutre
                .Cells.Add(TsCaCellulesRapport.ObtenirSeparateurPale())
            Next

            .Cells.Add(TsCaCellulesRapport.ObtenirVide(CELLULE_VIDE_GAUCHE))
        End With

        Return ligne
    End Function

    ''' <summary>
    ''' Permet d'obtenir la première ligne d'employé.
    ''' </summary>
    ''' <param name="pEmploye">Un employé de la source.</param>
    ''' <returns>Une ligne.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirEmployeHaut(ByVal pEmploye As TsDtSourceEmploye) As TsCuRow
        Dim ligne As New TsCuRow()

        With ligne
            .Cells.Add(TsCaCellulesRapport.ObtenirEmploye(ConstantesStyle.CELLULE_NOM_HAUT, pEmploye.Nom))
            .Cells.Add(TsCaCellulesRapport.ObtenirNoUA(ConstantesStyle.CELLULE_NO_UA_HAUT, pEmploye.NoUA.ToString))
        End With

        EnrichirEmployeCommun(ligne, pEmploye)

        Return ligne
    End Function

    ''' <summary>
    ''' Permet d'obtenir une ligne d'employé.
    ''' </summary>
    ''' <param name="pEmploye">Un employé de la source.</param>
    ''' <returns>Une ligne.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirEmploye(ByVal pEmploye As TsDtSourceEmploye) As TsCuRow
        Dim ligne As New TsCuRow()

        With ligne
            .Cells.Add(TsCaCellulesRapport.ObtenirEmploye(ConstantesStyle.CELLULE_NOM, pEmploye.Nom))
            .Cells.Add(TsCaCellulesRapport.ObtenirNoUA(ConstantesStyle.CELLULE_NO_UA, pEmploye.NoUA.ToString))
        End With

        EnrichirEmployeCommun(ligne, pEmploye)

        Return ligne
    End Function

    ''' <summary>
    ''' Permet d'obtenir la dernière ligne d'employé.
    ''' </summary>
    ''' <param name="pEmploye">Un employé de la source.</param>
    ''' <returns>Une ligne.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirEmployeBas(ByVal pEmploye As TsDtSourceEmploye) As TsCuRow
        Dim ligne As New TsCuRow()

        With ligne
            .Cells.Add(TsCaCellulesRapport.ObtenirEmploye(ConstantesStyle.CELLULE_NOM_BAS, pEmploye.Nom))
            .Cells.Add(TsCaCellulesRapport.ObtenirNoUA(ConstantesStyle.CELLULE_NO_UA_BAS, pEmploye.NoUA.ToString))
        End With

        EnrichirEmployeCommun(ligne, pEmploye)

        Return ligne
    End Function

    ''' <summary>
    ''' Permet d'obtenir la ligne des totaux.
    ''' </summary>
    ''' <returns>Une ligne.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirTotaux() As TsCuRow
        Dim ligne As New TsCuRow()

        With ligne
            .Cells.Add(TsCaCellulesRapport.ObtenirSousEnteteCoupe())

            AjouterSeparateurPale(ligne, 1, 1)

            Dim premier As Boolean = True
            Dim style As String = TsCuConstantesRapport.ConstantesStyle.CELLULE_TOTAL_GAUCHE
            For Each m In mLstMetiersSelect
                If premier = True Then
                    premier = False
                    style = TsCuConstantesRapport.ConstantesStyle.CELLULE_TOTAL
                End If

                Dim formule = ConstruireFormuleTotaux(m.ContextesPermis.Count)

                .Cells.Add(TsCaCellulesRapport.ObtenirTotal(style, formule))
            Next

            AjouterSeparateurPale(ligne, mNbMetiersSelect, mNbTachesSelect)

            For Each ts In mLstTachesSelect
                If premier = True Then
                    premier = False
                    style = TsCuConstantesRapport.ConstantesStyle.CELLULE_TOTAL
                End If

                Dim formule = ConstruireFormuleTotaux(ts.ContextesPermis.Count)

                .Cells.Add(TsCaCellulesRapport.ObtenirTotal(style, formule))
            Next

            AjouterSeparateurFonce(ligne, mNbMetiersSelect + mNbTachesSelect, mNbMetiersAutre + mNbTachesAutre)

            For Each ma In mLstMetiersAutre
                If premier = True Then
                    premier = False
                    style = TsCuConstantesRapport.ConstantesStyle.CELLULE_TOTAL
                End If

                Dim formule = ConstruireFormuleTotaux(ma.ContextesPermis.Count)

                .Cells.Add(TsCaCellulesRapport.ObtenirTotal(style, formule))
            Next

            AjouterSeparateurPale(ligne, mNbMetiersAutre, mNbTachesAutre)

            For Each ta In mLstTachesAutre
                If premier = True Then
                    premier = False
                    style = TsCuConstantesRapport.ConstantesStyle.CELLULE_TOTAL
                End If

                Dim formule = ConstruireFormuleTotaux(ta.ContextesPermis.Count)

                .Cells.Add(TsCaCellulesRapport.ObtenirTotal(style, formule))
            Next

            .Cells.Add(TsCaCellulesRapport.ObtenirVide(CELLULE_VIDE_GAUCHE))
        End With

        Return ligne
    End Function

    ''' <summary>
    ''' Permet d'obtenir une ligne de contexte d'unité administrative.
    ''' </summary>
    ''' <param name="pContexte">Un contexte d'unité administrative.</param>
    ''' <param name="pPosition">La position du contexte comparer aux autres contextes.</param>
    ''' <returns>Une ligne.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirContextes(ByVal pContexte As TsDtSourceContexteUA, ByVal pPosition As Integer) As TsCuRow
        Dim ligne As New TsCuRow()

        Dim formule As String = "= "

        For i = 1 To mNbEmployes
            formule &= String.Format("+IF(R[-{0}]C=&quot;{1}&quot;,1,0)", i + pPosition, pContexte.Symbole)
        Next

        With ligne
            .Cells.Add(TsCaCellulesRapport.ObtenirContexte(ConstantesStyle.CELLULE_CONTEXTE_GAUCHE, String.Format("  - Contexte {0}", pContexte.Titre)))
            .Cells.Add(TsCaCellulesRapport.ObtenirContexte(ConstantesStyle.CELLULE_CONTEXTE_DROITE, pContexte.Symbole))

            AjouterSeparateurPale(ligne, 1, 1)

            Dim premier As Boolean = True
            Dim StyleRegulier As String = TsCuConstantesRapport.ConstantesStyle.CELLULE_SOUS_TOTAL
            Dim StyleGauche As String = TsCuConstantesRapport.ConstantesStyle.CELLULE_SOUS_TOTAL_GAUCHE
            Dim StyleBarre As String = TsCuConstantesRapport.ConstantesStyle.CELLULE_SOUS_TOTAL_BARRE
            Dim StyleBarreGauche As String = TsCuConstantesRapport.ConstantesStyle.CELLULE_SOUS_TOTAL_BARRE_GAUCHE

            For Each m In mLstMetiersSelect
                If m.EstPermitContexte(pContexte.Titre) Then
                    .Cells.Add(TsCaCellulesRapport.ObtenirSousTotal(If(premier = True, StyleGauche, StyleRegulier), formule))
                Else
                    .Cells.Add(TsCaCellulesRapport.ObtenirSousTotal(If(premier = True, StyleBarreGauche, StyleBarre), ""))
                End If
                If premier = True Then premier = False
            Next

            AjouterSeparateurPale(ligne, mNbMetiersSelect, mNbTachesSelect)

            For Each ts In mLstTachesSelect
                If ts.EstPermitContexte(pContexte.Titre) Then
                    .Cells.Add(TsCaCellulesRapport.ObtenirSousTotal(If(premier = True, StyleGauche, StyleRegulier), formule))
                Else
                    .Cells.Add(TsCaCellulesRapport.ObtenirSousTotal(If(premier = True, StyleBarreGauche, StyleBarre), ""))
                End If
                If premier = True Then premier = False
            Next

            AjouterSeparateurFonce(ligne, mNbMetiersSelect + mNbTachesSelect, mNbMetiersAutre + mNbTachesAutre)

            For Each ma In mLstMetiersAutre
                If ma.EstPermitContexte(pContexte.Titre) Then
                    .Cells.Add(TsCaCellulesRapport.ObtenirSousTotal(If(premier = True, StyleGauche, StyleRegulier), formule))
                Else
                    .Cells.Add(TsCaCellulesRapport.ObtenirSousTotal(If(premier = True, StyleBarreGauche, StyleBarre), ""))
                End If
                If premier = True Then premier = False
            Next

            AjouterSeparateurPale(ligne, mNbMetiersAutre, mNbTachesAutre)

            For Each ta In mLstTachesAutre
                If ta.EstPermitContexte(pContexte.Titre) Then
                    .Cells.Add(TsCaCellulesRapport.ObtenirSousTotal(If(premier = True, StyleGauche, StyleRegulier), formule))
                Else
                    .Cells.Add(TsCaCellulesRapport.ObtenirSousTotal(If(premier = True, StyleBarreGauche, StyleBarre), ""))
                End If
                If premier = True Then premier = False
            Next

            .Cells.Add(TsCaCellulesRapport.ObtenirVide(CELLULE_VIDE_GAUCHE))
        End With

        Return ligne
    End Function

    ''' <summary>
    ''' Permet d'obtenir la première ligne de la légende.
    ''' </summary>
    ''' <returns>Une ligne.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirLegende1() As TsCuRow
        Dim ligne As New TsCuRow()
        Dim cmptColonne As Integer = 0

        With ligne
            .Cells.Add(TsCaCellulesRapport.ObtenirVide())
            .Cells.Add(TsCaCellulesRapport.ObtenirVide())
            .Cells.Add(TsCaCellulesRapport.ObtenirVide())

            .Cells.Add(TsCaCellulesRapport.ObtenirLegende(ConstantesStyle.CELLULE_LEGENDE_BARREE))
            .Cells.Add(TsCaCellulesRapport.ObtenirLegendeTexte("Non applicable car contexte non valide pour ce rôle"))

        End With

        Return ligne
    End Function

    ''' <summary>
    ''' Permet d'obtenir la seconde ligne de la légende.
    ''' </summary>
    ''' <returns>Une ligne.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirLegende2() As TsCuRow
        Dim ligne As New TsCuRow()
        Dim cmptColonne As Integer = 0

        With ligne
            .Cells.Add(TsCaCellulesRapport.ObtenirVide())
            .Cells.Add(TsCaCellulesRapport.ObtenirVide())
            .Cells.Add(TsCaCellulesRapport.ObtenirVide())

            .Cells.Add(TsCaCellulesRapport.ObtenirLegende(TsCuConstantesRapport.ConstantesStyle.CELLULE_LEGENDE_JAUNE))
            .Cells.Add(TsCaCellulesRapport.ObtenirLegendeTexte("Valeur non comptabilisée car non valide pour ce rôle"))

        End With

        Return ligne
    End Function

    ''' <summary>
    ''' Permet d'obtenir le bas du tableau du rapport.
    ''' </summary>
    ''' <returns>Une ligne.</returns>
    ''' <remarks></remarks>
    Public Function ObtenirFinRapport() As TsCuRow
        Dim ligne As New TsCuRow()

        With ligne
            For i As ULong = 1 To mLargeurTableau
                .Cells.Add(TsCaCellulesRapport.ObtenirVide(CELLULE_VIDE_HAUT))
            Next
        End With

        Return ligne
    End Function

#End Region

#Region "--- Fonctions privées ---"

    ''' <summary>
    ''' Permet de construire la formule qui sera associé à une cellule de Totaux.
    ''' </summary>
    ''' <param name="pNbContexteDisponible">Nombre de contexte disponible.</param>
    ''' <returns>Une formule.</returns>
    ''' <remarks></remarks>
    Private Function ConstruireFormuleTotaux(ByVal pNbContexteDisponible As Integer) As String
        Dim formule As String = "="

        If pNbContexteDisponible = 0 Then
            For i = 1 To mNbEmployes
                If formule <> "=" Then formule &= "+"
                formule &= String.Format("IF(UPPER(R[-{0}]C)=&quot;X&quot;,1,0)", i)
            Next
        Else
            If mNbContextes > 0 Then
                If formule <> "=" Then formule &= "+"
                formule &= String.Format("SUM(R[1]C:R[{0}]C)", mNbContextes)
            End If
        End If

        Return formule
    End Function

    ''' <summary>
    ''' Permet d'enrichir de la partie commune de chacune de type des lignes employé.
    ''' </summary>
    ''' <param name="pLigne">La ligne à enrichir.</param>
    ''' <param name="pEmploye">L'employé de la ligne.</param>
    ''' <remarks></remarks>
    Private Sub EnrichirEmployeCommun(ByVal pLigne As TsCuRow, ByVal pEmploye As TsDtSourceEmploye)
        With pLigne
            .AutoFitHeight = False

            AjouterSeparateurPale(pLigne, 1, 1)

            For Each m In mLstMetiersSelect
                .Cells.Add(TsCaCellulesRapport.ObtenirGrille(pEmploye.ObtenirValeurAssociee(m.Nom)))
                If pEmploye.EstValeurEtrangere(m.Nom) = True Then
                    .Cells.Last.StyleID = TsCuConstantesRapport.ConstantesStyle.CELLULE_GRILLE_MARQUE
                End If
            Next

            AjouterSeparateurPale(pLigne, mNbMetiersSelect, mNbTachesSelect)

            For Each ts In mLstTachesSelect
                .Cells.Add(TsCaCellulesRapport.ObtenirGrille(pEmploye.ObtenirValeurAssociee(ts.Nom)))
                If pEmploye.EstValeurEtrangere(ts.Nom) = True Then
                    .Cells.Last.StyleID = TsCuConstantesRapport.ConstantesStyle.CELLULE_GRILLE_MARQUE
                End If
            Next

            AjouterSeparateurFonce(pLigne, mNbMetiersSelect + mNbTachesSelect, mNbMetiersAutre + mNbTachesAutre)

            For Each ma In mLstMetiersAutre
                .Cells.Add(TsCaCellulesRapport.ObtenirGrille(pEmploye.ObtenirValeurAssociee(ma.Nom)))
            Next

            AjouterSeparateurPale(pLigne, mNbMetiersAutre, mNbTachesAutre)

            For Each ta In mLstTachesAutre
                .Cells.Add(TsCaCellulesRapport.ObtenirGrille(pEmploye.ObtenirValeurAssociee(ta.Nom)))
            Next

            .Cells.Add(TsCaCellulesRapport.ObtenirVide(CELLULE_VIDE_GAUCHE))
        End With
    End Sub

    ''' <summary>
    ''' Fonction de service. Permet d'ajouter à une ligne un séparateur pâle si les sections de gauche et de droite ont au moins un élément chacun.
    ''' </summary>
    ''' <param name="pLigne">La ligne qui recevera le séparateur.</param>
    ''' <param name="pNbSectionGauche">Nombre d'éléments dans la section de gauche.</param>
    ''' <param name="pNbSectionDroite">Nombre d'éléments dans la section de droite.</param>
    ''' <remarks></remarks>
    Private Sub AjouterSeparateurPale(ByVal pLigne As TsCuRow, ByVal pNbSectionGauche As Integer, ByVal pNbSectionDroite As Integer)
        If pNbSectionGauche > 0 And pNbSectionDroite > 0 Then
            pLigne.Cells.Add(TsCaCellulesRapport.ObtenirSeparateurPale())
        End If
    End Sub

    ''' <summary>
    ''' Fonction de service. Permet d'ajouter à une ligne un séparateur foncé si les sections de gauche et de droite ont au moins un élément chacun.
    ''' </summary>
    ''' <param name="pLigne">La ligne qui recevera le séparateur.</param>
    ''' <param name="pNbSectionGauche">Nombre d'éléments dans la section de gauche.</param>
    ''' <param name="pNbSectionDroite">Nombre d'éléments dans la section de droite.</param>
    ''' <remarks></remarks>
    Private Sub AjouterSeparateurFonce(ByVal pLigne As TsCuRow, ByVal pNbSectionGauche As Integer, ByVal pNbSectionDroite As Integer)
        If pNbSectionGauche > 0 And pNbSectionDroite > 0 Then
            pLigne.Cells.Add(TsCaCellulesRapport.ObtenirSeparateurFonce())
        End If
    End Sub

#End Region

End Class
