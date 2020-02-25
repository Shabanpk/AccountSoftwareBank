using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using BusinessEntities;
using System.Data;

namespace BusinessProcessObjects
{
    public class BP_Group
    {

        public long GetMaxID()
        {
            DAL_Group NewDALObj = new DAL_Group();
            return NewDALObj.GetMaxID();
        }

        public void InsertGroup(BE_Group NewBEObj)
        {
            DAL_Group NewDALOBj = new DAL_Group();
            NewDALOBj.InsertGroup(NewBEObj);
        }

        public bool DuplicateGroup(BE_Group  NewBEObj)
        {
            DAL_Group NewDalObj = new DAL_Group();
            return NewDalObj.DuplicateGroup(NewBEObj);

        }

        public DataTable GetGroupName(BE_Group NewBEObj)
        {
            DAL_Group NewDalObj = new DAL_Group();
            return NewDalObj.GetGroupName(NewBEObj);
        }

        public void UPdateGroupName(BE_Group NewBEObj)
        {
            DAL_Group NewDALObj = new DAL_Group();
            NewDALObj.UPdateGroupName(NewBEObj);
        }
    }
}
