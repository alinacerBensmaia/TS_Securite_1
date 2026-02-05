Imports System.IdentityModel.Metadata
Imports System.IdentityModel.Tokens
Imports System.Net
Imports System.Security.Cryptography.X509Certificates
Imports System.ServiceModel.Security
Imports System.Xml
Imports Microsoft.IdentityModel.Tokens

Friend Class TsCuValidCert

    ''' <summary>
    ''' Si le certificat est valide le code ne lance pas d'exception. Sinon une fin anormale sera provoquée
    ''' </summary>
    Friend Function ObtenirValidationCertificat() As TokenValidationParameters
        'ex : https://sts1.retraitequebec.gouv.qc.ca/adfs
        Dim Inf As Proprietes = New TsCuCommun().ObtenirInformationConfig()
        Dim AdresseCert As String = Inf.Authority.Replace("/adfs", "")
        Dim metadataAddress As String = String.Format("{0}/federationmetadata/2007-06/federationmetadata/2007-06/federationmetadata.xml", AdresseCert)
        ServicePointManager.CheckCertificateRevocationList = False
        Dim validationParameters = New TokenValidationParameters With {
            .IssuerSigningKey = New X509SecurityKey(New X509Certificate2(GetSigningCertificates(metadataAddress).FirstOrDefault().Certificate)),
            .ValidateIssuerSigningKey = True,
            .ValidateAudience = True,
            .ValidateIssuer = True,
            .ValidateLifetime = True
        }

        Return validationParameters
    End Function
    Private Function GetSigningCertificates(ByVal metadataAddress As String) As List(Of X509SecurityToken)
        Dim securityTokens As List(Of X509SecurityToken) = New List(Of X509SecurityToken)()
        If metadataAddress Is Nothing Then
            Throw New ArgumentNullException(metadataAddress)
        End If
        Using metadataReader As XmlReader = XmlReader.Create(metadataAddress)
            'On met none car notre certificat est self sign
            Dim serializer As MetadataSerializer = New MetadataSerializer() With {.CertificateValidationMode = X509CertificateValidationMode.None}
            Dim metadata As EntityDescriptor = TryCast(serializer.ReadMetadata(metadataReader), EntityDescriptor)
            If metadata IsNot Nothing Then
                Dim stsd As SecurityTokenServiceDescriptor = metadata.RoleDescriptors.OfType(Of SecurityTokenServiceDescriptor)().First()
                If stsd IsNot Nothing Then
                    Dim x509DataClauses As IEnumerable(Of X509RawDataKeyIdentifierClause) = stsd.Keys.Where(Function(key) key.KeyInfo IsNot Nothing _
                        AndAlso (key.Use = KeyType.Signing OrElse key.Use = KeyType.Unspecified)).[Select](Function(key) key.KeyInfo.OfType(Of X509RawDataKeyIdentifierClause)().First())
                    securityTokens.AddRange(x509DataClauses.[Select](Function(token) New X509SecurityToken(New X509Certificate2(token.GetX509RawData()))))
                Else
                    Throw New InvalidOperationException("Federation Metadata: RoleDescriptor introuvable")
                End If
            Else
                Throw New Exception("Invalide Federation Metadata")
            End If
        End Using
        Return securityTokens
    End Function
End Class
