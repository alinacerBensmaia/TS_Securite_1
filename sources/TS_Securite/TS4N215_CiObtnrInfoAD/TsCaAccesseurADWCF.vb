Imports System.ServiceModel
Imports Rrq.InfrastructureCommune.ScenarioTransactionnel
Imports System.DirectoryServices
Imports System.Text
Imports TS4N215_IObtnrInfoAD.TsIAccesseurADWCF.TsIadTypeRequete
Imports TS4N215_IObtnrInfoAD.TsIAccesseurADWCF.TsIadObjectCategory
Imports TS4N215_IObtnrInfoAD

'''-----------------------------------------------------------------------------
''' Project		: TS4N214_CiObtnrInfoAD
''' Class		: TsCaAccesseurWCF
''' 	
'''-----------------------------------------------------------------------------
''' <summary>
''' Classe d'affaire.
''' </summary>
'''-----------------------------------------------------------------------------
<ServiceBehavior(ConcurrencyMode:=ConcurrencyMode.Single, InstanceContextMode:=InstanceContextMode.PerCall, AddressFilterMode:=AddressFilterMode.Any)>
Public Class TsCaAccesseurADWCF
    Implements TsIAccesseurADWCF

    Protected Overridable ReadOnly Property ChampCodeUtilisateur As String = "sAMAccountName"
    Protected Overridable ReadOnly Property ChampNom As String = "Sn"
    Protected Overridable ReadOnly Property ChampPrenom As String = "GivenName"
    Protected Overridable ReadOnly Property ChampNomComplet As String = "DisplayName"
    Protected Overridable ReadOnly Property ChampCourriel As String = "Mail"
    Protected Overridable ReadOnly Property ChampUniteAdministrative As String = "Department"
    Protected Overridable ReadOnly Property ChampFonction As String = "Title"
    Protected Overridable ReadOnly Property ChampMembreDe As String = "MemberOf"
    Protected Overridable ReadOnly Property ChampSID As String = "ObjectSid"
    Protected Overridable ReadOnly Property ChampSociete As String = "company"
    Protected Overridable ReadOnly Property ChampDescription As String = "description"
    Protected Overridable ReadOnly Property ChampNumeroEmploye As String = "employeeNumber"
    Protected Overridable ReadOnly Property ChampNumeroTelephone As String = "telephoneNumber"


    Protected Overridable ReadOnly Property ChampEstBacule As String = "EXTENSIONATTRIBUTE12"

    <OperationBehavior(TransactionScopeRequired:=False, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)>
    Public Function RechercheActiveDirectory(NomServeurAD As String, pTypeRequete As TsIAccesseurADWCF.TsIadTypeRequete, strCritereRecherche As String, strCritereRechercheSecondaire As String, pObjectCategory As TsIAccesseurADWCF.TsIadObjectCategory) As DataTable Implements TsIAccesseurADWCF.RechercheActiveDirectory

        Dim attributAdAssocieAuTypeDeRecherche As TsCuTypeRequeteVsAttributAD
        Dim proprieteActiveDirectory As New Proprietes()

        attributAdAssocieAuTypeDeRecherche = New TsCuTypeRequeteVsAttributAD
        attributAdAssocieAuTypeDeRecherche.AjouterCombinaison(TsIAccesseurADWCF.TsIadTypeRequete.TsIadTrCodeUtilisateur, ChampCodeUtilisateur)
        attributAdAssocieAuTypeDeRecherche.AjouterCombinaison(TsIAccesseurADWCF.TsIadTypeRequete.TsIadTrNom, ChampNom)
        attributAdAssocieAuTypeDeRecherche.AjouterCombinaison(TsIAccesseurADWCF.TsIadTypeRequete.TsIadTrPrenom, ChampPrenom)
        attributAdAssocieAuTypeDeRecherche.AjouterCombinaison(TsIAccesseurADWCF.TsIadTypeRequete.TsIadTrNomComplet, ChampNomComplet)
        attributAdAssocieAuTypeDeRecherche.AjouterCombinaison(TsIAccesseurADWCF.TsIadTypeRequete.TsIadTrCourriel, ChampCourriel)
        attributAdAssocieAuTypeDeRecherche.AjouterCombinaison(TsIAccesseurADWCF.TsIadTypeRequete.TsIadTrUniteAdmn, ChampUniteAdministrative)
        attributAdAssocieAuTypeDeRecherche.AjouterCombinaison(TsIAccesseurADWCF.TsIadTypeRequete.TsIadTrFonction, ChampFonction)
        attributAdAssocieAuTypeDeRecherche.AjouterCombinaison(TsIAccesseurADWCF.TsIadTypeRequete.TsIadTrMembreDe, ChampMembreDe)
        attributAdAssocieAuTypeDeRecherche.AjouterCombinaison(TsIAccesseurADWCF.TsIadTypeRequete.TsIadTrSid, ChampSID)
        attributAdAssocieAuTypeDeRecherche.AjouterCombinaison(TsIAccesseurADWCF.TsIadTypeRequete.TsIadTrSociete, ChampSociete)
        attributAdAssocieAuTypeDeRecherche.AjouterCombinaison(TsIAccesseurADWCF.TsIadTypeRequete.TsIadTrDescription, ChampDescription)
        attributAdAssocieAuTypeDeRecherche.AjouterCombinaison(TsIAccesseurADWCF.TsIadTypeRequete.TsIadTrNoEmploye, ChampNumeroEmploye)
        attributAdAssocieAuTypeDeRecherche.AjouterCombinaison(TsIAccesseurADWCF.TsIadTypeRequete.TsIadTrNoTelephone, ChampNumeroTelephone)


        Dim query As String = "{0}"
        If pObjectCategory = TsIadOcPerson Then
            query = "(&(objectCategory=person){0})"
        ElseIf pObjectCategory = TsIadOcGroup Then
            query = "(&(objectCategory=group){0})"
        End If

        Dim specificCriteria As String
        If pTypeRequete = TsIadTrNomEtPrenom Then
            specificCriteria = String.Format("(&(Sn={0})(GivenName={1}))", strCritereRecherche, strCritereRechercheSecondaire)
        Else
            specificCriteria = String.Format("({0}={1})", attributAdAssocieAuTypeDeRecherche.ObtenirCombinaison(pTypeRequete), strCritereRecherche)
        End If


        Using dt As New DataTable("TblInfoActiveDir")
            Using root As New DirectoryEntry()
                root.AuthenticationType = AuthenticationTypes.Secure
                root.Path = String.Format("LDAP://{0}", NomServeurAD)

                Using searcher As New DirectorySearcher
                    searcher.CacheResults = False
                    searcher.SearchRoot = root
                    searcher.PageSize = 1000 ' Pour ne pas être limité a 1000 elements lors des recherches - On peux obtenir plusieurs page de 1000 elements....
                    searcher.Filter = String.Format(query, specificCriteria)

                    For Each prop As Propriete In proprieteActiveDirectory
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
                            For Each prop As Propriete In proprieteActiveDirectory
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

    <OperationBehavior(TransactionScopeRequired:=False, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)>
    Public Function RechercheGroupeAD(NomServeurAD As String, strGroupe As String, blnRechRecursive As Boolean) As DataTable Implements TsIAccesseurADWCF.RechercheGroupeAD

        Dim proprieteActiveDirectory As New Proprietes()

        Using dt As New DataTable("TblInfoActiveDir")
            For Each prop As Propriete In proprieteActiveDirectory
                dt.Columns.Add(New DataColumn(prop.NomSql, prop.TypeSql))
            Next

            Using root As New DirectoryEntry()
                root.AuthenticationType = AuthenticationTypes.Secure
                root.Path = String.Format("LDAP://{0}", NomServeurAD)

                Using searcher As New DirectorySearcher(root)
                    searcher.PageSize = 1000 ' Pour ne pas être limité a 1000 elements lors des recherches - On peux obtenir plusieurs page de 1000 elements....
                    For Each prop As Propriete In proprieteActiveDirectory
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

                                            For Each prop As Propriete In proprieteActiveDirectory
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
                                        Dim objDTAdd As DataTable = RechercheGroupeAD(NomServeurAD, srMembre.Properties("sAMAccountName")(0).ToString(), blnRechRecursive)

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

    <OperationBehavior(TransactionScopeRequired:=False, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)>
    Public Function ChercheDansGroupes(NomServeurAD As String, strACID As String, strGroupeRecherche As String) As Boolean Implements TsIAccesseurADWCF.ChercheDansGroupes
        Using root As New DirectoryEntry
            root.AuthenticationType = AuthenticationTypes.Secure
            root.Path = String.Format("LDAP://{0}", NomServeurAD)

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

    <OperationBehavior(TransactionScopeRequired:=False, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)>
    Public Function ObtenirMembresGroupe(NomServeurAD As String, NomGroupe As String) As String() Implements TsIAccesseurADWCF.ObtenirMembresGroupe
        Dim ListeGroupe() As String = Nothing

        Using root As New DirectoryEntry
            root.AuthenticationType = AuthenticationTypes.Secure
            root.Path = String.Format("LDAP://{0}", NomServeurAD)

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


                    Dim sbGroupSIDs As New Text.StringBuilder

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

    <OperationBehavior(TransactionScopeRequired:=False, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)>
    Public Function VerifierGroupeExiste(ByVal NomServeurAD As String, strGroupe As String) As Boolean Implements TsIAccesseurADWCF.VerifierGroupeExiste
        Dim result As SearchResult = Nothing

        Using root As New DirectoryEntry
            root.AuthenticationType = AuthenticationTypes.Secure
            root.Path = String.Format("LDAP://{0}", NomServeurAD)

            Using searcher As New DirectorySearcher
                searcher.CacheResults = False
                searcher.SearchRoot = root
                searcher.Filter = String.Format("(&(objectCategory=group)(sAMAccountName={0}))", strGroupe)

                result = searcher.FindOne()
            End Using
        End Using

        Return (Not result Is Nothing)
    End Function


    Public Function DomaineNT(ByVal NomServeurAD As String) As String Implements TsIAccesseurADWCF.DomaineNT

        Dim domNT As String = String.Empty

        Using de As New DirectoryEntry(String.Format("LDAP://{0}", NomServeurAD))
            Using searcher As New DirectorySearcher(de)
                searcher.SearchScope = SearchScope.Base
                searcher.PropertiesToLoad.Add("msDS-PrincipalName")
                searcher.Filter = "(objectClass=*)"

                Dim result As SearchResult = searcher.FindOne()
                domNT = result.Properties("msDS-PrincipalName")(0).ToString().ToUpper().Replace("\", String.Empty)
            End Using

            Return domNT
        End Using
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

End Class
