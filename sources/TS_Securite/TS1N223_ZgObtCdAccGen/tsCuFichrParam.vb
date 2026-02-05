Imports System.IO
Imports System.Xml
Imports Cfg = Rrq.InfrastructureCommune.Parametres.XuCuConfiguration

''' --------------------------------------------------------------------------------
''' Project:	TS1N223_ZgObtCdAccGen
''' Class:	    Rrq.Securite.tsCuFichrParam
''' <summary>
'''     Elle nous permet d'accéder aux paramètres de configuration.
''' 
''' ATTENTION!!!    Cette classe n'est plus utilisée par ce composant.  Ce composant
'''                 obtient les paramètres de configuration directement de l'utilitaire
'''                 XU4N011_Configuration.
'''                 On conserve cette classe, car certains composants TS1 appellent 
'''                 ces méthodes pour obtenir les paramètres de configuration...
''' 
''' </summary>
''' <remarks>
''' Historique des modifications: 
''' 
''' --------------------------------------------------------------------------------
''' Date		Nom			Description
''' 
''' --------------------------------------------------------------------------------
''' 2007-11-26	T206500		Transférer les paramètres de configuration dans le fichier
'''                         "TS1.config".  Il ne reste plus que les paramètres
'''                         d'environnement dans le fichier "TS12020.xml".
'''                         On doit utiliser l'utilitaire XU4N011_Configuration
'''                         pour obtenir ces paramètres.  
''' 
''' </remarks>
''' --------------------------------------------------------------------------------

<System.ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never)>
Public Class tsCuFichrParam
    Private _document As Xml.XmlDocument
    Private _derniereModif As DateTime

    Public Enum TypeParam
        InformationsDom = 0
        InformationsZone = 1
        Chemins = 2
        EnvironnementsNom = 3
        EnvironnementsVal = 4
        EnvironnementsID = 5
    End Enum

#Region "*-----  Méthodes  publiques  -----*"


    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	Rrq.Securite.tsCuFichrParam.LireParam
    ''' <summary>
    '''     Obtenir la valeur des paramètres de configuration
    ''' </summary>
    ''' <param name="strNomParam">
    ''' 	Nom du paramètre que l'on désire obtenir.
    ''' 	Value Type: string
    ''' </param>
    ''' <param name="typParm">
    ''' 	Type du paramètre. On y retrouve six types de paramètres regroupés sous
    '''     la variable TypeParam.
    '''     Ce type n'est plus utile depuis que les paramètres sont transférés dans le
    '''     fichier "TS1.Config".  Par contre, nous devons conserver ce paramètre pour 
    '''     éviter de modifier tous les composants qui utilisent cette méthode.
    ''' 	Value Type: TypeParam
    ''' </param>
    ''' <returns>
    '''     Valeur du paramètre demandé
    ''' </returns>    
    '''"Obsolete("Les paramètres sont maintenant dans le fichier de configuration, veuillez utiliser le composant de configuration.", True)> _
    ''' --------------------------------------------------------------------------------
    Public Function LireParam(ByVal strNomParam As String, ByVal typParm As TypeParam) As String
        ' 2007-11-13   T206500 (Manon Jalbert)
        ' le fichier param contient que les informations concernant l'environnement
        ' les autres informations sont maintenant inscrites dans le fichier TS1.config
        If estExclusDeTs1Config(typParm) Then
            Return lireFichierDeParametres(strNomParam, typParm)
        Else
            Return lireFichierDeConfig(strNomParam, typParm)
        End If
    End Function

    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	Rrq.Securite.tsCuFichrParam.LireNombreEnvrn
    ''' <summary>
    '''     Obtenir le nombre d'environnement traité par l'interface graphique TS1
    ''' </summary>
    ''' <returns>
    '''     Le nombre d'environnement inscrit
    '''"Obsolete("Les paramètres sont maintenant dans le fichier de configuration, veuillez utiliser le composant de configuration.", True)> _
    ''' </returns>    
    ''' --------------------------------------------------------------------------------
    Public Function LireNombreEnvrn() As String
        Dim fichierParametre As String = Cfg.ValeurSysteme("TS1", "TS1\TS1N223\FichierParam")
        assurerChargementFichier(fichierParametre)

        Dim node As XmlNode = _document.DocumentElement.SelectSingleNode("./environnements")
        Return node.Attributes("nombre").Value()
    End Function

