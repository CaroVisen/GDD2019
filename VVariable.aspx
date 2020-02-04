<%@ Page Title="" Language="C#"  EnableEventValidation="true"  EnableViewState="True" MasterPageFile="~/MasterPage.master"  Async="true" AutoEventWireup="true" CodeFile="VVariable.aspx.cs" Inherits="VVariable" %>

  
<%@ Register Assembly="Infragistics2.WebUI.UltraWebGrid.v6.3, Version=6.3.20063.1054, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.UltraWebGrid" TagPrefix="igtbl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH" Runat="Server">

	<table>
		<tr>
			<td>
				
				<asp:Label ID="lblbuscar" runat="server" Text="Cadena a buscar"></asp:Label>
				
			</td>
			<td>
				<asp:TextBox ID="txtbuscar" runat="server" onkeyup="if (event.keyCode == 13) __doPostBack('','');" ></asp:TextBox>
			</td>
			<td>
			
				<asp:LinkButton ID="lnkbuscar"  OnClick="Buscar"  runat="server">Buscar Evaluaciones</asp:LinkButton>
			
			</td>
		</tr>
	</table>

	
	<igtbl:UltraWebGrid id="gvwEvaluations"  Width="300px"  runat="server" DataKeyField="Id"   OnInitializeRow="gvwEvaluations_InitializeRow"   SkinID = "DefaultView"></igtbl:UltraWebGrid>

</asp:Content>

