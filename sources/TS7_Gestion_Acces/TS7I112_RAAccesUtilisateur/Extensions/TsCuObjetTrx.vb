Imports System.Runtime.CompilerServices
Imports System.Text

Public Module TsCuObjetTrx

    <Extension>
    Public Function ObtenirTexteDifferencesComptesSupp(pObjetTrx As TSCdObjetTrx, pIndHtml As Boolean) As String
        If Not pObjetTrx.IndComptesSuppModifie() Then Return ""

        Dim builder As New StringBuilder
        Dim indObjNul As Boolean = pObjetTrx.objUtilisateur Is Nothing OrElse pObjetTrx.objUtilisateur.ComptesSupplementaires Is Nothing

        If (indObjNul AndAlso pObjetTrx.IndADMServeur) OrElse (Not indObjNul AndAlso pObjetTrx.IndADMServeur <> pObjetTrx.objUtilisateur.ComptesSupplementaires.IndADMServeur) Then
            builder.AjoutLigneModification(pObjetTrx.IndADMServeur, "Serveur (AS)", pIndHtml)
        End If

        If (indObjNul AndAlso pObjetTrx.IndADMPoste) OrElse (Not indObjNul AndAlso pObjetTrx.IndADMPoste <> pObjetTrx.objUtilisateur.ComptesSupplementaires.IndADMPoste) Then
            builder.AjoutLigneModification(pObjetTrx.IndADMPoste, "Poste de travail (AP)", pIndHtml)
        End If

        If (indObjNul AndAlso pObjetTrx.IndADMDevelopeur) OrElse (Not indObjNul AndAlso pObjetTrx.IndADMDevelopeur <> pObjetTrx.objUtilisateur.ComptesSupplementaires.IndADMDevelopeur) Then
            builder.AjoutLigneModification(pObjetTrx.IndADMDevelopeur, "Atelier de développement (AU)", pIndHtml)
        End If

        If (indObjNul AndAlso pObjetTrx.IndADMCentral) OrElse (Not indObjNul AndAlso pObjetTrx.IndADMCentral <> pObjetTrx.objUtilisateur.ComptesSupplementaires.IndADMCentral) Then
            builder.AjoutLigneModification(pObjetTrx.IndADMCentral, "Central (TSS)", pIndHtml)
        End If

        If (indObjNul AndAlso pObjetTrx.IndEssaisAgent) OrElse (Not indObjNul AndAlso pObjetTrx.IndEssaisAgent <> pObjetTrx.objUtilisateur.ComptesSupplementaires.IndEssaisAgent) Then
            builder.AjoutLigneModification(pObjetTrx.IndEssaisAgent, "Essais RRSP agent (EA)", pIndHtml)
        End If

        If (indObjNul AndAlso pObjetTrx.IndEssaisCE) OrElse (Not indObjNul AndAlso pObjetTrx.IndEssaisCE <> pObjetTrx.objUtilisateur.ComptesSupplementaires.IndEssaisCE) Then
            builder.AjoutLigneModification(pObjetTrx.IndEssaisCE, "Essais RRSP chef d'équipe (EC)", pIndHtml)
        End If

        If (indObjNul AndAlso pObjetTrx.IndSoutienProdAgent) OrElse (Not indObjNul AndAlso pObjetTrx.IndSoutienProdAgent <> pObjetTrx.objUtilisateur.ComptesSupplementaires.IndSoutienProdAgent) Then
            builder.AjoutLigneModification(pObjetTrx.IndSoutienProdAgent, "Soutien production RRSP agent (PA)", pIndHtml)
        End If

        If (indObjNul AndAlso pObjetTrx.IndSoutienProdCE) OrElse (Not indObjNul AndAlso pObjetTrx.IndSoutienProdCE <> pObjetTrx.objUtilisateur.ComptesSupplementaires.IndSoutienProdCE) Then
            builder.AjoutLigneModification(pObjetTrx.IndSoutienProdCE, "Soutien production RRSP chef d'équipe (PC)", pIndHtml)
        End If

        Return builder.ToString
    End Function


    <Extension>
    Public Function ObtenirTexteTypesComptesSupp(pObjetTrx As TSCdObjetTrx) As String
        If pObjetTrx.objUtilisateur Is Nothing OrElse pObjetTrx.objUtilisateur.ComptesSupplementaires Is Nothing Then
            Return String.Empty
        End If

        Dim builder As New StringBuilder

        If pObjetTrx.objUtilisateur.ComptesSupplementaires.IndADMServeur Then
            builder.AjoutTypeCompte("Serveur (AS)")
        End If

        If pObjetTrx.objUtilisateur.ComptesSupplementaires.IndADMPoste Then
            builder.AjoutTypeCompte("Poste de travail (AP)")
        End If

        If pObjetTrx.objUtilisateur.ComptesSupplementaires.IndADMDevelopeur Then
            builder.AjoutTypeCompte("Atelier de développement (AU)")
        End If

        If pObjetTrx.objUtilisateur.ComptesSupplementaires.IndADMCentral Then
            builder.AjoutTypeCompte("Central (TSS)")
        End If

        If pObjetTrx.objUtilisateur.ComptesSupplementaires.IndEssaisAgent Then
            builder.AjoutTypeCompte("Essais RRSP agent (EA)")
        End If

        If pObjetTrx.objUtilisateur.ComptesSupplementaires.IndEssaisCE Then
            builder.AjoutTypeCompte("Essais RRSP chef d'équipe (EC)")
        End If

        If pObjetTrx.objUtilisateur.ComptesSupplementaires.IndSoutienProdAgent Then
            builder.AjoutTypeCompte("Soutien production RRSP agent (PA)")
        End If

        If pObjetTrx.objUtilisateur.ComptesSupplementaires.IndSoutienProdCE Then
            builder.AjoutTypeCompte("Soutien production RRSP chef d'équipe (PC)")
        End If

        Return builder.ToString
    End Function

    <Extension>
    Public Function IndComptesSuppModifie(pObjetTrx As TSCdObjetTrx) As Boolean
        If pObjetTrx.objUtilisateur Is Nothing OrElse pObjetTrx.objUtilisateur.ComptesSupplementaires Is Nothing Then
            Return pObjetTrx.IndADMCentral OrElse pObjetTrx.IndADMDevelopeur OrElse pObjetTrx.IndADMPoste OrElse pObjetTrx.IndADMServeur OrElse
                pObjetTrx.IndEssaisAgent OrElse pObjetTrx.IndEssaisCE OrElse pObjetTrx.IndSoutienProdAgent OrElse pObjetTrx.IndSoutienProdCE
        End If

        Return pObjetTrx.IndADMCentral <> pObjetTrx.objUtilisateur.ComptesSupplementaires.IndADMCentral OrElse
            pObjetTrx.IndADMDevelopeur <> pObjetTrx.objUtilisateur.ComptesSupplementaires.IndADMDevelopeur OrElse
            pObjetTrx.IndADMPoste <> pObjetTrx.objUtilisateur.ComptesSupplementaires.IndADMPoste OrElse
            pObjetTrx.IndADMServeur <> pObjetTrx.objUtilisateur.ComptesSupplementaires.IndADMServeur OrElse
            pObjetTrx.IndEssaisAgent <> pObjetTrx.objUtilisateur.ComptesSupplementaires.IndEssaisAgent OrElse
            pObjetTrx.IndEssaisCE <> pObjetTrx.objUtilisateur.ComptesSupplementaires.IndEssaisCE OrElse
            pObjetTrx.IndSoutienProdAgent <> pObjetTrx.objUtilisateur.ComptesSupplementaires.IndSoutienProdAgent OrElse
            pObjetTrx.IndSoutienProdCE <> pObjetTrx.objUtilisateur.ComptesSupplementaires.IndSoutienProdCE
    End Function

    <Extension>
    Public Sub AjoutLigneModification(pBuilder As StringBuilder, pIndAjout As Boolean, pTypeCompte As String, pIndHtml As Boolean)
        Dim texteAjout As String = "- Ajout d'un compte supplémentaire: "
        Dim texteRetrait As String = "- Retrait d'un compte supplémentaire: "
        Dim texteTypeCompte As String = "Type de compte: {0}"

        If pIndAjout Then
            pBuilder.AppendLine(texteAjout)
        Else
            pBuilder.AppendLine(texteRetrait)
        End If
        If pIndHtml Then pBuilder.Append("<br />")

        pBuilder.AppendFormat(texteTypeCompte, pTypeCompte)
        If pIndHtml Then pBuilder.Append("<br />")
        pBuilder.AppendLine()
    End Sub

    <Extension>
    Public Sub AjoutTypeCompte(pBuilder As StringBuilder, pTypeCompte As String)
        Dim texteTypeCompte As String = "Type de compte: {0}"

        If pBuilder.Length > 0 Then
            pBuilder.Append(", ")
        End If

        pBuilder.AppendFormat(texteTypeCompte, pTypeCompte)
    End Sub

    <Extension>
    Public Function IndComptesSuppPresent(pObjetTrx As TSCdObjetTrx) As Boolean
        Return pObjetTrx.IndADMCentral OrElse pObjetTrx.IndADMDevelopeur OrElse pObjetTrx.IndADMPoste OrElse pObjetTrx.IndADMServeur OrElse
            pObjetTrx.IndEssaisAgent OrElse pObjetTrx.IndEssaisCE OrElse pObjetTrx.IndSoutienProdAgent OrElse pObjetTrx.IndSoutienProdCE
    End Function

    <Extension>
    Public Sub SupprimerComptesSupp(pObjetTrx As TSCdObjetTrx)
        pObjetTrx.IndADMCentral = False
        pObjetTrx.IndADMDevelopeur = False
        pObjetTrx.IndADMPoste = False
        pObjetTrx.IndADMServeur = False

        pObjetTrx.IndEssaisAgent = False
        pObjetTrx.IndEssaisCE = False
        pObjetTrx.IndSoutienProdAgent = False
        pObjetTrx.IndSoutienProdCE = False
    End Sub

End Module
