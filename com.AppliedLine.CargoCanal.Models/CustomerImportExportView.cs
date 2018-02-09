using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class CustomerImportExportView: ImportView
    {
        public string StuffingLocation { get; set; }
        public string Origin { get; set; }
        public string ShippingInstructionNo { get; set; }
        public DateTime? ShippingInstructionDate { get; set; }
        public DateTime? CPORecievedDate { get; set; }
        public DateTime? OriginalDocumentDate { get; set; }
        public string WayBill { get; set; }
        public DateTime? WayBillDate { get; set; }
        public string DeclarationNo { get; set; }
        public DateTime? DeclarationDate { get; set; }
        public List<StatusUpdateView> StatusesViews { get; set; }
    }
}
