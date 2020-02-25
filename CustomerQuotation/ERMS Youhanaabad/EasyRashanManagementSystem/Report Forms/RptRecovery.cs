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
using System.IO;
using iTextSharp.text;
using iTextSharp;
using System.Reflection;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.Office.Interop.Excel;
using Infragistics.Win;
using Infragistics.Win.UltraWinListView;

namespace EasyRashanManagementSystem
{
    public partial class RptRecovery : Form
    {
        public RptRecovery()
        {
            InitializeComponent();
        }

        #region Modifiers

        long AreaID = 0;
        long EmpID = 0;
        long ClientID = 0;
        long PrID = 0;
        string MainQuery = "";
        string StartDate = "";
        string EndDate = "";
        long DRSMID = 0;
        long AreaIDInActive = 0;
        long EmpIDInActive = 0;

        #endregion

        #region Function

        private bool ValidateReportForm()
        {
            try
            {
                if (lstvReports.SelectedItems[0].Key.Equals(""))
                {
                    MessageBox.Show("Please Select a Report to Continue...", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    lstvReports.Focus();
                    return false;
                }
                if (lstvReports.SelectedItems[0].Key == null)
                {
                    MessageBox.Show("Please Select a Report to Continue...", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    lstvReports.Focus();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Easy Rashan Management Sytem", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
        }

        private void ClearForm()
        {
            ClientID = 0;
            PrID = 0;
            AreaID = 0;
            EmpID = 0;
            AreaIDInActive = 0;
            EmpIDInActive = 0;
        }

        private void LoadSalaryYear()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("YearID", typeof(int));
            dt.Columns.Add("YearName", typeof(string));

            DataRow dr = dt.NewRow();
            for (int i = 1; i <= 12; i++)
            {
                if (i == 1)
                {
                    dr["YearID"] = i;
                }
                else
                {
                    dr = dt.NewRow();
                    dr["YearID"] = i;
                }
                switch (i)
                {
                    case 1:
                        dr["YearName"] = "January";
                        dt.Rows.Add(dr);
                        break;
                    case 2:
                        dr["YearName"] = "February";
                        dt.Rows.Add(dr);
                        break;
                    case 3:
                        dr["YearName"] = "March";
                        dt.Rows.Add(dr);
                        break;
                    case 4:
                        dr["YearName"] = "April";
                        dt.Rows.Add(dr);
                        break;
                    case 5:
                        dr["YearName"] = "May";
                        dt.Rows.Add(dr);
                        break;
                    case 6:
                        dr["YearName"] = "June";
                        dt.Rows.Add(dr);
                        break;
                    case 7:
                        dr["YearName"] = "July";
                        dt.Rows.Add(dr);
                        break;
                    case 8:
                        dr["YearName"] = "August";
                        dt.Rows.Add(dr);
                        break;
                    case 9:
                        dr["YearName"] = "September";
                        dt.Rows.Add(dr);
                        break;
                    case 10:
                        dr["YearName"] = "Octobar";
                        dt.Rows.Add(dr);
                        break;
                    case 11:
                        dr["YearName"] = "November";
                        dt.Rows.Add(dr);
                        break;
                    case 12:
                        dr["YearName"] = "December";
                        dt.Rows.Add(dr);
                        break;
                }
            }
            cboMonthlyYear.ValueMember = "YearID";
            cboMonthlyYear.DisplayMember = "YearName";
            cboMonthlyYear.DataSource = dt;
            cboMonthlyYear.DisplayLayout.Bands[0].Columns[0].Hidden = true;
            cboMonthlyYear.Value = 1;
        }

        #endregion

        #region Events

        private void btnPreview_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            BP_RptRecovery NewBP = new BP_RptRecovery();
            try
            {
                if (!ValidateReportForm()) return;

                if (lstvReports.SelectedItems[0].Key.Equals("RptDailyActivity"))
                {
                    string CrDay = dtpAsOn.DateTime.Day.ToString();
                    string CrMonth = dtpAsOn.DateTime.Month.ToString();
                    string CrYear = dtpAsOn.DateTime.Year.ToString();
                    string CurrentDate = "";
                    if (Convert.ToInt32(CrDay) <= 9)
                    {
                        CrDay = "0" + CrDay;
                    }
                    if (Convert.ToInt32(CrMonth) <= 9)
                    {
                        CrMonth = "0" + CrMonth;
                    }
                    CurrentDate = CrYear + "-" + CrMonth + "-" + CrDay;




                    dt = NewBP.GetDailyActivity(CurrentDate);
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    else
                    {
                        Reports.RptDailyActivity rpt = new Reports.RptDailyActivity();
                        rpt.SetDataSource(dt);
                        CommonDAL.ShowReport(rpt, "Daily Activity Report");
                        dt.Dispose();
                    }
                }
                if (lstvReports.SelectedItems[0].Key.Equals("RptStockBalance"))
                {
                    System.Data.DataTable dtStock = new System.Data.DataTable();
                    DAL_RptRecovery NewBPObj = new DAL_RptRecovery();
                    dtStock = NewBPObj.GetStockBalance();
                    if (dtStock.Rows.Count == 0)
                    {
                        MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
                if (lstvReports.SelectedItems[0].Key.Equals("Rpt_Purchase"))
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
                    dtpr = NewBPPr.ShowPurchaseItems(FromDate,ToDate);
                    if (dtpr.Rows.Count == 0)
                    {
                        MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
                if (lstvReports.SelectedItems[0].Key.Equals("RptRecoveryAllClients"))
                {
                    System.Data.DataTable newdt = new System.Data.DataTable();
                    if (Convert.ToBoolean(chkAllDisb.CheckedValue) == true)
                    {
                        Reports.RptRecoveryAllClientsNodate RptAll = new Reports.RptRecoveryAllClientsNodate();
                        BP_RptRecovery NewObj = new BP_RptRecovery();
                        newdt = NewObj.AllClientDisbursmentReport();
                        if (newdt.Rows.Count == 0)
                        {
                            MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            newdt.Dispose();
                        }
                        else
                        {
                            RptAll.SetDataSource(newdt);
                            CommonDAL.ShowReport(RptAll, "All Client Disbursment Report");
                            newdt.Dispose();
                            RptAll.Dispose();
                        }
                    }
                    else
                    {
                        string AsOnDay = dtpAsOn.DateTime.Day.ToString();
                        string AsOnMonth = dtpAsOn.DateTime.Month.ToString();
                        string AsOnMonthName = dtpAsOn.DateTime.ToString("MMMM");
                        string AsOnYear = dtpAsOn.DateTime.Year.ToString();
                        string AsOnDate = "";
                        if (Convert.ToInt32(AsOnDay) <= 9)
                        {
                            AsOnDay = "0" + AsOnDay;
                        }
                        if (Convert.ToInt32(AsOnMonth) <= 9)
                        {
                            AsOnMonth = "0" + AsOnMonth;
                        }

                        AsOnDate = AsOnYear + "-" + AsOnMonth + "-" + AsOnDay;

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


                        BP_RptRecovery NewBPObj = new BP_RptRecovery();
                        newdt = NewBPObj.DisbursmentReport(FromDate, ToDate, Convert.ToBoolean(chkAson.CheckedValue), AsOnDate);
                        if (newdt.Rows.Count == 0)
                        {
                            MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                        else
                        {
                            if (Convert.ToBoolean(chkAson.CheckedValue) == true)
                            {
                                Reports.RptRecoveryAllClientsSingleDate newRptOneDay = new Reports.RptRecoveryAllClientsSingleDate();
                                newRptOneDay.SetDataSource(newdt);
                                TextObject txtAsOnDate = (TextObject)newRptOneDay.ReportDefinition.ReportObjects["txtAsOnDate"];
                                txtAsOnDate.Text = AsOnDay.ToString() + "-" + AsOnMonthName + "-" + AsOnYear;
                                CommonDAL.ShowReport(newRptOneDay, "Client Wise Disbursment Report");
                                newdt.Dispose();
                                newRptOneDay.Dispose();
                            }
                            else
                            {
                                Reports.RptRecoveryAllClients newRpt = new Reports.RptRecoveryAllClients();
                                newRpt.SetDataSource(newdt);
                                TextObject txtFrom = (TextObject)newRpt.ReportDefinition.ReportObjects["txtFromDate"];
                                txtFrom.Text = FromDay + "-" + FromMonthName + "-" + FromYear;
                                TextObject txtTo = (TextObject)newRpt.ReportDefinition.ReportObjects["txtToDate"];
                                txtTo.Text = Today + "-" + ToMonthName + "-" + ToYear;
                                CommonDAL.ShowReport(newRpt, "Date Wise Disbursment Report");
                                newdt.Dispose();
                                newRpt.Dispose();
                            }
                        }
                    }
                }
                if (lstvReports.SelectedItems[0].Key.Equals("RptRecoveryAreaWise"))
                {
                    if (Convert.ToBoolean(chkArea.CheckedValue) == true)
                    {
                        Reports.RptRecoveryAreaWise rptArea = new EasyRashanManagementSystem.Reports.RptRecoveryAreaWise();
                        BP_RptRecovery NewRptBPObj = new BP_RptRecovery();
                        System.Data.DataTable NewDtRpt = new System.Data.DataTable();

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
                        NewDtRpt = NewRptBPObj.RptAreaWiseAll(int.Parse(AreaID.ToString()), FromDate, ToDateQry);
                        if (NewDtRpt.Rows.Count == 0)
                        {
                            MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            NewDtRpt.Dispose();
                            AreaID = 0;
                        }
                        else
                        {
                            rptArea.SetDataSource(NewDtRpt);
                            TextObject txtFrom = (TextObject)rptArea.ReportDefinition.ReportObjects["txtFromDate"];
                            txtFrom.Text = FromDate;
                            TextObject txtTo = (TextObject)rptArea.ReportDefinition.ReportObjects["txtToDate"];
                            txtTo.Text = ToDateQry;
                            CommonDAL.ShowReport(rptArea, "Area Wise Due Report");
                            NewDtRpt.Dispose();
                            AreaID = 0;
                        }
                    }
                    else
                    {
                        Reports.RptRecoveryAreaWise rptArea = new EasyRashanManagementSystem.Reports.RptRecoveryAreaWise();
                        BP_RptRecovery NewRptBPObj = new BP_RptRecovery();
                        System.Data.DataTable NewDtRpt = new System.Data.DataTable();
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

                        NewDtRpt = NewRptBPObj.RptAreaWiseAll(int.Parse(AreaID.ToString()), FromDate, ToDateQry);
                        if (NewDtRpt.Rows.Count == 0)
                        {
                            MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            NewDtRpt.Dispose();
                        }
                        else
                        {
                            rptArea.SetDataSource(NewDtRpt);
                            TextObject txtFrom = (TextObject)rptArea.ReportDefinition.ReportObjects["txtFromDate"];
                            txtFrom.Text = FromDate;
                            TextObject txtTo = (TextObject)rptArea.ReportDefinition.ReportObjects["txtToDate"];
                            txtTo.Text = ToDateQry;
                            CommonDAL.ShowReport(rptArea, "Area Wise Due Report");
                            NewDtRpt.Dispose();
                            AreaID = 0;
                        }
                    }
                }
                if (lstvReports.SelectedItems[0].Key.Equals("RptRecoveryEmpWise"))
                {
                    if (Convert.ToBoolean(chkAllEmp.CheckedValue) == true && Convert.ToBoolean(chkArea.CheckedValue) == true)
                    {
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

                        Reports.RptRecoveryEmpWise rptAllEmp = new Reports.RptRecoveryEmpWise();
                        BP_RptRecovery NewEmpObj = new BP_RptRecovery();

                        dt = NewEmpObj.RptEmpWiseReport(int.Parse(EmpID.ToString()), FromDate, ToDateQry, int.Parse(AreaID.ToString()));

                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            dt.Dispose();
                            EmpID = 0;
                        }
                        else
                        {
                            rptAllEmp.SetDataSource(dt);
                            TextObject txtFrom = (TextObject)rptAllEmp.ReportDefinition.ReportObjects["txtFromDate"];
                            txtFrom.Text = FromDate;
                            TextObject txtTo = (TextObject)rptAllEmp.ReportDefinition.ReportObjects["txtToDate"];
                            txtTo.Text = ToDateQry;
                            CommonDAL.ShowReport(rptAllEmp, "All Emp Due Report");
                            dt.Dispose();
                            EmpID = 0;
                        }
                    }
                    else if (Convert.ToBoolean(chkAllEmp.CheckedValue) == false && Convert.ToBoolean(chkArea.CheckedValue) == true)
                    {
                        Reports.RptRecoveryEmpWise rptAllEmpWise = new Reports.RptRecoveryEmpWise();
                        BP_RptRecovery NewEmpObj = new BP_RptRecovery();
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
                        dt = NewEmpObj.RptEmpWiseReport(int.Parse(EmpID.ToString()), FromDate, ToDateQry, int.Parse(AreaID.ToString()));
                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            dt.Dispose();
                        }
                        else
                        {
                            rptAllEmpWise.SetDataSource(dt);
                            TextObject txtFrom = (TextObject)rptAllEmpWise.ReportDefinition.ReportObjects["txtFromDate"];
                            txtFrom.Text = FromDate;
                            TextObject txtTo = (TextObject)rptAllEmpWise.ReportDefinition.ReportObjects["txtToDate"];
                            txtTo.Text = ToDateQry;
                            CommonDAL.ShowReport(rptAllEmpWise, "All Emp Due Report");
                            dt.Dispose();
                            EmpID = 0;
                            AreaID = 0;
                        }
                    }
                    else if (Convert.ToBoolean(chkAllEmp.CheckedValue) == false && Convert.ToBoolean(chkArea.CheckedValue) == false)
                    {
                        Reports.RptRecoveryEmpWise rptAllEmpWise = new Reports.RptRecoveryEmpWise();
                        BP_RptRecovery NewEmpObj = new BP_RptRecovery();
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

                        dt = NewEmpObj.RptEmpWiseReport(int.Parse(EmpID.ToString()), FromDate, ToDateQry, int.Parse(AreaID.ToString()));
                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            dt.Dispose();
                        }
                        else
                        {
                            rptAllEmpWise.SetDataSource(dt);
                            TextObject txtFrom = (TextObject)rptAllEmpWise.ReportDefinition.ReportObjects["txtFromDate"];
                            txtFrom.Text = FromDate;
                            TextObject txtTo = (TextObject)rptAllEmpWise.ReportDefinition.ReportObjects["txtToDate"];
                            txtTo.Text = ToDateQry;
                            CommonDAL.ShowReport(rptAllEmpWise, "All Emp Due Report");
                            dt.Dispose();
                            EmpID = 0;
                            AreaID = 0;
                        }
                    }
                }
                if (lstvReports.SelectedItems[0].Key.Equals("RptClientDisbursment"))
                {
                    //Reports.RptClientDisbursment rptDisbursment = new EasyRashanManagementSystem.Reports.RptClientDisbursment();
                    Reports.Copy_of_RptNew rptDisbursment = new EasyRashanManagementSystem.Reports.Copy_of_RptNew();
                    BP_RptRecovery NewDisRpt = new BP_RptRecovery();
                    string AsOnDay = dtpAsOn.DateTime.Day.ToString();
                    string AsOnMonth = dtpAsOn.DateTime.Month.ToString();
                    string AsOnYear = dtpAsOn.DateTime.Year.ToString();
                    if (Convert.ToInt32(AsOnDay) <= 9)
                    {
                        AsOnDay = "0" + AsOnDay;
                    }
                    if (Convert.ToInt32(AsOnMonth) <= 9)
                    {
                        AsOnMonth = "0" + AsOnMonth;
                    }
                    if (ClientID == 0)
                    {
                        MessageBox.Show("Please Select Some Employee First", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        btnEmp.Focus();
                        return;
                    }
                    string DisDate = AsOnYear + "-" + AsOnMonth + "-" + AsOnDay;
                    dt = NewDisRpt.GetMemberDisbursmentReport(ClientID, DisDate);
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        dt.Dispose();
                        ClientID = 0;
                    }
                    else
                    {
                        rptDisbursment.SetDataSource(dt);
                        CommonDAL.ShowReport(rptDisbursment, "Member Disbursment Report");
                        dt.Dispose();
                        ClientID = 0;
                    }
                }
                else if (lstvReports.SelectedItems[0].Key.Equals("RptClientRecovery"))
                {
                    Reports.RptClientRecovery newRecovery = new EasyRashanManagementSystem.Reports.RptClientRecovery();
                    BP_RptRecovery NewrptBEObj = new BP_RptRecovery();

                    if (Convert.ToBoolean(chkAllEmp.CheckedValue) == true && Convert.ToBoolean(chkArea.CheckedValue) == true)
                    {
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
                        dt = NewrptBEObj.RptRecoveryDateWise(FromDate, ToDateQry);
                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            dt.Dispose();
                        }
                        else
                        {
                            newRecovery.SetDataSource(dt);
                            TextObject txtFromDate = (TextObject)newRecovery.ReportDefinition.ReportObjects["txtFromDate"];
                            txtFromDate.Text = FromDate;
                            TextObject txtToDate = (TextObject)newRecovery.ReportDefinition.ReportObjects["txtToDate"];
                            txtToDate.Text = ToDateQry;
                            CommonDAL.ShowReport(newRecovery, "Date Wise Recovery Report");
                            dt.Dispose();
                        }
                    }
                    else if (Convert.ToBoolean(chkAllEmp.CheckedValue) == false && Convert.ToBoolean(chkArea.CheckedValue) == true)
                    {
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
                        dt = NewrptBEObj.RptRecoveryDateWiseAndClient(FromDate, ToDateQry, EmpID);
                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            dt.Dispose();
                            ClientID = 0;
                        }
                        else
                        {
                            newRecovery.SetDataSource(dt);
                            TextObject txtFromDate = (TextObject)newRecovery.ReportDefinition.ReportObjects["txtFromDate"];
                            txtFromDate.Text = FromDate;
                            TextObject txtToDate = (TextObject)newRecovery.ReportDefinition.ReportObjects["txtToDate"];
                            txtToDate.Text = ToDateQry;
                            CommonDAL.ShowReport(newRecovery, "Date Wise & Client Wise Recovery Report");

                            dt.Dispose();
                            ClientID = 0;
                        }
                    }
                    else if (Convert.ToBoolean(chkAllEmp.CheckedValue) == false && Convert.ToBoolean(chkArea.CheckedValue) == false)
                    {
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
                        dt = NewrptBEObj.RptRecoveryDateWiseAndClientWiseArea(FromDate, ToDateQry, EmpID, int.Parse(AreaID.ToString()));
                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            dt.Dispose();
                            ClientID = 0;
                        }
                        else
                        {
                            newRecovery.SetDataSource(dt);
                            TextObject txtFromDate = (TextObject)newRecovery.ReportDefinition.ReportObjects["txtFromDate"];
                            txtFromDate.Text = FromDate;
                            TextObject txtToDate = (TextObject)newRecovery.ReportDefinition.ReportObjects["txtToDate"];
                            txtToDate.Text = ToDateQry;
                            CommonDAL.ShowReport(newRecovery, "Date Wise,Client Wise and AreaWise Recovery Report");
                            dt.Dispose();
                            ClientID = 0;
                        }
                    }
                    else if (Convert.ToBoolean(chkAllEmp.CheckedValue) == true && Convert.ToBoolean(chkArea.CheckedValue) == false)
                    {
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
                        dt = NewrptBEObj.RptRecoveryDateWiseOnlnyArea(FromDate, ToDateQry, int.Parse(AreaID.ToString()));
                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            dt.Dispose();
                            ClientID = 0;
                        }
                        else
                        {
                            newRecovery.SetDataSource(dt);
                            CommonDAL.ShowReport(newRecovery, "Date Wise,Client Wise and AreaWise Recovery Report");
                            dt.Dispose();
                            ClientID = 0;
                        }
                    }
                }
                if (lstvReports.SelectedItems[0].Key.Equals("Rpt_CustomerDirectSales"))
                {
                    Reports.Rpt_CustomerDirectSales RptDirectSales = new EasyRashanManagementSystem.Reports.Rpt_CustomerDirectSales();
                    dt = new System.Data.DataTable();
                    dt = NewBP.RptDirectSalesCustomer(DRSMID);
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        dt.Dispose();
                        DRSMID = 0;
                    }
                    else
                    {
                        RptDirectSales.SetDataSource(dt);
                        CommonDAL.ShowReport(RptDirectSales, "Direct Sales Report Customer Wise");
                        dt.Dispose();
                        DRSMID = 0;
                    }
                }
                if (lstvReports.SelectedItems[0].Key.Equals("Rpt_DirectSalesAllCustomers"))
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
                        MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
                        CommonDAL.ShowReport(NewDirectSalesAll, "Direct Sales Of All Customers");
                        dt.Dispose();
                        FromDate = "";
                        ToDateQry = "";
                    }

                }
                if (lstvReports.SelectedItems[0].Key.Equals("RptAreaWiseDueSummary"))
                {
                    string SelectedValue = "";
                    SelectedValue = optDueSummary.Text;
                    if (SelectedValue == "AreaWise")
                    {
                        Reports.RptAreaWiseDueSummary rptArea = new EasyRashanManagementSystem.Reports.RptAreaWiseDueSummary();
                        string day = dtpAsOn.DateTime.Day.ToString();
                        string Month = dtpAsOn.DateTime.Month.ToString();
                        string Year = dtpAsOn.DateTime.Year.ToString();
                        if (Convert.ToInt32(day) <= 9)
                        {
                            day = "0" + day;
                        }
                        if (Convert.ToInt32(Month) <= 9)
                        {
                            Month = "0" + Month;
                        }
                        string Date = Year + "-" + Month + "-" + day;
                        System.Data.DataTable dtRptDue = new System.Data.DataTable();
                        dtRptDue = NewBP.RptAreaWiseAll(0, "2012-03-01", Date);
                        if (dtRptDue.Rows.Count == 0)
                        {
                            MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            dtRptDue.Dispose();
                            Date = "";
                        }
                        else
                        {
                            rptArea.SetDataSource(dtRptDue);
                            TextObject txtDate = (TextObject)rptArea.ReportDefinition.ReportObjects["txtDate"];
                            txtDate.Text = Date;
                            CommonDAL.ShowReport(rptArea, "Area Wise Due Summary");
                            dtRptDue.Dispose();
                        }
                    }
                    else
                    {
                        Reports.RptEmpWiseDueSummary rptEmpSummary = new EasyRashanManagementSystem.Reports.RptEmpWiseDueSummary();
                        string day = dtpAsOn.DateTime.Day.ToString();
                        string Month = dtpAsOn.DateTime.Month.ToString();
                        string Year = dtpAsOn.DateTime.Year.ToString();
                        if (Convert.ToInt32(day) <= 9)
                        {
                            day = "0" + day;
                        }
                        if (Convert.ToInt32(Month) <= 9)
                        {
                            Month = "0" + Month;
                        }
                        string Date = Year + "-" + Month + "-" + day;
                        System.Data.DataTable dtEmpDue = new System.Data.DataTable();
                        dtEmpDue = NewBP.RptAreaWiseAll(0, "2012-03-01", Date);
                        if (dtEmpDue.Rows.Count == 0)
                        {
                            MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            dtEmpDue.Dispose();
                            Date = "";
                        }
                        else
                        {
                            rptEmpSummary.SetDataSource(dtEmpDue);
                            TextObject txtDate = (TextObject)rptEmpSummary.ReportDefinition.ReportObjects["txtDate"];
                            txtDate.Text = Date;
                            CommonDAL.ShowReport(rptEmpSummary, "Employee Wise Due Summary");
                            dtEmpDue.Dispose();
                        }
                    }
                }
                if (lstvReports.SelectedItems[0].Key.Equals("RptInActiveClients"))
                {
                    bool IsAllArea = false;
                    bool IsAllEmp = false;
                    Reports.RptInActiveClients rptInActive = new EasyRashanManagementSystem.Reports.RptInActiveClients();
                    DateTime SelectedDate = Convert.ToDateTime(dtpTo.Value);
                    DateTime FirstDayofMonth = new DateTime(SelectedDate.Year, SelectedDate.Month, 1);

                    string FromDay = FirstDayofMonth.Day.ToString();
                    string FromMonth = FirstDayofMonth.Month.ToString();
                    string FromYear = FirstDayofMonth.Year.ToString();


                    int SampleDay = int.Parse(FromDay.ToString());
                    int SampleMonth = int.Parse(FromMonth.ToString());
                    if (Convert.ToInt32(SampleDay) <= 9)
                    {
                        FromDay = "0" + SampleDay;
                    }
                    if (Convert.ToInt32(SampleMonth) <= 9)
                    {
                        FromMonth = "0" + SampleMonth;
                    }
                    string ActualFromDate = FromYear + "-" + FromMonth + "-" + FromDay;

                    string ToDay = SelectedDate.Day.ToString();
                    string ToMonth = SelectedDate.Month.ToString();
                    string ToYear = SelectedDate.Year.ToString();

                    int SampleToDate = int.Parse(ToDay.ToString());
                    if (Convert.ToInt32(SampleToDate) <= 9)
                    {
                        ToDay = "0" + SampleToDate;
                    }
                    int SampleToMonth = int.Parse(ToMonth.ToString());

                    if (Convert.ToInt32(SampleToMonth) <= 9)
                    {
                        ToMonth = "0" + SampleToMonth;
                    }

                    string ActualToYear = ToYear + "-" + ToMonth + "-" + ToDay;



                    if (Convert.ToBoolean(chkArea.CheckedValue) == true && Convert.ToBoolean(chkAllEmp.CheckedValue) == true)
                    {
                        IsAllArea = true;
                        IsAllEmp = true;
                        System.Data.DataTable dtInActiveAll = new System.Data.DataTable();

                        dtInActiveAll = NewBP.RptInActiveClients(IsAllEmp, IsAllArea, 0, 0, ActualFromDate, ActualToYear);
                        if (dtInActiveAll.Rows.Count == 0)
                        {
                            MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            dtInActiveAll.Dispose();
                            IsAllArea = false;
                            IsAllEmp = false;
                            EmpIDInActive = 0;
                            AreaIDInActive = 0;
                        }
                        else
                        {
                            rptInActive.SetDataSource(dtInActiveAll);
                            CommonDAL.ShowReport(rptInActive, "In Active Clients");
                            rptInActive.Dispose();
                            EmpIDInActive = 0;
                            AreaIDInActive = 0;
                        }
                    }
                    if (Convert.ToBoolean(chkArea.CheckedValue) == true && Convert.ToBoolean(chkAllEmp.CheckedValue) == false)
                    {
                        IsAllArea = true;
                        IsAllEmp = false;
                        if (txtEmpName.Text.Trim().Equals(""))
                        {
                            MessageBox.Show("Please Select Employee To Continue", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            btnEmp.Focus();
                            return;
                        }
                        else
                        {
                            System.Data.DataTable dtInActiveAll = new System.Data.DataTable();
                            dtInActiveAll = NewBP.RptInActiveClients(IsAllEmp, IsAllArea, EmpIDInActive, 0, ActualFromDate, ActualToYear);

                            if (dtInActiveAll.Rows.Count == 0)
                            {
                                MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                dtInActiveAll.Dispose();
                                IsAllArea = false;
                                IsAllEmp = false;
                                EmpIDInActive = 0;
                                AreaIDInActive = 0;
                                txtArea.Text = "";
                                txtEmpName.Text = "";
                            }
                            else
                            {
                                rptInActive.SetDataSource(dtInActiveAll);
                                CommonDAL.ShowReport(rptInActive, "In Active Clients");
                                rptInActive.Dispose();
                                EmpIDInActive = 0;
                                AreaIDInActive = 0;
                                txtArea.Text = "";
                                txtEmpName.Text = "";
                            }
                        }
                    }
                    if (Convert.ToBoolean(chkArea.CheckedValue) == false && Convert.ToBoolean(chkAllEmp.CheckedValue) == false)
                    {
                        IsAllArea = false;
                        IsAllEmp = false;
                        if (txtArea.Text.Trim().Equals(""))
                        {
                            MessageBox.Show("Please Select Area", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            btnArea.Focus();
                        }
                        else if (txtEmpName.Text.Trim().Equals(""))
                        {
                            MessageBox.Show("Please Select Employee", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            btnArea.Focus();
                        }
                        else
                        {
                            System.Data.DataTable dtInActiveAll = new System.Data.DataTable();
                            dtInActiveAll = NewBP.RptInActiveClients(IsAllEmp, IsAllArea, EmpIDInActive, AreaIDInActive, ActualFromDate, ActualToYear);
                            if (dtInActiveAll.Rows.Count == 0)
                            {
                                MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                dtInActiveAll.Dispose();
                                IsAllArea = false;
                                IsAllEmp = false;
                                EmpIDInActive = 0;
                                AreaIDInActive = 0;
                                txtArea.Text = "";
                                txtEmpName.Text = "";
                            }
                            else
                            {
                                rptInActive.SetDataSource(dtInActiveAll);
                                CommonDAL.ShowReport(rptInActive, "In Active Clients");
                                rptInActive.Dispose();
                                EmpIDInActive = 0;
                                AreaIDInActive = 0;
                                txtArea.Text = "";
                                txtEmpName.Text = "";
                            }
                        }
                    }
                    if (Convert.ToBoolean(chkArea.CheckedValue) == false && Convert.ToBoolean(chkAllEmp.CheckedValue) == true)
                    {
                        IsAllArea = false;
                        IsAllEmp = true;
                        if (txtArea.Text.Trim().Equals(""))
                        {
                            MessageBox.Show("Please select Area", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            btnArea.Focus();
                        }
                        else
                        {
                            System.Data.DataTable dtInActiveAll = new System.Data.DataTable();
                            dtInActiveAll = NewBP.RptInActiveClients(IsAllEmp, IsAllArea, 0, AreaIDInActive, ActualFromDate, ActualToYear);
                            if (dtInActiveAll.Rows.Count == 0)
                            {
                                MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                dtInActiveAll.Dispose();
                                IsAllArea = false;
                                IsAllEmp = false;
                                EmpIDInActive = 0;
                                AreaIDInActive = 0;
                                txtArea.Text = "";
                                txtEmpName.Text = "";
                            }
                            else
                            {
                                rptInActive.SetDataSource(dtInActiveAll);
                                CommonDAL.ShowReport(rptInActive, "In Active Clients");
                                rptInActive.Dispose();
                                EmpIDInActive = 0;
                                AreaIDInActive = 0;
                                txtArea.Text = "";
                                txtEmpName.Text = "";
                            }
                        }
                    }
                }
                if (lstvReports.SelectedItems[0].Key.Equals("Rpt_ClearanceDetails"))
                {
                    Reports.Rpt_ClearanceDetails rptClearance = new EasyRashanManagementSystem.Reports.Rpt_ClearanceDetails();
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
                    dt = NewBP.Rpt_ClearanceDetails(FromDate, ToDateQry);
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        dt.Dispose();
                        FromDate = "";
                        ToDateQry = "";
                    }
                    else
                    {
                        rptClearance.SetDataSource(dt);
                        TextObject txtFromDate = (TextObject)rptClearance.ReportDefinition.ReportObjects["txtFromDate"];
                        txtFromDate.Text = FromDate;
                        TextObject txtToDate = (TextObject)rptClearance.ReportDefinition.ReportObjects["txtToDate"];
                        txtToDate.Text = ToDateQry;
                        CommonDAL.ShowReport(rptClearance, "Daily Expense Voucher");
                        dt.Dispose();
                        FromDate = "";
                        ToDateQry = "";
                        rptClearance.Dispose();
                    }
                }
                if (lstvReports.SelectedItems[0].Key.Equals("RptDisbursmentProfit"))
                {
                    if (GlobalVaribles.UserStatus == "FMNG")
                    {
                        Reports.RptDisbursmentProfit rptProfit = new EasyRashanManagementSystem.Reports.RptDisbursmentProfit();
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
                        dt = NewBP.RptDisbursmentProfit(FromDate, ToDateQry);
                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            dt.Dispose();
                            FromDate = "";
                            ToDateQry = "";
                        }
                        else
                        {
                            rptProfit.SetDataSource(dt);
                            TextObject txtFromDate = (TextObject)rptProfit.ReportDefinition.ReportObjects["txtFromDate"];
                            txtFromDate.Text = FromDate;
                            TextObject txtToDate = (TextObject)rptProfit.ReportDefinition.ReportObjects["txtToDate"];
                            txtToDate.Text = ToDateQry;
                            CommonDAL.ShowReport(rptProfit, "Profit Percentage Summary Report");
                            dt.Dispose();
                            rptProfit.Dispose();
                            FromDate = "";
                            ToDateQry = "";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry: You are not authorized to view this Report. Contact to Administrator", GlobalVaribles.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                if (lstvReports.SelectedItems[0].Key.Equals("Rpt_Investment"))
                {
                    //Reports.Rpt_Investment rptInvestment = new Reports.Rpt_Investment();
                    //string Fromday = dtpFrom.DateTime.Day.ToString();
                    //string FromMonth = dtpFrom.DateTime.Month.ToString();
                    //string Year = dtpFrom.DateTime.Year.ToString();
                    //if (int.Parse(Fromday.ToString()) <= 9)
                    //{
                    //    Fromday = "0" + Fromday;
                    //}
                    //if (int.Parse(FromMonth.ToString()) <= 9)
                    //{
                    //    FromMonth = "0" + FromMonth;
                    //}
                    //string FromDate = Year + "-" + FromMonth + "-" + Fromday;

                    //string ToDate = dtpTo.DateTime.Day.ToString();
                    //string ToMonth = dtpTo.DateTime.Month.ToString();
                    //string ToYear = dtpTo.DateTime.Year.ToString();

                    //if (int.Parse(ToDate.ToString()) <= 9)
                    //{
                    //    ToDate = "0" + ToDate;
                    //}
                    //if (int.Parse(ToMonth.ToString()) <= 9)
                    //{
                    //    ToMonth = "0" + ToMonth;
                    //}
                    //string ToDateQry = ToYear + "-" + ToMonth + "-" + ToDate;
                    //dt = new System.Data.DataTable();
                    //dt = NewBP.RptInvestment(FromDate, ToDateQry);
                    //if (dt.Rows.Count == 0)
                    //{
                    //    MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //    dt.Dispose();
                    //    FromDate = "";
                    //    ToDateQry = "";
                    //}
                    //else
                    //{
                    //    rptInvestment.SetDataSource(dt);

                    //    CommonDAL.ShowReport(rptInvestment, "Investment Report");
                    //    dt.Dispose();
                    //    rptInvestment.Dispose();
                    //    FromDate = "";
                    //    ToDateQry = "";
                    //}
                }

                //RptClientDiscount
                if (lstvReports.SelectedItems[0].Key.Equals("Rpt_Cash"))
                {
                    Reports.Rpt_Cash rptCash = new Reports.Rpt_Cash();
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
                    dt = NewBP.Rpt_Cash(FromDate);

                    //if (FromDate == "2014-02-01")
                    //if (FromDate == "2016-08-12")
                    if (FromDate == "2016-10-31")
                    {
                        rptCash.SetDataSource(dt);
                        TextObject txtFromDate = (TextObject)rptCash.ReportDefinition.ReportObjects["txtFromDate"];
                        txtFromDate.Text = FromDate;
                        TextObject txtOpBalance = (TextObject)rptCash.ReportDefinition.ReportObjects["txtOpeningBalance"];
                        txtOpBalance.Text = "3041522";
                        TextObject txtClosingBalance = (TextObject)rptCash.ReportDefinition.ReportObjects["txtClosingBalance"];
                        //txtClosingBalance.Text = "32994";
                        //txtClosingBalance.Text = "20014082";
                        txtClosingBalance.Text = "3056383";
                        CommonDAL.ShowReport(rptCash, "Cash Balance Report");
                        dt.Dispose();
                        rptCash.Dispose();
                        FromDate = "";
                        ToDateQry = "";
                    }
                    else
                    {
                        //commented on 2017-02-05
                        //double PreviousOpening = Math.Round(NewBP.GetPreviousClosingBalance(FromDate), 0);
                        //double CurrClosingBalance = Math.Round(NewBP.GetCurrentClosingBalance(FromDate, PreviousOpening), 0);
                        //end of Commented lines.
                        if (FromDate == "2017-02-01")
                        {
                            double PreviousOpening = 3170;
                            double CurrClosingBalance = Math.Round(NewBP.GetCurrentClosingBalance(FromDate, PreviousOpening), 0);
                            rptCash.SetDataSource(dt);
                            TextObject txtFromDate = (TextObject)rptCash.ReportDefinition.ReportObjects["txtFromDate"];
                            txtFromDate.Text = FromDate;
                            TextObject txtOpBalance = (TextObject)rptCash.ReportDefinition.ReportObjects["txtOpeningBalance"];
                            txtOpBalance.Text = PreviousOpening.ToString();
                            TextObject txtClosingBalance = (TextObject)rptCash.ReportDefinition.ReportObjects["txtClosingBalance"];
                            txtClosingBalance.Text = CurrClosingBalance.ToString();
                            CommonDAL.ShowReport(rptCash, "Cash Balance Report");
                            dt.Dispose();
                            rptCash.Dispose();
                            FromDate = "";
                            ToDateQry = "";
                        }
                        else
                        {
                            System.Data.DataTable dtNew = NewBP.GetCashbalanceForCashReport(FromDate);
                            int TotalRowsInDataTable = dtNew.Rows.Count;
                            double PreOpening = Convert.ToDouble(dtNew.Rows[TotalRowsInDataTable-1]["OpeningBalance"]);

                            double CurrClosing = Convert.ToDouble(dtNew.Rows[TotalRowsInDataTable - 1]["Closing"]);


                            //double PreCurrClosingBalance = Math.Round(NewBP.GetCurrentClosingBalance("2017-02-01", 3170), 0);
                            //double PreviousOpening = PreCurrClosingBalance;
                            //double CurrClosingBalance = Math.Round(NewBP.GetCurrentClosingBalance(FromDate, PreCurrClosingBalance), 0);
                            rptCash.SetDataSource(dt);
                            TextObject txtFromDate = (TextObject)rptCash.ReportDefinition.ReportObjects["txtFromDate"];
                            txtFromDate.Text = FromDate;
                            TextObject txtOpBalance = (TextObject)rptCash.ReportDefinition.ReportObjects["txtOpeningBalance"];
                            txtOpBalance.Text = PreOpening.ToString();
                            TextObject txtClosingBalance = (TextObject)rptCash.ReportDefinition.ReportObjects["txtClosingBalance"];
                            txtClosingBalance.Text = CurrClosing.ToString();
                            CommonDAL.ShowReport(rptCash, "Cash Balance Report");
                            dt.Dispose();
                            rptCash.Dispose();
                            FromDate = "";
                            ToDateQry = "";
                        }
                    }
                }
                if (lstvReports.SelectedItems[0].Key.Equals("RptClientDiscount"))
                {
                    Reports.RptClientDiscount rptDiscount = new Reports.RptClientDiscount();
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
                    dt = NewBP.RptDiscountClientWise(FromDate,ToDateQry);
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        dt.Dispose();
                    }
                    else
                    {
                        rptDiscount.SetDataSource(dt);
                        CommonDAL.ShowReport(rptDiscount, "Client Wise Discount Report");
                    }
                }
                if (lstvReports.SelectedItems[0].Key.Equals("Rpt_StockTransfer"))
                {
                    Reports.Rpt_StockTransfer rptStockTransfer = new Reports.Rpt_StockTransfer();
                    //Reports.Rpt_Cash rptCash = new Reports.Rpt_Cash();
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
                    dt = NewBP.Rpt_StockTransfer(FromDate, ToDateQry);
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        dt.Dispose();
                        FromDate = "";
                        ToDateQry = "";
                    }
                    else
                    {
                        rptStockTransfer.SetDataSource(dt);
                        TextObject txtFromDate = (TextObject)rptStockTransfer.ReportDefinition.ReportObjects["txtFromDate"];
                        txtFromDate.Text = FromDate;
                        TextObject txtToDate = (TextObject)rptStockTransfer.ReportDefinition.ReportObjects["txtToDate"];
                        txtToDate.Text = ToDateQry;
                        CommonDAL.ShowReport(rptStockTransfer, "Stock Transfer Report");
                        dt.Dispose();
                        rptStockTransfer.Dispose();
                        FromDate = "";
                        ToDateQry = "";
                    }
                }
                if (lstvReports.SelectedItems[0].Key.Equals("RptSalary"))
                {
                    Reports.RptSalary NewRptObj = new Reports.RptSalary();
                    dt.Clear();
                    if (Convert.ToBoolean(chkAllEmp.CheckedValue) == true)
                    {
                        dt = NewBP.RptSalary(cboMonthlyYear.Value.ToString(), txtYear.Text.Trim(), true, 0);
                    }
                    else
                    {
                        dt = NewBP.RptSalary(cboMonthlyYear.Value.ToString(), txtYear.Text.Trim(), false, int.Parse(EmpID.ToString()));
                    }
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        dt.Dispose();
                    }
                    else
                    {
                        NewRptObj.SetDataSource(dt);
                        CommonDAL.ShowReport(NewRptObj, "Salary Slip");
                        dt.Clear();
                        NewRptObj.Dispose();
                    }
                }
                if (lstvReports.SelectedItems[0].Key.Equals("Rpt_NewActivity"))
                {
                    Reports.Rpt_NewActivity NewRptObj = new Reports.Rpt_NewActivity();
                    dt.Clear();
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

                    dt = NewBP.Rpt_NewActivity(FromDate);
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        dt.Dispose();
                    }
                    else
                    {
                        NewRptObj.SetDataSource(dt);
                        TextObject txtFromDate = (TextObject)NewRptObj.ReportDefinition.ReportObjects["FromDate"];
                        txtFromDate.Text = FromDate;
                        CommonDAL.ShowReport(NewRptObj, "New Daily Activity Report");
                        dt.Clear();
                        NewRptObj.Dispose();
                    }
                }
                if (lstvReports.SelectedItems[0].Key.Equals("RptEmpWiseClientDisbursement"))
                {
                    Reports.RptEmpWiseClientDisbursement NewRptObj = new Reports.RptEmpWiseClientDisbursement();
                    dt.Clear();
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

                    bool IsChecked = Convert.ToBoolean(chkAllEmp.CheckedValue);

                    dt = NewBP.RptEmpWiseClientDisbursement(IsChecked, EmpID, FromDate, ToDateQry);
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        dt.Dispose();
                    }
                    else
                    {
                        NewRptObj.SetDataSource(dt);
                        TextObject txtFromDate = (TextObject)NewRptObj.ReportDefinition.ReportObjects["txtFromDate"];
                        txtFromDate.Text = FromDate;
                        TextObject txtToDate = (TextObject)NewRptObj.ReportDefinition.ReportObjects["txtToDate"];
                        txtToDate.Text = ToDateQry;
                        CommonDAL.ShowReport(NewRptObj, "Employee Wise Disbursement Report");
                        dt.Clear();
                        NewRptObj.Dispose();
                    }
                }

                if (lstvReports.SelectedItems[0].Key.Equals("RptDisSummary"))
                {
                    Reports.RptEmpWiseClientDisbursement NewRptObj = new Reports.RptEmpWiseClientDisbursement();
                    dt.Clear();
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

                    bool IsChecked = Convert.ToBoolean(chkAllEmp.CheckedValue);

                    dt = NewBP.RptEmpWiseDisbursementSummary(FromDate, ToDateQry);
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        dt.Dispose();
                    }
                    else
                    {
                        NewRptObj.SetDataSource(dt);
                        TextObject txtFromDate = (TextObject)NewRptObj.ReportDefinition.ReportObjects["txtFromDate"];
                        txtFromDate.Text = FromDate;
                        TextObject txtToDate = (TextObject)NewRptObj.ReportDefinition.ReportObjects["txtToDate"];
                        txtToDate.Text = ToDateQry;
                        CommonDAL.ShowReport(NewRptObj, "Employee Wise Disbursement Report");
                        dt.Clear();
                        NewRptObj.Dispose();
                    }
                }
                //RptNewReceipt
                if (lstvReports.SelectedItems[0].Key.Equals("RptSalaryDisbusement"))
                {
                    Reports.RptSalaryDisbusementLetter NewRptObj = new Reports.RptSalaryDisbusementLetter();
                    dt.Clear();
                    dt = NewBP.SalaryTransferLetterToBank(cboMonthlyYear.Value.ToString(), txtYear.Text);
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        dt.Dispose();
                    }
                    else
                    {
                        NewRptObj.SetDataSource(dt);
                        //TextObject txtFromDate = (TextObject)NewRptObj.ReportDefinition.ReportObjects["txtFromDate"];
                        //txtFromDate.Text = FromDate;
                        
                        CommonDAL.ShowReport(NewRptObj, "Salary Transferred Letter to Bank");
                        dt.Clear();
                        NewRptObj.Dispose();
                    }
                }
                if (lstvReports.SelectedItems[0].Key.Equals("RptNewReceipt"))
                {
                    Reports.RptNewReceipt NewRptObj = new Reports.RptNewReceipt();
                    dt.Clear();
                    string CrDay = dtpAsOn.DateTime.Day.ToString();
                    string CrMonth = dtpAsOn.DateTime.Month.ToString();
                    string CrYear = dtpAsOn.DateTime.Year.ToString();
                    string CurrentDate = "";
                    if (Convert.ToInt32(CrDay) <= 9)
                    {
                        CrDay = "0" + CrDay;
                    }
                    if (Convert.ToInt32(CrMonth) <= 9)
                    {
                        CrMonth = "0" + CrMonth;
                    }
                    CurrentDate = CrYear + "-" + CrMonth + "-" + CrDay;
                    //dt = NewBP.CashReceiptReport(CurrentDate, Convert.ToInt32(ClientID));
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        dt.Dispose();
                    }
                    else
                    {
                        NewRptObj.SetDataSource(dt);
                        //TextObject txtFromDate = (TextObject)NewRptObj.ReportDefinition.ReportObjects["txtFromDate"];
                        //txtFromDate.Text = FromDate;

                        CommonDAL.ShowReport(NewRptObj, "Cash Receipt Report");
                        dt.Clear();
                        NewRptObj.Dispose();
                    }
                }
                if (lstvReports.SelectedItems[0].Key.Equals("RptInactiveClientComparison"))
                {
                    //RptTest NewRptObj = new RptTest();
                    //RptInActiveClientsComparison NewRptObj = new RptInActiveClientsComparison();
                    //DateTime dtnow = DateTime.Now.Date;
                    //DateTime dtmonth = dtnow.AddMonths(-3);
                    //int MonthID = dtmonth.Month;
                    //int YearID = dtmonth.Year;

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

                    dt = NewBP.GetInActiveClients(FromDate, ToDateQry);

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No Record Found", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        dt.Dispose();
                    }
                    else
                    {
                        Reports.RptInActiveClients NewRptObj = new Reports.RptInActiveClients();
                        NewRptObj.SetDataSource(dt);
                        CommonDAL.ShowReport(NewRptObj, "InActive Clients Comparison");
                        //ExcelCreation(dt);
                        dt.Clear();
                        NewRptObj.Dispose();
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
            if (lstvReports.SelectedItems[0].Key.Equals("RptDailyActivity"))
            {
                btnVendor.Enabled = false;
                txtVendorName.Enabled = false;
                dtpAsOn.Enabled = true;
                chkAllDisb.Enabled = false;
                chkAson.Enabled = false;
                chkAson.CheckedValue = true;
                dtpFrom.Enabled = false;
                dtpTo.Enabled = false;
                btnArea.Enabled = false;
                txtArea.Enabled = false;
                btnEmp.Enabled = false;
                txtEmpName.Enabled = false;
                chkArea.Enabled = false;
                chkAllEmp.Enabled = false;
                btnMemberCode.Enabled = false;
                btnDirectSales.Enabled = false;
                txtDirectSales.Enabled = false;
                optDueSummary.Enabled = false;
                lblSalaryMonth.Visible = false;
                lblSalaryYear.Visible = false;
                txtYear.Visible = false;
                cboMonthlyYear.Visible = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("RptEmpWiseClientDisbursement"))
            {
                btnVendor.Enabled = false;
                txtVendorName.Enabled = false;
                dtpAsOn.Enabled = false;
                chkAllDisb.Enabled = false;
                chkAson.Enabled = false;
                chkAson.CheckedValue = false;
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                btnArea.Enabled = false;
                txtArea.Enabled = false;
                btnEmp.Enabled = true;
                txtEmpName.Enabled = false;
                chkArea.Enabled = false;
                chkAllEmp.Enabled = true;
                btnMemberCode.Enabled = false;
                btnDirectSales.Enabled = false;
                txtDirectSales.Enabled = false;
                optDueSummary.Enabled = false;
                lblSalaryMonth.Visible = false;
                lblSalaryYear.Visible = false;
                txtYear.Visible = false;
                cboMonthlyYear.Visible = false;
            }

            if (lstvReports.SelectedItems[0].Key.Equals("RptClientDiscount"))
            {
                btnVendor.Enabled = false;
                txtVendorName.Enabled = false;
                dtpAsOn.Enabled = false;
                chkAllDisb.Enabled = false;
                chkAson.Enabled = false;
                chkAson.CheckedValue = false;
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                btnArea.Enabled = false;
                txtArea.Enabled = false;
                btnEmp.Enabled = false;
                txtEmpName.Enabled = false;
                chkArea.Enabled = false;
                chkAllEmp.Enabled = false;
                btnMemberCode.Enabled = false;
                btnDirectSales.Enabled = false;
                txtDirectSales.Enabled = false;
                optDueSummary.Enabled = false;
                lblSalaryMonth.Visible = false;
                lblSalaryYear.Visible = false;
                txtYear.Visible = false;
                cboMonthlyYear.Visible = false;
            }


            if (lstvReports.SelectedItems[0].Key.Equals("RptStockBalance"))
            {
                txtVendorName.Enabled = false;
                btnVendor.Enabled = false;
                dtpAsOn.Enabled = false;
                chkAllDisb.Enabled = false;
                chkAson.Enabled = false;
                dtpFrom.Enabled = false;
                dtpTo.Enabled = false;
                btnArea.Enabled = false;
                txtArea.Enabled = false;
                btnEmp.Enabled = false;
                txtEmpName.Enabled = false;
                chkArea.Enabled = false;
                chkAllEmp.Enabled = false;
                btnMemberCode.Enabled = false;
                btnDirectSales.Enabled = false;
                txtDirectSales.Enabled = false;
                optDueSummary.Enabled = false;
                lblSalaryMonth.Visible = false;
                lblSalaryYear.Visible = false;
                txtYear.Visible = false;
                cboMonthlyYear.Visible = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("Rpt_Purchase"))
            {
                btnVendor.Enabled = false;
                txtVendorName.Enabled = false;
                dtpAsOn.Enabled = false;
                chkAllDisb.Enabled = false;
                chkAson.Enabled = false;
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
                btnArea.Enabled = false;
                txtArea.Enabled = false;
                btnEmp.Enabled = false;
                txtEmpName.Enabled = false;
                chkArea.Enabled = false;
                chkAllEmp.Enabled = false;
                btnMemberCode.Enabled = false;
                btnDirectSales.Enabled = false;
                txtDirectSales.Enabled = false;
                optDueSummary.Enabled = false;
                lblSalaryMonth.Visible = false;
                lblSalaryYear.Visible = false;
                txtYear.Visible = false;
                cboMonthlyYear.Visible = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("RptRecoveryAllClients"))
            {
                txtVendorName.Enabled = false;
                btnVendor.Enabled = false;
                btnArea.Enabled = false;
                txtArea.Enabled = false;
                txtEmpName.Enabled = false;
                chkAllDisb.Enabled = true;
                chkAson.Enabled = false;
                dtpTo.Enabled = false;
                dtpFrom.Enabled = false;
                dtpAsOn.Enabled = false;
                chkArea.Enabled = false;
                chkAllEmp.Enabled = false;
                btnMemberCode.Enabled = false;
                btnDirectSales.Enabled = false;
                txtDirectSales.Enabled = false;
                optDueSummary.Enabled = false;
                lblSalaryMonth.Visible = false;
                lblSalaryYear.Visible = false;
                txtYear.Visible = false;
                cboMonthlyYear.Visible = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("RptRecoveryAreaWise"))
            {
                txtVendorName.Enabled = false;
                btnVendor.Enabled = false;
                btnArea.Enabled = false;
                txtArea.Enabled = false;
                txtEmpName.Enabled = false;
                btnEmp.Enabled = false;
                chkAllDisb.Enabled = false;
                chkAson.Enabled = false;
                dtpTo.Enabled = true;
                dtpFrom.Enabled = true;
                dtpAsOn.Enabled = false;
                chkArea.Enabled = true;
                chkAllEmp.Enabled = false;
                btnMemberCode.Enabled = false;
                btnDirectSales.Enabled = false;
                txtDirectSales.Enabled = false;
                optDueSummary.Enabled = false;
                lblSalaryMonth.Visible = false;
                lblSalaryYear.Visible = false;
                txtYear.Visible = false;
                cboMonthlyYear.Visible = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("RptRecoveryEmpWise"))
            {
                txtVendorName.Enabled = false;
                btnVendor.Enabled = false;
                btnDirectSales.Enabled = false;
                txtDirectSales.Enabled = false;

                if (Convert.ToBoolean(chkAllEmp.CheckedValue) == true)
                {
                    btnEmp.Enabled = false;
                }
                else
                {
                    btnEmp.Enabled = true;
                }
                chkArea.CheckedValue = true;
                if (Convert.ToBoolean(chkArea.CheckedValue) == true)
                {
                    txtArea.Enabled = false;
                    btnArea.Enabled = false;
                }
                else
                {
                    txtArea.Enabled = false;
                    btnArea.Enabled = true;
                }
                //btnEmp.Enabled = true;
                txtEmpName.Enabled = false;
                chkAllDisb.Enabled = true;
                chkAson.Enabled = false;
                dtpTo.Enabled = true;
                dtpFrom.Enabled = true;
                dtpAsOn.Enabled = false;
                chkArea.Enabled = true;
                chkAllEmp.Enabled = true;
                btnMemberCode.Enabled = false;
                optDueSummary.Enabled = false;
                lblSalaryMonth.Visible = false;
                lblSalaryYear.Visible = false;
                txtYear.Visible = false;
                cboMonthlyYear.Visible = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("RptClientDisbursment"))
            {
                chkAllDisb.Enabled = false;
                txtVendorName.Enabled = false;
                btnVendor.Enabled = false;
                btnArea.Enabled = false;
                txtArea.Enabled = false;
                btnEmp.Enabled = false;
                txtEmpName.Enabled = false;
                chkAson.Enabled = false;
                chkAson.CheckedValue = true;
                dtpTo.Enabled = false;
                dtpFrom.Enabled = false;
                dtpAsOn.Enabled = true;
                chkArea.Enabled = false;
                chkAllEmp.Enabled = false;
                chkAllEmp.CheckedValue = true;
                chkArea.CheckedValue = true;
                btnMemberCode.Enabled = true;
                btnDirectSales.Enabled = false;
                txtDirectSales.Enabled = false;
                optDueSummary.Enabled = false;
                lblSalaryMonth.Visible = false;
                lblSalaryYear.Visible = false;
                txtYear.Visible = false;
                cboMonthlyYear.Visible = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("RptClientRecovery"))
            {
                chkAllDisb.Enabled = false;
                txtVendorName.Enabled = false;
                btnVendor.Enabled = false;
                txtArea.Enabled = false;
                txtEmpName.Enabled = false;
                chkAson.Enabled = false;
                chkAson.CheckedValue = false;
                btnMemberCode.Enabled = false;
                chkAson.Enabled = false;
                dtpTo.Enabled = true;
                dtpFrom.Enabled = true;
                chkAllEmp.Enabled = true;
                chkArea.Enabled = true;
                btnDirectSales.Enabled = false;
                txtDirectSales.Enabled = false;
                if (Convert.ToBoolean(chkAllEmp.CheckedValue) == true)
                {

                    btnEmp.Enabled = false;
                }
                else
                {
                    btnEmp.Enabled = true;
                }
                chkArea.CheckedValue = true;
                if (Convert.ToBoolean(chkArea.CheckedValue) == true)
                {
                    btnArea.Enabled = false;
                }
                else
                {
                    btnArea.Enabled = true;
                }
                optDueSummary.Enabled = false;
                lblSalaryMonth.Visible = false;
                lblSalaryYear.Visible = false;
                txtYear.Visible = false;
                cboMonthlyYear.Visible = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("Rpt_CustomerDirectSales"))
            {
                chkAllDisb.Enabled = false;
                txtVendorName.Enabled = false;
                btnVendor.Enabled = false;
                btnArea.Enabled = false;
                txtArea.Enabled = false;
                btnEmp.Enabled = false;
                txtEmpName.Enabled = false;
                chkAson.Enabled = false;
                chkAson.CheckedValue = false;
                dtpTo.Enabled = false;
                dtpFrom.Enabled = false;
                dtpAsOn.Enabled = false;
                chkArea.Enabled = false;
                chkAllEmp.Enabled = false;
                chkAllEmp.CheckedValue = false;
                chkArea.CheckedValue = false;
                btnMemberCode.Enabled = false;
                txtArea.Enabled = false;
                txtEmpName.Enabled = false;
                btnArea.Enabled = false;
                btnEmp.Enabled = false;
                btnDirectSales.Enabled = true;
                txtDirectSales.Enabled = true;
                optDueSummary.Enabled = false;
                lblSalaryMonth.Visible = false;
                lblSalaryYear.Visible = false;
                txtYear.Visible = false;
                cboMonthlyYear.Visible = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("Rpt_DirectSalesAllCustomers"))
            {
                chkAllDisb.Enabled = false;
                txtVendorName.Enabled = false;
                btnVendor.Enabled = false;
                btnArea.Enabled = false;
                txtArea.Enabled = false;
                btnEmp.Enabled = false;
                txtEmpName.Enabled = false;
                chkAson.Enabled = false;
                chkAson.CheckedValue = false;
                dtpTo.Enabled = true;
                dtpFrom.Enabled = true;
                dtpAsOn.Enabled = false;
                chkArea.Enabled = false;
                chkAllEmp.Enabled = false;
                chkAllEmp.CheckedValue = false;
                chkArea.CheckedValue = false;
                btnMemberCode.Enabled = false;
                txtArea.Enabled = false;
                txtEmpName.Enabled = false;
                btnArea.Enabled = false;
                btnEmp.Enabled = false;
                btnDirectSales.Enabled = false;
                txtDirectSales.Enabled = false;
                optDueSummary.Enabled = false;
                lblSalaryMonth.Visible = false;
                lblSalaryYear.Visible = false;
                txtYear.Visible = false;
                cboMonthlyYear.Visible = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("RptAreaWiseDueSummary"))
            {
                chkAllDisb.Enabled = false;
                txtVendorName.Enabled = false;
                btnVendor.Enabled = false;
                btnArea.Enabled = false;
                txtArea.Enabled = false;
                btnEmp.Enabled = false;
                txtEmpName.Enabled = false;
                chkAson.Enabled = false;
                chkAson.CheckedValue = false;
                dtpTo.Enabled = false;
                dtpFrom.Enabled = false;
                dtpAsOn.Enabled = true;
                chkArea.Enabled = false;
                chkAllEmp.Enabled = false;
                chkAllEmp.CheckedValue = false;
                chkArea.CheckedValue = false;
                btnMemberCode.Enabled = false;
                txtArea.Enabled = false;
                txtEmpName.Enabled = false;
                btnArea.Enabled = false;
                btnEmp.Enabled = false;
                btnDirectSales.Enabled = false;
                txtDirectSales.Enabled = false;
                optDueSummary.Enabled = true;
                lblSalaryMonth.Visible = false;
                lblSalaryYear.Visible = false;
                txtYear.Visible = false;
                cboMonthlyYear.Visible = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("RptInActiveClients"))
            {
                chkAllDisb.Enabled = false;
                txtVendorName.Enabled = false;
                btnVendor.Enabled = false;
                btnArea.Enabled = true;
                txtArea.Enabled = false;
                btnEmp.Enabled = true;
                txtEmpName.Enabled = false;
                chkAson.Enabled = false;
                chkAson.CheckedValue = false;
                dtpTo.Enabled = true;
                dtpFrom.Enabled = true;
                dtpAsOn.Enabled = false;
                chkArea.Enabled = true;
                chkAllEmp.Enabled = true;
                chkAllEmp.CheckedValue = false;
                chkArea.CheckedValue = false;
                btnMemberCode.Enabled = false;
                btnDirectSales.Enabled = false;
                txtDirectSales.Enabled = false;
                optDueSummary.Enabled = false;
                lblSalaryMonth.Visible = false;
                lblSalaryYear.Visible = false;
                txtYear.Visible = false;
                cboMonthlyYear.Visible = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("Rpt_ClearanceDetails"))
            {
                chkAllDisb.Enabled = false;
                txtVendorName.Enabled = false;
                btnVendor.Enabled = false;
                btnArea.Enabled = true;
                txtArea.Enabled = false;
                btnEmp.Enabled = true;
                txtEmpName.Enabled = false;
                chkAson.Enabled = false;
                chkAson.CheckedValue = false;
                dtpTo.Enabled = true;
                dtpFrom.Enabled = true;
                dtpAsOn.Enabled = false;
                chkArea.Enabled = false;
                chkAllEmp.Enabled = false;
                chkAllEmp.CheckedValue = false;
                chkArea.CheckedValue = false;
                btnMemberCode.Enabled = false;
                btnDirectSales.Enabled = false;
                txtDirectSales.Enabled = false;
                optDueSummary.Enabled = false;
                lblSalaryMonth.Visible = false;
                lblSalaryYear.Visible = false;
                txtYear.Visible = false;
                cboMonthlyYear.Visible = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("RptDisbursmentProfit"))
            {
                chkAllDisb.Enabled = false;
                txtVendorName.Enabled = false;
                btnVendor.Enabled = false;
                btnArea.Enabled = false;
                txtArea.Enabled = false;
                btnEmp.Enabled = false;
                txtEmpName.Enabled = false;
                chkAson.Enabled = false;
                chkAson.CheckedValue = false;
                dtpTo.Enabled = true;
                dtpFrom.Enabled = true;
                dtpAsOn.Enabled = false;
                chkArea.Enabled = false;
                chkAllEmp.Enabled = false;
                chkAllEmp.CheckedValue = false;
                chkArea.CheckedValue = false;
                btnMemberCode.Enabled = false;
                btnDirectSales.Enabled = false;
                txtDirectSales.Enabled = false;
                optDueSummary.Enabled = false;
                lblSalaryMonth.Visible = false;
                lblSalaryYear.Visible = false;
                txtYear.Visible = false;
                cboMonthlyYear.Visible = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("Rpt_Investment"))
            {
                chkAllDisb.Enabled = false;
                txtVendorName.Enabled = false;
                btnVendor.Enabled = false;
                btnArea.Enabled = false;
                txtArea.Enabled = false;
                btnEmp.Enabled = false;
                txtEmpName.Enabled = false;
                chkAson.Enabled = false;
                chkAson.CheckedValue = false;
                dtpTo.Enabled = true;
                dtpFrom.Enabled = true;
                dtpAsOn.Enabled = false;
                chkArea.Enabled = false;
                chkAllEmp.Enabled = false;
                chkAllEmp.CheckedValue = false;
                chkArea.CheckedValue = false;
                btnMemberCode.Enabled = false;
                btnDirectSales.Enabled = false;
                txtDirectSales.Enabled = false;
                optDueSummary.Enabled = false;
                btnArea.Enabled = false;
                btnEmp.Enabled = false;
                lblSalaryMonth.Visible = false;
                lblSalaryYear.Visible = false;
                txtYear.Visible = false;
                cboMonthlyYear.Visible = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("Rpt_Cash"))
            {
                chkAllDisb.Enabled = false;
                txtVendorName.Enabled = false;
                btnVendor.Enabled = false;
                btnArea.Enabled = false;
                txtArea.Enabled = false;
                btnEmp.Enabled = false;
                txtEmpName.Enabled = false;
                chkAson.Enabled = false;
                chkAson.CheckedValue = false;
                dtpTo.Enabled = false;
                dtpFrom.Enabled = true;
                dtpAsOn.Enabled = false;
                chkArea.Enabled = false;
                chkAllEmp.Enabled = false;
                chkAllEmp.CheckedValue = false;
                chkArea.CheckedValue = false;
                btnMemberCode.Enabled = false;
                btnDirectSales.Enabled = false;
                txtDirectSales.Enabled = false;
                optDueSummary.Enabled = false;
                btnArea.Enabled = false;
                btnEmp.Enabled = false;
                lblSalaryMonth.Visible = false;
                lblSalaryYear.Visible = false;
                txtYear.Visible = false;
                cboMonthlyYear.Visible = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("Rpt_StockTransfer"))
            {
                chkAllDisb.Enabled = false;
                txtVendorName.Enabled = false;
                btnVendor.Enabled = false;
                btnArea.Enabled = false;
                txtArea.Enabled = false;
                btnEmp.Enabled = false;
                txtEmpName.Enabled = false;
                chkAson.Enabled = false;
                chkAson.CheckedValue = false;
                dtpTo.Enabled = true;
                dtpFrom.Enabled = true;
                dtpAsOn.Enabled = false;
                chkArea.Enabled = false;
                chkAllEmp.Enabled = false;
                chkAllEmp.CheckedValue = false;
                chkArea.CheckedValue = false;
                btnMemberCode.Enabled = false;
                btnDirectSales.Enabled = false;
                txtDirectSales.Enabled = false;
                optDueSummary.Enabled = false;
                btnArea.Enabled = false;
                btnEmp.Enabled = false;
                lblSalaryMonth.Visible = false;
                lblSalaryYear.Visible = false;
                txtYear.Visible = false;
                cboMonthlyYear.Visible = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("RptSalary"))
            {
                chkAllDisb.Enabled = false;
                txtVendorName.Enabled = false;
                btnVendor.Enabled = false;
                btnArea.Enabled = false;
                txtArea.Enabled = false;
                btnEmp.Enabled = false;
                txtEmpName.Enabled = false;
                chkAson.Enabled = false;
                chkAson.CheckedValue = false;
                dtpTo.Enabled = true;
                dtpFrom.Enabled = true;
                dtpAsOn.Enabled = false;
                chkArea.Enabled = false;
                chkAllEmp.Enabled = true;
                chkAllEmp.CheckedValue = true;
                chkArea.CheckedValue = false;
                btnMemberCode.Enabled = false;
                btnDirectSales.Enabled = false;
                txtDirectSales.Enabled = false;
                optDueSummary.Enabled = false;
                btnArea.Enabled = false;
                btnEmp.Enabled = false;
                lblSalaryMonth.Visible = true;
                lblSalaryYear.Visible = true;
                txtYear.Visible = true;
                cboMonthlyYear.Visible = true;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("Rpt_NewActivity"))
            {
                chkAllDisb.Enabled = false;
                txtVendorName.Enabled = false;
                btnVendor.Enabled = false;
                btnArea.Enabled = false;
                txtArea.Enabled = false;
                btnEmp.Enabled = false;
                txtEmpName.Enabled = false;
                chkAson.Enabled = false;
                chkAson.CheckedValue = false;
                dtpTo.Enabled = false;
                dtpFrom.Enabled = true;
                dtpAsOn.Enabled = false;
                chkArea.Enabled = false;
                chkAllEmp.Enabled = false;
                chkAllEmp.CheckedValue = false;
                chkArea.CheckedValue = false;
                btnMemberCode.Enabled = false;
                btnDirectSales.Enabled = false;
                txtDirectSales.Enabled = false;
                optDueSummary.Enabled = false;
                btnArea.Enabled = false;
                btnEmp.Enabled = false;
                lblSalaryMonth.Visible = false;
                lblSalaryYear.Visible = false;
                txtYear.Visible = false;
                cboMonthlyYear.Visible = false;
            }
            if (lstvReports.SelectedItems[0].Key.Equals("RptSalaryDisbusement"))
            {
                chkAllDisb.Enabled = false;
                txtVendorName.Enabled = false;
                btnVendor.Enabled = false;
                btnArea.Enabled = false;
                txtArea.Enabled = false;
                btnEmp.Enabled = false;
                txtEmpName.Enabled = false;
                chkAson.Enabled = false;
                chkAson.CheckedValue = false;
                dtpTo.Enabled = false;
                dtpFrom.Enabled = true;
                dtpAsOn.Enabled = false;
                chkArea.Enabled = false;
                chkAllEmp.Enabled = false;
                chkAllEmp.CheckedValue = false;
                chkArea.CheckedValue = false;
                btnMemberCode.Enabled = false;
                btnDirectSales.Enabled = false;
                txtDirectSales.Enabled = false;
                optDueSummary.Enabled = false;
                btnArea.Enabled = false;
                btnEmp.Enabled = false;
                lblSalaryMonth.Visible = true;
                lblSalaryYear.Visible = true;
                txtYear.Visible = true;
                cboMonthlyYear.Visible = true;
            }
        }

        private void btnVendor_Click(object sender, EventArgs e)
        {
            object rValue;
            try
            {
                rValue = FrmGSearch.Show("Select PrID,Convert(varchar(12),PrDate,106) AS [PrDate],VendorName From Trans_PurchaseM Where IsActive=1", false, "All Purchase Items");
                if (rValue == null)
                    return;
                PrID = long.Parse(rValue.ToString());
                System.Data.DataTable dt = new System.Data.DataTable();
                DAL_RptRecovery NewBP = new DAL_RptRecovery();
                dt = NewBP.GetVendorName(int.Parse(PrID.ToString()));
                if (dt.Rows.Count > 0)
                {
                    txtVendorName.Text = dt.Rows[0]["VendorName"].ToString();
                }
                dt.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnArea_Click(object sender, EventArgs e)
        {
            object rValue;
            try
            {
                rValue = FrmGSearch.Show("Select AreaID,AreaDescription From Areas_Def", false, "All Areas");
                if (rValue == null)
                    return;
                AreaID = long.Parse(rValue.ToString());
                AreaIDInActive = long.Parse(rValue.ToString());
                System.Data.DataTable dt = new System.Data.DataTable();

                BP_RptRecovery NewBP = new BP_RptRecovery();
                dt = NewBP.GetAreaName(int.Parse(AreaID.ToString()));
                if (dt.Rows.Count > 0)
                {
                    txtArea.Text = dt.Rows[0]["AreaDescription"].ToString();
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
                rValue = FrmGSearch.Show("Select EmpCode As [EmpID],EmpName From Emp_Def", false, "All Employees");
                if (rValue == null)
                    return;
                EmpID = long.Parse(rValue.ToString());
                EmpIDInActive = long.Parse(rValue.ToString());
                System.Data.DataTable dt = new System.Data.DataTable();
                BP_RptRecovery NewBP = new BP_RptRecovery();
                dt = NewBP.GetEmpName(int.Parse(EmpID.ToString()));
                if (dt.Rows.Count > 0)
                {
                    txtEmpName.Text = dt.Rows[0]["EmpName"].ToString();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void RptRecovery_Load(object sender, EventArgs e)
        {
            dtpAsOn.Value = GlobalVaribles.DateFormat();
            dtpFrom.Value = GlobalVaribles.DateFormat();
            dtpTo.Value = GlobalVaribles.DateFormat();
            if (Convert.ToBoolean(chkAllDisb.CheckedValue) == true)
            {
                chkAson.Enabled = false;
                dtpTo.Enabled = false;
                dtpFrom.Enabled = false;
                dtpAsOn.Enabled = false;
                txtVendorName.Enabled = false;
                btnVendor.Enabled = false;
                btnArea.Enabled = false;
                txtArea.Enabled = false;
                txtEmpName.Enabled = false;
                btnEmp.Enabled = false;
                chkArea.Enabled = false;
                chkAllEmp.Enabled = false;
            }
            else
            {
                dtpTo.Enabled = false;
                dtpFrom.Enabled = false;
            }
            LoadSalaryYear();
        }

        private void chkAson_CheckedValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(chkAson.CheckedValue) == true)
            {
                dtpAsOn.Enabled = true;
                dtpFrom.Enabled = false;
                dtpTo.Enabled = false;
            }
            else
            {
                dtpAsOn.Enabled = false;
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
            }
        }

        private void chkAllDisb_CheckedValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(chkAllDisb.CheckedValue) == true)
            {
                chkAson.Enabled = false;
                dtpTo.Enabled = false;
                dtpFrom.Enabled = false;
                dtpAsOn.Enabled = false;
            }
            else
            {
                chkAson.Enabled = true;
                chkAson.CheckedValue = true;
                dtpAsOn.Enabled = true;
                dtpTo.Enabled = false;
                dtpFrom.Enabled = false;
            }
        }

        private void chkArea_CheckedValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(chkArea.CheckedValue) == true)
            {
                btnArea.Enabled = false;
                AreaID = 0;
            }
            else
            {
                btnArea.Enabled = true;
            }
        }

        private void chkAllEmp_CheckedValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(chkAllEmp.CheckedValue) == true)
            {
                btnEmp.Enabled = false;
            }
            else
            {
                btnEmp.Enabled = true;
            }
        }

        private void btnMemberCode_Click(object sender, EventArgs e)
        {
            object rValue;
            try
            {
                rValue = FrmGSearch.Show("select ClientID,ClientCode AS [MemberCode],ClientName As [MemberName] From Trans_Clients Order By ClientID Asc", false, "All Clients");
                if (rValue == null)
                    return;
                ClientID = long.Parse(rValue.ToString());
                System.Data.DataTable dt = new System.Data.DataTable();
                BP_RptRecovery NewBPObj = new BP_RptRecovery();
                dt = NewBPObj.GetClientName(ClientID);
                if (dt.Rows.Count > 0)
                {
                    txtMemberName.Text = dt.Rows[0]["ClientName"].ToString();
                }
                dt.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnDirectSales_Click(object sender, EventArgs e)
        {
            object rValue;
            try
            {
                rValue = FrmGSearch.Show("Select DRS_ID,DRS_Name,Convert(varchar(11),DRS_Date,121) As [DirectSalesDate] From TRANS_DRSM Where ISActive=1", false, "Direct Sales Record");
                if (rValue == null)
                    return;
                DRSMID = long.Parse(rValue.ToString());
                BP_RptRecovery NewRptRecoveryObj = new BP_RptRecovery();
                System.Data.DataTable dt = new System.Data.DataTable();
                dt = NewRptRecoveryObj.GetCustomerName(DRSMID);
                if (dt.Rows.Count > 0)
                {
                    txtDirectSales.Text = dt.Rows[0]["DRS_Name"].ToString();
                }
                dt.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void chkAllEmp_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllEmp.Checked)
            {
                btnEmp.Enabled = false;
            }
            else
            {
                btnEmp.Enabled = true;
            }
        }

        void ExcelCreation(System.Data.DataTable dtTable)
        {
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel.Workbook worKbooK;
            Microsoft.Office.Interop.Excel.Worksheet worKsheeT;
            Microsoft.Office.Interop.Excel.Range celLrangE;

            try
            {
                excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = false;
                excel.DisplayAlerts = false;
                worKbooK = excel.Workbooks.Add(Type.Missing);
                excel.SheetsInNewWorkbook = 1;
               // worKsheeT = (Microsoft.Office.Interop.Excel.Worksheet)worKbooK.ActiveSheet;
                //worKsheeT.Name = "InActive Clients Comparison";


             //   worKsheeT.Range[worKsheeT.Cells[1, 1], worKsheeT.Cells[1, 6]].Merge();
            //    worKsheeT.Cells[1, 1] = "InActive Clients Comparison";
                //worKsheeT.Cells.Font.Size = 10;
              //  worKsheeT.Cells.HorizontalAlignment = XlHAlign.xlHAlignRight;
                //worKsheeT.Cells[1, 5].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                //worKsheeT.get_Range("A1", "G10").NumberFormat = "@";
                int rowcount = 2;

                foreach (DataRow datarow in dtTable.Rows)
                {
                    rowcount += 1;
                    for (int i = 1; i <= dtTable.Columns.Count; i++)
                    {
                        if (rowcount == 3)
                        {
                         //   worKsheeT.Cells[2, i] = dtTable.Columns[i - 1].ColumnName;
                            //worKsheeT.Cells.Font.Color = System.Drawing.Color.Black;

                        }
                      //  worKsheeT.Cells[rowcount, i] = datarow[i - 1].ToString();
                        if (rowcount > 3)
                        {
                            if (i == dtTable.Columns.Count)
                            {
                                if (rowcount % 2 == 0)
                                {
                                  //  celLrangE = worKsheeT.Range[worKsheeT.Cells[rowcount, 1], worKsheeT.Cells[rowcount, dtTable.Columns.Count]];
                                }
                            }
                        }
                    }
                }
             //   celLrangE = worKsheeT.Range[worKsheeT.Cells[1, 1], worKsheeT.Cells[rowcount, dtTable.Columns.Count]];
                //celLrangE.EntireColumn.AutoFit();

                //worKbooK.Password = "1234";
              //  Microsoft.Office.Interop.Excel.Borders border = celLrangE.Borders;
                //border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                //border.Weight = 2d;
              //  celLrangE = worKsheeT.Range[worKsheeT.Cells[1, 1], worKsheeT.Cells[2, dtTable.Columns.Count]];
                //worKsheeT.Cells.Locked = false;
                //worKsheeT.get_Range("A2", "G1048576").Locked = true;
                //worKsheeT.get_Range("F2", "F5").Locked = false;
                //worKsheeT.get_Range("G2", "N5").Locked = true;
                //worKsheeT.Protect(Password: 1111);
                DateTime dtFileName = DateTime.Now.Date;
                string Name = DateTime.Now.ToString("yyyyMMddHHmmsss");
                worKbooK.SaveAs(@"E:\InActiveClients\" + Name + "", Password: 1111);
                worKbooK.Close();
                excel.Quit();
                MessageBox.Show("File Exported Successfully", "Easy Rashan Management System", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                worKsheeT = null;
                celLrangE = null;
                worKbooK = null;
            }
        }

        #endregion

    }
}
