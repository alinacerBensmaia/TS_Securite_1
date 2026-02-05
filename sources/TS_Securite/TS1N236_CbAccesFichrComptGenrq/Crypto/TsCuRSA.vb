Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates
Imports System.Text
Imports Rrq.InfrastructureCommune.Parametres
Imports Rrq.Securite.Certificat

Public Class TsCuRSA

    Public Shared Function ObtenirCertificat() As X509Certificate2

        Dim Thumbprint As String = XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("TS1", "TS1\TS1N236\ThumbPrintCertificat")

        Dim x509Store As Security.Cryptography.X509Certificates.X509Store = New X509Store(StoreLocation.LocalMachine)
        x509Store.Open(OpenFlags.ReadOnly)
        Dim certx509Collection As X509Certificate2Collection = x509Store.Certificates

        For Each X509Cert As X509Certificate2 In certx509Collection
            If X509Cert.Thumbprint.Equals(Thumbprint.Replace(" ", String.Empty), StringComparison.CurrentCultureIgnoreCase) Then
                Return X509Cert
                Exit For
            End If
        Next

        'Si on arrive ici, cela veut dire que le certificat n'est pas installé sur la machine, on recupère le certificat qui est sur le réseau
        Dim NomFichierCertificat As String = XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("TS1", "TS1\TS1N236\NomFichierCertificat")
        Return TsCaCertificatSecurite.RecupererCertificatSecurite(NomFichierCertificat)

    End Function

    Public Shared Function Encrypt(ByVal x509 As X509Certificate2, ByVal ToEncrypt As String) As String

        If x509 Is Nothing Then
            Throw New ApplicationException("Le certificat n'a pas été fourni à la méthode de chiffrement")
        End If
        If String.IsNullOrEmpty(ToEncrypt) Then
            Throw New ApplicationException("Aucune information à chiffrer")
        End If

        Dim rsa As RSA = x509.GetRSAPublicKey()
        Dim bytestoEncrypt As Byte() = ASCIIEncoding.ASCII.GetBytes(ToEncrypt)
        Dim encryptedBytes As Byte() = rsa.Encrypt(bytestoEncrypt, RSAEncryptionPadding.OaepSHA256)
        Return System.Convert.ToBase64String(encryptedBytes)
    End Function

    Public Shared Function Decrypt(ByVal x509 As X509Certificate2, ByVal Todecrypt As String) As String

        If x509 Is Nothing Then
            Throw New ApplicationException("Le certificat n'a pas été fourni à la méthode de déchiffrement")
        End If
        If Todecrypt.Length <= 0 Then
            Throw New ApplicationException("Aucune information à déchiffrer")
        End If
        If Not (x509.HasPrivateKey) Then
            Throw New ApplicationException("Le certificat fourni n'a pas de clé privée")
        End If

        Dim rsa As RSA = x509.GetRSAPrivateKey()
        Dim plainbytes As Byte() = rsa.Decrypt(System.Convert.FromBase64String(Todecrypt), RSAEncryptionPadding.OaepSHA256)
        Dim enc As System.Text.ASCIIEncoding = New System.Text.ASCIIEncoding()
        Return enc.GetString(plainbytes)
    End Function

End Class
