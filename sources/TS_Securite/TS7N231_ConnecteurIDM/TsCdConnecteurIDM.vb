
''' <summary>
''' Connecteur de base pour ID Manager.
''' </summary>
''' <remarks></remarks>
Public MustInherit Class TsCdConnecteurIDM
    Inherits TsCuConnecteurCibleIgnorable

#Region "Variables privées"
    Protected accesIDM As TsBaAccesIDM

    Protected _disposedValue As Boolean = False
#End Region

#Region "Constructeurs"

    ''' <summary>
    ''' Constructeur de base.
    ''' </summary>
    ''' <param name="idCible">Identifiant du connecteur.</param>
    ''' <remarks></remarks>
    Sub New(ByVal idCible As String)
        MyBase.New(idCible)

        accesIDM = New TsBaAccesIDM()
    End Sub

#End Region

#Region "Fonctions de services"

    ''' <summary>
    ''' Relache les ressources gérées.
    ''' </summary>
    ''' <param name="disposing">Indique si l'on doit disposé des valeurs managées par l'objet.</param>
    ''' <remarks></remarks>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If Not _disposedValue Then
            If disposing Then
                accesIDM.Dispose()
            End If
        End If
        _disposedValue = True
    End Sub

#End Region

End Class
