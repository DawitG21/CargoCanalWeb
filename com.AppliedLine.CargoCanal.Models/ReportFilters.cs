using System;
using System.Collections.Generic;

namespace com.AppliedLine.CargoCanal.Models
{
    public class ReportFilters
    {
        public string ReportName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Bill { get; set; }
        public string TIN { get; set; }
        public string Cargo { get; set; }
        public string Country { get; set; }
        public string Problem { get; set; }
        public string Token { get; set; }
    }
}
