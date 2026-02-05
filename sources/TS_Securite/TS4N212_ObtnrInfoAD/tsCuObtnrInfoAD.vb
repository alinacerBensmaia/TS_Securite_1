Imports System.DirectoryServices
Imports System.Collections.Generic

''' --------------------------------------------------------------------------------
''' Project:	TS4N212_ObtnrInfoAD
''' Class:	Securite.tsCuObtnrInfoAD
''' <summary>
'''     Cette classe utilitaire récupère et retourne des informations sur un
'''     ou des utilisateurs selon le contenu de l'active directory.
''' </summary>
''' <remarks><para><pre>
''' Historique des modifications: 
''' 
''' --------------------------------------------------------------------------------
''' Date		Nom			Description
''' 
''' --------------------------------------------------------------------------------
''' 2005-05-24	t209376		Création initiale
''' 2005-09-13  t209376     Modification de la méthode EstMembreDe, ObtenirListeDT, 
'''                         ObtenirListeRS, RechercheActiveDirectory et ObtenirListeInfo
''' 2005-09-13  t209376     Ajout de la méthode ObtenirListeGroupes
''' 2005-09-13  t209376     Modification dans la propriété NmServeurAD
''' 
''' </pre></para>
''' </remarks>
''' --------------------------------------------------------------------------------
Public Class tsCuObtnrInfoAD
    Private Const AD_sAMAccountName As String = "sAMAccountName"

    Private _accesseurAD As TsIObtnrInfoAD
    Private _utilisateurCourant As TsCuUtilisateurAD


#Region " Constructeurs "

    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	Securite.tsCuObtnrInfoAD.New
    ''' <summary>
    '''     Constructeur par défaut de la classe.  La procédure fait l'affectation
    '''     des valeurs par défaut des variables de travail.
    ''' </summary>
    ''' <remarks><para><pre>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2005-05-24	t209376		Création initiale
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    Public Sub New()
        Me.New(Environment.UserName.ToUpper)
    End Sub

    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	Securite.tsCuObtnrInfoAD.New
    ''' <summary>
    '''     Constructeur overloadé de la classe.  La procédure recoit le code
    '''     utilisateur à traiter pour obtenir un utilisateur unique.  Elle appel        
    '''     le constructeur de base et la routine de recherche de l'information.
    '''     **** ATTENTION: En utilisant cette méthode la recherche sera faite
    '''     l'AD de l'ex RRQ
    ''' </summary>
    ''' <param name="codeUtilisateur">
    ''' 	Code utilisateur unique dont on veut récupérer l'information.
    ''' 	Value Type: <see cref="String" />	(System.String)
    ''' </param>
    ''' <remarks><para><pre>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2005-05-24	t209376		Création initiale
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    Public Sub New(ByVal codeUtilisateur As String)
        _accesseurAD = New TsCuObtnrInfoADRQ()
        _utilisateurCourant = _accesseurAD.ObtenirUtilisateur(codeUtilisateur.ToUpper)
    End Sub

    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	Securite.tsCuObtnrInfoAD.New
    ''' <summary>
    '''     Constructeur overloadé de la classe.  La procédure recoit le code
    '''     utilisateur à traiter et son domaine pour obtenir un utilisateur unique.  Elle appel        
    '''     le constructeur de base et la routine de recherche de l'information.
    ''' </summary>
    ''' <param name="strCodeUtilisateur">
    ''' 	Code utilisateur unique dont on veut récupérer l'information.
    ''' 	Value Type: <see cref="String" />	(System.String)
    ''' </param>
    ''' <param name="domaineUtilisateur">
    ''' 	Nom du domaine où sera faire la recherche des informations (AD de l'ex CARRA ou
    '''     AD de l'ex RRQ)
    ''' 	Value Type: <see cref="TsIadNomDomaine" />	
    ''' </param>
    ''' <remarks><para><pre>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2016-08-29	t207479		Création initiale
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    <Obsolete(Desuet.UTILISER_CONSTRUCTEUR, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Sub New(ByVal strCodeUtilisateur As String, ByVal domaineUtilisateur As TsIadNomDomaine)
        Me.New(strCodeUtilisateur)
    End Sub

#End Region

#Region "Méthodes publiques - Valide"

    Public Shared Function UtilisateurExiste(codeUtilisateur As String) As Boolean
        Try
            Dim user As TsCuUtilisateurAD = ObtenirUtilisateur(codeUtilisateur)
            Return Not (user Is Nothing)
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function ObtenirUtilisateur() As TsCuUtilisateurAD
        Dim temp As New tsCuObtnrInfoAD()
        Return temp._utilisateurCourant
    End Function

    Public Shared Function ObtenirUtilisateur(codeUtilisateur As String) As TsCuUtilisateurAD
        Dim temp As New tsCuObtnrInfoAD(codeUtilisateur)
        Return temp._utilisateurCourant
    End Function

    Public Shared Function ObtenirUtilisateurs(ByVal pTypeRequete As TsIadTypeRequete, ByVal pCritereRecherche As String) As List(Of TsCuUtilisateurAD)
        Return ObtenirUtilisateurs(pTypeRequete, pCritereRecherche, String.Empty)
    End Function

    Public Shared Function ObtenirUtilisateurs(ByVal pTypeRequete As TsIadTypeRequete, ByVal pCritereRecherche As String, ByVal pCritereRechercheSecondaire As String) As List(Of TsCuUtilisateurAD)
        Dim temp As New tsCuObtnrInfoAD()
        Return temp._accesseurAD.ObtenirListeUtilisateur(pTypeRequete, pCritereRecherche, pCritereRechercheSecondaire, TsIadObjectCategory.TsIadOcPerson)
    End Function

    Public Shared Function ObtenirUtilisateurs(ByVal pTypeRequete As TsIadTypeRequete, ByVal pCritereRecherche As String, ByVal ExclureCompteAdmin As Boolean) As List(Of TsCuUtilisateurAD)
        Dim temp As New tsCuObtnrInfoAD()
        Dim listeUtilisateur As List(Of TsCuUtilisateurAD) = ObtenirUtilisateurs(pTypeRequete, pCritereRecherche, String.Empty)
        If ExclureCompteAdmin Then
            listeUtilisateur.RemoveAll(Function(x) x.EstCompteAdmin = True)
        End If
        Return listeUtilisateur
    End Function

    <Obsolete(Desuet.UTILISER_OBTENIRULT_SANS_DOMAINE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Friend Shared Function ObtenirUtilisateur(codeUtilisateur As String, ByVal domaine As TsIadNomDomaine) As TsCuUtilisateurAD
        Dim temp As New tsCuObtnrInfoAD(codeUtilisateur, domaine)
        Return temp._utilisateurCourant
    End Function

    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	Securite.tsCuObtnrInfoAD.ObtenirCodeUtilisateur
    ''' <summary>
    '''     Récupère le code utilisateur dans l'AD qui correspond au Sid reçu en paramètre, 
    '''     sans modifier les valeurs des propriétés de l'instance en cours.
    ''' </summary>
    ''' <param name="tblSID">
    ''' 	Tableau de Bytes qui contient le Sid d'un compte dans l'AD. 
    ''' 	Reference Type: <see cref="Byte" />	(System.Byte)
    ''' </param>
    ''' <returns><see cref="String" />	(System.String)</returns>
    ''' <remarks><para><pre>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2005-05-24	t209376		Création initiale
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    Public Shared Function ObtenirCodeUtilisateur(ByRef tblSID As Byte()) As String
        ' Convertir le SID en string afin d'avoir une clé pour le dictionnaire
        Dim sid As String = convertSidToString(tblSID)

        Dim ts As New tsCuObtnrInfoAD()
        Using dt As DataTable = ts._accesseurAD.RechercheActiveDirectory(TsIadTypeRequete.TsIadTrSid, sid, String.Empty, TsIadObjectCategory.TsIadOcPerson)
            If dt.Rows.Count = 1 Then
                If Not IsDBNull(dt.Rows(0).Item(AD_sAMAccountName)) Then
                    Return dt.Rows(0).Item(AD_sAMAccountName).ToString()
                End If
            End If

            Return ts._accesseurAD.ObtenirUtilisateur("N/A").CodeUtilisateur
        End Using
    End Function

#End Region

#Region " Fonctions et méthodes privées "

    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	Securite.tsCuObtnrInfoAD.ConvertirSid
    ''' <summary>
    '''     Cette fonction convertie le Sid en chaine de caractère.
    ''' </summary>
    ''' <param name="tblSID">
    ''' 	Tableau de Bytes qui contient le Sid d'un compte dans l'AD. 
    ''' 	Reference Type: <see cref="Byte" />	(System.Byte)
    ''' </param>
    ''' <returns><see cref="String" />	(System.String)</returns>
    ''' <remarks><para><pre>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2005-05-24	t209376		Création initiale
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    Private Shared Function convertSidToString(ByRef tblSID As Byte()) As String
        Dim cle As String = String.Empty
        Dim octet As String
        Dim compteur As Integer

        ' Convertir le SID en string
        For compteur = LBound(tblSID) To UBound(tblSID)
            If tblSID(compteur) < &H10 Then
                octet = "\0" & Hex(tblSID(compteur))
            Else
                octet = "\" & Hex(tblSID(compteur))
            End If
            cle = cle & octet
        Next compteur

        Return cle
    End Function

