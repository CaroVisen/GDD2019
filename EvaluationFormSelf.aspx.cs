using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Data;
using System.Data;
using System.Globalization;
using User;
// ****************** 4 linea modificada del original
using System.Data.SqlClient;


public partial class EvaluationFormSelf : System.Web.UI.Page
{
    public string[] _params = new string[] { };
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            NB_save.Text = "Recordá ir grabando la evaluación a medida que la vas cargando.";
        }

        _params = Encryption.Decrypt(Request.Params["."]).Split('.');
        int profile = Convert.ToInt16(_params[1]);
        pId.Value = profile.ToString();

        SetButtonSend();

        SetButtons(profile);

        DataTable dt = new DataTable();
        dt = new clsDAO().SqlCall("SELECT TOP 1 BlockId FROM ResultCommentSelfEvaluation WHERE NetworkAssessmentId = " + _params[3] + " AND BlockId = 1");
        if (dt.Rows.Count > 0)
        {
            btnSave.Visible = false;
            btnSendSelf.Visible = false;
            btnPrint.Visible = true;
        }
        else
        {
            btnSave.Visible = true;
            btnSendSelf.Visible = true;
            btnPrint.Visible = false;
        }
    }

    private void SetButtons(int profile)
    {
        bool _visible = false;
        if (profile == 1)
        {
            _visible = ("1,2,5,8,11".IndexOf(_params[2].ToString(), 0) >= 0);
            btnSave.Visible = _visible;
            btnSendSelf.Visible = _visible;
            NB_save.Visible = _visible;
        }
    }

    // ******************* 3 linea modificada del original
    private void SetStatus(int current_profile)
    {
        // **************** 2 linea modificada del original
        //string username = UserHelper.GetUserId(Request.LogonUserIdentity.Name);
        //SQLHelper.ExecuteNonQuery(Cache["ApplicationDatabase"].ToString(), "[SetAssessmentsSatus]", new object[] { null, Convert.ToInt16(_params[0]), username, _status });


        string username = UserHelper.GetUserId(Request.LogonUserIdentity.Name);
        //SqlDataReader myReader = SQLHelper.ExecuteReader( Cache["ApplicationDatabase"].ToString( ) , "GetEvaluatedPeople" , new object[] { _params[2] } );
        //myReader.Read( );


        bool exit = false;
        string description = string.Empty;
        string assessment = string.Empty;
        string _letter = string.Empty;
        CultureInfo ci = new CultureInfo("es-AR");
        string separator = ci.NumberFormat.NumberDecimalSeparator;


        _params = Encryption.Decrypt(Request.Params["."]).Split('.');

        int groupId = Convert.ToInt16(_params[0]);
        int profile = Convert.ToInt16(_params[1]);

        t2c.Value = "0";
        Save();
        DataTable dt = new DataTable();
        dt = new clsDAO().SqlCall("GetSelfEvaluationCompletion " + _params[3]);
        if (dt == null || dt.Rows.Count == 0 || !dt.Rows[0][0].ToString().Equals("0"))
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "key", "SetSelfError(1);", true);
        }
        else
        {
            new clsDAO().SqlCall("SendSelfEvaluation " + _params[3]);
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "key", "SetSelfError(0);", true);
        }
    }

    private void SetButtonSend()
    {
        int profile = Convert.ToInt16(_params[1]);
        string lbl = "";
        string css = "bt_sm";
        switch (profile)
        {
            case 0:
                lbl = "Enviar";
                break;
            case 1:
                lbl = "Enviar al Concurrente";
                css = "bt_l";
                break;
            case 2:
                lbl = "Enviar";
                break;
            case 3:
                lbl = "Enviar";
                break;
            case 5:
                lbl = "Enviar";
                break;
        }
        btnPrint.CssClass = css;
        btnSendSelf.Text = lbl;
        btnSendSelf.CssClass = css;
        btnSendSelf.CommandArgument = profile.ToString();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Save();
        Button btn = (Button)sender;
        if (btn.CommandArgument != "")
        {
            SetStatus(Convert.ToInt32(_params[1].ToString()));
        }

        btn.Dispose();
    }

    private void Save()
    {
        SaveSE();
    }

    private void SaveSE()
    {

        {
            bool exit = false;
            string description = string.Empty;
            int _block = 0;
            int _fieldMI = 0;
            object[] valuesMI = new object[10];
            int _fieldPM = 0;
            object[] valuesPM = new object[6];
            int _fieldO = 0;
            int radioObjs = 0;
            string assessment = string.Empty;
            string _letter = string.Empty;
            CultureInfo ci = new CultureInfo("es-AR");
            string separator = ci.NumberFormat.NumberDecimalSeparator;
            for (int i = 0; !exit; i++)
            {
                Control answer = (Control)this.Master.FindControl("CPH").FindControl("SE").FindControl("ctl" + i.ToString("00"));
                exit = answer == null;
                if (!exit)
                {
                    try
                    {
                        if (answer.GetType().ToString() == "System.Web.UI.WebControls.TextBox")
                        {

                            TextBox myControl = (TextBox)answer;
                            //Tareas
                            if (myControl.Attributes["class"] != "tM" && myControl.Attributes["class"] != "tMp" && myControl.Attributes["class"] != "tMo")
                            {
                                if (!myControl.ReadOnly)
                                {
                                    valuesMI[_fieldMI] = myControl.Text;
                                    _fieldMI++;

                                    if (_fieldMI == 10)
                                    {
                                        new clsDAO().SqlExec("DELETE FROM ResultVariableSelfEvaluation WHERE NetworkAssessmentId = " + _params[3].ToString());
                                        for (int mi = 0; mi < valuesMI.Length; mi++)
                                            SQLHelper.ExecuteNonQuery(Cache["ApplicationDatabase"].ToString(), "SaveResultVariableSelfEvaluation", new object[] { _params[3].ToString(), valuesMI[mi] });
                                        Array.Clear(valuesMI, 0, valuesMI.Length);
                                        _fieldMI = 0;
                                    }
                                }

                                myControl.Dispose();
                            }

                            //Comentario
                            if (myControl.Attributes["class"] == "tM")
                            {
                                string[] values = myControl.SkinID.Split('_');
                                if (!myControl.ReadOnly)
                                {
                                    SQLHelper.ExecuteNonQuery(Cache["ApplicationDatabase"].ToString(), "[SaveResultCommentSelfEvaluation]", new object[] { _params[3].ToString(), myControl.Text });

                                }
                                myControl.Dispose();
                            }

                            //Agenda de desarrollo
                            if (myControl.Attributes["class"] == "tMp")
                            {
                                if (!myControl.ReadOnly)
                                {
                                    valuesPM[_fieldPM] = myControl.Text;
                                    _fieldPM++;

                                    if (_fieldPM == 5)
                                    {
                                        SQLHelper.ExecuteNonQuery(Cache["ApplicationDatabase"].ToString(), "[SaveResultImprovementPlanSelfEvaluation]", new object[] { Convert.ToInt16(myControl.SkinID.ToString()), 3, valuesPM[0], valuesPM[1], valuesPM[2], valuesPM[3], valuesPM[4], valuesPM[5] });
                                        Array.Clear(valuesPM, 0, valuesPM.Length);
                                        _fieldPM = 0;
                                    }
                                }
                                myControl.Dispose();
                            }

                            //Objetivos
                            if (myControl.Attributes["class"] == "tMo")
                            {
                                if (!myControl.ReadOnly)
                                {
                                    _fieldO++;
                                    SQLHelper.ExecuteNonQuery(Cache["ApplicationDatabase"].ToString(), "[SaveResultObjetives]", new object[] { _params[3].ToString(), _fieldO, myControl.Text });
                                }
                                myControl.Dispose();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                    try
                    {
                        if (answer.GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
                        {
                            DropDownList myControl = (DropDownList)answer;
                            if (myControl.Attributes["class"] == "tMpc")
                            {
                                if (myControl.Enabled)
                                {
                                    valuesPM[_fieldPM] = myControl.SelectedValue;
                                    _fieldPM++;
                                }
                                myControl.Dispose();
                            }
                            else
                            {
                                _fieldO++;
                                SQLHelper.ExecuteNonQuery(Cache["ApplicationDatabase"].ToString(), "[SaveResultObjetives]", new object[] { _params[3].ToString(), _fieldO, myControl.SelectedValue });
                            }
                        }
                    }
                    catch (Exception ex) { }

                    try
                    {
                        //Competencias
                        if (answer.GetType().ToString() == "System.Web.UI.WebControls.RadioButton")
                        {
                            RadioButton myControl = (RadioButton)answer;

                            if (myControl.Attributes["class"] != "tMom")
                            {
                                string[] values = myControl.SkinID.ToUpper().Split('_');
                                if (myControl.Checked)
                                {
                                    if (values.Length != 1)
                                    {
                                        _block = (Convert.ToInt16(values[1]) <= 5) ? 2 : 3;
                                        SQLHelper.ExecuteNonQuery(Cache["ApplicationDatabase"].ToString(), "SaveResultCompetencySelfEvaluation", new object[] { values[0], _block, values[1], values[2], values[3], true });
                                    }
                                }
                            }
                            else
                            {
                                if (myControl.Checked)
                                {
                                    _fieldO++;
                                    SQLHelper.ExecuteNonQuery(Cache["ApplicationDatabase"].ToString(), "[SaveResultObjetives]", new object[] { _params[3].ToString(), _fieldO, myControl.Text });
                                }
                                else
                                    radioObjs++;

                                if (radioObjs == 3)
                                {
                                    _fieldO++;
                                    SQLHelper.ExecuteNonQuery(Cache["ApplicationDatabase"].ToString(), "[SaveResultObjetives]", new object[] { _params[3].ToString(), _fieldO, "" });
                                }
                            }
                            myControl.Dispose();
                        }
                    }
                    catch (Exception ex) { }

                    answer.Dispose();
                }
            }
        }
    }
}
