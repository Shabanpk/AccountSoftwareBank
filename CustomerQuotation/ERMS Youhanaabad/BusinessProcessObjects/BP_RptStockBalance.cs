using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using BusinessEntities;
using System.Data;

namespace BusinessProcessObjects
{
    public class BP_RptStockBalance
    {
        public DataTable GetStockBalance()
        {
            DAL_RptStockBalance NewDALObj = new DAL_RptStockBalance();
            return NewDALObj.GetStockBalance();
        }
    }
}
