using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessEntities;
using DAL;
using System.Data;

namespace BusinessProcessObjects
{
    public class BP_StockTransfer
    {

        DAL_StockTransfer NewDALObj = new DAL_StockTransfer();


        public void Insert_StockTransferM(BE_StockTransfer NewBEObj)
        {
            NewDALObj.Insert_StockTransferM(NewBEObj);
        }


        public void InsertStockTransferD(BE_StockTransfer NewBEObj)
        {
            NewDALObj.InsertStockTransferD(NewBEObj);
        }

        public DataTable GetItemData(int ItemID)
        {
            return NewDALObj.GetItemData(ItemID);
        }

        public void LoadBranch(Infragistics.Win.UltraWinGrid.UltraCombo cbo)
        {
            NewDALObj.LoadBranch(cbo);
        }

    }
}
