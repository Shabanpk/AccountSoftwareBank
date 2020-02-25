using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KansaiProject.MainForm;
using System.Runtime.InteropServices;
using DAL;
using CrystalDecisions.CrystalReports.Engine;
using BusinessProcessObjects;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinExplorerBar;
using System.Diagnostics;

namespace EasyRashanManagementSystem.Report_Forms
{
    public partial class FrmReportForm : Form
    {
        DAL_LoanPosting DALObj = new DAL_LoanPosting();
        DataTable dt = new DataTable();
        DAL.DAL_RptRecovery NewDALObj = new DAL_RptRecovery();
        int PersonID = 0;
        string SaleType = "";
        int ItemID = 0;
        string ItemName = "";

        public FrmReportForm()
        {
            InitializeComponent();
        }

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

        private bool ValidateReportForm()
        {
            try
            {
                if (UEBReport.ActiveItem.Text == "")
                {
                    MessageBox.Show("Please Select a Report to Continue...", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                
                //if (lstvReports.SelectedItems[0].Key.Equals(""))
                //{
                //    MessageBox.Show("Please Select a Report to Continue...", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //    lstvReports.Focus();
                //    return false;
                //}
                //if (lstvReports.SelectedItems[0].Key == null)
                //{
                //    MessageBox.Show("Please Select a Report to Continue...", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //    lstvReports.Focus();
                //    return false;
                //}

                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                MessageBox.Show("Please Select Report List Item", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
        }

        void LoadEntryType()
        {
            
            dt.Columns.Add("Type");
            DataRow dr = dt.NewRow();
            dr["Type"] = "Customer";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["Type"] = "Gift";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["Type"] = "Loan";
            dt.Rows.Add(dr);
            CboType.DataSource = dt;
            CboType.ValueMember = "Type";
            CboType.DisplayMember = "Type";
            CboType.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
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

        private void FrmReportForm_Load(object sender, EventArgs e)
        {
            FormDesign();
            btnPreview.BackColor = ColorTranslator.FromHtml("#53a2b3");
            System.Drawing.Color col = System.Drawing.ColorTranslator.FromHtml("#75B4C1");
            btnPreview.FlatAppearance.MouseOverBackColor = col;
            btnPreview.FlatAppearance.BorderSize = 0;
            DALObj.LoadAssignPersonName(cboPersonName);
            NewDALObj.LoadItemName(cboItemName);
            cboPersonName.Enabled = false;
            cboMobileNo.Enabled = false;
            LoadEntryType();
            CboType.Enabled = false;
            cboItemName.Enabled = false;
            UEBReport.Appearance.BackColor = ColorTranslator.FromHtml("#556080");
            UEBReport.Appearance.ForeColor = Color.White;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmReportForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                FrmConfirmationMessage message = new FrmConfirmationMessage(GlobalVaribles.ConfirmationMsg, 2, this);
                message.ShowDialog();
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            BP_RptRecovery NewBP = new BP_RptRecovery();
            try
            {
                if (ValidateReportForm())
                {
                    // Stock Purchase Report
                    if (UEBReport.ActiveItem.Text == "Stock Purchase Report")
                    {
                        System.Data.DataTable dtpr = new System.Data.DataTable();
                        BP_RptRecovery NewBPPr = new BP_RptRecovery();
                        string FromDay = dtpFrom.DateTime.Day.ToString();
                        string FromMonth = Convert.ToString(dtpFrom.DateTime.Month);
                        string FromMonthName = dtpFrom.DateTime.ToString("MMMM");
                        string FromYear = Convert.ToString(dtpFrom.DateTime.Year);
                        string FromDate = "";
                        if (Convert.ToInt32(FromDay) <= 9)
                        {
                            FromDay = "0" + FromDay;
                        }
                        if (Convert.ToInt32(FromMonth) <= 9)
                        {
                            FromMonth = "0" + FromMonth;
                        }

                        FromDate = FromYear + "-" + FromMonth + "-" + FromDay.ToString();

                        string Today = dtpTo.DateTime.Day.ToString();
                        string ToMonth = Convert.ToString(dtpTo.DateTime.Month);
                        string ToYear = Convert.ToString(dtpTo.DateTime.Year);
                        string ToMonthName = dtpTo.DateTime.ToString("MMMM");
                        string ToDate = "";
                        if (Convert.ToInt32(Today) <= 9)
                        {
                            Today = "0" + Today;
                        }
                        if (Convert.ToInt32(ToMonth) <= 9)
                        {
                            ToMonth = "0" + ToMonth;
                        }

                        ToDate = ToYear + "-" + ToMonth + "-" + Today;
                        dtpr = NewBPPr.ShowPurchaseItems(FromDate, ToDate);
                        if (dtpr.Rows.Count == 0)
                        {
                            MessageBox.Show("No Record Found", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                        else
                        {
                            Reports.Rpt_Purchase rpt = new EasyRashanManagementSystem.Reports.Rpt_Purchase();
                            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["txtFromDate"];
                            txtFrom.Text = FromDate;
                            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["txtToDate"];
                            txtTo.Text = ToDate;
                            rpt.SetDataSource(dtpr);
                            CommonDAL.ShowReport(rpt, "All PurchaseItems");
                            dtpr.Dispose();
                        }
                    }

                    // Stock Purchase Report By Item
                    if (UEBReport.ActiveItem.Text == "Purchase Per Item Report")
                    {
                        System.Data.DataTable dtpr = new System.Data.DataTable();
                        BP_RptRecovery NewBPPr = new BP_RptRecovery();
                        string FromDay = dtpFrom.DateTime.Day.ToString();
                        string FromMonth = Convert.ToString(dtpFrom.DateTime.Month);
                        string FromMonthName = dtpFrom.DateTime.ToString("MMMM");
                        string FromYear = Convert.ToString(dtpFrom.DateTime.Year);
                        string FromDate = "";
                        if (Convert.ToInt32(FromDay) <= 9)
                        {
                            FromDay = "0" + FromDay;
                        }
                        if (Convert.ToInt32(FromMonth) <= 9)
                        {
                            FromMonth = "0" + FromMonth;
                        }

                        FromDate = FromYear + "-" + FromMonth + "-" + FromDay.ToString();

                        string Today = dtpTo.DateTime.Day.ToString();
                        string ToMonth = Convert.ToString(dtpTo.DateTime.Month);
                        string ToYear = Convert.ToString(dtpTo.DateTime.Year);
                        string ToMonthName = dtpTo.DateTime.ToString("MMMM");
                        string ToDate = "";
                        if (Convert.ToInt32(Today) <= 9)
                        {
                            Today = "0" + Today;
                        }
                        if (Convert.ToInt32(ToMonth) <= 9)
                        {
                            ToMonth = "0" + ToMonth;
                        }

                        ToDate = ToYear + "-" + ToMonth + "-" + Today;
                        if (!ItemName.Trim().Equals(""))
                        {
                            dtpr = NewDALObj.ShowPurchasePerItems(FromDate, ToDate, ItemID);
                            if (dtpr.Rows.Count == 0)
                            {
                                MessageBox.Show("No Record Found", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            }
                            else
                            {
                                Reports.Rpt_PurchaseItem rpt = new EasyRashanManagementSystem.Reports.Rpt_PurchaseItem();
                                TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["txtFromDate"];
                                txtFrom.Text = FromDate;
                                TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["txtToDate"];
                                txtTo.Text = ToDate;
                                rpt.SetDataSource(dtpr);
                                CommonDAL.ShowReport(rpt, "All PurchaseItems");
                                dtpr.Dispose();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please Select ItemName", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            CboType.Focus();
                        }
                    }

                    // Stock Balance Report
                    if (UEBReport.ActiveItem.Text == "Stock Balance Report")
                    {
                        System.Data.DataTable dtStock = new System.Data.DataTable();
                        DAL_RptRecovery NewBPObj = new DAL_RptRecovery();
                        dtStock = NewBPObj.GetStockBalance();
                        if (dtStock.Rows.Count == 0)
                        {
                            MessageBox.Show("No Record Found", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                        else
                        {
                            Reports.RptStockBalance rpt = new EasyRashanManagementSystem.Reports.RptStockBalance();
                            rpt.SetDataSource(dtStock);
                            TextObject txtDate = (TextObject)rpt.ReportDefinition.ReportObjects["txtDate"];
                            txtDate.Text = DateTime.Now.ToString();
                            CommonDAL.ShowReport(rpt, "Stock Balance Report");
                            dtStock.Dispose();
                        }
                    }

                    // Purchase and Sale By month  by specific type like loan customer and gift
                    //if (lstvReports.SelectedItems[0].Key.Equals("Rpt_DirectSaleByType"))
                    if (UEBReport.ActiveItem.Text == "General Sale Report"|| UEBReport.ActiveItem.Text == "General Gift Report")
                    {
                        Reports.Rpt_StockOutByType NewDirectSalesBalance = new Reports.Rpt_StockOutByType();
                        dt = new System.Data.DataTable();
                        string Fromday = dtpFrom.DateTime.Day.ToString();
                        string FromMonth = dtpFrom.DateTime.Month.ToString();
                        string Year = dtpFrom.DateTime.Year.ToString();
                        if (int.Parse(Fromday.ToString()) <= 9)
                        {
                            Fromday = "0" + Fromday;
                        }
                        if (int.Parse(FromMonth.ToString()) <= 9)
                        {
                            FromMonth = "0" + FromMonth;
                        }
                        string FromDate = Year + "-" + FromMonth + "-" + Fromday;

                        string ToDate = dtpTo.DateTime.Day.ToString();
                        string ToMonth = dtpTo.DateTime.Month.ToString();
                        string ToYear = dtpTo.DateTime.Year.ToString();

                        if (int.Parse(ToDate.ToString()) <= 9)
                        {
                            ToDate = "0" + ToDate;
                        }
                        if (int.Parse(ToMonth.ToString()) <= 9)
                        {
                            ToMonth = "0" + ToMonth;
                        }
                        string ToDateQry = ToYear + "-" + ToMonth + "-" + ToDate;
                        DAL_RptRecovery NewDALObj = new DAL_RptRecovery();
                        if (!SaleType.Trim().Equals(""))
                        {
                            dt = NewDALObj.Rpt_DirectSalesByType(FromDate, ToDateQry, SaleType);
                            if (dt.Rows.Count == 0)
                            {
                                MessageBox.Show("No Record Found", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                dt.Dispose();
                                FromDate = "";
                                ToDateQry = "";
                            }
                            else
                            {
                                NewDirectSalesBalance.SetDataSource(dt);
                                TextObject txtFromDate = (TextObject)NewDirectSalesBalance.ReportDefinition.ReportObjects["txtFromDate"];
                                txtFromDate.Text = FromDate;
                                TextObject txtToDate = (TextObject)NewDirectSalesBalance.ReportDefinition.ReportObjects["txtToDate"];
                                txtToDate.Text = ToDateQry;
                                TextObject txtTitle = (TextObject)NewDirectSalesBalance.ReportDefinition.ReportObjects["txtTitle"];
                                TextObject txtUrduGenral = (TextObject)NewDirectSalesBalance.ReportDefinition.ReportObjects["txtUrduGenral"];
                                if (SaleType == "Customer")
                                {
                                    txtTitle.Text = "General Customer Report";
                                    txtUrduGenral.Text = "جنرل کسٹمر رپورٹ";
                                }
                                else if (SaleType == "Loan")
                                {
                                    txtTitle.Text = "General Product Loan Report";
                                    txtUrduGenral.Text = "جنرل قرض کی رپورٹ";
                                }
                                else if (SaleType == "Gift")
                                {
                                    txtTitle.Text = "General Gift Report";
                                    txtUrduGenral.Text = "جنرل تحفہ کی رپورٹ";
                                }

                                CommonDAL.ShowReport(NewDirectSalesBalance, "Sales");
                                dt.Dispose();
                                FromDate = "";
                                ToDateQry = "";
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please Select Type", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            CboType.Focus();
                        }
                    }
                    // genral stock loan report 
                    if (UEBReport.ActiveItem.Text == "General Stock Laon Report")
                    {
                        Reports.Rpt_StockLoan NewDirectSalesBalance = new Reports.Rpt_StockLoan();
                        dt = new System.Data.DataTable();
                        string Fromday = dtpFrom.DateTime.Day.ToString();
                        string FromMonth = dtpFrom.DateTime.Month.ToString();
                        string Year = dtpFrom.DateTime.Year.ToString();
                        if (int.Parse(Fromday.ToString()) <= 9)
                        {
                            Fromday = "0" + Fromday;
                        }
                        if (int.Parse(FromMonth.ToString()) <= 9)
                        {
                            FromMonth = "0" + FromMonth;
                        }
                        string FromDate = Year + "-" + FromMonth + "-" + Fromday;

                        string ToDate = dtpTo.DateTime.Day.ToString();
                        string ToMonth = dtpTo.DateTime.Month.ToString();
                        string ToYear = dtpTo.DateTime.Year.ToString();

                        if (int.Parse(ToDate.ToString()) <= 9)
                        {
                            ToDate = "0" + ToDate;
                        }
                        if (int.Parse(ToMonth.ToString()) <= 9)
                        {
                            ToMonth = "0" + ToMonth;
                        }
                        string ToDateQry = ToYear + "-" + ToMonth + "-" + ToDate;
                        DAL_RptRecovery NewDALObj = new DAL_RptRecovery();
                        if (!SaleType.Trim().Equals(""))
                        {
                            dt = NewDALObj.Rpt_StockLoan(FromDate, ToDateQry, SaleType);
                            if (dt.Rows.Count == 0)
                            {
                                MessageBox.Show("No Record Found", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                dt.Dispose();
                                FromDate = "";
                                ToDateQry = "";
                            }
                            else
                            {
                                NewDirectSalesBalance.SetDataSource(dt);
                                TextObject txtFromDate = (TextObject)NewDirectSalesBalance.ReportDefinition.ReportObjects["txtFromDate"];
                                txtFromDate.Text = FromDate;
                                TextObject txtToDate = (TextObject)NewDirectSalesBalance.ReportDefinition.ReportObjects["txtToDate"];
                                txtToDate.Text = ToDateQry;
                                TextObject txtTitle = (TextObject)NewDirectSalesBalance.ReportDefinition.ReportObjects["txtTitle"];
                                if (SaleType == "Loan")
                                {
                                    txtTitle.Text = "General Product Loan Report";
                                }
                                CommonDAL.ShowReport(NewDirectSalesBalance, "Sales");
                                dt.Dispose();
                                FromDate = "";
                                ToDateQry = "";
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please Select Type", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            CboType.Focus();
                        }
                    }

                    // Detail Sale By Specific Type
                    //if (lstvReports.SelectedItems[0].Key.Equals("Rpt_DirectSaleBySpecificType"))
                    if (UEBReport.ActiveItem.Text == "Detail Sale Report" || UEBReport.ActiveItem.Text == "Detail Stock Laon Report" || UEBReport.ActiveItem.Text == "Detail Gift Report")
                    {
                        Reports.Rpt_DirectSalesAllCustomersByType NewDirectSalesAll = new EasyRashanManagementSystem.Reports.Rpt_DirectSalesAllCustomersByType();
                        dt = new System.Data.DataTable();
                        string Fromday = dtpFrom.DateTime.Day.ToString();
                        string FromMonth = dtpFrom.DateTime.Month.ToString();
                        string Year = dtpFrom.DateTime.Year.ToString();
                        if (int.Parse(Fromday.ToString()) <= 9)
                        {
                            Fromday = "0" + Fromday;
                        }
                        if (int.Parse(FromMonth.ToString()) <= 9)
                        {
                            FromMonth = "0" + FromMonth;
                        }
                        string FromDate = Year + "-" + FromMonth + "-" + Fromday;

                        string ToDate = dtpTo.DateTime.Day.ToString();
                        string ToMonth = dtpTo.DateTime.Month.ToString();
                        string ToYear = dtpTo.DateTime.Year.ToString();

                        if (int.Parse(ToDate.ToString()) <= 9)
                        {
                            ToDate = "0" + ToDate;
                        }
                        if (int.Parse(ToMonth.ToString()) <= 9)
                        {
                            ToMonth = "0" + ToMonth;
                        }
                        string ToDateQry = ToYear + "-" + ToMonth + "-" + ToDate;
                        if (!SaleType.Trim().Equals(""))
                        {

                            dt = NewDALObj.RptDirectSalesBySpecificType(FromDate, ToDateQry, SaleType);
                            if (dt.Rows.Count == 0)
                            {
                                MessageBox.Show("No Record Found", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                dt.Dispose();
                                FromDate = "";
                                ToDateQry = "";
                            }
                            else
                            {
                                NewDirectSalesAll.SetDataSource(dt);
                                TextObject txtFromDate = (TextObject)NewDirectSalesAll.ReportDefinition.ReportObjects["txtFromDate"];
                                txtFromDate.Text = FromDate;
                                TextObject txtToDate = (TextObject)NewDirectSalesAll.ReportDefinition.ReportObjects["txtToDate"];
                                txtToDate.Text = ToDateQry;
                                TextObject txtTitle = (TextObject)NewDirectSalesAll.ReportDefinition.ReportObjects["txtTitle"];
                                TextObject txtUrdudetial = (TextObject)NewDirectSalesAll.ReportDefinition.ReportObjects["txtUrdudetial"];
                                if (SaleType == "Customer")
                                {
                                    txtTitle.Text = "Detail Customer Report";
                                    txtUrdudetial.Text = "تفصیل کسٹمر رپورٹ ";
                                }
                                else if (SaleType == "Loan")
                                {
                                    txtTitle.Text = "Detail Product Loan Report";
                                    txtUrdudetial.Text = "مصنوعات کی قرض کی رپورٹ";
                                }
                                else if (SaleType == "Gift")
                                {
                                    txtTitle.Text = "Detail Gift Report";
                                    txtUrdudetial.Text = "تفصیل گفٹ رپورٹ";
                                }

                                CommonDAL.ShowReport(NewDirectSalesAll, "Detail Sales");
                                dt.Dispose();
                                FromDate = "";
                                ToDateQry = "";
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please Select Type", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            CboType.Focus();
                        }
                    }


                    // Purchase and Sale By month 
                    //if (lstvReports.SelectedItems[0].Key.Equals("Rpt_SaleBalance"))
                    if (UEBReport.ActiveItem.Text == "Genral Report")
                    {
                        Reports.Rpt_StockOutBalance NewDirectSalesBalance = new Reports.Rpt_StockOutBalance();

                        dt = new System.Data.DataTable();
                        string Fromday = dtpFrom.DateTime.Day.ToString();
                        string FromMonth = dtpFrom.DateTime.Month.ToString();
                        string Year = dtpFrom.DateTime.Year.ToString();
                        if (int.Parse(Fromday.ToString()) <= 9)
                        {
                            Fromday = "0" + Fromday;
                        }
                        if (int.Parse(FromMonth.ToString()) <= 9)
                        {
                            FromMonth = "0" + FromMonth;
                        }
                        string FromDate = Year + "-" + FromMonth + "-" + Fromday;

                        string ToDate = dtpTo.DateTime.Day.ToString();
                        string ToMonth = dtpTo.DateTime.Month.ToString();
                        string ToYear = dtpTo.DateTime.Year.ToString();

                        if (int.Parse(ToDate.ToString()) <= 9)
                        {
                            ToDate = "0" + ToDate;
                        }
                        if (int.Parse(ToMonth.ToString()) <= 9)
                        {
                            ToMonth = "0" + ToMonth;
                        }
                        string ToDateQry = ToYear + "-" + ToMonth + "-" + ToDate;
                        DAL_RptRecovery NewDALObj = new DAL_RptRecovery();
                        dt = NewDALObj.Rpt_DirectSalesBalance(FromDate, ToDateQry);
                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("No Record Found", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            dt.Dispose();
                            FromDate = "";
                            ToDateQry = "";
                        }
                        else
                        {
                            NewDirectSalesBalance.SetDataSource(dt);
                            TextObject txtFromDate = (TextObject)NewDirectSalesBalance.ReportDefinition.ReportObjects["txtFromDate"];
                            txtFromDate.Text = FromDate;
                            TextObject txtToDate = (TextObject)NewDirectSalesBalance.ReportDefinition.ReportObjects["txtToDate"];
                            txtToDate.Text = ToDateQry;
                            CommonDAL.ShowReport(NewDirectSalesBalance, "General Sales Report");
                            dt.Dispose();
                            FromDate = "";
                            ToDateQry = "";
                        }
                    }
                    // Detail Sale All Type
                    //if (lstvReports.SelectedItems[0].Key.Equals("Rpt_DirectSalesAllCustomers"))
                    if (UEBReport.ActiveItem.Text == "Detail Report")
                    {
                        Reports.Rpt_DirectSalesAllCustomers NewDirectSalesAll = new EasyRashanManagementSystem.Reports.Rpt_DirectSalesAllCustomers();
                        dt = new System.Data.DataTable();
                        string Fromday = dtpFrom.DateTime.Day.ToString();
                        string FromMonth = dtpFrom.DateTime.Month.ToString();
                        string Year = dtpFrom.DateTime.Year.ToString();
                        if (int.Parse(Fromday.ToString()) <= 9)
                        {
                            Fromday = "0" + Fromday;
                        }
                        if (int.Parse(FromMonth.ToString()) <= 9)
                        {
                            FromMonth = "0" + FromMonth;
                        }
                        string FromDate = Year + "-" + FromMonth + "-" + Fromday;

                        string ToDate = dtpTo.DateTime.Day.ToString();
                        string ToMonth = dtpTo.DateTime.Month.ToString();
                        string ToYear = dtpTo.DateTime.Year.ToString();

                        if (int.Parse(ToDate.ToString()) <= 9)
                        {
                            ToDate = "0" + ToDate;
                        }
                        if (int.Parse(ToMonth.ToString()) <= 9)
                        {
                            ToMonth = "0" + ToMonth;
                        }
                        string ToDateQry = ToYear + "-" + ToMonth + "-" + ToDate;
                        dt = NewBP.RptDirectSalesAllCustomer(FromDate, ToDateQry);
                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("No Record Found", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            dt.Dispose();
                            FromDate = "";
                            ToDateQry = "";
                        }
                        else
                        {
                            NewDirectSalesAll.SetDataSource(dt);
                            TextObject txtFromDate = (TextObject)NewDirectSalesAll.ReportDefinition.ReportObjects["txtFromDate"];
                            txtFromDate.Text = FromDate;
                            TextObject txtToDate = (TextObject)NewDirectSalesAll.ReportDefinition.ReportObjects["txtToDate"];
                            txtToDate.Text = ToDateQry;
                            CommonDAL.ShowReport(NewDirectSalesAll, "Detail All Sales");
                            dt.Dispose();
                            FromDate = "";
                            ToDateQry = "";
                        }
                    }

                    // General Loan Report
                    if (UEBReport.ActiveItem.Text == "General Loan Report")
                    {
                        Reports.Rpt_Investment rptInvestment = new Reports.Rpt_Investment();
                        string Fromday = dtpFrom.DateTime.Day.ToString();
                        string FromMonth = dtpFrom.DateTime.Month.ToString();
                        string Year = dtpFrom.DateTime.Year.ToString();
                        if (int.Parse(Fromday.ToString()) <= 9)
                        {
                            Fromday = "0" + Fromday;
                        }
                        if (int.Parse(FromMonth.ToString()) <= 9)
                        {
                            FromMonth = "0" + FromMonth;
                        }
                        string FromDate = Year + "-" + FromMonth + "-" + Fromday;

                        string ToDate = dtpTo.DateTime.Day.ToString();
                        string ToMonth = dtpTo.DateTime.Month.ToString();
                        string ToYear = dtpTo.DateTime.Year.ToString();

                        if (int.Parse(ToDate.ToString()) <= 9)
                        {
                            ToDate = "0" + ToDate;
                        }
                        if (int.Parse(ToMonth.ToString()) <= 9)
                        {
                            ToMonth = "0" + ToMonth;
                        }
                        string ToDateQry = ToYear + "-" + ToMonth + "-" + ToDate;
                        dt = new System.Data.DataTable();
                        dt = NewBP.RptInvestment(FromDate, ToDateQry);
                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("No Record Found", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            dt.Dispose();
                            FromDate = "";
                            ToDateQry = "";
                        }
                        else
                        {
                            rptInvestment.SetDataSource(dt);
                            CommonDAL.ShowReport(rptInvestment, "Loan Report");
                            dt.Dispose();
                            rptInvestment.Dispose();
                            FromDate = "";
                            ToDateQry = "";
                        }
                    }
                    // Loan Report by Person History
                    if (UEBReport.ActiveItem.Text == "Loan Report By Person")
                    {
                        Reports.Rpt_LoanByPerson rptInvestment = new Reports.Rpt_LoanByPerson();
                        dt = new System.Data.DataTable();
                        dt = NewDALObj.GetPersonRecord(PersonID);
                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("No Record Found", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            dt.Dispose();
                        }
                        else
                        {
                            rptInvestment.SetDataSource(dt);
                            CommonDAL.ShowReport(rptInvestment, "Loan Report By Person");
                            dt.Dispose();
                            rptInvestment.Dispose();
                        }
                    }

                    // Welfare Report
                    if (UEBReport.ActiveItem.Text == "Welfare Report")
                    {
                        Reports.Rpt_Walfare rptWalfare = new Reports.Rpt_Walfare();
                        string Fromday = dtpFrom.DateTime.Day.ToString();
                        string FromMonth = dtpFrom.DateTime.Month.ToString();
                        string Year = dtpFrom.DateTime.Year.ToString();
                        if (int.Parse(Fromday.ToString()) <= 9)
                        {
                            Fromday = "0" + Fromday;
                        }
                        if (int.Parse(FromMonth.ToString()) <= 9)
                        {
                            FromMonth = "0" + FromMonth;
                        }
                        string FromDate = Year + "-" + FromMonth + "-" + Fromday;

                        string ToDate = dtpTo.DateTime.Day.ToString();
                        string ToMonth = dtpTo.DateTime.Month.ToString();
                        string ToYear = dtpTo.DateTime.Year.ToString();

                        if (int.Parse(ToDate.ToString()) <= 9)
                        {
                            ToDate = "0" + ToDate;
                        }
                        if (int.Parse(ToMonth.ToString()) <= 9)
                        {
                            ToMonth = "0" + ToMonth;
                        }
                        string ToDateQry = ToYear + "-" + ToMonth + "-" + ToDate;
                        dt = new System.Data.DataTable();
                        dt = NewDALObj.RptWalfare(FromDate, ToDateQry);
                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("No Record Found", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            dt.Dispose();
                            FromDate = "";
                            ToDateQry = "";
                        }
                        else
                        {
                            rptWalfare.SetDataSource(dt);
                            CommonDAL.ShowReport(rptWalfare, "Walfare Report");
                            dt.Dispose();
                            rptWalfare.Dispose();
                            FromDate = "";
                            ToDateQry = "";
                        }
                    }

                    // Expense Report
                    if (UEBReport.ActiveItem.Text == "Expense Report")
                    {
                        Reports.Rpt_Expenses rptWalfare = new Reports.Rpt_Expenses();
                        string Fromday = dtpFrom.DateTime.Day.ToString();
                        string FromMonth = dtpFrom.DateTime.Month.ToString();
                        string Year = dtpFrom.DateTime.Year.ToString();
                        if (int.Parse(Fromday.ToString()) <= 9)
                        {
                            Fromday = "0" + Fromday;
                        }
                        if (int.Parse(FromMonth.ToString()) <= 9)
                        {
                            FromMonth = "0" + FromMonth;
                        }
                        string FromDate = Year + "-" + FromMonth + "-" + Fromday;

                        string ToDate = dtpTo.DateTime.Day.ToString();
                        string ToMonth = dtpTo.DateTime.Month.ToString();
                        string ToYear = dtpTo.DateTime.Year.ToString();

                        if (int.Parse(ToDate.ToString()) <= 9)
                        {
                            ToDate = "0" + ToDate;
                        }
                        if (int.Parse(ToMonth.ToString()) <= 9)
                        {
                            ToMonth = "0" + ToMonth;
                        }
                        string ToDateQry = ToYear + "-" + ToMonth + "-" + ToDate;
                        dt = new System.Data.DataTable();
                        dt = NewDALObj.RptExpenses(FromDate, ToDateQry);
                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("No Record Found", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            dt.Dispose();
                            FromDate = "";
                            ToDateQry = "";
                        }
                        else
                        {
                            rptWalfare.SetDataSource(dt);
                            CommonDAL.ShowReport(rptWalfare, "Expense Report");
                            dt.Dispose();
                            rptWalfare.Dispose();
                            FromDate = "";
                            ToDateQry = "";
                        }
                    }

                    // Previous Income Report
                    if (UEBReport.ActiveItem.Text == "Previous Income Report")
                    {
                        Reports.Rpt_PreviousData rptWalfare = new Reports.Rpt_PreviousData();
                        string Fromday = dtpFrom.DateTime.Day.ToString();
                        string FromMonth = dtpFrom.DateTime.Month.ToString();
                        string Year = dtpFrom.DateTime.Year.ToString();
                        if (int.Parse(Fromday.ToString()) <= 9)
                        {
                            Fromday = "0" + Fromday;
                        }
                        if (int.Parse(FromMonth.ToString()) <= 9)
                        {
                            FromMonth = "0" + FromMonth;
                        }
                        string FromDate = Year + "-" + FromMonth + "-" + Fromday;

                        string ToDate = dtpTo.DateTime.Day.ToString();
                        string ToMonth = dtpTo.DateTime.Month.ToString();
                        string ToYear = dtpTo.DateTime.Year.ToString();

                        if (int.Parse(ToDate.ToString()) <= 9)
                        {
                            ToDate = "0" + ToDate;
                        }
                        if (int.Parse(ToMonth.ToString()) <= 9)
                        {
                            ToMonth = "0" + ToMonth;
                        }
                        string ToDateQry = ToYear + "-" + ToMonth + "-" + ToDate;
                        dt = new System.Data.DataTable();
                        dt = NewDALObj.RptPreviousRecord(FromDate, ToDateQry);
                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("No Record Found", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            dt.Dispose();
                            FromDate = "";
                            ToDateQry = "";
                        }
                        else
                        {
                            rptWalfare.SetDataSource(dt);
                            CommonDAL.ShowReport(rptWalfare, "Previous Income Report");
                            dt.Dispose();
                            rptWalfare.Dispose();
                            FromDate = "";
                            ToDateQry = "";
                        }
                    }

                    // Previous Gift Report
                    if (UEBReport.ActiveItem.Text == "Previous Gift Report")
                    {
                        Reports.Rpt_PreviousGiftData rptWalfare = new Reports.Rpt_PreviousGiftData();
                        string Fromday = dtpFrom.DateTime.Day.ToString();
                        string FromMonth = dtpFrom.DateTime.Month.ToString();
                        string Year = dtpFrom.DateTime.Year.ToString();
                        if (int.Parse(Fromday.ToString()) <= 9)
                        {
                            Fromday = "0" + Fromday;
                        }
                        if (int.Parse(FromMonth.ToString()) <= 9)
                        {
                            FromMonth = "0" + FromMonth;
                        }
                        string FromDate = Year + "-" + FromMonth + "-" + Fromday;

                        string ToDate = dtpTo.DateTime.Day.ToString();
                        string ToMonth = dtpTo.DateTime.Month.ToString();
                        string ToYear = dtpTo.DateTime.Year.ToString();

                        if (int.Parse(ToDate.ToString()) <= 9)
                        {
                            ToDate = "0" + ToDate;
                        }
                        if (int.Parse(ToMonth.ToString()) <= 9)
                        {
                            ToMonth = "0" + ToMonth;
                        }
                        string ToDateQry = ToYear + "-" + ToMonth + "-" + ToDate;
                        dt = new System.Data.DataTable();
                        dt = NewDALObj.RptPreviousGiftRecord(FromDate, ToDateQry);
                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("No Record Found", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            dt.Dispose();
                            FromDate = "";
                            ToDateQry = "";
                        }
                        else
                        {
                            rptWalfare.SetDataSource(dt);
                            CommonDAL.ShowReport(rptWalfare, "Previous Gift Report");
                            dt.Dispose();
                            rptWalfare.Dispose();
                            FromDate = "";
                            ToDateQry = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            //old code by list view
            //System.Data.DataTable dt = new System.Data.DataTable();
            //BP_RptRecovery NewBP = new BP_RptRecovery();
            //try
            //{
            //    if (ValidateReportForm())
            //    {
            //        // Stock Purchase Report
            //        if (lstvReports.SelectedItems[0].Key.Equals("Rpt_Purchase"))
            //        {
            //            System.Data.DataTable dtpr = new System.Data.DataTable();
            //            BP_RptRecovery NewBPPr = new BP_RptRecovery();
            //            string FromDay = dtpFrom.DateTime.Day.ToString();
            //            string FromMonth = Convert.ToString(dtpFrom.DateTime.Month);
            //            string FromMonthName = dtpFrom.DateTime.ToString("MMMM");
            //            string FromYear = Convert.ToString(dtpFrom.DateTime.Year);
            //            string FromDate = "";
            //            if (Convert.ToInt32(FromDay) <= 9)
            //            {
            //                FromDay = "0" + FromDay;
            //            }
            //            if (Convert.ToInt32(FromMonth) <= 9)
            //            {
            //                FromMonth = "0" + FromMonth;
            //            }

            //            FromDate = FromYear + "-" + FromMonth + "-" + FromDay.ToString();

            //            string Today = dtpTo.DateTime.Day.ToString();
            //            string ToMonth = Convert.ToString(dtpTo.DateTime.Month);
            //            string ToYear = Convert.ToString(dtpTo.DateTime.Year);
            //            string ToMonthName = dtpTo.DateTime.ToString("MMMM");
            //            string ToDate = "";
            //            if (Convert.ToInt32(Today) <= 9)
            //            {
            //                Today = "0" + Today;
            //            }
            //            if (Convert.ToInt32(ToMonth) <= 9)
            //            {
            //                ToMonth = "0" + ToMonth;
            //            }

            //            ToDate = ToYear + "-" + ToMonth + "-" + Today;
            //            dtpr = NewBPPr.ShowPurchaseItems(FromDate, ToDate);
            //            if (dtpr.Rows.Count == 0)
            //            {
            //                MessageBox.Show("No Record Found", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            }
            //            else
            //            {
            //                Reports.Rpt_Purchase rpt = new EasyRashanManagementSystem.Reports.Rpt_Purchase();
            //                TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["txtFromDate"];
            //                txtFrom.Text = FromDate;
            //                TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["txtToDate"];
            //                txtTo.Text = ToDate;
            //                rpt.SetDataSource(dtpr);
            //                CommonDAL.ShowReport(rpt, "All PurchaseItems");
            //                dtpr.Dispose();
            //            }
            //        }

            //        // Stock Purchase Report By Item
            //        if (lstvReports.SelectedItems[0].Key.Equals("Rpt_PurchaseItem"))
            //        {
            //            System.Data.DataTable dtpr = new System.Data.DataTable();
            //            BP_RptRecovery NewBPPr = new BP_RptRecovery();
            //            string FromDay = dtpFrom.DateTime.Day.ToString();
            //            string FromMonth = Convert.ToString(dtpFrom.DateTime.Month);
            //            string FromMonthName = dtpFrom.DateTime.ToString("MMMM");
            //            string FromYear = Convert.ToString(dtpFrom.DateTime.Year);
            //            string FromDate = "";
            //            if (Convert.ToInt32(FromDay) <= 9)
            //            {
            //                FromDay = "0" + FromDay;
            //            }
            //            if (Convert.ToInt32(FromMonth) <= 9)
            //            {
            //                FromMonth = "0" + FromMonth;
            //            }

            //            FromDate = FromYear + "-" + FromMonth + "-" + FromDay.ToString();

            //            string Today = dtpTo.DateTime.Day.ToString();
            //            string ToMonth = Convert.ToString(dtpTo.DateTime.Month);
            //            string ToYear = Convert.ToString(dtpTo.DateTime.Year);
            //            string ToMonthName = dtpTo.DateTime.ToString("MMMM");
            //            string ToDate = "";
            //            if (Convert.ToInt32(Today) <= 9)
            //            {
            //                Today = "0" + Today;
            //            }
            //            if (Convert.ToInt32(ToMonth) <= 9)
            //            {
            //                ToMonth = "0" + ToMonth;
            //            }

            //            ToDate = ToYear + "-" + ToMonth + "-" + Today;
            //            if (!ItemName.Trim().Equals(""))
            //            {
            //                dtpr = NewDALObj.ShowPurchasePerItems(FromDate, ToDate, ItemID);
            //                if (dtpr.Rows.Count == 0)
            //                {
            //                    MessageBox.Show("No Record Found", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //                }
            //                else
            //                {
            //                    Reports.Rpt_PurchaseItem rpt = new EasyRashanManagementSystem.Reports.Rpt_PurchaseItem();
            //                    TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["txtFromDate"];
            //                    txtFrom.Text = FromDate;
            //                    TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["txtToDate"];
            //                    txtTo.Text = ToDate;
            //                    rpt.SetDataSource(dtpr);
            //                    CommonDAL.ShowReport(rpt, "All PurchaseItems");
            //                    dtpr.Dispose();
            //                }
            //            }
            //            else
            //            {
            //                MessageBox.Show("Please Select ItemName", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //                CboType.Focus();
            //            }
            //        }

            //        // Stock Balance Report
            //        if (lstvReports.SelectedItems[0].Key.Equals("RptStockBalance"))
            //        {
            //            System.Data.DataTable dtStock = new System.Data.DataTable();
            //            DAL_RptRecovery NewBPObj = new DAL_RptRecovery();
            //            dtStock = NewBPObj.GetStockBalance();
            //            if (dtStock.Rows.Count == 0)
            //            {
            //                MessageBox.Show("No Record Found", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            }
            //            else
            //            {
            //                Reports.RptStockBalance rpt = new EasyRashanManagementSystem.Reports.RptStockBalance();
            //                rpt.SetDataSource(dtStock);
            //                TextObject txtDate = (TextObject)rpt.ReportDefinition.ReportObjects["txtDate"];
            //                txtDate.Text = DateTime.Now.ToString();
            //                CommonDAL.ShowReport(rpt, "Stock Balance Report");
            //                dtStock.Dispose();
            //            }
            //        }

            //        // Detail Sale All Type
            //        if (lstvReports.SelectedItems[0].Key.Equals("Rpt_DirectSalesAllCustomers"))
            //        {
            //            Reports.Rpt_DirectSalesAllCustomers NewDirectSalesAll = new EasyRashanManagementSystem.Reports.Rpt_DirectSalesAllCustomers();
            //            dt = new System.Data.DataTable();
            //            string Fromday = dtpFrom.DateTime.Day.ToString();
            //            string FromMonth = dtpFrom.DateTime.Month.ToString();
            //            string Year = dtpFrom.DateTime.Year.ToString();
            //            if (int.Parse(Fromday.ToString()) <= 9)
            //            {
            //                Fromday = "0" + Fromday;
            //            }
            //            if (int.Parse(FromMonth.ToString()) <= 9)
            //            {
            //                FromMonth = "0" + FromMonth;
            //            }
            //            string FromDate = Year + "-" + FromMonth + "-" + Fromday;

            //            string ToDate = dtpTo.DateTime.Day.ToString();
            //            string ToMonth = dtpTo.DateTime.Month.ToString();
            //            string ToYear = dtpTo.DateTime.Year.ToString();

            //            if (int.Parse(ToDate.ToString()) <= 9)
            //            {
            //                ToDate = "0" + ToDate;
            //            }
            //            if (int.Parse(ToMonth.ToString()) <= 9)
            //            {
            //                ToMonth = "0" + ToMonth;
            //            }
            //            string ToDateQry = ToYear + "-" + ToMonth + "-" + ToDate;
            //            dt = NewBP.RptDirectSalesAllCustomer(FromDate, ToDateQry);
            //            if (dt.Rows.Count == 0)
            //            {
            //                MessageBox.Show("No Record Found", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //                dt.Dispose();
            //                FromDate = "";
            //                ToDateQry = "";
            //            }
            //            else
            //            {
            //                NewDirectSalesAll.SetDataSource(dt);
            //                TextObject txtFromDate = (TextObject)NewDirectSalesAll.ReportDefinition.ReportObjects["txtFromDate"];
            //                txtFromDate.Text = FromDate;
            //                TextObject txtToDate = (TextObject)NewDirectSalesAll.ReportDefinition.ReportObjects["txtToDate"];
            //                txtToDate.Text = ToDateQry;
            //                CommonDAL.ShowReport(NewDirectSalesAll, "Direct Sales Of All Customers");
            //                dt.Dispose();
            //                FromDate = "";
            //                ToDateQry = "";
            //            }
            //        }

            //        // Detail Sale By Specific Type
            //        if (lstvReports.SelectedItems[0].Key.Equals("Rpt_DirectSaleBySpecificType"))
            //        {
            //            Reports.Rpt_DirectSalesAllCustomersByType NewDirectSalesAll = new EasyRashanManagementSystem.Reports.Rpt_DirectSalesAllCustomersByType();
            //            dt = new System.Data.DataTable();
            //            string Fromday = dtpFrom.DateTime.Day.ToString();
            //            string FromMonth = dtpFrom.DateTime.Month.ToString();
            //            string Year = dtpFrom.DateTime.Year.ToString();
            //            if (int.Parse(Fromday.ToString()) <= 9)
            //            {
            //                Fromday = "0" + Fromday;
            //            }
            //            if (int.Parse(FromMonth.ToString()) <= 9)
            //            {
            //                FromMonth = "0" + FromMonth;
            //            }
            //            string FromDate = Year + "-" + FromMonth + "-" + Fromday;

            //            string ToDate = dtpTo.DateTime.Day.ToString();
            //            string ToMonth = dtpTo.DateTime.Month.ToString();
            //            string ToYear = dtpTo.DateTime.Year.ToString();

            //            if (int.Parse(ToDate.ToString()) <= 9)
            //            {
            //                ToDate = "0" + ToDate;
            //            }
            //            if (int.Parse(ToMonth.ToString()) <= 9)
            //            {
            //                ToMonth = "0" + ToMonth;
            //            }
            //            string ToDateQry = ToYear + "-" + ToMonth + "-" + ToDate;
            //            if (!SaleType.Trim().Equals(""))
            //            {

            //                dt = NewDALObj.RptDirectSalesBySpecificType(FromDate, ToDateQry, SaleType);
            //                if (dt.Rows.Count == 0)
            //                {
            //                    MessageBox.Show("No Record Found", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //                    dt.Dispose();
            //                    FromDate = "";
            //                    ToDateQry = "";
            //                }
            //                else
            //                {
            //                    NewDirectSalesAll.SetDataSource(dt);
            //                    TextObject txtFromDate = (TextObject)NewDirectSalesAll.ReportDefinition.ReportObjects["txtFromDate"];
            //                    txtFromDate.Text = FromDate;
            //                    TextObject txtToDate = (TextObject)NewDirectSalesAll.ReportDefinition.ReportObjects["txtToDate"];
            //                    txtToDate.Text = ToDateQry;
            //                    CommonDAL.ShowReport(NewDirectSalesAll, "Direct Sales");
            //                    dt.Dispose();
            //                    FromDate = "";
            //                    ToDateQry = "";
            //                }
            //            }
            //            else
            //            {
            //                MessageBox.Show("Please Select Type", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //                CboType.Focus();
            //            }
            //        }

            //        // Purchase and Sale By month 
            //        if (lstvReports.SelectedItems[0].Key.Equals("Rpt_SaleBalance"))
            //        {
            //            Reports.Rpt_StockOutBalance NewDirectSalesBalance = new Reports.Rpt_StockOutBalance();
            //            dt = new System.Data.DataTable();
            //            string Fromday = dtpFrom.DateTime.Day.ToString();
            //            string FromMonth = dtpFrom.DateTime.Month.ToString();
            //            string Year = dtpFrom.DateTime.Year.ToString();
            //            if (int.Parse(Fromday.ToString()) <= 9)
            //            {
            //                Fromday = "0" + Fromday;
            //            }
            //            if (int.Parse(FromMonth.ToString()) <= 9)
            //            {
            //                FromMonth = "0" + FromMonth;
            //            }
            //            string FromDate = Year + "-" + FromMonth + "-" + Fromday;

            //            string ToDate = dtpTo.DateTime.Day.ToString();
            //            string ToMonth = dtpTo.DateTime.Month.ToString();
            //            string ToYear = dtpTo.DateTime.Year.ToString();

            //            if (int.Parse(ToDate.ToString()) <= 9)
            //            {
            //                ToDate = "0" + ToDate;
            //            }
            //            if (int.Parse(ToMonth.ToString()) <= 9)
            //            {
            //                ToMonth = "0" + ToMonth;
            //            }
            //            string ToDateQry = ToYear + "-" + ToMonth + "-" + ToDate;
            //            DAL_RptRecovery NewDALObj = new DAL_RptRecovery();
            //            dt = NewDALObj.Rpt_DirectSalesBalance(FromDate, ToDateQry);
            //            if (dt.Rows.Count == 0)
            //            {
            //                MessageBox.Show("No Record Found", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //                dt.Dispose();
            //                FromDate = "";
            //                ToDateQry = "";
            //            }
            //            else
            //            {
            //                NewDirectSalesBalance.SetDataSource(dt);
            //                TextObject txtFromDate = (TextObject)NewDirectSalesBalance.ReportDefinition.ReportObjects["txtFromDate"];
            //                txtFromDate.Text = FromDate;
            //                TextObject txtToDate = (TextObject)NewDirectSalesBalance.ReportDefinition.ReportObjects["txtToDate"];
            //                txtToDate.Text = ToDateQry;
            //                CommonDAL.ShowReport(NewDirectSalesBalance, "Direct Sales Of All Customers");
            //                dt.Dispose();
            //                FromDate = "";
            //                ToDateQry = "";
            //            }
            //        }
            //        // Purchase and Sale By month  by specific type like loan customer and gift
            //        if (lstvReports.SelectedItems[0].Key.Equals("Rpt_DirectSaleByType"))
            //        {
            //            Reports.Rpt_StockOutByType NewDirectSalesBalance = new Reports.Rpt_StockOutByType();
            //            dt = new System.Data.DataTable();
            //            string Fromday = dtpFrom.DateTime.Day.ToString();
            //            string FromMonth = dtpFrom.DateTime.Month.ToString();
            //            string Year = dtpFrom.DateTime.Year.ToString();
            //            if (int.Parse(Fromday.ToString()) <= 9)
            //            {
            //                Fromday = "0" + Fromday;
            //            }
            //            if (int.Parse(FromMonth.ToString()) <= 9)
            //            {
            //                FromMonth = "0" + FromMonth;
            //            }
            //            string FromDate = Year + "-" + FromMonth + "-" + Fromday;

            //            string ToDate = dtpTo.DateTime.Day.ToString();
            //            string ToMonth = dtpTo.DateTime.Month.ToString();
            //            string ToYear = dtpTo.DateTime.Year.ToString();

            //            if (int.Parse(ToDate.ToString()) <= 9)
            //            {
            //                ToDate = "0" + ToDate;
            //            }
            //            if (int.Parse(ToMonth.ToString()) <= 9)
            //            {
            //                ToMonth = "0" + ToMonth;
            //            }
            //            string ToDateQry = ToYear + "-" + ToMonth + "-" + ToDate;
            //            DAL_RptRecovery NewDALObj = new DAL_RptRecovery();
            //          if(!SaleType.Trim().Equals(""))
            //            {
            //            dt = NewDALObj.Rpt_DirectSalesByType(FromDate, ToDateQry,SaleType);
            //            if (dt.Rows.Count == 0)
            //            {
            //                MessageBox.Show("No Record Found", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //                dt.Dispose();
            //                FromDate = "";
            //                ToDateQry = "";
            //            }
            //            else
            //            {
            //                NewDirectSalesBalance.SetDataSource(dt);
            //                TextObject txtFromDate = (TextObject)NewDirectSalesBalance.ReportDefinition.ReportObjects["txtFromDate"];
            //                txtFromDate.Text = FromDate;
            //                TextObject txtToDate = (TextObject)NewDirectSalesBalance.ReportDefinition.ReportObjects["txtToDate"];
            //                txtToDate.Text = ToDateQry;
            //                CommonDAL.ShowReport(NewDirectSalesBalance, "Direct Sales");
            //                dt.Dispose();
            //                FromDate = "";
            //                ToDateQry = "";
            //            }
            //            }
            //            else
            //            {
            //               MessageBox.Show("Please Select Type", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //              CboType.Focus();
            //            }
            //        }

            //        // General Loan Report
            //        if (lstvReports.SelectedItems[0].Key.Equals("Rpt_Loan"))
            //        {
            //            Reports.Rpt_Investment rptInvestment = new Reports.Rpt_Investment();
            //            string Fromday = dtpFrom.DateTime.Day.ToString();
            //            string FromMonth = dtpFrom.DateTime.Month.ToString();
            //            string Year = dtpFrom.DateTime.Year.ToString();
            //            if (int.Parse(Fromday.ToString()) <= 9)
            //            {
            //                Fromday = "0" + Fromday;
            //            }
            //            if (int.Parse(FromMonth.ToString()) <= 9)
            //            {
            //                FromMonth = "0" + FromMonth;
            //            }
            //            string FromDate = Year + "-" + FromMonth + "-" + Fromday;

            //            string ToDate = dtpTo.DateTime.Day.ToString();
            //            string ToMonth = dtpTo.DateTime.Month.ToString();
            //            string ToYear = dtpTo.DateTime.Year.ToString();

            //            if (int.Parse(ToDate.ToString()) <= 9)
            //            {
            //                ToDate = "0" + ToDate;
            //            }
            //            if (int.Parse(ToMonth.ToString()) <= 9)
            //            {
            //                ToMonth = "0" + ToMonth;
            //            }
            //            string ToDateQry = ToYear + "-" + ToMonth + "-" + ToDate;
            //            dt = new System.Data.DataTable();
            //            dt = NewBP.RptInvestment(FromDate, ToDateQry);
            //            if (dt.Rows.Count == 0)
            //            {
            //                MessageBox.Show("No Record Found", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //                dt.Dispose();
            //                FromDate = "";
            //                ToDateQry = "";
            //            }
            //            else
            //            {
            //                rptInvestment.SetDataSource(dt);
            //                CommonDAL.ShowReport(rptInvestment, "Loan Report");
            //                dt.Dispose();
            //                rptInvestment.Dispose();
            //                FromDate = "";
            //                ToDateQry = "";
            //            }
            //        }
            //        // Loan Report by Person History
            //        if (lstvReports.SelectedItems[0].Key.Equals("Rpt_LoanByPerson"))
            //        {
            //            Reports.Rpt_LoanByPerson rptInvestment = new Reports.Rpt_LoanByPerson();
            //            dt = new System.Data.DataTable();
            //            dt = NewDALObj.GetPersonRecord(PersonID);
            //            if (dt.Rows.Count == 0)
            //            {
            //                MessageBox.Show("No Record Found", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //                dt.Dispose();
            //            }
            //            else
            //            {
            //                rptInvestment.SetDataSource(dt);
            //                CommonDAL.ShowReport(rptInvestment, "Loan Report By Person");
            //                dt.Dispose();
            //                rptInvestment.Dispose();
            //            }
            //        }

            //        // Welfare Report
            //        if (lstvReports.SelectedItems[0].Key.Equals("Rpt_Walfare"))
            //        {
            //            Reports.Rpt_Walfare rptWalfare = new Reports.Rpt_Walfare();
            //            string Fromday = dtpFrom.DateTime.Day.ToString();
            //            string FromMonth = dtpFrom.DateTime.Month.ToString();
            //            string Year = dtpFrom.DateTime.Year.ToString();
            //            if (int.Parse(Fromday.ToString()) <= 9)
            //            {
            //                Fromday = "0" + Fromday;
            //            }
            //            if (int.Parse(FromMonth.ToString()) <= 9)
            //            {
            //                FromMonth = "0" + FromMonth;
            //            }
            //            string FromDate = Year + "-" + FromMonth + "-" + Fromday;

            //            string ToDate = dtpTo.DateTime.Day.ToString();
            //            string ToMonth = dtpTo.DateTime.Month.ToString();
            //            string ToYear = dtpTo.DateTime.Year.ToString();

            //            if (int.Parse(ToDate.ToString()) <= 9)
            //            {
            //                ToDate = "0" + ToDate;
            //            }
            //            if (int.Parse(ToMonth.ToString()) <= 9)
            //            {
            //                ToMonth = "0" + ToMonth;
            //            }
            //            string ToDateQry = ToYear + "-" + ToMonth + "-" + ToDate;
            //            dt = new System.Data.DataTable();
            //            dt = NewDALObj.RptWalfare(FromDate, ToDateQry);
            //            if (dt.Rows.Count == 0)
            //            {
            //                MessageBox.Show("No Record Found", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //                dt.Dispose();
            //                FromDate = "";
            //                ToDateQry = "";
            //            }
            //            else
            //            {
            //                rptWalfare.SetDataSource(dt);
            //                CommonDAL.ShowReport(rptWalfare, "Walfare Report");
            //                dt.Dispose();
            //                rptWalfare.Dispose();
            //                FromDate = "";
            //                ToDateQry = "";
            //            }
            //        }

            //        // Expense Report
            //        if (lstvReports.SelectedItems[0].Key.Equals("Rpt_Expenses"))
            //        {
            //            Reports.Rpt_Expenses rptWalfare = new Reports.Rpt_Expenses();
            //            string Fromday = dtpFrom.DateTime.Day.ToString();
            //            string FromMonth = dtpFrom.DateTime.Month.ToString();
            //            string Year = dtpFrom.DateTime.Year.ToString();
            //            if (int.Parse(Fromday.ToString()) <= 9)
            //            {
            //                Fromday = "0" + Fromday;
            //            }
            //            if (int.Parse(FromMonth.ToString()) <= 9)
            //            {
            //                FromMonth = "0" + FromMonth;
            //            }
            //            string FromDate = Year + "-" + FromMonth + "-" + Fromday;

            //            string ToDate = dtpTo.DateTime.Day.ToString();
            //            string ToMonth = dtpTo.DateTime.Month.ToString();
            //            string ToYear = dtpTo.DateTime.Year.ToString();

            //            if (int.Parse(ToDate.ToString()) <= 9)
            //            {
            //                ToDate = "0" + ToDate;
            //            }
            //            if (int.Parse(ToMonth.ToString()) <= 9)
            //            {
            //                ToMonth = "0" + ToMonth;
            //            }
            //            string ToDateQry = ToYear + "-" + ToMonth + "-" + ToDate;
            //            dt = new System.Data.DataTable();
            //            dt = NewDALObj.RptExpenses(FromDate, ToDateQry);
            //            if (dt.Rows.Count == 0)
            //            {
            //                MessageBox.Show("No Record Found", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //                dt.Dispose();
            //                FromDate = "";
            //                ToDateQry = "";
            //            }
            //            else
            //            {
            //                rptWalfare.SetDataSource(dt);
            //                CommonDAL.ShowReport(rptWalfare, "Expense Report");
            //                dt.Dispose();
            //                rptWalfare.Dispose();
            //                FromDate = "";
            //                ToDateQry = "";
            //            }
            //        }

            //        // Expense Report
            //        if (lstvReports.SelectedItems[0].Key.Equals("Rpt_PreviousData"))
            //        {
            //            Reports.Rpt_PreviousData rptWalfare = new Reports.Rpt_PreviousData();
            //            string Fromday = dtpFrom.DateTime.Day.ToString();
            //            string FromMonth = dtpFrom.DateTime.Month.ToString();
            //            string Year = dtpFrom.DateTime.Year.ToString();
            //            if (int.Parse(Fromday.ToString()) <= 9)
            //            {
            //                Fromday = "0" + Fromday;
            //            }
            //            if (int.Parse(FromMonth.ToString()) <= 9)
            //            {
            //                FromMonth = "0" + FromMonth;
            //            }
            //            string FromDate = Year + "-" + FromMonth + "-" + Fromday;

            //            string ToDate = dtpTo.DateTime.Day.ToString();
            //            string ToMonth = dtpTo.DateTime.Month.ToString();
            //            string ToYear = dtpTo.DateTime.Year.ToString();

            //            if (int.Parse(ToDate.ToString()) <= 9)
            //            {
            //                ToDate = "0" + ToDate;
            //            }
            //            if (int.Parse(ToMonth.ToString()) <= 9)
            //            {
            //                ToMonth = "0" + ToMonth;
            //            }
            //            string ToDateQry = ToYear + "-" + ToMonth + "-" + ToDate;
            //            dt = new System.Data.DataTable();
            //            dt = NewDALObj.RptPreviousRecord(FromDate, ToDateQry);
            //            if (dt.Rows.Count == 0)
            //            {
            //                MessageBox.Show("No Record Found", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //                dt.Dispose();
            //                FromDate = "";
            //                ToDateQry = "";
            //            }
            //            else
            //            {
            //                rptWalfare.SetDataSource(dt);
            //                CommonDAL.ShowReport(rptWalfare, "Expense Report");
            //                dt.Dispose();
            //                rptWalfare.Dispose();
            //                FromDate = "";
            //                ToDateQry = "";
            //            }
            //        }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //}
        }

        private void lstvReports_ItemSelectionChanged(object sender, Infragistics.Win.UltraWinListView.ItemSelectionChangedEventArgs e)
        {
            if (lstvReports.SelectedItems[0].Key.Equals("Rpt_Purchase"))
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                cboPersonName.Enabled = false;
                cboMobileNo.Enabled = false;
                CboType.Enabled = false;
                cboItemName.Enabled = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("Rpt_PurchaseItem"))
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                cboPersonName.Enabled = false;
                cboMobileNo.Enabled = false;
                CboType.Enabled = false;
                cboItemName.Enabled = true;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("RptStockBalance"))
            {
                dtpFrom.Enabled = false;
                dtpTo.Enabled = false;
                cboPersonName.Enabled = false;
                cboMobileNo.Enabled = false;
                CboType.Enabled = false;
                cboItemName.Enabled = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("Rpt_DirectSalesAllCustomers"))
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                cboPersonName.Enabled = false;
                cboMobileNo.Enabled = false;
                CboType.Enabled = false;
                cboItemName.Enabled = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("Rpt_SaleBalance"))
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                cboPersonName.Enabled = false;
                cboMobileNo.Enabled = false;
                CboType.Enabled = false;
                cboItemName.Enabled = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("Rpt_DirectSaleByType"))
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                cboPersonName.Enabled = false;
                cboMobileNo.Enabled = false;
                CboType.Enabled = true;
                cboItemName.Enabled = false;
            }

            if (lstvReports.SelectedItems[0].Key.Equals("Rpt_DirectSaleBySpecificType"))
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                cboPersonName.Enabled = false;
                cboMobileNo.Enabled = false;
                CboType.Enabled = true;
                cboItemName.Enabled = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("Rpt_Loan"))
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                cboPersonName.Enabled = false;
                cboMobileNo.Enabled = false;
                CboType.Enabled = false;
                cboItemName.Enabled = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("Rpt_LoanByPerson"))
            {
                dtpFrom.Enabled = false;
                dtpTo.Enabled = false;
                cboPersonName.Enabled = true;
                cboMobileNo.Enabled = true;
                CboType.Enabled = false;
                cboItemName.Enabled = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("Rpt_Walfare"))
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                cboPersonName.Enabled = false;
                cboMobileNo.Enabled = false;
                CboType.Enabled = false;
                cboItemName.Enabled = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("Rpt_Expenses"))
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                cboPersonName.Enabled = false;
                cboMobileNo.Enabled = false;
                CboType.Enabled = false;
                cboItemName.Enabled = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("Rpt_PreviousData"))
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                cboPersonName.Enabled = false;
                cboMobileNo.Enabled = false;
                CboType.Enabled = false;
                cboItemName.Enabled = false;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RptRecovery obj = new RptRecovery();
            obj.ShowDialog();
        }

        private void cboPersonName_ValueChanged(object sender, EventArgs e)
        {
            PersonID = Convert.ToInt32(cboPersonName.Value);
            DALObj.LoadMobileNo(cboMobileNo, PersonID);
        }

        private void CboType_ValueChanged(object sender, EventArgs e)
        {
            SaleType = CboType.Text;
        }

        private void cboItemName_ValueChanged(object sender, EventArgs e)
        {
            ItemID = Convert.ToInt32(cboItemName.Value);
            ItemName = cboItemName.Text;
        }

        private void FrmReportForm_Paint(object sender, PaintEventArgs e)
        {
            int width = this.Width - 1;
            int height = this.Height - 1;
            Pen greenPen = new Pen(GlobalVaribles.BorderColor);
            e.Graphics.DrawRectangle(greenPen, 0, 0, width, height);
        }

        private void UEBReport_ItemClick(object sender, ItemEventArgs e)
        {
            Debug.WriteLine(string.Format("The item '{0}', from group '{1}' has been clicked.", e.Item, e.Item.Group));
            if (e.Item.Text == "Stock Purchase Report")
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                cboPersonName.Enabled = false;
                cboMobileNo.Enabled = false;
                CboType.Enabled = false;
                cboItemName.Enabled = false;
            }
            if (e.Item.Text == "Purchase Per Item Report")
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                cboPersonName.Enabled = false;
                cboMobileNo.Enabled = false;
                CboType.Enabled = false;
                cboItemName.Enabled = true;
            }
            if (e.Item.Text == "Stock Balance Report")
            {
                dtpFrom.Enabled = false;
                dtpTo.Enabled = false;
                cboPersonName.Enabled = false;
                cboMobileNo.Enabled = false;
                CboType.Enabled = false;
                cboItemName.Enabled = false;
            }
            
            if (e.Item.Text == "General Sale Report")
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                cboPersonName.Enabled = false;
                cboMobileNo.Enabled = false;
                CboType.Enabled = true;
                CboType.Value = dt.Rows[0][0].ToString();
                CboType.ReadOnly = true;
                cboItemName.Enabled = false;
            }
            if (e.Item.Text == "Detail Sale Report")
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                cboPersonName.Enabled = false;
                cboMobileNo.Enabled = false;
                CboType.Enabled = true;
                CboType.Value = dt.Rows[0][0].ToString();
                CboType.ReadOnly = true;
                cboItemName.Enabled = false;
            }
            if (e.Item.Text == "General Stock Laon Report")
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                cboPersonName.Enabled = false;
                cboMobileNo.Enabled = false;
                CboType.Enabled = true;
                CboType.Value = dt.Rows[2][0].ToString();
                CboType.ReadOnly = true;
                cboItemName.Enabled = false;
            }
            if (e.Item.Text == "Detail Stock Laon Report")
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                cboPersonName.Enabled = false;
                cboMobileNo.Enabled = false;
                CboType.Enabled = true;
                CboType.Value = dt.Rows[2][0].ToString();
                CboType.ReadOnly = true;
                cboItemName.Enabled = false;
            }
            if (e.Item.Text == "General Gift Report")
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                cboPersonName.Enabled = false;
                cboMobileNo.Enabled = false;
                CboType.Enabled = true;
                CboType.Value = dt.Rows[1][0].ToString();
                CboType.ReadOnly = true;
                cboItemName.Enabled = false;
            }
            if (e.Item.Text == "Detail Gift Report")
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                cboPersonName.Enabled = false;
                cboMobileNo.Enabled = false;
                CboType.Enabled = true;
                CboType.Value = dt.Rows[1][0].ToString();
                CboType.ReadOnly = true;
                cboItemName.Enabled = false;
            }
            if (e.Item.Text == "Genral Report")
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                cboPersonName.Enabled = false;
                cboMobileNo.Enabled = false;
                CboType.Enabled = false;
                cboItemName.Enabled = false;
            }
            if (e.Item.Text == "Detail Report")
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                cboPersonName.Enabled = false;
                cboMobileNo.Enabled = false;
                CboType.Enabled = false;
                cboItemName.Enabled = false;
            }
            if (e.Item.Text == "General Loan Report")
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                cboPersonName.Enabled = false;
                cboMobileNo.Enabled = false;
                CboType.Enabled = false;
                cboItemName.Enabled = false;
            }
            if (e.Item.Text == "Loan Report By Person")
            {
                dtpFrom.Enabled = false;
                dtpTo.Enabled = false;
                cboPersonName.Enabled = true;
                cboMobileNo.Enabled = true;
                CboType.Enabled = false;
                cboItemName.Enabled = false;
            }
            if (e.Item.Text == "Welfare Report")
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                cboPersonName.Enabled = false;
                cboMobileNo.Enabled = false;
                CboType.Enabled = false;
                cboItemName.Enabled = false;
            }
            if (e.Item.Text == "Expense Report")
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                cboPersonName.Enabled = false;
                cboMobileNo.Enabled = false;
                CboType.Enabled = false;
                cboItemName.Enabled = false;
            }
            if (e.Item.Text == "Previous Income Report")
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                cboPersonName.Enabled = false;
                cboMobileNo.Enabled = false;
                CboType.Enabled = false;
                cboItemName.Enabled = false;
            }
            if (e.Item.Text == "Previous Gift Report")
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                cboPersonName.Enabled = false;
                cboMobileNo.Enabled = false;
                CboType.Enabled = false;
                cboItemName.Enabled = false;
            }
            
        }

        #endregion

    }
}
