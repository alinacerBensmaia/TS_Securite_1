Imports System.DirectoryServices.Protocols
Imports System.Net
Imports System.DirectoryServices.AccountManagement
Imports TS1N201_DtCdAccGenV1
Imports System.IO
Imports Rrq.Securite.PAM
Imports System.Text.RegularExpressions
Imports Rrq.InfrastructureCommune.Parametres

Public Class TsCaCodAccGen
    Private Const TYPE_DEPOT_AUTRE As String = "AUT"
    Private Const DOMAINE_UPN As String = "retraitequebec.gouv.qc.ca"
    Private Const OUI As String = "O"
    Private adminAD As TsDtCodeUsageMotPasse
    Private adminADLDS As TsDtCodeUsageMotPasse

#Region " Méthodes publiques "

    Public Sub ImporterCles(ByRef pChaineContexte As String)
        Dim regex As Regex = New Regex("^(?<Systeme>[A-Z]{2})(?<SousSys>[0-9DFIWRT])?((Conn|Accs)(?<ConnType>\w{3}))?", RegexOptions.Compiled)

        Using dsFichier As DataSet = TsCuInfoUtil.LireFichierCodeAcces()
            If dsFichier.Tables.Count > 0 Then
                Dim patchDoubleKey As Boolean = False
                For Each row As DataRow In dsFichier.Tables(0).Rows
                    Dim cle As New TsDtCleSym()

                    cle.CoIdnCleSymTs = row("Cle").ToString
                    If cle.CoIdnCleSymTs = "EE1IPRRRQ36" Then
                        If patchDoubleKey Then
                            Continue For
                        Else
                            patchDoubleKey = True
                        End If
                    End If

                    cle.CoEnvCleSymTs = row("Envrn").ToString()
                    If String.IsNullOrWhiteSpace(cle.CoEnvCleSymTs) Then
                        Dim envinfo As String = cle.CoIdnCleSymTs.Substring(cle.CoIdnCleSymTs.Length - 1)
                        cle.CoEnvCleSymTs = Environnements.ParseLettre(envinfo).Code
                    End If

                    'TODO try to handle Dom Ss-Dom and conn type
                    If regex.IsMatch(cle.CoIdnCleSymTs) Then
                        Dim match As Match = regex.Match(cle.CoIdnCleSymTs)
                        cle.CoSysCleSymTs = match.Groups("Systeme").Value
                        cle.CoSouCleSymTs = match.Groups("SousSys").Value
                        Select Case match.Groups("ConnType").Value
                            Case "WF0"
                                cle.CoTypDepCleTs = "WF"
                            Case "WF1"
                                cle.CoTypDepCleTs = "WF"
                            Case "AC0"
                                cle.CoTypDepCleTs = "AC"
                            Case Else
                                cle.CoTypDepCleTs = match.Groups("ConnType").Value
                        End Select
                        If String.IsNullOrWhiteSpace(cle.CoTypDepCleTs) Then
                            cle.CoTypDepCleTs = TYPE_DEPOT_AUTRE
                        End If
                    Else
                        cle.CoSysCleSymTs = "ZZ"
                        cle.CoTypDepCleTs = TYPE_DEPOT_AUTRE
                    End If


                    Select Case row("Type").ToString()
                        Case "H"
                            cle.CoTypCleSymTs = "HOD"
                        Case "D"
                            cle.CoTypCleSymTs = "DOM"
                        Case "I"
                            cle.CoTypCleSymTs = "INF"
                        Case "IV"
                            cle.CoTypCleSymTs = "INV"
                    End Select
                    cle.CoUtlGenCleTs = row("Code").ToString()
                    cle.VlMotPasCleTs = row("Mdp").ToString()

                    cle.DsCleSymTs = row("Desc").ToString()
                    cle.CmCleSymTs = row("Comm").ToString()
                    cle.VlVerCleSymTs = row("CodeVerif").ToString()

                    Dim groupe As New TsDtGroAd '(?<Month>\d{1,2})
                    groupe.NmGroActDirTs = row("Profil").ToString()
                    cle.LsGroAd.Add(groupe)

                    EnregistrerCle(pChaineContexte, cle, True, False, True)
                Next
            End If
        End Using
    End Sub

    ''' <summary>
    ''' Obtient les informations de l'état du fichier d'exporation
    ''' </summary>
    ''' <param name="ChaineContexte"></param>
    ''' <returns>Informations sur l'état du fichier d'exportation</returns>
    Public Shared Function ObtenirEtatFichierExportation(ByRef ChaineContexte As String) As TsDtEtaFicExp
        Dim etat As New TsDtEtaFicExp()
        Dim strFichCdAcc As String = Config.PASSWDFILEPROD

        If File.Exists(strFichCdAcc) Then
            etat.DtDerFicExp = File.GetLastWriteTime(strFichCdAcc)
        Else
            etat.DtDerFicExp = DateTime.MinValue
        End If

        Dim cd As New TsCdCodAccGen()
        etat.DtDerMajBd = cd.ObtenirDateDerniereModification(ChaineContexte)
        etat.InFicJou = etat.DtDerFicExp >= etat.DtDerMajBd

        Return etat
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ChaineContexte"></param>
    ''' <remarks>
    ''' NOTE : Un retour en arrière sur l'exportation de fichiers séparés pour les environnements d'essais a été effectué car
    '''        le système de graduation ne permet actuellement pas de graduer des fichiers différents pour les environnements d'essais.
    '''        Si nous décidons de réactiver cette fonctionnalité, le code entre /*****/ peut être remis en fonction.
    ''' </remarks>
    Public Sub ExporterCles(ByRef ChaineContexte As String)
        Dim listeCles As IList(Of TsDtCleSym) = ObtenirListeCle(ChaineContexte)

        Using dsTrtCodeAcces As DataSet = TsCuInfoUtil.LireFichierCodeAcces()
            If dsTrtCodeAcces.Tables.Contains("CdAcces") Then
                dsTrtCodeAcces.Tables("CdAcces").Clear()
            End If

            RemplirDsTrtCodeAcces(ChaineContexte, listeCles, dsTrtCodeAcces)
            dsTrtCodeAcces.AcceptChanges()

            'Encryption du mot de passe avec certificat de production
            '***CHG1 - Correction problème de timeout lors de l'exportation du fichier.
            'On a plus besoin d'encrypter le fichier, car il est déjà encrypté dans la BD avec le certificat
            'Using dsTrtCodeAccesPROD As DataSet = TsCuInfoUtil.EncrypterMotPasser(dsTrtCodeAcces, Config.NomCertificatPROD, Config.ThumbPrintCertificatPROD)

            Dim accesDonnee As New TsCdCodAccGen()
                accesDonnee.Journaliser(ChaineContexte, "EXP", Nothing, Nothing)

            TsCuInfoUtil.EcrireFichierCodeAccesXML(dsTrtCodeAcces, Config.PASSWDFILEPROD)

            'Écrire le fichier le serveur de publication (LAIA et PRIA) qui n'ont pas de IIS pour faire la lecture avec un compte de service TS 
            If Config.PASSWDFILEPRODPUB IsNot Nothing Then
                TsCuInfoUtil.EcrireFichierCodeAccesXML(dsTrtCodeAcces, Config.PASSWDFILEPRODPUB)
            End If
            'Fichier Libraire
            Using dsLibraire As DataSet = TsCuInfoUtil.CopierDataSet(dsTrtCodeAcces, "Cle IN('" & Config.CleSymboliqueLibraire & "')")
                TsCuInfoUtil.EcrireFichierCodeAccesXML(dsLibraire, Config.PASSWDFILELIBRAIRE)
            End Using

            Using dsZeaXML As DataSet = TsCuFichier.RemplirDsZea(dsTrtCodeAcces) 'Créer et remplir le dataset de la zone d'échange applicative
                'Obtenir les données XML et le schema du Dataset pour ensuite les chiffrer dans un fichier pour l'inforoute
                'Obtenir les données XML et le schema du Dataset pour ensuite les chiffrer dans un fichier pour l'inforoute ZEA
                TsCuInfoUtil.EcrireFichierCodeAccesXML(dsZeaXML, Config.PASSWDFILEZEAPROD) 'Ajout de la zone d'échange applicative
            End Using

            '--------------------------------------------------------------------------------------------
            'Créer le dataset qui contient que les clés de type Inforoute et Inforoute avec vérification
            '--------------------------------------------------------------------------------------------

            Using dsInforouteXML As DataSet = TsCuInfoUtil.CopierDataSet(dsTrtCodeAcces, "Type IN('I','IV')")

                TsCuInfoUtil.EcrireFichierCodeAccesXML(dsInforouteXML, Config.PASSWDFILEINFORTEPROD)

                'Obtenir les données XML et le schema du Dataset pour ensuite les chiffrer dans un fichier pour l'inforoute ZDE
                TsCuInfoUtil.EcrireFichierCodeAccesXML(dsInforouteXML, Config.PASSWDFILEZDEPROD)

            End Using



            'Encryption du mot de passe avec certificat d'unitaire
            Using dsUNIT As DataSet = TsCuInfoUtil.CopierDataSet(TsCuInfoUtil.DecrypterEncrypterMotPasser(dsTrtCodeAcces, Config.NomCertificatUNIT, Config.ThumbPrintCertificatUNIT), "Envrn IN('UNIT','ESSAIS')")
                TsCuInfoUtil.EcrireFichierCodeAccesXML(dsUNIT, Config.PASSWDFILEUNIT)

                Using dsZeaXMLUNIT As DataSet = TsCuFichier.RemplirDsZea(dsUNIT) 'Créer et remplir le dataset de la zone d'échange applicative
                    'Obtenir les données XML et le schema du Dataset pour ensuite les chiffrer dans un fichier pour l'inforoute
                    'Obtenir les données XML et le schema du Dataset pour ensuite les chiffrer dans un fichier pour l'inforoute ZEA
                    TsCuInfoUtil.EcrireFichierCodeAccesXML(dsZeaXMLUNIT, Config.PASSWDFILEZEAUNIT) 'Ajout de la zone d'échange applicative
                End Using

                '--------------------------------------------------------------------------------------------
                'Créer le dataset qui contient que les clés de type Inforoute et Inforoute avec vérification
                '--------------------------------------------------------------------------------------------

                Using dsInforouteXMLUNIT As DataSet = TsCuInfoUtil.CopierDataSet(dsUNIT, "Type IN('I','IV')")
                    TsCuInfoUtil.EcrireFichierCodeAccesXML(dsInforouteXMLUNIT, Config.PASSWDFILEINFORTEUNIT)

                    'Obtenir les données XML et le schema du Dataset pour ensuite les chiffrer dans un fichier pour l'inforoute ZDE
                    TsCuInfoUtil.EcrireFichierCodeAccesXML(dsInforouteXMLUNIT, Config.PASSWDFILEZDEUNIT)

                End Using

            End Using

        End Using

    End Sub

    Public Shared Function ObtenirListeCle(ByRef pChaineContexte As String) As IList(Of TsDtCleSym)
        Dim cd As New TsCdCodAccGen()
        Return cd.ObtenirListeCles(pChaineContexte)
    End Function

    Public Shared Function ObtenirCle(ByRef pChaineContexte As String, ByVal nmCle As String) As TsDtCleSym
        Dim cd As New TsCdCodAccGen()
        Return cd.ObtenirCle(pChaineContexte, nmCle)
    End Function

    Public Shared Function ObtenirCles(ByRef pChaineContexte As String, ByVal nmCleParent As String) As IList(Of TsDtCleSym)
        Dim cd As New TsCdCodAccGen()
        Return cd.ObtenirListeClesParent(pChaineContexte, nmCleParent)
    End Function

    Public Shared Function ObtenirCleRecherche(ByRef pChaineContexte As String,
                                               ByVal pCoTypCle As String,
                                               ByVal pCoTypEnv As String,
                                               ByVal pGroupeAd As String,
                                               ByVal pIdCle As String,
                                               ByVal pUsagerAd As String) As DataTable

        Dim cd As New TsCdCodAccGen()
        Return cd.ObtenirCleRecherche(pChaineContexte,
                                      pCoTypCle,
                                      pCoTypEnv,
                                      pGroupeAd,
                                      pIdCle,
                                      pUsagerAd)
    End Function

    Public Function EnregistrerCle(ByRef ChaineContexte As String,
                                   ByVal CleSymbolique As TsDtCleSym,
                                   ByVal pIndicMdpNouveau As Boolean,
                                   ByVal pIndicMaj As Boolean) As Boolean
        Return EnregistrerCle(ChaineContexte, CleSymbolique, pIndicMdpNouveau, pIndicMaj, False)
    End Function

    Public Shared Function DetruireCle(ByRef ChaineContexte As String, ByVal CleSymbolique As TsDtCleSym) As Boolean
        Dim reg As Regex = New Regex("^[A-Z]{2}[A-Z0-9]", RegexOptions.Compiled)
        Dim result As Boolean = True

        If CleSymbolique.CoIdnCleSymTs IsNot Nothing AndAlso Not String.IsNullOrEmpty(CleSymbolique.CoIdnCleSymTs.Trim()) Then
            Dim accesDonnee As TsCdCodAccGen = New TsCdCodAccGen()

            Dim CodesEnv As String() = CleSymbolique.CoEnvCleSymTs.Split(";"c)
            For Each env As String In CodesEnv
                Dim cle As New TsDtCleSym
                cle.CoEnvCleSymTs = env.Trim()
                Dim CodeIdentification As String = CleSymbolique.CoIdnCleSymTs
                ' Les clés de type 'autre' n'ont pas nécessairement l'environnement, exception faite si il y a plusieurs environnements
                If reg.IsMatch(CleSymbolique.CoIdnCleSymTs) AndAlso (CleSymbolique.CoTypDepCleTs <> TYPE_DEPOT_AUTRE OrElse CodesEnv.Length > 1) AndAlso
                   CodeIdentification.Substring(CodeIdentification.Length - 1) <> cle.CoEnvCleSymTs.LettreEnvironnement Then
                    CodeIdentification &= cle.CoEnvCleSymTs.LettreEnvironnement
                End If
                cle.CoIdnCleSymTs = CodeIdentification
                accesDonnee.LibererGroupe(ChaineContexte, cle)
                If Not accesDonnee.DetruireCle(ChaineContexte, cle) Then
                    result = False
                    Exit For
                Else
                    accesDonnee.Journaliser(ChaineContexte, "SUP", cle, Nothing)
                End If
            Next
        End If

        Return result
    End Function

    ''' <summary>
    ''' Afficher mot de passe
    ''' </summary>
    ''' <param name="cle">Clé symbolique à afficher</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AfficherMDP(ByRef ChaineContexte As String, ByVal cle As TsDtCleSym) As String
        Dim result As String = String.Empty
        If cle IsNot Nothing Then
            result = TsCuInfoUtil.DecrypterMotPasser(Config.NomCertificatPrivate, Config.ThumbPrintCertificatPROD, cle.VlMotPasCleTs)
            Dim accesDonnee As New TsCdCodAccGen()
            accesDonnee.Journaliser(ChaineContexte, "CON", cle, Nothing)
        End If
        Return result
    End Function

    Public Function AfficherMDPCleVecteur(ByRef ChaineContexte As String, ByVal cle As TsDtCleSym) As String
        Dim result As String = String.Empty
        If cle IsNot Nothing Then
            result = TsCuEncryption.Decrypt(cle.VlMotPasCleTs, cle.CoIdnCleSymTs)
            Dim accesDonnee As New TsCdCodAccGen()
            accesDonnee.Journaliser(ChaineContexte, "CON", cle, Nothing)
        End If
        Return result
    End Function

    Public Shared Function ObtenirIndicateursCreationCompte(ByRef ChaineContexte As String) As TsDtIndCreCpt
        ' (si la configuration est absente, on crée les comptes par défaut)
        Const VALEUR_DEFAUT As String = OUI
        Dim retour As New TsDtIndCreCpt()

        ' AD/LDS est dépendant de AD, si AD n'est pas actif, AD/LDS ne doit pas l'être non plus
        retour.InCreCptAdTs = Config.CreerCompteAD
        retour.InCreCptLdsTs = (retour.InCreCptAdTs AndAlso Config.CreerCompteADLDS)

        Return retour
    End Function

