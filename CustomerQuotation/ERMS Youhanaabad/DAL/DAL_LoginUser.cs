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
    public class DAL_LoginUser
    {
        string ConnString = DataAccess.ConnString;
        //string ConnString = @"Data Source=RGYOUHANAABAD\SQLEXPRESS;Initial Catalog=RashanGhar;User id=sa;password=iloveyoumuhammad786";
        //string ConnString = @"Data Source=JAWAD-PC\FAHAD;Initial Catalog=RashanGhar;user id=sa;password=12345;";
        //string ConnString = "Data Source=FAHAD;Initial Catalog=RashanGharGreenTown;User id=sa;password=123456";
        //string ConnString = "Data Source=192.168.3.90;Initial Catalog=RashanGharLatest;User id=sa;password=123";

        public bool CheckUserPassword(BE_LoginUser NewLoginObj)
        {
            bool PassswordCheck = false;
            try
            {
                //if (DateTime.Now < Convert.ToDateTime("2018-01-31"))
                //{
                    SqlConnection sqlconn = new SqlConnection(ConnString);
                    //SqlCommand sqlcomm = new SqlCommand("Select Password From Users Where UserName like '%"+NewLoginObj.UserName+"%' And Status=1", sqlconn);
                    SqlCommand sqlcomm = new SqlCommand("Select Password From Users Where UserName='"+NewLoginObj.UserName +"' And Status=1", sqlconn);
                    DataTable dt = new DataTable();
                    dt = GetdataTable(sqlcomm, sqlconn);
                    if (dt.Rows.Count > 0 && dt !=null)
                    {
                        if (dt.Rows[0]["Password"].ToString() == NewLoginObj.Password)
                        {
                            PassswordCheck = true;
                        }
                        else
                        {
                            PassswordCheck = false;
                        }
                    }
                //}
                //else
                //{
                //    MessageBox.Show("SQL DB Error","Easy Rashan Management System",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                //    return false;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            return PassswordCheck;
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

        //public DataTable GetUserDetail(BE_LoginUser DetailObj)
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        SqlConnection sqlconn = new SqlConnection(ConnString);
        //        SqlCommand sqlcomm = new SqlCommand("Select UserID,UserStatus,IsActive From Users Where UserName like '%" + DetailObj.UserName + "%'", sqlconn);
        //        dt = GetdataTable(sqlcomm, sqlconn);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message,"Easy Rashan Management System",MessageBoxButtons.OK,MessageBoxIcon.Stop );
        //    }
            
        //    return dt;
        //}

        public DataTable GetUserDetail(BE_LoginUser NewBEObj)
        {
            DataTable dt = new DataTable();
            try
            {
                string Qry = @"Select * from Def_UserRights A 
                              inner join Users B ON A.UserID = B.UserID
                              Where B.UserName Like '%" + NewBEObj.UserName + "%' and CanView=1";
                SqlConnection sqlconn = new SqlConnection(ConnString);
                SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
                dt = GetdataTable(sqlcomm, sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return dt;
        }

        public void LoadStyles(Infragistics.Win.UltraWinGrid.UltraCombo cbo)
        {
            SqlConnection sqlconn = new SqlConnection(ConnString);
            SqlCommand sqlcomm = new SqlCommand("select StyleID,Style from styles Where IsActive=1", sqlconn);
            //sqlcomm.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cbo.DataSource = dt;
            cbo.ValueMember = "StyleID";
            cbo.DisplayMember = "Style";
            cbo.DisplayLayout.Bands[0].Columns["StyleID"].Hidden = true;
            cbo.Value = 1;
            sqlconn.Close();
            da.Dispose();
            sqlcomm.Dispose();
        }

        public DataTable GetUserMenu(int UserID)
        {
            DataTable dt = new DataTable();
            try
            {
                string Qry = @"Select A.UserID,A.FormID,ISNULL(CanView,0) As [CanView],FormName,DisplayName,ISNULL(IsParent,0) As [IsParent] from Def_UserRights A
inner join Def_Form B ON A.FormID = B.FormID
where A.UserID = '" + UserID + "' and CanView=1 and B.Status=1 order by A.FormID asc";
                using (SqlConnection conn = new SqlConnection(DataAccess.ConnString))
                {
                    dt = DataAccess.GetDataTableByQry(Qry);
                    if (conn.State == ConnectionState.Open)
                        conn.Dispose();
                    conn.Close();
                    conn.ConnectionString = null;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return dt;
        }

    }
}
