''' <summary>
''' Connecteur pour l'Active Directory.
''' </summary>
''' <remarks></remarks>
Public Class TsCdConnecteurAD
    Inherits TsCuConnecteurCibleIgnorable

#Region "Constante"
    Private Const ATTRB_ROLE_DESCRIPTION As String = "Role_Description"
    Private Const PREFIX_ROLE As String = "RE"
#End Region

#Region "Property"
    ''' <summary>
    ''' Description du système cible compréhensible par l'utilisateur.
    ''' </summary>
    Public Overrides ReadOnly Property DescrCible() As String
        Get
            Return "Active Directory"
        End Get
    End Property

#End Region

#Region "Constructeurs"

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    Public Sub New(ByVal idCible As String)
        MyBase.New(idCible)
    End Sub

#End Region

#Region "Méthodes"
    ''' <summary>
    ''' Méthode du Connecteur AD. 
    ''' Cette fonction va appliquer les ajouts des rôles de la liste <paramref name="ajouts"/> dans l'AD.
    ''' </summary>
    ''' <param name="ajouts">Liste de rôles à ajouter.</param>
    ''' <param name="contnErreur">
    ''' True = Si une erreur est rencontrée, l'erreur est prise en compte, mais le traitement continue.
    ''' False = Si une erreur est rencontrée, l'erreur est prise en compte et le traitement s'arrête immédiatement.
    ''' </param>
    Public Overrides Function CreerRoles(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRole), Optional ByVal contnErreur As Boolean = False) As Boolean
        Dim toutOk As Boolean = True

        For Each elementAjout As TsCdConnxRole In ajouts
            Try
                If elementAjout.CibleAJour < TsECcCibleAJour.ExtractionAJour Then
                    TsBaAccesAD.CreerGroupe(elementAjout.NomRole)
                    elementAjout.CibleAJour = TsECcCibleAJour.AJour
                End If
            Catch ex As TsExcObjetDejaExistantAD
                '! L'objet était à jour.
                elementAjout.CibleAJour = TsECcCibleAJour.AJour
            End Try
        Next

        Return toutOk
    End Function

    ''' <summary>
    ''' Méthode du Connecteur AD. 
    ''' Cette fonction va appliquer les suppressions des rôles dans l'AD à partir de la liste <paramref name="suppr"/>.
    ''' </summary>
    ''' <param name="suppr">Liste de rôles à supprimer.</param>
    ''' <param name="contnErreur">
    ''' True = Si une erreur est rencontrée, l'erreur est prise en compte, mais le traitement continue.
    ''' False = Si une erreur est rencontrée, l'erreur est prise en compte et le traitement s'arrête immédiatement.
    ''' </param>
    Public Overrides Function DetruireRoles(ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRole), Optional ByVal contnErreur As Boolean = False) As Boolean
        Dim toutOk As Boolean = True

        For Each elementSupp As TsCdConnxRole In suppr
            Try
                If elementSupp.CibleAJour < TsECcCibleAJour.ExtractionAJour Then
                    TsBaAccesAD.DetruireGroupe(elementSupp.NomRole)
                    elementSupp.CibleAJour = TsECcCibleAJour.AJour
                End If
            Catch ex As TsExcGroupeInexistant
                '! L'objet était à jour.
                elementSupp.CibleAJour = TsECcCibleAJour.AJour
            End Try
        Next

        Return toutOk
    End Function

    ''' <summary>
    ''' Méthode du Connecteur AD. Cette fonction va appliquer les ajouts des liens Utilisateur/Ressource de la liste <paramref name="ajouts"/> dans l'AD
    ''' et va appliquer les suppressions des liens Utilisateur/Ressource dans l'AD à partir de la liste <paramref name="suppr"/>.
    ''' </summary>
    ''' <param name="ajouts">Liste de liens Utilisateur/Ressource à ajouter.</param>
    ''' <param name="suppr">Liste de liens Utilisateur/Ressource à supprimer.</param>
    ''' <param name="contnErreur">
    ''' True = Si une erreur est rencontrée, l'erreur est prise en compte, mais le traitement continue.
    ''' False = Si une erreur est rencontrée, l'erreur est prise en compte et le traitement s'arrête immédiatement.
    ''' </param>
    ''' <returns>Si la méthode a rencontrée au moins une erreur, elle retourne <c>False</c>, sinon elle retourne <c>True</c>.</returns>
    Public Overrides Function AppliquerLiensUserRessr(ByVal ajouts As IEnumerable(Of TsCdConnxUserRessr), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRessr), Optional ByVal contnErreur As Boolean = False) As Boolean
        Dim toutOk As Boolean = True

        For Each elementAjout As TsCdConnxUserRessr In ajouts
            Try
                If elementAjout.CibleAJour < TsECcCibleAJour.ExtractionAJour Then
                    TsBaAccesAD.AjouterMembreGroupe(elementAjout.NomRessource, elementAjout.CodeUtilisateur)
                    elementAjout.CibleAJour = TsECcCibleAJour.AJour
                End If
            Catch ex As TsExcGroupeInexistant
                toutOk = False
                elementAjout.CibleAJour = TsECcCibleAJour.PasAJour
                elementAjout.DescriptionErreur = "La ressource est inexistante."
                If contnErreur = False Then
                    Return False
                End If

            Catch ex As TsExcMembreInexistant
                toutOk = False
                elementAjout.CibleAJour = TsECcCibleAJour.PasAJour
                elementAjout.DescriptionErreur = "L'utilisateur est inexistant."
                If contnErreur = False Then
                    Return False
                End If

            Catch ex As TsExcLienDejaExistantAD
                '! L'objet était à jour.
                elementAjout.CibleAJour = TsECcCibleAJour.AJour
            Catch ex As TsExcObjetDejaExistantAD
                '! L'objet était à jour.
                elementAjout.CibleAJour = TsECcCibleAJour.AJour
            End Try
        Next

        For Each elementSupp As TsCdConnxUserRessr In suppr
            Try
                If elementSupp.CibleAJour < TsECcCibleAJour.ExtractionAJour Then
                    ' Si la ressource commence par RE, c'est un rôle donc on touche pas. 
                    ' Certains roles ont le même nom qu'une ressource.
                    If Not elementSupp.NomRessource.StartsWith(PREFIX_ROLE) Then
                        TsBaAccesAD.EnleverMembreGroupe(elementSupp.NomRessource, elementSupp.CodeUtilisateur)
                    End If
                    elementSupp.CibleAJour = TsECcCibleAJour.AJour
                End If
            Catch ex As TsExcGroupeInexistant
                toutOk = False
                elementSupp.CibleAJour = TsECcCibleAJour.PasAJour
                elementSupp.DescriptionErreur = "La ressource est inexistante."
                If contnErreur = False Then
                    Return False
                End If

            Catch ex As TsExcMembreInexistant
                toutOk = False
                elementSupp.CibleAJour = TsECcCibleAJour.PasAJour
                elementSupp.DescriptionErreur = "L'utilisateur est inexistant."
                If contnErreur = False Then
                    Return False
                End If


            Catch ex As TsExcLienInexistantAD
                '! L'objet était à jour.
                elementSupp.CibleAJour = TsECcCibleAJour.AJour
            Catch ex As TsExcServeurRefuseOperation '! Autre facon d'alerter que le lien est inexistant.
                '! L'objet était à jour.
                elementSupp.CibleAJour = TsECcCibleAJour.AJour
            End Try
        Next

        Return toutOk
    End Function

    ''' <summary>
    ''' Méthode du Connecteur AD. Cette fonction va appliquer les ajouts des liens Utilisateur/Role de la liste <paramref name="ajouts"/> dans l'AD
    ''' et va appliquer les suppressions des liens Utilisateur/Role dans l'AD à partir de la liste <paramref name="suppr"/>.
    ''' </summary>
    ''' <param name="ajouts">Liste de liens Utilisateur/Role à ajouter.</param>
    ''' <param name="suppr">Liste de liens Utilisateur/Role à supprimer.</param>
    ''' <param name="contnErreur">
    ''' True = Si une erreur est rencontrée, l'erreur est prise en compte, mais le traitement continue.
    ''' False =  Si une erreur est rencontrée, l'erreur est prise en compte et le traitement s'arrête immédiatement.
    ''' </param>
    ''' <returns>Si la méthode a rencontrée au moins une erreur, elle retourne <c>False</c>, sinon elle retourne <c>True</c>.</returns>
    Public Overrides Function AppliquerLiensUserRole(ByVal ajouts As IEnumerable(Of TsCdConnxUserRole), ByVal suppr As IEnumerable(Of TsCdConnxUserRole), Optional ByVal contnErreur As Boolean = False) As Boolean
        Dim toutOk As Boolean = True

        For Each elementAjout As TsCdConnxUserRole In ajouts
            Try
                If elementAjout.CibleAJour < TsECcCibleAJour.ExtractionAJour Then
                    TsBaAccesAD.AjouterMembreGroupe(elementAjout.NomRole, elementAjout.CodeUtilisateur)
                    elementAjout.CibleAJour = TsECcCibleAJour.AJour
                End If
            Catch ex As TsExcGroupeInexistant
                toutOk = False
                elementAjout.CibleAJour = TsECcCibleAJour.PasAJour
                elementAjout.DescriptionErreur = "Le rôle est inexistant."
                If contnErreur = False Then
                    Return False
                End If
            Catch ex As TsExcMembreInexistant
                toutOk = False
                elementAjout.CibleAJour = TsECcCibleAJour.PasAJour
                elementAjout.DescriptionErreur = "L'utilisateur est inexistant."
                If contnErreur = False Then
                    Return False
                End If

            Catch ex As TsExcLienDejaExistantAD
                '! L'objet était à jour.
                elementAjout.CibleAJour = TsECcCibleAJour.AJour
            Catch ex As TsExcObjetDejaExistantAD
                '! L'objet était à jour.
                elementAjout.CibleAJour = TsECcCibleAJour.AJour
            End Try
        Next

        For Each elementSupp As TsCdConnxUserRole In suppr
            Try
                If elementSupp.CibleAJour < TsECcCibleAJour.ExtractionAJour Then
                    TsBaAccesAD.EnleverMembreGroupe(elementSupp.NomRole, elementSupp.CodeUtilisateur)
                    elementSupp.CibleAJour = TsECcCibleAJour.AJour
                End If
            Catch ex As TsExcGroupeInexistant
                '! L'objet était à jour.
                elementSupp.CibleAJour = TsECcCibleAJour.AJour
            Catch ex As TsExcMembreInexistant
                toutOk = False
                elementSupp.CibleAJour = TsECcCibleAJour.PasAJour
                elementSupp.DescriptionErreur = "L'utilisateur est inexistant."
                If contnErreur = False Then
                    Return False
                End If

            Catch ex As TsExcLienInexistantAD
                '! L'objet était à jour.
                elementSupp.CibleAJour = TsECcCibleAJour.AJour
            End Try
        Next

        Return toutOk
    End Function

    ''' <summary>
    ''' Méthode du Connecteur AD. Cette fonction va appliquer les ajouts des liens Rôle/Rôle de la liste <paramref name="ajouts"/> dans l'AD
    ''' et va appliquer les suppressions des liens Rôle/Rôle dans l'AD à partir de la liste <paramref name="suppr"/>.
    ''' </summary>
    ''' <param name="ajouts">Liste de liens Rôle/Rôle à ajouter.</param>
    ''' <param name="suppr">Liste de liens Rôle/Rôle à supprimer.</param>
    ''' <param name="contnErreur">
    ''' True = Si une erreur est rencontrée, l'erreur est prise en compte, mais le traitement continue.
    ''' False = Si une erreur est rencontrée, l'erreur est prise en compte et le traitement s'arrête immédiatement.
    ''' </param>
    ''' <returns>Si la méthode a rencontrée au moins une erreur, elle retourne <c>False</c>, sinon elle retourne <c>True</c>.</returns>
    Public Overrides Function AppliquerLiensRoleRole(ByVal ajouts As IEnumerable(Of TsCdConnxRoleRole), ByVal suppr As IEnumerable(Of TsCdConnxRoleRole), Optional ByVal contnErreur As Boolean = False) As Boolean
        Dim toutOk As Boolean = True

        For Each elementAjout As TsCdConnxRoleRole In ajouts
            Try
                If elementAjout.CibleAJour < TsECcCibleAJour.ExtractionAJour Then
                    TsBaAccesAD.AjouterMembreGroupe(elementAjout.NomSousRole, elementAjout.NomRoleSup)
                    elementAjout.CibleAJour = TsECcCibleAJour.AJour
                End If
            Catch ex As TsExcGroupeInexistant
                toutOk = False
                elementAjout.CibleAJour = TsECcCibleAJour.PasAJour
                elementAjout.DescriptionErreur = "Le sous rôle est inexistant."
                If contnErreur = False Then
                    Return False
                End If

            Catch ex As TsExcMembreInexistant
                toutOk = False
                elementAjout.CibleAJour = TsECcCibleAJour.PasAJour
                elementAjout.DescriptionErreur = "Le rôle supérieur est inexistant."
                If contnErreur = False Then
                    Return False
                End If

            Catch ex As TsExcLienDejaExistantAD
                '! L'objet était à jour.
                elementAjout.CibleAJour = TsECcCibleAJour.AJour
            Catch ex As TsExcObjetDejaExistantAD
                '! L'objet était à jour.
                elementAjout.CibleAJour = TsECcCibleAJour.AJour
            End Try
        Next

        For Each elementSupp As TsCdConnxRoleRole In suppr
            Try
                If elementSupp.CibleAJour < TsECcCibleAJour.ExtractionAJour Then
                    TsBaAccesAD.EnleverMembreGroupe(elementSupp.NomSousRole, elementSupp.NomRoleSup)
                    elementSupp.CibleAJour = TsECcCibleAJour.AJour
                End If
            Catch ex As TsExcGroupeInexistant
                '! L'objet était à jour.
                elementSupp.CibleAJour = TsECcCibleAJour.AJour
            Catch ex As TsExcMembreInexistant
                '! L'objet était à jour.
                elementSupp.CibleAJour = TsECcCibleAJour.AJour
            Catch ex As TsExcLienInexistantAD
                '! L'objet était à jour.
                elementSupp.CibleAJour = TsECcCibleAJour.AJour
            End Try
        Next

        Return toutOk
    End Function

    ''' <summary>
    ''' Méthode du Connecteur AD. Cette fonction va appliquer les ajouts des liens Rôle/Ressource de la liste <paramref name="ajouts"/> dans l'AD
    ''' et va appliquer les suppressions des liens Rôle/Ressource dans l'AD à partir de la liste <paramref name="suppr"/>.
    ''' </summary>
    ''' <param name="ajouts">Liste de liens Rôle/Ressource à ajouter.</param>
    ''' <param name="suppr">Liste de liens Rôle/Ressource à supprimer.</param>
    ''' <param name="contnErreur">
    ''' True = Si une erreur est rencontrée, l'erreur est prise en compte, mais le traitement continue.
    ''' False = Si une erreur est rencontrée, l'erreur est prise en compte et le traitement s'arrête immédiatement.
    ''' </param>
    ''' <returns>Si la méthode a rencontrée au moins une erreur, elle retourne <c>False</c>, sinon elle retourne <c>True</c>.</returns>
    Public Overrides Function AppliquerLiensRoleRessr(ByVal ajouts As IEnumerable(Of TsCdConnxRoleRessr), ByVal suppr As IEnumerable(Of TsCdConnxRoleRessr), Optional ByVal contnErreur As Boolean = False) As Boolean
        Dim toutOk As Boolean = True

        For Each elementAjout As TsCdConnxRoleRessr In ajouts
            Try
                If elementAjout.CibleAJour < TsECcCibleAJour.ExtractionAJour Then
                    ' Cas special si la resource a le meme nom que le role. On ne fais pas de reference sur lui même.
                    If elementAjout.NomRessource <> elementAjout.NomRole Then
                        TsBaAccesAD.AjouterMembreGroupe(elementAjout.NomRessource, elementAjout.NomRole)
                    End If
                    elementAjout.CibleAJour = TsECcCibleAJour.AJour
                End If
            Catch ex As TsExcGroupeInexistant
                toutOk = False
                elementAjout.CibleAJour = TsECcCibleAJour.PasAJour
                elementAjout.DescriptionErreur = "Le rôle est inexistant."
                If contnErreur = False Then
                    Return False
                End If

            Catch ex As TsExcMembreInexistant
                toutOk = False
                elementAjout.CibleAJour = TsECcCibleAJour.PasAJour
                elementAjout.DescriptionErreur = "La ressource est inexistante."
                If contnErreur = False Then
                    Return False
                End If

            Catch ex As TsExcLienDejaExistantAD
                '! L'objet était à jour.
                elementAjout.CibleAJour = TsECcCibleAJour.AJour
            Catch ex As TsExcObjetDejaExistantAD
                '! L'objet était à jour.
                elementAjout.CibleAJour = TsECcCibleAJour.AJour
            Catch ex As TsExcServeurRefuseOperation '! Des groupes avec certains type dans L'AD ne peuvent possèder d'autres groupes d'un certains type. Ex: "Groupe global ne peut possèdé un groupe local."
                toutOk = False
                elementAjout.CibleAJour = TsECcCibleAJour.PasAJour
                elementAjout.DescriptionErreur = "Certaine ressources ne peuvent pas être membre des rôles. Ex: Groupe universel dans un groupe global."
                If contnErreur = False Then
                    Return False
                End If

            End Try
        Next

        For Each elementSupp As TsCdConnxRoleRessr In suppr
            Try
                If elementSupp.CibleAJour < TsECcCibleAJour.ExtractionAJour Then
                    ' Cas special si la resource a le meme nom que le role. On ne supprime pas le groupe
                    If elementSupp.NomRessource <> elementSupp.NomRole Then
                        TsBaAccesAD.EnleverMembreGroupe(elementSupp.NomRessource, elementSupp.NomRole)
                    End If
                    elementSupp.CibleAJour = TsECcCibleAJour.AJour
                End If
            Catch ex As TsExcGroupeInexistant
                '! L'objet était à jour.
                elementSupp.CibleAJour = TsECcCibleAJour.AJour
            Catch ex As TsExcMembreInexistant
                toutOk = False
                elementSupp.CibleAJour = TsECcCibleAJour.PasAJour
                elementSupp.DescriptionErreur = "La ressource est inexistante."
                If contnErreur = False Then
                    Return False
                End If

            Catch ex As TsExcLienInexistantAD
                '! L'objet était à jour.
                elementSupp.CibleAJour = TsECcCibleAJour.AJour
            Catch ex As TsExcServeurRefuseOperation '! Autre facon d'alerter que le lien est inexistant.
                '! L'objet était à jour.
                elementSupp.CibleAJour = TsECcCibleAJour.AJour
            End Try
        Next

        Return toutOk
    End Function

    ''' <summary>
    ''' Vérifie dans l'AD les liens entre les utilisateurs et les ressources qui sont déjà existants ou inexistants.
    ''' </summary>
    ''' <param name="ajouts">Liens dont l'inexistance est à vérifier dans l'AD.</param>
    ''' <param name="suppr">Liens dont l'existance est à vérifier dans l'AD.</param>
    ''' <remarks>
    ''' Pour chaque liens à ajouter, si dans l'AD le lien n'est pas fait, le lien verra sa variable
    ''' <see cref="TsCdConnxUserRessr.CibleAJour"/> changé à la valeurs <see cref="TsECcCibleAJour.AJour" />,
    ''' sinon elle sera changé à <see cref="TsECcCibleAJour.PasAJour" />.
    ''' Pour chaque liens à supprimer, si dans l'AD le lien est fait, alors le lien verra sa variable
    ''' <see cref="TsCdConnxUserRessr.CibleAJour"/> changé à la valeurs <see cref="TsECcCibleAJour.AJour" />,
    ''' sinon elle sera changé à <see cref="TsECcCibleAJour.PasAJour" />.
    ''' </remarks>
    Public Overrides Sub VerifierLiensUserRessr(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRessr), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRessr))
        For Each elementAjout As TsCdConnxUserRessr In ajouts
            Try
                If TsBaAccesAD.EstMembreGroupe(elementAjout.CodeUtilisateur, elementAjout.NomRessource) = True Then
                    elementAjout.CibleAJour = TsECcCibleAJour.AJour
                Else
                    elementAjout.CibleAJour = TsECcCibleAJour.PasAJour
                End If
            Catch ex As TsExcGroupeInexistant
                elementAjout.CibleAJour = TsECcCibleAJour.PasAJour
            End Try
        Next

        For Each elementSupp As TsCdConnxUserRessr In suppr
            Try
                ' Si la ressource commence par RE, c'est un rôle donc c'est pas un lien utilisateur-ressource. 
                ' Certains roles ont le même nom qu'une ressource.
                If TsBaAccesAD.EstMembreGroupe(elementSupp.CodeUtilisateur, elementSupp.NomRessource) = True _
                        AndAlso Not elementSupp.NomRessource.StartsWith(PREFIX_ROLE) Then
                    elementSupp.CibleAJour = TsECcCibleAJour.PasAJour
                Else
                    elementSupp.CibleAJour = TsECcCibleAJour.AJour
                End If
            Catch ex As TsExcGroupeInexistant
                elementSupp.CibleAJour = TsECcCibleAJour.AJour
            End Try
        Next
    End Sub

    ''' <summary>
    ''' Vérifie dans l'AD les liens entre les utilisateurs et les rôles qui sont déjà existants ou inexistants.
    ''' </summary>
    ''' <param name="ajouts">Liens dont l'inexistance est à vérifier dans l'AD.</param>
    ''' <param name="suppr">Liens dont l'existance est à vérifier dans l'AD.</param>
    ''' <remarks>
    ''' Pour chaque liens à ajouter, si dans l'AD le lien n'est pas fait, le lien verra sa variable
    ''' <see cref="TsCdConnxUserRessr.CibleAJour"/> changé à la valeurs <see cref="TsECcCibleAJour.AJour" />,
    ''' sinon elle sera changé à <see cref="TsECcCibleAJour.PasAJour" />.
    ''' Pour chaque liens à supprimer, si dans l'AD le lien est fait, alors le lien verra sa variable
    ''' <see cref="TsCdConnxUserRessr.CibleAJour"/> changé à la valeurs <see cref="TsECcCibleAJour.AJour" />,
    ''' sinon elle sera changé à <see cref="TsECcCibleAJour.PasAJour" />.
    ''' </remarks>
    Public Overrides Sub VerifierLiensUserRole(ByVal ajouts As IEnumerable(Of TsCdConnxUserRole), ByVal suppr As IEnumerable(Of TsCdConnxUserRole))
        For Each elementAjout As TsCdConnxUserRole In ajouts
            Try
                If TsBaAccesAD.EstMembreGroupe(elementAjout.CodeUtilisateur, elementAjout.NomRole) = True Then
                    elementAjout.CibleAJour = TsECcCibleAJour.AJour
                Else
                    elementAjout.CibleAJour = TsECcCibleAJour.PasAJour
                End If
            Catch ex As TsExcGroupeInexistant
                elementAjout.CibleAJour = TsECcCibleAJour.PasAJour
            End Try
        Next

        For Each elementSupp As TsCdConnxUserRole In suppr
            Try
                If TsBaAccesAD.EstMembreGroupe(elementSupp.CodeUtilisateur, elementSupp.NomRole) = True Then
                    elementSupp.CibleAJour = TsECcCibleAJour.PasAJour
                Else
                    elementSupp.CibleAJour = TsECcCibleAJour.AJour
                End If
            Catch ex As TsExcGroupeInexistant
                elementSupp.CibleAJour = TsECcCibleAJour.AJour
            End Try
        Next
    End Sub

    ''' <summary>
    ''' Vérifie dans l'AD les rôles qui sont déjà existants ou inexistants.
    ''' </summary>
    ''' <param name="ajouts">Roles dont l'inexistance est à vérifier dans l'AD.</param>
    ''' <param name="suppr">Roles dont l'existance est à vérifier dans l'AD.</param>
    ''' <remarks>
    ''' Cette méthode doit vérifier l'état des rôles dans l'AD et
    ''' marquer les rôles déjà existants de la liste <paramref name="ajouts" />
    ''' ainsi que les rôles déjà inexistants de la liste <paramref name="suppr" />
    ''' en changeant la variable <see cref="TsCdConnxRole.CibleAJour"/> une des valeurs
    ''' <see cref="TsECcCibleAJour.PasAJour" /> ou <see cref="TsECcCibleAJour.AJour" />.
    ''' </remarks>
    Public Overrides Sub VerifierRoles(ByVal ajouts As IEnumerable(Of TsCdConnxRole), ByVal suppr As IEnumerable(Of TsCdConnxRole))
        For Each elementAjout As TsCdConnxRole In ajouts

            If TsBaAccesAD.GroupeExiste(elementAjout.NomRole) = True Then
                elementAjout.CibleAJour = TsECcCibleAJour.AJour
            Else
                elementAjout.CibleAJour = TsECcCibleAJour.PasAJour
            End If
        Next

        For Each elementSuppr As TsCdConnxRole In suppr
            If TsBaAccesAD.GroupeExiste(elementSuppr.NomRole) = True Then
                elementSuppr.CibleAJour = TsECcCibleAJour.PasAJour
            Else
                elementSuppr.CibleAJour = TsECcCibleAJour.AJour
            End If
        Next
    End Sub

    ''' <summary>
    ''' Vérifie dans l'AD les liens d'héritages entre les rôles qui sont déjà existants ou inexistants.
    ''' </summary>
    ''' <param name="ajouts">Liens dont l'inexistance est à vérifier dans l'AD.</param>
    ''' <param name="suppr">Liens dont l'existance est à vérifier dans l'AD.</param>
    ''' <remarks>
    ''' Pour chaque liens à ajouter, si dans l'AD le lien n'est pas fait, le lien verra sa variable
    ''' <see cref="TsCdConnxUserRessr.CibleAJour"/> changé à la valeurs <see cref="TsECcCibleAJour.AJour" />,
    ''' sinon elle sera changé à <see cref="TsECcCibleAJour.PasAJour" />.
    ''' Pour chaque liens à supprimer, si dans l'AD le lien est fait, alors le lien verra sa variable
    ''' <see cref="TsCdConnxUserRessr.CibleAJour"/> changé à la valeurs <see cref="TsECcCibleAJour.AJour" />,
    ''' sinon elle sera changé à <see cref="TsECcCibleAJour.PasAJour" />.
    ''' </remarks>
    Public Overrides Sub VerifierLiensRoleRole(ByVal ajouts As IEnumerable(Of TsCdConnxRoleRole), ByVal suppr As IEnumerable(Of TsCdConnxRoleRole))
        For Each elementAjout As TsCdConnxRoleRole In ajouts
            Try
                If TsBaAccesAD.EstMembreGroupe(elementAjout.NomRoleSup, elementAjout.NomSousRole) = True Then
                    elementAjout.CibleAJour = TsECcCibleAJour.AJour
                Else
                    elementAjout.CibleAJour = TsECcCibleAJour.PasAJour
                End If
            Catch ex As TsExcGroupeInexistant
                elementAjout.CibleAJour = TsECcCibleAJour.PasAJour
            End Try
        Next

        For Each elementSuppr As TsCdConnxRoleRole In suppr
            Try
                If TsBaAccesAD.EstMembreGroupe(elementSuppr.NomRoleSup, elementSuppr.NomSousRole) = True Then
                    elementSuppr.CibleAJour = TsECcCibleAJour.PasAJour
                Else
                    elementSuppr.CibleAJour = TsECcCibleAJour.AJour
                End If
            Catch ex As TsExcGroupeInexistant
                elementSuppr.CibleAJour = TsECcCibleAJour.AJour
            End Try
        Next
    End Sub

    ''' <summary>
    ''' Vérifie dans l'AD les liens entre les rôles et les ressources qui sont déjà existants ou inexistants.
    ''' </summary>
    ''' <param name="ajouts">Liens dont l'inexistance est à vérifier dans l'AD.</param>
    ''' <param name="suppr">Liens dont l'existance est à vérifier dans l'AD.</param>
    ''' <remarks>
    ''' Pour chaque liens à ajouter, si dans l'AD le lien n'est pas fait, le lien verra sa variable
    ''' <see cref="TsCdConnxUserRessr.CibleAJour"/> changé à la valeurs <see cref="TsECcCibleAJour.AJour" />,
    ''' sinon elle sera changé à <see cref="TsECcCibleAJour.PasAJour" />.
    ''' Pour chaque liens à supprimer, si dans l'AD le lien est fait, alors le lien verra sa variable
    ''' <see cref="TsCdConnxUserRessr.CibleAJour"/> changé à la valeurs <see cref="TsECcCibleAJour.AJour" />,
    ''' sinon elle sera changé à <see cref="TsECcCibleAJour.PasAJour" />.
    ''' </remarks>
    Public Overrides Sub VerifierLiensRoleRessr(ByVal ajouts As IEnumerable(Of TsCdConnxRoleRessr), ByVal suppr As IEnumerable(Of TsCdConnxRoleRessr))
        For Each elementAjout As TsCdConnxRoleRessr In ajouts
            Try
                ' Cas special si la resource a le meme nom que le role. On ne fais pas de reference sur lui même.
                If elementAjout.NomRessource = elementAjout.NomRole Then
                    elementAjout.CibleAJour = TsECcCibleAJour.AJour
                Else
                    If TsBaAccesAD.EstMembreGroupe(elementAjout.NomRole, elementAjout.NomRessource) = True Then
                        elementAjout.CibleAJour = TsECcCibleAJour.AJour
                    Else
                        elementAjout.CibleAJour = TsECcCibleAJour.PasAJour
                    End If
                End If
            Catch ex As TsExcGroupeInexistant
                elementAjout.CibleAJour = TsECcCibleAJour.PasAJour
            End Try
        Next

        For Each elementSuppr As TsCdConnxRoleRessr In suppr
            Try
                ' Cas special si la resource a le meme nom que le role. On ne fais pas de reference sur lui même.
                If elementSuppr.NomRessource = elementSuppr.NomRole Then
                    elementSuppr.CibleAJour = TsECcCibleAJour.AJour
                Else
                    If TsBaAccesAD.EstMembreGroupe(elementSuppr.NomRole, elementSuppr.NomRessource) = True Then
                        elementSuppr.CibleAJour = TsECcCibleAJour.PasAJour
                    Else
                        elementSuppr.CibleAJour = TsECcCibleAJour.AJour
                    End If
                End If
            Catch ex As TsExcGroupeInexistant
                elementSuppr.CibleAJour = TsECcCibleAJour.AJour
            End Try
        Next
    End Sub

    ''' <summary>
    ''' Méthode du Connecteur AD 
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

        For Each ajout As TsCdConnxRoleAttrb In ajouts
            Try
                If ajout.NomAttrb = ATTRB_ROLE_DESCRIPTION Then
                    TsBaAccesAD.EcrireDescriptionGroupe(ajout.NomRole, ajout.Valeur)
                End If
            Catch ex As TsExcGroupeInexistant
                ajout.CibleAJour = TsECcCibleAJour.PasAJour
            End Try
        Next

        Return True
    End Function

    ''' <summary>
    ''' Méthode du connecteur Sage Phase 1.
    ''' Cette méthode va vérifier si les attributs des rôles sont à jour dans Sage.
    ''' </summary>
    ''' <param name="ajouts">Liste d'attributs de rôles à ajouter.</param>
    ''' <param name="suppr">Liste d'attributs de rôle à supprimer</param>
    Public Overrides Sub VerifierAttrbRoles(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRoleAttrb), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRoleAttrb))
        For Each ajout As TsCdConnxRoleAttrb In ajouts
            If ajout.NomAttrb = ATTRB_ROLE_DESCRIPTION Then
                Try
                    If TsBaAccesAD.ObtenirDescriptionGroupe(ajout.NomRole) = ajout.Valeur Then
                        ajout.CibleAJour = TsECcCibleAJour.AJour
                    Else
                        ajout.CibleAJour = TsECcCibleAJour.PasAJour
                    End If
                Catch ex As TsExcGroupeInexistant
                    ajout.CibleAJour = TsECcCibleAJour.PasAJour
                End Try
            Else
                'On ignore les autres champs
                ajout.CibleAJour = TsECcCibleAJour.AJour
            End If
        Next

        'Si on recoit un attribut a supprimer, il y'a probablement un ajout pour ce même champ.
        'On ignore donc les attributs a supprimer.
        For Each supp As TsCdConnxRoleAttrb In suppr
            supp.CibleAJour = TsECcCibleAJour.AJour
        Next
    End Sub

#End Region

End Class
