using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class ReportFilter
    {
        public string ReportName { get; set; }
        public string DocumentType { get; set; }
        public string CompanyID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<string> Queries { get; set; }

        public List<ConfigStatsTime> ConfigStatsTimes { get; set; }

        public List<Import> AllImports { get; set; }
        public List<Export> AllExports { get; set; }
    }
}
