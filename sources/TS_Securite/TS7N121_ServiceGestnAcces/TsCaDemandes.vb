Imports Rrq.Securite.GestionAcces
Imports Rrq.InfrastructureCommune.UtilitairesCommuns

Imports System.Xml.Serialization
Imports System.IO
Imports System.Net.Mail

Imports System.Text.RegularExpressions
Imports System.Text

Imports System.Security.Principal

Imports Configuration = Rrq.InfrastructureCommune.Parametres.XuCuConfiguration

''' <summary>
''' Class de services. 
''' Cette classe Sert à finalisée les opérations du projet.
''' Elle sert à opérer différentes opérations d'enregistrements vers Sage.
''' </summary>
Friend Class TsCaDemandes

#Region "CONSTANTE"

    Private Const NOM_FICHIER_CREATION As String = "Creatn_"
    Private Const NOM_FICHIER_MODIFICATION As String = "Modif_"
    Private Const NOM_FICHIER_SUPPRESSION As String = "Suppr_"

    Private Const FORMAT_DATE As String = "yyyy-MM-dd"

#End Region

#Region "Méthodes globaux"

    ''' <summary>
    ''' Méthode partagée. Cette méthode prend une demande de création d'utilisateur et la 
    ''' transforme en format d'une fiche heat et l'envois au service de sécurité.
    ''' </summary>
    ''' <param name="demndCreation">Une demande de création d'utilisateur.</param>
    ''' <param name="dateEffective">Quand sera effective cette demande.</param>
    Public Shared Sub DemanderCreation(ByVal demndCreation As TsCdDemndCreationModif, ByVal dateEffective As Date)
        DemanderCreation(demndCreation, dateEffective, Nothing)
    End Sub

    ''' <summary>
    ''' Méthode partagée. Cette méthode prend une demande de création d'utilisateur et un guid pour
    ''' mettre à jour les changements de la demande.
    ''' </summary>
    ''' <param name="demndCreation">Une demande de création d'utilisateur.</param>
    ''' <param name="dateEffective">Quand sera effective cette demande.</param>
    ''' <param name="guid">Indique l'entrée heat à mettre à jour. Laisser vide pour aucune</param>
    Public Shared Sub DemanderCreation(ByVal demndCreation As TsCdDemndCreationModif, ByVal dateEffective As Date, ByVal guid As String)
        If demndCreation.Mode = TsCdDemndCreationModif.TsDCMode.Modification Then Throw New TsExcErreurDemandeAjout
        If demndCreation.Utilisateur Is Nothing Then Throw New TsExcErreurGeneral("Il n'y a pas d'utilisateur à créer. Vous n'avez pas crée d'utilisateur avant de faire la demande.")
        If validerUtilisateur(demndCreation.Utilisateur) = False Then Throw New TsExcErreurGeneral("L'utilisateur est invalide.")

        '!--- Création de l'object Heat
        Dim heat As TsCdHeat = createHeatObject(guid)
        heat.FichierPieceJointe = demndCreation.PieceJointe
        heat.DateEffective = dateEffective

        RemplirHeatCreation(demndCreation, heat)
        EcrireChangementHeat(demndCreation, heat)
        heat.AppliquerChangement()
        '!---

        serialise(demndCreation, NOM_FICHIER_CREATION)
    End Sub

    ''' <summary>
    ''' Méthode partagée. Cette méthode prend une demande de modification et la 
    ''' transforme en format d'une fiche heat et l'envois au service de sécurité.
    ''' </summary>
    ''' <param name="demndModification">Une demande de modification des droits de rôles.</param>
    ''' <param name="dateEffective">Quand sera effective cette demande.</param>
    Public Shared Sub DemanderModification(ByVal demndModification As TsCdDemndCreationModif, ByVal dateEffective As Date)
        DemanderModification(demndModification, dateEffective, Nothing)
    End Sub

    ''' <summary>
    ''' Méthode partagée. Cette méthode prend une demande de modification et un guid pour
    ''' mettre à jour les changements de la demande.
    ''' </summary>
    ''' <param name="demndModification">Une demande de modification des droits de rôles.</param>
    ''' <param name="dateEffective">Quand sera effective cette demande.</param>
    ''' <param name="guid">Indique l'entrée heat à mettre à jour. Laisser vide pour aucune.</param>
    Public Shared Sub DemanderModification(ByVal demndModification As TsCdDemndCreationModif, ByVal dateEffective As Date, ByVal guid As String)
        If demndModification.Mode = TsCdDemndCreationModif.TsDCMode.Creation Then Throw New TsExcErreurDemandeModification
        If utilisateurAlterer(demndModification.Utilisateur, demndModification.UtilisateurOriginal) = True Then Throw New TsExcErreurGeneral("L'utilisateur a été altéré. Il ne correspond pas à l'utilisateur original.")

        '!--- Création de l'object Heat
        Dim heat As TsCdHeat = createHeatObject(guid)
        heat.FichierPieceJointe = demndModification.PieceJointe
        heat.DateEffective = dateEffective
        RemplirHeatModification(demndModification, heat)
        EcrireChangementHeat(demndModification, heat)

        heat.AppliquerChangement()
        '!---

        serialise(demndModification, NOM_FICHIER_MODIFICATION)
    End Sub

    ''' <summary>
    ''' Méthode partagée. Cette méthode prend une demande de destruction d'utilisateur et 
    ''' la transforme en format heat pour ensuit l'envoyer vers le service de sécurité.
    ''' </summary>
    ''' <param name="demndDestruction">Une demande de destruction.</param>
    ''' <param name="dateEffective">Quand sera effective cette demande.</param>
    Public Shared Sub DemanderDestruction(ByVal demndDestruction As TsCdDemandeDestruction, ByVal dateEffective As Date)
        DemanderDestruction(demndDestruction, dateEffective, Nothing)
    End Sub

    ''' <summary>
    ''' Méthode partagée. Cette méthode prend une demande de destruction d'utilisateur et un guid pour
    ''' mettre à jour les changements de la demande.
    ''' </summary>
    ''' <param name="demndDestruction">Une demande de destruction.</param>
    ''' <param name="dateEffective">Quand sera effective cette demande.</param>
    ''' <param name="guid">Indique l'entrée heat à mettre à jour. Laisser vide pour aucune.</param>
    Public Shared Sub DemanderDestruction(ByVal demndDestruction As TsCdDemandeDestruction, ByVal dateEffective As Date, ByVal guid As String)
        If demndDestruction.IDUtilisateur = "" Then Throw New TsExcErreurDemandeDestruction

        '!--- Création de l'object Heat
        Dim heat As TsCdHeat = createHeatObject(guid)
        heat.DateEffective = dateEffective
        RemplirHeatSuppression(demndDestruction, heat)
        heat.InfoDivers = "Détruire l'utilisateur: '" + demndDestruction.IDUtilisateur + "'"
        heat.AppliquerChangement()
        '!---

        serialise(demndDestruction, NOM_FICHIER_SUPPRESSION)
    End Sub

