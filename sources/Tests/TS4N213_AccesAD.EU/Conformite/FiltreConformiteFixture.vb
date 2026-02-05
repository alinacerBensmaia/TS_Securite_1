Imports Rrq.Securite

<TestClass()> Public Class FiltreConformiteFixture
    Private reglePrefix As FiltreConformite
    Private regleSuffix As FiltreConformite
    Private reglePrefixEtSuffix As FiltreConformite
    Private regleUneLettreMilieu As FiltreConformite
    Private regleDeuxLettre As FiltreConformite
    Private regleDoubleSimple As FiltreConformite
    Private regleDoubleComplexe As FiltreConformite

    <TestInitialize>
    Public Sub Init()
        reglePrefix = New FiltreConformite("TS1*")
        regleSuffix = New FiltreConformite("*FIN1")
        reglePrefixEtSuffix = New FiltreConformite("ROI*Niveau1")
        regleUneLettreMilieu = New FiltreConformite("ROI_?_ABC")
        regleDeuxLettre = New FiltreConformite("ROI_??_ABC")
        regleDoubleSimple = New FiltreConformite("ROI_?_*_Biztalk")
        regleDoubleComplexe = New FiltreConformite("ROA_??_XF_*Niveau?")
    End Sub

#Region "mauvais format de regle"

    <TestMethod, TestCategory("Constructeur")>
    <ExpectedException(GetType(ArgumentException))>
    Public Sub MauvaiseRegleSiVide()
        Dim regle As New FiltreConformite("")
    End Sub

    <TestMethod, TestCategory("Constructeur")>
    <ExpectedException(GetType(ArgumentException))>
    Public Sub MauvaiseRegleSiEspaceVide()
        Dim regle As New FiltreConformite(" ")
    End Sub

    <TestMethod, TestCategory("Constructeur")>
    <ExpectedException(GetType(ArgumentException))>
    Public Sub MauvaiseRegleSiNull()
        Dim regle As New FiltreConformite(Nothing)
    End Sub

    <TestMethod, TestCategory("Constructeur")>
    <ExpectedException(GetType(ArgumentException))>
    Public Sub MauvaiseRegleSiDeuxEtoiles()
        Dim regle As New FiltreConformite("A*B*C")
    End Sub

    <TestMethod, TestCategory("Constructeur")>
    <ExpectedException(GetType(ArgumentException))>
    Public Sub MauvaiseRegleSiEtoileSeule()
        Dim regle As New FiltreConformite("*")
    End Sub
    <TestMethod, TestCategory("Constructeur")>
    <ExpectedException(GetType(ArgumentException))>
    Public Sub MauvaiseRegleSiDeuxEtoileSeule()
        Dim regle As New FiltreConformite("**")
    End Sub
    <TestMethod, TestCategory("Constructeur")>
    <ExpectedException(GetType(ArgumentException))>
    Public Sub MauvaiseRegleSiPlusieursEtoileSeule()
        Dim regle As New FiltreConformite("***")
    End Sub

    <TestMethod, TestCategory("Constructeur")>
    <ExpectedException(GetType(ArgumentException))>
    Public Sub MauvaiseRegleSiQuestionSeule()
        Dim regle As New FiltreConformite("?")
    End Sub
    <TestMethod, TestCategory("Constructeur")>
    <ExpectedException(GetType(ArgumentException))>
    Public Sub MauvaiseRegleSiDeuxQuestionSeule()
        Dim regle As New FiltreConformite("??")
    End Sub
    <TestMethod, TestCategory("Constructeur")>
    <ExpectedException(GetType(ArgumentException))>
    Public Sub MauvaiseRegleSiPlusieursQuestionSeule()
        Dim regle As New FiltreConformite("???")
    End Sub

#End Region

