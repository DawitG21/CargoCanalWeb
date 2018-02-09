using System;
using System.Collections.Generic;

namespace com.AppliedLine.CargoCanal.Models
{
    public class ProblemUpdateView
    {
        public long ID { get; set; }
        public DateTime DateInserted { get; set; }
        public bool IsResolved { get; set; }
        public string ProblemName { get; set; }
        public DateTime ProblemDate { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public List<ProblemUpdateMessage> Messages { get; set; }
    }
}
