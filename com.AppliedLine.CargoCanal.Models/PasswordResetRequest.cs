using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class PasswordResetRequest
    {
        public string Username { get; set; }
        public string Referrer { get; set; }
        public string Url { get; set; }
    }
}