#Region "Type regle"

    <TestMethod, TestCategory("Type Regle")>
    Public Sub DevraitEtreReglePlain()
        Dim r As New FiltreConformite("ABC")
        Assert.AreEqual(FiltreConformite.Types.Plain, r.Type)
    End Sub

    <TestMethod, TestCategory("Type Regle")>
    Public Sub DevraitEtreRegleWildcardSuffix()
        Dim r As New FiltreConformite("ABC*")
        Assert.AreEqual(FiltreConformite.Types.Wildcard, r.Type)
    End Sub
    <TestMethod, TestCategory("Type Regle")>
    Public Sub DevraitEtreRegleWildcardPrefix()
        Dim r As New FiltreConformite("*ABC")
        Assert.AreEqual(FiltreConformite.Types.Wildcard, r.Type)
    End Sub
    <TestMethod, TestCategory("Type Regle")>
    Public Sub DevraitEtreRegleWildcardMilieu()
        Dim r As New FiltreConformite("AB*CD")
        Assert.AreEqual(FiltreConformite.Types.Wildcard, r.Type)
    End Sub

    <TestMethod, TestCategory("Type Regle")>
    Public Sub DevraitEtreRegleCaractere()
        Dim r As New FiltreConformite("AB?CD")
        Assert.AreEqual(FiltreConformite.Types.Caracter, r.Type)
    End Sub
    <TestMethod, TestCategory("Type Regle")>
    Public Sub DevraitEtreRegleCaractereSuffix()
        Dim r As New FiltreConformite("ABC?")
        Assert.AreEqual(FiltreConformite.Types.Caracter, r.Type)
    End Sub
    <TestMethod, TestCategory("Type Regle")>
    Public Sub DevraitEtreRegleCaracterePrefix()
        Dim r As New FiltreConformite("?ABC")
        Assert.AreEqual(FiltreConformite.Types.Caracter, r.Type)
    End Sub
    <TestMethod, TestCategory("Type Regle")>
    Public Sub DevraitEtreRegleCaractereMultipleGroupe()
        Dim r As New FiltreConformite("AB??CD")
        Assert.AreEqual(FiltreConformite.Types.Caracter, r.Type)
    End Sub

    <TestMethod, TestCategory("Type Regle")>
    Public Sub DevraitEtreRegleCaractereMultiple()
        Dim r As New FiltreConformite("A?BC?D")
        Assert.AreEqual(FiltreConformite.Types.Caracter, r.Type)
    End Sub

    <TestMethod, TestCategory("Type Regle")>
    Public Sub DevraitEtreRegleWildcardAndCaractereMultiple()
        Dim r As New FiltreConformite("ROI_?_*_Biztalk")
        Assert.AreEqual(FiltreConformite.Types.WildcardAndCaracter, r.Type)
    End Sub

#End Region

#Region "Prefix"

    <TestMethod, TestCategory("Regle Wildcard")>
    Public Sub ReglePrefixBonSiValeurEgalePrefix()
        Assert.IsTrue(reglePrefix.Correspond("TS1"))
    End Sub
    <TestMethod, TestCategory("Regle Wildcard")>
    Public Sub ReglePrefixBonSiValeurCommenceParPrefix()
        Assert.IsTrue(reglePrefix.Correspond("TS1ECONNECT"))
    End Sub

    <TestMethod, TestCategory("Regle Wildcard")>
    Public Sub ReglePrefixBonSiValeurCommenceParPrefixDifferenteCasse()
        Assert.IsTrue(reglePrefix.Correspond("ts1econnect"))
    End Sub

    <TestMethod, TestCategory("Regle Wildcard")>
    Public Sub ReglePrefixMauvaisSiValeurVide()
        Assert.IsFalse(reglePrefix.Correspond(""))
    End Sub

    <TestMethod, TestCategory("Regle Wildcard")>
    Public Sub ReglePrefixMauvaisSiValeurPlusCourteQuePrefix()
        Assert.IsFalse(reglePrefix.Correspond("T"))
    End Sub
    <TestMethod, TestCategory("Regle Wildcard")>
    Public Sub ReglePrefixMauvaisSiValeurNeCommencePasParPrefix()
        Assert.IsFalse(reglePrefix.Correspond("ROX_TS1"))
    End Sub

