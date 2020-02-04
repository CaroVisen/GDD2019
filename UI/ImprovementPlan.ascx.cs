using System;
using System.Web.UI.WebControls;
using User;
using System.Data.SqlClient;
using Data;
using System.Collections;
using System.Web.UI;
using System.Data;
using System.Design;

public partial class UI_ImprovementPlan : System.Web.UI.UserControl
{
    string[] _params = new string[] { };
    int tblID = 0;
    protected void Page_Load(object sender, EventArgs e)
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

        SqlDataReader myReader = SQLHelper.ExecuteReader(Cache["ApplicationDatabase"].ToString(), "[GetResultImprovementPlan]", new object[] { groupId, username, profile, filtro });

        AddSeparator();

        while (myReader.Read())
        {
            object[] values = new object[myReader.FieldCount];
            int f = myReader.GetValues(values);
            Build(values);
            tblID++;
            AddSeparator();
        }
        myReader.Close();
    }

    private void Build(object[] d)
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
        tblCellEvaluated.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(d[1].ToString().Trim().ToLower());
        tblCellEvaluated.ColumnSpan = 2;
        tblCellEvaluated.Attributes.Add("class", "td01");
        tblRowEvaluated.Cells.Add(tblCellEvaluated);
        tbl.Rows.Add(tblRowEvaluated);
        tblCellEvaluated.Dispose();
        tblRowEvaluated.Dispose();

        TableRow tblRowAclaracionCompetencias = new TableRow();
        TableCell tblCellAclaracionCompetencias = new TableCell();
        tblCellAclaracionCompetencias.Text = "Por favor, seleccionar 2 competencias que usted considera que el colaborador debería desarrollar y la forma en que podría mejorarlas.";
        tblCellAclaracionCompetencias.ForeColor = System.Drawing.Color.Red;
        tblRowAclaracionCompetencias.Cells.Add(new TableCell());
        tblRowAclaracionCompetencias.Cells.Add(tblCellAclaracionCompetencias);

        TableRow tblRow0 = new TableRow();
        TableRow tblRow1 = new TableRow();
        TableRow tblRow2 = new TableRow();
        TableRow tblRow3 = new TableRow();
        TableRow tblRow4 = new TableRow();

        TableCell tblCell01 = new TableCell();
        TableCell tblCell02 = new TableCell();

        DataTable dtCompetencies = new clsDAO().SqlCall("[GetCompetenciesImprovement] '" + d[0].ToString() + "'");

        DropDownList ddl1 = new DropDownList();
        ddl1.Attributes["onchange"] = "_k(event, '" + tbl.ClientID + "')";
        ddl1.Enabled = (d[8].ToString() == "t") ? true : false;
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
        ddl2.Attributes["onchange"] = "_k(event, '" + tbl.ClientID + "')";
        ddl2.Enabled = (d[8].ToString() == "t") ? true : false;
        ddl2.Items.Add("<< Seleccione una competencia a mejorar >>");
        foreach (DataRow dr in dtCompetencies.Rows)
        {
            ddl2.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
        }
        ddl2.SelectedValue = d[4].ToString();
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
        txt11.Text = d[3].ToString();
        tmpText = System.Text.RegularExpressions.Regex.Replace(txt11.Text, @"[^0-9a-zA-Z]+", "").Trim();
        if (tmpText.Length > 2 || tmpText.Equals("NA")) { color++; };
        txt11.Attributes["class"] = "tM";
        txt11.Attributes["onblur"] = "_o(this, '" + tbl.ClientID + "')";
        txt11.Attributes["onfocus"] = "_i(this)";
        txt11.Attributes["onkeyup"] = "_k(event, '" + tbl.ClientID + "')";
        txt11.Rows = 3;
        txt11.TextMode = TextBoxMode.MultiLine;
        txt11.Width = Unit.Percentage(99);
        txt11.ReadOnly = (d[8].ToString() == "t") ? false : true;
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
        txt21.Attributes["class"] = "tM";
        txt21.Attributes["onblur"] = "_o(this, '" + tbl.ClientID + "')";
        txt21.Attributes["onfocus"] = "_i(this)";
        txt21.Attributes["onkeyup"] = "_k(event, '" + tbl.ClientID + "')";
        txt21.Rows = 3;
        txt21.TextMode = TextBoxMode.MultiLine;
        txt21.Width = Unit.Percentage(99);
        txt21.ReadOnly = (d[8].ToString() == "t") ? false : true;
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
        txt31.Text = d[7].ToString();
        tmpText = System.Text.RegularExpressions.Regex.Replace(txt31.Text, @"[^0-9a-zA-Z]+", "").Trim();
        if (tmpText.Length > 2 || tmpText.Equals("NA")) { color++; };
        txt31.Attributes["class"] = "tM";
        txt31.Attributes["onblur"] = "_o(this, '" + tbl.ClientID + "')";
        txt31.Attributes["onfocus"] = "_i(this)";
        txt31.Attributes["onkeyup"] = "_k(event, '" + tbl.ClientID + "')";
        txt31.Rows = 3;
        txt31.TextMode = TextBoxMode.MultiLine;
        txt31.Width = Unit.Percentage(99);
        txt31.ReadOnly = (d[8].ToString() == "t") ? false : true;
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
        TableRow tblRowAclaracion = new TableRow();

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

        Label lblAclaracion = new Label();
        lblAclaracion.Text = "Para activar el botón ENVIAR tendrás que completar todos los campos de la Agenda de Desarrollo de tu colaborador.";
        lblAclaracion.ForeColor = System.Drawing.Color.Red;
        TableCell tblCellAclaracion = new TableCell();
        tblRowAclaracion.Controls.Add(tblCellAclaracion);
        tblCellAclaracion.Controls.Add(lblAclaracion);
        tblCellAclaracion.ColumnSpan = 2;

        TableRow tblRowSeguimiento = new TableRow();
        if (d[9].ToString().Equals("10") || d[9].ToString().Equals("12"))
        {
            TableCell tblCellSeguimiento = new TableCell();
            tblCellSeguimiento.ColumnSpan = 2;
            Label lblSeguimiento = new Label();
            lblSeguimiento.Text = "Añadir seguimiento de desarrollo";
            lblSeguimiento.Attributes.Add("class", "titulo");
            tblCellSeguimiento.Controls.Add(lblSeguimiento);
            TextBox txtSeguimiento = new TextBox();
            txtSeguimiento.Rows = 3;
            txtSeguimiento.TextMode = TextBoxMode.MultiLine;
            txtSeguimiento.Width = Unit.Percentage(99);
            txtSeguimiento.ReadOnly = false;
            txtSeguimiento.Attributes["class"] = "Seg";
            txtSeguimiento.Attributes["onkeyup"] = "kSeg('" + tbl.ClientID + "')";
            txtSeguimiento.SkinID = d[0].ToString();
            tblCellSeguimiento.Controls.Add(txtSeguimiento);
            DataTable dtSeguimiento = new DataTable();
            dtSeguimiento = new clsDAO().SqlCall("GetFollowupHistory " + d[0].ToString());
            if (dtSeguimiento.Rows.Count > 0)
            {
                Label lblHistorial = new Label();
                lblHistorial.Text = "Historial de seguimiento";
                lblHistorial.Attributes.Add("class", "titulo");
                tblCellSeguimiento.Controls.Add(lblHistorial);
                GridView gvSeguimiento = new GridView();
                gvSeguimiento.DataSource = dtSeguimiento;
                gvSeguimiento.DataBind();
                tblCellSeguimiento.Controls.Add(gvSeguimiento);
            }
            tblRowSeguimiento.Cells.Add(tblCellSeguimiento);
        }

        tbl.Rows.AddRange(new TableRow[] { tblRowAclaracionCompetencias, tblRow0, tblRow1, tblRow2, tblRow3, tblRow4, tblRowColor, tblRowAclaracion, tblRowCursos, tblRowSeguimiento });
        this.Controls.Add(tbl);
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
}
