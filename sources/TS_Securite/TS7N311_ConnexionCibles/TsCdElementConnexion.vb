''' <summary>
''' Représente un élément pour les connecteurs.
''' </summary>
Public Class TsCdElementConnexion
    Private _CibleAJour As TsECcCibleAJour
    Private _AncCibleAJour As TsECcCibleAJour

    ''' <summary>
    ''' Indique à quel point la cible est à jour pour cet élément.
    ''' </summary>
    Public Property CibleAJour() As TsECcCibleAJour
        Get
            Return Me._CibleAJour
        End Get
        Set(ByVal value As TsECcCibleAJour)
            Me._AncCibleAJour = Me._CibleAJour
            Me._CibleAJour = value
        End Set
    End Property


    Public ReadOnly Property ModificationEffectuee() As Boolean
        Get
            If Me._CibleAJour <> TsECcCibleAJour.Inconnu AndAlso Me._AncCibleAJour <> TsECcCibleAJour.Inconnu AndAlso _
                Me._AncCibleAJour < TsECcCibleAJour.ExtractionAJour AndAlso Me._CibleAJour >= TsECcCibleAJour.ExtractionAJour Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    ''' <summary>
    ''' Si une erreur survient pour cette élément inscrire les détail de l'erreur dans cette variable.
    ''' </summary>
    ''' <remarks>Si le champ est vide, nous considerons qu'il n'y a pas d'erreur.</remarks>
    Public DescriptionErreur As String = Nothing

End Class
