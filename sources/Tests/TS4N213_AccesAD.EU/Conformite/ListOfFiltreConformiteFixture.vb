Imports Rrq.Securite

<TestClass()> Public Class ListOfFiltreConformiteFixture
    Private regles As ListOfFiltreConformite

    <TestInitialize>
    Public Sub Init()
        Dim filtres As New List(Of String)
        filtres.Add("TS1*")
        filtres.Add("ROX_*")
        filtres.Add("ROA_??_XP_*")
        filtres.Add("ROI_?_*_BizTalk")

        regles = New ListOfFiltreConformite()
        regles.AddRange(filtres)
    End Sub

    <TestMethod()>
    Public Sub DevraitEtreOkPourTS1()
        Assert.IsTrue(regles.Correspond("TS1EConnNET"))
        Assert.IsTrue(regles.Correspond("TS1PConnNET"))
    End Sub

    <TestMethod()>
    Public Sub DevraitEtreOkPourROX()
        Assert.IsTrue(regles.Correspond("ROX_U_TSXA"))
        Assert.IsTrue(regles.Correspond("ROX_I_TSXA"))
        Assert.IsTrue(regles.Correspond("ROX_A_TSXA"))
        Assert.IsTrue(regles.Correspond("ROX_P_TSXA"))
    End Sub

    <TestMethod()>
    Public Sub DevraitEtreOkPourRoaXp()
        Assert.IsTrue(regles.Correspond("ROA_ID_XP_ASRetrograduer"))
        Assert.IsTrue(regles.Correspond("ROA_IP_XP_ASRetrograduer"))
        Assert.IsTrue(regles.Correspond("ROA_IS_XP_ASRetrograduer"))
        Assert.IsTrue(regles.Correspond("ROA_IU_XP_ASRetrograduer"))
        Assert.IsTrue(regles.Correspond("ROA_IX_XP_ASRetrograduer"))
    End Sub

    <TestMethod()>
    Public Sub DevraitEtreOkPourROIBizTalk()
        Assert.IsTrue(regles.Correspond("ROI_I_XU_BizTalk"))
        Assert.IsTrue(regles.Correspond("ROI_A_XU_BizTalk"))
        Assert.IsTrue(regles.Correspond("ROI_B_XU_BizTalk"))
        Assert.IsTrue(regles.Correspond("ROI_P_XU_BizTalk"))
    End Sub

    <TestMethod>
    Public Sub NeDevraitPasEtreBonPourROINiveauSecrt()
        Assert.IsFalse(regles.Correspond("ROI_I_PEFN101_INiveauSecrt1"))
        Assert.IsFalse(regles.Correspond("ROI_I_PEFN101_INiveauSecrt9"))
        Assert.IsFalse(regles.Correspond("ROI_P_PEFN101_INiveauSecrt1"))
        Assert.IsFalse(regles.Correspond("ROI_P_PEFN101_INiveauSecrt9"))
    End Sub

    <TestMethod()>
    Public Sub NeDevraitPasEtreBonPourRoaXf()
        Assert.IsFalse(regles.Correspond("ROA_D_XF_CnUiAdmin"))
        Assert.IsFalse(regles.Correspond("ROA_P_XF_CnUiAdmin"))
    End Sub

End Class