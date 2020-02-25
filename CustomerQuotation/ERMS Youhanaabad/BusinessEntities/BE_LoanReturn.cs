using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessEntities
{
   public class BE_LoanReturn
    {
        public long LoanReturnID { get; set; }

        public int PersonID { get; set; }

        public double Amount { get; set; }

        public DateTime ReturnDate { get; set; }

        public DateTime Modify_At { get; set; }

    }
}
