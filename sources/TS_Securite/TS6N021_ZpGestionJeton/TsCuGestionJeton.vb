Imports System.EnterpriseServices
Imports System.IO
Imports System.Text
Imports System.Threading
Imports System.Data
Imports System.Runtime.InteropServices
Imports Cfg = Rrq.InfrastructureCommune.Parametres.XuCuConfiguration
Imports Rrq.InfrastructureCommune.UtilitairesCommuns

'Définition de l'interface du composant
Public Interface ITsCuGestionJeton
    Function ValiderJeton(ByVal strUsager As String, ByVal strComposant As String, _
        ByVal strSessionID As String) As Boolean
    Function ObtenirJeton(ByVal strUsager As String, ByVal strComposant As String) As String
    Function ObtenirDepot(ByRef dsDepot As DataSet, ByVal strPropGroup As String, _
        ByVal strProp As String) As Boolean
End Interface

''' -----------------------------------------------------------------------------
''' Project	 : TS6N021_ZpGestionJeton
''' Class	 : TsCuGestionJeton
''' 
''' -----------------------------------------------------------------------------
''' <summary>
''' Cette classe permet d'obtenir et de valider un jeton de sécurité. Les jetons
''' générés lors de l'appel à la méthode ObtenirJeton sont concervés dans un dépot
''' en mémoire.
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[T208320]	2006-10-13	Created
''' </history>
''' -----------------------------------------------------------------------------
Public NotInheritable Class TsCuGestionJeton
    Inherits ServicedComponent
    Implements ITsCuGestionJeton

#Region "Variables privées"
    Private m_dsDepot As DataSet
#End Region

#Region "Fonctions et procédures publiques"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Cette fonction génère un jeton, l'enregistre dans le dépot en mémoire et 
    ''' retour la clé à l'appelant. La clé est une clé cryptographique qui sert à 
    ''' identifié l'appel à un composant faite par un usager précis.
    ''' </summary>
    ''' <param name="strUsager">Nom de l'usager authentifié sur le serveur local
    ''' </param>
    ''' <param name="strComposant">Nom du composant .NET que l'usager souhaite 
    ''' accéder sur le serveur distant</param>
    ''' <returns>Clé cryptographique</returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[T208320]	2006-10-13	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Function ObtenirJeton(ByVal strUsager As String, ByVal strComposant As String) As String Implements ITsCuGestionJeton.ObtenirJeton
        Try
            Dim strSessionID As String = GenererSessionID()
            Dim bPropExiste As Boolean
            Dim objSPGM As New SharedPropertyGroupManager
            Dim objSPG As SharedPropertyGroup = objSPGM.CreatePropertyGroup(C_SHRD_PROP_GROUP, _
                PropertyLockMode.SetGet, PropertyReleaseMode.Process, False)
            Dim objSP As SharedProperty = objSPG.CreateProperty(C_SHRD_PROP, bPropExiste)
            Dim drNouv As DataRow
            Dim dExpirJetonSec As Double = Convert.ToDouble(Cfg.ValeurSysteme("TS6", "TS6\TS6N021\ExpirJetonSec"))

            SyncLock TsCuMenageJeton.Instance
                'aller chercher la liste des jetons en mémoire contenu dans un shared property de COM+
                ObtenirCacheDepot(m_dsDepot, objSP)

                'créer un nouveau jeton dans le dataset obtenue du dépot
                drNouv = m_dsDepot.Tables(C_TABLE_NAME).NewRow
                drNouv.Item(C_DEPOT_CLE) = strSessionID
                drNouv.Item(C_DEPOT_USAGER) = strUsager
                drNouv.Item(C_DEPOT_COMPOSANT) = strComposant
                drNouv.Item(C_DEPOT_EXPIRATION) = DateTime.Now.AddSeconds(dExpirJetonSec)
                drNouv.Item(C_DEPOT_ACTIF) = True

                'ajouter le jeton
                m_dsDepot.Tables(C_TABLE_NAME).Rows.Add(drNouv)

                'inscrire le dépot dans le shared property de COM+
                EcrireCacheDepot(m_dsDepot, objSP)
            End SyncLock

            'retourner la clé de session à l'appelant
            Return strSessionID
        Catch ex As Exception
            'relancer l'exception
            Throw New Exception(ex.Message, ex)
        End Try
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Cette fonction vérifie dans le dépot en mémoire si le jeton correspondant à
    ''' la clé fournie en paramètre a expiré. Si le jeton n'est pas expiré il
    ''' est marqué comme inactif dans le dépot.
    ''' </summary>
    ''' <param name="strUsager">Nom de l'usager authentifié sur le serveur local
    ''' </param>
    ''' <param name="strComposant">Nom du composant .NET que l'usager souhaite 
    ''' accéder sur le serveur distant</param>
    ''' <param name="strSessionID">Clé cryptographique identifiant le jeton</param>
    ''' <returns>True: validation ok, False: validation pas ok</returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[T208320]	2006-10-13	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Function ValiderJeton(ByVal strUsager As String, ByVal strComposant As String, ByVal strSessionID As String) As Boolean Implements ITsCuGestionJeton.ValiderJeton
        Try
            Dim dtEntreeValiderJeton As DateTime = Now
            Dim dtExpiration As DateTime
            Dim bActif As Boolean
            Dim bRetour As Boolean
            Dim bPropExiste As Boolean
            Dim objSPGM As New SharedPropertyGroupManager
            Dim objSPG As SharedPropertyGroup = objSPGM.CreatePropertyGroup(C_SHRD_PROP_GROUP, _
                PropertyLockMode.SetGet, PropertyReleaseMode.Process, False)
            Dim objSP As SharedProperty = objSPG.CreateProperty(C_SHRD_PROP, bPropExiste)
            Dim drSelect() As DataRow

            SyncLock TsCuMenageJeton.Instance
                'aller chercher la liste des jetons en mémoire contenu dans un shared property de COM+
                ObtenirCacheDepot(m_dsDepot, objSP)

                'sélectionner le jeton correspondant à la clé de session, l'usager et le composant passés en paramètre
                drSelect = m_dsDepot.Tables(C_TABLE_NAME).Select("Cle = '" + strSessionID + _
                    "' and Usager = '" + strUsager + _
                    "' and Composant = '" + strComposant + "'")

                If drSelect.Length = 1 Then
                    'le jeton a été trouvé. valider si celui-ci est expiré
                    dtExpiration = drSelect(0).Item("Expiration")
                    bActif = drSelect(0).Item("Actif")

                    bRetour = (dtEntreeValiderJeton.ToString() <= dtExpiration.ToString()) And bActif

                    If Not bRetour Then
                        XuCuGestionEvent.AjouterEvenmSpecifique(XuGeJournalEvenement.XuGeJeSecuriteRRQ, XuGeTypeEvenement.XuGeTeInformation, 2, "Date/heure:" + DateTime.Now.ToString() + ", Date d'expir.: " + dtExpiration.ToString() + ", Actif: " + bActif.ToString() + ", Usager: " + strUsager + ", Composant: " + strComposant + ", Session ID: " + strSessionID)
                    End If

                    'désactiver le jeton en mémoire
                    drSelect(0).Item("Actif") = False

                    'inscrire le dépot dans le shared property de COM+
                    EcrireCacheDepot(m_dsDepot, objSP)
                Else
                    bRetour = False
                    XuCuGestionEvent.AjouterEvenmSpecifique(XuGeJournalEvenement.XuGeJeSecuriteRRQ, XuGeTypeEvenement.XuGeTeInformation, 2, "Usager: " + strUsager + ", Composant: " + strComposant + ", Session ID: " + strSessionID + ", Desc: Jeton trouvé " + drSelect.Length.ToString() + " fois dans le dépot")
                End If
            End SyncLock

            Return bRetour
        Catch ex As Exception
            'relancer l'exception
            Throw New Exception(ex.Message, ex)
        End Try
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Cette fonction lit le dépot en mémoire et le retourne à l'appelant sous forme
    ''' de dataset.
    ''' </summary>
    ''' <param name="dsDepot">Dataset qui recevra le contenu du dépot</param>
    ''' <param name="strPropGroup">Nom du SharedPropertyGroup de COM+</param>
    ''' <param name="strProp">Nom du SharedProperty de COM+ qui contient le dépot
    ''' </param>
    ''' <returns>True: traitement ok, False: traitement pas ok</returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[T208320]	2006-10-13	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Function ObtenirDepot(ByRef dsDepot As DataSet, ByVal strPropGroup As String, ByVal strProp As String) As Boolean Implements ITsCuGestionJeton.ObtenirDepot
        Try
            Return ObtenirCacheDepot(dsDepot, strPropGroup, strProp)
        Catch ex As Exception
            'relancer l'exception
            Throw New Exception(ex.Message, ex)
        End Try
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Cette procédure fait appel à la méthode d'écriture du dépot de la classe 
    ''' TsCuMenageJeton.
    ''' </summary>
    ''' <param name="dsDepot">Dataset qui contient le dépot</param>
    ''' <param name="strPropGroup">Nom du SharedPropertyGroup de COM+</param>
    ''' <param name="strProp">Nom du SharedProperty de COM+ qui recevra le dépot
    ''' </param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[T208320]	2006-10-13	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Sub EcrireCacheDepot(ByRef dsDepot As DataSet, ByVal strPropGroup As String, ByVal strProp As String)
        Try
            TsCuMenageJeton.Instance.EcrireCacheDepot(dsDepot, strPropGroup, strProp)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Cette procédure fait appel à la méthode d'écriture du dépot de la classe 
    ''' TsCuMenageJeton.
    ''' </summary>
    ''' <param name="dsDepot">Dataset qui contient le dépot</param>
    ''' <param name="objSP">Objet SharedProperty de COM+ qui recevra le dépot</param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[T208320]	2006-10-13	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Sub EcrireCacheDepot(ByRef dsDepot As DataSet, ByRef objSP As SharedProperty)
        Try
            TsCuMenageJeton.Instance.EcrireCacheDepot(dsDepot, objSP)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Cette fonction fait appel à la méthode de lecture du dépot de la classe
    ''' TsCuMenageJeton.
    ''' </summary>
    ''' <param name="dsDepot">Dataset qui recevra le contenu du dépot en mémoire
    ''' </param>
    ''' <param name="strPropGroup">Nom du SharedPropertyGroup de COM+</param>
    ''' <param name="strProp">Nom du SharedProperty de COM+ qui contient le dépot
    ''' </param>
    ''' <returns>True: traitement ok, False: traitement pas ok</returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[T208320]	2006-10-13	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Function ObtenirCacheDepot(ByRef dsDepot As DataSet, ByVal strPropGroup As String, ByVal strProp As String) As Boolean
        Try
            Return TsCuMenageJeton.Instance.ObtenirCacheDepot(dsDepot, strPropGroup, strProp)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Cette fonction fait appel à la méthode de lecture du dépot de la classe
    ''' TsCuMenageJeton.
    ''' </summary>
    ''' <param name="dsDepot">Dataset qui recevra le contenu du dépot en mémoire
    ''' </param>
    ''' <param name="objSP">Objet SharedProperty de COM+ qui contient le dépot</param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[T208320]	2006-10-13	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Sub ObtenirCacheDepot(ByRef dsDepot As DataSet, ByRef objSP As SharedProperty)
        Try
            TsCuMenageJeton.Instance.ObtenirCacheDepot(dsDepot, objSP)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "Fonctions et procédures privées"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Cette fonction retourne une clé cryptographique alléatoire qui servira de clé
    ''' de session pour un nouveau jeton.
    ''' </summary>
    ''' <returns>Clé cryptographique</returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[T208320]	2006-10-13	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Function GenererSessionID() As String
        Try
            Dim cspRng As System.Security.Cryptography.RandomNumberGenerator = _
                System.Security.Cryptography.RNGCryptoServiceProvider.Create()
            Dim a_bRndKey(14) As Byte

            cspRng.GetBytes(a_bRndKey)

            Return Convert.ToBase64String(a_bRndKey)
        Catch ex As Exception
            'relancer l'exception
            Throw New Exception(ex.Message, ex)
        End Try
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Cette méthode initialise le dataset qui recoit le dépot en mémoire.
    ''' </summary>
    ''' <param name="dsDepot">Dataset qui recevra le dépot en mémoire</param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[T208320]	2006-10-13	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub InitDepot(ByRef dsDepot As DataSet)
        Try
            SyncLock TsCuMenageJeton.Instance
                'créer le dataset et ajouter les champs
                dsDepot = New DataSet(C_TABLE_NAME)
                dsDepot.Tables.Add(C_TABLE_NAME)
                dsDepot.Tables(C_TABLE_NAME).Columns.Add(C_DEPOT_CLE, GetType(String))
                dsDepot.Tables(C_TABLE_NAME).Columns.Add(C_DEPOT_USAGER, GetType(String))
                dsDepot.Tables(C_TABLE_NAME).Columns.Add(C_DEPOT_COMPOSANT, GetType(String))
                dsDepot.Tables(C_TABLE_NAME).Columns.Add(C_DEPOT_ACTIF, GetType(Boolean))
                dsDepot.Tables(C_TABLE_NAME).Columns.Add(C_DEPOT_EXPIRATION, GetType(DateTime))

                'obtenir le dépot de la SharedProperty de COM+
                ObtenirCacheDepot(dsDepot, C_SHRD_PROP_GROUP, C_SHRD_PROP)
            End SyncLock
        Catch ex As Exception
            'relancer l'exception
            Throw New Exception(ex.Message, ex)
        End Try
    End Sub
#End Region

#Region "Contructeur"
    Public Sub New()
        Try
            'initialiser le dataset
            InitDepot(m_dsDepot)
        Catch ex As Exception
            'relancer l'exception
            Throw New Exception(ex.Message, ex)
        End Try
    End Sub
#End Region
End Class

''' -----------------------------------------------------------------------------
''' Project	 : TS6N021_ZpGestionJeton
''' Class	 : TsCuMenageJeton
''' 
''' -----------------------------------------------------------------------------
''' <summary>
''' Cette classe gère le ménage du dépot des jetons en mémoire. Seulement une 
''' instance de cette classe est créée à la fois.
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[T208320]	2006-10-13	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Class TsCuMenageJeton

#Region "Déclaration des variables publiques"
    Public Shared ReadOnly Instance As New TsCuMenageJeton
#End Region

#Region "Constructeur"
    Private Sub New()
        'démarrer la thread de ménage des jetons en mémoire
        Dim thdMenage As New Thread(AddressOf Menage)
        thdMenage.Start()
    End Sub
#End Region

#Region "Fonctions et procédures publiques"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Cette procédure inscrit le contenu du dataset passé en paramètre dans une 
    ''' SharedProperty sous forme XML.
    ''' </summary>
    ''' <param name="dsDepot">Dataset qui contient le dépot</param>
    ''' <param name="strPropGroup">Nom du SharedPropertyGroup</param>
    ''' <param name="strProp">Nom du SharedProperty</param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[T208320]	2006-10-13	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Sub EcrireCacheDepot(ByRef dsDepot As DataSet, ByVal strPropGroup As String, ByVal strProp As String)
        Try
            Dim bPropExiste As Boolean
            Dim objSPGM As New SharedPropertyGroupManager
            Dim objSPG As SharedPropertyGroup = objSPGM.CreatePropertyGroup(strPropGroup, PropertyLockMode.SetGet, _
                PropertyReleaseMode.Process, False)
            Dim objSP As SharedProperty = objSPG.CreateProperty(strProp, bPropExiste)

            If dsDepot.Tables(C_TABLE_NAME).Rows.Count > 0 Then
                'affecter le xml représentant le dataset à la propriété COM+
                objSP.Value = Encoding.UTF8.GetBytes(dsDepot.GetXml())
            Else
                objSP.Value = 0
            End If
        Catch ex As Exception
            'relancer l'exception
            Throw New Exception(ex.Message, ex)
        End Try
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Cette procédure inscrit le contenu du dataset passé en paramètre dans une 
    ''' SharedProperty sous forme XML.
    ''' </summary>
    ''' <param name="dsDepot">Dataset qui contient le dépot</param>
    ''' <param name="objSP"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[T208320]	2006-10-13	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Sub EcrireCacheDepot(ByRef dsDepot As DataSet, ByRef objSP As SharedProperty)
        Try
            If dsDepot.Tables(C_TABLE_NAME).Rows.Count > 0 Then
                objSP.Value = Encoding.UTF8.GetBytes(dsDepot.GetXml())
            Else
                objSP.Value = 0
            End If
        Catch ex As Exception
            'relancer l'exception
            Throw New Exception(ex.Message, ex)
        End Try
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Cette fonction lit le contenu du dépot de jeton dans le dataset passé en 
    ''' paramètre.
    ''' </summary>
    ''' <param name="dsDepot">Dataset qui recevra le contenu du dépot</param>
    ''' <param name="strPropGroup">Nom du SharedPropertyGroup</param>
    ''' <param name="strProp">Nom du SharedProperty qui contient le dépot en mémorie
    ''' </param>
    ''' <returns>True: traitement ok, False: traitement pas ok</returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[T208320]	2006-10-13	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Function ObtenirCacheDepot(ByRef dsDepot As DataSet, ByVal strPropGroup As String, ByVal strProp As String) As Boolean
        Try
            Dim bPropExiste As Boolean
            Dim objSPGM As New SharedPropertyGroupManager
            Dim objSPG As SharedPropertyGroup = objSPGM.CreatePropertyGroup(strPropGroup, PropertyLockMode.SetGet, _
                PropertyReleaseMode.Process, False)
            Dim objSP As SharedProperty = objSPG.CreateProperty(strProp, bPropExiste)

            dsDepot.Clear()

            If bPropExiste Then
                Dim strXml As String = String.Empty

                If objSP.Value.GetType().Name = "Byte[]" Then
                    'aller chercher le xml représentant le dépot de jetons
                    strXml = Encoding.UTF8.GetString(CType(objSP.Value, Byte()))
                End If

                If Not (strXml = "" Or strXml = ("<" + dsDepot.DataSetName + " />")) Then
                    'charger le xml dans le dataset
                    Dim srXml As System.IO.StringReader = New System.IO.StringReader(strXml)
                    dsDepot.ReadXml(srXml)
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Cette fonction lit le contenu du dépot de jeton dans le dataset passé en 
    ''' paramètre.
    ''' </summary>
    ''' <param name="dsDepot">Dataset qui recevra le contenu du dépot</param>
    ''' <param name="objSP">Objet SharedProperty qui contient le dépot en mémorie
    ''' </param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[T208320]	2006-10-13	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Sub ObtenirCacheDepot(ByRef dsDepot As DataSet, ByRef objSP As SharedProperty)
        Try
            Dim strXml As String = String.Empty

            dsDepot.Clear()

            If objSP.Value.GetType().Name = "Byte[]" Then
                'aller chercher le xml représentant le dépot de jetons
                strXml = Encoding.UTF8.GetString(CType(objSP.Value, Byte()))
            End If

            If Not (strXml = "" Or strXml = ("<" + dsDepot.DataSetName + " />")) Then
                'charger le xml dans le dataset
                Dim srXml As System.IO.StringReader = New System.IO.StringReader(strXml)
                dsDepot.ReadXml(srXml)
            End If
        Catch ex As Exception
            'relancer l'exception
            Throw New Exception(ex.Message, ex)
        End Try
    End Sub
