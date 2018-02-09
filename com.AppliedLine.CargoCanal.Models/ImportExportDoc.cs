using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class ImportExportDoc
    {
        public long ID { get; set; }
        public long DocumentID { get; set; }
        public long ImportExportID { get; set; }
        public bool Required { get; set; }
        public bool IsPublic { get; set; }
    }
}
