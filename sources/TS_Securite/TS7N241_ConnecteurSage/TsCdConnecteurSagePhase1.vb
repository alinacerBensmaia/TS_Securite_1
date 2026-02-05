''' <summary>
''' Connecteur pour Sage phase 1. 
''' Contient les méthodes de changements des rôles et des utilisateurs.
''' </summary>
Public Class TsCdConnecteurSagePhase1
    Inherits TsCdConnecteurSage

#Region "Propriétées"

    ''' <summary>
    ''' Description du système cible compréhensible pour l'utilisateur.
    ''' </summary>
    Public Overrides ReadOnly Property DescrCible() As String
        Get
            Return "Sage phase 1"
        End Get
    End Property

#End Region

#Region "Constructeurs"

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="IdCible">Identifiant du connecteur.</param>
    Public Sub New(ByVal IdCible As String)
        MyBase.New(IdCible)
    End Sub

    Public Sub New(ByVal IdCible As String, ByVal config As String)
        MyBase.New(IdCible, config)
    End Sub

#End Region

#Region "Méthodes"

    ''' <summary>
    ''' Méthode du Connecteur Sage Phase1. 
    ''' Cette méthode va créer les rôles de la liste <paramref name="ajouts"/> dans Sage.
    ''' </summary>
    ''' <param name="ajouts">Liste des rôles à créer.</param>
    ''' <param name="contnErreur">
    ''' True = Si une erreur est rencontrée, l'erreur est prise en compte, mais le traitement continue.
    ''' False = Si une erreur est rencontrée, l'erreur est prise en compte et le traitement s'arrête immédiatement.
    ''' </param>
    ''' <returns>Si la méthode a rencontrée au moins une erreur, elle retourne <c>False</c>, sinon elle retourne <c>True</c>.</returns>
    Public Overrides Function CreerRoles(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRole), Optional ByVal contnErreur As Boolean = False) As Boolean
        VerificationListe(ajouts, Function(e As TsCdConnxRole) accesSage.PresentRole(e.NomRole))
        Dim toutOk As Boolean = ApplicationListe(ajouts, contnErreur, _
                                Function(e As TsCdConnxRole) accesSage.AjouterRole(e.NomRole))
        If toutOk = False Then
            TesterErreursRole(ajouts)
        End If

        Return toutOk
    End Function

    ''' <summary>
    ''' Méthode du Connecteur Sage Phase1. 
    ''' Cette méthode va créer les rôles de la liste <paramref name="ajouts"/> dans Sage.
    ''' </summary>
    ''' <param name="ajouts">Liste des rôles à créer.</param>
    ''' <param name="contnErreur">
    ''' True = Si une erreur est rencontrée, l'erreur est prise en compte, mais le traitement continue.
    ''' False = Si une erreur est rencontrée, l'erreur est prise en compte et le traitement s'arrête immédiatement.
    ''' </param>
    ''' <returns>Si la méthode a rencontrée au moins une erreur, elle retourne <c>False</c>, sinon elle retourne <c>True</c>.</returns>
    Public Overrides Function CreerUsers(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUser), Optional ByVal contnErreur As Boolean = False) As Boolean
        VerificationListe(ajouts, Function(e As TsCdConnxUser) accesSage.PresentUser(e.CodeUtilisateur))
        Dim toutOk As Boolean = ApplicationListe(ajouts, contnErreur, _
                                Function(e As TsCdConnxUser) accesSage.AjouterUtilisateur(e.CodeUtilisateur))
        If toutOk = False Then
            TesterErreursUser(ajouts)
        End If

        Return toutOk
    End Function

    ''' <summary>
    ''' Méthode du Connecteur Sage Phase1. 
    ''' Cette méthode va ajouter et supprimer aux rôles les attributs des listes <paramref name=" ajouts"/> et <paramref name="suppr"/>.
    ''' </summary>
    ''' <param name="ajouts">Liste d'attributs à ajouter.</param>
    ''' <param name="suppr">Liste d'attributs à supprimer.</param>
    ''' <param name="contnErreur">
    ''' True = Si une erreur est rencontrée, l'erreur est prise en compte, mais le traitement continue.
    ''' False = Si une erreur est rencontrée, l'erreur est prise en compte et le traitement s'arrête immédiatement.
    ''' </param>
    ''' <returns>Si la méthode a rencontrée au moins une erreur, elle retourne <c>False</c>, sinon elle retourne <c>True</c>.</returns>
    Public Overrides Function AppliquerAttrbRoles(ByVal ajouts As IEnumerable(Of TsCdConnxRoleAttrb), ByVal suppr As IEnumerable(Of TsCdConnxRoleAttrb), Optional ByVal contnErreur As Boolean = False) As Boolean
        Dim toutOk As Boolean = False

        VerifierAttrbRoles(ajouts, suppr)

        toutOk = ApplicationListe(suppr, contnErreur, _
                                  Function(e As TsCdConnxRoleAttrb) accesSage.EffacerAttrbRole(e.NomRole, e.NomAttrb))
        If toutOk = True Or contnErreur = True Then
            toutOk = toutOk And ApplicationListe(ajouts, contnErreur, _
                                      Function(e As TsCdConnxRoleAttrb) accesSage.AjouterAttrbRole(e.NomRole, e.NomAttrb, e.Valeur))
        End If

        If toutOk = False Then
            TesterErreursRoleAttrb(ajouts, suppr)
        End If

        Return toutOk
    End Function

    ''' <summary>
    ''' Méthode du Connecteur Sage Phase1. 
    ''' Cette méthode va ajouter et supprimer aux rôles les attributs des listes <paramref name=" ajouts"/> et <paramref name="suppr"/>.
    ''' </summary>
    ''' <param name="ajouts">Liste d'attributs à ajouter.</param>
    ''' <param name="suppr">Liste d'attributs à supprimer.</param>
    ''' <param name="contnErreur">
    ''' True = Si une erreur est rencontrée, l'erreur est prise en compte, mais le traitement continue.
    ''' False = Si une erreur est rencontrée, l'erreur est prise en compte et le traitement s'arrête immédiatement.
    ''' </param>
    ''' <returns>Si la méthode a rencontrée au moins une erreur, elle retourne <c>False</c>, sinon elle retourne <c>True</c>.</returns>
    Public Overrides Function AppliquerAttrbUsers(ByVal ajouts As IEnumerable(Of TsCdConnxUserAttrb), ByVal suppr As IEnumerable(Of TsCdConnxUserAttrb), Optional ByVal contnErreur As Boolean = False) As Boolean
        Dim toutOk As Boolean = False

        VerifierAttrbUsers(ajouts, suppr)

        toutOk = ApplicationListe(suppr, contnErreur, _
                                  Function(e As TsCdConnxUserAttrb) accesSage.EffacerAttrbUser(e.CodeUtilisateur, e.NomAttrb))
        If toutOk = True Or contnErreur = True Then
            toutOk = toutOk And ApplicationListe(ajouts, contnErreur, _
                                      Function(e As TsCdConnxUserAttrb) accesSage.AjouterAttrbUser(e.CodeUtilisateur, e.NomAttrb, e.Valeur))
        End If

        If toutOk = False Then
            TesterErreursUserAttrb(ajouts, suppr)
        End If

        Return toutOk
    End Function

    ''' <summary>
    ''' Méthode du Connecteur Sage Phase1. Cette méthode va appliquer les ajouts des liens Rôle/Rôle de la liste <paramref name="ajouts"/> dans Sage
    ''' et va appliquer les suppressions des liens Rôle/Rôle dans Sage à partir de la liste <paramref name="suppr"/>.
    ''' </summary>
    ''' <param name="ajouts">Liste de liens Rôle/Rôle à ajouter.</param>
    ''' <param name="suppr">Liste de liens Rôle/Rôle à supprimer.</param>
    ''' <param name="contnErreur">
    ''' True = Si une erreur est rencontrée, l'erreur est prise en compte, mais le traitement continue.
    ''' False = Si une erreur est rencontrée, l'erreur est prise en compte et le traitement s'arrête immédiatement.
    ''' </param>
    ''' <returns>Si la méthode a rencontrée au moins une erreur, elle retourne <c>False</c>, sinon elle retourne <c>True</c>.</returns>
    Public Overrides Function AppliquerLiensRoleRole(ByVal ajouts As IEnumerable(Of TsCdConnxRoleRole), ByVal suppr As IEnumerable(Of TsCdConnxRoleRole), Optional ByVal contnErreur As Boolean = False) As Boolean
        Dim toutOk As Boolean = False

        VerifierLiensRoleRole(ajouts, suppr)

        toutOk = ApplicationListe(ajouts, contnErreur, _
                                  Function(e As TsCdConnxRoleRole) accesSage.AjouerLienRoleRole(e.NomRoleSup, e.NomSousRole))
        If toutOk = True Or contnErreur = True Then
            toutOk = toutOk And ApplicationListe(suppr, contnErreur, _
                                      Function(e As TsCdConnxRoleRole) accesSage.EffacerLienRoleRole(e.NomRoleSup, e.NomSousRole))
        End If

        If toutOk = False Then
            TesterErreursRoleRole(ajouts, suppr)
        End If

        Return toutOk
    End Function

    ''' <summary>
    ''' Méthode du Connecteur Sage Phase1. Cette méthode va appliquer les ajouts des liens Utilisateur/Rôle de la liste <paramref name="ajouts"/> dans Sage
    ''' et va appliquer les suppressions des liens Utilisateur/Rôle dans Sage à partir de la liste <paramref name="suppr"/>.
    ''' </summary>
    ''' <param name="ajouts">Liste de liens Utilisateur/Rôle à ajouter.</param>
    ''' <param name="suppr">Liste de liens Utilisateur/Rôle à supprimer.</param>
    ''' <param name="contnErreur">
    ''' True = Si une erreur est rencontrée, l'erreur est prise en compte, mais le traitement continue.
    ''' False = Si une erreur est rencontrée, l'erreur est prise en compte et le traitement s'arrête immédiatement.
    ''' </param>
    ''' <returns>Si la méthode a rencontrée au moins une erreur, elle retourne <c>False</c>, sinon elle retourne <c>True</c>.</returns>
    Public Overrides Function AppliquerLiensUserRole(ByVal ajouts As IEnumerable(Of TsCdConnxUserRole), ByVal suppr As IEnumerable(Of TsCdConnxUserRole), Optional ByVal contnErreur As Boolean = False) As Boolean
        Dim toutOk As Boolean = False

        VerifierLiensUserRole(ajouts, suppr)

        toutOk = ApplicationListe(ajouts, contnErreur, _
                                  Function(e As TsCdConnxUserRole) accesSage.AjouerLienUserRole(e.CodeUtilisateur, e.NomRole))
        If toutOk = True Or contnErreur = True Then
            toutOk = toutOk And ApplicationListe(suppr, contnErreur, _
                                      Function(e As TsCdConnxUserRole) accesSage.EffacerLienUserRole(e.CodeUtilisateur, e.NomRole))
        End If

        If toutOk = False Then
            TesterErreursUserRole(ajouts, suppr)
        End If

        Return toutOk
    End Function

    ''' <summary>
    ''' Méthode du Connecteur Sage Phase 1.
    ''' Cette méthode va vérifier si les rôles des listes de rôles qui sont à jour dans Sage.
    ''' </summary>
    ''' <param name="ajouts">La liste des rôles à ajouter.</param>
    ''' <param name="suppr">La liste des rôles à effacer.</param>
    Public Overrides Sub VerifierRoles(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRole), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRole))
        VerificationListe(ajouts, Function(e As TsCdConnxRole) accesSage.PresentRole(e.NomRole))
        VerificationListe(suppr, Function(e As TsCdConnxRole) accesSage.AbsentRole(e.NomRole))
    End Sub

    ''' <summary>
    ''' Méthode du Connecteur Sage Phase 1.
    ''' Cette méthode va vérifier si les utilisateurs des listes de utilisateurs qui sont à jour dans Sage.
    ''' </summary>
    ''' <param name="ajouts">La liste des utilisateurs à ajouter.</param>
    ''' <param name="suppr">La liste des utilisateurs à effacer.</param>
    Public Overrides Sub VerifierUsers(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUser), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUser))
        VerificationListe(ajouts, Function(e As TsCdConnxUser) accesSage.PresentUser(e.CodeUtilisateur))
        VerificationListe(suppr, Function(e As TsCdConnxUser) accesSage.AbsentUser(e.CodeUtilisateur))
    End Sub

    ''' <summary>
    ''' Méthode du connecteur Sage Phase 1.
    ''' Cette méthode va vérifier si les attributs des rôles sont à jour dans Sage.
    ''' </summary>
    ''' <param name="ajouts">Liste d'attributs de rôles à ajouter.</param>
    ''' <param name="suppr">Liste d'attributs de rôle à supprimer</param>
    Public Overrides Sub VerifierAttrbRoles(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRoleAttrb), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRoleAttrb))
        TrouverModification(ajouts, suppr, Function(e1 As TsCdConnxRoleAttrb) e1.NomRole + e1.NomAttrb)

        VerificationListe(ajouts, Function(e As TsCdConnxRoleAttrb) accesSage.EgaleAttrbRole(e.NomRole, e.NomAttrb, e.Valeur))
        VerificationListe(suppr, Function(e As TsCdConnxRoleAttrb) accesSage.EgaleAttrbRole(e.NomRole, e.NomAttrb, e.Valeur))
    End Sub

    ''' <summary>
    ''' Méthode du connecteur Sage Phase 1.
    ''' Cette méthode va vérifier si les attributs des utilisateurs sont à jour dans Sage.
    ''' </summary>
    ''' <param name="ajouts">Liste d'attributs d'utilisateurs à ajouter.</param>
    ''' <param name="suppr">Liste d'attributs d'utilisateurs à supprimer</param>
    Public Overrides Sub VerifierAttrbUsers(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUserAttrb), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUserAttrb))
        TrouverModification(ajouts, suppr, Function(e1 As TsCdConnxUserAttrb) e1.CodeUtilisateur + e1.NomAttrb)

        VerificationListe(ajouts, Function(e As TsCdConnxUserAttrb) accesSage.EgaleAttrbUser(e.CodeUtilisateur, e.NomAttrb, e.Valeur))
        VerificationListe(suppr, Function(e As TsCdConnxUserAttrb) accesSage.EgaleAttrbUser(e.CodeUtilisateur, e.NomAttrb, e.Valeur))
    End Sub

    ''' <summary>
    ''' Méthode du Connecteur Sage Phase 1.
    ''' Cette méthode va vérifier si les liens des listes de liens entre rôles qui sont à jour dans Sage.
    ''' </summary>
    ''' <param name="ajouts">La liste des liens entre rôles à ajouter.</param>
    ''' <param name="suppr">La liste des liens entre rôles à effacer.</param>
    ''' <remarks></remarks>
    Public Overrides Sub VerifierLiensRoleRole(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRoleRole), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRoleRole))
        VerificationListe(ajouts, Function(e As TsCdConnxRoleRole) accesSage.PresentLienRoleRole(e.NomRoleSup, e.NomSousRole))
        VerificationListe(suppr, Function(e As TsCdConnxRoleRole) accesSage.AbsentLienRoleRole(e.NomRoleSup, e.NomSousRole))
    End Sub

    ''' <summary>
    ''' Méthode du Connecteur Sage Phase 1.
    ''' Cette méthode va vérifier si les liens des listes de liens entre rôles qui sont à jour dans Sage.
    ''' </summary>
    ''' <param name="ajouts">La liste des liens entre rôles à ajouter.</param>
    ''' <param name="suppr">La liste des liens entre rôles à effacer.</param>
    ''' <remarks></remarks>
    Public Overrides Sub VerifierLiensUserRole(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRole), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRole))
        VerificationListe(ajouts, Function(e As TsCdConnxUserRole) accesSage.PresentLienUserRole(e.CodeUtilisateur, e.NomRole))
        VerificationListe(suppr, Function(e As TsCdConnxUserRole) accesSage.AbsentLienUserRole(e.CodeUtilisateur, e.NomRole))
    End Sub

#End Region

#Region "Fonctions de services"

    ''' <summary>
    ''' Fonction de services. Sert à tester les eurreurs qui sont survenus durant la création/supression d'un utilisateur.
    ''' </summary>
    ''' <param name="ajouts">Liste d'éléments à ajouter. Peut contenir des éléments sans erreur.</param>
    Private Sub TesterErreursUser(ByVal ajouts As IEnumerable(Of TsCdConnxUser))
        DepileurErreur(ajouts, True, AddressOf accesSage.DeffnErreurUser)
    End Sub

    ''' <summary>
    ''' Fonction de services. Sert à tester les eurreurs qui sont survenus durant la création/supression d'un rôle.
    ''' </summary>
    ''' <param name="ajouts">Liste d'éléments à ajouter. Peut contenir des éléments sans erreur.</param>
    Private Sub TesterErreursRole(ByVal ajouts As IEnumerable(Of TsCdConnxRole))
        DepileurErreur(ajouts, True, AddressOf accesSage.DeffnErreurRole)
    End Sub

    ''' <summary>
    ''' Fonction de services. Sert à tester les eurreurs qui sont survenus durant la création/supression d'un lien utilisateur/rôle.
    ''' </summary>
    ''' <param name="ajouts">Liste d'éléments à ajouter. Peut contenir des éléments sans erreur.</param>
    Private Sub TesterErreursUserRole(ByVal ajouts As IEnumerable(Of TsCdConnxUserRole), ByVal suppr As IEnumerable(Of TsCdConnxUserRole))
        DepileurErreur(ajouts, True, AddressOf accesSage.DeffnErreurLienUserRole)
        DepileurErreur(suppr, False, AddressOf accesSage.DeffnErreurLienUserRole)
    End Sub

    ''' <summary>
    ''' Fonction de services. Sert à tester les eurreurs qui sont survenus durant la création/supression d'un lien rôle supérieur/sous-rôle.
    ''' </summary>
    ''' <param name="ajouts">Liste d'éléments à ajouter. Peut contenir des éléments sans erreur.</param>
    Private Sub TesterErreursRoleRole(ByVal ajouts As IEnumerable(Of TsCdConnxRoleRole), ByVal suppr As IEnumerable(Of TsCdConnxRoleRole))
        DepileurErreur(ajouts, True, AddressOf accesSage.DeffnErreurLienRoleRole)
        DepileurErreur(suppr, False, AddressOf accesSage.DeffnErreurLienRoleRole)
    End Sub

    ''' <summary>
    ''' Fonction de services. Sert à tester les eurreurs qui sont survenus durant la création/supression d'un attribut d'un utilisateur.
    ''' </summary>
    ''' <param name="ajouts">Liste d'éléments à ajouter. Peut contenir des éléments sans erreur.</param>
    Private Sub TesterErreursUserAttrb(ByVal ajouts As IEnumerable(Of TsCdConnxUserAttrb), ByVal suppr As IEnumerable(Of TsCdConnxUserAttrb))
        DepileurErreur(ajouts, True, AddressOf accesSage.DeffnErreurUserAttbr)
        DepileurErreur(suppr, False, AddressOf accesSage.DeffnErreurUserAttbr)
    End Sub

    ''' <summary>
    ''' Fonction de services. Sert à tester les eurreurs qui sont survenus durant la création/supression d'un attribut d'un rôle.
    ''' </summary>
    ''' <param name="ajouts">Liste d'éléments à ajouter. Peut contenir des éléments sans erreur.</param>
    Private Sub TesterErreursRoleAttrb(ByVal ajouts As IEnumerable(Of TsCdConnxRoleAttrb), ByVal suppr As IEnumerable(Of TsCdConnxRoleAttrb))
        DepileurErreur(ajouts, True, AddressOf accesSage.DeffnErreurRoleAttbr)
        DepileurErreur(suppr, False, AddressOf accesSage.DeffnErreurRoleAttbr)
    End Sub

#End Region

End Class
