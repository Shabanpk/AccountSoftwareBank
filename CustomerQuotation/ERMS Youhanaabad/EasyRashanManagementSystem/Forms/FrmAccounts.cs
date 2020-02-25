using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using DAL;
using System.Data.SqlClient;
using KansaiProject.MainForm;

namespace EasyRashanManagementSystem.Forms
{
    public partial class FrmAccounts : Form
    {
        string ConnString = DataAccess.ConnString;
        //private long CustomerID = 0L;

        #region Modifiers

        long MUnitID = 0;
        long EditID = 0;
        bool GetRecord = false;
        bool UpdateRecord = false;
        long lastAddedTransactionID = 0;
        long CustomerID = 0;


        #endregion

        #region Form Move Variables & Methods

        private const int HT_CAPTION = 0x2;
        private const int WM_NCLBUTTONDOWN = 0x00A1;
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern bool ReleaseCapture();
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private void PanelHeader_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        #endregion

        public FrmAccounts()
        {
            InitializeComponent();
        }

        private bool ValidateEntry()
        {

            if (txtCredit.Text.Trim().Equals("") && txtDebit.Text.Trim().Equals(""))
            {
                MessageBox.Show("Please Enter the Credit or Debit Value", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtCredit.Focus();
                return false;
            }
            else if (txtCredit.Text.Trim().Equals("0") && txtDebit.Text.Trim().Equals("0"))
            {
                MessageBox.Show("Please Enter the Credit or Debit Value", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtCredit.Focus();
                return false;
            }
            else if (!txtCredit.Text.Trim().Equals("") && !txtDebit.Text.Trim().Equals(""))
            {
                MessageBox.Show("Please Enter one value Credit or Debit", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtCredit.Focus();
                return false;
            }
            else if (txtCredit.Text.Trim().Equals("0"))
            {
                MessageBox.Show("Please Enter Credit value", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtCredit.Focus();
                return false;
            }
            else if (txtDebit.Text.Trim().Equals("0"))
            {
                MessageBox.Show("Please Enter Debit Value", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtDebit.Focus();
                return false;
            }
          
            return true;
        }

        public void FormDesign()
        {
            PanelHeader.BackColor = GlobalVaribles.PanelHeader;
            lblheader.BackColor = GlobalVaribles.PanelHeader;
            lblheader.ForeColor = GlobalVaribles.lblheaderforeColor;
            btnClose.BackColor = GlobalVaribles.btnCloseBackColor;
            btnClose.FlatAppearance.BorderSize = GlobalVaribles.btnCloseBorder;
            this.BackColor = GlobalVaribles.FormColor;
            btnSearch.Appearance.BackColor = GlobalVaribles.btnBackColor;
            btnSearch.Appearance.ForeColor = GlobalVaribles.btnForeColor;
            btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            btnSearch.Font = GlobalVaribles.btnFontStyle;
            btnOK.Appearance.BackColor = GlobalVaribles.btnBackColor;
            btnOK.Appearance.ForeColor = GlobalVaribles.btnForeColor;
            btnOK.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            btnOK.Font = GlobalVaribles.btnFontStyle;
            btnClear.Appearance.BackColor = GlobalVaribles.btnBackColor;
            btnClear.Appearance.ForeColor = GlobalVaribles.btnForeColor;
            btnClear.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            btnClear.Font = GlobalVaribles.btnFontStyle;
            btnUpdate.Appearance.BackColor = GlobalVaribles.btnBackColor;
            btnUpdate.Appearance.ForeColor = GlobalVaribles.btnForeColor;
            btnUpdate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            btnUpdate.Font = GlobalVaribles.btnFontStyle;
            btnSearch.Appearance.BackColor = GlobalVaribles.btnBackColor;
            btnSearch.Appearance.ForeColor = GlobalVaribles.btnForeColor;
            btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            btnSearch.Font = GlobalVaribles.btnFontStyle;
            PanelFooter.BackColor = GlobalVaribles.PanelFooter;
        }

        private void ClearForm()
        {
            UpdateRecord = false;
            CustomerID = 0;
            txtReferenceNumber.Text = "";
            txtCustomerName.Text = "";
            txtPhoneNo.Text = "";
            txtTransactionNumber.Text = "";
            txtCredit.Text = "";
            txtDebit.Text = "";
            txtDescription.Text = "";
            btnOK.Enabled = true;
            btnUpdate.Enabled = false;
            txtCredit.Focus();
           // GetMaxCustomerID();
             MUnitID = 0;
             EditID = 0;
             GetRecord = false;
             lastAddedTransactionID = 0;
             dtpTransactionDate.Value = DateTime.Now;
        }

        private static DataTable GetDataTable(SqlCommand sqlcomm, SqlConnection sqlconn)
        {

            sqlcomm.Connection = sqlconn;
            SqlDataAdapter da = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        private string GenerateTransactionID(long CustomerID)
        {
            string Transnumber = "";
            SqlConnection sqlconn = new SqlConnection(ConnString);
            sqlconn.Open();
            SqlCommand sqlcomm = new SqlCommand("Select IsNull(Max(TransactionNumber),0)  As [TransactionNumber] From Trans_AccountsD where customerID='"+CustomerID+"'  ", sqlconn);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sqlcomm);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0][0].ToString() == "0")
                {
                    lastAddedTransactionID = long.Parse(dt.Rows[0]["TransactionNumber"].ToString());
                }
                else
                {
                    Transnumber = Convert.ToString(dt.Rows[0]["TransactionNumber"].ToString());
                    //Transnumber = Convert.ToString(lastAddedTransactionID.ToString());
                    string[] ID = Transnumber.Split('-');
                    lastAddedTransactionID = long.Parse(ID[1]);

                    //string[] ID = lblId.Text.Split(':');
                    //NewBEObj.CityID = Convert.ToInt32(ID[1]);
                }
                
            }
            // txtCustomerID.Text = MUnitID.ToString();
            //txtReferenceNumber.Text = "Ref-" + txtCustomerID.Text;

            sqlconn.Close();
            da.Dispose();
            sqlcomm.Dispose();
            dt.Dispose();
           // lastAddedTransactionID = long.Parse("0001");
            string demo = Convert.ToString(lastAddedTransactionID + 1).PadLeft(4, '0');
            return demo;
        }

        private bool InserAccountDetails()
        {
            bool IsSave = false;
            SqlTransaction sqlTransaction = (SqlTransaction)null;
            try
            {
                string Qry = @"Insert into Trans_AccountsD(CustomerID,TransactionNumber,TransactionDate,CreditAmount,DebitAmount,Description,Created_At,Created_Id,Modify_At,IsActive)
                                Values
                                (@CustomerID,@TransactionNumber,@TransactionDate,@CreditAmount,@DebitAmount,@Description,@Created_At,@Created_Id,@Modify_At,@IsActive)";
                SqlConnection sqlConnection = new SqlConnection(this.ConnString);
                SqlCommand command = new SqlCommand(Qry, sqlConnection);
                sqlConnection.Open();
                sqlTransaction = sqlConnection.BeginTransaction();
                //command.CommandText = "INSERT INTO Trans_InvestmentPosting\r\n                                        (InvestmentPostingID,InvestorsID, InvestmentDate, ModifiedDate, Amount, IsActive)\r\n                                        VALUES (@InvestmentPostingID,@InvestorsID, @InvestmentDate, @ModifiedDate, @Amount, @IsActive)";
                command.Transaction = sqlTransaction;
                command.CommandType = CommandType.Text;
                command.CommandText = Qry;
               // this.CustomerID = int.Parse(DataAccess.GetMaxNO("CustomerID", "Trans_AccountsM").ToString());
                // NewBEObj.ExpenseID = this.InvestmentPosting;
                command.Parameters.AddWithValue("@CustomerID", CustomerID);
                command.Parameters.AddWithValue("@TransactionNumber", txtTransactionNumber.Text);
                command.Parameters.AddWithValue("@TransactionDate", Convert.ToDateTime(dtpTransactionDate.Value));
                if (txtCredit.Text.Trim().Equals("0") || txtCredit.Text.Trim().Equals(""))
                {
                    txtCredit.Text = "NULL";
                }
                if (txtDebit.Text.Trim().Equals("0") || txtDebit.Text.Trim().Equals(""))
                {
                    txtDebit.Text = "NULL";
                }
                command.Parameters.AddWithValue("@CreditAmount", txtCredit.Text);
                command.Parameters.AddWithValue("@DebitAmount", txtDebit.Text);
                command.Parameters.AddWithValue("@Description", txtDescription.Text);
                command.Parameters.AddWithValue("@Created_At", DateTime.Now);
                command.Parameters.AddWithValue("@Created_Id", GlobalVaribles.UserID);
                command.Parameters.AddWithValue("@Modify_At", (object)"1900-01-01");
                command.Parameters.AddWithValue("@IsActive", chkIsActive.CheckedValue);

                command.ExecuteNonQuery();
                command.Parameters.Clear();
                sqlTransaction.Commit();
                sqlConnection.Close();
                command.Dispose();
                sqlTransaction.Dispose();
                IsSave = true;
                
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                sqlTransaction.Rollback();
            }
            return IsSave;
        }

        bool UpdateAccountDetails()
        {
            bool IsSave = false;
            SqlTransaction sqlTransaction = (SqlTransaction)null;
            try
            {
//                string Qry = @"Insert into Trans_AccountsD(CustomerID,TransactionNumber,TransactionDate,CreditAmount,DebitAmount,Created_At,Created_Id,Modify_At,IsActive)
//                                Values
//                                (@CustomerID,@TransactionNumber,@TransactionDate,@CreditAmount,@DebitAmount,@Created_At,@Created_Id,@Modify_At,@IsActive)";

                string Qry = @"Update Trans_AccountsD
                                set
                                TransactionDate=@TransactionDate,
                                CreditAmount=@CreditAmount,
                                DebitAmount=@DebitAmount,
                                Description=@Description,
                                Modify_At=@Modify_At,
                                Modify_Id=@Modify_Id,
                                IsActive=@IsActive
                                where TransactionNumber=@TransactionNumber";
                SqlConnection sqlConnection = new SqlConnection(this.ConnString);
                SqlCommand command = new SqlCommand(Qry, sqlConnection);
                sqlConnection.Open();
                sqlTransaction = sqlConnection.BeginTransaction();
                command.Transaction = sqlTransaction;
                command.CommandType = CommandType.Text;
                command.CommandText = Qry;
                // this.CustomerID = int.Parse(DataAccess.GetMaxNO("CustomerID", "Trans_AccountsM").ToString());
                // NewBEObj.ExpenseID = this.InvestmentPosting;
                command.Parameters.AddWithValue("@TransactionNumber", txtTransactionNumber.Text);
                
                command.Parameters.AddWithValue("@TransactionDate", Convert.ToDateTime(dtpTransactionDate.Value));
                if (txtCredit.Text.Trim().Equals("0") || txtCredit.Text.Trim().Equals(""))
                {
                    txtCredit.Text = "NULL";
                }
                if (txtDebit.Text.Trim().Equals("0") || txtDebit.Text.Trim().Equals(""))
                {
                    txtDebit.Text = "NULL";
                }
                command.Parameters.AddWithValue("@CreditAmount", txtCredit.Text);
                command.Parameters.AddWithValue("@DebitAmount", txtDebit.Text);
                command.Parameters.AddWithValue("@Description", txtDescription.Text);

                command.Parameters.AddWithValue("@Modify_Id", GlobalVaribles.UserID);
                command.Parameters.AddWithValue("@Modify_At", DateTime.Now);
                command.Parameters.AddWithValue("@IsActive", chkIsActive.CheckedValue);

                command.ExecuteNonQuery();
                command.Parameters.Clear();
                sqlTransaction.Commit();
                sqlConnection.Close();
                command.Dispose();
                sqlTransaction.Dispose();
                IsSave = true;

            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                sqlTransaction.Rollback();
            }
            return IsSave;
        }

        private void FrmAccounts_Load(object sender, EventArgs e)
        {
            FormDesign();
            //GetMaxCustomerID();
            btnUpdate.Enabled = false;
            txtCustomerName.Focus();
            dtpTransactionDate.Value = DateTime.Now;
        }

        private void FrmAccounts_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FrmAccounts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                FrmConfirmationMessage message = new FrmConfirmationMessage(GlobalVaribles.ConfirmationMsg, 2, this);
                message.ShowDialog();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCredit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                    (e.KeyChar != '.'))
                {
                        e.Handled = true;
                }

                // only allow one decimal point
                if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
                {
                    e.Handled = true;
                }
        }

