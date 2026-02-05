Imports TS7I132_CiGererRapports.TsCuConstantesRapport

''' <summary>
''' Cette classe contient les lignes du rapport.
''' </summary>
''' <remarks></remarks>
Friend Class TsCaLignesRapport
    'TODO FAIRE UN SOURCE EN PROPIÉTÉ

#Region "--- Variables ---"

    Private mNbMetiersSelect As Integer
    Private mNbTachesSelect As Integer
    Private mNbMetiersAutre As Integer
    Private mNbTachesAutre As Integer

    Private mLstMetiersSelect As List(Of TsDtSourceUa)
    Private mLstTachesSelect As List(Of TsDtSourceUa)
    Private mLstMetiersAutre As List(Of TsDtSourceUa)
    Private mLstTachesAutre As List(Of TsDtSourceUa)

    Private mLargeurTableau As ULong
    Private mLargeurUaSelect As ULong
    Private mLargeurUaAutre As ULong

    Private mNbEmployes As Integer
    Private mNbContextes As Integer

    Private mDate As Date

#End Region

#Region "--- Constructeurs ---"

    Public Sub New(ByVal pSource As TsDtSourceRapport)
        mLstMetiersSelect = pSource.ObtenirRolesMetiersSelectionnes
        mLstTachesSelect = pSource.ObtenirRolesTachesSelectionnes
        mLstMetiersAutre = pSource.ObtenirRolesMetiersAutre
        mLstTachesAutre = pSource.ObtenirRolesTachesAutre

        mNbMetiersSelect = mLstMetiersSelect.Count
        mNbTachesSelect = mLstTachesSelect.Count
        mNbMetiersAutre = mLstMetiersAutre.Count
        mNbTachesAutre = mLstTachesAutre.Count

        mLargeurTableau = ObtenirLargeurTableau()
        mLargeurUaSelect = ObtenirLargeurSectionUaSelect()
        mLargeurUaAutre = ObtenirLargeurSectionUaAutre()

        mNbEmployes = pSource.Employes.Count
        mNbContextes = pSource.Contextes.Count

        mDate = pSource.Date
    End Sub

#End Region

