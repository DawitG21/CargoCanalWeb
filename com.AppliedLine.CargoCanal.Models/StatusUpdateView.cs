using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class StatusUpdateView
    {
        public long ID { get; set; }
        public long ImportExportID { get; set; }
        public string Location { get; set; } //e.g. Modjo
        public string StatusText { get; set; }
        public string Abbr { get; set; }
        public DateTime StatusDate { get; set; }
        public DateTime DateInserted { get; set; }
    }
}
