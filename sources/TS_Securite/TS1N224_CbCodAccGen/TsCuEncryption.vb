Imports System.Security.Cryptography
Imports System.Text

''' <summary>
''' Classes de méthodes statique pour manipuler des fichiers de clés symbolique
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class TsCuEncryption
    Private Sub New()
        'Classe non instanciable de méthodes statique (pour la performance)
    End Sub

    ''' <summary>
    ''' Decrypter
    ''' </summary>
    ''' <param name="cryptText">Texte encrypté</param>
    ''' <param name="KeyComponent"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function Decrypt(ByVal cryptText As String, ByVal KeyComponent As String) As String
        Dim result As String = String.Empty
        Dim K_Fichr(31) As Byte
        Dim V_Fichr(15) As Byte

        TsCuConversions.ConvertirStringByte(K_Fichr, V_Fichr, KeyComponent)

        Using objRijndael As New RijndaelManaged
            Dim objChiff As ICryptoTransform = objRijndael.CreateDecryptor(K_Fichr, V_Fichr)

            Dim currentPosition As Integer = 0
            Dim sourceBytes() As Byte = System.Convert.FromBase64String(cryptText)
            Dim sourceByteLength As Integer = sourceBytes.Length
            Dim inputBlockSize As Integer = objChiff.InputBlockSize
            'Dim outputBlockSize As Integer = objChiff.OutputBlockSize

            Dim targetBytes(1023) As Byte

            Dim numBytesRead As Integer = 0
            While sourceByteLength - currentPosition > inputBlockSize
                numBytesRead = objChiff.TransformBlock(sourceBytes, currentPosition, inputBlockSize, targetBytes, currentPosition)
                currentPosition += numBytesRead
            End While
            Dim finalBytes As Byte() = objChiff.TransformFinalBlock(sourceBytes, currentPosition, sourceByteLength - currentPosition)
            finalBytes.CopyTo(targetBytes, currentPosition)

            Dim finalTargetBytes() As Byte = targetBytes.TrimArray(True)
            If finalTargetBytes.Length > 16 Then
                Dim ffTargetBytes(finalTargetBytes.Length - 17) As Byte
                Array.Copy(finalTargetBytes, ffTargetBytes, 16)
                Array.Copy(finalTargetBytes, 32, ffTargetBytes, 16, finalTargetBytes.Length - 32)
                finalTargetBytes = ffTargetBytes
            End If
            result = Encoding.ASCII.GetString(finalTargetBytes)
        End Using

        Return result
    End Function

    ''' <summary>
    ''' Encrypter
    ''' </summary>
    ''' <param name="clearText">Texte non encrypté</param>
    ''' <param name="KeyComponent"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function Encrypt(ByVal clearText As String, ByVal KeyComponent As String) As String
        Dim result As String = ""
        Dim K_Fichr(31) As Byte
        Dim V_Fichr(15) As Byte

        TsCuConversions.ConvertirStringByte(K_Fichr, V_Fichr, KeyComponent)

        Using objRijndael As New RijndaelManaged
            Dim objChiff As ICryptoTransform = objRijndael.CreateEncryptor(K_Fichr, V_Fichr)

            Dim currentPosition As Integer = 0
            Dim sourceBytes() As Byte = Encoding.ASCII.GetBytes(clearText)
            Dim sourceByteLength As Integer = sourceBytes.Length
            Dim inputBlockSize As Integer = objChiff.InputBlockSize
            'Dim outputBlockSize As Integer = objChiff.OutputBlockSize

            Dim targetBytes(1023) As Byte

            Dim numBytesRead As Integer = 0
            While sourceByteLength - currentPosition > inputBlockSize
                numBytesRead = objChiff.TransformBlock(sourceBytes, currentPosition, inputBlockSize, targetBytes, currentPosition)
                currentPosition += numBytesRead
            End While
            Dim finalBytes As Byte() = objChiff.TransformFinalBlock(sourceBytes, currentPosition, sourceByteLength - currentPosition)
            finalBytes.CopyTo(targetBytes, currentPosition)

            result = Convert.ToBase64String(targetBytes.TrimArray(False))
        End Using

        Return result
    End Function
End Class


