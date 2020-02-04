using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Data;
using System.Web.UI.WebControls;
using Kofba.Framework.UI;
using Infragistics.WebUI.UltraWebGrid;
using Kofba.Framework.UI.UltraWebGrid;
using System.Data;
using User;

public partial class VLocal : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

		if( !Page.IsPostBack )
		{

			string filtro_local = string.Empty;

			if( UserHelper.IsAdmin(Request.LogonUserIdentity.Name) && !UserHelper.IsAdminUO(Request.LogonUserIdentity.Name) )
			{
				filtro_local = "%";
			}
			else
			{
				DataSet Ds = SQLHelper.ExecuteDataset( HttpContext.Current.Cache["ApplicationDatabase"].ToString( ) , "GetLocalsByRRHHManager" , new object[] { UserHelper.GetUserId(Request.LogonUserIdentity.Name) } );
				filtro_local = Ds.Tables[0].Rows[0].ItemArray[0].ToString( );
			}

			string filtro_direccion = string.Empty;

			if( UserHelper.IsVisionGlobal(Request.LogonUserIdentity.Name) )
			{
				if( filtro_local == string.Empty )
					filtro_local = "%";
				DataSet Ds = SQLHelper.ExecuteDataset( HttpContext.Current.Cache["ApplicationDatabase"].ToString( ) , "GetDireccionById" , new object[] { UserHelper.GetUserId(Request.LogonUserIdentity.Name) } );
				filtro_direccion = Ds.Tables[0].Rows[0].ItemArray[0].ToString( );
			}

			if( UserHelper.IsVisionGlobalTotal(Request.LogonUserIdentity.Name) )
			{
				if( filtro_local == string.Empty )
					filtro_local = "%";
				filtro_direccion = "%";
			}
			

            if( filtro_direccion == string.Empty )
				filtro_direccion = "%";

            


			gvwEvaluations.DataSource = SQLHelper.ExecuteDataset( HttpContext.Current.Cache["ApplicationDatabase"].ToString( ) , "GetEvaluationsViews" , new object[] { txtbuscar.Text , filtro_local , filtro_direccion } );
			gvwEvaluations.DataBind( );
			gvwEvaluations.Columns.FromKey( "Local" ).IsGroupByColumn = true;
			gvwEvaluations.Columns.FromKey( "Evaluado" ).Width = Unit.Pixel( 140 );
			//gvwEvaluations.Columns.FromKey( "Evaluador" ).Hidden = true;
			gvwEvaluations.Columns.FromKey( "Legajo Evaluador" ).Hidden = true;
			//gvwEvaluations.Columns.FromKey( "Concurrente" ).Hidden = true;
			gvwEvaluations.Columns.FromKey( "Legajo Concurrente" ).Hidden = true;
			gvwEvaluations.Columns.FromKey( "Doble Reporte" ).Hidden = true;
			gvwEvaluations.Columns.FromKey( "Legajo Doble Reporte" ).Hidden = true;
			gvwEvaluations.Columns.FromKey( "Gerencia" ).Hidden = true;
			gvwEvaluations.Columns.FromKey( "Tipo Variable" ).Hidden = true;
			gvwEvaluations.Columns.FromKey( "Grupo" ).Hidden = true;

		}
		else
		{
			string filtro_local = string.Empty;


			if( UserHelper.IsAdmin(Request.LogonUserIdentity.Name) && !UserHelper.IsAdminUO(Request.LogonUserIdentity.Name) )
			{
				filtro_local = "%";
			}
			else
			{
				DataSet Ds = SQLHelper.ExecuteDataset( HttpContext.Current.Cache["ApplicationDatabase"].ToString( ) , "GetLocalsByRRHHManager" , new object[] { UserHelper.GetUserId(Request.LogonUserIdentity.Name) } );
				filtro_local = Ds.Tables[0].Rows[0].ItemArray[0].ToString( );
			}


			string filtro_direccion = string.Empty;

			if( UserHelper.IsVisionGlobal(Request.LogonUserIdentity.Name) )
			{
				if( filtro_local == string.Empty )
					filtro_local = "%";
				DataSet Ds = SQLHelper.ExecuteDataset( HttpContext.Current.Cache["ApplicationDatabase"].ToString( ) , "GetDireccionById" , new object[] { UserHelper.GetUserId(Request.LogonUserIdentity.Name) } );
				filtro_direccion = Ds.Tables[0].Rows[0].ItemArray[0].ToString( );
			}

			if( UserHelper.IsVisionGlobalTotal(Request.LogonUserIdentity.Name) )
			{
				if( filtro_local == string.Empty )
					filtro_local = "%";
				filtro_direccion = "%";
			}
			if( filtro_direccion == string.Empty )
				filtro_direccion = "%";


			gvwEvaluations.DataSource = SQLHelper.ExecuteDataset( HttpContext.Current.Cache["ApplicationDatabase"].ToString( ) , "GetEvaluationsViews" , new object[] { txtbuscar.Text , filtro_local , filtro_direccion } );
			gvwEvaluations.DataBind( );
			gvwEvaluations.Columns.FromKey( "Local" ).IsGroupByColumn = true;
			gvwEvaluations.Columns.FromKey( "Evaluado" ).Width = Unit.Pixel( 140 );
			//gvwEvaluations.Columns.FromKey( "Evaluador" ).Hidden = true;
			gvwEvaluations.Columns.FromKey( "Legajo Evaluador" ).Hidden = true;
			//gvwEvaluations.Columns.FromKey( "Concurrente" ).Hidden = true;
			gvwEvaluations.Columns.FromKey( "Legajo Concurrente" ).Hidden = true;
			gvwEvaluations.Columns.FromKey( "Doble Reporte" ).Hidden = true;
			gvwEvaluations.Columns.FromKey( "Legajo Doble Reporte" ).Hidden = true;
			gvwEvaluations.Columns.FromKey( "Gerencia" ).Hidden = true;
			gvwEvaluations.Columns.FromKey( "Tipo Variable" ).Hidden = true;
			gvwEvaluations.Columns.FromKey( "Grupo" ).Hidden = true;
		}

    }



	protected void gvwEvaluations_DBlClick( object sender , ClickEventArgs e )
	{


		DataSet Ds = new DataSet( );

		Ds = SQLHelper.ExecuteDataset( Cache["ApplicationDatabase"].ToString( ) , "GetEvaluatorbyEvaluated" , new object[] { e.Row.Cells[e.Row.Cells.IndexOf( "Legajo Evaluado" )].ToString( ) } );
		string evaluador = Ds.Tables[0].Rows[0].ItemArray[0].ToString( );
		string groupid = Ds.Tables[0].Rows[0].ItemArray[1].ToString( );


		if( ( e.Row.Cells.All[0] != null ) )
		{
            Response.Redirect(Server.HtmlEncode("./EvaluationFormLink.aspx?.=" + Server.UrlEncode(Encryption.Encrypt(groupid + ".2." + evaluador))));
		}
	}



	protected void gvwEvaluations_InitializeRow( object sender , Infragistics.WebUI.UltraWebGrid.RowEventArgs e )
	{

		DataSet Ds = new DataSet( );

		Ds = SQLHelper.ExecuteDataset( Cache["ApplicationDatabase"].ToString( ) , "GetEvaluatorbyEvaluated" , new object[] { e.Row.Cells[e.Row.Cells.IndexOf( "Legajo Evaluado" )].ToString( ) } );
		string evaluador = Ds.Tables[0].Rows[0].ItemArray[0].ToString( );
		string groupid = Ds.Tables[0].Rows[0].ItemArray[1].ToString( );
		string url = string.Empty;

		if( ( e.Row.Cells.All[0] != null ) )
		{
            url = Server.HtmlEncode("./EvaluationFormLink.aspx?.=" + Server.UrlEncode(Encryption.Encrypt(groupid + ".2." + evaluador)));
		}

        Ds = SQLHelper.ExecuteDataset( Cache["ApplicationDatabase"].ToString( ) , "GetEvaluationIdByEvaluated" , new object[] { e.Row.Cells[e.Row.Cells.IndexOf( "Legajo Evaluado" )].ToString( ) } );

        if (e.Row.Cells[e.Row.Cells.IndexOf("Estado")].ToString() != "Pendiente")
        {
            e.Row.Cells.FromKey("Link").Text = "Visualización";
            e.Row.Cells.FromKey("Link").TargetURL = url;
        }
        else
        {
            e.Row.Cells.FromKey("Link").Text = "No disponible";
        }

        if (UserHelper.IsViewReport(Request.LogonUserIdentity.Name))
        {
            e.Row.Cells.FromKey("Reporte").Text = "Ver Impresión";
            e.Row.Cells.FromKey("Reporte").TargetURL = Server.HtmlEncode("./PrintReport.aspx?EvaluationId=" + Ds.Tables[0].Rows[0].ItemArray[0].ToString());
        }
        else
        {
            e.Row.Cells.FromKey("Reporte").Text = "No disponible";
        }
    }



	protected void Buscar( object sender , EventArgs e )
	{

	}
}
