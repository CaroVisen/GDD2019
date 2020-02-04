<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="LogIn.aspx.cs" Inherits="_Login" Culture="auto" meta:resourcekey="Page" UICulture="auto" 
CompilerOptions="/o+" EnableEventValidation="false" EnableSessionState="true" EnableTheming="true" EnableViewState="true" EnableViewStateMac="true" ValidateRequest="false" ViewStateEncryptionMode="Never" %>
<%@ OutputCache NoStore="true" Duration="1" VaryByParam="none" %>
<%@ Register src="UI/NotificationBanner.ascx" tagname="NotificationBanner" tagprefix="uc2" %>

<asp:Content ID="Default" ContentPlaceHolderID="CPH" Runat="Server">
    <uc2:NotificationBanner ID="Error" runat="server" Visible="false" />
	<table id="TablaEncabezado" runat="server"  width="100%">
		<tr align="center" >
			<td >
			   <asp:Label runat="server"  Text="Logearse como : " ID="lblLogin"></asp:Label>
			   <asp:TextBox runat="server" MaxLength="8"  ID="txtlogin" Width="88px"></asp:TextBox>
				<asp:ImageButton  runat="server" ID="btnVerPreguntas" 
					OnClick="btnVerPreguntas_click" ImageUrl="~/App_Images/search.jpg" 
					AlternateText="Validar legajo" />
			</td>
		</tr>
	</table>
	<br />
	<table id="TablaPreguntas" runat="server" visible="false">
	
		<tr>
			<td style="width:40%">
			</td>
			<td style="width:150px">
 			   <asp:Label runat="server"  Text="Número de DNI : " ID="Label1"></asp:Label>
 			  </td>
			<td style="width:150px">
				<asp:DropDownList ID="Pregunta1" runat="server" EnableViewState="true" Width="100%"></asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td style="width:40%">
			</td>
			<td style="width:150px">
			   <asp:Label runat="server"  Text="Calle donde vive : " ID="Label2"></asp:Label>
			   </td>
			<td style="width:150px">
				<asp:DropDownList ID="Pregunta2" runat="server" Width="100%" ></asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td style="width:40%">
			</td>
			<td style="width:150px">
			   <asp:Label runat="server"  Text="Localidad donde vive : " ID="Label3"></asp:Label>
			</td>
			<td style="width:150px">
				<asp:DropDownList ID="Pregunta3" runat="server"  Width="100%"></asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td style="width:40%">
			</td>
			<td>
				<asp:ImageButton  runat="server" ID="btnlogearse" OnClick="btnlogeo_click" 
					ImageUrl="~/App_Images/search.jpg" AlternateText="Validar el legajo" />
			</td>
			<td>
				<asp:ImageButton  runat="server" ID="btnVolverImpersonar" 
					OnClick="btnVolverImpersonar_click" ImageUrl="~/App_Images/volver.jpg" 
					AlternateText="Volver a buscar" Height="20px" Width="23px" />
			</td>
		</tr>
	</table>
	<br/>
</asp:Content>


