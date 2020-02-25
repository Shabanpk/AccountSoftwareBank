using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infragistics.Win.UltraWinGrid;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using BusinessEntities;

namespace DAL
{
   public class DAL_LoanPosting
    {
        private long InvestmentPosting = 0L;

        string ConnString = DataAccess.ConnString;

        public void LoadAssignPersonName(UltraCombo cbo)
        {
            SqlConnection connection = new SqlConnection(this.ConnString);
            SqlCommand selectCommand = new SqlCommand("Select PersonID,PersonName From Trans_LoanAssign Where IsActive=1", connection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            cbo.DataSource = dataTable;
            cbo.ValueMember = "PersonID";
            cbo.DisplayMember = "PersonName";
            cbo.DisplayLayout.Bands[0].Columns["PersonID"].Hidden = true;
            cbo.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            cbo.Value = 0;
            connection.Close();
            sqlDataAdapter.Dispose();
            selectCommand.Dispose();
        }

        public void LoadMobileNo(UltraCombo cbo,int PersonID)
        {
//            string Qry=@"Select A.PersonID,B.PersonPhone from Trans_LoanPosting A
//inner join Trans_LoanAssign B ON A.PersonID=B.PersonID
//where LoanPostingID='" + PersonID + "' and A.IsActive=1";
            string Qry = @"Select PersonID,PersonPhone from Trans_LoanAssign A where PersonID='"+PersonID+"' ";
            SqlConnection connection = new SqlConnection(this.ConnString);
            SqlCommand selectCommand = new SqlCommand(Qry, connection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            cbo.DataSource = dataTable;
            cbo.ValueMember = "PersonID";
            cbo.DisplayMember = "PersonPhone";
            cbo.DisplayLayout.Bands[0].Columns["PersonID"].Hidden = true;
            cbo.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            //cbo.Value = 1;

            connection.Close();
            sqlDataAdapter.Dispose();
            selectCommand.Dispose();
        }

        public void Insert_Record(BE_LoanPosting NewBEObj)
        {
            SqlTransaction sqlTransaction = (SqlTransaction)null;
            try
            {
                SqlConnection sqlConnection = new SqlConnection(this.ConnString);
                SqlCommand command = new SqlCommand("InsertLoanPosting", sqlConnection);
                sqlConnection.Open();
                sqlTransaction = sqlConnection.BeginTransaction();
                //command.CommandText = "INSERT INTO Trans_InvestmentPosting\r\n                                        (InvestmentPostingID,InvestorsID, InvestmentDate, ModifiedDate, Amount, IsActive)\r\n                                        VALUES (@InvestmentPostingID,@InvestorsID, @InvestmentDate, @ModifiedDate, @Amount, @IsActive)";
                command.Transaction = sqlTransaction;
                command.CommandType = CommandType.StoredProcedure;
                this.InvestmentPosting =int.Parse(DataAccess.GetMaxNO("LoanPostingID","Trans_LoanPosting").ToString());
                NewBEObj.LoanPostingID = this.InvestmentPosting;
                command.Parameters.AddWithValue("@LoanPostingID", (object)NewBEObj.LoanPostingID);
                command.Parameters.AddWithValue("@PersonID", (object)NewBEObj.PersonID);
                command.Parameters.AddWithValue("@Amount", (object)NewBEObj.Amount);
                command.Parameters.AddWithValue("@Created_At", (object)NewBEObj.Created_At);
                command.Parameters.AddWithValue("@Modify_At", (object)"1900-01-01");
                command.Parameters.AddWithValue("@IsActive", NewBEObj.IsActive);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
                command.CommandText = "Insert_CashBalance";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CashInDate", (object)NewBEObj.Created_At);
                command.Parameters.AddWithValue("@TableID", (object)NewBEObj.LoanPostingID);
                command.Parameters.AddWithValue("@TableName", (object)"Trans_InvestmentPosting");
                command.Parameters.AddWithValue("@Dr", (object)NewBEObj.Amount);
                command.Parameters.AddWithValue("@Cr", (object)0);
                command.Parameters.AddWithValue("@IpAddress", (object)"192.168.3.67");
                command.ExecuteNonQuery();
                command.Parameters.Clear();
                sqlTransaction.Commit();
                sqlConnection.Close();
                command.Dispose();
                sqlTransaction.Dispose();
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                sqlTransaction.Rollback();
                //( sqlTransaction.Rollback());
            }
        }

        public void Update_Record(BE_LoanPosting NewUpdateObj)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(this.ConnString);
                sqlConnection.Open();
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                SqlCommand command = sqlConnection.CreateCommand();
                //SqlCommand command = new SqlCommand("UpdateLoanPosting", sqlConnection);
                command.Transaction = sqlTransaction;
                command.CommandText = " UPDATE    CashBalance\r\n                                                       SET Dr =@Dr \r\n                                                       WHERE (TableName LIKE '%Trans_LoanPosting%') AND (TableID = @TableID)";
                command.Parameters.AddWithValue("@TableID", (object)NewUpdateObj.LoanPostingID);
                command.Parameters.AddWithValue("@Dr", (object)NewUpdateObj.Amount);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
                //command.CommandText = "UPDATE Trans_InvestmentPosting\r\n                                         SET InvestorsID =@InvestorsID,\r\n                                         ModifiedDate=@ModifiedDate,\r\n                                         Amount =@Amount\r\n                                         WHERE (InvestmentPostingID = @InvestmentPostingID)";
                command = new SqlCommand("UpdateLoanPosting", sqlConnection);
                command.Transaction = sqlTransaction;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LoanPostingID", (object)NewUpdateObj.LoanPostingID);
                command.Parameters.AddWithValue("@PersonID", (object)NewUpdateObj.PersonID);
                command.Parameters.AddWithValue("@Amount", (object)NewUpdateObj.Amount);
                command.Parameters.AddWithValue("@Modify_At", (object)NewUpdateObj.Modify_At);
                command.Parameters.AddWithValue("@IsActive", NewUpdateObj.IsActive);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
                sqlTransaction.Commit();
                sqlConnection.Close();
                command.Dispose();
                sqlTransaction.Dispose();
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        public DataTable GetInvestmentDetails(long InvestmentID)
        {
            DataTable dataTable1 = new DataTable();
            DataTable dataTable2;
            try
            {
                SqlConnection sqlConnection = new SqlConnection(this.ConnString);
                //SqlCommand sqlcomm = new SqlCommand("Select InvestmentPostingID,INVP.InvestorsID,PersonName,Convert(varchar(11),INVP.InvestmentDate,106) As [InvestmentDate],PersonPhone,PersonNIc,Amount From Trans_InvestmentPosting INVP\r\n                                                      Inner Join Trans_Investors Inv On INV.InvestorsID=INVP.InvestorsID\r\n                                                      where InvestmentPostingID=@InvestmentID", sqlConnection);
                string Qry = @"Select LoanPostingID,A.PersonID,PersonName,Convert(varchar(11),A.Created_At,106) As [Created_At],PersonPhone,PersonCNIC,Amount 
From Trans_LoanPosting A                                                    
Inner Join Trans_LoanAssign B On A.PersonID=B.PersonID
where LoanPostingID=@PersonID";
                SqlCommand sqlcomm = new SqlCommand(Qry, sqlConnection);
                sqlcomm.Parameters.AddWithValue("@PersonID", (object)InvestmentID);
                dataTable2 = DataAccess.GetDataTable(sqlcomm, sqlConnection);
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return dataTable1 = (DataTable)null;
            }
            return dataTable2;
        }

        public DataTable GetDuplicateInvestorID(int PersonID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection sqlConnection = new SqlConnection(this.ConnString);
                string qry = @"Select Count(PersonID) As [PersonID] From Trans_LoanPosting Where PersonID='" + PersonID + "'";
                dt = DataAccess.GetDataTable(qry, sqlConnection);
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in DAL_InvestmentPosting.GetDuplicateInvestorID() \n" + ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return dt = null;
            }
        }
    }
}
