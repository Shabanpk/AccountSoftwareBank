using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using BusinessEntities;
using System.Data;


namespace BusinessProcessObjects
{
    public class BP_FrmRptDueDate
    {

        public DataTable GetReportData(string Options, string FromDate, string ToDate)
        {
            DAL_FrmRptDueDate NewDAL = new DAL_FrmRptDueDate();
            return NewDAL.GetReportData(Options,FromDate,ToDate);
        }

        public DataTable ShowReportData(string param)
        {
            DAL_FrmRptDueDate NewDAL = new DAL_FrmRptDueDate();
            return NewDAL.ShowReportData(param);
        }

    }
}
