using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BusinessEntities;
using System.Data.SqlClient;
using DAL;

namespace BusinessProcessObjects
{
    public class BP_RptClients
    {
        public DataTable GetRptDatatable(BE_RptClients BE_RptClient, string Rpttype)
        {
            DAL_RptClients NewDALRpt = new DAL_RptClients();
            return NewDALRpt.GetRptDatatable(BE_RptClient,Rpttype);
        }
    }
}
