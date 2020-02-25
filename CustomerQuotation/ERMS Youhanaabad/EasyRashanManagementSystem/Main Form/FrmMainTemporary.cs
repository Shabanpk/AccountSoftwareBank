using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EasyRashanManagementSystem.Main_Form
{
    public partial class FrmMainTemporary : Form
    {
        int Second = 0;

        public FrmMainTemporary()
        {
            InitializeComponent();
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
                mnuNewSystem.MenuItems.Add(mnuChangePassword);

                MenuItem mnuLogout = new MenuItem("@Logout");
                mnuLogout.Text = "&Logout";
                mnuNewSystem.MenuItems.Add(mnuLogout);

                MenuItem mnuExit = new MenuItem("@Exit");
                mnuExit.Text = "&Exit";
                mnuNewSystem.MenuItems.Add(mnuExit);


                string menuName = "Configuration";
                string menuName1 = "Stock";
                string menupurchase = "Purchase";
                string menuSale = "Sale";
                string menuGift = "Gift";
                string menuProductLoan = "ProductLoan";
                string menuWelfare = "Welfare";
                string menuExpenses = "Expenses";
                string menuLaon = "Loan";
                string MenuName2 = "Reports";


                MenuItem mnuConfiguration = new MenuItem("Configuration");
                mnuConfiguration.Text = "Con&figuration";
                mnuConfiguration.Index = 0;

                MenuItem mnuStock = new MenuItem("Stock");
                mnuStock.Text = "S&tock";
                mnuStock.Index = 1;

                MenuItem mnuPurchase = new MenuItem("Purchase");
                mnuPurchase.Text = "&Purchase";
                mnuPurchase.Index = 2;

                MenuItem mnuSale = new MenuItem("Sal&e");
                mnuSale.Text = "Sal&e";
                mnuSale.Index = 3;

                MenuItem mnuGift = new MenuItem("&Gift");
                mnuGift.Text = "&Gift";
                mnuGift.Index = 4;

                MenuItem mnuProductLoan = new MenuItem("&Product");
                mnuProductLoan.Text = "&Product Loan";
                mnuProductLoan.Index = 5;


                MenuItem mnuLaon = new MenuItem("Loan");
                mnuLaon.Text = "Loa&n";
                mnuLaon.Index = 6;

                MenuItem mnuWelfare = new MenuItem("Wel&fare");
                mnuWelfare.Text = "Wel&fare";
                mnuWelfare.Index = 7;

                MenuItem mnuExpenses = new MenuItem("Ex&penses");
                mnuExpenses.Text = "Ex&penses";
                mnuExpenses.Index = 8;

                MenuItem mnuPrevious = new MenuItem("Ex&penses");
                mnuPrevious.Text = "P&revious Record";
                mnuPrevious.Index = 9;

                //menu.MenuItems.Add(mnuConfiguration);
                // Load Menu Configuration
                foreach (DataRow dr in dt.Rows)
                {
                    string childmenu = "User Management";
                    int i = 0;
                    string DBValue = dr["DisplayName"].ToString();

                    if (DBValue == childmenu)
                    {

                        //ContextMenuStrip  test = new System.Windows.Forms.ContextMenuStrip();
                        //  var exitMenuItem = test.Items.Add("Exit");
                        //  exitMenuItem.Image= Properties.Resources.Easy_Roshan;
                        //  test.ContextMenuStrip = test;

                        //var menus = new ContextMenuStrip();
                        //var item = new ToolStripMenuItem("Configuration");
                        //item.Image = Properties.Resources.Easy_Roshan;
                        ////item.Click += OnClick;
                        //menus.Items.Add(item);
                        //menus.Show(this, this.PointToClient(MousePosition));

                        MenuItem mnuUserManagement = new MenuItem("&UserManagement");
                        mnuUserManagement.Text = "&User Management";
                        mnuUserManagement.Index = 0;
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
                    string childmenuname1 = "Measuring Unit";
                    string childmenuname2 = "Item Group";
                    string childmenuname3 = "Item Defination";
                    string childmenuname4 = "Stock Insertion";
                    int i = 0;
                    string DBValue = drr["DisplayName"].ToString();
                    if (DBValue == childmenuname1)
                    {
                        MenuItem mnuMeasuring = new MenuItem("&MeasuringUnit");
                        mnuMeasuring.Text = "&Measuring Unit";
                        mnuMeasuring.Index = 0;
                        mnuStock.MenuItems.Add(mnuMeasuring);
                        if (i == 0)
                        {
                            menu.MenuItems.Add(mnuStock);
                            i++;
                        }
                    }
                    else if (DBValue == childmenuname2)
                    {
                        MenuItem mnuGroup = new MenuItem("&ItemGroup");
                        mnuGroup.Text = "Item &Group";
                        mnuGroup.Index = 1;
                        mnuStock.MenuItems.Add(mnuGroup);
                        if (i == 0)
                        {
                            menu.MenuItems.Add(mnuStock);
                            i++;
                        }
                    }
                    else if (DBValue == childmenuname3)
                    {
                        MenuItem mnuitem = new MenuItem("&ItemDefination");
                        mnuitem.Text = "Item &Defination";
                        mnuitem.Index = 2;
                        mnuStock.MenuItems.Add(mnuitem);
                        if (i == 0)
                        {
                            menu.MenuItems.Add(mnuStock);
                            i++;
                        }
                    }
                    else if (DBValue == childmenuname4)
                    {
                        MenuItem mnuOpening = new MenuItem("&StockInsertion");
                        mnuOpening.Text = "&Stock Insertion";
                        mnuOpening.Index = 3;
                        mnuStock.MenuItems.Add(mnuOpening);
                        if (i == 0)
                        {
                            menu.MenuItems.Add(mnuStock);
                            i++;
                        }
                    }
                }
                // Load Menu Purchase
                foreach (DataRow dr in dt.Rows)
                {
                    string childmenuPurchase = "Purchase Form";
                    int i = 0;
                    string DBValue = dr["DisplayName"].ToString();

                    if (DBValue == childmenuPurchase)
                    {
                        MenuItem mnuPurchaseForm = new MenuItem("&Purchase");
                        mnuPurchaseForm.Text = "&Purchase Form";
                        mnuPurchaseForm.Index = 0;
                        mnuPurchase.MenuItems.Add(mnuPurchaseForm);
                        if (i == 0)
                        {
                            menu.MenuItems.Add(mnuPurchase);
                            i++;
                        }
                    }
                }
                // Load Menu Sale
                foreach (DataRow dr in dt.Rows)
                {
                    string childmenuSale = "Direct Sale";
                    int i = 0;
                    string DBValue = dr["DisplayName"].ToString();

                    if (DBValue == childmenuSale)
                    {
                        MenuItem mnusale = new MenuItem("&DirectSale");
                        mnusale.Text = "&Sale Form";
                        mnusale.Index = 0;
                        mnuSale.MenuItems.Add(mnusale);
                        if (i == 0)
                        {
                            menu.MenuItems.Add(mnuSale);
                            i++;
                        }
                    }
                }

                // Load Menu Gift
                foreach (DataRow dr in dt.Rows)
                {
                    string childmenuGift = "Gift";
                    int i = 0;
                    string DBValue = dr["DisplayName"].ToString();

                    if (DBValue == childmenuGift)
                    {
                        MenuItem mnuGiftstock = new MenuItem("&Gift");
                        mnuGiftstock.Text = "&Gift From";
                        mnuGiftstock.Index = 0;
                        mnuGift.MenuItems.Add(mnuGiftstock);
                        if (i == 0)
                        {
                            menu.MenuItems.Add(mnuGift);
                            i++;
                        }
                    }
                }

                // Load Menu Product Loan
                foreach (DataRow dr in dt.Rows)
                {
                    string childmenuProduct = "Product Loan";
                    int i = 0;
                    string DBValue = dr["DisplayName"].ToString();

                    if (DBValue == childmenuProduct)
                    {
                        MenuItem mnuProductLoanstock = new MenuItem("&Gift");
                        mnuProductLoanstock.Text = "&Loan Stock";
                        mnuProductLoanstock.Index = 0;
                        mnuProductLoan.MenuItems.Add(mnuProductLoanstock);
                        if (i == 0)
                        {
                            menu.MenuItems.Add(mnuProductLoan);
                            i++;
                        }
                    }
                }

                // Loan Menu Laon
                foreach (DataRow drloan in dt.Rows)
                {
                    string childmenunameLoanAssign = "Loan Assign";
                    string childmenunameLoanPosting = "Loan Posting";
                    string childmenunameLaonReturn = "Loan Return";
                    int i = 0;
                    string DBValue = drloan["DisplayName"].ToString();
                    if (DBValue == childmenunameLoanAssign)
                    {
                        MenuItem mnuLoamAssign = new MenuItem("&LoanAssign");
                        mnuLoamAssign.Text = "Loan &Assign";
                        mnuLoamAssign.Index = 0;
                        mnuLaon.MenuItems.Add(mnuLoamAssign);
                        if (i == 0)
                        {
                            menu.MenuItems.Add(mnuLaon);
                            i++;
                        }
                    }
                    else if (DBValue == childmenunameLoanPosting)
                    {
                        MenuItem mnuLoanPosting = new MenuItem("&LoanPosting");
                        mnuLoanPosting.Text = "Loan &Posting";
                        mnuLoanPosting.Index = 1;
                        mnuLaon.MenuItems.Add(mnuLoanPosting);
                        if (i == 0)
                        {
                            menu.MenuItems.Add(mnuLaon);
                            i++;
                        }
                    }
                    else if (DBValue == childmenunameLaonReturn)
                    {
                        MenuItem mnuLoanReturn = new MenuItem("&LoanReturn");
                        mnuLoanReturn.Text = "Loan &Return";
                        mnuLoanReturn.Index = 2;
                        mnuLaon.MenuItems.Add(mnuLoanReturn);
                        if (i == 0)
                        {
                            menu.MenuItems.Add(mnuLaon);
                            i++;
                        }
                    }
                }

                // Load Menu Welfare
                foreach (DataRow dr in dt.Rows)
                {
                    string childmenuWelfare = "Welfare";
                    int i = 0;
                    string DBValue = dr["DisplayName"].ToString();

                    if (DBValue == childmenuWelfare)
                    {
                        MenuItem mnuWelf = new MenuItem("Wel&fare");
                        mnuWelf.Text = "&Welfare Form";
                        mnuWelf.Index = 0;
                        mnuWelfare.MenuItems.Add(mnuWelf);
                        if (i == 0)
                        {
                            menu.MenuItems.Add(mnuWelfare);
                            i++;
                        }
                    }
                }

                // Load Menu Expenses
                foreach (DataRow dr in dt.Rows)
                {
                    string childmenuExpenses = "Expenses";
                    int i = 0;
                    string DBValue = dr["DisplayName"].ToString();

                    if (DBValue == childmenuExpenses)
                    {
                        MenuItem mnuexpens = new MenuItem("Ex&penses");
                        mnuexpens.Text = "Ex&penses Form";
                        mnuexpens.Index = 0;
                        mnuExpenses.MenuItems.Add(mnuexpens);
                        if (i == 0)
                        {
                            menu.MenuItems.Add(mnuExpenses);
                            i++;
                        }
                    }
                }

                // Load Menu Previous Data
                foreach (DataRow dr in dt.Rows)
                {
                    string childmenuPrevious = "Previous Record";
                    int i = 0;
                    string DBValue = dr["DisplayName"].ToString();

                    if (DBValue == childmenuPrevious)
                    {
                        MenuItem mnupre = new MenuItem("Previous Record");
                        mnupre.Text = "P&revious Record";
                        mnupre.Index = 0;
                        mnuPrevious.MenuItems.Add(mnupre);
                        if (i == 0)
                        {
                            menu.MenuItems.Add(mnuPrevious);
                            i++;
                        }
                    }
                }

                // Load Menu Reports
                MenuItem mnuReports = new MenuItem(@"Reports");
                mnuReports.Text = "&Reports";
                menu.MenuItems.Add(mnuReports);
            }
            this.Menu = menu;
        }

        private void FrmMainTemporary_Load(object sender, EventArgs e)
        {

            timer1.Interval = 1000;
            timer1.Start();
            string UserName = GlobalVaribles.UserName;
            StatusBar.Items["lblStatusBarLogin"].Text = "Welcome " + UserName;
            StatusBar.Items["lblStatusBarReady"].Text = "Ready     " + GlobalVaribles.SoftwareVersion;
            //GlobalVaribles.HideProgressBar();
           // MainSubMenu();

          //  panel1.BackColor = ColorTranslator.FromHtml("#002D40");
          //  pictureBox1.BackColor = ColorTranslator.FromHtml("#ABABAB");
            label1.BackColor = ColorTranslator.FromHtml("#002D40");
            label1.ForeColor = Color.White;
            label2.BackColor = ColorTranslator.FromHtml("#002D40");
            label2.ForeColor = Color.White;
            label3.BackColor = ColorTranslator.FromHtml("#002D40");
            label3.ForeColor = Color.White;
            label4.BackColor = ColorTranslator.FromHtml("#002D40");
            label4.ForeColor = Color.White;
            panel2.BackColor = ColorTranslator.FromHtml("#7E502F");
            label5.ForeColor = Color.White;
            label6.ForeColor = Color.White;
            label7.ForeColor = Color.White;

            panel3.BackColor = ColorTranslator.FromHtml("#002D40");
            panel4.BackColor = ColorTranslator.FromHtml("#002D40");
            label9.ForeColor = Color.White;
            label10.ForeColor = Color.White;
            label11.ForeColor = Color.White;

           // button1.Hide();

            Button btnAdmin = new Button();
            btnAdmin.Height = 47;
            btnAdmin.Width = 78;
            btnAdmin.Location = new Point(150,0);
            btnAdmin.BackColor = Color.Red;
            btnAdmin.Text = "saddddddddddd";
            this.Controls.Add(btnAdmin);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            StatusBar.Items["lblStatusBarDatetime"].Text = DateTime.Now.ToString("dd MMMM yyyy hh:mm:ss tt");
            Second = Second + 1;
            string UserName = GlobalVaribles.UserName;
            StatusBar.Items["lblStatusBarLogin"].Text = "Welcome " + UserName;
        }

    }
}
