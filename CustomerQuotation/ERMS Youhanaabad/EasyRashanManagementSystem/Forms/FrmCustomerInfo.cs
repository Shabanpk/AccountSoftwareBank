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
using KansaiProject.MainForm;
using System.Data.SqlClient;

namespace EasyRashanManagementSystem.Forms
{
    public partial class FrmCustomerInfo : Form
    {
        string ConnString = DataAccess.ConnString;
        private long CustomerID = 0L;

        #region Modifiers

        long MUnitID = 0;
        bool GetRecord = false;
        bool UpdateRecord = false;

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

        public FrmCustomerInfo()
        {
            InitializeComponent();
        }

        #region Functions

        private void GetMaxCustomerID()
        {
            //string ConnString = "Data Source=HADI-B94B6CB176;Initial Catalog=RashanGhar;User id=sa;password=iloveyoumuhammad786";
            SqlConnection sqlconn = new SqlConnection(ConnString);
            sqlconn.Open();
            SqlCommand sqlcomm = new SqlCommand("Select IsNull(Max(CustomerID),0)+ 1  As [CustomerID] From Def_Customer ", sqlconn);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sqlcomm);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                MUnitID = long.Parse(dt.Rows[0]["CustomerID"].ToString());
            }
            //ClsGlobal NewMaxIDFunction = new ClsGlobal { ColName = "MUnitID", TableName = "MUnit", whereClause = "" };
            //GlobalFunctions NewMaxID = new GlobalFunctions();
            ////MUnitID = NewMaxID.MaxID(NewMaxIDFunction);
            txtCustomerID.Text = MUnitID.ToString();
            sqlconn.Close();
            da.Dispose();
            sqlcomm.Dispose();
            dt.Dispose();
        }

        private void InsertCustomer()
        {
            SqlTransaction sqlTransaction = (SqlTransaction)null;
            try
            {
                string Qry = @"Insert into Def_Customer(CustomerID,CustomerName,ReferenceNo,ContactPerson,PhoneNo,Email,Address,IsAssignBuildingDetails,Created_At,Created_Id,Modify_At,IsActive)
                                Values
                                (@CustomerID,@CustomerName,@ReferenceNo,@ContactPerson,@PhoneNo,@Email,@Address,@IsAssignBuildingDetails,@Created_At,@Created_Id,@Modify_At,@IsActive)";
                SqlConnection sqlConnection = new SqlConnection(this.ConnString);
                SqlCommand command = new SqlCommand(Qry, sqlConnection);
                sqlConnection.Open();
                sqlTransaction = sqlConnection.BeginTransaction();
                //command.CommandText = "INSERT INTO Trans_InvestmentPosting\r\n                                        (InvestmentPostingID,InvestorsID, InvestmentDate, ModifiedDate, Amount, IsActive)\r\n                                        VALUES (@InvestmentPostingID,@InvestorsID, @InvestmentDate, @ModifiedDate, @Amount, @IsActive)";
                command.Transaction = sqlTransaction;
                command.CommandType = CommandType.Text;
                command.CommandText = Qry;
                this.CustomerID = int.Parse(DataAccess.GetMaxNO("CustomerID", "Def_Customer").ToString());
               // NewBEObj.ExpenseID = this.InvestmentPosting;
                command.Parameters.AddWithValue("@CustomerID", CustomerID);
                command.Parameters.AddWithValue("@CustomerName", txtCustomerName.Text);
                command.Parameters.AddWithValue("@ReferenceNo", txtReferenceNumber.Text);
                command.Parameters.AddWithValue("@ContactPerson", txtContactPerson.Text);
                command.Parameters.AddWithValue("@PhoneNo", txtPhoneNo.Text);
                command.Parameters.AddWithValue("@Email",txtEmail.Text);
                command.Parameters.AddWithValue("@Address", txtAddress.Text);
                command.Parameters.AddWithValue("@IsAssignBuildingDetails", false);
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
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, DataAccess.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                sqlTransaction.Rollback();
            }
            //try
            //{
            //    SqlConnection sqlconn = new SqlConnection(ConnString);
            //    sqlconn.Open();
            //    SqlCommand sqlcomm = new SqlCommand("InsertMUnit", sqlconn);
            //    sqlcomm.CommandType = CommandType.StoredProcedure;
            //    sqlcomm.Parameters.AddWithValue("@MUnitID", txtMUnitID.Text);
            //    sqlcomm.Parameters.AddWithValue("@MUnitName", txtMUnitName.Text);
            //    sqlcomm.Parameters.AddWithValue("@IsActive", chkIsActive.CheckedValue);
            //    sqlcomm.ExecuteNonQuery();
            //    sqlconn.Close();
            //    sqlcomm.Dispose();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //}
            ////BE_MUnit NewObjMUnit = new BE_MUnit { MUnitID = int.Parse(txtMUnitID.Text.ToString()), MUnitName = txtMUnitName.Text, IsActive = bool.Parse(chkIsActive.CheckedValue.ToString()) };
            ////BP_Munit NewBpObj = new BP_Munit();
            ////NewBpObj.InsertMUnit(NewObjMUnit);
        }

        private bool ValidateEntry()
        {
            if (txtReferenceNumber.Text.Trim().Equals(""))
            {
                MessageBox.Show("Please Enter reference Number", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtCustomerName.Focus();
                return false;
            }
            else if (txtCustomerName.Text.Trim().Equals(""))
            {
                MessageBox.Show("Please Enter Customer Name", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtCustomerName.Focus();
                return false;
            }
            return true;
        }

        private void ClearForm()
        {
            UpdateRecord = false;
            txtReferenceNumber.Text = "";
            txtCustomerName.Text = "";
            txtCustomerID.Text = "";
            txtContactPerson.Text = "";
            txtPhoneNo.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";
            btnOK.Enabled = true;
            btnEdit.Enabled = false;
            txtCustomerName.Focus();
            GetMaxCustomerID();
        }

        private bool GetDuplicate()
        {
            SqlConnection sqlconn = new SqlConnection(ConnString);
            sqlconn.Open();
            SqlCommand sqlcomm = new SqlCommand("Select CustomerId From Def_Customer Where ReferenceNo='" + txtReferenceNumber.Text + "'", sqlconn);
            DataTable dt = new DataTable();
            dt = GetDataTable(sqlcomm, sqlconn);
            if (dt.Rows.Count > 0)
            {
                GetRecord = true;
            }
            else
            {
                GetRecord = false;
            }
            //BE_MUnit NewBEObj = new BE_MUnit { MUnitName=txtMUnitName.Text.Trim()};
            //BP_Munit NEwBPObj = new BP_Munit();
            //GetRecord = NEwBPObj.DuplicateMUnit(NewBEObj);
            sqlconn.Close();
            sqlcomm.Dispose();
            dt.Dispose();
            return GetRecord;
        }

        public void UpdateCustomer()
        {
            SqlTransaction sqlTransaction = (SqlTransaction)null;
            try
            {
                string Qry = @"Update Def_Customer
                                set
                                CustomerName=@CustomerName,
                                ReferenceNo=@ReferenceNo,
                                ContactPerson=@ContactPerson,
                                PhoneNo=@PhoneNo,
                                Email=@Email,
                                Address=@Address,
                                Modify_At=@Modify_At,
                                Modify_Id=@Modify_Id,
                                IsActive=@IsActive
                                where CustomerID=@CustomerID";
                SqlConnection sqlConnection = new SqlConnection(this.ConnString);
                SqlCommand command = new SqlCommand(Qry, sqlConnection);
                sqlConnection.Open();
                sqlTransaction = sqlConnection.BeginTransaction();
                command.Transaction = sqlTransaction;
                command.CommandType = CommandType.Text;
                command.CommandText = Qry;
                 command.Parameters.AddWithValue("@CustomerID", txtCustomerID.Text);
                command.Parameters.AddWithValue("@CustomerName", txtCustomerName.Text);
                command.Parameters.AddWithValue("@ReferenceNo", txtReferenceNumber.Text);
                command.Parameters.AddWithValue("@ContactPerson", txtContactPerson.Text);
                command.Parameters.AddWithValue("@PhoneNo", txtPhoneNo.Text);
                command.Parameters.AddWithValue("@Email",txtEmail.Text);
                command.Parameters.AddWithValue("@Address", txtAddress.Text);
                command.Parameters.AddWithValue("@Modify_At", DateTime.Now);
                command.Parameters.AddWithValue("@Modify_Id", GlobalVaribles.UserID);
                command.Parameters.AddWithValue("@IsActive", chkIsActive.CheckedValue);
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
            //string a = string.Empty;
            //SqlConnection sqlconn = new SqlConnection(ConnString);
            //SqlCommand sqlcomm = new SqlCommand("UpdateMUnitName", sqlconn);
            //sqlcomm.CommandType = CommandType.StoredProcedure;
            //sqlcomm.Parameters.AddWithValue("@MUnitID", txtMUnitID.Text);
            //sqlcomm.Parameters.AddWithValue("@MUnitName", txtMUnitName.Text);
            //sqlcomm.Parameters.AddWithValue("@IsActive", chkIsActive.CheckedValue);

            //try
            //{
            //    sqlconn.Open();
            //    sqlcomm.ExecuteNonQuery();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //}
            //finally
            //{
            //    sqlconn.Close();
            //    sqlcomm.Dispose();
            //}
            ////BE_MUnit NewBEObj = new BE_MUnit { MUnitID=int.Parse(MUnitID.ToString()),MUnitName=txtMUnitName.Text.Trim(),IsActive=bool.Parse(chkIsActive.CheckedValue.ToString()) };
            ////BP_Munit NewBPObj = new BP_Munit();
            ////NewBPObj.UPdateMUnit(NewBEObj);
        }

        private static DataTable GetDataTable(SqlCommand sqlcomm, SqlConnection sqlconn)
        {
            //string a = ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;
            //string a = "Data Source=HADI-B94B6CB176;Initial Catalog=RashanGhar;User id=sa;password=iloveyoumuhammad786";
            //string a = "Data Source=PHAHAD;Initial Catalog=RashanGhar;User id=sa;password=123456"; 
            //sqlconn.ConnectionString = "Data Source=PHAHAD;Initial Catalog=RashanGhar;User id=sa;password=123456";
            sqlcomm.Connection = sqlconn;
            SqlDataAdapter da = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }


        public void FormDesign()
        {
            PanelHeader.BackColor = GlobalVaribles.PanelHeader;
            lblheader.BackColor = GlobalVaribles.PanelHeader;
            lblheader.ForeColor = GlobalVaribles.lblheaderforeColor;
            btnClose.BackColor = GlobalVaribles.btnCloseBackColor;
            btnClose.FlatAppearance.BorderSize = GlobalVaribles.btnCloseBorder;
            this.BackColor = GlobalVaribles.FormColor;
            btnCustomerID.Appearance.BackColor = GlobalVaribles.btnBackColor;
            btnCustomerID.Appearance.ForeColor = GlobalVaribles.btnForeColor;
            btnCustomerID.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            btnCustomerID.Font = GlobalVaribles.btnFontStyle;
            btnOK.Appearance.BackColor = GlobalVaribles.btnBackColor;
            btnOK.Appearance.ForeColor = GlobalVaribles.btnForeColor;
            btnOK.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            btnOK.Font = GlobalVaribles.btnFontStyle;
            btnClear.Appearance.BackColor = GlobalVaribles.btnBackColor;
            btnClear.Appearance.ForeColor = GlobalVaribles.btnForeColor;
            btnClear.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            btnClear.Font = GlobalVaribles.btnFontStyle;
            btnEdit.Appearance.BackColor = GlobalVaribles.btnBackColor;
            btnEdit.Appearance.ForeColor = GlobalVaribles.btnForeColor;
            btnEdit.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            btnEdit.Font = GlobalVaribles.btnFontStyle;
            PanelFooter.BackColor = GlobalVaribles.PanelFooter;
        }

        #endregion

        #region Events

        private void FrmCustomerStatus_Load(object sender, EventArgs e)
        {
            FormDesign();
            GetMaxCustomerID();
            btnEdit.Enabled = false;
        }

        private void FrmCustomerStatus_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                FrmConfirmationMessage message = new FrmConfirmationMessage(GlobalVaribles.ConfirmationMsg, 2, this);
                message.ShowDialog();
            }
        }

        private void FrmCustomerStatus_Paint(object sender, PaintEventArgs e)
        {
            int width = this.Width - 1;
            int height = this.Height - 1;
            Pen greenPen = new Pen(GlobalVaribles.BorderColor);
            e.Graphics.DrawRectangle(greenPen, 0, 0, width, height);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!UpdateRecord)
            {
                if (ValidateEntry())
                {
                    if (GetDuplicate())
                    {
                        MessageBox.Show("Reference Number Duplicate Record", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        txtReferenceNumber.Focus();
                    }
                    else
                    {
                        InsertCustomer();
                        MessageBox.Show("Record Inserted Successfully", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearForm();
                    }
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (ValidateEntry())
            {
                UpdateCustomer();
                MessageBox.Show("Record Updated Successfully", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                btnEdit.Enabled = false;
                btnOK.Enabled = true;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
            GetMaxCustomerID();
        }

        private void btnCustomerID_Click(object sender, EventArgs e)
        {
            object rValue;
            try
            {

                rValue = FrmGSearch.Show("Select CustomerID,ReferenceNo,CustomerName,PhoneNo,Email From Def_Customer", false, "All Customers");
                if (rValue == null)
                    return;
                MUnitID = long.Parse(rValue.ToString());
                DataTable dt = new DataTable();
                SqlConnection sqlconn = new SqlConnection(ConnString);
                sqlconn.Open();
                string Qry = @"Select CustomerId,CustomerName,Referenceno,ContactPerson,PhoneNo,Email,Address,IsActive From Def_Customer Where CustomerId=@CustomerID";
                SqlCommand sqlcomm = new SqlCommand(Qry,sqlconn);
                //SqlCommand sqlcomm = new SqlCommand("GetMUnitName", sqlconn);
                //sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlcomm.Parameters.AddWithValue("@CustomerID", MUnitID);
                dt = GetDataTable(sqlcomm, sqlconn);
                if (dt.Rows.Count > 0)
                {
                    UpdateRecord = true;
                    GetRecord = true;
                    txtCustomerID.Text = dt.Rows[0]["CustomerID"].ToString();
                    txtReferenceNumber.Text = dt.Rows[0]["ReferenceNo"].ToString();
                    txtCustomerName.Text = dt.Rows[0]["CustomerName"].ToString();
                    txtContactPerson.Text = dt.Rows[0]["ContactPerson"].ToString();
                    txtPhoneNo.Text = dt.Rows[0]["PhoneNo"].ToString();
                    txtEmail.Text = dt.Rows[0]["Email"].ToString();
                    txtAddress.Text = dt.Rows[0]["Address"].ToString();
                    chkIsActive.CheckedValue = bool.Parse(dt.Rows[0]["IsActive"].ToString());
                    btnEdit.Enabled = true;
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

        #endregion

    }
}
