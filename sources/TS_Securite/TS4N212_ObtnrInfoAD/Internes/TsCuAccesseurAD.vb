Imports System.DirectoryServices
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Security.Principal
Imports System.Text
Imports System.Text.RegularExpressions
Imports Rrq.InfrastructureCommune.Parametres
Imports TS4N214_IObtnrInfoAD

''' ------------------------------------------------------------------------------------
''' *** ATTENTION **
''' *** IMPORTANT ***
''' Cette classe a été modifiée le 6 janvier 2025 pour intégrer des appels à la logique  
''' f'affaire, exclusivement pour les systèmes administratifs web. Ces systèmes étant  
''' amenés à disparaître, et afin de ne pas impacter les autres systèmes fonctionnels,  
''' nous avons volontairement évité l'utilisation de l'orientation objet.  
''' À la place, de simples conditions "If" ont été utilisées. 
''' ------------------------------------------------------------------------------------

Friend Class TsCuAccesseurAD
    Private ReadOnly _proprieteActiveDirectory As Proprietes
    Private _attributAdAssocieAuTypeDeRecherche As TsCuTypeRequeteVsAttributAD
    Private _appelerLAF As Boolean = XuCuPolitiqueConfig.ConfigDomaine.ObtenirValeurSystemeOptionnelle("TS4", "TS4N212\AppelerLAF", "Non").Equals("Oui")


    Public Sub New(ByVal NomServeurAD As String, ByVal TypeRequeteVsAttributAD As TsCuTypeRequeteVsAttributAD)
        _proprieteActiveDirectory = New Proprietes()
        _attributAdAssocieAuTypeDeRecherche = TypeRequeteVsAttributAD
        _NmServeurAD = NomServeurAD
    End Sub


    Public Property NmServeurAD() As String

    Private _domaineNT As String = String.Empty
    ''' <summary>
    ''' Obtient le nom netBIOS du domaine dynamiquement.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property DomaineNT As String
        Get
            If Not String.IsNullOrEmpty(_domaineNT) Then Return _domaineNT

            If EffectuerAppelLAF() Then
                Return DomaineNTWCF()
            End If

            Dim serveurActiveDirectory As String = NmServeurAD()
            Using de As New DirectoryEntry(String.Format("LDAP://{0}", serveurActiveDirectory))
                Using searcher As New DirectorySearcher(de)
                    searcher.SearchScope = SearchScope.Base
                    searcher.PropertiesToLoad.Add("msDS-PrincipalName")
                    searcher.Filter = "(objectClass=*)"

                    Dim result As SearchResult = searcher.FindOne()
                    _domaineNT = result.Properties("msDS-PrincipalName")(0).ToString().ToUpper().Replace("\", String.Empty)
                End Using

                Return _domaineNT
            End Using
        End Get
    End Property

    <MethodImpl(MethodImplOptions.NoInlining)>
    Private Function DomaineNTWCF() As String
        Dim appelLAF As New Rrq.Securite.TsCuAccesADWCF()
        Return appelLAF.DomaineNT(NmServeurAD())
    End Function

    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	Securite.tsCuObtnrInfoAD.RechercheActiveDirectory
    ''' <summary>
    '''     Effectue une recherche dans l'AD en utilisant les paramètres reçus.
    ''' </summary>
    ''' <param name="pTypeRequete">
    ''' 	Indicateur du champs sur lequel le recherche doit être effectuée. 
    ''' </param>
    ''' <param name="strCritereRecherche">
    ''' 	Valeur recherchée, peut être une valeur spécifique ou un masque de recherche (ex: T20*). 
    ''' </param>
    ''' <param name="strCritereRechercheSecondaire">
    ''' 	Le critère de recherche secondaire à l'active directory utilisé lors d'une recherche nom et prénom. 
    ''' </param>
    ''' <returns><see cref="Data.DataTable" />	(System.Data.DataTable)</returns>
    ''' <remarks><para><pre>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2005-05-24	t209376		Création initiale
    ''' 2005-09-13  t209376     Ajout du paramètre optionnel strCritereRechercheSecondaire
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    Friend Function RechercheActiveDirectory(ByVal pTypeRequete As TsIadTypeRequete, ByVal strCritereRecherche As String,
                                              ByVal strCritereRechercheSecondaire As String,
                                              ByVal pObjectCategory As TsIadObjectCategory) As DataTable

        If EffectuerAppelLAF() Then
            Return RechercheActiveDirectoryWCF(pTypeRequete, strCritereRecherche, strCritereRechercheSecondaire, pObjectCategory)
        End If

        Dim query As String = "{0}"
        If pObjectCategory = TsIadObjectCategory.TsIadOcPerson Then
            query = "(&(objectCategory=person){0})"
        ElseIf pObjectCategory = TsIadObjectCategory.TsIadOcGroup Then
            query = "(&(objectCategory=group){0})"
        End If

        Dim specificCriteria As String
        If pTypeRequete = TsIadTypeRequete.TsIadTrNomEtPrenom Then
            specificCriteria = String.Format("(&(Sn={0})(GivenName={1}))", strCritereRecherche, strCritereRechercheSecondaire)
        Else
            specificCriteria = String.Format("({0}={1})", _attributAdAssocieAuTypeDeRecherche.ObtenirCombinaison(pTypeRequete), strCritereRecherche)
        End If


        Using dt As New DataTable("TblInfoActiveDir")
            Using root As New DirectoryEntry()
                root.AuthenticationType = AuthenticationTypes.Secure
                root.Path = String.Format("LDAP://{0}", NmServeurAD())

                Using searcher As New DirectorySearcher
                    searcher.CacheResults = False
                    searcher.SearchRoot = root
                    searcher.PageSize = 1000 ' Pour ne pas être limité a 1000 elements lors des recherches - On peux obtenir plusieurs page de 1000 elements....
                    searcher.Filter = String.Format(query, specificCriteria)

                    For Each prop As Propriete In _proprieteActiveDirectory
                        searcher.PropertiesToLoad.Add(prop.NomAd)
                        dt.Columns.Add(New DataColumn(prop.NomSql, prop.TypeSql))
                    Next

                    Using resultats As SearchResultCollection = searcher.FindAll()
                        For Each result As SearchResult In resultats
                            'ne pas retourner les types 'contact'
                            If result.Properties("objectClass").Contains("contact") Then
                                Continue For
                            End If

                            Dim row As DataRow = dt.NewRow
                            For Each prop As Propriete In _proprieteActiveDirectory
                                If prop.EstUserAccountControl() Then row(prop.NomSql) = 0
                                If prop.EstEstCompteAdmin() Then
                                    row(prop.NomSql) = estUnCompteAdministrateur(prop, result)
                                    Continue For
                                End If

                                If result.Properties.Contains(prop.NomAd) Then
                                    ' Parcourir la collection liée à la propriété
                                    For Each valeur As Object In result.Properties(prop.NomAd)
                                        If Not prop.Exceptionnel Then
                                            'n'est pas un cas d'exception
                                            row(prop.NomSql) = IIf(valeur Is DBNull.Value, Nothing, valeur)

                                        Else
                                            ' on traite un cas d'exception 
                                            If prop.EstMemberOf() Then
                                                Dim InfoGrp() As String = Split(CType(valeur, String), ",")
                                                Dim sTmp As String = ""
                                                If InfoGrp(0).Length > 3 Then sTmp = InfoGrp(0).Substring(3)
                                                If IsDBNull(row(prop.NomSql)) Then
                                                    row(prop.NomSql) = sTmp
                                                Else
                                                    row(prop.NomSql) = row(prop.NomSql).ToString + ";" & sTmp
                                                End If

                                            ElseIf prop.EstObjectSid() Then
                                                ' Convertie le Sid en Byte vers une chaîne de caractères
                                                row(prop.NomSql) = ConvertirSid(CType(valeur, Byte()))

                                            End If
                                        End If
                                    Next
                                End If
                            Next
                            dt.Rows.Add(row)
                        Next
                    End Using

                    Return dt.Copy
                End Using
            End Using
        End Using
    End Function

    <MethodImpl(MethodImplOptions.NoInlining)>
    Private Function RechercheActiveDirectoryWCF(ByVal pTypeRequete As TsIadTypeRequete, ByVal strCritereRecherche As String,
                                              ByVal strCritereRechercheSecondaire As String,
                                              ByVal pObjectCategory As TsIadObjectCategory) As DataTable
        Dim appelLAF As New Rrq.Securite.TsCuAccesADWCF()
        Return appelLAF.RechercheActiveDirectory(NmServeurAD(), CInt(pTypeRequete), strCritereRecherche, strCritereRechercheSecondaire, CInt(pObjectCategory))
    End Function


    Private Function estUnCompteAdministrateur(prop As Propriete, result As SearchResult) As Boolean
        Const ADMIN As String = "A"
        Dim retour As Boolean = False

        Dim valeur As String = getStringProperty(result, prop.NomAd)
        'si c'est le nouvel AD, la propriété contient un acronyme du type de compte (deuxième lettre 'A' = Admin)
        If Not String.IsNullOrEmpty(valeur) Then
            If valeur.Length < 2 Then
                'chaine pas assez longue... reset de la valeur pour appliquer ancienne logique
                valeur = String.Empty
            Else
                'valider la deuxième lettre
                retour = (valeur.Substring(1, 1).ToUpper() = ADMIN)
            End If
        End If

        'si la propriété n'est pas présente ou vide, c'est surement l'ancien domaine... ancienne logique
        If String.IsNullOrEmpty(valeur) Then
            retour = result.Path.Contains("Administrateurs")
        End If

        Return retour
    End Function

    Private Function getStringProperty(source As SearchResult, propertyName As String) As String
        Dim values As ResultPropertyValueCollection = source.Properties(propertyName)
        If values Is Nothing Then Return String.Empty
        If values.Count = 0 Then Return String.Empty

        Dim value As Object = values.Item(0)
        If value Is Nothing Then Return String.Empty

        Return value.ToString()
    End Function

    Friend Function RechercheGroupeAD(ByVal strGroupe As String, ByVal blnRechRecursive As Boolean) As DataTable

        If EffectuerAppelLAF() Then
            Return RechercheGroupeADWCF(strGroupe, blnRechRecursive)
        End If

        Using dt As New DataTable("TblInfoActiveDir")
            For Each prop As Propriete In _proprieteActiveDirectory
                dt.Columns.Add(New DataColumn(prop.NomSql, prop.TypeSql))
            Next

            Using root As New DirectoryEntry()
                root.AuthenticationType = AuthenticationTypes.Secure
                root.Path = String.Format("LDAP://{0}", NmServeurAD())

                Using searcher As New DirectorySearcher(root)
                    searcher.PageSize = 1000 ' Pour ne pas être limité a 1000 elements lors des recherches - On peux obtenir plusieurs page de 1000 elements....
                    For Each prop As Propriete In _proprieteActiveDirectory
                        searcher.PropertiesToLoad.Add(prop.NomAd)
                    Next
                    searcher.PropertiesToLoad.Add("objectCategory") 'important que cette propriété soit la dernière

                    Using groupSearcher As New DirectorySearcher(root)
                        groupSearcher.CacheResults = False
                        groupSearcher.PropertyNamesOnly = True
                        groupSearcher.PropertiesToLoad.Add("name")
                        groupSearcher.ReferralChasing = ReferralChasingOption.None
                        groupSearcher.Filter = String.Format("(&(objectCategory=group)(sAMAccountName={0}))", strGroupe)

                        Dim srGroupe As SearchResult = groupSearcher.FindOne
                        Using deGroupe As DirectoryEntry = srGroupe.GetDirectoryEntry()
                            searcher.Filter = String.Format("(&(|(objectCategory=person)(objectCategory=group))(memberof={0}))", deGroupe.Properties("distinguishedName").Value)

                            Using srcMmbr As SearchResultCollection = searcher.FindAll()
                                For Each srMembre As SearchResult In srcMmbr
                                    If srMembre.Properties("objectCategory")(0).ToString().StartsWith("CN=Person") Then
                                        Dim rows() As DataRow = dt.Select("sAMAccountName='" + srMembre.Properties("sAMAccountName")(0).ToString() + "'")
                                        If rows.Length = 0 Then
                                            Dim row As DataRow = dt.NewRow

                                            For Each prop As Propriete In _proprieteActiveDirectory
                                                If prop.EstUserAccountControl() Then row(prop.NomSql) = 0
                                                If prop.EstEstCompteAdmin() Then
                                                    row(prop.NomSql) = estUnCompteAdministrateur(prop, srMembre)
                                                    Continue For
                                                End If

                                                If srMembre.Properties.Contains(prop.NomAd) Then
                                                    ' Parcourir la collection liée à la propriété
                                                    For Each valeur As Object In srMembre.Properties(prop.NomAd)

                                                        ' Vérifie si l'on traite un cas d'exception ou non
                                                        If Not prop.Exceptionnel Then
                                                            row(prop.NomSql) = IIf(valeur Is DBNull.Value, Nothing, valeur)

                                                        Else
                                                            If prop.EstMemberOf() Then
                                                                Dim InfoGrp() As String = Split(CType(valeur, String), ",")
                                                                Dim sTmp As String = String.Empty
                                                                If InfoGrp(0).Length > 3 Then sTmp = InfoGrp(0).Substring(3)
                                                                If IsDBNull(row(prop.NomSql)) Then
                                                                    row(prop.NomSql) = sTmp
                                                                Else
                                                                    row(prop.NomSql) = row(prop.NomSql).ToString & ";" & sTmp
                                                                End If

                                                            ElseIf prop.EstObjectSid() Then
                                                                ' Convertie le Sid en Byte vers une chaîne de caractères
                                                                row(prop.NomSql) = ConvertirSid(CType(valeur, Byte()))
                                                            End If
                                                        End If
                                                    Next
                                                End If
                                            Next

                                            dt.Rows.Add(row)
                                        End If

                                    ElseIf blnRechRecursive Then
                                        Dim objDTAdd As DataTable = RechercheGroupeAD(srMembre.Properties("sAMAccountName")(0).ToString(), blnRechRecursive)

                                        For Each row As DataRow In objDTAdd.Rows
                                            Dim rows() As DataRow = dt.Select("sAMAccountName='" + row.Item("sAMAccountName").ToString() + "'")
                                            If rows.Length = 0 Then dt.ImportRow(row)
                                        Next
                                    End If
                                Next
                            End Using
                        End Using

                    End Using
                End Using
            End Using

            Return dt.Copy
        End Using
    End Function


    <MethodImpl(MethodImplOptions.NoInlining)>
    Private Function RechercheGroupeADWCF(ByVal strGroupe As String, ByVal blnRechRecursive As Boolean) As DataTable
        Dim appelLAF As New Rrq.Securite.TsCuAccesADWCF()
        Return appelLAF.RechercheGroupeAD(NmServeurAD(), strGroupe, blnRechRecursive)
    End Function
    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	Securite.tsCuObtnrInfoAD.ChercheDansGroupes
    ''' <summary>
    '''     Fonction récursive de recherche de groupe.  Il recherche dans toute la
    '''     cascade de groupes que l'utilisateur peut avoir.
    ''' </summary>
    ''' <param name="strGroupeRecherche">
    ''' 	Nom du groupe que l'on recherche. 
    ''' 	Value Type: <see cref="String" />	(System.String)
    ''' </param>
    ''' <param name="strACID">
    ''' 	Nom du groupe dans lequel on recherche présentement le groupe désiré. 
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
    ''' 2005-06-06	t209376		Création initiale
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    Public Function ChercheDansGroupes(ByVal strACID As String, ByVal strGroupeRecherche As String) As Boolean

        If EffectuerAppelLAF() Then
            Return ChercheDansGroupesWCF(strACID, strGroupeRecherche)
        End If

        Using root As New DirectoryEntry
            root.AuthenticationType = AuthenticationTypes.Secure
            root.Path = String.Format("LDAP://{0}", NmServeurAD())

            Using searcher As New DirectorySearcher
                searcher.CacheResults = False
                searcher.SearchRoot = root
                searcher.Filter = String.Format("(&(objectCategory=group)(sAMAccountName={0}))", strACID)
                searcher.PageSize = 1000 ' Pour ne pas être limité a 1000 elements lors des recherches - On peux obtenir plusieurs page de 1000 elements....

                Using resultats As SearchResultCollection = searcher.FindAll()
                    Dim result As SearchResult = Nothing
                    If resultats.Count > 0 Then result = resultats(0)
                    If result Is Nothing Then
                        Return Nothing
                    End If


                    Dim sbGroupSIDs As New StringBuilder

                    sbGroupSIDs.Append("(|")
                    Using entry As DirectoryEntry = result.GetDirectoryEntry
                        entry.RefreshCache(New String() {"tokenGroups"})
                        For Each sid As Byte() In entry.Properties("tokenGroups")
                            sbGroupSIDs.AppendFormat("(objectSid={0})", ConvertirOctetString(sid))
                        Next
                    End Using
                    sbGroupSIDs.Append(")")

                    If sbGroupSIDs.ToString <> "(|)" Then
                        Using ds As New DirectorySearcher(root, sbGroupSIDs.ToString, New String() {"memberof", "sAMAccountName"})
                            ds.PageSize = 1000 ' Pour ne pas être limité a 1000 elements lors des recherches - On peux obtenir plusieurs page de 1000 elements....

                            Using collectionResultatsUser As SearchResultCollection = ds.FindAll()
                                Dim intCtr As Integer = 0
                                While intCtr < collectionResultatsUser.Count
                                    If collectionResultatsUser(intCtr).Properties("sAMAccountName")(0).ToString = strGroupeRecherche Then
                                        Return True
                                    End If

                                    intCtr += 1
                                End While
                            End Using
                        End Using
                    End If
                End Using
            End Using
        End Using

        Return False
    End Function

    <MethodImpl(MethodImplOptions.NoInlining)>
    Private Function ChercheDansGroupesWCF(ByVal strACID As String, ByVal strGroupeRecherche As String) As Boolean
        Dim appelLAF As New Rrq.Securite.TsCuAccesADWCF()
        Return appelLAF.ChercheDansGroupes(NmServeurAD(), strACID, strGroupeRecherche)
    End Function

    Private Function ConvertirOctetString(ByVal bytes As Byte()) As String
        Dim sbBytes As New StringBuilder

        Dim i As Integer = 0
        While i < bytes.Length
            sbBytes.AppendFormat("\{0}", bytes(i).ToString("X2"))
            i += 1
        End While

        Return sbBytes.ToString
    End Function

    Public Function ObtenirMembresGroupe(ByVal NomGroupe As String) As String()

        If EffectuerAppelLAF() Then
            Return ObtenirMembresGroupeWCF(NomGroupe)
        End If

        Dim ListeGroupe() As String = Nothing

        Using root As New DirectoryEntry
            root.AuthenticationType = AuthenticationTypes.Secure
            root.Path = String.Format("LDAP://{0}", NmServeurAD())

            Using searcher As New DirectorySearcher
                searcher.CacheResults = False
                searcher.SearchRoot = root
                searcher.PageSize = 1000 ' Pour ne pas être limité a 1000 elements lors des recherches - On peux obtenir plusieurs page de 1000 elements....
                searcher.Filter = String.Format("(&(objectCategory=group)(sAMAccountName={0}))", NomGroupe)


                Using resultats As SearchResultCollection = searcher.FindAll()
                    Dim result As SearchResult = Nothing
                    If resultats.Count > 0 Then result = resultats(0)
                    If result Is Nothing Then
                        Return Nothing
                    End If


                    Dim sbGroupSIDs As New StringBuilder

                    sbGroupSIDs.Append("(|")
                    Using entry As DirectoryEntry = result.GetDirectoryEntry
                        entry.RefreshCache(New String() {"tokenGroups"})
                        For Each sid As Byte() In entry.Properties("tokenGroups")
                            sbGroupSIDs.AppendFormat("(objectSid={0})", ConvertirOctetString(sid))
                        Next
                    End Using
                    sbGroupSIDs.Append(")")

                    If sbGroupSIDs.ToString <> "(|)" Then
                        Using ds As New DirectorySearcher(root, sbGroupSIDs.ToString, New String() {"memberof", "sAMAccountName"})
                            ds.PageSize = 1000 ' Pour ne pas être limité a 1000 elements lors des recherches - On peux obtenir plusieurs page de 1000 elements....

                            Using collectionResultatsUser As SearchResultCollection = ds.FindAll()
                                Dim intCtr As Integer = 0

                                ReDim ListeGroupe(collectionResultatsUser.Count - 1)

                                While intCtr < collectionResultatsUser.Count
                                    ListeGroupe(intCtr) = collectionResultatsUser(intCtr).Properties("sAMAccountName")(0).ToString()
                                    intCtr += 1
                                End While
                            End Using
                        End Using
                    End If
                End Using
            End Using
        End Using

        Return ListeGroupe
    End Function


    <MethodImpl(MethodImplOptions.NoInlining)>
    Private Function ObtenirMembresGroupeWCF(ByVal NomGroupe As String) As String()
        Dim appelLAF As New Rrq.Securite.TsCuAccesADWCF()
        Return appelLAF.ObtenirMembresGroupe(NmServeurAD(), NomGroupe)
    End Function

    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	Securite.tsCuObtnrInfoAD.ConvertirSid
    ''' <summary>
    '''     Cette fonction convertie le Sid en chaine de caractère.
    ''' </summary>
    ''' <param name="tblSID">
    ''' 	Tableau de Bytes qui contient le Sid d'un compte dans l'AD. 
    ''' 	Reference Type: <see cref="Byte" />	(System.Byte)
    ''' </param>
    ''' <returns><see cref="String" />	(System.String)</returns>
    ''' <remarks><para><pre>
    ''' Historique des modifications: 
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' Date		Nom			Description
    ''' 
    ''' --------------------------------------------------------------------------------
    ''' 2005-05-24	t209376		Création initiale
    ''' 
    ''' </pre></para>
    ''' </remarks>
    ''' --------------------------------------------------------------------------------
    Public Function ConvertirSid(ByRef tblSID As Byte()) As String
        Dim strCle As String = String.Empty
        Dim strOctet As String
        Dim intCtr As Integer

        ' Convertir le SID en string
        For intCtr = LBound(tblSID) To UBound(tblSID)
            If tblSID(intCtr) < &H10 Then
                strOctet = "\0" & Hex(tblSID(intCtr))
            Else
                strOctet = "\" & Hex(tblSID(intCtr))
            End If
            strCle = strCle & strOctet
        Next intCtr

        Return strCle
    End Function


    ''' <summary>
    '''   Cette méthode retourne une liste de groupe contenu dans l'AD selon un filtre.
    ''' </summary>
    ''' <param name="Filtre">Filtre a appliquer pour la rechercher dans l'AD.</param>
    ''' <returns>Une collection contenant groupes trouvés.</returns>
    Public Function ObtenirListeGroupes(ByVal Filtre As String) As SearchResultCollection

        Dim Resultats As SearchResultCollection = Nothing

        Using root As New DirectoryEntry()
            root.AuthenticationType = AuthenticationTypes.Secure
            root.Path = String.Format("LDAP://{0}", NmServeurAD())

            Using searcher As New DirectorySearcher(root)
                searcher.ReferralChasing = ReferralChasingOption.All
                searcher.SearchScope = SearchScope.Subtree
                searcher.PageSize = 1000 ' Pour ne pas être limité a 1000 elements lors des recherches - On peux obtenir plusieurs page de 1000 elements....
                searcher.Filter = String.Format("(&(objectCategory=group)(sAMAccountName={0}))", Filtre)

                Resultats = searcher.FindAll()
            End Using
        End Using

        Return Resultats
    End Function


    Public Function VerifierGroupeExiste(ByVal strGroupe As String) As Boolean
        Dim result As SearchResult = Nothing

        Using root As New DirectoryEntry
            root.AuthenticationType = AuthenticationTypes.Secure
            root.Path = String.Format("LDAP://{0}", NmServeurAD())

            Using searcher As New DirectorySearcher
                searcher.CacheResults = False
                searcher.SearchRoot = root
                searcher.Filter = String.Format("(&(objectCategory=group)(sAMAccountName={0}))", strGroupe)

                result = searcher.FindOne()
            End Using
        End Using

        Return (Not result Is Nothing)
    End Function

    'Vérifie si l'appelant est XU7N043. 
    Private Function AppelPourCreationPoolWXCF() As Boolean
        Dim assemblyCourant As Assembly = Assembly.GetExecutingAssembly()
        Dim pileAppel As StackTrace = New StackTrace()
        Dim methodes() As StackFrame = pileAppel.GetFrames
        For Each methode As StackFrame In methodes
            If methode.GetMethod.DeclaringType IsNot Nothing AndAlso methode.GetMethod.DeclaringType.Assembly.GetName.Name.ToUpper.Trim.Equals("XU7N043_GERERSITESIISWCF") Then
                Return True
            End If
        Next
        Return False
    End Function

    ' L'appel à la logique d'affaire doit être effectué uniquement si :  
    ' 1. Le paramètre "AppelerLAF" est défini sur "oui" dans le fichier de configuration.  
    ' 2. L'appel n'est pas initié par le composant "XU7N043".  
    ' 3. Nous ne sommes pas déjà dans la logique d'affaire.  
    '
    ' Remarques :  
    ' - Il est impossible d'appeler la logique d'affaire lorsque le composant "XU7N043" est en cours d'exécution,  
    '   car ce dernier est en train de créer les pools IIS.  
    ' - Si nous sommes déjà dans la logique d'affaire, un nouvel appel est inutile. 
    Private Function EffectuerAppelLAF() As Boolean
        If _appelerLAF And Not AppelPourCreationPoolWXCF() And Not EstLAF() Then
            Return True
        End If
        Return False
    End Function

    Private Function EstLAF() As Boolean
        'Le nom de l'usager est composé comme tel:  Domaine\CodeUtilisateur 
        Dim strDomCodeUtil As String() = WindowsIdentity.GetCurrent.Name.Split("\".ToCharArray())
        Dim regexComptePool As Regex = New Regex("ZAP[IABSP]W")
        If regexComptePool.IsMatch(strDomCodeUtil(1)) Then
            Return True
        End If

        Return False
    End Function

End Class


Public Class TsCuTypeRequeteNonSupporteeException
    Inherits ApplicationException

    Public Sub New(ByVal ex As Exception)
        MyBase.New("Type de requête reçu non supporté par le composant", ex)
    End Sub
End Class


Public Class TsCuRetourMultipleException
    Inherits ApplicationException

    Public Sub New()
        MyBase.New("Plus d'une entrée de l'active directory retournée.")
    End Sub
End Class