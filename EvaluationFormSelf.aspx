﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="EvaluationFormSelf.aspx.cs" Inherits="EvaluationFormSelf" Culture="auto" meta:resourcekey="Page"
    UICulture="auto" CompilerOptions="/o+" EnableEventValidation="false" EnableSessionState="true"
    EnableTheming="true" EnableViewState="false" EnableViewStateMac="false" ValidateRequest="false"
    ViewStateEncryptionMode="Never" %>

<%@ OutputCache NoStore="true" Duration="1" VaryByParam="none" %>
<%@ Register Src="UI/ManagementIndicators.ascx" TagName="ManagementIndicators" TagPrefix="uc1" %>
<%@ Register Src="UI/NotificationBanner.ascx" TagName="NotificationBanner" TagPrefix="uc2" %>
<%@ Register Src="UI/Competencies.ascx" TagName="Competencies" TagPrefix="uc3" %>
<%@ Register Src="UI/ImprovementPlan.ascx" TagName="ImprovementPlan" TagPrefix="uc4" %>
<%@ Register Src="UI/SelfEvaluation.ascx" TagName="SelfEvaluation" TagPrefix="uc5" %>

<asp:Content ID="Content" ContentPlaceHolderID="CPH" runat="Server">
    <asp:HiddenField ID="pId" runat="server" />
    <asp:HiddenField ID="t1" runat="server" />
    <asp:HiddenField ID="t2" runat="server" />
    <asp:HiddenField ID="t2c" runat="server" />
    <asp:HiddenField ID="t3" runat="server" />
    <asp:HiddenField ID="t4" runat="server" />
    <div class="tab-pane" id="tabPane1" style="width: 100%;">
        <%--<div style="text-align: right;">
            <a href="#b" class="link">Ir a pie de página</a>
        </div>--%>
        <div class="tab-page" id="tabSE">
            <h2 class="tab">Autoevaluación</h2>
            <uc5:SelfEvaluation ID="SE" runat="server" />
        </div>
        <%--<div style="text-align: right;">
            <a href="#t" class="link">Subir</a>
        </div>--%>
    </div>
    <br />
    <a name="b"></a>
    <asp:UpdatePanel ID="UP" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Grabar" CssClass="bt_sm" EnableTheming="False"
                            OnClick="btnSave_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnSendSelf" runat="server" Text="Enviar" EnableTheming="False" OnClick="btnSave_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnPrint" runat="server" Text="Imprimir" EnableTheming="False" OnClientClick="javascript:window.print();" />
                    </td>
                    <td>
                        <input id="btnClose" type="button" value="Salir" class="bt_sm" onclick="javascript: Redirect('Default.aspx');" />
                    </td>
                    <td valign="middle">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
                            <ProgressTemplate>
                                &nbsp;&nbsp;<img src="App_Images/ajax-loader.gif" alt="" /><span class="texto"><asp:Label
                                    ID="lblProcess" Text="&nbsp;Procesando..." runat="server"></asp:Label></span>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
            </table>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc2:NotificationBanner ID="NB_save" runat="server" />
</asp:Content>
