using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using BusinessEntities;
using System.Data;

namespace BusinessProcessObjects
{
    public class Bp_RptPurchase
    {
        public DataTable ShowPurchaseItems(int PrID)
        {
            DAL_RptPurchase NewDALObj = new DAL_RptPurchase();
            return NewDALObj.ShowPurchaseItems(PrID);
        }

        public DataTable GetVendorName(int PrID)
        {
            DAL_RptPurchase NewDALObj = new DAL_RptPurchase();
            return NewDALObj.GetVendorName(PrID);
        }
    }
}
