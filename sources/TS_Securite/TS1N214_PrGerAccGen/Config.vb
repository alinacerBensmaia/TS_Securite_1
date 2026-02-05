Imports Rrq.InfrastructureCommune.Parametres

Friend Class Config

    Public Shared ReadOnly Property TypesConnexion As String()
        Get
            Return XuCuConfiguration.ClefsSysteme("TS1", "TS1\TS1N214\TypesConnexion")
        End Get
    End Property

    Public Shared Function ObtenirValeurs(cle As String) As String()
        Return XuCuConfiguration.ObtenirValeurSystemeOptionnelle("TS1", cle).Split(";"c)
    End Function

    Public Shared ReadOnly Property PrefixeRechercheProfils As String
        Get
            Return XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N214\PrefixeRechercheProfils")
        End Get
    End Property

    Public Shared ReadOnly Property AfficherMenuImporter As Boolean
        Get
            Return (XuCuPolitiqueConfig.ConfigDomaine.ObtenirValeurSystemeOptionnelle("TS1", "TS1N214\AfficherMenuImporter", "False") = "True")
        End Get
    End Property
End Class
