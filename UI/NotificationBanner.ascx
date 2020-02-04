<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NotificationBanner.ascx.cs" Inherits="UI_NotificationBanner" %>
<table width="100%">
<tr>
<td>
    <asp:Table ID="MImsg" Width="100%" BorderColor="#9F0004" BorderStyle="Dotted" BorderWidth="2" CellSpacing="0" CellPadding="5" runat="server"> 
        <asp:TableRow>
            <asp:TableCell CssClass="msg">
                <table border="0" cellpadding="5" cellspacing="0">
                    <tr>
                        <td>
                            <img width="40" height="38" src="App_Images/alerta.png" />
                        </td>
                        <td width = "100%">
                            <span class="titulo"><asp:Label ID="lblMessage" runat="server"></asp:Label></span>
                        </td>
                    </tr>
                </table>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <br />
</td>
</tr>
</table>
<br />


