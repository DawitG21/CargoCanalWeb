using System;

namespace com.AppliedLine.CargoCanal.Models
{
    public class Cargo
    {
        public Cargo()
        {
            CargoName = string.Empty;
            IsEnabled = true;
        }

        public long ID { get; set; }
        public string CargoName { get; set; }        
        public long ImpExpTypeID { get; set; }
        public bool IsEnabled { get; set; }
    }
}
