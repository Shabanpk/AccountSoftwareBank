using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using BusinessEntities;

namespace DAL
{
    public class DataAccess
    {
        public static string ProjectName = "Karwan Management System";
        
        public static string GetConnection()
        {
            string s = ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;
            return s;
        }

        //old LIve Server Now 2014-11-20
        //public static string ConnString = @"Data Source=ADMINISTRATOR;Initial Catalog=RashanGhar;user id=sa;password=Muhammad786;";

        //Live Server 2017-04-29
        //public static string ConnString = @"Data Source=ADMIN\SQLEXPRESS;Initial Catalog=RashanGhar;user id=sa;password=Muhammad786;";
        
        //Local Server
        //public static string ConnString = @"Data Source=Fahad\NEWSQLSERVER;Initial Catalog=RashanGhar;user id=sa;password=78886;";
        //public static string ConnString = @"Data Source=SMOKINGGUNS\SMOKINGGUNS;Initial Catalog=RashanGharNew;user id=sa;password=admin22;";


        //public static string ConnString = @"Data Source=shaban;Initial Catalog=Karwan;User id=sa;password=78886";    

        //mine
        //public static string ConnString = @"Data Source=CS-006618LT;Initial Catalog=Karwan;User id=sa;password=78886";
        //public static string ConnString = @"Data Source=CS-006618LT;Initial Catalog=RaizuldinQuotation;User id=sa;password=78886";
        public static string ConnString = @"Data Source=DESKTOP-GPL38Q6;Initial Catalog=RaizuldinQuotation;User id=sa;password=78886";
        
        //public static string ConnString = @"Data Source=shaban;Initial Catalog=Karwan;User id=sa;password=78886";

        //public static string ConnString = @"Data Source=shaban;Initial Catalog=StudentTest;User id=sa;password=78886";

        //public static string ConnString = @"Data Source=SERVER\HABIBURREHMAN;Initial Catalog=Karwan;User id=sa;password=78886";

        //public static string ConnString = @"Data Source=SERVER;Initial Catalog=Karwan;User id=sa;password=78886";

        public static DataTable GetDataTable(SqlCommand sqlcomm, SqlConnection sqlconn)
        {
            //string a = ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;
            sqlconn.ConnectionString = ConnString;
            sqlcomm.Connection = sqlconn;
            SqlDataAdapter da = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        
        public static DataTable GetDataTable(string qry, SqlConnection sqlconn)
        {
            //string a = ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;
            sqlconn.ConnectionString = ConnString ;
            SqlDataAdapter da = new SqlDataAdapter(qry, sqlconn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public void GetMaxID()
        {
            //ClsGlobal VarMaxID = new ClsGlobal();
            //string a = ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;
            //SqlConnection sqlconn = new SqlConnection(a);
            //SqlCommand sqlcomm = new SqlCommand("Select IsNull(MAX(" + VarMaxID.ColName   + "),0) + 1 AS MaxID From " + VarMaxID.TableName +" " + VarMaxID.whereClause +" ",sqlconn );
            //sqlconn.Open();
            //SqlDataReader dr = sqlcomm.ExecuteReader();
            //long MaxID = 1;
            //if (dr.Read())
            //{
            //    MaxID = long.Parse(dr["MaxID"].ToString());
            //    dr.Close();
            //    dr.Dispose();
            //    sqlcomm.Dispose();
            //    sqlconn.Close();
            //}
            //return true ;
        }

        public static int GetMaxNO(string ColumnName, string TableName)
        {
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                //if (sqlconn.State == ConnectionState.Closed)
                //    sqlconn.Open();
                string qry = "Select Isnull(max(" + ColumnName + "),0)+1 As [MaxID] From " + TableName + "";
                int MaxID = 0;
                DataTable dt = new DataTable();
                dt = GetDataTable(qry, sqlconn);
                if (dt.Rows.Count > 0)
                {
                    MaxID = int.Parse(dt.Rows[0]["MaxID"].ToString());
                }
                return MaxID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetDataTableByQry(string Qry)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                SqlDataAdapter adap = new SqlDataAdapter(Qry, conn);
                adap.Fill(dt);
                conn.Dispose();
                conn.Close();
            }
            return dt;
        }

        public static bool InserRecord(string Qry)
        {
            bool IsSave = false;
            using (SqlConnection Conn = new SqlConnection(ConnString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = Qry;
                    cmd.Connection = Conn;
                    if (Conn.State == ConnectionState.Closed)
                        Conn.Open();
                    cmd.ExecuteNonQuery();
                    IsSave = true;
                }
            }
            return IsSave;
        }

        public static DataTable GetData(string Qry)
        {
            DataTable dt = new DataTable();
            using (SqlConnection Conn = new SqlConnection(ConnString))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(Qry, Conn))
                {
                    da.Fill(dt);
                }
            }

            return dt;
        }

    }
}
