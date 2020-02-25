using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using BusinessEntities;
using System.Data;

namespace BusinessProcessObjects
{
    public class BP_RptRecovery
    {

        public DataTable GetDailyActivity(string CurrentDate)
        {
            DAL_RptRecovery NewDAlObj = new DAL_RptRecovery();
            return NewDAlObj.GetDailyActivity(CurrentDate);
        }

        public DataTable GetClientIssuenceReport(string param)
        {
            DAL_RptRecovery NewDALObj = new DAL_RptRecovery();
            return NewDALObj.GetClientIssuenceReport(param);
        }

        public DataTable GetStockBalance()
        {
            DAL_RptRecovery NewDALObj = new DAL_RptRecovery();
            return NewDALObj.GetStockBalance();
        }

        public DataTable ShowPurchaseItems(string FromDate,string ToDate)
        {
            DAL_RptRecovery NewDALObj=new DAL_RptRecovery();
            return NewDALObj.ShowPurchaseItems(FromDate,ToDate);
        }

        public DataTable GetAreaName(int AreaID)
        {
            DAL_RptRecovery NewDALObj = new DAL_RptRecovery();
            return NewDALObj.GetAreaName(AreaID);
        }

        public DataTable GetEmpName(int EmpID)
        {
            DAL_RptRecovery NEWDALOBJ = new DAL_RptRecovery();
            return NEWDALOBJ.GetEmpName(EmpID);
        }

        public DataTable DisbursmentReport(string From, string To, bool IsAll, string OnlyDate)
        {
            DAL_RptRecovery NewDALObj = new DAL_RptRecovery();
            return NewDALObj.DisbursmentReport(From, To,IsAll,OnlyDate);
        }

        public DataTable AllClientDisbursmentReport()
        {
            DAL_RptRecovery NewDAlObj = new DAL_RptRecovery();
            return NewDAlObj.AllClientDisbursmentReport();
        }

        public DataTable RptAreaWiseAll(int AreaID,string FromDate,string ToDate)
        {
            DAL_RptRecovery NewDAlObj = new DAL_RptRecovery();
            return NewDAlObj.RptAreaWiseAll(AreaID,FromDate,ToDate);
        }

        public DataTable RptEmpWiseReport(int EmpID, string FromDate, string ToDate, int AreaID)
        {
            DAL_RptRecovery NewDalOBj = new DAL_RptRecovery();
            return NewDalOBj.RptEmpWiseReport(EmpID,FromDate,ToDate,AreaID);
        }

        public DataTable GetClientName(long ClientID)
        {
            DAL_RptRecovery NewDalOBj = new DAL_RptRecovery();
            return NewDalOBj.GetClientName(ClientID);
        }

        public DataTable GetMemberDisbursmentReport(long ClientID, string DisDate)
        {
            DAL_RptRecovery NewDalOBj = new DAL_RptRecovery();
            return NewDalOBj.GetMemberDisbursmentReport(ClientID,DisDate);
        }

        public DataTable GetMemberDisbursmentReportEnglish(long IssueID)
        {
            DAL_RptRecovery NewDalOBj = new DAL_RptRecovery();
            return NewDalOBj.GetMemberDisbursmentReportEnglish(IssueID);
        }

        public DataTable GetMemberDisbursmentReportUrdu(long IssueID)
        {
            DAL_RptRecovery NewDalOBj = new DAL_RptRecovery();
            return NewDalOBj.GetMemberDisbursmentReportUrdu(IssueID);
        }

        public DataTable RptRecoveryDateWise(string FromDate, string ToDate)
        {
            DAL_RptRecovery NewDalOBj = new DAL_RptRecovery();
            return NewDalOBj.RptRecoveryDateWise(FromDate, ToDate);
        }

        public DataTable RptRecoveryDateWiseAndClient(string FromDate, string ToDate, long EmpID)
        {
            DAL_RptRecovery NewDalOBj = new DAL_RptRecovery();
            return NewDalOBj.RptRecoveryDateWiseAndClient(FromDate, ToDate, EmpID);
        }

        public DataTable RptRecoveryDateWiseAndClientWiseArea(string FromDate, string ToDate, long EmpID, int AreaID)
        {
            DAL_RptRecovery NewDalOBj = new DAL_RptRecovery();
            return NewDalOBj.RptRecoveryDateWiseAndClientWiseArea(FromDate, ToDate, EmpID, AreaID);
        }

        public DataTable RptRecoveryDateWiseOnlnyArea(string FromDate, string ToDate, int AreaID)
        {
            DAL_RptRecovery NewDalOBj = new DAL_RptRecovery();
            return NewDalOBj.RptRecoveryDateWiseOnlnyArea(FromDate, ToDate, AreaID);
        }

        public DataTable GetCustomerName(long DRS_ID)
        {
            DAL_RptRecovery NewDALCustomerObj = new DAL_RptRecovery();
            return NewDALCustomerObj.GetCustomerName(DRS_ID);
        }

