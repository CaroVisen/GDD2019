using System.Web.UI;
using System.Web.UI.Adapters;

public class PageStateAdapter : PageAdapter
{
    public override PageStatePersister GetStatePersister()
    {
        return new SessionPageStatePersister(this.Page);
    }

}

