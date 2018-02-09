using System;

namespace com.AppliedLine.CargoCanal.Models
{
    public class DryPortTransaction
    {
        public long ID { get; set; }
        public long ImportExportID { get; set; }
        public long DryPortID { get; set; }
        public long PersonID { get; set; }
        public DateTime DateInserted { get; set; }
        public bool IsInbound { get; set; }
    }
}
