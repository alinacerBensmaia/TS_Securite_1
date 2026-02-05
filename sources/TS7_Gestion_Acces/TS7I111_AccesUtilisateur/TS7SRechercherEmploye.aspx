<%@ Register TagPrefix="cc2" Namespace="Rrq.Web.ServicesCommunsPetitsSystemes.ScenarioTransactionnel"
    Assembly="XL5I021_GestnDialg" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TS7SRechercherEmploye.aspx.vb"
    Inherits="TS7I111_AccesUtilisateur.TS7SRechercherEmploye" %>

<%@ Register TagPrefix="cc1" Namespace="Rrq.Web.ServicesCommunsPetitsSystemes.Utilitaires"
    Assembly="XL2I041_CtlAffchAscxPartage" %>
<%@ Register TagPrefix="NI" Namespace="Rrq.Web.GabaritsPetitsSystemes.ControlesBase"
    Assembly="NI1I213_ControlesBase" %>
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
            <asp:Label ID="lblTitre" runat="server" CssClass="titre">Important</asp:Label>
            <asp:Panel ID="pnlInfo" runat="server" CssClass="ValPageBob Validation" Visible="false">
                <ul>
                    <li>
                        <asp:Label ID="lblMessageInfo" runat="server" Text="Info!!!">Info!!!</asp:Label></li></ul>
            </asp:Panel>
          <asp:ValidationSummary ID="ValPage" runat="server" CssClass="Validation valPagex">
            </asp:ValidationSummary>
           <br />
            <div Class="degrade" id="Critere" runat="server" visible="false">
                <br />
               <div Class="espaceHaut" align="left">
                    <ASP:Label ID = "lblNom" Width="150px" CssClass="etiquette" runat="server">Nom</asp:Label>
                    <ASP:TextBox ID = "txtNom" TabIndex="100" Width="200px" CssClass="texte" runat="server"></asp:TextBox>&nbsp;</div>
                <div Class="espaceHaut" align="left">
                    <ASP:Label ID = "lblPrenom" Width="150px" CssClass="etiquette" runat="server">Prénom</asp:Label>
                    <ASP:TextBox ID = "txtPrenom" TabIndex="110" Width="200px" CssClass="texte" runat="server"></asp:TextBox>&nbsp;</div>
                <div Class="espaceHaut" align="left">
                    <ASP:Label ID = "lblCodeUtil" Width="150px" CssClass="etiquette" runat="server">Code utilisateur</asp:Label>
                    <ASP:TextBox ID = "txtCodeUtilisateur" TabIndex="120" Width="200px" CssClass="texte"
                        runat="server"></asp:TextBox>&nbsp;<asp:CustomValidator ID = "valErreur" runat="server"
                            Display="None" EnableClientScript="False"></asp:CustomValidator></div>
                <ASP:Panel ID = "pnlCmd" runat="server" Style="padding-top: 10px" visible="false">
                    <ASP:Button ID = "cmdRechercher" TabIndex="130" runat="server" CssClass="boutonaction boutonADroite"
                        Width="120px" Text="Rechercher" ToolTip="Rechercher un employé"></asp:Button>
                    <ASP:Button ID = "cmdPrecedent1" TabIndex="140" runat="server" CssClass="boutonnormal boutonADroite"
                        Width="120px" Text="Précédent"></asp:Button>
                </asp:Panel>
            </div>
           <ASP:Panel ID = "pnlResultatRecherche" Style="padding-top: 20px" Width="100%" runat="server"
                HorizontalAlign="Left" Visible="false">
                <ASP:Label ID = "lblTitreSelection" runat="server" CssClass="soustitre">Employés trouvés</asp:Label>
                <div Class="degrade" align="left">
                    <NI:NiCuGrillePageTrie ID = "grdEmployes" TabIndex="180" runat="server" CssClass="grille texte"
                        Width="100%" ShowFooter="True" CellPadding="1" CellSpacing="1" ItemStyle-CssClass="ligneimpaire"
                        AlternatingItemStyle-CssClass="lignepaire" FooterStyle-CssClass="bastableau"
                        HeaderStyle-CssClass="entete" EnableViewState="False" BorderWidth="0px" AutoGenerateColumns="False"
                        PageSize="15" AllowSorting="True" AllowPaging="True">
                        <FooterStyle CssClass = "bastableau" ></FooterStyle>
                           <AlternatingItemStyle CssClass = "lignepaire" ></AlternatingItemStyle>
                           <ItemStyle CssClass = "ligneimpaire" ></ItemStyle>
                           <HeaderStyle CssClass = "entete" ></HeaderStyle>
                           <Columns>
                           <asp:TemplateColumn>
                                <ItemTemplate>
                           <NI:NiCuRadioBoutonGrille ID = "chkEmployeSelect" runat="server" TabIndex="180" GroupName="optGrpEmployes"
                                        Value='<%# DataBinder.Eval(Container.DataItem, "ID") %>'></NI:NiCuRadioBoutonGrille>
                                </ItemTemplate>
                                <HeaderStyle Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="NomComplet" SortExpression="NomComplet" ReadOnly="True"
                                HeaderText="Nom complet">
                                <HeaderStyle Width="25%"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="NoUniteAdm" SortExpression="NoUniteAdm" ReadOnly="True"
                                HeaderText="Unit&#233; adm.">
                                <HeaderStyle Width="25%"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="ID" SortExpression="ID" ReadOnly="True" HeaderText="Code utilisateur">
                                <HeaderStyle Width="20%"></HeaderStyle>
                            </asp:BoundColumn>
                        </Columns>
                        <PagerStyle Visible="False"></PagerStyle>
                    </NI:NiCuGrillePageTrie>
                    <asp:Panel CssClass="grillepagination" ID="pnlPaging" Style="padding-bottom: 10px"
                        runat="server" Width="100%" Visible ="false">
                        <asp:LinkButton ID="HyPrecedent" TabIndex="190" Visible="False" runat="server" CssClass="texte"
                            Width="37px" CommandArgument="prev" style="display:inline">&lt;Précédent</asp:LinkButton>&nbsp;&nbsp;
                        <asp:LinkButton ID="hypPage1" TabIndex="200" Visible="False" runat="server" CssClass="texte">1</asp:LinkButton>
                        <asp:LinkButton ID="hypPage2" Style="padding-left: 8px" TabIndex="210" Visible="False"
                            runat="server" CssClass="texte">2</asp:LinkButton>
                        <asp:LinkButton ID="hypPage3" Style="padding-left: 8px" TabIndex="220" Visible="False"
                            runat="server" CssClass="texte">3</asp:LinkButton>
                        <asp:LinkButton ID="hypPage4" Style="padding-left: 8px" TabIndex="230" Visible="False"
                            runat="server" CssClass="texte">4</asp:LinkButton>
                        <asp:LinkButton ID="hypPage5" Style="padding-left: 8px" TabIndex="240" Visible="False"
                            runat="server" CssClass="texte">5</asp:LinkButton>
                        <asp:LinkButton ID="hypPage6" Style="padding-left: 8px" TabIndex="250" Visible="False"
                            runat="server" CssClass="texte">6</asp:LinkButton>
                        <asp:LinkButton ID="hypPage7" Style="padding-left: 8px" TabIndex="260" Visible="False"
                            runat="server" CssClass="texte">7</asp:LinkButton>
                        <asp:LinkButton ID="hypPage8" Style="padding-left: 8px" TabIndex="270" Visible="False"
                            runat="server" CssClass="texte">8</asp:LinkButton>
                        <asp:LinkButton ID="hySuivant" Style="padding-left: 8px" TabIndex="280" Visible="False"
                            runat="server" CssClass="texte" CommandArgument="next">Suivant&gt;</asp:LinkButton></asp:Panel>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlNavigation" runat="server" Style="padding-top: 10px" CssClass="EncadreBoutonNavigation" Visible="false" >
                <asp:Button ID="cmdRecommencer" TabIndex="5020" runat="server" CssClass="boutonnormal boutonAGauche"
                    Text="Recommencer" ToolTip="Recommencer l'opération au début"></asp:Button>
                <asp:Button ID="cmdSelectionner" TabIndex="5000" runat="server" CssClass="boutonaction boutonADroite"
                    Text="Sélectionner" ToolTip="Sélectionner l'utilisateur"></asp:Button>
                <asp:Button ID="cmdPrecedent" TabIndex="5010" runat="server" CssClass="boutonnormal boutonADroite"
                    Width="120px" Text="Précédent"></asp:Button>
            </asp:Panel>
    <asp:Panel ID="pnlImportant" runat="server">
        <p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Toute demande d'habilitation d'accès doit être initiée à partir du portail de services <a href="https://retraitequebecprod.service-now.com/sp?id=index" target="_blank">CASI en ligne</a>. Pour plus d'information sur l'utilisation du portail, consultez la page <a href="http://portail/site/site0062/casienligne/Pages/default.aspx" target="_blank">Formation CASI en ligne</a> du site de la formation bureautique.
                &nbsp;</p>
    </asp:Panel>
        </div>
        
       
        <cc1:XlCrAffchAscxPartage ID="XlCrAffchAscxPartage3" runat="server" NomASCX="NI1P513_BasPageGabarit" />
    
           
       
        </div>
    </form>
</body>
</html>