#Region "--- Méthodes ---"

    ''' <summary>
    ''' Permet d'obtenir une ligne d'espacement.
    ''' </summary>
    Public Function Espacement() As TsCuRow
        Return New TsCuRow()
    End Function

    ''' <summary>
    ''' Permet d'obtenir une ligne d'espacement.
    ''' </summary>
    Public Function EnteteRapport(ByVal pLstNoUA As List(Of String)) As TsCuRow
        Dim msgEntete As String
        If pLstNoUA.Count = 1 Then
            msgEntete = "Assignation des rôles aux employés de l'unité "
        Else
            msgEntete = "Assignation des rôles aux employés des unités "
        End If
        For i = 0 To pLstNoUA.Count - 1
            If i = pLstNoUA.Count - 1 Then
                msgEntete &= String.Format("<Font><B>{0}</B></Font> ", pLstNoUA(i))
            ElseIf i = pLstNoUA.Count - 2 Then
                msgEntete &= String.Format("<Font><B>{0}</B></Font> et ", pLstNoUA(i))
            Else
                msgEntete &= String.Format("<Font><B>{0}</B></Font>, ", pLstNoUA(i))
            End If
        Next
        msgEntete &= "au " & mDate.ToString("yyyy-MM-dd")

        Dim ligne As New TsCuRow()
        With ligne
            .Cells.Add(TsCaCellulesRapport.ObtenirEntete(mLargeurTableau, msgEntete))
        End With
        Return ligne
    End Function

    ''' <summary>
    ''' Permet d'obtenir une ligne d'espacement.
    ''' </summary>
    Public Function EnteteTableau() As TsCuRow
        Dim ligne As New TsCuRow()

        With ligne
            .Cells.Add(TsCaCellulesRapport.ObtenirVide)
            .Cells.Add(TsCaCellulesRapport.ObtenirVide)
            .Cells.Add(TsCaCellulesRapport.ObtenirVideDroite)

            Dim style As String = TsCuConstantesRapport.ConstantesStyle.CELLULE_ENTETE_TABLEAU

            If mLargeurUaSelect > 0 Then
                .Cells.Add(TsCaCellulesRapport.ObtenirEnteteTableau(style, mLargeurUaSelect, "UA sélectionnées", "UAs"))
            End If

            If mLargeurUaSelect > 0 And mLargeurUaAutre > 0 Then
                .Cells.Add(TsCaCellulesRapport.ObtenirEnteteTableau(style, 1, "", ""))
            End If

            If mLargeurUaAutre > 0 Then
                .Cells.Add(TsCaCellulesRapport.ObtenirEnteteTableau(style, mLargeurUaAutre, "Autre UA", "Aut"))
            End If

            .Cells.Add(TsCaCellulesRapport.ObtenirVideGauche())
        End With

        Return ligne
    End Function

    Public Function SousEnteteTableau() As TsCuRow
        Dim ligne As New TsCuRow()

        With ligne
            .Cells.Add(TsCaCellulesRapport.ObtenirVide)
            .Cells.Add(TsCaCellulesRapport.ObtenirVide)
            .Cells.Add(TsCaCellulesRapport.ObtenirVide)

            Dim premier As Boolean = True
            Dim style As String = TsCuConstantesRapport.ConstantesStyle.CELLULE_SOUS_ENTETE_TABLEAU_GAUCHE
            If mNbMetiersSelect > 0 Then
                .Cells.Add(TsCaCellulesRapport.ObtenirSousEntete(style, CULng(mNbMetiersSelect), "Métier", "Mér"))
                If premier = True Then
                    premier = False
                    style = TsCuConstantesRapport.ConstantesStyle.CELLULE_SOUS_ENTETE_TABLEAU
                End If
            End If

            AjouterSeparateurPale(ligne, mNbMetiersSelect, mNbTachesSelect)

            If mNbTachesSelect > 0 Then
                .Cells.Add(TsCaCellulesRapport.ObtenirSousEntete(style, CULng(mNbTachesSelect), "Tâche", "Tâe"))
                If premier = True Then
                    premier = False
                    style = TsCuConstantesRapport.ConstantesStyle.CELLULE_SOUS_ENTETE_TABLEAU
                End If
            End If

            AjouterSeparateurFonce(ligne, mNbMetiersSelect + mNbTachesSelect, mNbMetiersAutre + mNbTachesAutre)

            If mNbMetiersAutre > 0 Then
                .Cells.Add(TsCaCellulesRapport.ObtenirSousEntete(style, CULng(mNbMetiersAutre), "Métier", "Mér"))
                If premier = True Then
                    premier = False
                    style = TsCuConstantesRapport.ConstantesStyle.CELLULE_SOUS_ENTETE_TABLEAU
                End If
            End If

            AjouterSeparateurPale(ligne, mNbMetiersAutre, mNbTachesAutre)

            If mNbTachesAutre > 0 Then
                .Cells.Add(TsCaCellulesRapport.ObtenirSousEntete(style, CULng(mNbTachesAutre), "Tâche", "Tâe"))
                If premier = True Then
                    premier = False
                    style = TsCuConstantesRapport.ConstantesStyle.CELLULE_SOUS_ENTETE_TABLEAU
                End If
            End If

            .Cells.Add(TsCaCellulesRapport.ObtenirVideGauche())

        End With

        Return ligne
    End Function

    Public Function UA() As TsCuRow
        Dim ligne As New TsCuRow()

        With ligne
            .Height = 200
            .Cells.Add(TsCaCellulesRapport.ObtenirVide)
            .Cells.Add(TsCaCellulesRapport.ObtenirVide)
            .Cells.Add(TsCaCellulesRapport.ObtenirVideDroite)

            '--- Section Métiers selectionnées
            Dim style As String = TsCuConstantesRapport.ConstantesStyle.CELLULE_UA
            For Each ms In mLstMetiersSelect
                .Cells.Add(TsCaCellulesRapport.ObtenirUA(style, ms.Nom))
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

            .Cells.Add(TsCaCellulesRapport.ObtenirVideGauche())
        End With

        Return ligne
    End Function

    Public Function EnteteCoupee() As TsCuRow
        Dim ligne As New TsCuRow()

        With ligne
            .Cells.Add(TsCaCellulesRapport.ObtenirVide())
            .Cells.Add(TsCaCellulesRapport.ObtenirEnteteCouper("Nom employé"))
            .Cells.Add(TsCaCellulesRapport.ObtenirEnteteCouper("UA"))

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

            .Cells.Add(TsCaCellulesRapport.ObtenirVideGauche())
        End With

        Return ligne
    End Function

    Public Function EmployeHaut(ByVal pEmploye As TsDtSourceEmploye) As TsCuRow
        Dim ligne As New TsCuRow()

        With ligne
            .Cells.Add(TsCaCellulesRapport.ObtenirVide())
            .Cells.Add(TsCaCellulesRapport.ObtenirEmployeHaut(pEmploye.Nom))
            .Cells.Add(TsCaCellulesRapport.ObtenirNoUAHaut(pEmploye.NoUA.ToString))
        End With

        EmployeCommun(ligne, pEmploye)

        Return ligne
    End Function

    Public Function Employe(ByVal pEmploye As TsDtSourceEmploye) As TsCuRow
        Dim ligne As New TsCuRow()

        With ligne
            .Cells.Add(TsCaCellulesRapport.ObtenirVide())
            .Cells.Add(TsCaCellulesRapport.ObtenirEmploye(pEmploye.Nom))
            .Cells.Add(TsCaCellulesRapport.ObtenirNoUA(pEmploye.NoUA.ToString))
        End With

        EmployeCommun(ligne, pEmploye)

        Return ligne
    End Function

    Public Function EmployeBas(ByVal pEmploye As TsDtSourceEmploye) As TsCuRow
        Dim ligne As New TsCuRow()

        With ligne
            .Cells.Add(TsCaCellulesRapport.ObtenirVide())
            .Cells.Add(TsCaCellulesRapport.ObtenirEmployeBas(pEmploye.Nom))
            .Cells.Add(TsCaCellulesRapport.ObtenirNoUABas(pEmploye.NoUA.ToString))
        End With

        EmployeCommun(ligne, pEmploye)

        Return ligne
    End Function

    'TODO DÉPLACER
    Private Function ConstruireFormuleTotaux(ByVal pNbContexteDisponible As Integer) As String
        Dim formule As String = "="

        If pNbContexteDisponible = 0 Then
            For i = 1 To mNbEmployes
                If formule <> "=" Then formule &= "+"
                formule &= String.Format("+IF(UPPER(R[-{0}]C)=&quot;X&quot;,1,0)", i)
            Next
        Else
            If mNbContextes > 0 Then
                If formule <> "=" Then formule &= "+"
                formule &= String.Format("SUM(R[1]C:R[{0}]C)", mNbContextes)
            End If
        End If

        Return formule
    End Function


    Public Function Totaux() As TsCuRow
        Dim ligne As New TsCuRow()

        

        With ligne
            .Cells.Add(TsCaCellulesRapport.ObtenirVide())
            .Cells.Add(TsCaCellulesRapport.ObtenirSousEnteteCoupee())

            Dim premier As Boolean = True 'TODO Trouver les PREMIER pour faire le teste à toutes les boucle.
            Dim style As String = TsCuConstantesRapport.ConstantesStyle.CELLULE_TOTAL_GAUCHE
            For Each ms In mLstMetiersSelect
                If premier = True Then
                    premier = False
                    style = TsCuConstantesRapport.ConstantesStyle.CELLULE_TOTAL
                End If

                Dim formule = ConstruireFormuleTotaux(ms.ContextesPermis.Count)

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

            .Cells.Add(TsCaCellulesRapport.ObtenirVideGauche())
        End With

        Return ligne
    End Function

    Public Function Contexte(ByVal pContexte As TsDtSourceContexteUA, ByVal pPosition As Integer) As TsCuRow
        Dim ligne As New TsCuRow()

        Dim formule As String = "= "

        For i = 1 To mNbEmployes
            formule &= String.Format("+IF(UPPER(R[-{0}]C)=UPPER(&quot;{1}&quot;),1,0)", i + pPosition, pContexte.Symbole)
        Next

        With ligne
            .Cells.Add(TsCaCellulesRapport.ObtenirVide())
            .Cells.Add(TsCaCellulesRapport.ObtenirContexteGauche(String.Format("  - Contexte {0}", pContexte.Titre)))
            .Cells.Add(TsCaCellulesRapport.ObtenirContexteDroite(pContexte.Symbole))

            Dim premier As Boolean = True
            Dim StyleRegulier As String = TsCuConstantesRapport.ConstantesStyle.CELLULE_SOUS_TOTAL
            Dim StyleGauche As String = TsCuConstantesRapport.ConstantesStyle.CELLULE_SOUS_TOTAL_GAUCHE
            Dim StyleBarre As String = TsCuConstantesRapport.ConstantesStyle.CELLULE_SOUS_TOTAL_BARRE
            Dim StyleBarreGauche As String = TsCuConstantesRapport.ConstantesStyle.CELLULE_SOUS_TOTAL_BARRE_GAUCHE

            For Each ms In mLstMetiersSelect
                If ms.EstPermitContexte(pContexte.Titre) Then
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

            .Cells.Add(TsCaCellulesRapport.ObtenirVideGauche())
        End With

        Return ligne
    End Function

    Public Function Legende1() As TsCuRow
        Dim ligne As New TsCuRow()

        With ligne
            .Cells.Add(TsCaCellulesRapport.ObtenirVide())
            .Cells.Add(TsCaCellulesRapport.ObtenirVide())
            .Cells.Add(TsCaCellulesRapport.ObtenirVide())

            .Cells.Add(TsCaCellulesRapport.ObtenirLegendeBarre())
            .Cells.Add(TsCaCellulesRapport.ObtenirLegendeTexte("Non applicable car contexte non valide pour ce rôle"))

        End With
        Return ligne
    End Function

    Public Function Legende2() As TsCuRow
        Dim ligne As New TsCuRow()

        With ligne
            .Cells.Add(TsCaCellulesRapport.ObtenirVide())
            .Cells.Add(TsCaCellulesRapport.ObtenirVide())
            .Cells.Add(TsCaCellulesRapport.ObtenirVide())

            .Cells.Add(TsCaCellulesRapport.ObtenirLegendeJaune())
            .Cells.Add(TsCaCellulesRapport.ObtenirLegendeTexte("Valeur non comptabilisée car non valide pour ce rôle"))

        End With

        Return ligne
    End Function

    Public Function FinRapport() As TsCuRow
        Dim ligne As New TsCuRow()

        With ligne
            .Cells.Add(TsCaCellulesRapport.ObtenirVide())
            For i As ULong = 1 To mLargeurTableau
                .Cells.Add(TsCaCellulesRapport.ObtenirVideHaut())
            Next
        End With

        Return ligne
    End Function

#End Region

#Region "--- Fonctions privées ---"

    Private Sub EmployeCommun(ByVal pLigne As TsCuRow, ByVal pEmploye As TsDtSourceEmploye)
        With pLigne
            .AutoFitHeight = False

            For Each ms In mLstMetiersSelect
                .Cells.Add(TsCaCellulesRapport.ObtenirGrille(pEmploye.ObtenirValeurAssociee(ms.Nom)))
            Next

            AjouterSeparateurPale(pLigne, mNbMetiersSelect, mNbTachesSelect)

            For Each ts In mLstTachesSelect
                .Cells.Add(TsCaCellulesRapport.ObtenirGrille(pEmploye.ObtenirValeurAssociee(ts.Nom)))
            Next

            AjouterSeparateurFonce(pLigne, mNbMetiersSelect + mNbTachesSelect, mNbMetiersAutre + mNbTachesAutre)

            For Each ma In mLstMetiersAutre
                .Cells.Add(TsCaCellulesRapport.ObtenirGrille(pEmploye.ObtenirValeurAssociee(ma.Nom)))
            Next

            AjouterSeparateurPale(pLigne, mNbMetiersAutre, mNbTachesAutre)

            For Each ta In mLstTachesAutre
                .Cells.Add(TsCaCellulesRapport.ObtenirGrille(pEmploye.ObtenirValeurAssociee(ta.Nom)))
            Next

            .Cells.Add(TsCaCellulesRapport.ObtenirVideGauche())
        End With
    End Sub

    'TODO METTRE DANS LA BOITE À OUTILS
    Private Function ObtenirLargeurTableau() As ULong
        Dim largeurEnteteCoupee = 2
        Dim sectionSelect = ObtenirLargeurSectionUaSelect()
        Dim sectionautre = ObtenirLargeurSectionUaAutre()


        Dim resultat = largeurEnteteCoupee + sectionautre + sectionSelect

        If sectionautre > 0 And sectionSelect > 0 Then ' Colonne du séparateur
            resultat += 1
        End If

        Return CULng(resultat)
    End Function

    Private Function ObtenirLargeurSectionUaSelect() As ULong

        Dim resultat = mNbMetiersSelect + mNbTachesSelect

        If mNbMetiersSelect > 0 And mNbTachesSelect > 0 Then ' Colonne du séparateur
            resultat += 1
        End If

        Return CULng(resultat)
    End Function

    Private Function ObtenirLargeurSectionUaAutre() As ULong
        Dim resultat = mNbMetiersAutre + mNbTachesAutre

        If mNbMetiersAutre > 0 And mNbTachesAutre > 0 Then ' Colonne du séparateur
            resultat += 1
        End If

        Return CULng(resultat)
    End Function

    Private Sub AjouterSeparateurPale(ByVal pLigne As TsCuRow, ByVal pLargeurSectionGauche As Integer, ByVal pLargeurSectionDroite As Integer)
        If pLargeurSectionGauche > 0 And pLargeurSectionDroite > 0 Then
            pLigne.Cells.Add(TsCaCellulesRapport.ObtenirSeparateurPale())
        End If
    End Sub

    Private Sub AjouterSeparateurFonce(ByVal pLigne As TsCuRow, ByVal pLargeurSectionGauche As Integer, ByVal pLargeurSectionDroite As Integer)
        If pLargeurSectionGauche > 0 And pLargeurSectionDroite > 0 Then
            pLigne.Cells.Add(TsCaCellulesRapport.ObtenirSeparateurFonce())
        End If
    End Sub

#End Region


End Class
