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
    public class DAL_DirectSales
    {
        string ConnString = DataAccess.ConnString;
        //string ConnString = "Data Source=HADI-B94B6CB176;Initial Catalog=RashanGhar;User id=sa;password=iloveyoumuhammad786";
        //string ConnString = @"Data Source=JAWAD-PC\FAHAD;Initial Catalog=RashanGhar;user id=sa;password=12345;";
        //string ConnString = "Data Source=FAHAD;Initial Catalog=RashanGhar;User id=sa;password=123456";
        long MaxID = 0;

        private long GetMaxID()
        {
            long a = 0;
            SqlConnection sqlconn = new SqlConnection(ConnString);
            SqlCommand sqlcomm = new SqlCommand("Select IsNull(Max(DRS_ID),0)+1 As [MaxID] From Trans_DRSM", sqlconn);
            sqlconn.Open();
            SqlDataReader dr = sqlcomm.ExecuteReader();
            if (dr.Read())
            {
                a = long.Parse(dr["MaxID"].ToString());
                dr.Close();
                dr.Dispose();
                sqlcomm.Dispose();
                sqlconn.Close();
            }
            return a;
        }

        private long GetMaxIDHistory()
        {
            long a = 0;
            SqlConnection sqlconn = new SqlConnection(ConnString);
            SqlCommand sqlcomm = new SqlCommand("Select IsNull(Max(AccountID),0)+1 As [MaxAccountID] From Trans_AccountHistory", sqlconn);
            sqlconn.Open();
            SqlDataReader dr = sqlcomm.ExecuteReader();
            if (dr.Read())
            {
                a = long.Parse(dr["MaxAccountID"].ToString());
                dr.Close();
                dr.Dispose();
                sqlcomm.Dispose();
                sqlconn.Close();
            }

            return a;
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

        public bool Insert_DirectSalesM(BE_DirectSales NewINSBE)
        {
            SqlTransaction trans = null;
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = sqlconn.CreateCommand();
                sqlconn.Open();
                trans = sqlconn.BeginTransaction();
                sqlcomm.CommandText = "Insert_Trans_DRSM";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Transaction = trans;
                MaxID = GetMaxID();
                NewINSBE.DR_ID = MaxID;
                sqlcomm.Parameters.AddWithValue("@DRS_ID",NewINSBE.DR_ID);
                sqlcomm.Parameters.AddWithValue("@DRS_Date", NewINSBE.DRS_Date);
                sqlcomm.Parameters.AddWithValue("@ModifiedDate", NewINSBE.ModifiedDate);
                sqlcomm.Parameters.AddWithValue("@DRS_Name", NewINSBE.DRS_Name);
                sqlcomm.Parameters.AddWithValue("@MobileNo", NewINSBE.MobileNo);
                sqlcomm.Parameters.AddWithValue("@Remarks", NewINSBE.Remarks);
                sqlcomm.Parameters.AddWithValue("@IsLoan", NewINSBE.IsLoan);
                sqlcomm.Parameters.AddWithValue("@Type", NewINSBE.Type);
                sqlcomm.Parameters.AddWithValue("@IsActive", NewINSBE.IsActive);
                sqlcomm.Parameters.AddWithValue("@Discount", NewINSBE.Discount);
                sqlcomm.ExecuteNonQuery();
                sqlcomm.Parameters.Clear();

                trans.Commit();
                sqlconn.Close();
                sqlcomm.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName);
                trans.Rollback();
                return false;
            }
        }

        public bool Insert_DirectSalesD(BE_DirectSales NewINSBEDetail)
        {
            SqlTransaction trans = null;
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = sqlconn.CreateCommand();
                sqlconn.Open();
                trans = sqlconn.BeginTransaction();
                sqlcomm.CommandText = "Insert_Trans_DRSD";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Transaction = trans;
                //NewINSBEDetail.DR_ID = MaxID;
                sqlcomm.Parameters.AddWithValue("@DRS_ID",NewINSBEDetail.DR_ID);
                sqlcomm.Parameters.AddWithValue("@ItemID", NewINSBEDetail.ItemID);
                sqlcomm.Parameters.AddWithValue("@ItemPrice", NewINSBEDetail.ItemPrice);
                sqlcomm.Parameters.AddWithValue("@SalesPrice", NewINSBEDetail.SalesPrice);
                sqlcomm.Parameters.AddWithValue("@Qty", NewINSBEDetail.Qty);
                sqlcomm.ExecuteNonQuery();
                sqlcomm.Parameters.Clear();

                sqlcomm.CommandText = "Insert_Stock";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Transaction = trans;
                sqlcomm.Parameters.AddWithValue("@StockDate", DateTime.Now);
                sqlcomm.Parameters.AddWithValue("@TableID", NewINSBEDetail.DR_ID);
                sqlcomm.Parameters.AddWithValue("@TableName", "Trans_DRSM");
                sqlcomm.Parameters.AddWithValue("@InsertionDate", DateTime.Now);
                sqlcomm.Parameters.AddWithValue("@ItemID", NewINSBEDetail.ItemID);
                sqlcomm.Parameters.AddWithValue("@Dr", 0);
                sqlcomm.Parameters.AddWithValue("@Cr", NewINSBEDetail.Qty);
                sqlcomm.Parameters.AddWithValue("@IpAddress", "192.168.7.60");
                sqlcomm.Parameters.AddWithValue("@userID", NewINSBEDetail.UserID);
                sqlcomm.Parameters.AddWithValue("@IsActive", 1);
                sqlcomm.ExecuteNonQuery();
                sqlcomm.Parameters.Clear();

                // Commented by shaban
                sqlcomm.CommandText = "Insert_CashBalance";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Transaction = trans;
                //string Ip = GetHostIP();
                sqlcomm.Parameters.AddWithValue("@CashInDate", DateTime.Now);
                sqlcomm.Parameters.AddWithValue("@TableID", NewINSBEDetail.DR_ID);
                sqlcomm.Parameters.AddWithValue("@TableName", "Trans_DRSM");
                sqlcomm.Parameters.AddWithValue("@Dr", NewINSBEDetail.NetAmount);
                sqlcomm.Parameters.AddWithValue("@Cr", 0);
                sqlcomm.Parameters.AddWithValue("@IpAddress", "192.168.7.0");
                sqlcomm.ExecuteNonQuery();
                sqlcomm.Parameters.Clear();
                
                trans.Commit();
                sqlconn.Close();
                sqlcomm.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                trans.Rollback();
                return false;
            }
        }

        public double GetStock(int ItemID)
        {
            double Stock = 0.0;
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = new SqlCommand("", sqlconn);
                sqlcomm.Parameters.AddWithValue("@ItemID", ItemID);
                DataTable dt = new DataTable();
                dt = GetdataTable(sqlcomm, sqlconn);
                if (dt.Rows.Count > 0)
                {
                    Stock = double.Parse(dt.Rows[0][""].ToString());
                }
                sqlconn.Close();
                sqlcomm.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return Stock;
        }

        public DataTable GetItemDetails(int ItemID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = new SqlCommand("GetItemDetails", sqlconn);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@ItemID", ItemID);
                dt = GetdataTable(sqlcomm, sqlconn);
                sqlconn.Close();
                sqlcomm.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public bool Update_DirectSales(BE_DirectSales NewUpdateBE)
        {
            SqlTransaction trans = null;
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = sqlconn.CreateCommand();
                sqlconn.Open();
                trans = sqlconn.BeginTransaction();
                sqlcomm.CommandText = "Update_Trans_DRSM";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Transaction = trans;
                sqlcomm.Parameters.AddWithValue("@DRS_ID",NewUpdateBE.DR_ID);
                sqlcomm.Parameters.AddWithValue("@ModifiedDate", NewUpdateBE.ModifiedDate);
                sqlcomm.Parameters.AddWithValue("@DRS_Name", NewUpdateBE.DRS_Name);
                sqlcomm.Parameters.AddWithValue("@MobileNo", NewUpdateBE.MobileNo);
                sqlcomm.Parameters.AddWithValue("@Remarks", NewUpdateBE.Remarks);
                sqlcomm.Parameters.AddWithValue("@IsLoan", NewUpdateBE.IsLoan);
                sqlcomm.Parameters.AddWithValue("@Discount", NewUpdateBE.Discount);
                sqlcomm.Parameters.AddWithValue("@Type", NewUpdateBE.Type);
                //sqlcomm.Parameters.AddWithValue("@SlipNo", NewUpdateBE.SlipNo);
                sqlcomm.ExecuteNonQuery();
                sqlcomm.Parameters.Clear();

                //Delete Detail of Direct Sales Table
                sqlcomm.CommandText = "Delete_Trans_DRSD";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@DRS_ID",NewUpdateBE.DR_ID);
                sqlcomm.ExecuteNonQuery();
                sqlcomm.Parameters.Clear();

                //Deelte Stock Entry From Stock Table
                sqlcomm.CommandText = "Delete_DirectSalesStock";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@TableID", NewUpdateBE.DR_ID);
                sqlcomm.ExecuteNonQuery();
                sqlcomm.Parameters.Clear();

                // Commeneted by shaban
                //Delete Cash Entry From Cash Table
                sqlcomm.CommandText = "Delete_DirectSalesCash";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Transaction = trans;
                sqlcomm.Parameters.AddWithValue("@TableID", NewUpdateBE.DR_ID);
                sqlcomm.ExecuteNonQuery();
                sqlcomm.Parameters.Clear();

                trans.Commit();
                sqlconn.Close();
                sqlcomm.Dispose();
                trans.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                trans.Rollback();
                return false;
            }
            
        }

        public DataTable GetDirectSales_Details(int DRS_ID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = new SqlCommand("GetDirectSales_Details", sqlconn);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@DRS_ID",DRS_ID);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public bool Received_ProductLoan(BE_DirectSales NewUpdateBE)
        {
            SqlTransaction trans = null;
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = sqlconn.CreateCommand();
                sqlconn.Open();
                trans = sqlconn.BeginTransaction();
                sqlcomm.CommandText = "Update_ProductLoan";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Transaction = trans;
                sqlcomm.Parameters.AddWithValue("@DRS_ID", NewUpdateBE.DR_ID);
                sqlcomm.Parameters.AddWithValue("@LoanReceivedDate", NewUpdateBE.LoanReceivedDate);
                sqlcomm.Parameters.AddWithValue("@IsLoan", NewUpdateBE.IsLoan);
                sqlcomm.ExecuteNonQuery();
                sqlcomm.Parameters.Clear();

                trans.Commit();
                sqlconn.Close();
                sqlcomm.Dispose();
                trans.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                trans.Rollback();
                return false;
            }

        }

        public bool Update_ProductLoan(BE_DirectSales NewUpdateBE)
        {
            SqlTransaction trans = null;
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = sqlconn.CreateCommand();
                sqlconn.Open();
                trans = sqlconn.BeginTransaction();
                sqlcomm.CommandText = "Update_ProductLoan";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Transaction = trans;
                sqlcomm.Parameters.AddWithValue("@DRS_ID", NewUpdateBE.DR_ID);
                sqlcomm.Parameters.AddWithValue("@LoanReceivedDate", NewUpdateBE.LoanReceivedDate);
                sqlcomm.Parameters.AddWithValue("@IsLoan", NewUpdateBE.IsLoan);
                sqlcomm.ExecuteNonQuery();
                sqlcomm.Parameters.Clear();

                trans.Commit();
                sqlconn.Close();
                sqlcomm.Dispose();
                trans.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                trans.Rollback();
                return false;
            }

        }


    }
}
