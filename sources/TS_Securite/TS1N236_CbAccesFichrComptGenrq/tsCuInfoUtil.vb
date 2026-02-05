Imports System.IO
Imports System.Security.Cryptography
Imports Rrq.InfrastructureCommune.Parametres
Imports System.Security.Principal
Imports System.Collections.Generic
Imports System.Runtime.CompilerServices
Imports Rrq.Securite.Applicative
Imports System.Configuration

''' <summary>
''' Dans cette classe, on y retrouve les méthodes utilisées par plus d'une classe
''' Ce sont des méthodes générales de traitement.
''' </summary>
''' <remarks>
''' Historique des modifications: 
''' --------------------------------------------------------------------------------
''' Date		Nom			Description
''' --------------------------------------------------------------------------------
''' 2007-11-26	T206500		Transférer les paramètres de configuration dans le fichier
'''                         "TS1.config". On utilise l'utilitaire XU4N011_Configuration
'''                         pour obtenir ces paramètres. 
'''                         Seulement la classe TsCuInfoUtil est conservée dans 
'''                         ce fichier. Chaque classe est maintenant isolée dans son
'''                         propre fichier. 
'''                         Enlever les propriétés, elles n'étaient pas utiles ici.
'''                         Conserver seulement les méthodes qui sont utilisées par 
'''                         plus d'une classe. Les méthodes utilisées par une classe
'''                         sont devenues des méthodes privées dans leur classe.
'''                         Normaliser les noms des variables et des méthodes.
'''                         Enlever toutes les variables privées. On ne conserve pas
'''                         d'état dans cette classe utilitaire.
''' 
''' 2009-12-08  T206500     Effectuer des modifications pour régler les problèmes
'''                         de performance.  
'''                         On lit le fichier des codes d'accès aux 5 minutes afin de 
'''                         limiter les accès au serveur FIC1.
'''                         Les groupes d'accès de l'utilisateur sont conservés en SID                        
'''                         dans le jeton de sécurité.  Avant, on effectuait une conversion
'''                         de chaque groupe d'accès.  Ce qui impliquait un accès à l'AD
'''                         pour chaque groupe.  Maintenant, on compare le SID du groupe
'''                         d'accès avec le SID du profil de la clé symbolique.
'''                         Donc, seulement, le profil recherché est converti en SID. On
'''                         effectue un accès à l'AD.
''' 
''' 2010-01-11  T206500     Ajout des constantes NOM_GROUPE_ADMIN_SECURITE et 
'''                         DELAI_VERIF_FICHIER_CODE_ACCES     
''' 
''' 2010-01-13  T206500     Effectuer un SyncLock avant chaque chargement de fichier.
'''                         Refaire les comparaisons juste avant de charger, car
'''                         on ne veut pas charger inutilement.                   
'''                         Utiliser un dataset local lors du chargement.
''' 
''' 2010-01-21  t206500    Utiliser la méthode ObtenirCodeUsager pour obtenir le code
'''                        utilisateur. On fait une exception lorsque c'est le compte SYSTEM. 
''' 
''' 2010-01-25  t206500    Dans la méthode EstMembreDe, on ajoute une execption.
'''                        Le compte SYSTEM  a accès à toutes les clés symboliques
'''                        lorsque c'est le composant TS5N132_ZpGerCOMPlus qui 
'''                        effectue l'appel à TS1N223.
''' --------------------------------------------------------------------------------
''' </remarks>
Friend Class tsCuInfoUtil
    Private Const NOM_GROUPE_ADMIN_SECURITE As String = "ROA_X_TS_TS1N223_ZgObtCdAccGen"
    Private Const DELAI_VERIF_FICHIER_CODE_ACCES As Double = 5