#End Region

#Region "Suffix"

    <TestMethod, TestCategory("Regle Wildcard")>
    Public Sub RegleSuffixBonSiValeurEgaleSuffix()
        Assert.IsTrue(regleSuffix.Correspond("FIN1"))
    End Sub
    <TestMethod, TestCategory("Regle Wildcard")>
    Public Sub RegleSuffixBonSiValeurTermineParSuffix()
        Assert.IsTrue(regleSuffix.Correspond("ROI_U_FIN1"))
    End Sub

    <TestMethod, TestCategory("Regle Wildcard")>
    Public Sub RegleSuffixBonSiValeurTermineParSuffixDifferenteCasse()
        Assert.IsTrue(regleSuffix.Correspond("roi_u_fin1"))
    End Sub

    <TestMethod, TestCategory("Regle Wildcard")>
    Public Sub RegleSuffixMauvaisSiValeurVide()
        Assert.IsFalse(regleSuffix.Correspond(""))
    End Sub

    <TestMethod, TestCategory("Regle Wildcard")>
    Public Sub RegleSuffixMauvaisSiValeurNeTerminePasParSuffix()
        Assert.IsFalse(regleSuffix.Correspond("FIN1_ROI"))
    End Sub

    <TestMethod, TestCategory("Regle Wildcard")>
    Public Sub RegleSuffixMauvaisSiValeurPlusCourteQueSuffix()
        Assert.IsFalse(regleSuffix.Correspond("1"))
    End Sub
#End Region

#Region "PrefixEtSuffix"

    <TestMethod, TestCategory("Regle Wildcard")>
    Public Sub ReglePrefixEtSuffixBonSiValeurEgaleSansEtoile()
        Assert.IsTrue(reglePrefixEtSuffix.Correspond("ROINiveau1"))
    End Sub
    <TestMethod, TestCategory("Regle Wildcard")>
    Public Sub ReglePrefixEtSuffixBonSiValeurCommenceParPreffixEtTermineParSuffix()
        Assert.IsTrue(reglePrefixEtSuffix.Correspond("ROI_U_SecrtNiveau1"))
    End Sub

    <TestMethod, TestCategory("Regle Wildcard")>
    Public Sub ReglePrefixEtSuffixBonSiValeurCommenceParPreffixEtTermineParSuffixDifferenteCasse()
        Assert.IsTrue(reglePrefixEtSuffix.Correspond("roi_u_SecrtNIVEAU1"))
    End Sub

    <TestMethod, TestCategory("Regle Wildcard")>
    Public Sub ReglePrefixEtSuffixMauvaisSiValeurVide()
        Assert.IsFalse(reglePrefixEtSuffix.Correspond(""))
    End Sub

    <TestMethod, TestCategory("Regle Wildcard")>
    Public Sub ReglePrefixEtSuffixMauvaisSiValeurNeTerminePasParSuffix()
        Assert.IsFalse(reglePrefixEtSuffix.Correspond("ROI_U_SecrtNiveau9"))
    End Sub

    <TestMethod, TestCategory("Regle Wildcard")>
    Public Sub ReglePrefixEtSuffixMauvaisSiValeurNeCommencePasParPrefix()
        Assert.IsFalse(reglePrefixEtSuffix.Correspond("ROX_U_SecrtNiveau1"))
    End Sub

    <TestMethod, TestCategory("Regle Wildcard")>
    Public Sub ReglePrefixEtSuffixMauvaisSiValeurPlusCourteQueSuffix()
        Assert.IsFalse(reglePrefixEtSuffix.Correspond("ROI_1"))
    End Sub

#End Region

