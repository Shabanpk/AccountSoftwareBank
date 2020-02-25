using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using BusinessEntities;
using System.Data;

namespace BusinessProcessObjects
{
    public class BP_RptClientIssuencecDateWise
    {
        public DataTable GetClientName(int ClientID)
        {
            DAL_RptClientIssuenceDateWise NewDALObj = new DAL_RptClientIssuenceDateWise();
            return NewDALObj.GetClientName(ClientID);
        }

        public DataTable GetReportData(int ClientID)
        {
            DAL_RptClientIssuenceDateWise NewDAL = new DAL_RptClientIssuenceDateWise();
            return NewDAL.GetReportData(ClientID);
        }

        public DataTable GetAreaName(int AreaID)
        {
            DAL_RptClientIssuenceDateWise NEWDALObj = new DAL_RptClientIssuenceDateWise();
            return NEWDALObj.GetAreaName(AreaID);
        }

        public DataTable GetEmpName(int EmpID)
        {
            DAL_RptClientIssuenceDateWise NEWDALObj = new DAL_RptClientIssuenceDateWise();
            return NEWDALObj.GetEmpName(EmpID);
        }
    }
}
