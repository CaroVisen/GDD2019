using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
/// <summary>
/// Summary description for clsHTML
/// </summary>
public class clsHTML
{
    #region :: VERSION ::

    /*
     * Class version    :       0.1.6
     * Nombre y apellido:       Klein Santiago.
     * Fecha de creación:       16-11-2009
     * Fecha modificado :       17/11/2009
     * 
     * 
     * :: Funciones misc
     *      - ColorToHexString, le pasas un color y devuelve el hexadecimal
     *      - RetrieveToolTip, devuelve tooltip js
     * :: Funciones idiomas
     *      - getDayEspañol, le pasas el dia en inglés y devuelve el dia en español
     * 
     * :: Funciones utiles
     *      - SortTable, se le pasa un dt, con orderBy ej: "CAMPO1 DESC, CAMPO2 ASC"
     * 
     * :: Funciones html
     *      - public String wTableHtml(DataTable tmpDT)
     *        [ devuelve en formato HTML la tabla     ]
     * 
     *      - public String wTableHtml(DataTable tmpDT, int itemsFROM, int itemsTO)
     *        [ devuelve en formato html la dt, con el rango FROM y TO            ]
     * 
     *      - public String wTableHtml(DataTable tmpDT, int itemsFROM, int itemsTO, String orderBy)
     *        [ mersh: wTableHtml(tmpDT, itemsFROM, itemsTO) & wTableHtml(tmpDT, orderBy)         ]
     * 
     *      - public String wTableHtml(DataTable tmpDT, String orderBy)
     *        [ devuelve en formato HTML la tabla con el filtro de orden ASC o DESC ]
     * 
     *      - public String wTableHtml(DataTable tmpDT, Int16 calcula)
     *        [ calcula = 1, devuelve el total de la columna 2                           ]
     *        [ calcula = 2, devuelve el total de todas las columnas menos de la primera ]
     */

    #endregion

    #region :: FUNCIONES MISC

