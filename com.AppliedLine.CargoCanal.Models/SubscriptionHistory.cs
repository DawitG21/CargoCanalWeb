using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class SubscriptionHistory
    {
        public long ID { get; set; }
        public long CompanyID { get; set; }
        public long SubcriptionTypeID { get; set; }
        public DateTime ActivatedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
