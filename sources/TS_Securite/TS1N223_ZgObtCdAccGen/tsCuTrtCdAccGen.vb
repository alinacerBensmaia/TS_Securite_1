Imports Rrq.InfrastructureCommune.Parametres
Imports System.IO

''' <summary>
''' Elle nous permet de gérer les codes d'accès génériques.  Les données concernant les 
''' codes d'accès génériques se retrouvent dans un fichier xml protégé.  Seulement, un 
''' groupe particulier (sécurité) a accès à la gestion des codes d'accès génériques.
''' 
''' Pour effectuer la gestion des codes d'accès génériques, les informations nous sont 
''' transmises via les propriétés.  Toutes les valeurs véhiculent par ces propriétés.
''' </summary>
''' <remarks>
''' Historique des modifications: 
''' --------------------------------------------------------------------------------
''' Date		Nom			Description
''' 
''' --------------------------------------------------------------------------------
''' 2007-11-26	T206500		Normaliser cette classe
'''                         -  On obtient les paramètres de configuration directement
'''                         de l'utilitaire XU4N011_Configuration 
'''                         -  On a enlevé la classe tsCuCompte.  Tout le code relatif 
'''                         à la gestion des clés est transféré ici.  Cela évite de 
'''                         conserver un état inutile dans tsCuCompte.  Les méthodes
'''                         sont maintenant privées dans cette classe.
''' 
''' 2010-01-21  t206500    Utiliser la méthode ObtenirCodeUsager pour obtenir le code
'''                        utilisateur. On fait une exception lorsque c'est le compte SYSTEM.                         
''' 
''' </remarks>
<Microsoft.VisualBasic.ComClass(tsCuTrtCdAccGen.ClassId, tsCuTrtCdAccGen.InterfaceId, tsCuTrtCdAccGen.EventsId)>
Public Class tsCuTrtCdAccGen

#Region "*-----       COM GUIDs       -----*"

    ' These  GUIDs provide the COM identity for this class and its COM interfaces. If you change them, existing clients will no longer be able to access the class.
    Public Const ClassId As String = "D0FB190B-1714-42ba-BEAA-2A7A0ECB22EF"
    Public Const InterfaceId As String = "CD918CC7-98F2-4ff0-91FF-2EB90A430491"
    Public Const EventsId As String = "932AE6EF-7370-4db2-9FFC-1E1D8786E6E3"

#End Region

#Region "*---- Variables d'énumération ----*"

    Public Enum TypeEtat
        Ajout = 0
        Modification = 1
    End Enum

#End Region

#Region "*-----   Variables privées   -----*"

    Private objInfoUtil As New tsCuInfoUtil

    Private _DataSetCdAcces As DataSet
    Private _CleAcces As String
    Private _CodeAcces As String
    Private _CodeVerification As String
    Private _Commentaire As String
    Private _Description As String
    Private _Environnement As String
    Private _MotDePasse As String
    Private _Profil As String
    Private _TypeCompte As Zone
    Private _TypeCompteComplet As String

#End Region

