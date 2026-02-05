<%@ Register TagPrefix="cc2" Namespace="Rrq.Web.ServicesCommunsPetitsSystemes.ScenarioTransactionnel"
    Assembly="XL5I021_GestnDialg" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TS7SAjouterRoles.aspx.vb"
    Inherits="TS7I111_AccesUtilisateur.TS7SAjouterRoles" %>

<%@ Register TagPrefix="cc1" Namespace="Rrq.Web.ServicesCommunsPetitsSystemes.Utilitaires"
    Assembly="XL2I041_CtlAffchAscxPartage" %>
<%@ Register Assembly="XL2I111_CtrlDate" Namespace="Rrq.Web.ServicesCommunsPetitsSystemes.Utilitaires"
    TagPrefix="cc4" %>
<%@ Register Assembly="NI1I213_ControlesBase" Namespace="Rrq.Web.GabaritsPetitsSystemes.ControlesBase"
    TagPrefix="cc3" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Accès utilisateurs</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <!--<script type="text/javascript" src="../Code/Scripts/police.js"></script>-->
    <link id="stylePolice" href="../F_Style/TS7styles.css" type="text/css" rel="stylesheet" />
    <link id="stylePoliceGros" href="" type="text/css" rel="stylesheet" />
    <style type="text/css">
        .style1
        {
            width: 121px;
            overflow: auto;
        }
    </style>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <div class="page">
        <cc1:XlCrAffchAscxPartage ID="XlCrAffchAscxPartage1" runat="server" NomASCX="NI1P512_BandeauPIVGabarit" />
        <cc1:XlCrAffchAscxPartage ID="XlCrAffchAscxPartage2" runat="server" NomASCX="NI1P511_MenuGaucheGabarit" />
        <div class="pageCentre texte">
            <asp:Label ID="lblTitre" runat="server" CssClass="titre">Ajouter des rôles</asp:Label>
            <asp:Panel ID="pnlInfo" runat="server" CssClass="ValPageBob Validation" Visible="false">
                <ul>
                    <li>
                        <asp:Label ID="lblMessageInfo" runat="server" Text="Info!!!">Info!!!</asp:Label></li></ul>
            </asp:Panel>
            <asp:ValidationSummary ID="ValPage" runat="server" CssClass="Validation valPagex">
            </asp:ValidationSummary>
            <asp:Repeater ID="rptgrdListeRoles" runat="server">
                <ItemTemplate>
                    <br />
                    <cc1:XlCrAffchAscxPartage ID="XLCrUAPrincipal" runat="server" NomASCX="NI1P516_PlusMoinsInfos"
                        Visible="true" />
                    <asp:Panel ID="pnlListeRoles" runat="server" Visible="True">
                        <br />
                        <asp:Label ID="lblRoleUA" runat="server" CssClass="soustitre" Text=""></asp:Label>
                        <cc3:NiCuGrillePageTrie ID="grdListeRolesMetier" TabIndex="190" runat="server" CssClass="grille texte"
                            Width="100%" EnableViewState="False" AutoGenerateColumns="False" BorderWidth="0px"
                            HeaderStyle-CssClass="entete" FooterStyle-CssClass="bastableau" AlternatingItemStyle-CssClass="lignepaire"
                            ItemStyle-CssClass="ligneimpaire" CellSpacing="1" CellPadding="1" ShowFooter="True"
                            AllowPaging="False" AllowSorting="True" PageSize="10000" OnItemCommand="grdListeRoles_ItemCommand">
                            <FooterStyle CssClass="bastableau"></FooterStyle>
                            <AlternatingItemStyle CssClass="lignepaire"></AlternatingItemStyle>
                            <ItemStyle CssClass="ligneimpaire"></ItemStyle>
                            <HeaderStyle CssClass="entete"></HeaderStyle>
                            <Columns>
                                <asp:TemplateColumn>
                                    <ItemTemplate>
                                        <cc3:NiCuCaseCocheGrille ID="chkSelection" EnableViewState="true" runat="server"
                                            Checked='<%#CocherCase(DataBinder.Eval(Container.DataItem, "SELECT"),DataBinder.Eval(Container, "DataItem.ID"))%>'
                                            IdentifiantUnique='<%# Container.DataItem("ID") %>' Enabled='<%#DesactiverCaseCoche(DataBinder.Eval(Container, "DataItem.ID"))%>' Visible="True"></cc3:NiCuCaseCocheGrille>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn SortExpression="Nom" HeaderText="R&#244;les métiers">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRoleUtilisateur" runat="server" CssClass="texte">
											<%#AfficherSuggestion(DataBinder.Eval(Container, "DataItem.ID"), "Metier", DataBinder.Eval(Container, "DataItem.LienTachesMetiers"), "Debut")%><%#DataBinder.Eval(Container, "DataItem.Nom")%><%#AfficherSuggestion(DataBinder.Eval(Container, "DataItem.ID"), "Metier", DataBinder.Eval(Container, "DataItem.LienTachesMetiers"), "Fin")%>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkAfficherDetail" runat="server" CommandName="AfficherDetail"
                                            CommandArgument='<%# ctype(DataBinder.Eval(Container,"DataItem.description"),string)%>'
                                            ToolTip='<%#ObtenirInfoBulleDetail(DataBinder.Eval(Container,"DataItem.Nom")) %>'>Afficher détails<br /></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="20%"></ItemStyle>
                                </asp:TemplateColumn>
                            </Columns>
                            <PagerStyle Visible="False"></PagerStyle>
                        </cc3:NiCuGrillePageTrie>
                        <asp:Panel ID="pnlPagingMetier" runat="server" CssClass="grillepagination" Width="100%">
                            <div align="center">
                                <asp:LinkButton ID="HyDebut" TabIndex="200" runat="server" CssClass="texte" Visible="False"
                                    Width="64px" CommandArgument="debut">&lt;&lt; Début</asp:LinkButton>
                                <asp:LinkButton ID="HyPrecedent" Style="padding-left: 8px" TabIndex="201" runat="server"
                                    CssClass="texte" Visible="False" Width="37px" CommandArgument="prev">&lt;Précédent</asp:LinkButton>
                                <asp:LinkButton ID="hypPage1" Style="padding-left: 8px" TabIndex="202" runat="server"
                                    CssClass="texte" Visible="False" CommandArgument="Page1">1</asp:LinkButton>
                                <asp:LinkButton ID="hypPage2" Style="padding-left: 8px" TabIndex="203" runat="server"
                                    CssClass="texte" Visible="False" CommandArgument="Page2">2</asp:LinkButton>
                                <asp:LinkButton CommandArgument="Page3" ID="hypPage3" Style="padding-left: 8px" runat="server"
                                    CssClass="texte" OnCommand="GererPagination" Visible="False" TabIndex="204">3</asp:LinkButton>
                                <asp:LinkButton CommandArgument="Page4" ID="hypPage4" Style="padding-left: 8px" TabIndex="205"
                                    runat="server" CssClass="texte" Visible="False">4</asp:LinkButton>
                                <asp:LinkButton ID="hypPage5" CommandArgument="Page5" Style="padding-left: 8px" runat="server"
                                    CssClass="texte" Visible="False" TabIndex="206">5</asp:LinkButton>
                                <asp:LinkButton ID="hypPage6" CommandArgument="Page6" Style="padding-left: 8px" TabIndex="207"
                                    runat="server" CssClass="texte" Visible="False">6</asp:LinkButton>
                                <asp:LinkButton ID="hypPage7" CommandArgument="Page7" Style="padding-left: 8px" TabIndex="208"
                                    runat="server" CssClass="texte" Visible="False">7</asp:LinkButton>
                                <asp:LinkButton ID="hypPage8" CommandArgument="Page8" Style="padding-left: 8px" TabIndex="209"
                                    runat="server" CssClass="texte" Visible="False">8</asp:LinkButton>
                                <asp:LinkButton ID="hySuivant" Style="padding-left: 8px" TabIndex="210" runat="server"
                                    CssClass="texte" Visible="False" CommandArgument="next">Suivant&gt;</asp:LinkButton>
                                <asp:LinkButton ID="hyFin" Style="padding-left: 8px" TabIndex="211" runat="server"
                                    CssClass="texte" Visible="False" CommandArgument="fin">Fin &gt;&gt;</asp:LinkButton></div>
                        </asp:Panel>
                        <br />
                        <cc3:NiCuGrillePageTrie ID="grdListeRolesTaches" TabIndex="190" runat="server" CssClass="grille texte"
                            Width="100%" EnableViewState="False" AutoGenerateColumns="False" BorderWidth="0px"
                            HeaderStyle-CssClass="entete" FooterStyle-CssClass="bastableau" AlternatingItemStyle-CssClass="lignepaire"
                            ItemStyle-CssClass="ligneimpaire" CellSpacing="1" CellPadding="1" ShowFooter="True"
                            AllowPaging="False" AllowSorting="True" PageSize="10000" OnItemCommand="grdListeRoles_ItemCommand">
                            <FooterStyle CssClass="bastableau"></FooterStyle>
                            <AlternatingItemStyle CssClass="lignepaire"></AlternatingItemStyle>
                            <ItemStyle CssClass="ligneimpaire"></ItemStyle>
                            <HeaderStyle CssClass="entete"></HeaderStyle>
                            <Columns>
                                <asp:TemplateColumn>
                                    <ItemTemplate>
                                        <cc3:NiCuCaseCocheGrille ID="chkSelection" EnableViewState="true" runat="server"
                                            Checked='<%#CocherCase(DataBinder.Eval(Container.DataItem, "SELECT"),DataBinder.Eval(Container, "DataItem.ID"))%>'
                                            IdentifiantUnique='<%# Container.DataItem("ID") %>' Enabled='<%#DesactiverCaseCoche(DataBinder.Eval(Container, "DataItem.ID"))%>' Visible="True"></cc3:NiCuCaseCocheGrille>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn SortExpression="Nom" HeaderText="R&#244;le de tâches">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRoleUtilisateur" runat="server" CssClass="texte">
											<%#AfficherSuggestion(DataBinder.Eval(Container, "DataItem.ID"), "Taches", DataBinder.Eval(Container, "DataItem.LienTachesMetiers"), "Debut")%><%#DataBinder.Eval(Container, "DataItem.Nom")%><%#AfficherSuggestion(DataBinder.Eval(Container, "DataItem.ID"), "Taches", DataBinder.Eval(Container, "DataItem.LienTachesMetiers"), "Fin")%>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkAfficherDetail" runat="server" CommandName="AfficherDetail"
                                            CommandArgument='<%# ctype(DataBinder.Eval(Container,"DataItem.description"),string)%>'
                                            ToolTip='<%#ObtenirInfoBulleDetail(DataBinder.Eval(Container,"DataItem.Nom")) %>'>Afficher détails<br /></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="20%"></ItemStyle>
                                </asp:TemplateColumn>
                            </Columns>
                            <PagerStyle Visible="False"></PagerStyle>
                        </cc3:NiCuGrillePageTrie>
                        <asp:Panel ID="pnlPagingTaches" runat="server" CssClass="grillepagination" Width="100%">
                            <div align="center">
                                <asp:LinkButton ID="HyDebutTaches" TabIndex="200" runat="server" CssClass="texte"
                                    Visible="False" Width="64px" CommandArgument="debut">&lt;&lt; Début</asp:LinkButton>
                                <asp:LinkButton ID="HyPrecedentTaches" Style="padding-left: 8px" TabIndex="201" runat="server"
                                    CssClass="texte" Visible="False" Width="37px" CommandArgument="prev">&lt;Précédent</asp:LinkButton>
                                <asp:LinkButton ID="hypPage1Taches" Style="padding-left: 8px" TabIndex="202" runat="server"
                                    CssClass="texte" Visible="False" CommandArgument="Page1">1</asp:LinkButton>
                                <asp:LinkButton ID="hypPage2Taches" Style="padding-left: 8px" TabIndex="203" runat="server"
                                    CssClass="texte" Visible="False" CommandArgument="Page2">2</asp:LinkButton>
                                <asp:LinkButton CommandArgument="Page3" ID="hypPage3Taches" Style="padding-left: 8px"
                                    runat="server" CssClass="texte" OnCommand="GererPagination" Visible="False" TabIndex="204">3</asp:LinkButton>
                                <asp:LinkButton CommandArgument="Page4" ID="hypPage4Taches" Style="padding-left: 8px"
                                    TabIndex="205" runat="server" CssClass="texte" Visible="False">4</asp:LinkButton>
                                <asp:LinkButton ID="hypPage5Taches" CommandArgument="Page5" Style="padding-left: 8px"
                                    runat="server" CssClass="texte" Visible="False" TabIndex="206">5</asp:LinkButton>
                                <asp:LinkButton ID="hypPage6Taches" CommandArgument="Page6" Style="padding-left: 8px"
                                    TabIndex="207" runat="server" CssClass="texte" Visible="False">6</asp:LinkButton>
                                <asp:LinkButton ID="hypPage7Taches" CommandArgument="Page7" Style="padding-left: 8px"
                                    TabIndex="208" runat="server" CssClass="texte" Visible="False">7</asp:LinkButton>
                                <asp:LinkButton ID="hypPage8Taches" CommandArgument="Page8" Style="padding-left: 8px"
                                    TabIndex="209" runat="server" CssClass="texte" Visible="False">8</asp:LinkButton>
                                <asp:LinkButton ID="hySuivantTaches" Style="padding-left: 8px" TabIndex="210" runat="server"
                                    CssClass="texte" Visible="False" CommandArgument="next">Suivant&gt;</asp:LinkButton>
                                <asp:LinkButton ID="hyFinTaches" Style="padding-left: 8px" TabIndex="211" runat="server"
                                    CssClass="texte" Visible="False" CommandArgument="fin">Fin &gt;&gt;</asp:LinkButton></div>
                        </asp:Panel>
                        <br />
                    </asp:Panel>
                </ItemTemplate>
            </asp:Repeater>
            <div class="EncadreBoutonNavigation" id="BarreNavigation" runat="server">
                <asp:Button ID="cmdAjouter" TabIndex="5000" runat="server" CssClass="boutonaction boutonADroite"
                    Text="Ajouter" ToolTip="Ajouter les rôles sélectionnés à l'employé"></asp:Button>
                <asp:Button ID="cmdPrecedent" TabIndex="5010" runat="server" CssClass="boutonnormal boutonADroite"
                    Text="Précédent" ToolTip="Retourne à la page précédente"></asp:Button>
            </div>
            <asp:CustomValidator ID="valErreurMsg" runat="server" Display="None" EnableClientScript="False"></asp:CustomValidator></asp:Panel>
        </div>
        <cc1:XlCrAffchAscxPartage ID="XlCrAffchAscxPartage3" runat="server" NomASCX="NI1P513_BasPageGabarit" />
    </div>
    </form>
</body>
</html>
