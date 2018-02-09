using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class SearchBill
    {
        public string SearchText { get; set; }
        public Guid Token { get; set; }
        public long CompanyID { get; set; }
    }
}
