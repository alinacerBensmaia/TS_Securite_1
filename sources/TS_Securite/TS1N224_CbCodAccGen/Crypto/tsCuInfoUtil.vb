Imports System.IO
Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates
Imports Rrq.Securite.Certificat

''' --------------------------------------------------------------------------------
''' Project:	TS1N224_CbCodAccGen
''' Class:	    Rrq.Securite.TsCuInfoUtil
''' <summary>
''' Dans cette classe, on y retrouve les méthodes utilisées par plus d'une classe
''' Ce sont des méthodes générales de traitement.
''' ''' </summary>
''' <remarks>
''' Historique des modifications: 
''' --------------------------------------------------------------------------------
''' Date		Nom			Description
''' --------------------------------------------------------------------------------
''' 2007-11-26	T206500		Transférer les paramètres de configuration dans le fichier
'''                         "TS1.config". On utilise l'utilitaire XU4N011_Configuration
'''                         pour obtenir ces paramètres. 
'''                         Seulement la classe TsCuInfoUtil est conservée dans 
'''                         ce fichier. Chaque classe est maintenant isolée dans son
'''                         propre fichier. 
'''                         Enlever les propriétés, elles n'étaient pas utiles ici.
'''                         Conserver seulement les méthodes qui sont utilisées par 
'''                         plus d'une classe. Les méthodes utilisées par une classe
'''                         sont devenues des méthodes privées dans leur classe.
'''                         Normaliser les noms des variables et des méthodes.
'''                         Enlever toutes les variables privées. On ne conserve pas
'''                         d'état dans cette classe utilitaire.
''' 
''' 2009-12-08  T206500     Effectuer des modifications pour régler les problèmes
'''                         de performance.  
'''                         On lit le fichier des codes d'accès aux 5 minutes afin de 
'''                         limiter les accès au serveur FIC1.
'''                         Les groupes d'accès de l'utilisateur sont conservés en SID                        
'''                         dans le jeton de sécurité.  Avant, on effectuait une conversion
'''                         de chaque groupe d'accès.  Ce qui impliquait un accès à l'AD
'''                         pour chaque groupe.  Maintenant, on compare le SID du groupe
'''                         d'accès avec le SID du profil de la clé symbolique.
'''                         Donc, seulement, le profil recherché est converti en SID. On
'''                         effectue un accès à l'AD.
''' 
''' 2010-01-11  T206500     Ajout des constantes NOM_GROUPE_ADMIN_SECURITE et 
'''                         DELAI_VERIF_FICHIER_CODE_ACCES     
''' 
''' 2010-01-13  T206500     Effectuer un SyncLock avant chaque chargement de fichier.
'''                         Refaire les comparaisons juste avant de charger, car
'''                         on ne veut pas charger inutilement.                   
'''                         Utiliser un dataset local lors du chargement.
''' 
''' 2010-01-21  t206500    Utiliser la méthode ObtenirCodeUsager pour obtenir le code
'''                        utilisateur. On fait une exception lorsque c'est le compte SYSTEM. 
''' 
''' 2010-01-25  t206500    Dans la méthode EstMembreDe, on ajoute une execption.
'''                        Le compte SYSTEM  a accès à toutes les clés symboliques
'''                        lorsque c'est le composant TS5N132_ZpGerCOMPlus qui 
'''                        effectue l'appel à TS1N224.
''' 
''' </remarks>
''' --------------------------------------------------------------------------------

Friend NotInheritable Class TsCuInfoUtil
    'Private Const NOM_GROUPE_ADMIN_SECURITE As String = "AdmResp Securite"
    Private Const DELAI_VERIF_FICHIER_CODE_ACCES As Double = 5
    Private Shared messagesLock As New Object

#Region "--- Constructeur ---"

    Private Sub New()
        'transformer la classe en classe "static", en mettant constructeur privé ça empèche de créer un instance de la classe
    End Sub

#End Region

#Region "--- Méthodes Amies ---"

    ''' --------------------------------------------------------------------------------
    ''' Class.Method  (friend):	LireFichierCodeAcces
    ''' <summary>
    ''' Lire le fichier contenant les informations des codes d'accès génériques (TS1N214.xml).
    ''' Si le fichier a été modifié depuis la dernière lecture, lire le fichier sur disque.
    ''' Sinon, retourner le dataset conservé en mémoire.
    ''' </summary>
    ''' <param name="dsCodeAcces">
    '''     dataset qui contient toutes les clés symboliques du fichier XML (TS1N214.xml).	
    '''     Value Type: dataset
    ''' </param>
    ''' <remarks>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2007-11-26	t206500		Normaliser la méthode
    '''                         Correspond à l'ancienne méthode "ReadPwdDS".    
    ''' 
    ''' 2009-12-01  t206500     Transférer du code dans la méthode ChargerFichierCodeAcces
    '''                         afin de faire un lock seulement lors de l'accès au fichier
    '''                         Utilisation d'un flag pour forcer le rafraichissement
    '''                         des données.  Pour une question de performance,
    '''                         on vérifie aux 15 minutes si on doit rafraîchir les 
    '''                         clés symboliques en mémoire.
    ''' 
    ''' 2010-01-13  T206500     Effectuer toutes les vérifications dans cette méthode.
    '''                         On utilise Nullable avec la variable dtProchVerifModifDepot
    '''                         afin d'éviter que l'on compare une valeur partielle.
    '''                         Effectuer le SyncLock avant chaque lecture du fichier
    '''                         des codes d'accès et avant la vérification de la date
    '''                         du fichier des codes d'accès.
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    Friend Shared Function LireFichierCodeAcces() As DataSet
        Dim strNomFichCdAcc As String = Config.PASSWDFILEPROD


        ' 1ere vérification  -  On vérifie si le fichier est vide (la première fois)
        If TsCuVarShared.dsCachedCodeAcces Is Nothing Then
            SyncLock messagesLock
                ChargerFichierCodeAcces(strNomFichCdAcc)
            End SyncLock
        Else
            ' 2ième vérification  -  On vérifie si le nom du fichier a été modifié depuis le dernier chargement en mémoire
            If TsCuVarShared.strDernNomFichCdAcc <> strNomFichCdAcc Then
                SyncLock messagesLock
                    ChargerFichierCodeAcces(strNomFichCdAcc)
                End SyncLock
            Else

                ' 3ième vérification  -  On vérifie si on a atteint le délai de vérification sur FIC1
                If Now > TsCuVarShared.dtProchVerifModifDepot.Value Then

                    ' Initialisation de la prochaine vérification de changement du fichier des codes accès génériques
                    ' On utilise Nullable afin de s'assurer que la date n'est pas partielle lors de la vérification
                    TsCuVarShared.dtProchVerifModifDepot = New Nullable(Of DateTime)(DateAdd(DateInterval.Minute, DELAI_VERIF_FICHIER_CODE_ACCES, Now))

                    '4ième vérification  -  On vérifie si le fichier a été modifié depuis le dernier chargement en mémoire
                    If TsCuVarShared.dtDernModifDepot <> File.GetLastWriteTime(strNomFichCdAcc) Then
                        SyncLock messagesLock
                            ChargerFichierCodeAcces(strNomFichCdAcc)
                        End SyncLock
                    End If
                End If
            End If
        End If

        Return TsCuVarShared.dsCachedCodeAcces
    End Function

    ''' --------------------------------------------------------------------------------
    ''' Class.Method  (friend):	EcrireFichierCodeAcces
    ''' <summary>
    ''' Ecrire le fichier contenant les données des codes d'accès génériques (TS1N214.xml).
    ''' On utilise cette méthode pour créer le fichier de l'inforoute et ZDE.
    ''' </summary>
    ''' <param name="dsCodeAcces">
    '''     le dataset que l'on doit copier dans le fichier XML.
    ''' 	Value Type: dataset
    ''' </param>
    ''' <param name="strNomFich">
    ''' 	Nom du fichier qui contient les données des codes d'accès génériques TS1N214.xml
    ''' 	Value Type: string
    ''' </param>
    ''' <remarks>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2007-11-26	t206500		Normaliser la méthode
    '''                         Correspond à l'ancienne méthode "WritePwdDS".
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    Friend Shared Sub EcrireFichierCodeAcces(ByRef dsCodeAcces As DataSet,
                                             ByVal strNomFich As String)

        If dsCodeAcces IsNot Nothing AndAlso
           Not String.IsNullOrEmpty(strNomFich) Then

            ' Il peut y avoir plus d'un fichier de destination pour le même contenu
            Dim fichiers As String() = strNomFich.Split(";"c)

            For Each fichier As String In fichiers
                'Prendre un backup des fichiers avant de les modifier
                TsCuFichier.BackupFile(fichier)

                ' 2007-11-26 T206500 Ramener la clé d'encryption et le vecteur d'initialisation.
                Dim K_Fichr(31) As Byte
                Dim V_Fichr(15) As Byte

                TsCuConversions.ConvertirStringByte(K_Fichr, V_Fichr)

                Using objChiffreur As FileStream = New FileStream(fichier, FileMode.Create, FileAccess.Write, FileShare.None),
                      objRijndael As New RijndaelManaged

                    Dim objChiff As ICryptoTransform = objRijndael.CreateEncryptor(K_Fichr, V_Fichr)
                    Using objStrm As New CryptoStream(objChiffreur, objChiff, CryptoStreamMode.Write)
                        dsCodeAcces.WriteXml(objStrm)
                    End Using
                End Using
            Next
        End If
    End Sub

