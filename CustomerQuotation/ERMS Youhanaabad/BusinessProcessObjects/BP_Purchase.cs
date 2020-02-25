using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BusinessEntities;
using System.Data.SqlClient;
using DAL;

namespace BusinessProcessObjects
{
    public class BP_Purchase
    {
        public void InsertPurchaseM(BE_Purchase NewInsBE)
        {
            DAL_Purchase NewDalIns = new DAL_Purchase();
            NewDalIns.InsertPurchaseMaster(NewInsBE);
        }

        public void InsertPurchaseD(BE_Purchase NewInsBEDetail)
        {
            DAL_Purchase NewBEDetail = new DAL_Purchase();
            NewBEDetail.InsertPurchaseDetail(NewInsBEDetail);
        }

        public void UpdatePurchaseMaster(BE_Purchase NewUpdateBE)
        {
            DAL_Purchase NewDAlUpdate = new DAL_Purchase();
            NewDAlUpdate.UpdatePurchaseMaster(NewUpdateBE);
        }

        public DataTable GetItemDetails(int ItemID)
        {
            DAL_Purchase NewDALObj = new DAL_Purchase();
            return NewDALObj.GetItemDetail(ItemID);
        }

        public DataTable GetPurchasedItems(int PrID)
        {
            DAL_Purchase NewPurDAL = new DAL_Purchase();
            return NewPurDAL.GetPurchasedItems(PrID);
        }

    }
}
