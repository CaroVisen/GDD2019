<%@ Page Title="" Language="C#"  EnableEventValidation="true"  EnableViewState="True" MasterPageFile="~/MasterPageAdmin.master"  Async="true" AutoEventWireup="true" CodeFile="Administracion.aspx.cs" Inherits="Administracion" %>

  

<asp:Content ID="Content1" ContentPlaceHolderID="CPH" Runat="Server">

	

	<table style="width: 100%">
		<tr>
			<td>
				<asp:Label ID="lblEvaluado" runat="server" Text="Evaluado :"></asp:Label>
			</td>
			<td>

	

	<asp:DropDownList ID="ddlevaluados" runat="server" AutoPostBack="True" 
		onselectedindexchanged="ddlevaluados_SelectedIndexChanged" Width="391px" 
                    CausesValidation="True">
	</asp:DropDownList>

	

			</td>
		</tr>
		<tr>
			<td>
				<asp:Label ID="lblEvaluador" runat="server" Text="Evaluador :"></asp:Label>
			</td>
			<td>

	

	<asp:DropDownList ID="ddlevaluadores" runat="server"   AutoPostBack="True" 
		onselectedindexchanged="ddlevaluadores_SelectedIndexChanged" Width="391px">
	</asp:DropDownList>

	

			</td>
		</tr>
		<tr>
			<td>
				<asp:Label ID="Label3" runat="server" Text="Concurrente :"></asp:Label>
			</td>
			<td>

	

	<asp:DropDownList ID="ddlconcurrentes" runat="server"  Width="391px">
	</asp:DropDownList>

	

			</td>
		</tr>
		<tr>
			<td>
				<asp:Label ID="Label4" runat="server" Text="Doble Reporte :"></asp:Label>
			</td>
			<td>

	

	<asp:DropDownList ID="ddldreporte" runat="server"  Width="391px">
	</asp:DropDownList>

	

			</td>
		</tr>
		<tr>
			<td colspan="3">
				<table>
				<tr>
					<td>
						<asp:Button ID="Button1" runat="server" Text="Guardar Cambios" 
							onclick="Button1_Click" />
					</td>
					<td>
						<asp:Button ID="Button2" runat="server" onclick="Button2_Click" 
							Text="Reiniciar evaluación" />
					</td>
					<td>
						<asp:Button ID="Button4" runat="server" onclick="Button4_Click" 
							Text="Reiniciar autoevaluación" />
					</td>
					<td>
						<asp:Button ID="Button3" runat="server" onclick="Eliminar_Evaluado" 
							Text="Eliminar evaluado"  />
					</td>
					<td>
						<asp:Button ID="Boton_Reimprimir" runat="server" onclick="Reimprimir_Evaluado" 
							Text="Permitir Reimpresión"  />
					</td>
				</tr>
				</table>
			</td>
		</tr>
	</table>

<script type="text/javascript">
    function onEnterpress(e) {
        //define any varible
        var KeyPress
        conole.log(e.keyCode);
        //if which property of event object is supported 
        if (e && e.which) {
            e = e

            //character code is contained in NN4's which property
            KeyPress = e.which
        }
        else {
            e = event
            KeyPress = e.keyCode
        }

        //13 is the key code of enter key
        if (KeyPress == 13) {
            //frmLogin is the name of form
            document.frmLogin.submit()
            return false
        }
        else {
            return true
        }
    }

</script>

</asp:Content>


