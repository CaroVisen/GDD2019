<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Notification.aspx.cs" Inherits="Notification" Culture="auto" meta:resourcekey="Page" UICulture="auto" %>

<%@ Register src="UI/NotificationBanner.ascx" tagname="NotificationBanner" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH" Runat="Server">
    
    <uc1:NotificationBanner ID="NotificationBanner1" runat="server" />
</asp:Content>