    #region -- Data Members --
    static char[] hexDigits = {
         '0', '1', '2', '3', '4', '5', '6', '7',
         '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};
    #endregion

    public String ColorToHexString(Color color)
    {
        byte[] bytes = new byte[3];
        bytes[0] = color.R;
        bytes[1] = color.G;
        bytes[2] = color.B;
        char[] chars = new char[bytes.Length * 2];
        for (int i = 0; i < bytes.Length; i++)
        {
            int b = bytes[i];
            chars[i * 2] = hexDigits[b >> 4];
            chars[i * 2 + 1] = hexDigits[b & 0xF];
        }
        return new string(chars);
    }

    public String RetrieveToolTip(String title, String desc)
    {
        return "onmouseout=\"UnTip()\" onmouseover=\"Tip('" + desc + "', TITLE, '" + title + "', BGCOLOR, '#ffcccc', FONTCOLOR, '#800000', FONTSIZE, '9pt', FONTFACE, 'Courier New, Courier, mono', BORDERCOLOR, '#c00000')\"";
    }

    #endregion

    #region :: FUNCIONES IDIOMAS ::

    public String getDayEspanol(String day)
    {
        switch (day)
        {
            case "Monday":
                return "Lunes";
            case "Tuesday":
                return "Martes";
            case "Wednesday":
                return "Miércoles"; 
            case "Thursday":
                return "Jueves";
            case "Friday":
                return "Viernes";
            case "Saturday":
                return "Sábado";
            case "Sunday":
                return "Domingo";
            default:
                return "¿?¿?¿?¿";
        }
    }

    #endregion

    #region :: FUNCIONES ÚTILES ::

    public DataTable SortTable(DataTable tmpDT, String orderBy)
    {
        try
        {
            DataView obj = new DataView(tmpDT);

            obj.Sort = orderBy;
            obj.ApplyDefaultSort = true;

            return obj.ToTable();
        }
        catch (Exception ex)
        {
            return new DataTable();
        }
    }
    #endregion

    #region :: FUNCIONES HTML ::

    public String wTableHtml(DataTable tmpDT)
    {
        String str = String.Empty; bool tmpColor = false; String tmpHColor = String.Empty;

        str += "<table style=\"font-family:Arial\"><tr>";
        foreach (DataColumn objDC in tmpDT.Columns)
        {
            str += "<td  style=\"font-size:x-small;background-color: #1e60aa;color:white;font-weight:bold;\">" + objDC.ColumnName + "</td>";
        }
        str += "</tr>";

        foreach (DataRow objDR in tmpDT.Rows)
        {
            tmpColor = !tmpColor; if (tmpColor) { tmpHColor = "#FFFFFF"; } else { tmpHColor = "#F2F2F2"; }

            str += "<tr style=\"font-size:xx-small;background-color:" + tmpHColor + ";\">";
            foreach (DataColumn objDC in tmpDT.Columns)
            {
                str += "<td>" + objDR[objDC.ColumnName].ToString() + "</td>";
            }
            str += "</tr>";
        }
        str += "</table>";

        return str;
    }

    public String wWriteFirstColumn(DataTable tmpDT)
    {
        String str = String.Empty; bool tmpColor = false; String tmpHColor = String.Empty;

        str += "<table width=\"100%\" style=\"font-family:Arial\">";

        

        foreach (DataRow objDR in tmpDT.Rows)
        {
            str += "<tr style=\"font-size:small;background-color:" + tmpHColor + ";\">";
            str += "<td align=left>" + objDR[0].ToString() + "<hr></td>";
            str += "</tr>";
        }

        

        str += "</table>";

        return str;
    }

    public String wTableHtmlNoColumn(DataTable tmpDT,bool valores)
    {
        String str = String.Empty; bool tmpColor = false; String tmpHColor = String.Empty;
        decimal total = 0;//tmpDT.Rows.Count;

        foreach (DataRow objDR in tmpDT.Rows)
        {
            total += decimal.Parse(objDR[1].ToString());
        }

        str += "<table width=\"100%\" style=\"font-family:Arial\">";

        str += "<tr style=\"font-size:small;background-color:" + tmpHColor + ";\">";

        foreach (DataRow objDR in tmpDT.Rows)
        {
            str += "<td align=center>" + objDR[0].ToString() + "</td>";
        }

        str += "</tr>";

        str += "<tr style=\"font-size:x-small;background-color:" + tmpHColor + ";\">";
        int contador=0;

        foreach (DataRow objDR in tmpDT.Rows)
        {
            decimal aux;

            if (total > 0)
            {
                //asdasdsad
                //float aux = float.Parse(objDR[1].ToString() * 100 / total);
                aux = (decimal.Parse(objDR[1].ToString()) * 100 / total);
            }
            else
            {
                aux = 0;
            }

            String cl = String.Empty;

            switch (contador)
            {
                case 0:
                    cl = "<a style=\"background-color:rgb(255,70,70);\">&nbsp;&nbsp;&nbsp;</a>";
                    break;
                case 1:
                    cl = "<a style=\"background-color:rgb(55,255,55);\">&nbsp;&nbsp;&nbsp;</a>";
                    break;
                case 2:
                    cl = "<a style=\"background-color:rgb(105,105,255);\">&nbsp;&nbsp;&nbsp;</a>";
                    break;
                case 3:
                    cl = "<a style=\"background-color:rgb(255,255,128);\">&nbsp;&nbsp;&nbsp;</a>";
                    break;
                case 4:
                    cl = "<a style=\"background-color:rgb(250,120,250);\">&nbsp;&nbsp;&nbsp;</a>";
                    break;
                case 5:
                    cl = "<a style=\"background-color:rgb(158,255,255);\">&nbsp;&nbsp;&nbsp;</a>";
                    break;
                case 6:
                    cl = "<a style=\"background-color:rgb(255,206,61);\">&nbsp;&nbsp;&nbsp;</a>";
                    break;
                default:
                    cl = "<a style=\"background-color:rgb(192,192,192);\">&nbsp;&nbsp;&nbsp;</a>";
                    break;
            }

            String add = (valores == true ? " - " + objDR[1].ToString() : String.Empty);
            str += "<td align=center>" + String.Format("{0:0.00}%", aux) + add + " " + cl + "  </td>";
            
            contador++;
        }
        str += "</tr>";
        str += "</table>";

        return str;
    }

    public String wTableHtml(DataTable tmpDT, int itemsFROM, int itemsTO)
    {
        String str = String.Empty; bool tmpColor = false; String tmpHColor = String.Empty;

        if (itemsTO > tmpDT.Rows.Count)
            itemsTO = tmpDT.Rows.Count;

        str += "<table style=\"font-family:Arial\"><tr>";
        foreach (DataColumn objDC in tmpDT.Columns)
        {
            str += "<td  style=\"font-size:x-small;background-color: #1e60aa;color:white;font-weight:bold;\">" + objDC.ColumnName + "</td>";
        }
        str += "</tr>";

        for (int i = itemsFROM; i < itemsTO; i++)
        {
            tmpColor = !tmpColor; if (tmpColor) { tmpHColor = "#FFFFFF"; } else { tmpHColor = "#F2F2F2"; }

            str += "<tr style=\"font-size:xx-small;background-color:" + tmpHColor + ";\">";
            for (int y = 0; y < tmpDT.Columns.Count; y++)
            {
                str += "<td>" + tmpDT.Rows[i][y].ToString() + "</td>";
            }
            str += "</tr>";
        }

        str += "</table>";

        return str;
    }

    public String wTableHtml(DataTable tmpDT, int itemsFROM, int itemsTO, String orderBy)
    {
        String str = String.Empty; bool tmpColor = false; String tmpHColor = String.Empty;

        tmpDT = SortTable(tmpDT, orderBy);

        if (itemsTO > tmpDT.Rows.Count)
            itemsTO = tmpDT.Rows.Count;

        str += "<table style=\"font-family:Arial\"><tr>";
        foreach (DataColumn objDC in tmpDT.Columns)
        {
            str += "<td  style=\"font-size:x-small;background-color: #1e60aa;color:white;font-weight:bold;\">" + objDC.ColumnName + "</td>";
        }
        str += "</tr>";

        for (int i = itemsFROM; i < itemsTO; i++)
        {
            tmpColor = !tmpColor; if (tmpColor) { tmpHColor = "#FFFFFF"; } else { tmpHColor = "#F2F2F2"; }

            str += "<tr style=\"font-size:xx-small;background-color:" + tmpHColor + ";\">";
            for (int y = 0; y < tmpDT.Columns.Count; y++)
            {
                str += "<td>" + tmpDT.Rows[i][y].ToString() + "</td>";
            }
            str += "</tr>";
        }

        str += "</table>";

        return str;
    }

    public String wTableHtml(DataTable tmpDT, String orderBy)
    {
        tmpDT = SortTable(tmpDT, orderBy);

        String str = String.Empty; bool tmpColor = false; String tmpHColor = String.Empty;

        str += "<table style=\"font-family:Arial\"><tr>";
        foreach (DataColumn objDC in tmpDT.Columns)
        {
            str += "<td  style=\"font-size:x-small;background-color: #1e60aa;color:white;font-weight:bold;\">" + objDC.ColumnName + "</td>";
        }
        str += "</tr>";

        foreach (DataRow objDR in tmpDT.Rows)
        {
            tmpColor = !tmpColor; if (tmpColor) { tmpHColor = "#FFFFFF"; } else { tmpHColor = "#F2F2F2"; }

            str += "<tr style=\"font-size:xx-small;background-color:" + tmpHColor + ";\">";
            foreach (DataColumn objDC in tmpDT.Columns)
            {
                str += "<td>" + objDR[objDC.ColumnName].ToString() + "</td>";
            }
            str += "</tr>";
        }
        str += "</table>";

        return str;
    }

    public String wTableHtml(DataTable tmpDT, Int16 calcula)
    {
        String str = String.Empty; bool tmpColor = false; String tmpHColor = String.Empty;
        Double total = 0;
        Double total2 = 0;

        if (calcula == 2 && tmpDT.Columns.Count != 3)
            calcula = 1;

        str += "<table style=\"font-family:Arial\"><tr>";
        foreach (DataColumn objDC in tmpDT.Columns)
        {
            str += "<td  style=\"font-size:x-small;background-color: #1e60aa;color:white;font-weight:bold;\">" + objDC.ColumnName + "</td>";
        }
        str += "</tr>";

        foreach (DataRow objDR in tmpDT.Rows)
        {
            tmpColor = !tmpColor; if (tmpColor) { tmpHColor = "#FFFFFF"; } else { tmpHColor = "#F2F2F2"; }

            str += "<tr style=\"font-size:xx-small;background-color:" + tmpHColor + ";\">";
            foreach (DataColumn objDC in tmpDT.Columns)
            {
                str += "<td>" + objDR[objDC.ColumnName].ToString() + "</td>";
            }
            if (calcula == 1)
                total += Convert.ToDouble(objDR[1]);
            if (calcula == 2)
            {
                total += Convert.ToDouble(objDR[1]);
                total2 += Convert.ToDouble(objDR[2]);
            }

            str += "</tr>";
        }
        if (calcula == 1)
            str += "<tr style=\"font-size:xx-small;background-color:Silver;\">" +
                "<td>Total</td>" +
                "<td>" + total + "</td>";
        if (calcula == 2)
            str += "<tr style=\"font-size:xx-small;background-color:Silver;\">" +
                "<td>Total</td>" +
                "<td>" + total + "</td>" +
                "<td>" + total2 + "</td>";

        str += "</table>";

        return str;
    }

    #endregion

}
