using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Reflection;

/// <summary>
/// Detalle de una evaluación
/// </summary>

public class ManagementIndicatorsDetail 
{    
    private string fullName;
    private string description; 
    private decimal weight;
    private string unit;
    private string _result;
    private string group;
    private string status;
    private decimal code;
    private decimal networkassessmentid;
    private decimal resultvariableid;
    private string letter;

    #region FullName : String (RW)
    public string FullName
    {
        get { return fullName; }
        set { fullName = value; }
    }
    #endregion

    #region Description : String (RW)
    public string Description
    {
        get { return description; }
        set { description = value; }
    }    
    #endregion

    #region Weight : Decimal (RW)
    public decimal Weight
    {
        get { return weight; }
        set { weight = value; }
    }
    #endregion

    #region Unit : String (RW)
    public string Unit
    {
        get { return unit; }
        set { unit = value; }
    }
    #endregion
    
    #region Result : Decimal (RW)
    public string Result
    {
        get { return _result; }
        set { _result = value; }
    }
    #endregion

    #region Group : String (RW)
    public string Group
    {
        get { return group; }
        set { group = value; }
    }
    #endregion

    #region Status : Bool (RW)
    public string Status
    {
        get { return status; }
        set { status = value; }
    }
    
    #endregion

    #region Code : Decimal (RW)
    public decimal Code
    {
        get { return code; }
        set { code = value; }
    }
    
    #endregion

    #region NetworkAssessmentId : Decimal (RW)
    public decimal NetworkAssessmentId
    {
        get { return networkassessmentid; }
        set { networkassessmentid = value; }
    }
    #endregion

    #region ResultVariableId : Decimal (RW)
    public decimal ResultVariableId
    {
        get { return resultvariableid; }
        set { resultvariableid = value; }
    }
    #endregion

    #region Letter : string (RW)
    public string Letter
    {
        get { return letter; }
        set { letter = value; }
    }    
    #endregion

    #region Properties : IList (R)
    public IList Properties
    {
        get 
        {
            IList lst = new ArrayList(); 
            lst.Add(FullName);
            lst.Add(Description);
            lst.Add(Weight);
            lst.Add(Unit);
            lst.Add(Result);
            lst.Add(Group); 
            lst.Add(Status); 
            lst.Add(Code);
            lst.Add(NetworkAssessmentId);
            lst.Add(Letter);
            lst.Add(ResultVariableId);


            return lst;
        }
    }
    #endregion

}
