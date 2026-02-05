<%@ Register TagPrefix="cc2" Namespace="Rrq.Web.ServicesCommunsPetitsSystemes.ScenarioTransactionnel"
    Assembly="XL5I021_GestnDialg" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TS7SSupprimerEmploye.aspx.vb"
    Inherits="TS7I111_AccesUtilisateur.TS7SSupprimerEmploye" %>

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
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <div class="page">
        <cc1:XlCrAffchAscxPartage ID="XlCrAffchAscxPartage1" runat="server" NomASCX="NI1P512_BandeauPIVGabarit" />
        <cc1:XlCrAffchAscxPartage ID="XlCrAffchAscxPartage2" runat="server" NomASCX="NI1P511_MenuGaucheGabarit" />
        <div class="pageCentre texte">
            <asp:Label ID="lblTitre" runat="server" CssClass="titre">Supprimer un employé</asp:Label>
            <asp:ValidationSummary ID="ValPage" runat="server" CssClass="Validation valPagex">
            </asp:ValidationSummary>
            <p>
                <asp:Button ID="cmdRechercherEmploye" runat="server" Text="Rechercher un employé"
                    CausesValidation="False" ToolTip="Effectuer la recherche d'un employé" TabIndex="100" />
                <asp:Label ID="lblEmployeRecherche" runat="server" CssClass="etiquette"></asp:Label>
                <asp:CustomValidator ID="valMessageErreur" runat="server" Display="None" EnableClientScript="False"></asp:CustomValidator></p>
            <br />
            <cc3:NiCrLibelle ID="NiCrDAteEffective" runat="server" AfficherPuce="False">Date effective de la demande</cc3:NiCrLibelle><br />
            <cc4:XlCrDate ID="XlCrDatEffective" runat="server" Width="99px" CssClass="date" CssClassBouton=""
                PositionCalendrier="Sous_le_textbox" />
            <asp:CustomValidator ID="valDatEffectiv" runat="server" Display="None" EnableClientScript="False"></asp:CustomValidator>
            <div class="EncadreBoutonNavigation" id="BarreNavigation" runat="server">
                <asp:Button ID="cmdSupprimer" TabIndex="5000" runat="server" CssClass="boutonaction boutonADroite"
                    Width="120px" Text="Supprimer" ToolTip="Supprimer l'employé sélectionné"></asp:Button>
                <asp:Button ID="cmdAnnuler" TabIndex="5010" runat="server" CssClass="boutonnormal boutonADroite"
                    Width="120px" Text="Annuler" ToolTip="Retourner à la page <Gérer les rôles>">
                </asp:Button>
            </div>
            <p>
                &nbsp;</p>
        </div>
        <cc1:XlCrAffchAscxPartage ID="XlCrAffchAscxPartage3" runat="server" NomASCX="NI1P513_BasPageGabarit" />
    </div>
    </form>
   </body>
</html>
