<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Views.ascx.cs" Inherits="UI_Views" EnableViewState="false"%>
<script language="javascript" type="text/javascript">
    function switchViews(obj,row)
    {
        var div = document.getElementById(obj);
        var img = document.getElementById('img' + obj);

        if (div.style.display=="none")
        {
            div.style.display = "inline";
            if (row=='alt')
            {
                img.src="img/exp.gif";
                mce_src="img/exp.gif";
            }
            else
            {
                img.src="img/exp.gif";
                mce_src="img/exp.gif";
            }
            img.alt = "Ocultar detalles";
        }
        else
        {
            div.style.display = "none";
            if (row=='alt')
            {
                img.src="img/col.gif";
                mce_src="img/col.gif";
            }
            else
            {
                img.src="img/col.gif";
                mce_src="img/col.gif";
            }
            img.alt = "Mostrar detalles";
        }
    }
</script>
<%--<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" DataKeyNames="EvaluatedName" OnRowDataBound="gv_RowDataBound" Width="650px">
    <Columns>
        <asp:TemplateField ItemStyle-Width="10%">
            <ItemTemplate>
                <a href="javascript:switchViews('div<%# Eval("LocalUO") %>', 'one');">
                    <img id="imgdiv<%# Eval("LocalUO") %>" alt="Mostrar detalles" border="0" src="img/col.gif" />
                </a>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <a href="javascript:switchViews('div<%# Eval("LocalUO") %>', 'alt');">
                    <img id="imgdiv<%# Eval("LocalUO") %>" alt="Mostrar detalles" border="0" src="img/col.gif" />
                </a>
            </AlternatingItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="LocalUO" HeaderText="Local" ItemStyle-Width="40%" />
        <asp:BoundField DataField="CountEval" HeaderText="Cant. Eval." ItemStyle-Width="10%" />
        <asp:TemplateField>
            <ItemTemplate>
                    </td>
                </tr>
                <tr>
                    <td colspan="100%">
                        <div id="div<%# Eval("LocalUO") %>" style="display:none;position:relative;left:25px;" >
                            <asp:GridView ID="GridView2" runat="server" Width="80%" AutoGenerateColumns="false" DataKeyNames="EvaluatedID" EmptyDataText="_Empty_Data_Text">
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>--%>

<asp:Panel ID="Panel1" runat="server"></asp:Panel>
<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" EnableViewState="false" >
<Columns>
<asp:BoundField DataField="Gerencia" /> 
<asp:BoundField DataField="Area" /> 
<asp:BoundField DataField="FullName" /> 
</Columns>
</asp:GridView>