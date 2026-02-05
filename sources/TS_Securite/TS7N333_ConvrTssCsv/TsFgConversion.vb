Imports Rrq.Securite.GestionAcces
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions

Public Class TsFgConversion

    Private Const RX_CMD_TSS As String = "^TSS (ADDTO|REMOVE)\(([^)]*)\) PROFILE\(([^)]*)\)"
    Private Const IDX_CMD_TSS As Integer = 1
    Private Const IDX_CMD_TSS_UID As Integer = 2
    Private Const IDX_CMD_TSS_GID As Integer = 3
    Private Const BIDON As String = "C:\Temp\Bidon.log"

    Private Sub cmdConvertir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdConvertir.Click
        Dim cheminExtractionTSS As String
        Using dlgOpenFile As New OpenFileDialog()

            dlgOpenFile.Filter = "Fichier Texte (*.Txt)|*.txt|Tous les fichier (*.*)|*.*"
            dlgOpenFile.Title = "Ouvrir le fichier d'extraction TSS "
            dlgOpenFile.Multiselect = False
            If dlgOpenFile.ShowDialog() <> Windows.Forms.DialogResult.OK Then
                Return
            End If
            cheminExtractionTSS = dlgOpenFile.FileName
        End Using

        Dim cheminAccountsCSV As String
        Using dlgSaveFile As New SaveFileDialog()
            dlgSaveFile.Filter = "Fichier CSV (*.csv)|*.csv|Tous les fichier (*.*)|*.*"
            dlgSaveFile.Title = "Choisir le fichier «Accounts»"
            If dlgSaveFile.ShowDialog() <> Windows.Forms.DialogResult.OK Then
                Return
            End If
            cheminAccountsCSV = dlgSaveFile.FileName
        End Using

        Dim cheminGroupsCSV As String
        Using dlgSaveFile As New SaveFileDialog()
            dlgSaveFile.Filter = "Fichier CSV (*.csv)|*.csv|Tous les fichier (*.*)|*.*"
            dlgSaveFile.Title = "Choisir le fichier «Groups»"
            If dlgSaveFile.ShowDialog() <> Windows.Forms.DialogResult.OK Then
                Return
            End If
            cheminGroupsCSV = dlgSaveFile.FileName
        End Using

        Dim accesTSS As New TsBaAccesTSS(cheminExtractionTSS, BIDON)
        Using accesCSV As New TsCdAccountGroupCSV(cheminAccountsCSV, cheminGroupsCSV, True)

            For Each uid In accesTSS.ObtenirUtilisateurs()
                accesCSV.AjouterUtilisateur(uid)
                For Each gid In accesTSS.ObtenirGroupes(uid)
                    accesCSV.AjouterGroupe(gid)
                    If Not accesCSV.AjouterGroupeUtilisateur(gid, uid) Then
                        MsgBox(String.Format("Erreur lors de l'ajout {0}-{1}", gid, uid))
                        Return
                    End If
                Next
            Next

            'Necessaire pour forcer la fermeture du fichier BIDON
            accesTSS.Dispose()

        End Using
    End Sub


    Private Sub cmdAppliquer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAppliquer.Click
        Dim cheminCommandeTSS As String
        Using dlgOpenFile As New OpenFileDialog()

            dlgOpenFile.Filter = "Fichier Texte (*.Txt)|*.txt|Tous les fichier (*.*)|*.*"
            dlgOpenFile.Title = "Ouvrir le fichier de commandes TSS "
            dlgOpenFile.Multiselect = False
            If dlgOpenFile.ShowDialog() <> Windows.Forms.DialogResult.OK Then
                Return
            End If
            cheminCommandeTSS = dlgOpenFile.FileName
        End Using

        Dim cheminAccountsCSV As String
        Using dlgOpenFile As New OpenFileDialog()

            dlgOpenFile.Filter = "Fichier CSV (*.csv)|*.csv|Tous les fichier (*.*)|*.*"
            dlgOpenFile.Title = "Choisir le fichier «Accounts»"
            dlgOpenFile.Multiselect = False
            If dlgOpenFile.ShowDialog() <> Windows.Forms.DialogResult.OK Then
                Return
            End If
            cheminAccountsCSV = dlgOpenFile.FileName
        End Using

        Dim cheminGroupsCSV As String
        Using dlgOpenFile As New OpenFileDialog()

            dlgOpenFile.Filter = "Fichier CSV (*.csv)|*.csv|Tous les fichier (*.*)|*.*"
            dlgOpenFile.Title = "Choisir le fichier «Groups»"
            dlgOpenFile.Multiselect = False
            If dlgOpenFile.ShowDialog() <> Windows.Forms.DialogResult.OK Then
                Return
            End If
            cheminGroupsCSV = dlgOpenFile.FileName
        End Using

        Using accesCSV As New TsCdAccountGroupCSV(cheminAccountsCSV, cheminGroupsCSV), _
                reader As New StreamReader(cheminCommandeTSS, Encoding.ASCII)

            Dim rxTss As New Regex(RX_CMD_TSS)

            Dim ligne = reader.ReadLine()
            Do While ligne IsNot Nothing
                Dim m = rxTss.Match(ligne)

                If m.Success Then
                    Dim gid = m.Groups(IDX_CMD_TSS_GID).Value
                    Dim uid = m.Groups(IDX_CMD_TSS_UID).Value
                    If m.Groups(IDX_CMD_TSS).Value = "ADDTO" Then
                        If Not accesCSV.AjouterGroupeUtilisateur(gid, uid) Then
                            If MsgBox("Échec de l'ajout du groupe " + gid + " pour l'utilisateur " + uid, MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then
                                Return
                            End If
                        End If
                    ElseIf m.Groups(IDX_CMD_TSS).Value = "REMOVE" Then
                        If Not accesCSV.EnleverGroupeUtilisateur(gid, uid) Then
                            If MsgBox("Échec du retrait du groupe " + gid + " pour l'utilisateur " + uid, MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then
                                Return
                            End If
                        End If
                    Else
                        MsgBox("RegEx et code incohérent")
                        Return
                    End If
                Else
                    If MsgBox("La ligne de commande ne match pas notre expression régulière: " + vbCrLf + ligne, MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then
                        Return
                    End If
                End If
                ligne = reader.ReadLine()
            Loop

        End Using

    End Sub
End Class
