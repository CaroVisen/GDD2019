using System.Web;


namespace User
{
    public sealed class UserHelper
    {
        private UserHelper() { }

        public static bool IsAdmin(string _User)
        {
            return System.Configuration.ConfigurationManager.AppSettings["Admins"].ToString().Contains(UserHelper.GetUserId(_User).ToString());
        }

        public static bool IsAdminUO(string _User)
        {
            return System.Configuration.ConfigurationManager.AppSettings["AdminsUO"].ToString().Contains(UserHelper.GetUserId(_User).ToString());
        }

        public static bool IsVisionGlobal(string _User)
        {
            return System.Configuration.ConfigurationManager.AppSettings["VisionGlobal"].ToString().Contains(UserHelper.GetUserId(_User).ToString());
        }

        public static bool IsVisionGlobalTotal(string _User)
        {
            return System.Configuration.ConfigurationManager.AppSettings["VisionGlobalTotal"].ToString().Contains(UserHelper.GetUserId(_User).ToString());
        }

        public static bool IsViewReport(string _User)
        {
            return System.Configuration.ConfigurationManager.AppSettings["ViewReport"].ToString().Contains(UserHelper.GetUserId(_User).ToString());
        }

        public static string GetUserId(string _User)
        {
            string strUser = HttpContext.Current.User.Identity.Name.ToString();
            if (!strUser.Equals(_User))
            {
                strUser = _User;
                strUser = "AR03039393";
            }
#if DEBUG
            strUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
#endif
            strUser = "AR03039393";
            if (System.Configuration.ConfigurationManager.AppSettings["Test"].ToString().Equals("1") &&
            System.Configuration.ConfigurationManager.AppSettings["TestDeveloper"].ToString().Equals(strUser.ToUpper().Replace("SA\\", "")) && HttpContext.Current.Session["UserImpersonate"] == null)
            {
                HttpContext.Current.Session["UserImpersonate"] = null;
                HttpContext.Current.Session["UserLogged"] = null;
                HttpContext.Current.Session["UserLogged"] = System.Configuration.ConfigurationManager.AppSettings["TestUser"].ToString();
                return HttpContext.Current.Session["UserLogged"].ToString();
            }
            else if (HttpContext.Current.Session["UserImpersonate"] == null)
            {
                HttpContext.Current.Session["UserImpersonate"] = null;
                HttpContext.Current.Session["UserLogged"] = null;
                HttpContext.Current.Session["UserLogged"] = strUser.ToUpper().Replace("SA\\", "");
                return HttpContext.Current.Session["UserLogged"].ToString();
            }
            else
            {
                //HttpContext.Current.Session["UserLogged"] = HttpContext.Current.Session["UserImpersonate"];
                return HttpContext.Current.Session["UserLogged"].ToString();
            }
        }
    }
}

