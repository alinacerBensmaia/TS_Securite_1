Imports System.Collections.Generic
Imports Rrq.InfrastructureCommune.UtilitairesCommuns

Public MustInherit Class tsCuTrtParmt
    Private objStrm As System.IO.Stream

    Protected MustOverride ReadOnly Property DsParmt() As DataSet

    Protected MustOverride ReadOnly Property DtParmt() As DataTable

    Protected MustOverride ReadOnly Property objObtnrParmt() As TS6N621_ZgObtParmChif.TsCuObtnrParmt

    Public MustOverride Function Ajouter(type As TsCuParamsChiffrement.TypeCle, ByVal recordAAjouter As DataRow, nomFichierServeurExtranet As String, nomFichierServeurInterne As String, nomFichierServeurInforoute As String) As Boolean

    Public MustOverride Function CopieCle(cleACopier As DataRow) As DataRow

    Public MustOverride Sub ObtenirValeurs(ByVal objObtnrParmt As TS6N621_ZgObtParmChif.TsCuObtnrParmt, ByVal nomFichierServeurInterne As String)

    ''' <summary>
    '''     Fonction d'ajout de clé de chiffrement.
    ''' </summary>
    ''' <param name="type"></param>
    ''' <param name="recordAAjouter"></param>
    ''' <param name="nomFichierServeurExtranet"></param>
    ''' <param name="nomFichierServeurInterne"></param>
    ''' <param name="nomFichierServeurInforoute"></param>
    ''' <param name="majInterneCommun"></param>
    ''' <param name="majExterneCommun"></param>
    ''' <returns></returns>
    Public Function Ajouter(type As TsCuParamsChiffrement.TypeCle, ByVal recordAAjouter As DataRow, nomFichierServeurExtranet As String, nomFichierServeurInterne As String, nomFichierServeurInforoute As String, majInterneCommun As Boolean, majExterneCommun As Boolean) As Boolean
        Dim oRijndael As New System.Security.Cryptography.RijndaelManaged
        Dim tabLignes As DataRow()

        Try
            ' Obtenir le premier record pour le type demandé qui a le ID le plus grand
            MettreAJourSourcePrincipale(type, recordAAjouter, nomFichierServeurInterne, tabLignes)

            If majInterneCommun Then
                MettreAJourSourceExterne(nomFichierServeurExtranet)
            End If

            If majExterneCommun Then
                MettreAJourSourceInterne(nomFichierServeurInforoute)
            End If

            objObtnrParmt.ChiffrementDechiffrementPermis = False

            Return True
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Private Sub MettreAJourSourcePrincipale(type As TsCuParamsChiffrement.TypeCle, ByRef recordAAjouter As DataRow, nomFichierServeurInterne As String, ByRef tabLignes() As DataRow)

        Try
            tabLignes = DtParmt.Select("Type='" & type.ToString & "'", "ID DESC")

            ' Initialiser les valeurs du record 
            recordAAjouter.Item("Type") = type.ToString
            If CType(recordAAjouter.Item("Code"), String).Length = 0 Then
                recordAAjouter.Item("Code") = recordAAjouter.Item("ID")
            End If

            With DsParmt
                ' Ajouter le record dans le datatable
                DtParmt.Rows.Add(recordAAjouter)
                DtParmt.AcceptChanges()
                .AcceptChanges()

                ' Obtenir les données XML et le schema du Dataset pour ensuite les chiffrer
                ' dans un fichier (contient tous les types)
                objObtnrParmt.ChiffrementDechiffrementPermis = True
                objObtnrParmt.ChiffrerFichier(nomFichierServeurInterne, DsParmt)
            End With

        Catch ex As Exception
            XuCuGestionEvent.AjouterEvenmNormalise(XuGeJournalEvenement.XuGeJeApplicationRRQ, XuGeTypeEvenement.XuGeTeErreur, ex.Message, ex.Source, ex.StackTrace, "", Nothing)
            Throw ex
        End Try

    End Sub

    Private Function RepertoireExiste(nomFichier As String) As Boolean

        Dim reprt As String = nomFichier.Substring(0, nomFichier.LastIndexOf("\"))
        Dim nomFich As String = nomFichier.Substring(nomFichier.LastIndexOf("\") + 1)
        If Not IO.Directory.Exists(reprt) Then
            XuCuGestionEvent.AjouterEvenmNormalise(XuGeJournalEvenement.XuGeJeApplicationRRQ, XuGeTypeEvenement.XuGeTeAvertissement, "Répertoire n'existe pas : " & reprt, [GetType].Assembly.GetName.ToString, "", "", Nothing)
            Dim message As String = String.Format("Mise à jour du fichier {0} impossible. {1}Le répertoire {2} n'existe pas", nomFich, Chr(13) & Chr(10), reprt)
            MsgBox(message)
            Return False
        End If

        Return True
    End Function

    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	tsCuTrtParmChif.MettreAJourSource
    ''' <summary>
    '''     Fonction de mise à jour des clés de chiffrement.
    ''' </summary>
    ''' <param name="parmtEvn">
    ''' 	Paramètres pour l'ajout de la clé, environnement, type de clé, fichier soruce
    ''' 	Value Type: <see cref="String" />	(System.String)
    ''' </param>
    ''' <returns><see cref="Boolean" />	(System.Boolean)</returns>
    ''' <remarks><para><pre>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2005-02-22	t209376		Création initiale
    ''' 2005-02-28	t209376		Ajout des paramètres strSource et strDestination pour la copie
    ''' 2005-07-13  t209376     Ajout du type de clé
    ''' 2005-07-14  t209376     Retrait des paramètres de source et destination
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    Public Function MettreAJourSource(majInterneCommun As Boolean, majExterneCommun As Boolean, nomFichierServeurExtranet As String, nomFichierServeurInterne As String, nomFichierServeurInforoute As String) As Boolean

        Try
            DsParmt.AcceptChanges()

            ' Obtenir les données XML et le schema du Dataset pour ensuite les chiffrer
            ' dans un fichier (contient tous les types)
            objObtnrParmt.ChiffrementDechiffrementPermis = True

            objObtnrParmt.ChiffrerFichier(nomFichierServeurInterne, DsParmt)

            If majInterneCommun Then
                MettreAJourSourceExterne(nomFichierServeurExtranet)
            End If

            If majExterneCommun Then
                MettreAJourSourceInterne(nomFichierServeurInforoute)
            End If

            objObtnrParmt.ChiffrementDechiffrementPermis = False

            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    ''' <summary>
    ''' Cette méthode est utilisée pour mettre à jour le fichier configuré dans TS6.config, clé "CheminServeurExtranet"
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub MettreAJourSourceExterne(strChemin As String)

        Try
            If strChemin <> "" Then

                If Not RepertoireExiste(strChemin) Then
                    Exit Sub
                End If

                'Créer le dataset de l'Extranet et lui ajouter le record de type Externe
                Dim drsParmtEx As DataRow() = DtParmt.Select()
                Dim dsParmtEx As DataSet = DsParmt.Clone
                For Each drParmtEx As DataRow In drsParmtEx
                    dsParmtEx.Tables(0).ImportRow(drParmtEx)
                Next
                dsParmtEx.AcceptChanges()

                ' Obtenir les données XML et le schema du Dataset pour ensuite les chiffrer
                ' dans un fichier pour l'Extranet
                objObtnrParmt.ChiffrerFichier(strChemin, dsParmtEx)
            Else
                Throw New ApplicationException("Le chemin du fichier Extranet est obligatoire lorsqu'on fait une mise à jour pour le type Externe")
            End If
        Catch ex As Exception
            XuCuGestionEvent.AjouterEvenmNormalise(XuGeJournalEvenement.XuGeJeApplicationRRQ, XuGeTypeEvenement.XuGeTeErreur, ex.Message, ex.Source, ex.StackTrace, "", Nothing)
            Throw ex
        End Try

    End Sub

    ''' <summary>
    ''' Cette méthode est utilisée pour mettre à jour le fichier configuré dans TS6.config, clé "CheminServeurInforoute"
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub MettreAJourSourceInterne(strChemin As String)

        Try
            If strChemin <> "" Then

                If Not RepertoireExiste(strChemin) Then
                    Exit Sub
                End If

                'Créer le dataset de l'Inforoute et lui ajouter le record de type SQAG
                Dim drsParmtInforoute As DataRow() = DtParmt.Select()
                Dim dsParmtInforoute As DataSet = DsParmt.Clone
                For Each drParmtInforoute As DataRow In drsParmtInforoute
                    dsParmtInforoute.Tables(0).ImportRow(drParmtInforoute)
                Next
                dsParmtInforoute.AcceptChanges()

                ' Obtenir les données XML et le schema du Dataset pour ensuite les chiffrer
                ' dans un fichier pour l'Inforoute
                objObtnrParmt.ChiffrerFichier(strChemin, dsParmtInforoute)

            Else
                Throw New ApplicationException("Le chemin du fichier Inforoute est obligatoire lorsqu'on fait une mise à jour pour le type SQAG")
            End If
        Catch ex As Exception
            XuCuGestionEvent.AjouterEvenmNormalise(XuGeJournalEvenement.XuGeJeApplicationRRQ, XuGeTypeEvenement.XuGeTeErreur, ex.Message, ex.Source, ex.StackTrace, "", Nothing)
            Throw ex
        End Try

    End Sub


    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	tsCuTrtParmChif.ObtenirSource
    ''' <summary>
    '''     Fonction de lecture du fichier de clés en fonction du type en paramètre.
    ''' </summary>
    ''' <param name="typeCle">
    ''' </param>
    ''' <returns><see cref="Data.DataTable" />	(System.Data.DataTable)</returns>
    ''' <remarks><para><pre>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2005-02-22	t209376		Création initiale
    ''' 2005-07-12  t209376     Ajout du type de clé
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    Public Function ObtenirSource(typeCle As TsCuParamsChiffrement.TypeCle) As DataRow()
        Dim objRecords As DataRow() = Nothing

        Try
            objRecords = DtParmt.Select("Type='" & typeCle.ToString & "'")
            Return objRecords
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private sourceAjour As Boolean = False

    Public Sub ChargerSource(nomFichierServeurInterne As String)

        If Not sourceAjour Then
            Try
                Try
                    DsParmt.Clear()
                    objObtnrParmt.ChiffrementDechiffrementPermis = True
                    ObtenirValeurs(objObtnrParmt, nomFichierServeurInterne)
                    objObtnrParmt.ChiffrementDechiffrementPermis = False
                Catch e As Exception
                    XuCuGestionEvent.AjouterEvenmNormalise(XuGeJournalEvenement.XuGeJeApplicationRRQ, XuGeTypeEvenement.XuGeTeErreur, e.Message, e.Source, e.StackTrace, "", Nothing)
                End Try
                sourceAjour = True
            Catch ex As Exception
                Throw ex
            End Try
        End If

    End Sub

End Class
