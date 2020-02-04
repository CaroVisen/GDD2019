using System;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Data;
using User;
using System.Data;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Menu_Load();
    }

    public string UserLogged()
    {
        return lblUserLogged.Text;
    }

    void Menu_Load()
    {
        XmlDataSource dsMenu = new XmlDataSource();

        DataSet myReaderEnabled = SQLHelper.ExecuteDataset(Cache["ApplicationDatabase"].ToString(), "GetUserLogged", new object[] { UserHelper.GetUserId(Request.LogonUserIdentity.Name) });
        if (myReaderEnabled.Tables[0].Rows.Count > 0)
        {
            lblUserLogged.Text = myReaderEnabled.Tables[0].Rows[0].ItemArray[0].ToString();
        }
        else if (Session["UserLogged"] != null && !Session["UserLogged"].Equals(""))
        {
            lblUserLogged.Text = Session["UserLogged"].ToString();
        }
        else
        {
            lblUserLogged.Text = "";
        }

        if (Cache["Profile"].ToString().IndexOf(UserHelper.GetUserId(Request.LogonUserIdentity.Name)) > 0)
        {
            if (UserHelper.IsAdminUO(Request.LogonUserIdentity.Name))
                dsMenu.XPath = @"/*/*[@profile='Admin' or @profile='VisionGlobal' or @profile='Everyone' ]";
            else
                dsMenu.XPath = @"/*/*[@profile='Admin' or @profile='VisionGlobal' or @profile='Everyone'  or @profile='AdminCentral']";
        }
        else if (Cache["VisionGlobal"].ToString().IndexOf(UserHelper.GetUserId(Request.LogonUserIdentity.Name)) > 0 || Cache["VisionGlobalTotal"].ToString().IndexOf(UserHelper.GetUserId(Request.LogonUserIdentity.Name)) > 0)
            dsMenu.XPath = @"/*/*[@profile='VisionGlobal' or @profile='Everyone']";
        else
            dsMenu.XPath = @"/*/*[@profile='Everyone']";

        if (myReaderEnabled.Tables[0].Rows.Count > 0 && !myReaderEnabled.Tables[0].Rows[0].ItemArray[1].ToString().Equals(""))
        {
            dsMenu.XPath = dsMenu.XPath.Replace("]", " or @profile='Self']");
        }

        dsMenu.DataFile = "~/App_Data/Menu.xml";
        Menu.DataSource = dsMenu;
        Menu.DataBind();
        dsMenu.Dispose();
    }
}
