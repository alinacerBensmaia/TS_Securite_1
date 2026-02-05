''' <summary>
''' Connecteur pour ID manager phase Top Secret.
''' </summary>
''' <remarks></remarks>
Public Class TsCdConnecteurIDMTSS
    Inherits TsCdConnecteurIDM

#Region "Propriétées"
    ''' <summary>
    ''' Description du système cible compréhensible pour l'utilisateur.
    ''' </summary>
    Public Overrides ReadOnly Property DescrCible() As String
        Get
            Return "ID Manager pour Top Secret"
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
    ''' Méthode du Connecteur IDM Phase TSS. 
    ''' Cette fonction va appliquer les ajouts des rôles de la liste <paramref name="ajouts"/> dans l'ID Manager pour TSS.
    ''' </summary>
    ''' <param name="ajouts">Liste de rôles à ajouter.</param>
    ''' <param name="contnErreur">
    ''' True = Si une erreur est rencontrée, l'erreur est prise en compte, mais le traitement continue.
    ''' False = Si une erreur est rencontrée, l'erreur est prise en compte et le traitement s'arrête immédiatement.
    ''' </param>
    Public Overrides Function AppliquerLiensRoleRessr(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRoleRessr), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRoleRessr), Optional ByVal contnErreur As Boolean = False) As Boolean
        Dim toutOk As Boolean = True

        For Each elementAjout As TsCdConnxRoleRessr In ajouts
            If elementAjout.CibleAJour < TsECcCibleAJour.ExtractionAJour Then
                Dim descriptionErreur As String = accesIDM.AjouterLienRoleRessourceIDMTSS(elementAjout.NomRole, elementAjout.NomRessource)
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

        For Each elementSuppr As TsCdConnxRoleRessr In suppr
            If elementSuppr.CibleAJour < TsECcCibleAJour.ExtractionAJour Then
                Dim descriptionErreur As String = accesIDM.EffacerLienRoleRessourceIDMTSS(elementSuppr.NomRole, elementSuppr.NomRessource)
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
    ''' Méthode du Connecteur IDM Phase TSS. 
    ''' Vérifie si les liens rôle ressource sont à jour ou non.
    ''' </summary>
    ''' <param name="ajouts">Liste des liens à ajouter.</param>
    ''' <param name="suppr">Liste des liens à supprimer.</param>
    ''' <remarks></remarks>
    Public Overrides Sub VerifierLiensRoleRessr(ByVal ajouts As System.Collections.Generic.IEnumerable(Of TsCdConnxRoleRessr), ByVal suppr As System.Collections.Generic.IEnumerable(Of TsCdConnxRoleRessr))
        For Each elementAjout As TsCdConnxRoleRessr In ajouts
            If accesIDM.PresentLienRoleRessourceTSS(elementAjout.NomRole, elementAjout.NomRessource) = True Then
                elementAjout.CibleAJour = TsECcCibleAJour.AJour
            Else
                elementAjout.CibleAJour = TsECcCibleAJour.PasAJour
            End If
        Next

        For Each elementSuppr As TsCdConnxRoleRessr In suppr
            If accesIDM.AbsentLienRoleRessourceTSS(elementSuppr.NomRole, elementSuppr.NomRessource) = True Then
                elementSuppr.CibleAJour = TsECcCibleAJour.AJour
            Else
                elementSuppr.CibleAJour = TsECcCibleAJour.PasAJour
            End If
        Next
    End Sub
#End Region
End Class
