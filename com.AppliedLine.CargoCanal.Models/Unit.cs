using System;

namespace com.AppliedLine.CargoCanal.Models
{
    public class Unit
    {
        public long ID { get; set; }
        public string UnitName { get; set; }
        public decimal BaseUnitRate { get; set; }
        public bool IsVolume { get; set; }
    }
}