#Region "Question milieu seul"

    <TestMethod, TestCategory("Regle Caractere")>
    Public Sub RegleUneLettreMilieuMauvaisSiValeurEgaleSansQuestion()
        Assert.IsFalse(regleUneLettreMilieu.Correspond("ROI__ABC"))
    End Sub
    <TestMethod, TestCategory("Regle Caractere")>
    Public Sub RegleUneLettreMilieuBonSiQuestionRemplacerParCaractere()
        Assert.IsTrue(regleUneLettreMilieu.Correspond("ROI_U_ABC"))
    End Sub

    <TestMethod, TestCategory("Regle Caractere")>
    Public Sub RegleUneLettreMilieuBonSiQuestionRemplacerParCaractereDifferenteCasse()
        Assert.IsTrue(regleUneLettreMilieu.Correspond("roi_u_abc"))
    End Sub

    <TestMethod, TestCategory("Regle Caractere")>
    Public Sub RegleUneLettreMilieuMauvaisSiValeurVide()
        Assert.IsFalse(regleUneLettreMilieu.Correspond(""))
    End Sub

    <TestMethod, TestCategory("Regle Caractere")>
    Public Sub RegleUneLettreMilieuMauvaisSiValeurNeTerminePasParSuffix()
        Assert.IsFalse(regleUneLettreMilieu.Correspond("ROI_U_ABCD"))
    End Sub

    <TestMethod, TestCategory("Regle Caractere")>
    Public Sub RegleUneLettreMilieuMauvaisSiValeurNeCommencePasParPrefix()
        Assert.IsFalse(regleUneLettreMilieu.Correspond("ROX_U_ABC"))
    End Sub

    <TestMethod, TestCategory("Regle Caractere")>
    Public Sub RegleUneLettreMilieuSiValeurPlusCourte()
        Assert.IsFalse(regleUneLettreMilieu.Correspond("ROI_ABC"))
    End Sub

#End Region

#Region "Question milieu multiple"

    <TestMethod, TestCategory("Regle Caractere")>
    Public Sub RegleDeuxLettreMilieuMauvaisSiValeurEgaleSansQuestion()
        Assert.IsFalse(regleDeuxLettre.Correspond("ROI__ABC"))
    End Sub
    <TestMethod, TestCategory("Regle Caractere")>
    Public Sub RegleDeuxLettreMilieuBonSiQuestionRemplacerParCaractere()
        Assert.IsTrue(regleDeuxLettre.Correspond("ROI_UU_ABC"))
    End Sub

    <TestMethod, TestCategory("Regle Caractere")>
    Public Sub RegleDeuxLettreMilieuBonSiQuestionRemplacerParCaractereDifferenteCasse()
        Assert.IsTrue(regleDeuxLettre.Correspond("roi_uu_abc"))
    End Sub

    <TestMethod, TestCategory("Regle Caractere")>
    Public Sub RegleDeuxLettreMilieuMauvaisSiValeurVide()
        Assert.IsFalse(regleDeuxLettre.Correspond(""))
    End Sub

    <TestMethod, TestCategory("Regle Caractere")>
    Public Sub RegleDeuxLettreMilieuMauvaisSiValeurNeTerminePasParSuffix()
        Assert.IsFalse(regleDeuxLettre.Correspond("ROI_UU_ABCD"))
    End Sub

    <TestMethod, TestCategory("Regle Caractere")>
    Public Sub RegleDeuxLettreMilieuMauvaisSiValeurNeCommencePasParPrefix()
        Assert.IsFalse(regleDeuxLettre.Correspond("ROX_UU_ABC"))
    End Sub

    <TestMethod, TestCategory("Regle Caractere")>
    Public Sub RegleDeuxLettreMilieuSiValeurPlusCourte()
        Assert.IsFalse(regleDeuxLettre.Correspond("ROI_ABC"))
    End Sub

#End Region

