using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BusinessEntities;
using BusinessProcessObjects;
using DAL;
using System.Data.SqlClient;
using VersatileEducationSystem;
using CrystalDecisions.CrystalReports.Engine;

namespace EasyRashanManagementSystem
{
    public partial class RptClientIssuence : Form
    {
        public RptClientIssuence()
        {
            InitializeComponent();
        }

        #region Modifiers

        long ClientID;
        long PrID;
        long AreaID;
        long EmpID;

        #endregion

        #region Functions

        private bool ValidateReportForm()
        {
            try
            {
                if (lstvReports.SelectedItems[0].Key.Equals(""))
                {
                    MessageBox.Show("Please Select any report from Menu to Continue","Easy Rashan Management System",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    lstvReports.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Easy Rashan Management System",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return false;
            }
        }

        private void ClearForm()
        {
            ClientID = 0;
            PrID = 0;
            AreaID = 0;
            EmpID = 0;
        }


        #endregion

        #region Events

        private void btnClientID_Click(object sender, EventArgs e)
        {
            object rValue;
            try
            {
                rValue = FrmGSearch.Show("Select ClientID,ClientCode as MemberCode,ClientName As MemberName,NIC,Address From Trans_Clients",false,"All Clients");
                if (rValue == null)
                    return;
                ClientID = long.Parse(rValue.ToString());

                DataTable dt = new DataTable();
                BP_RptClientIssuencecDateWise NewBPObj = new BP_RptClientIssuencecDateWise();
                dt = NewBPObj.GetClientName(int.Parse(ClientID.ToString()));
                if (dt.Rows.Count > 0)
                {
                    txtMemberName.Text = dt.Rows[0]["ClientName"].ToString();
                }
                dt.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Easy Rashan Management System",MessageBoxButtons.OK,MessageBoxIcon.Stop);
            }

        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateReportForm()) return;
                if (lstvReports.SelectedItems[0].Key.Equals("RptClientIssuenceDateWise"))
                {
                    DataTable dt = new DataTable();
                    BP_RptClientIssuencecDateWise NewBP = new BP_RptClientIssuencecDateWise();
                    dt = NewBP.GetReportData(int.Parse(ClientID.ToString()));
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    else
                    {
                        Reports.RptClientIssuenceDateWise rpt = new EasyRashanManagementSystem.Reports.RptClientIssuenceDateWise();
                        rpt.SetDataSource(dt);

                        CommonDAL.ShowReport(rpt, "Isuence Report DateWise");
                        dt.Dispose();
                    }
                }
                if (lstvReports.SelectedItems[0].Key.Equals("RptStockBalance"))
                {
                    DataTable dt = new DataTable();

                    BP_RptStockBalance NewBPObj = new BP_RptStockBalance();
                    dt = NewBPObj.GetStockBalance();
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        Reports.RptStockBalance rpt = new EasyRashanManagementSystem.Reports.RptStockBalance();
                        rpt.SetDataSource(dt);
                        TextObject txtDate = (TextObject)rpt.ReportDefinition.ReportObjects["txtDate"];
                        txtDate.Text = DateTime.Now.ToString();
                        CommonDAL.ShowReport(rpt,"Stock Balance Report");
                        dt.Dispose();
                    }
                }
                if (lstvReports.SelectedItems[0].Key.Equals("Rpt_Purchase"))
                {
                    DataTable dt = new DataTable();
                    Bp_RptPurchase NewBP = new Bp_RptPurchase();
                    dt = NewBP.ShowPurchaseItems(int.Parse(PrID.ToString()));
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        Reports.Rpt_Purchase rpt = new EasyRashanManagementSystem.Reports.Rpt_Purchase();
                        rpt.SetDataSource(dt);
                        CommonDAL.ShowReport(rpt, "All PurchaseItems");
                        dt.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void lstvReports_ItemSelectionChanged(object sender, Infragistics.Win.UltraWinListView.ItemSelectionChangedEventArgs e)
        {
            if (lstvReports.SelectedItems[0].Key.Equals("RptClientIssuenceDateWise"))
            {
                btnClientID.Visible = true;
                txtMemberName.Visible = true;
                lblMemberName.Visible = true;
                lblVendorName.Visible = false;
                txtVendorName.Visible = false;
                btnVendor.Visible = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("RptStockBalance"))
            {
                btnClientID.Visible = false;
                txtMemberName.Visible = false;
                lblMemberName.Visible = false;
                lblVendorName.Visible = false;
                txtVendorName.Visible = false;
                btnVendor.Visible = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("Rpt_Purchase"))
            {
                lblVendorName.Visible = true;
                txtVendorName.Visible = true;
                btnVendor.Visible = true;
                btnClientID.Visible = false;
                txtMemberName.Visible = false;
                lblMemberName.Visible = false;
            }
        }

        private void btnVendor_Click(object sender, EventArgs e)
        {
            object rValue;
            rValue = FrmGSearch.Show("Select PrID,Convert(varchar(12),PrDate,106) AS [PrDate],VendorName From Trans_PurchaseM Where IsActive=1", false, "All Purchase Items");
            if (rValue == null)
                return;
            PrID = long.Parse(rValue.ToString());
            DataTable dt = new DataTable();
            Bp_RptPurchase NewBP = new Bp_RptPurchase();
            dt = NewBP.GetVendorName(int.Parse(PrID.ToString()));
            if (dt.Rows.Count > 0)
            {
                txtVendorName.Text = dt.Rows[0]["VendorName"].ToString();
            }
            dt.Dispose();
        }

        private void btnArea_Click(object sender, EventArgs e)
        {
            object rValue;
            try
            {
                rValue = FrmGSearch.Show("Select AreaID,AreaName From Areas_Def", false, "All Areas");
                if (rValue == null)
                    return;
                AreaID = long.Parse(rValue.ToString());
                DataTable dt = new DataTable();
                BP_RptClientIssuencecDateWise NewBP = new BP_RptClientIssuencecDateWise();
                dt = NewBP.GetAreaName(int.Parse(AreaID.ToString()));
                if (dt.Rows.Count > 0)
                {
                    txtArea.Text = dt.Rows[0]["AreaName"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnEmp_Click(object sender, EventArgs e)
        {
            object rValue;
            try
            {
                rValue = FrmGSearch.Show("Select EmpName From Emp_Def",false,"All Employees");
                if (rValue == null)
                    return;
                EmpID = long.Parse(rValue.ToString());
                DataTable dt = new DataTable();
                BP_RptClientIssuencecDateWise NewBP = new BP_RptClientIssuencecDateWise();
                dt=NewBP.GetEmpName(int.Parse(EmpID.ToString()));
                if (dt.Rows.Count>0)
                {
                    txtEmpName.Text=dt.Rows[0]["EmpName"].ToString();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Easy Rashan Management System",MessageBoxButtons.OK,MessageBoxIcon.Stop);
            }
        }

        #endregion

    }
}