#Region "*-----  Propriétés publiques -----*"

    ''' <summary>
    ''' Transmettre le dataset comprenant les informations relatives de tous les codes
    ''' d'accès génériques.  Lors de la modification et de la suppression d'une clé, la clé
    ''' est ajoutée dans le dataset par l'application appelante (TS1N213_ZgGerCdAccGen).
    ''' TS1N213 nous transmet le dataset via cette propriété.
    ''' </summary>
    ''' <value>
    ''' le dataset complet qui comprend toutes les informations des codes d'accès génériques
    ''' </value>
    Public WriteOnly Property DataSetCdAcces() As DataSet
        Set(ByVal Value As DataSet)
            _DataSetCdAcces = Value
        End Set
    End Property

    ''' <summary>
    ''' État de la mise à jour, soit la valeur 0 pour l'ajout ou 1 pour la modification
    ''' </summary>
    ''' <value>
    ''' l'état de la mise à jour
    ''' </value>
    Public Property Etat() As TypeEtat

    ''' <summary>
    ''' Transmettre la clé symbolique que l'on désire gérer  (ajouter, modifier, supprimer)
    ''' </summary>
    ''' <value>
    ''' clé d'accès symbolique
    ''' </value>
    Public WriteOnly Property CleAcces() As String
        Set(ByVal Value As String)
            _CleAcces = Value
        End Set
    End Property

    ''' <summary>
    ''' Transmettre le code d'accès de la clé symbolique gérée (ajouter, modifier, supprimer)
    ''' </summary>
    ''' <value>
    ''' code d'accès
    ''' </value>
    Public WriteOnly Property CodeAcces() As String
        Set(ByVal Value As String)
            _CodeAcces = Value
        End Set
    End Property

    ''' <summary>
    ''' Transmettre le code de vérification de la clé symbolique gérée (ajouter, modifier, 
    ''' supprimer).  Ce code de vérification est utilisée seulement dans le type "Inforoute
    ''' avec Vérification".  Cela apporte une validation supplémentaire avant d'obtenir
    ''' les informations de cette clé symbolique.
    ''' </summary>
    ''' <value>
    ''' code de vérification
    ''' </value>
    Public WriteOnly Property CodeVerification() As String
        Set(ByVal Value As String)
            _CodeVerification = Value
        End Set
    End Property

    ''' <summary>
    ''' Transmettre les commentaires de la clé symbolique gérée (ajouter, modifier, supprimer).  
    ''' </summary>
    ''' <value>
    ''' commentaire
    ''' </value>
    Public WriteOnly Property Commentaire() As String
        Set(ByVal Value As String)
            _Commentaire = Value
        End Set
    End Property

    ''' <summary>
    ''' Transmettre la description de la clé symbolique gérée (ajouter, modifier, supprimer).  
    ''' </summary>
    ''' <value>
    ''' description
    ''' </value>
    Public WriteOnly Property Description() As String
        Set(ByVal Value As String)
            _Description = Value
        End Set
    End Property

    ''' <summary>
    ''' Transmettre l'environnement de la clé symbolique gérée (ajouter, modifier, supprimer).
    ''' </summary>
    ''' <value>
    ''' environnement
    ''' </value>
    Public WriteOnly Property Environnement() As String
        Set(ByVal Value As String)
            _Environnement = Value
        End Set
    End Property

    ''' <summary>
    ''' Transmettre le mot de passe de la clé symbolique gérée (ajouter, modifier, supprimer).  
    ''' </summary>
    ''' <value>
    ''' mot de passe
    ''' </value>
    Public WriteOnly Property MotDePasse() As String
        Set(ByVal Value As String)
            _MotDePasse = Value
        End Set
    End Property

    ''' <summary>
    ''' Transmettre le profil de la clé symbolique gérée (ajouter, modifier, supprimer).  
    ''' </summary>
    ''' <value>
    ''' profil
    ''' </value>
    Public WriteOnly Property Profil() As String
        Set(ByVal Value As String)
            _Profil = Value
        End Set
    End Property

    ''' <summary>
    ''' Transmettre le type de compte de la clé symbolique gérée (ajouter, modifier, supprimer).  
    ''' Il existe quatre types:     D  -->  Domaine  
    '''                             H  -->  Hors Domaine
    '''                             I  -->  Inforoute 
    '''                             IV -->  Inforoute avec vérification
    ''' </summary>
    ''' <value>
    ''' type de compte
    ''' </value>
    Public WriteOnly Property TypeCompte() As Zone
        Set(ByVal Value As Zone)
            _TypeCompte = Value
        End Set
    End Property

    ''' <summary>
    ''' Transmettre le type de compte de la clé symbolique gérée (ajouter, modifier, supprimer).  
    ''' Il existe quatre types:  Domaine,Hors Domaine, Inforoute et Inforoute avec vérification
    ''' </summary>
    ''' <value>
    ''' type de compte complet
    ''' </value>
    Public WriteOnly Property TypeCompteComplet() As String
        Set(ByVal Value As String)
            _TypeCompteComplet = Value
        End Set
    End Property

#End Region

#Region "*-----  Méthodes publiques   -----*"

    ''' <summary>
    ''' Obtenir le dataset qui contient toutes les informations de tous les codes d'accès
    ''' génériques.   L'application TS1N213_ZgGerCdAccGen obtient le dataset pour y
    ''' effectuer le traitement nécessaire.
    ''' </summary>
    ''' <param name="objDSLoc">
    '''     dataset complet de tous les codes d'accès génériques 	
    ''' 	Value Type: dataset
    ''' </param>
    ''' <remarks>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2007-11-26	t206500		Normaliser la méthode
    '''                         Retourner le dataset seulement à l'équipe sécurité
    '''                         ie. groupe "AdmResp Securite"
    '''                         Retourner une copie du dataset, parce que l'outil
    '''                         effectue des ajustements dans ce dataset
    '''                         
    ''' 2010-01-21  t206500    Utiliser la méthode ObtenirCodeUsager pour obtenir le code
    '''                        utilisateur. On fait une exception lorsque c'est le compte SYSTEM. 
    ''' 
    ''' </remarks>
    Public Sub ObtenirDS(ByRef objDSLoc As DataSet)
        Try
            Dim codeUsager As String = objInfoUtil.ObtenirCodeUsager()

            'Vérifier si l'utilisateur a les droits nécessaires avant de lui retourner tous les codes d'accès génériques
            If objInfoUtil.ValiderAdmin(codeUsager, "") Then
                'Lire le fichier des codes acces et lui retourner
                Using dsObtCdAcc As DataSet = objInfoUtil.LireFichierCodeAcces()
                    'Retourner une copie du dataset pour ne pas que les modifications effectuées
                    'par l'outil soient automatiquement répercutées dans mon dataset
                    objDSLoc = dsObtCdAcc.Copy
                End Using

            Else
                Throw New TsCuDroitsGestionInsuffisants(codeUsager)
            End If

        Catch ex As Exception
            Dim objJournalisation As New tsCuJournalisation()
            objJournalisation.EcrireJournal(ex)
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' Effectuer la mise-à-jour de la clé symbolique traitée.  On appelle cette méthode
    ''' lors de l'ajout d'une nouvelle clé symbolique et de la modification d'une clé
    ''' existante.  
    ''' 
    ''' Lors de l'ajout, les données concernant la nouvelle clé nous sont transmises 
    ''' via les propriétés.  On doit ajouter la nouvelle ligne dans le dataset.
    ''' 
    ''' Lors de la modification, la modification est effectuée par l'application TS1N213.
    ''' Donc, on accepte les changements dans le dataset sans se poser de question ...
    ''' 
    ''' </summary>
    ''' <returns>
    ''' On y retourne l'état de la méthode:  0 pour ajouter et 1 pour modifier.
    ''' Cette valeur n'est pas utilisée dans TS1N213.  On conserve pour compatibilité
    ''' </returns>
    ''' 
    ''' <remarks>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2007-11-26	t206500		Normaliser la méthode
    '''                         -  Fusionner le code des méthodes MettreAJour et Ajouter
    '''                         dans cette méthode
    '''                         -  La classe tsCuCompte n'existe plus.  Donc, on n'a plus
    '''                         à conserver d'état dans tsCuCompte.  Tout le traitement
    '''                         de gestion des comptes s'effectue dans cette classe.
    '''                         -  Effectuer la validation du chemin d'accès à l'inforoute
    '''                         avant de débuter le traitement. Si le fichier n'existe pas, 
    '''                         il ne faut pas effectuer la modification dans le fichier 
    '''                         complet. 
    ''' 
    ''' 2010-01-21  t206500    Utiliser la méthode ObtenirCodeUsager pour obtenir le code
    '''                        utilisateur. On fait une exception lorsque c'est le compte SYSTEM.                         
    ''' 
    ''' </remarks>
    Public Function SauvegardeCompte() As Integer
        Dim journal As New tsCuJournalisation()
        Dim cheminFichierInforoute As String = String.Empty
        Dim cheminFichierZDE As String = String.Empty

        Try
            Dim codeUsager As String = objInfoUtil.ObtenirCodeUsager

            'Si compte Inforoute, valider si le fichier inforoute et ZDE sont présents
            If _TypeCompte.Est(Zone.Inforoute.Ou(Zone.InforouteAvecVerification)) Then
                validerFichierInforoute(cheminFichierInforoute, cheminFichierZDE)
            End If

            'Vérifier si l'utilisateur a les droits nécessaires 
            If objInfoUtil.ValiderAdmin(codeUsager, _CleAcces) Then

                If Etat = TypeEtat.Ajout Then
                    ajouter()

                    'Si compte de type I ou IV, modifier les fichiers de l'inforoute et ZDE
                    If _TypeCompte.Est(Zone.Inforoute.Ou(Zone.InforouteAvecVerification)) Then
                        traiterFichierInforoute(cheminFichierInforoute, cheminFichierZDE)
                    End If
                    journal.EcrireJournal(_TypeCompteComplet, _CleAcces, _Profil)

                Else
                    modifier()

                    'Si compte de type I ou IV, modifier les fichiers de l'inforoute et ZDE
                    If _TypeCompte.Est(Zone.Inforoute.Ou(Zone.InforouteAvecVerification)) Then
                        traiterFichierInforoute(cheminFichierInforoute, cheminFichierZDE)
                    End If
                    journal.EcrireJournal(_TypeCompteComplet, _CleAcces, 105)

                End If

            Else
                journal.EcrireJournal(_TypeCompteComplet, _CleAcces, codeUsager, "HAUT")
                Throw New TsCuDroitsGestionInsuffisants(codeUsager)
            End If

        Catch ex As Exception
            _DataSetCdAcces.RejectChanges()

            journal.EcrireJournal(ex)
            Throw

        Finally
            If Not journal Is Nothing Then journal = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Effectuer la supression de la clé symbolique traitée.  
    ''' La suppression de la ligne est effectuée par l'application TS1N213.  
    ''' Donc, on accepte les changements dans le dataset sans se poser de question...
    ''' </summary>
    ''' <returns>
    ''' On ne retourne rien.  Elle n'est pas traitée comme une fonction par TS1N213.
    ''' On conserve seulement pour compatibilité.
    ''' </returns>
    ''' 
    ''' <remarks>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2007-11-26	t206500		Normaliser la méthode
    '''                         -  Ramener le code de Supprimer dans cette méthode
    '''                         -  La classe tsCuCompte n'existe plus.  Donc, on n'a plus
    '''                         à conserver d'état dans tsCuCompte.  Tout le traitement
    '''                         de gestion des comptes s'effectue dans cette classe. 
    '''                         -  Effectuer la validation du chemin d'accès à l'inforoute
    '''                         avant de débuter le traitement. Si le fichier n'existe pas, 
    '''                         il ne faut pas effectuer la modification dans le fichier 
    '''                         complet. 
    '''                         
    ''' 2010-01-21  t206500    Utiliser la méthode ObtenirCodeUsager pour obtenir le code
    '''                        utilisateur. On fait une exception lorsque c'est le compte SYSTEM. 
    ''' 
    ''' </remarks>
    Public Function SupprimerCompte() As Integer
        Dim journal As New tsCuJournalisation()
        Dim cheminFichierInforoute As String = String.Empty
        Dim cheminFichierZDE As String = String.Empty

        Try
            Dim codeUsager As String = objInfoUtil.ObtenirCodeUsager

            'Si compte Inforoute, valider si le fichier inforoute et ZDE sont présents
            If _TypeCompte.Est(Zone.Inforoute.Ou(Zone.InforouteAvecVerification)) Then
                validerFichierInforoute(cheminFichierInforoute, cheminFichierZDE)
            End If

            'Vérifier si l'utilisateur a les droits nécessaires
            If objInfoUtil.ValiderAdmin(codeUsager, _CleAcces) Then
                supprimer()

                'Si compte de type I ou IV, modifier les fichiers de l'inforoute et ZDE
                If _TypeCompte.Est(Zone.Inforoute.Ou(Zone.InforouteAvecVerification)) Then
                    traiterFichierInforoute(cheminFichierInforoute, cheminFichierZDE)
                End If

                journal.EcrireJournal(_TypeCompteComplet, _CleAcces, 106)

            Else
                journal.EcrireJournal(_TypeCompteComplet, _CleAcces, codeUsager, "HAUT")
                Throw New TsCuDroitsGestionInsuffisants(codeUsager)
            End If

        Catch ex As Exception
            _DataSetCdAcces.RejectChanges()
            journal.EcrireJournal(ex)
            Throw

        Finally
            If Not journal Is Nothing Then journal = Nothing
        End Try
    End Function

