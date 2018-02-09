using System;
using System.Collections.Generic;

namespace com.AppliedLine.CargoCanal.Models
{
    public class ProblemUpdate
    {
        public long ID { get; set; }
        public long ImportExportID { get; set; }
        public long ProblemID { get; set; }
        public DateTime ProblemDate { get; set; }
        public DateTime DateInserted { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public bool IsResolved { get; set; }
        public List<ProblemUpdateMessage> Messages { get; set; }
    }
}
