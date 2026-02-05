Public NotInheritable Class TsCuConversionsTypes
    Private Sub New()
        'Non instanciable
    End Sub

    ''' <summary>
    ''' Obtenir la string sans blanc
    ''' </summary>
    ''' <param name="pValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function GetString(pValue As Object) As String
        Dim valeur As String = String.Empty

        If pValue IsNot Nothing Then
            valeur = pValue.ToString().Trim()
        End If

        Return valeur
    End Function
End Class
