using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;
using Data;
using User;


public partial class UI_ManagementIndicators : System.Web.UI.UserControl, IManagementIndicators
{
    string[] _params = new string[] { };
    bool group8 = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!IsPostBack)
        //{
            
        //}
        LoadManagementIndicators();
    }


    private void LoadManagementIndicators()
    {
        string username = UserHelper.GetUserId(Request.LogonUserIdentity.Name);
        string oldGroup = string.Empty;
        string group = string.Empty;
        string letter = string.Empty;
        string fullName = string.Empty;
        int tblNum = 0;

        _params = Encryption.Decrypt(Request.Params["."]).Split('.');

        int groupId = Convert.ToInt16(_params[0]);
        int profile = Convert.ToInt16(_params[1]);

        if (profile > 1)
        {
            username = _params[2];
        }

		// Si se evalua a jefes tiene que aparecer el mensaje que aclara que no se completan los indicadores
		Tabla_Mensajes_Jefes.Visible = ( groupId == 8 );
        MImsg.Visible = (groupId != 8);
		string filtro = "%";

		if( _params.Length > 3 )
			filtro = SQLHelper.ExecuteDataset( Cache["ApplicationDatabase"].ToString( ) , "GetEvaluatedByEvaluated" , new object[] { _params[3] } ).Tables[0].Rows[0].ItemArray[0].ToString();

		SqlDataReader myReader = SQLHelper.ExecuteReader( Cache["ApplicationDatabase"].ToString( ) , "[GetManagementIndicators]" , new object[] { groupId , username , profile , filtro } );
        IList<ManagementIndicatorsDetail> details = new List<ManagementIndicatorsDetail>();

        while (myReader.Read())
        {
            group = myReader.GetValue(0).ToString();

            if (group != string.Empty && oldGroup != string.Empty && group != oldGroup)
            {
                AddControl(letter, fullName, details, tblNum);
                letter = string.Empty;
                tblNum += 1;
            }

            ManagementIndicatorsDetail d = new ManagementIndicatorsDetail();
            d.FullName = myReader[0].ToString();
            d.Description = myReader[1].ToString();
            d.Weight = (decimal)myReader[2];
            d.Unit = myReader[3].ToString();
            d.Result = (string)myReader[4];
            d.Group = myReader[5].ToString();
            d.Status = myReader[6].ToString();
            d.Code = (decimal)myReader[7];
            d.NetworkAssessmentId= (decimal)myReader[8];
            d.ResultVariableId = (decimal)myReader[9];
            d.Letter = myReader[10].ToString();
            if (d.Group.Equals("Grupo H"))
            {
                d.Result = "100";
                group8 = true;
            }
            details.Add(d);
            
            fullName = d.FullName;
            if (d.Letter.ToString() !=string.Empty)
            {
                letter = d.Letter.ToString();
            }

            oldGroup = group;
        }
        myReader.Close();
        AddControl(letter, fullName, details, tblNum);
    }

    private void AddControl(string letter, string fullName, IList<ManagementIndicatorsDetail> details, int tblNum)
    {
        Table tbl = new Table();
        tbl.Attributes.Add("width", "100%");
        tbl.BorderWidth = 0;
        tbl.CellSpacing = 1;
        tbl.CellPadding = 0;
        tbl.ID = "TBL" + tblNum;
        Build(details, tbl, fullName, letter);
        this.Controls.Add(tbl);

        details.Clear();
        tbl.Dispose();
    }

    public void Build(IList<ManagementIndicatorsDetail> details, Table tabla, string fullName, string letter)
    {
        //cabecera
        TableRow tblRowEvaluated = new TableRow();
        TableCell tblCellEvaluated = new TableCell();
        TableRow tblRowHeader = new TableRow();
        TableCell tblCell1 = new TableCell();
        TableCell tblCell2 = new TableCell();
        TableCell tblCell3 = new TableCell();
        TableCell tblCell4 = new TableCell();
        tblCellEvaluated.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(fullName.Trim().ToLower());
        tblCellEvaluated.ColumnSpan = 4;
        tblCellEvaluated.Attributes.Add("class", "td01");
        tblRowEvaluated.Cells.Add(tblCellEvaluated);
        tabla.Rows.Add(tblRowEvaluated);
        tblCellEvaluated.Dispose();
        tblRowEvaluated.Dispose();

        tblRowHeader.Attributes.Add("class", "td03");

        tblCell1.Attributes.Add("width", "68%");
        tblCell1.Text = "Descripción";
        tblCell2.Text = "Ponderación 100%";
        tblCell3.Text = "Unidad de Medida";
        tblCell4.Text = "Resultado Logrado %";

        tblRowHeader.Cells.AddRange(new TableCell[] { tblCell1, tblCell2, tblCell3, tblCell4 });
        tabla.Rows.Add(tblRowHeader);

        tblCell1.Dispose();
        tblCell2.Dispose();
        tblCell3.Dispose();
        tblCell4.Dispose();
        tblRowHeader.Dispose();
        //fin cabecera

        //Filas comienzo  
        int rowId = 0;
        foreach (ManagementIndicatorsDetail d in details)
        {
            TableRow tblRow = new TableRow();
            for (int i = 1; i < 5; i++)
            {
                bool resultOnly = !(d.Code == 0);                
                TableCell tblCell = new TableCell();
                switch (i)
                {
                    case 1:
                        if (d.Status == "true" && d.Code == 0)
                        {
                            TextBox txtDescription = new TextBox();                            
                            txtDescription.Text = d.Properties[i].ToString();
                            txtDescription.Attributes["firstLine"] = (d == details[0]).ToString();
                            txtDescription.Attributes["class"] = "text1 d";
                            txtDescription.Attributes["onkeypress"] = "k(event)";
                            txtDescription.Attributes["onpaste"] = "k(event)";
                            txtDescription.Attributes["onblur"] = "b(this)";
                            txtDescription.Attributes["onfocus"] = "f(this)";                            
                            HttpResponse myHttpReponse = Response;
                            HtmlTextWriter myHtmlTextWriter = new HtmlTextWriter(myHttpReponse.Output);
                            txtDescription.Attributes.AddAttributes(myHtmlTextWriter);
                            tblCell.Attributes.Add("class", "td02r");
                            txtDescription.ID = "";
                            txtDescription.Width = Unit.Percentage(98);
                            txtDescription.Height = 18;
                            txtDescription.SkinID = i.ToString() + "_" + d.NetworkAssessmentId.ToString() + "_" + d.ResultVariableId.ToString();
                            tblCell.Controls.Add(txtDescription);
                            txtDescription.Dispose();
                        }
                        else
                        {
                            tblCell.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(d.Properties[i].ToString().Trim().ToLower());
                            tblCell.Attributes.Add("class", "td02");
                        };
                        break;
                    case 2:
                        if (d.Status == "true" && d.Code == 0)
                        {
                            TextBox txtWeight = new TextBox();
                            txtWeight.Text = (d.Properties[i].ToString() == "0") ? string.Empty: d.Properties[i].ToString();                            
                            txtWeight.Attributes["class"] = "textbox w";
                            txtWeight.Attributes["firstLine"] = (d == details[0]).ToString();
                            txtWeight.Attributes["onkeypress"] = "k(event)";
                            txtWeight.Attributes["onblur"] = "b(this)";
                            txtWeight.Attributes["onfocus"] = "f(this)";
                            HttpResponse myHttpReponse = Response;
                            HtmlTextWriter myHtmlTextWriter = new HtmlTextWriter(myHttpReponse.Output);
                            txtWeight.Attributes.AddAttributes(myHtmlTextWriter);
                            tblCell.Attributes.Add("class", "td02r");
                            txtWeight.ID = "";
                            txtWeight.Width = Unit.Percentage(95);
                            txtWeight.Height = 18;
                            txtWeight.SkinID = i.ToString() + "_" + d.NetworkAssessmentId.ToString() + "_" + d.ResultVariableId.ToString();

                            tblCell.Controls.Add(txtWeight);
                            txtWeight.Dispose();
                        }
                        else
                        {
                            tblCell.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(d.Properties[i].ToString().Trim().ToLower());
                            tblCell.Attributes.Add("class", "tdr");
                        };
                        break;
                    case 4:
                        if (d.Status == "true")
                        {
                            TextBox txtValue = new TextBox();
                            txtValue.Text = (d.Properties[i].ToString() == "0") ? string.Empty : d.Properties[i].ToString();
                            txtValue.Attributes["firstLine"] = (d == details[0]).ToString();
                            txtValue.Attributes["resultOnly"] = resultOnly.ToString();
                            txtValue.Attributes["class"] = "textbox v";
                            txtValue.Attributes["onkeypress"] = "return k(event)";
                            txtValue.Attributes["onblur"]="b(this)";
                            txtValue.Attributes["onfocus"]= "f(this)";
                            HttpResponse myHttpReponse = Response;
                            HtmlTextWriter myHtmlTextWriter = new HtmlTextWriter(myHttpReponse.Output);
                            txtValue.Attributes.AddAttributes(myHtmlTextWriter);
                            tblCell.Attributes.Add("class", "td02r");
                            txtValue.ID = "";
                            txtValue.Width = Unit.Percentage(95);
                            txtValue.Height = 18;
                            txtValue.SkinID = i.ToString() + "_" + d.NetworkAssessmentId.ToString() + "_" + d.ResultVariableId.ToString();
                            tblCell.Controls.Add(txtValue);
                            txtValue.Dispose();
                        }
                        else
                        {
                            tblCell.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(d.Properties[i].ToString().Trim().ToLower());
                            tblCell.Attributes.Add("class", "tdr");

                            if (group8)
                            {
                                tblCell.BackColor = System.Drawing.Color.Gray;
                                tblCell.ForeColor = System.Drawing.Color.Gray;
                            }
                        };
                        break;

                    default:
                        tblCell.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(d.Properties[i].ToString().Trim().ToLower());
                        tblCell.Attributes.Add("class", "td02c");
                        break;
                }



                tblRow.Cells.Add(tblCell);
                tblCell.Dispose();

            }

            tabla.Rows.Add(tblRow);
            tblRow.Dispose();
            rowId++;
        }

        //Fin Filas


        //arma pie y el separador/
        TableRow tblRowFoot = new TableRow();
        TableCell tblCellFoot1 = new TableCell();
        TableCell tblCellFoot2 = new TableCell();
        HiddenField txtLetter = new HiddenField();
        
        tblCellFoot1.Text = "Resultado obtenido:";
        tblCellFoot1.Attributes.Add("class", "textor");
        tblCellFoot1.ColumnSpan = 3;
        tblCellFoot2.Text = letter.Split('_')[0];
        if (group8)
        {
            tblCellFoot2.Text = "D";
            tblCellFoot2.BackColor = System.Drawing.Color.Gray;
            tblCellFoot2.ForeColor = System.Drawing.Color.Gray;
        }   
        txtLetter.Value=letter;
        txtLetter.ID = tabla.ID + "_l";
        this.Controls.Add(txtLetter);
        tblCellFoot2.Attributes.Add("class", "td03");
        tblRowFoot.Cells.AddRange(new TableCell[] { tblCellFoot1, tblCellFoot2 });
        tabla.Rows.Add(tblRowFoot);
        tblCellFoot1.Dispose();
        tblCellFoot2.Dispose();
        txtLetter.Dispose();
        tblRowFoot.Dispose();

        TableRow tblRowSeparator = new TableRow();
        TableCell tblCellSeparator = new TableCell();
        tblCellSeparator.Text = "&nbsp;";
        tblRowSeparator.Cells.Add(tblCellSeparator);
        tabla.Rows.Add(tblRowSeparator);
        tblCellSeparator.Dispose();
        tblRowSeparator.Dispose();
        //fin arma pie
    }
}
