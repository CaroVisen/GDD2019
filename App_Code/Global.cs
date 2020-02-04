using System;
using System.IO;
using System.IO.Compression;
using System.Web;
using System.ComponentModel;
using System.Web.UI;

namespace AccessManagement.WebSite
{
	public class Global : HttpApplication
	{
		private IContainer components = null;

		public Global()
		{
			InitializeComponent();
		}	
		
		protected void Application_Start(Object sender, EventArgs e)
		{
           HttpRuntime.Cache["ApplicationDatabase"] = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationDatabase"].ConnectionString;
        }
	    
		protected void Session_Start(Object sender, EventArgs e)
        {
            HttpRuntime.Cache["UserName"] = "AR00025078";// "AR01317236";//"AR01016114";//User.Identity.Name.ToString().Replace("SA\\", "").Replace("sa\\", "").ToUpper();//"AR00023636";
            HttpRuntime.Cache["Profile"] = System.Configuration.ConfigurationManager.AppSettings["Admins"].ToString();
			HttpRuntime.Cache["VisionGlobal"] = System.Configuration.ConfigurationManager.AppSettings["VisionGlobal"].ToString( );
			HttpRuntime.Cache["VisionGlobalTotal"] = System.Configuration.ConfigurationManager.AppSettings["VisionGlobalTotal"].ToString( );

        }
	    
		protected void Application_BeginRequest(Object sender, EventArgs e){}
	    
		protected void Application_EndRequest(Object sender, EventArgs e){}
	    
	    protected void Application_AuthenticateRequest(Object sender, EventArgs e){}
	    
		protected void Application_Error(Object sender, EventArgs e) {}
	    
		protected void Session_End(Object sender, EventArgs e) 
        {
            HttpRuntime.Cache.Remove("UserName");
        }

		protected void Application_End(Object sender, EventArgs e) 
        {
            HttpRuntime.Cache.Remove("ApplicationDatabase");
        }

		private void InitializeComponent()
		{    
			this.components = new System.ComponentModel.Container();
		}

        void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            string acceptEncoding = app.Request.Headers["Accept-Encoding"];
            Stream prevUncompressedStream = app.Response.Filter;

            if (!(app.Context.CurrentHandler is Page) ||
                app.Request["HTTP_X_MICROSOFTAJAX"] != null)
                return;

            if (acceptEncoding == null || acceptEncoding.Length == 0)
                return;

            acceptEncoding = acceptEncoding.ToLower();

            if (acceptEncoding.Contains("deflate") || acceptEncoding == "*")
            {
                // defalte
                app.Response.Filter = new DeflateStream(prevUncompressedStream,
                    CompressionMode.Compress);
                app.Response.AppendHeader("Content-Encoding", "deflate");
            }
            else if (acceptEncoding.Contains("gzip"))
            {
                // gzip
                app.Response.Filter = new GZipStream(prevUncompressedStream,
                    CompressionMode.Compress);
                app.Response.AppendHeader("Content-Encoding", "gzip");
            }
        }

	}
}
