using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessEntities
{
    public class BE_DirectSales
    {
        public long DR_ID { get; set; }
        public DateTime DRS_Date { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string DRS_Name { get; set; }
        public string MobileNo { get; set; }
        public bool IsActive { get; set; }
        public int ItemID { get; set; }
        public double ItemPrice { get; set; }
        public double  SalesPrice { get; set; }
        public double Qty { get; set; }
        public double NetAmount { get; set; }
        public string SlipNo { get; set; }
        public int Discount { get; set; }
        public int UserID { get; set; }
        public string Type { get; set; }
        public string Remarks { get; set; }
        public bool IsLoan { get; set; }
        public DateTime LoanReceivedDate { get; set; }
    }
}
