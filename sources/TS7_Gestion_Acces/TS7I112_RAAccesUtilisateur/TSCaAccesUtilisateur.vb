Imports Rrq.Web.GabaritsPetitsSystemes.Utilitaires
Imports Rrq.Securite.GestionAcces
Imports Rrq.Web.AccesUtilisateurs.Utilitaires
Imports Rrq.Web.GabaritsPetitsSystemes.Controles
Imports System.Collections.Generic
Imports System.Linq



Public Class TSCaAccesUtilisateur
    Inherits Rrq.Web.GabaritsPetitsSystemes.Utilitaires.NiCaBase
    Protected Overrides ReadOnly Property CodeSysteme() As String
        Get
            Return "TS7"
        End Get
    End Property

#Region "Fonctions privées"
    Public Function ObtenirUtilisateurUnique(ByVal strCodeUtilisateur As String) As DataTable
        Return TSCuGeneral.UtilisateurUniqueDataTable(TsCaServiceGestnAcces.ObtenirUtilisateur(strCodeUtilisateur.ToUpper))
    End Function

    Public Function ObtenirRechercherUtilisateur(ByVal strNom As String, ByVal strPrenom As String) As DataTable
        Return TSCuGeneral.UtilisateurDataTable(TsCaServiceGestnAcces.RechercherUtilisateur(String.Concat(strNom, " ", strPrenom)))
    End Function

    Public Function ObtenirUniteAdmin() As DataTable
        Return TSCuGeneral.UniteAdminDataTable(TsCaServiceGestnAcces.ObtenirListeUnitesAdmin())
    End Function

    Public Function ObtenirRoleUniteAdmin(ByVal strIDRole As String) As DataTable
        Return TSCuGeneral.RoleDataTable(
                        TsCaServiceGestnAcces.ObtenirRolesUniteAdmin(strIDRole))
    End Function

    Public Function ValiderSiListeAjout(ByVal dtListe As DataTable) As Boolean
        'Vérifier s'il y a eu ajout dans la liste
        'Afin de gérer l'approbation
        Dim blnRetour As Boolean = False
        For Each dr As DataRow In dtListe.Rows
            If dr.Item("strSupprimer").Equals("False") And dr.Item("Action").Equals("A") Then
                blnRetour = True
                Exit For
            End If
        Next
        Return blnRetour
    End Function

    Public Function ValiderSiListeModif(ByVal dtListe As DataTable) As Boolean
        'Vérifier s'il y a eu ajout dans la liste
        'Afin de gérer l'approbation
        Dim blnRetour As Boolean = False
        For Each dr As DataRow In dtListe.Rows
            'Un role de supprimer
            If dr.Item("strSupprimer").Equals("True") And dr.Item("Action").Equals("O") Then
                blnRetour = True
                Exit For
            End If
            'Une date de fin d'inscrite
            If Not dr.Item("DateFin").Equals(String.Empty) Then
                blnRetour = True
                Exit For
            End If
        Next
        Return blnRetour
    End Function


    Public Sub EnvoyerMessageSecurite(ByRef cntxTS7 As NiCaContexte,
                                                ByVal strRepertoire As String,
                                                ByVal strUA As String,
                                                ByVal strMetier As String,
                                                ByVal strOptFichier As String,
                                                ByVal strOptDossier As String,
                                                ByVal strDatEffective As String)

        Dim trxTS7 As TSCdObjetTrx = CType(cntxTS7.TrxCourante, TSCdObjetTrx)

        Dim strLignePointillee As String = "<hr color='#000000' size='1' style='border-style: dotted; border-width: 1'>"
        Dim strAdresseCourriel As String
        Dim strMessageConclusion As String = String.Empty
        'Composer le message à envoyer
        Dim strMessage As String = String.Concat("<br />", "Bonjour,", "<br /><br />",
                                                 "Une demande d'accès à un répertoire a été demandée :<br /><br />",
                                                 strLignePointillee)
        'Coder les lignes pour le message
        strMessage = String.Concat(strMessage,
                                   "<table cellPadding='5' width='100%'>",
                                        "<tr>",
                                            "<td><b>Date</b><br />", NiCuGeneral.ObtenirDateJour(), "</td>",
                                        "</tr>",
                                        "<tr>",
                                            "<td><b>Répertoire</b><br />", strRepertoire, "</td>",
                                        "</tr>",
                                        "<tr>",
                                            "<td><b>Unité administrative</b><br />", strUA, "</td>",
                                        "</tr>",
                                         "<tr>",
                                            "<td><b>Métiers</b><br />", strMetier, "</td>",
                                        "</tr>",
                                         "<tr>",
                                            "<td><b>Permission sur les fichiers(Documents)</b><br />", strOptFichier, "</td>",
                                        "</tr>",
                                        "<tr>",
                                            "<td><b>Permission sur les répertoires(Dossiers)</b><br />", strOptDossier, "</td>",
                                        "</tr>",
                                        "<tr>",
                                            "<td><b>Date effective de la demande</b><br />", strDatEffective, "</td>",
                                        "</tr>",
                                    "</table>")

        'Envoyer le courriel au responsable de la securite

        strAdresseCourriel = TSCuDomVal.obtenirAdresseCourriel()
        'TSCuGeneral.EnvoyerCourriel("Demande d'accès", strMessage, strAdresseCourriel, True)

    End Sub

    Public Sub CreerFichierTxt(ByVal strNmRepertoire As String,
                                ByVal strNoUnitAdmn As String,
                                ByVal strRoleMetier As String,
                                ByVal strOptFichier As String,
                                ByVal strOptDossier As String,
                                ByVal strDatEffective As String,
                                ByVal strInfoDemandeur As String)

        Dim strPath As String = TSCuDomVal.ObtenirCheminHeat()
        Dim separateur As String = "|"
        Dim NomFichier As String = "\TS7N111_" & DateTime.Now.ToString("yyyyMMddHHmmss") & ".txt"

        'Dim fs As New IO.FileStream(strPath & NomFichier, IO.FileMode.Create, IO.FileAccess.Write)
        'Ticket Generator doit recevoir un fichier sous un format ANSI
        Dim infoAInscrire As New Text.StringBuilder
        With infoAInscrire
            .Append(strNmRepertoire)
            .Append(separateur)
            .Append("")
            .Append(separateur)
            .Append(strRoleMetier)
            .Append(separateur)
            .Append(strOptFichier)
            .Append(separateur)
            .Append(strOptDossier)
            .Append(separateur)
            .Append(strDatEffective)
            .Append(separateur)
            .Append(strNoUnitAdmn)
            .Append(separateur)
            .Append("")
            .Append(separateur)
            .Append(strInfoDemandeur)
            .Append(separateur)
            .Append(TSCuDomVal.ObtenirAssignationHeat)

            Dim s As New IO.StreamWriter(strPath & NomFichier, False, System.Text.Encoding.GetEncoding("iso-8859-1"))
            s.WriteLine(infoAInscrire.ToString)

            'Fermer le fichier 

            s.Close()
        End With

    End Sub
    ''' <summary>
    ''' 'Message réservé uniquement aux date d'expiration qui dépasse 14 jours apres la date effective.
    ''' </summary>
    ''' <param name="cntxTS7"></param>
    ''' <param name="strAncNoUnitAdmn"></param>
    ''' <param name="strNomEmp"></param>
    ''' <param name="strNomDemandeur"></param>
    ''' <param name="strDatEffective"></param>
    ''' <param name="dtRolesConfirm"></param>
    ''' <param name="dtListeUADemandeur"></param>
    ''' <remarks></remarks>
    Public Sub EnvoyerMessageProlongationExpiration(ByRef cntxTS7 As NiCaContexte,
                                                    ByVal strAncNoUnitAdmn As String,
                                                    ByVal strNomEmp As String,
                                                    ByVal strNomDemandeur As String,
                                                    ByVal strDatEffective As String,
                                                    ByVal dtRolesConfirm As DataTable,
                                                    ByVal dtListeUADemandeur As DataTable)

        Dim trxTS7 As TSCdObjetTrx = CType(cntxTS7.TrxCourante, TSCdObjetTrx)
        Dim strMessage As String
        Dim strAdresseDestinataires As String = String.Empty


        strMessage = ConstruireMessageDateExpirRole(cntxTS7, strAncNoUnitAdmn, strNomEmp, strNomDemandeur, strDatEffective, dtRolesConfirm, dtListeUADemandeur)

        Dim strAdresseDemandeur As String = cntxTS7.UtilisateurCourant.AdresseCourriel

        'Adresse du gestionnaire de l'ancienne UA
        Dim strAdresseGestionnaireUA As String = ObtenirAdresseCourrielGestionnaire(strAncNoUnitAdmn)

        If Not String.IsNullOrEmpty(strAdresseGestionnaireUA) Then
            strAdresseDestinataires &= ";" & strAdresseGestionnaireUA
        Else
            strAdresseDestinataires = strAdresseGestionnaireUA
        End If

        'Adresse des répondants sécurité de l'ancienne UA
        Dim lstAdresseRepondants As New List(Of String)
        lstAdresseRepondants = ObtenirAdresseCourrielRepondants(strAncNoUnitAdmn)

        For Each adresse As String In lstAdresseRepondants
            If Not String.IsNullOrEmpty(strAdresseDestinataires) Then
                strAdresseDestinataires &= ";" & adresse
            Else
                strAdresseDestinataires = adresse
            End If
        Next

        'Si aucun gestionnaire dans le fichier des gestionnaires, on envoie un courriel au support technique.
        If String.IsNullOrEmpty(strAdresseGestionnaireUA) Then
            EnvoyerMessageSupportTechnique(strAncNoUnitAdmn)
        End If

        'Envoyer le courriel au gestionnaires et aux répondants.
        'Le demandeur doit être en CC si possible.
        'Si aucun destinataire, on met le demandeur comme destinataire.

        If String.IsNullOrEmpty(strAdresseDestinataires) Then
            'Pas de gestionnaires ni de répondants sécurité, on met le demandeur comme destinataire.
            'Todo : verfier si on doit envoyer un courriel au pilote si pas de gestionnaires.
            strAdresseDestinataires = strAdresseDemandeur
            TSCuGeneral.EnvoyerCourriel("Prolongation des accès de " & strNomEmp, strMessage, TSCuDomVal.ObtenirAdrCourrielGestionAcces, strAdresseDestinataires, True)
        Else
            TSCuGeneral.EnvoyerCourriel("Prolongation des accès de " & strNomEmp, strMessage, TSCuDomVal.ObtenirAdrCourrielGestionAcces, strAdresseDestinataires, strAdresseDemandeur, True)
        End If


    End Sub

    Public Sub EnvoyerMessageConfirmation(ByRef cntxTS7 As NiCaContexte,
                                          ByVal strNoUnitAdmnUtilisateur As String,
                                              ByVal strNomEmp As String,
                                              ByVal strNomDemandeur As String,
                                              ByVal strDatEffective As String,
                                              ByRef dtRolesConfirm As DataTable)

        Dim trxTS7 As TSCdObjetTrx = CType(cntxTS7.TrxCourante, TSCdObjetTrx)
        Dim strMessageConclusion As String = String.Empty

        'Composer le message à envoyer
        Dim strMessage As String = String.Concat("<br />", "Une modification aux rôles de " & strNomEmp & " a été demandée par " & strNomDemandeur & ".", "<br /><br />")

        'Entete et description de l'utilisateur
        strMessage &= "<strong><u>Information sur l'employé</u><strong>"

        strMessage = String.Concat(strMessage, "<table cellPadding='0' width='100%'>",
                                   "<tr>",
                                        "<td width='50%' align=""left""><strong>Nom : </strong>", trxTS7.strNom, "</td>",
                                        "<td width='50%' align=""left""><strong>Prénom : </strong>", trxTS7.strPrenom, "</td>",
                                    "</tr>",
                                    "<tr>",
                                        "<td width='50%' align=""left""><strong>Ville : </strong>", trxTS7.strVille, "</td>",
                                        "<td width='50%' align=""left""><strong>Fin de contrat : ", trxTS7.strFinContrat, "</td>",
                                    "</tr>",
                                    "<tr>",
                                        "<td colspan='2'><strong>Unité administrative : </strong>", trxTS7.strUaPrincOpt, "</td>",
                                   "</tr></table>")

        'Zone retrait s'il y a lieu
        Dim strZoneRetrait As String = String.Empty
        Dim pos As Integer

        If Not (trxTS7.dtUAUtilisateurCopie Is Nothing OrElse trxTS7.dtUAUtilisateurCopie.Rows.Count = 0) Then
            For Each dr As DataRow In trxTS7.dtUAUtilisateurCopie.Rows
                If dr("Action").Equals("S") Then
                    If NiCuADO.PointerDT(trxTS7.dtListeUnitesAdmin, "IDRole", dr("IDRole").ToString, pos) Then
                        If String.IsNullOrEmpty(strZoneRetrait) Then
                            strZoneRetrait = "<table cellPadding='0' width='100%'>"
                        End If
                        strZoneRetrait &= String.Concat("<tr><td>Unité administrative: ", trxTS7.dtListeUnitesAdmin.Rows(pos).Item("Nom"), "</td></tr>")
                    End If
                End If
            Next
        End If


        If Not String.IsNullOrEmpty(strZoneRetrait) Then
            'Fermer la table du retrait des information de l'employe
            strZoneRetrait &= "</table>"

            'Afficher le titre si des UA dans la zone de retrait.
            strZoneRetrait = "<strong><u>Retrait d'information à l'employé</u></strong> <br />" & strZoneRetrait
            strMessage &= "<br />" & strZoneRetrait & "<br />"
        End If


        'Inscription des roles
        If Not (dtRolesConfirm Is Nothing OrElse dtRolesConfirm.Rows.Count = 0) Then
            Dim drRoleMetier As IEnumerable(Of DataRow) = dtRolesConfirm.Select("ID like 'REM_%'")
            Dim drRoleTache As IEnumerable(Of DataRow) = dtRolesConfirm.Select("ID like 'RET_%'")

            If Not (drRoleMetier Is Nothing OrElse drRoleMetier.Count = 0) Then
                'Mettre dans l'ordre alphabetique par NomAAfficher
                drRoleMetier = drRoleMetier.OrderBy(Function(x) x("NomAAfficher"))

                'Inscription des roles Métier
                strMessage = String.Concat(strMessage, "<table cellPadding='0' width='100%'>",
                                  "<tr>",
                                       "<th width='50%' align=""left"">Rôles métiers</th>",
                                       "<th width='20%' align=""left"">Prolonger jusqu'au</th>",
                                       "<th width='25%' align=""left"">Action sur le rôle</th>",
                                  "</tr>")

                For Each drRole As DataRow In drRoleMetier
                    strMessage = String.Concat(strMessage,
                                           "<tr>",
                                                "<td width='50%' align=""left"">", drRole.Item("NomAAfficher"), "</td>",
                                                "<td width='25%' align=""left"">", drRole.Item("DateFin"), "</td>",
                                                "<td width='25%' align=""left"">", drRole.Item("ActionAfficher"), "</td>",
                                           "</tr>")
                Next

                strMessage = String.Concat(strMessage, "</table>")
            End If

            strMessage &= "<br />"

            'Savoir si on a des contexte.  Si oui, on affiche la colonne contexte.
            Dim blnAfficherContexte As Boolean = True
            Dim drRolesContexte As IEnumerable(Of DataRow)

            drRolesContexte = dtRolesConfirm.Select("DomValcontexte <> ''")
            blnAfficherContexte = Not drRolesContexte Is Nothing AndAlso drRolesContexte.Count > 0

            If Not (drRoleTache Is Nothing OrElse drRoleTache.Count = 0) Then
                'Mettre en ordre alphabétique
                drRoleTache = drRoleTache.OrderBy(Function(x) x("NomAAfficher"))

                If blnAfficherContexte Then
                    'Inscription des roles de Tache
                    strMessage = String.Concat(strMessage, "<table cellPadding='0' width='100%'>",
                                      "<tr>",
                                           "<th width='40%' align=""left"">Rôles de tâches</th>",
                                           "<th width='15%' align=""left"">Contexte</th>",
                                           "<th width='20%' align=""left"">Prolonger jusqu'au</th>",
                                           "<th width='25%' align=""left"">Action sur le rôle</th>",
                                      "</tr>")

                    For Each drRole As DataRow In drRoleTache
                        strMessage = String.Concat(strMessage,
                                               "<tr>",
                                                    "<td width='40%' align=""left"">", drRole.Item("NomAAfficher"), "</td>",
                                                    "<td width='15%' align=""left"">", drRole.Item("Contexte"), "</td>",
                                                    "<td width='20%' align=""left"">", drRole.Item("DateFin"), "</td>",
                                                    "<td width='25%' align=""left"">", drRole.Item("ActionAfficher"), "</td>",
                                               "</tr>")
                    Next

                    strMessage = String.Concat(strMessage, "</table>")
                Else
                    'PAS DE COLONNE DE CONTEXTE AFFICHÉE.
                    'Inscription des roles de Tache
                    strMessage = String.Concat(strMessage, "<table cellPadding='0' width='100%'>",
                                      "<tr>",
                                           "<th width='40%' align=""left"">Rôles de tâches</th>",
                                           "<th width='20%' align=""left"">Prolonger jusqu'au</th>",
                                           "<th width='25%' align=""left"">Action sur le rôle</th>",
                                      "</tr>")

                    For Each drRole As DataRow In drRoleTache
                        strMessage = String.Concat(strMessage,
                                               "<tr>",
                                                    "<td width='40%' align=""left"">", drRole.Item("NomAAfficher"), "</td>",
                                                    "<td width='20%' align=""left"">", drRole.Item("DateFin"), "</td>",
                                                    "<td width='25%' align=""left"">", drRole.Item("ActionAfficher"), "</td>",
                                               "</tr>")
                    Next

                    strMessage = String.Concat(strMessage, "</table>")
                End If

            End If
        End If

        Dim nomFichier As String = "Aucun"
        If Not trxTS7.FichierPieceJointe Is Nothing AndAlso Not String.IsNullOrEmpty(trxTS7.FichierPieceJointe.NouveauNomFichier) Then
            nomFichier = trxTS7.FichierPieceJointe.NouveauNomFichier
        End If

        If trxTS7.IndComptesSuppModifie Then
            strMessage = String.Concat(strMessage, "<br /><br />", "<b>Modifications aux Comptes Supplémentaires : </b> " & "<br /><span style=""font-weight:normal;"">" & trxTS7.ObtenirTexteDifferencesComptesSupp(True) & "</span>")
        ElseIf trxTS7.IndAChoisiConserver Then
            Dim typesComptes As String = trxTS7.ObtenirTexteTypesComptesSupp()
            strMessage = String.Concat(strMessage, "<br /><br /><b>La conservation du ou des comptes supplémentaires suivants:</b> <span style=""font-weight:normal;"">", typesComptes, "</span> <b>nécessite une confirmation du demandeur.</b><br />")
        End If

        strMessage = String.Concat(strMessage, "<br /><br />", "<b>La demande nécessite des accès non couverts par les rôles, précisez ceux-ci :  </b> " & "<br /><span style=""font-weight:normal;"">" & trxTS7.strTexteLibre & "</span>")

        strMessage = String.Concat(strMessage, "<br /><br />", "<b>Fichier joint : </b> " & "<br /><span style=""font-weight:normal;"">" & nomFichier & "</span>")

        strMessage = String.Concat(strMessage, "<br /><br />", "<b>Date effective :</b> " & "<span style=""font-weight:normal;"">" & strDatEffective & "</span>")

        strMessage = String.Concat(strMessage, "<br /><br />", "Veuillez communiquer avec le demandeur pour obtenir des informations supplémentaires sur cette demande <br /><br />Merci.")

        'Adresse courriel du demandeur.
        Dim strAdresses As String = cntxTS7.UtilisateurCourant.AdresseCourriel

        Dim strAdresseGestionnaire As String = ObtenirAdresseCourrielGestionnaire(strNoUnitAdmnUtilisateur)
        If Not String.IsNullOrEmpty(strAdresseGestionnaire) Then
            strAdresses &= ";" & strAdresseGestionnaire
        End If


        'Adresse des répondants sécurité de la nouvelle UA
        Dim lstAdresseRepondants As New List(Of String)
        lstAdresseRepondants = ObtenirAdresseCourrielRepondants(strNoUnitAdmnUtilisateur)

        For Each adresse As String In lstAdresseRepondants
            If Not String.IsNullOrEmpty(strAdresses) Then
                strAdresses &= ";" & adresse
            Else
                strAdresses = adresse
            End If
        Next

        'Envoyer le courriel au gestionnaire
        TSCuGeneral.EnvoyerCourriel("Modification aux rôles de " & strNomEmp, strMessage, TSCuDomVal.ObtenirAdrCourrielGestionAcces, strAdresses, True)


    End Sub

    Public Sub EnvoyerMessageSupportTechnique(ByVal noUniteAdm As String)


        'Composer le message à envoyer
        Dim strMessage As String = String.Concat("<br />", "Il n'y a pas de gestionnaire inscrit au fichier ""t_gestionnaires.txt"" pour l'unité administrative " & noUniteAdm & ".", "<br /><br />")


        Dim strAdresseDestinataires As String = TSCuDomVal.ObtenirCheminAdrCourrielSupportTechnique()

        'Envoyer le courriel au gestionnaire
        TSCuGeneral.EnvoyerCourriel("Anomalie dans la gestion des accès : gestionnaire manquant", strMessage, TSCuDomVal.ObtenirAdrCourrielGestionAcces, strAdresseDestinataires, True)


    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="cntxTS7"></param>
    ''' <param name="strNoUnitAdmn"></param>
    ''' <param name="strNomEmp"></param>
    ''' <param name="strNomDemandeur"></param>
    ''' <param name="strDatEffective"></param>
    ''' <param name="dtRolesConfirm"></param>
    ''' <remarks></remarks>
    Public Function ConstruireMessageDateExpirRole(ByRef cntxTS7 As NiCaContexte,
                                          ByVal strNoUnitAdmn As String,
                                          ByVal strNomEmp As String,
                                          ByVal strNomDemandeur As String,
                                          ByVal strDatEffective As String,
                                          ByRef dtRolesConfirm As DataTable,
                                          ByRef dtUADemandeur As DataTable) As String

        Dim trxTS7 As TSCdObjetTrx = CType(cntxTS7.TrxCourante, TSCdObjetTrx)
        Dim strMessageConclusion As String = String.Empty

        'Composer le message à envoyer
        Dim strMessage As String = String.Concat("<br />Bonjour,<br /><br />Le mouvement de personnel de ", strNomEmp, " inclut une demande de prolongation de ses accès.")
        strMessage = strMessage & "<br /><br /><b>Demandeur : </b>" & strNomDemandeur

        strMessage = String.Concat(strMessage, "<table cellPadding='0' width='100%'>",
                                   "<tr>",
                                        "<th width='50%' align=""left"">Rôle</th>",
                                        "<th width='50%' align=""left"">Date expiration demandée</th>",
                                   "</tr>")

        'Vérifier si des dates d'expiration dépasse la date de plus de 14 jours.

        Dim dExpirationvalide As DateTime = CType(strDatEffective, DateTime).AddDays(14)

        If Not (dtRolesConfirm Is Nothing OrElse dtRolesConfirm.Rows.Count = 0) Then
            For Each drRole As DataRow In dtRolesConfirm.Rows

                'Ne doit pas être un role ajouté.
                If drRole("ActionAfficher").ToString() = "Modifier" Then
                    'Le demandeur ne doit pas être responsable du role.
                    If Not TSCuGeneral.boolResponsableRole(drRole("ListeUniteAdministrativeResponsable").ToString, dtUADemandeur) Then
                        If Not String.IsNullOrEmpty(drRole("DateFin").ToString) Then
                            If CType(drRole("DateFin").ToString, Date) > dExpirationvalide Then
                                strMessage = String.Concat(strMessage,
                                        "<tr>",
                                                    "<td width='50%' align=""left"">", drRole.Item("Nom"), "</td>",
                                                    "<td width='25%' align=""left"">", drRole.Item("DateFin"), "</td>",
                                                    "</tr>")
                            End If
                        End If
                    End If

                End If

            Next
        End If


        strMessage = String.Concat(strMessage, "</table>")

        strMessage = String.Concat(strMessage, "<br /><br />", "Veuillez communiquer avec le demandeur pour obtenir des informations supplémentaires sur cette demande <br /><br />Merci.")
        Return strMessage

    End Function
    Public Function ObtenirAdresseCourrielGestionnaire(ByVal strUA As String) As String

        Dim strCheminAdrCourrielGestionnaire As String = TSCuDomVal.ObtenirCheminAdrCourrielGestionnaires
        Dim strLine As String
        Dim strAdrCourrielGestionnaire As String = String.Empty
        Dim objStreamReader As StreamReader

        'Pass the file path and the file name to the StreamReader constructor.
        objStreamReader = New StreamReader(strCheminAdrCourrielGestionnaire, System.Text.Encoding.UTF7)

        'Read the first line of text.
        strLine = objStreamReader.ReadLine

        'Continue to read until you reach the end of the file.
        Do While Not strLine Is Nothing

            'Read the next line.
            strLine = objStreamReader.ReadLine

            If Not strLine Is Nothing Then

                Dim tabInfos() As String

                'Séparer les infos qui sont séparés par le délimiteur ";"
                tabInfos = Split(strLine, ";")

                'Indice (0) correspond au numéro d'unité administrative
                If tabInfos(0) = strUA Then
                    strAdrCourrielGestionnaire = tabInfos(2)
                    Exit Do
                End If

            End If
        Loop

        'Close the file.
        objStreamReader.Close()

        Return strAdrCourrielGestionnaire

    End Function

    Public Function ObtenirAdresseCourrielRepondants(ByVal strUA As String) As List(Of String)

        Dim strCheminAdrCourrielGestionnaire As String = TSCuDomVal.ObtenirCheminAdrCourrielRepondantsSecurite
        Dim strLine As String
        Dim lstAdrCourrielRepondants As New List(Of String)
        Dim objStreamReader As StreamReader

        'Pass the file path and the file name to the StreamReader constructor.
        objStreamReader = New StreamReader(strCheminAdrCourrielGestionnaire, System.Text.Encoding.UTF7)

        'Read the first line of text.
        strLine = objStreamReader.ReadLine

        'Continue to read until you reach the end of the file.
        Do While Not strLine Is Nothing

            'Read the next line.
            strLine = objStreamReader.ReadLine

            If Not strLine Is Nothing Then

                Dim tabInfos() As String

                'Séparer les infos qui sont séparés par le délimiteur ";"
                tabInfos = Split(strLine, ";")

                'Indice (0) correspond au numéro d'unité administrative
                If tabInfos(0) = strUA Then
                    lstAdrCourrielRepondants.Add(tabInfos(2))
                    ' Exit Do
                End If

            End If
        Loop

        'Close the file.
        objStreamReader.Close()

        Return lstAdrCourrielRepondants

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ObtenirRegleCoherence() As List(Of TsCdRegleCoherence)


        'Lire le fichier
        Dim strCheminFichier As String = TSCuDomVal.ObtenirFichierReglesCoherence() ' "\\saadevl\mouvperso$\t_Coherence_Essai.txt"
        Dim objStreamReader As StreamReader
        Dim strLine As String
        Dim lstReglesCoherence As New List(Of TsCdRegleCoherence)

        objStreamReader = New StreamReader(strCheminFichier, System.Text.Encoding.UTF7)

        ' strLine = objStreamReader.ReadLine
        strLine = String.Empty

        Do While Not strLine Is Nothing

            strLine = objStreamReader.ReadLine

            If Not strLine Is Nothing Then

                strLine = strLine.Replace("""", "")

                Dim tabInfos() As String

                'Séparer les infos qui sont séparés par le délimiteur ","
                'tabInfos(0) : contient la regle et le type de validation
                'tabinfos(1) : contient le role concerné par la regle.
                tabInfos = Split(strLine, ",")

                'si on a au moins un role et une regle
                If tabInfos.Length > 1 Then
                    'Obtenir le id du role et sa relation. On les recoit de cette facon :
                    'RET_5230_Secretaire_CU = Role associé : RET_5230_Secretaire, Type de Relation : CU (Choix unique)

                    Dim idRoleAss As String
                    Dim strRelation As String
                    Dim intPosition As Integer = 0

                    intPosition = tabInfos(0).LastIndexOf("_")
                    strRelation = tabInfos(0).Substring(intPosition + 1)
                    idRoleAss = tabInfos(0).Substring(0, intPosition)

                    'Verifier si déja dans la liste des règles
                    Dim drRegle As New TsCdRegleCoherence
                    'drRegle = lstReglesCoherence.Where(Function(x) x.IDRole = tabInfos(1)) '.Single
                    If lstReglesCoherence.Where(Function(x) x.IDRole = tabInfos(1)).Count = 0 Then
                        'ajout
                        drRegle = New TsCdRegleCoherence()
                        drRegle.IDRole = tabInfos(1).ToString
                        lstReglesCoherence.Add(drRegle)
                    Else
                        drRegle = lstReglesCoherence.Where(Function(x) x.IDRole = tabInfos(1)).Single
                    End If

                    'Remplir les listes selon le type de relation
                    Select Case strRelation
                        Case "CU"
                            drRegle.LstChoixUnique.Add(idRoleAss)
                        Case "O"
                            drRegle.LstObligatoire.Add(idRoleAss)
                        Case "I"
                            drRegle.LstIncoherent.Add(idRoleAss)
                        Case "CM"
                            drRegle.LstChoixMultiples.Add(idRoleAss)

                    End Select


                End If
            End If

        Loop

        'Close the file.
        objStreamReader.Close()

        Return lstReglesCoherence
    End Function

    Public Function ValiderReglesCoherence(ByVal dtRolesAValider As DataTable, ByVal lstReglesCoherence As List(Of TsCdRegleCoherence)) As List(Of String)
        'RETOUR STRUCTURE
        '0 = le role validé
        '1 = le type de relation en problème
        '2 = le ou les roles en erreur dans le cas des obligatoires et incohérents.
        Dim lstErreur As New List(Of String)

        If lstReglesCoherence Is Nothing OrElse lstReglesCoherence.Count = 0 Then
            lstReglesCoherence = ObtenirRegleCoherence()
        End If

        Dim drRegle As TsCdRegleCoherence

        'parcourir les roles a valider

        For Each RolesAValider As DataRow In dtRolesAValider.Rows


            'Retrouver le role dans la liste des cohérences
            Dim IDRoleAValider As String = RolesAValider("ID").ToString
            If lstReglesCoherence.Where(Function(x) x.IDRole = IDRoleAValider).Count > 0 Then
                drRegle = lstReglesCoherence.Where(Function(x) x.IDRole = IDRoleAValider).Single

                'Valider les roles selon un ordre précis de type relation
                '1 - Incohérents
                If Not (drRegle.LstIncoherent Is Nothing OrElse drRegle.LstIncoherent.Count = 0) Then
                    Dim strIncoherent As String = "'" & String.Join("','", drRegle.LstIncoherent) & "'"
                    'Les roles incohérents ne doivent pas être sélectionné avec le role a valider.
                    If dtRolesAValider.Select("ID in (" & strIncoherent & ") and ID <> '" & IDRoleAValider & "'").Count > 0 Then
                        lstErreur.Add(IDRoleAValider)
                        lstErreur.Add("I")

                        Dim strRolesErreur As String = String.Empty

                        'Envoyer tous les noms de roles incohérents
                        For Each roleErreur As String In drRegle.LstIncoherent
                            If Not String.IsNullOrEmpty(strRolesErreur) Then
                                strRolesErreur = strRolesErreur & "<span style=""font-weight:normal;""> et </span>"
                            End If
                            strRolesErreur = strRolesErreur & DirectCast(TSCuGeneral.RechercherRoleParID(roleErreur), TsCdRole).Nom
                        Next
                        lstErreur.Add(strRolesErreur)
                        Return lstErreur
                    End If
                End If

                '2 - Choix Unique
                If Not (drRegle.LstChoixUnique Is Nothing OrElse drRegle.LstChoixUnique.Count = 0) Then
                    Dim strChoixUnique As String = "'" & String.Join("','", drRegle.LstChoixUnique) & "'"
                    'Doit avoir seulement un choix parmi la liste.
                    If Not dtRolesAValider.Select("ID in (" & strChoixUnique & ") and ID <> '" & IDRoleAValider & "'").Count = 1 Then
                        lstErreur.Add(IDRoleAValider)
                        lstErreur.Add("CU")
                        lstErreur.Add("")
                        Return lstErreur
                    End If
                End If

                '3 - Obligatoire
                If Not (drRegle.LstObligatoire Is Nothing OrElse drRegle.LstObligatoire.Count = 0) Then
                    'Doit avoir sélectionner tous les roles obligatoires.
                    Dim blnValide As Boolean = True
                    Dim strRoleErreur As String = String.Empty

                    For Each roleObligatoire As String In drRegle.LstObligatoire
                        If dtRolesAValider.Select("ID = '" & roleObligatoire & "' and ID <> '" & IDRoleAValider & "'").Count = 0 Then
                            blnValide = False

                        End If
                        If Not String.IsNullOrEmpty(strRoleErreur) Then
                            strRoleErreur = strRoleErreur & "<span style=""font-weight:normal;""> et </span>"
                        End If
                        strRoleErreur = strRoleErreur & DirectCast(TSCuGeneral.RechercherRoleParID(roleObligatoire), TsCdRole).Nom


                    Next
                    If Not blnValide Then
                        lstErreur.Add(IDRoleAValider)
                        lstErreur.Add("O")
                        lstErreur.Add(strRoleErreur)

                        Return lstErreur
                    End If

                End If

                '4 - Choix multiples
                If Not (drRegle.LstChoixMultiples Is Nothing OrElse drRegle.LstChoixMultiples.Count = 0) Then
                    Dim strChoixMultiple As String = "'" & String.Join("','", drRegle.LstChoixMultiples) & "'"
                    'Doit avoir au moins un choix parmi la liste.
                    If dtRolesAValider.Select("ID in (" & strChoixMultiple & ") and ID <> '" & IDRoleAValider & "'").Count = 0 Then
                        lstErreur.Add(IDRoleAValider)
                        lstErreur.Add("CM")
                        lstErreur.Add("")
                        Return lstErreur
                    End If
                End If

            End If

        Next

        Return lstErreur

    End Function

#End Region

End Class
