using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Notification : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //switch (Request.Params["msg"])
        //{
        //    case "1":
                Label lblAppName = (Label)this.Master.FindControl("lblAppName");
                lblAppName.Text += "<br/><br/>";
                this.Master.FindControl("Menu").Visible = false;
				NotificationBanner1.Text = "Ud. no posee acceso a este aplicativo!";
        //        break;
        //    case "2":
        //        NotificationBanner1.Text = "Ud. no posee evaluaciones pendientes.";
        //        break;
        //    case "3":
        //        NotificationBanner1.Text = "La evaluación se envió exitosamente.";
        //        break;
        //    default:
        //        break;
        //}
    }
}
