Imports Rrq.InfrastructureCommune.Parametres
Imports System.Text
Imports System.IO

'''-----------------------------------------------------------------------------
''' Project		: TS5N133_GererVerrous
''' Class		: TsCdGererVerrous
'''
'''-----------------------------------------------------------------------------
''' <summary>
''' S'occupe de donner le statut d'un service et l'état d'une demande de
''' graduation via le fichier Verrous.
''' </summary>
''' <remarks></remarks>
''' <history>
''' 	[T208105] 	2003-11-18	Created
''' </history>
'''-----------------------------------------------------------------------------
#Region "--- Énumérations ---"

Public Enum TsGvEtatVerrous
    TsGvEVModification
    TsGvEVLecture
    TsGvEVVerrouille
End Enum

#End Region

Public Class TsCdGererVerrous

#Region "--- Énumérations ---"
    Private Enum TsGvEtatOuverture
        TsGvEoLectureSeul = 0
        TsGvEoEcriture = 1
        TsGvEoFermerFichier = 2
        TsGvEoArreterTraitement = 100

    End Enum
#End Region

#Region "--- Constantes ---"
    Private Const FICHIER_VERROUS As String = "TS51033_Verrous.xml"

    ' Nom des champs et table du fichier XML
    Private Const CHAMP_ETAT As String = "Etat"
    Private Const CHAMP_NOM As String = "NomServeur"
    Private Const CHAMP_COMPTE As String = "AppliquePar"
    Private Const CHAMP_RESERVE As String = "ReservePar"
    Private Const CHAMP_ACTIVITE As String = "Activite"
    Private Const TABLE_SERVEUR As String = "Serveur"
    Private Const TABLE_CONSOLE As String = "Console"
    Private Const DATASET_VERROUS As String = "Verrous"

    ' État inscrit dans le fichier XML
    Private Const ETAT_VERROUILLE As String = "TsGvEVVerrouille"
    Private Const ETAT_OUVERT As String = "TsGvEVModification"
    Private Const ETAT_LECTURE As String = "TsGvEVLecture"

    ' Délai de 5 sec pour essayer d'ouvrir le fichier de verrous
    Private Const NOMBRE_ESSAI_MAXIMUM As Integer = 50
    Private Const DELAIS_ESSAI As Integer = 100

#End Region

#Region "--- Variables ---"

    Private mNomFichierVerrous As String
    Private mUtilisateur As String
    Private mFichierVerrous As FileStream
    Private mDonneeVerrous As New DataSet
    Private mEtatTrvDansFichier As Boolean
    Private mAppeleParConsole As Boolean
    Private mDtFichVerrou As New Date(1900, 1, 1)

#End Region

#Region "--- Constructeurs ---"
    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	TsCdGererVerrous.New
    ''' <summary>
    '''     Initialise une nouvelle instance de la classe TsCdGererVerrous.
    ''' </summary>
    ''' --------------------------------------------------------------------------------
    Public Sub New(ByVal pParentEstConsole As Boolean)

        mUtilisateur = Environment.UserName
        mNomFichierVerrous = CheminSecuriteCOM & FICHIER_VERROUS
        mAppeleParConsole = pParentEstConsole

    End Sub
#End Region

#Region "--- Publiques ---"

