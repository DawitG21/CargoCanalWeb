using System;

namespace com.AppliedLine.CargoCanal.Models
{
    public class Port
    {
        public long ID { get; set; }
        public string PortName { get; set; }
        public string PortCode { get; set; }
        public long CountryID { get; set; }
        public long ModeOfTransportID { get; set; }
        public bool IsDryPort { get; set; }
    }
}