        public DataTable RptDirectSalesCustomer(long DRS_ID)
        {
            DAL_RptRecovery NewRptDirectSales = new DAL_RptRecovery();
            return NewRptDirectSales.RptDirectSalesCustomer(DRS_ID);
        }

        public DataTable RptDirectSalesAllCustomer(string FromDate, string ToDate)
        {
            DAL_RptRecovery NewDALDirectSalesAll = new DAL_RptRecovery();
            return NewDALDirectSalesAll.RptDirectSalesAllCustomer(FromDate, ToDate);
        }

        public DataTable RptInActiveClients(bool IsAllEmp, bool IsAllArea, long EmpID, long AreaID,string FromDate,string ToDate)
        {
            DAL_RptRecovery NewDALObj = new DAL_RptRecovery();
            return NewDALObj.RptInActiveClients(IsAllEmp, IsAllArea, EmpID, AreaID,FromDate,ToDate);
        }

        public DataTable Rpt_ClearanceDetails(string FromDate, string ToDate)
        {
            DAL_RptRecovery NewDALObj = new DAL_RptRecovery();
            return NewDALObj.Rpt_ClearanceDetails(FromDate, ToDate);
        }

        public DataTable RptDisbursmentProfit(string FromDate, string ToDate)
        {
            DAL_RptRecovery NewDALObj = new DAL_RptRecovery();
            return NewDALObj.RptDisbursmentProfit(FromDate, ToDate);
        }

        public DataTable RptInvestment(string FromDate, string ToDate)
        {
            DAL_RptRecovery NewDALObj = new DAL_RptRecovery();
            return NewDALObj.RptInvestment(FromDate, ToDate);
        }

        public DataTable Rpt_Cash(string FromDate)
        {
            DAL_RptRecovery NewDALObj = new DAL_RptRecovery();
            return NewDALObj.Rpt_Cash(FromDate);
        }

        public DataTable Rpt_StockTransfer(string FromDate, string ToDate)
        {
            DAL_RptRecovery NewDALObj = new DAL_RptRecovery();
            return NewDALObj.Rpt_StockTransfer(FromDate, ToDate);
        }

        public DataTable RptSalary(string MonthID, string SalaryYear, bool IsAllEmployees, int EmpID)
        {
            DAL_RptRecovery NewDALObj = new DAL_RptRecovery();
            return NewDALObj.RptSalary(MonthID, SalaryYear, IsAllEmployees, EmpID);
        }

        public DataTable Rpt_NewActivity(string FromDate)
        {
            DAL_RptRecovery NewDALObj = new DAL_RptRecovery();
            return NewDALObj.Rpt_NewActivity(FromDate);
        }

        
        public double GetCurrentClosingBalance(string FromDate,double PreOpeningBalance)
        {
            DAL_RptRecovery NewDALObj = new DAL_RptRecovery();
            return NewDALObj.GetCurrentClosingBalance(FromDate,PreOpeningBalance);
        }

        public double GetPreviousClosingBalance(string FromDate)
        {
            DAL_RptRecovery NewDALObj = new DAL_RptRecovery();
            return NewDALObj.GetPreviousClosingBalance(FromDate);
        }
        

        public DataTable RptDiscountClientWise(string FromDate, string ToDate)
        {
            DAL_RptRecovery NewDALObj = new DAL_RptRecovery();
            return NewDALObj.RptDiscountClientWise(FromDate,ToDate);
        }

        public DataTable RptEmpWiseClientDisbursement(bool IsAll,long EmpID,string FromDate,string ToDate)
        {
            DAL_RptRecovery NewDALObj = new DAL_RptRecovery();
            return NewDALObj.RptEmpWiseClientDisbursement(IsAll,EmpID,FromDate,ToDate);
        }

        public DataTable RptEmpWiseDisbursementSummary(string FromDate, string ToDate)
        {
            DAL_RptRecovery NewDALObj = new DAL_RptRecovery();
            return NewDALObj.RptEmpWiseDisbursementSummary(FromDate, ToDate);
        }


        public DataTable GetCashbalanceForCashReport(string FromDate)
        {
            DAL_RptRecovery NewDALObj = new DAL_RptRecovery();
            return NewDALObj.GetCashbalanceForCashReport(FromDate);
        }

        public DataTable SalaryTransferLetterToBank(string SalaryMonth, string SalaryYear)
        {
            DAL_RptRecovery NewDALObj = new DAL_RptRecovery();
            return NewDALObj.SalaryTransferLetterToBank(SalaryMonth,SalaryYear);
        }

        public DataTable CashReceiptReport(int RecoveryID)
        {
            DAL_RptRecovery NewDALObj = new DAL_RptRecovery();
            return NewDALObj.CashReceiptReport(RecoveryID);
        }

        public DataTable GetInActiveClients(string FromDate, string ToDate)
        {
            DAL_RptRecovery NewDALObj = new DAL_RptRecovery();
            return NewDALObj.GetInActiveClients(FromDate, ToDate);
        }

    }
}
