<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EvaluatedPeople.ascx.cs" Inherits="UI_PeopleAssess" %>
<%@ Register Src="QualificationsDescriptions.ascx" TagName="QualificationsDescriptions" TagPrefix="uc1" %>
<%@ Register Src="NotificationBanner.ascx" TagName="NotificationBanner" TagPrefix="uc2" %>

<uc1:QualificationsDescriptions ID="QD" runat="server" />

<asp:Table ID="TblSelfEvaluation" Width="100%" BorderWidth="0" CellSpacing="1" CellPadding="0" runat="server" Visible="false">
    <asp:TableRow>
        <asp:TableCell ColumnSpan="6" CssClass="td01">
            <asp:Label ID="Label3" meta:resourcekey="lblHeaderSelfEvaluation" runat="server"></asp:Label></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell ColumnSpan="6" Height="2px"></asp:TableCell>
    </asp:TableRow>
</asp:Table>

<asp:UpdatePanel ID="UP" runat="server">
    <ContentTemplate>
        <asp:Table ID="Tbl1" Width="100%" BorderWidth="0" CellSpacing="1" CellPadding="0" runat="server">
            <asp:TableRow>
                <asp:TableCell ColumnSpan="6" CssClass="td01">
                    <asp:Label ID="Label2" meta:resourcekey="lblHeader" runat="server"></asp:Label></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="2" CssClass="td03">
                    <asp:Label ID="lblName" meta:resourcekey="lblName" runat="server"></asp:Label></asp:TableCell>
                <asp:TableCell CssClass="td03">
                    <asp:Label ID="lblPost" meta:resourcekey="lblPost" runat="server"></asp:Label></asp:TableCell>
                <asp:TableCell CssClass="td03">
                    <asp:Label ID="Label1" meta:resourcekey="lblResult" runat="server"></asp:Label></asp:TableCell>
                <asp:TableCell ColumnSpan="2" CssClass="td03">
                    <asp:Label ID="lblAs" meta:resourcekey="lblStatus" runat="server"></asp:Label></asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:Table ID="Tbl2" Width="100%" BorderWidth="0" CellSpacing="1" CellPadding="0" runat="server">
    <asp:TableRow>
        <asp:TableCell ColumnSpan="6" CssClass="td01">
            <asp:Label ID="lblHeaderConcurrent" meta:resourcekey="lblHeaderConcurrent" runat="server"></asp:Label></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell ColumnSpan="2" CssClass="td03">
            <asp:Label ID="Label4" meta:resourcekey="lblName" runat="server"></asp:Label></asp:TableCell>
        <asp:TableCell CssClass="td03">
            <asp:Label ID="Label5" meta:resourcekey="lblPost" runat="server"></asp:Label></asp:TableCell>
        <asp:TableCell CssClass="td03">
            <asp:Label ID="Label6" meta:resourcekey="lblResult" runat="server"></asp:Label></asp:TableCell>
        <asp:TableCell ColumnSpan="2" CssClass="td03">
            <asp:Label ID="Label7" meta:resourcekey="lblStatus" runat="server"></asp:Label></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell ColumnSpan="6" Height="2px"></asp:TableCell>
    </asp:TableRow>
</asp:Table>

<asp:Table ID="Tbl3" Width="100%" BorderWidth="0" CellSpacing="1" CellPadding="0" runat="server">
    <asp:TableRow>
        <asp:TableCell ColumnSpan="6" CssClass="td01">
            <asp:Label ID="lblHeaderDoubleReport" meta:resourcekey="lblHeaderDoubleReport" runat="server"></asp:Label></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell ColumnSpan="2" CssClass="td03">
            <asp:Label ID="Label9" meta:resourcekey="lblName" runat="server"></asp:Label></asp:TableCell>
        <asp:TableCell CssClass="td03">
            <asp:Label ID="Label10" meta:resourcekey="lblPost" runat="server"></asp:Label></asp:TableCell>
        <asp:TableCell CssClass="td03">
            <asp:Label ID="Label11" meta:resourcekey="lblResult" runat="server"></asp:Label></asp:TableCell>
        <asp:TableCell ColumnSpan="2" CssClass="td03">
            <asp:Label ID="Label12" meta:resourcekey="lblStatus" runat="server"></asp:Label></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell ColumnSpan="6" Height="2px"></asp:TableCell>
    </asp:TableRow>
</asp:Table>

<asp:Table ID="Tbl4" Width="100%" BorderWidth="0" CellSpacing="1" CellPadding="0" runat="server">
    <asp:TableRow>
        <asp:TableCell ColumnSpan="6" CssClass="td01">
            <asp:Label ID="lblHeaderHHRR" meta:resourcekey="lblHeaderHHRR" runat="server"></asp:Label></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell ColumnSpan="2" CssClass="td03">
            <asp:Label ID="Label14" meta:resourcekey="lblName" runat="server"></asp:Label></asp:TableCell>
        <asp:TableCell CssClass="td03">
            <asp:Label ID="Label15" meta:resourcekey="lblPost" runat="server"></asp:Label></asp:TableCell>
        <asp:TableCell CssClass="td03">
            <asp:Label ID="Label16" meta:resourcekey="lblResult" runat="server"></asp:Label></asp:TableCell>
        <asp:TableCell ColumnSpan="2" CssClass="td03">
            <asp:Label ID="Label17" meta:resourcekey="lblStatus" runat="server"></asp:Label></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell ColumnSpan="6" Height="2px"></asp:TableCell>
    </asp:TableRow>

</asp:Table>
<uc2:NotificationBanner ID="NB" runat="server" Visible="false" />

