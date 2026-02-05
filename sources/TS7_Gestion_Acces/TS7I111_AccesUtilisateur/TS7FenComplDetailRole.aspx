<%@ Register TagPrefix="cc1" Namespace="Rrq.Web.ServicesCommunsPetitsSystemes.ScenarioTransactionnel"
    Assembly="XL5I021_GestnDialg" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TS7FenComplDetailRole.aspx.vb"
    Inherits="TS7I111_AccesUtilisateur.TS7FenComplDetailRole" %>

<%@ Register TagPrefix="cc2" Namespace="XL2I121_Textbox" Assembly="XL2I121_Textbox" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>
        <%=session("mstrTitre")%>
    </title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1" />
    <meta name="vs_defaultClientScript" content="JavaScript" />
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <link id="stylePolice" href="../F_Style/TS7styles.css" type="text/css" rel="stylesheet" />
    <link id="stylePoliceGros" href="" type="text/css" rel="stylesheet" />
    <script src="../Code/Scripts/NI1police.js" type="text/javascript"></script>
    <script src="../Code/Scripts/NI1ChangementContenu.js" type="text/javascript"></script>
    <base target="_self" />
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <p>
        <cc1:XlCrGererDialogue ID="ctlGestionDialogue" runat="server"></cc1:XlCrGererDialogue>
    </p>
    <div align="center" style="padding-left: 20px; padding-top: 20px">
        <table cellpadding="1" cellspacing="0" width="90%" border="0">
            <tr>
                <td>
                    <cc2:XlCrTextbox ID="txtSaisie" runat="server" TextMode="MultiLine" Width="600px"
                        Rows="15" MaxLength="512" TabIndex="100"></cc2:XlCrTextbox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="zonebouton" align="right" style="padding-top: 10px">
                        <asp:Button ID="cmdBouton1" runat="server" Text="action1" CssClass="boutonnormal"
                            TabIndex="110"></asp:Button>&nbsp;&nbsp;
                        <asp:Button ID="cmdBouton2" runat="server" Text="action2" CssClass="boutonnormal"
                            TabIndex="120"></asp:Button>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
