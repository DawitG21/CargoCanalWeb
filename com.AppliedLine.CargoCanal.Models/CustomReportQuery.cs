using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class CustomReportQuery
    {
        public List<string> Queries { get; set; }
        public long CompanyTypeID { get; set; }
        public long CompanyID { get; set; }
    }
}
