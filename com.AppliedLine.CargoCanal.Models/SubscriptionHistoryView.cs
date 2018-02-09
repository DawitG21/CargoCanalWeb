using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class SubscriptionHistoryView
    {
        public long ID { get; set; }
        public string SubscriptionName{ get; set; }
        public string RenewalCycle { get; set; }
        public string ServiceType { get; set; }
        public string Category { get; set; }
        public decimal Cost { get; set; }
        public string Currency { get; set; }
        public decimal AmountPaid { get; set; }
        public string AmountPaidCurrency { get; set; }
        public int MaximumUsers { get; set; }
        public int UsersCount { get; set; }
        public DateTime ActivatedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }
    }
}
