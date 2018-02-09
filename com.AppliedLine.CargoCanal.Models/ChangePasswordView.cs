using System;

namespace com.AppliedLine.CargoCanal.Models
{
    public class ChangePasswordView
    {
        public long ID { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
