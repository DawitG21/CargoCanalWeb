using System;

namespace com.AppliedLine.CargoCanal.Models
{
    public class Import
    {
        public long ID { get; set; }
        public long ImportExportID { get; set; }
        public string BillNumber { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime? ETA { get; set; }
        public DateTime? ATA { get; set; }
    }
}
