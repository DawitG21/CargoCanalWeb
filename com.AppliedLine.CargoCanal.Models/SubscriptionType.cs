using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class SubscriptionType
    {
        public long ID { get; set; }
        public string SubscriptionName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Cost { get; set; }
        public string Currency { get; set; }
        public string RenewalCycle { get; set; }
    }
}
