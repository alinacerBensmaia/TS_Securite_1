Imports Rrq.Securite.GestionAcces.portailsage
Imports Rrq.Securite.GestionAcces.portailsage1
Imports Rrq.Securite.GestionAcces.portailsage2

' Méthodes en lien avec les métadonnées de Sage (liste des configurations, leurs paramètres, etc...)
Public Module TsBaMetaSage

    ''' <summary>
    ''' Obtient les paramètres d'une configuration de Sage (nom des BD, etc...).
    ''' </summary>
    Public Function cfg_get_databases(ByVal cfgName As String) As TsCdSageConfiguration
        ' Ici on doit faire une transformation des données pour réparer des incohrences dans le Xml retourné par Sage
        Dim env As IncoherenceSage.TsCdSageEnveloppeConfigurationSeule = _
                        call_sage(Of SageDataService, IncoherenceSage.TsCdSageEnveloppeConfigurationSeule)("cfg_get_databases", cfgName)
        ' Pourquoi Sage a mis un tag bidon???
        Dim cfg As IncoherenceSage.TsCdSageConfigurationSeule = env.Config
        ' On fait une simple copie,
        ' les champs de TsCdSageConfiguration et IncoherenceSage.TsCdSageConfigurationSeule sont compatibles
        Return Copier(Of TsCdSageConfiguration)(cfg)
    End Function

End Module
