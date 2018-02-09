using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class ImportExportAndTracking
    {
        public ImportExportAndTracking()
        {
            Import = new Import();
            Export = new Export();
            Tracking = new Tracking();
        }

        public Import Import { get; set; }
        public Export Export { get; set; }
        public Tracking Tracking { get; set; }
    }
}
