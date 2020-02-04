using System;
using System.Data.SqlClient;
using Data;
using User;
using System.Web;
using System.Data;

public partial class _Logout : System.Web.UI.Page
{



    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HttpContext.Current.Session["UserLogged"] = null;
            HttpContext.Current.Session["UserImpersonate"] = null;
            HttpContext.Current.Session["UserLogged"] = HttpContext.Current.User.Identity.Name.ToString().Replace("SA\\", "").Replace("sa\\", "").ToUpper();
            Response.Redirect("Default.aspx");
        }
    }

}
