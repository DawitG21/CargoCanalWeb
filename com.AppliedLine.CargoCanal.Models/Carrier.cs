using System;

namespace com.AppliedLine.CargoCanal.Models
{
    public class Carrier
    {
        public Carrier()
        {
            CarrierName = string.Empty;
        }
                
        public long ID { get; set; }
        public string CarrierName { get; set; }
        public long CountryID { get; set; }
        public long ModeOfTransportID { get; set; }
    }
}
