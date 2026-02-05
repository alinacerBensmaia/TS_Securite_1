Imports System.Globalization
Imports RIS = Rrq.InfrastructureCommune.ScenarioTransactionnel

Friend NotInheritable Class TsCuRecherche
    Private Sub New()
        'Ne peut pas faire new sur la classe de méthodes shared (static)
    End Sub

    ''' <summary>
    ''' Retourne une table de correspondance permettant d'obtenir la description
    ''' de chaque type d'environnement.
    ''' </summary>
    ''' <returns></returns>
    Friend Shared Function RemplirDataViewEnvironnement() As DataView
        Dim vueEnvrn As DataView = Nothing

        vueEnvrn = New DataView(TsCuPrGerAccGen.TableEnv)

        Return vueEnvrn
    End Function

    ''' <summary>
    ''' Retourne une table de correspondance permettant d'obtenir la description de chaque type.
    ''' </summary>
    ''' <returns>Dataview contenant la liste des types de cle</returns>
    Friend Shared Function RemplirDataViewType() As DataView
        Dim vueType As DataView

        vueType = New DataView(TsCuPrGerAccGen.RemplirTableTypeCle(False))

        Return vueType
    End Function

    Friend Shared Function ObtenirCleRecherche(ByVal pCoTypeCle As String, _
                                               ByVal pCoTypeEnv As String, _
                                               ByVal pGroupeAd As String, _
                                               ByVal pIdCle As String, _
                                               ByVal pUsagerAd As String) As DataTable
        Dim CaAffaire As TS1N215_INiveauSecrt9.TsICompI
        Dim chaineContexte As String = String.Empty
        Dim contexte As Object = Nothing
        Dim dt As DataTable

        Using appel As New RIS.XuCuAppelerCompI(Of TS1N215_INiveauSecrt9.TsICompI)

            chaineContexte = appel.PreparerAppel(contexte, TsCuPrGerAccGen.ObtenirCodeEnv())

            CaAffaire = appel.CreerComposantIntegration(chaineContexte)
            dt = CaAffaire.ObtenirCleRecherche(chaineContexte, _
                                               pCoTypeCle, _
                                               pCoTypeEnv, _
                                               pGroupeAd, _
                                               pIdCle, _
                                               pUsagerAd)
            appel.AnalyserRetour(chaineContexte, Nothing)
        End Using

        Return dt
    End Function

    ''' <summary>
    ''' Retourne un gabarit de table de donnée vide.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Le gabarit permet d'ajouté les colonnes à la liste de façon "bound"</remarks>
    Friend Shared Function ObtenirGrilleRechercheVide() As DataTable

        '-------------------------------------------------------------------------------
        ' Créer un gabarit de donnée vide pour ajouter les colonnes.
        ' ------------------------------------------------------------------------------
        Dim dtGabarit As New DataTable

        dtGabarit.Locale = CultureInfo.InvariantCulture
        dtGabarit.Columns.Add("Type")
        dtGabarit.Columns.Add("Cle")
        dtGabarit.Columns.Add("Env")
        dtGabarit.Columns.Add("Code")
        dtGabarit.Columns.Add("Profil")

        Return dtGabarit

    End Function
End Class
