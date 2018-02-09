using System;

namespace com.AppliedLine.CargoCanal.Models
{
    public class Role
    {
        public long ID { get; set; }
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
        public string Description { get; set; }
        public long CompanyID { get; set; }
    }
}
