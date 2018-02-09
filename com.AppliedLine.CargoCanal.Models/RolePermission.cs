using System;

namespace com.AppliedLine.CargoCanal.Models
{

    public class RolePermission
    {
        public long ID { get; set; }
        public long RoleID { get; set; }
        public bool EditUser { get; set; }
        public bool EditAdmin { get; set; }
        public bool EditSA { get; set; }
        public bool EditFF { get; set; }
        public bool EditCC { get; set; }
        public bool EditAll { get; set; }
    }
}
