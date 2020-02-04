<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ManagementIndicators.ascx.cs" Inherits="UI_ManagementIndicators" %>
        <asp:Table ID="MImsg" Width="100%" BorderColor="#9F0004" BorderStyle="Dotted" BorderWidth="2" CellSpacing="0" CellPadding="5" runat="server"> 
            <asp:TableRow >
                <asp:TableCell CssClass="msg">
                    <span>
                    Por favor, completar los Indicadores de Gestión por cada persona a evaluar.<br />
                    Las calificaciones que surjen de los <i>“Resultados obtenidos”</i> (para este grupo), “<u>NO tienen peso en la Evaluación</u>”, es decir, no influyen en el resultado final de la Gestión del Desempeño.<br />
                    Al completar el porcentaje de cumplimiento de cada indicador, solo utilizar números enteros (sin coma ni punto).
                    </span>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table ID="Tabla_Mensajes_Jefes" Width="100%" BorderColor="#9F0004" BorderStyle="Dotted" BorderWidth="2" CellSpacing="0" CellPadding="5" runat="server"> 
            <asp:TableRow >
                <asp:TableCell CssClass="titulo">
                    <span>
                    <b>AVISO IMPORTANTE:</b><br />
                    Los indicadores de Gestión (Factores Críticos) del nivel "Jefes", ya han sido cargados y evaluados a través  del <br />
                    Sistema TOPS, por tal motivo no serán evaluados en la herramienta Gestión del desempeño.
                    </span>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <br />


