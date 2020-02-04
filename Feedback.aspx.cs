using System;
using System.Data.SqlClient;
using Data;
using User;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

public partial class _Default : System.Web.UI.Page
{
    clsDAO objDAO = new clsDAO();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string username = UserHelper.GetUserId(Request.LogonUserIdentity.Name);
            DataTable dt = objDAO.SqlCall("[GetEvaluatedFeedbackByEvaluator] '" + username + "'");

            foreach (DataRow dr in dt.Rows)
            {
                if (dr[2].ToString().Equals("False"))
                {
                    ListItem lst = new ListItem(dr[1].ToString(), dr[0].ToString());
                    chkFeedback.Items.Add(lst);
                }

                if (dr[2].ToString().Equals("True"))
                {
                    ListItem lst = new ListItem(dr[1].ToString(), dr[0].ToString());
                    lbxFeedback.Items.Add(lst);
                }
            }

            if (chkFeedback.Items.Count > 0)
            {
                chkFeedback.Visible = true;
                btnFeedback.Visible = true;
            }
            else
            {
                chkFeedback.Visible = false;
                btnFeedback.Visible = false;
            }
        }
    }

    protected void btnFeedback_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in chkFeedback.Items)
        {
            if (li.Selected)
            {
                objDAO.SqlExec("[SaveFeedbackByEvaluated] '" + li.Value + "'");
            }
        }

        Response.Redirect("~/Feedback.aspx");
    }

    protected void btnFeedback0_Click(object sender, EventArgs e)
    {
        if (lbxFeedback.SelectedIndex > -1)
            objDAO.SqlExec("[RemoveFeedbackByEvaluated] '" + lbxFeedback.SelectedItem.Value + "'");
    }
}
