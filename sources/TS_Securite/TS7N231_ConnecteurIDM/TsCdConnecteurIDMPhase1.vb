''' <summary>
''' Connecteur pour ID Manager phase 1.
''' </summary>
''' <remarks></remarks>
Public Class TsCdConnecteurIDMPhase1
    Inherits TsCdConnecteurIDM

#Region "Propriétées"
    ''' <summary>
    ''' Description du système cible compréhensible pour l'utilisateur.
    ''' </summary>
    Public Overrides ReadOnly Property DescrCible() As String
        Get
            Return "ID Manager phase 1"
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
    ''' Méthode du Connecteur IDM Phase1. 
    ''' Cette fonction va appliquer les ajouts des rôles de la liste <paramref name="ajouts"/> dans l'ID manager.
    ''' </summary>
    ''' <param name="ajouts">Liste de rôles à ajouter.</param>
    ''' <param name="contnErreur">
    ''' True = Si une erreur est rencontrée, l'erreur est prise en compte, mais le traitement continue.
    ''' False = Si une erreur est rencontrée, l'erreur est prise en compte et le traitement s'arrête immédiatement.
    ''' </param>
    Public Overrides Function CreerRoles(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRole), Optional ByVal contnErreur As Boolean = False) As Boolean
        Dim toutOk As Boolean = True

        For Each elementAjout As TsCdConnxRole In ajouts
            If elementAjout.CibleAJour < TsECcCibleAJour.ExtractionAJour Then
                Dim descriptionErreur As String = accesIDM.AjouterRoleIDM(elementAjout.NomRole)
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

        Return toutOk
    End Function

    ''' <summary>
    ''' Méthode du Connecteur IDM Phase1. Cette fonction va appliquer les ajouts des liens Rôle/Rôle de la liste <paramref name="ajouts"/> dans l'ID manager
    ''' et va appliquer les suppressions des liens Rôle/Rôle dans l'ID manager à partir de la liste <paramref name="suppr"/>.
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
            If elementAjout.CibleAJour < TsECcCibleAJour.ExtractionAJour Then
                Dim descriptionErreur As String = accesIDM.AjouerLienRoleRoleIDM(elementAjout.NomRoleSup, elementAjout.NomSousRole)
                If descriptionErreur = "" Then
                    elementAjout.CibleAJour = TsECcCibleAJour.AJour
                    elementAjout.DescriptionErreur = descriptionErreur
                Else
                    elementAjout.CibleAJour = TsECcCibleAJour.PasAJour
                    toutOk = False
                    If contnErreur = False Then
                        Exit For
                    End If
                End If
            End If
        Next

        For Each elementSuppr As TsCdConnxRoleRole In suppr
            If elementSuppr.CibleAJour < TsECcCibleAJour.ExtractionAJour Then
                Dim descriptionErreur As String = accesIDM.EffacerLienRoleRoleIDM(elementSuppr.NomRoleSup, elementSuppr.NomSousRole)
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
    ''' Méthode du Connecteur IDM Phase 1.
    ''' Cette méthode va vérifier si les rôles des listes de rôles qui sont à jour dans ID Manager.
    ''' </summary>
    ''' <param name="ajouts">La liste des rôles à ajouter.</param>
    ''' <param name="suppr">La liste des rôles à effacer.</param>
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
    ''' Méthode du Connecteur IDM Phase 1.
    ''' Cette méthode va vérifier si les liens des listes de liens entre rôles qui sont à jour dans ID Manager.
    ''' </summary>
    ''' <param name="ajouts">La liste des liens entre rôles à ajouter.</param>
    ''' <param name="suppr">La liste des liens entre rôles à effacer.</param>
    ''' <remarks></remarks>
    Public Overrides Sub VerifierLiensRoleRole(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRoleRole), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRoleRole))
        For Each elementAjout As TsCdConnxRoleRole In ajouts
            If accesIDM.PresentLienRoleRoleIDM(elementAjout.NomRoleSup, elementAjout.NomSousRole) = True Then
                elementAjout.CibleAJour = TsECcCibleAJour.AJour
            Else
                elementAjout.CibleAJour = TsECcCibleAJour.PasAJour
            End If
        Next

        For Each elementSupp As TsCdConnxRoleRole In suppr
            If accesIDM.AbsentLienRoleRoleIDM(elementSupp.NomRoleSup, elementSupp.NomSousRole) = True Then
                elementSupp.CibleAJour = TsECcCibleAJour.AJour
            Else
                elementSupp.CibleAJour = TsECcCibleAJour.PasAJour
            End If
        Next
    End Sub

#End Region

End Class