#End Region

#Region "*-----   Méthodes privées    -----*"

    ''' <summary>
    ''' Effectuer l'ajout de la clé symbolique demandée.
    ''' TS1N213 vérifie si le type et la clé symbolique sont uniques avant de demander
    ''' l'ajout de la clé symbolique.  On n'a pas à effectuer de validation.
    ''' Inscrire le dataset des codes d'accès génériques dans le fichier complet.
    ''' Si le type de la clé symbolique est "Inforoute" ou "Inforoute avec vérification",
    ''' inscrire le dataset dans les fichiers de l'environnement Inforoute et ZDE.  
    ''' </summary>
    ''' <remarks>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2007-11-29	t206500		Normalisation de la méthode
    '''                         Inscrire les codes d'accès génériques de type 'I' et 'IV'
    '''                         dans le fichier Inforoute et ZDE.
    '''                         Utiliser le même dataset temporaire pour l'inforoute et 
    '''                         ZDE 
    '''                         On obtient les paramètres de configuration directement
    '''                         de l'utilitaire XU4N011_Configuration
    '''                         
    ''' 
    ''' </remarks>
    Private Sub ajouter()
        Dim strFichCdAcc As String = XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N223\PASSWDFILE")

        'Lors d'un ajout, TS1N213 (Interface graphique) n'ajoute pas la nouvelle ligne...
        'Il nous transmet le dataset sans le nouveau compte.
        'Donc, on fait le changement dans le dataset.

        '!!!!!!!!!!!!!!  SPÉCIFICATIONS  !!!!!!!!!!!!!!!   ATTENTION   !!!!!!!!!!!!!!
        ' TS1N213 a vérifié si la clé existe avec le même type dans la table CdAcces
        ' avant de nous le transmettre. La clé d'accès peut être créée avec deux types 
        ' différents.  Cela peut causer un problème.
        ' Si on appelle la méthode "ObtenirCodeAccesMotDePasse", on lui transmet seulement
        ' une clé symbolique. Cette méthode retourne une ligne, sinon une erreur est générée.
        ' Si une clé d'accès existe avec le type D et H, "ObtenirInfoCompte" va trouver 2
        ' clés.  L'utilisateur ne peut pas préciser sa recherche.  Il ne pourra jamais 
        ' obtenir sa clé.  L'équipe de sécurité doit porter une attention particulière
        ' lors de la création d'une clé d'accès.  Il est possible qu'une même clé existe,
        ' mais dans deux zones différentes (ie. Domaine et Inforoute).
        '!!!!!!!!!!!!!!  SPÉCIFICATIONS  !!!!!!!!!!!!!!!   ATTENTION   !!!!!!!!!!!!!!

        Dim drNouv As DataRow = _DataSetCdAcces.Tables("CdAcces").NewRow

        'Créer le nouveau compte dans le dataset des codes accès
        drNouv.Item("Type") = _TypeCompte.Code
        drNouv.Item("Cle") = _CleAcces
        drNouv.Item("Envrn") = _Environnement
        drNouv.Item("Code") = _CodeAcces
        drNouv.Item("Mdp") = _MotDePasse
        drNouv.Item("Profil") = _Profil
        drNouv.Item("Desc") = _Description
        drNouv.Item("Comm") = _Commentaire
        drNouv.Item("CodeVerif") = _CodeVerification

        _DataSetCdAcces.Tables("CdAcces").Rows.Add(drNouv)
        _DataSetCdAcces.AcceptChanges()

        'Mettre à jour le fichier complet des codes accès 
        objInfoUtil.EcrireFichierCodeAcces(_DataSetCdAcces, strFichCdAcc)
    End Sub

    ''' <summary>
    ''' Effectuer la mise-à-jour de la clé symbolique demandée.
    ''' Inscrire le dataset des codes d'accès génériques dans le fichier complet.
    ''' Si le type de la clé symbolique est "Inforoute" ou "Inforoute avec vérification",
    ''' inscrire le dataset dans les fichiers de l'environnement Inforoute et ZDE.  
    ''' </summary>
    ''' <remarks>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2007-11-29	t206500		Normalisation de la méthode
    '''                         Inscrire les codes d'accès génériques de type 'I' et 'IV'
    '''                         dans le fichier Inforoute et ZDE.
    '''                         Utiliser le même dataset temporaire pour l'inforoute et 
    '''                         ZDE 
    '''                         On obtient les paramètres de configuration directement
    '''                         de l'utilitaire XU4N011_Configuration
    '''                         
    '''
    ''' </remarks>
    Private Sub modifier()
        Dim strFichCdAcc As String = XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N223\PASSWDFILE")

        ' TS1N213 (Interface graphique) modifie le dataset et nous le transmet 
        ' avant d'appeler MettreAJour.   Donc, on accepte les changements dans
        ' le DS sans se poser de question...
        _DataSetCdAcces.AcceptChanges()

        'Mettre à jour le fichier complet des codes accès
        objInfoUtil.EcrireFichierCodeAcces(_DataSetCdAcces, strFichCdAcc)
    End Sub

    ''' <summary>
    ''' Effectuer la suppression de la clé symbolique demandée.
    ''' Inscrire le dataset des codes d'accès génériques dans le fichier complet.
    ''' Si le type de la clé symbolique est "Inforoute" ou "Inforoute avec vérification",
    ''' inscrire le dataset dans les fichiers de l'environnement Inforoute et ZDE.  
    ''' </summary>
    ''' <remarks>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2007-11-29	t206500		Normalisation de la méthode
    '''                         Inscrire seulement les codes d'accès génériques de type 
    '''                         'I' et 'IV' dans le fichier Inforoute et ZDE.
    '''                         Utiliser le même dataset temporaire pour l'inforoute et 
    '''                         ZDE
    '''                         On obtient les paramètres de configuration directement
    '''                         de l'utilitaire XU4N011_Configuration
    '''                         
    '''
    ''' </remarks>
    Private Sub supprimer()
        Dim strFichCdAcc As String = XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N223\PASSWDFILE")

        ' TS1N213 (Interface graphique) modifie le dataset et nous le transmet avant
        ' d'appeler SupprimerCompte.   Donc, on accepte les changements dans le DS 
        ' sans se poser de question...
        _DataSetCdAcces.AcceptChanges()

        'Mettre à jour le fichier complet des codes accès
        objInfoUtil.EcrireFichierCodeAcces(_DataSetCdAcces, strFichCdAcc)
    End Sub


    '' *******************************************************************************
    '' 	Droits d'auteur 2008	ts1n223_zgobtcdaccgen. Tout droits réservés.
    '' *******************************************************************************
    '' <summary>
    '' Cette méthode nous permet de valider si les fichiers de l'inforoute et ZDE 
    '' existe avant d'effectuer le traitement (ajouter, modifier, supprimer) demandé
    '' </summary>
    '' <param name="strFichInforoute">
    '' 	Chemin du fichier des codes d'accès génériques de l'inforoute. 
    '' </param>
    '' <param name="strFichZDE">
    '' 	Chemin du fichier des codes d'accès génériques de ZDE. 
    '' </param>
    '' "exception cref="Rrq.CS.ServicesCommuns.ScenarioTransactionnel.XZCuRrqException">
    '' 	Cette exception est lancée si les chemins des fichiers de l'inforoute et ZDE
    '     sont absents ou invalides.
    '' </exception>
    '' <remarks>
    '' Historique des modifications: 
    '' <para><pre>
    '' --------------------------------------------------------------------------------
    '' Date		Nom			Description
    '' 
    '' --------------------------------------------------------------------------------
    '' 2008-02-05	t206500		Création initiale
    '' 
    '' </pre></para>
    '' </remarks>
    Private Sub validerFichierInforoute(ByRef strFichInforoute As String, ByRef strFichZDE As String)
        Try
            'Obtenir les noms des fichiers de l'inforoute 
            'si la n'existe pas l'erreur XuExcClefExiste est retournée
            strFichInforoute = XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N223\PASSWDFILEINFORTE")
            strFichZDE = XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N223\PASSWDFILEZDE")

            'Vérifier la longueur des noms de fichiers
            If strFichInforoute.Length = 0 Or strFichZDE.Length = 0 Then Throw New TsCuNomFichInforouteAbsent

            'Vérifier si le fichier de l'inforoute existe
            If Not (File.Exists(strFichInforoute)) Then Throw New TsCuNomFichInforouteAbsent

            'Vérifier si le fichier de l'inforoute ZDE existe
            If Not (File.Exists(strFichZDE)) Then Throw New TsCuNomFichInforouteAbsent

        Catch ex As XuExcCClefExistePas
            'Généraliser les messages d'erreurs
            Throw New TsCuNomFichInforouteAbsent
        End Try
    End Sub

    ''' <summary>
    ''' Inscrire les ajustements apportés à la clé symbolique traitée dans les fichiers 
    ''' de l'environnement Inforoute et ZDE.  
    ''' Actuellement, les fichiers sont les mêmes dans ces deux environnements.
    ''' On retrouve tous les codes d'accès génériques de type Inforoute ('I') et Inforoute
    ''' avec vérification ('IV') dans ces fichiers.
    ''' </summary>
    ''' <param name="strFichInforoute">
    ''' 	Chemin du fichier des codes d'accès génériques de l'inforoute. 
    ''' </param>
    ''' <param name="strFichZDE">
    ''' 	Chemin du fichier des codes d'accès génériques de ZDE. 
    ''' </param>
    ''' <remarks>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2007-11-26	t206500		Création de la méthode
    '''                         Lorsque la clé symbolique traitée est de type 'I' ou 'IV',
    '''                         on doit l'inscrire dans le fichier Inforoute et ZDE.
    '''                         On obtient les paramètres de configuration directement
    '''                         de l'utilitaire XU4N011_Configuration
    '''                             
    ''' </remarks>
    Private Sub traiterFichierInforoute(ByVal strFichInforoute As String, ByVal strFichZDE As String)
        'dataset qui contient que les clés de type Inforoute et Inforoute avec vérification
        Dim drsInforoute As DataRow() = Nothing
        Dim dsInforoute As DataSet = Nothing

        Try
            Dim zonesInforoute As New Zones({Zone.Inforoute, Zone.InforouteAvecVerification})
            Dim whereClause As String = String.Format("Type IN ({0})", zonesInforoute.ToStringList())

            'Créer le dataset qui contient que les clés de type Inforoute et Inforoute avec vérification
            drsInforoute = _DataSetCdAcces.Tables("CdAcces").Select(whereClause)
            dsInforoute = _DataSetCdAcces.Clone

            For Each drInforoute As DataRow In drsInforoute
                dsInforoute.Tables(0).ImportRow(drInforoute)
            Next
            dsInforoute.AcceptChanges()

            'Obtenir les données XML et le schema du Dataset pour ensuite les chiffrer dans un fichier pour l'inforoute
            objInfoUtil.EcrireFichierCodeAcces(dsInforoute, strFichInforoute)

            'Obtenir les données XML et le schema du Dataset pour ensuite les chiffrer dans un fichier pour l'inforoute ZDE
            objInfoUtil.EcrireFichierCodeAcces(dsInforoute, strFichZDE)

        Finally
            If Not drsInforoute Is Nothing Then drsInforoute = Nothing
            If Not dsInforoute Is Nothing Then dsInforoute.Dispose()
        End Try
    End Sub

#End Region

End Class
