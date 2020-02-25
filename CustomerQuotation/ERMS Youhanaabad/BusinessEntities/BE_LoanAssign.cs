using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessEntities
{
   public class BE_LoanAssign
    {
        public int PersonID { get; set; }

        public string PersonName { get; set; }

        public string PersonNIC { get; set; }

        public string PersonAddress { get; set; }

        public string PersonEmail { get; set; }

        public string PersonPhone { get; set; }

        public string CityName { get; set; }

        public DateTime Created_At { get; set; }

        public DateTime Modify_At { get; set; }

        public bool IsActive { get; set; }

    }
}
