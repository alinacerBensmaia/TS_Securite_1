'*** Confirmer que j'ai seulement besoin d'une seule méthode... Comment on aiguille l'appelant?
'*** Attribut pour configurer le traitement de l'appelant


''' <summary>
''' Classe abstraite pour un connecteur à un système cible.
''' </summary>
Public MustInherit Class TsCuConnecteurCible
    Implements TsIConncMajDefntRole
    Implements TsIConncMajDefntUser
    Implements TsIConncMajDefntRessr
    Implements TsITerminologieCible
    Implements TsIConncInteraction

    Private _idCible As String
    Private _disposedValue As Boolean = False        ' Pour détecter les appels redondants

    ''' <summary>
    ''' Construit une instance du connecteur en spécifiant explicitement le système cible concerné.
    ''' </summary>
    ''' <param name="idCible">Identificateur du système cible (resName3 dans Sage).</param>
    Public Sub New(ByVal idCible As String)
        _idCible = idCible
    End Sub

    ''' <summary>
    ''' Identificateur du système cible (resName3 dans Sage).
    ''' </summary>
    Public ReadOnly Property IdCible() As String
        Get
            Return _idCible
        End Get
    End Property


    ''' <summary>
    ''' Description du système cible compréhensible par l'utilisateur.
    ''' </summary>
    ''' <value></value>
    ''' <remarks>
    ''' Par défaut c'est la même valeur que IdCible.
    ''' Peut être overridé par exemple pour des cibles plus complexes (ex: ID-Synch/TSS...)
    ''' </remarks>
    Public Overridable ReadOnly Property DescrCible() As String
        Get
            Return IdCible
        End Get
    End Property


    ' TsIConncDefntRole
    Public Overridable Function CreerRoles(ByVal ajouts As IEnumerable(Of TsCdConnxRole), Optional ByVal contnErreur As Boolean = False) As Boolean Implements TsIConncMajDefntRole.CreerRoles
        Throw New NotSupportedException("Le connecteur pour la cible " + Me.DescrCible + " ne supporte pas CreerRoles.")
    End Function

    Public Overridable Function DetruireRoles(ByVal suppr As IEnumerable(Of TsCdConnxRole), Optional ByVal contnErreur As Boolean = False) As Boolean Implements TsIConncMajDefntRole.DetruireRoles
        Throw New NotSupportedException("Le connecteur pour la cible " + Me.DescrCible + " ne supporte pas DetruireRoles.")
    End Function

    Public Overridable Function AppliquerAttrbRoles(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRoleAttrb), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRoleAttrb), Optional ByVal contnErreur As Boolean = False) As Boolean Implements TsIConncMajDefntRole.AppliquerAttrbRoles
        Throw New NotSupportedException("Le connecteur pour la cible " + Me.DescrCible + " ne supporte pas AppliquerAttrbRoles.")
    End Function

    Public Overridable Function AppliquerLiensRoleRole(ByVal ajouts As IEnumerable(Of TsCdConnxRoleRole), ByVal suppr As IEnumerable(Of TsCdConnxRoleRole), Optional ByVal contnErreur As Boolean = False) As Boolean Implements TsIConncMajDefntRole.AppliquerLiensRoleRole
        Throw New NotSupportedException("Le connecteur pour la cible " + Me.DescrCible + " ne supporte pas AppliquerLiensRoleRole.")
    End Function

    Public Overridable Function AppliquerLiensRoleRessr(ByVal ajouts As IEnumerable(Of TsCdConnxRoleRessr), ByVal suppr As IEnumerable(Of TsCdConnxRoleRessr), Optional ByVal contnErreur As Boolean = False) As Boolean Implements TsIConncMajDefntRole.AppliquerLiensRoleRessr
        Throw New NotSupportedException("Le connecteur pour la cible " + Me.DescrCible + " ne supporte pas AppliquerLiensRoleRessr.")
    End Function

    Public Overridable Sub VerifierRoles(ByVal ajouts As IEnumerable(Of TsCdConnxRole), ByVal suppr As IEnumerable(Of TsCdConnxRole)) Implements TsIConncMajDefntRole.VerifierRoles
        Throw New NotSupportedException("Le connecteur pour la cible " + Me.DescrCible + " ne supporte pas VerifierRoles.")
    End Sub

    Public Overridable Sub VerifierAttrbRoles(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRoleAttrb), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRoleAttrb)) Implements TsIConncMajDefntRole.VerifierAttrbRoles
        Throw New NotSupportedException("Le connecteur pour la cible " + Me.DescrCible + " ne supporte pas VerifierAttrbRoles.")
    End Sub

    Public Overridable Sub VerifierLiensRoleRole(ByVal ajouts As IEnumerable(Of TsCdConnxRoleRole), ByVal suppr As IEnumerable(Of TsCdConnxRoleRole)) Implements TsIConncMajDefntRole.VerifierLiensRoleRole
        Throw New NotSupportedException("Le connecteur pour la cible " + Me.DescrCible + " ne supporte pas VerifierLiensRoleRole.")
    End Sub

    Public Overridable Sub VerifierLiensRoleRessr(ByVal ajouts As IEnumerable(Of TsCdConnxRoleRessr), ByVal suppr As IEnumerable(Of TsCdConnxRoleRessr)) Implements TsIConncMajDefntRole.VerifierLiensRoleRessr
        Throw New NotSupportedException("Le connecteur pour la cible " + Me.DescrCible + " ne supporte pas VerifierLiensRoleRessr.")
    End Sub


    ' TsIConncDefntUser
    Public Overridable Function CreerUsers(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUser), Optional ByVal contnErreur As Boolean = False) As Boolean Implements TsIConncMajDefntUser.CreerUsers
        Throw New NotSupportedException("Le connecteur pour la cible " + Me.DescrCible + " ne supporte pas CreerUsers.")
    End Function

    Public Overridable Function DetruireUsers(ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUser), Optional ByVal contnErreur As Boolean = False) As Boolean Implements TsIConncMajDefntUser.DetruireUsers
        Throw New NotSupportedException("Le connecteur pour la cible " + Me.DescrCible + " ne supporte pas DetruireUsers.")
    End Function

    Public Overridable Function AppliquerAttrbUsers(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUserAttrb), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUserAttrb), Optional ByVal contnErreur As Boolean = False) As Boolean Implements TsIConncMajDefntUser.AppliquerAttrbUsers
        Throw New NotSupportedException("Le connecteur pour la cible " + Me.DescrCible + " ne supporte pas AppliquerAttrbUsers.")
    End Function

    Public Overridable Function AppliquerLiensUserRessr(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRessr), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRessr), Optional ByVal contnErreur As Boolean = False) As Boolean Implements TsIConncMajDefntUser.AppliquerLiensUserRessr
        Throw New NotSupportedException("Le connecteur pour la cible " + Me.DescrCible + " ne supporte pas AppliquerLiensUserRessr.")
    End Function

    Public Overridable Function AppliquerLiensUserRole(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRole), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRole), Optional ByVal contnErreur As Boolean = False) As Boolean Implements TsIConncMajDefntUser.AppliquerLiensUserRole
        Throw New NotSupportedException("Le connecteur pour la cible " + Me.DescrCible + " ne supporte pas AppliquerLiensUserRole.")
    End Function

    Public Overridable Sub VerifierUsers(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUser), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUser)) Implements TsIConncMajDefntUser.VerifierUsers
        Throw New NotSupportedException("Le connecteur pour la cible " + Me.DescrCible + " ne supporte pas VerifierUsers.")
    End Sub

    Public Overridable Sub VerifierAttrbUsers(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUserAttrb), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUserAttrb)) Implements TsIConncMajDefntUser.VerifierAttrbUsers
        Throw New NotSupportedException("Le connecteur pour la cible " + Me.DescrCible + " ne supporte pas VerifierAttrbUsers.")
    End Sub

    Public Overridable Sub VerifierLiensUserRessr(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRessr), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRessr)) Implements TsIConncMajDefntUser.VerifierLiensUserRessr
        Throw New NotSupportedException("Le connecteur pour la cible " + Me.DescrCible + " ne supporte pas VerifierLiensUserRessr.")
    End Sub

    Public Overridable Sub VerifierLiensUserRole(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRole), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRole)) Implements TsIConncMajDefntUser.VerifierLiensUserRole
        Throw New NotSupportedException("Le connecteur pour la cible " + Me.DescrCible + " ne supporte pas VerifierLiensUserRole.")
    End Sub

    ' TsIConncDefntRessr
    Public Overridable Function CreerRessr(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRessr), Optional ByVal contnErreur As Boolean = False) As Boolean Implements TsIConncMajDefntRessr.CreerRessr
        Throw New NotSupportedException("Le connecteur pour la cible " + Me.DescrCible + " ne supporte pas CreerRessr.")
    End Function

    Public Overridable Function DetruireRessr(ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRessr), Optional ByVal contnErreur As Boolean = False) As Boolean Implements TsIConncMajDefntRessr.DetruireRessr
        Throw New NotSupportedException("Le connecteur pour la cible " + Me.DescrCible + " ne supporte pas DetruireRessr.")
    End Function

    Public Overridable Function AppliquerAttrbRessr(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRessrAttrb), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRessrAttrb), Optional ByVal contnErreur As Boolean = False) As Boolean Implements TsIConncMajDefntRessr.AppliquerAttrbRessr
        Throw New NotSupportedException("Le connecteur pour la cible " + Me.DescrCible + " ne supporte pas AppliquerAttrbRessr.")
    End Function

    Public Overridable Sub VerifierAttrbRessr(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRessrAttrb), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRessrAttrb)) Implements TsIConncMajDefntRessr.VerifierAttrbRessr
        Throw New NotSupportedException("Le connecteur pour la cible " + Me.DescrCible + " ne supporte pas VerifierAttrbRessr.")
    End Sub

    Public Overridable Sub VerifierRessr(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRessr), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRessr)) Implements TsIConncMajDefntRessr.VerifierRessr
        Throw New NotSupportedException("Le connecteur pour la cible " + Me.DescrCible + " ne supporte pas VerifierRessr.")
    End Sub

    Public Overridable Function Traduire(ByVal role As TsCdConnxRole, Optional ByVal action As TsECcAction = TsECcAction.Aucune) As String Implements TsITerminologieCible.Traduire
        Select Case action
            Case TsECcAction.Aucune
                Return "rôle " + role.NomRole
            Case TsECcAction.Ajout
                Return "création du rôle " + role.NomRole
            Case TsECcAction.Suppression
                Return "destruction du rôle " + role.NomRole
            Case Else
                Throw New ArgumentOutOfRangeException("action: " + action.ToString())
        End Select
    End Function

    Public Overridable Function Traduire(ByVal roleRole As TsCdConnxRoleRole, Optional ByVal action As TsECcAction = TsECcAction.Aucune) As String Implements TsITerminologieCible.Traduire
        Select Case action
            Case TsECcAction.Aucune
                Return "héritage du rôle " + roleRole.NomSousRole + " par le rôle " + roleRole.NomRoleSup
            Case TsECcAction.Ajout
                Return "ajout de l'héritage du rôle " + roleRole.NomSousRole + " par le rôle " + roleRole.NomRoleSup
            Case TsECcAction.Suppression
                Return "supression de l'héritage du rôle " + roleRole.NomSousRole + " par le rôle " + roleRole.NomRoleSup
            Case Else
                Throw New ArgumentOutOfRangeException("action: " + action.ToString())
        End Select
    End Function

    Public Overridable Function Traduire(ByVal roleRessr As TsCdConnxRoleRessr, Optional ByVal action As TsECcAction = TsECcAction.Aucune) As String Implements TsITerminologieCible.Traduire
        Select Case action
            Case TsECcAction.Aucune
                Return "lien de la ressource " + roleRessr.NomRessource + " (" + roleRessr.CatgrRessource + ") au rôle " + roleRessr.NomRole
            Case TsECcAction.Ajout
                Return "association de la ressource " + roleRessr.NomRessource + " (" + roleRessr.CatgrRessource + ") au rôle " + roleRessr.NomRole
            Case TsECcAction.Suppression
                Return "dissociation de la ressource " + roleRessr.NomRessource + " (" + roleRessr.CatgrRessource + ") et du rôle " + roleRessr.NomRole
            Case Else
                Throw New ArgumentOutOfRangeException("action: " + action.ToString())
        End Select
    End Function

    Public Overridable Function Traduire(ByVal userRole As TsCdConnxUserRole, Optional ByVal action As TsECcAction = TsECcAction.Aucune) As String Implements TsITerminologieCible.Traduire
        Select Case action
            Case TsECcAction.Aucune
                Return "lien du rôle " + userRole.NomRole + " à l'utilisateur " + userRole.CodeUtilisateur
            Case TsECcAction.Ajout
                Return "ajout du rôle " + userRole.NomRole + " à l'utilisateur " + userRole.CodeUtilisateur
            Case TsECcAction.Suppression
                Return "suppression du rôle " + userRole.NomRole + " de l'utilisateur " + userRole.CodeUtilisateur
            Case Else
                Throw New ArgumentOutOfRangeException("action: " + action.ToString())
        End Select
    End Function

    Public Overridable Function Traduire(ByVal userRessr As TsCdConnxUserRessr, Optional ByVal action As TsECcAction = TsECcAction.Aucune) As String Implements TsITerminologieCible.Traduire
        Select Case action
            Case TsECcAction.Aucune
                Return "lien de la ressource " + userRessr.NomRessource + " (" + userRessr.CatgrRessource + ") au rôle " + userRessr.CodeUtilisateur
            Case TsECcAction.Ajout
                Return "association de la ressource " + userRessr.NomRessource + " (" + userRessr.CatgrRessource + ") au rôle " + userRessr.CodeUtilisateur
            Case TsECcAction.Suppression
                Return "dissociation de la ressource " + userRessr.NomRessource + " (" + userRessr.CatgrRessource + ") et du rôle " + userRessr.CodeUtilisateur
            Case Else
                Throw New ArgumentOutOfRangeException("action: " + action.ToString())
        End Select
    End Function

    ''' <remarks>
    ''' On suppose que le connecteur n'a pas d'état à réinitialiser. Ça devrait être le cas pour la plupart des connecteurs,
    ''' mais il se peut que certains connecteurs plus complexes ne supportent pas la réinitialisation passive. Dans ce cas
    ''' ils devront overrider cette méthode pour soit se réinitialiser correctement ou lancer une exception.
    ''' </remarks>
    Public Overridable Function Initialiser() As Boolean Implements TsIConncInteraction.Initialiser
        Return True
    End Function




#Region " IDisposable Support "

    ' IDisposable
    ''' <summary>
    ''' Fonction pour disposer les ressource. Sera éventuellement remplacer dans les hérités.
    ''' </summary>
    ''' <param name="disposing"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not _disposedValue Then
            If disposing Then
                '!  : libérez des ressources managées en cas d'appel explicite
            End If

            '!  : libérez des ressources non managées partagées
        End If
        _disposedValue = True
    End Sub

    ' Ce code a été ajouté par Visual Basic pour permettre l'implémentation correcte du modèle IDisposable.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Ne modifiez pas ce code. Ajoutez du code de nettoyage dans Dispose(ByVal disposing As Boolean) ci-dessus.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region


End Class

