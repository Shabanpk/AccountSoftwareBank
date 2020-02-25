using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessEntities
{
    public class BE_PasswordChange
    {
        public int UserID { get; set; }
        public string  UserName { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
