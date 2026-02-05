Imports System.DirectoryServices
Imports System.Text

Friend Class TsCuAccesseurAD

#Region " Énumération, constantes et variables "
    ' -----------------------------------------------------------------------------------
    ' Déclaration des constantes identifiants les champs et leurs types, 
    ' à être récupérés de l'AD.
    ' -----------------------------------------------------------------------------------
    Private Const LST_PROP_AD As String = "sAMAccountName,Sn,GivenName,DisplayName,Mail,Department,Title,MemberOf,ObjectSid,company,description,employeeNumber,initials,personalTitle,userAccountControl,objectClass" 'userAccountControl et objectClass doivent toujours être en dernier
    Private Const LST_TYPE_AD As String = "System.String,System.String,System.String,System.String,System.String,System.String,System.String,System.String,System.String,System.String,System.String,System.String,System.String,System.String,System.Int32,System.String"
    Private Const LST_CAS_EXCEP As String = "MemberOf,ObjectSid"

    ' -----------------------------------------------------------------------------------
    ' Déclaration des variables de travail
    ' -----------------------------------------------------------------------------------
    Private mNmServeurAD As String
    Private mProprietes() As String
    Private mTypesProps() As String
    Private mTypeRequeteVsAttributAD As TsCuTypeRequeteVsAttributAD
    Private m_deRoot As DirectoryEntry
    Private m_deRootUtil As DirectoryEntry
    Private m_dsRootSearch As DirectorySearcher
    Private m_dsGrpSearch As DirectorySearcher

