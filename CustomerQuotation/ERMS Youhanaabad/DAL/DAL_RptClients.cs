using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BusinessEntities;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;

namespace DAL
{
    public class DAL_RptClients
    {
        //string ConnString = "Data Source=HADI-B94B6CB176;Initial Catalog=RashanGhar;User id=sa;password=iloveyoumuhammad786";
        //string ConnString = @"Data Source=JAWAD-PC\FAHAD;Initial Catalog=RashanGhar;user id=sa;password=12345;";
        string ConnString = "Data Source=PHAHAD;Initial Catalog=RashanGhar;User id=sa;password=123456";

        private DataTable GetdataTable(SqlCommand sqlcomm, SqlConnection sqlconn)
        {
            sqlcomm.Connection = sqlconn;
            SqlDataAdapter da = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        private DataTable GetDataTable(string qry, SqlConnection sqlconn)
        {
            sqlconn.ConnectionString = ConnString;
            SqlDataAdapter da = new SqlDataAdapter(qry, sqlconn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public DataTable GetRptDatatable(BE_RptClients BE_RptClient,string Rpttype)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.Connection = sqlconn;
                if (Rpttype=="MemberName")
                {
                    sqlcomm.CommandText = "RptSearch_ClientName";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@ClientName",BE_RptClient.MemberName);
                    dt = GetdataTable(sqlcomm, sqlconn);
                    sqlcomm.Dispose();
                }
                else if (Rpttype == "MemberCode")
                {
                    sqlcomm.CommandText = "RptSearch_MemberCode";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@ClientCode", BE_RptClient.MemberCode);
                    dt = GetdataTable(sqlcomm, sqlconn);
                    sqlcomm.Dispose();
                }
                else if (Rpttype == "NIC")
                {
                    sqlcomm.CommandText = "RptSearch_NIC";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@NIC", BE_RptClient.NIC);
                    dt = GetdataTable(sqlcomm, sqlconn);
                    sqlcomm.Dispose();
                }
                else if (Rpttype == "CellNo")
                {
                    sqlcomm.CommandText = "RptSearch_CellNo";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@CellNo", BE_RptClient.CellNo);
                    dt = GetdataTable(sqlcomm, sqlconn);
                    sqlcomm.Dispose();
                }
                else if (Rpttype == "Limit")
                {
                    sqlcomm.CommandText = "RptSearch_Limit";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@Limit", BE_RptClient.Limit);
                    dt = GetdataTable(sqlcomm, sqlconn);
                    sqlcomm.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Easy Rashan Management System",MessageBoxButtons.OK,MessageBoxIcon.Stop);
            }
            return dt;
        }

        
    }
}
