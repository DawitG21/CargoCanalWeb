using System;

namespace com.AppliedLine.CargoCanal.Models
{
    public class SubCargo
    {
        public long ID { get; set; }
        public long CargoID { get; set; }
        public string Description { get; set; }
        public long ImpExpTypeID { get; set; }
        public long UnitID { get; set; }
        public bool HasDim { get; set; }
        public bool IsEnabled { get; set; }
    }
}
