using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using BusinessEntities;
using System.Data;

namespace BusinessProcessObjects
{
    public class BP_Munit
    {
        public string InsertMUnit(BE_MUnit NewObjMUnit)
        {
            DAL_MUnit NewDALObj = new DAL_MUnit();
            return NewDALObj.InsertMUnit(NewObjMUnit);
        }

        public bool DuplicateMUnit(BE_MUnit NewObjMunit)
        {
            DAL_MUnit NewDalObj=new DAL_MUnit();
            return NewDalObj.DuplicateMUnit(NewObjMunit);
            
        }

        public DataTable GetMUnitName(BE_MUnit NewBEObj)
        {
            DAL_MUnit NewDALObj = new DAL_MUnit();
            return NewDALObj.GetMUnitName(NewBEObj);
        }

        public string UPdateMUnit(BE_MUnit NewBEUpdateObj)
        {
            DAL_MUnit NewDALObj = new DAL_MUnit();
            return NewDALObj.UPdateMUnit(NewBEUpdateObj);
        }
    }
}
