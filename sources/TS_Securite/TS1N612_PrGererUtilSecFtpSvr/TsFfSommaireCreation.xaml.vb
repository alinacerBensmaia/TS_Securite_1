Imports System.Collections.Generic
Imports System.Text
Imports System.ComponentModel
Imports TS1N621_INiveauSecrt1
Imports Rrq.InfrastructureCommune.ScenarioTransactionnel

Partial Public Class TsFfSommaireCreation

    Private m_ListeSommaire As List(Of TsDtInfoCleSymbolique)
    Private m_CreationEnCours As Boolean = False
    Private m_Worker As BackgroundWorker

    Public Sub New(ByVal listeSommaire As List(Of TsDtInfoCleSymbolique))

        InitializeComponent()

        m_ListeSommaire = listeSommaire
        lstSommaire.ItemsSource = m_ListeSommaire

    End Sub

    Private Sub btnOk_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnOk.Click

        Close()

    End Sub

    Private Sub btnCopierTout_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnCopierTout.Click

        Dim objSommaireTexte As New StringBuilder()

        For Each cle As TsDtInfoCleSymbolique In m_ListeSommaire
            objSommaireTexte.AppendLine(cle.CoEnv)
            objSommaireTexte.Append("Nom de la clé : ")
            objSommaireTexte.AppendLine(cle.NmUtl)
            objSommaireTexte.Append("Compte : ")
            objSommaireTexte.AppendLine(cle.NmCom)
            objSommaireTexte.Append("Mot de passe : ")
            objSommaireTexte.AppendLine(cle.VlMotPasCle)
            objSommaireTexte.Append("Description : ")
            objSommaireTexte.AppendLine(cle.DsCle)
            objSommaireTexte.Append("Profil : ")
            objSommaireTexte.AppendLine(cle.NmProUtl)
            objSommaireTexte.AppendLine()
        Next

        Clipboard.SetText(objSommaireTexte.ToString())

    End Sub

    Private Sub lstSommaire_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs)
        lstSommaire.SelectedItem = Nothing
    End Sub

    Private Sub btnCreerCle_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnCreerCle.Click

        lblStatut.Foreground = System.Windows.Media.Brushes.Black
        lblStatut.Content = "Création des clés en cours : 0 / " + m_ListeSommaire.Count.ToString()
        btnCreerCle.IsEnabled = False
        btnOk.IsEnabled = False

        m_Worker = New BackgroundWorker()
        m_Worker.WorkerReportsProgress = True
        AddHandler m_Worker.DoWork, AddressOf backgroundCreation_DoWork
        AddHandler m_Worker.ProgressChanged, AddressOf backgroundCreation_ProgressChanged
        AddHandler m_Worker.RunWorkerCompleted, AddressOf backgroundCreation_RunWorkerCompleted
        m_Worker.RunWorkerAsync()

    End Sub

    Private Sub backgroundCreation_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs)

        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)

        Dim iCount As Integer = 0

        For Each cle As TsDtInfoCleSymbolique In m_ListeSommaire

            iCount += 1

            If cle.InCreCle = False Then

                cle.InCreCle = CreerCleSymbolique(cle)

            End If

            worker.ReportProgress(iCount)

        Next

        worker.ReportProgress(Integer.MinValue)

        ExporterClesSymboliques()

    End Sub

    Private Sub backgroundCreation_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs)

        If e.ProgressPercentage = Integer.MinValue Then
            lblStatut.Content = "Exportation des clés en cours"
        Else
            lblStatut.Content = "Création des clés en cours : " + e.ProgressPercentage.ToString() + " / " + m_ListeSommaire.Count.ToString()
        End If

    End Sub

    Private Sub backgroundCreation_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs)

        m_CreationEnCours = False

        If (e.Error IsNot Nothing) Then
            lblStatut.Foreground = System.Windows.Media.Brushes.Red
            lblStatut.Content = "Erreur lors de la création/exportation des clés"

            Dim sErreur As String
            Dim eErreurNiveau As Exception = e.Error

            While e.Error.InnerException IsNot Nothing
                eErreurNiveau = e.Error.InnerException
            End While

            sErreur = e.Error.ToString()

            MessageBox.Show("Une erreur est survenue lors de la création des clés :" + Environment.NewLine + Environment.NewLine + sErreur,
                            "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            lblStatut.Foreground = System.Windows.Media.Brushes.Green
            lblStatut.Content = "Création des clés effectuée avec succès"
        End If

        btnCreerCle.IsEnabled = True
        btnOk.IsEnabled = True

    End Sub

    Private Sub Window_Closing(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        e.Cancel = m_CreationEnCours

    End Sub

    Private Function CreerCleSymbolique(ByVal cle As TsDtInfoCleSymbolique) As Boolean

        Dim nouvCle As TS1N201_DtCdAccGenV1.TsDtCleSym = New TS1N201_DtCdAccGenV1.TsDtCleSym

        nouvCle.CoEnvCleSymTs = TsFdGererUtilSecFtpSvr.ObtenirCodeEnv(cle.CoEnv.Substring(0, 1))
        nouvCle.CoSysCleSymTs = cle.NmUtl.Substring(0, 2)
        nouvCle.CoSouCleSymTs = cle.NmUtl.Substring(2, 1)
        nouvCle.DsCleSymTs = cle.DsCle
        nouvCle.CoIdnCleSymTs = cle.NmUtl
        nouvCle.CoUtlGenCleTs = cle.NmCom
        nouvCle.VlMotPasCleTs = cle.VlMotPasCle
        nouvCle.CoTypCleSymTs = "DOM"
        nouvCle.CoTypDepCleTs = "AUT"
        nouvCle.LsGroAd = (From item In cle.NmProUtl.Split(","c).AsEnumerable
                            Select New TS1N201_DtCdAccGenV1.TsDtGroAd With {.NmGroActDirTs = GetString(item)}).ToList()

        Dim CaAffaire As TS1N215_INiveauSecrt2.TsICompI
        Dim resultat As Boolean = True
        Dim contexte As Object = Nothing

        Using objAppel As New XuCuAppelerCompI(Of TS1N215_INiveauSecrt2.TsICompI)
            Dim chaineContexte As String = objAppel.PreparerAppel(contexte, TsFdGererUtilSecFtpSvr.ObtenirCodeEnv())

            CaAffaire = objAppel.CreerComposantIntegration(chaineContexte)
            resultat = CaAffaire.EnregistrerCle(chaineContexte, nouvCle, True, False)
            objAppel.AnalyserRetour(chaineContexte, Nothing)
        End Using

        Return resultat

    End Function

    Private Sub ExporterClesSymboliques()

        Dim CaAffaire As TS1N215_INiveauSecrt2.TsICompI
        Dim chaineContexte As String = String.Empty
        Dim contexte As Object = Nothing

        Using objAppel As New XuCuAppelerCompI(Of TS1N215_INiveauSecrt2.TsICompI)
            chaineContexte = objAppel.PreparerAppel(contexte, TsFdGererUtilSecFtpSvr.ObtenirCodeEnv())

            CaAffaire = objAppel.CreerComposantIntegration(chaineContexte)
            CaAffaire.ExporterCles(chaineContexte)
            objAppel.AnalyserRetour(chaineContexte, Nothing)
        End Using

    End Sub

    ''' <summary>
    ''' Obtenir la string sans blanc
    ''' </summary>
    ''' <param name="pValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function GetString(pValue As Object) As String
        Dim valeur As String = String.Empty

        If pValue IsNot Nothing Then
            valeur = pValue.ToString.Trim
        End If

        Return valeur
    End Function

End Class
