using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;

public interface IManagementIndicators
{
    void Build(IList<ManagementIndicatorsDetail> details, Table table, string fullName, string result);
}
