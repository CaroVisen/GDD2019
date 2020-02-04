
using System;
using System.ComponentModel;


public partial class UI_NotificationBanner : System.Web.UI.UserControl
{
    #region Text: string (RW)
    [Bindable(true)]
    [Category("Appearance")]
    [DefaultValue("")]
    [Localizable(true)]
    public string Text
    {
        get
        {
            return lblMessage.Text;
        }
        set
        {
            lblMessage.Text = value;
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
    }
}
