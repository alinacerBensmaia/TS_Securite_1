Imports System.ServiceModel
Imports Rrq.CS.ServicesCommuns.ScenarioTransactionnel
Imports Rrq.InfrastructureCommune.ScenarioTransactionnel
Imports TS1N621_INiveauSecrt1
Imports Rrq.InfrastructureCommune.Parametres
Imports SFTPCOMINTERFACELib
Imports Rrq.Securite.PAM
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic

'''-----------------------------------------------------------------------------
''' Project		: TS1N621_CiGererUtilSecFtpSvr
''' Class		: TsCaNiveauSecrt1
''' 	
'''-----------------------------------------------------------------------------
''' <summary>
''' Classe d'affaire.
''' </summary>
'''-----------------------------------------------------------------------------
<ServiceBehavior(ConcurrencyMode:=ConcurrencyMode.Single, InstanceContextMode:=InstanceContextMode.PerCall, AddressFilterMode:=AddressFilterMode.Any)>
Public Class TsCaNiveauSecrt1
    Inherits ClassesBaseIntegration.XuCaBaseComposantV2
    Implements TsICompI

#Region "Membres Privés"

    Private Const REPRT_TETE As String = "/Partenaires/"
    Private Const REPRT_ACCP As String = "ACCP/"
    Private Const REPRT_INTG As String = "INTG/"
    Private Const REPRT_PROD As String = "PROD/"
    Private Const REPRT_UNIT As String = "UNIT/"
    Private Const REPRT_RRQ As String = "RRQ"
    Private Const REPRT_VERS As String = "_vers_"

    Private Const TS1N221_ENVIRONNEMENT_ACCP As String = "Accp"
    Private Const TS1N221_ENVIRONNEMENT_INTG As String = "Intg"
    Private Const TS1N221_ENVIRONNEMENT_PROD As String = "Prod"
    Private Const TS1N221_ENVIRONNEMENT_UNIT As String = "Essai"
    Private Const TS1N221_TYPE_PARTN As String = "Compte FTP Partenaires"
    Private Const TS1N221_TYPE_INTERNE As String = "Compte FTP Clientele Interne"
    Private Const TS1N221_TYPE_EXTERNE As String = "Compte FTP Clientele Externe"
    Private Const TS1N221_DESC_1 As String = "Compte FTP "
    Private Const TS1N221_DESC_2 As String = " pour l'environnement "
    Private Const TS1N221_PROFIL_ESSAI As String = "TS1EAdminSEEL"
    Private Const TS1N221_PROFIL_PROD As String = "TS1PAdminSEEL"
    Private Const TS1N221_NIVEAU As String = "Normal"

    Private mConfigurationInitialisee As Boolean = False
    Private mSFTPServer As CIServer
    Private mSFTPServerEssais As CIServer

    Private strCheminDepotEmployesACCP As String
    Private strCheminDepotEmployesINTG As String
    Private strCheminDepotEmployesPROD As String
    Private strCheminDepotEmployesUNIT As String
    Private strCheminListeUtilisateursACCP As String
    Private strCheminListeUtilisateursINTG As String
    Private strCheminListeUtilisateursPROD As String
    Private strCheminListeUtilisateursUNIT As String
    Private strNomServeurSecureFTPAdmin As String
    Private strPortServeurSecureFTPAdmin As String
    Private strNomServeurSecureFTPAdminEssais As String
    Private strPortServeurSecureFTPAdminEssais As String

    Private Enum TypePermission
        Download = 1
        Upload = 2
        DownloadUpload = 3
        Liste = 4
        Complet = 5
    End Enum

#End Region

    <OperationBehavior(TransactionScopeRequired:=False, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)>
    Private Function CreerCompteFtp(ByRef ChaineContexte As String, ByVal InformationsCreation As TsDtCreationComptes) _
        As Generic.IList(Of TsDtInfoCleSymbolique) Implements TsICompI.CreerCompteFtp

        Dim listeCreation As New List(Of TsDtInfoCleSymbolique)()

        Try

            If InformationsCreation.InCreUni OrElse InformationsCreation.InCreInt OrElse InformationsCreation.InCreAcp Then

                Dim Site As CISite = ConnecterFtpEssais(ChaineContexte)

                InitialiserSiteEnvironnement(ChaineContexte, mSFTPServerEssais, Site, InformationsCreation.InCreUni, REPRT_UNIT, InformationsCreation, listeCreation)
                InitialiserSiteEnvironnement(ChaineContexte, mSFTPServerEssais, Site, InformationsCreation.InCreInt, REPRT_INTG, InformationsCreation, listeCreation)
                InitialiserSiteEnvironnement(ChaineContexte, mSFTPServerEssais, Site, InformationsCreation.InCreAcp, REPRT_ACCP, InformationsCreation, listeCreation)

                FermerFtpEssais()

            End If

            If InformationsCreation.InCrePrd Then

                Dim Site As CISite = ConnecterFtp(ChaineContexte)

                InitialiserSiteEnvironnement(ChaineContexte, mSFTPServer, Site, InformationsCreation.InCrePrd, REPRT_PROD, InformationsCreation, listeCreation)

                FermerFtp()

            End If

        Catch ex As XuExcEErrFonctionnelle
            MyBase.CreerRetourErrFonctionnelle(ChaineContexte, ex)
        Catch ex As XuExcEErrValidation
            'Pour les applications Client/Serveur, on doit convertir l'exception XU en exception XZ.
            MyBase.CreerRetourErrValidation(ChaineContexte, New XZCuErrValdtException(ex))
        End Try

        Return listeCreation

    End Function


    <OperationBehavior(TransactionScopeRequired:=False, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)>
    Private Function ObtenirListeComptes(ByRef ChaineContexte As String) As Generic.IList(Of String) Implements TsICompI.ObtenirListeComptes

        Try
            Dim lRetour As New List(Of String)()

            Dim site As CISite = ConnecterFtpEssais(ChaineContexte)

            Dim asACCP As String = site.GetFolderList(REPRT_TETE & REPRT_ACCP)
            Dim asINTG As String = site.GetFolderList(REPRT_TETE & REPRT_INTG)
            Dim asUNIT As Char() = site.GetFolderList(REPRT_TETE & REPRT_UNIT)

            lRetour.AddRange(Regex.Split(asACCP, "(\r\n|\r|\n)", RegexOptions.ExplicitCapture).ToList)
            lRetour.AddRange(Regex.Split(asINTG, "(\r\n|\r|\n)", RegexOptions.ExplicitCapture).ToList)
            lRetour.AddRange(Regex.Split(asUNIT, "(\r\n|\r|\n)", RegexOptions.ExplicitCapture).ToList)

            FermerFtpEssais()

            If EstProduction() Then

                site = ConnecterFtp(ChaineContexte)

                Dim asPROD As String = site.GetFolderList(REPRT_TETE & REPRT_PROD)

                lRetour.AddRange(Regex.Split(asPROD, "(\r\n|\r|\n)", RegexOptions.ExplicitCapture).ToList)

                FermerFtp()

            End If

            lRetour.Sort()

            Return lRetour

        Catch ex As XuExcEErrFonctionnelle
            MyBase.CreerRetourErrFonctionnelle(ChaineContexte, ex)
        Catch ex As XuExcEErrValidation
            'Pour les applications Client/Serveur, on doit convertir l'exception XU en exception XZ.
            MyBase.CreerRetourErrValidation(ChaineContexte, New XZCuErrValdtException(ex))
        End Try

        Return Nothing

    End Function
    '<OperationBehavior(TransactionScopeRequired:=False, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)>
    'Private Function ObtenirListeComptes(ByRef ChaineContexte As String) As Generic.IList(Of String) Implements TsICompI.ObtenirListeComptes

    '    Try
    '        Dim lRetour As New List(Of String)()

    '        Dim site As CISite = ConnecterFtpEssais(ChaineContexte)

    '        Dim aUsers As Object() = DirectCast(site.GetUsers, Object())
    '        For I As Integer = 0 To aUsers.Length - 1
    '            If Not lRetour.Contains(aUsers(I).ToString()) Then
    '                lRetour.Add(aUsers(I).ToString())
    '            End If
    '        Next

    '        FermerFtpEssais()

    '        If EstProduction() Then

    '            site = ConnecterFtp(ChaineContexte)

    '            aUsers = DirectCast(site.GetUsers, Object())
    '            For I As Integer = 0 To aUsers.Length - 1
    '                lRetour.Add(aUsers(I).ToString())
    '            Next

    '            FermerFtp()

    '        End If

    '        Return lRetour

    '    Catch ex As XuExcEErrFonctionnelle
    '        MyBase.CreerRetourErrFonctionnelle(ChaineContexte, ex)
    '    Catch ex As XuExcEErrValidation
    '        'Pour les applications Client/Serveur, on doit convertir l'exception XU en exception XZ.
    '        MyBase.CreerRetourErrValidation(ChaineContexte, New XZCuErrValdtException(ex))
    '    End Try

    '    Return Nothing

    'End Function


    <OperationBehavior(TransactionScopeRequired:=False, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)>
    Private Sub SupprimerCompte(ByRef ChaineContexte As String, ByVal NomCompte As String) Implements TsICompI.SupprimerCompte

        Try

            Dim Site As CISite
            Dim ClientSettings As CIClientSettings
            Dim strPrefixeUsager As String = NomCompte.Substring(0, 1)
            Dim strCodeEnv As String = NomCompte.Substring(1, 2).ToUpper()

            'Ouverture de la connection au serveur
            If strCodeEnv = "PR" Then
                Site = ConnecterFtp(ChaineContexte)
            Else
                Site = ConnecterFtpEssais(ChaineContexte)
            End If

            Dim strClient As String = NomCompte.Substring(3)

            'On supprime si l'utilisateur est avec compte FTP a été créé
            Dim isCompteFtpPresent As Boolean = EstUtilisateurAvecPresenceCompteFtp(NomCompte, Site)

            If isCompteFtpPresent Then
                'On supprime l'utilisateur et son home folder                
                ClientSettings = Site.GetUserSettings(NomCompte)
                Site.RemoveFolder(ClientSettings.GetHomeDirString())
                Site.RemoveUser(NomCompte)
            Else
                'On supprime juste les dépots de fichiers
                Dim CodeRepertoire As String = String.Empty
                Select Case strCodeEnv
                    Case "UN"
                        CodeRepertoire = REPRT_UNIT
                    Case "IN"
                        CodeRepertoire = REPRT_INTG
                    Case "AC"
                        CodeRepertoire = REPRT_ACCP
                    Case "PR"
                        CodeRepertoire = REPRT_PROD
                End Select

                'On supprime les dépots de fichiers
                Site.RemoveFolder(REPRT_TETE & CodeRepertoire & NomCompte & "/")
            End If

            If strPrefixeUsager = "I" Then
                'Destruction des répertoires de téléchargement des fichiers.

                Select Case strCodeEnv
                    Case "UN"
                        IO.Directory.Delete(strCheminDepotEmployesUNIT & strPrefixeUsager & REPRT_UNIT.Substring(0, 2) & strClient, True)
                    Case "IN"
                        IO.Directory.Delete(strCheminDepotEmployesINTG & strPrefixeUsager & REPRT_INTG.Substring(0, 2) & strClient, True)
                    Case "AC"
                        IO.Directory.Delete(strCheminDepotEmployesACCP & strPrefixeUsager & REPRT_ACCP.Substring(0, 2) & strClient, True)
                    Case "PR"
                        IO.Directory.Delete(strCheminDepotEmployesPROD & strPrefixeUsager & REPRT_PROD.Substring(0, 2) & strClient, True)
                End Select
            End If

            'Destruction des utilisateurs dans le fichier TS1ListeUtilisateurs.xml
            Select Case strCodeEnv
                Case "UN"
                    SupprimerListeUtilisateurs(strCheminListeUtilisateursUNIT, strPrefixeUsager & REPRT_UNIT.Substring(0, 2) & strClient)
                Case "IN"
                    SupprimerListeUtilisateurs(strCheminListeUtilisateursINTG, strPrefixeUsager & REPRT_INTG.Substring(0, 2) & strClient)
                Case "AC"
                    SupprimerListeUtilisateurs(strCheminListeUtilisateursACCP, strPrefixeUsager & REPRT_ACCP.Substring(0, 2) & strClient)
                Case "PR"
                    SupprimerListeUtilisateurs(strCheminListeUtilisateursPROD, strPrefixeUsager & REPRT_PROD.Substring(0, 2) & strClient)
            End Select

            'Fermeture de la connection au serveur
            If strCodeEnv = "PR" Then
                FermerFtp()
            Else
                FermerFtpEssais()
            End If

            'Si un compte de type Clientèle Externe (Internaute), on doit aussi la supprimer de la 
            'voute de mot de passe PAM
            If NomCompte.StartsWith("E") Then
                If isCompteFtpPresent Then
                    Dim device As String = XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N621\DeviceVoute")

                    Dim application As String = XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N621\ApplicationVoute")

                    Dim objTraitDemndMotPasse As New TsCuDemndMajMotPasse(device, application)

                    objTraitDemndMotPasse.SupprimerUsagerMotPasse(NomCompte)
                End If
            End If

        Catch ex As XuExcEErrFonctionnelle
            MyBase.CreerRetourErrFonctionnelle(ChaineContexte, ex)
        Catch ex As XuExcEErrValidation
            'Pour les applications Client/Serveur, on doit convertir l'exception XU en exception XZ.
            MyBase.CreerRetourErrValidation(ChaineContexte, New XZCuErrValdtException(ex))
        End Try

    End Sub

    <OperationBehavior(TransactionScopeRequired:=False, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)>
    Private Sub DeverrouillerCompte(ByRef ChaineContexte As String, ByVal NomCompte As String) Implements TsICompI.DeverrouillerCompte

        Try

            Dim Site As CISite
            Dim ClientSettings As CIClientSettings
            Dim strCodeEnv As String = NomCompte.Substring(1, 2).ToUpper()

            'Ouverture de la connection au serveur
            If strCodeEnv = "PR" Then
                Site = ConnecterFtp(ChaineContexte)
            Else
                Site = ConnecterFtpEssais(ChaineContexte)
            End If

            'On déverrouille l'utilisateur
            ClientSettings = Site.GetUserSettings(NomCompte)
            ClientSettings.SetEnableAccount(SFTPAdvBool.abTrue)

            'Fermeture de la connection au serveur
            If strCodeEnv = "PR" Then
                mSFTPServer.ApplyChanges()
                FermerFtp()
            Else
                mSFTPServerEssais.ApplyChanges()
                FermerFtpEssais()
            End If

        Catch ex As XuExcEErrFonctionnelle
            MyBase.CreerRetourErrFonctionnelle(ChaineContexte, ex)
        Catch ex As XuExcEErrValidation
            'Pour les applications Client/Serveur, on doit convertir l'exception XU en exception XZ.
            MyBase.CreerRetourErrValidation(ChaineContexte, New XZCuErrValdtException(ex))
        End Try

    End Sub

#Region "Méthodes privées"

    ''' <summary>
    ''' Charge la configuration pour la connexion au FTP, si la configuration est déjà en mémoire, on ne la recharge pas inutilement
    ''' </summary>
    ''' <param name="ChaineContexte"></param>
    Private Sub InitialiserConfiguration(ByRef ChaineContexte As String)

        If Not mConfigurationInitialisee Then



            'On va chercher le nom du serveur et le port de SecureFTPServer
            strCheminDepotEmployesACCP = XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N611\Environnements\ACCP\CheminDepotEmployes")
            strCheminDepotEmployesINTG = XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N611\Environnements\INTG\CheminDepotEmployes")
            strCheminDepotEmployesPROD = XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N611\Environnements\PROD\CheminDepotEmployes")
            strCheminDepotEmployesUNIT = XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N611\Environnements\UNIT\CheminDepotEmployes")
            strCheminListeUtilisateursACCP = XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N611\Environnements\ACCP\CheminListeUtilisateurs")
            strCheminListeUtilisateursINTG = XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N611\Environnements\INTG\CheminListeUtilisateurs")
            strCheminListeUtilisateursPROD = XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N611\Environnements\PROD\CheminListeUtilisateurs")
            strCheminListeUtilisateursUNIT = XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N611\Environnements\UNIT\CheminListeUtilisateurs")

            strNomServeurSecureFTPAdmin = XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N611\NomServeurSecureFTPAdmin")
            strPortServeurSecureFTPAdmin = XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N611\PortServeurSecureFTPAdmin")

            strNomServeurSecureFTPAdminEssais = XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N611\NomServeurSecureFTPAdminEssais")
            strPortServeurSecureFTPAdminEssais = XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N611\PortServeurSecureFTPAdminEssais")

            mConfigurationInitialisee = True

        End If

    End Sub

    ''' <summary>
    ''' Détermine si on est en mode production et qu'il y a deux serveurs à utiliser ou si il faut uniquement utiliser un seul serveur
    ''' </summary>
    ''' <returns>Vrai si on doit utiliser un serveur différent pour la production, faux sinon</returns>
    Private Function EstProduction() As Boolean

        Return strNomServeurSecureFTPAdmin <> strNomServeurSecureFTPAdminEssais OrElse strPortServeurSecureFTPAdmin <> strPortServeurSecureFTPAdminEssais

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Function ConnecterFtp(ByRef ChaineContexte As String) As CISite

        Dim Sites As CISites

        If mSFTPServer Is Nothing Then

            InitialiserConfiguration(ChaineContexte)

            mSFTPServer = New CIServer()

            Dim objTraitDemndMotPasse As New TsCuDemndRecprMotPasse
            Dim AdminServeurSecureFTP As TsDtCodeUsageMotPasse
            AdminServeurSecureFTP = objTraitDemndMotPasse.ObtenirCodeUsagerMotPasse(XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N621\TargetAliasCompteAdministrateurFTP"))

            'Ouverture de la connection au serveur
            mSFTPServer.Connect(strNomServeurSecureFTPAdmin, Convert.ToInt32(strPortServeurSecureFTPAdmin),
                               AdminServeurSecureFTP.CodeUsager, AdminServeurSecureFTP.MotPasse)

        End If

        Sites = mSFTPServer.Sites

        'On sélectionne le premier serveur dans la liste des serveurs.  Il n'y aura qu'un serveur de toute façon.
        Return Sites.Item(0)

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub FermerFtp()

        If mSFTPServer IsNot Nothing Then

            mSFTPServer.Close()
            mSFTPServer = Nothing

        End If

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Function ConnecterFtpEssais(ByRef ChaineContexte As String) As CISite

        Dim Sites As CISites

        If mSFTPServerEssais Is Nothing Then

            InitialiserConfiguration(ChaineContexte)

            mSFTPServerEssais = New CIServer()

            Dim objTraitDemndMotPasse As New TsCuDemndRecprMotPasse
            Dim AdminServeurSecureFTPEssais As TsDtCodeUsageMotPasse
            AdminServeurSecureFTPEssais = objTraitDemndMotPasse.ObtenirCodeUsagerMotPasse(XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N621\TargetAliasCompteAdministrateurFTPEssais"))

            'Ouverture de la connection au serveur
            mSFTPServerEssais.Connect(strNomServeurSecureFTPAdminEssais, Convert.ToInt32(strPortServeurSecureFTPAdminEssais),
                               AdminServeurSecureFTPEssais.CodeUsager, AdminServeurSecureFTPEssais.MotPasse)

        End If

        Sites = mSFTPServerEssais.Sites

        'On sélectionne le premier serveur dans la liste des serveurs.  Il n'y aura qu'un serveur de toute façon.
        Return Sites.Item(0)

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub FermerFtpEssais()

        If mSFTPServerEssais IsNot Nothing Then

            mSFTPServerEssais.Close()
            mSFTPServerEssais = Nothing

        End If

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="intLongueurMot"></param>
    ''' <param name="blnUtiliseMinuscules"></param>
    ''' <param name="blnUtiliseMajuscules"></param>
    ''' <param name="blnUtiliseChiffres"></param>
    ''' <param name="blnUtiliseSymboles"></param>
    ''' <returns></returns>
    Private Function GenererMotDePasse(ByVal intLongueurMot As Integer, ByVal blnUtiliseMinuscules As Boolean,
                                        ByVal blnUtiliseMajuscules As Boolean, ByVal blnUtiliseChiffres As Boolean,
                                        ByVal blnUtiliseSymboles As Boolean) As String
        Dim objTS6ZgLibOutils As New TS6N011_ZgLibOutils.TsCuMotDePasse()

        Return objTS6ZgLibOutils.GenererMotDePasse(intLongueurMot, blnUtiliseMinuscules, blnUtiliseMajuscules, blnUtiliseChiffres, blnUtiliseSymboles)
    End Function

    ''' <summary>
    ''' Effectue le traitement initial pour la création d'un utilisateur et de son répertoire physique
    ''' pour un environnement donné
    ''' </summary>
    ''' <param name="ChaineContexte"></param>
    ''' <param name="objFtpServer">Instance du serveur FTP</param>
    ''' <param name="objSite">Objet de contrôle du site FTP</param>
    ''' <param name="blnActif">Créer les répertoire lorsqu'à vrai, ignorer la création lorsqu'à faux</param>
    ''' <param name="strSousRepEnv">Nom du sous répertoire</param>
    ''' <param name="objInformationsCreation"></param>
    ''' <param name="listeCreation">Liste des éléments créés</param>
    Private Sub InitialiserSiteEnvironnement(ByRef ChaineContexte As String, ByVal objFtpServer As CIServer, ByVal objSite As CISite, ByVal blnActif As Boolean,
                                             ByVal strSousRepEnv As String, ByVal objInformationsCreation As TsDtCreationComptes,
                                             ByVal listeCreation As List(Of TsDtInfoCleSymbolique))

        If blnActif Then

            Try
                ' Paramètres généraux
                Dim strType As String = String.Empty

                Select Case objInformationsCreation.VlPfxUsg
                    Case "Z"
                        strType = TS1N221_TYPE_PARTN
                    Case "E"
                        strType = TS1N221_TYPE_EXTERNE
                    Case "I"
                        strType = TS1N221_TYPE_INTERNE
                End Select

                Dim strUtilisateur As String = objInformationsCreation.VlPfxUsg & strSousRepEnv.Substring(0, 2) & objInformationsCreation.VlAbrCli

                'On créé les répertoires physique liés au nouvel utilisateur
                objSite.CreatePhysicalFolder(REPRT_TETE & strSousRepEnv & strUtilisateur & "/")
                objSite.CreatePhysicalFolder(REPRT_TETE & strSousRepEnv & strUtilisateur & "/" & objInformationsCreation.VlAbrCli & REPRT_VERS & REPRT_RRQ & "/")
                objSite.CreatePhysicalFolder(REPRT_TETE & strSousRepEnv & strUtilisateur & "/" & REPRT_RRQ & REPRT_VERS & objInformationsCreation.VlAbrCli & "/")

                Dim strMotDePasse As String = String.Empty

                If Not (strType = TS1N221_TYPE_EXTERNE AndAlso objInformationsCreation.InCreCompteFtp = False) Then
                    'On créé le nouvel utilisateur dans chaque environnement
                    strMotDePasse = GenererMotDePasse(10, True, True, True, True)
                    objSite.CreateUser(strUtilisateur, strMotDePasse, 0, "")

                    'On ajuste les différentes propriétés pour chaque utilisateur créé
                    AjusterProprietes(objFtpServer, objSite, strUtilisateur, REPRT_TETE & strSousRepEnv & strUtilisateur & "/", objInformationsCreation)

                    'On ajuste les paramètres de sécurité sur les répertoires physique créé antérieurement en
                    'associant l'utilisateur créé à chaque répertoire et en lui donnant les droits prévu.
                    AjusterPermissions(objSite, strUtilisateur, REPRT_TETE & strSousRepEnv & objInformationsCreation.VlPfxUsg & strSousRepEnv.Substring(0, 2), objInformationsCreation)
                    'Else
                    '    'Appliquer les modification
                    '    objFtpServer.ApplyChanges()
                End If

                ' Paramètres et permissions selon l'environnement
                Dim strDepotEnv As String = String.Empty
                Dim strListeEnv As String = String.Empty
                Dim strTypeEnv As String = TS1N221_ENVIRONNEMENT_UNIT
                Dim strProfil As String = TS1N221_PROFIL_ESSAI
                Dim strEnvironnement As String = String.Empty

                Select Case strSousRepEnv
                    Case REPRT_UNIT
                        strEnvironnement = "Unitaire"
                        strDepotEnv = strCheminDepotEmployesUNIT
                        strListeEnv = strCheminListeUtilisateursUNIT
                        'L'environnement UNITAIRE n'as pas de user ZxxCENTRAL
                        Try
                            Dim objPermissions As Permission
                            objPermissions = objSite.GetBlankPermission(REPRT_TETE & strSousRepEnv & strUtilisateur & "/" &
                                                                        REPRT_RRQ & REPRT_VERS & objInformationsCreation.VlAbrCli & "/", "ZUNAdmSEEL")
                            AppliquerPermissions(TypePermission.DownloadUpload, objPermissions)
                            objSite.SetPermission(objPermissions)
                            objPermissions = Nothing
                        Catch
                        End Try
                    Case REPRT_INTG
                        strEnvironnement = "Intégration"
                        strDepotEnv = strCheminDepotEmployesINTG
                        strListeEnv = strCheminListeUtilisateursINTG
                        AjusterPermissions(objSite, "ZINCENTRAL", "ZINAdmSEEL", REPRT_TETE & strSousRepEnv & objInformationsCreation.VlPfxUsg & strSousRepEnv.Substring(0, 2), objInformationsCreation)
                    Case REPRT_ACCP
                        strEnvironnement = "Acceptation"
                        strDepotEnv = strCheminDepotEmployesACCP
                        strListeEnv = strCheminListeUtilisateursACCP
                        AjusterPermissions(objSite, "ZACCENTRAL", "ZACAdmSEEL", REPRT_TETE & strSousRepEnv & objInformationsCreation.VlPfxUsg & strSousRepEnv.Substring(0, 2), objInformationsCreation)
                    Case REPRT_PROD
                        strEnvironnement = "Production"
                        strDepotEnv = strCheminDepotEmployesPROD
                        strListeEnv = strCheminListeUtilisateursPROD
                        strTypeEnv = TS1N221_ENVIRONNEMENT_PROD
                        strProfil = TS1N221_PROFIL_PROD
                        AjusterPermissions(objSite, "ZPRCENTRAL", "ZPRAdmSEEL", REPRT_TETE & strSousRepEnv & objInformationsCreation.VlPfxUsg & strSousRepEnv.Substring(0, 2), objInformationsCreation)
                End Select

                If (strType = TS1N221_TYPE_EXTERNE AndAlso objInformationsCreation.InCreCompteFtp = True) Then
                    Dim device As String = XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N621\DeviceVoute")

                    Dim application As String = XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N621\ApplicationVoute")

                    Dim objTraitDemndMotPasse As New TsCuDemndMajMotPasse(device, application)

                    objTraitDemndMotPasse.InscrireUsagerMotPasse(strUtilisateur,
                                                                TS1N221_DESC_1 & objInformationsCreation.VlAbrCli & TS1N221_DESC_2 & strTypeEnv & ".",
                                                                strType, strMotDePasse)
                End If

                If objInformationsCreation.VlPfxUsg = "I" Then
                    'Création des répertoires de téléchargement des fichiers.
                    IO.Directory.CreateDirectory(strDepotEnv & strUtilisateur)
                End If

                'Ajout des utilisateurs dans le fichier TS1ListeUtilisateurs.xml
                AjouterListeUtilisateurs(strListeEnv, strUtilisateur)

                If Not (strType = TS1N221_TYPE_EXTERNE AndAlso objInformationsCreation.InCreCompteFtp = False) Then

                    Dim objCleCreation As New TsDtInfoCleSymbolique()

                    With objCleCreation
                        .CoEnv = strEnvironnement
                        .NmUtl = "EE1" & strUtilisateur
                        .NmCom = strUtilisateur
                        .VlMotPasCle = strMotDePasse
                        .DsCle = TS1N221_DESC_1 & objInformationsCreation.VlAbrCli & TS1N221_DESC_2 & strTypeEnv
                        .NmProUtl = strProfil
                    End With

                    listeCreation.Add(objCleCreation)
                End If
            Catch exCOM As System.Runtime.InteropServices.COMException
                If exCOM.ErrorCode = -2147467259 Then
                    Throw New XuExcEErrValidation("Erreur lors de l'ajout.  L'utilisateur spécifié ou une de ses références est existant.")
                Else
                    Throw
                End If
            End Try

        End If

    End Sub

    Private Sub AjusterProprietes(ByVal objFtpServer As CIServer, ByVal objSite As CISite,
                                  ByVal strCodeUtil As String, ByVal strReprtRacine As String,
                                  ByVal objInformationsCreation As TsDtCreationComptes)
        Dim ClientSettings As CIClientSettings

        ClientSettings = objSite.GetUserSettings(strCodeUtil)
        ClientSettings.SetEnableAccount(SFTPAdvBool.abTrue)
        ClientSettings.SetHomeDir(SFTPAdvBool.abTrue)
        ClientSettings.SetHomeDirString(strReprtRacine)
        ClientSettings.SetHomeDirIsRoot(SFTPAdvBool.abTrue)
        ClientSettings.SetFTPS(SFTPAdvBool.abTrue)
        If objInformationsCreation.InConFtpNonSec Then
            ClientSettings.SetClearFTP(SFTPAdvBool.abTrue)
            ClientSettings.SetHomeIP(1)
            ClientSettings.SetHomeIPString(objInformationsCreation.VlIpRac)
        Else
            ClientSettings.SetClearFTP(SFTPAdvBool.abInherited)
        End If
        objFtpServer.ApplyChanges()

        ClientSettings = Nothing
    End Sub

    Private Overloads Sub AjusterPermissions(ByVal objSite As CISite, ByVal strCodeUtil As String, ByVal strTroncCommunReprt As String,
                                             ByVal objInformationsCreation As TsDtCreationComptes)
        Dim objPermissions As Permission

        Try
            objPermissions = objSite.GetBlankPermission(strTroncCommunReprt & objInformationsCreation.VlAbrCli & "/", strCodeUtil)
            AppliquerPermissions(TypePermission.Liste, objPermissions)
            objSite.SetPermission(objPermissions)
        Catch
        End Try

        Try
            objPermissions = objSite.GetBlankPermission(strTroncCommunReprt & objInformationsCreation.VlAbrCli & "/" &
                                                        objInformationsCreation.VlAbrCli & REPRT_VERS & REPRT_RRQ & "/", strCodeUtil)
            AppliquerPermissions(TypePermission.Upload, objPermissions)
            objSite.SetPermission(objPermissions)
        Catch
        End Try

        Try
            objPermissions = objSite.GetBlankPermission(strTroncCommunReprt & objInformationsCreation.VlAbrCli & "/" & REPRT_RRQ & REPRT_VERS &
                                                        objInformationsCreation.VlAbrCli & "/", strCodeUtil)
            AppliquerPermissions(TypePermission.Download, objPermissions)
            objSite.SetPermission(objPermissions)
        Catch
        End Try

        objPermissions = Nothing
    End Sub

    Private Overloads Sub AjusterPermissions(ByVal objSite As CISite, ByVal strCodeUtilCentral As String, ByVal strCodeUtilAdmSEEL As String,
                                             ByVal strTroncCommunReprt As String, ByVal objInformationsCreation As TsDtCreationComptes)
        Dim objPermissions As Permission

        Try
            objPermissions = objSite.GetBlankPermission(strTroncCommunReprt & objInformationsCreation.VlAbrCli & "/" & objInformationsCreation.VlAbrCli &
                                                        REPRT_VERS & REPRT_RRQ & "/", strCodeUtilCentral)
            AppliquerPermissions(TypePermission.Download, objPermissions)
            objSite.SetPermission(objPermissions)
        Catch
        End Try

        Try
            objPermissions = objSite.GetBlankPermission(strTroncCommunReprt & objInformationsCreation.VlAbrCli & "/" & REPRT_RRQ & REPRT_VERS &
                                                        objInformationsCreation.VlAbrCli & "/", strCodeUtilCentral)
            AppliquerPermissions(TypePermission.Upload, objPermissions)
            objSite.SetPermission(objPermissions)
        Catch
        End Try

        Try
            objPermissions = objSite.GetBlankPermission(strTroncCommunReprt & objInformationsCreation.VlAbrCli & "/" & REPRT_RRQ & REPRT_VERS &
                                                        objInformationsCreation.VlAbrCli & "/", strCodeUtilAdmSEEL)
            AppliquerPermissions(TypePermission.DownloadUpload, objPermissions)
            objSite.SetPermission(objPermissions)
        Catch
        End Try

        objPermissions = Nothing
    End Sub

    Private Sub AppliquerPermissions(ByVal typPerm As TypePermission, ByRef objPerm As Permission)
        If typPerm = TypePermission.Download Then               'DOWNLOAD
            objPerm.DirCreate = False
            objPerm.DirDelete = False
            objPerm.DirList = True
            objPerm.DirShowHidden = False
            objPerm.DirShowInList = True
            objPerm.DirShowReadOnly = False
            objPerm.FileAppend = False
            objPerm.FileDelete = True
            objPerm.FileDownload = True
            objPerm.FileRename = False
            objPerm.FileUpload = False
        ElseIf typPerm = TypePermission.Upload Then             'UPLOAD
            objPerm.DirCreate = False
            objPerm.DirDelete = False
            objPerm.DirList = True
            objPerm.DirShowHidden = False
            objPerm.DirShowInList = True
            objPerm.DirShowReadOnly = False
            objPerm.FileAppend = True
            objPerm.FileDelete = False
            objPerm.FileDownload = False
            objPerm.FileRename = False
            objPerm.FileUpload = True
        ElseIf typPerm = TypePermission.DownloadUpload Then     'DOWNLOADUPLOAD
            objPerm.DirCreate = False
            objPerm.DirDelete = False
            objPerm.DirList = True
            objPerm.DirShowHidden = False
            objPerm.DirShowInList = True
            objPerm.DirShowReadOnly = False
            objPerm.FileAppend = True
            objPerm.FileDelete = True
            objPerm.FileDownload = True
            objPerm.FileRename = False
            objPerm.FileUpload = True
        ElseIf typPerm = TypePermission.Liste Then              'LISTE
            objPerm.DirCreate = False
            objPerm.DirDelete = False
            objPerm.DirList = True
            objPerm.DirShowHidden = False
            objPerm.DirShowInList = True
            objPerm.DirShowReadOnly = False
            objPerm.FileAppend = False
            objPerm.FileDelete = False
            objPerm.FileDownload = False
            objPerm.FileRename = False
            objPerm.FileUpload = False
        Else                                                    'COMPLET
            objPerm.DirCreate = True
            objPerm.DirDelete = True
            objPerm.DirList = True
            objPerm.DirShowHidden = True
            objPerm.DirShowInList = True
            objPerm.DirShowReadOnly = True
            objPerm.FileAppend = True
            objPerm.FileDelete = True
            objPerm.FileDownload = True
            objPerm.FileRename = True
            objPerm.FileUpload = True
        End If
    End Sub

    Private Sub AjouterListeUtilisateurs(ByVal strFichier As String, ByVal strCodeUtil As String)
        Dim objDsLstUtils As New ListeUtilisateurs
        Dim objDR As DataRow()

        If IO.File.Exists(strFichier) Then objDsLstUtils.ReadXml(strFichier)
        objDR = objDsLstUtils.Utilisateur.Select("CodeUtilisateur='" & strCodeUtil & "'")
        If objDR.Length = 0 Then
            objDsLstUtils.Utilisateur.AddUtilisateurRow(strCodeUtil)
            objDsLstUtils.WriteXml(strFichier)
        End If

        objDsLstUtils.Dispose()
    End Sub

    Private Sub SupprimerListeUtilisateurs(ByVal strFichier As String, ByVal strCodeUtil As String)
        Dim objDsLstUtils As New ListeUtilisateurs
        Dim objDR As DataRow()

        objDsLstUtils.ReadXml(strFichier)
        objDR = objDsLstUtils.Utilisateur.Select("CodeUtilisateur='" & strCodeUtil & "'")
        If objDR.Length = 1 Then objDsLstUtils.Utilisateur.RemoveUtilisateurRow(objDR(0))
        objDsLstUtils.WriteXml(strFichier)

        objDsLstUtils.Dispose()
    End Sub

    'utilisateur est avec compte FTP a été créé
    Private Function EstUtilisateurAvecPresenceCompteFtp(ByVal compte As String, ByVal ftpSite As CISite) As Boolean
        Dim arUsers As Object = ftpSite.GetUsers()
        For j As Integer = LBound(arUsers) To UBound(arUsers)
            If compte = arUsers(j) Then
                Return True
            End If
        Next
        Return False
    End Function

#End Region

End Class
