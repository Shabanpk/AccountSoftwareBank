using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BusinessEntities;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;
using System.Net;
using System.Security.Principal;


namespace DAL
{
    public class DAL_OpeningBalance
    {
        string ConnString = DataAccess.ConnString;
        //public string ConnString = "Data Source=HADI-B94B6CB176;Initial Catalog=RashanGhar;User id=sa;password=iloveyoumuhammad786";
        //string ConnString = @"Data Source=JAWAD-PC\FAHAD;Initial Catalog=RashanGhar;user id=sa;password=12345;";
        //public string ConnString = "Data Source=PHAHAD;Initial Catalog=RashanGhar;User id=sa;password=123456";

        public long GetMaxID()
        {
            long a = 0;
            SqlConnection sqlconn = new SqlConnection(ConnString);
            SqlCommand sqlcomm = new SqlCommand("Select IsNull(Max(OpeningBalanceID),0)+1 As [MaxOpeningID] From OpeningBalanceM", sqlconn);
            sqlconn.Open();
            SqlDataReader dr = sqlcomm.ExecuteReader();
            if (dr.Read())
            {
                a = long.Parse(dr["MaxOpeningID"].ToString());
                dr.Close();
                dr.Dispose();
                sqlcomm.Dispose();
                sqlconn.Close();
            }
            return a;
        }

        public void InsertOpeningBalanceM(BE_OpeningBalance  OPeningObjM)
        {
            SqlTransaction trans = null;
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = sqlconn.CreateCommand();
                sqlconn.Open();
                trans = sqlconn.BeginTransaction();
                sqlcomm.CommandText = "Insert_OpeningBalanceM";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Transaction = trans;

                long MaxID = GetMaxID();
                sqlcomm.Parameters.AddWithValue("@OpeningBalanceID", MaxID);
                sqlcomm.Parameters.AddWithValue("@OpeningDate", OPeningObjM.OpeningDate);
                sqlcomm.Parameters.AddWithValue("@Remarks", OPeningObjM.Remarks);
                sqlcomm.Parameters.AddWithValue("@UserID", OPeningObjM.UserID);
                sqlcomm.Parameters.AddWithValue("@IsActive", OPeningObjM.IsActive);
                sqlcomm.ExecuteNonQuery();
                sqlcomm.Parameters.Clear();
                //sqlconn.Open();
                trans.Commit();
                sqlconn.Close();
                sqlcomm.Dispose();
                trans.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                trans.Rollback();
            }
        }

        public void InsertOpeningBalanceDetail(BE_OpeningBalance ObjDet)
        {
            SqlTransaction trans = null;
            try
            {
                //string IpAddress;
                //IpAddress=GetHostIP();
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = sqlconn.CreateCommand();
                sqlconn.Open();
                trans = sqlconn.BeginTransaction();
                sqlcomm.CommandText = "Insert_OpeningBalanceD";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Transaction = trans;
                sqlcomm.Parameters.AddWithValue("@OpeningBalanceID", ObjDet.OpeningBalanceID);
                sqlcomm.Parameters.AddWithValue("@ItemID", ObjDet.ItemID);
                sqlcomm.Parameters.AddWithValue("@Qty", ObjDet.ItemQty);
                sqlcomm.ExecuteNonQuery();
                sqlcomm.Parameters.Clear();

                sqlcomm.CommandText = "Insert_Stock";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Transaction = trans;
                sqlcomm.Parameters.AddWithValue("@StockDate", DateTime.Now );
                sqlcomm.Parameters.AddWithValue("@TableID", ObjDet.OpeningBalanceID);
                sqlcomm.Parameters.AddWithValue("@TableName","OpeningBalanceM");
                sqlcomm.Parameters.AddWithValue("@InsertionDate", DateTime.Now);
                sqlcomm.Parameters.AddWithValue ("@ItemID",ObjDet.ItemID );
                sqlcomm.Parameters.AddWithValue("@Dr", ObjDet.ItemQty );
                sqlcomm.Parameters.AddWithValue("@Cr",0);
                sqlcomm.Parameters.AddWithValue("@Ipaddress","172.16.100.87");
                sqlcomm.Parameters.AddWithValue("@UserID", 1);
                sqlcomm.Parameters.AddWithValue("@IsActive",1);
                sqlcomm.ExecuteNonQuery();
                sqlcomm.Parameters.Clear();
                trans.Commit();
                sqlconn.Close();
                sqlcomm.Dispose();
                trans.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

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

        public DataTable GetItemDetail(int ItemID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = new SqlCommand("GetOpeningBalance", sqlconn);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@ItemID", ItemID);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;    
        }

        public DataTable GetItems(int ItemID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString );
                SqlCommand sqlcomm = new SqlCommand("GetItems", sqlconn);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@ItemID",ItemID );
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public DataTable GetOpBal(int OpBalID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = new SqlCommand("GetOpeningBalance", sqlconn);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@OpBalID", OpBalID);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public string GetHostIP()
        {
            IPAddress[] ip = Dns.GetHostAddresses(Dns.GetHostName());

            string str = "";
            for (int i = 0; i < ip.Length; i++)
                str += ip[i].ToString();
            return str;
        }
    }
}
