using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BusinessEntities;
using DAL;

namespace BusinessProcessObjects
{
    public class BP_OpeningBalance
    {
        public void InsertOpeningBalanceM(BE_OpeningBalance ObjMaster)
        {
            DAL_OpeningBalance ObjMasterDAL = new DAL_OpeningBalance();
            ObjMasterDAL.InsertOpeningBalanceM(ObjMaster);
        }

        public void InsertOpeningBalanceD(BE_OpeningBalance ObjDetail)
        {
            DAL_OpeningBalance NewObjDetail = new DAL_OpeningBalance();
            NewObjDetail.InsertOpeningBalanceDetail(ObjDetail);
        }

        public long GetMaxID()
        {
            DAL_OpeningBalance NewMaxIDDAL = new DAL_OpeningBalance();
            return NewMaxIDDAL.GetMaxID();
        }

        public DataTable GetItems(int ItemID)
        {
            DAL_OpeningBalance NewDALObj = new DAL_OpeningBalance();
            return NewDALObj.GetItems(ItemID);
        }

        public DataTable GetOpBalID(int OpBal)
        {
            DAL_OpeningBalance NewDALObj = new DAL_OpeningBalance();
            return NewDALObj.GetOpBal(OpBal);
        }
    }
}
