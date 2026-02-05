<%@ Register TagPrefix="cc2" Namespace="Rrq.Web.ServicesCommunsPetitsSystemes.ScenarioTransactionnel"
    Assembly="XL5I021_GestnDialg" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TS7SPageConfirmation.aspx.vb"
    Inherits="TS7I111_AccesUtilisateur.TS7SPageConfirmation" %>

<%@ Register Assembly="XL2I121_Textbox" Namespace="XL2I121_Textbox" TagPrefix="cc4" %>
<%@ Register Assembly="NI1I213_ControlesBase" Namespace="Rrq.Web.GabaritsPetitsSystemes.ControlesBase"
    TagPrefix="cc3" %>
<%@ Register TagPrefix="cc1" Namespace="Rrq.Web.ServicesCommunsPetitsSystemes.Utilitaires"
    Assembly="XL2I041_CtlAffchAscxPartage" %>
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
            <asp:Label ID="lblTitre" runat="server" CssClass="titre">Page de confirmation</asp:Label><asp:CustomValidator
                ID="valErreur" runat="server" Display="None" EnableClientScript="False"></asp:CustomValidator>
            <asp:Panel ID="pnlInfo" runat="server" CssClass="ValPageBob Validation" Visible="False">
                <ul>
                    <li>
                        <asp:Label ID="lblMessageInfo" runat="server" Text="Info!!!">Info!!!</asp:Label></li></ul>
            </asp:Panel>
            <asp:ValidationSummary ID="ValPage" runat="server" CssClass="Validation valPagex">
            </asp:ValidationSummary>
            <br />
            <cc3:NiCrLibelle ID="NiCrInfoUtilisateur" runat="server" CssClass="soustitre">Informations sur l'employé</cc3:NiCrLibelle>
            <table class="espaceTableau texte degrade" cellspacing="0" cellpadding="0" width="100%">
                <tr>
                    <td width="50%">
                        <cc3:NiCrLibelle ID="NiCrNomEmploye" runat="server" AfficherPuce="False">Nom</cc3:NiCrLibelle>&nbsp;
                        <asp:Label ID="lblValeurNomEmploye" runat="server" CssClass="texte"></asp:Label>
                    </td>
                    <td>
                        <cc3:NiCrLibelle ID="NiCrPrenomEmploye" runat="server" AfficherPuce="False">Prénom</cc3:NiCrLibelle>&nbsp;
                        <asp:Label ID="lblValeurPrenomEmploye" runat="server" CssClass="texte"></asp:Label>
                    </td>
                </tr>
                <%--<tr><td colspan="2">
                                <cc3:NiCrLibelle ID="NiCrCourriel" runat="server" AfficherPuce="False">Courriel</cc3:NiCrLibelle>&nbsp;
                             <asp:Label ID="lblValeurCourrielEmploye" runat="server" CssClass="texte"></asp:Label></td>
                         </tr>--%>
                <tr>
                    <td>
                        <cc3:NiCrLibelle ID="NiCrVille" runat="server" AfficherPuce="False">Ville</cc3:NiCrLibelle>&nbsp;
                        <asp:Label ID="lblValeurVille" runat="server" CssClass="texte"></asp:Label>
                    </td>
                    <td>
                        <cc3:NiCrLibelle ID="NiCrDateFinContrat" runat="server" AfficherPuce="False">Fin de contrat</cc3:NiCrLibelle>&nbsp;
                        <asp:Label ID="lblValeurDateFinContrat" runat="server" CssClass="texte"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblUA1" runat="server" CssClass="etiquette">Unité administrative</asp:Label>&nbsp;
                        <asp:Label ID="lblValeurUAPrinc" runat="server" CssClass="texte"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlRetrait" runat="server">
                <br />
                <cc3:NiCrLibelle ID="NiCrLibelle1" runat="server" CssClass="soustitre" Width="300px">Retrait d'information à l'employé</cc3:NiCrLibelle>
                <div class="degrade">
                    <asp:Label ID="lblValeurUA" runat="server" CssClass="texte" Visible="False"></asp:Label>
                    <asp:Label ID="lblValeurEquip" runat="server" CssClass="texte" Visible="False"></asp:Label>
                    &nbsp; &nbsp;</div>
            </asp:Panel>
            <br />
            <asp:Label ID="lblRoles" runat="server" CssClass="soustitre">Rôle(s) assigné(s) à l'utilisateur:</asp:Label>
            <div class="degrade">
                <asp:DataGrid ID="grdRolesMetier" TabIndex="210" runat="server" CssClass="grille texte"
                    Width="100%" AllowSorting="false" GridLines="None" ShowFooter="True" CellPadding="1"
                    CellSpacing="1" ItemStyle-CssClass="ligneimpaire" AlternatingItemStyle-CssClass="lignepaire"
                    FooterStyle-CssClass="bastableau" HeaderStyle-CssClass="entete" BorderWidth="0px"
                    AutoGenerateColumns="False">
                    <FooterStyle CssClass="bastableau"></FooterStyle>
                    <AlternatingItemStyle CssClass="lignepaire"></AlternatingItemStyle>
                    <ItemStyle CssClass="ligneimpaire"></ItemStyle>
                    <HeaderStyle CssClass="entete"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="ID" ReadOnly="True" Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField="NomAAfficher" ReadOnly="True" Visible="true" HeaderText="R&#244;les métiers">
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Prolonger jusqu'au" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label ID="lblDatFin" CssClass="Texte" Width="100%" Text='<%#DataBinder.Eval(Container,"DataItem.DateFin")%>'
                                    Visible="True" runat="server"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="15%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Action sur le r&#244;le" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label ID="lblAction" CssClass="Texte" Width="100%" Text='<%#DataBinder.Eval(Container,"DataItem.ActionAfficher") %>'
                                    Visible="True" runat="server"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="20%" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
                <asp:DataGrid ID="grdRolesTaches" TabIndex="210" runat="server" CssClass="grille texte"
                    Width="100%" AllowSorting="false" GridLines="None" ShowFooter="True" CellPadding="1"
                    CellSpacing="1" ItemStyle-CssClass="ligneimpaire" AlternatingItemStyle-CssClass="lignepaire"
                    FooterStyle-CssClass="bastableau" HeaderStyle-CssClass="entete" BorderWidth="0px"
                    AutoGenerateColumns="False">
                    <FooterStyle CssClass="bastableau"></FooterStyle>
                    <AlternatingItemStyle CssClass="lignepaire"></AlternatingItemStyle>
                    <ItemStyle CssClass="ligneimpaire"></ItemStyle>
                    <HeaderStyle CssClass="entete"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="ID" ReadOnly="True" Visible="False"></asp:BoundColumn>
                        <asp:BoundColumn DataField="NomAAfficher" ReadOnly="True" Visible="true" HeaderText="R&#244;les de tâches">
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Contexte" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label ID="lblContexte" CssClass="Texte" Width="100%" Text='<%#DataBinder.Eval(Container,"DataItem.Contexte")%>'
                                    Visible="True" runat="server"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="15%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Prolonger jusqu'au" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label ID="lblDatFin" CssClass="Texte" Width="100%" Text='<%#DataBinder.Eval(Container,"DataItem.DateFin")%>'
                                    Visible="True" runat="server"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="15%" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Action sur le r&#244;le" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label ID="lblAction" CssClass="Texte" Width="100%" Text='<%#DataBinder.Eval(Container,"DataItem.ActionAfficher") %>'
                                    Visible="True" runat="server"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="20%" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </div>
            <p>
                <asp:Label ID="lblTitreComptesSupplementaires" runat="server" CssClass="etiquette">Modifications aux Comptes Supplémentaires :</asp:Label><br/>
                <asp:Label ID="lblComptesSupplementaires" runat="server"></asp:Label><br />
                <br />
                <asp:Label ID="lblTitreTexteLibre" runat="server" CssClass="etiquette">La demande nécessite des accès non couverts par les rôles, précisez ceux-ci :</asp:Label><br />
                <asp:Label ID="lblTexteLibre" runat="server"></asp:Label><br />
                <br />
                <asp:Label ID="lblTitrePieceJointe" runat="server" CssClass="etiquette">Fichier Joint :</asp:Label><br/>
                <asp:Label ID="lblPieceJointe" runat="server"></asp:Label><br />
                <br />
                <asp:Label ID="lblValeurDatEffective" runat="server" CssClass="etiquette"></asp:Label>
            </p>
            <div class="EncadreBoutonNavigation" id="BarreNavigation" runat="server">
                <asp:Button ID="cmdEnregistrer" TabIndex="5000" runat="server" CssClass="boutonaction boutonADroite"
                    Width="120px" Text="Enregistrer"></asp:Button>
                <asp:Button ID="cmdPrecedent" TabIndex="5010" runat="server" CssClass="boutonnormal boutonADroite"
                    Width="120px" Text="Précédent"></asp:Button>
            </div>
            <p>
                &nbsp;</p>
        </div>
        <cc1:XlCrAffchAscxPartage ID="XlCrAffchAscxPartage3" runat="server" NomASCX="NI1P513_BasPageGabarit" />
    </div>
    </form>    
</body>
</html>
