using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class StatusSummarySub
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public JObject Days { get; set; }
        public DataTable ImportExport { get; set; }
    }
}
