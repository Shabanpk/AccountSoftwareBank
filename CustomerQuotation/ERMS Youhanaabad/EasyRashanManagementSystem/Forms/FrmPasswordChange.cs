using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DAL;
using BusinessEntities;
using BusinessProcessObjects;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using KansaiProject.MainForm;

namespace EasyRashanManagementSystem
{
    public partial class FrmPasswordChange : Form
    {
        DAL_PasswordChange NewDALLObj = new DAL_PasswordChange();

        public FrmPasswordChange()
        {
            InitializeComponent();
        }

        #region Modifiers
        long UserID = 0;
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

        bool ValidateForm()
        {
            bool IsValid = false;
            try
            {
                if (string.IsNullOrEmpty(txtOldPassword.Text))
                {
                    MessageBox.Show("Please Enter Old Password", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtOldPassword.Focus();
                    IsValid = false;
                }
                else if (string.IsNullOrEmpty(txtNewPassword.Text))
                {
                    MessageBox.Show("Please Enter New Password", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtNewPassword.Focus();
                    IsValid = false;
                }
                else if (string.IsNullOrEmpty(txtRetypePassword.Text))
                {
                    MessageBox.Show("Please Retype Password", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtRetypePassword.Focus();
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

        bool IsCorrectPassword()
        {
            bool IsValid = false;
            try
            {
                IsValid = NewDALLObj.CheckPassword(GlobalVaribles.UserID, txtOldPassword.Text.Trim());
                if (!IsValid)
                {
                    MessageBox.Show("Correct Old Password", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    IsValid = false;
                    txtOldPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                IsValid = false;
                throw ex;
            }
            return IsValid;
        }

        private bool PassrodMatch()
        {
            bool IsMatch = false;
            try
            {
                if (txtNewPassword.Text == txtRetypePassword.Text)
                {
                    IsMatch = true;
                }
                else
                {
                    IsMatch = false;
                    MessageBox.Show("New Password  and old Password does't match", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch (Exception ex)
            {
                IsMatch = false;
                throw ex;
            }
            return IsMatch;
        }

        bool SaveRecord()
        {
            bool IsSave = false;

            BusinessEntities.BE_PasswordChange NewBEObj = new BusinessEntities.BE_PasswordChange();
            NewBEObj.UserID = GlobalVaribles.UserID;
            NewBEObj.NewPassword = txtNewPassword.Text;
            IsSave = NewDALLObj.SaveRecord(NewBEObj);
            return IsSave;
        }

        void ClearForm()
        {
            txtOldPassword.Text = "";
            txtOldPassword.Focus();
            txtNewPassword.Text = "";
            txtRetypePassword.Text = "";
        }

        public void FormDesign()
        {
            PanelHeader.BackColor = GlobalVaribles.PanelHeader;
            lblheader.BackColor = GlobalVaribles.PanelHeader;
            lblheader.ForeColor = GlobalVaribles.lblheaderforeColor;
            btnClose.BackColor = GlobalVaribles.btnCloseBackColor;
            btnClose.FlatAppearance.BorderSize = GlobalVaribles.btnCloseBorder;
            this.BackColor = GlobalVaribles.FormColor;
            PanelFooter.BackColor = GlobalVaribles.PanelFooter;
        }

        #endregion

        #region Events

        private void FrmPasswordChange_Load(object sender, EventArgs e)
        {
            FormDesign();
            btnUpdate.BackColor = ColorTranslator.FromHtml("#53a2b3");
            System.Drawing.Color col = System.Drawing.ColorTranslator.FromHtml("#75B4C1");
            btnUpdate.FlatAppearance.MouseOverBackColor = col;
            btnUpdate.FlatAppearance.BorderSize = 0;
        }

        private void txtOldPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtNewPassword.Focus();
            }
        }

        private void txtNewPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnUpdate.Focus();
            }
        }

        private void txtRetypePassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnUpdate.Focus();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                if (IsCorrectPassword())
                {
                    if (PassrodMatch())
                    {
                        if (SaveRecord())
                        {
                            MessageBox.Show("Record Saved", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearForm();
                        }
                    }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmPasswordChange_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                FrmConfirmationMessage message = new FrmConfirmationMessage(GlobalVaribles.ConfirmationMsg, 2, this);
                message.ShowDialog();
            }
        }

        private void pixbox_MouseDown(object sender, MouseEventArgs e)
        {
            txtOldPassword.PasswordChar = '\0';
            txtNewPassword.PasswordChar = '\0';
            txtRetypePassword.PasswordChar = '\0';
        }

        private void pixbox_MouseUp(object sender, MouseEventArgs e)
        {
            txtOldPassword.PasswordChar = '*';
            txtNewPassword.PasswordChar = '*';
            txtRetypePassword.PasswordChar = '*';
        }

        private void FrmPasswordChange_Paint(object sender, PaintEventArgs e)
        {
            int width = this.Width - 1;
            int height = this.Height - 1;
            Pen greenPen = new Pen(GlobalVaribles.BorderColor);
            e.Graphics.DrawRectangle(greenPen, 0, 0, width, height);
        }

        #endregion
    }
}

