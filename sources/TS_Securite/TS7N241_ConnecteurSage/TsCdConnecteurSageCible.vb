''' <summary>
''' Connecteur pour Sage pour les cibles(Active Directory, TopSecret, etc).
''' </summary>
Public Class TsCdConnecteurSageCible
    Inherits TsCdConnecteurSage

#Region "Propriétées"

    ''' <summary>
    ''' Description du système cible compréhensible pour l'utilisateur.
    ''' </summary>
    Public Overrides ReadOnly Property DescrCible() As String
        Get
            Return "Sage: " + IdCible
        End Get
    End Property

#End Region

#Region "Constructeurs"

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="IdCible">Identifiant du conneceteur</param>
    Public Sub New(ByVal IdCible As String)
        MyBase.New(IdCible)
    End Sub

#End Region

#Region "Méthodes"

    ''' <summary>
    ''' Méthode permetant de faire des ajouts de ressources dans la configuration Sage.
    ''' </summary>
    ''' <param name="ajouts">Liste des ressources à ajouter.</param>
    ''' <param name="contnErreur">En cas d'erreur, cette varraible indique si le traitement doit être intérompus.</param>
    ''' <returns>Si au moins une erreur a été rencontrée, la méthode renvoira False, sinon elle revoira True.</returns>
    Public Overrides Function CreerRessr(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRessr), Optional ByVal contnErreur As Boolean = False) As Boolean
        VerificationListe(ajouts, Function(e As TsCdConnxRessr) accesSage.PresentRessr(e.NomRessource, e.CatgrRessource, IdCible))
        Dim toutOk As Boolean = ApplicationListe(ajouts, contnErreur, _
                                Function(e As TsCdConnxRessr) accesSage.AjouterRessr(e.NomRessource, e.CatgrRessource, IdCible))

        If toutOk = False Then
            TesterErreursRessr(ajouts, New List(Of TsCdConnxRessr))
        End If

        Return toutOk
    End Function

    ''' <summary>
    ''' Méthode permettant de faire des suppresions de ressources dans la configuration Sage.
    ''' </summary>
    ''' <param name="suppr">Liste des ressources à supprimer.</param>
    ''' <param name="contnErreur">En cas d'erreur, cette varraible indique si le traitement doit être intérompus.</param>
    ''' <returns>Si au moins une erreur a été rencontrée, la méthode renvoira False, sinon elle revoira True.</returns>
    ''' <remarks></remarks>
    Public Overrides Function DetruireRessr(ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRessr), Optional ByVal contnErreur As Boolean = False) As Boolean
        VerificationListe(suppr, Function(e As TsCdConnxRessr) accesSage.AbsentRessr(e.NomRessource, e.CatgrRessource, IdCible))
        Dim toutOk As Boolean = ApplicationListe(suppr, contnErreur, _
                                Function(e As TsCdConnxRessr) accesSage.EffacerRessr(e.NomRessource, e.CatgrRessource, IdCible))
        If toutOk = False Then
            TesterErreursRessr(New List(Of TsCdConnxRessr), suppr)
        End If

        Return toutOk
    End Function

    ''' <summary>
    ''' Méthode qui va ajouter, supprimer ou modiffier des champs attribut de ressources.
    ''' </summary>
    ''' <param name="ajouts">Liste des attributs de ressources à ajouté.</param>
    ''' <param name="suppr">Liste des attributs de ressources à supprimer.</param>
    ''' <param name="contnErreur">En cas d'erreur, cette varraible indique si le traitement doit être intérompus.</param>
    ''' <returns>Si au moins une erreur a été rencontrée, la méthode renvoira False, sinon elle revoira True.</returns>
    Public Overrides Function AppliquerAttrbRessr(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRessrAttrb), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRessrAttrb), Optional ByVal contnErreur As Boolean = False) As Boolean
        Dim toutOk As Boolean = False
        VerifierAttrbRessr(ajouts, suppr)

        toutOk = ApplicationListe(suppr, contnErreur, _
                                  Function(e As TsCdConnxRessrAttrb) accesSage.EffacerAttrbRessr(e.NomRessource, e.CatgrRessource, IdCible, e.NomAttrb))
        If toutOk = True Or contnErreur = True Then
            toutOk = toutOk And ApplicationListe(ajouts, contnErreur, _
                                      Function(e As TsCdConnxRessrAttrb) accesSage.AjouterAttrbRessr(e.NomRessource, e.CatgrRessource, IdCible, e.NomAttrb, e.Valeur))
        End If

        If toutOk = False Then
            TesterErreursRessrAttrb(ajouts, suppr)
        End If

        Return toutOk
    End Function

    ''' <summary>
    ''' Méthode qui va ajouter ou supprimer des liens entre des rôles et des ressources.
    ''' </summary>
    ''' <param name="ajouts">Liste d'ajouts de liens Rôle/Ressource.</param>
    ''' <param name="suppr">Liste de suppressions de liens Rôle/Ressource.</param>
    ''' <param name="contnErreur">En cas d'erreur, cette varraible indique si le traitement doit être intérompus.</param>
    ''' <returns>Si au moins une erreur a été rencontrée, la méthode renvoira False, sinon elle revoira True.</returns>
    Public Overrides Function AppliquerLiensRoleRessr(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRoleRessr), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRoleRessr), Optional ByVal contnErreur As Boolean = False) As Boolean
        Dim toutOk As Boolean = False
        VerifierLiensRoleRessr(ajouts, suppr)
        toutOk = ApplicationListe(ajouts, contnErreur, _
                                  Function(e As TsCdConnxRoleRessr) accesSage.AjouterLienRoleRessr(e.NomRessource, e.CatgrRessource, IdCible, e.NomRole))
        If toutOk = True Or contnErreur = True Then
            toutOk = toutOk And ApplicationListe(suppr, contnErreur, _
                                      Function(e As TsCdConnxRoleRessr) accesSage.EffacerLienRoleRessr(e.NomRessource, e.CatgrRessource, IdCible, e.NomRole))
        End If

        If toutOk = False Then
            TesterErreursLienRoleRessr(ajouts, suppr)
        End If

        Return toutOk
    End Function

    ''' <summary>
    ''' Méthode qui va ajouter ou supprimer des liens entre des utilisateurs et des ressources.
    ''' </summary>
    ''' <param name="ajouts">Liste d'ajouts de liens Utilisateur/Ressource.</param>
    ''' <param name="suppr">Liste de suppressions de liens Utilisateur/Ressource.</param>
    ''' <param name="contnErreur">En cas d'erreur, cette varraible indique si le traitement doit être intérompus.</param>
    ''' <returns>Si au moins une erreur a été rencontrée, la méthode renvoira False, sinon elle revoira True.</returns>
    Public Overrides Function AppliquerLiensUserRessr(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRessr), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRessr), Optional ByVal contnErreur As Boolean = False) As Boolean
        Dim toutOk As Boolean = False

        VerifierLiensUserRessr(ajouts, suppr)

        toutOk = ApplicationListe(ajouts, contnErreur, _
                                  Function(e As TsCdConnxUserRessr) accesSage.AjouterLienUserRessr(e.NomRessource, e.CatgrRessource, IdCible, e.CodeUtilisateur))
        If toutOk = True Or contnErreur = True Then
            toutOk = toutOk And ApplicationListe(suppr, contnErreur, _
                                      Function(e As TsCdConnxUserRessr) accesSage.EffacerLienUserRessr(e.NomRessource, e.CatgrRessource, IdCible, e.CodeUtilisateur))
        End If

        If toutOk = False Then
            TesterErreursLienUserRessr(ajouts, suppr)
        End If

        Return toutOk
    End Function

    ''' <summary>
    ''' Méthode qui vérifie si les ajouts ou les suppressions de ressources sont déja appliquées dans sage.
    ''' </summary>
    ''' <param name="ajouts">Liste d'ajouts de ressources.</param>
    ''' <param name="suppr">Liste de suppressions de ressources.</param>
    Public Overrides Sub VerifierRessr(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRessr), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRessr))
        VerificationListe(ajouts, Function(e As TsCdConnxRessr) accesSage.PresentRessr(e.NomRessource, e.CatgrRessource, IdCible))
        VerificationListe(suppr, Function(e As TsCdConnxRessr) accesSage.AbsentRessr(e.NomRessource, e.CatgrRessource, IdCible))
    End Sub

    ''' <summary>
    ''' Méthode qui vérifie si les ajouts ou les suppressions des champs attribut des ressources sont déja appliquées dans sage.
    ''' </summary>
    ''' <param name="ajouts">Liste d'ajouts d'attributs de ressources.</param>
    ''' <param name="suppr">Liste de suppressions d'attributs de ressources.</param>
    Public Overrides Sub VerifierAttrbRessr(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRessrAttrb), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRessrAttrb))
        TrouverModification(ajouts, suppr, Function(e1 As TsCdConnxRessrAttrb) e1.NomRessource + e1.CatgrRessource + IdCible + e1.NomAttrb)

        VerificationListe(ajouts, Function(e As TsCdConnxRessrAttrb) accesSage.EgaleAttrbRessr(e.NomRessource, e.CatgrRessource, IdCible, e.NomAttrb, e.Valeur))
        VerificationListe(suppr, Function(e As TsCdConnxRessrAttrb) accesSage.EgaleAttrbRessr(e.NomRessource, e.CatgrRessource, IdCible, e.NomAttrb, e.Valeur))
    End Sub

    ''' <summary>
    ''' Méthode qui vérifie si les ajouts ou les suppressions des liens Rôle/Ressource sont déja appliquées dans sage.
    ''' </summary>
    ''' <param name="ajouts">Liste d'ajouts de liens Rôle/Ressources.</param>
    ''' <param name="suppr">Liste de suppressions de liens Rôle/Ressources.</param>
    Public Overrides Sub VerifierLiensRoleRessr(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRoleRessr), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRoleRessr))
        VerificationListe(ajouts, Function(e As TsCdConnxRoleRessr) accesSage.PresentLienRoleRessr(e.NomRessource, e.CatgrRessource, IdCible, e.NomRole))
        VerificationListe(suppr, Function(e As TsCdConnxRoleRessr) accesSage.AbsentLienRoleRessr(e.NomRessource, e.CatgrRessource, IdCible, e.NomRole))
    End Sub

    ''' <summary>
    ''' Méthode qui vérifie si les ajouts ou les suppressions des liens Utilisateur/Ressource sont déja appliquées dans sage.
    ''' </summary>
    ''' <param name="ajouts">Liste d'ajouts de liens Utilisateur/Ressources.</param>
    ''' <param name="suppr">Liste de suppressions de liens Utilisateur/Ressources.</param>
    Public Overrides Sub VerifierLiensUserRessr(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRessr), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRessr))
        VerificationListe(ajouts, Function(e As TsCdConnxUserRessr) accesSage.PresentLienUserRessr(e.NomRessource, e.CatgrRessource, IdCible, e.CodeUtilisateur))
        VerificationListe(suppr, Function(e As TsCdConnxUserRessr) accesSage.AbsentLienUserRessr(e.NomRessource, e.CatgrRessource, IdCible, e.CodeUtilisateur))
    End Sub