        private void txtDebit_KeyPress(object sender, KeyPressEventArgs e)
        {
         if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                    (e.KeyChar != '.'))
                {
                        e.Handled = true;
                }

                // only allow one decimal point
                if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
                {
                    e.Handled = true;
                }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateEntry())
            {
                if (CustomerID != 0)
                {
                    if (InserAccountDetails())
                    {
                        MessageBox.Show("Record Inserted Successfully", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearForm();
                    }
                }
                else
                {
                    MessageBox.Show("Please Select the Reference Number", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtCredit.Focus();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            object rValue;
            try
            {

                rValue = FrmGSearch.Show("Select TransactionNumber,TransactionNumber from Trans_AccountsD", false, "All Reference Numbers");
                if (rValue == null)
                    return;
                //EditID = long.Parse(rValue.ToString());
                string trans = rValue.ToString();
                DataTable dt = new DataTable();
                SqlConnection sqlconn = new SqlConnection(ConnString);
                sqlconn.Open();
               // string Qry = @"Select CustomerId,CustomerName,Referenceno,PhoneNo,Address,IsActive From Trans_AccountsM Where CustomerId=@CustomerID";
                string Qry = @"Select A.customerID,CustomerName,ReferenceNo,PhoneNo,TransactionNumber,TransactionDate,
                                CreditAmount,DebitAmount,Description,B.IsActive from Trans_AccountsM A
                                inner join Trans_AccountsD B on A.CustomerId = B.CustomerId
                                where TransactionNumber=@TransactionNumber";
                SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
                //SqlCommand sqlcomm = new SqlCommand("GetMUnitName", sqlconn);
                //sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@TransactionNumber", trans);
                dt = GetDataTable(sqlcomm, sqlconn);
                if (dt.Rows.Count > 0)
                {
                    UpdateRecord = true;
                    GetRecord = true;
                    //txtCustomerID.Text = dt.Rows[0]["CustomerID"].ToString();
                    txtReferenceNumber.Text = dt.Rows[0]["ReferenceNo"].ToString();
                    txtCustomerName.Text = dt.Rows[0]["CustomerName"].ToString();
                    txtPhoneNo.Text = dt.Rows[0]["PhoneNo"].ToString();
                    txtTransactionNumber.Text = dt.Rows[0]["TransactionNumber"].ToString();
                    dtpTransactionDate.Value= Convert.ToDateTime(dt.Rows[0]["TransactionDate"].ToString());
                    //Convert.ToDateTime(dtpInvestmentDate.Value);
                    txtCredit.Text = dt.Rows[0]["CreditAmount"].ToString();
                    txtDebit.Text = dt.Rows[0]["DebitAmount"].ToString();
                    txtDescription.Text = dt.Rows[0]["Description"].ToString();
                    if (dt.Rows[0]["DebitAmount"].ToString() == "NULL")
                    {
                        txtDebit.Text = "";
                    }
                    if (dt.Rows[0]["CreditAmount"].ToString() == "NULL")
                    {
                        txtCredit.Text = "";
                    }
                    chkIsActive.CheckedValue = bool.Parse(dt.Rows[0]["IsActive"].ToString());
                    btnUpdate.Enabled = true;
                    btnOK.Enabled = false;

                }
                dt.Dispose();
                sqlcomm.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            object rValue;
            try
            {

                rValue = FrmGSearch.Show("Select CustomerID,ReferenceNo,CustomerName,PhoneNo From Trans_AccountsM", false, "All Customers");
                if (rValue == null)
                    return;
                MUnitID = long.Parse(rValue.ToString());
                DataTable dt = new DataTable();
                SqlConnection sqlconn = new SqlConnection(ConnString);
                sqlconn.Open();
                string Qry = @"Select CustomerId,CustomerName,Referenceno,PhoneNo,Address,IsActive From Trans_AccountsM Where CustomerId=@CustomerID";
                SqlCommand sqlcomm = new SqlCommand(Qry, sqlconn);
                sqlcomm.Parameters.AddWithValue("@CustomerID", MUnitID);
                dt = GetDataTable(sqlcomm, sqlconn);
                if (dt.Rows.Count > 0)
                {
                    // UpdateRecord = true;
                    //GetRecord = true;
                    //txtCustomerID.Text = dt.Rows[0]["CustomerID"].ToString();
                    CustomerID = long.Parse(dt.Rows[0]["CustomerID"].ToString());
                    txtReferenceNumber.Text = dt.Rows[0]["ReferenceNo"].ToString();
                    txtCustomerName.Text = dt.Rows[0]["CustomerName"].ToString();
                    txtPhoneNo.Text = dt.Rows[0]["PhoneNo"].ToString();

                    txtTransactionNumber.Text = txtReferenceNumber.Text + "-" + GenerateTransactionID(CustomerID).ToString();
                    //  btnEdit.Enabled = true;
                    //btnOK.Enabled = false;

                }
                dt.Dispose();
                sqlcomm.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (ValidateEntry())
            {
                if (txtReferenceNumber.Text.ToString() != "")
                {
                    if (UpdateAccountDetails())
                    {
                        MessageBox.Show("Record Updated Successfully", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearForm();
                    }
                }
                else
                {
                    MessageBox.Show("Please Select the Reference Number", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtCredit.Focus();
                }
            }
        }

    }
}
