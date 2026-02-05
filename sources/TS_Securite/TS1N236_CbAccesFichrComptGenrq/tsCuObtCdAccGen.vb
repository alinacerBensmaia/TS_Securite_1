Imports System.Collections.Generic
Imports System.Text
Imports System.Security.Principal
Imports Rrq.Securite.Applicative
Imports Rrq.InfrastructureCommune.Parametres
Imports Rrq.InfrastructureCommune.UtilitairesCommuns
Imports System.Reflection

''' <summary>
''' Elle nous permet d'obtenir le code d'accès et le mot de passe de la clé 
''' symbolique désirée.
''' Également, elle nous permet d'obtenir la liste des clés accessibles pour 
''' l'utilisateur courant.  Cette fonction est surtout utilisée par l'interface
''' de requête interactive.
''' De plus, une fonction est disponible pour nous retourner la liste des clés 
''' symboliques qui correspondent à des critères bien définis.
''' </summary>
''' <remarks>
''' Historique des modifications: 
''' --------------------------------------------------------------------------------
''' Date		Nom			Description
''' --------------------------------------------------------------------------------
''' 2007-11-26	T206500		Normaliser cette classe
'''                         -  Seulement la classe TsCuObtCdAcGen est conservée dans 
'''                         ce fichier.  Chaque classe est maintenant isolée dans son
'''                         propre fichier.
'''                         -  On obtient les paramètres de configuration directement
'''                         de l'utilitaire XU4N011_Configuration
'''                         -  Tous les accès sont maintenant validés dans la méthode
'''                         ValiderAcces.   On n'accède plus l'active directory 2 fois
'''                         -  Ramener toutes les méthodes privées qui sont seulement
'''                         utilisées par cette classe.
''' 
''' 2010-01-21  t206500    Utiliser la méthode ObtenirCodeUsager pour obtenir le code
'''                        utilisateur. On fait une exception lorsque c'est le compte SYSTEM. 
''' --------------------------------------------------------------------------------
''' </remarks>
<Microsoft.VisualBasic.ComClass(tsCuObtCdAccGen.ClassId, tsCuObtCdAccGen.InterfaceId, tsCuObtCdAccGen.EventsId)>
Public Class tsCuObtCdAccGen
    Private objInfoUtil As New tsCuInfoUtil
    Private _assemblyCaller As Reflection.Assembly = System.Reflection.Assembly.GetAssembly(Me.GetType)
    Private _UserDomaineZE As String = String.Empty

#Region "*-----      COM GUIDs        -----*"

    ' These  GUIDs provide the COM identity for this class and its COM interfaces. If you change them, existing clients will no longer be able to access the class.
    Public Const ClassId As String = "5B42EB66-30D7-46C8-9C06-9FD569D1EF7B"
    Public Const InterfaceId As String = "8C3ABE28-4FA5-4C4B-A462-D01210FE16DE"
    Public Const EventsId As String = "2B9DCE07-DD9F-4B8B-8E29-B36262C8881F"


#End Region

#Region "*-----   Méthodes publiques   -----*"

    ''' <summary>
    ''' Obtenir le code accès et le mot de passe de la clé symbolique voulue.
    ''' La clé symbolique est recherchée dans le champ 'Cle'.   Depuis la normalisation
    ''' des clés symboliques, on doit utiliser cette méthode.
    ''' </summary>
    ''' <param name="strCle">
    ''' 	Nom de la clé symbolique.
    ''' 	Value Type: string
    ''' </param>
    ''' <param name="strRaison">
    ''' 	La raison de la demande.  Elle est obligatoire.  On la retrouve dans l'EventLog
    ''' 	Value Type: string
    ''' </param>
    ''' <param name="strCompte">
    ''' 	Le code d'accès correspondant à la clé symbolique demandée.  Si l'usager a les 
    '''     droits nécessaires, le code d'accès y est inscrit.
    ''' 	Value Type: string
    ''' </param>
    ''' <param name="strMDP">
    ''' 	Le mot de passe du code d'accès correspondant à la clé symbolique demandée.  
    '''     Si l'usager a les droits nécessaires, le mot de passe y est inscrit.
    ''' 	Value Type: string
    ''' </param> 
    ''' <remarks>
    ''' Historique des modifications: 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' --------------------------------------------------------------------------------
    ''' 2007-11-26	t206500		Normaliser la méthode
    '''                         -  On appelle une méthode privée en lui transférant un code
    '''                         de vérification à blanc.  On évite de répéter le code.  
    '''                         Également, on évite de conserver un état du code de 
    '''                         vérification qui aurait pu nous causer des problèmes.
    '''                         -  Regrouper l'initialisation des valeurs lorsqu'une erreur
    '''                         est générée
    ''' --------------------------------------------------------------------------------
    ''' </remarks>
    Public Sub ObtenirCodeAccesMotDePasse(ByVal strCle As String, ByVal strRaison As String, ByRef strCompte As String, ByRef strMDP As String)
        Dim objJournalisation As New tsCuJournalisation()

        Try

            'Si l'appel à TS est fait à partir de la présentation (TS1N233_CIACCESSEURWCF) ou
            'Si l'appel à TS a été fait à partir d'un pool différent de TS (TS1N235_CSACCESSEURSERVICEAOS), on obtient le vrai code usager demandeur
            If AssemblyCaller.GetName.Name.ToUpper.Equals("TS1N233_CIACCESSEURWCF") Or AssemblyCaller.GetName.Name.ToUpper.Equals("TS1N235_CSACCESSEURSERVICEAOS") Then
                If String.IsNullOrWhiteSpace(UserDomaineZE) Then
                    ObtenirInfoCodeAccesMotPasse(strCle, strRaison, objInfoUtil.ObtenirCodeUsagerDemandeur, strCompte, strMDP)
                Else
                    Dim strDomCodeUtil As String() = UserDomaineZE.Split("\".ToCharArray())
                    ObtenirInfoCodeAccesMotPasse(strCle, strRaison, strDomCodeUtil(1), strCompte, strMDP)
                End If
            Else
                    ObtenirInfoCodeAccesMotPasse(strCle, strRaison, objInfoUtil.ObtenirCodeUsager, strCompte, strMDP)
            End If

        Catch exRO As TsCuRaisonObligatoire
            exRO.AssignerRaison(strCompte, strMDP)
            Throw

        Catch exDI As TsCuDroitsInsuffisants
            exDI.AssignerRaison(strCompte, strMDP)
            Throw

        Catch exCVA As TsCuCodeVerifAbsent
            exCVA.AssignerRaison(strCompte, strMDP)
            objJournalisation.EcrireJournal(exCVA)
            Throw

        Catch exCVI As TsCuCodeVerifInvalide
            exCVI.AssignerRaison(strCompte, strMDP)
            objJournalisation.EcrireJournal(exCVI)
            Throw

        Catch exCI As TsCuCodeInexistant
            exCI.AssignerRaison(strCompte, strMDP)
            objJournalisation.EcrireJournal(exCI)
            Throw

        Catch exRM As TsCuResultatMultiple
            exRM.AssignerRaison(strCompte, strMDP)
            objJournalisation.EcrireJournal(exRM)
            Throw

        Catch ex As Exception
            strCompte = "<inexistant>"
            strMDP = "<aucun>"
            objJournalisation.EcrireJournal(ex)
            Throw

        Finally
            objJournalisation = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' Obtenir le code accès et le mot de passe de la clé symbolique voulue.
    ''' La clé symbolique est recherchée dans le champ 'Cle'.   Depuis la normalisation
    ''' des clés symboliques, on doit utiliser cette méthode.
    ''' </summary>
    ''' <param name="strCle">
    ''' 	Nom de la clé symbolique.
    ''' 	Value Type: string
    ''' </param>
    ''' <param name="strRaison">
    ''' 	La raison de la demande.  Elle est obligatoire.  On la retrouve dans l'EventLog
    ''' 	Value Type: string
    ''' </param>
    ''' <param name="strCompte">
    ''' 	Le code d'accès correspondant à la clé symbolique demandée.  Si l'usager a les 
    '''     droits nécessaires, le code d'accès y est inscrit.
    ''' 	Value Type: string
    ''' </param>
    ''' <param name="strMDP">
    ''' 	Le mot de passe du code d'accès correspondant à la clé symbolique demandée.  
    '''     Si l'usager a les droits nécessaires, le mot de passe y est inscrit.
    ''' 	Value Type: string
    ''' </param> 
    ''' <remarks>
    ''' Historique des modifications: 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' --------------------------------------------------------------------------------
    ''' 2007-11-26	t206500		Normaliser la méthode
    '''                         -  On appelle une méthode privée en lui transférant un code
    '''                         de vérification à blanc.  On évite de répéter le code.  
    '''                         Également, on évite de conserver un état du code de 
    '''                         vérification qui aurait pu nous causer des problèmes.
    '''                         -  Regrouper l'initialisation des valeurs lorsqu'une erreur
    '''                         est générée
    ''' --------------------------------------------------------------------------------
    ''' </remarks>
    Public Sub ObtenirCodeAccesMotDePasseLibraire(ByVal strCle As String, ByVal strRaison As String, ByRef strCompte As String, ByRef strMDP As String)
        Dim objJournalisation As New tsCuJournalisation()
        Dim EmpruntIdent As XuCuEmpruntIdent = Nothing
        Try

            Dim strCodeUsager As String = String.Empty

            If AssemblyCaller.GetName.Name.ToUpper.Equals("TS1N233_CIACCESSEURWCF") Or AssemblyCaller.GetName.Name.ToUpper.Equals("TS1N235_CSACCESSEURSERVICEAOS") Then
                'Si l'appel à TS est fait à partir de la présentation (TS1N233_CIACCESSEURWCF) ou
                'Si l'appel à TS a été fait à partir d'un pool différent de TS (TS1N235_CSACCESSEURSERVICEAOS), on obtient le vrai code usager demandeur
                strCodeUsager = objInfoUtil.ObtenirCodeUsagerDemandeur
            Else
                strCodeUsager = objInfoUtil.ObtenirCodeUsager
            End If

            ObtenirInfoCodeAccesMotPasseLibraire(strCodeUsager, strCompte, strMDP)

            Using InfrastructureCommune.UtilitairesCommuns.XuCuEmpruntIdent.DebuterEmpruntCompte(strCompte, strMDP)

                ObtenirInfoCodeAccesMotPasse(strCle, strRaison, strCodeUsager, strCompte, strMDP)

            End Using

        Catch exRO As TsCuRaisonObligatoire
            exRO.AssignerRaison(strCompte, strMDP)
            Throw

        Catch exDI As TsCuDroitsInsuffisants
            exDI.AssignerRaison(strCompte, strMDP)
            Throw

        Catch exCVA As TsCuCodeVerifAbsent
            exCVA.AssignerRaison(strCompte, strMDP)
            objJournalisation.EcrireJournal(exCVA)
            Throw

        Catch exCVI As TsCuCodeVerifInvalide
            exCVI.AssignerRaison(strCompte, strMDP)
            objJournalisation.EcrireJournal(exCVI)
            Throw

        Catch exCI As TsCuCodeInexistant
            exCI.AssignerRaison(strCompte, strMDP)
            objJournalisation.EcrireJournal(exCI)
            Throw

        Catch exRM As TsCuResultatMultiple
            exRM.AssignerRaison(strCompte, strMDP)
            objJournalisation.EcrireJournal(exRM)
            Throw

        Catch ex As Exception
            strCompte = "<inexistant>"
            strMDP = "<aucun>"
            objJournalisation.EcrireJournal(ex)
            Throw

        Finally
            objJournalisation = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' Obtenir la liste des clés accessibles par l'utilisateur.  Cette méthode est utilisée 
    ''' par l'interface de requête interactive.
    ''' Elle retourne un datarow contenant toutes les informations des clés symboliques
    ''' accessibles.
    ''' </summary>
    ''' <param name="strTri">
    ''' 	L'ordre de tri que l'on désire obtenir l'information.
    ''' 	Value Type: string
    ''' </param>
    ''' <returns>
    '''     un datarow qui contient toutes les informations relatives aux clés symboliques
    '''     que l'utilisateur a accès.
    ''' </returns>
    ''' <remarks>
    ''' Historique des modifications: 
    ''' </remarks>
    Public Function ObtenirClesAccessible(ByVal strTri As String) As DataRow()
        Try
            Dim ts As New TsCaVerfrSecrtApplicative()
            Dim groupes As IList(Of TsDtGroupe) = ts.ObtenirGroupeUtilisateur(WindowsIdentity.GetCurrent())

            Dim memberOf As New StringBuilder()
            Dim delimiter As String = String.Empty
            For Each groupe As TsDtGroupe In groupes
                memberOf.AppendFormat("{0}'{1}'", delimiter, groupe.NmGrpSec.Replace("'", "''"))
                delimiter = ","
            Next

            If memberOf.Length > 0 Then
                Using dsClesAccess As DataSet = objInfoUtil.LireFichierCodeAcces()

                    Dim where As String = String.Format("Profil IN ({0})", memberOf.ToString())
                    Return dsClesAccess.Tables("CdAcces").Select(where, strTri)
                End Using
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Dim objJournalisation As New tsCuJournalisation()
            objJournalisation.EcrireJournal(ex)
            Throw
        End Try
    End Function

    ''' <summary>
    ''' Obtenir la liste des clés symboliques répondant à des critères bien précis.
    ''' Elle retourne un datarow contenant toutes les informations des clés symboliques.
    ''' Seulement les administrateurs des codes accès génériques ont accès à cette méthode.
    ''' 
    ''' PROBLÈME RENCONTRÉ.....
    ''' Cette méthode a déjà été appelée par le composant TS5N131_ZpComPlus pour obtenir 
    ''' la liste des clés symboliques 'TS5%'.
    ''' Cela nous a causé des problèmes, car dans l'inforoute, ceux qui utilisent cet outil
    ''' ne sont pas dans le groupe de sécurité.  Donc, cet outil n'utilisera plus cette méthode
    ''' Il utilisera la méthode ObtenirListeCleTS5 à partir de septembre 2009...
    ''' </summary>
    ''' <param name="strClause">
    ''' 	Les critères de sélection qui nous permettent d'extraire les clés symboliques.
    ''' 	Value Type: string
    ''' </param>
    ''' <returns>
    '''     un datarow qui contient toutes les informations relatives aux clés symboliques
    '''     correspondant aux critères désirés.
    ''' </returns>
    ''' <remarks>
    ''' Historique des modifications: 
    ''' </remarks>
    Public Function ObtenirListeRecherchee(ByVal strClause As String) As DataRow()
        Try

            Dim strCleAcces As String = String.Empty.PadLeft(10)
            Dim strCodeUsager As String = objInfoUtil.ObtenirCodeUsager

            If objInfoUtil.ValiderAdmin(strCodeUsager, strCleAcces) Then
                Using dsListeRech As DataSet = objInfoUtil.LireFichierCodeAcces()
                    Return dsListeRech.Tables("CdAcces").Select(strClause)
                End Using
            Else
                Throw New TsCuDroitsInsuffisants(strCodeUsager)
            End If

        Catch ex As Exception
            Dim objJournalisation As New tsCuJournalisation()
            objJournalisation.EcrireJournal(ex)
            Throw
        End Try
    End Function

    ''' <summary>
    ''' Obtenir la liste des clés symboliques TS5.
    ''' Elle retourne un dataTable contenant seulement la clé et le code des clés symboliques TS5%.
    ''' Cette méthode est appelée par TS5N131_ZpComPlus.
    ''' </summary>
    ''' <returns>
    '''     DataTable qui contient la clé et le code des clés symboliques TS5%.
    ''' </returns>
    ''' <remarks>
    ''' Historique des modifications: 
    ''' </remarks>
    Public Function ObtenirListeCleTS5() As DataTable
        Try
            Dim zones As Zones = Zone.GetCurrents()

            ' Obtenir les données
            Using dsListeRech As DataSet = objInfoUtil.LireFichierCodeAcces()

                ' Sélectionner que les lignes qui correspondent au critère
                Dim where As String = String.Format("Type IN ({0}) and Cle like '%TS5%'", zones.ToStringList())
                Dim drListeRech As DataRow() = dsListeRech.Tables("CdAcces").Select(where)

                ' Création d'un nouveau DataTable, car on ne retourne pas toutes les colonnes
                Using dtCleTS5 As New DataTable()
                    dtCleTS5.Columns.Add("Cle")
                    dtCleTS5.Columns.Add("Code")

                    ' Copier les données nécessaires dans le nouveau datatable
                    For Each rowListeRech As DataRow In drListeRech
                        dtCleTS5.AjouterLigneTS5From(rowListeRech)
                    Next

                    Return dtCleTS5
                End Using
            End Using

        Catch ex As Exception
            Dim objJournalisation As New tsCuJournalisation()
            objJournalisation.EcrireJournal(ex)
            Throw
        End Try
    End Function

    Public Property AssemblyCaller() As Reflection.Assembly
        Get
            Return _assemblyCaller
        End Get
        Set(ByVal value As Reflection.Assembly)
            _assemblyCaller = value
        End Set
    End Property

    Public Property UserDomaineZE() As String
        Get
            Return _UserDomaineZE
        End Get
        Set(ByVal value As String)
            'On s'assure que c'est juste le CI qui remplisse cette information 
            'On s'assure que c'est un compte du domaine ZE qui a été reçu en paramètre
            If Assembly.GetCallingAssembly.GetName.Name.ToUpper.Equals("TS1N233_CIACCESSEURWCF") And value.StartsWith("ZERRQ") Then
                _UserDomaineZE = value
            End If
        End Set
    End Property

