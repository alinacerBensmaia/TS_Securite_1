Imports Rrq.InfrastructureCommune.Parametres

Namespace AccesSecurite

    Friend Class Fabrique

        Public Shared Function CreerAccesSecuriteTS1() As IDepotSecurite
            Dim contenuADLDS As Boolean = (XuCuPolitiqueConfig.ConfigDomaine.ObtenirValeurSystemeOptionnelle("TS1", "TS1\TS1N214\SecrtTS1MigreVersADLDS") = "O")
            Return creerAccesSecurite(contenuADLDS)
        End Function

        Public Shared Function CreerAccesSecuriteROI() As IDepotSecurite
            Dim contenuADLDS As Boolean = (XuCuPolitiqueConfig.ConfigDomaine.ObtenirValeurSystemeOptionnelle("TS1", "TS1\TS1N214\SecrtROIMigreVersADLDS") = "O")
            Return creerAccesSecurite(contenuADLDS)
        End Function

        Private Shared Function creerAccesSecurite(estADLDS As Boolean) As IDepotSecurite
            If estADLDS Then Return New NouveauCodeAccesADLDS()
            Return New AncienCodeAccesAD()
        End Function

    End Class

End Namespace
