using System;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Data;
using User;
using System.Web.UI;
using System.Data;

public partial class UI_PeopleAssess : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string parameter = Request["__EVENTARGUMENT"];
        if (parameter != null)
        {
            SetStatus(parameter); System.Threading.Thread.Sleep(1300);
        };

        Build();
    }

    private void SetStatus(string parameter)
    {
        SQLHelper.ExecuteNonQuery(Cache["ApplicationDatabase"].ToString(), "SetStatus", new object[] { parameter, 12 });
    }

    public void Build()
    {
        string username = UserHelper.GetUserId(Request.LogonUserIdentity.Name);
        string group = string.Empty;
        string name = string.Empty;
        bool pendings = false;

        SqlDataReader myReader = SQLHelper.ExecuteReader(Cache["ApplicationDatabase"].ToString(), "GetEvaluatedPeople", new object[] { username });

        #region SelfEvaluation
        DataTable dt = new DataTable();
        clsDAO objDAO = new clsDAO();
        dt = objDAO.SqlCall("GetSelfEvaluationAllowance '" + username + "'");
        if (dt.Rows.Count > 0)
        {
            TblSelfEvaluation.Visible = true;

            TableRow tblRowGroup = new TableRow();
            TableCell tblCellGroup = new TableCell();
            TableCell tblCellButton = new TableCell();
            Button button = new Button();

            button.Text = "Autoevaluación";
            button.Attributes.Add("class", "btn");
            button.PostBackUrl = Server.HtmlEncode("~/EvaluationFormSelf.aspx?.=" + Server.UrlEncode(Encryption.Encrypt(dt.Rows[0][2].ToString() + ".0.0." + dt.Rows[0][0].ToString())));
            tblCellGroup.Text = dt.Rows[0][1].ToString();
            tblCellGroup.ColumnSpan = 4;
            tblCellGroup.Attributes.Add("class", "td06");
            tblRowGroup.Cells.Add(tblCellGroup);
            tblRowGroup.Cells.Add(tblCellButton);
            tblCellButton.Controls.Add(button);
            tblCellButton.Attributes.Add("class", "td06");
            tblCellButton.ColumnSpan = 2;
            TblSelfEvaluation.Rows.Add(tblRowGroup);
            tblCellGroup.Dispose();
            tblCellButton.Dispose();
            tblRowGroup.Dispose();
            button.Dispose();
        }
        #endregion

        #region Evaluator
        int c = 0;
        Tbl1.Visible = (myReader.HasRows);

        while (myReader.Read())
        {
            pendings = true;
            if (group != myReader.GetValue(0).ToString())
            {
                TableRow tblRowGroup = new TableRow();
                TableCell tblCellGroup = new TableCell();
                TableCell tblCellButton = new TableCell();
                Button button = new Button();

                bool _status = ("1,2,5,8,11".IndexOf(myReader[8].ToString(), 0) >= 0);

                DataSet myReaderEnabled = SQLHelper.ExecuteDataset(Cache["ApplicationDatabase"].ToString(), "GetAssessmentAccesibility", new object[] { username });

                if (myReaderEnabled.Tables[0].Rows[0].ItemArray[0].ToString() == "1" || myReaderEnabled.Tables[0].Rows[0].ItemArray[0].ToString() == "5")
                {
                    button.Visible = true;
                }
                else
                {
                    button.Visible = false;

                }

                button.Text = (_status) ? "Evaluar" : "Visualizar";
                button.ToolTip = ((_status) ? "Evalua" : "Visualiza") + " las personas del " + myReader.GetValue(0).ToString();
                button.Attributes.Add("class", "btn");
                button.PostBackUrl = Server.HtmlEncode("~/EvaluationForm.aspx?.=" + Server.UrlEncode(Encryption.Encrypt(myReader.GetValue(6).ToString() + ".1." + myReader[8].ToString())));
                tblCellGroup.Text = myReader.GetValue(0).ToString();
                tblCellGroup.ColumnSpan = 4;
                tblCellGroup.Attributes.Add("class", "td06");
                tblRowGroup.Cells.Add(tblCellGroup);
                tblRowGroup.Cells.Add(tblCellButton);
                tblCellButton.Controls.Add(button);
                tblCellButton.Attributes.Add("class", "td06");
                tblCellButton.ColumnSpan = 2;
                Tbl1.Rows.Add(tblRowGroup);
                tblCellGroup.Dispose();
                tblCellButton.Dispose();
                tblRowGroup.Dispose();
                button.Dispose();
            }

            TableRow tblRow = new TableRow();


            TableCell tblCellImg = new TableCell();
            ImageButton img = new ImageButton();

            //if (((decimal)myReader[7] != 10))
            //{
            img.ID = c.ToString();
            img.ImageUrl = "~/App_Images/search.jpg";
            img.ToolTip = "Ver solo esta evaluación";
            c++;
            img.PostBackUrl = Server.HtmlEncode("~/EvaluationForm.aspx?.=" + Server.UrlEncode(Encryption.Encrypt(myReader[6].ToString() + ".1." + myReader[7].ToString() + "." + myReader[9].ToString())));
            tblCellImg.Width = 16;
            tblCellImg.Attributes.Add("class", "td02");
            tblCellImg.Controls.Add(img);
            img.Dispose();

            if ((decimal)myReader[7] == 10 || (decimal)myReader[7] == 12)
            {
                img = new ImageButton();
                img.ID = c.ToString();
                c++;
                img.ImageUrl = "~/App_Images/imp.gif";
                img.OnClientClick = "p(" + myReader[9].ToString() + ")";
                img.ToolTip = "Imprimir Evaluación";
                tblCellImg.Controls.Add(img);
                img.Dispose();
                tblCellImg.Width = 38;
            }

            tblRow.Cells.Add(tblCellImg);
            tblCellImg.Dispose();
            //}
            //else
            //{

            //    img.ID = c.ToString();
            //    img.ImageUrl = "~/App_Images/imp.gif";
            //    img.OnClientClick = "p(" + myReader[9].ToString() + ")";
            //    img.ToolTip = "Imprimir Evaluación";
            //    c++;
            //    tblCellImg.Width = 16;
            //    tblCellImg.Attributes.Add("class", "td02");
            //    tblCellImg.Controls.Add(img);
            //    img.Dispose();
            //    tblRow.Cells.Add(tblCellImg);
            //    tblCellImg.Dispose();
            //}

            for (int i = 1; i < 6; i++)
            {
                TableCell tblCell = new TableCell();
                switch (i)
                {
                    case 3:
                        tblCell.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase((i == 5) ? "" : myReader.GetValue(i).ToString().Trim().ToLower());
                        tblCell.Attributes.Add("class", "td02c");
                        break;
                    case 4:
                        tblCell.Text = myReader.GetValue(i).ToString();
                        tblCell.Attributes.Add("class", "td02");
                        break;
                    case 5:
                        tblCell.Attributes.Add("class", (i == 5) ? myReader.GetValue(i).ToString() : "td02");
                        break;
                    default:
                        tblCell.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase((i == 5) ? "" : myReader.GetValue(i).ToString().Trim().ToLower());
                        tblCell.Attributes.Add("class", "td02");
                        break;
                }

                if (i == 1 && ((decimal)myReader[7] != 10))
                {
                    // tblCell.ColumnSpan = 2;
                }
                tblRow.Cells.Add(tblCell);
                tblCell.Dispose();
            }
            Tbl1.Rows.Add(tblRow);
            tblRow.Dispose();

            group = myReader.GetValue(0).ToString();
        }
        if (Tbl1.Visible) { AddSeparator(Tbl1); };
        #endregion

        #region Concurrent
        myReader.NextResult();
        Tbl2.Visible = (myReader.HasRows);
        name = string.Empty;
        group = string.Empty;
        while (myReader.Read())
        {
            pendings = true;
            if (name != myReader.GetValue(0).ToString())
            {
                TableRow tblRowName = new TableRow();
                TableCell tblCellName = new TableCell();
                tblCellName.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase("Evaluador: " + myReader.GetValue(0).ToString().Trim().ToLower());
                tblCellName.ColumnSpan = 6;
                tblCellName.Attributes.Add("class", "td06L");
                tblRowName.Cells.Add(tblCellName);
                Tbl2.Rows.Add(tblRowName);
                tblCellName.Dispose();
                tblRowName.Dispose();
                group = string.Empty;
            }

            if (group != myReader.GetValue(1).ToString())
            {
                TableRow tblRowGroup = new TableRow();
                TableCell tblCellGroup = new TableCell();
                TableCell tblCellButton = new TableCell();
                Button button = new Button();
                button.Text = "Aprobar/Rechazar";
                button.ToolTip = "Aprueba/Rechaza personas del " + myReader.GetValue(1).ToString();
                button.Attributes.Add("class", "btn");
                button.PostBackUrl = Server.HtmlEncode("~/EvaluationForm.aspx?.=" + Server.UrlEncode(Encryption.Encrypt(myReader.GetValue(7).ToString() + ".2." + myReader[9].ToString())));
                tblCellGroup.Text = myReader.GetValue(1).ToString();
                tblCellGroup.ColumnSpan = 4;
                tblCellGroup.Attributes.Add("class", "td06");
                tblRowGroup.Cells.Add(tblCellGroup);
                tblRowGroup.Cells.Add(tblCellButton);
                tblCellButton.Controls.Add(button);
                tblCellButton.Attributes.Add("class", "td06");
                tblCellButton.ColumnSpan = 2;
                Tbl2.Rows.Add(tblRowGroup);
                tblCellGroup.Dispose();
                tblCellButton.Dispose();
                tblRowGroup.Dispose();
                button.Dispose();
            }

            TableRow tblRow = new TableRow();
            for (int i = 2; i < 7; i++)
            {
                TableCell tblCell = new TableCell();

                switch (i)
                {
                    case 4:
                        tblCell.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase((i == 6) ? "" : myReader.GetValue(i).ToString().Trim().ToLower());
                        tblCell.Attributes.Add("class", "td02c");
                        break;
                    case 5:
                        tblCell.Text = myReader.GetValue(i).ToString();
                        tblCell.Attributes.Add("class", "td02");
                        break;
                    case 6:
                        tblCell.Attributes.Add("class", (i == 6) ? myReader.GetValue(i).ToString() : "td02");
                        break;
                    default:
                        tblCell.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase((i == 6) ? "" : myReader.GetValue(i).ToString().Trim().ToLower());
                        tblCell.Attributes.Add("class", "td02");
                        break;
                }
                if (i == 2)
                {
                    tblCell.ColumnSpan = 2;
                }
                tblRow.Cells.Add(tblCell);
                tblCell.Dispose();
            }
            Tbl2.Rows.Add(tblRow);
            tblRow.Dispose();

            name = myReader.GetValue(0).ToString();
            group = myReader.GetValue(1).ToString();

        }
        if (Tbl2.Visible) { AddSeparator(Tbl2); };
        #endregion

        #region Double Report
        myReader.NextResult();
        Tbl3.Visible = (myReader.HasRows);
        name = string.Empty;
        group = string.Empty;
        while (myReader.Read())
        {
            pendings = true;
            if (name != myReader.GetValue(0).ToString())
            {
                TableRow tblRowName = new TableRow();
                TableCell tblCellName = new TableCell();
                tblCellName.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase("Evaluador: " + myReader.GetValue(0).ToString().Trim().ToLower());
                tblCellName.ColumnSpan = 6;
                tblCellName.Attributes.Add("class", "td06L");
                tblRowName.Cells.Add(tblCellName);
                Tbl3.Rows.Add(tblRowName);
                tblCellName.Dispose();
                tblRowName.Dispose();
                group = string.Empty;
            }

            if (group != myReader.GetValue(1).ToString())
            {
                TableRow tblRowGroup = new TableRow();
                TableCell tblCellGroup = new TableCell();
                TableCell tblCellButton = new TableCell();
                Button button = new Button();
                button.Text = "Aprobar/Rechazar";
                button.ToolTip = "Aprueba/Rechaza personas del " + myReader.GetValue(1).ToString();
                button.Attributes.Add("class", "btn");
                button.PostBackUrl = Server.HtmlEncode("~/EvaluationForm.aspx?.=" + Server.UrlEncode(Encryption.Encrypt(myReader.GetValue(7).ToString() + ".3." + myReader[9].ToString())));
                tblCellGroup.Text = myReader.GetValue(1).ToString();
                tblCellGroup.ColumnSpan = 4;
                tblCellGroup.Attributes.Add("class", "td06");
                tblRowGroup.Cells.Add(tblCellGroup);
                tblRowGroup.Cells.Add(tblCellButton);
                tblCellButton.Controls.Add(button);
                tblCellButton.Attributes.Add("class", "td06");
                tblCellButton.ColumnSpan = 2;
                Tbl3.Rows.Add(tblRowGroup);
                tblCellGroup.Dispose();
                tblCellButton.Dispose();
                tblRowGroup.Dispose();
                button.Dispose();
            }

            TableRow tblRow = new TableRow();
            for (int i = 2; i < 7; i++)
            {
                TableCell tblCell = new TableCell();
                switch (i)
                {
                    case 4:
                        tblCell.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase((i == 6) ? "" : myReader.GetValue(i).ToString().Trim().ToLower());
                        tblCell.Attributes.Add("class", "td02c");
                        break;
                    case 5:
                        tblCell.Text = myReader.GetValue(i).ToString();
                        tblCell.Attributes.Add("class", "td02");
                        break;
                    case 6:
                        tblCell.Attributes.Add("class", (i == 6) ? myReader.GetValue(i).ToString() : "td02");
                        break;
                    default:
                        tblCell.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase((i == 6) ? "" : myReader.GetValue(i).ToString().Trim().ToLower());
                        tblCell.Attributes.Add("class", "td02");
                        break;
                }
                if (i == 2)
                {
                    tblCell.ColumnSpan = 2;
                }
                tblRow.Cells.Add(tblCell);
                tblCell.Dispose();
            }
            Tbl3.Rows.Add(tblRow);
            tblRow.Dispose();

            name = myReader.GetValue(0).ToString();
            group = myReader.GetValue(1).ToString();

        }
        if (Tbl3.Visible) { AddSeparator(Tbl3); };
        #endregion

        #region HHRR
        myReader.NextResult();
        Tbl4.Visible = (myReader.HasRows);
        name = string.Empty;
        group = string.Empty;
        while (myReader.Read())
        {
            pendings = true;
            if (name != myReader.GetValue(0).ToString())
            {
                TableRow tblRowName = new TableRow();
                TableCell tblCellName = new TableCell();
                tblCellName.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase("Evaluador: " + myReader.GetValue(0).ToString().Trim().ToLower());
                tblCellName.ColumnSpan = 6;
                tblCellName.Attributes.Add("class", "td06L");
                tblRowName.Cells.Add(tblCellName);
                Tbl4.Rows.Add(tblRowName);
                tblCellName.Dispose();
                tblRowName.Dispose();
                group = string.Empty;
            }

            if (group != myReader.GetValue(1).ToString())
            {
                TableRow tblRowGroup = new TableRow();
                TableCell tblCellGroup = new TableCell();
                TableCell tblCellButton = new TableCell();
                Button button = new Button();
                button.Text = "Aprobar/Rechazar";
                button.ToolTip = "Aprueba/Rechaza personas del " + myReader.GetValue(1).ToString();
                button.Attributes.Add("class", "btn");
                button.PostBackUrl = Server.HtmlEncode("~/EvaluationForm.aspx?.=" + Server.UrlEncode(Encryption.Encrypt(myReader.GetValue(7).ToString() + ".5." + myReader[9].ToString())));
                tblCellGroup.Text = myReader.GetValue(1).ToString();
                tblCellGroup.ColumnSpan = 4;
                tblCellGroup.Attributes.Add("class", "td06");
                tblRowGroup.Cells.Add(tblCellGroup);
                tblRowGroup.Cells.Add(tblCellButton);
                tblCellButton.Controls.Add(button);
                tblCellButton.Attributes.Add("class", "td06");
                tblCellButton.ColumnSpan = 2;
                Tbl4.Rows.Add(tblRowGroup);
                tblCellGroup.Dispose();
                tblCellButton.Dispose();
                tblRowGroup.Dispose();
                button.Dispose();
            }

            TableRow tblRow = new TableRow();
            for (int i = 2; i < 7; i++)
            {
                TableCell tblCell = new TableCell();
                switch (i)
                {
                    case 4:
                        tblCell.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase((i == 6) ? "" : myReader.GetValue(i).ToString().Trim().ToLower());
                        tblCell.Attributes.Add("class", "td02c");
                        break;
                    case 5:
                        tblCell.Text = myReader.GetValue(i).ToString();
                        tblCell.Attributes.Add("class", "td02");
                        break;
                    case 6:
                        tblCell.Attributes.Add("class", (i == 6) ? myReader.GetValue(i).ToString() : "td02");
                        break;
                    default:
                        tblCell.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase((i == 6) ? "" : myReader.GetValue(i).ToString().Trim().ToLower());
                        tblCell.Attributes.Add("class", "td02");
                        break;
                }
                if (i == 2)
                {
                    tblCell.ColumnSpan = 2;
                }
                tblRow.Cells.Add(tblCell);
                tblCell.Dispose();
            }
            Tbl4.Rows.Add(tblRow);
            tblRow.Dispose();

            name = myReader.GetValue(0).ToString();
            group = myReader.GetValue(1).ToString();
        }
        if (Tbl4.Visible) { AddSeparator(Tbl4); };
        #endregion

        myReader.Close();

        QD.Visible = pendings;

        if (!pendings)
        {
            NB.Text = "Usted no posee evaluaciones pendientes.";
            NB.Visible = true;
        }

    }

    private void AddSeparator(Table tblSeparator)
    {
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

