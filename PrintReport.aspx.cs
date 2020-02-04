using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kofba.Framework.Utils;
using User;
using System.Configuration;

public partial class PrintReport : System.Web.UI.Page
{
    PrintForm Pf = new PrintForm();

    protected void Page_Load(object sender, EventArgs e)
    {
        clsDAO objDAO = new clsDAO();

        int _Status = Convert.ToInt32(objDAO.SqlCall("GetStatusPrint '" + Request.QueryString["EvaluationId"] + "'").Rows[0][0].ToString());
        string _Evaluated = objDAO.SqlCall("GetEvaluatedByEvaluated '" + Request.QueryString["EvaluationId"].ToString() + "'").Rows[0][0].ToString();
        string _Evaluator = objDAO.SqlCall("GetEvaluatorbyEvaluated '" + _Evaluated + "'").Rows[0][0].ToString();

        if ((ConfigurationManager.AppSettings["Printers"].Contains(UserHelper.GetUserId(Request.LogonUserIdentity.Name))) || ((_Status == 10 || _Status == 12) && UserHelper.GetUserId(Request.LogonUserIdentity.Name).Replace("SA\\", "").Equals(_Evaluator)))
        {
            try
            {
                //string parameter = Request.Params["__EVENTARGUMENT"];

                string parameter = Request.QueryString["EvaluationId"];

                TemplateProcessor tp = new TemplateProcessor();

                Pf.SetData(Convert.ToInt32(parameter));

                tp.Filename = Server.MapPath("PrintPage.htm");
                tp.References = new object[] { Pf };

                Response.Clear();
                Response.Write(tp.Parse());
                Response.End();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        else if ((UserHelper.GetUserId(Request.LogonUserIdentity.Name).Replace("SA\\", "").Equals(_Evaluated)) && (_Status == 10 || _Status == 12))
        {
            try
            {
                //string parameter = Request.Params["__EVENTARGUMENT"];

                string parameter = Request.QueryString["EvaluationId"];

                TemplateProcessor tp = new TemplateProcessor();

                Pf.SetData(Convert.ToInt32(parameter));

                tp.Filename = Server.MapPath("PrintPageView.htm");
                tp.References = new object[] { Pf };

                Response.Clear();
                Response.Write(tp.Parse());
                Response.End();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        else
        {
            RegisterStartupScript("startupScript", "<script language=JavaScript type=\"text/javascript\">alert('Usted no posee permisos para imprimir');window.location.replace(\"Default.aspx\");</script>");
        }
    }

}
