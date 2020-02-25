using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BusinessEntities;
using BusinessProcessObjects;
using EasyRashanManagementSystem.Main_Form;
using EasyRashanManagementSystem.Forms;
using System.Runtime.InteropServices;
using EasyRashanManagementSystem.Report_Forms;

namespace EasyRashanManagementSystem
{
    public partial class FrmLogin : Form
    {
        Form fc = null;
        DAL.DAL_LoginUser NewDLLObj = new DAL.DAL_LoginUser();
        
        public FrmLogin()
        {
            InitializeComponent();
            txtPassword.PasswordChar = '*';
            
        }

        #region Modifiers

        bool PasswordCheck = false;

        #endregion

        #region Main Panel Variables

        private const int HT_CAPTION = 0x2;
        private const int WM_NCLBUTTONDOWN = 0x00A1;
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern bool ReleaseCapture();
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        #endregion

        #region Functions

        private bool CheckUserPassword()
        {
            BE_LoginUser PasswordCheckObj = new BE_LoginUser();
            BP_LoginUser PasswordObj = new BP_LoginUser();
            PasswordCheckObj.UserName = txtUserName.Text;
            PasswordCheckObj.Password = txtPassword.Text;
            PasswordCheck = PasswordObj.CheckUserPassword(PasswordCheckObj);
            return PasswordCheck;
        }

        private void SetUsers()
        {
            BE_LoginUser NewLoginObj = new BE_LoginUser();
            //GlobalVaribles gblVar = new GlobalVaribles();
            BP_LoginUser GlobalBPObj = new BP_LoginUser();
            NewLoginObj.UserName = txtUserName.Text;
            //gblVar.Style = cboStyles.Text;
            DataTable dt = new DataTable();
            dt=GlobalBPObj.GetUserDetail(NewLoginObj);
            if (dt.Rows.Count > 0)
            {
                GlobalVaribles.UserID = Convert.ToInt32(dt.Rows[0]["UserID"]);
                GlobalVaribles.UserStatus = dt.Rows[0]["UserStatus"].ToString();
                GlobalVaribles.UserName = dt.Rows[0]["UserName"].ToString();
                //txtUserID.Text = GlobalVaribles.UserID.ToString();
                GlobalVaribles.dtSetUser = dt.Copy();
                SetMenu(GlobalVaribles.UserID);
            }
        }

