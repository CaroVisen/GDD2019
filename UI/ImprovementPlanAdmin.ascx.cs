using System;
using System.Web.UI.WebControls;
using User;
using System.Data.SqlClient;
using Data;
using System.Data;
using System.Collections;
using System.Web.UI;

public partial class UI_ImprovementPlanAdmin : System.Web.UI.UserControl
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

        SqlDataReader myReader = SQLHelper.ExecuteReader(Cache["ApplicationDatabase"].ToString(), "[GetResultImprovementPlanAdmin]", new object[] { groupId, username, profile, filtro });

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
        int color = 0;
        Table tbl = new Table();
        tbl.Attributes.Add("width", "100%");
        tbl.BorderWidth = 1;
        tbl.CellSpacing = 1;
        tbl.CellPadding = 0;
        tbl.ID = "tbl_" + tblID.ToString();

        TableRow tblRowEvaluated = new TableRow();
        TableCell tblCellEvaluated = new TableCell();
        tblCellEvaluated.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(d[1].ToString().Trim().ToLower());
        tblCellEvaluated.ColumnSpan = 3;
        tblCellEvaluated.Attributes.Add("class", "td01");
        tblRowEvaluated.Cells.Add(tblCellEvaluated);
        tbl.Rows.Add(tblRowEvaluated);
        tblCellEvaluated.Dispose();
        tblRowEvaluated.Dispose();

        TableRow tblRow1 = new TableRow();
        TableRow tblRow2 = new TableRow();
        TableRow tblRow3 = new TableRow();
        TableRow tblRow4 = new TableRow();

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
        tblCell11.Width = 100;
        tblCell11.Text = "70%  On The Job";
        tblCell11.Attributes.Add("class", "td03");
        tblCell21.Attributes.Add("class", "td02");
        tblCell31.Attributes.Add("class", "td02");

        TextBox txt1 = new TextBox();
        txt1.Text = d[2].ToString();
        if (txt1.Text.Trim().Length > 0) { color++; };
        txt1.Attributes["class"] = "tM";
        txt1.Attributes["onblur"] = "_o(this, '" + tbl.ClientID + "')";
        txt1.Attributes["onfocus"] = "_i(this)";
        txt1.Attributes["onkeyup"] = "_k(event, '" + tbl.ClientID + "')";
        txt1.Rows = 3;
        txt1.TextMode = TextBoxMode.MultiLine;
        txt1.Width = Unit.Percentage(98);
        //txt1.ReadOnly = (d[8].ToString() == "t") ? false : true;
        txt1.ReadOnly = true;
        txt1.SkinID = d[0].ToString();
        tblCell21.Controls.Add(txt1);

        //TextBox txt11 = new TextBox();
        //txt11.Text = d[3].ToString();
        //if (txt11.Text.Trim().Length > 0) { color++; };
        //txt11.Attributes["class"] = "tM";
        //txt11.Attributes["onblur"] = "_o(this, '" + tbl.ClientID + "')";
        //txt11.Attributes["onfocus"] = "_i(this)";
        //txt11.Attributes["onkeyup"] = "_k(event, '" + tbl.ClientID + "')";
        //txt11.Rows = 3;
        //txt11.TextMode = TextBoxMode.MultiLine;
        //txt11.Width = Unit.Percentage(98);
        ////txt11.ReadOnly = (d[8].ToString() == "t") ? false : true;
        //txt11.ReadOnly = true;
        //txt11.SkinID = d[0].ToString();
        //tblCell31.Controls.Add(txt11);

        DropDownList ddl1 = new DropDownList();
        ddl1.Attributes["onchange"] = "_k(event, '" + tbl.ClientID + "')";
        ddl1.Enabled = (d[8].ToString() == "t") ? true : false;
        ddl1.Items.Add("<< Seleccione una competencia a mejorar >>");
        ddl1.Items.Add("Compañerismo");
        ddl1.Items.Add("Esfuerzo");
        ddl1.Items.Add("Prueba");
        tblCell21.Controls.Add(ddl1);

        tblCell21.Controls.Add(new LiteralControl("<br />"));

        DropDownList ddl2 = new DropDownList();
        ddl2.Attributes["onchange"] = "_k(event, '" + tbl.ClientID + "')";
        ddl2.Enabled = (d[8].ToString() == "t") ? true : false;
        ddl2.Items.Add("<< Seleccione una competencia a mejorar >>");
        ddl2.Items.Add("Compañerismo");
        ddl2.Items.Add("Esfuerzo");
        ddl2.Items.Add("Prueba");
        tblCell21.Controls.Add(ddl2);

        TableCell tblCell111 = new TableCell();
        TableCell tblCell211 = new TableCell();
        TableCell tblCell311 = new TableCell();
        tblCell111.Width = 100;
        tblCell111.Text = "Competencias Técnicas";
        tblCell111.Attributes.Add("class", "td03");
        tblCell211.Attributes.Add("class", "td02");
        tblCell311.Attributes.Add("class", "td02");
        TextBox txt2 = new TextBox();
        txt2.Text = d[4].ToString();
        if (txt2.Text.Trim().Length > 0) { color++; };
        txt2.Attributes["class"] = "tM";
        txt2.Attributes["onblur"] = "_o(this, '" + tbl.ClientID + "')";
        txt2.Attributes["onfocus"] = "_i(this)";
        txt2.Attributes["onkeyup"] = "_k(event, '" + tbl.ClientID + "')";
        txt2.Rows = 3;
        txt2.TextMode = TextBoxMode.MultiLine;
        txt2.Width = Unit.Percentage(98);
        //txt2.ReadOnly = (d[8].ToString() == "t") ? false : true;
        txt2.ReadOnly = true;
        txt2.SkinID = d[0].ToString();
        tblCell211.Controls.Add(txt2);
        TextBox txt21 = new TextBox();
        txt21.Text = d[5].ToString();
        if (txt21.Text.Trim().Length > 0) { color++; };
        txt21.Attributes["class"] = "tM";
        txt21.Attributes["onblur"] = "_o(this, '" + tbl.ClientID + "')";
        txt21.Attributes["onfocus"] = "_i(this)";
        txt21.Attributes["onkeyup"] = "_k(event, '" + tbl.ClientID + "')";
        txt21.Rows = 3;
        txt21.TextMode = TextBoxMode.MultiLine;
        txt21.Width = Unit.Percentage(98);
        //txt21.ReadOnly = (d[8].ToString() == "t") ? false : true;
        txt21.ReadOnly = true;
        txt21.SkinID = d[0].ToString();
        tblCell311.Controls.Add(txt21);

        TableCell tblCell1111 = new TableCell();
        TableCell tblCell2111 = new TableCell();
        TableCell tblCell3111 = new TableCell();
        tblCell1111.Width = 100;
        tblCell1111.Text = "Aspectos Actitudinales";
        tblCell1111.Attributes.Add("class", "td03");
        tblCell2111.Attributes.Add("class", "td02");
        tblCell3111.Attributes.Add("class", "td02");
        TextBox txt3 = new TextBox();
        txt3.Text = d[6].ToString();
        if (txt3.Text.Trim().Length > 0) { color++; };
        txt3.Attributes["class"] = "tM";
        txt3.Attributes["onblur"] = "_o(this, '" + tbl.ClientID + "')";
        txt3.Attributes["onfocus"] = "_i(this)";
        txt3.Attributes["onkeyup"] = "_k(event, '" + tbl.ClientID + "')";
        txt3.Rows = 3;
        txt3.TextMode = TextBoxMode.MultiLine;
        txt3.Width = Unit.Percentage(98);
        //txt3.ReadOnly = (d[8].ToString() == "t") ? false : true;
        txt3.ReadOnly = true;
        txt3.SkinID = d[0].ToString();
        tblCell2111.Controls.Add(txt3);
        TextBox txt31 = new TextBox();
        txt31.Text = d[7].ToString();
        if (txt31.Text.Trim().Length > 0) { color++; };
        txt31.Attributes["class"] = "tM";
        txt31.Attributes["onblur"] = "_o(this, '" + tbl.ClientID + "')";
        txt31.Attributes["onfocus"] = "_i(this)";
        txt31.Attributes["onkeyup"] = "_k(event, '" + tbl.ClientID + "')";
        txt31.Rows = 3;
        txt31.TextMode = TextBoxMode.MultiLine;
        txt31.Width = Unit.Percentage(98);
        //txt31.ReadOnly = (d[8].ToString() == "t") ? false : true;
        txt31.ReadOnly = true;
        txt31.SkinID = d[0].ToString();
        tblCell3111.Controls.Add(txt31);

        tblRow1.Cells.AddRange(new TableCell[] { tblCell1, tblCell2, tblCell3 });
        tblRow2.Cells.AddRange(new TableCell[] { tblCell11, tblCell21, tblCell31 });
        tblRow3.Cells.AddRange(new TableCell[] { tblCell111, tblCell211, tblCell311 });
        tblRow4.Cells.AddRange(new TableCell[] { tblCell1111, tblCell2111, tblCell3111 });

        TableRow tblRowColor = new TableRow();

        TableCell tblCellColor = new TableCell();
        TableCell tblCellColorL = new TableCell();
        tblCellColorL.ColumnSpan = 2;

        if (color == 6)
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

        TableRow tblRowSeguimiento = new TableRow();
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

        tbl.Rows.AddRange(new TableRow[] { tblRow1, tblRow2, tblRow3, tblRow4, tblRowColor, tblRowSeguimiento });
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
