using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BusinessEntities;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DAL
{
   public class DAL_AssignLoan
    {
       string ConnString = DataAccess.ConnString;

      public void SaveRecord(BE_LoanAssign NewBEObj)
       {
           try
           {
               SqlConnection sqlConnection = new SqlConnection(this.ConnString);
               //SqlCommand command = sqlConnection.CreateCommand();
               SqlCommand command = new SqlCommand("InsertLoanAssign", sqlConnection);
               sqlConnection.Open();
               SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
               //string str = "INSERT INTO trans_Investors\r\n                               (InvestorsID,InvestmentDate,ModifiedDate, PersonName, PersonPhone,PersonNIC, PersonEmail, PersonAddress,  IsActive)\r\n                               VALUES     (@InvestorsID,@InvestmentDate,@ModifiedDate, @PersonName, @PersonPhone,@PersonNIC, @PersonEmail, @PersonAddress,  @IsActive)";
               NewBEObj.PersonID = int.Parse(DataAccess.GetMaxNO("PersonID", "Trans_LoanAssign").ToString());
               //command.CommandText = str;
               command.CommandType = CommandType.StoredProcedure;
               command.Transaction = sqlTransaction;
               command.Parameters.AddWithValue("@PersonID", (object)NewBEObj.PersonID);
               command.Parameters.AddWithValue("@PersonName", (object)NewBEObj.PersonName);
               command.Parameters.AddWithValue("@PersonPhone", (object)NewBEObj.PersonPhone);
               command.Parameters.AddWithValue("@PersonCNIC", (object)NewBEObj.PersonNIC);
               command.Parameters.AddWithValue("@PersonEmail", (object)NewBEObj.PersonEmail);
               command.Parameters.AddWithValue("@PersonAddress", (object)NewBEObj.PersonAddress);
               command.Parameters.AddWithValue("@Created_At", (object)NewBEObj.Created_At);
               command.Parameters.AddWithValue("@CityName", (object)NewBEObj.CityName);
               command.Parameters.AddWithValue("@Modify_At", NewBEObj.Modify_At);
               command.Parameters.AddWithValue("@IsActive", NewBEObj.IsActive);
               command.ExecuteNonQuery();
               command.Parameters.Clear();
               sqlTransaction.Commit();
               sqlConnection.Close();
               command.Dispose();
           }
           catch (Exception ex)
           {
               int num = (int)MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
           }
       }

       public void UpdateRecord(BE_LoanAssign NewBEObj)
       {
           try
           {
               SqlConnection sqlConnection = new SqlConnection(this.ConnString);
               //SqlCommand command = sqlConnection.CreateCommand();
               SqlCommand command = new SqlCommand("UpdateLoanAssign", sqlConnection);
               sqlConnection.Open();
               SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
               //string str = "UPDATE Trans_Investors\r\n                               SET ModifiedDate =@ModifiedDate, \r\n                               PersonName =@PersonName, \r\n                               PersonPhone=@PersonPhone,\r\n                               PersonNIC =@PersonNIC, \r\n                               PersonEmail =@PersonEmail, \r\n                               PersonAddress =@PersonAddress, \r\n                               IsActive =@IsActive\r\n                               Where InvestorsID=@InvestorsID";
               //command.CommandText = str;
               //command.CommandType = CommandType.Text;
               command.CommandType= CommandType.StoredProcedure;
               command.Transaction = sqlTransaction;
               command.Parameters.AddWithValue("@PersonID", (object)NewBEObj.PersonID);
               command.Parameters.AddWithValue("@PersonName", (object)NewBEObj.PersonName);
               command.Parameters.AddWithValue("@PersonPhone", (object)NewBEObj.PersonPhone);
               command.Parameters.AddWithValue("@PersonCNIC", (object)NewBEObj.PersonNIC);
               command.Parameters.AddWithValue("@PersonEmail", (object)NewBEObj.PersonEmail);
               command.Parameters.AddWithValue("@PersonAddress", (object)NewBEObj.PersonAddress);
               command.Parameters.AddWithValue("@CityName", (object)NewBEObj.CityName);
               command.Parameters.AddWithValue("@Modify_At", NewBEObj.Modify_At);
               command.Parameters.AddWithValue("@IsActive", NewBEObj.IsActive);
               command.ExecuteNonQuery();
               command.Parameters.Clear();
               sqlTransaction.Commit();
               sqlConnection.Close();
               command.Dispose();
           }
           catch (Exception ex)
           {
               int num = (int)MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
           }
       }

       public DataTable GetInvestmentDetails(long InvestorsID)
       {
           DataTable dataTable = new DataTable();
           try
           {
               SqlConnection sqlconn = new SqlConnection(this.ConnString);
               SqlCommand sqlcomm = new SqlCommand();
               string str = "Select * From Trans_LoanAssign where PersonID=@PersonID";
               sqlcomm.CommandText = str;
               sqlcomm.Parameters.AddWithValue("@PersonID", (object)InvestorsID);
               dataTable = DataAccess.GetDataTable(sqlcomm, sqlconn);
           }
           catch (Exception ex)
           {
               int num = (int)MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
           }
           return dataTable;
       }

    }
}
