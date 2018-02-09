using System;

namespace com.AppliedLine.CargoCanal.Models
{
    public class Login
    {
        public Login()
        {
            IsActive = true;
        }

        public long ID { get; set; }
        public long PersonID { get; set; }        
        public string Username { get; set; }
        public string Password { get; set; }
        public long RoleID { get; set; }
        public bool IsActive { get; set; }
        public Guid Token { get; set; }
        public DateTime? LastSeen { get; set; }
        public DateTime? LastPasswordChange { get; set; }
    }
}
