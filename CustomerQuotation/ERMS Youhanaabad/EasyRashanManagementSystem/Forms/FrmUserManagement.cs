using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using DAL;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.InteropServices;
using KansaiProject.MainForm;

namespace EasyRashanManagementSystem.Forms
{
    public partial class FrmUserManagement : Form
    {

        BusinessEntities.BE_Users NewBEObj = new BusinessEntities.BE_Users();
        DAL.DAL_UserManagement NewDALLObj = new DAL.DAL_UserManagement();
        int i = 0;

        public FrmUserManagement()
        {
            InitializeComponent();
            txtpassword.PasswordChar = '*';
            txtretypepass.PasswordChar = '*';
        }

        #region Modifiers
        long UserID = 0;
        bool IsEdit = false;

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

        #region Functions

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private long GetMaxUserId()
        {
            long MaxID;
            try
            {
                MaxID = DataAccess.GetMaxNO("UserID", "Users");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MaxID;
        }

        bool ValidateForm()
        {
            bool IsValid = false;
            try
            {
                if (string.IsNullOrEmpty(txtusername.Text))
                {
                    MessageBox.Show("User Name Empty", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtusername.Focus();
                    IsValid = false;
                }
                else if (string.IsNullOrEmpty(txtpassword.Text))
                {
                    MessageBox.Show("Password Empty", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtpassword.Focus();
                    IsValid = false;
                }
                else if (string.IsNullOrEmpty(txtretypepass.Text))
                {
                    MessageBox.Show("ReType Password Empty", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtretypepass.Focus();
                    IsValid = false;
                }
                else if (txtpassword.Text != txtretypepass.Text)
                {
                    MessageBox.Show("Password Does't Match", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtpassword.Focus();
                    IsValid = false;
                }
                else
                    IsValid = true;
            }
            catch (Exception ex)
            {
                IsValid = false;
                throw ex;
            }
            return IsValid;
        }

        bool ValidateDataGrid()
        {
            bool IsValid = false;

            foreach (DataGridViewRow row in dataGrdUserManagement.Rows)
            {
                if (Convert.ToBoolean(row.Cells["CanView"].Value) == true)
                {
                    IsValid = true;
                }
            }
            return IsValid;
        }

        bool SaveRecord()
        {
            bool IsSave = false;
            try
            {
                NewBEObj.UserID = Convert.ToInt32(lblUserID.Text);
                NewBEObj.UserName = txtusername.Text;
                NewBEObj.Password = txtpassword.Text;
                //NewBEObj.EncryptPassword = Encrypt(txtpassword.Text);
               // NewBEObj.RoleID = Convert.ToInt32(0);
                NewBEObj.Created_At = DateTime.Now.Date;
                NewBEObj.Created_Id = Convert.ToInt32(GlobalVaribles.UserID);
                NewBEObj.Status = true;

                NewBEObj.dtUserRight = new DataTable("dtUser");
                NewBEObj.dtUserRight.Columns.Add("UserID", typeof(int));
                NewBEObj.dtUserRight.Columns.Add("FormID", typeof(int));
                NewBEObj.dtUserRight.Columns.Add("CanView", typeof(int));

                DataRow drRow;
                foreach (DataGridViewRow dr in dataGrdUserManagement.Rows)
                {
                    drRow = NewBEObj.dtUserRight.NewRow();
                    drRow["UserID"] = NewBEObj.UserID;
                    drRow["FormID"] = dr.Cells["FormID"].Value;
                    if (Convert.ToBoolean(dr.Cells["CanView"].Value) == true)
                    {
                        drRow["CanView"] = dr.Cells["CanView"].Value;
                    }
                    else
                    {
                        drRow["CanView"] = 0;
                    }

                    NewBEObj.dtUserRight.Rows.Add(drRow);
                }

                IsSave = NewDALLObj.SaveRecord(NewBEObj);
            }
            catch (Exception ex)
            {
                IsSave = false;
                throw ex;
            }
            return IsSave;
        }

        void ClearForm()
        {
            //dataGrdUserManagement.DataSource = null;
            //dataGrdUserManagement.Rows.Clear();
            //dataGrdUserManagement.Refresh();
            lblUserID.Text = "";
            txtusername.Text = "";
            txtpassword.Text = "";
            txtretypepass.Text = "";
            txtusername.Focus();

            btnDelete.Visible = false;
            btnUpdate.Enabled = false;
            btnSave.Enabled = true;
        }

        void LoadDataGridView()
        {
            DataTable dt = new DataTable();
            DAL.DAL_UserManagement NewDLLObj = new DAL.DAL_UserManagement();
            dt = NewDLLObj.GetFormName();
            if (dt != null && dt.Rows.Count > 0)
            {
                if (i == 0)
                {
                    dataGrdUserManagement.DataSource = dt;

                    DataGridViewCheckBoxColumn CheckBox = new DataGridViewCheckBoxColumn();
                    CheckBox.HeaderText = "Rights";
                    CheckBox.Name = "CanView";
                    CheckBox.Width = 5;
                    CheckBox.CellTemplate.Style.BackColor = Color.WhiteSmoke;
                    CheckBox.DefaultCellStyle.ForeColor = Color.DimGray;
                    dataGrdUserManagement.Columns.Add(CheckBox);

                    dataGrdUserManagement.Columns[0].Visible = false;
                    DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
                    columnHeaderStyle.BackColor = Color.Beige;
                    columnHeaderStyle.Font = new Font("Open Sans", 10, FontStyle.Regular);
                    columnHeaderStyle.ForeColor = Color.Black;
                    dataGrdUserManagement.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
                    //dataGrdUserManagement.AllowUserToAddRows = false;
                    //dataGrdUserManagement.ReadOnly = true;
                    dataGrdUserManagement.Columns[1].ReadOnly = true;

                    dataGrdUserManagement.Columns[1].Width = 165;
                    dataGrdUserManagement.Columns[2].Width = 70;

                    dataGrdUserManagement.RowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
                    dataGrdUserManagement.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                    dataGrdUserManagement.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
                    dataGrdUserManagement.EnableHeadersVisualStyles = false;
                    this.dataGrdUserManagement.GridColor = Color.DarkGray;
                    dataGrdUserManagement.ForeColor = Color.DimGray;
                    dataGrdUserManagement.EnableHeadersVisualStyles = false;
                    dataGrdUserManagement.ColumnHeadersHeight = 30;
                    dataGrdUserManagement.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                    dataGrdUserManagement.ColumnHeadersDefaultCellStyle.Padding = new Padding(0, 0, 0, 0);
                    dataGrdUserManagement.Columns[1].HeaderCell.Style.Padding = new Padding(08, 0, 0, 0);
                    dataGrdUserManagement.Columns[2].HeaderCell.Style.Padding = new Padding(08, 0, 0, 0);
                    //dataGrdUserManagement.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    dataGrdUserManagement.Columns[1].DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
                    dataGrdUserManagement.Columns[2].DefaultCellStyle.Padding = new Padding(0, 0, 0, 0);
                }
                else
                {
                    dataGrdUserManagement.DataSource = dt;
                    dataGrdUserManagement.Columns.Remove("CanView");
                    DataGridViewCheckBoxColumn CheckBx = new DataGridViewCheckBoxColumn();
                    CheckBx.HeaderText = "Rights";
                    CheckBx.Name = "CanView";
                    CheckBx.Width = 5;
                    CheckBx.CellTemplate.Style.BackColor = Color.WhiteSmoke;
                    CheckBx.DefaultCellStyle.ForeColor = Color.DimGray;
                    dataGrdUserManagement.Columns.Add(CheckBx);

                    dataGrdUserManagement.Columns[0].Visible = false;
                    DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
                    columnHeaderStyle.BackColor = Color.Beige;
                    columnHeaderStyle.Font = new Font("Open Sans", 10, FontStyle.Regular);
                    columnHeaderStyle.ForeColor = Color.Black;
                    dataGrdUserManagement.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
                    //dataGrdUserManagement.AllowUserToAddRows = false;
                    //dataGrdUserManagement.ReadOnly = true;
                    dataGrdUserManagement.Columns[1].ReadOnly = true;

                    dataGrdUserManagement.Columns[1].Width = 165;
                    dataGrdUserManagement.Columns[2].Width = 70;

                    dataGrdUserManagement.RowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
                    dataGrdUserManagement.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                    dataGrdUserManagement.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
                    dataGrdUserManagement.EnableHeadersVisualStyles = false;
                    this.dataGrdUserManagement.GridColor = Color.DarkGray;
                    dataGrdUserManagement.ForeColor = Color.DimGray;
                    dataGrdUserManagement.EnableHeadersVisualStyles = false;
                    dataGrdUserManagement.ColumnHeadersHeight = 30;
                    dataGrdUserManagement.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                    dataGrdUserManagement.ColumnHeadersDefaultCellStyle.Padding = new Padding(0, 0, 0, 0);
                    dataGrdUserManagement.Columns[1].HeaderCell.Style.Padding = new Padding(08, 0, 0, 0);
                    dataGrdUserManagement.Columns[2].HeaderCell.Style.Padding = new Padding(08, 0, 0, 0);
                    //dataGrdUserManagement.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;


                    dataGrdUserManagement.Columns[1].DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
                    dataGrdUserManagement.Columns[2].DefaultCellStyle.Padding = new Padding(0, 0, 0, 0);
                }
            }
        }

        bool UpdateRecord()
        {
            bool IsUpdate = false;
            try
            {
                NewBEObj.UserID = Convert.ToInt32(lblUserID.Text);
                NewBEObj.UserName = txtusername.Text;
                NewBEObj.Password = txtpassword.Text;
                NewBEObj.Modify_At = DateTime.Now.Date;
                NewBEObj.Modify_Id = Convert.ToInt32(GlobalVaribles.UserID);
                NewBEObj.Status = true;

                NewBEObj.dtUserRight = new DataTable("dtUser");
                NewBEObj.dtUserRight.Columns.Add("UserID", typeof(int));
                NewBEObj.dtUserRight.Columns.Add("FormID", typeof(int));
                NewBEObj.dtUserRight.Columns.Add("CanView", typeof(int));

                DataRow drRow;
                foreach (DataGridViewRow dr in dataGrdUserManagement.Rows)
                {
                    drRow = NewBEObj.dtUserRight.NewRow();
                    drRow["UserID"] = NewBEObj.UserID;
                    drRow["FormID"] = dr.Cells["FormID"].Value;
                    if (Convert.ToBoolean(dr.Cells["CanView"].Value) == true)
                    {
                        drRow["CanView"] = dr.Cells["CanView"].Value;
                    }
                    else
                    {
                        drRow["CanView"] = 0;
                    }

                    NewBEObj.dtUserRight.Rows.Add(drRow);
                }

                IsUpdate = NewDALLObj.UpdateRecord(NewBEObj);
            }
            catch (Exception ex)
            {
                IsUpdate = false;
                throw ex;
            }
            return IsUpdate;
        }

        #region Password Encrypt & Decrypt

        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        #endregion

        bool ServerAuthentication()
        {
            bool IsAvailable = false;
            IsAvailable = GlobalVaribles.PingHost(GlobalVaribles.IpAddress);
            return IsAvailable;
        }

        public void FormDesign()
        {
            PanelHeader.BackColor = GlobalVaribles.PanelHeader;
            lblheader.BackColor = GlobalVaribles.PanelHeader;
            lblheader.ForeColor = GlobalVaribles.lblheaderforeColor;
            btnClose.BackColor = GlobalVaribles.btnCloseBackColor;
            btnClose.FlatAppearance.BorderSize = GlobalVaribles.btnCloseBorder;
            this.BackColor = GlobalVaribles.FormColor;
            btnEdit.Appearance.BackColor = GlobalVaribles.btnBackColor;
            btnEdit.Appearance.ForeColor = GlobalVaribles.btnForeColor;
            btnEdit.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            btnEdit.Font = GlobalVaribles.btnFontStyle;

            btnSave.Appearance.BackColor = GlobalVaribles.btnBackColor;
            btnSave.Appearance.ForeColor = GlobalVaribles.btnForeColor;
            btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            btnSave.Font = GlobalVaribles.btnFontStyle;

            btnClear.Appearance.BackColor = GlobalVaribles.btnBackColor;
            btnClear.Appearance.ForeColor = GlobalVaribles.btnForeColor;
            btnClear.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            btnClear.Font = GlobalVaribles.btnFontStyle;
            btnEdit.Appearance.BackColor = GlobalVaribles.btnBackColor;
            btnEdit.Appearance.ForeColor = GlobalVaribles.btnForeColor;
            btnEdit.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            btnEdit.Font = GlobalVaribles.btnFontStyle;
            btnUpdate.Appearance.BackColor = GlobalVaribles.btnBackColor;
            btnUpdate.Appearance.ForeColor = GlobalVaribles.btnForeColor;
            btnUpdate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            btnUpdate.Font = GlobalVaribles.btnFontStyle;
            btnDelete.Appearance.BackColor = GlobalVaribles.btnBackColor;
            btnDelete.Appearance.ForeColor = GlobalVaribles.btnForeColor;
            btnDelete.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            btnDelete.Font = GlobalVaribles.btnFontStyle;
            PanelFooter.BackColor = GlobalVaribles.PanelFooter;
        }

        #endregion

        #region Events

        private void FrmUserManagement_Load(object sender, EventArgs e)
        {
            //this.Location = new Point(490, 160);
            FormDesign();
            lblUserID.Text = GetMaxUserId().ToString();
            txtusername.Focus();
            btnDelete.Visible = false;
            LoadDataGridView();
            btnUpdate.Enabled = false;
            i = 0;
            txtusername.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                if (ValidateDataGrid())
                {
                    if (SaveRecord())
                    {
                        MessageBox.Show("Record Inserted Successfully", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearForm();
                        i++;
                        FrmUserManagement_Load(this, new EventArgs());
                    }
                }
                else
                {
                    MessageBox.Show("Define User Rights In Grid ", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

        private void pixbox_MouseDown(object sender, MouseEventArgs e)
        {
            txtpassword.PasswordChar = '\0';
            txtretypepass.PasswordChar = '\0';
        }

        private void pixbox_MouseUp(object sender, MouseEventArgs e)
        {
            txtpassword.PasswordChar = '*';
            txtretypepass.PasswordChar = '*';
        }

        private void txtusername_KeyPress(object sender, KeyPressEventArgs e)
        {
             //only enter string and space and numeric

            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            // // only enter integer
            //     if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            //     {
            //         e.Handled = true;
            //     }
            // only enter string and space 
            //if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            //{
            //    e.Handled = true;
            //}
            // // alpha numeric 
            //     if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar))
            //     {
            //         e.Handled = true;
            //     }
            // // only string and .
            //     if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && (e.KeyChar != '.'))
            //     {
            //         e.Handled = true;
            //     }
            //           // alpha numeric and space 
            //     if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            //     {
            //         e.Handled = true;
            //     }
            // // alpha numeric and space and .
            //     if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && (e.KeyChar != '.'))
            //     {
            //         e.Handled = true;
            //     }

            // // only string 
            //     if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
            //     {
            //         e.Handled = true;
            //     }
            // // alpha numeric and -
            //     if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '-'))
            //     {
            //         e.Handled = true;
            //     }
            //              // only enter string and space and special character 1-0
            //     if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && (e.KeyChar != '!') && (e.KeyChar != '@') && (e.KeyChar != '#') && (e.KeyChar != '$') && (e.KeyChar != '%') && (e.KeyChar != '^') && (e.KeyChar != '&') && (e.KeyChar != '*') && (e.KeyChar != '(') && (e.KeyChar != ')') && (e.KeyChar != '.'))
            //     {
            //         e.Handled = true;
            //     }

            // // only enter string and space and .
            //     if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && (e.KeyChar) != '.')
            //     {
            //         e.Handled = true;
            //     }
            // // only string space and - 
            //     if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && (e.KeyChar) != '-')
            //     {
            //         e.Handled = true;
            //     }

            //// only enter string and space and -
            //     if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && (e.KeyChar) != '-')
            //     {
            //         e.Handled = true;
            //     }

            //  // alpha numeric and space and -
            //     if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && (e.KeyChar != '-'))
            //     {
            //         e.Handled = true;
            //     }

        }

        private void dataGrdUserManagement_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            object rValue;
            try
            {
                rValue = FrmGSearch.Show("Select UserID,UserName from Users where Status=1", false, "All System Users Records");
                
                if (rValue == null)
                    return;
                if (Convert.ToInt64(rValue.ToString()) == 1)
                {
                    MessageBox.Show("Admin Account Can't Be Edit", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                UserID = Convert.ToInt64(rValue.ToString());
                lblUserID.Text = Convert.ToInt64(UserID).ToString();
                DataTable dt = new DataTable();
                DAL_UserManagement NewDLLObj = new DAL_UserManagement();
                dt = NewDLLObj.GetUserDetail(Convert.ToInt32(UserID));
                if (dt != null && dt.Rows.Count > 0)
                {
                    txtusername.Text = dt.Rows[0]["UserName"].ToString();
                    txtpassword.Text = dt.Rows[0]["Password"].ToString();
                    txtretypepass.Text = dt.Rows[0]["Password"].ToString();
                    btnDelete.Visible = true;
                    btnUpdate.Enabled = true;
                    btnSave.Enabled = false;
                    //cboroleid
                }
                DataTable dtUserRights = new DataTable();
                dtUserRights = NewDLLObj.GetUserRightsDetails(Convert.ToInt32(UserID));
                if (dtUserRights != null && dtUserRights.Rows.Count > 0)
                {

                    int DataGridRow = 0;
                    foreach (DataRow dr in dtUserRights.Rows)
                    {
                        dataGrdUserManagement.Rows[DataGridRow].Cells["FormID"].Value = dr["FormID"];
                        dataGrdUserManagement.Rows[DataGridRow].Cells[2].Value = dr["DisplayName"];
                        dataGrdUserManagement.Rows[DataGridRow].Cells["CanView"].Value = dr["CanView"];
                        DataGridRow++;
                    }
                }
                else
                {
                    LoadDataGridView();
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
            i++;
            FrmUserManagement_Load(this, new EventArgs());
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                if (ValidateDataGrid())
                {
                    if (UpdateRecord())
                    {
                        MessageBox.Show("Record Inserted Successfully", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearForm();
                        i++;
                        FrmUserManagement_Load(this, new EventArgs());
                    }
                }
                else
                {
                    MessageBox.Show("Define User Rights In Grid ", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult obj = MessageBox.Show("Are You Want To delete the selected Record?", GlobalVaribles.ProjectName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (obj == System.Windows.Forms.DialogResult.Yes)
            {
                bool IsDelete = false;
                if (lblUserID.Text == "1")
                {
                    MessageBox.Show("Admin Account Can't Delete", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    IsDelete = NewDALLObj.DeleteRecord(Convert.ToInt32(lblUserID.Text));
                    if (IsDelete)
                    {
                        MessageBox.Show("Record Delete Successfully", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        i++;
                        ClearForm();
                        FrmUserManagement_Load(this, new EventArgs());
                        btnDelete.Visible = false;
                    }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    
        private void FrmUserManagement_Paint(object sender, PaintEventArgs e)
        {
            int width = this.Width - 1;
            int height = this.Height - 1;
            Pen greenPen = new Pen(GlobalVaribles.BorderColor);
            e.Graphics.DrawRectangle(greenPen, 0, 0, width, height);
        }

        private void FrmUserManagement_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                FrmConfirmationMessage message = new FrmConfirmationMessage(GlobalVaribles.ConfirmationMsg, 2, this);
                message.ShowDialog();
            }
        }

        #endregion
    }
}
    