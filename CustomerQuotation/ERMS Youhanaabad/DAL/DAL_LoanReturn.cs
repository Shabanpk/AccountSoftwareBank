using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using BusinessEntities;
using System.Data;
using Infragistics.Win.UltraWinGrid;

namespace DAL
{
   public class DAL_LoanReturn
    {
       private long LoanReturnID = 0L;

        string ConnString = DataAccess.ConnString;

        public void LoadAssignPersonName(UltraCombo cbo)
        {
            SqlConnection connection = new SqlConnection(this.ConnString);
            //SqlCommand selectCommand = new SqlCommand("Select PersonID,PersonName From Trans_LoanAssign Where IsActive=1", connection);
            SqlCommand selectCommand = new SqlCommand("Select A.PersonID As [PersonID],B.PersonName As [PersonName] From Trans_LoanPosting A inner join Trans_LoanAssign B ON A.PersonID=B.PersonID Where A.IsActive=1", connection);
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

        public void LoadMobileNo(UltraCombo cbo, int PersonID)
        {
            //            string Qry=@"Select A.PersonID,B.PersonPhone from Trans_LoanPosting A
            //inner join Trans_LoanAssign B ON A.PersonID=B.PersonID
            //where LoanPostingID='" + PersonID + "' and A.IsActive=1";
            //string Qry = @"Select PersonID,PersonPhone from Trans_LoanAssign A where PersonID='" + PersonID + "' ";
            string Qry = @"Select A.PersonID,B.PersonPhone from Trans_LoanPosting A
inner join Trans_LoanAssign B ON A.PersonID = B.PersonID
where A.PersonID= '"+PersonID+"' and A.IsActive=1";
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

        public void Insert_Record(BE_LoanReturn NewBEObj)
        {
            SqlTransaction sqlTransaction = (SqlTransaction)null;
            try
            {
                SqlConnection sqlConnection = new SqlConnection(this.ConnString);
                SqlCommand command = sqlConnection.CreateCommand();
                sqlConnection.Open();
                sqlTransaction = sqlConnection.BeginTransaction();
                //command.CommandText = "INSERT INTO Trans_InvestmentReturn\r\n                                        (InvestmentReturnID, InvestorID, ModifiedDate,ReturnDate,ProfitSharing,Amount)\r\n                                        VALUES     (@InvestmentReturnID, @InvestorID, @ModifiedDate,@ReturnDate, @ProfitSharing,@Amount)";
                command.CommandText = @"insert into Trans_LoanReturn (LoanReturnID,PersonID,Amount,Modify_At,ReturnDate) Values (@LoanReturnID,@PersonID,@Amount,@Modify_At,@ReturnDate)";
                command.Transaction = sqlTransaction;
                this.LoanReturnID = int.Parse(DataAccess.GetMaxNO("LoanReturnID", "Trans_LoanReturn").ToString());
                NewBEObj.LoanReturnID = this.LoanReturnID;
                command.Parameters.AddWithValue("@LoanReturnID", (object)NewBEObj.LoanReturnID);
                command.Parameters.AddWithValue("@PersonID", (object)NewBEObj.PersonID);
                command.Parameters.AddWithValue("@ReturnDate", (object)NewBEObj.ReturnDate);
                command.Parameters.AddWithValue("@Modify_At", (object)"1900-01-01");
                command.Parameters.AddWithValue("@Amount", (object)NewBEObj.Amount);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
                double num = NewBEObj.Amount;
                command.CommandText = "Insert_CashBalance";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CashInDate", (object)NewBEObj.ReturnDate);
                command.Parameters.AddWithValue("@TableID", (object)NewBEObj.LoanReturnID);
                command.Parameters.AddWithValue("@TableName", (object)"Trans_LoanReturn");
                command.Parameters.AddWithValue("@Dr", (object)0);
                command.Parameters.AddWithValue("@Cr", (object)num);
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
            }
        }

        public void Update_Record(BE_LoanReturn NewUpdateObj)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(this.ConnString);
                sqlConnection.Open();
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                SqlCommand command = sqlConnection.CreateCommand();
                command.Transaction = sqlTransaction;
                double num =NewUpdateObj.Amount;
                command.CommandText = " UPDATE CashBalance\r\n                                                       SET Cr =@Cr \r\n                                                       WHERE (TableName LIKE '%Trans_LoanReturn%') AND (TableID = @TableID)";
                command.Parameters.AddWithValue("@TableID", (object)NewUpdateObj.LoanReturnID);
                command.Parameters.AddWithValue("@Cr", (object)num);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
                command.CommandText = "Update Trans_LoanReturn Set PersonID=@PersonID,Modify_At=@Modify_At,Amount=@Amount where LoanReturnID=@LoanReturnID";
                //command.CommandText = Qry;
                command.Parameters.AddWithValue("@LoanReturnID", (object)NewUpdateObj.LoanReturnID);
                command.Parameters.AddWithValue("@Modify_At", (object)NewUpdateObj.Modify_At);
                command.Parameters.AddWithValue("@PersonID", (object)NewUpdateObj.PersonID);
                command.Parameters.AddWithValue("@Amount", (object)NewUpdateObj.Amount);
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

        public DataTable GetInvestmentDetails(long ReturnID)
        {
            DataTable dataTable1 = new DataTable();
            DataTable dataTable2;
            try
            {
                SqlConnection sqlConnection = new SqlConnection(this.ConnString);
                string Qry = @"Select * from Trans_LoanReturn A inner join Trans_LoanAssign B ON A.PersonID = B.PersonID where LoanReturnID='"+ReturnID+"'";
                SqlCommand sqlcomm = new SqlCommand(Qry, sqlConnection);
                //sqlcomm.Parameters.AddWithValue("@LoanReturnID", (object)ReturnID);
                dataTable2 = DataAccess.GetDataTableByQry(Qry);
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return dataTable1 = (DataTable)null;
            }
            return dataTable2;
        }

    }
}