#Region "*-----    Méthodes Friend    -----*"

    ''' <summary>
    ''' Cette méthode retourne le code usager.
    ''' Dans le cas de l'utilisation du compte SYSTEM, elle retourne le nom de 
    ''' l'ordinateur
    ''' </summary>
    ''' <returns>
    ''' Le nom de l'usager
    ''' </returns>
    ''' <remarks>
    ''' Historique des modifications: 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' --------------------------------------------------------------------------------
    ''' 2010-01-19  T206500		Création de la méthode
    '''                         On utilise cette méthode pour effectuer un traitement 
    '''                         spécial lorsque c'est le compte SYSTEM.
    '''                         Lorsque c'est le compte système, on utilise le nom du 
    '''                         serveur comme code usager.  Cela arrive avec l'outil
    '''                         de graduation WEB quand il appelle TS5N132_ZpGerComPlus. 
    ''' --------------------------------------------------------------------------------
    ''' </remarks>
    Friend Function ObtenirCodeUsager() As String
        If WindowsIdentity.GetCurrent.IsSystem Then
            'Certains outils de sécurité s'exécutent avec le compte SYSTEM
            'Dans ces cas, on doit retourner le nom de la machine
            Return Environment.MachineName

        Else
            'Le nom de l'usager est composé comme tel:  Domaine\CodeUtilisateur 
            Dim strDomCodeUtil As String() = WindowsIdentity.GetCurrent.Name.Split("\".ToCharArray())
            'Retourner le code d'utilisateur sans le domaine
            Return strDomCodeUtil(1)
        End If
    End Function


    Friend Function ObtenirCodeUsagerDemandeur() As String
        Dim strDomCodeUtil As String() = System.ServiceModel.OperationContext.Current.ServiceSecurityContext.WindowsIdentity.Name.Split("\".ToCharArray())
        Return strDomCodeUtil(1)
    End Function

    Private Shared _verrouDeLectureFichier As New Object()
    ''' <summary>
    ''' Lire le fichier contenant les informations des codes d'accès génériques (TS1N213.xml).
    ''' Si le fichier a été modifié depuis la dernière lecture, lire le fichier sur disque.
    ''' Sinon, retourner le dataset conservé en mémoire.
    ''' </summary>
    ''' <remarks>
    ''' Historique des modifications: 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' --------------------------------------------------------------------------------
    ''' 2007-11-26	t206500		Normaliser la méthode
    '''                         Correspond à l'ancienne méthode "ReadPwdDS".    
    ''' 
    ''' 2009-12-01  t206500     Transférer du code dans la méthode ChargerFichierCodeAcces
    '''                         afin de faire un lock seulement lors de l'accès au fichier
    '''                         Utilisation d'un flag pour forcer le rafraichissement
    '''                         des données.  Pour une question de performance,
    '''                         on vérifie aux 15 minutes si on doit rafraîchir les 
    '''                         clés symboliques en mémoire.
    ''' 
    ''' 2010-01-13  T206500     Effectuer toutes les vérifications dans cette méthode.
    '''                         On utilise Nullable avec la variable dtProchVerifModifDepot
    '''                         afin d'éviter que l'on compare une valeur partielle.
    '''                         Effectuer le SyncLock avant chaque lecture du fichier
    '''                         des codes d'accès et avant la vérification de la date
    '''                         du fichier des codes d'accès.
    ''' --------------------------------------------------------------------------------
    ''' </remarks>
    Friend Function LireFichierCodeAcces() As DataSet
        Dim strNomFichCdAcc As String = XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("TS1", "TS1\TS1N236\PASSWDFILE")

        ' 1ere vérification  -  On vérifie si le fichier est vide (la première fois)
        If tsCuVarShared.dsCachedCodeAcces Is Nothing Then
            SyncLock _verrouDeLectureFichier
                chargerFichierCodeAcces(strNomFichCdAcc)
            End SyncLock

        Else
            ' 2ième vérification  -  On vérifie si le nom du fichier a été modifié depuis le dernier chargement en mémoire
            If Not tsCuVarShared.strDernNomFichCdAcc.Est(strNomFichCdAcc) Then
                SyncLock _verrouDeLectureFichier
                    chargerFichierCodeAcces(strNomFichCdAcc)
                End SyncLock

            Else
                '3ième vérification  -  On vérifie si le fichier a été modifié depuis le dernier chargement en mémoire
                If tsCuVarShared.dtDernModifDepot <> File.GetLastWriteTime(strNomFichCdAcc) Then
                    SyncLock _verrouDeLectureFichier
                        chargerFichierCodeAcces(strNomFichCdAcc)
                    End SyncLock


                    ' 4ième vérification  -  On vérifie si on a atteint le délai de vérification sur FIC1
                ElseIf DateTime.Now > tsCuVarShared.dtProchVerifModifDepot.Value Then
                    ' Initialisation de la prochaine vérification de changement du fichier des codes accès génériques
                    ' On utilise Nullable afin de s'assurer que la date n'est pas partielle lors de la vérification
                    tsCuVarShared.dtProchVerifModifDepot = New Nullable(Of DateTime)(DateTime.Now.AddMinutes(DELAI_VERIF_FICHIER_CODE_ACCES))
                End If
            End If
        End If

        Return tsCuVarShared.dsCachedCodeAcces.Copy
    End Function

    Friend Function LireFichierCodeAccesLibraire() As Dictionary(Of String, DataRow)
        Dim strNomFichCdAcc As String = XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("TS1", "TS1\TS1N236\PASSWDFILELIBRAIRE")

        Const MAX_ESSAIS As Integer = 10

        Using donneesTemporaires As New DataSet()
            Dim dicCodeAcces As New Dictionary(Of String, DataRow)()
            Dim exceptionOriginal As Exception = Nothing
            Dim succes As Boolean = False

            Dim quantiteEssais As Integer = 0
            Do While quantiteEssais < MAX_ESSAIS
                Try

                    Using objFS As FileStream = New FileStream(strNomFichCdAcc, FileMode.Open, FileAccess.Read, FileShare.None)
                        donneesTemporaires.ReadXml(objFS)

                        For Each row As DataRow In donneesTemporaires.Tables("CdAcces").Rows
                            dicCodeAcces.Add(row.GenererCle(), row)
                        Next
                        succes = True
                    End Using

                Catch ex As Exception
                    exceptionOriginal = ex
                    Threading.Thread.Sleep(100)
                    quantiteEssais += 1
                End Try

                If succes Then Exit Do
            Loop

            If Not succes Then
                Throw New TsCuLectureDepotImpossible(exceptionOriginal, strNomFichCdAcc)
            End If

            Return dicCodeAcces

        End Using ' Libérer le dataset temporaire
    End Function

    Friend Function LireFichierCodeAccesEnDictionnaire() As Dictionary(Of String, DataRow)
        ' On s'assure que c'est à jour en appelant l'ancienne méthode
        LireFichierCodeAcces()
        Return tsCuVarShared.dicCachedCodeAcces
    End Function


    ''' <summary>
    ''' Valider si l'utilisateur appartient au groupe de sécurité.
    ''' La gestion des codes d'accès génériques est effectuée seulement par le groupe 
    ''' de sécurité.
    ''' </summary>
    ''' <param name="strUsager">
    ''' 	Usager qui demande l'information.
    ''' 	Value Type: string
    ''' </param>
    ''' <param name="strCleAcces">
    ''' 	Code accès qui correspond à la clé symbolique demandée.
    ''' 	Value Type: string
    ''' </param>
    ''' <returns>
    '''     Valeur booléenne qui indique si l'utilisateur a droits nécessaires
    ''' </returns>
    ''' <remarks>
    ''' Historique des modifications: 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' --------------------------------------------------------------------------------
    ''' 2007-11-26	t206500		Normaliser la méthode
    '''                         -  On obtient les paramètres de configuration directement
    '''                         de l'utilitaire XU4N011_Configuration
    ''' 
    ''' 2009-03-02  t206500     On effectue une validation sur le serveur de l'inforoute
    ''' 
    ''' 2009-12-08  t206500     Déterminer la valeur du domaine ici pour le passer à la
    '''                         méthode EstMembreDe
    ''' 
    ''' 2010-01-11  t206500     Utilisation de la constante NOM_GROUPE_ADMIN_SECURITE
    ''' --------------------------------------------------------------------------------
    ''' </remarks>
    Friend Function ValiderAdmin(ByVal strUsager As String, ByVal strCleAcces As String) As Boolean
        Dim zones As Zones = Zone.GetCurrents()

        If zones.Contient(Zone.Domaine) Then
            If strCleAcces.Length < 10 Then strCleAcces = strCleAcces.PadRight(10)
            Dim sub10 As String = strCleAcces.ToUpper.Substring(0, 10)
            Dim modeExecution As String = ConfigurationManager.AppSettings.Get("EE1\EE1I111\ModeExecution")

            If sub10.Est("EE1ADMSEEL") AndAlso modeExecution.Est("EeAwcMeExterne") Then
                Return False
            Else
                Return estMembreDe(strUsager, NOM_GROUPE_ADMIN_SECURITE)
            End If

        Else
            Return estMembreDe(strUsager, NOM_GROUPE_ADMIN_SECURITE)
        End If
    End Function

    ''' <summary>
    ''' Valider si l'utilisateur a les accès nécessaires pour obtenir l'information.
    ''' Après avoir obtenu les informations de la clé symbolique demandée, on vérifie si
    ''' l'utilisateur a les droits nécessaires.
    ''' </summary>
    ''' <param name="strUsager">
    ''' 	Usager qui demande l'information.
    ''' 	Value Type: string
    ''' </param>
    ''' <param name="strCleAcces">
    ''' 	Code accès qui correspond à la clé symbolique demandée.
    ''' 	Value Type: string
    ''' </param>
    ''' <param name="strTypeCourant">
    ''' 	Type de la clé symbolique demandée.
    ''' 	Value Type: string
    ''' </param> 
    ''' <param name="strProfilCourant">
    ''' 	Profil (groupe) de la clé symbolique demandée.
    ''' 	Value Type: string
    ''' </param>
    ''' <param name="strCodeVerifCourant">
    ''' 	Le code de vérification de la clé symbolique que l'on retrouve dans le fichier
    '''     XML qui nous permet d'offrir une validation supplémentaire avant de retourner
    '''     le code d'accès.  On utilise ce code de vérification seulement dans le type 
    '''     "Inforoute avec Vérification".
    ''' 	Value Type: string
    ''' </param>
    ''' <param name="strCodeVerif">
    ''' 	Le code de vérification transmis par l'usager qui nous permet d'offrir une 
    '''     validation supplémentaire avant de retourner le code d'accès.  
    '''     On utilise ce code seulement dans le type "Inforoute avec Vérification".
    ''' 	Value Type: string
    ''' </param>
    ''' <returns>
    '''     Valeur booléenne qui indique si l'utilisateur a accès
    ''' </returns>
    ''' <remarks>
    ''' Historique des modifications: 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' --------------------------------------------------------------------------------
    ''' 2007-11-26	t206500		Normaliser la méthode
    '''                         -  Vérifier si l'utilsateur est dans le groupe de sécurité 
    '''                         en même temps que l'on vérifie si il est dans le profil de
    '''                         la clé symbolique.  Cela évite d'accéder l'ActiveDirectory
    '''                         deux fois. De plus, on détermine la valeur du domaine dans 
    '''                         cette méthode et on le transmet à EstMembreDe. Afin d'éviter
    '''                         de le demander deux fois.
    ''' 
    ''' 2009-12-08  t206500     Finalement, on effectue un deuxième appel à la méthode 
    '''                         EstMembreDe avec le groupe de sécurité, si on n'a pas trouvé 
    '''                         le profil recherché.
    '''                         Dans la méthode EstMembreDe, on effectue un appel à l'AD 
    '''                         seulement pour convertir le groupe recherché en SID.  
    '''                         Plus d'avantage à tout faire dans le même appel.
    ''' 
    ''' 2010-01-11  t206500     Utilisation de la constante NOM_GROUPE_ADMIN_SECURITE
    ''' --------------------------------------------------------------------------------
    ''' </remarks>
    Friend Function ValiderAcces(ByVal strUsager As String,
                                ByVal strCleAcces As String,
                                ByVal strTypeCourant As String,
                                ByVal strProfilCourant As String,
                                ByVal strCodeVerifCourant As String,
                                ByVal strCodeVerif As String) As Boolean

        If strTypeCourant.Equals(Zone.Domaine.Code) Or strTypeCourant.Equals(Zone.HorsDomaine.Code) Then
            ' Dans la zone Domaine ('D'), on valide si l'utilisateur a accès au profil de la clé demandée.
            If strCleAcces.Length < 10 Then strCleAcces = strCleAcces.PadRight(10)

            Dim sub10 As String = strCleAcces.ToUpper.Substring(0, 10)
            Dim modeExecution As String = ConfigurationManager.AppSettings.Get("EE1\EE1I111\ModeExecution")

            If sub10.Est("EE1ADMSEEL") AndAlso modeExecution.Est("EeAwcMeExterne") Then
                Return True
            Else
                Dim strProfils() As String = strProfilCourant.Split(","c)

                ' Si est membre d'au moins un profil, on donne l'authorisation
                For Each profil As String In strProfils
                    If estMembreDe(strUsager, profil) Then
                        Return True
                    End If
                Next

                ' si il n'est pas dans le profil courant, on vérifie s'il est dans le groupe sécurité
                If estMembreDe(strUsager, NOM_GROUPE_ADMIN_SECURITE) Then
                    Return True
                Else
                    Return False
                End If
            End If

        Else
            ' Dans la zone INFOROUTE ('I') ou ('IV'), on retourne la clé sans vérifier le profil. 
            Dim typeCourant As Zone = Zone.Parse(strTypeCourant)

            ' Ils ont accès à toutes les clés de l'inforoute.
            If typeCourant.Est(Zone.Inforoute) Then Return True

            ' Si le code de vérification est nécessaire
            If typeCourant.Est(Zone.InforouteAvecVerification) Then
                If strCodeVerif.Length = 0 Then Throw New TsCuCodeVerifAbsent
                If Not strCodeVerif.Est(strCodeVerifCourant) Then Throw New TsCuCodeVerifInvalide()
                Return True
            End If
        End If
    End Function

#End Region

#Region "*-----    Méthodes Privées   -----*"



    ''' <summary>
    ''' Vérifier si l'usager est dans le profil de la clé symbolique demandée.
    ''' </summary>
    ''' <param name="strUsager">
    '''     Code de l'usager dont on désire obtenir la liste des groupes
    ''' 	Value Type: string
    ''' </param>
    ''' <param name="strGroupeRech">
    '''     Groupe (profil) de la clé symbolique demandée
    ''' 	Value Type: string
    ''' </param>
    ''' <returns>
    ''' Valeur booléenne qui indique si l'usager est dans le profil de la clé symbolique
    ''' demandée (ou du groupe "AdmResp Securite")
    ''' </returns>
    ''' <remarks>
    ''' Historique des modifications: 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' --------------------------------------------------------------------------------
    ''' 2007-11-26	t206500		Normaliser la méthode
    '''                         Permettre de lui transmettre un deuxième groupe pour 
    '''                         y effectuer la validation en même temps  
    ''' 
    ''' 2009-12-08  t206500     Effectuer une transformation du groupe recherche en SID
    '''                         On compare les SIDs.  Les groupes dans le token de sécurité
    '''                         de l'usager sont conservés en SID.
    '''                         Donc, de cette façon, on effectue seulement un TRANSLATE.
    '''                         Avant, on effectuait une transformation en caractère 
    '''                         de chaque groupe du token.  
    '''                      
    '''                         Enlever la transmission d'un deuxième groupe (trop compliqué
    '''                         à gérer avec la requête LINK) et de toute façon cela ne
    '''                         sauve pas vraiment du temps.  Il faut quand même faire
    '''                         le TRANSLATE du deuxième groupe.  
    ''' 
    ''' 2010-01-25  t206500     Le compte SYSTEM  a accès à toutes les clés symboliques
    '''                         lorsque c'est le composant TS5N132_ZpGerCOMPlus qui 
    '''                         effectue l'appel à TS1N223.
    ''' --------------------------------------------------------------------------------
    ''' </remarks>
    Private Function estMembreDe(ByVal strUsager As String, ByVal strGroupeRech As String) As Boolean
        'On vérifie l'exception immédiatement.  Lors de la graduation WEB, il utilise l'outil TS5N131_ZpComPlus.
        'Cet outil sécurise les lots COM+.  Il fait appel à TS1N223 pour obtenir le code accès/mot de passe 
        'de l'identité du lot à sécuriser.  Cet outil roule avec le compte SYSTEM.
        If WindowsIdentity.GetCurrent.IsSystem And Process.GetCurrentProcess.ProcessName.Est("TS5N132_ZpGerCOMPlus") Then
            'si c'est le compte SYSTEM et que l'appel provient de l'outil de sécurité qui gère les
            'comptes COM+, on autorise l'accès à toutes les clés symboliques sans validation du profil d'accès
            Return True
        End If

        Return estMembreGroupe(strUsager, strGroupeRech)
    End Function

    <MethodImpl(MethodImplOptions.NoInlining)>
    Private Function estMembreGroupe(ByVal strUsager As String, ByVal strGroupeRech As String) As Boolean
        Try
            Dim objVerifSec As New TsCaVerfrSecrtApplicative()
            Return objVerifSec.EstMembreGroupe(strGroupeRech, strUsager)

        Catch ex As TsCuGroupeSecuriteInexistantException
            Return False
        Catch ex1 As TsCuUtilisateurInexistantException
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Obtenir le fichier des codes accès sur le disque.
    ''' </summary>
    ''' <param name="strNomFichCdAcc">
    ''' 	Nom du fichier XML qui contient les informations des clés symboliques TS1N213.xml
    ''' 	Value Type: string
    ''' </param>
    ''' <remarks>
    ''' Historique des modifications: 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' --------------------------------------------------------------------------------
    ''' 2009-12-01	t206500		Création de la méthode
    '''                         Isoler l'accès sur disque dans cette méthode
    '''                         Initialisation de la prochaine vérification du 
    '''                         changement du dépôt.  On lit le dépôt aux 15 minutes
    ''' 
    ''' 2010-01-13  T206500     Réfaire les validations avant d'effectuer le chargement
    '''                         Utiliser un dataset temporaire afin d'éviter qu'un autre
    '''                         process utilise le dataset pendant le chargement.
    ''' --------------------------------------------------------------------------------
    ''' </remarks>
    Private Sub chargerFichierCodeAcces(ByVal strNomFichCdAcc As String)
        Const MAX_ESSAIS As Integer = 10

        Using donneesTemporaires As New DataSet()
            ' On doit refaire les validations pour s'assurer que l'on ne charge pas inutilement.
            ' Si on avait deux appels en même temps, le deuxième a attendu parce que nous
            ' avons un synclock.  Par contre, on ne veut pas que le chargement s'effectue deux fois.  
            ' L'appel précédent a déjà chargé en mémoire le dataset.
            Dim cacheVide As Boolean = tsCuVarShared.dsCachedCodeAcces Is Nothing
            Dim nomFichierDifferent As Boolean = Not (tsCuVarShared.strDernNomFichCdAcc.Est(strNomFichCdAcc))
            Dim fichierEteModifie As Boolean = tsCuVarShared.dtDernModifDepot <> File.GetLastWriteTime(strNomFichCdAcc)

            If (cacheVide) OrElse (nomFichierDifferent) OrElse (fichierEteModifie) Then
                Dim dicCodeAcces As New Dictionary(Of String, DataRow)()
                Dim exceptionOriginal As Exception = Nothing
                Dim succes As Boolean = False

                Dim quantiteEssais As Integer = 0
                Do While quantiteEssais < MAX_ESSAIS
                    Try

                        Using objFS As FileStream = New FileStream(strNomFichCdAcc, FileMode.Open, FileAccess.Read, FileShare.None)
                            donneesTemporaires.ReadXml(objFS)

                            For Each row As DataRow In donneesTemporaires.Tables("CdAcces").Rows
                                dicCodeAcces.Add(row.GenererCle(), row)
                            Next
                            succes = True
                        End Using

                    Catch ex As Exception
                        exceptionOriginal = ex
                        Threading.Thread.Sleep(100)
                        quantiteEssais += 1
                    End Try

                    If succes Then Exit Do
                Loop

                If Not succes Then
                    Throw New TsCuLectureDepotImpossible(exceptionOriginal, strNomFichCdAcc)
                End If

                ''---------------------------------------------------------------------------------------------------------
                '' 2010-01-13   -  T206500  -  Manon Jalbert  
                '' Transférer les instructions suivantes à la fin lorsque le fichier est chargé dans le dataset temporaire.
                '' Rafraichir les informations conservées en mémoire.
                ''---------------------------------------------------------------------------------------------------------
                ' Défini un clé primaire pour accélérer la recherche
                donneesTemporaires.Tables("CdAcces").PrimaryKey = {donneesTemporaires.Tables("CdAcces").Columns("Cle"), donneesTemporaires.Tables("CdAcces").Columns("Type")}

                ' Rafraichir le dateset des codes d'accès en mémoire
                tsCuVarShared.dsCachedCodeAcces = donneesTemporaires.Copy
                tsCuVarShared.dicCachedCodeAcces = dicCodeAcces

                ' Obtenir la dernière date de modification du fichier et la conserver en mémoire
                tsCuVarShared.dtDernModifDepot = File.GetLastWriteTime(strNomFichCdAcc)

                ' Déterminer l'heure de la prochaine vérification du changement du dépot
                ' --> On utilise Nullable afin de s'assurer que la date n'est pas partielle lors de la vérification
                tsCuVarShared.dtProchVerifModifDepot = New Nullable(Of DateTime)(DateTime.Now.AddMinutes(DELAI_VERIF_FICHIER_CODE_ACCES))

                ' Conserver le nom du fichier des codes d'accès en mémoire
                tsCuVarShared.strDernNomFichCdAcc = strNomFichCdAcc
            End If

        End Using ' Libérer le dataset temporaire
    End Sub

#End Region

#Region " DisposableContainer(Of T) "

    Private Class DisposableContainer(Of T As IDisposable)
        Implements IDisposable
        Private ReadOnly _principal As T
        Private ReadOnly _disposables() As IDisposable

        Public Sub New(principal As T, ParamArray disposables() As IDisposable)
            _principal = principal
            _disposables = disposables
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Using _principal : End Using
            For Each disposable As IDisposable In _disposables
                Using disposable : End Using
            Next
        End Sub

        Public Shared Widening Operator CType(ByVal source As DisposableContainer(Of T)) As T
            Return source._principal
        End Operator

    End Class

#End Region

End Class