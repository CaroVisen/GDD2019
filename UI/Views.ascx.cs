#region Usings

using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Data;
using System.Collections;
using System.Web.UI.HtmlControls;

#endregion

public partial class UI_Views : System.Web.UI.UserControl
{
    #region Variables
    //string _userID = SecurityHelper.GetCurrentUser().UserId.ToUpper();
    //string _filter = string.Empty;
    //ItemCollection<SurveyFinalReport> surveys = new ItemCollection<SurveyFinalReport>();
    //SecurityUser _user = new SecurityUser();
    #endregion

    #region Properties

    #region Filter : string (RW)
    public string Filter
    {
        get { return filter; }
        set { filter = value; }
    }
    private string filter = string.Empty;

    #endregion
    #region UserID : string (RW)
    public string UserID
    {
        get { return userId; }
        set { userId = value; }
    }

    private string userId;
    #endregion



    #endregion

    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        //LoadData();
    }

    protected void gv_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    GridView gv = (GridView)e.Row.FindControl("GridView2");
            //    SurveyFinalReport item = (SurveyFinalReport)e.Row.DataItem;
            //    ItemCollection<SurveyFinalReport> evaluated = surveys.FilterEqual("LocalUO", item.LocalUO);

            //    ColumnsGridEvaluated(ref gv);

            //    gv.EmptyDataText = StringTranslator.Translate("_Empty_Data_Text");
            //    gv.DataSource = evaluated;
            //    gv.DataBind();
            //}
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    #endregion

    public void LoadData(string storeName, object[] parameters, string[] groupBy)
    {
        LoadData(storeName, parameters, groupBy, "");
    }

    public void LoadData(string storeName, object[] parameters, string[] groupBy, string filter)
    {
        //GridView gv = new GridView();
        //foreach (string col in groupBy)
        //{
        //    TemplateField tf = new TemplateField();
        //    BoundField bf = new BoundField();
        //    bf.DataField = col;
        //    bf.HeaderText = col;
        //    gv.Columns.Add (bf);
        //}

        //SqlDataReader myReader = SQLHelper.ExecuteReader(Cache["ApplicationDatabase"].ToString(), "[" + storeName + "]", parameters);
        ////DataTable table = new DataTable(storeName);
        ////for (int i = 0; i < groupBy.Length; i++)
        ////    table.Columns.Add(new DataColumn(groupBy[i].ToString()));

        //int deep = 0;
        //string[] groups = new string[] {"Gerencia", "Area"};
        //string key = string.Empty;
        //while (myReader.Read())
        //{
        //    group = myReader.GetValue(0).ToString();

        //    if (group != string.Empty && oldGroup != string.Empty && group != oldGroup)
        //    {
        //        AddControl(letter, fullName, details, tblNum);
        //        letter = string.Empty;
        //        tblNum += 1;
        //    }

        //    oldGroup = group;
        //}
        //myReader.Close();
        //AddControl(letter, fullName, details, tblNum);
    }


    private string Key(string[] groups) 
    {
        string key = string.Empty;
        for (int i = 0; i < groups.Length; i++)
        {
            
        }

        return "";
    }

    //private void AddControl(string letter, string fullName, IList<ManagementIndicatorsDetail> details, int tblNum)
    //{
    //    Table tbl = new Table();
    //    tbl.Attributes.Add("width", "100%");
    //    tbl.BorderWidth = 0;
    //    tbl.CellSpacing = 1;
    //    tbl.CellPadding = 0;
    //    tbl.ID = "TBL" + tblNum;
    //    //Build(details, tbl, fullName, letter);
    //    this.Controls.Add(tbl);

    //    details.Clear();
    //    tbl.Dispose();
    //}
    

    void helper_GroupEnd(string groupName, object[] values, GridViewRow row)
    {        
        row.Cells[0].Text = "bfgdfggdfgdfgfdgd";//values[0].ToString();
    }

    void helper_GroupStart(string groupName, object[] values, GridViewRow row)
    {
        row.Cells[0].Text = "<div>";
        //HtmlTableRow tr = new HtmlTableRow();
        ////tr.InnerHtml = "<div>";
        //this.Controls.Add(tr);
        //row.Cells[0].Text = values[0].ToString();
    }

    void helper_GroupSummary(string groupName, object[] values, GridViewRow row)
    {
        row.Cells[0].Text= values[0].ToString();                
    }

    private void GenerateGrid(DataTable dt)
    {
        throw new NotImplementedException();
    }

    private void CreateNewGrid()
    {
        throw new NotImplementedException();
    }


    //private void LoadData()
    //{
    //    try
    //    {
    //        surveys = FilterData();
    //        ItemCollection<SurveyFinalReport> evaluations = GetLocals(surveys);

    //        if (evaluations.Count > 0)
    //            evaluations.Sort(delegate(SurveyFinalReport x, SurveyFinalReport y)
    //                               { return x.LocalUO.CompareTo(y.LocalUO); }
    //                            );

    //        GridView1.EmptyDataText = StringTranslator.Translate(Translator.GetObject("_Empty_Data_Text"));
    //        GridView1.DataSource = evaluations;
    //        GridView1.DataBind();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception(ex.Message);
    //    }
    //}

    //private ItemCollection<SurveyFinalReport> FilterData()
    //{
    //    ItemCollection<SurveyFinalReport> evaluationsFiltered = new ItemCollection<SurveyFinalReport>();

    //    try
    //    {
    //        if ((_user.ProfileId == 1) || (_user.ProfileId == 3))
    //        {
    //            evaluationsFiltered = SurveyFinalReport.GetEvaluations(string.Empty);
    //        }
    //        else
    //        {
    //            DataSet ds;

    //            switch (_user.ProfileId)
    //            {
    //                case 2: //Evaluador 
    //                    evaluationsFiltered = SurveyFinalReport.GetEvaluationsByEvaluator(UserID, string.Empty);
    //                    break;

    //                case 4: //Administrador Parcial (Jefes RRHH)
    //                case 5: //Visión Parcial
    //                case 6: //Visión Global por U.O. 
    //                case 7: //Visión Global por Area Comercial
    //                    ds = SurveyFinalReport.GetHHRR(UserID);

    //                    if (ds.Tables[0].Rows.Count > 0)
    //                    {
    //                        evaluationsFiltered = SurveyFinalReport.GetEvaluationsByLocal(UserID, ds.Tables[0].Rows[0]["PLANTAS_SUPERVISA"].ToString(), string.Empty);
    //                    }
    //                    break;

    //                case 8: //Visión Global por Local_UO
    //                    ds = SurveyFinalReport.GetHHRR(UserID);

    //                    if (ds.Tables[0].Rows.Count > 0)
    //                    {
    //                        evaluationsFiltered = SurveyFinalReport.GetEvaluationsByDireccion(UserID, ds.Tables[0].Rows[0]["PLANTAS_SUPERVISA"].ToString(), string.Empty);
    //                    }
    //                    break;
    //            }
    //        }

    //        return evaluationsFiltered;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception(ex.Message);
    //    }
    //}

    //private ItemCollection<SurveyFinalReport> GetLocals(ItemCollection<SurveyFinalReport> surveys)
    //{
    //    ItemCollection<SurveyFinalReport> localsView = new ItemCollection<SurveyFinalReport>();
    //    ItemCollection<SurveyFinalReport> locals = SurveyFinalReport.GetLocals();
    //    int nLocals = 0;

    //    try
    //    {
    //        foreach (SurveyFinalReport local in locals)
    //        {
    //            nLocals = surveys.FilterEqual("LocalUO", local.LocalUO).Count;

    //            if (nLocals > 0)
    //            {
    //                local.CountEval = nLocals;
    //                localsView.Add(local);
    //            }
    //        }

    //        return localsView.FilterLike("LocalUO", Filter);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception(ex.Message);
    //    }
    //}

    private void ColumnsGridEvaluated(ref GridView gvEvaluated)
    {
        try
        {
            //HyperLinkField linkEvaluated = new HyperLinkField();
            //BoundField colEvaluated = new BoundField();
            //BoundField colResult = new BoundField();
            //BoundField colStatus = new BoundField();
            //string[] NavFields = { "EvaluationUserId", "LinkPage" };

            //gvEvaluated.Columns.Clear();

            //linkEvaluated.DataTextField = "EvaluatedName";
            //linkEvaluated.DataNavigateUrlFormatString = "../{1}?EvaluationId={0}&View=4";
            //linkEvaluated.HeaderText = StringTranslator.Translate("EvaluatedName");
            //linkEvaluated.DataNavigateUrlFields = NavFields;
            //gvEvaluated.Columns.Add(linkEvaluated);
            
            //colEvaluated.DataField = "EvaluatedID";
            //colEvaluated.HeaderText = StringTranslator.Translate("EvaluatedID");
            //gvEvaluated.Columns.Add(colEvaluated);

            //colResult.DataField = "FinalResLetter";
            //colResult.HeaderText = StringTranslator.Translate("FinalResLetter_View");
            //gvEvaluated.Columns.Add(colResult);

            //colStatus.DataField = "EvaluationStatus";
            //colStatus.HeaderText = StringTranslator.Translate("EvaluationStatus");
            //gvEvaluated.Columns.Add(colStatus);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    //#endregion
}