        bool ValidateForm()
        {
            bool IsValid = false;
            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                MessageBox.Show("User Name Empty", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else if (string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Password Empty", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
                IsValid = true;
            return IsValid;
        }

        void SetMenu(int UserID)
        {
            GlobalVaribles.dtMainMenu = new DataTable();
            DataTable dt = new DataTable();
            dt = NewDLLObj.GetUserMenu(GlobalVaribles.UserID);
            GlobalVaribles.dtMainMenu = dt.Copy();
        }

        void MainSubMenu()
        {
            MainMenu menu = new MainMenu();
            DataTable dt = new DataTable();
            dt = GlobalVaribles.dtMainMenu;
            if (dt != null && dt.Rows.Count > 0)
            {
                MenuItem mnuNewSystem = new MenuItem(@"System");

                if (Convert.ToInt32(dt.Rows[0]["UserID"]) == 1)
                {
                    mnuNewSystem.Text = "&Admin";
                    menu.MenuItems.Add(mnuNewSystem);
                }
                else
                {
                    mnuNewSystem.Text = "S&ystem";
                    menu.MenuItems.Add(mnuNewSystem);
                }
                MenuItem mnuChangePassword = new MenuItem("@ChangePassword");
                mnuChangePassword.Text = "C&hange Password";
                mnuChangePassword.Click += new EventHandler(mnuChangePassword_Click);
                mnuNewSystem.MenuItems.Add(mnuChangePassword);

                MenuItem mnuLogout = new MenuItem("@Logout");
                mnuLogout.Text = "&Logout";
                mnuLogout.Click += new EventHandler(mnuLogout_Click);
                mnuNewSystem.MenuItems.Add(mnuLogout);

                MenuItem mnuExit = new MenuItem("@Exit");
                mnuExit.Text = "&Exit";
                mnuExit.Click += new EventHandler(mnuExit_Click);
                mnuNewSystem.MenuItems.Add(mnuExit);


                string menuName = "Configuration";
                //string menuName1 = "Defination";
                string menuName1 = "Accounts";
                string MenuName2 = "Reports";


                MenuItem mnuConfiguration = new MenuItem("Configuration");
                mnuConfiguration.Text = "Con&figuration";
                mnuConfiguration.Index = 0;

                MenuItem mnuStock = new MenuItem("Accounts");
                mnuStock.Text = "&Accounts";
                mnuStock.Index = 1;


                //menu.MenuItems.Add(mnuConfiguration);
                // Load Menu Configuration
                foreach (DataRow dr in dt.Rows)
                {
                    string childmenu = "User Management";
                    int i = 0;
                    string DBValue = dr["DisplayName"].ToString();

                    if (DBValue == childmenu)
                    {

                        MenuItem mnuUserManagement = new MenuItem("&UserManagement");
                        mnuUserManagement.Text = "&User Management";
                        mnuUserManagement.Index = 0;
                        mnuUserManagement.Click += new EventHandler(mnuUserManagement_Click);
                        mnuConfiguration.MenuItems.Add(mnuUserManagement);
                        if (i == 0)
                        {
                            menu.MenuItems.Add(mnuConfiguration);
                            i++;
                        }
                    }
                }

                // Load Menu Stock
                foreach (DataRow drr in dt.Rows)
                {
                    string childmenuname1 = "Version";
                    string childmenuname2 = "Customer Information";
                    string childmenuname3 = "Building Parameter";
                    string childmenuname4 = "Customer Status";
                    string childmenuname5 = "Customer Defination";
                    string childmenuname6 = "Account Transaction";
                    int i = 0;
                    string DBValue = drr["DisplayName"].ToString();
                    if (DBValue == childmenuname1)
                    {
                        MenuItem mnuVersion = new MenuItem("&Version");
                        mnuVersion.Text = "&Version";
                        mnuVersion.Index = 0;
                        mnuVersion.Click += new EventHandler(mnuVersion_Click); mnuStock.MenuItems.Add(mnuVersion);
                        if (i == 0)
                        {
                            menu.MenuItems.Add(mnuStock);
                            i++;
                        }
                    }
                    else if (DBValue == childmenuname2)
                    {
                        MenuItem mnuCustomerInfo = new MenuItem("&CustomerInformation");
                        mnuCustomerInfo.Text = "&Customer Information";
                        mnuCustomerInfo.Index = 1;
                        mnuCustomerInfo.Click += new EventHandler(mnuCustomerInfo_Click);
                        mnuStock.MenuItems.Add(mnuCustomerInfo);
                        if (i == 0)
                        {
                            menu.MenuItems.Add(mnuStock);
                            i++;
                        }
                    }
                    else if (DBValue == childmenuname3)
                    {
                        MenuItem mnuBuildingPar = new MenuItem("&BuildingParameter");
                        mnuBuildingPar.Text = "&Building Parameter";
                        mnuBuildingPar.Index = 2;
                        mnuBuildingPar.Click += new EventHandler(mnuBuildingPar_Click);
                        mnuStock.MenuItems.Add(mnuBuildingPar);
                        if (i == 0)
                        {
                            menu.MenuItems.Add(mnuStock);
                            i++;
                        }
                    }
                    else if (DBValue == childmenuname4)
                    {
                        MenuItem mnuCustomerStatus = new MenuItem("&CustomerStatus");
                        mnuCustomerStatus.Text = "Customer &Status";
                        mnuCustomerStatus.Index = 3;
                        mnuCustomerStatus.Click += new EventHandler(mnuCustomerStatus_Click);
                        mnuStock.MenuItems.Add(mnuCustomerStatus);
                        if (i == 0)
                        {
                            menu.MenuItems.Add(mnuStock);
                            i++;
                        }
                    }

                    else if (DBValue == childmenuname5)
                    {
                        MenuItem mnuCustomerDefination = new MenuItem("&Customer Defination");
                        mnuCustomerDefination.Text = "Customer &Defination";
                        mnuCustomerDefination.Index = 4;
                        mnuCustomerDefination.Click += new EventHandler(mnuCustomerDefination_Click);
                        mnuStock.MenuItems.Add(mnuCustomerDefination);
                        if (i == 0)
                        {
                            menu.MenuItems.Add(mnuStock);
                            i++;
                        }
                    }

                    else if (DBValue == childmenuname6)
                    {
                        MenuItem mnuAccountsTrans = new MenuItem("Account &Transaction");
                        mnuAccountsTrans.Text = "Account &Transaction";
                        mnuAccountsTrans.Index = 5;
                        mnuAccountsTrans.Click += new EventHandler(mnuAccountsTrans_Click);
                        mnuStock.MenuItems.Add(mnuAccountsTrans);
                        if (i == 0)
                        {
                            menu.MenuItems.Add(mnuStock);
                            i++;
                        }
                    }

                }

                // Load Menu Reports
                MenuItem mnuReports = new MenuItem(@"Reports");
                mnuReports.Text = "&Reports";
                mnuReports.Click += new EventHandler(mnuReports_Click);
                menu.MenuItems.Add(mnuReports);
            }
            this.Menu = menu;
        }

       
        #endregion

        #region Events

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            //PanelHeader.BackColor = ColorTranslator.FromHtml("#002D40");
           // label1.BackColor = ColorTranslator.FromHtml("#002D40");
            lblLogin.BackColor = ColorTranslator.FromHtml("#002D40");
            lblLogin.BackColor = ColorTranslator.FromHtml("#ECEEF7");
            //label1.BackColor = ColorTranslator.FromHtml("#ECEEF7");
            this.BackColor = ColorTranslator.FromHtml("#ECEEF7");
            label2.BackColor = ColorTranslator.FromHtml("#002D40");
            label5.BackColor = ColorTranslator.FromHtml("#002D40");
            //PanelFooter.BackColor = ColorTranslator.FromHtml("#DEE1EE");
            panel1.BackColor = ColorTranslator.FromHtml("#DEE1EE");
            panel2.BackColor = ColorTranslator.FromHtml("#DEE1EE");
            lblPass.BackColor = ColorTranslator.FromHtml("#DEE1EE");
            lblUserName.BackColor = ColorTranslator.FromHtml("#DEE1EE");
               
            btnClose.BackColor = ColorTranslator.FromHtml("#002D40");
            btnClose.ForeColor = ColorTranslator.FromHtml("#002D40");

            btnLogin.Appearance.BackColor = ColorTranslator.FromHtml("#002D40");
            btnLogin.Appearance.ForeColor = Color.White;
            btnLogin.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            btnLogin.Font = new System.Drawing.Font("Open Sans", 9, FontStyle.Bold);

            btnClose.Appearance.BackColor = ColorTranslator.FromHtml("#002D40");
            btnClose.Appearance.ForeColor = Color.White;
            btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            btnClose.Font = new System.Drawing.Font("Open Sans", 9, FontStyle.Bold);

            dtpDate.Value = DateTime.Now;
            dtpTime.Value = DateTime.Now;
            dtpDate.Format = DateTimePickerFormat.Custom;
            dtpDate.CustomFormat = "MMMM dd, yyyy";

            dtpTime.Format = DateTimePickerFormat.Time;
            dtpTime.ShowUpDown = true;
            

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                if (CheckUserPassword())
                {
                    SetUsers();
                    fc = Application.OpenForms["FrmMainScreen"];
                    if (fc != null)
                    {
                        fc.Close();
                        fc.Dispose();
                        //this.Close();
                        MainSubMenu();
                        FrmMainScreen frmMain = new FrmMainScreen();
                        frmMain.IsMdiContainer = true;
                        frmMain.ShowDialog();
                        //fc.IsMdiContainer = true;
                        //fc.Menu = menu;
                        //fc.WindowState = FormWindowState.Maximized;
                        //fc.Focus();
                    }
                    else
                    {
                        this.Close();
                    }   
                }
                else
                {
                    MessageBox.Show("Invalid UserName/Password", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void PanelHeader_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        private void btnCross_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FrmLogin_Paint(object sender, PaintEventArgs e)
        {
            int width = this.Width - 1;
            int height = this.Height - 1;
            Pen greenPen = new Pen(ColorTranslator.FromHtml("#002D40"));
            e.Graphics.DrawRectangle(greenPen, 0, 0, width, height);
        }

        #region Events Form Click

        void mnuUserManagement_Click(object sender, EventArgs e)
        {
            Form fc = Application.OpenForms["FrmUserManagement"];
            Form fcParentMenu = Application.OpenForms["FrmMainScreen"];
            if (fc != null)
            {
                fc.Focus();
            }
            else
            {
                FrmUserManagement f1 = new FrmUserManagement();
                f1.MdiParent = fcParentMenu;
                f1.Show();
            }

        }

        void mnuChangePassword_Click(object sender, EventArgs e)
        {

            Form fc = Application.OpenForms["FrmPasswordChange"];
            Form fcParentMenu = Application.OpenForms["FrmMainScreen"];
            if (fc != null)
            {
                fc.Focus();
            }
            else
            {
                FrmPasswordChange f1 = new FrmPasswordChange();
                f1.MdiParent = fcParentMenu;
                f1.Show();
            }
        }

        void mnuLogout_Click(object sender, EventArgs e)
        {
            GlobalVaribles.UserID = 0;
            FrmLogin frmLogin = new FrmLogin();
            //this.WindowState = FormWindowState.Minimized;
            frmLogin.ShowDialog();
        }

        void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        void mnuVersion_Click(object sender, EventArgs e)
        {
            Form fc = Application.OpenForms["FrmVersion"];
            Form fcParentMenu = Application.OpenForms["FrmMainScreen"];
            if (fc != null)
            {
                fc.Focus();
            }
            else
            {
                FrmVersion f1 = new FrmVersion();
                f1.MdiParent = fcParentMenu;
                f1.Show();
            }
        }

        void mnuBuildingPar_Click(object sender, EventArgs e)
        {
            Form fc = Application.OpenForms["FrmBuildingParameter"];
            Form fcParentMenu = Application.OpenForms["FrmMainScreen"];
            if (fc != null)
            {
                fc.Focus();
            }
            else
            {
                FrmBuildingParameter f1 = new FrmBuildingParameter();
                f1.MdiParent = fcParentMenu;
                f1.Show();
            }
        }

        void mnuCustomerInfo_Click(object sender, EventArgs e)
        {
            Form fc = Application.OpenForms["FrmCustomerInfo"];
            Form fcParentMenu = Application.OpenForms["FrmMainScreen"];
            if (fc != null)
            {
                fc.Focus();
            }
            else
            {
                FrmCustomerInfo f1 = new FrmCustomerInfo();
                f1.MdiParent = fcParentMenu;
                f1.Show();
            }
        }

        void mnuCustomerStatus_Click(object sender, EventArgs e)
        {
            Form fc = Application.OpenForms["FrmCustomerStatus"];
            Form fcParentMenu = Application.OpenForms["FrmMainScreen"];
            if (fc != null)
            {
                fc.Focus();
            }
            else
            {
                FrmCustomerStatus f1 = new FrmCustomerStatus();
                f1.MdiParent = fcParentMenu;
                f1.Show();
            }
        }

        void mnuMeasuring_Click(object sender, EventArgs e)
        {
            Form fc = Application.OpenForms["FrmMeasuringUnit"];
            Form fcParentMenu = Application.OpenForms["FrmMainScreen"];
            if (fc != null)
            {
                fc.Focus();
            }
            else
            {
                FrmMeasuringUnit f1 = new FrmMeasuringUnit();
                f1.MdiParent = fcParentMenu;
                f1.Show();
            }
        }

        void mnuReports_Click(object sender, EventArgs e)
        {
            Form fc = Application.OpenForms["FrmReportForm"];
            Form fcParentMenu = Application.OpenForms["FrmMainScreen"];
            if (fc != null)
            {
                fc.Focus();
            }
            else
            {
                FrmReportForm f1 = new FrmReportForm();
                f1.MdiParent = fcParentMenu;
                f1.Show();
            }
        }

        void mnuAccountsTrans_Click(object sender, EventArgs e)
        {
            Form fc = Application.OpenForms["FrmAccounts"];
            Form fcParentMenu = Application.OpenForms["FrmMainScreen"];
            if (fc != null)
            {
                fc.Focus();
            }
            else
            {
                FrmAccounts f1 = new FrmAccounts();
                f1.MdiParent = fcParentMenu;
                f1.Show();
            }
        }

        void mnuCustomerDefination_Click(object sender, EventArgs e)
        {
            Form fc = Application.OpenForms["FrmCustomerDefination"];
            Form fcParentMenu = Application.OpenForms["FrmMainScreen"];
            if (fc != null)
            {
                fc.Focus();
            }
            else
            {
                FrmCustomerDefination f1 = new FrmCustomerDefination();
                f1.MdiParent = fcParentMenu;
                f1.Show();
            }
        }


        #endregion

        #endregion
    }
}