#End Region

#Region "*-----   Méthodes  privées   -----*"

    Private Sub assurerChargementFichier(ByVal cheminFichier As String)
        If Not _document Is Nothing Then
            'si le fichier a été modifié depuis le dernier chargement
            If _derniereModif <> File.GetLastWriteTime(cheminFichier) Then
                _document = Nothing
            End If
        End If

        If _document Is Nothing Then
            chargerFichier(cheminFichier)
        End If
    End Sub

    Private Sub chargerFichier(ByVal cheminFichier As String)
        Const MAX_ESSAIS As Integer = 10
        Dim compteurEssais As Integer = 0
        Dim exceptionOriginal As IOException = Nothing

        _document = New XmlDocument
        Do While compteurEssais < MAX_ESSAIS
            Try
                _document.Load(cheminFichier)
                _derniereModif = File.GetLastWriteTime(cheminFichier)
                Return

            Catch ex As IOException
                exceptionOriginal = ex
                Threading.Thread.Sleep(100)
                compteurEssais += 1
            End Try
        Loop

        'si nous sommes ici, la lecture n'a pas fonctionnée
        _document = Nothing
        Throw New TsCuLectureXMLImpossible(exceptionOriginal)
    End Sub

    Private Function estExclusDeTs1Config(typParm As TypeParam) As Boolean
        Return (typParm = TypeParam.EnvironnementsID Or typParm = TypeParam.EnvironnementsNom Or typParm = TypeParam.EnvironnementsVal)
    End Function

    Private Function lireFichierDeParametres(ByVal nomParametre As String, ByVal typeParametre As TypeParam) As String
        Dim fichierParametre As String = Cfg.ValeurSysteme("TS1", "TS1\TS1N223\FichierParam")
        assurerChargementFichier(fichierParametre)

        Dim xPath As String = "./environnements/environnement[@{0}=""{1}""]"
        Dim filter As String = String.Empty
        Dim valueIn As String = String.Empty

        Select Case typeParametre
            Case TypeParam.EnvironnementsNom
                filter = "nom"
                valueIn = "valeur"

            Case TypeParam.EnvironnementsVal
                filter = "valeur"
                valueIn = "nom"

            Case TypeParam.EnvironnementsID
                filter = "id"
                valueIn = "valeur"

            Case Else
                xPath = String.Empty
        End Select

        If Not String.IsNullOrEmpty(xPath) Then
            Dim node As XmlNode = _document.DocumentElement.SelectSingleNode(xPath)
            Return node.Attributes(valueIn).Value()
        Else
            Return String.Empty
        End If
    End Function

    Private Function lireFichierDeConfig(ByVal nomParametre As String, ByVal typeParametre As TypeParam) As String
        Select Case typeParametre
            Case TypeParam.InformationsDom
                If nomParametre.ToLower.Equals("domaine") Then
                    Return Cfg.ValeurSysteme("TS1", "TS1\TS1N223\NomDomaine")
                End If
                If nomParametre.ToLower.Equals("zone") Then
                    Return Cfg.ValeurSysteme("TS1", "TS1\TS1N223\NomZone")
                End If

            Case TypeParam.InformationsZone
                Return Cfg.ValeurSysteme("TS1", "TS1\TS1N223\Zone")

            Case TypeParam.Chemins
                Return Cfg.ValeurSysteme("TS1", "TS1\TS1N223\" & nomParametre)

        End Select

        Return String.Empty
    End Function

#End Region

End Class
