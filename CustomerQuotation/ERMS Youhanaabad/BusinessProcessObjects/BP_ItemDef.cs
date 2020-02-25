using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BusinessEntities;
using System.Data.SqlClient;
using DAL;



namespace BusinessProcessObjects
{
    public class BP_ItemDef
    {
        public string InsertItem(BE_ItemDef NewBEObj)
        {
            DAL_ItemDef NewDalObj = new DAL_ItemDef();
            return NewDalObj.InsertItem(NewBEObj);
        }

        public bool DuplicateItemName(BE_ItemDef NewBEObj)
        {
            DAL_ItemDef NewDalObj = new DAL_ItemDef();
            return NewDalObj.DuplicateItemName(NewBEObj);
        }

        public DataTable GetItemName(BE_ItemDef NewBEObj)
        {
            DAL_ItemDef NewDALObj = new DAL_ItemDef();
            return NewDALObj.GetItemName(NewBEObj);
        }

        public string UPdateItem(BE_ItemDef NewBEObj)
        {
            DAL_ItemDef NewDALObj = new DAL_ItemDef();
            return NewDALObj.UPdateItem(NewBEObj);
        }

        public void LoadGroup(Infragistics.Win.UltraWinGrid.UltraCombo cbo)
        {
            DAL_ItemDef NewObj = new DAL_ItemDef();
            NewObj.LoadGroups(cbo);
        }

        public void LoadMunit(Infragistics.Win.UltraWinGrid.UltraCombo cbo)
        {
            DAL_ItemDef NewDALobj = new DAL_ItemDef();
            NewDALobj.LoadMunits(cbo );
        }

    }
}
