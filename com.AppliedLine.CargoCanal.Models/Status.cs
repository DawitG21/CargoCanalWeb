using System;

namespace com.AppliedLine.CargoCanal.Models
{
    public class Status
    {
        public long ID { get; set; }
        public string Description { get; set; }
        public string Abbr { get; set; }
        public long ImpExpTypeID { get; set; }
        public bool Sea { get; set; }
        public bool Air { get; set; }
        public bool Truck { get; set; }
        public bool PipeLine { get; set; }
    }
}
