using System;
using System.Data.SqlClient;
using Data;
using User;
using System.Web;
using System.Data;

public partial class _Login : System.Web.UI.Page
{

  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string username = UserHelper.GetUserId(Request.LogonUserIdentity.Name);
			txtlogin.Focus( );
        }
    }

	protected void btnlogeo_click( object sender , EventArgs e )
	{
		if( Pregunta1.SelectedValue.ToString( ) == "1" && Pregunta2.SelectedValue.ToString( ) == "1" && Pregunta3.SelectedValue.ToString( ) == "1" )
		{
			HttpContext.Current.Session["UserLogged"] = Session["UserImpersonate"];
			Response.Redirect( "Default.aspx" );
		}
		else
		{
			Error.Visible = true;
			Error.Text = "Los datos ingresados son inválidos, por favor cambie la selección y valide nuevamente!";
		}
	}

	protected void btnVerPreguntas_click( object sender , EventArgs e )
	{

		txtlogin.Text = txtlogin.Text.PadLeft( 8 , '0' );

		Pregunta1.DataTextField = "NRO_DOCUMENTO";
		Pregunta1.DataValueField = "Respuesta";
		Pregunta1.DataSource = SQLHelper.ExecuteDataset( Cache["ApplicationDatabase"].ToString( ) , "GetQuestion1" , new object[] { txtlogin.Text } );
		Pregunta1.DataBind( );


		if( Pregunta1.Items.Count > 2 )
		{

			txtlogin.Enabled = false;
			TablaPreguntas.Visible = true;
			Error.Visible = false;

			Session["UserImpersonate"] = "AR" + txtlogin.Text;

			Pregunta2.DataTextField = "CALLE";
			Pregunta2.DataValueField = "Respuesta";
			Pregunta2.DataSource = SQLHelper.ExecuteDataset( Cache["ApplicationDatabase"].ToString( ) , "GetQuestion2" , new object[] { txtlogin.Text } );
			Pregunta2.DataBind( );


			Pregunta3.DataTextField = "LOCALIDAD";
			Pregunta3.DataValueField = "Respuesta";
			Pregunta3.DataSource = SQLHelper.ExecuteDataset( Cache["ApplicationDatabase"].ToString( ) , "GetQuestion3" , new object[] { txtlogin.Text } );
			Pregunta3.DataBind( );
		}
		else
		{
			Error.Visible = true;
			Error.Text = "No existe el legajo, por favor ingrese un legajo válido!";
		}
	}

	protected void btnVolverImpersonar_click( object sender , EventArgs e )
	{
		Error.Visible = false;
		TablaEncabezado.Visible = true;
		txtlogin.Text = string.Empty;
		txtlogin.Enabled = true;
		txtlogin.Focus( );
		TablaPreguntas.Visible = false;
	}

}
