using System;

namespace com.AppliedLine.CargoCanal.Models
{
    public class Tracking
    {
        public long ID { get; set; }
        public long ImportExportID { get; set; }
        public string TrackingNumber { get; set; }
        public string CustomerEmail { get; set; }
        public long AgentID { get; set; }
        public DateTime DateInserted { get; set; }
    }
}