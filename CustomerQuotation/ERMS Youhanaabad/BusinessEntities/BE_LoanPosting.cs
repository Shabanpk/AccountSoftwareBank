using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessEntities
{
    public class BE_LoanPosting
    {
        public long LoanPostingID { get; set; }

        public int PersonID { get; set; }

        public double Amount { get; set; }

        public DateTime Created_At { get; set; }

        public DateTime Modify_At { get; set; }

        public bool IsActive { get; set; }
    }
}
