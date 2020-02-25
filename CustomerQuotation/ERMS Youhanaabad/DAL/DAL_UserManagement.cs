using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BusinessEntities;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace DAL
{
   public class DAL_UserManagement
    {
       string ConnString = DataAccess.ConnString;

       public DataTable GetFormName()
       {
           DataTable dt = new DataTable();
           try
           {
               SqlConnection Conn = new SqlConnection(DataAccess.ConnString);
               string Qry = @"Select FormID,DisplayName As Module from Def_Form where status=1";
               dt = DataAccess.GetDataTableByQry(Qry);
           }
           catch (Exception ex)
           {
               throw ex;
           }
           return dt;

       }

       public DataTable GetUserDetail(int UserID)
       {
           DataTable dt = new DataTable();
           string Qry = @"Select * from Users where UserID ='" + UserID + "' ";
           dt = DataAccess.GetDataTableByQry(Qry);
           return dt;
       }

       public DataTable GetUserRightsDetails(int UserID)
       {
           DataTable dt = new DataTable();
           string Qry = @"Select A.FormID,DisplayName,CanView from Def_UserRights A
                        inner join Def_Form B ON A.FormID = B.FormID
                        where UserID ='" + UserID + "'";
           dt = DataAccess.GetDataTableByQry(Qry);
           return dt;
       }

       public bool SaveRecord(BusinessEntities.BE_Users NewBEObj)
       {
           bool IsSave = false;
           SqlConnection SqlConn = new SqlConnection(DataAccess.ConnString);
           SqlCommand cmd = new SqlCommand();
           try
           {
               if (SqlConn.State == ConnectionState.Closed)
                   SqlConn.Open();
               //string Qry=@"";
               cmd.Connection = SqlConn;
               cmd.CommandText = "Insert_UserDef";
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.AddWithValue("@UserID", NewBEObj.UserID);
               cmd.Parameters.AddWithValue("@UserName", NewBEObj.UserName);
               cmd.Parameters.AddWithValue("@Password", NewBEObj.Password);
               cmd.Parameters.AddWithValue("@Status", NewBEObj.Status);
               cmd.Parameters.AddWithValue("@Created_At", NewBEObj.Created_At);
               cmd.Parameters.AddWithValue("@Created_Id", NewBEObj.Created_Id);
               int i = cmd.ExecuteNonQuery();
               cmd.Parameters.Clear();
               if (i != 0)
               {
                   cmd.CommandText = "Insert_UserRights";
                   cmd.CommandType = CommandType.StoredProcedure;

                   if (NewBEObj.dtUserRight.Rows.Count > 0)
                   {
                       foreach (DataRow dr in NewBEObj.dtUserRight.Rows)
                       {
                           cmd.Parameters.AddWithValue("@UserID", dr["UserID"]);
                           cmd.Parameters.AddWithValue("@FormID", dr["FormID"]);
                           cmd.Parameters.AddWithValue("@CanView", dr["CanView"]);
                           cmd.Parameters.AddWithValue("@Created_At", NewBEObj.Created_At);
                           cmd.Parameters.AddWithValue("@Created_Id", NewBEObj.Created_Id);
                           cmd.ExecuteNonQuery();
                           cmd.Parameters.Clear();
                       }
                   }
                   IsSave = true;
               }
               else
               {
                   IsSave = false;
               }
           }
           catch (Exception ex)
           {
               IsSave = false;
               MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
               //throw ex;
           }

           return IsSave;
       }

       public bool UpdateRecord(BusinessEntities.BE_Users NewBEObj)
       {
           bool IsUpdate = false;
           SqlConnection SqlConn = new SqlConnection(DataAccess.ConnString);
           SqlCommand cmd = new SqlCommand();
           try
           {
               if (SqlConn.State == ConnectionState.Closed)
                   SqlConn.Open();
               cmd.Connection = SqlConn;
               cmd.CommandText = "Update_UsrDef";
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.AddWithValue("@UserID", NewBEObj.UserID);
               cmd.Parameters.AddWithValue("@UserName", NewBEObj.UserName);
               cmd.Parameters.AddWithValue("@Password", NewBEObj.Password);
               cmd.Parameters.AddWithValue("@Status", NewBEObj.Status);
               cmd.Parameters.AddWithValue("@Modify_At", NewBEObj.Modify_At);
               cmd.Parameters.AddWithValue("@Modify_Id", NewBEObj.Modify_Id);
               int i = cmd.ExecuteNonQuery();
               cmd.Parameters.Clear();
               if (i != 0)
               {
                   cmd.CommandText = "Delete_User_Rights";
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.Parameters.AddWithValue(@"UserID", NewBEObj.UserID);
                   cmd.ExecuteNonQuery();
                   cmd.Parameters.Clear();

                   cmd.CommandText = "Insert_UserRights";
                   cmd.CommandType = CommandType.StoredProcedure;
                   if (NewBEObj.dtUserRight != null && NewBEObj.dtUserRight.Rows.Count > 0)
                   {
                       foreach (DataRow dr in NewBEObj.dtUserRight.Rows)
                       {
                           cmd.Parameters.AddWithValue("@UserID", dr["UserID"]);
                           cmd.Parameters.AddWithValue("@FormID", dr["FormID"]);
                           cmd.Parameters.AddWithValue("@CanView", dr["CanView"]);
                           cmd.Parameters.AddWithValue("@Created_At", NewBEObj.Modify_At);
                           cmd.Parameters.AddWithValue("@Created_Id", NewBEObj.Modify_Id);
                           cmd.ExecuteNonQuery();
                           cmd.Parameters.Clear();
                       }
                   }
                   IsUpdate = true;
               }
           }
           catch (Exception ex)
           {
               IsUpdate = false;
               MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
               //throw ex;
           }
           return IsUpdate;
       }

       public bool DeleteRecord(int UserID)
       {
           bool IsDelete = false;
           string Qry = "Delete_User_Def";
           SqlConnection SqlConn = new SqlConnection(DataAccess.ConnString);
           SqlCommand cmd = new SqlCommand();
           try
           {
               if (SqlConn.State == ConnectionState.Closed)
                   SqlConn.Open();
               cmd.Connection = SqlConn;
               cmd.CommandText = Qry;
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.AddWithValue("@UserID", UserID);
               int i = cmd.ExecuteNonQuery();
               cmd.Parameters.Clear();
               if (i != 0)
               {
                   IsDelete = true;
               }
               else
               {
                   IsDelete = false;
               }
           }
           catch (Exception ex)
           {
               IsDelete = false;
               MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
               //throw ex;
           }
           return IsDelete;
       }

    }
}