#Region "Question autres"

    <TestMethod, TestCategory("Regle Caractere")>
    Public Sub RegleQuestionPrefix()
        Dim r As New FiltreConformite("?ABC")
        Assert.IsTrue(r.Correspond("1ABC"))
    End Sub
    <TestMethod, TestCategory("Regle Caractere")>
    Public Sub RegleQuestionSuffix()
        Dim r As New FiltreConformite("ABC?")
        Assert.IsTrue(r.Correspond("ABC1"))
    End Sub
    <TestMethod, TestCategory("Regle Caractere")>
    Public Sub RegleQuestionSuffixEtPrefix()
        Dim r As New FiltreConformite("?ABC?")
        Assert.IsTrue(r.Correspond("1ABC1"))
    End Sub
    <TestMethod, TestCategory("Regle Caractere")>
    Public Sub RegleQuestionSuffixEtPrefixEtMilieu()
        Dim r As New FiltreConformite("?A?C?")
        Assert.IsTrue(r.Correspond("1A2C1"))
    End Sub

#End Region

#Region "Wildcard and character"

    <TestMethod, TestCategory("Regle Wildcard & Caratere")>
    Public Sub RegleDoubleBonSiCorrespond()
        Assert.IsTrue(regleDoubleSimple.Correspond("ROI_P_XU_Biztalk"))
    End Sub

    <TestMethod, TestCategory("Regle Wildcard & Caratere")>
    Public Sub RegleDoubleBonSiCorrespondAvecDifferenteCasse()
        Assert.IsTrue(regleDoubleSimple.Correspond("roi_p_xu_BIZTALK"))
    End Sub

    <TestMethod, TestCategory("Regle Wildcard & Caratere")>
    Public Sub RegleDoubleMauvaisSiValeurVide()
        Assert.IsFalse(regleDoubleSimple.Correspond(""))
    End Sub

    <TestMethod, TestCategory("Regle Wildcard & Caratere")>
    Public Sub RegleDoubleComplexeBonSiCorrespond()
        Assert.IsTrue(regleDoubleComplexe.Correspond("ROA_PU_XF_AbcNiveau1"))
    End Sub


    <TestMethod, TestCategory("Regle Wildcard & Caratere")>
    Public Sub RegleDoubleComplexeBonSiCorrespondAvecDifferenteCasse()
        Assert.IsTrue(regleDoubleComplexe.Correspond("roa_pu_xf_abcNIVEAU1"))
    End Sub

    <TestMethod, TestCategory("Regle Wildcard & Caratere")>
    Public Sub RegleDoubleComplexeMauvaisSiValeurVide()
        Assert.IsFalse(regleDoubleComplexe.Correspond(""))
    End Sub

    <TestMethod, TestCategory("Regle Wildcard & Caratere")>
    Public Sub RegleDoubleComplexeMauvaisSiValeurTropCourte()
        Assert.IsFalse(regleDoubleComplexe.Correspond("ROA_PU_Niveau1"))
    End Sub

#End Region

#Region "Wildcard and character autres"

    <TestMethod, TestCategory("Regle Wildcard & Caratere")>
    Public Sub RegleDoubleEstSuffix()
        Dim r As New FiltreConformite("*abc?ef")
        Assert.IsTrue(r.Correspond("asdfsdf_abcdef"))
    End Sub
    <TestMethod, TestCategory("Regle Wildcard & Caratere")>
    Public Sub RegleDoubleEstPresqueSuffix()
        Dim r As New FiltreConformite("?*abc?ef")
        Assert.IsTrue(r.Correspond("asdfsdf_abcdef"))
    End Sub

    <TestMethod, TestCategory("Regle Wildcard & Caratere")>
    Public Sub RegleDoubleEstPrefix()
        Dim r As New FiltreConformite("abc?ef*")
        Assert.IsTrue(r.Correspond("abcdef_asdfsdf"))
    End Sub

    <TestMethod, TestCategory("Regle Wildcard & Caratere")>
    Public Sub RegleDoubleEstPresquePrefix()
        Dim r As New FiltreConformite("abc?ef*?")
        Assert.IsTrue(r.Correspond("abcdef_asdfsdf"))
    End Sub

#End Region

End Class