#End Region

    '!-----------------------------------
    '! Vieille méthode de demande de changement.
    '! Raison: Lorsque nous avons implanté Heat, les envois par courriels sont devenus inutilisés.
    '!-----------------------------------
#Region "Méthodes globals obselette"



    '''' <summary>
    '''' Cette Méthode va prendre la demande de création en entrée et la formater en version Courriel.
    '''' Elle sera ensuite expédié au service de sécurité.
    '''' </summary>
    '''' <param name="demndCreation">Ce paramettre doit être un nouvel utilisateur.</param>
    '''' <param name="dateEffective">Cette information sera directement et uniquement envoyé dans le courriel.</param>
    '''' <remarks></remarks>
    'Public Shared Sub DemanderCreationObselette(ByVal demndCreation As TsCdDemndCreationModif, ByVal dateEffective As Date)
    '    If demndCreation.Mode = TsCdDemndCreationModif.TsDCMode.Modification Then Throw New TsExcErreurDemandeAjout
    '    If demndCreation.Utilisateur Is Nothing Then Throw New TsExcErreurGeneral("Il n'y a pas d'utilisateur à créer. Vous n'avez pas créer d'utilisateur avant de faire la demande.")
    '    If validerUtilisateur(demndCreation.Utilisateur) = False Then Throw New TsExcErreurDemandeAjout

    '    '!--- Conception du email
    '    Dim operations As List(Of TsCdOperationRole)

    '    demndCreation.SimplifierOperations()
    '    operations = demndCreation.OperationsRoles

    '    Dim corpsMessage As New StringBuilder("")
    '    Dim sections As New StringBuilder("")
    '    Dim tmpString As String

    '    sections.AppendLine(ecrireSection(demndCreation.Utilisateur))

    '    If operations.Count > 0 Then sections.AppendLine(ecrireSection("Rôle(s) assigné(s) à l'utilisateur:", operations))
    '    '!-------------------

    '    '!-------------------
    '    '! Création du fichier XML.
    '    Dim nomFichier As String = NOM_FICHIER_CREATION + Date.Now.ToString("yyyy-MM-dd HH;mm;ss") + ".xml"
    '    Dim cheminAcces As String = Configuration.ValeurSysteme("TS7", "TS7N121\CheminAttente")

    '    nomFichier = cheminAcces + nomFichier
    '    serialiseCreation(demndCreation, nomFichier)
    '    '!-------------------

    '    '! Terminer le fichier HTML avec le nom du fichier lié.
    '    tmpString = My.Resources.Ts7TbHtmlPrincipal.Corps
    '    corpsMessage.AppendLine(String.Format(tmpString, "DEMANDE DE CRÉATION", sections.ToString, "<b>Date d'entrée en vigueur: </b>" + dateEffective.ToString("yyyy-MM-dd")))

    '    '! Ajustement et envois du courriel.
    '    Dim Email As New XuCuEnvoiCourriel()
    '    Dim configDestinataire As String = Configuration.ValeurSysteme("TS7", "TS7N121\EmailRespOperationelSecurite")
    '    Dim configExpediteur As String = Configuration.ValeurSysteme("TS7", "TS7N121\EmailSource")

    '    Email.FormatMessage = XuCuEnvoiCourriel.XuEcFormatMessage.XuEcFmHTML
    '    Email.Destinataire = configDestinataire
    '    Email.Expediteur = configExpediteur
    '    Email.Objet = "Demande d'ajout d'un nouvel utilisateur"
    '    Email.Message = corpsMessage.ToString

    '    Email.EnvoyerCourriel()

    'End Sub

    '''' <summary>
    '''' Cette méthode va prendre la demande de modification en entrée et la formater en version courriel.
    '''' Le courriel sera ensuite expédié au service de sécurité.
    '''' </summary>
    '''' <param name="demndModification">Ce paramettre doit avoir un utilisateur déja existant dans Sage.</param>
    '''' <param name="dateEffective">Cette information sera directement et uniquement envoyé dans le courriel.</param>
    '''' <remarks>Restriction: L'objet d'entrée doit avoir des modifications.</remarks>
    'Public Shared Sub DemanderModificationObselette(ByVal demndModification As TsCdDemndCreationModif, ByVal dateEffective As Date)
    '    If demndModification.Mode = TsCdDemndCreationModif.TsDCMode.Creation Then Throw New TsExcErreurDemandeModification
    '    If utilisateurAlterer(demndModification.Utilisateur, demndModification.UtilisateurOriginal) = True Then Throw New TsExcErreurGeneral("L'utilisateur a été altérer.Il ne conrespond pas à l'utilisateur original.")

    '    '!-------------------
    '    '! Conception du email
    '    Dim operations As IList(Of TsCdOperationRole)
    '    Dim ajouts As New List(Of TsCdOperationRole)
    '    Dim modifications As New List(Of TsCdOperationRole)
    '    Dim suppresions As New List(Of TsCdOperationRole)


    '    demndModification.SimplifierOperations()
    '    operations = demndModification.OperationsRoles

    '    For Each op As TsCdOperationRole In operations
    '        Select Case op.Operation
    '            Case TsCdOperationRole.TsSgaOperation.Ajout
    '                ajouts.Add(op)
    '            Case TsCdOperationRole.TsSgaOperation.Modification
    '                modifications.Add(op)
    '            Case TsCdOperationRole.TsSgaOperation.Suppression
    '                suppresions.Add(op)
    '        End Select
    '    Next
    '    Dim corpsMessage As New StringBuilder("")
    '    Dim sections As New StringBuilder("")
    '    Dim tmpString As String

    '    sections.AppendLine(ecrireSectionUtilstrModif(demndModification))

    '    If ajouts.Count > 0 Then sections.AppendLine(ecrireSection("Rôle(s) assigné(s) à l'utilisateur:", ajouts))
    '    If modifications.Count > 0 Then sections.AppendLine(ecrireSection("Rôle(s) modifié(s):", modifications))
    '    If suppresions.Count > 0 Then sections.AppendLine(ecrireSection("Rôle(s) désassigné(s) pour l'utilisateur:", suppresions))
    '    '!-------------------

    '    '!-------------------
    '    '! Création du fichier XML.
    '    Dim nomFichier As String = NOM_FICHIER_MODIFICATION + Date.Now.ToString("yyyy-MM-dd HH;mm;ss") + ".xml"
    '    Dim cheminAcces As String = Configuration.ValeurSysteme("TS7", "TS7N121\CheminAttente")

    '    nomFichier = cheminAcces + nomFichier
    '    serialiseCreation(demndModification, nomFichier)
    '    '!-------------------

    '    '! Terminer le fichier HTML avec le nom du fichier lié.
    '    tmpString = My.Resources.Ts7TbHtmlPrincipal.Corps
    '    corpsMessage.AppendLine(String.Format(tmpString, "DEMANDE DE MODIFICATION", sections.ToString, "<b>Date d'entrée en vigueur: </b>" + dateEffective.ToString("yyyy-MM-dd")))

    '    '! Ajustement et envois du courriel.
    '    Dim Email As New XuCuEnvoiCourriel()
    '    Dim configDestinataire As String = Configuration.ValeurSysteme("TS7", "TS7N121\EmailRespOperationelSecurite")
    '    Dim configExpediteur As String = Configuration.ValeurSysteme("TS7", "TS7N121\EmailSource")

    '    Email.FormatMessage = XuCuEnvoiCourriel.XuEcFormatMessage.XuEcFmHTML
    '    Email.Destinataire = configDestinataire
    '    Email.Expediteur = configExpediteur
    '    Email.Objet = "Demande de modification d'un utilisateur"
    '    Email.Message = corpsMessage.ToString

    '    Email.EnvoyerCourriel()
    'End Sub

    '''' <summary>
    '''' Cette Méthode va prendre la demande de Supression en entrée et la formater en version Courriel.
    '''' Elle sera ensuite expédié au service de sécurité.
    '''' </summary>
    '''' <param name="demndDestruction">Ce paramettre doit avoir un utilisateur déja existant dans Sage</param>
    '''' <param name="dateEffective">Cette information sera directement et uniquement envoyé dans le courriel.</param>
    '''' <remarks></remarks>
    'Public Shared Sub DemanderDestructionObselette(ByVal demndDestruction As TsCdDemandeDestruction, ByVal dateEffective As Date)
    '    If demndDestruction.IDUtilisateur = "" Then Throw New TsExcErreurDemandeDestruction

    '    '!-------------------
    '    '! Conception du email

    '    Dim corpsMessage As New StringBuilder("")
    '    Dim sections As New StringBuilder("")
    '    Dim tmpString As String

    '    Dim utilisateur As TsCdUtilisateur = TsCaServiceGestnAcces.ObtenirUtilisateur(demndDestruction.IDUtilisateur)

    '    sections.AppendLine(ecrireSectionDestruction(utilisateur))
    '    '!-------------------

    '    '!-------------------
    '    '! Création du fichier XML.
    '    Dim nomFichier As String = NOM_FICHIER_SUPPRESSION + Date.Now.ToString("yyyy-MM-dd HH;mm;ss") + ".xml"
    '    Dim cheminAcces As String = Configuration.ValeurSysteme("TS7", "TS7N121\CheminAttente")

    '    nomFichier = cheminAcces + nomFichier
    '    serialiseSuppression(demndDestruction, nomFichier)
    '    '!-------------------

    '    '! Terminer le fichier HTML avec le nom du fichier lié.
    '    tmpString = My.Resources.Ts7TbHtmlPrincipal.Corps
    '    corpsMessage.AppendLine(String.Format(tmpString, "DEMANDE DE SUPPRESSION", sections.ToString, "<b>Date d'entrée en vigueur: </b>" + dateEffective.ToString("yyyy-MM-dd")))

    '    '! Ajustement et envois du courriel.
    '    Dim Email As New XuCuEnvoiCourriel()
    '    Dim configDestinataire As String = Configuration.ValeurSysteme("TS7", "TS7N121\EmailRespOperationelSecurite")
    '    Dim configExpediteur As String = Configuration.ValeurSysteme("TS7", "TS7N121\EmailSource")

    '    Email.FormatMessage = XuCuEnvoiCourriel.XuEcFormatMessage.XuEcFmHTML
    '    Email.Destinataire = configDestinataire
    '    Email.Expediteur = configExpediteur
    '    Email.Objet = "Demande de suppression d'utilisateur"
    '    Email.Message = corpsMessage.ToString

    '    Email.EnvoyerCourriel()

    'End Sub

#End Region

#Region "Functions de services"

    ''' <summary>
    ''' Fonction de service. Remplis la section utilisateur du heat.
    ''' </summary>
    ''' <param name="utilisateur">L'utilisateur.</param>
    ''' <param name="heat">Le heat à remplir.</param>
    ''' <param name="demandeur">Le nom du demandeur du changement.</param>
    ''' <remarks></remarks>
    Private Shared Sub RemplirHeatUtilisateur(ByVal utilisateur As TsCdUtilisateur, ByVal heat As TsCdHeat, demandeur As String)

        Dim currentName As String = WindowsIdentity.GetCurrent().Name.Split("\".ToCharArray()).Last()
        Dim infoUtilisateurAD As TsCuUtilisateurAD = tsCuObtnrInfoAD.ObtenirUtilisateur(currentName)

        With utilisateur
            heat.Reference = TexteHeatReferencePrefixe & infoUtilisateurAD.NomComplet
            heat.NomEmploye = .Nom
            heat.PrenomEmploye = .Prenom
            heat.Id = .ID
            heat.DateFin = .DateFin
            heat.NomDemandeur = demandeur
        End With
    End Sub

    ''' <summary>
    ''' Fonction de service. Remplit les champs Heat essentiels à partir des informations de la demande de création.
    ''' </summary>
    ''' <param name="demndCreation">La demande de création qui contient les informations.</param>
    ''' <param name="heat">L'objet heat qui sera remplis des informations de la demande.</param>
    Private Shared Sub RemplirHeatCreation(ByVal demndCreation As TsCdDemndCreationModif, ByVal heat As TsCdHeat)
        RemplirHeatUtilisateur(demndCreation.Utilisateur, heat, demndCreation.NomDemandeur)

        heat.NouveauUA = demndCreation.Utilisateur.NoUniteAdmin
        heat.Mouvement = TexteHeatNouveau
        heat.SousCategorie = TexteHeatSousCategorie
    End Sub

    ''' <summary>
    ''' Fonction de service. Remplit les champs Heat essentiels à partir des informations de la demande de modification.
    ''' </summary>
    ''' <param name="demndModification">La demande de modification qui contient les informations.</param>
    ''' <param name="heat">L'objet heat qui sera remplis des informations de la demande.</param>
    Private Shared Sub RemplirHeatModification(ByVal demndModification As TsCdDemndCreationModif, ByVal heat As TsCdHeat)
        Dim indicateurChangementUtilisateur As Boolean = False
        RemplirHeatUtilisateur(demndModification.Utilisateur, heat, demndModification.NomDemandeur)

        heat.Mouvement = TexteHeatChangement
        heat.SousCategorie = TexteHeatSousCategorie
        heat.Organisation = demndModification.Organisation
        heat.AncienUA = demndModification.UtilisateurOriginal.NoUniteAdmin
        If demndModification.Utilisateur.NoUniteAdmin <> demndModification.UtilisateurOriginal.NoUniteAdmin Then
            heat.NouveauUA = demndModification.Utilisateur.NoUniteAdmin
        End If
    End Sub

    ''' <summary>
    ''' Fonction de service. Remplit les champs heat essentiel à partir des informations de la demande de destruction.
    ''' </summary>
    ''' <param name="demndSupp">La demande de destruction qui contient les informations.</param>
    ''' <param name="heat">L'objet heat qui sera remplis des informations de la demande.</param>
    Private Shared Sub RemplirHeatSuppression(ByVal demndSupp As TsCdDemandeDestruction, ByVal heat As TsCdHeat)
        Dim utilisateur As TsCdUtilisateur = TsCdUtilisateur.TraductionUtilisateur(TsCuAccesSage.ObtenirUtilisateur(demndSupp.IDUtilisateur))
        RemplirHeatUtilisateur(utilisateur, heat, demndSupp.NomDemandeur)

        heat.Mouvement = TexteHeatSuppression
        heat.SousCategorie = TexteHeatSousCategorie
        heat.NouveauUA = utilisateur.NoUniteAdmin
    End Sub

    ''' <summary>
    ''' Fonction de service. Prend une demande de changements en mode création ou en mode modification pour en extraire les changements à insérer dans une fiche heat.
    ''' </summary>
    ''' <param name="demande">Un demande à analyser.</param>
    ''' <param name="heat">Une fiche heat à remplir.</param>
    Private Shared Sub EcrireChangementHeat(ByVal demande As TsCdDemndCreationModif, ByVal heat As TsCdHeat)
        Dim infoDivers As New StringBuilder
        Dim lstOperations As List(Of TsCdOperationRole)

        demande.SimplifierOperations()
        lstOperations = demande.OperationsRoles

        '!--- Changment de l'utilisateur
        Dim u As TsCdUtilisateur = demande.Utilisateur
        Dim uOri As TsCdUtilisateur = demande.UtilisateurOriginal

        If uOri IsNot Nothing Then
            If u.ID <> "" Then
                infoDivers.AppendLine(String.Format("**Compte sélectionné : {0} **", u.ID))
                infoDivers.AppendLine()
            End If

            If u.DateFin <> uOri.DateFin Then
                If u.FinPrevue = False Then
                    infoDivers.AppendLine(" - Il n'y a plus de date de fin de contrat.")
                    infoDivers.AppendLine()
                Else
                    infoDivers.AppendLine("- La date de fin de contrat a été changé pour: " + u.DateFin.ToString(FORMAT_DATE))
                    infoDivers.AppendLine()
                End If
            End If
            If u.NoUniteAdmin <> uOri.NoUniteAdmin Then
                infoDivers.AppendLine("- Le numéro de l'unité administrative de l'utilisateur a été changé pour: " + u.NoUniteAdmin)
                infoDivers.AppendLine()
            End If
            If u.DateApprobation <> uOri.DateApprobation Then
                If u.ApprobationAccepter = False Then
                    infoDivers.AppendLine(" - Il n'y a plus de date d'approbation.")
                    infoDivers.AppendLine()
                Else
                    infoDivers.AppendLine("- La date d'approbation a été changé pour: " + u.DateFin.ToString(FORMAT_DATE))
                    infoDivers.AppendLine()
                End If
            End If
        End If
        '!---

        If lstOperations.Count > 1 Then
            infoDivers.AppendLine("Voici les changements dans les rôles: ")
        End If

        For Each op As TsCdOperationRole In lstOperations
            Select Case op.Operation
                Case TsCdOperationRole.TsSgaOperation.Ajout
                    infoDivers.AppendLine("- Ajout du rôle: ")
                Case TsCdOperationRole.TsSgaOperation.Modification
                    infoDivers.AppendLine("- Modification du rôle: ")
                Case TsCdOperationRole.TsSgaOperation.Suppression
                    infoDivers.AppendLine("- Suppression du rôle: ")
            End Select
            infoDivers.AppendLine("  Code du rôle: " + op.IdRole)
            infoDivers.AppendLine("  Nom: " + op.Nom)
            If op.FinPrevue = True Then infoDivers.AppendLine("  Date de fin d'assignation: " + op.DateFin.ToString(FORMAT_DATE))
        Next

        If Not String.IsNullOrEmpty(demande.ModifComptesSupp) Then
            infoDivers.AppendLine("Modification aux Comptes Supplémentaires :")
            infoDivers.AppendLine(demande.ModifComptesSupp)
        End If

        If Not String.IsNullOrEmpty(demande.ValidComptesSupp) Then
            infoDivers.AppendLine(demande.ValidComptesSupp)
        End If

        If Not String.IsNullOrEmpty(demande.TexteLibre) Then
            infoDivers.AppendLine("  Remarque : ")
            infoDivers.AppendLine(demande.TexteLibre)
        End If

        heat.InfoDivers = infoDivers.ToString
    End Sub

    ''' <summary>
    ''' Sert à identifier si l'utilisateur a été changé dans le mode utilisation.
    ''' </summary>
    ''' <param name="utlisateur"></param>
    ''' <param name="utilisateurOriginal"></param>
    ''' <returns></returns>
    ''' <remarks>Les champs: NoUniteAdmin, finPrevu, dateFin sont des champs qui peuvent être altéré.</remarks>
    Private Shared Function utilisateurAlterer(ByVal utlisateur As TsCdUtilisateur, ByVal utilisateurOriginal As TsCdUtilisateur) As Boolean
        If utlisateur.Courriel <> utilisateurOriginal.Courriel Then Return True
        If utlisateur.ID <> utilisateurOriginal.ID Then Return True
        If utlisateur.Nom <> utilisateurOriginal.Nom Then Return True
        'C'est normal en cas de changement d'unité, le nom complet change aussi
        'If utlisateur.NomComplet <> utilisateurOriginal.NomComplet Then Return True
        If utlisateur.Prenom <> utilisateurOriginal.Prenom Then Return True
        If utlisateur.Ville <> utilisateurOriginal.Ville Then Return True

        Return False
    End Function

    ''' <summary>
    ''' Valide si l'utilisateur est conforme avec des champs valide
    ''' </summary>
    Private Shared Function validerUtilisateur(ByVal pUtilisateur As TsCdUtilisateur) As Boolean
        Dim patternVide As New Regex("^ *$")
        Dim patternCouriel As New Regex("^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$")

        If patternVide.IsMatch(pUtilisateur.Nom) = True Then Return False

        If patternVide.IsMatch(pUtilisateur.Prenom) = True Then Return False

        If patternCouriel.IsMatch(pUtilisateur.Courriel) = False Then Return False

        If patternVide.IsMatch(pUtilisateur.Ville) = True Then Return False

        If patternVide.IsMatch(pUtilisateur.NoUniteAdmin) = True Then Return False

        If pUtilisateur.FinPrevue = True Then
            If pUtilisateur.DateFin < Date.Now Then Return False
        End If

        Return True
    End Function

    Private Shared Sub serialise(ByVal demande As Object, ByVal prefixeFichier As String)
        '!--- Création du fichier XML
        Dim nomFichier As String = String.Format("{0}{1}.xml", Path.Combine(Configuration.ValeurSysteme("TS7", "TS7N121\CheminAttente"), prefixeFichier), Date.Now.ToString("yyyy-MM-dd HH;mm;ss"))

        Dim mySerializer As XmlSerializer = New System.Xml.Serialization.XmlSerializer(demande.GetType())
        Dim fichierInfo As New FileInfo(nomFichier)
        If My.Computer.FileSystem.DirectoryExists(fichierInfo.DirectoryName) = False Then
            My.Computer.FileSystem.CreateDirectory(fichierInfo.DirectoryName)
        End If
        Dim myWriter As System.IO.StreamWriter = New System.IO.StreamWriter(nomFichier)

        mySerializer.Serialize(myWriter, demande)

        myWriter.Close()
    End Sub

    ''' <summary>
    ''' Écrit une section pour une demande de destruction.
    ''' </summary>
    Private Shared Function ecrireSectionDestruction(ByVal utilisateur As TsCdUtilisateur) As String
        Dim corpsMessage As New System.Text.StringBuilder("")

        Dim sections As New System.Text.StringBuilder("")
        Dim sousSections As New System.Text.StringBuilder("")
        Dim tmpString As String

        tmpString = My.Resources.Ts7TbHtmlPrincipal.CleValeur
        sousSections.AppendLine(String.Format(tmpString, "Code utilisateur:", utilisateur.ID))
        If utilisateur.NomComplet <> "" Then
            sousSections.AppendLine(String.Format(tmpString, "Nom:", utilisateur.NomComplet))
        Else
            sousSections.AppendLine(String.Format(tmpString, "Nom:", utilisateur.Prenom + " " + utilisateur.Nom))
        End If
        sousSections.AppendLine(String.Format(tmpString, "Unité administrative:", utilisateur.NoUniteAdmin))

        tmpString = My.Resources.Ts7TbHtmlPrincipal.SousSection
        sections.AppendLine(String.Format(tmpString, sousSections))
        tmpString = My.Resources.Ts7TbHtmlPrincipal.Section
        corpsMessage.AppendLine(String.Format(tmpString, "Information sur l'utilisateur:"))
        corpsMessage.Append(sections.ToString)

        Return corpsMessage.ToString
    End Function

    ''' <summary>
    ''' Écrit une section d'utilisateur en mode modification.
    ''' </summary>
    Private Shared Function ecrireSectionUtilstrModif(ByVal demandeModif As TsCdDemndCreationModif) As String
        Dim utilisateur As TsCdUtilisateur = demandeModif.Utilisateur
        Dim utilisateurOri As TsCdUtilisateur = demandeModif.UtilisateurOriginal

        Dim corpsMessage As New System.Text.StringBuilder("")

        Dim sections As New System.Text.StringBuilder("")
        Dim sousSections As New System.Text.StringBuilder("")
        Dim tmpString As String

        tmpString = My.Resources.Ts7TbHtmlPrincipal.CleValeur
        sousSections.AppendLine(String.Format(tmpString, "Code utilisateur:", utilisateur.ID))
        If (utilisateur.NomComplet = "") Then
            sousSections.AppendLine(String.Format(tmpString, "Nom:", utilisateur.Nom))
            sousSections.AppendLine(String.Format(tmpString, "Prenom:", utilisateur.Prenom))
        Else
            sousSections.AppendLine(String.Format(tmpString, "Nom:", utilisateur.NomComplet))
        End If
        If utilisateur.Courriel <> "" Then sousSections.AppendLine(String.Format(tmpString, "Courriel:", utilisateur.Courriel))
        If utilisateur.Ville <> "" Then sousSections.AppendLine(String.Format(tmpString, "Ville:", utilisateur.Ville))



        '! Champs Modifiables-----------------
        If utilisateur.NoUniteAdmin <> utilisateurOri.NoUniteAdmin Then
            Dim texte As String = "<B><font color=red>L'unité administrative a été modifié:</font><B>"
            sousSections.AppendLine(String.Format(tmpString, texte, utilisateur.NoUniteAdmin))
        Else
            Dim texte As String = "Unité administrative:"
            sousSections.AppendLine(String.Format(tmpString, texte, utilisateur.NoUniteAdmin))
        End If

        If utilisateur.FinPrevue = utilisateurOri.FinPrevue Then
            If utilisateur.FinPrevue = True Then
                Dim texte As String = "Date de fin de contrat:"
                sousSections.AppendLine(String.Format(tmpString, texte, utilisateur.DateFin.ToString("yyyy-MM-dd")))
            End If
        Else
            If utilisateur.FinPrevue = True Then
                Dim texte As String = "<B><font color=red>La date de fin de contrat a été modifiée:</font><B>"
                sousSections.AppendLine(String.Format(tmpString, texte, utilisateur.DateFin.ToString("yyyy-MM-dd")))
            Else
                Dim texte As String = "<B><font color=red>La date de fin de contrat a été modifiée:</font><B>"
                Dim texte2 As String = "Il n'y a plus de date de fin de contrat."
                sousSections.AppendLine(String.Format(tmpString, texte, texte2))
            End If
        End If

        If utilisateur.ApprobationAccepter = utilisateurOri.ApprobationAccepter Then
            If utilisateur.ApprobationAccepter = True Then
                Dim texte As String = "Approbation:"
                sousSections.AppendLine(String.Format(tmpString, texte, utilisateur.DateApprobation.ToString("yyyy-MM-dd")))
            End If
        Else
            If utilisateur.ApprobationAccepter = True Then
                Dim texte As String = "<B><font color=red>Date d'approbation de rôle:</font><B>"
                sousSections.AppendLine(String.Format(tmpString, texte, utilisateur.DateApprobation.ToString("yyyy-MM-dd")))
            Else
                Dim texte As String = "<B><font color=red>Approbation:</font><B>"
                Dim texte2 As String = "L'utilisateur n'a plus d'approbation."
                sousSections.AppendLine(String.Format(tmpString, texte, texte2))
            End If
        End If

        '! -----------------------------------
        tmpString = My.Resources.Ts7TbHtmlPrincipal.SousSection
        sections.AppendLine(String.Format(tmpString, sousSections))
        tmpString = My.Resources.Ts7TbHtmlPrincipal.Section
        corpsMessage.AppendLine(String.Format(tmpString, "Information sur l'utilisateur:"))
        corpsMessage.Append(sections.ToString)

        Return corpsMessage.ToString
    End Function

    ''' <summary>
    ''' Écrit une section d'utilisateur.
    ''' </summary>
    Private Shared Function ecrireSection(ByVal utilisateur As TsCdUtilisateur) As String
        Dim corpsMessage As New System.Text.StringBuilder("")

        Dim sections As New System.Text.StringBuilder("")
        Dim sousSections As New System.Text.StringBuilder("")
        Dim tmpString As String

        tmpString = My.Resources.Ts7TbHtmlPrincipal.CleValeur
        sousSections.AppendLine(String.Format(tmpString, "Code utilisateur:", utilisateur.ID))
        If (utilisateur.NomComplet = "") Then
            sousSections.AppendLine(String.Format(tmpString, "Nom:", utilisateur.Nom))
            sousSections.AppendLine(String.Format(tmpString, "Prenom:", utilisateur.Prenom))
        Else
            sousSections.AppendLine(String.Format(tmpString, "Nom:", utilisateur.NomComplet))
        End If
        If utilisateur.Courriel <> "" Then sousSections.AppendLine(String.Format(tmpString, "Courriel:", utilisateur.Courriel))
        If utilisateur.Ville <> "" Then sousSections.AppendLine(String.Format(tmpString, "Ville:", utilisateur.Ville))
        If utilisateur.NoUniteAdmin <> "" Then sousSections.AppendLine(String.Format(tmpString, "Unité administrative:", utilisateur.NoUniteAdmin))
        If utilisateur.FinPrevue = True Then sousSections.AppendLine(String.Format(tmpString, "Date de fin de contrat:", utilisateur.DateFin.ToString("yyyy-MM-dd")))

        If utilisateur.ApprobationAccepter = True Then
            sousSections.AppendLine(String.Format(tmpString, "Date d'approbation de rôles: ", utilisateur.DateApprobation.ToString("yyyy-MM-dd")))
        End If


        tmpString = My.Resources.Ts7TbHtmlPrincipal.SousSection
        sections.AppendLine(String.Format(tmpString, sousSections))
        tmpString = My.Resources.Ts7TbHtmlPrincipal.Section
        corpsMessage.AppendLine(String.Format(tmpString, "Information sur l'utilisateur:"))
        corpsMessage.Append(sections.ToString)

        Return corpsMessage.ToString
    End Function

    ''' <summary>
    ''' Écrit une section de rôles.
    ''' </summary>
    Private Shared Function ecrireSection(ByVal titreEntete As String, ByVal operationRoles As List(Of TsCdOperationRole)) As String
        Dim corpsMessage As New System.Text.StringBuilder("")
        Dim sousSection As System.Text.StringBuilder
        Dim sections As New System.Text.StringBuilder("")
        Dim tmpString As String

        For Each element As TsCdOperationRole In operationRoles
            sousSection = New System.Text.StringBuilder("")
            Select Case element.Operation
                Case TsCdOperationRole.TsSgaOperation.Ajout
                    sousSection.Append(ecrireAjout(element))
                Case TsCdOperationRole.TsSgaOperation.Modification
                    sousSection.Append(ecrireAjout(element))
                Case TsCdOperationRole.TsSgaOperation.Suppression
                    sousSection.Append(ecrireAjout(element))
            End Select
            tmpString = My.Resources.Ts7TbHtmlPrincipal.SousSection
            sections.AppendLine(String.Format(tmpString, sousSection))
        Next

        tmpString = My.Resources.Ts7TbHtmlPrincipal.Section
        corpsMessage.AppendLine(String.Format(tmpString, titreEntete))
        corpsMessage.Append(sections.ToString)

        Return corpsMessage.ToString
    End Function

    ''' <summary>
    ''' Écrit une sous-section de rôles ajoutés.
    ''' </summary>
    Private Shared Function ecrireAjout(ByVal operationRole As TsCdOperationRole) As String
        Dim corpsMessage As New System.Text.StringBuilder("")
        Dim tmpString As String

        tmpString = My.Resources.Ts7TbHtmlPrincipal.CleValeur
        corpsMessage.AppendLine(String.Format(tmpString, "Code du rôle:", operationRole.IdRole))
        corpsMessage.Append(String.Format(tmpString, "Nom:", operationRole.Nom))
        If operationRole.FinPrevue = True Then
            corpsMessage.AppendLine()
            corpsMessage.Append(String.Format(tmpString, "Date fin d'assignation:", operationRole.DateFin.ToString("yyyy-MM-dd")))
        End If

        Return corpsMessage.ToString
    End Function

    ''' <summary>
    ''' Écrit une sous-section de rôles modifiés.
    ''' </summary>
    Private Shared Function ecrireModification(ByVal operationRole As TsCdOperationRole) As String
        Dim corpsMessage As New System.Text.StringBuilder("")
        Dim tmpString As String

        tmpString = My.Resources.Ts7TbHtmlPrincipal.CleValeur
        corpsMessage.AppendLine(String.Format(tmpString, "Code du rôle:", operationRole.IdRole))
        corpsMessage.Append(String.Format(tmpString, "Nom:", operationRole.Nom))
        If operationRole.FinPrevue = True Then
            corpsMessage.AppendLine()
            corpsMessage.Append(String.Format(tmpString, "Date fin d'assignation:", operationRole.DateFin.ToString("yyyy-MM-dd")))
        Else
            corpsMessage.AppendLine()
            corpsMessage.Append(String.Format(tmpString, "Ce Rôle n'est plus limité par une fin d'assignation.", ""))
        End If

        Return corpsMessage.ToString
    End Function

    ''' <summary>
    ''' Écrit une sous-section de rôles supprimés.
    ''' </summary>
    Private Shared Function ecrireSupression(ByVal operationRole As TsCdOperationRole) As String
        Dim corpsMessage As New System.Text.StringBuilder("")
        Dim tmpString As String

        tmpString = My.Resources.Ts7TbHtmlPrincipal.CleValeur
        corpsMessage.AppendLine(String.Format(tmpString, "Code du rôle:", operationRole.IdRole))
        corpsMessage.Append(String.Format(tmpString, "Nom:", operationRole.Nom))

        Return corpsMessage.ToString
    End Function

    ''' <summary>
    ''' Instancie l'object TsCdHeat
    ''' </summary>
    ''' <param name="guid">Peut être vide pour signifier aucune</param>
    ''' <returns>a TsCdHeat object</returns>
    ''' <remarks></remarks>
    Private Shared Function createHeatObject(ByVal guid As String) As TsCdHeat
        '!--- Création de l'object Heat
        Dim heat As TsCdHeat = Nothing
        If String.IsNullOrEmpty(guid) Then
            Return New TsCdHeat()
        Else
            Return New TsCdHeat(guid)
        End If
    End Function

#End Region

#Region "Paramètres"

    Private Shared ReadOnly Property TexteHeatNouveau() As String
        Get
            Return Configuration.ValeurSysteme("TS7", "TS7N121\HEAT-Nouveau")
        End Get
    End Property

    Private Shared ReadOnly Property TexteHeatChangement() As String
        Get
            Return Configuration.ValeurSysteme("TS7", "TS7N121\HEAT-Modifier")
        End Get
    End Property

    Private Shared ReadOnly Property TexteHeatSuppression() As String
        Get
            Return Configuration.ValeurSysteme("TS7", "TS7N121\HEAT-Supprimer")
        End Get
    End Property

    Private Shared ReadOnly Property TexteHeatSousCategorie() As String
        Get
            Return Configuration.ValeurSysteme("TS7", "TS7N121\HEAT-Sous-Categorie")
        End Get
    End Property

    Private Shared ReadOnly Property TexteHeatReferencePrefixe() As String
        Get
            Return Configuration.ValeurSysteme("TS7", "TS7N121\HEAT-Reference-Prefixe")
        End Get
    End Property

#End Region
End Class
