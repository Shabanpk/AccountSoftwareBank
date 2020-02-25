using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessEntities
{
    public class BE_Previous
    {
        public long ReocordID { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Modify_At { get; set; }
        public bool IsActive { get; set; }
    }
}
