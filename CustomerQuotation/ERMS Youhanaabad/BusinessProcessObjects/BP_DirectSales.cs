using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using BusinessEntities;
using System.Data;

namespace BusinessProcessObjects
{
    public class BP_DirectSales
    {
        DAL_DirectSales NewDALObj = new DAL_DirectSales();

        public bool Insert_DirectSalesM(BE_DirectSales NewINSBE)
        {
            return NewDALObj.Insert_DirectSalesM(NewINSBE);
        }

        public bool Insert_DirectSalesD(BE_DirectSales NewINSBEDetail)
        {
            return NewDALObj.Insert_DirectSalesD(NewINSBEDetail);
        }

        public double GetStock(int ItemID)
        {
            return NewDALObj.GetStock(ItemID);
        }

        public DataTable GetItemDetails(int ItemID)
        {
            return NewDALObj.GetItemDetails(ItemID);
        }

        public DataTable GetDirectSales_Details(int DRS_ID)
        {
            return NewDALObj.GetDirectSales_Details(DRS_ID);
        }

        public bool Update_DirectSales(BE_DirectSales NewUpdateBE)
        {
            return NewDALObj.Update_DirectSales(NewUpdateBE);
        }
    }
}