#End Region

#Region "Fonctions de services"

    ''' <summary>
    ''' Fonction de services. Sert à tester les eurreurs qui sont survenus durant la création/supression d'une ressource.
    ''' </summary>
    ''' <param name="suppr">Liste d'éléments à supprimer. Peut contenir des éléments sans erreur.</param>
    ''' <param name="ajouts">Liste d'éléments à ajouter. Peut contenir des éléments sans erreur.</param>
    Private Sub TesterErreursRessr(ByVal ajouts As IEnumerable(Of TsCdConnxRessr), ByVal suppr As IEnumerable(Of TsCdConnxRessr))
        DepileurErreurRessr(ajouts, True, IdCible, AddressOf accesSage.DeffnErreurRessr)
        DepileurErreurRessr(suppr, False, IdCible, AddressOf accesSage.DeffnErreurRessr)
    End Sub

    ''' <summary>
    ''' Fonction de services. Sert à tester les eurreurs qui sont survenus durant la création/supression d'un lien rôle/ressource.
    ''' </summary>
    ''' <param name="suppr">Liste d'éléments à supprimer. Peut contenir des éléments sans erreur.</param>
    ''' <param name="ajouts">Liste d'éléments à ajouter. Peut contenir des éléments sans erreur.</param>
    Private Sub TesterErreursLienRoleRessr(ByVal ajouts As IEnumerable(Of TsCdConnxRoleRessr), ByVal suppr As IEnumerable(Of TsCdConnxRoleRessr))
        DepileurErreurRessr(ajouts, True, IdCible, AddressOf accesSage.DeffnErreurLienRoleRessr)
        DepileurErreurRessr(suppr, False, IdCible, AddressOf accesSage.DeffnErreurLienRoleRessr)
    End Sub

    ''' <summary>
    ''' Fonction de services. Sert à tester les eurreurs qui sont survenus durant la création/supression d'un lien utilisateur/ressource.
    ''' </summary>
    ''' <param name="suppr">Liste d'éléments à supprimer. Peut contenir des éléments sans erreur.</param>
    ''' <param name="ajouts">Liste d'éléments à ajouter. Peut contenir des éléments sans erreur.</param>
    Private Sub TesterErreursLienUserRessr(ByVal ajouts As IEnumerable(Of TsCdConnxUserRessr), ByVal suppr As IEnumerable(Of TsCdConnxUserRessr))
        DepileurErreurRessr(ajouts, True, IdCible, AddressOf accesSage.DeffnErreurLienUserRessr)
        DepileurErreurRessr(suppr, False, IdCible, AddressOf accesSage.DeffnErreurLienUserRessr)
    End Sub

    ''' <summary>
    ''' Fonction de services. Sert à tester les eurreurs qui sont survenus durant la création/supression d'un attribut d'une ressource.
    ''' </summary>
    ''' <param name="suppr">Liste d'éléments à supprimer. Peut contenir des éléments sans erreur.</param>
    ''' <param name="ajouts">Liste d'éléments à ajouter. Peut contenir des éléments sans erreur.</param>
    Private Sub TesterErreursRessrAttrb(ByVal ajouts As IEnumerable(Of TsCdConnxRessrAttrb), ByVal suppr As IEnumerable(Of TsCdConnxRessrAttrb))
        DepileurErreurRessr(ajouts, True, IdCible, AddressOf accesSage.DeffnErreurRessrAttbr)
        DepileurErreurRessr(suppr, False, IdCible, AddressOf accesSage.DeffnErreurRessrAttbr)
    End Sub

#End Region

End Class
