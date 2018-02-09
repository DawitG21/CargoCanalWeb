using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class User
    {
        public Login Login { get; set; }
        public Person Person { get; set; }
        public Company Company { get; set; }
    }
}
