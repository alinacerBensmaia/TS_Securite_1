<%@ Register TagPrefix="cc2" Namespace="Rrq.Web.ServicesCommunsPetitsSystemes.ScenarioTransactionnel"
    Assembly="XL5I021_GestnDialg" %>
<%@ Import Namespace="System.ComponentModel" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TS7SGererRolesEmploye.aspx.vb"
    Inherits="TS7I111_AccesUtilisateur.TS7SGererRolesEmploye" %>

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
    <script>
        function ajouterLeCokkiePourQueLaModaleApparaisseEnCasDeChangementDePage(){
            SetCookie("NI1I521_IndAvertissementInterruption" + "TS7", "1", "persiste");
        }
    </script>
    <link id="stylePolice" href="../F_Style/TS7styles.css" type="text/css" rel="stylesheet" />
    <link id="stylePoliceGros" href="" type="text/css" rel="stylesheet" />
    <style type="text/css">
        #fileChemin {
            width: 97%;
        }
    </style>
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <div class="page">
        <cc1:XlCrAffchAscxPartage ID="XlCrAffchAscxPartage1" runat="server" NomASCX="NI1P512_BandeauPIVGabarit" />
        <cc1:XlCrAffchAscxPartage ID="XlCrAffchAscxPartage2" runat="server" NomASCX="NI1P511_MenuGaucheGabarit" />
        <div class="pageCentre texte">
            <asp:Label ID="lblTitre" runat="server" CssClass="titre">Gérer les rôles associés à un employé</asp:Label>
            <asp:Panel ID="pnlInfo" runat="server" CssClass="ValPageBob Validation" Visible="false">
                <ul>
                    <li>
                        <asp:Label ID="lblMessageInfo" runat="server" Text="Info!!!">Info!!!</asp:Label></li></ul>
            </asp:Panel>
            <asp:ValidationSummary ID="ValPage" runat="server" CssClass="Validation valPagex">
            </asp:ValidationSummary>
            <br />
            <asp:Label ID="lblInfo" runat="server" CssClass="soustitre">Information sur l'employé</asp:Label>
            <asp:CustomValidator ID="ValErreur" runat="server" Display="None" EnableClientScript="False"></asp:CustomValidator>
            <table class="espaceTableau texte degrade" cellspacing="0" cellpadding="0" width="100%">
                <tr>
                    <td width="10%">
                        <cc3:NiCrLibelle ID="NiCrNomEmploye" runat="server" AfficherPuce="True">Nom</cc3:NiCrLibelle>
                    </td>
                    <td width="40%">
                        :&nbsp;<asp:TextBox ID="txtNomEmploye" runat="server" CssClass="texte" TabIndex="100"
                            AutoPostBack="True"></asp:TextBox>
                        <asp:Label ID="lblValeurNomEmploye" runat="server" CssClass="texte"></asp:Label>
                        <asp:CustomValidator ID="valNomRequis" runat="server" Display="None" EnableClientScript="False"></asp:CustomValidator>
                    </td>
                    <td width="17%">
                        <cc3:NiCrLibelle ID="NiCrPrenomEmploye" runat="server" AfficherPuce="True">Prénom</cc3:NiCrLibelle>
                    </td>
                    <td width="33%">
                        :&nbsp;<asp:TextBox ID="txtPrenomEmploye" runat="server" CssClass="texte" TabIndex="110"
                            AutoPostBack="True"></asp:TextBox>
                        <asp:Label ID="lblValeurPrenomEmploye" runat="server" CssClass="texte"></asp:Label>
                        <asp:CustomValidator ID="valPrenomRequis" runat="server" Display="None" EnableClientScript="False"></asp:CustomValidator>
                    </td>
                </tr>
                
                <tr>
                    <td>
                        <cc3:NiCrLibelle ID="NiCrVille" runat="server" AfficherPuce="True">Ville</cc3:NiCrLibelle>
                    </td>
                    <td>
                        :&nbsp;<asp:CustomValidator ID="valVilleRequis" runat="server" Display="None" EnableClientScript="False"></asp:CustomValidator>
                        <asp:DropDownList ID="ddwVille" runat="server" CssClass="texte" TabIndex="130" Width="200px">
                        </asp:DropDownList>
                        <asp:Label ID="lblValeurVille" runat="server" CssClass="texte"></asp:Label>
                    </td>
                    <td>
                        <cc3:NiCrLibelle ID="NiCrDateFinContrat" runat="server" AfficherPuce="False">Fin de contrat</cc3:NiCrLibelle>
                    </td>
                    <td>
                        :&nbsp;<cc4:XlCrDate ID="XlCrDateFinContrat" runat="server" CssClass="date" Width="100px"
                            TabIndex="140"></cc4:XlCrDate>
                        <asp:Label ID="lblValeurDateFinContrat" runat="server" CssClass="texte"></asp:Label>
                        <asp:CustomValidator ID="ValDatFinContrat" runat="server" Display="None" EnableClientScript="False"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblUA1" runat="server" CssClass="etiquette">Unité administrative</asp:Label>
                        &nbsp;:&nbsp;
                        <asp:DropDownList ID="ddwUAPrincipal" runat="server" CssClass="texte" TabIndex="150"
                            AutoPostBack="True" Width="320px">
                        </asp:DropDownList>
                        <asp:Label ID="lblValeurUAPrinc" runat="server" CssClass="texte"></asp:Label>
                        <asp:CustomValidator ID="valUAPrincipal" runat="server" Display="None" EnableClientScript="False"></asp:CustomValidator>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Panel ID="pnlGroupRole" runat="server">
                <div>
                    <asp:PlaceHolder ID="plcResultatVide" runat="server" Visible="false">
                        <asp:Label ID="lblResultatVide" runat="server" Visible="false" Text="Aucun rôle métier ou de tâche assigné à cet employé"></asp:Label>
                    </asp:PlaceHolder>
                    <asp:CustomValidator ID="valDateValidMetier" runat="server" Display="None" EnableClientScript="False"></asp:CustomValidator>
                    <cc3:NiCuGrillePageTrie ID="grdRolesMetier" TabIndex="210" runat="server" CssClass="grille texte"
                        Width="100%" AllowSorting="True" AllowPaging="False" ShowFooter="True" CellPadding="1"
                        CellSpacing="1" ItemStyle-CssClass="ligneimpaire" AlternatingItemStyle-CssClass="lignepaire"
                        FooterStyle-CssClass="bastableau" HeaderStyle-CssClass="entete" BorderWidth="0px"
                        AutoGenerateColumns="False" PageSize="1000">
                        <FooterStyle CssClass="bastableau"></FooterStyle>
                        <AlternatingItemStyle CssClass="lignepaire"></AlternatingItemStyle>
                        <ItemStyle CssClass="ligneimpaire"></ItemStyle>
                        <HeaderStyle CssClass="entete"></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn DataField="ID" ReadOnly="True" Visible="False"></asp:BoundColumn>
                            <asp:TemplateColumn SortExpression="Nom" HeaderText="R&#244;les m&eacute;tiers">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" CssClass="Texte" Font-Strikeout='<%#ObtenirFontRetirer(DataBinder.Eval(Container, "DataItem.strSupprimer")) %>'
                                        Width="100%" Text='<%#DataBinder.Eval(Container, "DataItem.Nom")%>' Visible="True" Style='<%#AfficherSuggestion(DataBinder.Eval(Container, "DataItem.ID"), "Metier", DataBinder.Eval(Container, "DataItem.LienTachesMetiers"), "Debut")%>'
                                        runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn ItemStyle-Wrap="false">
                                <HeaderTemplate>
                                    <cc3:NiCrLibelle ID="lblDateExpiration" runat="server" CssClass="texte"><acronym title='<%= Me.ContexteApp.ObtenirMessageNonFormate("TS70023I")%>' style="color:White;font-weight:normal;">Prolonger jusqu'au</acronym> </cc3:NiCrLibelle>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%--<asp:HyperLink ID="HyperLink1" runat="server" Text='<%# iif(Eval("fieldname")="SomeType","something","something else") %>' NavigateUrl='http://someURL' /> --%>
                                    <cc4:XlCrDate ID="txtDatExpRole" runat="server" CssClass="date" Width="100px" TabIndex="140"
                                        Text='<%#IIf(Eval("strSupprimer") = "True", "", DataBinder.Eval(Container, "DataItem.DateFin")) %>'
                                        Visible='<%#Eval("strSupprimer") = "False"%>'>
                                    </cc4:XlCrDate>
                                </ItemTemplate>
                                <HeaderStyle Width="15%" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn ItemStyle-Wrap="false" HeaderText="Action sur le r&#244;le">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkAfficherDetail" runat="server" CommandName="AfficherDetail"
                                        CommandArgument='<%#CType(DataBinder.Eval(Container, "DataItem.description"), String)%>'
                                        ToolTip='<%#ObtenirInfoBulleDetail(DataBinder.Eval(Container, "DataItem.Nom")) %>'>Afficher détails<br /></asp:LinkButton>
                                    <asp:LinkButton ID="LnkRetirer" runat="server" CommandName="RetirerRole" CommandArgument='<%#CType(DataBinder.Eval(Container, "DataItem.ID"), String)%>'
                                        ToolTip='<%#ObtenirInfoBulleRetirer(DataBinder.Eval(Container, "DataItem.ID"), AfficherRetirerSupprimer(DataBinder.Eval(Container, "DataItem.strSupprimer"))) %>'> <%#AfficherRetirerSupprimer(DataBinder.Eval(Container, "DataItem.strSupprimer"))%> </asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle Width="20%" />
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle Visible="False"></PagerStyle>
                    </cc3:NiCuGrillePageTrie>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlPaging" runat="server" CssClass="grillepagination" Width="100%">
                <div align="center">
                    <asp:LinkButton ID="HyDebut" TabIndex="300" runat="server" CssClass="texte" Visible="False"
                        Width="64px" CommandArgument="debut">&lt;&lt; Début</asp:LinkButton>
                    <asp:LinkButton ID="HyPrecedent" Style="padding-left: 8px" TabIndex="301" runat="server"
                        CssClass="texte" Visible="False" Width="37px" CommandArgument="prev">&lt;Précédent</asp:LinkButton>
                    <asp:LinkButton ID="hypPage1" Style="padding-left: 8px" TabIndex="302" runat="server"
                        CssClass="texte" Visible="False">1</asp:LinkButton>
                    <asp:LinkButton ID="hypPage2" Style="padding-left: 8px" TabIndex="303" runat="server"
                        CssClass="texte" Visible="False">2</asp:LinkButton>
                    <asp:LinkButton ID="hypPage3" Style="padding-left: 8px" runat="server" CssClass="texte"
                        Visible="False" TabIndex="304">3</asp:LinkButton>
                    <asp:LinkButton ID="hypPage4" Style="padding-left: 8px" TabIndex="305" runat="server"
                        CssClass="texte" Visible="False">4</asp:LinkButton>
                    <asp:LinkButton ID="hypPage5" Style="padding-left: 8px" runat="server" CssClass="texte"
                        Visible="False" TabIndex="306">5</asp:LinkButton>
                    <asp:LinkButton ID="hypPage6" Style="padding-left: 8px" TabIndex="307" runat="server"
                        CssClass="texte" Visible="False">6</asp:LinkButton>
                    <asp:LinkButton ID="hypPage7" Style="padding-left: 8px" TabIndex="308" runat="server"
                        CssClass="texte" Visible="False">7</asp:LinkButton>
                    <asp:LinkButton ID="hypPage8" Style="padding-left: 8px" TabIndex="309" runat="server"
                        CssClass="texte" Visible="False">8</asp:LinkButton>
                    <asp:LinkButton ID="hySuivant" Style="padding-left: 8px" TabIndex="310" runat="server"
                        CssClass="texte" Visible="False" CommandArgument="next">Suivant&gt;</asp:LinkButton>
                    <asp:LinkButton ID="hyFin" Style="padding-left: 8px" TabIndex="311" runat="server"
                        CssClass="texte" Visible="False" CommandArgument="fin">Fin &gt;&gt;</asp:LinkButton></div>
            </asp:Panel>
            <br />
            <!--- grille ROLES DE TACHE -->
            <asp:Panel ID="pnlRolesTaches" runat="server">
                <div>
                    <asp:CustomValidator ID="csValidateRolesTaches" runat="server" Display="None" EnableClientScript="False"></asp:CustomValidator>
                    <cc3:NiCuGrillePageTrie ID="grdRolesTache" TabIndex="210" runat="server" CssClass="grille texte"
                        Width="100%" AllowSorting="True" AllowPaging="False" ShowFooter="True" CellPadding="1"
                        CellSpacing="1" ItemStyle-CssClass="ligneimpaire" AlternatingItemStyle-CssClass="lignepaire"
                        FooterStyle-CssClass="bastableau" HeaderStyle-CssClass="entete" BorderWidth="0px"
                        AutoGenerateColumns="False" PageSize="1000">
                        <FooterStyle CssClass="bastableau"></FooterStyle>
                        <AlternatingItemStyle CssClass="lignepaire"></AlternatingItemStyle>
                        <ItemStyle CssClass="ligneimpaire"></ItemStyle>
                        <HeaderStyle CssClass="entete"></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn DataField="ID" ReadOnly="True" Visible="False"></asp:BoundColumn>
                            <asp:TemplateColumn SortExpression="Nom" HeaderText="R&#244;les de tâches">
                                <ItemTemplate>
                                    <asp:Label ID="lblNom" CssClass="Texte" Font-Strikeout='<%#ObtenirFontRetirer(DataBinder.Eval(Container, "DataItem.strSupprimer")) %>'
                                        Width="100%" Text='<%#DataBinder.Eval(Container, "DataItem.Nom")%>' Visible="True" Style='<%#AfficherSuggestion(DataBinder.Eval(Container, "DataItem.ID"), "Taches", DataBinder.Eval(Container, "DataItem.LienTachesMetiers"), "Debut")%>'
                                        runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Contexte" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:PlaceHolder ID="plcContexteListe" runat="server">
                                        <asp:DropDownList ID="lstContexte" runat="server">
                                        </asp:DropDownList>
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="plcContexteLabel" runat="server">
                                        <asp:Label ID="lblContexte" runat="server" Text="----"></asp:Label>
                                    </asp:PlaceHolder>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn ItemStyle-Wrap="false">
                                <HeaderTemplate>
                                    <cc3:NiCrLibelle ID="NiCrLibelle1" runat="server" CssClass="texte"><acronym title='<%= Me.ContexteApp.ObtenirMessageNonFormate("TS70023I")%>' style="color:White;font-weight:normal;">Prolonger jusqu'au</acronym> </cc3:NiCrLibelle>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%--<cc4:xlcrdate id="txtDatExpRole" runat="server" cssclass="date" width="100px" TabIndex="140" text='<%#DataBinder.Eval(Container,"DataItem.DateFin") %>'></cc4:xlcrdate>--%>
                                    <cc4:XlCrDate ID="txtDatExpRole" runat="server" CssClass="date" Width="100px" TabIndex="140"
                                        Text='<%#IIf(Eval("strSupprimer") = "True", "", DataBinder.Eval(Container, "DataItem.DateFin")) %>'
                                         Visible='<%#Eval("strSupprimer") = "False"%>'>
                                    </cc4:XlCrDate>
                                </ItemTemplate>
                                <HeaderStyle Width="15%" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Action sur le r&#244;le" ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="AfficherDetail"
                                        CommandArgument='<%#CType(DataBinder.Eval(Container, "DataItem.description"), String)%>'
                                        ToolTip='<%#ObtenirInfoBulleDetail(DataBinder.Eval(Container, "DataItem.Nom")) %>'> Afficher détails<br /></asp:LinkButton>
                                    <%--<asp:LinkButton id="LnkRetirer" runat="server" CommandName="RetirerRole"  CommandArgument='<%# ctype(DataBinder.Eval(Container,"DataItem.ID"),string)%>' ToolTip='<%#ObtenirInfoBulleRetirer(DataBinder.Eval(Container,"DataItem.ID")) %>'> <%#AfficherRetirerSupprimer(DataBinder.Eval(Container, "DataItem.strSupprimer"))%> </asp:LinkButton>--%>
                                    <asp:LinkButton ID="LnkRetirer" runat="server" CommandName="RetirerRole" CommandArgument='<%#CType(DataBinder.Eval(Container, "DataItem.ID"), String)%>'
                                        ToolTip='<%#ObtenirInfoBulleRetirer(DataBinder.Eval(Container, "DataItem.ID"), AfficherRetirerSupprimer(DataBinder.Eval(Container, "DataItem.strSupprimer"))) %>'> <%#AfficherRetirerSupprimer(DataBinder.Eval(Container, "DataItem.strSupprimer"))%> </asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle Width="20%" />
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle Visible="False"></PagerStyle>
                    </cc3:NiCuGrillePageTrie>
                </div>
                <asp:Panel ID="pnlPageRole" runat="server" CssClass="grillepagination" Width="100%">
                    <div align="center">
                        <asp:LinkButton ID="hypPageRole1" TabIndex="300" runat="server" CssClass="texte"
                            Visible="False" Width="64px" CommandArgument="debut">&lt;&lt; Début</asp:LinkButton>
                        <asp:LinkButton ID="hypPageRole2" Style="padding-left: 8px" TabIndex="301" runat="server"
                            CssClass="texte" Visible="False" Width="37px" CommandArgument="prev">&lt;Précédent</asp:LinkButton>
                        <asp:LinkButton ID="hypPageRole3" Style="padding-left: 8px" TabIndex="302" runat="server"
                            CssClass="texte" Visible="False">1</asp:LinkButton>
                        <asp:LinkButton ID="hypPageRole4" Style="padding-left: 8px" TabIndex="303" runat="server"
                            CssClass="texte" Visible="False">2</asp:LinkButton>
                        <asp:LinkButton ID="hypPageRole5" Style="padding-left: 8px" runat="server" CssClass="texte"
                            Visible="False" TabIndex="304">3</asp:LinkButton>
                        <asp:LinkButton ID="hypPageRole6" Style="padding-left: 8px" TabIndex="305" runat="server"
                            CssClass="texte" Visible="False">4</asp:LinkButton>
                        <asp:LinkButton ID="hypPageRole7" Style="padding-left: 8px" runat="server" CssClass="texte"
                            Visible="False" TabIndex="306">5</asp:LinkButton>
                        <asp:LinkButton ID="hypPageRole8" Style="padding-left: 8px" TabIndex="307" runat="server"
                            CssClass="texte" Visible="False">6</asp:LinkButton>
                        <asp:LinkButton ID="hypPageRole9" Style="padding-left: 8px" TabIndex="308" runat="server"
                            CssClass="texte" Visible="False">7</asp:LinkButton>
                        <asp:LinkButton ID="hypPageRole10" Style="padding-left: 8px" TabIndex="309" runat="server"
                            CssClass="texte" Visible="False">8</asp:LinkButton>
                        <asp:LinkButton ID="hypPageRole11" Style="padding-left: 8px" TabIndex="310" runat="server"
                            CssClass="texte" Visible="False" CommandArgument="next">Suivant&gt;</asp:LinkButton>
                        <asp:LinkButton ID="hypPageRole12" Style="padding-left: 8px" TabIndex="311" runat="server"
                            CssClass="texte" Visible="False" CommandArgument="fin">Fin &gt;&gt;</asp:LinkButton></div>
                </asp:Panel>
            </asp:Panel>
            <div style="vertical-align: top;">
                <%--<asp:HyperLink ID="HyperLink1" runat="server" Text='<%# iif(Eval("fieldname")="SomeType","something","something else") %>' NavigateUrl='http://someURL' /> --%>
                <table width="100%">
                    <tr>
                        <td align="right" valign="middle" width="47%" style="white-space: nowrap;">
                            <asp:Label CssClass="texte" ID="lblConsulterAjouterRole" runat="server" Text="Consulter / Ajouter des rôles "></asp:Label>
                            &nbsp;<span style="vertical-align: middle;"><asp:Label ID="lblImgGroupeBoutons" runat="server"
                                Font-Size="X-Large" Text="{"></asp:Label></span>
                        </td>
                        <td align="left" valign="top">
                            <asp:Button CssClass="boutonnormal boutonAGauche" ID="cmdAjouterUA" runat="server"
                                Text="d'une unité administrative" Width="200px" />
                            <br style="line-height: 13px;" />
                            <br />
                            <asp:Button CssClass="boutonnormal boutonAGauche" ID="cmdAjouterEmploye" runat="server"
                                Text="d'un employé modèle" Width="200px" />
                        </td>
                    </tr>
                </table>
                <br />
                <table class="tableaupage texte degrade" id="Table1" cellspacing="0" cellpadding="0"
                    border="0">
                     <tr>
                        <td>                     
                            <cc3:NiCrLibelle ID="lblComptesSupp" runat="server" AfficherPuce="False">Comptes supplémentaires</cc3:NiCrLibelle>                    
                            <br /><br />
                            <table width="100%"  Class="texte">
                                <tr >
                                    <td style="width: 50%">
                                        Administration
                                    </td>
                                    <td>                                        
                                        Essais et soutien
                                    </td>
                                </tr>
                                <tr >
                                    <td style="width: 50%">
                                        <asp:CheckBox ID="cbADMServeur" runat="server" /> Serveur (AS)
                                    </td>
                                    <td>                                        
                                        <asp:CheckBox ID="cbEssaisAgent" runat="server" /> Essais RRSP agent (EA)
                                    </td>
                                </tr>
                                <tr >
                                    <td style="width: 50%">
                                        <asp:CheckBox ID="cbADMPoste" runat="server" /> Poste de travail (AP)
                                    </td>
                                    <td>                                        
                                        <asp:CheckBox ID="cbEssaisCE" runat="server" /> Essais RRSP chef d'équipe (EC)
                                    </td>
                                </tr>
                                <tr >
                                    <td style="width: 50%">
                                        <asp:CheckBox ID="cbADMDevelopeur" runat="server" /> Atelier de développement (AU)
                                    </td>
                                    <td>                                        
                                        <asp:CheckBox ID="cbSoutienProdAgent" runat="server" /> Soutien production RRSP agent (PA)
                                    </td>
                                </tr>
                                <tr >
                                    <td style="width: 50%">
                                        <%--<asp:CheckBox ID="cbADMCentral" runat="server" /> Central (TSS)--%>
                                    </td>
                                    <td>                                        
                                        <asp:CheckBox ID="cbSoutienProdCE" runat="server" /> Soutien production RRSP chef d'équipe (PC)
                                    </td>
                                </tr>
                            </table>                          
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <cc3:NiCrLibelle ID="lblBesoinSuppl" runat="server" AfficherPuce="False">La demande nécessite des accès non couverts par les rôles, précisez ceux-ci :</cc3:NiCrLibelle><br />
                            <cc5:XlCrTextbox ID="txtBesoinSuppl" runat="server" TextMode="MultiLine" Width="600px"
                                Rows="3" MaxLength="512" TabIndex="400"></cc5:XlCrTextbox>
                        </td>
                    </tr>
                    <tr>
                        <td>    
                            <br />
                            <cc3:NiCrLibelle ID="lblJoindreFichier" runat="server" AfficherPuce="False">Joindre un fichier</cc3:NiCrLibelle>                    
                            <br />
                            <cc5:XlCrTextbox ID="txtFichierJoint" runat="server" Width="405px" visible="false"></cc5:XlCrTextbox>
                            <asp:Button ID="btnFauxParcourir" runat="server" Height="22px" Text="Parcourir" Width="90px" Visible="false" Enabled="false" />                            
                            <asp:FileUpload ID="fileFichierJoint" runat="server" Width="500px" style="background-color:white;" onchange="ajouterLeCokkiePourQueLaModaleApparaisseEnCasDeChangementDePage()"/>
                            <asp:Button ID="btnAnnulerFichier" runat="server" Height="22px" Text="Annuler" Width="100px" Enabled="false" />                                  
                        </td>
                    </tr>
                    <%--<tr>
                        <td class="espacehaut">
                            <cc3:NiCrLibelle ID="NiCrDAteEffective" runat="server" AfficherPuce="False">Date effective de la demande</cc3:NiCrLibelle><br />
                            <cc4:XlCrDate ID="XlCrDatEffective" runat="server" Width="99px" CssClassBouton="" CssClass="date"
                                PositionCalendrier="Sous_le_textbox" ReadOnly="" />
                            <asp:CustomValidator ID="valDatEffectiv" runat="server" Display="None" EnableClientScript="False"></asp:CustomValidator>
                        </td>
                    </tr>--%>
                </table>
                <br />
                <div class="EncadreBoutonNavigation" id="BarreNavigation" runat="server">
                    <asp:Button ID="cmRecommencer" TabIndex="5010" runat="server" CssClass="boutonnormal boutonAGauche"
                        Text="Recommencer" ToolTip="Recommencer les modifications au début"></asp:Button>
                    <asp:Button ID="cmdSuivant" TabIndex="5000" runat="server" CssClass="boutonaction boutonADroite"
                        Text="Suivant" ToolTip="Accéder à la page de confirmation"></asp:Button>
                </div>
            </div>
            </div>
            <cc1:XlCrAffchAscxPartage ID="XlCrAffchAscxPartage3" runat="server" NomASCX="NI1P513_BasPageGabarit" />
        </div>
    </form>
</body>
</html>