#Region "--- Méthodes ---"
    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	TsCdGererVerrous.Verrouille
    ''' <summary>
    '''     Modifie l'état du verrou du serveur au fichier de verrouillage 
    ''' </summary>
    ''' <param name="NomServeur">
    ''' 	Nom du serveur courant. 
    ''' </param>
    ''' <param name="EtatVerrou">
    ''' 	Nouvel état de verrou à appliquer. 
    ''' </param>
    ''' --------------------------------------------------------------------------------
    Public Sub Verrouille(ByVal NomServeur As String, ByVal EtatVerrou As TsGvEtatVerrous)

        EcrireDonne(NomServeur, EtatVerrou)

    End Sub


    '''-----------------------------------------------------------------------------
    ''' Class.Method:	TsCdGererVerrous.EtatVerrou
    ''' <summary>
    ''' Récupère l'état du verrou du serveur au fichier de verrouillage
    ''' </summary>
    ''' <param name="NomServeur">
    ''' 	Nom du serveur à interroger. 
    ''' </param>
    ''' <returns>État du verrou</returns>
    ''' <remarks></remarks>
    '''-----------------------------------------------------------------------------
    Public Function EtatVerrou(ByVal NomServeur As String) As TsGvEtatVerrous

        Dim sEtat As String
        Dim Etat As TsGvEtatVerrous

        ' Vérifier si la console est déjà réservé par un autre utilisateur
        If mAppeleParConsole AndAlso ConsoleEstReserve() Then
            Return TsGvEtatVerrous.TsGvEVLecture
        End If

        ' Récupérer l'état du serveur.  
        sEtat = LireEtat(NomServeur, ETAT_OUVERT)

        Select Case sEtat
            Case ETAT_OUVERT
                Etat = TsGvEtatVerrous.TsGvEVModification
            Case ETAT_LECTURE
                Etat = TsGvEtatVerrous.TsGvEVLecture
            Case ETAT_VERROUILLE
                Etat = TsGvEtatVerrous.TsGvEVVerrouille
        End Select

        Return Etat

    End Function

    '''-----------------------------------------------------------------------------
    ''' Class.Method:	TsCdGererVerrous.NotifierEnVie
    ''' <summary>
    ''' Inscrit au fichier des verrous que la console est réservé en modifications 
    ''' à l'utilisateur courant si elle est libre.
    ''' </summary>
    ''' <remarks></remarks>
    '''-----------------------------------------------------------------------------
    Public Sub NotifierEnVie()
        ' On met a jour l'enregistrement pour notifier que la console est réservée
        If Not ConsoleEstReserve() Then
            Dim Table As DataTable = Nothing
            Dim Ligne As DataRow

            Try
                Table = GererDonneeXML(TsGvEtatOuverture.TsGvEoEcriture).Tables(TABLE_CONSOLE)
                If Table.Rows.Count = 0 Then
                    Ligne = Table.NewRow
                    Ligne.Item(CHAMP_RESERVE) = mUtilisateur
                    Ligne.Item(CHAMP_ACTIVITE) = DateTime.Now.Ticks.ToString()
                    Table.Rows.Add(Ligne)
                    Table.AcceptChanges()
                    GererDonneeXML(TsGvEtatOuverture.TsGvEoFermerFichier)
                Else
                    Ligne = Table.Rows(0)
                    Ligne.Item(CHAMP_ACTIVITE) = DateTime.Now.Ticks.ToString()
                    Table.AcceptChanges()
                    GererDonneeXML(TsGvEtatOuverture.TsGvEoFermerFichier)
                End If

            Finally
                If Not Table Is Nothing Then Table.Dispose()
                Ligne = Nothing

            End Try

        End If

    End Sub

    '''-----------------------------------------------------------------------------
    ''' Class.Method:	TsCdGererVerrous.NotifierFinVie
    ''' <summary>
    ''' Si la console est réservé par l'utilisateur courant, efface la réservation de la table.
    ''' </summary>
    ''' <remarks></remarks>
    '''-----------------------------------------------------------------------------
    Public Sub NotifierFinVie()
        ' On efface l'enregistrement pour notifier que la console n'est plus réservée
        If Not ConsoleEstReserve() Then
            Dim Table As DataTable = Nothing
            Dim Ligne As DataRow            

            Try
                Table = GererDonneeXML(TsGvEtatOuverture.TsGvEoEcriture).Tables(TABLE_CONSOLE)
                If Table.Rows.Count > 0 Then
                    Ligne = Table.Rows(0)
                    Table.Rows(0).Delete()

                    Table.AcceptChanges()
                    GererDonneeXML(TsGvEtatOuverture.TsGvEoFermerFichier)
                End If

            Finally
                If Not Table Is Nothing Then Table.Dispose()

            End Try

        End If

    End Sub

#End Region

#End Region

#Region "--- Privées ---"

