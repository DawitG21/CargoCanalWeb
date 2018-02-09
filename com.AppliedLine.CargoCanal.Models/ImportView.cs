using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class ImportView
    {
        public long ID { get; set; }
        public long ImpExpTypeID { get; set; }
        public string Bill { get; set; }
        public DateTime BillDate { get; set; }
        public string Carrier { get; set; }
        public bool Completed { get; set; }
        public bool Terminated { get; set; }
        public DateTime DateInitiated { get; set; }
        public DateTime DateInserted { get; set; }
        public DateTime? ETA { get; set; }
        public string IncoTerm { get; set; }
        public string MOT { get; set; }
        public string Reason { get; set; }
        public string ReferenceNo { get; set; }
        public bool ReImportExport { get; set; }
        public string Remark { get; set; }
        public string PortOfLoading { get; set; }
        public string PortOfDischarge { get; set; }
        public bool Unimodal { get; set; }
        public string Vessel { get; set; }
        public string VoyageNumber { get; set; }
        public LC LC { get; set; }
        public ConsigneeImportExportWithTin Consignee { get; set; }
        public Cost Cost { get; set; }
        public List<ItemView> Items { get; set; }
        public List<Document> Documents { get; set; }
        public int ProblemsUnresolved { get; set; }
    }
}
