using System;

namespace com.AppliedLine.CargoCanal.Models
{
    public class ItemDetailView
    {
        public long ID { get; set; }        
        public string ItemNumber { get; set; }
        public string StuffMode { get; set; }
        // value is null for export
        public string Destination { get; set; }
    }
}
