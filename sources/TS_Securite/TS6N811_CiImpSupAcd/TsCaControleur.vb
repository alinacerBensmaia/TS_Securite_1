Imports Rrq.InfrastructureLotPFI.ScenarioTransactionnel
Imports Rrq.InfrastructureCommune.UtilitairesCommuns
Imports Cfg = Rrq.InfrastructureCommune.Parametres.XuCuConfiguration
Imports System.IO
Imports System.Data.SqlClient
Imports System.Text
Imports System.Text.RegularExpressions

'''-----------------------------------------------------------------------------
''' Project		: TS6N811_CiImpSupAcd
''' Class		: TsCaControleur
'''-----------------------------------------------------------------------------
''' <summary>
''' Travail permettant de cumuler les traces d'utilisation du mode SuperAcid à partir d'un travail lot PFI sans prise de points de synchronisation.
''' </summary>
''' <history>
''' Historique des modifications:
''' ----------------------------------------------------------------------------------------------------------
''' Demande    Date		   Nom			        Description
''' ----------------------------------------------------------------------------------------------------------
''' [Z20514]   2014-04-05  Éric Simard (C)      Création initiale (Migration d'un Lot NT au Lot PFI à partir du projet TS6N811_ZpImpSupAcd
''' </history>
''' --------------------------------------------------------------------------------
Public Class TsCaControleur
    Inherits XdCaTravailNonEncadre


#Region "--- Variables ---"

    ' Contient le répertoire de dépôt du fichier SuperAcid
    Private mRepDepSSA As String

    ' Contient le répertoire de dépôt des anomalies
    Private mChemAnomalie As String

#End Region

#Region "--- Méthodes protégées ---"

    Protected Overrides Sub ExecuterTravailNonEncadre(ByRef pChaineContexte As String)

        'Déclaration des variables
        Dim scConn As SqlConnection = Nothing
        Dim stTrans As SqlTransaction = Nothing
        Dim sqlCmd As SqlCommand
        Dim suppFichier As Boolean = False 'indique si le fichier est à supprimer
        mRepDepSSA = Cfg.ValeurSysteme("TS6", "TS6\TS6N811\CheminDepSuperAcid")
        mChemAnomalie = Cfg.ValeurSysteme("TS6", "TS6\TS6N812\CheminDepUntAdm") + "Anomalies\"

        'Supprimer le contenu du répertoire "Anomalies"
        For Each sFichier As String In Directory.GetFiles(mChemAnomalie)
            File.Delete(sFichier)
        Next

        Try
            Dim objAccesBd As New XuCuAccesBd

            'Ouverture de la connexion SQL
            scConn = CType(objAccesBd.ObtenirConnexionSqlAvecEmprunt(Cfg.ValeurSysteme("TS6", "TS6\TS6N811\ConnexionSQL\CleSymbolique"), _
                                                                     "Importation des fichiers de suivi pour l'accès au mode Super Acid au central", _
                                                                     Cfg.ValeurSysteme("TS6", "TS6\TS6N811\ConnexionSQL\NomDatasource"), _
                                                                     Cfg.ValeurSysteme("TS6", "TS6\TS6N811\ConnexionSQL\NomBaseDonnees")),  _
                                                                     SqlConnection)

            'On doit passer la validation des Unité Admin. avant de continuer.
            If ValiderUADsFichier(scConn) Then

                For Each sFichSuperAcid As String In Directory.GetFiles(mRepDepSSA)

                    Try

                        'Begin transaction et création de commande
                        stTrans = scConn.BeginTransaction()
                        sqlCmd = New SqlCommand("", scConn, stTrans)

                        'Lis le fichier et fait les insertion dans la BD
                        suppFichier = LireFichierSupAcd(sqlCmd, sFichSuperAcid)

                        stTrans.Commit()

                    Catch
                        If (Not (stTrans Is Nothing)) Then stTrans.Rollback()
                        Throw
                    Finally

                        'On efface le fichier si tout c'est bien déroulé
                        If (suppFichier) Then
                            File.Delete(sFichSuperAcid)
                        End If

                    End Try
                Next

            End If  'ValiderUADsFichier

        Finally

            'Fermeture de la connexion SQL
            If (Not (scConn Is Nothing) AndAlso scConn.State <> ConnectionState.Closed) Then
                scConn.Close()
            End If
        End Try

    End Sub

#End Region

#Region "--- Privées ---"

#Region "--- Méthodes ---"

    ''' Class.Method:	TsCaControleur.DoublerApostrophes
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sTexte">
    ''' 	[Mettre description ici]. 
    ''' 	Value Type: <see cref="String" />	(System.String)
    ''' </param>
    ''' --------------------------------------------------------------------------------
    Private Function DoublerApostrophes(ByVal sTexte As String) As String
        Dim re As New System.Text.RegularExpressions.Regex("'{1}")
        Return re.Replace(sTexte, "''")
    End Function

    ''' Class.Method:	TsCaControleur.ValiderUADsFichier
    ''' <summary>
    ''' Cette fonction valide les unitées administratives inscrites au fichier(input) avant de commencer
    ''' le traitement de chargement des données.
    ''' </summary>
    ''' <param name="sqlConn">
    ''' 	Value Type: <see cref="Data.SqlClient.SqlConnection" />	(System.Data.SqlClient.SqlConnection)
    ''' </param>
    ''' --------------------------------------------------------------------------------
    Private Function ValiderUADsFichier(ByVal sqlConn As SqlConnection) As Boolean
        'Déclaration des variables
        Dim estValide As Boolean = True
        Dim dtUA As DataTable = ObtnrUA(sqlConn)
        Dim sNomFichAnomalies As String = mChemAnomalie & "Erreurs_detectees" & ".txt"
        Dim swFichSupAcd As StreamWriter = Nothing
        Dim srFichSA As StreamReader = Nothing
        Dim buffer As String
        Dim line As String
        Dim endOfBlock As Boolean
        Dim match As Match

        Try
            'Création de l'écriveur de fichier
            swFichSupAcd = New StreamWriter(sNomFichAnomalies)

            'On parcourt le répertoire pour les fichiers
            For Each sFichSuperAcid As String In Directory.GetFiles(mRepDepSSA)

                Try

                    'Création du lecteur de fichier
                    srFichSA = New StreamReader(sFichSuperAcid)

                    While srFichSA.Peek <> -1
                        'On lit un block de données du fichier d'input
                        endOfBlock = False
                        buffer = ""
                        While Not endOfBlock
                            line = srFichSA.ReadLine
                            If Regex.IsMatch(line, "^\*+$") Then
                                endOfBlock = True
                            Else
                                buffer &= line & vbCrLf
                            End If
                        End While

                        'Récupère et valide le code d'Unité administrative - ligne UNITE=XXXX
                        match = Regex.Match(buffer, "^UNITE=([0-9]+)", RegexOptions.Multiline)
                        If Not match.Success OrElse Not ValiderUA(match.Groups(1).Value, dtUA) Then
                            estValide = False

                            'Entête pour indiquer d'où proviennent les entrées en erreurs
                            swFichSupAcd.WriteLine("##### " & sFichSuperAcid & " #####" & vbCrLf)

                            'Écriture dans fichier anomalies
                            EcrireFicAccSupAcd(buffer, swFichSupAcd)

                        End If

                    End While

                Finally

                    'On ferme le lecteur                
                    srFichSA.Close()

                    'on dispose du lecteur
                    If (TypeOf srFichSA Is IDisposable) Then
                        DirectCast(srFichSA, IDisposable).Dispose()
                    End If

                End Try

            Next 'sFichSuperAcid

        Finally

            'on ferme l'écriveur
            swFichSupAcd.Close()

            'On dispose de l'écriveur
            If (TypeOf swFichSupAcd Is IDisposable) Then
                DirectCast(swFichSupAcd, IDisposable).Dispose()
            End If

            If estValide Then
                File.Delete(sNomFichAnomalies)
            Else
                EnvoyerCourrielAdmin(sNomFichAnomalies, dtUA)
            End If

        End Try

        'Valeur de retour
        Return estValide

    End Function

    ''' Class.Method:	TsCaControleur.ObtnrUA
    ''' <summary>
    ''' Récupère les codes d'unitées administratives dans la banque.
    ''' </summary>
    ''' <param name="sqlConn">
    ''' 	Value Type: <see cref="Data.SqlClient.SqlConnection" />	(System.Data.SqlClient.SqlConnection)
    ''' </param>
    ''' --------------------------------------------------------------------------------
    Private Function ObtnrUA(ByVal sqlConn As SqlConnection) As DataTable
        Dim reqUA As String = "SELECT NO_UNT_ADM FROM TS1.UNADMSA"
        Dim sqlCmd As SqlCommand
        Dim dtUA As New DataTable

        Try
            sqlCmd = New SqlCommand(reqUA, sqlConn)
            Dim sqlDa As New SqlDataAdapter(sqlCmd)

            sqlDa.Fill(dtUA)

            Return dtUA

        Catch ex As SqlException
            Throw New TsCuErreurSQL(ex, reqUA)
        End Try

    End Function

    ''' Class.Method:	TsCaControleur.ValiderUA
    ''' <summary>
    ''' Permet de valider si c'est une UA valide.
    ''' </summary>
    ''' <param name="dtUA">
    ''' 	[Mettre description ici]. 
    ''' 	Value Type: <see cref="Data.DataTable" />	(System.Data.DataTable)
    ''' </param>
    ''' --------------------------------------------------------------------------------
    Private Function ValiderUA(ByVal UA As String, ByVal dtUA As DataTable) As Boolean
        Dim estValide As Boolean = False

        For Each UnitAdm As DataRow In dtUA.Rows
            If UA = UnitAdm.Item(0).ToString Then
                estValide = True
                Exit For
            End If
        Next

        Return estValide

    End Function

    ''' Class.Method:	TsCaControleur.EnvoyerCourrielAdmin
    ''' <summary>
    ''' Cette méthode sert à envoyer un courriel aux administrateurs en cas de problème.
    ''' </summary>
    ''' --------------------------------------------------------------------------------
    Private Sub EnvoyerCourrielAdmin(ByVal sNomFichAnomalies As String, ByVal dtUA As DataTable)
        Dim ecAlerte As New XuCuEnvoiCourriel
        Dim strUA As String = String.Empty
        Dim message As New StringBuilder

        'On fait la liste des Unitées Administratives
        For Each UnitAdm As DataRow In dtUA.Rows
            strUA = strUA + UnitAdm.Item(0).ToString + "<br>"
        Next

        'Affect l'adresse de l'expéditeur
        ecAlerte.Expediteur = System.Reflection.[Assembly].GetExecutingAssembly().GetName().Name + "@rrq.gouv.qc.ca"

        'Le message est de format HTML
        ecAlerte.FormatMessage = XuCuEnvoiCourriel.XuEcFormatMessage.XuEcFmHTML

        'Envoyer courriel au groupe sécurité        
        ecAlerte.Destinataire = Cfg.ValeurSysteme("TS6", "TS6\TS6N812\CourrielGRPSecurite")
        ecAlerte.Objet = "UTILISATIONS SUPER ACID UNITÉ ADMINISTRATIVE NON SUPPORTÉE"

        'Le message à envoyer
        message.Append("Des unités administratives non valides sont présentes dans le fichier de données.")
        message.Append("<BR>")
        message.Append("<BR>")
        message.Append("Voir les ")
        message.Append("<a href=""" & sNomFichAnomalies & """>erreurs détectées</a>")
        message.Append(" pour plus de détails : ")
        message.Append("<BR>")
        message.Append("<BR>")
        message.Append("La liste des unitées administratives supportées est: ")
        message.Append("<BR>")
        message.Append(strUA)
        message.Append("<BR>")
        message.Append("<BR>")
        message.Append("P.S. NE PAS RÉPONDRE À CE MESSAGE.")

        'Envoyer le message
        ecAlerte.Message = message.ToString()
        ecAlerte.EnvoyerCourriel()

    End Sub

    ''' Class.Method:	TsCaControleur.EcrireFicAccSupAcd
    ''' <summary>
    ''' Cette méthode écrit les anomalies dans un fichier plat.
    ''' </summary>
    ''' <param name="buffer">
    ''' 	Value Type: <see cref="String" />	(System.String)
    ''' </param>
    ''' <param name="swFichSupAcd">
    ''' 	Value Type: <see cref="IO.StreamWriter" />	(System.IO.StreamWriter)
    ''' </param>
    ''' --------------------------------------------------------------------------------
    Private Sub EcrireFicAccSupAcd(ByVal buffer As String, ByVal swFichSupAcd As StreamWriter)

        swFichSupAcd.Write(buffer)

    End Sub

    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	TsCaControleur.LireFichierSupAcd
    ''' <summary>
    ''' Cette méthode fait la lecture d'un fichier super acid est y insert les entrées
    ''' lues dans la base de donnée SQL prévue à cet effet.
    ''' </summary>
    ''' <param name="sqlCmd">
    ''' 	Commande SQL à exécuter. Elle devra être initialisée avec un connexion et une transaction.
    ''' 	Value Type: <see cref="Data.SqlClient.SqlCommand" />	(System.Data.SqlClient.SqlCommand)
    ''' </param>
    ''' <param name="sFichSuperAcid">
    ''' 	Le chemin du fichier super acid.
    ''' 	Value Type: <see cref="String" />	(System.String)
    ''' </param>
    ''' <returns><see cref="Boolean" />	Retourne vrai si la fonction se termine sans erreurs. </returns>
    ''' <remarks><para><pre>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2007-11-20	t20840b		Création initiale
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    Private Function LireFichierSupAcd(ByVal sqlCmd As SqlCommand, ByVal sFichSuperAcid As String) As Boolean

        Const ENCODAGE_FRANCAIS As Integer = 1252
        Dim srFichSA As StreamReader = Nothing
        Dim fsFichier As FileStream = Nothing
        Dim bFinTrouve As Boolean = False
        Dim content As String
        Dim aucuneErreur As Boolean = True ' indique s'il y a eu une erreur lors du traitement
        Dim block As String
        Dim lineBuffer(80) As Char
        Dim matches As MatchCollection
        Dim match As Match
        Dim m1, m2 As Integer


        Try

            'On crée le lecteur pour le fichier
            fsFichier = New FileStream(sFichSuperAcid, FileMode.Open, FileAccess.Read, FileShare.Read)
            srFichSA = New StreamReader(fsFichier, Text.Encoding.GetEncoding(ENCODAGE_FRANCAIS))

            content = srFichSA.ReadToEnd

            matches = Regex.Matches(content, "^\*+", RegexOptions.Multiline)

            For i As Integer = matches.Count - 1 To 0 Step -1
                m2 = matches(i).Index
                If i > 0 Then
                    m1 = matches(i - 1).Index
                Else
                    m1 = 0
                End If

                block = content.Substring(m1, m2 - m1)
                Dim sReqInsert As String = "INSERT INTO TS1.DEMSUAC(PR_UTL_SUP_ACD, NM_UTL_SUP_ACD, NO_TEL_UTL_SUP_ACD, NO_USG, NO_UNT_ADM, " + _
                            "DH_DEM_SUP_ACD, DS_CHO_MEN_SA, VL_DUR_DEM_SUP_ACD, DS_RAI_DEM_SA1, DS_RAI_DEM_SA2, DS_RAI_DEM_SA3, DS_RAI_DEM_SA4, " + _
                            "DS_RAI_DEM_SA5, DS_RAI_DEM_SA6)VALUES("

                If block.Length > 0 Then
                    match = Regex.Match(block, "^NOM=(\S+) (.+)", RegexOptions.Multiline)
                    If match.Success Then
                        sReqInsert &= formatValue(match.Groups(1).Value)
                        sReqInsert &= "," & formatValue(match.Groups(2).Value)
                    End If

                    match = Regex.Match(block, "^TELEPHONE=(.+)", RegexOptions.Multiline)
                    If match.Success Then
                        sReqInsert &= "," & formatValue(match.Groups(1).Value)
                    End If

                    match = Regex.Match(block, "^USER=(.+)", RegexOptions.Multiline)
                    If match.Success Then
                        sReqInsert &= "," & formatValue(match.Groups(1).Value)
                    End If

                    match = Regex.Match(block, "^UNITE=(.+)", RegexOptions.Multiline)
                    If match.Success Then
                        sReqInsert &= "," & formatValue(match.Groups(1).Value)
                    End If

                    match = Regex.Match(block, "^TEMPS=([0-9][0-9]:[0-9][0-9]:[0-9][0-9])\s+([0-9][0-9]/[0-9][0-9]/[0-9][0-9]) +(.+)?$", RegexOptions.Multiline)
                    If match.Success Then
                        Dim dd As DateTime = DateTime.Parse(match.Groups(2).Value & " " & match.Groups(1).Value)
                        sReqInsert &= "," & formatValue(dd.ToString("s"))
                        sReqInsert &= "," & formatValue(match.Groups(3).Value)
                    End If

                    match = Regex.Match(block, "^DUREE=(.+)", RegexOptions.Multiline)
                    If match.Success Then
                        sReqInsert &= "," & formatValue(match.Groups(1).Value)
                    Else
                        sReqInsert &= ",'Durée ?'"
                    End If

                    For j As Integer = 1 To 6
                        match = Regex.Match(block, "^RAISON" & j & "=(.+)", RegexOptions.Multiline)
                        If match.Success Then
                            sReqInsert &= "," & formatValue(match.Groups(1).Value)
                        Else
                            sReqInsert &= ",NULL"
                        End If
                    Next
                    sReqInsert &= ")"

                    'Insérer suivi dans la banque
                    sqlCmd.CommandText = sReqInsert

                    Try
                        'executer la commande SQL
                        sqlCmd.ExecuteNonQuery()
                    Catch ex As SqlException
                        ' Si c'est une erreur d'insertion due au fait que
                        ' la ligne est déjà présente on ignore l'erreur.
                        If (ex.Number = 2601) Then
                            'Nous sommes au record déjà inséré.
                            Exit For
                        Else
                            aucuneErreur = False
                            Throw New TsCuErreurSQL(ex, sReqInsert)
                        End If
                    End Try
                End If
            Next

        Finally

            ''Fermeture du streamreader
            If (Not (srFichSA Is Nothing)) Then
                srFichSA.Close()
            End If

            'Disposer de l'objet
            If TypeOf srFichSA Is IDisposable Then
                DirectCast(srFichSA, IDisposable).Dispose()
            End If


        End Try

        Return aucuneErreur

    End Function

    Private Function formatValue(ByVal value As String) As String
        Dim result As String = ""
        value = value.Trim

        If value.Length = 0 Then
            result = "NULL"
        Else
            result = "'" & DoublerApostrophes(value) & "'"
        End If
        Return result
    End Function

#End Region

#End Region

End Class

<Serializable()> Public Class TsCuErreurSQL
    Inherits ApplicationException

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub

    Public Sub New(ByVal innerEx As SqlException, ByVal sCmdSQL As String)
        MyBase.New("Message: " + innerEx.Message + vbNewLine + "Commande SQL: " + sCmdSQL, innerEx)
    End Sub

End Class