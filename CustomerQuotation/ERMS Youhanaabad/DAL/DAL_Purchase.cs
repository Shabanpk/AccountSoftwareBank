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
    public class DAL_Purchase
    {
        long MaxID;
        string ConnString = DataAccess.ConnString;
        //public string ConnString = "Data Source=HADI-B94B6CB176;Initial Catalog=RashanGhar;User id=sa;password=iloveyoumuhammad786";
        //string ConnString = @"Data Source=JAWAD-PC\FAHAD;Initial Catalog=RashanGhar;user id=sa;password=12345;";
        //public string ConnString = "Data Source=PHAHAD;Initial Catalog=RashanGhar;User id=sa;password=123456";
        //public string ConnString = "Data Source=192.168.3.90;Initial Catalog=RashanGhar;User id=sa;password=123";

        private long GetMaxID()
        {
            long a = 0;
            SqlConnection sqlconn = new SqlConnection(ConnString);
            //SqlCommand sqlcomm = new SqlCommand("Select IsNull(Max(PrID),0)+1 As [MaxPrID] From Trans_PurchaseM Where IsActive=1", sqlconn);
            SqlCommand sqlcomm = new SqlCommand("Select IsNull(Max(PrID),0)+1 As [MaxPrID] From Trans_PurchaseM", sqlconn);
            sqlconn.Open();
            SqlDataReader dr = sqlcomm.ExecuteReader();
            if (dr.Read())
            {
                a = long.Parse(dr["MaxPrID"].ToString());
                dr.Close();
                dr.Dispose();
                sqlcomm.Dispose();
                sqlconn.Close();
            }
            return a;
        }

        private long GetMaxKnockOffID()
        {
            long a = 0;
            SqlConnection sqlconn = new SqlConnection(ConnString);
            SqlCommand sqlcomm = new SqlCommand("Select IsNull(Max(knockOffID),0)+1 As [MaxKnockOffID] From KnockOffPurchase", sqlconn);
            sqlconn.Open();
            SqlDataReader dr = sqlcomm.ExecuteReader();
            if (dr.Read())
            {
                a = long.Parse(dr["MaxKnockOffID"].ToString());
                dr.Close();
                dr.Dispose();
                sqlcomm.Dispose();
                sqlconn.Close();
            }
            return a;
        }

        public void InsertPurchaseMaster(BE_Purchase NewInsObj)
        {
            SqlTransaction trans = null;
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = sqlconn.CreateCommand();
                sqlconn.Open();
                trans = sqlconn.BeginTransaction();
//                sqlcomm.CommandText = @"INSERT INTO Trans_PurchaseM
//                      (PrID, PrDate, ModifiedDate, VendorName, Remarks, IPAddress, IsActive,ChkCreditSales,CreditDueDate)
//VALUES (@PrId,@PrDate,@ModifiedDate,@VendorName,@Remarks,@IPAddress,@IsActive,@ChkCreditSales,@CreditDueDate)
//";            
                sqlcomm.CommandText = @"Insert_PurchaseM";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Transaction = trans;
                MaxID = GetMaxID();
                NewInsObj.PrID = int.Parse(MaxID.ToString());
                string IPAddress = GetHostIP();
                sqlcomm.Parameters.AddWithValue("@PrID", NewInsObj.PrID);
                sqlcomm.Parameters.AddWithValue("@PrDate", NewInsObj.PrDate);
                sqlcomm.Parameters.AddWithValue("@ModifiedDate",NewInsObj.PrDate);
                sqlcomm.Parameters.AddWithValue("@VendorName", NewInsObj.VendorName);
                sqlcomm.Parameters.AddWithValue("@IpAddress", IPAddress);
                sqlcomm.Parameters.AddWithValue("@Remarks",NewInsObj.Remarks);
                sqlcomm.Parameters.AddWithValue("@IsActive", NewInsObj.IsActive);
                sqlcomm.Parameters.AddWithValue("@ChkCreditSales", NewInsObj.ChkCreditSales);
                sqlcomm.Parameters.AddWithValue("@CreditDueDate", NewInsObj.CreditDueDate);
                sqlcomm.ExecuteNonQuery();
                sqlcomm.Parameters.Clear();

                if (NewInsObj.ChkCreditSales == true)
                {
                    sqlcomm.CommandText = @"Insert_KnockOff";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    long KnockOffMaxID = GetMaxKnockOffID();

                    sqlcomm.Parameters.AddWithValue("@KnockOffID", KnockOffMaxID);
                    sqlcomm.Parameters.AddWithValue("@PrID", NewInsObj.PrID);
                    sqlcomm.Parameters.AddWithValue("@PrDate", NewInsObj.PrDate);
                    sqlcomm.Parameters.AddWithValue("@KnockOffDate", Convert.ToDateTime("1900-01-01"));
                    sqlcomm.Parameters.AddWithValue("@CreditDueDate", NewInsObj.CreditDueDate);
                    sqlcomm.Parameters.AddWithValue("@PrAmount", NewInsObj.TotalPRAmount);
                    sqlcomm.Parameters.AddWithValue("@KnockOffAmount", 0);
                    sqlcomm.Parameters.AddWithValue("@KnockOffStatus", 0);
                    sqlcomm.ExecuteNonQuery();
                    sqlcomm.Parameters.Clear();
                }
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

        public void InsertPurchaseDetail(BE_Purchase NewInsDetObj)
        {
            SqlTransaction trans = null;
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = sqlconn.CreateCommand();
                sqlconn.Open();
                trans = sqlconn.BeginTransaction();
                sqlcomm.CommandText = "Insert_PurchaseD";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Transaction = trans;
                sqlcomm.Parameters.AddWithValue("@PrID", NewInsDetObj.PrID);
                sqlcomm.Parameters.AddWithValue("@ItemID", NewInsDetObj.ItemID);
                sqlcomm.Parameters.AddWithValue("@MUnitID", NewInsDetObj.MUnitID);
                sqlcomm.Parameters.AddWithValue("@PrQty",NewInsDetObj.PrQty);
                sqlcomm.Parameters.AddWithValue("@PurchasePrice", NewInsDetObj.PurchasePrice);
                sqlcomm.ExecuteNonQuery();
                //string a = GetHostIP();
                string a = "192.168.3.67";
                sqlcomm.Parameters.Clear();
                //changed on Request of Saghar Sb on 17-12-2016
                sqlcomm.CommandText = "Insert_Stock";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@StockDate", DateTime.Now);
                sqlcomm.Parameters.AddWithValue("@TableID", NewInsDetObj.PrID);
                sqlcomm.Parameters.AddWithValue("@TableName", "Trans_PurchaseD");
                sqlcomm.Parameters.AddWithValue("@InsertionDate", DateTime.Now);
                sqlcomm.Parameters.AddWithValue("@ItemID", NewInsDetObj.ItemID);
                sqlcomm.Parameters.AddWithValue("@Dr", NewInsDetObj.PrQty);
                sqlcomm.Parameters.AddWithValue("@Cr", 0);
                //sqlcomm.Parameters.AddWithValue("@Ipaddress", "192.168.9.70");
                sqlcomm.Parameters.AddWithValue("@Ipaddress", a);
                sqlcomm.Parameters.AddWithValue("@UserID", NewInsDetObj.UserID);
                sqlcomm.Parameters.AddWithValue("@IsActive", 1);
                sqlcomm.ExecuteNonQuery();
                sqlcomm.Parameters.Clear();
                // Commented by shaban
                sqlcomm.CommandText = "Insert_CashBalance";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Transaction = trans;
                string Ip = GetHostIP();
                sqlcomm.Parameters.AddWithValue("@CashInDate", DateTime.Now);
                sqlcomm.Parameters.AddWithValue("@TableID", NewInsDetObj.PrID);
                sqlcomm.Parameters.AddWithValue("@TableName", "Trans_purchaseD");
                sqlcomm.Parameters.AddWithValue("@Dr", 0);
                sqlcomm.Parameters.AddWithValue("@Cr", Math.Round(NewInsDetObj.PurchasePrice, 0));
                sqlcomm.Parameters.AddWithValue("@IpAddress", "192.168.8.0");
                
                //sqlcomm.ExecuteNonQuery();
                //sqlcomm.Parameters.Clear();
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

        public void UpdatePurchaseMaster(BE_Purchase NewObjUpdateM)
        {
            SqlTransaction trans = null;
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = sqlconn.CreateCommand();
                sqlconn.Open();
                trans = sqlconn.BeginTransaction();
                sqlcomm.CommandText = "Update_PurchaseM";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Transaction = trans;
                sqlcomm.Parameters.AddWithValue("@PrID", NewObjUpdateM.PrID);
                sqlcomm.Parameters.AddWithValue("@ModifiedDate", NewObjUpdateM.ModifiedDate);
                sqlcomm.Parameters.AddWithValue("@VendorName", NewObjUpdateM.VendorName);
                sqlcomm.Parameters.AddWithValue("@Remarks", NewObjUpdateM.Remarks);
                sqlcomm.Parameters.AddWithValue("@IsActive", true);
                sqlcomm.Parameters.AddWithValue("@ChkCreditSales", NewObjUpdateM.ChkCreditSales);
                sqlcomm.Parameters.AddWithValue("@CreditDueDate", NewObjUpdateM.CreditDueDate);
                sqlcomm.ExecuteNonQuery();
                sqlcomm.Parameters.Clear();

                if (NewObjUpdateM.ChkCreditSales ==true && NewObjUpdateM.OldStatus==true)
                {
                    sqlcomm.CommandText = @"Update_KnockOff";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    sqlcomm.Parameters.AddWithValue("@PRID", NewObjUpdateM.PrID);
                    sqlcomm.Parameters.AddWithValue("@PRDate", NewObjUpdateM.PrDate);
                    sqlcomm.Parameters.AddWithValue("@CreditDueDate", NewObjUpdateM.CreditDueDate);
                    sqlcomm.Parameters.AddWithValue("@TotalPRAmount", NewObjUpdateM.TotalPRAmount);
                    sqlcomm.ExecuteNonQuery();
                    sqlcomm.Parameters.Clear();
                }
                else if (NewObjUpdateM.OldStatus==false && NewObjUpdateM.ChkCreditSales==true)
                {
                    sqlcomm.CommandText = @"Insert_KnockOff";
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                    long KnockOffMaxID = GetMaxKnockOffID();
                    sqlcomm.Parameters.AddWithValue("@KnockOffID", KnockOffMaxID);
                    sqlcomm.Parameters.AddWithValue("@PrID", NewObjUpdateM.PrID);
                    sqlcomm.Parameters.AddWithValue("@PrDate", NewObjUpdateM.PrDate);
                    sqlcomm.Parameters.AddWithValue("@KnockOffDate", Convert.ToDateTime("1900-01-01"));
                    sqlcomm.Parameters.AddWithValue("@CreditDueDate", NewObjUpdateM.CreditDueDate);
                    sqlcomm.Parameters.AddWithValue("@PrAmount", NewObjUpdateM.TotalPRAmount);
                    sqlcomm.Parameters.AddWithValue("@KnockOffAmount", 0);
                    sqlcomm.Parameters.AddWithValue("@KnockOffStatus", 0);
                    sqlcomm.ExecuteNonQuery();
                    sqlcomm.Parameters.Clear();
                }
                else if (NewObjUpdateM.OldStatus == true && NewObjUpdateM.ChkCreditSales == false)
                {
                    sqlcomm.CommandText = "Delete_KnockOff";
                    sqlcomm.Parameters.AddWithValue("@PrID",NewObjUpdateM.PrID);
                    sqlcomm.ExecuteNonQuery();
                    sqlcomm.Parameters.Clear();
                }

                sqlcomm.CommandText = "Delete_PurchaseD";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Transaction = trans;
                sqlcomm.Parameters.AddWithValue("@PrID", NewObjUpdateM.PrID);
                sqlcomm.ExecuteNonQuery();
                sqlcomm.Parameters.Clear();

                sqlcomm.CommandText = "Delete_PurchaseStock";
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@PrID", NewObjUpdateM.PrID);
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
                trans.Rollback();
            }
        }

        public string GetHostIP()
        {
            IPAddress[] ip = Dns.GetHostAddresses(Dns.GetHostName());

            string str = "";
            for (int i = 0; i < ip.Length; i++)
                str += ip[i].ToString();
            return str;
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
                string qry = @"Select ItemID,ItemName,ITMs.MunitID,MunitName,IsNull((Select Top 1 IsNull(PurchasePrice,0) As [PurchasePrice] From ItemPriceHistory ITPH Where ItemID=ITMS.ItemID Order by Historydate Desc),0) As [PurchasePrice] From Items ITMS Inner Join Munit Mu on Itms.MunitID=Mu.MunitID Where ITMS.ItemID=@ItemID";
                SqlCommand sqlcomm = new SqlCommand(qry,sqlconn);
                sqlcomm.CommandType = CommandType.Text;
                sqlcomm.Parameters.AddWithValue("@ItemID",ItemID);
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

        public DataTable GetPurchasedItems(int PrID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = new SqlCommand("GetPurchasedItems",sqlconn);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@PrID",PrID);
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

    }
}
