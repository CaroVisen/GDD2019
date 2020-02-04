using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using User;
using System.Data.SqlClient;
using System.Data;
using Data;
using System.Collections;

public partial class UI_SelfEvaluation : System.Web.UI.UserControl
{
    bool setCompetency = false;
    ArrayList fields = new ArrayList();
    ArrayList fieldsValues = new ArrayList();
    const int cellWidth = 533;
    bool setColor = true;
    bool blnSent = false;
    int numBlock = 0;
    string[] _params = new string[] { };
    int tblID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        /*if (UserHelper.IsAdmin(Request.LogonUserIdentity.Name))
            LoadCompetenciesAdmin();
        else*/
        LoadManagementIndicators();
        LoadCompetencies();
        LoadImprovementPlan();
    }

    private Boolean CheckSentStatus(String id)
    {
        DataTable dt = new DataTable();
        dt = new clsDAO().SqlCall("SELECT TOP 1 BlockId FROM ResultCommentSelfEvaluation WHERE NetworkAssessmentId = " + id + " AND BlockId = 1");
        if (dt.Rows.Count > 0)
            return true;
        else
            return false;
    }

    private void LoadManagementIndicators()
    {
        string username = UserHelper.GetUserId(Request.LogonUserIdentity.Name);
        string oldGroup = string.Empty;
        string group = string.Empty;
        string letter = string.Empty;
        string fullName = string.Empty;
        decimal id = 0;
        int tblNum = 0;

        _params = Encryption.Decrypt(Request.Params["."]).Split('.');

        int groupId = Convert.ToInt16(_params[0]);
        int profile = Convert.ToInt16(_params[1]);

        if (profile > 1)
        {
            username = _params[2];
        }

        string filtro = "%";

        if (_params.Length > 3)
            filtro = SQLHelper.ExecuteDataset(Cache["ApplicationDatabase"].ToString(), "GetEvaluatedByEvaluated", new object[] { _params[3] }).Tables[0].Rows[0].ItemArray[0].ToString();

        SqlDataReader myReader = SQLHelper.ExecuteReader(Cache["ApplicationDatabase"].ToString(), "[GetManagementIndicatorsSelfEvaluation]", new object[] { groupId, username, profile, filtro });
        IList<ManagementIndicatorsDetail> details = new List<ManagementIndicatorsDetail>();
        //profile = 0;
        while (myReader.Read())
        {
            group = myReader.GetValue(0).ToString();
            ManagementIndicatorsDetail d = new ManagementIndicatorsDetail();

            if (group != string.Empty && oldGroup != string.Empty && group != oldGroup)
            {
                if (profile == 0)
                    for (int i = 0; i < 10 - details.Count; i++)
                    {
                        d = new ManagementIndicatorsDetail();
                        d.FullName = fullName;
                        d.Description = "";
                        d.Group = oldGroup;
                        d.NetworkAssessmentId = id;
                        d.Status = "true";
                        d.Code = 0;

                        details.Add(d);
                    }

                AddControlMI(letter, fullName, details, tblNum);
                letter = string.Empty;
                tblNum += 1;
            }


            d.FullName = myReader[0].ToString();
            d.Description = myReader[1].ToString();
            d.Group = myReader[2].ToString();
            d.NetworkAssessmentId = (decimal)myReader[3];
            d.Status = (profile == 0 ? "true" : "false");
            d.Code = 0;

            details.Add(d);

            fullName = d.FullName;
            oldGroup = group;
            id = d.NetworkAssessmentId;
        }

        if (profile == 0)
            for (int i = details.Count; i < 10; i++)
            {
                ManagementIndicatorsDetail d = new ManagementIndicatorsDetail();
                d = new ManagementIndicatorsDetail();
                d.FullName = fullName;
                d.Description = "";
                d.Group = oldGroup;
                d.NetworkAssessmentId = id;
                d.Status = "true";
                d.Code = 0;

                details.Add(d);
            }

        myReader.Close();
        AddControlMI(letter, fullName, details, tblNum);
    }

    private void AddControlMI(string letter, string fullName, IList<ManagementIndicatorsDetail> details, int tblNum)
    {
        Table tbl = new Table();
        tbl.Attributes.Add("width", "100%");
        tbl.BorderWidth = 0;
        tbl.CellSpacing = 1;
        tbl.CellPadding = 0;
        tbl.ID = "TBL" + tblNum;
        BuildMI(details, tbl, fullName, letter);
        this.Controls.Add(tbl);

        details.Clear();
        tbl.Dispose();
    }

    public void BuildMI(IList<ManagementIndicatorsDetail> details, Table tabla, string fullName, string letter)
    {
        //cabecera
        TableRow tblRowEvaluated = new TableRow();
        TableCell tblCellEvaluated = new TableCell();
        TableRow tblRowHeader = new TableRow();
        TableCell tblCell1 = new TableCell();
        //TableCell tblCell2 = new TableCell();
        //TableCell tblCell3 = new TableCell();
        //TableCell tblCell4 = new TableCell();
        tblCellEvaluated.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(fullName.Trim().ToLower());
        tblCellEvaluated.ColumnSpan = 4;
        tblCellEvaluated.Attributes.Add("class", "td01");
        tblRowEvaluated.Cells.Add(tblCellEvaluated);
        tabla.Rows.Add(tblRowEvaluated);
        tblCellEvaluated.Dispose();
        tblRowEvaluated.Dispose();

        tblRowHeader.Attributes.Add("class", "td03");

        tblCell1.Attributes.Add("width", "68%");
        tblCell1.Text = "Descripción de principales tareas";
        //tblCell2.Text = "Ponderación 100%";
        //tblCell3.Text = "Unidad de Medida";
        //tblCell4.Text = "Resultado Logrado %";

        tblRowHeader.Cells.AddRange(new TableCell[] { tblCell1 });
        tabla.Rows.Add(tblRowHeader);

        tblCell1.Dispose();
        //tblCell2.Dispose();
        //tblCell3.Dispose();
        //tblCell4.Dispose();
        tblRowHeader.Dispose();
        //fin cabecera

        //Filas comienzo  
        int rowId = 0;
        foreach (ManagementIndicatorsDetail d in details)
        {
            TableRow tblRow = new TableRow();
            for (int i = 1; i < 2; i++)
            {
                if (!d.Properties[i].ToString().Trim().Equals("") || !CheckSentStatus(d.NetworkAssessmentId.ToString()))
                {
                    bool resultOnly = !(d.Code == 0);
                    TableCell tblCell = new TableCell();
                    if (d.Status == "true" && d.Code == 0 && !CheckSentStatus(d.NetworkAssessmentId.ToString()))
                    {
                        TextBox txtDescription = new TextBox();
                        txtDescription.Text = d.Properties[i].ToString();
                        txtDescription.Attributes["firstLine"] = (d == details[0]).ToString();
                        txtDescription.Attributes["class"] = "text1 d";
                        //txtDescription.Attributes["onkeypress"] = "k(event)";
                        //txtDescription.Attributes["onblur"] = "b(this)";
                        //txtDescription.Attributes["onfocus"] = "f(this)";
                        HttpResponse myHttpReponse = Response;
                        HtmlTextWriter myHtmlTextWriter = new HtmlTextWriter(myHttpReponse.Output);
                        txtDescription.Attributes.AddAttributes(myHtmlTextWriter);
                        tblCell.Attributes.Add("class", "td02r");
                        txtDescription.ID = "";
                        txtDescription.Width = Unit.Percentage(98);
                        txtDescription.Height = 18;
                        txtDescription.SkinID = i.ToString() + "_" + d.NetworkAssessmentId.ToString() + "_" + d.ResultVariableId.ToString();
                        txtDescription.ReadOnly = ((_params[1].ToString().Equals("0") && !CheckSentStatus(d.NetworkAssessmentId.ToString())) ? false : true);
                        tblCell.Controls.Add(txtDescription);
                        txtDescription.Dispose();
                    }
                    else
                    {
                        tblCell.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(d.Properties[i].ToString().Trim().ToLower());
                        tblCell.Attributes.Add("class", "td02");
                    }

                    tblRow.Cells.Add(tblCell);
                    tblCell.Dispose();
                }
            }

            tabla.Rows.Add(tblRow);
            tblRow.Dispose();
            rowId++;
        }

        //Fin Filas


        //arma pie y el separador/
        TableRow tblRowSeparator = new TableRow();
        TableCell tblCellSeparator = new TableCell();
        tblCellSeparator.Text = "&nbsp;";
        tblRowSeparator.Cells.Add(tblCellSeparator);
        tabla.Rows.Add(tblRowSeparator);
        tblCellSeparator.Dispose();
        tblRowSeparator.Dispose();
        //fin arma pie
    }

    private void LoadCompetencies()
    {
        string username = UserHelper.GetUserId(Request.LogonUserIdentity.Name);
        string oldGroup = string.Empty;
        string group = string.Empty;
        object[][] details = new object[5][];
        int i = 0;


        _params = Encryption.Decrypt(Request.Params["."]).Split('.');

        int groupId = Convert.ToInt16(_params[0]);
        int profile = Convert.ToInt16(_params[1]);
        if (profile > 1)
        {
            username = _params[2];
        }

        string filtro = "%";

        if (_params.Length > 3)
            filtro = SQLHelper.ExecuteDataset(Cache["ApplicationDatabase"].ToString(), "GetEvaluatedByEvaluated", new object[] { _params[3] }).Tables[0].Rows[0].ItemArray[0].ToString();


        SqlDataReader myReader = SQLHelper.ExecuteReader(Cache["ApplicationDatabase"].ToString(), "[GetCompetenciesSelfEvaluation]", new object[] { groupId, username, profile, filtro });


        bool existone = false;

        for (int f = 0; f < myReader.FieldCount; f++)
        {
            fields.Add(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(myReader.GetName(f).Split('_')[0].ToString().Replace('[', ' ').Replace(']', ' ').Trim().ToLower()));
            fieldsValues.Add(myReader.GetName(f).ToString());



            if (f >= 4 && !existone)
            {
                if (fieldsValues[f].ToString().Split('_')[2].ToLower() == "t") { setColor = true; existone = true; }


            }
        }

        // ********************** 7 linea modificada original
        int indicegrupo = 0;

        while (myReader.Read())
        {
            group = myReader.GetValue(0).ToString();
            if (group != string.Empty && oldGroup != string.Empty && group != oldGroup)
            {
                numBlock++;
                AddCompetency(details);
                Array.Clear(details, 0, details.Length);
                i = 0;
            }
            object[] values = new object[myReader.FieldCount];
            int fieldcount = myReader.GetValues(values);
            details[i] = values;
            i++;
            oldGroup = group;
        }
        numBlock++;
        AddCompetency(details);

        AddCommentBlock("Comentarios");

        myReader.NextResult();



        //Comments
        string[] labels = new string[] { "Evaluado" };
        bool _enable = false;
        bool _visible = false;
        while (myReader.Read())
        {
            AddSeparator();
            Label lbl = new Label();
            if ((_params[1].ToString().Equals("0") && !CheckSentStatus(myReader.GetValue(0).ToString())) ? false : true)
                lbl.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(myReader.GetValue(1).ToString().Trim().ToLower());
            lbl.Attributes.Add("class", "titulo");
            lbl.Height = 20;
            this.Controls.Add(lbl);
            lbl.Dispose();

            Panel pnlContainer = new Panel();
            pnlContainer.Attributes.Add("class", "tab-pane");

            //Literal L = new Literal();
            ////L.Text = "<div style = 'position:absolute; left:655px;  top:7px;z-index:1000;'><a href='#t' class='link'>Ir a Competencias</a></div>";
            //// ******************** 10 linea modificada del original
            //L.Text = "<div style = 'position:absolute; left:655px;  top:-20px;z-index:1000;'><a href='#t' class='link'>Ir a Competencias</a></div>";
            //pnlContainer.Controls.Add(L);
            //L.Dispose();

            int _tabs = 1;

            // 1 = siempre
            // 2 = > 3,4,5,6,7,8
            // 3 = > 6

            // 1  Pendiente
            // 2  Iniciada
            // 3  Pendiente en Concurrente
            // 4  Aprobada por Concurrente
            // 5  Rechazada por Concurrente
            // 6  Pendiente en Doble Reporte
            // 7  Aprobada por Doble Reporte
            // 8  Rechazada por Doble Reporte
            // 9  Pendiente en RRHH
            // 10 Aprobada por RRHH
            // 11 Rechazada por RRHH
            // 12 Cerrada

            int _status = Convert.ToInt16(myReader[5]);

            _tabs = 1;

            for (int c = 0; c < 1; c++)
            {
                Panel pnlPage = new Panel();
                Literal H = new Literal();
                pnlPage.Attributes.Add("class", "tab-page");
                H.Text = "<h2 class='tab'>" + labels[c] + "</h2>";
                pnlPage.Controls.Add(H);

                TextBox txtA = new TextBox();
                txtA.Rows = 3;
                txtA.TextMode = TextBoxMode.MultiLine;
                //txtA.Text = myReader.GetValue( c + 2 ).ToString( );
                // ************* 7 linea modificada del original

                switch (c)
                {
                    case 0:
                        //if( myReader.GetValue( 7 ).ToString( ) == "A" && indicegrupo == 1 )
                        //    txtA.Text = myReader.GetValue( c + 2 ).ToString( );
                        //if( indicegrupo == 0 )
                        txtA.Text = myReader.GetValue(c + 2).ToString();
                        break;
                    case 1:
                        if (myReader.GetValue(7).ToString() == "A")
                            txtA.Text = myReader.GetValue(c + 2).ToString();
                        if (indicegrupo == 0)
                            txtA.Text = myReader.GetValue(c + 2).ToString();
                        if (txtA.Text == string.Empty)
                            txtA.Text = "En caso de elegir esta opción no olvide que es obligatorio completar el comentario ...........";
                        break;
                    case 2:
                        if (myReader.GetValue(8).ToString() == "A")
                            txtA.Text = myReader.GetValue(c + 2).ToString();
                        if (indicegrupo == 0)
                            txtA.Text = myReader.GetValue(c + 2).ToString();
                        if (txtA.Text == string.Empty)
                            txtA.Text = "En caso de elegir esta opción no olvide que es obligatorio completar el comentario ...........";
                        break;
                    case 3:
                        if (myReader.GetValue(10).ToString() == "A")
                            txtA.Text = myReader.GetValue(c + 6).ToString();
                        else
                            txtA.Text = string.Empty;
                        if (txtA.Text == string.Empty)
                            txtA.Text = "En caso de elegir esta opción no olvide que es obligatorio completar el comentario ...........";
                        break;
                }

                txtA.Attributes["class"] = "tM";
                //txtA.Attributes["onBlur"] = "o(this)";
                //txtA.Attributes["onFocus"] = "_i(this)";
                //txtA.Attributes["onkeyup"] = "kIP(this)";

                txtA.SkinID = myReader.GetValue(0).ToString() + "_" + c.ToString();
                HttpResponse myHttpReponse = Response;
                HtmlTextWriter myHtmlTextWriter = new HtmlTextWriter(myHttpReponse.Output);
                txtA.Attributes.AddAttributes(myHtmlTextWriter);

                RadioButton radioA = new RadioButton();
                radioA.Text = "Aprobar";
                //radioA.GroupName = "AR";
                // **************** 7 linea modificada original
                radioA.GroupName = "AR" + indicegrupo.ToString();
                radioA.Attributes["onClick"] = "_a(this)";
                radioA.Checked = true;


                // *************** agregado del original
                switch (c)
                {
                    case 0:
                        if (myReader.GetValue(7).ToString() == "A")
                            radioA.Checked = true;
                        break;
                    case 1:
                        if (myReader.GetValue(7).ToString() == "A")
                            radioA.Checked = true;
                        break;
                    case 2:
                        if (myReader.GetValue(8).ToString() == "A")
                            radioA.Checked = true;
                        break;
                    case 3:
                        if (myReader.GetValue(10).ToString() == "A")
                            radioA.Checked = true;
                        break;
                }
                RadioButton radioR = new RadioButton();
                radioR.Text = "Rechazar";
                //radioR.GroupName = "AR";
                // **************** 7 linea modificada original
                radioR.GroupName = "AR" + indicegrupo.ToString();

                radioR.Attributes["onClick"] = "_r(this)";
                // *************** agregado del original
                switch (c)
                {
                    case 0:
                        if (myReader.GetValue(7).ToString() == "R")
                            radioR.Checked = false;
                        break;
                    case 1:
                        if (myReader.GetValue(7).ToString() == "R")
                            radioR.Checked = false;
                        break;
                    case 2:
                        if (myReader.GetValue(8).ToString() == "R")
                            radioR.Checked = false;
                        break;
                    case 3:
                        if (myReader.GetValue(10).ToString() == "R")
                            radioR.Checked = false;
                        break;
                }

                TextBox txtR = new TextBox();

                // 3  Pendiente en Concurrente
                // 4  Aprobada por Concurrente
                // 5  Rechazada por Concurrente
                // 6  Pendiente en Doble Reporte
                // 7  Aprobada por Doble Reporte
                // 8  Rechazada por Doble Reporte
                // 9  Pendiente en RRHH
                // 10 Aprobada por RRHH
                // 11 Rechazada por RRHH
                // 12 Cerrada

                switch (profile)
                {
                    case 1:
                        switch (c)
                        {
                            case 0:
                                radioA.Visible = false;
                                txtA.Visible = true;
                                radioR.Visible = false;
                                break;
                            case 1:
                                switch (_status)
                                {
                                    case 5:
                                        radioA.Visible = false;
                                        txtA.Visible = false;
                                        radioR.Visible = true;
                                        txtR.Visible = true;
                                        break;
                                    case 6:
                                        radioA.Visible = false;
                                        txtA.Visible = true;
                                        radioR.Visible = false;
                                        txtR.Visible = false;
                                        break;
                                    case 8:
                                        radioA.Visible = false;
                                        txtA.Visible = true;
                                        radioR.Visible = false;
                                        txtR.Visible = false;
                                        break;
                                    case 9:
                                        radioA.Visible = false;
                                        txtA.Visible = true;
                                        radioR.Visible = false;
                                        txtR.Visible = false;
                                        break;
                                    case 10:
                                        radioA.Visible = false;
                                        txtA.Visible = true;
                                        radioR.Visible = false;
                                        txtR.Visible = false;
                                        break;
                                    case 11:
                                        radioA.Visible = false;
                                        txtA.Visible = true;
                                        txtR.Visible = false;
                                        radioR.Visible = false;
                                        break;
                                    case 12:
                                        radioA.Visible = false;
                                        txtA.Visible = true;
                                        radioR.Visible = false;
                                        txtR.Visible = false;
                                        break;
                                }
                                break;
                            case 2:
                                switch (_status)
                                {
                                    case 5:
                                        radioA.Visible = false;
                                        txtA.Visible = false;
                                        radioR.Visible = false;
                                        txtR.Visible = true;
                                        break;
                                    case 8:
                                        radioA.Visible = false;
                                        txtA.Visible = false;
                                        radioR.Visible = false;
                                        txtR.Visible = true;
                                        break;
                                    case 9:
                                        radioA.Visible = false;
                                        txtA.Visible = true;
                                        radioR.Visible = false;
                                        txtR.Visible = false;
                                        break;
                                    case 10:
                                        radioA.Visible = false;
                                        txtA.Visible = true;
                                        radioR.Visible = false;
                                        txtR.Visible = false;
                                        break;
                                    case 11:
                                        radioA.Visible = false;
                                        txtA.Visible = true;
                                        txtR.Visible = false;
                                        radioR.Visible = false;
                                        break;
                                    case 12:
                                        radioA.Visible = false;
                                        txtA.Visible = true;
                                        radioR.Visible = false;
                                        txtR.Visible = false;
                                        break;
                                }
                                break;
                            case 3:
                                switch (_status)
                                {
                                    case 5:
                                        radioA.Visible = false;
                                        txtA.Visible = false;
                                        radioR.Visible = false;
                                        txtR.Visible = true;
                                        break;
                                    case 8:
                                        radioA.Visible = false;
                                        txtA.Visible = false;
                                        radioR.Visible = false;
                                        txtR.Visible = true;
                                        break;
                                    case 9:
                                        radioA.Visible = true;
                                        txtA.Visible = true;
                                        radioR.Visible = true;
                                        txtR.Visible = true;
                                        break;
                                    case 10:
                                        radioA.Visible = false;
                                        txtA.Visible = true;
                                        radioR.Visible = false;
                                        txtR.Visible = false;
                                        break;
                                    case 11:
                                        radioA.Visible = false;
                                        txtA.Visible = false;
                                        txtR.Visible = true;
                                        radioR.Visible = false;
                                        break;
                                    case 12:
                                        radioA.Visible = false;
                                        txtA.Visible = true;
                                        radioR.Visible = false;
                                        txtR.Visible = false;
                                        break;
                                }
                                break;

                        }
                        txtA.ReadOnly = !(("1.2.5.8.11".IndexOf(_status.ToString()) >= 0) && (c == 0));
                        txtR.ReadOnly = !(("1.2.5.8.11".IndexOf(_status.ToString()) >= 0) && (c == 0));

                        break;
                    case 2:
                        // *************** 6 modificado del original
                        //radioA.Visible = (c >= 1);
                        //txtA.ReadOnly = ((_status == 3) && (c == 1));
                        //radioR.Visible = (c >= 1);
                        //txtR.ReadOnly = ((_status == 3) && (c == 1));
                        //break;
                        radioA.Visible = (c >= 1);
                        txtA.ReadOnly = (c == 0); // || ( c != 0 && txtA.Text == string.Empty );
                        radioR.Visible = (c >= 1);
                        txtR.ReadOnly = (c == 0); // || ( c != 0 && txtR.Text == string.Empty );
                        break;

                    case 3:
                        _visible = (c >= 1);
                        _enable = (_status == 6) && (c == 2);
                        switch (c)
                        {
                            case 0:
                                radioA.Visible = false;
                                txtA.Visible = true;
                                radioR.Visible = false;
                                txtA.ReadOnly = true;
                                txtR.ReadOnly = true;
                                break;
                            case 1:
                                radioA.Visible = !("8.9.6".IndexOf(_status.ToString()) >= 0);
                                txtA.Visible = !("8.11".IndexOf(_status.ToString()) >= 0);
                                radioR.Visible = !("8.9.6".IndexOf(_status.ToString()) >= 0);
                                txtR.Visible = ("8.11".IndexOf(_status.ToString()) >= 0);
                                txtA.ReadOnly = true;
                                txtR.ReadOnly = true;
                                break;
                            case 2:
                                radioA.Visible = !("8.9.11".IndexOf(_status.ToString()) >= 0);
                                txtA.Visible = !("8.9".IndexOf(_status.ToString()) >= 0);
                                radioR.Visible = !("8.9.11".IndexOf(_status.ToString()) >= 0);
                                txtR.Visible = ("8.9.6".IndexOf(_status.ToString()) >= 0);
                                txtA.ReadOnly = false;
                                txtR.ReadOnly = false;
                                break;
                            case 3:
                                radioA.Visible = true;
                                radioR.Visible = true;
                                break;

                        }
                        //radioA.Enabled = ( c == 3 );
                        //txtA.ReadOnly = ( c < 3 ) || ( c != 0 && c != 3 && txtA.Text == string.Empty );
                        //radioR.Enabled = ( c == 3 );
                        //txtR.ReadOnly = ( c < 3 ) || ( c != 0 && c != 3 && txtR.Text == string.Empty );
                        break;

                    // *************** 11 lineas agregadas al original
                    case 5:
                        switch (c)
                        {
                            case 0:
                                radioA.Visible = false;
                                txtA.Visible = true;
                                radioR.Visible = false;
                                break;
                            case 1:
                                radioA.Visible = !("8.9".IndexOf(_status.ToString()) >= 0);
                                txtA.Visible = !("8.11".IndexOf(_status.ToString()) >= 0);
                                radioR.Visible = !("8.9".IndexOf(_status.ToString()) >= 0);
                                txtR.Visible = ("8.11".IndexOf(_status.ToString()) >= 0);
                                break;
                            case 2:
                                radioA.Visible = !("8.9.11".IndexOf(_status.ToString()) >= 0);
                                txtA.Visible = !("8".IndexOf(_status.ToString()) >= 0);
                                radioR.Visible = !("8.9.11".IndexOf(_status.ToString()) >= 0);
                                txtR.Visible = ("8".IndexOf(_status.ToString()) >= 0);
                                break;
                            case 3:
                                radioA.Visible = true;
                                radioR.Visible = true;
                                break;

                        }
                        //radioA.Enabled = ( c == 3 );
                        txtA.ReadOnly = (c < 3) || (c != 0 && c != 3 && txtA.Text == string.Empty);
                        //radioR.Enabled = ( c == 3 );
                        txtR.ReadOnly = (c < 3) || (c != 0 && c != 3 && txtR.Text == string.Empty);
                        break;
                    default:
                        _visible = true;
                        _enable = false;
                        break;
                }


                //tab 1


                txtA.ReadOnly = (!(profile == 0 && !CheckSentStatus(txtA.SkinID.Split('_')[0].ToString())));
                radioA.Visible = false;

                pnlPage.Controls.Add(radioA);
                pnlPage.Controls.Add(txtA);

                if (c >= 1)
                {
                    txtR.Rows = 3;
                    txtR.TextMode = TextBoxMode.MultiLine;
                    txtR.Text = myReader.GetValue(c + 2).ToString();
                    // ******************* agregado del original
                    switch (c)
                    {
                        case 0:
                            if (myReader.GetValue(7).ToString() == "R")
                                txtR.Text = myReader.GetValue(c + 2).ToString();
                            else
                                txtR.Text = string.Empty;
                            if (txtR.Text == string.Empty)
                                txtR.Text = "En caso de elegir esta opción no olvide que es obligatorio completar el comentario ...........";
                            break;
                        case 1:
                            if (myReader.GetValue(7).ToString() == "R")
                                txtR.Text = myReader.GetValue(c + 2).ToString();
                            else
                                txtR.Text = string.Empty;
                            if (txtR.Text == string.Empty)
                                txtR.Text = "En caso de elegir esta opción no olvide que es obligatorio completar el comentario ...........";
                            break;
                        case 2:
                            if (myReader.GetValue(8).ToString() == "R")
                                txtR.Text = myReader.GetValue(c + 2).ToString();
                            else
                                txtR.Text = string.Empty;
                            if (txtR.Text == string.Empty)
                                txtR.Text = "En caso de elegir esta opción no olvide que es obligatorio completar el comentario ...........";
                            break;
                        case 3:
                            if (myReader.GetValue(10).ToString() == "R")
                                txtR.Text = myReader.GetValue(c + 6).ToString();
                            else
                                txtR.Text = string.Empty;
                            if (txtR.Text == string.Empty)
                                txtR.Text = "En caso de elegir esta opción no olvide que es obligatorio completar el comentario ...........";
                            break;

                    }

                    txtR.Attributes["class"] = "tM";
                    txtR.Attributes["onBlur"] = "o(this)";
                    txtR.Attributes["onFocus"] = "_i(this)";
                    if (_enable)
                    {
                        txtA.Attributes["onkeyup"] = "kIP(this)";
                    }

                    txtR.SkinID = myReader.GetValue(0).ToString() + "_" + c.ToString();
                    HttpResponse myHttpReponseR = Response;
                    HtmlTextWriter myHtmlTextWriterR = new HtmlTextWriter(myHttpReponseR.Output);
                    txtA.Attributes.AddAttributes(myHtmlTextWriterR);

                    pnlPage.Controls.Add(radioR);
                    pnlPage.Controls.Add(txtR);
                }

                pnlContainer.Controls.Add(pnlPage);
                txtA.Dispose();
                txtR.Dispose();
                radioA.Dispose();
                radioR.Dispose();
                H.Dispose();
                pnlPage.Dispose();
                pnlContainer.Dispose();


                // ************* 7 linea agregada al original
                indicegrupo++;

            }
            this.Controls.Add(pnlContainer);
        }
        myReader.Close();
    }

    private void AddCompetency(object[][] d)
    {
        Table tblContainer = new Table();
        TableRow tblRow = new TableRow();
        TableCell tblCellLeft = new TableCell();
        TableCell tblCellRight = new TableCell();
        Panel pnlLeft = new Panel();
        Panel pnlRight = new Panel();
        Table tblLeft = new Table();
        Table tblRight = new Table();

        tblContainer.BorderWidth = 0;
        tblContainer.CellSpacing = 1;
        tblContainer.CellPadding = 0;

        switch (d[0][0].ToString())
        {
            case "1":
                AddCompetencyBlock("Competencias FEMSA", true);
                break;
            default:
                string blocks = "1|2|3|4|5|";
                if ((!setCompetency) && blocks.IndexOf(d[0][0].ToString() + "|", 0) == -1)
                {
                    setCompetency = true;
                    AddCompetencyBlock("Competencias Funcionales", false);
                }
                break;
        }

        Build(d, tblLeft, tblRight);

        pnlLeft.Controls.Add(tblLeft);
        pnlLeft.Attributes.Add("class", "pnlL");
        pnlRight.Attributes.Add("class", "pnlR");
        pnlRight.Controls.Add(tblRight);

        tblCellLeft.VerticalAlign = VerticalAlign.Top;
        tblCellLeft.HorizontalAlign = HorizontalAlign.Left;
        tblCellLeft.Controls.Add(pnlLeft);
        tblCellRight.Controls.Add(pnlRight);
        Panel pnlAll = new Panel();
        if (d[0].Length - 4 > 10)
            tblCellLeft.Width = cellWidth;
        else
            tblCellLeft.Width = Unit.Percentage(50);
        pnlAll.Controls.Add(tblCellLeft);
        pnlAll.Controls.Add(tblCellRight);
        TableCell tblCellAll = new TableCell();
        tblCellAll.Controls.Add(pnlAll);
        tblRow.Cells.Add(tblCellAll);

        tblContainer.Rows.Add(tblRow);

        this.Controls.Add(tblContainer);

        AddSeparator();

        tblLeft.Dispose();
        tblRight.Dispose();
        pnlLeft.Dispose();
        pnlRight.Dispose();
        tblCellLeft.Dispose();
        tblCellRight.Dispose();
        tblRow.Dispose();
        tblContainer.Dispose();

    }

    private void AddSeparator()
    {
        Table tblSeparator = new Table();
        TableRow tblRowSeparator = new TableRow();
        TableCell tblCellSeparator = new TableCell();
        tblCellSeparator.Attributes.Add("class", "separator1");
        tblRowSeparator.Cells.Add(tblCellSeparator);
        tblSeparator.Rows.Add(tblRowSeparator);
        this.Controls.Add(tblSeparator);
        tblCellSeparator.Dispose();
        tblRowSeparator.Dispose();
    }

    public void Build(object[][] d, Table tblL, Table tblR)
    {
        int color = 0;
        string _style = string.Empty;

        TableRow tblRow = new TableRow();
        TableCell tblCell = new TableCell();
        //----------------------------
        //Competencia
        tblCell.Text = d[0][2].ToString().Trim();
        if (d[0].Length - 4 > 10)
            tblCell.Width = cellWidth;
        else
            tblCell.Width = Unit.Percentage(50);
        tblCell.Attributes.Add("class", "td08");
        //tblCell.Attributes.Add("nowrap", "true");
        tblRow.Cells.Add(tblCell);



        TableRow tblRowR = new TableRow();
        for (int c = 4; c < d[0].Length; c++)
        {
            TableCell tblCell0 = new TableCell();
            tblCell0.Text = fields[c].ToString();
            tblCell0.ToolTip = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(fieldsValues[c].ToString().Split('_')[3].ToLower());
            tblCell0.Attributes.Add("class", "td04");
            tblRowR.Cells.Add(tblCell0);
            tblCell0.Dispose();
        }
        tblR.Rows.Add(tblRowR);
        tblRowR.Dispose();


        for (int i = 0; i < d.Length; i++)
        {
            TableRow tblRow0 = new TableRow();
            TableCell tblCell1 = new TableCell();
            tblCell1.Text = d[i][3].ToString().Trim();
            if (d[0].Length - 4 > 10)
                tblCell1.Width = cellWidth;
            else
                tblCell1.Width = Unit.Percentage(50);
            tblCell1.Attributes.Add("class", "td02_");
            tblRow0.Cells.Add(tblCell1);
            tblCell1.Dispose();
            tblL.Rows.Add(tblRow0);
            tblRow0.Dispose();

            TableRow tblRowR0 = new TableRow();
            for (int c = 4; c < d[i].Length; c++)
            {
                TableCell tblCell0 = new TableCell();
                tblCell0.Attributes.Add("class", "td05");
                RadioButton radio = new RadioButton();
                radio.EnableViewState = false;
                radio.GroupName = c.ToString() + numBlock.ToString();
                radio.SkinID = fieldsValues[c].ToString().Split('_')[1].ToString().ToLower() + "_" + d[i][0].ToString().Trim() + "_" + d[i][1].ToString() + "_" + (i + 1).ToString() + "_" + d[i][4].ToString();
                radio.Checked = (d[i][c].ToString() != "0") ? true : false;
                if (radio.Checked) { color++; }
                radio.Enabled = ((_params[1].ToString().Equals("0") && !CheckSentStatus(fieldsValues[c].ToString().Split('_')[1].ToString())) ? true : false);
                tblCell0.Controls.Add(radio);
                radio.Dispose();
                tblRowR0.Cells.Add(tblCell0);
                tblCell0.Dispose();
            }
            tblR.Rows.Add(tblRowR0);
            tblRowR0.Dispose();
        }

        TableRow tblRowColor = new TableRow();
        TableCell tblCellColorL = new TableCell();
        tblCellColorL.ID = "C" + numBlock.ToString();

        if (color == fields.Count - 4)
        {
            _style = "g";
        }
        else if (color == 0)
        {
            _style = "r";
        }
        else
        {
            _style = "y";
        }



        //if (color == fields.Count - 4)
        //    tblR.Attributes["onclick"] = "c(this,false)";
        //else
        //    tblR.Attributes["onclick"] = "c(this,true)";

        tblL.Rows.AddAt(0, tblRow);
        tblRow.Dispose();


        tblCellColorL.Attributes.Add("class", _style);
        tblCellColorL.Visible = false;
        tblRowColor.Cells.Add(tblCellColorL);
        tblL.Rows.Add(tblRowColor);
        tblCellColorL.Dispose();
        tblRowColor.Dispose();
        tblL.Dispose();
        tblR.Dispose();
    }

    private void AddCompetencyBlock(string text, bool evaluatedPeoble)
    {
        Table tbl = new Table();
        tbl.Attributes.Add("width", "100%");
        tbl.BorderWidth = 0;
        tbl.CellSpacing = 0;
        //tbl.CellPadding = 3;

        TableRow tblRow = new TableRow();
        TableCell tblCell = new TableCell();
        tblCell.Text = text;
        tblCell.ColumnSpan = 2;
        tblCell.Height = 20;
        tblCell.Attributes.Add("class", "td01");
        tblRow.Cells.Add(tblCell);
        tbl.Rows.Add(tblRow);
        tblCell.Dispose();
        tblRow.Dispose();

        if (evaluatedPeoble)
        {
            TableRow tblRowE = new TableRow();
            TableCell tblCellE = new TableCell();
            TableCell tblCellE1 = new TableCell();
            tblCellE.Width = cellWidth + 6;
            tblCellE1.Attributes.Add("class", "td03");
            tblCellE1.Text = "Personas a Evaluar (" + (fields.Count - 4).ToString() + ")";
            tblRowE.Cells.AddRange(new TableCell[] { tblCellE1 });
            tbl.Rows.Add(tblRowE);
            tblCellE.Dispose();
            tblRowE.Dispose();
        }
        this.Controls.Add(tbl);
        tbl.Dispose();
    }

    private void AddCommentBlock(string text)
    {
        Table tbl = new Table();
        tbl.Attributes.Add("width", "100%");
        tbl.BorderWidth = 0;
        tbl.CellSpacing = 0;

        TableRow tblRow = new TableRow();
        TableCell tblCell = new TableCell();
        tblCell.Text = text;
        tblCell.ColumnSpan = 2;
        tblCell.Height = 20;
        tblCell.Attributes.Add("class", "td01");
        tblRow.Cells.Add(tblCell);
        tbl.Rows.Add(tblRow);
        tblCell.Dispose();
        tblRow.Dispose();
        this.Controls.Add(tbl);
        tbl.Dispose();

        TableRow tblRowAclaracion = new TableRow();
        TableCell tblCellAclaracion = new TableCell();
        Label lblAclaracion = new Label();
        lblAclaracion.Text = "En caso de querer realizar algún comentario adicional a la autoevaluación, por favor realizarlo a continuación:";
        lblAclaracion.ForeColor = System.Drawing.Color.Red;
        tblCellAclaracion.Controls.Add(lblAclaracion);
        tblRowAclaracion.Cells.Add(tblCellAclaracion);
        tbl.Rows.Add(tblRowAclaracion);
    }

    private void LoadImprovementPlan()
    {
        string username = UserHelper.GetUserId(Request.LogonUserIdentity.Name);
        string oldGroup = string.Empty;
        string group = string.Empty;
        object[][] details = new object[0][];

        _params = Encryption.Decrypt(Request.Params["."]).Split('.');

        int groupId = Convert.ToInt16(_params[0]);
        int profile = Convert.ToInt16(_params[1]);

        if (profile > 1)
        {
            username = _params[2];
        }

        string filtro = "%";

        if (_params.Length > 3)
            filtro = SQLHelper.ExecuteDataset(Cache["ApplicationDatabase"].ToString(), "GetEvaluatedByEvaluated", new object[] { _params[3] }).Tables[0].Rows[0].ItemArray[0].ToString();

        SqlDataReader myReader = SQLHelper.ExecuteReader(Cache["ApplicationDatabase"].ToString(), "[GetResultImprovementPlanSelfEvaluation]", new object[] { groupId, username, profile, filtro });

        AddSeparatorIP();

        while (myReader.Read())
        {
            object[] values = new object[myReader.FieldCount];
            int f = myReader.GetValues(values);
            BuildIP(values);
            tblID++;
            AddSeparatorIP();
        }
        myReader.Close();
    }

    private void BuildIP(object[] d)
    {
        string _style = "";
        string tmpText = "";
        int color = 0;
        Table tbl = new Table();
        tbl.Attributes.Add("width", "100%");
        tbl.BorderWidth = 0;
        tbl.CellSpacing = 1;
        tbl.CellPadding = 0;
        tbl.ID = "tbl_" + tblID.ToString();

        TableRow tblRowEvaluated = new TableRow();
        TableCell tblCellEvaluated = new TableCell();
        if (_params[1].ToString().Equals("0"))
            tblCellEvaluated.Text = "Agenda de desarrollo";
        else
            tblCellEvaluated.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(d[1].ToString().Trim().ToLower());
        tblCellEvaluated.ColumnSpan = 2;
        tblCellEvaluated.Attributes.Add("class", "td01");
        tblRowEvaluated.Cells.Add(tblCellEvaluated);
        tbl.Rows.Add(tblRowEvaluated);
        tblCellEvaluated.Dispose();
        tblRowEvaluated.Dispose();

        TableRow tblRowAclaracion = new TableRow();
        TableCell tblCellAclaracion = new TableCell();
        tblCellAclaracion.Text = "Por favor, seleccionar 2 competencias que usted considera que debería desarrollar y la forma en que podría mejorarlas.";
        tblCellAclaracion.ForeColor = System.Drawing.Color.Red;
        tblRowAclaracion.Cells.Add(new TableCell());
        tblRowAclaracion.Cells.Add(tblCellAclaracion);

        TableRow tblRow0 = new TableRow();
        TableRow tblRow1 = new TableRow();
        TableRow tblRow2 = new TableRow();
        TableRow tblRow3 = new TableRow();
        TableRow tblRow4 = new TableRow();

        TableCell tblCell01 = new TableCell();
        TableCell tblCell02 = new TableCell();

        DataTable dtCompetencies = new clsDAO().SqlCall("[GetCompetenciesImprovement] '" + d[0].ToString() + "'");

        DropDownList ddl1 = new DropDownList();
        ddl1.Attributes["class"] = "tMpc";
        ddl1.Enabled = ((_params[1].ToString().Equals("0") && !CheckSentStatus(d[0].ToString())) ? true : false);
        ddl1.Items.Add("<< Seleccione una competencia a mejorar >>");
        foreach (DataRow dr in dtCompetencies.Rows)
        {
            ddl1.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
        }
        ddl1.SelectedValue = d[2].ToString();
        if (ddl1.SelectedIndex > 0)
            color++;
        tblCell02.Controls.Add(ddl1);

        DropDownList ddl2 = new DropDownList();
        ddl2.Attributes["class"] = "tMpc";
        ddl2.Enabled = ((_params[1].ToString().Equals("0") && !CheckSentStatus(d[0].ToString())) ? true : false);
        ddl2.Items.Add("<< Seleccione una competencia a mejorar >>");
        foreach (DataRow dr in dtCompetencies.Rows)
        {
            ddl2.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
        }
        ddl2.SelectedValue = d[3].ToString();
        if (ddl2.SelectedIndex > 0 && ddl1.SelectedIndex != ddl2.SelectedIndex)
            color++;
        tblCell02.Controls.Add(ddl2);


        TableCell tblCell1 = new TableCell();
        TableCell tblCell2 = new TableCell();
        TableCell tblCell3 = new TableCell();

        tblCell2.Text = "Qué mejorar";
        tblCell3.Text = "Cómo";
        tblCell2.Attributes.Add("class", "td03");
        tblCell3.Attributes.Add("class", "td03");

        TableCell tblCell11 = new TableCell();
        TableCell tblCell21 = new TableCell();
        TableCell tblCell31 = new TableCell();
        tblCell11.Width = 200;
        //tblCell2.Width = Unit.Percentage(40);
        //tblCell3.Width = Unit.Percentage(40);

        //tblCell11.Text = "70%  On The Job";
        //tblCell11.ToolTip = "ON THE JOB:\n1.Enriquecimiento de las responsabilidades.\n2.Rotación en el trabajo, ampliación del puesto.\n3.Asignaciones a proyectos desafiantes.\n4.Transferencias laterales o asignaciones especiales.\n5.Dar oportunidad para hacer presentaciones y luego exponerlas.\n6.Involucramiento en reuniones, juntas, discusiones, etc.\n7.Liderar como instructor, capacitar a otros, etc.\n8.Sesiones especiales de feedback.\nCOMPLEMENTARIAS:\n9.Células de entrenamiento.\n10.Investigación. Aprendizaje del mercado.\n11.Autoestudio.\n12.Bench de prácticas.\n13.Programas institucionales.\n14.Círculo de aprendizaje con otros pares.";
        HyperLink lnk = new HyperLink();
        lnk.Text = "70%  On The Job";
        lnk.NavigateUrl = "~/img/On The Job.jpg";
        lnk.Target = "blank";
        lnk.ForeColor = System.Drawing.Color.Black;
        tblCell11.Controls.Add(lnk);

        tblCell11.Attributes.Add("class", "td03");
        tblCell21.Attributes.Add("class", "td02");
        tblCell31.Attributes.Add("class", "td02");

        TextBox txt11 = new TextBox();
        txt11.Text = d[4].ToString();
        tmpText = System.Text.RegularExpressions.Regex.Replace(txt11.Text, @"[^0-9a-zA-Z]+", "").Trim();
        if (tmpText.Length > 2 || tmpText.Equals("NA")) { color++; };
        txt11.Attributes["class"] = "tMp";
        txt11.Rows = 3;
        txt11.TextMode = TextBoxMode.MultiLine;
        txt11.Width = Unit.Percentage(98);
        txt11.ReadOnly = ((_params[1].ToString().Equals("0") && !CheckSentStatus(d[0].ToString())) ? false : true);
        txt11.SkinID = d[0].ToString();
        tblCell31.Controls.Add(txt11);

        TableCell tblCell111 = new TableCell();
        TableCell tblCell211 = new TableCell();
        TableCell tblCell311 = new TableCell();
        tblCell111.Width = 100;

        HyperLink lnk2 = new HyperLink();
        lnk2.Text = "20%  Coaching";
        lnk2.NavigateUrl = "~/img/Coaching.jpg";
        lnk2.Target = "blank";
        lnk2.ForeColor = System.Drawing.Color.Black;
        tblCell111.Controls.Add(lnk2);

        tblCell111.Attributes.Add("class", "td03");
        tblCell211.Attributes.Add("class", "td02");
        tblCell311.Attributes.Add("class", "td02");

        TextBox txt21 = new TextBox();
        txt21.Text = d[5].ToString();
        tmpText = System.Text.RegularExpressions.Regex.Replace(txt21.Text, @"[^0-9a-zA-Z]+", "").Trim();
        if (tmpText.Length > 2 || tmpText.Equals("NA")) { color++; };
        txt21.Attributes["class"] = "tMp";
        txt21.Rows = 3;
        txt21.TextMode = TextBoxMode.MultiLine;
        txt21.Width = Unit.Percentage(98);
        txt21.ReadOnly = ((_params[1].ToString().Equals("0") && !CheckSentStatus(d[0].ToString())) ? false : true);
        txt21.SkinID = d[0].ToString();
        tblCell311.Controls.Add(txt21);

        TableCell tblCell1111 = new TableCell();
        TableCell tblCell2111 = new TableCell();
        TableCell tblCell3111 = new TableCell();
        tblCell1111.Width = 100;

        HyperLink lnk3 = new HyperLink();
        lnk3.Text = "10% Capacitación Formal";
        lnk3.NavigateUrl = "~/img/Capacitación.jpg";
        lnk3.Target = "blank";
        lnk3.ForeColor = System.Drawing.Color.Black;
        tblCell1111.Controls.Add(lnk3);

        tblCell1111.Attributes.Add("class", "td03");
        tblCell2111.Attributes.Add("class", "td02");
        tblCell3111.Attributes.Add("class", "td02");

        TextBox txt31 = new TextBox();
        txt31.Text = d[6].ToString();
        tmpText = System.Text.RegularExpressions.Regex.Replace(txt31.Text, @"[^0-9a-zA-Z]+", "").Trim();
        if (tmpText.Length > 2 || tmpText.Equals("NA")) { color++; };
        txt31.Attributes["class"] = "tMp";
        txt31.Rows = 3;
        txt31.TextMode = TextBoxMode.MultiLine;
        txt31.Width = Unit.Percentage(98);
        txt31.ReadOnly = ((_params[1].ToString().Equals("0") && !CheckSentStatus(d[0].ToString())) ? false : true);
        txt31.SkinID = d[0].ToString();
        tblCell3111.Controls.Add(txt31);

        //tblRow1.Cells.AddRange(new TableCell[] { tblCell1, tblCell2, tblCell3 });
        //tblRow2.Cells.AddRange(new TableCell[] { tblCell11, tblCell21, tblCell31 });
        //tblRow3.Cells.AddRange(new TableCell[] { tblCell111, tblCell211, tblCell311 });
        //tblRow4.Cells.AddRange(new TableCell[] { tblCell1111, tblCell2111, tblCell3111 });
        tblRow0.Cells.AddRange(new TableCell[] { tblCell01, tblCell02 });
        tblRow1.Cells.AddRange(new TableCell[] { tblCell1, tblCell3 });
        tblRow2.Cells.AddRange(new TableCell[] { tblCell11, tblCell31 });
        tblRow3.Cells.AddRange(new TableCell[] { tblCell111, tblCell311 });
        tblRow4.Cells.AddRange(new TableCell[] { tblCell1111, tblCell3111 });

        TableRow tblRowColor = new TableRow();
        TableRow tblRowCursos = new TableRow();
        //TableRow tblRowAclaracion = new TableRow();

        TableCell tblCellColor = new TableCell();
        TableCell tblCellColorL = new TableCell();
        tblCellColorL.ColumnSpan = 2;

        if (color == 5)
        {
            //Button btn = (Button)this.Page.Master.FindControl("CPH").FindControl("btnSend");
            //btn.Enabled = true;

            _style = "g";
        }
        else if (color == 0)
        {
            _style = "r";
        }
        else
        {
            _style = "y";
        }

        tblCellColorL.Attributes.Add("class", _style);


        //       tblCellColorL.ID = "C";
        tblRowColor.Cells.AddRange(new TableCell[] { tblCellColor, tblCellColorL });


        DataTable dt = new clsDAO().SqlCall("[sp_GetAllRegistrationsData] '" + d[0].ToString() + "','" + System.Configuration.ConfigurationManager.AppSettings["Year"] + "'");
        TreeView tvw = new TreeView();
        TreeNode tnCursos = new TreeNode("Cursos del empleado", "Cursos del empleado");

        if (dt.Rows.Count > 0)
        {
            tnCursos.SelectAction = TreeNodeSelectAction.Expand;
            tnCursos.Expanded = false;
        }
        else
        {
            tnCursos = new TreeNode("Sin cursos en el período evaluado", "Sin cursos en el período evaluado");
            tnCursos.SelectAction = TreeNodeSelectAction.None;
            tnCursos.Expanded = false;
        }

        tvw.Nodes.Add(tnCursos);

        foreach (DataRow dr in dt.Rows)
        {
            TreeNode tnCurso = new TreeNode(dr[0].ToString());
            tnCurso.SelectAction = TreeNodeSelectAction.None;
            tnCursos.ChildNodes.Add(tnCurso);
        }
        TableCell tblCellCursos = new TableCell();
        tblRowCursos.Controls.Add(tblCellCursos);
        tblCellCursos.Controls.Add(tvw);
        tblCellCursos.ColumnSpan = 2;

        //Label lblAclaracion = new Label();
        //lblAclaracion.Text = "Para activar el botón ENVIAR tendrás que completar todos los campos de la Agenda de Desarrollo de tu colaborador.";
        //lblAclaracion.ForeColor = System.Drawing.Color.Red;
        //TableCell tblCellAclaracion = new TableCell();
        //tblRowAclaracion.Controls.Add(tblCellAclaracion);
        //tblCellAclaracion.Controls.Add(lblAclaracion);
        //tblCellAclaracion.ColumnSpan = 2;


        int intObjPos = 0;
        bool blnDatos = false;

        dt = new clsDAO().SqlCall("[GetResultObjetives] '" + d[0].ToString() + "'," + _params[1]);
        if (dt.Rows.Count > 0)
            blnDatos = true;

        TableRow tblRowObj = new TableRow();
        TableCell tblCellObj = new TableCell();
        tblCellObj.Text = "Objetivos para el desarrollo profesional";
        tblCellObj.ColumnSpan = 2;
        tblCellObj.Attributes.Add("class", "td01");
        tblRowObj.Cells.Add(tblCellObj);

        TableRow tblRowObj0 = new TableRow();
        TableCell tblCellObj0 = new TableCell();
        tblCellObj0.Text = "Objetivos " + (Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Year"]) + 1).ToString();
        tblCellObj0.ColumnSpan = 2;
        tblCellObj0.Attributes.Add("class", "td03");
        tblCellObj0.Attributes.Add("align", "center");
        tblRowObj0.Cells.Add(tblCellObj0);

        TableRow tblRowObj1 = new TableRow();
        TableCell tblCellObj1 = new TableCell();
        tblCellObj1.ColumnSpan = 2;
        Label lblAclaracionObj1 = new Label();
        lblAclaracionObj1.Text = "Por favor, completar con 3 objetivos que ustedes se propone alcanzar durante el 2019 acorde al puesto que ocupa y las responsabilidades que tiene asignadas.";
        lblAclaracionObj1.ForeColor = System.Drawing.Color.Red;
        tblCellObj1.Controls.Add(lblAclaracionObj1);
        tblRowObj1.Cells.Add(tblCellObj1);

        TableRow tblRowObj2 = new TableRow();
        TableCell tblCellObj21 = new TableCell();
        tblCellObj21.Text = "Objetivo 1";
        tblCellObj21.Width = 75;
        tblCellObj21.Attributes.Add("class", "td02");
        tblCellObj21.Attributes.Add("align", "center");
        tblRowObj2.Cells.Add(tblCellObj21);
        TableCell tblCellObj22 = new TableCell();
        TextBox txtObj22 = new TextBox();
        txtObj22.Attributes["class"] = "tMo";
        txtObj22.Width = Unit.Percentage(98);
        txtObj22.ReadOnly = ((_params[1].ToString().Equals("0") && !CheckSentStatus(d[0].ToString())) ? false : true);
        txtObj22.SkinID = d[0].ToString();
        tblCellObj22.Controls.Add(txtObj22);
        tblRowObj2.Cells.Add(tblCellObj22);
        if (blnDatos)
        {
            txtObj22.Text = dt.Rows[intObjPos][3].ToString();
            intObjPos++;
        }


        TableRow tblRowObj3 = new TableRow();
        TableCell tblCellObj31 = new TableCell();
        tblCellObj31.Text = "Objetivo 2";
        tblCellObj31.Width = 75;
        tblCellObj31.Attributes.Add("class", "td02");
        tblCellObj31.Attributes.Add("align", "center");
        tblRowObj3.Cells.Add(tblCellObj31);
        TableCell tblCellObj32 = new TableCell();
        TextBox txtObj32 = new TextBox();
        txtObj32.Attributes["class"] = "tMo";
        txtObj32.Width = Unit.Percentage(98);
        txtObj32.ReadOnly = ((_params[1].ToString().Equals("0") && !CheckSentStatus(d[0].ToString())) ? false : true);
        txtObj32.SkinID = d[0].ToString();
        tblCellObj32.Controls.Add(txtObj32);
        tblRowObj3.Cells.Add(tblCellObj32);
        if (blnDatos)
        {
            txtObj32.Text = dt.Rows[intObjPos][3].ToString();
            intObjPos++;
        }

        TableRow tblRowObj4 = new TableRow();
        TableCell tblCellObj41 = new TableCell();
        tblCellObj41.Text = "Objetivo 3";
        tblCellObj41.Width = 75;
        tblCellObj41.Attributes.Add("class", "td02");
        tblCellObj41.Attributes.Add("align", "center");
        tblRowObj4.Cells.Add(tblCellObj41);
        TableCell tblCellObj42 = new TableCell();
        TextBox txtObj42 = new TextBox();
        txtObj42.Attributes["class"] = "tMo";
        txtObj42.Width = Unit.Percentage(98);
        txtObj42.ReadOnly = ((_params[1].ToString().Equals("0") && !CheckSentStatus(d[0].ToString())) ? false : true);
        txtObj42.SkinID = d[0].ToString();
        tblCellObj42.Controls.Add(txtObj42);
        tblRowObj4.Cells.Add(tblCellObj42);
        if (blnDatos)
        {
            txtObj42.Text = dt.Rows[intObjPos][3].ToString();
            intObjPos++;
        }

        TableRow tblRowObjT1 = new TableRow();
        TableCell tblCellObjT1 = new TableCell();
        tblCellObjT1.Text = "Desarrollo profesional";
        tblCellObjT1.ColumnSpan = 2;
        tblCellObjT1.Attributes.Add("class", "td01");
        tblRowObjT1.Cells.Add(tblCellObjT1);

        TableRow tblRowObj5 = new TableRow();
        TableCell tblCellObj5 = new TableCell();
        tblCellObj5.Attributes.Add("class", "td03");
        tblCellObj5.Attributes.Add("align", "left");
        tblCellObj5.ColumnSpan = 2;
        Label lblAclaracionObj5 = new Label();
        lblAclaracionObj5.Text = "1) ¿Cúales son tus áreas de interes? (por favor indicar en orden de preferencia siendo 1 la primer prioridad y 3 la última)";
        //lblAclaracionObj5.ForeColor = System.Drawing.Color.Red;
        tblCellObj5.Controls.Add(lblAclaracionObj5);
        tblRowObj5.Cells.Add(tblCellObj5);

        TableRow tblRowObj6 = new TableRow();
        TableCell tblCellObj6 = new TableCell();
        tblCellObj6.ColumnSpan = 2;
        tblRowObj6.Cells.Add(tblCellObj6);
        Table tblObj6 = new Table();
        tblObj6.Width = Unit.Percentage(99);
        tblCellObj6.Controls.Add(tblObj6);

        string[] arrOpcionesObj6 = new string[] {   "Administración y Finanzas",
                                                    "Marketing Operativo",
                                                    "Auditoría Interna",
                                                    "Planeamiento",
                                                    "Comercial",
                                                    "Recursos Humanos",
                                                    "Legales",
                                                    "Supply Chain"
                                                };

        for (int i = 0; i < arrOpcionesObj6.Length; i += 2)
        {
            TableRow tblObj61 = new TableRow();
            tblObj6.Rows.Add(tblObj61);
            TableCell tblCellObj611 = new TableCell();
            TableCell tblCellObj612 = new TableCell();
            tblCellObj611.Text = arrOpcionesObj6[i];
            DropDownList ddlObj612 = new DropDownList();
            tblCellObj612.Controls.Add(ddlObj612);
            ddlObj612.Items.Add(new ListItem("-"));
            for (int l = 1; l <= arrOpcionesObj6.Length; l++)
            {
                ddlObj612.Items.Add(new ListItem(l.ToString()));
            }
            ddlObj612.Enabled = !((_params[1].ToString().Equals("0") && !CheckSentStatus(d[0].ToString())) ? false : true);
            if (blnDatos)
            {
                ddlObj612.SelectedValue = dt.Rows[intObjPos][3].ToString();
                intObjPos++;
            }
            TableCell tblCellObj621 = new TableCell();
            TableCell tblCellObj622 = new TableCell();

            tblObj61.Cells.Add(tblCellObj611);
            tblCellObj611.Width = Unit.Percentage(40);
            tblCellObj611.Attributes.Add("class", "td02");
            tblCellObj611.Attributes.Add("align", "center");
            tblObj61.Cells.Add(tblCellObj612);
            tblCellObj612.Width = Unit.Percentage(10);
            tblObj61.Cells.Add(tblCellObj621);
            tblCellObj621.Width = Unit.Percentage(40);
            tblCellObj621.Attributes.Add("class", "td02");
            tblCellObj621.Attributes.Add("align", "center");
            tblObj61.Cells.Add(tblCellObj622);
            tblCellObj622.Width = Unit.Percentage(10);

            if ((i + 1) < arrOpcionesObj6.Length)
            {
                tblCellObj621.Text = arrOpcionesObj6[i + 1];
                DropDownList ddlObj622 = new DropDownList();
                tblCellObj622.Controls.Add(ddlObj622);
                ddlObj622.Items.Add(new ListItem("-"));
                for (int l = 1; l <= arrOpcionesObj6.Length; l++)
                {
                    ddlObj622.Items.Add(new ListItem(l.ToString()));
                }

                ddlObj622.Enabled = !((_params[1].ToString().Equals("0") && !CheckSentStatus(d[0].ToString())) ? false : true);
                if (blnDatos)
                {
                    ddlObj622.SelectedValue = dt.Rows[intObjPos][3].ToString();
                    intObjPos++;
                }
            }
            else
            {
                tblCellObj621.Text = "";
                tblCellObj621.Attributes.Remove("class");
            }
        }

        TableRow tblRowObj7 = new TableRow();
        TableCell tblCellObj7 = new TableCell();
        tblCellObj7.Attributes.Add("class", "td03");
        tblCellObj7.Attributes.Add("align", "left");
        tblCellObj7.ColumnSpan = 2;
        Label lblAclaracionObj7 = new Label();
        lblAclaracionObj7.Text = "2) ¿A que posición aspirás en el mediano plazo? (dentro de los próximos 3 años)";
        //lblAclaracionObj5.ForeColor = System.Drawing.Color.Red;
        tblCellObj7.Controls.Add(lblAclaracionObj7);
        tblRowObj7.Cells.Add(tblCellObj7);

        TableRow tblRowObj8 = new TableRow();

        TableCell tblCellObj81 = new TableCell();
        tblCellObj81.Text = "Posición";
        tblCellObj81.Width = 75;
        tblCellObj81.Attributes.Add("class", "td02");
        tblCellObj81.Attributes.Add("align", "center");
        tblRowObj8.Cells.Add(tblCellObj81);
        TableCell tblCellObj82 = new TableCell();
        TextBox txtObj82 = new TextBox();
        txtObj82.Attributes["class"] = "tMo";
        txtObj82.Width = Unit.Percentage(99);
        txtObj82.ReadOnly = ((_params[1].ToString().Equals("0") && !CheckSentStatus(d[0].ToString())) ? false : true);
        txtObj82.SkinID = d[0].ToString();
        tblCellObj82.Controls.Add(txtObj82);
        tblRowObj8.Cells.Add(tblCellObj82);
        if (blnDatos)
        {
            txtObj82.Text = dt.Rows[intObjPos][3].ToString();
            intObjPos++;
        }

        TableRow tblRowObj9 = new TableRow();
        TableCell tblCellObj9 = new TableCell();
        tblCellObj9.Attributes.Add("class", "td03");
        tblCellObj9.Attributes.Add("align", "left");
        tblCellObj9.ColumnSpan = 2;
        Label lblAclaracionObj9 = new Label();
        lblAclaracionObj9.Text = "3) Movilidad: ¿Estás interesado en tener una experiencia internacional en otro país?";
        //lblAclaracionObj5.ForeColor = System.Drawing.Color.Red;
        tblCellObj9.Controls.Add(lblAclaracionObj9);
        tblRowObj9.Cells.Add(tblCellObj9);

        TableRow tblRowObj10 = new TableRow();
        TableCell tblCellObj10 = new TableCell();
        tblCellObj10.ColumnSpan = 2;
        RadioButton rdo1 = new RadioButton();
        RadioButton rdo2 = new RadioButton();
        RadioButton rdo3 = new RadioButton();
        rdo1.GroupName = "Movilidad";
        rdo2.GroupName = "Movilidad";
        rdo3.GroupName = "Movilidad";
        rdo1.Attributes.Add("class", "tMom");
        rdo2.Attributes.Add("class", "tMom");
        rdo3.Attributes.Add("class", "tMom");
        rdo1.Text = "Sí, por tiempo indeterminado";
        rdo2.Text = "Sí, por tiempo determinado";
        rdo3.Text = "No trabajaría en el exterior";
        if(((_params[1].ToString().Equals("0") && !CheckSentStatus(d[0].ToString())) ? false : true))
        {
            rdo1.Enabled = false;
            rdo2.Enabled = false;
            rdo3.Enabled = false;
        }
        tblCellObj10.Controls.Add(rdo1);
        tblCellObj10.Controls.Add(rdo2);
        tblCellObj10.Controls.Add(rdo3);
        tblCellObj10.Attributes.Add("align", "center");
        tblRowObj10.Cells.Add(tblCellObj10);
        if (blnDatos)
        {
            string strChoice = dt.Rows[intObjPos][3].ToString();

            rdo1.Checked = false;
            rdo2.Checked = false;
            rdo3.Checked = false;

            if (strChoice.Equals(rdo1.Text))
                rdo1.Checked = true;
            if (strChoice.Equals(rdo2.Text))
                rdo2.Checked = true;
            if (strChoice.Equals(rdo3.Text))
                rdo3.Checked = true;

            intObjPos++;
        }

        TableRow tblRowObj11 = new TableRow();
        TableCell tblCellObj11 = new TableCell();
        tblCellObj11.Attributes.Add("class", "td03");
        tblCellObj11.Attributes.Add("align", "left");
        tblCellObj11.ColumnSpan = 2;
        Label lblAclaracionObj11 = new Label();
        lblAclaracionObj11.Text = "4) En caso de estar interesado, por favor seleccionar en orden de prioridad los paises elegidos:";
        //lblAclaracionObj5.ForeColor = System.Drawing.Color.Red;
        tblCellObj11.Controls.Add(lblAclaracionObj11);
        tblRowObj11.Cells.Add(tblCellObj11);

        TableRow tblRowObj12 = new TableRow();
        TableCell tblCellObj12 = new TableCell();
        tblCellObj12.ColumnSpan = 2;
        tblRowObj12.Cells.Add(tblCellObj12);
        Table tblObj12 = new Table();
        tblObj12.Width = Unit.Percentage(99);
        tblCellObj12.Controls.Add(tblObj12);

        string[] arrOpcionesObj12 = new string[] {  "Brasil",
                                                    "México",
                                                    "Colombia",
                                                    "Nicaragua",
                                                    "Costa Rica",
                                                    "Panamá",
                                                    "Uruguay",
                                                    "Venezuela",
                                                    "Guatemala"
                                                  };

        for (int i = 0; i < arrOpcionesObj12.Length; i += 2)
        {
            TableRow tblObj121 = new TableRow();
            tblObj12.Rows.Add(tblObj121);
            TableCell tblCellObj1211 = new TableCell();
            TableCell tblCellObj1212 = new TableCell();
            tblCellObj1211.Text = arrOpcionesObj12[i];
            DropDownList ddlObj1212 = new DropDownList();
            tblCellObj1212.Controls.Add(ddlObj1212);
            ddlObj1212.Items.Add(new ListItem("-"));
            for (int l = 1; l <= arrOpcionesObj12.Length; l++)
            {
                ddlObj1212.Items.Add(new ListItem(l.ToString()));
            }
            ddlObj1212.Enabled = !((_params[1].ToString().Equals("0") && !CheckSentStatus(d[0].ToString())) ? false : true);
            if (blnDatos)
            {
                ddlObj1212.SelectedValue = dt.Rows[intObjPos][3].ToString();
                intObjPos++;
            }

            TableCell tblCellObj1221 = new TableCell();
            TableCell tblCellObj1222 = new TableCell();

            tblObj121.Cells.Add(tblCellObj1211);
            tblCellObj1211.Width = Unit.Percentage(40);
            tblCellObj1211.Attributes.Add("class", "td02");
            tblCellObj1211.Attributes.Add("align", "center");
            tblObj121.Cells.Add(tblCellObj1212);
            tblCellObj1212.Width = Unit.Percentage(10);
            tblObj121.Cells.Add(tblCellObj1221);
            tblCellObj1221.Width = Unit.Percentage(40);
            tblCellObj1221.Attributes.Add("class", "td02");
            tblCellObj1221.Attributes.Add("align", "center");
            tblObj121.Cells.Add(tblCellObj1222);
            tblCellObj1222.Width = Unit.Percentage(10);

            if ((i + 1) < arrOpcionesObj12.Length)
            {
                tblCellObj1221.Text = arrOpcionesObj12[i + 1];
                DropDownList ddlObj1222 = new DropDownList();
                tblCellObj1222.Controls.Add(ddlObj1222);
                ddlObj1222.Items.Add(new ListItem("-"));
                for (int l = 1; l <= arrOpcionesObj12.Length; l++)
                {
                    ddlObj1222.Items.Add(new ListItem(l.ToString()));
                }

                ddlObj1222.Enabled = !((_params[1].ToString().Equals("0") && !CheckSentStatus(d[0].ToString())) ? false : true);
                if (blnDatos)
                {
                    ddlObj1222.SelectedValue = dt.Rows[intObjPos][3].ToString();
                    intObjPos++;
                }
            }
            else
            {
                tblCellObj1221.Text = "";
                tblCellObj1221.Attributes.Remove("class");
            }
        }

        TableRow tblRowObj13 = new TableRow();
        TableCell tblCellObj13 = new TableCell();
        tblCellObj13.Text = "Desarrollo / Entrenamiento " + System.Configuration.ConfigurationManager.AppSettings["Year"];
        tblCellObj13.ColumnSpan = 2;
        tblCellObj13.Attributes.Add("class", "td03");
        tblCellObj13.Attributes.Add("align", "center");
        tblRowObj13.Cells.Add(tblCellObj13);

        TableRow tblRowObj14 = new TableRow();
        TableCell tblCellObj14 = new TableCell();
        tblCellObj14.Attributes.Add("class", "td03");
        tblCellObj14.Attributes.Add("align", "left");
        tblCellObj14.ColumnSpan = 2;
        Label lblAclaracionObj14 = new Label();
        lblAclaracionObj14.Text = "1) ¿Considera haber cumplido su agenda de desarrollo planteada para el 2018?";
        //lblAclaracionObj14.ForeColor = System.Drawing.Color.Red;
        tblCellObj14.Controls.Add(lblAclaracionObj14);
        tblRowObj14.Cells.Add(tblCellObj14);

        TableRow tblRowObj15 = new TableRow();
        TableCell tblCellObj15 = new TableCell();
        tblCellObj15.ColumnSpan = 2;
        RadioButton rdo4 = new RadioButton();
        RadioButton rdo5 = new RadioButton();
        rdo4.GroupName = "Cumplimiento";
        rdo5.GroupName = "Cumplimiento";
        rdo4.Text = "Sí";
        rdo5.Text = "No";
        tblCellObj15.Controls.Add(rdo4);
        tblCellObj15.Controls.Add(rdo5);
        tblCellObj15.Attributes.Add("align", "center");
        tblRowObj15.Cells.Add(tblCellObj15);

        TableRow tblRowObj16 = new TableRow();
        TableCell tblCellObj16 = new TableCell();
        tblCellObj16.Attributes.Add("class", "td03");
        tblCellObj16.Attributes.Add("align", "left");
        tblCellObj16.ColumnSpan = 2;
        Label lblAclaracionObj16 = new Label();
        lblAclaracionObj16.Text = "2) Por favor enumere los entrenamientos en los cuales participó durante el 2018:";
        //lblAclaracionObj16.ForeColor = System.Drawing.Color.Red;
        tblCellObj16.Controls.Add(lblAclaracionObj16);
        tblRowObj16.Cells.Add(tblCellObj16);

        TableRow tblRowObj17 = new TableRow();
        TableCell tblCellObj17 = new TableCell();
        tblCellObj17.ColumnSpan = 2;
        TextBox txtObj17 = new TextBox();
        txtObj17.Width = Unit.Percentage(99);
        txtObj17.ReadOnly = ((_params[1].ToString().Equals("0") && !CheckSentStatus(d[0].ToString())) ? false : true);
        txtObj17.SkinID = d[0].ToString();
        tblCellObj17.Controls.Add(txtObj17);
        tblRowObj17.Cells.Add(tblCellObj17);

        TableRow tblRowObj18 = new TableRow();
        TableCell tblCellObj18 = new TableCell();
        tblCellObj18.Attributes.Add("class", "td03");
        tblCellObj18.Attributes.Add("align", "left");
        tblCellObj18.ColumnSpan = 2;
        Label lblAclaracionObj18 = new Label();
        lblAclaracionObj18.Text = "3) ¿Fueron éstos acordes a sus necesidades de capacitación detectadas a principio de año?";
        //lblAclaracionObj18.ForeColor = System.Drawing.Color.Red;
        tblCellObj18.Controls.Add(lblAclaracionObj18);
        tblRowObj18.Cells.Add(tblCellObj18);

        TableRow tblRowObj19 = new TableRow();
        TableCell tblCellObj19 = new TableCell();
        tblCellObj19.ColumnSpan = 2;
        TextBox txtObj19 = new TextBox();
        txtObj19.Width = Unit.Percentage(99);
        txtObj19.ReadOnly = ((_params[1].ToString().Equals("0") && !CheckSentStatus(d[0].ToString())) ? false : true);
        txtObj19.SkinID = d[0].ToString();
        tblCellObj19.Controls.Add(txtObj19);
        tblRowObj19.Cells.Add(tblCellObj19);

        tblRowColor.Visible = false;
        tbl.Rows.AddRange(new TableRow[] {  tblRowAclaracion,
                                            tblRow0,
                                            tblRow1,
                                            tblRow2,
                                            tblRow3,
                                            tblRow4,
                                            tblRowColor,
                                            //tblRowAclaracion,
                                            tblRowCursos,
                                            tblRowObj,
                                            tblRowObj0,
                                            tblRowObj1,
                                            tblRowObj2,
                                            tblRowObj3,
                                            tblRowObj4,
                                            tblRowObjT1,
                                            tblRowObj5,
                                            tblRowObj6,
                                            tblRowObj7,
                                            tblRowObj8,
                                            tblRowObj9,
                                            tblRowObj10,
                                            tblRowObj11,
                                            tblRowObj12
                                            //,
                                            //tblRowObj13,
                                            //tblRowObj14,
                                            //tblRowObj15,
                                            //tblRowObj16,
                                            //tblRowObj17,
                                            //tblRowObj18,
                                            //tblRowObj19
                                        });
        this.Controls.Add(tbl);
    }

    private void AddSeparatorIP()
    {
        Table tblSeparator = new Table();
        TableRow tblRowSeparator = new TableRow();
        TableCell tblCellSeparator = new TableCell();
        tblCellSeparator.Attributes.Add("class", "separator1");
        tblRowSeparator.Cells.Add(tblCellSeparator);
        tblSeparator.Rows.Add(tblRowSeparator);
        this.Controls.Add(tblSeparator);
        tblCellSeparator.Dispose();
        tblRowSeparator.Dispose();
    }
}
