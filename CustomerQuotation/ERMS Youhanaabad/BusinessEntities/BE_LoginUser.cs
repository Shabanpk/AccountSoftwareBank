using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessEntities
{
    public class BE_LoginUser
    {
        public int userID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }
}
