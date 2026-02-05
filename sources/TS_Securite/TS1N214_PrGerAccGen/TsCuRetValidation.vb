Imports System.Collections.Generic

''' <summary>
''' Classe utilitaire pour le retour des validations
''' </summary>
''' <remarks></remarks>
Public Class TsCuRetValidation
    Private mControlInvalide As Object
    Private mListeErreur As List(Of String) = New List(Of String)

    Friend ReadOnly Property ListeErreur() As List(Of String)
        Get
            Return mListeErreur
        End Get
    End Property

    Friend Property ControlInvalide() As Object
        Get
            Return mControlInvalide
        End Get
        Set(ByVal value As Object)
            mControlInvalide = value
        End Set
    End Property

End Class
