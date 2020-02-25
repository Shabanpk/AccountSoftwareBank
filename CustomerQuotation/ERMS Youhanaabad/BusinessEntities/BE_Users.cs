using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace BusinessEntities
{
    public class BE_Users
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EncryptPassword { get; set; }
        public string UserStatus { get; set; }
        public int RoleID { get; set; }
        public DateTime Created_At { get; set; }
        public int Created_Id { get; set; }
        public DateTime Modify_At { get; set; }
        public int Modify_Id { get; set; }
        public bool Status { get; set; }
        public DataTable dtUserRight { get; set; }
    }
}
