using System;
using System.Collections.Generic;

namespace com.AppliedLine.CargoCanal.Models
{
    public class Item
    {
        public long ID { get; set; }
        public long ImportExportID { get; set; }        
        public DateTime DateInserted { get; set; }            
        public string ItemOrderNo { get; set; }
        public long CargoID { get; set; }
        public long SubCargoID { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal NetWeight { get; set; }
        public long WeightUnitID { get; set; }
        public decimal Volume { get; set; }
        public long VolumeUnitID { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public bool Dangerous { get; set; }
        public List<ItemDetail> ItemDetails { get; set; }
    }
}