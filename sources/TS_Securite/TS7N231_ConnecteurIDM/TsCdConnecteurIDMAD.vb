''' <summary>
''' Connecteur pour ID manager phase Active Directory.
''' </summary>
''' <remarks></remarks>
Public Class TsCdConnecteurIDMAD
    Inherits TsCdConnecteurIDM

#Region "Propriétées"
    ''' <summary>
    ''' Description du système cible compréhensible pour l'utilisateur.
    ''' </summary>
    Public Overrides ReadOnly Property DescrCible() As String
        Get
            Return "ID Manager pour Active Directory"
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
    ''' Méthode du Connecteur IDM Phase AD. 
    ''' Cette fonction va appliquer les ajouts des rôles de la liste <paramref name="ajouts"/> dans l'ID Manager pour l'AD.
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
                Dim descriptionErreur As String = accesIDM.AjouterRoleIDMAD(elementAjout.NomRole)
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
    ''' Méthode du Connecteur IDM Phase AD. 
    ''' Cette fonction va appliquer les suppressions des rôles dans l'ID Manager pour l'AD à partir de la liste <paramref name="suppr"/>.
    ''' </summary>
    ''' <param name="suppr">Liste de rôles à supprimer.</param>
    ''' <param name="contnErreur">
    ''' True = Si une erreur est rencontrée, l'erreur est prise en compte, mais le traitement continue.
    ''' False = Si une erreur est rencontrée, l'erreur est prise en compte et le traitement s'arrête immédiatement.
    ''' </param>
    Public Overrides Function DetruireRoles(ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRole), Optional ByVal contnErreur As Boolean = False) As Boolean
        Dim toutOk As Boolean = True

        For Each elementSuppr As TsCdConnxRole In suppr
            If elementSuppr.CibleAJour < TsECcCibleAJour.ExtractionAJour Then
                Dim descriptionErreur As String = accesIDM.EffacerRoleIDMAD(elementSuppr.NomRole)
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
    ''' Méthode du Connecteur IDM Phase AD.
    ''' Cette méthode va vérifier si les rôles des listes de rôles sont à jour dans ID Manager pour Active Directory.
    ''' </summary>
    ''' <param name="ajouts">La liste des rôles à ajouter.</param>
    ''' <param name="suppr">La liste des rôles à effacer.</param>
    ''' <remarks></remarks>
    Public Overrides Sub VerifierRoles(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRole), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRole))
        For Each elementAjout As TsCdConnxRole In ajouts
            If accesIDM.PresentRoleIDMAD(elementAjout.NomRole) = True Then
                elementAjout.CibleAJour = TsECcCibleAJour.AJour
            Else
                elementAjout.CibleAJour = TsECcCibleAJour.PasAJour
            End If
        Next

        For Each elementSupp As TsCdConnxRole In suppr
            If accesIDM.AbsentRoleIDMAD(elementSupp.NomRole) = True Then
                elementSupp.CibleAJour = TsECcCibleAJour.AJour
            Else
                elementSupp.CibleAJour = TsECcCibleAJour.PasAJour
            End If
        Next
    End Sub

#End Region

End Class
