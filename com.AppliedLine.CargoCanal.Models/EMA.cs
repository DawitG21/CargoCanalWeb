using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class EMA
    {
        public long ID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Agent { get; set; }
        public decimal CostPerContainer { get; set; }
        public decimal CostPerKM { get; set; }
        public decimal CostPerKG { get; set; }
        public decimal SeaFreight { get; set; }
        public decimal PortAndClearance { get; set; }
        public decimal InlandTransport { get; set; }
        public decimal CustomStorage { get; set; }
        public string Demurrage { get; set; }
    }
}
