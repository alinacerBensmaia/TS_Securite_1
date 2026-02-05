Imports Cfg = Rrq.InfrastructureCommune.Parametres.XuCuConfiguration

Public Class TsCuParamsChiffrement

    Private Const PREFIX_FICHIER_CHIFFR As String = "TS6Chiffrement"
    Private Const PREFIX_FICHIER_CERTF As String = "TS6Certificat"
    Private Const PREFIX_FICHIER_SEL As String = "TS6Sel"

    Public Enum Envrn
        Acceptation = 0
        Formation_Acceptation = 1
        Intégration = 3
        Production = 4
        Simulation = 5
        Unitaire = 6
    End Enum

    Public Enum TypeFichier
        Chiffrement
        Certificat
        Sel
    End Enum

    Public Enum TypeCle
        Interne
        SQAG
        Externe
    End Enum


    Private Shared ReadOnly Property CheminServeurInforoute As String
        Get
            Return Cfg.ValeurSysteme("TS6", "TS6\TS6N611\CheminServeurInforoute")
        End Get
    End Property

    Private Shared ReadOnly Property CheminServeurExtranet As String
        Get
            Return Cfg.ValeurSysteme("TS6", "TS6\TS6N611\CheminServeurExtranet")
        End Get
    End Property

    Private Shared ReadOnly Property PrefixServeurInterne As String
        Get
            Return Cfg.ValeurSysteme("TS6", "TS6\TS6N611\PrefixServeurInterne")
        End Get
    End Property

    Private Shared ReadOnly Property SuffixServeurInterne As String
        Get
            Return Cfg.ValeurSysteme("TS6", "TS6\TS6N611\SuffixServeurInterne")
        End Get
    End Property


    Friend Shared ReadOnly Property NomFichierServeurInterne(envrn As Envrn, typeFichier As TypeFichier) As String
        Get
            Dim strFichier As String = ""

            strFichier = PrefixServeurInterne & "\{0}" & SuffixServeurInterne & ObtenirPrefixeFichier(typeFichier) & "{0}.xml"
            strFichier = String.Format(strFichier, ObtenirCorrespondanceEnvironnement(envrn))

            Return strFichier
        End Get
    End Property

    Friend Shared ReadOnly Property NomFichierServeurExtranet(envrn As Envrn, typeFichier As TypeFichier) As String
        Get
            Dim strFichier As String = ""

            strFichier = CheminServeurExtranet & "{0}\Securite\" & ObtenirPrefixeFichier(typeFichier) & "{0}.xml"
            strFichier = String.Format(strFichier, ObtenirCorrespondanceEnvironnement(envrn))

            Return strFichier
        End Get
    End Property

    Friend Shared ReadOnly Property NomFichierServeurInforoute(envrn As Envrn, typeFichier As TypeFichier) As String
        Get
            Dim strFichier As String = ""

            strFichier = CheminServeurInforoute & "{0}\Securite\" & ObtenirPrefixeFichier(typeFichier) & "{0}.xml"
            strFichier = String.Format(strFichier, ObtenirCorrespondanceEnvironnement(envrn))

            Return strFichier
        End Get
    End Property

    Private Shared Function ObtenirPrefixeFichier(typeFichier As TypeFichier) As String
        Dim valeurRetour As String = String.Empty
        Select Case typeFichier
            Case TypeFichier.Chiffrement
                valeurRetour = ObtenirPrefixeFichier("PrefixFichierChiffr", PREFIX_FICHIER_CHIFFR)
            Case TypeFichier.Certificat
                valeurRetour = ObtenirPrefixeFichier("PrefixFichierCertf", PREFIX_FICHIER_CERTF)
            Case TypeFichier.Sel
                valeurRetour = ObtenirPrefixeFichier("PrefixFichierSel", PREFIX_FICHIER_SEL)
        End Select

        Return valeurRetour

    End Function

    Private Shared Function ObtenirPrefixeFichier(cle As String, defaut As String) As String

        Dim valeur As String = Cfg.ObtenirValeurSystemeOptionnelle("TS6", String.Format("TS6\TS6N611\{0}", cle))

        If String.IsNullOrEmpty(valeur) Then
            valeur = defaut
        End If

        Return valeur

    End Function

    Public Shared Function ObtenirCorrespondanceEnvironnement(envrn As Envrn) As String

        Dim nomEnvrn As String = ""

        Select Case envrn
            Case Envrn.Unitaire
                nomEnvrn = "UNIT"
            Case Envrn.Acceptation
                nomEnvrn = "ACCP"
            Case Envrn.Formation_Acceptation
                nomEnvrn = "FORA"
            Case Envrn.Intégration
                nomEnvrn = "INTG"
            Case Envrn.Production
                nomEnvrn = "PROD"
            Case Envrn.Simulation
                nomEnvrn = "SIML"
        End Select

        Return nomEnvrn

    End Function


End Class
