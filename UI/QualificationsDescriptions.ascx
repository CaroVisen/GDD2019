<%@ Control Language="C#" AutoEventWireup="true" CodeFile="QualificationsDescriptions.ascx.cs" Inherits="UI_QualificationsDescriptions" %>

<asp:Table ID="Table" Width="100%" BorderWidth="0" CellSpacing="1" CellPadding="0" runat="server">
    <asp:TableRow>
        <asp:TableCell ColumnSpan="2" CssClass="td01">
            <asp:Label ID="Label12" meta:resourcekey="lblHeader" runat="server"></asp:Label></asp:TableCell>
    </asp:TableRow>

    <asp:TableRow>
        <asp:TableCell CssClass="td03" Width="120px">
            <asp:Label ID="lblName" meta:resourcekey="lblQualification" runat="server"></asp:Label></asp:TableCell>
        <asp:TableCell CssClass="td03">
            <asp:Label ID="lblPost" meta:resourcekey="lblDescription" runat="server"></asp:Label></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell CssClass="td06" Width="120px">
            <asp:Label ID="Label1" Text="1" runat="server"></asp:Label></asp:TableCell>
        <asp:TableCell CssClass="td07">
            <asp:Label CssClass="descripcion" ID="Label2" meta:resourcekey="lblDescriptionA1" runat="server"></asp:Label>
            <asp:Label ID="Label3" meta:resourcekey="lblDescriptionA" runat="server"></asp:Label>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell CssClass="td06" Width="120px">
            <asp:Label ID="Label4" Text="2" runat="server"></asp:Label></asp:TableCell>
        <asp:TableCell CssClass="td07">
            <asp:Label CssClass="descripcion" ID="Label13" meta:resourcekey="lblDescriptionB1" runat="server"></asp:Label>
            <asp:Label ID="Label5" meta:resourcekey="lblDescriptionB" runat="server"></asp:Label>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell CssClass="td06" Width="120px">
            <asp:Label ID="Label6" Text="3" runat="server"></asp:Label></asp:TableCell>
        <asp:TableCell CssClass="td07">
            <asp:Label CssClass="descripcion" ID="Label14" meta:resourcekey="lblDescriptionC1" runat="server"></asp:Label><asp:Label ID="Label7" meta:resourcekey="lblDescriptionC" runat="server"></asp:Label></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell CssClass="td06" Width="120px">
            <asp:Label ID="Label8" Text="4" runat="server"></asp:Label></asp:TableCell>
        <asp:TableCell CssClass="td07">
            <asp:Label CssClass="descripcion" ID="Label15" meta:resourcekey="lblDescriptionCP1" runat="server"></asp:Label><asp:Label ID="Label9" meta:resourcekey="lblDescriptionCP" runat="server"></asp:Label></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell CssClass="td06" Width="120px">
            <asp:Label ID="Label10" Text="5" runat="server"></asp:Label></asp:TableCell>
        <asp:TableCell CssClass="td07">
            <asp:Label CssClass="descripcion" ID="Label16" meta:resourcekey="lblDescriptionD1" runat="server"></asp:Label><asp:Label ID="Label11" meta:resourcekey="lblDescriptionD" runat="server"></asp:Label></asp:TableCell>
    </asp:TableRow>
</asp:Table>
<br />
