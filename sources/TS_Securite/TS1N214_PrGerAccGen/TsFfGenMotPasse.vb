Imports TS6N011_ZgLibOutils

''' --------------------------------------------------------------------------------
''' Project:	TS1N214_PrGerAccGen
''' Class:	TsFfGenMotPasse
''' <summary>
''' Fenêtre de génération de mot de passe.
''' </summary>
''' <remarks><para><pre>
''' Historique des modifications: 
''' 
''' --------------------------------------------------------------------------------
''' Date		Nom			        Description
''' 
''' --------------------------------------------------------------------------------
''' 2014-06-05	Simon Dallaire		Création initiale
''' 
''' </pre></para>
''' </remarks>
''' --------------------------------------------------------------------------------
Public Class TsFfGenMotPasse

#Region "Propriétés"

    Private m_MotDePasse As String
    ''' <summary>
    ''' Mot de passe généré ou chaine vide si l'opération a été annulée
    ''' </summary>
    Public ReadOnly Property MotDePasse As String
        Get
            Return m_MotDePasse
        End Get
    End Property

#End Region

#Region "Événements"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnGenerer_Click(sender As System.Object, e As System.EventArgs) Handles btnGenerer.Click

        Dim generateurMp As New TsCuMotDePasse()
        m_MotDePasse = generateurMp.GenererMotDePasse(Convert.ToInt32(numLongueur.Value), _
                                                      chkIncMinuscules.Checked, chkIncMajuscules.Checked, _
                                                      chkIncChiffres.Checked, chkIncSymboles.Checked)
        Close()

    End Sub

#End Region

#Region "Méthodes publiques"

    ''' <summary>
    ''' Affiche la fenêtre de génération de mot de passe
    ''' </summary>
    ''' <param name="owner">Fenêtre parent</param>
    ''' <returns>Mot de pass généré ou une chaine vide si l'opération a été annulée</returns>
    Public Shared Function AfficherGenererMotPasse(ByVal owner As IWin32Window, ByVal numLongueurMotPasse As Integer) As String

        Dim fenetre As New TsFfGenMotPasse()

        fenetre.numLongueur.Value = numLongueurMotPasse

        fenetre.ShowDialog(owner)

        Return fenetre.MotDePasse

    End Function

#End Region

End Class