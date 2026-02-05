Imports System.ServiceModel
Imports Rrq.CS.ServicesCommuns.ScenarioTransactionnel
Imports Rrq.InfrastructureCommune.ScenarioTransactionnel
Imports TS1N215_INiveauSecrt9
Imports TS1N224 = TS1N224_CbCodAccGen

'''-----------------------------------------------------------------------------
''' Project		: TS1N215_CiCodAccGen
''' Class		: TsCaNiveauSecrt9
''' 	
'''-----------------------------------------------------------------------------
''' <summary>
''' Classe d'affaire.
''' </summary>
'''-----------------------------------------------------------------------------
<ServiceBehavior(ConcurrencyMode:=ConcurrencyMode.Single, InstanceContextMode:=InstanceContextMode.PerCall, AddressFilterMode:=AddressFilterMode.Any)> _
Public Class TsCaNiveauSecrt9
    Inherits ClassesBaseIntegration.XuCaBaseComposantV2
	Implements TsICompI

    <OperationBehavior(TransactionScopeRequired:=false, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)> _
	Private Function ObtenirListeCle(ByRef ChaineContexte as String) as Generic.IList(Of TS1N201_DtCdAccGenV1.TsDtCleSym) Implements TsICompI.ObtenirListeCle
		
        Dim listeCle As Generic.IList(Of TS1N201_DtCdAccGenV1.TsDtCleSym)
        listeCle = New Generic.List(Of TS1N201_DtCdAccGenV1.TsDtCleSym)

        Try

            listeCle = TS1N224.TsCaCodAccGen.ObtenirListeCle(ChaineContexte)

        Catch ex As XuExcEErrFonctionnelle
            MyBase.CreerRetourErrFonctionnelle(ChaineContexte, ex)
        Catch ex As XuExcEErrValidation
            'Pour les applications Client/Serveur, on doit convertir l'exception XU en exception XZ.
            MyBase.CreerRetourErrValidation(ChaineContexte, New XZCuErrValdtException(ex))
        End Try

        Return listeCle

	End Function

	
	<OperationBehavior(TransactionScopeRequired:=false, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)> _
	Private Function ObtenirCle(ByRef ChaineContexte as String, ByVal IdCle as String) as TS1N201_DtCdAccGenV1.TsDtCleSym Implements TsICompI.ObtenirCle

        Dim cle As TS1N201_DtCdAccGenV1.TsDtCleSym = Nothing

        Try

            cle = TS1N224.TsCaCodAccGen.ObtenirCle(ChaineContexte, IdCle)

        Catch ex As XuExcEErrFonctionnelle
            MyBase.CreerRetourErrFonctionnelle(ChaineContexte, ex)
        Catch ex As XuExcEErrValidation
            'Pour les applications Client/Serveur, on doit convertir l'exception XU en exception XZ.
            MyBase.CreerRetourErrValidation(ChaineContexte, New XZCuErrValdtException(ex))
        End Try

        Return cle

	End Function

	
	<OperationBehavior(TransactionScopeRequired:=false, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)> _
	Private Function ObtenirCles(ByRef ChaineContexte as String, ByVal IdCleParent as String) as Generic.IList(Of TS1N201_DtCdAccGenV1.TsDtCleSym) Implements TsICompI.ObtenirCles

        Dim listeCle As Generic.IList(Of TS1N201_DtCdAccGenV1.TsDtCleSym)
        listeCle = New Generic.List(Of TS1N201_DtCdAccGenV1.TsDtCleSym)

        Try

            listeCle = TS1N224.TsCaCodAccGen.ObtenirCles(ChaineContexte, IdCleParent)

        Catch ex As XuExcEErrFonctionnelle
            MyBase.CreerRetourErrFonctionnelle(ChaineContexte, ex)
        Catch ex As XuExcEErrValidation
            'Pour les applications Client/Serveur, on doit convertir l'exception XU en exception XZ.
            MyBase.CreerRetourErrValidation(ChaineContexte, New XZCuErrValdtException(ex))
        End Try

        Return listeCle

	End Function

	
	<OperationBehavior(TransactionScopeRequired:=false, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)> _
	Private Function ObtenirCleRecherche(ByRef ChaineContexte as String, ByVal CoTypCle as String, ByVal CoTypEnv as String, ByVal GroupeAd as String, ByVal IdCle as String, ByVal UsagerAd as String) as DataTable Implements TsICompI.ObtenirCleRecherche
		
		Try

            Return TS1N224.TsCaCodAccGen.ObtenirCleRecherche(ChaineContexte, CoTypCle, CoTypEnv, GroupeAd, IdCle, UsagerAd)

		Catch ex as XuExcEErrFonctionnelle
			MyBase.CreerRetourErrFonctionnelle(ChaineContexte, ex)
		Catch ex as XuExcEErrValidation
			'Pour les applications Client/Serveur, on doit convertir l'exception XU en exception XZ.
			MyBase.CreerRetourErrValidation(ChaineContexte, New XZCuErrValdtException(ex))
		End Try
		
		Return Nothing
		
	End Function

	
	<OperationBehavior(TransactionScopeRequired:=false, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)> _
	Private Function ObtenirIndicateursCreationCompte(ByRef ChaineContexte as String) as TS1N201_DtCdAccGenV1.TsDtIndCreCpt Implements TsICompI.ObtenirIndicateursCreationCompte
		
		Try
			
            Return TS1N224.TsCaCodAccGen.ObtenirIndicateursCreationCompte(ChaineContexte)

		Catch ex as XuExcEErrFonctionnelle
			MyBase.CreerRetourErrFonctionnelle(ChaineContexte, ex)
		Catch ex as XuExcEErrValidation
			'Pour les applications Client/Serveur, on doit convertir l'exception XU en exception XZ.
			MyBase.CreerRetourErrValidation(ChaineContexte, New XZCuErrValdtException(ex))
		End Try
		
		Return Nothing
		
	End Function

	
	<OperationBehavior(TransactionScopeRequired:=false, ReleaseInstanceMode:=ReleaseInstanceMode.BeforeAndAfterCall)> _
	Private Function ObtenirEtatFichierExportation(ByRef ChaineContexte as String) as TS1N201_DtCdAccGenV1.TsDtEtaFicExp Implements TsICompI.ObtenirEtatFichierExportation
		
		Try
			
            Return TS1N224.TsCaCodAccGen.ObtenirEtatFichierExportation(ChaineContexte)

		Catch ex as XuExcEErrFonctionnelle
			MyBase.CreerRetourErrFonctionnelle(ChaineContexte, ex)
		Catch ex as XuExcEErrValidation
			'Pour les applications Client/Serveur, on doit convertir l'exception XU en exception XZ.
			MyBase.CreerRetourErrValidation(ChaineContexte, New XZCuErrValdtException(ex))
		End Try
		
		Return Nothing
		
	End Function


End Class
