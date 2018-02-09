using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class PasswordResetChange
    {
        public Guid Uid { get; set; }
        public string Usalt { get; set; }
        public string Url { get; set; }
        public string Password { get; set; }
    }
}
