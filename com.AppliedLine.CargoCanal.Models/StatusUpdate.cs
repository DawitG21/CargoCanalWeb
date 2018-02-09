using System;

namespace com.AppliedLine.CargoCanal.Models
{
    public class StatusUpdate
    {
        public long ID { get; set; }
        public long ImportExportID { get; set; }
        public long StatusID { get; set; }
        public DateTime StatusDate { get; set; }
        public DateTime DateInserted { get; set; }
        public long LocationID { get; set; }
    }
}