#End Region

#Region " Méthodes privées "

    ''' <summary>
    ''' Remplir le dataset des code d'accès
    ''' </summary>
    ''' <param name="ChaineContexte">Chaine de contexte</param>
    ''' <param name="pListeCles">Liste de clés</param>
    ''' <param name="pDsTrtCodeAcces">Dataset de code d'accès à remplir</param>
    ''' <remarks></remarks>
    Private Sub RemplirDsTrtCodeAcces(ByRef ChaineContexte As String, ByVal pListeCles As IList(Of TsDtCleSym), ByVal pDsTrtCodeAcces As DataSet)
        For Each cleProxy As TsDtCleSym In pListeCles
            Dim cle As TsDtCleSym = ObtenirCle(ChaineContexte, cleProxy.CoIdnCleSymTs)
            Dim row As DataRow = pDsTrtCodeAcces.Tables("CdAcces").NewRow

            row("Cle") = cle.CoIdnCleSymTs
            Select Case cle.CoTypCleSymTs
                Case "HOD"
                    row("Type") = "H"
                Case "DOM"
                    row("Type") = "D"
                Case "INF"
                    row("Type") = "I"
                Case "INV"
                    row("Type") = "IV"
            End Select
            row("Code") = cle.CoUtlGenCleTs
            If Not String.IsNullOrWhiteSpace(cle.VlMotPasCleTs) Then
                '***CHG1 - Correction problème de timeout lors de l'exportation du fichier.
                'Comme le mot de passe est déjà chiffré avec le bon certificat, on ne fait que le garder tel quel, sans déchiffrer 
                'row("Mdp") = TsCuInfoUtil.DecrypterMotPasser(Config.NomCertificatPrivate, cle.VlMotPasCleTs)
                row("Mdp") = cle.VlMotPasCleTs
            End If
            row("Desc") = cle.DsCleSymTs
            row("Comm") = cle.CmCleSymTs
            row("CodeVerif") = cle.VlVerCleSymTs
            Select Case cle.CoEnvCleSymTs
                Case "ESSA"
                    row("Envrn") = "ESSAIS"
                Case Else
                    row("Envrn") = cle.CoEnvCleSymTs
            End Select

            row("Profil") = cle.LsGroAd(0).NmGroActDirTs

            pDsTrtCodeAcces.Tables("CdAcces").Rows.Add(row)
            If cle.CoIdnCleSymTs = "EE1IPRRRQ36" Then
                'Patch pour générer une clé en double dans le résultat et maintenir cette clé qu'il n'est pas possible d'avoir dans la BD
                Dim row2 As DataRow = pDsTrtCodeAcces.Tables("CdAcces").NewRow
                row2.ItemArray = row.ItemArray
                row2("Type") = "D"
                row2("Profil") = "TS1IPRRRQ36"
                pDsTrtCodeAcces.Tables("CdAcces").Rows.Add(row2)
            End If
        Next
    End Sub

    Private Function EnregistrerCle(ByRef pChaineContexte As String,
                                    ByVal CleSymbolique As TsDtCleSym,
                                    ByVal pIndicMdpNouveau As Boolean,
                                    ByVal pIndicMaj As Boolean,
                                    ByVal pIndicIgnoreNom As Boolean) As Boolean

        If pIndicMaj Then
            'Update
            Return enregistrerCleEnUpdate(pChaineContexte, CleSymbolique, pIndicMdpNouveau)
        Else
            'Insert
            Return enregistrerCleEnInsert(pChaineContexte, CleSymbolique, pIndicIgnoreNom)
        End If
    End Function

    Private Function enregistrerCleEnUpdate(ByRef pChaineContexte As String, ByVal CleSymbolique As TsDtCleSym, ByVal pIndicMdpNouveau As Boolean) As Boolean
        Dim accesDonnee As New TsCdCodAccGen()

        For Each env As String In CleSymbolique.CoEnvCleSymTs.Split(";"c)
            Dim cle As New TsDtCleSym()
            cle.CoEnvCleSymTs = env.Trim()
            cle.CoIdnCleSymTs = CleSymbolique.CoIdnCleSymTs
            cle.CoSysCleSymTs = CleSymbolique.CoSysCleSymTs
            cle.CoSouCleSymTs = CleSymbolique.CoSouCleSymTs
            cle.CmCleSymTs = CleSymbolique.CmCleSymTs
            cle.DsCleSymTs = CleSymbolique.DsCleSymTs
            cle.CoTypDepCleTs = CleSymbolique.CoTypDepCleTs
            cle.CoTypCleSymTs = CleSymbolique.CoTypCleSymTs
            cle.CoUtlGenCleTs = CleSymbolique.CoUtlGenCleTs
            cle.VlVerCleSymTs = CleSymbolique.VlVerCleSymTs
            If pIndicMdpNouveau Then
                cle.VlMotPasCleTs = TsCuInfoUtil.EncrypterMotPasser(Config.NomCertificatPROD, Config.ThumbPrintCertificatPROD, CleSymbolique.VlMotPasCleTs)
            Else
                'Quand l'utilisateur consulte la clé symbolique, elle est déchiffé et rendu ici, on l'inscrit sans le chiffrement.
                'Pour éviter ça, on s'assurer que le mot de passe n'a pas plus de 62 caractères. Si c'est le cas, il est déjà chiffré, on peut l'enregistré tel quel.
                'Sinon, on doit le chiffré avant.
                If CleSymbolique.VlMotPasCleTs.Length > 62 Then
                    cle.VlMotPasCleTs = CleSymbolique.VlMotPasCleTs
                Else
                    cle.VlMotPasCleTs = TsCuInfoUtil.EncrypterMotPasser(Config.NomCertificatPROD, Config.ThumbPrintCertificatPROD, CleSymbolique.VlMotPasCleTs)
                End If
            End If
            cle.LsGroAd = CleSymbolique.LsGroAd


            ' Obtenir la clé avant modification pour la journalisation avant de procéder avec modification
            Dim cleAvant As TsDtCleSym = accesDonnee.ObtenirCle(pChaineContexte, cle.CoIdnCleSymTs)
            If Not accesDonnee.ModifierCle(pChaineContexte, cle) Then
                Return False
            Else
                accesDonnee.Journaliser(pChaineContexte, "MOD", cleAvant, cle)
            End If
            TsCuGroupe.GererGroupe(pChaineContexte, cle, accesDonnee)
        Next

        Return True
    End Function

    Private Function enregistrerCleEnInsert(ByRef pChaineContexte As String, ByVal CleSymbolique As TsDtCleSym, ByVal pIndicIgnoreNom As Boolean) As Boolean
        Dim regEx As New Regex("^[A-Z]{2}[A-Z0-9]", RegexOptions.Compiled)
        Dim accesDonnee As New TsCdCodAccGen()


        'On va chercher le code usage et mot de passe pour la conexion LDAP à l'AD
        Dim demandeMotPasse As New TsCuDemndRecprMotPasse()
        adminAD = demandeMotPasse.ObtenirCodeUsagerMotPasse(Config.CompteAD)
        adminADLDS = demandeMotPasse.ObtenirCodeUsagerMotPasse(Config.CompteADLDS)

        For Each env As String In CleSymbolique.CoEnvCleSymTs.Split(";"c)
            Dim e As Environnements = Environnements.ParseCode(env.Trim())

            If Not e.Est(Environnements.Tous) Then
                Dim cle As New TsDtCleSym()
                cle.CoEnvCleSymTs = e.Code

                Dim CodeIdentification As String = CleSymbolique.CoIdnCleSymTs
                If regEx.IsMatch(CleSymbolique.CoIdnCleSymTs) _
                    AndAlso Not pIndicIgnoreNom _
                    AndAlso (CleSymbolique.CoTypDepCleTs <> TYPE_DEPOT_AUTRE OrElse CleSymbolique.InAjtEnv) _
                    AndAlso CodeIdentification.Substring(CodeIdentification.Length - 1) <> cle.CoEnvCleSymTs.LettreEnvironnement Then

                    CodeIdentification &= cle.CoEnvCleSymTs.LettreEnvironnement
                End If
                cle.CoIdnCleSymTs = CodeIdentification

                cle.CoSysCleSymTs = CleSymbolique.CoSysCleSymTs
                cle.CoSouCleSymTs = CleSymbolique.CoSouCleSymTs
                cle.CmCleSymTs = CleSymbolique.CmCleSymTs
                cle.DsCleSymTs = CleSymbolique.DsCleSymTs
                cle.CoTypCleSymTs = CleSymbolique.CoTypCleSymTs
                cle.CoTypDepCleTs = CleSymbolique.CoTypDepCleTs
                cle.VlVerCleSymTs = CleSymbolique.VlVerCleSymTs

                cle.VlMotPasCleTs = TsCuInfoUtil.EncrypterMotPasser(Config.NomCertificatPROD, Config.ThumbPrintCertificatPROD, TsCuGroupe.GererMultiple(CleSymbolique.VlMotPasCleTs, e.Code))
                cle.CoUtlGenCleTs = TsCuGroupe.GererMultiple(CleSymbolique.CoUtlGenCleTs, e.Code)
                cle.LsGroAd = TsCuGroupe.GererMultipleGroupe(CleSymbolique.LsGroAd, e.Code)

                If Not accesDonnee.InsertCle(pChaineContexte, cle) Then
                    Return False
                Else
                    accesDonnee.Journaliser(pChaineContexte, "AJT", Nothing, cle)
                End If

                TsCuGroupe.GererGroupe(pChaineContexte, cle, accesDonnee)

                If CleSymbolique.StIndCreCpt IsNot Nothing Then
                    creerComptesDansDepotsDeSecurite(pChaineContexte, CleSymbolique, cle)
                End If
            End If
        Next

        Return True
    End Function

    Private Sub creerComptesDansDepotsDeSecurite(ByRef pChaineContexte As String, ByVal CleSymbolique As TsDtCleSym, cle As TsDtCleSym)
        Const VALEUR_DEFAUT As String = OUI

        ' Créer les comptes AD et TSS, si la configuration proscrit la création, omettre cette étape
        ' (si la configuration est absente, on crée les comptes par défaut)
        Dim doitCreerCompteAD As Boolean = Config.CreerCompteAD

        If CleSymbolique.StIndCreCpt.InCreCptAdTs AndAlso doitCreerCompteAD Then
            CreerCompteAD(pChaineContexte, cle)

            ' Les comptes AD/LDS peuvent être créés uniquement si le compte AD correspondant a été créé
            Dim doitCreerCompteADLDS As Boolean = Config.CreerCompteADLDS
            If CleSymbolique.StIndCreCpt.InCreCptLdsTs AndAlso doitCreerCompteADLDS Then
                CreerCompteADLDS(pChaineContexte, cle)
            End If
        End If

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="pChaineContexte"></param>
    ''' <param name="nouveauCompte"></param>
    Private Sub CreerCompteAD(ByRef pChaineContexte As String, ByVal nouveauCompte As TsDtCleSym)
        If Not typeConnexionAssocie("TypesConnexionLdapAD", nouveauCompte.CoTypDepCleTs) Then Return
        If CompteContientDomaine(nouveauCompte) And Not EstCompteDomaineRQ(nouveauCompte) Then Return

        Try
            Dim container As String = Config.ContainerAD
            Dim domaine As String = Config.DomaineAD

            'mot de passe du nouveau compte
            Dim motPasse As String = TsCuInfoUtil.DecrypterMotPasser(Config.NomCertificatPrivate, Config.ThumbPrintCertificatPROD, nouveauCompte.VlMotPasCleTs)

            'création du nouveau compte dans le domaine RQ
            creerCompte(adminAD, domaine, container, nouveauCompte, motPasse)

        Catch ex As PrincipalExistsException
            ' Si le compte existe déjà, on ignore cette opération
        End Try
    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nouveauCompte"></param>
    Private Function CompteContientDomaine(ByVal nouveauCompte As TsDtCleSym) As Boolean
        If nouveauCompte.CoUtlGenCleTs.Split(CType("\", Char())).Count > 1 Then
            Return True
        End If
        Return False
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nouveauCompte"></param>
    Private Function EstCompteDomaineRQ(ByVal nouveauCompte As TsDtCleSym) As Boolean
        If nouveauCompte.CoUtlGenCleTs.ToUpper.Trim.StartsWith("RQ\") Then
            Return True
        End If
        Return False
    End Function


    Private Sub creerCompte(admin As TsDtCodeUsageMotPasse, domaine As String, container As String, nouveauCompte As TsDtCleSym, motPasse As String)
        Using pc As New PrincipalContext(ContextType.Domain, domaine, container, admin.CodeUsager, admin.MotPasse)
            Using up As New UserPrincipal(pc)

                With nouveauCompte
                    'Création de l'utilisateur
                    up.Name = .CoUtlGenCleTs.Substring(nouveauCompte.CoUtlGenCleTs.IndexOf("\") + 1)
                    up.DisplayName = .CoUtlGenCleTs.Substring(nouveauCompte.CoUtlGenCleTs.IndexOf("\") + 1)
                    up.SamAccountName = .CoUtlGenCleTs.Substring(nouveauCompte.CoUtlGenCleTs.IndexOf("\") + 1)
                    up.UserPrincipalName = String.Format("{0}@{1}", .CoUtlGenCleTs.Substring(nouveauCompte.CoUtlGenCleTs.IndexOf("\") + 1), DOMAINE_UPN)
                    up.SetPassword(motPasse)
                    up.Description = ObtenirChaineNomComplet(nouveauCompte)
                    up.PasswordNeverExpires = True
                    up.UserCannotChangePassword = True
                    up.Enabled = True
                    up.PasswordNotRequired = False
                End With

                up.Save()
            End Using
        End Using
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="pChaineContexte"></param>
    ''' <param name="CleSymbolique"></param>
    Private Sub CreerCompteADLDS(ByRef pChaineContexte As String, ByVal CleSymbolique As TsDtCleSym)
        If Not typeConnexionAssocie("TypesConnexionLdapADLDS", CleSymbolique.CoTypDepCleTs) Then Return

        Try
            Dim compteCnx As String = adminADLDS.CodeUsager
            Dim motDePasseCnx As String = adminADLDS.MotPasse

            Dim domaine As String = Config.DomaineADLDS
            Dim container As String = Config.ContainerADLDS

            Using ContexteConn As New PrincipalContext(ContextType.ApplicationDirectory, domaine, container, compteCnx, motDePasseCnx)
                Using Utilisateur As New UserPrincipal(ContexteConn)
                    'Création de l'utilisateur
                    Utilisateur.Name = CleSymbolique.CoUtlGenCleTs.Substring(CleSymbolique.CoUtlGenCleTs.IndexOf("\") + 1)
                    Utilisateur.UserPrincipalName = CleSymbolique.CoUtlGenCleTs.Substring(CleSymbolique.CoUtlGenCleTs.IndexOf("\") + 1)
                    Utilisateur.Description = ObtenirChaineNomComplet(CleSymbolique)

                    Utilisateur.Save()
                End Using
            End Using

        Catch ex As PrincipalExistsException
            ' Si le compte existe déjà, on ignore cette opération
        End Try
    End Sub



    ''' <summary>
    ''' Vérifie si un compte doit être créé pour le type de connexion spécifié
    ''' </summary>
    ''' <param name="CleConfiguration">
    ''' Clé de configuration qui contient les types de connexions qui requièrent un la création d'un compte
    ''' (nom de clé seulement, sans le chemin d'accès)
    ''' </param>
    ''' <param name="TypeConnexion">Type de connexion à valider</param>
    ''' <returns>Vrai si le type de connexion requiert que l'on crée un compte</returns>
    Private Function typeConnexionAssocie(ByVal CleConfiguration As String, ByVal TypeConnexion As String) As Boolean
        Dim valeurConfig As String = Config.ObtenirValeurDe(CleConfiguration)
        If String.IsNullOrEmpty(valeurConfig) Then Return False

        Dim listeTypeConnexion() As String = valeurConfig.Split(","c)
        Return listeTypeConnexion.Contains(TypeConnexion)
    End Function

    ''' <summary>
    ''' Construit la chaîne qui servira de nom complet ou de description pour les comptes d'AD et de TSS
    ''' </summary>
    ''' <param name="CleSymbolique">Informations de la clé symbolique pour laquelle on veut obtenir le nom complet</param>
    ''' <returns>Chaine représentant le nom complet</returns>
    Private Function ObtenirChaineNomComplet(ByVal CleSymbolique As TsDtCleSym) As String
        With CleSymbolique
            Return String.Concat(.CoSysCleSymTs, .CoSouCleSymTs, " Connexion ", .CoTypDepCleTs, " ", ObtenirNomCompletEnv(.CoEnvCleSymTs))
        End With
    End Function

    ''' <summary>
    ''' Obtient le nom complet d'un environnement à partir de son code (EX : UNIT = Unitaire)
    ''' </summary>
    ''' <param name="codeEnv">Code de l'environnement</param>
    ''' <returns>Nom complet de l'environnement</returns>
    Private Function ObtenirNomCompletEnv(ByVal codeEnv As String) As String
        Return Environnements.ParseCode(codeEnv).Description
    End Function

#End Region

End Class