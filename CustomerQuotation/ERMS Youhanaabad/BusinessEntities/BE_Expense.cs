using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessEntities
{
   public class BE_Expense
    {
        public long ExpenseID { get; set; }
        public DateTime AssignDate { get; set; }
        public string Expenses { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Modify_At { get; set; }
        public bool IsActive { get; set; }

    }
}
