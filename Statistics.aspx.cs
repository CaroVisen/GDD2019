using System;
using System.Drawing;
using Infragistics.UltraChart.Resources.Appearance;
using System.Data.SqlClient;
using Data;
using Infragistics.Excel;
using System.Web;
using System.Data;

public partial class Statistics : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Refresh();
    }

    void Refresh()
    {

        int complete = 0;
        int incomplete = 0;
        SqlDataReader myReader = SQLHelper.ExecuteReader(Cache["ApplicationDatabase"].ToString(), "GetStatistics");
        while (myReader.Read())
        {
            complete = (int)myReader.GetValue(0);
            incomplete = (int)myReader.GetValue(1);
        }
        
        
        NumericSeries serie = new NumericSeries();

        serie.Points.Add(
            new NumericDataPoint(
                complete,
                "Completo",
                false)
            );

        serie.Points.Add(
            new NumericDataPoint(
                incomplete,
                "Imcompleto",
                false)
            );

        serie.PEs.Add(new PaintElement(Color.Green));
        serie.PEs.Add(new PaintElement(Color.DarkRed));

        UltraChart1.CompositeChart.Series.Add(serie);

    }


    protected void Button1_Click(object sender, EventArgs e)
    {
    
    }
    
    protected void Button2_Click(object sender, EventArgs e)
    {
        DataSet myReader = SQLHelper.ExecuteDataset(Cache["ApplicationDatabase"].ToString(), "GetPendingSurveyPerUser");
        Workbook theWorkBook = ExcelHelper.ExcelExporter.DataSetToExcel(myReader);
        ExcelHelper.ExcelExporter.WriteToResponse(theWorkBook, "Evaluaciones_pendientes.xls", Page.Response);
    }
}

namespace ExcelHelper
{

    public class ExcelExporter
    {
        public static void WriteToResponse(Workbook theWorkBook, string FileName, HttpResponse resp)
        {
            System.IO.MemoryStream theStream = new System.IO.MemoryStream();

            BIFF8Writer.WriteWorkbookToStream(theWorkBook, theStream);

            byte[] byteArr = (byte[])Array.CreateInstance(typeof(byte), theStream.Length);

            theStream.Position = 0;
            theStream.Read(byteArr, 0, (int)theStream.Length);
            theStream.Close();

            resp.Clear();

            resp.AddHeader("content-disposition", "attachment; filename=" + FileName);

            resp.BinaryWrite(byteArr);

            resp.End();

        }


        public static void WriteToDisk(Workbook theWorkBook, string theFile)
        {
            BIFF8Writer.WriteWorkbookToFile(theWorkBook, theFile);
        }

        public static Workbook DataSetToExcel(DataSet ds)
        {
            Workbook b = new Workbook();
            Worksheet w;

            int iRow = 0;
            int iCell = 0;

            foreach (DataTable t in ds.Tables)
            {
                w = b.Worksheets.Add(t.TableName);

                iCell = 0;

                foreach (DataColumn col in t.Columns)
                {
                    iRow = 0;

                    w.Rows[iRow].Cells[iCell].Value = col.ColumnName;

                    iRow += 1;

                    foreach (DataRow r in t.Rows)
                    {
                        if (!(r[col.ColumnName] is byte[]))
                        {
                            w.Rows[iRow].Cells[iCell].Value = r[col.ColumnName];
                        }
                        else
                        {
                            w.Rows[iRow].Cells[iCell].Value = r[col.ColumnName].ToString();
                        }

                        iRow += 1;
                    }

                    iCell += 1;
                } 
            } 

            return b;
        }

    }

}