#End Region


    ''' --------------------------------------------------------------------------------
    ''' Class.Method  (private):	ChargerFichierCodeAcces
    ''' <summary>
    ''' Obtenir le fichier des codes accès sur le disque.
    ''' </summary>
    ''' <param name="strNomFichCdAcc">
    ''' 	Nom du fichier XML qui contient les informations des clés symboliques TS1N214.xml
    ''' 	Value Type: string
    ''' </param>
    ''' <remarks>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2009-12-01	t206500		Création de la méthode
    '''                         Isoler l'accès sur disque dans cette méthode
    '''                         Initialisation de la prochaine vérification du 
    '''                         changement du dépôt.  On lit le dépôt aux 15 minutes
    ''' 
    ''' 2010-01-13  T206500     Réfaire les validations avant d'effectuer le chargement
    '''                         Utiliser un dataset temporaire afin d'éviter qu'un autre
    '''                         process utilise le dataset pendant le chargement.
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    Private Shared Sub ChargerFichierCodeAcces(ByVal strNomFichCdAcc As String)
        Dim intCpt As Integer = 0

        Dim exOrig As Exception = Nothing
        Dim blnLectOK As Boolean = False
        Using dsCodeAccesTemp As New DataSet

            ' On doit refaire les validations pour s'assurer que l'on ne charge pas inutilement.
            ' Si on avait deux appels en même temps, le deuxième a attendu parce que nous
            ' avons un synclock.  Par contre, on ne veut pas que le chargement s'effectue deux fois.  
            ' L'appel précédent a déjà chargé en mémoire le dataset.

            If TsCuVarShared.dsCachedCodeAcces Is Nothing OrElse
               TsCuVarShared.strDernNomFichCdAcc <> strNomFichCdAcc OrElse
               TsCuVarShared.dtDernModifDepot <> File.GetLastWriteTime(strNomFichCdAcc) Then

                Do While intCpt < 10
                    Try
                        ' 2007-11-26 T206500 Ramener la clé d'encryption et le vecteur d'initialisation dans cette méthode.
                        ' 2008-02-20 T206500 Renommer les variables pour qu'elles soient un peu moins claires à la décompilation (avec reflector)...

                        Using objFileStream As FileStream = New FileStream(strNomFichCdAcc, FileMode.Open, FileAccess.Read, FileShare.Read)
                            dsCodeAccesTemp.ReadXml(objFileStream)
                        End Using
                        blnLectOK = True

                    Catch ex As Exception
                        exOrig = ex
                        Threading.Thread.Sleep(100)
                        intCpt += 1
                    End Try

                    If blnLectOK Then Exit Do
                Loop

                If Not blnLectOK Then
                    Throw New TsCuLectureDepotImpossible(exOrig, strNomFichCdAcc)
                End If

                ''---------------------------------------------------------------------------------------------------------
                '' 2010-01-13   -  T206500  -  Manon Jalbert  
                '' Transférer les instructions suivantes à la fin lorsque le fichier est chargé dans le dataset temporaire.
                '' Rafraichir les informations conservées en mémoire.
                ''---------------------------------------------------------------------------------------------------------

                ' Rafraichir le dateset des codes d'accès en mémoire
                TsCuVarShared.dsCachedCodeAcces = dsCodeAccesTemp.Copy

                ' Obtenir la dernière date de modification du fichier et la conserver en mémoire
                TsCuVarShared.dtDernModifDepot = File.GetLastWriteTime(strNomFichCdAcc)

                ' Déterminer l'heure de la prochaine vérification du changement du dépot
                ' --> On utilise Nullable afin de s'assurer que la date n'est pas partielle lors de la vérification
                TsCuVarShared.dtProchVerifModifDepot = New Nullable(Of DateTime)(DateAdd(DateInterval.Minute, DELAI_VERIF_FICHIER_CODE_ACCES, Now))

                ' Conserver le nom du fichier des codes d'accès en mémoire
                TsCuVarShared.strDernNomFichCdAcc = strNomFichCdAcc
            End If
        End Using
    End Sub

    Friend Shared Sub EcrireFichierCodeAccesXML(ByRef dsCodeAcces As DataSet,
                                             ByVal strNomFich As String)

        If dsCodeAcces IsNot Nothing AndAlso
           Not String.IsNullOrEmpty(strNomFich) Then

            ' Il peut y avoir plus d'un fichier de destination pour le même contenu
            Dim fichiers As String() = strNomFich.Split(";"c)

            For Each fichier As String In fichiers
                'Prendre un backup des fichiers avant de les modifier
                TsCuFichier.BackupFile(fichier)

                Using objFileStream As FileStream = New FileStream(fichier, FileMode.Create, FileAccess.Write, FileShare.None)
                    dsCodeAcces.WriteXml(objFileStream)
                End Using
            Next
        End If
    End Sub

    Public Shared Function DecrypterEncrypterMotPasser(ByVal dataSetMotPasse As DataSet, ByVal NomPrintCertificat As String, ByVal thumbPrintCertificat As String) As DataSet

        Dim certificatChiffrement As X509Certificate2 = TsCuRSA.ObtenirCertificat(thumbPrintCertificat)

        If certificatChiffrement Is Nothing Then
            certificatChiffrement = TsCaCertificatSecurite.RecupererCertificatSecurite(NomPrintCertificat)
        End If

        Dim certificatDechiffrement As X509Certificate2 = TsCuRSA.ObtenirCertificat(Config.ThumbPrintCertificatPROD)

        If certificatDechiffrement Is Nothing Then
            certificatDechiffrement = TsCaCertificatSecurite.RecupererCertificatSecurite(Config.NomCertificatPrivate)
        End If


        'Cloner DataSet
        Dim outCopieDataSet As DataSet = dataSetMotPasse.Copy()
        For Each dr As DataRow In outCopieDataSet.Tables("CdAcces").Rows
            ''***CHG1 - Correction problème de timeout lors de l'exportation du fichier.
            'Cette méthode n'est utilisé que lorsqu'on encrypte les mots de passe pour unitaire
            'Comme maintenant dans la BD le mot de passe est chiffré avec le certificat de prod, il faut le déchiffrer avec le certificat de prod
            'pour ensuite le chiffrer avec le certificat de dev
            'dr("Mdp") = TsCuRSA.Encrypt(certificat, dr("Mdp").ToString)
            dr("Mdp") = TsCuRSA.Encrypt(certificatChiffrement, TsCuRSA.Decrypt(certificatDechiffrement, dr("Mdp").ToString))
        Next

        outCopieDataSet.AcceptChanges()

        Return outCopieDataSet

    End Function

    Public Shared Function EncrypterMotPasser(ByVal NomPrintCertificat As String, ByVal thumbPrintCertificat As String, ByVal textaCrypt As String) As String

        Dim certificat As X509Certificate2 = TsCuRSA.ObtenirCertificat(thumbPrintCertificat)

        If certificat Is Nothing Then
            certificat = TsCaCertificatSecurite.RecupererCertificatSecurite(NomPrintCertificat)
        End If

        Return TsCuRSA.Encrypt(certificat, textaCrypt)

    End Function

    Public Shared Function CopierDataSet(ByVal pDataSetOriginal As DataSet, ByVal pFiltre As String) As DataSet
        Dim drFiltre As DataRow() = pDataSetOriginal.Tables("CdAcces").Select(pFiltre)
        Dim outCopieDataSet As DataSet = pDataSetOriginal.Clone()
        For Each dr As DataRow In drFiltre
            outCopieDataSet.Tables("CdAcces").ImportRow(dr)
        Next

        outCopieDataSet.AcceptChanges()

        Return outCopieDataSet

    End Function

    Public Shared Function DecrypterMotPasser(ByVal NomPrintCertificat As String, ByVal thumbPrintCertificat As String, ByVal cryptText As String) As String

        Dim certificatChiffrement As X509Certificate2 = TsCuRSA.ObtenirCertificat(thumbPrintCertificat)

        If certificatChiffrement Is Nothing Then
            certificatChiffrement = TsCaCertificatSecurite.RecupererCertificatSecurite(NomPrintCertificat)
        End If

        Return TsCuRSA.Decrypt(certificatChiffrement, cryptText)

    End Function

End Class

