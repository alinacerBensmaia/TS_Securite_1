Imports System.Runtime.Caching
Imports System.Globalization
Imports System.Collections.Generic

' Inspirer de https://connect.microsoft.com/VisualStudio/feedback/details/723620/memorycache-class-needs-a-clear-method
Public Class TsCuChangeMonitor
    Inherits CacheEntryChangeMonitor

    Private uId As String
    Private Shared Event dememo As EventHandler(Of EventArgs)

    Public Sub New()
        Dim initializationComplete As Boolean = False
        Try
            uId = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)
            AddHandler dememo, AddressOf OnSignalRaised
            initializationComplete = True
        Finally
            MyBase.InitializationComplete()
            If Not initializationComplete Then
                Dispose(True)
            End If
            MyBase.InitializationComplete()
        End Try
    End Sub

    Public Overrides ReadOnly Property UniqueId() As String
        Get
            Return uId
        End Get
    End Property

    Public Overrides ReadOnly Property CacheKeys As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
        Get
            Return Nothing
        End Get
    End Property

    Public Overrides ReadOnly Property LastModified As System.DateTimeOffset
        Get
            Return DateTimeOffset.Now.AddHours(1)
        End Get
    End Property

    Public Overrides ReadOnly Property RegionName As String
        Get
            Return String.Empty
        End Get
    End Property

    Public Shared Sub DememoriseTout()
        RaiseEvent dememo(Nothing, New EventArgs)
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        RemoveHandler dememo, AddressOf OnSignalRaised
    End Sub

    Private Sub OnSignalRaised(ByVal sender As Object, ByVal e As EventArgs)
        ' Cache objects obligated to remove the entry upon change notification.
        MyBase.OnChanged(Nothing)
    End Sub
End Class
