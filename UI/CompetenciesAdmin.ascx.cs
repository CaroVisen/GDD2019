using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using User;
using System.Data.SqlClient;
using Data;
using System.Collections;

public partial class UI_CompetenciesAdmin : System.Web.UI.UserControl
{
    bool setCompetency = false;
    ArrayList fields = new ArrayList();
    ArrayList fieldsValues = new ArrayList();
    const int cellWidth = 533;
    bool setColor = true;
    int numBlock = 0;
    string[] _params = new string[] { };

    protected void Page_Load(object sender, EventArgs e)
    {
        /*if (UserHelper.IsAdmin(Request.LogonUserIdentity.Name))
            LoadCompetenciesAdmin();
        else*/
            LoadCompetenciesAdmin();
    }

    private void LoadCompetenciesAdmin()
    {


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


            SqlDataReader myReader = SQLHelper.ExecuteReader(Cache["ApplicationDatabase"].ToString(), "GetCompetenciesAdmin", new object[] { groupId, username, profile, filtro });

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
            string[] labels = new string[] { "Evaluador", "Concurrente", "Doble Reporte", "Jefe RRHH" };
            bool _enable = false;
            bool _visible = false;
            while (myReader.Read())
            {
                AddSeparator();
                Label lbl = new Label();
                lbl.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(myReader.GetValue(1).ToString().Trim().ToLower());
                lbl.Attributes.Add("class", "titulo");
                lbl.Height = 20;
                this.Controls.Add(lbl);
                lbl.Dispose();

                Panel pnlContainer = new Panel();
                pnlContainer.Attributes.Add("class", "tab-pane");

                Literal L = new Literal();
                //L.Text = "<div style = 'position:absolute; left:655px;  top:7px;z-index:1000;'><a href='#t' class='link'>Ir a Competencias</a></div>";
                // ******************** 10 linea modificada del original
                L.Text = "<div style = 'position:absolute; left:655px;  top:-20px;z-index:1000;'><a href='#t' class='link'>Ir a Competencias</a></div>";
                pnlContainer.Controls.Add(L);
                L.Dispose();

                int _tabs = 1;




                int _status = Convert.ToInt16(myReader[5]);

                //switch (profile)
                //{
                  //  case 1:
                        if (_status == 4 && !Convert.ToBoolean(myReader[6]))
                        {
                            _tabs = 2;
                            if (_status == 11)
                                _tabs = 4;
                        }
                        if (_status == 5 && !Convert.ToBoolean(myReader[6]))
                        {
                            _tabs = 2;
                            if (_status == 11)
                                _tabs = 4;
                        }
                        else if (_status == 11)
                        {
                            _tabs = 4;
                        }
                        else if (_status == 8)
                        {
                            _tabs = 1;
                        }
                        else if (_status == 5)
                        {
                            _tabs = 1;
                        }
                        else if (_status == 12)
                        {
                            _tabs = 4;
                        }
                        else if (_status == 6 && Convert.ToBoolean(myReader[6]))
                        {
                            _tabs = 2;
                        }
                        else if (_status >= 7 && Convert.ToBoolean(myReader[6]))
                        {
                            _tabs = 3;
                        }
                        else if (_status == 9)
                        {
                            _tabs = 2;
                        }
                        if (_status == 10)
                            _tabs = 4;
                        //break;
                    // ******************* 8 lineas agregadas al original
                    //case 5:
                      //  _tabs = 4;
                        //break;
                    //default:
                      //  _tabs = profile;
                        //break;
                //}

                for (int c = 0; c < _tabs; c++)
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
                    txtA.Attributes["onBlur"] = "o(this)";
                    txtA.Attributes["onFocus"] = "_i(this)";
                    txtA.Attributes["onkeyup"] = "kIP(this)";

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
                    radioA.Enabled = false;

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
                    radioR.Enabled = false;
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
                            txtA.ReadOnly = !(("1.2.3.4.5.6.7.8.9.10.11.12".IndexOf(_status.ToString()) >= 0) && (c == 0));
                            txtR.ReadOnly = !(("1.2.3.4.5.6.7.8.9.10.11.12".IndexOf(_status.ToString()) >= 0) && (c == 0));

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

                    
                    txtA.ReadOnly = true;
                    txtR.ReadOnly = true;
                    

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


        SqlDataReader myReader = SQLHelper.ExecuteReader(Cache["ApplicationDatabase"].ToString(), "[GetCompetencies]", new object[] { groupId, username, profile, filtro });

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
        string[] labels = new string[] { "Evaluador", "Concurrente", "Doble Reporte", "Jefe RRHH" };
        bool _enable = false;
        bool _visible = false;
        while (myReader.Read())
        {
            AddSeparator();
            Label lbl = new Label();
            lbl.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(myReader.GetValue(1).ToString().Trim().ToLower());
            lbl.Attributes.Add("class", "titulo");
            lbl.Height = 20;
            this.Controls.Add(lbl);
            lbl.Dispose();

            Panel pnlContainer = new Panel();
            pnlContainer.Attributes.Add("class", "tab-pane");

            Literal L = new Literal();
            //L.Text = "<div style = 'position:absolute; left:655px;  top:7px;z-index:1000;'><a href='#t' class='link'>Ir a Competencias</a></div>";
            // ******************** 10 linea modificada del original
            L.Text = "<div style = 'position:absolute; left:655px;  top:-20px;z-index:1000;'><a href='#t' class='link'>Ir a Competencias</a></div>";
            pnlContainer.Controls.Add(L);
            L.Dispose();

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

            switch (profile)
            {
                case 1:
                    if (_status == 4 && !Convert.ToBoolean(myReader[6]))
                    {
                        _tabs = 2;
                        if (_status == 11)
                            _tabs = 4;
                    }
                    if (_status == 5 && !Convert.ToBoolean(myReader[6]))
                    {
                        _tabs = 2;
                        if (_status == 11)
                            _tabs = 4;
                    }
                    else if (_status == 11)
                    {
                        _tabs = 4;
                    }
                    else if (_status == 12)
                    {
                        _tabs = 4;
                    }
                    else if (_status == 6 && Convert.ToBoolean(myReader[6]))
                    {
                        _tabs = 2;
                    }
                    else if (_status >= 7 && Convert.ToBoolean(myReader[6]))
                    {
                        _tabs = 3;
                    }
                    else if (_status == 9)
                    {
                        _tabs = 2;
                    }
                    if (_status == 10)
                        _tabs = 4;
                    break;
                // ******************* 8 lineas agregadas al original
                case 5:
                    _tabs = 4;
                    break;
                default:
                    _tabs = profile;
                    break;
            }

            for (int c = 0; c < _tabs; c++)
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
                txtA.Attributes["onBlur"] = "o(this)";
                txtA.Attributes["onFocus"] = "_i(this)";
                txtA.Attributes["onkeyup"] = "kIP(this)";

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

        //tblCellRight.Enabled = false;
        tblCellLeft.VerticalAlign = VerticalAlign.Top;
        tblCellLeft.HorizontalAlign = HorizontalAlign.Left;
        tblCellLeft.Controls.Add(pnlLeft);
        tblCellRight.Controls.Add(pnlRight);
        tblRow.Cells.AddRange(new TableCell[] { tblCellLeft, tblCellRight });


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
        tblCell.Width = cellWidth;
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
            tblCell1.Width = cellWidth;
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
                radio.Enabled = radio.Checked || (fieldsValues[c].ToString().Split('_')[2].ToLower() == "t");
                radio.Enabled = false;
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



        if (color == fields.Count - 4)
            tblR.Attributes["onclick"] = "c(this,false)";
        else
            tblR.Attributes["onclick"] = "c(this,true)";

        tblL.Rows.AddAt(0, tblRow);
        tblRow.Dispose();


        tblCellColorL.Attributes.Add("class", _style);
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
            tblRowE.Cells.AddRange(new TableCell[] { tblCellE, tblCellE1 });
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
    }
}
