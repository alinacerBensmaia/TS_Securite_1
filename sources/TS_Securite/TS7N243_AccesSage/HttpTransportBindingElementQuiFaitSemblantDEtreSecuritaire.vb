Imports System.ServiceModel.Channels

Friend Class HttpTransportBindingElementQuiFaitSemblantDEtreSecuritaire
    Inherits HttpTransportBindingElement
    Implements ISecurityCapabilities

    Public Overrides Function GetProperty(Of T As Class)(ByVal context As System.ServiceModel.Channels.BindingContext) As T
        If GetType(T) Is GetType(ISecurityCapabilities) Then
            Return DirectCast(DirectCast(Me, ISecurityCapabilities), T)
        End If

        Return MyBase.GetProperty(Of T)(context)
    End Function

    Public Overrides Function Clone() As System.ServiceModel.Channels.BindingElement
        Dim tpe = New HttpTransportBindingElementQuiFaitSemblantDEtreSecuritaire
        tpe.MaxReceivedMessageSize = Me.MaxReceivedMessageSize
        Return tpe
    End Function

    Public Overrides Function BuildChannelFactory(Of TChannel)(ByVal context As System.ServiceModel.Channels.BindingContext) As System.ServiceModel.Channels.IChannelFactory(Of TChannel)
        'Stop
        Return MyBase.BuildChannelFactory(Of TChannel)(context)
    End Function

    Public Overrides Function CanBuildChannelListener(Of TChannel As {Class, System.ServiceModel.Channels.IChannel})(ByVal context As System.ServiceModel.Channels.BindingContext) As Boolean
        'Stop
        Return MyBase.CanBuildChannelListener(Of TChannel)(context)
    End Function

    Public Overrides ReadOnly Property Scheme() As String
        Get
            'Return "http"
            Return MyBase.Scheme
        End Get
    End Property

    Public ReadOnly Property SupportedRequestProtectionLevel() As System.Net.Security.ProtectionLevel Implements System.ServiceModel.Channels.ISecurityCapabilities.SupportedRequestProtectionLevel
        Get
            Return Net.Security.ProtectionLevel.EncryptAndSign
        End Get
    End Property

    Public ReadOnly Property SupportedResponseProtectionLevel() As System.Net.Security.ProtectionLevel Implements System.ServiceModel.Channels.ISecurityCapabilities.SupportedResponseProtectionLevel
        Get
            Return Net.Security.ProtectionLevel.EncryptAndSign
        End Get
    End Property

    Public ReadOnly Property SupportsClientAuthentication() As Boolean Implements System.ServiceModel.Channels.ISecurityCapabilities.SupportsClientAuthentication
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property SupportsClientWindowsIdentity() As Boolean Implements System.ServiceModel.Channels.ISecurityCapabilities.SupportsClientWindowsIdentity
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property SupportsServerAuthentication() As Boolean Implements System.ServiceModel.Channels.ISecurityCapabilities.SupportsServerAuthentication
        Get
            Return True
        End Get
    End Property
End Class
