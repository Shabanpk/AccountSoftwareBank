using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
namespace BusinessEntities
{
    public class BE_OpeningBalance
    {
        public int OpeningBalanceID { get; set; }
        public DateTime OpeningDate { get; set; }
        //public int ItemGroupID { get; set; }
        public string Remarks { get; set; }
        public int UserID { get; set; }
        public DataTable ItemDetail { get; set; }
        public int ItemID { get; set; }
        public double ItemQty{ get; set; }
        public bool IsActive { get; set; }
    }
}
