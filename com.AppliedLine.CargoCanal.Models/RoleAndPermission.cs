using System;

namespace com.AppliedLine.CargoCanal.Models
{
    public class RoleAndPermission
    {
        public Role Role { get; set; }
        public RolePermission RolePermission { get; set; }
        public Guid Token { get; set; }
    }
}
