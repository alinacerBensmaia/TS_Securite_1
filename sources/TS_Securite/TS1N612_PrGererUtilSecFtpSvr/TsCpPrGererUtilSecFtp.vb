Imports System.Runtime.InteropServices
Imports Rrq.CS.ServicesCommuns.ScenarioTransactionnel
Imports System.Windows.Forms

'''-----------------------------------------------------------------------------
''' Project		: TS1N612_PrGererUtilSecFtpSvr
''' Class		: TsCpPrGererUtilSecFtpSvr
'''
'''-----------------------------------------------------------------------------
''' <summary>
''' Classe de point d'entrée pour la fonction. 
''' </summary>
''' <remarks></remarks>
''' <history>
''' 	[T208914] 	2014-06-11	Création
''' </history>
'''-----------------------------------------------------------------------------
Public Class TsCpPrGererUtilSecFtp
    Inherits ClasseRRQ.XzCpFonctionBase
    ''-----------------------------------------------------------------------------
    '' <summary>
    '' Requise par le navigateur pour la classe d'entrée
    '' </summary>
    ''-----------------------------------------------------------------------------
    Implements XzIDestination


#Region "--- Implémentation de l'interface ""IDestination"" ---"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Details"></param>
    Private Sub Ouvrir(ByRef Details As Object) 'Implements XzIDestination.Ouvrir
        '   Déclaration de la fenêtre de document
        Dim FrmGererUtilSecFtpSvr As New TsFdGererUtilSecFtpSvr()

        '   Ouvrir la fenêtre de document
        FrmGererUtilSecFtpSvr.Show()
    End Sub

    ''' <summary>
    ''' Ouvre la fenêtre de document de façon modale avec retour d'information.
    ''' (Appelé par la méthode NaviguerAvecRetour du Navigateur)
    ''' </summary>
    ''' <param name="Methode"></param>
    ''' <param name="Details"></param>
    Public Sub Appeler(ByVal Methode As String, ByRef Details As Object) 'Implements XzIDestination.Appeler

        Dim CodeFonction As String = String.Empty
        Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Navigateur.NaviguerAvecRetour(CodeFonction, Methode, Details)

    End Sub

#End Region

    Private Sub XyIDestination_Ouvrir(ByRef Details As Object) Implements XzIDestination.Ouvrir
        'On n'utilise pas le contexte général, il faut l'effacer
        Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Navigateur.Identifiant = ""
        Ouvrir(Details)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Methode"></param>
    ''' <param name="Details"></param>
    Private Sub XyIDestination_Appeler(ByVal Methode As String, ByRef Details As Object) Implements XzIDestination.Appeler

        Try
            Select Case Methode
                Case Else
                    Throw New COMException("Mauvaise méthode", XzExcNavigateur.XzExcNaMauvaiseMethode)
            End Select
        Catch exValid As XZCuErrValdtException
#Disable Warning CP0034 ' Vous ne pouvez lever une exception de type 'XZCuErrValdtException' dans un composant d'intégration.
            Throw exValid
#Enable Warning CP0034 ' Vous ne pouvez lever une exception de type 'XZCuErrValdtException' dans un composant d'intégration.
        End Try
    End Sub
End Class