#End Region

#Region "Désuèt - Propriétés publiques"

    ''' --------------------------------------------------------------------------------
    ''' <summary>
    '''     Cette propriété reçoit ou retourne le code utilisateur traité.
    '''     Lorsqu'elle reçoit l'information, elle appelle la routine de recherche
    '''     de l'information d'un utilisateur unique.
    ''' </summary>
    ''' <value></value>
    ''' <remarks><para><pre>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2005-05-24	t209376		Création initiale
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    <Obsolete(Desuet.UTILISEZ_212_OBTENIR_UTILISATEUR, Desuet.OBSOLETE_USAGE_ALLOWED_TEMP)>
    Public Property CodeUtilisateur() As String
        Get
            Return _utilisateurCourant.CodeUtilisateur
        End Get
        Set(ByVal Value As String)
            Dim codeUtilisateur As String = Value.ToUpper
            _utilisateurCourant = _accesseurAD.ObtenirUtilisateur(codeUtilisateur)
        End Set
    End Property

    ''' --------------------------------------------------------------------------------
    ''' <summary>
    '''     Cette propriété retourne le nom de l'utilisateur tel que récupéré dans
    '''     l'active directory.
    ''' </summary>
    ''' <value></value>
    ''' <remarks><para><pre>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2005-05-24	t209376		Création initiale
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    <Obsolete(Desuet.UTILISEZ_212_OBTENIR_UTILISATEUR, Desuet.OBSOLETE_USAGE_ALLOWED_TEMP)>
    Public ReadOnly Property Nom() As String
        Get
            Return _utilisateurCourant.Nom
        End Get
    End Property

    ''' --------------------------------------------------------------------------------
    ''' <summary>
    '''     Cette propriété retourne le prénom de l'utilisateur tel que récupéré
    '''     dans l'active directory. 
    ''' </summary>
    ''' <value></value>
    ''' <remarks><para><pre>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2005-05-24	t209376		Création initiale
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    <Obsolete(Desuet.UTILISEZ_212_OBTENIR_UTILISATEUR, Desuet.OBSOLETE_USAGE_ALLOWED_TEMP)>
    Public ReadOnly Property Prenom() As String
        Get
            Return _utilisateurCourant.Prenom
        End Get
    End Property

    ''' --------------------------------------------------------------------------------
    ''' <summary>
    '''     Cette propriété retourne le nom complet (nom prénom (ua)) de l'utilisateur
    '''     tel que récupéré dans l'active directory.
    ''' </summary>
    ''' <value></value>
    ''' <remarks><para><pre>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2005-05-24	t209376		Création initiale
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    <Obsolete(Desuet.UTILISEZ_212_OBTENIR_UTILISATEUR, Desuet.OBSOLETE_USAGE_ALLOWED_TEMP)>
    Public ReadOnly Property NomComplet() As String
        Get
            Return _utilisateurCourant.NomComplet
        End Get
    End Property

    ''' --------------------------------------------------------------------------------
    ''' <summary>
    '''     Cette propriété retourne l'adresse courriel de l'utilisateur tel que
    '''     récupéré dans l'active directory.
    ''' </summary>
    ''' <value></value>
    ''' <remarks><para><pre>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2005-05-24	t209376		Création initiale
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    <Obsolete(Desuet.UTILISEZ_212_OBTENIR_UTILISATEUR, Desuet.OBSOLETE_USAGE_ALLOWED_TEMP)>
    Public ReadOnly Property Courriel() As String
        Get
            Return _utilisateurCourant.Courriel
        End Get
    End Property

    ''' --------------------------------------------------------------------------------
    ''' <summary>
    '''     Cette propriété retourne l'unité administrative de l'utilisateur tel
    '''     que récupéré dans l'active directory.
    ''' </summary>
    ''' <value></value>
    ''' <remarks><para><pre>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2005-05-24	t209376		Création initiale
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    <Obsolete(Desuet.UTILISEZ_212_OBTENIR_UTILISATEUR, Desuet.OBSOLETE_USAGE_ALLOWED_TEMP)>
    Public ReadOnly Property UniteAdmn() As String
        Get
            Return _utilisateurCourant.UniteAdmninistrative
        End Get
    End Property

    ''' --------------------------------------------------------------------------------
    ''' <summary>
    '''     Cette propriété retourne la fonction de l'utilisateur tel que
    '''     récupéré dans l'active directory.
    ''' </summary>
    ''' <value></value>
    ''' <remarks><para><pre>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2005-05-24	t209376		Création initiale
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    <Obsolete(Desuet.UTILISEZ_212_OBTENIR_UTILISATEUR, Desuet.OBSOLETE_USAGE_ALLOWED_TEMP)>
    Public ReadOnly Property Fonction() As String
        Get
            Return _utilisateurCourant.Fonction
        End Get
    End Property

    ''' --------------------------------------------------------------------------------
    ''' <summary>
    '''     Cette propriété retourne le nom du poste de travail de l'utilisateur tel que
    '''     récupéré des variables d'environnement.
    ''' </summary>
    ''' <value></value>
    ''' <remarks><para><pre>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2005-05-24	t209376		Création initiale
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    <Obsolete(Desuet.UTILISEZ_212_OBTENIR_UTILISATEUR, Desuet.OBSOLETE_USAGE_ALLOWED_TEMP)>
    Public ReadOnly Property NomPoste() As String
        Get
            Return _utilisateurCourant.NomPoste
        End Get
    End Property

    ''' --------------------------------------------------------------------------------
    ''' <summary>
    '''     Cette propriété retourne la société de l'utilisateur.
    ''' </summary>
    ''' <value></value>
    ''' <remarks><para><pre>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2006-06-20	T209376		Création initiale
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    <Obsolete(Desuet.UTILISEZ_212_OBTENIR_UTILISATEUR, Desuet.OBSOLETE_USAGE_ALLOWED_TEMP)>
    Public ReadOnly Property Societe() As String
        Get
            Return _utilisateurCourant.Societe
        End Get
    End Property

    ''' --------------------------------------------------------------------------------
    ''' <summary>
    '''     Cette propriété retourne la domaine de l'utilisateur.
    ''' </summary>
    ''' <value></value>
    ''' <remarks><para><pre>
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    <Obsolete(Desuet.MEMBRE_NON_UTILISE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public ReadOnly Property DomaineUtilisateur() As TsIadNomDomaine
        Get
            Return _utilisateurCourant.DomaineUtilisateur
        End Get
    End Property

    ''' --------------------------------------------------------------------------------
    ''' <summary>
    '''     Cette propriété retourne "VRAI" si l'utilisateur est desactivé dans l'Active 
    '''     Directory ou "FAUX" s'il est activé.
    ''' </summary>
    ''' <value></value>
    ''' <remarks><para><pre>
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    <Obsolete(Desuet.UTILISEZ_212_OBTENIR_UTILISATEUR, Desuet.OBSOLETE_USAGE_ALLOWED_TEMP)>
    Public ReadOnly Property UtilisateurDesactive() As Boolean
        Get
            Return _utilisateurCourant.UtilisateurDesactive
        End Get
    End Property

    ''' --------------------------------------------------------------------------------
    ''' <summary>
    '''     Cette propriété retourne le numéro d'employé
    ''' </summary>
    ''' <value></value>
    ''' <remarks><para><pre>
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    <Obsolete(Desuet.UTILISEZ_212_OBTENIR_UTILISATEUR, Desuet.OBSOLETE_USAGE_ALLOWED_TEMP)>
    Public ReadOnly Property NumeroEmploye() As String
        Get
            Return _utilisateurCourant.NumeroEmploye
        End Get
    End Property

