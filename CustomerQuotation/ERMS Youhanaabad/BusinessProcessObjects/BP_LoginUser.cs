using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using BusinessEntities;
using System.Data;

namespace BusinessProcessObjects
{
    public class BP_LoginUser
    {
        public bool CheckUserPassword(BE_LoginUser BEObj)
        {
            DAL_LoginUser NewDAlObj = new DAL_LoginUser();
            return NewDAlObj.CheckUserPassword(BEObj);
        }

        public DataTable GetUserDetail(BE_LoginUser DetailObj)
        {
            DAL_LoginUser NewDalObj = new DAL_LoginUser();
            return NewDalObj.GetUserDetail(DetailObj);
        }

        public void LoadStyles(Infragistics.Win.UltraWinGrid.UltraCombo cbo)
        {
            DAL_LoginUser NewDALObj = new DAL_LoginUser();
            NewDALObj.LoadStyles(cbo);
        }
    }
}
