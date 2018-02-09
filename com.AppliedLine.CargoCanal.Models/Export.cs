using System;

namespace com.AppliedLine.CargoCanal.Models
{
    public class Export
    {
        public long ID { get; set; }
        public long ImportExportID { get; set; }
        public DateTime? ETA { get; set; }
        public DateTime? ATA { get; set; }
        public DateTime? CPORecievedDate { get; set; }
        public DateTime? OriginalDocumentDate { get; set; }
        public string WayBill { get; set; }
        public DateTime? WayBillDate { get; set; }
        public string DeclarationNo { get; set; }
        public DateTime? DeclarationDate { get; set; }
        public long StuffingLocationID { get; set; }
        public long OriginID { get; set; }
        public string ShippingInstructionNo { get; set; }
        public DateTime? ShippingInstructionDate { get; set; }
    }
}
