using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class DashboardImportExportSummary
    {
        public long Total { get; set; }
        public long Completed { get; set; }
        public double PercentCompleted { get; set; }
        public long Pending { get; set; }
        public double PercentPending { get; set; }
    }
}
