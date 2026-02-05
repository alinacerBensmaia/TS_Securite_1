Imports Rrq.InfrastructureCommune.Parametres

Friend Class TsCuObtnrInfoADRQ
    Inherits TsCuBaseIObtnrInfoAD
    Implements TsIObtnrInfoAD


    Sub New()
        MyBase.New()
    End Sub

    Protected Overrides ReadOnly Property Domaine As TsIadNomDomaine
        Get
            Return Domaines.Rq.EnumValue
        End Get
    End Property

    Protected Overrides ReadOnly Property NomServeur() As String
        Get
            Return XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("General", "ServeurActiveDirectory")
        End Get
    End Property

End Class
