Imports Rrq.InfrastructureCommune.Parametres
Imports System.Data.SqlClient
Imports Rrq.Securite.GestionAcces.TsCdHeat.ModeAcces
Imports Rrq.InfrastructureCommune.UtilitairesCommuns
Imports System.Text
Imports System.IO

''' <summary>
''' Classe de données et de communication pour les intéraction avec Heat.
''' </summary>
''' <History>
''' Historique des modifications: 
''' ---------------------------------------------------------------------------------------------------------- 
''' Demande    Date		   Nom			        Description 
''' ---------------------------------------------------------------------------------------------------------- 
''' [------]   2013-08-12  Martin Bellemare     Modification: Lorsque la banque Heat n'a pas subit de changement
'''                                             un courriel est envoyé à l'info-boutique au lieu de lancé une erreur fatal.
''' </History>
Public Class TsCdHeat

#Region "Constantes"

    Private Const FORMAT_DATE As String = "yyyy-MM-dd"

#End Region

#Region "Enums"

    Enum ModeAcces
        AvecGuid
        SansGuid
    End Enum

#End Region

#Region "Variables privées"

    Private _DateFin As String
    Private _DateEffective As String
    Private _DateRetour As String

    Private mode As ModeAcces

    Private _guid As String

#End Region

#Region "--- Variables publiques ---"

    Public Reference As String
    Public NomEmploye As String
    Public PrenomEmploye As String
    Public Id As String
    Public Categorie As String
    Public Compagnie As String
    Public Mouvement As String
    Public AncienUA As String
    Public AncienPort As String
    Public AncienTelephone As String
    Public AncienPoste As String
    Public NouveauUA As String
    Public NouveauPort As String
    Public NouveauTelephone As String
    Public NouveauPoste As String
    Public Transaction As String
    Public NoRRQ As String
    Public InfoDivers As String
    Public ImprimanteBureautique As String
    Public ImprimanteCentral As String
    Public AutreLogiciel As String
    Public Commentaire As String
    Public SousCategorie As String
    Public GuidRRQ As String
    Public Organisation As String

#End Region

#Region "--- Propriétés ---"

    ''' <summary>
    ''' Nom de la personne ayant fait la demande de changement.
    ''' </summary>
    ''' <remarks></remarks>
    Public Property NomDemandeur As String


    ''' <summary>
    ''' Changer ou obtenir la date de la fin de contrat.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DateFin() As Date
        Get
            Return Date.Parse(_DateFin)
        End Get
        Set(ByVal value As Date)
            If value = Nothing Then
                _DateFin = Nothing
            Else
                _DateFin = value.ToString(FORMAT_DATE)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Changer ou obtenir la date de retour au travail.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DateRetour() As Date
        Get
            Return Date.Parse(_DateRetour)
        End Get
        Set(ByVal value As Date)
            _DateRetour = value.ToString(FORMAT_DATE)
        End Set
    End Property

    ''' <summary>
    ''' Changer ou obtenir la date d'entrée en vigueur.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DateEffective() As Date
        Get
            Return Date.Parse(_DateEffective)
        End Get
        Set(ByVal value As Date)
            _DateEffective = value.ToString(FORMAT_DATE)
        End Set
    End Property

    Public Property FichierPieceJointe As TsCuFichierPieceJointe

#End Region

#Region "Constructeurs"

    ''' <summary>
    ''' Constructeur d'une nouvelle requête.
    ''' </summary>
    Public Sub New()
        mode = SansGuid
    End Sub

    ''' <summary>
    ''' Constructeur d'une modification d'une requête déja envoyée.
    ''' </summary>
    Public Sub New(ByVal guid As String)
        mode = AvecGuid
        _guid = guid
    End Sub

#End Region

#Region "Méthodes"

    ''' <summary>
    ''' Méthode permettant d'actriver l'envois des informations.
    ''' </summary>
    ''' <remarks>
    ''' Deux mode:
    ''' - Mode sans guid: déposera un fichier dans le dossier de ticket générator.
    ''' - Mode avec guid: changera par appel sql les information d'une demande déja envoyé.
    ''' </remarks>
    Public Sub AppliquerChangement()

        Select Case mode
            Case AvecGuid
                AppliquerChangementAvecGuid()
            Case SansGuid
                AppliquerChangementSansGuid()
        End Select

    End Sub

#End Region

#Region "Fonctions de services"

    ''' <summary>
    ''' Fonction de service. Action à prendre en mode avec guid.
    ''' </summary>
    ''' <exception cref="ApplicationException">L'appel sql a changé plus d'une ligne. Les opérations seront annulés.</exception>
    ''' <exception cref="ApplicationException">Le guid est inexistant dans la Base de Données.</exception>
    Private Sub AppliquerChangementAvecGuid()
        Dim cheminDepot As String = XuCuConfiguration.ValeurSysteme("TS7", "TS7N121\CheminDepotHeatAvecGuid")

        'If My.Computer.FileSystem.FileExists(cheminDepot) = False Then
        '    My.Computer.FileSystem.CreateDirectory(cheminDepot)
        'End If

        Dim nomFichierTS7 As String = String.Concat(cheminDepot, "\TS7_", _guid, ".txt")

        Dim nouveauNomPieceJointe As String = SauvegarderPieceJointe(FichierPieceJointe)
        Dim utilisateur As String = tsCuObtnrInfoAD.ObtenirUtilisateur().CodeUtilisateur

        Dim fichierSortie As New IO.StreamWriter(nomFichierTS7, False, System.Text.Encoding.GetEncoding("iso-8859-1"))
        Dim lstOrdonner() As String = {_guid, utilisateur, _DateEffective, ObtenirTexteDemande(), nouveauNomPieceJointe}

        Dim i As Integer = 0
        For Each elem As String In lstOrdonner
            If elem <> Nothing Then
                Dim chaineTexte As String = elem.Replace(vbCr, "\R").Replace(vbLf, "\N")
                fichierSortie.Write(chaineTexte)
            End If
            i += 1
            If i < lstOrdonner.Length Then
                fichierSortie.Write("|")
            End If
        Next

        fichierSortie.Flush()
        fichierSortie.Close()
    End Sub

    ''' <summary>
    ''' Fonction de service. Action à prendre en mode sans guid.
    ''' </summary>
    Private Sub AppliquerChangementSansGuid()
        Dim cheminDepot As String = XuCuConfiguration.ValeurSysteme("TS7", "TS7N121\CheminDepotHeat")

        Dim nomNumero As Integer = 1

        If My.Computer.FileSystem.FileExists(cheminDepot) = False Then
            My.Computer.FileSystem.CreateDirectory(cheminDepot)
        End If

        While My.Computer.FileSystem.FileExists(cheminDepot + "\Auth" + nomNumero.ToString + ".txt") And nomNumero <= 100
            nomNumero += 1
        End While
        If nomNumero > 100 Then
            Throw New ApplicationException("Tout les noms du fichier Heat sont déjà tous utilisés.")
        End If

        Dim nomGestionnaire As String = String.Empty
        Dim noContrat As String = String.Empty
        Dim dateDebutContrat As String = String.Empty
        Dim dateFinContrat As String = String.Empty
        Dim noUniteAdminResp As String = String.Empty
        Dim nouveauNomPieceJointe As String = SauvegarderPieceJointe(FichierPieceJointe)

        Dim fichierSortie As New IO.StreamWriter(cheminDepot + "\Auth" + nomNumero.ToString + ".txt", False, System.Text.Encoding.GetEncoding("iso-8859-1"))
        Dim lstOrdonner() As String = {Reference, NomEmploye, PrenomEmploye, Id,
                                       Categorie, Compagnie, _DateFin, Mouvement, _DateRetour,
                                       AncienUA, AncienPort, AncienTelephone, AncienPoste,
                                       NouveauUA, NouveauPort, NouveauTelephone, NouveauPoste,
                                       Transaction, NoRRQ, InfoDivers, ImprimanteBureautique, ImprimanteCentral,
                                       AutreLogiciel, Commentaire, _DateEffective, SousCategorie, GuidRRQ,
                                       nomGestionnaire, noContrat, dateDebutContrat, dateFinContrat, noUniteAdminResp, nouveauNomPieceJointe, Organisation}

        Dim i As Integer = 0
        For Each elem As String In lstOrdonner
            If elem <> Nothing Then
                Dim chaineTexte As String = elem.Replace(vbCr, "\R").Replace(vbLf, "\N")
                fichierSortie.Write(chaineTexte)
            End If
            i += 1
            If i < lstOrdonner.Length Then
                fichierSortie.Write("|")
            End If
        Next

        fichierSortie.Flush()
        fichierSortie.Close()
    End Sub

    Private Function SauvegarderPieceJointe(fichier As TsCuFichierPieceJointe) As String
        If fichier Is Nothing OrElse fichier.NouveauNomFichier Is Nothing Then Return String.Empty

        If Not String.IsNullOrEmpty(fichier.CheminDepot) Then
            If My.Computer.FileSystem.FileExists(fichier.CheminDepot) = False Then
                My.Computer.FileSystem.CreateDirectory(fichier.CheminDepot)
            End If
        End If

        File.WriteAllBytes(fichier.NouveauNomFichier, fichier.Contenu)

        Return fichier.NouveauNomFichier
    End Function

    ''' <summary>
    ''' Fonction de service. Renvois le texte de demande à envoyer par SQL.
    ''' </summary>
    ''' <returns>Texte de demande.</returns>
    Private Function ObtenirTexteDemande() As String
        If InfoDivers = Nothing Then
            Return " "
        Else
            Return InfoDivers
        End If
    End Function

    ''' <summary>
    ''' Permet d'envoyé la modification Heat par courriel.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EnvoyerCourriel()
        Dim expediteur As New XuCuEnvoiCourriel()

        Dim texte As New StringBuilder()
        With texte

            If String.IsNullOrEmpty(NomDemandeur) Then
                .AppendLine(String.Format("Le service TS7N121 n'a pu inscrire les modifications aux rôles sur la fiche Heat correspondant au Mouvement de personnel de {0} {1}.<BR>", PrenomEmploye, NomEmploye))
            Else
                .AppendLine(String.Format("Le service TS7N121 n'a pu inscrire les modifications aux rôles sur la fiche Heat correspondant au Mouvement de personnel de {0} {1}, envoyé par {2}.<BR>", PrenomEmploye, NomEmploye, NomDemandeur))
            End If

            .AppendLine("<BR>")
            .AppendLine(String.Format("Svp compléter manuellement les informations ci-dessous de la fiche, lorsqu'elle sera créée par le traitement automatisé (Calllog.GUIDRRQ = {0}).<BR>", _guid))
            .AppendLine("<BR>")
            .AppendLine(String.Format(" - Date de saisie = {0}<BR>", _DateEffective))
            .AppendLine(String.Format(" - Modifications du bloc Autorisations d'accès: <Table border=""1""><TR><TD><pre>{0}</pre></TD></TR></Table>", ObtenirTexteDemande()))
        End With

        With expediteur
            .FormatMessage = XuCuEnvoiCourriel.XuEcFormatMessage.XuEcFmHTML
            .Destinataire = ObtenirAdressesDestinataire()
            .CopieConforme = ObtenirAdressesCopiesConformes()
            .Objet = "Erreur dans la gestion des accès."
            .Message = texte.ToString
            .Expediteur = ObtenirExpediteur()
            .EnvoyerCourriel()
        End With
    End Sub

    ''' <summary>
    ''' Permet d'obtenir du fichier config TS7.config les adresses des destinataires.
    ''' </summary>
    ''' <returns>Les adresses des destinataires séparer par des virgules.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirAdressesDestinataire() As String
        Dim lstCles As String() = XuCuConfiguration.ClefsSysteme("TS7", "TS7\TS7N121\AdressesCourriels\A\")

        Dim adresses As String = ""
        For Each cle As String In lstCles
            If adresses <> "" Then adresses &= ", "
            adresses &= XuCuConfiguration.ValeurSysteme("TS7", cle)

        Next

        Return adresses
    End Function

    ''' <summary>
    ''' Permet d'obtenir du fichier config TS7.config les adresses des copies conformes.
    ''' </summary>
    ''' <returns>Les adresses des destinataires séparer par des virgules.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirAdressesCopiesConformes() As String
        Try
            Dim lstCles As String() = XuCuConfiguration.ClefsSysteme("TS7", "TS7\TS7N121\AdressesCourriels\CC\")

            Dim adresses As String = ""
            For Each cle As String In lstCles
                If adresses <> "" Then adresses &= ", "
                adresses &= XuCuConfiguration.ValeurSysteme("TS7", cle)
            Next

            Return adresses
        Catch ex As XuExcCClefExistePas
            Return ""
        End Try
    End Function

    ''' <summary>
    ''' Permet d'obtenir du fichier config TS7.config les adresses des destinataires.
    ''' </summary>
    ''' <returns>Les adresses des destinataires séparer par des virgules.</returns>
    ''' <remarks></remarks>
    Private Function ObtenirExpediteur() As String
        Dim adresse As String = XuCuConfiguration.ValeurSysteme("TS7", "TS7\TS7N121\AdressesCourriels\Expediteur")

        Return adresse
    End Function

#End Region

End Class
