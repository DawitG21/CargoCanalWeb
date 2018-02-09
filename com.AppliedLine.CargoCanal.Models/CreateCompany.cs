using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class CreateCompany
    {
        public Company Company { get; set; }
        public Login Login { get; set; }
        public Person Person { get; set; }
    }
}