#End Region

#Region "Désuèt - Publique Utilisé"

    <Obsolete(Desuet.UTILISEZ_COMPOSANT_213, Desuet.OBSOLETE_USAGE_ALLOWED_TEMP)>
    Public Function ObtenirMembresDuGroupe(ByVal strGroupe As String, ByVal blnRechRecursive As Boolean) As DataTable
        ''
        ''DE.RetentionPolicy, IN3I141_AffchUtilsSite
        ''  Ils utilisent pour obtenir les membre de groupe AD CARRA et RRQ
        ''
        Dim dtAD As Data.DataTable = Nothing

        Try

            dtAD = _accesseurAD.RechercheGroupeAD(strGroupe, blnRechRecursive)

            Return dtAD.Copy
        Finally
            ' Libérer les ressources utilisées
            If Not IsNothing(dtAD) Then
                dtAD.Dispose()
                dtAD = Nothing
            End If
        End Try
    End Function

    ''' <summary>
    '''  Cette méthode vérifie si un groupe existe dans l'AD.
    ''' </summary>
    ''' <param name="NomGroupe">Nom du groupe a vérifier</param>
    ''' <returns>True si le groupe existe et False si ce n'est pas le cas.</returns>
    <Obsolete(Desuet.UTILISEZ_COMPOSANT_213, Desuet.OBSOLETE_USAGE_ALLOWED_TEMP)>
    Public Function GroupeExiste(ByVal NomGroupe As String) As Boolean
        ''
        ''DE.RetentionPolicy, IN3I141_AffchUtilsSite
        ''  Ils valident que le groupe recherché existe bien dans l'AD CARRA et RRQ (groupe SPUser)
        ''
        Dim Resultats As SearchResultCollection

        Resultats = _accesseurAD.ObtenirListeGroupes(NomGroupe)

        If Resultats IsNot Nothing AndAlso Resultats.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	Securite.tsCuObtnrInfoAD.Domaine
    ''' <summary>
    ''' Cette méthode permet de determiner le domaine de l'active directory à accéder (ex RRQ ou 
    '''     ex CARRA) la recherche sera faire.
    ''' </summary>
    ''' <param name="NomDomaine">
    ''' 	Le nom domaine doit indique quel AD devra être utilisé: TsDomaineRRQ, TsDomaineCARRA ou sur les deux (TsMultiDomaine)
    ''' </param>
    <Obsolete(Desuet.UTILISEZ_212_OBTENIR_UTILISATEUR, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Sub Domaine(ByVal NomDomaine As TsIadNomDomaine)
        ''
        ''DE.RetentionPolicy, GE1N221_SaisirNASEmpl, IN3I141_AffchUtilsSite, Ni1I211_Utilitaires
        ''  DE : Utiliser pour assigner CARRA et RRQ
        ''  GE : MultiDomaine pour recherche par numéro employé
        ''  IN : Utiliser pour assigner CARRA et RRQ
        ''  NI : MultiDomaine
        ''
        _accesseurAD = New TsCuObtnrInfoADRQ()

    End Sub

    <Obsolete(Desuet.LISTE_DT, Desuet.OBSOLETE_USAGE_ALLOWED_TEMP)>
    Public Function ObtenirListeDT(ByVal pTypeRequete As TsIadTypeRequete, ByVal pCritereRecherche As String) As DataTable
        ''
        ''GE1N221_SaisirNASEmpl, Ni1I211_Utilitaires, XD8N171_PrConsulterFichier, XU2N713_ImpressionCentralise
        ''  GE : recherche utilisateur avec NoEmploye (multidomaine)
        ''  NI : Recherche par code utilisateur, lecture de toutes les propriétés incluant "memberOf"
        ''  XD : Utiliser pour vérifier si un utilisateur existe dans l'AD
        ''  XU : Utiliser pour vérifier si un utilisateur existe dans l'AD
        ''
        Return ObtenirListeDT(pTypeRequete, pCritereRecherche, String.Empty, TsIadObjectCategory.TsIadOcTous)
    End Function

    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	Securite.tsCuObtnrInfoAD.ObtenirListeDT
    ''' <summary>
    '''     Cette propriété retourne une liste d'utilisateurs sous format DataTable
    '''     récupéré dans l'active directory.  
    ''' </summary>
    ''' <param name="pTypeRequete">
    ''' 	Le champ sur lequel s'effectue la requête à l'active directory. 
    ''' 	Value Type: see cref="Securite.tsCuObtnrInfoAD.XzIadTypeRequete" />	(Rrq.Securite.tsCuObtnrInfoAD.XzIadTypeRequete)
    ''' </param>
    ''' <param name="pCritereRecherche">
    ''' 	Le critère de recherche à l'active directory. 
    ''' 	Value Type: <see cref="String" />	(System.String)
    ''' </param>
    ''' <param name="pCritereRechercheSecondaire">
    ''' 	Le critère de recherche secondaire à l'active directory utilisé lors d'une recherche nom et prénom. 
    '''     Value Type: <see cref="String" />	(System.String)
    ''' </param>
    ''' <returns><see cref="Data.DataTable" />	(System.Data.DataTable)</returns>
    ''' --------------------------------------------------------------------------------    
    <Obsolete(Desuet.LISTE_DT, Desuet.OBSOLETE_USAGE_ALLOWED_TEMP)>
    Public Function ObtenirListeDT(ByVal pTypeRequete As TsIadTypeRequete, ByVal pCritereRecherche As String, ByVal pCritereRechercheSecondaire As String) As DataTable
        ''
        ''GE1N221_SaisirNASEmpl, Ni1I211_Utilitaires, XD8N171_PrConsulterFichier, XU2N713_ImpressionCentralise
        ''  GE : recherche utilisateur avec NoEmploye (multidomaine)
        ''  NI : Recherche par code utilisateur, lecture de toutes les propriétés incluant "memberOf"
        ''  XD : Utiliser pour vérifier si un utilisateur existe dans l'AD
        ''  XU : Utiliser pour vérifier si un utilisateur existe dans l'AD
        ''
        Return ObtenirListeDT(pTypeRequete, pCritereRecherche, pCritereRechercheSecondaire, TsIadObjectCategory.TsIadOcTous)
    End Function

    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	Securite.tsCuObtnrInfoAD.EstMembreDe
    ''' <summary>
    '''     Vérifie si le groupe passé en paramètre fait partie des groupes dont est 
    '''     membre l'utilisateur de l'instance.
    ''' </summary>
    ''' <param name="strGroupe">
    ''' 	Groupe dont on désire vérifier l'appartenance. 
    ''' 	Value Type: <see cref="String" />	(System.String)
    ''' </param>
    ''' <param name="blnRecursif">
    ''' 	Si le paramètre est True (par défaut), la recherche s'effectue de manière récursive
    '''     à l'intérieur de tous les groupes dont est membre l'utilisateur et rallonge le temps d'exécution.
    '''     Si le paramètre est False, la recherche ne s'effectue qu'au premier niveau des groupes dont
    '''     l'utilisateur est membre ce qui est de loin plus rapide. 
    ''' 	Value Type: <see cref="Boolean" />	(System.Boolean)
    ''' </param>
    ''' <returns><see cref="Boolean" />	(System.Boolean)</returns>
    ''' <remarks><para><pre>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2005-05-24	t209376		Création initiale
    ''' 2005-09-13  t209376     Ajout du paramètre optionnel blnRecursif
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    <Obsolete(Desuet.UTILISEZ_COMPOSANT_311, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Function EstMembreDe(ByVal strGroupe As String, ByVal blnRecursif As Boolean) As Boolean
        ''
        ''XE7N411_ExecuterInstrSql - Délesté 
        ''XW7N050_GererSources - Migré à TS4N213
        ''
        Dim tabGroupes() As String

        Dim blnPresent As Boolean

        tabGroupes = _utilisateurCourant.MembreDe
        blnPresent = False

        For Each strGrp As String In tabGroupes
            If String.Compare(strGrp, strGroupe, True) = 0 Then
                blnPresent = True
                Exit For
            Else
                If blnRecursif Then
                    blnPresent = _accesseurAD.ChercheDansGroupes(strGrp, strGroupe)
                    If blnPresent Then Exit For
                End If
            End If
        Next

        Return blnPresent
    End Function

    <Obsolete(Desuet.UTILISEZ_COMPOSANT_311, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Function EstMembreDe(ByVal strGroupe As String) As Boolean
        ''
        ''XE7N411_ExecuterInstrSql - Délesté 
        ''XW7N050_GererSources - Migré à TS4N213
        ''
        Return EstMembreDe(strGroupe, True)
    End Function

#End Region

#Region "Désuèt - Publique Non-utilisé"

    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	Securite.tsCuObtnrInfoAD.ConvertirSid
    ''' <summary>
    '''     Cette fonction convertie le Sid en chaine de caractère.
    ''' </summary>
    ''' <param name="tblSID">
    ''' 	Tableau de Bytes qui contient le Sid d'un compte dans l'AD. 
    ''' 	Reference Type: <see cref="Byte" />	(System.Byte)
    ''' </param>
    ''' <returns><see cref="String" />	(System.String)</returns>
    ''' <remarks><para><pre>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2005-05-24	t209376		Création initiale
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    <Obsolete(Desuet.MEMBRE_NON_UTILISE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Function ConvertirSid(ByRef tblSID As Byte()) As String
        Return convertSidToString(tblSID)
    End Function

    ''' --------------------------------------------------------------------------------
    ''' <summary>
    ''' Cette méthode permet de determiner le domaine de l'active directory à accéder
    ''' </summary>
    ''' <param name="nomDomaine">Nom du domaine à être consulté:
    ''' "RRQ_QC"           - Pour le domaine RRQ
    ''' "INTRA"            - Pour le domaine CARRA 
    ''' "MULTIDOMAINE"     - Pour le domaine RRQ et CARRA 
    ''' </param>
    ''' --------------------------------------------------------------------------------
    <Obsolete(Desuet.MEMBRE_NON_UTILISE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Sub Domaine(ByVal nomDomaine As String)
        _accesseurAD = New TsCuObtnrInfoADRQ()
    End Sub

    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	Securite.tsCuObtnrInfoAD.ObtenirListe
    ''' <summary>
    '''     Cette propriété retourne une liste d'utilisateurs récupérés dans l'active directory.
    ''' </summary>
    ''' <param name="pTypeRequete">
    ''' 	Le champ sur lequel s'effectue la requête à l'active directory. 
    ''' </param>
    ''' <param name="pCritereRecherche">
    ''' 	Le critère de recherche à l'active directory. 
    ''' </param>
    ''' <param name="pCritereRechercheSecondaire">
    ''' 	Le critère de recherche secondaire à l'active directory utilisé lors d'une recherche nom et prénom. 
    '''     Value Type: <see cref="String" />	(System.String)
    ''' </param>
    ''' <param name="pCategorie">
    ''' 	Catégorie d'objets (personne, groupe ou les deux) à utilisé lors de la recherche. 
    ''' </param>
    ''' <returns>Retourne une liste d'utilisateurs sous format List(Of TsCuUtilisateur)</returns>
    ''' --------------------------------------------------------------------------------
    ''' 
    <Obsolete(Desuet.MEMBRE_NON_UTILISE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Function ObtenirListe(ByVal pTypeRequete As TsIadTypeRequete, ByVal pCritereRecherche As String,
                                   ByVal pCritereRechercheSecondaire As String,
                                   ByVal pCategorie As TsIadObjectCategory) As List(Of TsCuUtilisateurAD)

        Return _accesseurAD.ObtenirListeUtilisateur(pTypeRequete, pCritereRecherche, pCritereRechercheSecondaire, pCategorie)
    End Function
    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	Securite.tsCuObtnrInfoAD.ObtenirListe
    ''' <summary>
    '''     Cette propriété retourne une liste d'utilisateurs récupérés dans l'active directory.
    ''' </summary>
    ''' <param name="pTypeRequete">
    ''' 	Le champ sur lequel s'effectue la requête à l'active directory. 
    ''' 	Value Type: see cref="Securite.tsCuObtnrInfoAD.XzIadTypeRequete" />	(Rrq.Securite.tsCuObtnrInfoAD.XzIadTypeRequete)
    ''' </param>
    ''' <param name="pCritereRecherche">
    ''' 	Le critère de recherche à l'active directory. 
    ''' 	Value Type: <see cref="String" />	(System.String)
    ''' </param>
    ''' <param name="pCritereRechercheSecondaire">
    ''' 	Le critère de recherche secondaire à l'active directory utilisé lors d'une recherche nom et prénom. 
    '''     Value Type: <see cref="String" />	(System.String)
    ''' </param>
    ''' <returns>Retourne une liste d'utilisateurs sous format List(Of TsCuUtilisateur)</returns>
    ''' --------------------------------------------------------------------------------
    <Obsolete(Desuet.MEMBRE_NON_UTILISE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Function ObtenirListe(ByVal pTypeRequete As TsIadTypeRequete, ByVal pCritereRecherche As String, ByVal pCritereRechercheSecondaire As String) As List(Of TsCuUtilisateurAD)
        Return _accesseurAD.ObtenirListeUtilisateur(pTypeRequete, pCritereRecherche, pCritereRechercheSecondaire, TsIadObjectCategory.TsIadOcTous)
    End Function
    <Obsolete(Desuet.MEMBRE_NON_UTILISE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Function ObtenirListe(ByVal pTypeRequete As TsIadTypeRequete, ByVal pCritereRecherche As String) As List(Of TsCuUtilisateurAD)
        Return ObtenirListe(pTypeRequete, pCritereRecherche, String.Empty)
    End Function

    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	Securite.tsCuObtnrInfoAD.ObtenirListeDT
    ''' <summary>
    '''     Cette propriété retourne une liste d'utilisateurs sous format DataTable
    '''     récupéré dans l'active directory.  
    ''' </summary>
    ''' <param name="pTypeRequete">
    ''' 	Le champ sur lequel s'effectue la requête à l'active directory. 
    ''' </param>
    ''' <param name="pCritereRecherche">
    ''' 	Le critère de recherche à l'active directory. 
    ''' </param>
    ''' <param name="pCritereRechercheSecondaire">
    ''' 	Le critère de recherche secondaire à l'active directory utilisé lors d'une recherche nom et prénom. 
    ''' </param>
    ''' <param name="pCategorie">
    ''' 	Catégorie d'objets (personne, groupe ou les deux) à utilisé lors de la recherche. 
    ''' </param>
    ''' <returns><see cref="Data.DataTable" />	(System.Data.DataTable)</returns>
    ''' --------------------------------------------------------------------------------
    <Obsolete(Desuet.MEMBRE_NON_UTILISE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Function ObtenirListeDT(ByVal pTypeRequete As TsIadTypeRequete, ByVal pCritereRecherche As String, ByVal pCritereRechercheSecondaire As String, ByVal pCategorie As TsIadObjectCategory) As DataTable
        Dim dtAD As Data.DataTable = Nothing

        Try
            dtAD = _accesseurAD.RechercheActiveDirectory(pTypeRequete, pCritereRecherche, pCritereRechercheSecondaire, pCategorie)
            Return dtAD.Copy

        Finally
            ' Libérer les ressources utilisées
            If Not IsNothing(dtAD) Then
                dtAD.Dispose()
                dtAD = Nothing
            End If
        End Try
    End Function

    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	Securite.tsCuObtnrInfoAD.ObtenirListeGroupes
    ''' <summary>
    '''     Cette méthode retourne la liste des groupes auquel l’utilisateur est membre.
    ''' </summary>
    ''' <returns><see cref="String" />	(System.String)</returns>
    ''' <remarks><para><pre>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2005-09-13	t209376		Création initiale
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    <Obsolete(Desuet.MEMBRE_NON_UTILISE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Function ObtenirListeGroupes() As String()
        Return _utilisateurCourant.MembreDe
    End Function

    <Obsolete(Desuet.MEMBRE_NON_UTILISE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Function ObtenirListeGroupes(ByVal blnRechRecursive As Boolean) As String()
        Dim tabGroupes() As String
        Dim tabSousGroupe() As String

        Dim blnPresent As Boolean

        tabGroupes = _utilisateurCourant.MembreDe
        blnPresent = False

        Array.Sort(tabGroupes)

        For Each strGrp As String In tabGroupes
            If blnRechRecursive Then
                tabSousGroupe = _accesseurAD.ObtenirMembresGroupe(strGrp)

                If tabSousGroupe IsNot Nothing AndAlso tabSousGroupe.Length > 0 Then
                    Dim EndroitCopie As Integer = tabGroupes.Length

                    For Each Groupe As String In tabSousGroupe
                        If Array.IndexOf(tabGroupes, Groupe) = -1 Then
                            ReDim Preserve tabGroupes(tabGroupes.Length)
                            tabGroupes(EndroitCopie) = Groupe
                            EndroitCopie += 1
                        End If
                    Next
                End If
            End If
        Next

        Array.Sort(tabGroupes)

        Return tabGroupes
    End Function

    ''' <summary>
    '''   Cette méthode permet d'obtenir une liste de groupe de l'AD débutant par le filtre
    '''   passé en paramètre.
    ''' </summary>
    ''' <param name="Filtre">Filtre a appliquer pour la recherche.</param>
    ''' <returns>Retourne la liste des groupe correspondant au filtre.</returns>
    <Obsolete(Desuet.MEMBRE_NON_UTILISE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Function ObtenirListeGroupesDebutePar(ByVal Filtre As String) As List(Of String)
        Dim Resultats As SearchResultCollection
        Dim ListeGroupe As New List(Of String)

        Resultats = _accesseurAD.ObtenirListeGroupes(Filtre & "*")

        For Each Groupe As SearchResult In Resultats
            ListeGroupe.Add(Groupe.Properties.Item(AD_sAMAccountName).Item(0).ToString)
        Next

        Return ListeGroupe
    End Function

    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	Securite.tsCuObtnrInfoAD.ObtenirListeMembreGroupe
    ''' <summary>
    '''     Cette méthode retourne la liste des membres du groupe passé en paramètre.
    ''' </summary>
    ''' <param name="strGroupe">
    ''' 	Représente le groupe pour lequel on désire avoir la liste des membres. 
    ''' 	Value Type: <see cref="String" />	(System.String)
    ''' </param>
    ''' <returns><see cref="Data.DataTable" />	(System.Data.DataTable)</returns>
    ''' <remarks><para><pre>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2006-06-20	T209376		Création initiale
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    <Obsolete(Desuet.MEMBRE_NON_UTILISE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Function ObtenirListeMembreGroupe(ByVal strGroupe As String) As List(Of TsCuUtilisateurAD)
        Return _accesseurAD.ObtenirListeMembreGroupe(strGroupe, False)
    End Function

    <Obsolete(Desuet.MEMBRE_NON_UTILISE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Function ObtenirListeMembreGroupe(ByVal strGroupe As String, ByVal blnRechRecursive As Boolean) As List(Of TsCuUtilisateurAD)
        Return _accesseurAD.ObtenirListeMembreGroupe(strGroupe, blnRechRecursive)
    End Function

    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	Securite.tsCuObtnrInfoAD.ObtenirMembresDuGroupe
    ''' <summary>
    '''     Cette méthode retourne la liste des membres du groupe passé en paramètre.
    ''' </summary>
    ''' <param name="strGroupe">
    ''' 	Représente le groupe pour lequel on désire avoir la liste des membres. 
    ''' 	Value Type: <see cref="String" />	(System.String)
    ''' </param>
    ''' <returns><see cref="Data.DataTable" />	(System.Data.DataTable)</returns>
    ''' <remarks><para><pre>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2006-06-20	T209376		Création initiale
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    <Obsolete(Desuet.MEMBRE_NON_UTILISE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Function ObtenirMembresDuGroupe(ByVal strGroupe As String) As DataTable
        Return ObtenirMembresDuGroupe(strGroupe, False)
    End Function

    <Obsolete(Desuet.MEMBRE_NON_UTILISE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Function VerifierGroupeExiste(ByVal strGroupe As String) As Boolean
        Return _accesseurAD.VerifierGroupeExiste(strGroupe)
    End Function

#End Region

End Class

