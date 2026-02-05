<%@ Page Language="vb" AutoEventWireup="False" CodeBehind="rap_ParUnitAdm.aspx.vb"
    Inherits="TS7I131_GererRapports.rap_ParUnitAdm" %>

<%@ Register TagPrefix="cc2" Namespace="Rrq.Web.ServicesCommunsPetitsSystemes.ScenarioTransactionnel"
    Assembly="XL5I021_GestnDialg" %>
<%@ Import Namespace="System.ComponentModel" %>
<%@ Register Assembly="XL2I111_CtrlDate" Namespace="Rrq.Web.ServicesCommunsPetitsSystemes.Utilitaires"
    TagPrefix="cc4" %>
<%@ Register Assembly="NI1I213_ControlesBase" Namespace="Rrq.Web.GabaritsPetitsSystemes.ControlesBase"
    TagPrefix="cc3" %>
<%@ Register TagPrefix="cc1" Namespace="Rrq.Web.ServicesCommunsPetitsSystemes.Utilitaires"
    Assembly="XL2I041_CtlAffchAscxPartage" %>
<%@ Register TagPrefix="cc5" Namespace="XL2I121_Textbox" Assembly="XL2I121_Textbox" %>
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
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <div class="page">
        <cc1:XlCrAffchAscxPartage ID="XlCrAffchAscxPartage1" runat="server" NomASCX="NI1P512_BandeauPIVGabarit" />
        <cc1:XlCrAffchAscxPartage ID="XlCrAffchAscxPartage2" runat="server" NomASCX="NI1P511_MenuGaucheGabarit" />
        <div class="pageCentre texte">
            <asp:Label ID="lblTitre" runat="server" CssClass="titre">Rapports d'assignation des rôles aux employés</asp:Label>
            <asp:Panel ID="pnlInfo" runat="server" CssClass="ValPageBob Validation" Visible="false">
                <ul>
                    <li>
                        <asp:Label ID="lblMessageInfo" runat="server" Text="Info!!!">Info!!!</asp:Label></li></ul>
            </asp:Panel>
            <asp:ValidationSummary ID="ValPage" runat="server" CssClass="Validation valPagex">
            </asp:ValidationSummary>
            <br />
            <asp:Label ID="lblInfo" runat="server" CssClass="soustitre">Choisir au plus cinq unités administratives</asp:Label>
            <asp:CustomValidator ID="ValErreur" runat="server" Display="None" EnableClientScript="False"></asp:CustomValidator>
            <table class="espaceTableau texte degrade" cellspacing="0" cellpadding="0" width="100%">
                <tr>
                    <td>
                        <cc3:nicugrillepagetrie id="grdUADemandeur" tabIndex="210" runat="server" CssClass="grille texte"
                            Width="100%" AllowSorting="False" AllowPaging="False" ShowFooter="True" cellpadding="1"
                            cellspacing="1" ItemStyle-CssClass="ligneimpaire" AlternatingItemStyle-CssClass="lignepaire"
                            FooterStyle-CssClass="bastableau" headerStyle-CssClass="entete" PageSize="10000"
                            GridLines="None" BorderWidth="0px" AutoGenerateColumns="False">
                            <footerstyle cssclass="bastableau"></footerstyle>
                            <alternatingitemstyle cssclass="lignepaire"></alternatingitemstyle>
                            <itemstyle cssclass="ligneimpaire"></itemstyle>
                            <headerstyle cssclass="entete"></headerstyle>
                            <columns>
                               <asp:BoundColumn DataField="IDRole"   ReadOnly="True" Visible="False">
							    </asp:BoundColumn>
							    <asp:TemplateColumn SortExpression="" HeaderText=""  >
                                    <ItemTemplate>
                                       
											<cc3:NiCuCaseCocheGrille id="chkSelection"  EnableViewState="true" runat="server"  IdentifiantUnique='<%# Container.DataItem("IDRole") %>' Visible="True" checked='<%# ctype(DataBinder.Eval(Container.DataItem, "SELECT"),string)="O"%>'>
											</cc3:NiCuCaseCocheGrille>
                                       
                                    </ItemTemplate>
                                    <itemstyle horizontalalign="Center" width="2%" />
                                    
                                </asp:TemplateColumn>
                            
                				<asp:TemplateColumn ItemStyle-Wrap="true"  HeaderText="Vos unités administratives">
									<ItemTemplate>
						                <asp:Label ID="lblNomUA" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NoAbbreviation") %>'></asp:Label>
                                    </ItemTemplate>
                				 <headerstyle width="20%" />
								</asp:TemplateColumn>
                            </columns>
                            <pagerstyle visible="False"></pagerstyle>
                        </cc3:nicugrillepagetrie>
                    </td>
                </tr>
            </table>
            <cc1:XlCrAffchAscxPartage ID="XLCrUAAutres" runat="server" NomASCX="NI1P516_PlusMoinsInfos"
                Visible="true" />
            <asp:Panel ID="pnlListeRoles" runat="server" Visible="True">
                <table class="texte" cellspacing="0" cellpadding="0" width="100%">
                    <tr>
                        <td>
                            <cc3:nicugrillepagetrie id="grdUAAutres" tabIndex="210" runat="server" CssClass="grille texte"
                                Width="100%" AllowSorting="False" AllowPaging="False" ShowFooter="True" cellpadding="1"
                                cellspacing="1" ItemStyle-CssClass="ligneimpaire" AlternatingItemStyle-CssClass="lignepaire"
                                FooterStyle-CssClass="bastableau" headerStyle-CssClass="entete" BorderWidth="0px"
                                GridLines="None" PageSize="10000" AutoGenerateColumns="False">
                                <footerstyle cssclass="bastableau"></footerstyle>
                                <alternatingitemstyle cssclass="lignepaire"></alternatingitemstyle>
                                <itemstyle cssclass="ligneimpaire"></itemstyle>
                                <headerstyle cssclass="entete"></headerstyle>
                                <columns>
                               <asp:BoundColumn DataField="IDRole"   ReadOnly="True" Visible="False">
							    </asp:BoundColumn>
							    <asp:TemplateColumn SortExpression="" HeaderText=""  >
                                    <ItemTemplate>
											<cc3:NiCuCaseCocheGrille id="chkSelectionAutresUA"  EnableViewState="true" runat="server"  IdentifiantUnique='<%# Container.DataItem("IDRole") %>' Visible="True" checked='<%# ctype(DataBinder.Eval(Container.DataItem, "SELECT"),string)="O"%>'>
											</cc3:NiCuCaseCocheGrille>
                                       
                                    </ItemTemplate>
                                    <Itemstyle horizontalalign="Center" width="2%" />
                                    
                                </asp:TemplateColumn>
                            
                				<asp:TemplateColumn ItemStyle-Wrap="true">
                				<%--<HeaderTemplate>
                				<asp:Label runat="server" ID="titreAutresUA"><%=AfficherTitreAutresUA()%></asp:Label>
                				</HeaderTemplate>--%>
									<ItemTemplate>
						                <asp:Label ID="lblNomUA" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NoAbbreviation")  %>'></asp:Label>
                                    </ItemTemplate>
                				 <headerstyle width="20%" />
								</asp:TemplateColumn>
                            </columns>
                                <pagerstyle visible="False"></pagerstyle>
                            </cc3:nicugrillepagetrie>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <table class="texte" cellspacing="0" cellpadding="0" width="100%">
                <tr>
                    <td>
                        <asp:Button ID="cmdAfficher" runat="server" CssClass="boutonaction boutonADroite"
                            Text="Produire le rapport" />
                    </td>
                </tr>
            </table>
        </div>
        <cc1:XlCrAffchAscxPartage ID="XlCrAffchAscxPartage3" runat="server" NomASCX="NI1P513_BasPageGabarit" />
    </div>
    </form>
</body>
</html>
