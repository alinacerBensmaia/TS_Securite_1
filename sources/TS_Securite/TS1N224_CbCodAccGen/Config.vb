Imports Rrq.InfrastructureCommune.Parametres

Friend Class Config
    Private Const OUI As String = "O"
    Private Const NON As String = "N"

    Public Shared ReadOnly Property PASSWDFILEPROD As String
        Get
            Return XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N224\PASSWDFILEPROD")
        End Get
    End Property

    Public Shared ReadOnly Property PASSWDFILEPRODPUB As String
        Get
            Return XuCuConfiguration.ObtenirValeurSystemeOptionnelle("TS1", "TS1\TS1N224\PASSWDFILEPRODPUB")
        End Get
    End Property

    Public Shared ReadOnly Property PASSWDFILEINFORTEPROD As String
        Get
            Return XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N224\PASSWDFILEINFORTEPROD")
        End Get
    End Property
    Public Shared ReadOnly Property PASSWDFILEZDEPROD As String
        Get
            Return XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N224\PASSWDFILEZDEPROD")
        End Get
    End Property
    Public Shared ReadOnly Property PASSWDFILEZEAPROD As String
        Get
            Return XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N224\PASSWDFILEZEAPROD")
        End Get
    End Property

    Public Shared ReadOnly Property PASSWDFILEUNIT As String
        Get
            Return XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N224\PASSWDFILEUNIT")
        End Get
    End Property

    Public Shared ReadOnly Property PASSWDFILEINFORTEUNIT As String
        Get
            Return XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N224\PASSWDFILEINFORTEUNIT")
        End Get
    End Property
    Public Shared ReadOnly Property PASSWDFILEZDEUNIT As String
        Get
            Return XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N224\PASSWDFILEZDEUNIT")
        End Get
    End Property
    Public Shared ReadOnly Property PASSWDFILEZEAUNIT As String
        Get
            Return XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N224\PASSWDFILEZEAUNIT")
        End Get
    End Property

    Public Shared ReadOnly Property PASSWDFILELIBRAIRE As String
        Get
            Return XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N224\PASSWDFILELIBRAIRE")
        End Get
    End Property

    Public Shared ReadOnly Property NomCertificatPROD As String
        Get
            Return XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N224\NomCertificatPROD")
        End Get
    End Property

    Public Shared ReadOnly Property ThumbPrintCertificatPROD As String
        Get
            Return XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N224\ThumbPrintCertificatPROD")
        End Get
    End Property

    Public Shared ReadOnly Property NomCertificatPrivate As String
        Get
            Return XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N224\NomCertificatPrivate")
        End Get
    End Property

    Public Shared ReadOnly Property NomCertificatUNIT As String
        Get
            Return XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N224\NomCertificatUNIT")
        End Get
    End Property

    Public Shared ReadOnly Property ThumbPrintCertificatUNIT As String
        Get
            Return XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N224\ThumbPrintCertificatUNIT")
        End Get
    End Property

    Public Shared ReadOnly Property CleSymboliqueLibraire As String
        Get
            Return XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N224\CleSymboliqueLibraire")
        End Get
    End Property


    Public Shared ReadOnly Property SystemesExportationZEA As String()
        Get
            Return XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N224\SystemesExportationZEA").Split(","c)
        End Get
    End Property

    Public Shared ReadOnly Property CreerCompteAD As Boolean
        Get
            Return (XuCuConfiguration.ObtenirValeurSystemeOptionnelle("TS1", "TS1\TS1N224\CreerCompteAD", OUI) = OUI)
        End Get
    End Property
    Public Shared ReadOnly Property CreerCompteADLDS As Boolean
        Get
            Return (XuCuConfiguration.ObtenirValeurSystemeOptionnelle("TS1", "TS1\TS1N224\CreerCompteADLDS", OUI) = OUI)
        End Get
    End Property
    Private Shared ReadOnly Property UtiliserDomaineRQ As Boolean
        Get
            Return (XuCuConfiguration.ObtenirValeurSystemeOptionnelle("TS1", "TS1\TS1N224\UtiliserDomaineRQ", NON) = OUI)
        End Get
    End Property

    Public Shared ReadOnly Property DomaineAD As String
        Get
            If UtiliserDomaineRQ Then
                Return XuCuPolitiqueConfig.ConfigDomaine.ValeurSysteme("General", "ServeurActiveDirectory")
            Else
                Return XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N224\DomaineCnxLdapAD")
            End If
        End Get
    End Property
    Public Shared ReadOnly Property CompteAD As String
        Get
            Return XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N224\CompteCnxLdapAD")
        End Get
    End Property
    Public Shared ReadOnly Property ContainerAD As String
        Get
            Return XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N224\ContainerCnxLdapAD")
        End Get
    End Property

    Public Shared ReadOnly Property DomaineADLDS As String
        Get
            Return XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N224\DomaineCnxLdapADLDS")
        End Get
    End Property
    Public Shared ReadOnly Property CompteADLDS As String
        Get
            Return XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N224\CompteCnxLdapADLDS")
        End Get
    End Property
    Public Shared ReadOnly Property ContainerADLDS As String
        Get
            Return XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N224\ContainerCnxLdapADLDS")
        End Get
    End Property
    Friend Shared Function ObtenirValeurDe(cleConfiguration As String) As String
        Return XuCuConfiguration.ValeurSysteme("TS1", "TS1\TS1N224\" & cleConfiguration)
    End Function

End Class
