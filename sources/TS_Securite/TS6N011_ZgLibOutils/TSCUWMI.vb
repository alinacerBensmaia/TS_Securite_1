Imports System.Management
''' --------------------------------------------------------------------------------
''' Project:	TS6N011_ZgLibOutils
''' Class:	TSCUWMI
''' <summary>
'''     Classe permettant la gestion du composant WMI
''' </summary>
''' <remarks><para><pre>
''' Historique des modifications: 
''' 
''' --------------------------------------------------------------------------------
''' Date		Nom			Description
''' 
''' --------------------------------------------------------------------------------
''' 2004-07-23	T206375		Création initiale
''' 
''' </pre></para>
''' </remarks>
''' --------------------------------------------------------------------------------
<Serializable()> Public Class TSCUWMI
    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	TSCUWMI.GetResultatRequete
    ''' <summary>
    '''     Fonction permettant de retourner une collection de type WMI à l'aide de la requete passé 
    '''     en paramètre.
    ''' </summary>
    ''' <param name="nomMachine">
    ''' 	Nom de la machine 
    ''' 	Value Type: <see cref="String" />	(System.String)
    ''' </param>
    ''' <param name="requete">
    ''' 	Requête en format sql 
    ''' 	Value Type: <see cref="String" />	(System.String)
    ''' </param>
    ''' <returns><see cref="Management.ManagementObjectCollection" />	(System.Management.ManagementObjectCollection)</returns>
    ''' <remarks><para><pre>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2004-07-22	T206375		Création initiale
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    Public Function GetResultatRequete(ByVal nomMachine As String, ByVal requete As String) As ManagementObjectCollection
        Dim scope As New ManagementScope("\\" & nomMachine & "\root\cimv2")
        Dim objRequete As New SelectQuery
        Dim searcher As ManagementObjectSearcher

        Try
            objRequete.QueryString = requete

            searcher = New ManagementObjectSearcher(scope, objRequete)

            ' Forward only
            searcher.Options.Rewindable = False

            GetResultatRequete = searcher.Get()

        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
