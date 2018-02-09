using System;

namespace com.AppliedLine.CargoCanal.Models
{
    public class ItemDetail
    {
        public long ID { get; set; }        
        public long ItemID { get; set; }
        public string ItemNumber { get; set; }
        public DateTime DateInserted { get; set; }
        public long StuffModeID { get; set; }
        
        // value is null for export
        public long? DestinationID { get; set; }
    }
}
