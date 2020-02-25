using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessEntities
{
    public class BE_ItemDef
    {
        public int  ItemID { get; set; }
        public string ItemName { get; set; }
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public int MUnitID { get; set; }
        public string MUnitName { get; set; }
        public float ItemPrice { get; set; }
        public bool IsActive { get; set; }
    }
}
