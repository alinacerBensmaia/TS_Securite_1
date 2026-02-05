Imports System.IO
Imports Rrq.CS.ServicesCommuns.ScenarioTransactionnel
Imports System.Collections.Generic
Imports System.Text
Imports TS6N011_ZgLibOutils
Imports TS1N201_DtCdAccGenV1

Public Class TsFfImportation

#Region "--- Constantes ---"

    Private Const CHAMP_CSV_IMPORT_SYSTEME As Integer = 0
    Private Const CHAMP_CSV_IMPORT_SOUS_SYSTEME As Integer = 1
    Private Const CHAMP_CSV_IMPORT_ENVIRONNEMENT As Integer = 2
    Private Const CHAMP_CSV_IMPORT_CODE_UTIL As Integer = 3
    Private Const CHAMP_CSV_IMPORT_MOT_PASSE As Integer = 4
    Private Const CHAMP_CSV_IMPORT_TYPE_CLE As Integer = 5
    Private Const CHAMP_CSV_IMPORT_CODE_CONX As Integer = 6
    Private Const CHAMP_CSV_IMPORT_CODE_VERIF As Integer = 7
    Private Const CHAMP_CSV_IMPORT_PROFILS As Integer = 8
    Private Const CHAMP_CSV_IMPORT_CLE As Integer = 9
    Private Const CHAMP_CSV_IMPORT_COMMENTAIRE As Integer = 10
    Private Const CHAMP_CSV_IMPORT_DESCRIPTION As Integer = 11
    Private Const CHAMP_CSV_IMPORT_ERREUR As Integer = 12

    Private LISTE_ENV As String() = {"ESSA", "UNIT", "INTG", "ACCP", "FORA", "FORP", "SIML", "PROD"}

