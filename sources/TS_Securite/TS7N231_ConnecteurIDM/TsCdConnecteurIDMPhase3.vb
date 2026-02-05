''' <summary>
''' Connecteur pour ID Manager phase 3.
''' </summary>
''' <remarks></remarks>
Public Class TsCdConnecteurIDMPhase3
    Inherits TsCdConnecteurIDM
#Region "Propriétées"
    ''' <summary>
    ''' Description du système cible compréhensible pour l'utilisateur.
    ''' </summary>
    Public Overrides ReadOnly Property DescrCible() As String
        Get
            Return "ID Manager phase 3"
        End Get
    End Property
#End Region

#Region "Constructeurs"
    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="IdCible">Identifiant du conneceteur</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal IdCible As String)
        MyBase.New(IdCible)
    End Sub
#End Region

#Region "Méthodes"

    ''' <summary>
    ''' Méthode du Connecteur IDM Phase 3.
    ''' Détruit les rôles à partir de la liste <paramref name="suppr"/>.
    ''' </summary>
    ''' <param name="suppr">La liste des rôles à détruire.</param>
    ''' <param name="contnErreur">
    ''' True = continue malgré les erreurs rencontrée.
    ''' False = arrête dès qu'une erreur est rencontrée.
    ''' </param>
    ''' <returns>
    ''' True si aucune erreur n'a été rencontré.
    ''' False si au moins une erreur a été rencontré.
    ''' </returns>
    ''' <remarks></remarks>
    Public Overrides Function DetruireRoles(ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRole), Optional ByVal contnErreur As Boolean = False) As Boolean
        Dim toutOk As Boolean = True

        For Each elementSuppr As TsCdConnxRole In suppr
            If elementSuppr.CibleAJour < TsECcCibleAJour.ExtractionAJour Then
                Dim descriptionErreur As String = accesIDM.EffacerRoleIDM(elementSuppr.NomRole)
                If descriptionErreur = "" Then
                    elementSuppr.CibleAJour = TsECcCibleAJour.AJour
                Else
                    elementSuppr.CibleAJour = TsECcCibleAJour.PasAJour
                    elementSuppr.DescriptionErreur = descriptionErreur
                    toutOk = False
                    If contnErreur = False Then
                        Exit For
                    End If
                End If
            End If
        Next

        Return toutOk
    End Function

    ''' <summary>
    '''Méthode du Connecteur IDM Phase3. Cette fonction va appliquer les ajouts des liens Utilisateur/Role de la liste <paramref name="ajouts"/> dans l'ID manager
    ''' et va appliquer les suppressions des liens Utilisateur/Role dans l'ID manager à partir de la liste <paramref name="suppr"/>.
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
            If elementAjout.CibleAJour < TsECcCibleAJour.ExtractionAJour Then
                Dim descriptionErreur As String = accesIDM.AjouterLienUtilisateurRoleIDM(elementAjout.CodeUtilisateur, elementAjout.NomRole)
                If descriptionErreur = "" Then
                    elementAjout.CibleAJour = TsECcCibleAJour.AJour
                Else
                    elementAjout.CibleAJour = TsECcCibleAJour.PasAJour
                    elementAjout.DescriptionErreur = descriptionErreur
                    toutOk = False
                    If contnErreur = False Then
                        Exit For
                    End If
                End If
            End If
        Next

        For Each elementSuppr As TsCdConnxUserRole In suppr
            If elementSuppr.CibleAJour < TsECcCibleAJour.ExtractionAJour Then
                Dim descriptionErreur As String = accesIDM.EffacerLienUtilisateurRoleIDM(elementSuppr.CodeUtilisateur, elementSuppr.NomRole)
                If descriptionErreur = "" Then
                    elementSuppr.CibleAJour = TsECcCibleAJour.AJour
                Else
                    elementSuppr.CibleAJour = TsECcCibleAJour.PasAJour
                    elementSuppr.DescriptionErreur = descriptionErreur
                    toutOk = False
                    If contnErreur = False Then
                        Exit For
                    End If
                End If
            End If
        Next

        Return toutOk
    End Function

    ''' <summary>
    ''' Méthode du Connecteur IDM Phase 3.
    ''' Vérifie si les rôles à partir de la liste <paramref name="suppr"/> et de la liste <paramref name="ajouts"/>.
    ''' </summary>
    ''' <param name="suppr">La liste des rôles à détruire.</param>
    ''' <param name="ajouts">La liste des rôles à ajouter.</param>
    ''' <remarks></remarks>
    Public Overrides Sub VerifierRoles(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRole), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRole))
        For Each elementAjout As TsCdConnxRole In ajouts
            If accesIDM.PresentRoleIDM(elementAjout.NomRole) = True Then
                elementAjout.CibleAJour = TsECcCibleAJour.AJour
            Else
                elementAjout.CibleAJour = TsECcCibleAJour.PasAJour
            End If
        Next

        For Each elementSupp As TsCdConnxRole In suppr
            If accesIDM.AbsentRoleIDM(elementSupp.NomRole) = True Then
                elementSupp.CibleAJour = TsECcCibleAJour.AJour
            Else
                elementSupp.CibleAJour = TsECcCibleAJour.PasAJour
            End If
        Next
    End Sub

    ''' <summary>
    ''' Méthode du Connecteur IDM Phase 3.
    ''' Cette méthode va vérifier si les liens entre rôles et utilisateurs des listes de liens qui sont à jour dans ID Manager.
    ''' </summary>
    ''' <param name="ajouts">La liste des liens entre rôles et utilisateurs à ajouter.</param>
    ''' <param name="suppr">La liste des liens entre rôles et utilisaters à effacer.</param>
    ''' <remarks></remarks>
    Public Overrides Sub VerifierLiensUserRole(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRole), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxUserRole))
        For Each elementAjout As TsCdConnxUserRole In ajouts
            If accesIDM.PresentLienUtilisateurRoleIDM(elementAjout.CodeUtilisateur, elementAjout.NomRole) = True Then
                elementAjout.CibleAJour = TsECcCibleAJour.AJour
            Else
                elementAjout.CibleAJour = TsECcCibleAJour.PasAJour
            End If
        Next

        For Each elementSupp As TsCdConnxUserRole In suppr
            If accesIDM.AbsentLienUtilisateurRoleIDM(elementSupp.CodeUtilisateur, elementSupp.NomRole) = True Then
                elementSupp.CibleAJour = TsECcCibleAJour.AJour
            Else
                elementSupp.CibleAJour = TsECcCibleAJour.PasAJour
            End If
        Next
    End Sub
#End Region
End Class
