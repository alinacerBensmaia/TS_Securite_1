Public NotInheritable Class TsCuConversions

#Region "--- Constructeur ---"
    Private Sub New()
        'Classe de méthodes statique non instanciable
    End Sub
#End Region


    ''' Class.Method:	Securite.tsCuInfoUtil.ConvertirStringByte
    ''' <summary>
    ''' Transformer la clé et le vecteur qui sont conservés en string en un array de byte
    ''' </summary>
    ''' <param name="K_Fichr">Clé d'encryption en array de byte.</param>
    ''' <param name="V_Fichr">Vecteur d'encryption en array de byte.</param>
    ''' <remarks>
    ''' Historique des modifications: 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' --------------------------------------------------------------------------------
    ''' 2008-02-21	T206500		Création initiale
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    Friend Shared Sub ConvertirStringByte(ByRef K_Fichr() As Byte, ByRef V_Fichr() As Byte)
        Dim strK() As String = TsCuVarShared.strK_Fichr.Split(",".ToCharArray)
        For i As Integer = 0 To 31
            K_Fichr(i) = Convert.ToByte(strK(i))
        Next

        Dim strV() As String = TsCuVarShared.strV_Fichr.Split(",".ToCharArray)
        For i As Integer = 0 To 15
            V_Fichr(i) = Convert.ToByte(strV(i))
        Next
    End Sub

    Friend Shared Sub ConvertirStringByte(ByRef K_Fichr() As Byte, ByRef V_Fichr() As Byte, ByVal keyComponent As String)
        Dim strK() As String = TsCuVarShared.strK_Fichr.Split(","c)
        For i As Integer = 0 To 31
            K_Fichr(i) = Convert.ToByte(strK(i))
        Next
        For i As Integer = 0 To keyComponent.Length - 1
            K_Fichr(32 - keyComponent.Length + i) = Convert.ToByte(keyComponent.Substring(i, 1).ToCharArray()(0))
        Next

        Dim strV() As String = TsCuVarShared.strV_Fichr.Split(","c)
        For i As Integer = 0 To 15
            V_Fichr(i) = Convert.ToByte(strV(i))
        Next
    End Sub
End Class
