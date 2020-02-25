using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BusinessEntities;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DAL
{
    public class DAL_ItemDef
    {
        //string ConnString = DataAccess.GetConnection();
        string ConnString = DataAccess.ConnString;

        public string InsertItem(BE_ItemDef ObjItemInsert)
        {
            string a = string.Empty;
            SqlConnection sqlconn = new SqlConnection(ConnString);
            SqlCommand sqlcomm = new SqlCommand("InsertItems", sqlconn);
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Parameters.AddWithValue("@ItemID", ObjItemInsert.ItemID);
            sqlcomm.Parameters.AddWithValue("@ItemName", ObjItemInsert.ItemName);
            sqlcomm.Parameters.AddWithValue("@GroupID", ObjItemInsert.GroupID);
            sqlcomm.Parameters.AddWithValue("@MunitID", ObjItemInsert.MUnitID);
            sqlcomm.Parameters.AddWithValue("@ItemPrice",ObjItemInsert.ItemPrice );
            sqlcomm.Parameters.AddWithValue("@IsActive", ObjItemInsert.IsActive );

            try
            {
                sqlconn.Open();
                sqlcomm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                sqlconn.Close();
                sqlcomm.Dispose();
            }
            return a;
        }

        public bool DuplicateItemName(BE_ItemDef NewUPdateItem)
        {
            SqlConnection sqlconn = new SqlConnection(ConnString);
            string qry = "Select ItemID From Items Where ItemName like '%" + NewUPdateItem.ItemName + "%'";
            DataTable dt = new DataTable();
            dt = DataAccess.GetDataTable(qry, sqlconn);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public DataTable GetItemName(BE_ItemDef GetItemName)
        {
            SqlConnection sqlconn = new SqlConnection(ConnString);
            SqlCommand sqlcomm = new SqlCommand("GetItemname", sqlconn);
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Parameters.AddWithValue("@ItemID", GetItemName.ItemID );
            DataTable dt = new DataTable();
            dt = DataAccess.GetDataTable(sqlcomm, sqlconn);
            try
            {
                sqlconn.Open();
                sqlcomm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                sqlconn.Close();
                sqlcomm.Dispose();
            }
            return dt;
        }

        public string UPdateItem(BE_ItemDef NewBEUpdateObj)
        {
            string a = string.Empty;
            SqlConnection sqlconn = new SqlConnection(ConnString);
            SqlCommand sqlcomm = new SqlCommand("UpdateItem", sqlconn);
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Parameters.AddWithValue("@ItemID", NewBEUpdateObj.ItemID );
            sqlcomm.Parameters.AddWithValue("@ItemName", NewBEUpdateObj.ItemName );
            sqlcomm.Parameters.AddWithValue("@GroupID",NewBEUpdateObj.GroupID );
            sqlcomm.Parameters.AddWithValue("@MunitID",NewBEUpdateObj.MUnitID );
            sqlcomm.Parameters.AddWithValue("@ItemPrice",NewBEUpdateObj.ItemPrice );
            //sqlcomm.Parameters.AddWithValue("@IsActive", NewBEUpdateObj.IsActive);

            try
            {
                sqlconn.Open();
                sqlcomm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                sqlconn.Close();
                sqlcomm.Dispose();
            }
            return a;
        }

        public void LoadGroups(Infragistics.Win.UltraWinGrid.UltraCombo cbo)
        {
            SqlConnection sqlconn = new SqlConnection(ConnString);
            SqlCommand sqlcomm = new SqlCommand("LoadGroups",sqlconn );
            sqlcomm.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcomm );
            DataTable dt = new DataTable();
            da.Fill(dt);
            cbo.DataSource = dt;
            cbo.ValueMember = "GroupID";
            cbo.DisplayMember = "GroupName";
            cbo.DisplayLayout.Bands[0].Columns["GroupID"].Hidden = true;
            cbo.Value = 1;
            sqlconn.Close();
            da.Dispose();
            sqlcomm.Dispose();
        }

        public void LoadMunits(Infragistics.Win.UltraWinGrid.UltraCombo cbo)
        {
            SqlConnection sqlconn = new SqlConnection(ConnString);
            SqlCommand sqlcomm = new SqlCommand("LoadMunits",sqlconn );
            sqlcomm.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            da.Fill(dt );
            cbo.DataSource = dt;
            cbo.ValueMember = "MUnitID";
            cbo.DisplayMember = "MunitName";
            cbo.DisplayLayout.Bands[0].Columns["MunitID"].Hidden = true;
            cbo.Value = 1;
            sqlconn.Close();
            da.Dispose();
            sqlcomm.Dispose();
        }
    }
}
