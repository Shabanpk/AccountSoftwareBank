using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DAL;
using System.Data.SqlClient;

namespace EasyRashanManagementSystem
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        #region Explorer bar Click Event

        private void ebpMain_ItemClick(object sender, Infragistics.Win.UltraWinExplorerBar.ItemEventArgs e)
        {
            //Shaban Comment
            //if (e.Item.Key.Equals("UsrCreation"))
            //{
            //    ClsFunctions.ShowForm(new FrmUserCreation(),this,true,true);
            //}

            //if (e.Item.Key.Equals("PassChange"))
            //{
            //    ClsFunctions.ShowForm(new FrmPasswordChange(),this,true,true );
            //}

            //if (e.Item.Key.Equals("UsrRights"))
            //{
            //    ClsFunctions.ShowForm(new FrmUserRights(),this,true,true );
            //}

            //if (e.Item.Key.Equals(""))
            //{
            //    ClsFunctions.ShowForm(new FrmInActiveUser(),this,true,true );
            //}

            //if (e.Item.Key.Equals("InActiveUser"))
            //{
            //    ClsFunctions.ShowForm(new FrmInActiveUser(),this,true,true );
            //}

            //if (e.Item.Key.Equals("MUnit"))
            //{
            //    ClsFunctions.ShowForm(new FrmMeasuringUnit(),this,true,true );
            //}

            //if (e.Item.Key.Equals("GroupDef"))
            //{
            //    ClsFunctions.ShowForm(new FrmGroupDef(),this,true,true );
            //}
            //if (e.Item.Key.Equals("ItemDef"))
            //{
            //    ClsFunctions.ShowForm(new FrmitemDefination(),this,true,true );
            //}

            //if (e.Item.Key.Equals("OpBalance"))
            //{
            //    ClsFunctions.ShowForm(new FrmOpeningBalance(),this,true,true );
            //}

            //if (e.Item.Key.Equals("BranchDef"))
            //{
            //    ClsFunctions.ShowForm(new FrmBranchDef(),this,true,true );
            //}

            //if (e.Item.Key.Equals("AreaDef"))
            //{
            //    ClsFunctions.ShowForm(new FrmAreaDef(),this,true,true );
            //}

            //if (e.Item.Key.Equals("ClientReq"))
            //{
            //    ClsFunctions.ShowForm(new FrmClient(),this,true,true );
            //}
            //if (e.Item.Key.Equals("PacketIssue"))
            //{
            //    ClsFunctions.ShowForm(new FrmItemCreation(),this,true,true );
            //}
            //if (e.Item.Key.Equals("FrmPur"))
            //{
            //    ClsFunctions.ShowForm(new FrmPurchase(),this,true,true);
            //}
            //if (e.Item.Key.Equals("FrmRecovry"))
            //{
            //    ClsFunctions.ShowForm(new FrmRecovery(),this,true,true);
            //}
            //if (e.Item.Key.Equals("EmpDef"))
            //{
            //    ClsFunctions.ShowForm(new FrmEmpDef(),this,true,true);
            //}
            //if (e.Item.Key.Equals("BankDep"))
            //{
            //    ClsFunctions.ShowForm(new FrmBankDeposit(),this,true,true);
            //}
            //if (e.Item.Key.Equals("AccHead"))
            //{
            //    ClsFunctions.ShowForm(new FrmAccountHeadDef (),this,true,true);
            //}
            //if (e.Item.Key.Equals("AccHead_Det"))
            //{
            //    ClsFunctions.ShowForm(new FrmHeadDetails(),this,true,true);
            //}
            //if (e.Item.Key.Equals("FrmClearance"))
            //{
            //    ClsFunctions.ShowForm(new FrmClearance (),this,true,true);
            //}
            //if (e.Item.Key.Equals("RptRecovery"))
            //{
            //    ClsFunctions.ShowForm(new RptRecovery(),this,true,true);
            //}
            ////if (e.Item.Key.Equals("BankWithDrawl"))
            ////{
            ////    ClsFunctions.ShowForm(new FrmBankWithDrawl(),this,true,true);
            ////}
            //if (e.Item.Key.Equals("FrmRptDueDate"))
            //{
            //    ClsFunctions.ShowForm(new FrmRptDueReport(),this,true,true);
            //}
            //if (e.Item.Key.Equals("DrSales"))
            //{
            //    ClsFunctions.ShowForm(new FrmDirectSales(),this,true,true);
            //}
            //if (e.Item.Key.Equals("RegClient"))
            //{
            //    ClsFunctions.ShowForm(new FrmRegistration(), this, true, true); 
            //}
            //if (e.Item.Key.Equals("AreaSwap"))
            //{
            //    ClsFunctions.ShowForm(new FrmAreaSwapping(), this, true, true);
            //}

            //if (e.Item.Key.Equals("Investors"))
            //{
            //    ClsFunctions.ShowForm(new FrmInvestors(),this,true,true);
            //}

            //if (e.Item.Key.Equals("InvestmentPosting"))
            //{
            //    ClsFunctions.ShowForm(new FrmInvestmentPosting(),this,true,true);
            //}

            //if (e.Item.Key.Equals("InvestmentReturn"))
            //{
            //    ClsFunctions.ShowForm(new FrmInvestmentReturn(),this,true,true);
            //}

            //if (e.Item.Key.Equals("OtherSales"))
            //{
            //    ClsFunctions.ShowForm(new FrmOtherResource(),this,true,true);
            //}

            //if (e.Item.Key.Equals("GodownDef"))
            //    ClsFunctions.ShowForm(new FrmGodownDef(),this,true,true);
            //if (e.Item.Key.Equals("Stocktransfer"))
            //    ClsFunctions.ShowForm(new FrmStockTransfer(),this,true,true);
            //if (e.Item.Key.Equals("DBBackup"))
            //    ClsFunctions.ShowForm(new FrmDBBackup(),this,true,true);
            //if (e.Item.Key.Equals("FrmSalary"))
            //    ClsFunctions.ShowForm(new FrmSalary(), this, true, true);
            //if (e.Item.Key.Equals("DepartmentForm"))
            //    ClsFunctions.ShowForm(new FrmDepartments(), this, true, true);
            //if (e.Item.Key.Equals("KnockOff"))
            //    ClsFunctions.ShowForm(new FrmKnockOff(),this,true,true);
        }

        #endregion

        private void FrmMain_Load(object sender, EventArgs e)
        {
            DAL_DbBackUp NewDALObj = new DAL_DbBackUp();
            DataTable dt = new DataTable();
            DateTime dtDueDate = DateTime.Now.Date;
            
            string Fromday = dtDueDate.Day.ToString();
            string FromMonth = dtDueDate.Month.ToString();
            string Year = dtDueDate.Year.ToString();
            if (int.Parse(Fromday.ToString()) <= 9)
            {
                Fromday = "0" + Fromday;
            }
            if (int.Parse(FromMonth.ToString()) <= 9)
            {
                FromMonth = "0" + FromMonth;
            }
            string FromDate = Year + "-" + FromMonth + "-" + Fromday;
            dt = NewDALObj.GetDueDate(FromDate);
            //dt = NewDALObj.ColumnCreation();
            if (dt.Rows.Count>0)
            {
                //Shaban Comment
                //FrmRemainingKnockOff NewFrm = new FrmRemainingKnockOff();
                //NewFrm.ShowDialog();
            }
            //NewDALObj.TableCreation();
            //Timer MyTimer = new Timer();
            //MyTimer.Interval = (1 * 60 * 1000); // 1 mins
            //MyTimer.Tick += new EventHandler(MyTimer_Tick);
            //MyTimer.Start();
            //GlobalVaribles gblvar = new GlobalVaribles();
            //Infragistics.Win.AppStyling.StyleManager.Load(Application.StartupPath + "\\styles\\" + gblvar.Style   );
        }

        private void MyTimer_Tick(object sender, EventArgs e)
        {
            //DAL_DbBackUp NewDALObj = new DAL_DbBackUp();
            //NewDALObj.DbBackUp();
        }
    }
}
