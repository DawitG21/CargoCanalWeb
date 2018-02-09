using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class UsersInCompanyView
    {
        public long ID { get; set; } //LOGIN ID
        public long RoleID { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Photo { get; set; }
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
