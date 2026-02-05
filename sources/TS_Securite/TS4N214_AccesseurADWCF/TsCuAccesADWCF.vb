Imports TS4N215_IObtnrInfoAD

Public Class TsCuAccesADWCF

    Public Function RechercheActiveDirectory(ByVal NomServeurAD As String, ByVal pTypeRequete As Integer,
                                      ByVal strCritereRecherche As String, ByVal strCritereRechercheSecondaire As String,
                                      ByVal pObjectCategory As Integer) As DataTable
        Using appelLAF As New TsCuProxyAccesseurADWCF()
            Return appelLAF.RechercheActiveDirectory(NomServeurAD, CType(pTypeRequete, TS4N215_IObtnrInfoAD.TsIAccesseurADWCF.TsIadTypeRequete), strCritereRecherche, strCritereRechercheSecondaire, CType(pObjectCategory, TS4N215_IObtnrInfoAD.TsIAccesseurADWCF.TsIadObjectCategory))
        End Using
    End Function

    Public Function RechercheGroupeAD(ByVal NomServeurAD As String, ByVal strGroupe As String, ByVal blnRechRecursive As Boolean) As DataTable
        Using appelLAF As New TsCuProxyAccesseurADWCF()
            Return appelLAF.RechercheGroupeAD(NomServeurAD, strGroupe, blnRechRecursive)
        End Using
    End Function

    Public Function ChercheDansGroupes(ByVal NomServeurAD As String, ByVal strACID As String, ByVal strGroupeRecherche As String) As Boolean
        Using appelLAF As New TsCuProxyAccesseurADWCF()
            Return appelLAF.ChercheDansGroupes(NomServeurAD, strACID, strGroupeRecherche)
        End Using
    End Function

    Public Function ObtenirMembresGroupe(ByVal NomServeurAD As String, ByVal NomGroupe As String) As String()
        Using appelLAF As New TsCuProxyAccesseurADWCF()
            Return appelLAF.ObtenirMembresGroupe(NomServeurAD, NomGroupe)
        End Using
    End Function

    Public Function VerifierGroupeExiste(ByVal NomServeurAD As String, ByVal strGroupe As String) As Boolean
        Using appelLAF As New TsCuProxyAccesseurADWCF()
            Return appelLAF.VerifierGroupeExiste(NomServeurAD, strGroupe)
        End Using
    End Function

    Public Function DomaineNT(ByVal NomServeurAD As String) As String
        Using appelLAF As New TsCuProxyAccesseurADWCF()
            Return appelLAF.DomaineNT(NomServeurAD)
        End Using
    End Function

End Class
