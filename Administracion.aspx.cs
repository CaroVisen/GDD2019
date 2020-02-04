using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Data;
using System.Data;
using System.Web.UI.WebControls;
using Kofba.Framework.UI;
using Infragistics.WebUI.UltraWebGrid;
using Kofba.Framework.UI.UltraWebGrid;

public partial class Administracion : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            ddlevaluados.Attributes.Add("onKeyPress", "javascript:onEnterpress(event);");
            ddlevaluadores.Attributes.Add("onKeyPress", "javascript:onEnterpress(event);");
            ddlconcurrentes.Attributes.Add("onKeyPress", "javascript:onEnterpress(event);");
            ddldreporte.Attributes.Add("onKeyPress", "javascript:onEnterpress(event);");

            ddlevaluados.DataTextField = "Evaluado";
            ddlevaluados.DataValueField = "Legajo Evaluado";
            ddlevaluados.DataSource = SQLHelper.ExecuteDataset(HttpContext.Current.Cache["ApplicationDatabase"].ToString(), "[GetEvaluatedEvaluations]");
            ddlevaluados.DataBind();


            ddlevaluadores.DataTextField = "Usuario";
            ddlevaluadores.DataValueField = "UsuarioId";
            ddlevaluadores.DataSource = SQLHelper.ExecuteDataset(HttpContext.Current.Cache["ApplicationDatabase"].ToString(), "GetAll");
            ddlevaluadores.DataBind();

            ListItem Item = new ListItem();
            Item.Value = "AR00000000";
            Item.Text = "Ninguno";
            ddlevaluadores.Items.Insert(0, Item);

            ddlconcurrentes.DataTextField = "Usuario";
            ddlconcurrentes.DataValueField = "UsuarioId";
            ddlconcurrentes.DataSource = SQLHelper.ExecuteDataset(HttpContext.Current.Cache["ApplicationDatabase"].ToString(), "GetAll");
            ddlconcurrentes.DataBind();

            Item.Value = "AR00000000";
            Item.Text = "Ninguno";
            ddlconcurrentes.Items.Insert(0, Item);

            ddldreporte.DataTextField = "Usuario";
            ddldreporte.DataValueField = "UsuarioId";
            ddldreporte.DataSource = SQLHelper.ExecuteDataset(HttpContext.Current.Cache["ApplicationDatabase"].ToString(), "GetAll");
            ddldreporte.DataBind();

            Item.Value = "AR00000000";
            Item.Text = "Ninguno";
            ddldreporte.Items.Insert(0, Item);

            DataSet Ds = SQLHelper.ExecuteDataset(HttpContext.Current.Cache["ApplicationDatabase"].ToString(), "GetEvaluationByEvaluated", new object[] { ddlevaluados.SelectedValue.ToString() });

            ddlevaluadores.SelectedIndex = ddlevaluadores.Items.IndexOf(ddlevaluadores.Items.FindByValue(Ds.Tables[0].Rows[0].ItemArray[3].ToString()));

            ddlconcurrentes.SelectedIndex = ddlconcurrentes.Items.IndexOf(ddlconcurrentes.Items.FindByValue(Ds.Tables[0].Rows[0].ItemArray[5].ToString()));

            ddldreporte.SelectedIndex = ddldreporte.Items.IndexOf(ddldreporte.Items.FindByValue(Ds.Tables[0].Rows[0].ItemArray[7].ToString()));

        }

    }


    protected void ddlevaluados_SelectedIndexChanged(object sender, EventArgs e)
    {


        DataSet Ds = SQLHelper.ExecuteDataset(HttpContext.Current.Cache["ApplicationDatabase"].ToString(), "GetEvaluationByEvaluated", new object[] { ddlevaluados.SelectedValue.ToString() });

        ddlevaluadores.SelectedIndex = ddlevaluadores.Items.IndexOf(ddlevaluadores.Items.FindByValue(Ds.Tables[0].Rows[0].ItemArray[3].ToString()));

        ddlconcurrentes.SelectedIndex = ddlconcurrentes.Items.IndexOf(ddlconcurrentes.Items.FindByValue(Ds.Tables[0].Rows[0].ItemArray[5].ToString()));

        ddldreporte.SelectedIndex = ddldreporte.Items.IndexOf(ddldreporte.Items.FindByValue(Ds.Tables[0].Rows[0].ItemArray[7].ToString()));

        Boton_Reimprimir.Enabled = Ds.Tables[0].Rows[0].ItemArray[10].ToString() == "Cerrada";

    }


    protected void ddlevaluadores_SelectedIndexChanged(object sender, EventArgs e)
    {



        ListItem concurrentselected = new ListItem();

        DataSet Ds = SQLHelper.ExecuteDataset(HttpContext.Current.Cache["ApplicationDatabase"].ToString(), "GetEvaluatorConcurrent", new object[] { ddlevaluadores.SelectedValue.ToString() });

        if (Ds.Tables[0].Rows.Count > 0)
        {
            if (Ds.Tables[0].Rows[0].ItemArray[0] != null)
            {
                concurrentselected.Value = Ds.Tables[0].Rows[0].ItemArray[0].ToString();
                concurrentselected.Text = Ds.Tables[0].Rows[0].ItemArray[1].ToString();
                ddlconcurrentes.SelectedIndex = ddlconcurrentes.Items.IndexOf(concurrentselected);
            }
        }

    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        string concurrente = string.Empty;
        if (ddlconcurrentes.SelectedValue.ToString() == "AR00000000")
            concurrente = null;
        else
            concurrente = ddlconcurrentes.SelectedValue.ToString();

        if (ddldreporte.SelectedValue.ToString() == "AR00000000")
            SQLHelper.ExecuteNonQuery(HttpContext.Current.Cache["ApplicationDatabase"].ToString(), "UpdateEvaluation", new object[] { ddlevaluados.SelectedValue.ToString(), ddlevaluadores.SelectedValue.ToString(), concurrente, null });
        else
            SQLHelper.ExecuteNonQuery(HttpContext.Current.Cache["ApplicationDatabase"].ToString(), "UpdateEvaluation", new object[] { ddlevaluados.SelectedValue.ToString(), ddlevaluadores.SelectedValue.ToString(), concurrente, ddldreporte.SelectedValue.ToString() });
        string errorMessage = "alert('¡Los cambios se guardaron correctamente!')";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", errorMessage, true);

    }
    protected void Button2_Click(object sender, EventArgs e)
    {

        SQLHelper.ExecuteNonQuery(HttpContext.Current.Cache["ApplicationDatabase"].ToString(), "ResetByEvaluated", new object[] { ddlevaluados.SelectedValue.ToString() });
        string errorMessage = "alert('¡La evaluación de " + ddlevaluados.SelectedItem.Text + " fue reiniciada con éxito!')";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", errorMessage, true);

    }


    protected void Reimprimir_Evaluado(object sender, EventArgs e)
    {

        SQLHelper.ExecuteNonQuery(HttpContext.Current.Cache["ApplicationDatabase"].ToString(), "RePrintByNetworkAssessmentId", new object[] { ddlevaluados.SelectedValue.ToString() });
        string errorMessage = "alert('¡La evaluación de " + ddlevaluados.SelectedItem.Text + " volvió a estar pendiente de impresión!')";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", errorMessage, true);
    }


    protected void Eliminar_Evaluado(object sender, EventArgs e)
    {
        SQLHelper.ExecuteNonQuery(HttpContext.Current.Cache["ApplicationDatabase"].ToString(), "DeleteByEvaluated", new object[] { ddlevaluados.SelectedValue.ToString() });
        string errorMessage = "alert('¡La evaluación de " + ddlevaluados.SelectedItem.Text + " fue eliminada con éxito!')";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", errorMessage, true);

        ddlevaluados.DataTextField = "Evaluado";
        ddlevaluados.DataValueField = "Legajo Evaluado";
        ddlevaluados.DataSource = SQLHelper.ExecuteDataset(HttpContext.Current.Cache["ApplicationDatabase"].ToString(), "[GetEvaluatedEvaluations]");
        ddlevaluados.DataBind();

        ddlevaluadores.DataTextField = "Usuario";
        ddlevaluadores.DataValueField = "UsuarioId";
        ddlevaluadores.DataSource = SQLHelper.ExecuteDataset(HttpContext.Current.Cache["ApplicationDatabase"].ToString(), "GetAll");
        ddlevaluadores.DataBind();

        ddlconcurrentes.DataTextField = "Usuario";
        ddlconcurrentes.DataValueField = "UsuarioId";
        ddlconcurrentes.DataSource = SQLHelper.ExecuteDataset(HttpContext.Current.Cache["ApplicationDatabase"].ToString(), "GetAll");
        ddlconcurrentes.DataBind();

        ddldreporte.DataTextField = "Usuario";
        ddldreporte.DataValueField = "UsuarioId";
        ddldreporte.DataSource = SQLHelper.ExecuteDataset(HttpContext.Current.Cache["ApplicationDatabase"].ToString(), "GetAll");
        ddldreporte.DataBind();
        ListItem Item = new ListItem();
        Item.Value = "AR00000000";
        Item.Text = "Ninguno";
        ddldreporte.Items.Insert(0, Item);

        DataSet Ds = SQLHelper.ExecuteDataset(HttpContext.Current.Cache["ApplicationDatabase"].ToString(), "GetEvaluationByEvaluated", new object[] { ddlevaluados.SelectedValue.ToString() });

        ddlevaluadores.SelectedIndex = ddlevaluadores.Items.IndexOf(ddlevaluadores.Items.FindByValue(Ds.Tables[0].Rows[0].ItemArray[3].ToString()));

        ddlconcurrentes.SelectedIndex = ddlconcurrentes.Items.IndexOf(ddlconcurrentes.Items.FindByValue(Ds.Tables[0].Rows[0].ItemArray[5].ToString()));

        ddldreporte.SelectedIndex = ddldreporte.Items.IndexOf(ddldreporte.Items.FindByValue(Ds.Tables[0].Rows[0].ItemArray[7].ToString()));

    }

    protected void Button4_Click(object sender, EventArgs e)
    {
        SQLHelper.ExecuteNonQuery(HttpContext.Current.Cache["ApplicationDatabase"].ToString(), "ResetSelfEvaluation", new object[] { ddlevaluados.SelectedValue.ToString() });
        string errorMessage = "alert('¡La autoevaluación de " + ddlevaluados.SelectedItem.Text + " fue reiniciada con éxito!')";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", errorMessage, true);
    }
}
