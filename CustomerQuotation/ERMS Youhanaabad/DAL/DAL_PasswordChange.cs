using System;
using System.Collections.Generic;
using System.Text;
using BusinessEntities;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DAL
{
    public class DAL_PasswordChange
    {
        string ConnString = DataAccess.ConnString;

        public bool CheckPassword(int UserID, string OldPassword)
        {
            bool IsValid = false;
            try
            {
                string Qry = @"Select UserID,UserName,Password from Users where UserID='" + UserID + "' ";
                DataTable dt = new DataTable();
                dt = DataAccess.GetDataTableByQry(Qry);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (OldPassword == dt.Rows[0]["Password"].ToString())
                    {
                        IsValid = true;
                    }
                    else
                    {
                        IsValid = false;
                    }
                }
            }
            catch (Exception ex)
            {
                IsValid = false;
                throw ex;
            }
            return IsValid;
        }

        public bool SaveRecord(BusinessEntities.BE_PasswordChange NewBEObj)
        {
            bool IsSave = false;
            SqlConnection Conn = new SqlConnection(ConnString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                string Qry = @"Update Users 
                            set
                            Password='" + NewBEObj.NewPassword + "'where UserID='" + NewBEObj.UserID + "' ";
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();
                cmd.Connection = Conn;
                cmd.CommandText = Qry;
                //cmd.Parameters.AddWithValue("@UserID",NewBEObj.UserID);
                //cmd.Parameters.AddWithValue("@NewPassword",NewBEObj.NewPassword);
                IsSave = DataAccess.InserRecord(Qry);
                //int i = cmd.ExecuteNonQuery();
                //if (i != 0)
                //{
                //    IsSave = true;
                //}
                //else
                //{
                //    IsSave = false;
                //}
            }
            catch (Exception ex)
            {
                IsSave = false;
                throw ex;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                cmd.Dispose();
                Conn.ConnectionString = null;
            }
            return IsSave;
        }

    }
}
