using System;
using System.Data.SqlClient;
using Data;
using User;
using System.Web;
using System.Data;

public partial class _Default : System.Web.UI.Page
{

    //protected override void OnPreInit(EventArgs e)
    //{
    //    base.OnPreInit(e);
    //    string ctrlName = Request["__EVENTTARGET"];
    //} 


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string username = UserHelper.GetUserId(Request.LogonUserIdentity.Name);

            SqlDataReader myReader = SQLHelper.ExecuteReader(HttpContext.Current.Cache["ApplicationDatabase"].ToString(), "GetUserAccessControl", new object[] { username });

            if (!myReader.HasRows)
            {
                myReader.Dispose();
                Response.Redirect("Notification.aspx", true);

            }
            else
            {
                DataSet myReaderEnabled = SQLHelper.ExecuteDataset(Cache["ApplicationDatabase"].ToString(), "GetAssessmentAccesibility", new object[] { username });

                int intFecha = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
                string strFecha = System.Configuration.ConfigurationManager.AppSettings["EndDate"];
                bool activo = (intFecha <= Convert.ToInt32(strFecha));
                strFecha = strFecha.Substring(6, 2) + "/" + strFecha.Substring(4, 2) + "/" + strFecha.Substring(0, 4);

                if (myReaderEnabled.Tables[0].Rows[0].ItemArray[0].ToString() == "1" || myReaderEnabled.Tables[0].Rows[0].ItemArray[0].ToString() == "5")
                {
                    if (activo)
                    {
                        Notification.Visible = false;
                    }
                    else
                    {
                        lblMessage.Text = "La fecha límite para completar las evaluaciones fue el " + strFecha + ", no obstante si te han quedado evaluaciones por completar te pedimos las finalices para poder cerrar el proceso. En breve el aplicativo quedará OFF LINE, es decir que no se podrán realizar modificaciones en el sistema.";
                        //lblMessage.Text = "No podrá modificar las evaluaciones debido a que ha expirado el tiempo para completar las mismas. Cabe destacar que la fecha límite para completarlas era la siguiente: " + strFecha + "";
                        Notification.Visible = true;
                    }
                }
                else
                {
                    lblMessage.Text = "Usted no puede modificar las evaluaciones debido a que no cuenta con el perfil correspondiente.";
                    Notification.Visible = true;
                }
            }


        }


    }
}
