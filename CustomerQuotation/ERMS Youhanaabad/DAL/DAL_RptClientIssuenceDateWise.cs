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
    public class DAL_RptClientIssuenceDateWise
    {
        string ConnString = "Data Source=HADI-B94B6CB176;Initial Catalog=RashanGhar;User id=sa;password=iloveyoumuhammad786";
        //string ConnString = @"Data Source=JAWAD-PC\FAHAD;Initial Catalog=RashanGhar;user id=sa;password=12345;";
        //string ConnString = "Data Source=PHAHAD;Initial Catalog=RashanGhar;User id=sa;password=123456";

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

        public DataTable GetClientName(int ClientID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = new SqlCommand();
                sqlcomm.CommandText = "GetClientName";
                sqlcomm.Connection = sqlconn;
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@ClientID",ClientID);
                dt = GetdataTable(sqlcomm, sqlconn);
                sqlconn.Close();
                sqlcomm.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Easy Rashan Management System",MessageBoxButtons.OK,MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable GetReportData(int ClientID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = new SqlCommand("Rpt_ClientIssuenceDateWise",sqlconn);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@ClientID",ClientID);
                
                dt = GetdataTable(sqlcomm, sqlconn);
                sqlconn.Close();
                sqlcomm.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Easy Rashan Management System",MessageBoxButtons.OK,MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable GetAreaName(int AreaID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm= new SqlCommand();
                string SqlQry = "Select AreaName From Areas_Def where AreaID ='" + AreaID + "'";
                sqlcomm.CommandText = SqlQry;
                sqlcomm.Connection = sqlconn;
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Easy Rashan Management System",MessageBoxButtons.OK,MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable GetEmpName(int EmpID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = new SqlCommand();
                string SqlQry = "Select EmpName From Emp_Def Where EmpID='"+EmpID+"'";
                sqlcomm.CommandText = SqlQry;
                sqlcomm.Connection = sqlconn;
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }
    }
}