#End Region

    Private mRepertoire As String
    Private mNomFichier As String
    Private DtbConnection As DataTable
    Private mWorker As New System.ComponentModel.BackgroundWorker()

    Private listeErreurs As New List(Of String)()
    Private ligneCount As Integer = 0
    Private ligneSucces As Integer = 0
    Private demarre As Boolean = False
    Private mIndCreationAd As Boolean
    Private mIndCreationAdLds As Boolean
    Private mIndCreationCompte As TS1N201_DtCdAccGenV1.TsDtIndCreCpt

    Public Sub New(ByVal repertoire As String, ByVal nomFichier As String, ByVal pDtbConnection As DataTable)

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        mRepertoire = repertoire
        mNomFichier = nomFichier
        DtbConnection = pDtbConnection

        btnAnnuler.Text = "Démarrer"
        lblImportation.Visible = False

        mIndCreationCompte = TsCuPrGerAccGen.ObtenirIndicateursCreationCompte()
        chkAD.Enabled = mIndCreationCompte.InCreCptAdTs

        ' Par défaut aucune case n'est cochée donc si AD n'est pas coché, AD/LDS doit être désactivé
        chkADLDS.Enabled = False

    End Sub

    Private Sub btnAnnuler_Click(sender As System.Object, e As System.EventArgs) Handles btnAnnuler.Click
        If demarre Then
            mWorker.CancelAsync()
        Else
            btnAnnuler.Text = "Annuler"
            lblImportation.Visible = True
            chkAD.Enabled = False
            chkADLDS.Enabled = False
            demarre = True
            ControlBox = False

            mIndCreationAd = chkAD.Checked
            mIndCreationAdLds = chkADLDS.Checked AndAlso chkAD.Checked


            AddHandler mWorker.DoWork, AddressOf background_DoWork
            AddHandler mWorker.RunWorkerCompleted, AddressOf background_RunWorkerCompleted
            mWorker.RunWorkerAsync()
        End If
    End Sub

    Private Sub background_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)

        Using sr As New StreamReader(Path.Combine(mRepertoire, mNomFichier))

            While Not sr.EndOfStream

                If e.Cancel Then
                    Exit While
                End If

                Dim ligne As String = sr.ReadLine()

                If Not String.IsNullOrEmpty(ligne) Then
                    Dim champs() As String = DecouperLigneCSV(ligne)

                    ' Si on est au début du fichier, vérifier qu'on a le bon format, si le format est bon, on assume qu'il est bon pour le reste des lignes
                    ' et les autres lignes qui n'ont pas le bon format seront placées en erreur individuellement. Si le format n'est pas bon sur la première
                    ' ligne, on assume que tout le fichier est invalide
                    If champs.Length < 12 OrElse champs.Length > 13 Then
                        If ligneCount = 0 Then
                            Throw New FormatException("Le format du fichier sélectionné est incorrect")
                        Else
                            ligneCount += 1
                            listeErreurs.Add(RecomposerLigneCSVAvecErreur(champs, "Le format de la ligne est invalide"))
                            Continue While
                        End If
                    End If

                    ' Il faut ignorer les lignes d'en-tête pour le traitement
                    If champs(CHAMP_CSV_IMPORT_SYSTEME) = "SYSTEME" AndAlso _
                        ((champs.Length = 12 AndAlso champs(CHAMP_CSV_IMPORT_DESCRIPTION) = "DESCRIPTION") OrElse _
                         (champs.Length = 13 AndAlso champs(CHAMP_CSV_IMPORT_ERREUR) = "ERREUR")) Then
                        Continue While
                    End If

                    ligneCount += 1

                    ' Effectuer les validations pour la ligne, si une erreur est détectée, journaliser, sinon procéder avec la création
                    Dim erreurValidation As String = ValiderChampsCSV(champs)

                    If Not String.IsNullOrEmpty(erreurValidation) Then
                        listeErreurs.Add(RecomposerLigneCSVAvecErreur(champs, erreurValidation))
                    Else
                        Try
                            Dim impCle As TS1N201_DtCdAccGenV1.TsDtCleSym = ExtraireCleCSV(champs)
                            TsCuPrGerAccGen.SauvegardeCle(impCle, True, False)
                            ligneSucces += 1
                        Catch ex As XZCuErrValdtException
                            listeErreurs.Add(RecomposerLigneCSVAvecErreur(champs, ex.MsgErreur.NumMessage))
                        End Try
                    End If
                End If

            End While

        End Using

    End Sub

    Private Sub background_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)

        If (e.Error IsNot Nothing) Then
            MessageBox.Show(e.Error.Message, "Importation à partir d'un fichier",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else

            If listeErreurs.Count = 0 Then
                MessageBox.Show(String.Format("Importation de {0} clés effectuée avec succès", ligneSucces), "Importation à partir d'un fichier",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else

                Dim nomFichierErreur As String = Path.Combine(mRepertoire, "Erreurs_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv")

                Using sw As New StreamWriter(nomFichierErreur)

                    For Each ligne As String In listeErreurs
                        sw.WriteLine(ligne)
                    Next

                End Using

                MessageBox.Show(String.Format("L'importation des clés a été complétée avec erreurs :" + Environment.NewLine + _
                                              "   - {0} Clés importées" + Environment.NewLine + "   - {1} Clés en erreur" + _
                                              Environment.NewLine + Environment.NewLine + "Les écarts ont été enregistrés dans '{2}'", _
                                              ligneSucces, listeErreurs.Count, nomFichierErreur), _
                                "Importation à partir d'un fichier", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End If

        End If

        Close()

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="pChamps"></param>
    ''' <returns></returns>
    Private Function ExtraireCleCSV(ByVal pChamps() As String) As TS1N201_DtCdAccGenV1.TsDtCleSym

        Dim cleCsv As New TS1N201_DtCdAccGenV1.TsDtCleSym()

        cleCsv.CoIdnCleSymTs = pChamps(CHAMP_CSV_IMPORT_CLE)

        cleCsv.CoEnvCleSymTs = pChamps(CHAMP_CSV_IMPORT_ENVIRONNEMENT)      ' Environnement
        cleCsv.CoSysCleSymTs = pChamps(CHAMP_CSV_IMPORT_SYSTEME)            ' Système
        cleCsv.CoSouCleSymTs = pChamps(CHAMP_CSV_IMPORT_SOUS_SYSTEME)       ' Sous-Système
        cleCsv.CmCleSymTs = pChamps(CHAMP_CSV_IMPORT_COMMENTAIRE)           ' Commentaire
        cleCsv.DsCleSymTs = pChamps(CHAMP_CSV_IMPORT_DESCRIPTION)           ' Description
        cleCsv.VlVerCleSymTs = pChamps(CHAMP_CSV_IMPORT_CODE_VERIF)         ' Code de verification
        cleCsv.CoUtlGenCleTs = pChamps(CHAMP_CSV_IMPORT_CODE_UTIL)          ' Code utilisateur
        cleCsv.VlMotPasCleTs = pChamps(CHAMP_CSV_IMPORT_MOT_PASSE)          ' Mot de passe
        cleCsv.CoTypDepCleTs = pChamps(CHAMP_CSV_IMPORT_CODE_CONX)          ' Type de connexion
        cleCsv.CoTypCleSymTs = pChamps(CHAMP_CSV_IMPORT_TYPE_CLE)           ' Type de clé

        If String.IsNullOrEmpty(cleCsv.VlMotPasCleTs) Then
            Dim generateurMp As New TsCuMotDePasse()
            cleCsv.VlMotPasCleTs = generateurMp.GenererMotDePasse(8, True, True, True, False)
        End If

        ' Créer une liste pour les profils
        cleCsv.LsGroAd = (From item In pChamps(CHAMP_CSV_IMPORT_PROFILS).Split(","c).AsEnumerable()
                          Select New TS1N201_DtCdAccGenV1.TsDtGroAd With {.NmGroActDirTs = TsCuConversionsTypes.GetString(item)}).ToList()

        ' Tronquer les descriptions trop longues
        If cleCsv.CmCleSymTs.Length > 1000 Then
            cleCsv.CmCleSymTs = cleCsv.CmCleSymTs.Substring(0, 1000)
        End If
        If cleCsv.DsCleSymTs.Length > 250 Then
            cleCsv.DsCleSymTs = cleCsv.DsCleSymTs.Substring(0, 250)
        End If
        If cleCsv.VlVerCleSymTs.Length > 20 Then
            cleCsv.VlVerCleSymTs = cleCsv.VlVerCleSymTs.Substring(0, 20)
        End If
        If cleCsv.CoUtlGenCleTs.Length > 50 Then
            cleCsv.CoUtlGenCleTs = cleCsv.CoUtlGenCleTs.Substring(0, 50)
        End If

        cleCsv.StIndCreCpt = New TsDtIndCreCpt()
        cleCsv.StIndCreCpt.InCreCptAdTs = mIndCreationAd
        cleCsv.StIndCreCpt.InCreCptLdsTs = mIndCreationAdLds AndAlso mIndCreationAd

        Return cleCsv

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="pChamps"></param>
    ''' <returns>Description de l'erreur détectée ou chaine vide si il n'y a pas eu d'erreur</returns>
    Private Function ValiderChampsCSV(ByVal pChamps() As String) As String

        Dim erreursChamps As New List(Of String)()

        ' La clé est obligatoire
        If String.IsNullOrWhiteSpace(pChamps(CHAMP_CSV_IMPORT_CLE)) Then
            erreursChamps.Add("Le champ 'Clé' est obligatoire")
        End If

        ' Le champ système est obligatoire et doit comporter deux caractères
        If pChamps(CHAMP_CSV_IMPORT_SYSTEME).Length <> 2 Then
            erreursChamps.Add("Le champ 'Système' est obligatoire et doit contenir exactement deux caractères")
        End If

        ' Le champ sous-système est optionnel et si présent doit être composé d'un seul caractère
        If pChamps(CHAMP_CSV_IMPORT_SOUS_SYSTEME).Length > 1 Then
            erreursChamps.Add("Le champ 'Sous-Système' si présent doit être composé d'un seul caractère")
        End If

        ' Le champ environnement et doit être composé d'un seul caractère représentant un environnement connu
        If Not LISTE_ENV.Contains(pChamps(CHAMP_CSV_IMPORT_ENVIRONNEMENT)) Then
            erreursChamps.Add("Le champ 'Environnement' doit représenter un et un seul environnement connu")
        End If

        ' Le code utilisateur est obligatoire
        If String.IsNullOrEmpty(pChamps(CHAMP_CSV_IMPORT_CODE_UTIL)) Then
            erreursChamps.Add("Le champ 'Code Utilisateur' est obligatoire")
        End If

        ' Le type de connexion est obligatoire et doit correspondre à un code d'un type connu
        If Not ObtenirTypeConnexionExiste(pChamps(CHAMP_CSV_IMPORT_CODE_CONX)) Then
            erreursChamps.Add("Le champ 'Type de connexion' est obligatoire et doit correspondre à un code d'un type connu")
        End If

        ' Le code de type de clé est obligatoire et doit être valide
        If Not TsCuPrGerAccGen.EstTypeCleValide(pChamps(CHAMP_CSV_IMPORT_TYPE_CLE)) Then
            erreursChamps.Add("Le champ 'Code de type de clé' est obligatoire et doit correspondre à un code d'un type connu")
        End If

        ' Le profil est obligatoire et doit représenter un groupe existant de l'AD
        If Not TsCuPrGerAccGen.ValiderGroupes(pChamps(CHAMP_CSV_IMPORT_PROFILS)) Then
            erreursChamps.Add("Le champ 'Profil' est obligatoire et droit représenter un groupe existant dans le dépôt de sécurité")
        End If

        If erreursChamps.Count = 0 Then
            Return String.Empty
        Else
            ' Concaténer les erreurs au cas où il y en aurait plusieurs
            Dim erreurBuilder As New StringBuilder()
            For Each erreur As String In erreursChamps
                If erreurBuilder.Length > 0 Then
                    erreurBuilder.Append("--")
                End If
                erreurBuilder.Append(erreur)
            Next
            Return erreurBuilder.ToString()
        End If

    End Function

    Private Function ObtenirTypeConnexionExiste(ByVal pType As String) As Boolean
        Dim row As DataRow = Nothing
        Dim retour As Boolean = False

        For Each row In DtbConnection.Rows
            If row("Value").ToString() = pType Then
                retour = True
                Exit For
            End If
        Next

        Return retour
    End Function

    ''' <summary>
    ''' Permet d'extraire les champs d'une ligne CSV
    ''' </summary>
    ''' <param name="ligne">Ligne du CSV à traiter</param>
    ''' <returns>Champs extraits de la ligne</returns>
    Private Function DecouperLigneCSV(ByVal ligne As String) As String()

        Dim champs As New List(Of String)()

        Dim debutChamp As Integer = 0
        Dim finChamp As Integer = -1
        Dim premierChar As Boolean = True
        Dim estChaine As Boolean = False
        Dim champComplet As Boolean = False

        For i As Integer = 0 To ligne.Length - 1

            If premierChar AndAlso ligne(i) = """"c Then
                ' Si un champ débute par un ", c'est que la chaine est entre double quote
                debutChamp = i + 1
                estChaine = True
            ElseIf ligne(i) = """"c AndAlso ligne.Length > (i + 1) AndAlso ligne(i + 1) = """"c Then
                ' Gérer le double quote dans une chaine ""
                i += 1
                finChamp = i + 1
            ElseIf estChaine Then
                ' Si on est dans une chaine, vérifier qu'on a pas atteint la fin
                If ligne(i) = """"c AndAlso (ligne.Length <= (i + 1) OrElse ligne(i + 1) = ";"c) Then
                    estChaine = False
                    finChamp = i - 1
                    i += 1
                    champComplet = True
                Else
                    finChamp = i
                End If
            ElseIf ligne(i) = ";"c Then
                ' Un ; qui n'est pas dans une chaine représente la fin d'un champ
                finChamp = i - 1
                champComplet = True
            Else
                finChamp = i
            End If

            If champComplet OrElse i = (ligne.Length - 1) Then
                If debutChamp <= finChamp Then
                    ' Remplacer les "" par des " avant d'inscrire la valeur du champ
                    champs.Add(ligne.Substring(debutChamp, finChamp - debutChamp + 1).Trim().Replace("""""", """"))
                Else
                    ' Si le début est après la fin, c'est que c'est un champ vide
                    champs.Add(String.Empty)
                End If
                debutChamp = i + 1
                finChamp = i
                champComplet = False
                premierChar = True
                estChaine = False
            Else
                premierChar = False
            End If

        Next

        Return champs.ToArray()

    End Function

    ''' <summary>
    ''' Permet de créer une chaine valide qui pourra être inscrite dans un CSV
    ''' </summary>
    ''' <param name="pChamps">Valeurs de champs individuels</param>
    ''' <param name="pErreur">Description de l'erreur à ajouter (peut être une chaine vide)</param>
    ''' <returns>Chaine représentant une ligne à inscrire dans le CSV</returns>
    Private Function RecomposerLigneCSVAvecErreur(ByVal pChamps() As String, ByVal pErreur As String) As String

        Dim retour As New StringBuilder()
        Dim compteur As Integer = 0

        For Each champ As String In pChamps
            compteur += 1

            ' Faire en sorte que si une erreur était présente, qu'elle soit ignorée pour éviter les doubles
            If compteur = 13 AndAlso pChamps.Length = 13 Then
                Continue For
            End If

            Dim quotes As Boolean = False
            Dim champMod As String = champ
            If champ.Contains("""") OrElse champ.Contains(";") Then
                ' Encapsuler le champ entre des double quotes seulement si il contient un ; ou un "
                quotes = True
                ' Remplacer les " par ""
                champMod = champ.Replace("""", """""")
            End If

            If quotes Then
                retour.Append("""")
            End If

            retour.Append(champMod)

            If quotes Then
                retour.Append("""")
            End If

            retour.Append(";")
        Next

        retour.Append(pErreur)

        Return retour.ToString()

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub chkAD_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkAD.CheckedChanged

        ' Si AD n'est pas activé, on ne devrait pas pouvoir créer de compte AD/LDS
        If chkAD.Checked Then
            chkADLDS.Enabled = mIndCreationCompte.InCreCptLdsTs AndAlso mIndCreationCompte.InCreCptAdTs
        Else
            chkADLDS.Checked = False
            chkADLDS.Enabled = False
        End If

    End Sub

End Class