#End Region

#Region "*-----    Méthodes privées   -----*"

    ''' <summary>
    ''' Obtenir le code accès et le mot de passe de la clé symbolique demandée.  
    ''' Si l'usager a les accès nécessaires, on retourne le code accès et le mot de passe.
    ''' </summary>
    ''' <param name="strCle">
    ''' 	Nom de la clé symbolique.  Dans ce cas, elle correspond plutôt à une description
    ''' 	Value Type: string
    ''' </param>
    ''' <param name="strRaison">
    ''' 	Raison de la demande d'information pour la clé symbolique.  On retrouve la raison
    '''     dans l'EventLog.  
    ''' 	Value Type: string
    ''' </param> 
    ''' <param name="strCodeVerif">
    ''' 	Le code de vérification qui nous permet d'offrir une validation supplémentaire
    '''     avant de retourner le code d'accès.  On utilise ce code de vérification 
    '''     seulement dans le type "Inforoute avec Vérification"
    ''' 	Value Type: string
    ''' </param>
    ''' <param name="strCompte">
    ''' 	On y retourne le code accès de la clé symbolique demandée.
    ''' 	Value Type: string
    ''' </param>
    ''' <param name="strMDP">
    ''' 	On y retourne le mot de passe de la clé symbolique demandée.
    ''' 	Value Type: string
    ''' </param>
    ''' <remarks>
    ''' Historique des modifications: 
    ''' </remarks>
    Private Sub ObtenirInfoCodeAccesMotPasse(ByVal strCle As String,
                                             ByVal strRaison As String,
                                             ByVal strCodeUsager As String,
                                             ByRef strCompte As String,
                                             ByRef strMDP As String)

        'il doit y avoir une raison
        If String.IsNullOrWhiteSpace(strRaison) Then Throw New TsCuRaisonObligatoire

        Dim zones As Zones = Zone.GetCurrents()

        Dim drInfoCompte As DataRow = ObtenirInfoCompte(zones, strCle)

        Dim objJournalisation As New tsCuJournalisation()

        If objInfoUtil.ValiderAcces(strCodeUsager, strCle, drInfoCompte.Item("Type").ToString, drInfoCompte.Item("Profil").ToString, drInfoCompte.Item("CodeVerif").ToString, String.Empty) Then

            strCompte = drInfoCompte.Item("Code").ToString
            strMDP = TsCuRSA.Decrypt(TsCuRSA.ObtenirCertificat(), drInfoCompte.Item("MDP").ToString)

            objJournalisation.EcrireJournal(strCle, strCompte, strCodeUsager, "normal", strRaison, tsCuJournalisation.TypeEvenement.Information)

        Else
            objJournalisation.EcrireJournal(strCle, strCompte, strCodeUsager, "HAUT", "<Opération non permise> " & strRaison, tsCuJournalisation.TypeEvenement.Erreur)
            Throw New TsCuDroitsInsuffisants()
        End If
    End Sub

    Private Sub ObtenirInfoCodeAccesMotPasseLibraire(ByVal strCodeUsager As String,
                                                    ByRef strCompte As String,
                                             ByRef strMDP As String)

        Dim strCle As String = XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("TS1", "TS1\TS1N236\CleSymboliqueLibraire")
        Dim strRaison As String = "Impersonifier le libraire afin de lui donner accès aux clé symbolique de production"

        Dim zones As Zones = Zone.GetCurrents()

        Dim drInfoCompte As DataRow = ObtenirInfoCompteLibraire(zones, strCle)

        Dim objJournalisation As New tsCuJournalisation()
        If objInfoUtil.ValiderAcces(strCodeUsager, strCle, drInfoCompte.Item("Type").ToString, drInfoCompte.Item("Profil").ToString, drInfoCompte.Item("CodeVerif").ToString, String.Empty) Then

            strCompte = drInfoCompte.Item("Code").ToString
            strMDP = TsCuRSA.Decrypt(TsCuRSA.ObtenirCertificat(), drInfoCompte.Item("MDP").ToString)

            objJournalisation.EcrireJournal(strCle, strCompte, strCodeUsager, "normal", strRaison, tsCuJournalisation.TypeEvenement.Information)

        Else
            objJournalisation.EcrireJournal(strCle, strCompte, strCodeUsager, "HAUT", "<Opération non permise> " & strRaison, tsCuJournalisation.TypeEvenement.Erreur)
            Throw New TsCuDroitsInsuffisants()
        End If
    End Sub

    ''' <summary>
    ''' Obtenir les informations concernant la clé symbolique demandé.  
    ''' Elle recherche la clé demandée dans le champ 'Cle'.   Maintenant, la 
    ''' nomenclature de la clé symbolique nous permet de distinguer l'environnement.   
    ''' Cette méthode remplace la méthode Obtenir.
    ''' </summary>
    ''' <param name="zones">
    ''' 	La zone dans laquelle la clé est recherchée.  2 zones:  "D, H" ou "I, IV"
    ''' 	Value Type: string
    ''' </param>
    ''' <param name="strCle">
    ''' 	Nom de la clé symbolique.  
    ''' 	Value Type: string
    ''' </param>
    ''' <returns>
    ''' On retourne l'information de la clé symbolique demandée dans un datarow. Value Type: datarow
    ''' </returns>
    ''' <exception cref="TsCuResultatMultiple">
    '''    !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    '''    !!!!! Précision si cette erreur survient un moment donné      !!!!!
    '''    !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    '''    !Il est possible qu'une même clé existe pour 2 types différents.  !
    '''    !La validation pour l'ajout d'un code accès générique est:        !
    '''    !clé symbolique + type de la clé symbolique (D, H, I, IV)         !
    '''    !                                                                 !
    '''    !Donc, si une clé symbolique existe pour le type D et H, on a un  !
    '''    !résultat multiple.L'utilisateur a un problème à ce moment là,    !
    '''    !parce qu'il n'a pas le moyen de préciser sa recherche...         !
    '''    !Normalement, cela ne devrait jamais arriver.                     !  
    '''    !Si cela arrive, vérifier avec l'équipe de sécurité.              !
    '''    !                                                                 !
    '''    !Par contre, il peut avoir deux clés symboliques pareilles,       !
    '''    !mais pour une zone différente.                                   !
    '''    !Par exemple, la clé X dans le type Domaine et Inforoute.         ! 
    '''    !Dans ce cas, c'est OK puisque l'on fait une recherche avec la    ! 
    '''    !zone.Inforoute et domaine ne sont pas dans la même zone.         !
    '''    !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    ''' </exception>
    ''' <remarks>
    ''' Historique des modifications: 
    ''' </remarks>
    Private Function ObtenirInfoCompte(ByVal zones As Zones, ByVal strCle As String) As DataRow
        Dim dsInfoCdAcc As Dictionary(Of String, DataRow) = objInfoUtil.LireFichierCodeAccesEnDictionnaire()

        strCle = strCle.ToUpperInvariant()
        Dim tabLignes As New List(Of DataRow)()
        For Each zone As Zone In zones
            Dim clef As String = String.Concat(strCle, zone)

            If dsInfoCdAcc.ContainsKey(clef) Then
                tabLignes.Add(dsInfoCdAcc(clef))
            End If
        Next

        If tabLignes.Count = 0 Then Throw New TsCuCodeInexistant(strCle)
        If tabLignes.Count > 1 Then Throw New TsCuResultatMultiple(strCle) 'voir la description de l'erreur dans le commentaire d'en-tete

        'il y a exactement 1 item retourné
        Return tabLignes(0)
    End Function

    Private Function ObtenirInfoCompteLibraire(ByVal zones As Zones, ByVal strCle As String) As DataRow
        Dim dsInfoCdAcc As Dictionary(Of String, DataRow) = objInfoUtil.LireFichierCodeAccesLibraire()

        strCle = strCle.ToUpperInvariant()
        Dim tabLignes As New List(Of DataRow)()
        For Each zone As Zone In zones
            Dim clef As String = String.Concat(strCle, zone)

            If dsInfoCdAcc.ContainsKey(clef) Then
                tabLignes.Add(dsInfoCdAcc(clef))
            End If
        Next

        If tabLignes.Count = 0 Then Throw New TsCuCodeInexistant(strCle)
        If tabLignes.Count > 1 Then Throw New TsCuResultatMultiple(strCle) 'voir la description de l'erreur dans le commentaire d'en-tete

        'il y a exactement 1 item retourné
        Return tabLignes(0)
    End Function

#End Region

End Class





