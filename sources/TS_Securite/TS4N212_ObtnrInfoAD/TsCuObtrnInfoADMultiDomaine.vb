Imports Rrq.InfrastructureCommune.Parametres
Imports System.DirectoryServices
Imports System.Collections.Generic

Friend Class TsCuObtnrInfoADMultiDomaine
    Implements TsIObtnrInfoAD

    Private objetADRRQ As TsIObtnrInfoAD
    Private objetADCARRA As TsIObtnrInfoAD
    Private _accesseurADCARRA As TsCuAccesseurAD
    Private _relationTypeReqAttributAD As TsCuTypeRequeteVsAttributAD
    Private Const ADS_UF_ACCOUNTDISABLE As Integer = &H2


    Sub New()
        objetADRRQ = New TsCuObtnrInfoADRRQ()
        objetADCARRA = New TsCuObtnrInfoADCARRA()
    End Sub

#Region " Propriétés "
    Public ReadOnly Property Domaine As TsIadNomDomaine Implements TsIObtnrInfoAD.Domaine
        Get
            Return TsIadNomDomaine.TsMultiDomaine
        End Get
    End Property

#End Region

#Region " Fonctions et méthodes publiques "

    Public Function ObtenirUtilisateur(ByVal strCodeUtilisateur As String) As TsCuUtilisateurAD Implements TsIObtnrInfoAD.ObtenirUtilisateur

        Try
            Return objetADRRQ.ObtenirUtilisateur(strCodeUtilisateur)
        Catch exRRQ As TsCuCodeUtilisateurInexistantException
            Try
                Return objetADCARRA.ObtenirUtilisateur(strCodeUtilisateur)
            Catch exCARRA As Exception
                Throw exCARRA
            End Try
        Catch exRRQ As Exception
            Throw exRRQ
        End Try

    End Function


    Public Function ObtenirListeGroupes(ByVal Filtre As String) As SearchResultCollection Implements TsIObtnrInfoAD.ObtenirListeGroupes
        Throw New TsCuMethodeNonSupporteMultipleDomaine("ObtenirListeGroupes")
    End Function


    Public Function ObtenirListeUtilisateur(ByVal pTypeRequete As TsIadTypeRequete, ByVal pCritereRecherche As String, _
                                       ByVal pCritereRechercheSecondaire As String, _
                                       ByVal pCategorie As TsIadObjectCategory) As List(Of TsCuUtilisateurAD) Implements TsIObtnrInfoAD.ObtenirListeUtilisateur

        Dim listeUtilisateur As New List(Of TsCuUtilisateurAD)
        listeUtilisateur = objetADRRQ.ObtenirListeUtilisateur(pTypeRequete, pCritereRecherche, pCritereRechercheSecondaire, pCategorie)
        listeUtilisateur.AddRange(objetADCARRA.ObtenirListeUtilisateur(pTypeRequete, pCritereRecherche, pCritereRechercheSecondaire, pCategorie))

        Return listeUtilisateur


    End Function


    Public Function VerifierGroupeExiste(ByVal strGroupe As String) As Boolean Implements TsIObtnrInfoAD.VerifierGroupeExiste
        Throw New TsCuMethodeNonSupporteMultipleDomaine("VerifierGroupeExiste")

        'If Not objetADRRQ.VerifierGroupeExiste(strGroupe) Then
        '    Return objetADCARRA.VerifierGroupeExiste(strGroupe)
        'End If

        'Return True

    End Function

    Public Function ObtenirListeMembreGroupe(ByVal strGroupe As String, ByVal blnRechRecursive As Boolean) As List(Of TsCuUtilisateurAD) Implements TsIObtnrInfoAD.ObtenirListeMembreGroupe

        Throw New TsCuMethodeNonSupporteMultipleDomaine("ObtenirListeMembreGroupe")

        'Dim listeUtilisateur As New List(Of TsCuUtilisateurAD)
        'Try
        '    listeUtilisateur = objetADRRQ.ObtenirListeMembreGroupe(strGroupe, blnRechRecursive)
        '    Try
        '        listeUtilisateur.AddRange(objetADCARRA.ObtenirListeMembreGroupe(strGroupe, blnRechRecursive))
        '    Catch CARRA As Exception
        '    End Try
        'Catch exRRQ As Exception
        '    Try
        '        listeUtilisateur = objetADCARRA.ObtenirListeMembreGroupe(strGroupe, blnRechRecursive)
        '    Catch exCARRA As Exception
        '    End Try
        'End Try

        'Return listeUtilisateur

    End Function


    Public Function RechercheGroupeAD(ByVal strGroupe As String, ByVal blnRechRecursive As Boolean) As DataTable Implements TsIObtnrInfoAD.RechercheGroupeAD
        Throw New TsCuMethodeNonSupporteMultipleDomaine("RechercheGroupeAD")

        Dim DTRetour As DataTable

        Try
            DTRetour = objetADRRQ.RechercheGroupeAD(strGroupe, blnRechRecursive)
            DTRetour.Merge(objetADCARRA.RechercheGroupeAD(strGroupe, blnRechRecursive))
            Return DTRetour
        Finally
            If Not IsNothing(DTRetour) Then
                DTRetour.Dispose()
                DTRetour = Nothing
            End If
        End Try

    End Function

    Public Function ChercheDansGroupes(ByVal strACID As String, ByVal strGroupeRecherche As String) As Boolean Implements TsIObtnrInfoAD.ChercheDansGroupes

        Throw New TsCuMethodeNonSupporteMultipleDomaine("ObtenirListeMembreGroupe")

        'If Not objetADRRQ.ChercheDansGroupes(strACID, strGroupeRecherche) Then
        '    Return objetADCARRA.ChercheDansGroupes(strACID, strGroupeRecherche)
        'End If

        'Return True
    End Function


    Public Function RechercheActiveDirectory(ByVal pTypeRequete As TsIadTypeRequete, ByVal strCritereRecherche As String, _
                                      Optional ByVal strCritereRechercheSecondaire As String = "", _
                                      Optional ByVal pObjectCategory As TsIadObjectCategory = TsIadObjectCategory.TsIadOcTous) As DataTable Implements TsIObtnrInfoAD.RechercheActiveDirectory

        Dim DTRetour As DataTable

        Try
            DTRetour = objetADRRQ.RechercheActiveDirectory(pTypeRequete, strCritereRecherche, strCritereRechercheSecondaire, pObjectCategory)
            DTRetour.Merge(objetADCARRA.RechercheActiveDirectory(pTypeRequete, strCritereRecherche, strCritereRechercheSecondaire, pObjectCategory))
            Return DTRetour

        Finally
            If Not IsNothing(DTRetour) Then
                DTRetour.Dispose()
                DTRetour = Nothing
            End If
        End Try

    End Function

    Public Function ObtenirMembresGroupe(ByVal NomGroupe As String) As String() Implements TsIObtnrInfoAD.ObtenirMembresGroupe
        Throw New TsCuMethodeNonSupporteMultipleDomaine("ObtenirMembresGroupe")
        'Dim membresGroupeRRQ As String() = objetADRRQ.ObtenirMembresGroupe(NomGroupe)
        'Dim membresGroupeCARRA As String() = objetADCARRA.ObtenirMembresGroupe(NomGroupe)
        'Dim AList As ArrayList = New ArrayList(membresGroupeRRQ)
        'AList.AddRange(membresGroupeCARRA)
        'Return CType(AList.ToArray, String())
    End Function
#End Region

#Region " Fonctions et méthodes privés "

    Private Sub CombinerTypeRequeteVSAttributAD() Implements TsIObtnrInfoAD.CombinerTypeRequeteVSAttributAD
    End Sub

    Private ReadOnly Property NomServeur() As String Implements TsIObtnrInfoAD.NomServeur
        Get
            Return String.Empty
        End Get
    End Property

#End Region
End Class
