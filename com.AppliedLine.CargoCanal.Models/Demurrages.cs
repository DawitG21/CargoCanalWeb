using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    class Demurrages
    {
        public DataTable DemurrageTable { get; set; }
        public JArray DemmurageImportsWithStatuses { get; set; }
    }
}
