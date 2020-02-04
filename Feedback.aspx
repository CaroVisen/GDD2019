<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Feedback.aspx.cs" Inherits="_Default" Culture="auto" meta:resourcekey="Page"
    UICulture="auto" CompilerOptions="/o+" EnableEventValidation="false" EnableSessionState="true"
    EnableTheming="true" EnableViewState="true" EnableViewStateMac="false" ValidateRequest="false"
    ViewStateEncryptionMode="Never" %>

<%@ OutputCache NoStore="true" Duration="1" VaryByParam="none" %>
<asp:Content ID="Default" ContentPlaceHolderID="CPH" runat="Server">
    <table runat="server">
        <tr>
            <td>
                <asp:CheckBoxList ID="chkFeedback" runat="server" Width="350px" EnableViewState="true">
                </asp:CheckBoxList>
                <asp:Button ID="btnFeedback" Text="Feedback realizado" runat="server" OnClick="btnFeedback_Click" />
            </td>
            <td style="vertical-align: top">
                <asp:Label Text="Feedback realizado" runat="server" /><br />
                <asp:ListBox ID="lbxFeedback" runat="server" Width="350px" Height="300px" />
                <br />
                <asp:Button ID="btnFeedback0" Text="Eliminar feedback" runat="server" OnClick="btnFeedback0_Click" />
            </td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">
// <!CDATA[
// ]]>
    </script>

</asp:Content>
<%--<%@ Page Language="C#" CompilerOptions="/o+" EnableEventValidation="false" EnableSessionState="false" EnableTheming="false" EnableViewState="false" EnableViewStateMac="false" ValidateRequest="false" ViewStateEncryptionMode="Never" %>--%>