<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Statistics.aspx.cs" Inherits="Statistics" Culture="auto" meta:resourcekey="Page" UICulture="auto" %>
<%@ Register Assembly="Infragistics2.WebUI.UltraWebChart.v6.3, Version=6.3.20063.1054, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.UltraWebChart" TagPrefix="igchart" %>
<%@ Register Assembly="Infragistics2.WebUI.UltraWebChart.v6.3, Version=6.3.20063.1054, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.UltraChart.Resources.Appearance" TagPrefix="igchartprop" %>
<%@ Register Assembly="Infragistics2.WebUI.UltraWebChart.v6.3, Version=6.3.20063.1054, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.UltraChart.Data" TagPrefix="igchartdata" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH" Runat="Server">

<asp:UpdatePanel runat="server" UpdateMode="Conditional">
<ContentTemplate>

<table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF">
  <tr>
        <td class="td01">Estadística de avance</td>
  </tr>
  <tr>
    <td>
        <igchart:UltraChart id="UltraChart1" runat="server"  Width="680px" Height="350px" Border-Color="#ffffff" Version="6.3" Transform3D-YRotation="-18" Transform3D-XRotation="41" Transform3D-Scale="100" Transform3D-ZRotation="0" Transform3D-Perspective="50" ChartType="PieChart3D" Data-UseMinMax="False" Legend-BorderColor="White" Legend-SpanPercentage="25" Legend-Visible="True" Legend-Location="Bottom" EnableViewState="False">
            <DeploymentScenario ImageURL="GDD.png" FilePath="~"></DeploymentScenario>
            <Tooltips Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"></Tooltips>
            <PieChart3D>
                <Labels Font="Verdana, 7pt" />
            </PieChart3D>
            <Legend Visible="True" Location="Bottom" SpanPercentage="15" BackgroundColor="White" BorderStyle="Solid" BorderThickness="0" BorderColor="White">
            </Legend>
        </igchart:UltraChart> 
    </td>
    </tr>
    <tr>
        <td>
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="Button1" runat="server" Text="Actualizar gráfico" OnClick="Button1_Click" CssClass="bt_bg" /> 
                        </td>
                        <td valign="middle">
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
                                <ProgressTemplate>
                                    &nbsp;&nbsp;<img src="App_Images/ajax-loader.gif" alt="" /><span class="texto"><asp:Label ID="lblProcess" runat="server" meta:resourcekey="lblProcess"></asp:Label></span>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </td>
                    </tr>
                </table>
        </td>  
    </tr>
</table>  
</ContentTemplate>
</asp:UpdatePanel>

<table>
    <tr>
        <td>
            &nbsp;&nbsp;<asp:LinkButton ID="Button2" runat="server" Text="Descargar listado de pendientes"  OnClick="Button2_Click" CssClass="link" /> 
        </td>
    </tr>
</table>



<br />
</asp:Content>

    