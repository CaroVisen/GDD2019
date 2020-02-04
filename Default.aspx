<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" Culture="auto" meta:resourcekey="Page" UICulture="auto" 
CompilerOptions="/o+" EnableEventValidation="false" EnableSessionState="true" EnableTheming="true" EnableViewState="false" EnableViewStateMac="false" ValidateRequest="false" ViewStateEncryptionMode="Never" %>
<%@ OutputCache NoStore="true" Duration="1" VaryByParam="none" %>
<%@ Register src="UI/NotificationBanner.ascx" tagname="NotificationBanner" tagprefix="uc2" %>
<%@ Register src="UI/EvaluatedPeople.ascx" tagname="EvaluatedPeople" tagprefix="uc1" %>
<%--<%@ Register src="UI/QualificationsDescriptions.ascx" tagname="QualificationsDescriptions" tagprefix="uc3" %>--%>

<asp:Content ID="Default" ContentPlaceHolderID="CPH" Runat="Server">
<%--        <uc3:QualificationsDescriptions ID="QD" runat="server" />
        <br />--%>
        <uc1:EvaluatedPeople ID="EP" runat="server" />
		 <table width="100%" id="Notification" runat="server" visible="false" >
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
											<span class="titulo"><asp:Label ID="lblMessage"  runat="server"></asp:Label></span>
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

</asp:Content>


<%--<%@ Page Language="C#" CompilerOptions="/o+" EnableEventValidation="false" EnableSessionState="false" EnableTheming="false" EnableViewState="false" EnableViewStateMac="false" ValidateRequest="false" ViewStateEncryptionMode="Never" %>--%>