#End Region

    Sub New(ByVal NomServeurAD As String, ByVal TypeRequeteVsAttributAD As TsCuTypeRequeteVsAttributAD)
        NmServeurAD = NomServeurAD
        mProprietes = Split(LST_PROP_AD, ",")
        mTypesProps = Split(LST_TYPE_AD, ",")
        mTypeRequeteVsAttributAD = TypeRequeteVsAttributAD
    End Sub


    Public Property NmServeurAD() As String
        Get
            Return mNmServeurAD
        End Get

        Set(ByVal Value As String)
            mNmServeurAD = Value
        End Set
    End Property


    Public ReadOnly Property Proprietes() As String()
        Get
            Return mProprietes
        End Get
    End Property

    Public ReadOnly Property TypesProps() As String()
        Get
            Return mTypesProps
        End Get
    End Property

    Public ReadOnly Property ListeCasException() As String
        Get
            Return LST_CAS_EXCEP
        End Get
    End Property



    ''' --------------------------------------------------------------------------------
    ''' Class.Method:	Securite.tsCuObtnrInfoAD.RechercheActiveDirectory
    ''' <summary>
    '''     Effectue une recherche dans l'AD en utilisant les paramètres reçus.
    ''' </summary>
    ''' <param name="pTypeRequete">
    ''' 	Indicateur du champs sur lequel le recherche doit être effectuée. 
    ''' 	Value Type: <see cref="Securite.tsCuObtnrInfoAD.TsIadTypeRequete" />	(Rrq.Securite.tsCuObtnrInfoAD.TsIadTypeRequete)
    ''' </param>
    ''' <param name="strCritereRecherche">
    ''' 	Valeur recherchée, peut être une valeur spécifique ou un masque de recherche (ex: T20*). 
    ''' 	Value Type: <see cref="String" />	(System.String)
    ''' </param>
    ''' <param name="strCritereRechercheSecondaire">
    ''' 	Le critère de recherche secondaire à l'active directory utilisé lors d'une recherche nom et prénom. 
    '''     Value Type: <see cref="String" />	(System.String)
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
    Friend Function RechercheActiveDirectory(ByVal pTypeRequete As TsIadTypeRequete, ByVal strCritereRecherche As String, _
                                              Optional ByVal strCritereRechercheSecondaire As String = "", _
                                              Optional ByVal pObjectCategory As TsIadObjectCategory = TsIadObjectCategory.TsIadOcTous) As DataTable

        Dim objDT As New DataTable("TblInfoActiveDir")
        Dim objValeurCourant As Object
        Dim InfoGrp() As String
        Dim sReqObjCat As String = ""
        Dim sTmp As String
        Dim intCtr As Integer
        Dim intCptr As Integer


        Using deRoot As New DirectoryEntry
            deRoot.AuthenticationType = AuthenticationTypes.Secure
            deRoot.Path = String.Format("LDAP://{0}", NmServeurAD())

            Using dsSearcher As New DirectorySearcher
                dsSearcher.CacheResults = False
                dsSearcher.SearchRoot = deRoot
                dsSearcher.PageSize = 1000 ' Pour ne pas être limité a 1000 elements lors des recherches - On peux obtenir plusieurs page de 1000 elements....

                If pObjectCategory = TsIadObjectCategory.TsIadOcPerson Then
                    sReqObjCat = "objectCategory=person"
                ElseIf pObjectCategory = TsIadObjectCategory.TsIadOcGroup Then
                    sReqObjCat = "objectCategory=group"
                End If

                If pTypeRequete = TsIadTypeRequete.TsIadTrNomEtPrenom Then
                    If sReqObjCat = "" Then
                        dsSearcher.Filter = String.Format("(&(Sn={0})(GivenName={1}))", strCritereRecherche, strCritereRechercheSecondaire)
                    Else
                        dsSearcher.Filter = String.Format("(&(" + sReqObjCat + ")(&(Sn={0})(GivenName={1})))", strCritereRecherche, strCritereRechercheSecondaire)
                    End If
                Else
                    If sReqObjCat = "" Then
                        dsSearcher.Filter = String.Format("({0}={1})", mTypeRequeteVsAttributAD.ObtenirCombinaison(pTypeRequete), strCritereRecherche)
                    Else
                        dsSearcher.Filter = String.Format("(&(" + sReqObjCat + ")({0}={1}))", mTypeRequeteVsAttributAD.ObtenirCombinaison(pTypeRequete), strCritereRecherche)
                    End If
                End If

                For intCtr = LBound(mProprietes) To UBound(mProprietes)
                    dsSearcher.PropertiesToLoad.Add(mProprietes(intCtr))
                    objDT.Columns.Add(New DataColumn(mProprietes(intCtr), System.Type.GetType(mTypesProps(intCtr))))
                Next

                Try
                    Using collectionResultats As SearchResultCollection = dsSearcher.FindAll()
                        For Each result As SearchResult In collectionResultats
                            Dim nr As DataRow = objDT.NewRow

                            For intCtr = LBound(mProprietes) To UBound(mProprietes)

                                If mProprietes(intCtr) = "userAccountControl" Then nr(mProprietes(intCtr)) = 0
                                If result.Properties.Contains(mProprietes(intCtr)) Then
                                    ' Parcourir la collection liée à la propriété
                                    For Each objValeurCourant In result.Properties(mProprietes(intCtr))
                                        ' Vérifie si l'on traite un cas d'exception ou non
                                        intCptr = InStr(1, LST_CAS_EXCEP, mProprietes(intCtr), CompareMethod.Text)
                                        If intCptr = 0 Then
                                            nr(mProprietes(intCtr)) = IIf(objValeurCourant Is System.DBNull.Value, Nothing, objValeurCourant)
                                        ElseIf intCptr = 1 Then ' Champs MemberOf
                                            InfoGrp = Split(CType(objValeurCourant, String), ",")
                                            sTmp = ""
                                            If InfoGrp(0).Length > 3 Then sTmp = InfoGrp(0).Substring(3)
                                            If IsDBNull(nr(mProprietes(intCtr))) Then
                                                nr(mProprietes(intCtr)) = sTmp
                                            Else
                                                nr(mProprietes(intCtr)) = nr(mProprietes(intCtr)).ToString + ";" & sTmp
                                            End If
                                        Else ' Champs ObjectSid
                                            ' Convertie le Sid en Byte vers une chaîne de caractères
                                            nr(mProprietes(intCtr)) = ConvertirSid(CType(objValeurCourant, Byte()))
                                        End If
                                    Next
                                    objValeurCourant = Nothing
                                End If
                            Next
                            objDT.Rows.Add(nr)
                        Next
                    End Using

                    Return objDT.Copy

                Finally
                    ' Libérer les ressources utilisées
                    If Not IsNothing(objDT) Then
                        objDT.Dispose()
                        objDT = Nothing
                    End If
                End Try
            End Using
        End Using
    End Function



    Friend Function RechercheGroupeAD(ByVal strGroupe As String, ByVal blnRechRecursive As Boolean) As DataTable
        Dim srGroupe As SearchResult
        Dim objDT As New DataTable("TblInfoActiveDir")

        Dim objValeurCourantMembre As Object
        Dim InfoGrp() As String
        Dim sTmp As String
        Dim intCptr As Integer

        ObtenirObjetsRacine()

        m_dsGrpSearch.Filter = String.Format("(&(objectCategory=group)(sAMAccountName={0}))", strGroupe)

        Try
            srGroupe = m_dsGrpSearch.FindOne

            Using deGroupe As DirectoryEntry = srGroupe.GetDirectoryEntry()
                Dim srcMmbr As SearchResultCollection = Nothing

                Try
                    m_dsRootSearch.Filter = String.Format("(&(|(objectCategory=person)(objectCategory=group))(memberof={0}))", deGroupe.Properties("distinguishedName").Value)

                    For intCtr As Integer = 0 To mProprietes.Length - 1
                        objDT.Columns.Add(New DataColumn(mProprietes(intCtr), System.Type.GetType(mTypesProps(intCtr))))
                    Next

                    srcMmbr = m_dsRootSearch.FindAll()

                    For Each srMembre As SearchResult In srcMmbr
                        If srMembre.Properties("objectCategory")(0).ToString().StartsWith("CN=Person") Then
                            Dim a_drSelect() As DataRow = objDT.Select("sAMAccountName='" + srMembre.Properties("sAMAccountName")(0).ToString() + "'")
                            If a_drSelect.Length = 0 Then
                                Dim nr As DataRow = objDT.NewRow

                                For intCtr As Integer = 0 To mProprietes.Length - 1
                                    If mProprietes(intCtr) = "userAccountControl" Then nr(mProprietes(intCtr)) = 0
                                    If srMembre.Properties.Contains(mProprietes(intCtr)) Then
                                        ' Parcourir la collection liée à la propriété
                                        For Each objValeurCourantMembre In srMembre.Properties(mProprietes(intCtr))
                                            ' Vérifie si l'on traite un cas d'exception ou non
                                            intCptr = InStr(1, LST_CAS_EXCEP, mProprietes(intCtr), CompareMethod.Text)
                                            If intCptr = 0 Then
                                                nr(mProprietes(intCtr)) = IIf(objValeurCourantMembre Is System.DBNull.Value, Nothing, objValeurCourantMembre)
                                            ElseIf intCptr = 1 Then ' Champs MemberOf
                                                InfoGrp = Split(CType(objValeurCourantMembre, String), ",")
                                                sTmp = ""
                                                If InfoGrp(0).Length > 3 Then sTmp = InfoGrp(0).Substring(3)
                                                If IsDBNull(nr(mProprietes(intCtr))) Then
                                                    nr(mProprietes(intCtr)) = sTmp
                                                Else
                                                    nr(mProprietes(intCtr)) = nr(mProprietes(intCtr)).ToString & ";" & sTmp
                                                End If
                                            Else ' Champs ObjectSid
                                                ' Convertie le Sid en Byte vers une chaîne de caractères
                                                nr(mProprietes(intCtr)) = ConvertirSid(CType(objValeurCourantMembre, Byte()))
                                            End If
                                        Next
                                        objValeurCourantMembre = Nothing
                                    End If
                                Next
                                objDT.Rows.Add(nr)
                            End If
                        ElseIf blnRechRecursive Then
                            Dim objDTAdd As DataTable = RechercheGroupeAD(srMembre.Properties("sAMAccountName")(0).ToString(), blnRechRecursive)

                            For Each drRow As DataRow In objDTAdd.Rows
                                Dim a_drSelect() As DataRow = objDT.Select("sAMAccountName='" + drRow.Item("sAMAccountName").ToString() + "'")
                                If a_drSelect.Length = 0 Then objDT.ImportRow(drRow)
                            Next
                        End If
                    Next
                Finally
                    If Not srcMmbr Is Nothing Then srcMmbr.Dispose()
                End Try
            End Using

            Return objDT.Copy
        Finally
            ' Libérer les ressources utilisées

            DetruireObjetsRacine()

            If Not IsNothing(objDT) Then
                objDT.Dispose()
                objDT = Nothing
            End If
        End Try

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
        Dim blnPresent As Boolean = False

        Using deRoot As New DirectoryEntry
            deRoot.AuthenticationType = AuthenticationTypes.Secure
            deRoot.Path = String.Format("LDAP://{0}", NmServeurAD())

            Using dsSearcher As New DirectorySearcher
                dsSearcher.CacheResults = False
                dsSearcher.SearchRoot = deRoot
                dsSearcher.Filter = String.Format("(&(objectCategory=group)(sAMAccountName={0}))", strACID)
                dsSearcher.PageSize = 1000 ' Pour ne pas être limité a 1000 elements lors des recherches - On peux obtenir plusieurs page de 1000 elements....

                Using collectionResultats As SearchResultCollection = dsSearcher.FindAll()
                    Dim result As SearchResult = Nothing

                    If collectionResultats.Count > 0 Then result = collectionResultats(0)

                    If result Is Nothing Then
                        Return Nothing
                    End If

                    Dim sbGroupSIDs As New StringBuilder

                    sbGroupSIDs.Append("(|")

                    Dim deUser As DirectoryEntry = result.GetDirectoryEntry

                    Try
                        deUser.RefreshCache(New String() {"tokenGroups"})
                        For Each sid As Byte() In deUser.Properties("tokenGroups")
                            sbGroupSIDs.AppendFormat("(objectSid={0})", ConvertirOctetString(sid))
                        Next
                    Finally
                        deUser.Dispose()
                    End Try

                    sbGroupSIDs.Append(")")

                    If sbGroupSIDs.ToString <> "(|)" Then
                        Using ds As New DirectorySearcher(deRoot, sbGroupSIDs.ToString, New String() {"memberof", "sAMAccountName"})
                            ds.PageSize = 1000 ' Pour ne pas être limité a 1000 elements lors des recherches - On peux obtenir plusieurs page de 1000 elements....

                            Using collectionResultatsUser As SearchResultCollection = ds.FindAll()

                                Dim intCtr As Integer = 0
                                While intCtr < collectionResultatsUser.Count
                                    If collectionResultatsUser(intCtr).Properties("sAMAccountName")(0).ToString = strGroupeRecherche Then
                                        blnPresent = True
                                        Exit While
                                    End If

                                    intCtr += 1
                                End While
                            End Using
                        End Using
                    End If
                End Using
            End Using
        End Using

        Return blnPresent
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
        Dim ListeGroupe() As String = Nothing

        Using deRoot As New DirectoryEntry
            deRoot.AuthenticationType = AuthenticationTypes.Secure
            deRoot.Path = String.Format("LDAP://{0}", NmServeurAD())

            Using dsSearcher As New DirectorySearcher
                dsSearcher.CacheResults = False
                dsSearcher.SearchRoot = deRoot
                dsSearcher.PageSize = 1000 ' Pour ne pas être limité a 1000 elements lors des recherches - On peux obtenir plusieurs page de 1000 elements....
                dsSearcher.Filter = String.Format("(&(objectCategory=group)(sAMAccountName={0}))", NomGroupe)


                Using collectionResultats As SearchResultCollection = dsSearcher.FindAll()
                    Dim result As SearchResult = Nothing
                    If collectionResultats.Count > 0 Then result = collectionResultats(0)

                    If result Is Nothing Then
                        Return Nothing
                    End If

                    Dim sbGroupSIDs As New StringBuilder

                    sbGroupSIDs.Append("(|")

                    Dim deUser As DirectoryEntry = result.GetDirectoryEntry

                    Try
                        deUser.RefreshCache(New String() {"tokenGroups"})
                        For Each sid As Byte() In deUser.Properties("tokenGroups")
                            sbGroupSIDs.AppendFormat("(objectSid={0})", ConvertirOctetString(sid))
                        Next
                    Finally
                        deUser.Dispose()
                    End Try


                    sbGroupSIDs.Append(")")

                    If sbGroupSIDs.ToString <> "(|)" Then
                        Using ds As New DirectorySearcher(deRoot, sbGroupSIDs.ToString, New String() {"memberof", "sAMAccountName"})
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
                searcher.Filter = "(&(objectCategory=group)(sAMAccountName=" + Filtre + "))"
                searcher.PageSize = 1000 ' Pour ne pas être limité a 1000 elements lors des recherches - On peux obtenir plusieurs page de 1000 elements....

                Resultats = searcher.FindAll()
            End Using
        End Using

        Return Resultats
    End Function


    Public Function VerifierGroupeExiste(ByVal strGroupe As String) As Boolean
        Dim result As SearchResult = Nothing

        Using deRoot As New DirectoryEntry
            Using dsSearcher As New DirectorySearcher

                deRoot.AuthenticationType = AuthenticationTypes.Secure
                deRoot.Path = String.Format("LDAP://{0}", NmServeurAD())

                dsSearcher.CacheResults = False
                dsSearcher.SearchRoot = deRoot
                dsSearcher.Filter = String.Format("(&(objectCategory=group)(sAMAccountName={0}))", strGroupe)

                result = dsSearcher.FindOne()
            End Using
        End Using

        Return (Not result Is Nothing)
    End Function



    Private Sub ObtenirObjetsRacine()
        If m_deRoot Is Nothing Then
            m_deRoot = New DirectoryEntry
            m_deRoot.AuthenticationType = AuthenticationTypes.Secure
            m_deRoot.Path = String.Format("LDAP://{0}", NmServeurAD())
        End If

        If m_dsRootSearch Is Nothing Then
            m_dsRootSearch = New DirectorySearcher
            m_dsRootSearch.PageSize = 1000 ' Pour ne pas être limité a 1000 elements lors des recherches - On peux obtenir plusieurs page de 1000 elements....
            m_dsRootSearch.SearchRoot = m_deRoot

            For intCtr As Integer = 0 To mProprietes.Length - 1
                m_dsRootSearch.PropertiesToLoad.Add(mProprietes(intCtr))
            Next

            'important que cette propriété soit la dernière
            m_dsRootSearch.PropertiesToLoad.Add("objectCategory")
        End If

        If m_dsGrpSearch Is Nothing Then
            m_dsGrpSearch = New DirectorySearcher
            m_dsGrpSearch.CacheResults = False
            m_dsGrpSearch.SearchRoot = m_deRoot
            m_dsGrpSearch.PropertyNamesOnly = True
            m_dsGrpSearch.PropertiesToLoad.Add("name")
            m_dsGrpSearch.ReferralChasing = ReferralChasingOption.None
        End If
    End Sub

    Private Sub DetruireObjetsRacine()
        If Not m_deRoot Is Nothing Then m_deRoot.Dispose()
        If Not m_dsRootSearch Is Nothing Then m_dsRootSearch.Dispose()
        If Not m_dsGrpSearch Is Nothing Then m_dsGrpSearch.Dispose()

        m_deRoot = Nothing
        m_dsRootSearch = Nothing
        m_dsGrpSearch = Nothing
    End Sub

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