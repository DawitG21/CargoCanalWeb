using System.Collections.Generic;

namespace com.AppliedLine.CargoCanal.Models
{
    public class ItemView
    {
        public long ID { get; set; }
        public string ItemOrderNo { get; set; }
        public string Cargo { get; set; }
        public string SubCargo { get; set; }
        public string GrossWeight { get; set; }
        public string NetWeight { get; set; }
        public string Volume { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public bool Dangerous { get; set; }
        public List<ItemDetailView> ItemDetails { get; set; }
    }
}