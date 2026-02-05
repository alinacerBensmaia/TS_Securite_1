Imports System.Collections.Generic

''' <summary>
'''   Classe qui représente un membre de l'AD.
''' </summary>
Public Class TsCuUtilisateurAD
    Private _codeUtilisateur As String


#Region "Constructeurs"

    ''' <summary>
    '''   Constructeur de base de la classe. Utilisé exclusivement par la les classes 
    ''' TsCuObtnrInfoADCARRA et TsCuObtrnInfoADRRQ
    ''' </summary>
    ''' <param name="CodeUtilisateur">Code de l'utilisateur dont on veut obtenir les informations.</param>
    Friend Sub New(ByVal CodeUtilisateur As String, ByVal nom As String, ByVal prenom As String, ByVal nomComplet As String, ByVal courriel As String, ByVal uniteAdmninistrative As String, ByVal fonction As String, ByVal societe As String, ByVal numeroEmploye As String, ByVal utilisateurDesactive As Boolean, ByVal nomDomaine As TsIadNomDomaine, ByVal membreDe As String, ByVal nomPoste As String, ByVal numeroTelephone As String, ByVal estBascule As Boolean, ByVal estCompteAdmin As Boolean)
        initialize(CodeUtilisateur, nom, prenom, nomComplet, courriel, uniteAdmninistrative, fonction, societe, numeroEmploye, utilisateurDesactive, nomDomaine, nomPoste, numeroTelephone, estBascule, estCompteAdmin)
    End Sub

    Private Sub initialize(codeUtilisateur As String)
        Dim temp As TsCuUtilisateurAD = tsCuObtnrInfoAD.ObtenirUtilisateur(codeUtilisateur)
        initialize(temp.CodeUtilisateur, temp.Nom, temp.Prenom, temp.NomComplet, temp.Courriel, temp.UniteAdmninistrative, temp.Fonction, temp.Societe, temp.NumeroEmploye, temp.UtilisateurDesactive, TsIadNomDomaine.TsMultiDomaine, temp.NomPoste, temp.NumeroTelephone, temp.EstBascule, temp.EstCompteAdmin)
    End Sub
    Private Sub initialize(ByVal codeUtilisateur As String, ByVal nom As String, ByVal prenom As String, ByVal nomComplet As String, ByVal courriel As String, ByVal uniteAdmninistrative As String, ByVal fonction As String, ByVal societe As String, ByVal numeroEmploye As String, ByVal utilisateurDesactive As Boolean, ByVal nomDomaine As TsIadNomDomaine, nomPoste As String, ByVal numeroTelephone As String, ByVal estBascule As Boolean, ByVal estCompteAdmin As Boolean)
        Dim d As Domaines = Domaines.Parse(nomDomaine)

        _codeUtilisateur = codeUtilisateur
        _Courriel = courriel
        _DomaineUtilisateur = d.EnumValue
        _DomaineNT = d.DomaineNT
        _ServeurActiveDirectory = d.ServerActiveDirectory
        _Fonction = fonction
        _Nom = nom
        _NomComplet = nomComplet
        _NomPoste = nomPoste
        _NumeroEmploye = numeroEmploye
        _Prenom = prenom
        _Societe = societe
        _UniteAdmninistrative = uniteAdmninistrative
        _UtilisateurDesactive = utilisateurDesactive
        _NumeroTelephone = numeroTelephone
        _EstBascule = estBascule
        _EstCompteAdmin = estCompteAdmin
    End Sub

#End Region

#Region "Propriétés"

    ''' <summary>
    '''   Code de l'utilisateur (sAMAccountName).
    ''' </summary>
    Public Property CodeUtilisateur() As String
        Get
            Return _codeUtilisateur
        End Get

        <Obsolete(Desuet.MEMBRE_NON_UTILISE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
        Set(ByVal Value As String)
            If _codeUtilisateur <> Value Then
                initialize(CodeUtilisateur)
            End If
        End Set
    End Property

    ''' <summary>
    '''   Courriel de l'utilisateur (Mail).
    ''' </summary>
    Public ReadOnly Property Courriel() As String

    ''' <summary>
    '''   Fonction de l'utilisateur (Title).
    ''' </summary>
    Public ReadOnly Property Fonction() As String

    ''' <summary>
    '''   Nom de famille de l'utilisateur.
    ''' </summary>
    Public ReadOnly Property Nom() As String

    ''' <summary>
    '''   Nom, Prénom et UniteAdmninistrative de l'utilisateur.
    ''' </summary>
    Public ReadOnly Property NomComplet() As String

    ''' <summary>
    '''   Nom du poste
    ''' </summary>
    Public ReadOnly Property NomPoste() As String

    ''' <summary>
    '''   Prénom de l'utilisateur.
    ''' </summary>
    Public ReadOnly Property Prenom() As String

    ''' <summary>
    '''   Compagnie que l'utilisateur fait partie.
    ''' </summary>
    Public ReadOnly Property Societe() As String

    ''' <summary>
    '''   Unité admninistrative de l'utilisateur.
    ''' </summary>
    Public ReadOnly Property UniteAdmninistrative() As String

    ''' <summary>
    '''   Unité admninistrative de l'utilisateur.
    ''' </summary>
    Public ReadOnly Property UtilisateurDesactive() As Boolean

    ''' <summary>
    '''   Enumération représentant le domaine active directory de l'utilisateur.
    ''' </summary>
    Public ReadOnly Property DomaineUtilisateur() As TsIadNomDomaine

    ''' <summary>
    '''   Nom du domaine active directory de l'utilisateur.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property DomaineNT() As String

    ''' <summary>
    ''' Nom du serveur active directory de l'utilisateur.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property ServeurActiveDirectory As String

    ''' <summary>
    '''   Numéro d'employé.
    ''' </summary>
    Public ReadOnly Property NumeroEmploye() As String


    ''' <summary>
    '''  Numéro de téléphone
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property NumeroTelephone() As String

    ''' <summary>
    '''   Indique si l'utilisateur est basculé sur le domaine en consultation.
    ''' </summary>
    ''' 
    Friend Property EstBascule() As Boolean

    ''' <summary>
    '''   Indique si le compte est un compte administrateur.
    ''' </summary>
    Public Property EstCompteAdmin() As Boolean




#End Region

#Region "Désuèt - Publique Utilisé"

    ''' <summary>
    '''   Constructeur de base de la classe.
    ''' </summary>
    ''' <param name="CodeUtilisateur">Code de l'utilisateur dont on veut obtenir les informations.</param>
    <Obsolete(Desuet.UTILISEZ_212_OBTENIR_UTILISATEUR, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Sub New(ByVal CodeUtilisateur As String)
        ''
        ''XW7N158_Courrier, XW7N304_Verrous
        ''  Constructeur utilisé pour créer un objet de données, ne gère pas le multi-domaine
        ''
        initialize(CodeUtilisateur)
    End Sub

    ''' <summary>
    '''   Groupes dont l'utilisateur est membre.
    ''' </summary>
    <Obsolete(Desuet.UTILISEZ_COMPOSANT_311, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Friend ReadOnly Property MembreDe() As String()
        Get
            Return New String() {}
        End Get
    End Property

#End Region

#Region "Désuèt - Publique Non-Utilisé"

    <Obsolete(Desuet.MEMBRE_NON_UTILISE, Desuet.OBSOLETE_USAGE_NOT_ALLOWED)>
    Public Sub RafraichirInformations()
        initialize(_codeUtilisateur)
    End Sub

#End Region

End Class
