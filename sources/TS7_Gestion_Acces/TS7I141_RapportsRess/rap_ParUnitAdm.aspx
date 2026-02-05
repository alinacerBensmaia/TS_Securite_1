<%@ Page Language="vb" AutoEventWireup="False" CodeBehind="rap_ParUnitAdm.aspx.vb"
    Inherits="TS7I141_RapportsRess.rap_ParUnitAdm" %>

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
            <asp:Label ID="lblTitre" runat="server" CssClass="titre">Rapports d'assignation des rôles aux employés</asp:Label><br />
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
                               <asp:BoundColumn DataField="No"   ReadOnly="True" Visible="False">
							    </asp:BoundColumn>
							    <asp:TemplateColumn SortExpression="" HeaderText=""  >
                                    <ItemTemplate>
											<cc3:NiCuCaseCocheGrille id="chkSelectionAutresUA"  EnableViewState="true" runat="server"  IdentifiantUnique='<%# Container.DataItem("No") %>' Visible="True" checked='<%# ctype(DataBinder.Eval(Container.DataItem, "SELECT"), String) = "O"%>'>
											</cc3:NiCuCaseCocheGrille>
                                       
                                    </ItemTemplate>
                                    <Itemstyle horizontalalign="Center" width="2%" />
                                    
                                </asp:TemplateColumn>
                            
                				<asp:TemplateColumn ItemStyle-Wrap="true">
                				
									<ItemTemplate>
						                <asp:Label ID="lblNomUA" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.No")  %>'></asp:Label>
                                    </ItemTemplate>
                				 <headerstyle width="20%" />
								</asp:TemplateColumn>
                            </columns>
                                <pagerstyle visible="False"></pagerstyle>
                            </cc3:nicugrillepagetrie>
                    </td>
                </tr>
            </table>
            
            <br />
            <table class="texte" cellspacing="0" cellpadding="0" width="100%">
                <tr>
                    <td align="right">
                        <asp:Button ID="cmdAfficher" runat="server" BackColor="#D66F2C"
                            Text="Produire le rapport" style="height:22px;color: white;padding-left:15px;padding-right:15px;border:1px solid white;	outline:1px solid black;font-weight:bold;font-size: 12px;" />
                    </td>
                </tr>
            </table>
        </div>
        <cc1:XlCrAffchAscxPartage ID="XlCrAffchAscxPartage3" runat="server" NomASCX="NI1P513_BasPageGabarit" />
    </div>
    </form>
</body>
</html>