#End Region

#Region "Procédure de ménage des jetons en mémoire"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Cette procédure gère le ménage des jetons inactifs ou expirés dans le dépot 
    ''' en mémoire. Celle-ci est exécuté par une thread qui est démarrée à la création 
    ''' de la classe.
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[T208320]	2006-10-13	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub Menage()
        Try
            Dim bPropExiste As Boolean
            Dim iDelaisMenageSec As Integer
            Dim dVieJetonMin As Double
            Dim drRow As DataRow
            Dim dsDepot As New DataSet(C_TABLE_NAME)
            Dim objSPGM As New SharedPropertyGroupManager
            Dim objSPG As SharedPropertyGroup = objSPGM.CreatePropertyGroup(C_SHRD_PROP_GROUP, _
                PropertyLockMode.SetGet, PropertyReleaseMode.Process, False)
            Dim objSP As SharedProperty = objSPG.CreateProperty(C_SHRD_PROP, bPropExiste)

            'créer le dataset qui va recevoir les données du dépot en mémoire
            dsDepot.Tables.Add(C_TABLE_NAME)
            dsDepot.Tables(C_TABLE_NAME).Columns.Add(C_DEPOT_CLE, GetType(String))
            dsDepot.Tables(C_TABLE_NAME).Columns.Add(C_DEPOT_USAGER, GetType(String))
            dsDepot.Tables(C_TABLE_NAME).Columns.Add(C_DEPOT_COMPOSANT, GetType(String))
            dsDepot.Tables(C_TABLE_NAME).Columns.Add(C_DEPOT_ACTIF, GetType(Boolean))
            dsDepot.Tables(C_TABLE_NAME).Columns.Add(C_DEPOT_EXPIRATION, GetType(DateTime))

            Do While True
                'aller chercher les délais d'expiration des jetons
                iDelaisMenageSec = Convert.ToInt32(Cfg.ValeurSysteme("TS6", "TS6\TS6N021\DelaisMenageSec"))
                dVieJetonMin = Convert.ToDouble(Cfg.ValeurSysteme("TS6", "TS6\TS6N021\VieJetonMin"))

                SyncLock TsCuMenageJeton.Instance
                    'aller chercher la liste des jetons en mémoire contenu dans un shared property de COM+
                    ObtenirCacheDepot(dsDepot, objSP)

                    Dim a_drRows() As DataRow = dsDepot.Tables(C_TABLE_NAME).Select()

                    For Each drRow In a_drRows
                        'déterminer si le jeton courant est expiré ou inactif
                        If drRow.Item(C_DEPOT_ACTIF) = False Or DateTime.Now > _
                                    DirectCast(drRow.Item(C_DEPOT_EXPIRATION), DateTime).AddMinutes(dVieJetonMin) Then

                            'supprimer le jeton du dépot
                            drRow.Delete()
                        End If

                        Thread.Sleep(1)
                    Next

                    dsDepot.Tables(C_TABLE_NAME).AcceptChanges()

                    'inscrire le dépot dans le shared property de COM+
                    EcrireCacheDepot(dsDepot, objSP)
                End SyncLock

                'attendre avant de réexécuter le traitement de ménage
                Thread.Sleep(iDelaisMenageSec * 1000)
            Loop
        Catch ex As Exception
            'relancer l'exception
            Throw New Exception(ex.Message, ex)
        End Try
    End Sub
#End Region
End Class