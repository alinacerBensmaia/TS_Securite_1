Imports Rrq.InfrastructureLotPFI.ScenarioTransactionnel
Imports Rrq.InfrastructureCommune.ScenarioTransactionnel
Imports Rrq.InfrastructureCommune.UtilitairesCommuns
Imports System.Data.SqlClient
Imports System.Text
Imports System.IO
Imports Cfg = Rrq.InfrastructureCommune.Parametres.XuCuConfiguration

'''-----------------------------------------------------------------------------
''' Project		: TS6N812_CiSuiSupAcd
''' Class		: TsCaControleur
'''-----------------------------------------------------------------------------
''' <summary>
''' Travail permettant de produire les rapports et tableaux sommaires sur l'utilisation du mode SuperAcid 
''' à partir d'un travail lot PFI sans prise de points de synchronisation.
''' </summary>
''' <history>
''' Historique des modifications:
''' ----------------------------------------------------------------------------------------------------------
''' Demande    Date		   Nom			        Description
''' ----------------------------------------------------------------------------------------------------------
''' [Z20514]   2014-04-09  Éric Simard (C)      Création initiale (Migration d'un Lot NT au Lot PFI à partir du projet TS6N812_ZpSuiSupAcd
''' </history>
''' --------------------------------------------------------------------------------
Public Class TsCaControleur
    Inherits XdCaTravailNonEncadre

#Region "--- Méthodes protégées ---"

    Protected Overrides Sub ExecuterTravailNonEncadre(ByRef pChaineContexte As String)

        'Affecter la date courante avec la date central trouvée dans le contexte
        Dim dateProd As DateTime = ObtenirDateProd(Convert.ToDateTime(XuCaContexte.DateCentral(pChaineContexte)))

        'création des fichiers de détails des accès au Super Acid
        Dim dtDetSA As DataTable = ObtenirDetailSupAcd(dateProd)
        Dim ecAlerte As New XuCuEnvoiCourriel

        Dim a_drUntAdmSupp() As DataRow = dtDetSA.Select("VL_UNT_ADM_SUP = 'O'")
        Dim dtDetSACopie As DataTable = dtDetSA.Clone
        Dim sDernUntAdm As String = ""

        For Each drUtlSupAcd As DataRow In a_drUntAdmSupp

            'Si on débute un nouveau bloque de ligne pour une nouvelle unité administrative
            'On prépare le rapport mensuel et on envoi un courriel à l'administrateur de la dernière unité.
            If sDernUntAdm <> "" AndAlso sDernUntAdm <> drUtlSupAcd.Item("NO_UNT_ADM").ToString() Then
                'inscrire les utilisations dans un fichier et envoyer un courriel au gestionnaire de l'unité administrative
                EnvoyerUtilSupAcdUntAdm(ecAlerte, dtDetSACopie.Select(), sDernUntAdm, dateProd)

                'vider le DataTable temporaire
                dtDetSACopie.Rows.Clear()
            End If

            'ajouter la rangée courante au DataTable temporaire
            dtDetSACopie.ImportRow(drUtlSupAcd)

            'conserver l'unité administrative courante
            sDernUntAdm = drUtlSupAcd.Item("NO_UNT_ADM").ToString()
        Next

        'S'il reste des lignes dans la table temporaire
        'c'est que c'était le dernier bloque d'unités administratives.
        'Il faut donc préparer un rapport mensuel et un courriel
        If dtDetSACopie.Rows.Count > 0 Then
            'inscrire les utilisations dans un fichier et envoyer un courriel au gestionnaire de l'unité administrative
            EnvoyerUtilSupAcdUntAdm(ecAlerte, dtDetSACopie.Select(), sDernUntAdm, dateProd)
        End If

        'End If

        'création du rapport mensuel
        Dim commandeSQl As New SqlCommand("select NO_UNT_ADM, NM_GES_UNT_ADM, PR_GES_UNT_ADM, AD_COU_ELE_GES from TS1.UNADMSA order by NO_UNT_ADM")
        Dim dtUntAdm As DataTable = ObtenirDataTable(commandeSQl)
        EcrireFicRappMens(dtUntAdm, ObtenirStatsSupAcdMens(dateProd), dateProd)


        If (dateProd.Month Mod 3) = 0 Then
            'Création du rapport trimestriel
            EcrireFicRappTrim(dtUntAdm, ObtenirStatsSupAcdTrim(dateProd), dateProd)
        End If

        'Envoyer Courriel Groupe Sécurité
        Dim sCheminRapp As String = Cfg.ValeurSysteme("TS6", "TS6\TS6N812\CheminDepRapp")
        Dim sNomFichier As String = sCheminRapp + "STATIST-ACC-SACID-" + dateProd.ToString("MMMM").ToUpper() + ".html"
        EnvoyerCourrielGRPSecurite(ecAlerte, sNomFichier)

    End Sub

#End Region

#Region "--- Privées ---"

#Region "--- Propriétés ---"

    Dim _nomDuProgramme As String = System.Reflection.[Assembly].GetExecutingAssembly().GetName().Name
    Private ReadOnly Property NomDuProgramme() As String
        Get
            Return _nomDuProgramme
        End Get

    End Property

#End Region

#Region "--- Méthodes ---"

    Private Sub EnvoyerUtilSupAcdUntAdm(ByRef ecAlerte As XuCuEnvoiCourriel, ByVal a_drUntAdm() As DataRow, _
                                        ByVal sNoUntAdm As String, ByVal dateProd As DateTime)

        Dim sChemUntAdm As String = Cfg.ValeurSysteme("TS6", "TS6\TS6N812\CheminDepUntAdm")
        Dim sNomFichUntAdm As String = sChemUntAdm + sNoUntAdm + "\UTIL_SUPER_ACID_" + dateProd.ToString("MMMM_yyyy").ToUpper() + ".txt"
        Dim message As New StringBuilder

        'CRÉER le répertoire s'il n'existe pas 
        Dim repertoire As New DirectoryInfo(sChemUntAdm & sNoUntAdm)
        If (Not repertoire.Exists) Then
            repertoire.Create()
        End If


        'Écrire les informations au fichier
        EcrireFicAccSupAcd(a_drUntAdm, sNomFichUntAdm)

        'Envoyer un courriel au gestionnaire de l'unité administrative ou le ré-aiguiller le cas échaéant.
        ecAlerte.Expediteur = Cfg.ValeurSysteme("TS6", "TS6\TS6N812\CourrielGRPSecurite")
        ecAlerte.Destinataire = CType(IIf(Cfg.ValeurSysteme("TS6", "TS6\TS6N812\ReaiguillerCourriels") = "True", _
                                          Cfg.ValeurSysteme("TS6", "TS6\TS6N812\CourrielGRPSecurite"), _
                                          a_drUntAdm(0).Item("AD_COU_ELE_GES").ToString()), String)
        ecAlerte.Objet = "UTILISATIONS SUPER ACID UNITÉ ADMINISTRATIVE " + sNoUntAdm

        'Le courriel est en format HTML
        ecAlerte.FormatMessage = XuCuEnvoiCourriel.XuEcFormatMessage.XuEcFmHTML

        'Contenu du message
        message.Append("Dans le cadre du suivi de l’utilisation du mode Super Acid à l’ordinateur central, ")
        message.Append("<br>")
        message.Append("le fichier du ")

        'hyper lien vers le fichier
        message.Append("<a href=""")
        message.Append(sNomFichUntAdm)
        message.Append(""">")
        message.Append("détail des utilisations ")
        message.Append("</a>")

        message.Append(" effectuées par du personnel ")
        message.Append("de votre service pour le mois de ")
        message.Append(dateProd.ToString("MMMM yyyy"))
        message.Append(" a été produit.")
        message.Append("<br>")
        message.Append("<br>")


        'Conclusion du message
        message.Append("N’hésitez pas à communiquer avec le groupe de la sécurité pour plus de renseignements. ")
        message.Append("<br>")
        message.Append("<br>")
        message.Append("P.S. NE PAS RÉPONDRE À CE MESSAGE.")


        'Envoyer le message
        ecAlerte.Message = message.ToString()
        ecAlerte.EnvoyerCourriel()

    End Sub

    Private Function ObtenirDetailSupAcd(ByVal dateProd As DateTime) As DataTable
        Dim sRequete As String = _
            "select " + _
                "'O' as VL_UNT_ADM_SUP, DSA.NO_USG, DSA.DH_DEM_SUP_ACD, DSA.PR_UTL_SUP_ACD, DSA.NM_UTL_SUP_ACD, DSA.NO_UNT_ADM, " + _
                "DSA.NO_TEL_UTL_SUP_ACD, DSA.VL_DUR_DEM_SUP_ACD, DSA.DS_RAI_DEM_SA1, DSA.DS_RAI_DEM_SA2, DSA.DS_RAI_DEM_SA3, " + _
                "DSA.DS_RAI_DEM_SA4, DSA.DS_RAI_DEM_SA5, DSA.DS_RAI_DEM_SA6, DSA.DS_CHO_MEN_SA, UASA.AD_COU_ELE_GES " + _
            "from " + _
                "TS1.DEMSUAC as DSA, TS1.UNADMSA as UASA " + _
            "where " + _
                "datepart(""mm"", DSA.DH_DEM_SUP_ACD) = datepart(""mm"", @dateProd) and " + _
                "datepart(""yy"", DSA.DH_DEM_SUP_ACD) = datepart(""yy"", @dateProd) and " + _
                "DSA.NO_UNT_ADM = UASA.NO_UNT_ADM " '+ _

        'Création du paramètre SQL à passer à la requête
        Dim paramSQL As New SqlParameter("@dateProd", SqlDbType.DateTime)
        paramSQL.Direction = ParameterDirection.Input
        paramSQL.Value = dateProd

        'Création de la commande SQL, inclus le paramètre SQL
        Dim commandeSQL As SqlCommand = New SqlCommand(sRequete)
        commandeSQL.Parameters.Add(paramSQL)

        Return ObtenirDataTable(commandeSQL)

    End Function

    Private Function ObtenirStatsSupAcdMens(ByVal dateProd As DateTime) As DataTable
        Dim sRequete As String = _
            "select " + _
                "case when (convert(varchar(10), DSA.DH_DEM_SUP_ACD, 108) >= '08:30:00' and " + _
                    "convert(varchar(10), DSA.DH_DEM_SUP_ACD, 108) <= '16:30:00') and " + _
                    "(datepart(""dw"", DSA.DH_DEM_SUP_ACD) > 1 and datepart(""dw"", DSA.DH_DEM_SUP_ACD) < 7) then 'O' else 'N' end as VL_ACC_HRE_OUV, " + _
                "case when DSA.DS_CHO_MEN_SA is null OR DSA.DS_CHO_MEN_SA ='SPIPROD' then 'N' else 'O' end as VL_ACC_T20_BNQ, DSA.NO_UNT_ADM, " + _
                "case when DSA.DS_CHO_MEN_SA ='SPIPROD' then 'O' else 'N' end as VL_ACC_SPI, " + _
                "datepart(""mm"", DSA.DH_DEM_SUP_ACD) as VL_MOI_DEM_SUP_ACD, DSA.DH_DEM_SUP_ACD, DSA.NO_USG " + _
            "into " + _
                "#STATS_TEMP " + _
            "from " + _
                "TS1.DEMSUAC as DSA, TS1.UNADMSA as UASA " + _
            "where " + _
                "DSA.NO_UNT_ADM = UASA.NO_UNT_ADM " + _
                "and (datepart(""mm"", DSA.DH_DEM_SUP_ACD) = datepart(""mm"", @dateProd) " + _
                "and datepart(""yy"", DSA.DH_DEM_SUP_ACD) = datepart(""yy"", @dateProd)) " + _
            "" + _
            "select " + _
                "DSA.VL_ACC_HRE_OUV, DSA.VL_ACC_T20_BNQ, DSA.VL_ACC_SPI, " + _
                "count(DSA.NO_USG) as VL_NBR_ACC, DSA.NO_UNT_ADM, DSA.VL_MOI_DEM_SUP_ACD " + _
            "from " + _
                "#STATS_TEMP as DSA " + _
            "group by " + _
                "DSA.NO_UNT_ADM, datepart(""mm"", DSA.DH_DEM_SUP_ACD), DSA.VL_ACC_HRE_OUV, DSA.VL_ACC_T20_BNQ, DSA.VL_ACC_SPI, DSA.VL_MOI_DEM_SUP_ACD " + _
            "" + _
            "union " + _
            "" + _
            "select " + _
                "'T' as VL_ACC_HRE_OUV, DSA.VL_ACC_T20_BNQ, DSA.VL_ACC_SPI, " + _
                "count(DSA.NO_USG) as VL_NBR_ACC, DSA.NO_UNT_ADM, DSA.VL_MOI_DEM_SUP_ACD " + _
            "from " + _
                "#STATS_TEMP as DSA " + _
            "group by " + _
                "DSA.NO_UNT_ADM, datepart(""mm"", DSA.DH_DEM_SUP_ACD), DSA.VL_ACC_T20_BNQ, DSA.VL_ACC_SPI, DSA.VL_MOI_DEM_SUP_ACD " + _
            "" + _
            "drop table #STATS_TEMP"

        'Création du paramètre SQL date courante
        Dim paramSQL As New SqlParameter("@dateProd", SqlDbType.DateTime)
        paramSQL.Direction = ParameterDirection.Input
        paramSQL.Value = dateProd

        'Création de la commande SQL inclus le Paramètre SQL date courante
        Dim commandeSQL As New SqlCommand(sRequete)
        commandeSQL.Parameters.Add(paramSQL)


        Return ObtenirDataTable(commandeSQL)

    End Function

    Private Function ObtenirStatsSupAcdTrim(ByVal dateProd As DateTime) As DataTable
        Dim sRequete As String = _
            "select " + _
                "case when (convert(varchar(10), DSA.DH_DEM_SUP_ACD, 108) >= '08:30:00' and " + _
                    "convert(varchar(10), DSA.DH_DEM_SUP_ACD, 108) <= '16:30:00') and " + _
                    "(datepart(""dw"", DSA.DH_DEM_SUP_ACD) > 1 and datepart(""dw"", DSA.DH_DEM_SUP_ACD) < 7) then 'O' else 'N' end as VL_ACC_HRE_OUV, " + _
                "case when DSA.DS_CHO_MEN_SA is null OR DSA.DS_CHO_MEN_SA ='SPIPROD' then 'N' else 'O' end as VL_ACC_T20_BNQ, DSA.NO_UNT_ADM, " + _
                "case when DSA.DS_CHO_MEN_SA ='SPIPROD' then 'O' else 'N' end as VL_ACC_SPI, " + _
                "datepart(""mm"", DSA.DH_DEM_SUP_ACD) as VL_MOI_DEM_SUP_ACD, DSA.DH_DEM_SUP_ACD, DSA.NO_USG " + _
            "into " + _
                "#STATS_TEMP " + _
            "from " + _
                "TS1.DEMSUAC as DSA, TS1.UNADMSA as UASA " + _
            "where " + _
                "DSA.NO_UNT_ADM = UASA.NO_UNT_ADM " + _
                "and ((datepart(""mm"", DSA.DH_DEM_SUP_ACD) = datepart(""mm"", @dateProd) " + _
                "and datepart(""yy"", DSA.DH_DEM_SUP_ACD) = datepart(""yy"", @dateProd)) " + _
                "or (datepart(""mm"", DSA.DH_DEM_SUP_ACD) = datepart(""mm"", dateadd(""mm"", -1, @dateProd)) " + _
                "and datepart(""yy"", DSA.DH_DEM_SUP_ACD) = datepart(""yy"", dateadd(""mm"", -1, @dateProd))) " + _
                "or (datepart(""mm"", DSA.DH_DEM_SUP_ACD) = datepart(""mm"", dateadd(""mm"", -2, @dateProd)) " + _
                "and datepart(""yy"", DSA.DH_DEM_SUP_ACD) = datepart(""yy"", dateadd(""mm"", -2, @dateProd)))) " + _
            "" + _
            "select " + _
                "DSA.VL_ACC_HRE_OUV, DSA.VL_ACC_T20_BNQ, DSA.VL_ACC_SPI, " + _
                "count(DSA.NO_USG) as VL_NBR_ACC, DSA.NO_UNT_ADM, DSA.VL_MOI_DEM_SUP_ACD " + _
            "from " + _
                "#STATS_TEMP as DSA " + _
            "group by " + _
                "DSA.NO_UNT_ADM, datepart(""mm"", DSA.DH_DEM_SUP_ACD), DSA.VL_ACC_HRE_OUV, DSA.VL_ACC_T20_BNQ, DSA.VL_ACC_SPI, DSA.VL_MOI_DEM_SUP_ACD " + _
            "" + _
            "union " + _
            "" + _
            "select " + _
                "'T' as VL_ACC_HRE_OUV, DSA.VL_ACC_T20_BNQ, DSA.VL_ACC_SPI, " + _
                "count(DSA.NO_USG) as VL_NBR_ACC, DSA.NO_UNT_ADM, DSA.VL_MOI_DEM_SUP_ACD " + _
            "from " + _
                "#STATS_TEMP as DSA " + _
            "group by " + _
                "DSA.NO_UNT_ADM, datepart(""mm"", DSA.DH_DEM_SUP_ACD), DSA.VL_ACC_T20_BNQ, DSA.VL_ACC_SPI, DSA.VL_MOI_DEM_SUP_ACD " + _
            "" + _
            "order by " + _
                "DSA.NO_UNT_ADM, DSA.VL_ACC_HRE_OUV, DSA.VL_ACC_T20_BNQ, DSA.VL_ACC_SPI, DSA.VL_MOI_DEM_SUP_ACD " + _
            "" + _
            "drop table #STATS_TEMP"

        'Création du paramètre SQL date courante
        Dim paramSQL As New SqlParameter("@dateProd", SqlDbType.DateTime)
        paramSQL.Direction = ParameterDirection.Input
        paramSQL.Value = dateProd

        'Création de la commande SQL inclus le Paramètre SQL date courante
        Dim commandeSQL As New SqlCommand(sRequete)
        commandeSQL.Parameters.Add(paramSQL)

        Return ObtenirDataTable(commandeSQL)
    End Function

    Private Function ObtenirDataTable(ByVal commandeSQL As SqlCommand) As DataTable
        Dim scConn As SqlConnection = Nothing
        Dim saAdpt As SqlDataAdapter = Nothing
        Dim dtResultat As New DataTable

        Try
            Dim objAccesBd As New XuCuAccesBd

            'Ouverture de la connexion SQL
            scConn = CType(objAccesBd.ObtenirConnexionSqlAvecEmprunt(Cfg.ValeurSysteme("TS6", "TS6\TS6N811\ConnexionSQL\CleSymbolique"), _
                                                                     "Suivi pour l'accès au mode Super Acid au central", _
                                                                     Cfg.ValeurSysteme("TS6", "TS6\TS6N811\ConnexionSQL\NomDatasource"), _
                                                                     Cfg.ValeurSysteme("TS6", "TS6\TS6N811\ConnexionSQL\NomBaseDonnees")),  _
                                                                     SqlConnection)

            'assigner la connection SQL à la commande.
            commandeSQL.Connection = scConn

            'Executer la commande SQL et retourner le résultat dans un data table.
            saAdpt = New SqlDataAdapter(commandeSQL)
            saAdpt.Fill(dtResultat)

            Return dtResultat
        Finally

            If Not (saAdpt Is Nothing) Then
                saAdpt.Dispose()
            End If

            If (Not (scConn Is Nothing) AndAlso scConn.State <> ConnectionState.Closed) Then
                scConn.Close()
            End If


        End Try
    End Function

    Private Sub EcrireFicAccSupAcd(ByVal a_drDetAccSA() As DataRow, ByVal sNomFichier As String)
        Dim swFichSupAcd As StreamWriter = Nothing
        Dim sSpacer As New String(CType(vbTab, Char), 6)

        Try
            swFichSupAcd = New StreamWriter(sNomFichier)

            For Each drAccSupAcd As DataRow In a_drDetAccSA
                swFichSupAcd.WriteLine("NOM:" + (New String(CType(vbTab, Char), 3)) + drAccSupAcd.Item("PR_UTL_SUP_ACD").ToString() + " " + _
                    drAccSupAcd.Item("NM_UTL_SUP_ACD").ToString())
                swFichSupAcd.WriteLine("TÉLÉPHONE:" + (New String(CType(vbTab, Char), 2)) + drAccSupAcd.Item("NO_TEL_UTL_SUP_ACD").ToString())
                swFichSupAcd.WriteLine("UTILISATEUR:" + (New String(CType(vbTab, Char), 2)) + drAccSupAcd.Item("NO_USG").ToString())
                swFichSupAcd.WriteLine("UNITÉ ADMINISTRATIVE:" + (New String(CType(vbTab, Char), 1)) + drAccSupAcd.Item("NO_UNT_ADM").ToString())
                swFichSupAcd.WriteLine("DATE/HEURE:" + (New String(CType(vbTab, Char), 2)) + drAccSupAcd.Item("DH_DEM_SUP_ACD").ToString())
                swFichSupAcd.WriteLine("DURÉE:" + (New String(CType(vbTab, Char), 3)) + drAccSupAcd.Item("VL_DUR_DEM_SUP_ACD").ToString())
                If drAccSupAcd.Item("DS_CHO_MEN_SA").ToString() <> "" Then swFichSupAcd.WriteLine("CHOIX MENU:" + (New String(CType(vbTab, Char), 2)) + _
                    drAccSupAcd.Item("DS_CHO_MEN_SA").ToString())
                swFichSupAcd.WriteLine("RAISON:" + (New String(CType(vbTab, Char), 3)) + drAccSupAcd.Item("DS_RAI_DEM_SA1").ToString())
                If drAccSupAcd.Item("DS_RAI_DEM_SA2").ToString() <> "" Then swFichSupAcd.WriteLine((New String(CType(vbTab, Char), 3)) + _
                    drAccSupAcd.Item("DS_RAI_DEM_SA2").ToString())
                If drAccSupAcd.Item("DS_RAI_DEM_SA3").ToString() <> "" Then swFichSupAcd.WriteLine((New String(CType(vbTab, Char), 3)) + _
                    drAccSupAcd.Item("DS_RAI_DEM_SA3").ToString())
                If drAccSupAcd.Item("DS_RAI_DEM_SA4").ToString() <> "" Then swFichSupAcd.WriteLine((New String(CType(vbTab, Char), 3)) + _
                    drAccSupAcd.Item("DS_RAI_DEM_SA4").ToString())
                If drAccSupAcd.Item("DS_RAI_DEM_SA5").ToString() <> "" Then swFichSupAcd.WriteLine((New String(CType(vbTab, Char), 3)) + _
                    drAccSupAcd.Item("DS_RAI_DEM_SA5").ToString())
                If drAccSupAcd.Item("DS_RAI_DEM_SA6").ToString() <> "" Then swFichSupAcd.WriteLine((New String(CType(vbTab, Char), 3)) + _
                    drAccSupAcd.Item("DS_RAI_DEM_SA6").ToString())
                swFichSupAcd.WriteLine("")
            Next
        Finally
            'Si l'objet exists on le ferme une fois les opérations terminées
            If (Not (swFichSupAcd Is Nothing)) Then
                swFichSupAcd.Close()
            End If
        End Try
    End Sub

    Private Sub EcrireFicRappMens(ByVal dtUntAdm As DataTable, ByVal dtStatsSA As DataTable, ByVal dateProd As DateTime)
        Const C_TITR_RAPP_MENS As String = "<center><h2><b>Tableau des accès en mode SUPERACID, T20 BANQ et SPIPROD</b></h2></center><br><b>Direction : VPTI<br>" + _
                                                    "Nombre d'accès pour la période : {0}</b><br>"
        Const C_BPAG_RAPP_MENS As String = "<i>Préparé par l'équipe d’architecture de sécurité (5240)</i>"
        Const C_TITR_TABL_ACCS As String = "<center><b>Accès en mode SUPER ACID</b></center>"
        Const C_TITR_TABL_ACCS_T20_BANQ As String = "<center><b>Accès en mode SUPER ACID T20 BANQ</b></center>"
        Const C_TITR_TABL_ACCS_SPI_PROD As String = "<center><b>Utilisation du compte SPIPROD (mise à jour SPITAB)</b></center>"
        Const C_LABL_HRES_NORM As String = "Heures normales de bureau<br>8h30 à 16h30"
        Const C_LABL_HRES_DEHR As String = "Heures en dehors des heures normales de bureau<br>16h30 à 8h30 + fin de semaine"
        Const C_LABL_TOTL_ACCS_MOIS As String = "Total des accès du mois"
        Const C_LABL_TOTL_DSI As String = "<b>TOTAL VPTI</b>"

        Dim sCheminRapp As String = Cfg.ValeurSysteme("TS6", "TS6\TS6N812\CheminDepRapp")
        Dim sNomFichier As String = sCheminRapp + "STATIST-ACC-SACID-" + dateProd.ToString("MMMM").ToUpper() + ".html"
        Dim swFichStatMens As StreamWriter = Nothing
        Dim iNbAcc As Integer = 0
        Dim a_drStatSA() As DataRow

        Try
            swFichStatMens = New StreamWriter(sNomFichier, False, System.Text.Encoding.Default)

            'titre du rapport
            swFichStatMens.WriteLine("<html><head><title></title></head><body>")
            swFichStatMens.WriteLine("<table width=750><tr><td>")
            swFichStatMens.WriteLine(String.Format(C_TITR_RAPP_MENS, dateProd.ToString("MMMM yyyy").ToUpper()))

            'tableau d'accès
            'titre du tableau
            swFichStatMens.WriteLine("<p><table width=100% border=1 align=center><tr bgcolor=silver><td colspan=" + (dtUntAdm.Rows.Count + 2).ToString() + ">" + C_TITR_TABL_ACCS + "</td></tr>")

            'titre des colonnes
            swFichStatMens.WriteLine("<tr><td>&nbsp;</td>")
            For Each drUntAdm As DataRow In dtUntAdm.Rows
                swFichStatMens.WriteLine("<td><center><b>" + drUntAdm.Item("NO_UNT_ADM").ToString() + "</b></center></td>")
            Next
            swFichStatMens.WriteLine("<td><center>" + C_LABL_TOTL_DSI + "</center></td></tr>")

            'stats par heures normales de bureau
            swFichStatMens.WriteLine("<tr><td>" + C_LABL_HRES_NORM + "</td>")
            For Each drUntAdm As DataRow In dtUntAdm.Rows
                a_drStatSA = dtStatsSA.Select("NO_UNT_ADM=" + drUntAdm.Item("NO_UNT_ADM").ToString() + " AND VL_ACC_HRE_OUV='O' AND VL_ACC_T20_BNQ='N' AND VL_ACC_SPI='N'")

                If a_drStatSA.Length > 0 Then
                    swFichStatMens.WriteLine("<td><center>" + a_drStatSA(0).Item("VL_NBR_ACC").ToString() + "</center></td>")
                    iNbAcc += Convert.ToInt32(a_drStatSA(0).Item("VL_NBR_ACC"))
                Else
                    swFichStatMens.WriteLine("<td><center>0</center></td>")
                End If
            Next
            swFichStatMens.WriteLine("<td><center><b>" + iNbAcc.ToString() + "</b></center></td></tr>")

            'stats par heures en dehors des heures normales de bureau
            iNbAcc = 0

            swFichStatMens.WriteLine("<tr><td>" + C_LABL_HRES_DEHR + "</td>")
            For Each drUntAdm As DataRow In dtUntAdm.Rows
                a_drStatSA = dtStatsSA.Select("NO_UNT_ADM=" + drUntAdm.Item("NO_UNT_ADM").ToString() + " AND VL_ACC_HRE_OUV='N' AND VL_ACC_T20_BNQ='N' AND VL_ACC_SPI='N'")

                If a_drStatSA.Length > 0 Then
                    swFichStatMens.WriteLine("<td><center>" + a_drStatSA(0).Item("VL_NBR_ACC").ToString() + "</center></td>")
                    iNbAcc += Convert.ToInt32(a_drStatSA(0).Item("VL_NBR_ACC"))
                Else
                    swFichStatMens.WriteLine("<td><center>0</center></td>")
                End If
            Next
            swFichStatMens.WriteLine("<td><center><b>" + iNbAcc.ToString() + "</b></center></td></tr>")

            'stats totales pour le mois
            iNbAcc = 0

            swFichStatMens.WriteLine("<tr bgcolor=silver><td>" + C_LABL_TOTL_ACCS_MOIS + "</td>")
            For Each drUntAdm As DataRow In dtUntAdm.Rows
                a_drStatSA = dtStatsSA.Select("NO_UNT_ADM=" + drUntAdm.Item("NO_UNT_ADM").ToString() + " AND VL_ACC_HRE_OUV='T' AND VL_ACC_T20_BNQ='N' AND VL_ACC_SPI='N'")

                If a_drStatSA.Length > 0 Then
                    swFichStatMens.WriteLine("<td><center>" + a_drStatSA(0).Item("VL_NBR_ACC").ToString() + "</center></td>")
                    iNbAcc += Convert.ToInt32(a_drStatSA(0).Item("VL_NBR_ACC"))
                Else
                    swFichStatMens.WriteLine("<td><center>0</center></td>")
                End If
            Next
            swFichStatMens.WriteLine("<td><center><b>" + iNbAcc.ToString() + "</b></center></td></tr></table></p>")

            'tableau d'accès T20 BANQ
            'titre du tableau
            swFichStatMens.WriteLine("<p><table width=100% border=1><tr bgcolor=silver><td colspan=" + (dtUntAdm.Rows.Count + 2).ToString() + ">" + C_TITR_TABL_ACCS_T20_BANQ + "</td></tr>")

            'titre des colonnes
            Dim iNbUntAdm As Integer = dtUntAdm.Rows.Count - 1

            swFichStatMens.WriteLine("<tr><td>&nbsp;</td>")
            For iIndex As Integer = 1 To iNbUntAdm
                swFichStatMens.WriteLine("<td>&nbsp;</td>")
            Next

            swFichStatMens.WriteLine("<td><center><b>5240</b></center></td>")
            swFichStatMens.WriteLine("<td><center>" + C_LABL_TOTL_DSI + "</center></td></tr>")

            'stats par heures normales de bureau
            swFichStatMens.WriteLine("<tr><td>" + C_LABL_HRES_NORM + "</td>")

            a_drStatSA = dtStatsSA.Select("NO_UNT_ADM=5240 AND VL_ACC_HRE_OUV='O' AND VL_ACC_T20_BNQ='O'")

            For iIndex As Integer = 1 To iNbUntAdm
                swFichStatMens.WriteLine("<td><center>--</center></td>")
            Next

            If a_drStatSA.Length > 0 Then
                swFichStatMens.WriteLine("<td><center>" + a_drStatSA(0).Item("VL_NBR_ACC").ToString() + "</center></td>")
                swFichStatMens.WriteLine("<td><center><b>" + a_drStatSA(0).Item("VL_NBR_ACC").ToString() + "</b></center></td></tr>")
            Else
                swFichStatMens.WriteLine("<td><center>0</center></td>")
                swFichStatMens.WriteLine("<td><center><b>0</b></center></td></tr>")
            End If

            'stats par heures en dehors des heures normales de bureau
            swFichStatMens.WriteLine("<tr><td>" + C_LABL_HRES_DEHR + "</td>")

            a_drStatSA = dtStatsSA.Select("NO_UNT_ADM=5240 AND VL_ACC_HRE_OUV='N' AND VL_ACC_T20_BNQ='O'")

            For iIndex As Integer = 1 To iNbUntAdm
                swFichStatMens.WriteLine("<td><center>--</center></td>")
            Next

            If a_drStatSA.Length > 0 Then
                swFichStatMens.WriteLine("<td><center>" + a_drStatSA(0).Item("VL_NBR_ACC").ToString() + "</center></td>")
                swFichStatMens.WriteLine("<td><center><b>" + a_drStatSA(0).Item("VL_NBR_ACC").ToString() + "</b></center></td></tr>")
            Else
                swFichStatMens.WriteLine("<td><center>0</center></td>")
                swFichStatMens.WriteLine("<td><b><center>0</center></b></td></tr>")
            End If

            'stats totales pour le mois
            swFichStatMens.WriteLine("<tr bgcolor=silver><td>" + C_LABL_TOTL_ACCS_MOIS + "</td>")

            a_drStatSA = dtStatsSA.Select("NO_UNT_ADM=5240 AND VL_ACC_HRE_OUV='T' AND VL_ACC_T20_BNQ='O'")

            For iIndex As Integer = 1 To iNbUntAdm
                swFichStatMens.WriteLine("<td><center>--</center></td>")
            Next

            If a_drStatSA.Length > 0 Then
                swFichStatMens.WriteLine("<td><center>" + a_drStatSA(0).Item("VL_NBR_ACC").ToString() + "</center></td>")
                swFichStatMens.WriteLine("<td><center><b>" + a_drStatSA(0).Item("VL_NBR_ACC").ToString() + "</b></center></td></tr></table></p>")
            Else
                swFichStatMens.WriteLine("<td><center>0</center></td>")
                swFichStatMens.WriteLine("<td><b><center>0</center></b></td></tr></table></p>")
            End If

            'tableau d'accès SPIPROD
            'titre du tableau
            swFichStatMens.WriteLine("<p><table width=100% border=1><tr bgcolor=silver><td colspan=" + (dtUntAdm.Rows.Count + 2).ToString() + ">" + C_TITR_TABL_ACCS_SPI_PROD + "</td></tr>")

            'titre des colonnes

            swFichStatMens.WriteLine("<tr><td>&nbsp;</td>")
            For Each drUntAdm As DataRow In dtUntAdm.Rows
                swFichStatMens.WriteLine("<td><center><b>" + drUntAdm.Item("NO_UNT_ADM").ToString() + "</b></center></td>")
            Next
            swFichStatMens.WriteLine("<td><center>" + C_LABL_TOTL_DSI + "</center></td></tr>")

            iNbAcc = 0

            'stats par heures normales de bureau
            swFichStatMens.WriteLine("<tr><td>" + C_LABL_HRES_NORM + "</td>")
            For Each drUntAdm As DataRow In dtUntAdm.Rows
                a_drStatSA = dtStatsSA.Select("NO_UNT_ADM=" + drUntAdm.Item("NO_UNT_ADM").ToString() + " AND VL_ACC_HRE_OUV='O' AND VL_ACC_SPI='O'")

                If a_drStatSA.Length > 0 Then
                    swFichStatMens.WriteLine("<td><center>" + a_drStatSA(0).Item("VL_NBR_ACC").ToString() + "</center></td>")
                    iNbAcc += Convert.ToInt32(a_drStatSA(0).Item("VL_NBR_ACC"))
                Else
                    swFichStatMens.WriteLine("<td><center>0</center></td>")
                End If
            Next
            swFichStatMens.WriteLine("<td><center><b>" + iNbAcc.ToString() + "</b></center></td></tr>")

            'stats par heures en dehors des heures normales de bureau
            iNbAcc = 0

            swFichStatMens.WriteLine("<tr><td>" + C_LABL_HRES_DEHR + "</td>")
            For Each drUntAdm As DataRow In dtUntAdm.Rows
                a_drStatSA = dtStatsSA.Select("NO_UNT_ADM=" + drUntAdm.Item("NO_UNT_ADM").ToString() + " AND VL_ACC_HRE_OUV='N' AND VL_ACC_SPI='O'")

                If a_drStatSA.Length > 0 Then
                    swFichStatMens.WriteLine("<td><center>" + a_drStatSA(0).Item("VL_NBR_ACC").ToString() + "</center></td>")
                    iNbAcc += Convert.ToInt32(a_drStatSA(0).Item("VL_NBR_ACC"))
                Else
                    swFichStatMens.WriteLine("<td><center>0</center></td>")
                End If
            Next
            swFichStatMens.WriteLine("<td><center><b>" + iNbAcc.ToString() + "</b></center></td></tr>")

            'stats totales pour le mois
            iNbAcc = 0

            swFichStatMens.WriteLine("<tr bgcolor=silver><td>" + C_LABL_TOTL_ACCS_MOIS + "</td>")
            For Each drUntAdm As DataRow In dtUntAdm.Rows
                a_drStatSA = dtStatsSA.Select("NO_UNT_ADM=" + drUntAdm.Item("NO_UNT_ADM").ToString() + " AND VL_ACC_HRE_OUV='T' AND VL_ACC_SPI='O'")

                If a_drStatSA.Length > 0 Then
                    swFichStatMens.WriteLine("<td><center>" + a_drStatSA(0).Item("VL_NBR_ACC").ToString() + "</center></td>")
                    iNbAcc += Convert.ToInt32(a_drStatSA(0).Item("VL_NBR_ACC"))
                Else
                    swFichStatMens.WriteLine("<td><center>0</center></td>")
                End If
            Next
            swFichStatMens.WriteLine("<td><center><b>" + iNbAcc.ToString() + "</b></center></td></tr></table></p>")

            'écriture du bas de page
            swFichStatMens.WriteLine(C_BPAG_RAPP_MENS)
            swFichStatMens.WriteLine("</td></tr></table></body></html>")
        Finally
            'On ferme l'écriveur
            swFichStatMens.Close()

            'On dispose de l'écriveur (c'est plus clean comme ca)
            If (TypeOf swFichStatMens Is IDisposable) Then
                DirectCast(swFichStatMens, IDisposable).Dispose()
            End If

        End Try
    End Sub

    Private Sub EcrireFicRappTrim(ByVal dtUntAdm As DataTable, ByVal dtStatsSA As DataTable, ByVal dateProd As DateTime)

        Const C_TITR_RAPP_TRIM As String = "<center><b><h2>Tableau des accès en mode SUPERACID</h1><br>Direction des systèmes d'information<br>" + _
                                            "Nombre d'accès pour le {0} trimestre {1}</b></center><br>"
        Const C_TITR_TABL_ACCS As String = "<center><b>Accès en mode SUPER ACID</b></center>"
        Const C_LABL_COLN_TOTL_SUPR_ACID As String = "<center><b>Total<br>Super acid(1)</b></center>"
        Const C_LABL_RANG_MOIS As String = "<b>Mois</b>"
        Const C_LABL_HRES_NORM As String = "<b>Heures normales de bureau</b><br>8h30 à 16h30"
        Const C_LABL_HRES_DEHR As String = "Heures en dehors des heures normales de bureau<br>16h30 à 8h30 + Heures fin de semaine"
        Const C_LABL_TOTL_MOIS As String = "Total du mois"
        Const C_TITR_TABL_TOTL_ACCS As String = "<center><b>Nombre d'accès totaux SUPER ACID à la VPTI</b></center>"
        Const C_LABL_COLN_ACCS_SPI As String = "<b>SPIPROD(3)</b>"
        Const C_LABL_COLN_ACCS_T20_BANQ As String = "<b>Banques 5240(2)</b>"
        Const C_LABL_COLN_TOTL_DSI_SUPR_ACID As String = "<b>Super Acid(1)</b>"
        Const C_LABL_COLN_TOTL_DSI As String = "<b>Total VPTI(1)+(2)+(3)</b>"
        Const C_BPAG_RAPP_TRIM As String = "<i>Préparé par l'équipe d’architecture de sécurité (5240)</i>"

        Dim sCheminRapp As String = Cfg.ValeurSysteme("TS6", "TS6\TS6N812\CheminDepRapp")
        Dim iTrimCour As Integer = ObtenirTrimestreCourant(dateProd)
        Dim sNomFichier As String = sCheminRapp + "STATIST-ACC-SACID-TR" + iTrimCour.ToString() + ".html"
        Dim swFichStatTrim As StreamWriter = Nothing
        Dim a_drStatSA() As DataRow
        Dim a_iNbAccSA(2, 2) As Integer

        Try
            swFichStatTrim = New StreamWriter(sNomFichier, False, System.Text.Encoding.Default)

            'Titre du rapport
            swFichStatTrim.WriteLine("<html><head><title></title></head><body>")
            swFichStatTrim.WriteLine("<table width=750><tr><td>")
            swFichStatTrim.WriteLine(String.Format(C_TITR_RAPP_TRIM, IIf(iTrimCour = 1, _
                iTrimCour.ToString() + "ier", iTrimCour.ToString() + "ième").ToString(), dateProd.Year))

            '1er Tableau d'accès
            '***************************************************************************************
            'Titre du tableau
            swFichStatTrim.WriteLine("<p><table width=100% border=1 align=center><tr bgcolor=silver><td colspan=" + _
                (1 + ((dtUntAdm.Rows.Count + 1) * 3)).ToString() + ">" + C_TITR_TABL_ACCS + "</td></tr>")

            'Titre des colonnes
            swFichStatTrim.WriteLine("<tr><td>&nbsp;</td>")
            For Each drUntAdm As DataRow In dtUntAdm.Rows
                swFichStatTrim.WriteLine("<td colspan=3><center><b>" + drUntAdm.Item("NO_UNT_ADM").ToString() + "</b></center></td>")
            Next
            swFichStatTrim.WriteLine("<td colspan=3>" + C_LABL_COLN_TOTL_SUPR_ACID + "</td></tr>")

            'Titre des mois
            swFichStatTrim.WriteLine("<tr><td>" + C_LABL_RANG_MOIS + "</td>")

            Dim iNbUntAdm As Integer = dtUntAdm.Rows.Count + 1
            For iIndex As Integer = 1 To iNbUntAdm
                For iMois As Integer = 2 To 0 Step -1
                    swFichStatTrim.WriteLine("<td><center><b>" + dateProd.AddMonths(-iMois).ToString("MMM").ToLower() + "</b></center></td>")
                Next
            Next
            swFichStatTrim.WriteLine("</tr>")

            'Stats par heures normales de bureau
            For iIndex As Integer = 0 To 2
                For iIndex2 As Integer = 0 To 2
                    a_iNbAccSA(iIndex, iIndex2) = 0
                Next
            Next

            swFichStatTrim.WriteLine("<tr><td>" + C_LABL_HRES_NORM + "</td>")
            For Each drUntAdm As DataRow In dtUntAdm.Rows
                For iMois As Integer = 2 To 0 Step -1
                    a_drStatSA = dtStatsSA.Select("NO_UNT_ADM=" + drUntAdm.Item("NO_UNT_ADM").ToString() + " AND VL_ACC_HRE_OUV='O' " + _
                        "AND VL_ACC_T20_BNQ='N' AND VL_ACC_SPI='N' AND VL_MOI_DEM_SUP_ACD=" + dateProd.AddMonths(-iMois).Month.ToString())

                    If a_drStatSA.Length > 0 Then
                        swFichStatTrim.WriteLine("<td><center>" + a_drStatSA(0).Item("VL_NBR_ACC").ToString() + "</center></td>")
                        a_iNbAccSA(0, iMois) += Convert.ToInt32(a_drStatSA(0).Item("VL_NBR_ACC"))
                    Else
                        swFichStatTrim.WriteLine("<td><center>0</center></td>")
                    End If
                Next
            Next

            For iMois As Integer = 2 To 0 Step -1
                swFichStatTrim.WriteLine("<td><center><b>" + a_iNbAccSA(0, iMois).ToString() + "</b></center></td>")
            Next
            swFichStatTrim.WriteLine("</tr>")

            'Stats par heures en dehors des heures normales de bureau
            swFichStatTrim.WriteLine("<tr><td>" + C_LABL_HRES_DEHR + "</td>")
            For Each drUntAdm As DataRow In dtUntAdm.Rows
                For iMois As Integer = 2 To 0 Step -1
                    a_drStatSA = dtStatsSA.Select("NO_UNT_ADM=" + drUntAdm.Item("NO_UNT_ADM").ToString() + " AND VL_ACC_HRE_OUV='N' " + _
                                            "AND VL_ACC_T20_BNQ='N' AND VL_ACC_SPI='N' AND VL_MOI_DEM_SUP_ACD=" + dateProd.AddMonths(-iMois).Month.ToString())

                    If a_drStatSA.Length > 0 Then
                        swFichStatTrim.WriteLine("<td><center>" + a_drStatSA(0).Item("VL_NBR_ACC").ToString() + "</center></td>")
                        a_iNbAccSA(1, iMois) += Convert.ToInt32(a_drStatSA(0).Item("VL_NBR_ACC"))
                    Else
                        swFichStatTrim.WriteLine("<td><center>0</center></td>")
                    End If
                Next
            Next

            For iMois As Integer = 2 To 0 Step -1
                swFichStatTrim.WriteLine("<td><center><b>" + a_iNbAccSA(1, iMois).ToString() + "</b></center></td>")
            Next
            swFichStatTrim.WriteLine("</tr>")

            'Stats totales pour le mois
            swFichStatTrim.WriteLine("<tr bgcolor=silver><td>" + C_LABL_TOTL_MOIS + "</td>")
            For Each drUntAdm As DataRow In dtUntAdm.Rows
                For iMois As Integer = 2 To 0 Step -1
                    a_drStatSA = dtStatsSA.Select("NO_UNT_ADM=" + drUntAdm.Item("NO_UNT_ADM").ToString() + " AND VL_ACC_HRE_OUV='T' " + _
                                            "AND VL_ACC_T20_BNQ='N' AND VL_ACC_SPI='N' AND VL_MOI_DEM_SUP_ACD=" + dateProd.AddMonths(-iMois).Month.ToString())

                    If a_drStatSA.Length > 0 Then
                        swFichStatTrim.WriteLine("<td><center>" + a_drStatSA(0).Item("VL_NBR_ACC").ToString() + "</center></td>")
                        a_iNbAccSA(2, iMois) += Convert.ToInt32(a_drStatSA(0).Item("VL_NBR_ACC"))
                    Else
                        swFichStatTrim.WriteLine("<td><center>0</center></td>")
                    End If
                Next
            Next

            For iMois As Integer = 2 To 0 Step -1
                swFichStatTrim.WriteLine("<td><center><b>" + a_iNbAccSA(2, iMois).ToString() + "</b></center></td>")
            Next
            swFichStatTrim.WriteLine("</tr></table></p><br>")

            '2eme tableau: d'accès T20 BANQ + Totaux
            '********************************************************************************************
            'Titre du tableau
            swFichStatTrim.WriteLine("<p><table width=100% border=1 align=center><tr bgcolor=silver><td colspan=10>" + C_TITR_TABL_TOTL_ACCS + "</td></tr>")

            'Titre des colonnes
            swFichStatTrim.WriteLine("<tr><td>&nbsp;</td>")
            swFichStatTrim.WriteLine("<td colspan=3>" + C_LABL_COLN_ACCS_SPI + "</td>")
            swFichStatTrim.WriteLine("<td colspan=3>" + C_LABL_COLN_ACCS_T20_BANQ + "</td>")
            swFichStatTrim.WriteLine("<td colspan=3>" + C_LABL_COLN_TOTL_DSI_SUPR_ACID + "</td>")
            swFichStatTrim.WriteLine("<td colspan=3>" + C_LABL_COLN_TOTL_DSI + "</td></tr>")

            'Titre des mois
            swFichStatTrim.WriteLine("<tr><td>" + C_LABL_RANG_MOIS + "</td>")
            For iIndex As Integer = 1 To 3
                For iMois As Integer = 2 To 0 Step -1
                    swFichStatTrim.WriteLine("<td><center><b>" + dateProd.AddMonths(-iMois).ToString("MMM").ToLower() + "</b></center></td>")
                Next
            Next

            swFichStatTrim.WriteLine("</tr>")

            'Stats par heures normales de bureau

            Dim a_iNbAccSpi(2) As Integer

            swFichStatTrim.WriteLine("<tr><td>" + C_LABL_HRES_NORM + "</td>")
            For iMois As Integer = 2 To 0 Step -1
                a_drStatSA = dtStatsSA.Select("VL_ACC_HRE_OUV='O' " + _
                    "AND VL_ACC_SPI='O' AND VL_MOI_DEM_SUP_ACD=" + dateProd.AddMonths(-iMois).Month.ToString())

                If a_drStatSA.Length > 0 Then
                    swFichStatTrim.WriteLine("<td><center>" + a_drStatSA(0).Item("VL_NBR_ACC").ToString() + "</center></td>")
                    a_iNbAccSpi(iMois) = Convert.ToInt32(a_drStatSA(0).Item("VL_NBR_ACC"))
                Else
                    swFichStatTrim.WriteLine("<td><center>0</center></td>")
                    a_iNbAccSpi(iMois) = 0
                End If
            Next

            Dim a_iNbAcc(2) As Integer
            For iMois As Integer = 2 To 0 Step -1
                a_drStatSA = dtStatsSA.Select("NO_UNT_ADM=5240 AND VL_ACC_HRE_OUV='O' " + _
                    "AND VL_ACC_T20_BNQ='O' AND VL_MOI_DEM_SUP_ACD=" + dateProd.AddMonths(-iMois).Month.ToString())

                If a_drStatSA.Length > 0 Then
                    swFichStatTrim.WriteLine("<td><center>" + a_drStatSA(0).Item("VL_NBR_ACC").ToString() + "</center></td>")
                    a_iNbAcc(iMois) = Convert.ToInt32(a_drStatSA(0).Item("VL_NBR_ACC"))
                Else
                    swFichStatTrim.WriteLine("<td><center>0</center></td>")
                    a_iNbAcc(iMois) = 0
                End If
            Next

            For iMois As Integer = 2 To 0 Step -1
                swFichStatTrim.WriteLine("<td><center>" + a_iNbAccSA(0, iMois).ToString() + "</center></td>")
            Next

            For iMois As Integer = 2 To 0 Step -1
                swFichStatTrim.WriteLine("<td><center><b>" + (a_iNbAccSpi(iMois) + a_iNbAcc(iMois) + a_iNbAccSA(0, iMois)).ToString() + "</b></center></td>")
            Next

            swFichStatTrim.WriteLine("</tr>")

            'Stats par heures en dehors des heures normales de bureau
            swFichStatTrim.WriteLine("<tr><td>" + C_LABL_HRES_DEHR + "</td>")

            For iMois As Integer = 2 To 0 Step -1
                a_drStatSA = dtStatsSA.Select("VL_ACC_HRE_OUV='N' " + _
                    "AND VL_ACC_SPI='O' AND VL_MOI_DEM_SUP_ACD=" + dateProd.AddMonths(-iMois).Month.ToString())

                If a_drStatSA.Length > 0 Then
                    swFichStatTrim.WriteLine("<td><center>" + a_drStatSA(0).Item("VL_NBR_ACC").ToString() + "</center></td>")

                    a_iNbAccSpi(iMois) = Convert.ToInt32(a_drStatSA(0).Item("VL_NBR_ACC"))
                Else
                    swFichStatTrim.WriteLine("<td><center>0</center></td>")
                    a_iNbAccSpi(iMois) = 0
                End If
            Next


            For iMois As Integer = 2 To 0 Step -1
                a_drStatSA = dtStatsSA.Select("NO_UNT_ADM=5240 AND VL_ACC_HRE_OUV='N' " + _
                    "AND VL_ACC_T20_BNQ='O' AND VL_MOI_DEM_SUP_ACD=" + dateProd.AddMonths(-iMois).Month.ToString())

                If a_drStatSA.Length > 0 Then
                    swFichStatTrim.WriteLine("<td><center>" + a_drStatSA(0).Item("VL_NBR_ACC").ToString() + "</center></td>")

                    a_iNbAcc(iMois) = Convert.ToInt32(a_drStatSA(0).Item("VL_NBR_ACC"))
                Else
                    swFichStatTrim.WriteLine("<td><center>0</center></td>")
                    a_iNbAcc(iMois) = 0
                End If
            Next

            For iMois As Integer = 2 To 0 Step -1

                swFichStatTrim.WriteLine("<td><center>" + a_iNbAccSA(1, iMois).ToString() + "</center></td>")
            Next

            For iMois As Integer = 2 To 0 Step -1

                swFichStatTrim.WriteLine("<td><center><b>" + (a_iNbAccSpi(iMois) + a_iNbAcc(iMois) + a_iNbAccSA(1, iMois)).ToString() + "</b></center></td>")
            Next

            swFichStatTrim.WriteLine("</tr>")

            'Stats totales pour le mois
            swFichStatTrim.WriteLine("<tr bgcolor=silver><td>" + C_LABL_TOTL_MOIS + "</td>")

            For iMois As Integer = 2 To 0 Step -1
                a_drStatSA = dtStatsSA.Select("VL_ACC_HRE_OUV='T' " + _
                    "AND VL_ACC_SPI='O' AND VL_MOI_DEM_SUP_ACD=" + dateProd.AddMonths(-iMois).Month.ToString())

                If a_drStatSA.Length > 0 Then
                    swFichStatTrim.WriteLine("<td><center>" + a_drStatSA(0).Item("VL_NBR_ACC").ToString() + "</center></td>")

                    a_iNbAccSpi(iMois) = Convert.ToInt32(a_drStatSA(0).Item("VL_NBR_ACC"))
                Else
                    swFichStatTrim.WriteLine("<td><center>0</center></td>")

                    a_iNbAccSpi(iMois) = 0
                End If
            Next

            For iMois As Integer = 2 To 0 Step -1
                a_drStatSA = dtStatsSA.Select("NO_UNT_ADM=5240 AND VL_ACC_HRE_OUV='T' " + _
                    "AND VL_ACC_T20_BNQ='O' AND VL_MOI_DEM_SUP_ACD=" + dateProd.AddMonths(-iMois).Month.ToString())

                If a_drStatSA.Length > 0 Then
                    swFichStatTrim.WriteLine("<td><center>" + a_drStatSA(0).Item("VL_NBR_ACC").ToString() + "</center></td>")

                    a_iNbAcc(iMois) = Convert.ToInt32(a_drStatSA(0).Item("VL_NBR_ACC"))
                Else
                    swFichStatTrim.WriteLine("<td><center>0</center></td>")

                    a_iNbAcc(iMois) = 0
                End If
            Next

            For iMois As Integer = 2 To 0 Step -1

                swFichStatTrim.WriteLine("<td><center>" + a_iNbAccSA(2, iMois).ToString() + "</center></td>")
            Next

            For iMois As Integer = 2 To 0 Step -1

                swFichStatTrim.WriteLine("<td><center><b>" + (a_iNbAccSpi(iMois) + a_iNbAcc(iMois) + a_iNbAccSA(2, iMois)).ToString() + "</b></center></td>")
            Next

            swFichStatTrim.WriteLine("</tr></table></p>")

            'écriture du bas de page
            swFichStatTrim.WriteLine(C_BPAG_RAPP_TRIM)
            swFichStatTrim.WriteLine("</td></tr></table></body></html>")
        Finally
            swFichStatTrim.Close()
        End Try

    End Sub

    Private Function ObtenirTrimestreCourant(ByVal dateProd As DateTime) As Integer
        Dim dTrim As Double = dateProd.Month / 3

        If dTrim = 1.0 Then Return 4 'trimestre 4 : jan,fev,mars
        If dTrim = 2.0 Then Return 1 'trimestre 3 : avril,mai,juin  
        If dTrim = 3.0 Then Return 2 'trimestre 2 : juil,aout,sept
        If dTrim = 4.0 Then Return 3 'trimestre 1 : oct,nov,dec
    End Function

    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	TsCaControleur.EnvoyerCourrielGRPSecurite
    ''' <summary>
    ''' Envoi un courriel au groupe de sécurité
    ''' </summary>
    ''' <param name="courriel">
    ''' 	courriel à envoyer
    ''' 	Value Type: <see cref="Rrq.InfrastructureCommune.UtilitairesCommuns.XuCuEnvoiCourriel" />	(Rrq.InfrastructureCommune.UtilitairesCommuns.XuCuEnvoiCourriel)
    ''' </param>    
    ''' <remarks>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2007-11-16	t20840b		Création initiale
    ''' </remarks>
    ''' <pre></pre>    
    ''' --------------------------------------------------------------------------------
    Private Sub EnvoyerCourrielGRPSecurite(ByVal courriel As XuCuEnvoiCourriel, ByVal sNomFichier As String)

        Dim message As New StringBuilder
        Const NOUVELLE_LIGNE As String = "<br>"

        'Configurer les paramètres du courriel
        courriel.Expediteur = NomDuProgramme & "@rrq.gouv.qc.ca"
        courriel.Destinataire = Cfg.ValeurSysteme("TS6", "TS6\TS6N812\CourrielGRPSecurite")
        courriel.Objet = "SUPER ACID "

        'Le format du message est HTML
        courriel.FormatMessage = XuCuEnvoiCourriel.XuEcFormatMessage.XuEcFmHTML

        'Préparation du message à envoyer
        message.Append("Le traitement mensuel (TS6N812) du mode Super Acid a été exécuté.")
        message.Append(NOUVELLE_LIGNE)
        message.Append("Consulter le ")
        message.Append("<a href=""" & "" & sNomFichier & """>tableau sommaire mensuel</a>")
        message.Append(" pour plus de détails.")
        message.Append(NOUVELLE_LIGNE)
        message.Append(NOUVELLE_LIGNE)
        message.Append("P.S.NE PAS RÉPONDRE À CE MESSAGE.")

        courriel.Message = message.ToString()

        'envoyer le courriel
        courriel.EnvoyerCourriel()


    End Sub

    Private Function ObtenirDateProd(ByVal dateCentral As DateTime) As DateTime

        Return dateCentral.AddMonths(-1)

    End Function

#End Region

#End Region

End Class
