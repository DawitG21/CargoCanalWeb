using System;

namespace com.AppliedLine.CargoCanal.Models
{
    public class DryPort
    {
        public long ID { get; set; }
        public string PortName { get; set; }
        public long CountryID { get; set; }
        public long LocationID { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
