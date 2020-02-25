using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BusinessEntities;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DAL
{
    public class DAL_MUnit
    {
        private long InvestmentPosting = 0L;

        //string ConnString = DataAccess.GetConnection();
        string ConnString = DataAccess.ConnString;

        public string InsertMUnit(BE_MUnit NewMUnitInsertObj)
        {
            string a = string.Empty;
            SqlConnection sqlconn = new SqlConnection(ConnString);
            SqlCommand sqlcomm = new SqlCommand("InsertMUnit", sqlconn);
            sqlcomm.CommandType = CommandType.StoredProcedure;
            this.InvestmentPosting = int.Parse(DataAccess.GetMaxNO("MUnitID", "MUnit").ToString());
            NewMUnitInsertObj.MUnitID = this.InvestmentPosting;
            sqlcomm.Parameters.AddWithValue("@MUnitID", NewMUnitInsertObj.MUnitID );
            sqlcomm.Parameters.AddWithValue("@MUnitName", NewMUnitInsertObj.MUnitName );
            sqlcomm.Parameters.AddWithValue("@IsActive", NewMUnitInsertObj.IsActive );
            
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

        public bool DuplicateMUnit(BE_MUnit NewObjMUnit)
        {
            SqlConnection sqlconn = new SqlConnection(ConnString);
            string qry = "Select MUnitID From MUnit Where MUnitName like '%" + NewObjMUnit.MUnitName  + "%'";
            DataTable dt = new DataTable();
            dt = DataAccess.GetDataTable(qry, sqlconn);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public DataTable GetMUnitName(BE_MUnit MUnitName)
        {
            SqlConnection sqlconn = new SqlConnection(ConnString);
            SqlCommand sqlcomm = new SqlCommand("GetMUnitName", sqlconn);
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Parameters.AddWithValue("@MUnitID", MUnitName.MUnitID );
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

        public string UPdateMUnit(BE_MUnit NewBEUpdateObj)
        {
            string a = string.Empty;
            SqlConnection sqlconn = new SqlConnection(ConnString);
            SqlCommand sqlcomm = new SqlCommand("UpdateMUnitName", sqlconn);
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Parameters.AddWithValue("@MUnitID", NewBEUpdateObj.MUnitID);
            sqlcomm.Parameters.AddWithValue("@MUnitName", NewBEUpdateObj.MUnitName);
            sqlcomm.Parameters.AddWithValue("@IsActive", NewBEUpdateObj.IsActive);

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

    }
}
