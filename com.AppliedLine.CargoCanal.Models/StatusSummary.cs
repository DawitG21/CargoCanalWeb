using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class StatusSummary
    {
        public StatusSummary()
        {
            OnVoyage = new StatusSummarySub() { Name = "On Voyage", Code = "VOY" };
            Discharged = new StatusSummarySub() { Name = "Cargo Discharged", Code = "DCH" };
            UCC = new StatusSummarySub() { Name = "Under Customs Clearance (UCC)", Code = "UCC" };
            CRL = new StatusSummarySub() { Name = "Cargo Ready For Loading (CRL)", Code = "CRL" };
            Dispatched = new StatusSummarySub() { Name = "Cargo Dispatched (CD1)", Code = "CD1" };
            UC1 = new StatusSummarySub() { Name = "Under Customs Inspection (UC1)", Code = "UC1" };
            Delivered = new StatusSummarySub() { Name = "Cargo Delivered (CD2)", Code = "CD2" };
        }

        public StatusSummarySub OnVoyage { get; set; }
        public StatusSummarySub Discharged { get; set; }
        public StatusSummarySub UCC { get; set; }
        public StatusSummarySub CRL { get; set; }
        public StatusSummarySub Dispatched { get; set; }
        public StatusSummarySub UC1 { get; set; }
        public StatusSummarySub Delivered { get; set; }
    }
}
