''' <summary>
''' Connecteur pour Sage phase 3.
''' Contient les méthode de suppresion d'utilisateur et de rôle.
''' </summary>
Public Class TsCdConnecteurSagePhase3
    Inherits TsCdConnecteurSage

#Region "Propriétées"

    ''' <summary>
    ''' Description du système cible compréhensible pour l'utilisateur.
    ''' </summary>
    Public Overrides ReadOnly Property DescrCible() As String
        Get
            Return "Sage phase 3"
        End Get
    End Property

#End Region

#Region "Constructeurs"

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="IdCible">Identifiant du connecteur.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal IdCible As String)
        MyBase.New(IdCible)
    End Sub

    Public Sub New(ByVal IdCible As String, ByVal config As String)
        MyBase.New(IdCible, config)
    End Sub
#End Region

#Region "Méthodes"

    ''' <summary>
    ''' Méthode du connecteur Sage phase 3.
    ''' Cette méthode efface les rôles de la liste <paramref name="ajouts"/> dans la config de Sage.
    ''' </summary>
    ''' <param name="suppr">Liste de rôles à effacer.</param>
    ''' <param name="contnErreur">Indique si le traitement doit continuer ou non, si une erreur est rencontrée.</param>
    ''' <returns>Si la méthode a rencontrée au moins une erreur, elle retourne <c>False</c>, sinon elle retourne <c>True</c>.</returns>
    Public Overrides Function DetruireRoles(ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRole), Optional ByVal contnErreur As Boolean = False) As Boolean
        VerificationListe(suppr, Function(e As TsCdConnxRole) accesSage.AbsentRole(e.NomRole))
        Dim toutOk As Boolean = ApplicationListe(suppr, contnErreur, _
                                Function(e As TsCdConnxRole) accesSage.EffacerRole(e.NomRole))
        If toutOk = False Then
            TesterErreursRole(suppr)
        End If

        Return toutOk
    End Function

    ''' <summary>
    ''' Méthode du connecteur Sage phase 3.
    ''' Cette méthode efface les utilisateurs de la liste <paramref name="ajouts"/> dans la config de Sage.
    ''' </summary>
    ''' <param name="suppr">Liste d'utilisateurs à effacer.</param>
    ''' <param name="contnErreur">Indique si le traitement doit continuer ou non, si une erreur est rencontrée.</param>
    ''' <returns>Si la méthode a rencontrée au moins une erreur, elle retourne <c>False</c>, sinon elle retourne <c>True</c>.</returns>
    Public Overrides Function DetruireUsers(ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUser), Optional ByVal contnErreur As Boolean = False) As Boolean
        VerificationListe(suppr, Function(e As TsCdConnxUser) accesSage.AbsentUser(e.CodeUtilisateur))
        Dim toutOk As Boolean = ApplicationListe(suppr, contnErreur, _
                                Function(e As TsCdConnxUser) accesSage.EffacerUser(e.CodeUtilisateur))

        If toutOk = False Then
            TesterErreursUser(suppr)
        End If

        Return toutOk
    End Function

    ''' <summary>
    ''' Méthode du Connecteur Sage Phase 3.
    ''' Cette méthode va vérifier si les rôles des listes de rôles sont à jour dans Sage.
    ''' </summary>
    ''' <param name="ajouts">La liste des rôles à ajouter.</param>
    ''' <param name="suppr">La liste des rôles à effacer.</param>
    Public Overrides Sub VerifierRoles(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRole), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRole))
        VerificationListe(ajouts, Function(e As TsCdConnxRole) accesSage.PresentRole(e.NomRole))
        VerificationListe(suppr, Function(e As TsCdConnxRole) accesSage.AbsentRole(e.NomRole))
    End Sub

    ''' <summary>
    ''' Méthode du Connecteur Sage Phase 3.
    ''' Cette méthode va vérifier si les utilisateurs des listes de utilisateurs sont à jour dans Sage.
    ''' </summary>
    ''' <param name="ajouts">La liste des utilisateurs à ajouter.</param>
    ''' <param name="suppr">La liste des utilisateurs à effacer.</param>
    Public Overrides Sub VerifierUsers(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUser), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUser))
        VerificationListe(ajouts, Function(e As TsCdConnxUser) accesSage.PresentUser(e.CodeUtilisateur))
        VerificationListe(suppr, Function(e As TsCdConnxUser) accesSage.AbsentUser(e.CodeUtilisateur))
    End Sub

#End Region

#Region "Fonctions de services"

    ''' <summary>
    ''' Fonction de services. Sert à tester les eurreurs qui sont survenus durant la supression d'un utilisateur.
    ''' </summary>
    ''' <param name="suppr">Liste d'éléments à supprimer. Peut contenir des éléments sans erreur.</param>
    Private Sub TesterErreursUser(ByVal suppr As IEnumerable(Of TsCdConnxUser))
        DepileurErreur(suppr, False, AddressOf accesSage.DeffnErreurUser)
    End Sub

    ''' <summary>
    ''' Fonction de services. Sert à tester les eurreurs qui sont survenus durant la supression d'un rôle.
    ''' </summary>
    ''' <param name="suppr">Liste d'éléments à supprimer. Peut contenir des éléments sans erreur.</param>
    Private Sub TesterErreursRole(ByVal suppr As IEnumerable(Of TsCdConnxRole))
        DepileurErreur(suppr, False, AddressOf accesSage.DeffnErreurRole)
    End Sub

#End Region

End Class
