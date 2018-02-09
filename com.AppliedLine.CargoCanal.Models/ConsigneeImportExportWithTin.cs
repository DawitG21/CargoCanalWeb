using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class ConsigneeImportExportWithTin
    {
        public long ID { get; set; }
        public long ImportExportID { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ContactName { get; set; }
        public string ContactMobile { get; set; }
        public string TIN { get; set; }

    }
}
