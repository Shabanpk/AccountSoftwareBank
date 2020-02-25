using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BusinessEntities;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DAL
{
    public class DAL_Group
    {
        string ConnString = DataAccess.ConnString;
        private long InvestmentPosting = 0L;

        //public string ConnString = "Data Source=HADI-B94B6CB176;Initial Catalog=RashanGhar;User id=sa;password=iloveyoumuhammad786";
        //string ConnString = @"Data Source=JAWAD-PC\FAHAD;Initial Catalog=RashanGhar;user id=sa;password=12345;";
        //public string ConnString = "Data Source=PHAHAD;Initial Catalog=RashanGhar;User id=sa;password=123456";

        public long GetMaxID()
        {
            long a = 0;
            SqlConnection sqlconn = new SqlConnection(ConnString);
            SqlCommand sqlcomm = new SqlCommand("Select IsNull(Max(GroupID),0)+1 As [GroupID] From Groups", sqlconn);
            sqlconn.Open();
            SqlDataReader dr = sqlcomm.ExecuteReader();
            if (dr.Read())
            {
                a = long.Parse(dr["GroupID"].ToString());
                dr.Close();
                dr.Dispose();
                sqlcomm.Dispose();
                sqlconn.Close();
            }
            return a;
        }

        public void InsertGroup(BE_Group BEObj)
        {
            SqlConnection sqlconn = new SqlConnection(ConnString);
            SqlCommand sqlcomm = new SqlCommand("InsertGroup", sqlconn);
            sqlcomm.CommandType = CommandType.StoredProcedure;
            this.InvestmentPosting = int.Parse(DataAccess.GetMaxNO("GroupID", "Groups").ToString());
            BEObj.GroupID = this.InvestmentPosting;
            sqlcomm.Parameters.AddWithValue("@GroupID", BEObj.GroupID );
            sqlcomm.Parameters.AddWithValue("@GroupName", BEObj.GroupName );
            sqlcomm.Parameters.AddWithValue("@IsActive", BEObj.IsActive );
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
        }

        public bool DuplicateGroup(BE_Group  NewObjDuplicate)
        {
            SqlConnection sqlconn = new SqlConnection(ConnString);
            string qry = "Select GroupID From Groups Where GroupName like '%" + NewObjDuplicate.GroupName  + "%'";
            DataTable dt = new DataTable();
            dt = GetDataTable(qry, sqlconn);
            if (dt.Rows.Count > 0)
            {
                return true  ;
            }
            return false ;
        }

        public DataTable GetGroupName(BE_Group NewBEObj)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = new SqlCommand("GetGroupName", sqlconn);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@GroupID", NewBEObj.GroupID);
                dt = GetDataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public void UPdateGroupName(BE_Group NewUpdateBE)
        {
            SqlConnection sqlconn = new SqlConnection(ConnString);
            SqlCommand sqlcomm = new SqlCommand("UpdateGroup", sqlconn);
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Parameters.AddWithValue("@GroupID", NewUpdateBE.GroupID );
            sqlcomm.Parameters.AddWithValue("@GroupName", NewUpdateBE.GroupName );
            sqlcomm.Parameters.AddWithValue("@IsActive", NewUpdateBE.IsActive );

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
        }

        private DataTable GetDataTable(SqlCommand sqlcomm, SqlConnection sqlconn)
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
    }
}
