using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.ProviderBase;
using System.Data.OleDb;
using System.Runtime.InteropServices;

/// <summary>
/// Summary description for clsDAO
/// </summary>
public class clsDAO
{
    SqlConnection cnn;

    public string strNombreConexion
    {
        get { return ConfigurationManager.ConnectionStrings["ApplicationDatabase"].ConnectionString; }
    }

    public string strNombreConexionSeg
    {
        get { return ConfigurationManager.ConnectionStrings["Security"].ConnectionString; }
    }

    public System.Data.DataTable SqlCall(String strValue)
    {
        try
        {
            cnn = new SqlConnection(strNombreConexion);
            SqlCommand sqlComm = new SqlCommand(strValue);
            sqlComm.Connection = cnn;
            sqlComm.CommandTimeout = 30000;
            cnn.Open();
            //sqlComm.ExecuteScalar();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = sqlComm;
            DataTable tmpDT = new DataTable();
            da.Fill(tmpDT);

            cnn.Close();
            return tmpDT;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public System.Data.DataTable SqlSecurityProfiles(String usuario)
    {
        try
        {
            cnn = new SqlConnection(strNombreConexionSeg);
            SqlCommand sqlComm = new SqlCommand("SP_OBTENER_PERFILES_USUARIO '" + usuario + "','" + ConfigurationManager.AppSettings["ApplicationId"] + "'");
            sqlComm.Connection = cnn;
            sqlComm.CommandTimeout = 30000;
            cnn.Open();
            //sqlComm.ExecuteScalar();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = sqlComm;
            DataTable tmpDT = new DataTable();
            da.Fill(tmpDT);

            cnn.Close();
            return tmpDT;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void SqlExec(String strValue)
    {
        try
        {
            cnn = new SqlConnection(strNombreConexion);
            SqlCommand sqlComm = new SqlCommand(strValue);
            sqlComm.Connection = cnn;
            sqlComm.CommandTimeout = 30000;
            cnn.Open();
            sqlComm.ExecuteScalar();
            sqlComm = null;
            cnn.Close();
        }
        catch (Exception ex)
        {
            cnn.Close();
            throw ex;
        }

    }

    public DataTable devuelveSPSQL(string strSP, List<SqlParameter> lstParametros)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommand command = new SqlCommand();
        cnn = new SqlConnection(strNombreConexion);
        cnn.Open();
        command = new SqlCommand();
        foreach (SqlParameter objParametro in lstParametros)
        {
            command.Parameters.Add(objParametro);
        }
        command.CommandTimeout = 2000;
        command.Connection = cnn;
        command.CommandText = strSP;
        command.CommandType = CommandType.StoredProcedure;
        //command.ExecuteScalar();
        da.SelectCommand = command;
        da.Fill(dt);

        command.Dispose();
        cnn.Close();

        return dt;
    }
}