#Region "--- Propriétés ---"
    '''-----------------------------------------------------------------------------
    ''' <summary>
    ''' Permet l'obtention du chemin du dépôt de la sécurité COM+.
    ''' </summary>
    '''-----------------------------------------------------------------------------
    Private ReadOnly Property CheminSecuriteCOM() As String
        Get
            If mNomFichierVerrous = vbNullString Then
                mNomFichierVerrous = XuCuConfiguration.ValeurSysteme("TS5", "CheminSecuriteCOM")
            End If

            Return mNomFichierVerrous

        End Get

    End Property

#End Region

#Region "--- Méthodes ---"

    '''-----------------------------------------------------------------------------
    ''' Class.Method:	TsCdGererVerrous.EcrireDonne
    ''' <summary>
    ''' Fait une écriture dans le fichier XML du Verrous.
    ''' </summary>
    ''' <param name="pNomServeur">
    ''' 	Nom du serveur 
    ''' </param>
    ''' <param name="pEtat">
    ''' 	Etat du verrou à appliquer 
    ''' </param>
    ''' <remarks>
    '''     L'écriture d'une nouvelle entrée signifie que si l'élément existe
    '''     il est mis à jour.
    ''' </remarks>
    '''-----------------------------------------------------------------------------
    Private Sub EcrireDonne(ByVal pNomServeur As String, _
                            ByVal pEtat As TsGvEtatVerrous)

        Dim Table As DataTable = Nothing
        Dim NouvelleLigne As DataRow
        Dim Lignes() As DataRow

        Try
            ' Étape # 1
            ' Obtenir le contenu du fichier et la table correspondante
            Table = GererDonneeXML(TsGvEtatOuverture.TsGvEoEcriture).Tables(TABLE_SERVEUR)

            ' Étape # 2
            ' Rechercher la présence du Serveur dans la table
            Lignes = Table.Select("NomServeur='" & pNomServeur & "'")

            ' Trois cas de figure sont ici envisageable
            If Lignes.Length() = 0 Then
                ' Étape # 3 A
                ' Ajouter la nouvelle ligne car il n'y a pas de ligne pour 
                ' le Serveur
                NouvelleLigne = Table.NewRow
                NouvelleLigne(CHAMP_NOM) = pNomServeur
                NouvelleLigne(CHAMP_ETAT) = pEtat.ToString
                NouvelleLigne(CHAMP_COMPTE) = mUtilisateur

                Table.Rows.Add(NouvelleLigne)
                Table.AcceptChanges()

            ElseIf Lignes.Length() = 1 Then
                ' Étape # 3 B
                ' Mettre à jour la ligne correspondant au Serveur
                NouvelleLigne = Lignes(0)
                NouvelleLigne(CHAMP_NOM) = pNomServeur
                NouvelleLigne(CHAMP_ETAT) = pEtat.ToString
                NouvelleLigne(CHAMP_COMPTE) = mUtilisateur

                Table.AcceptChanges()

            ElseIf Lignes.Length() > 1 Then
                ' Cas ou le fichier est non conforme.
                ' Ne devrait pas se produire.
                GererDonneeXML(TsGvEtatOuverture.TsGvEoArreterTraitement)
                Table = Nothing
                Throw New IOException("La table Verrous du fichier " & _
                                    mNomFichierVerrous & " est non-conforme.")

            End If
        Finally
            If Not Table Is Nothing Then
                GererDonneeXML(TsGvEtatOuverture.TsGvEoFermerFichier)
                Table.Dispose()
            End If

        End Try

    End Sub

    '''-----------------------------------------------------------------------------
    ''' Class.Method:	TsCdGererVerrous.LireEtat
    ''' <summary>
    '''     Lit une information dans une des tables du Disjonteur
    ''' </summary>
    ''' <param name="pNomserveur">
    ''' 	Nom du serveur à rechecher
    ''' </param>
    ''' <param name="pValeurDefaut">
    ''' 	Valeur qui sera retourné si on ne trouve pas l'élément correspondant
    '''     dans les données du Verrous.
    ''' </param>
    ''' <returns>
    '''     Valeur trouvé dans le fichier Verrous XML
    ''' </returns>
    '''-----------------------------------------------------------------------------
    Private Function LireEtat(ByVal pNomServeur As String, _
                             ByVal pValeurDefaut As String) As String

        Dim Retour As String = pValeurDefaut
        Dim Table As DataTable = Nothing
        Dim Lignes() As DataRow

        mEtatTrvDansFichier = False

        Try
            Table = GererDonneeXML(TsGvEtatOuverture.TsGvEoLectureSeul).Tables(TABLE_SERVEUR)

            Lignes = Table.Select("NomServeur='" & pNomServeur & "'")
            If Lignes.Length() = 1 Then
                ' On considère que la valeur du champ Etat ne sera jamais 
                ' à blanc.  Pour cette raison on envoie son contenu peut
                ' importe la valeur défaut suggéré.
                Retour = Lignes(0).Item(CHAMP_ETAT).ToString()
                mEtatTrvDansFichier = True
            ElseIf Lignes.Length > 1 Then
                ' Cas ou le fichier est non conforme.
                ' Ne devrait pas se produire.
                GererDonneeXML(TsGvEtatOuverture.TsGvEoArreterTraitement)
                Table = Nothing
                Throw New IOException("La table Serveur du fichier " & _
                                      mNomFichierVerrous & " est non-conforme.")
            End If

        Finally
            If Not Table Is Nothing Then
                GererDonneeXML(TsGvEtatOuverture.TsGvEoFermerFichier)
                Table.Dispose()
            End If

        End Try

        Return Retour

    End Function

    Private Function ConsoleEstReserve() As Boolean
        Dim Table As DataTable = Nothing
        Dim Etat As Boolean = False
        Dim TempsActuel As Date = Now
        Dim TempsFichier As Date

        Try
            Table = GererDonneeXML(TsGvEtatOuverture.TsGvEoLectureSeul).Tables(TABLE_CONSOLE)
            If Table.Rows.Count > 0 Then
                If Table.Rows(0).Item(CHAMP_RESERVE).ToString <> mUtilisateur  Then
                    Etat = True
                End If

                ' Ajouter un délai de 5 minutes au cas où l'utilisateur est en attente d'une commande 
                TempsFichier = New Date(CLng(Table.Rows(0).Item(CHAMP_ACTIVITE).ToString)).AddMinutes(5)

                If TempsFichier < TempsActuel Then
                    Etat = False
                    ' Libère une console désuète...
                    Table = GererDonneeXML(TsGvEtatOuverture.TsGvEoEcriture).Tables(TABLE_CONSOLE)
                    Table.Rows(0).Delete()
                    Table.AcceptChanges()
                    GererDonneeXML(TsGvEtatOuverture.TsGvEoFermerFichier)
                End If
            End If

            Return Etat
        Finally
            If Not Table Is Nothing Then Table.Dispose()

        End Try

    End Function

    Private Function GererDonneeXML(ByVal pEtatOuverture As TsGvEtatOuverture) As DataSet

        Dim AccesFichier As FileAccess = FileAccess.Read
        Dim PartageFichier As FileShare
        Dim NombreEssai As Integer = 0
        Dim FermerFichier As Boolean

        If pEtatOuverture = TsGvEtatOuverture.TsGvEoEcriture Then
            AccesFichier = FileAccess.ReadWrite
            PartageFichier = FileShare.None
            FermerFichier = False

        ElseIf pEtatOuverture = TsGvEtatOuverture.TsGvEoLectureSeul Then
            AccesFichier = FileAccess.Read
            PartageFichier = FileShare.ReadWrite
            FermerFichier = True

        ElseIf pEtatOuverture = TsGvEtatOuverture.TsGvEoFermerFichier Then
            ' S'assure que le fichier était ouvert en écriture
            If Not mFichierVerrous Is Nothing Then
                mFichierVerrous.SetLength(0)
                mDonneeVerrous.WriteXml(mFichierVerrous, XmlWriteMode.WriteSchema)
                mFichierVerrous.Close()
                mFichierVerrous = Nothing
            End If
            Return Nothing

        ElseIf pEtatOuverture = TsGvEtatOuverture.TsGvEoArreterTraitement Then
            Try
                If Not mFichierVerrous Is Nothing Then
                    mFichierVerrous.Close()
                    mFichierVerrous = Nothing
                End If

            Catch ex As IOException
                Return Nothing
            End Try

            Return Nothing

        End If

        If Not IO.File.Exists(mNomFichierVerrous) Then CreerFichierVerrou()

        If mDtFichVerrou = IO.File.GetLastWriteTime(mNomFichierVerrous) AndAlso _
           AccesFichier = FileAccess.Read Then
            Return mDonneeVerrous
        End If

        Do While mFichierVerrous Is Nothing
            Try
                mFichierVerrous = New FileStream(mNomFichierVerrous, _
                                                     FileMode.Open, _
                                                     AccesFichier, _
                                                     PartageFichier)

                mDonneeVerrous.Clear()
                mDonneeVerrous.ReadXml(mFichierVerrous, XmlReadMode.ReadSchema)

            Catch exIO As IOException
                If NombreEssai >= NOMBRE_ESSAI_MAXIMUM Then
                    Throw
                End If
                Threading.Thread.Sleep(DELAIS_ESSAI)
                NombreEssai += 1

            End Try

        Loop

        If FermerFichier AndAlso pEtatOuverture = TsGvEtatOuverture.TsGvEoLectureSeul Then
            mFichierVerrous.Close()
            mFichierVerrous = Nothing
        End If

        mDtFichVerrou = IO.File.GetLastWriteTime(mNomFichierVerrous)
        Return mDonneeVerrous

    End Function

    Private Sub CreerFichierVerrou()
        Dim Ds As DataSet = New DataSet(DATASET_VERROUS)
        Dim Dt As DataTable

        Dt = New DataTable(TABLE_SERVEUR)
        Dt.Columns.Add(CHAMP_NOM, GetType(System.String))
        Dt.Columns.Add(CHAMP_ETAT, GetType(System.String))
        Dt.Columns.Add(CHAMP_COMPTE, GetType(System.String))
        Ds.Tables.Add(Dt)
        Dt.Dispose()

        Dt = New DataTable(TABLE_CONSOLE)
        Dt.Columns.Add(CHAMP_RESERVE, GetType(System.String))
        Dt.Columns.Add(CHAMP_ACTIVITE, GetType(System.String))
        Ds.Tables.Add(Dt)

        Ds.WriteXml(mNomFichierVerrous, XmlWriteMode.WriteSchema)
        Ds.Dispose()
        Ds = Nothing
        Dt.Dispose()
        Dt = Nothing

    End Sub

#End Region

#End Region

End Class
