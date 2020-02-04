<%@ Page Language="C#" %>

<script runat="server">

   void button_Click(object sender, EventArgs e)
   {
      label.Text = "Refreshed by server side event handler at " + DateTime.Now +".<br>Value provided:" + text.Text;
   }

</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
   <title>How to update an UpdatePanel with JavaScript</title>
   
   <script type="text/javascript">
   function UpdPanelUpdate(id)
   {
      var obj = document.getElementById("<%= text.ClientID %>");
      obj.value=id;
       __doPostBack("<%= button.ClientID %>","");
   }
   </script>
   
</head>
<body>
   <form id="form1" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server" />
      <div>
      

   <a href="javascript:UpdPanelUpdate('Value passed by javascript')">Update the Panel</a>

    <asp:TextBox ID="text" runat="server" style="display:none;"></asp:TextBox>
    <asp:Button ID="button" runat="server" OnClick="button_Click" style="display:none;"/>
      
         <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
            <ContentTemplate>
               <asp:Label ID="label" runat="server"></asp:Label>
            </ContentTemplate>
            <Triggers>
               <asp:AsyncPostBackTrigger ControlID="button" EventName="Click" />
            </Triggers>
         </asp:UpdatePanel>
      </div>
   </form>
</body>
</html>
