using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessEntities
{
    public class BE_Purchase
    {
        public int PrID { get; set; }
        public DateTime PrDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string VendorName { get; set; }
        public string  Remarks { get; set; }
        public string IPAddress { get; set; }
        public int ItemID { get; set; }
        public int MUnitID { get; set; }
        public double PrQty { get; set; }
        public double PurchasePrice { get; set; }
        public bool IsActive { get; set; }
        public bool ChkCreditSales { get; set; }
        public DateTime CreditDueDate { get; set; }
        public int CreditDays { get; set; }
        public double TotalPRAmount { get; set; }
        public bool OldStatus { get; set; }
        public int UserID { get; set; }
    }
}
