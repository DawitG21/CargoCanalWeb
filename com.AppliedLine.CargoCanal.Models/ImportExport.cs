using System;
using System.Collections.Generic;

namespace com.AppliedLine.CargoCanal.Models
{
    public class ImportExport
    {
        public ImportExport()
        {
            ReferenceNo = string.Empty;
            Vessel = string.Empty;
            VoyageNumber = string.Empty;
            Remark = string.Empty;
        }

        public long ID { get; set; }
        public long CountryID { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime DateInserted { get; set; }
        public DateTime DateChanged { get; set; }
        public long ImpExpTypeID { get; set; }
        public long ImportExportReasonID { get; set; }
        public long PortOfLoadingID { get; set; }
        public long PortOfDischargeID { get; set; }
        public long ModeOfTransportID { get; set; }
        public DateTime DateInitiated { get; set; }
        public long ConsigneeID { get; set; }

        // references the forwarder's Company
        public long CompanyID { get; set; }
        public long CreatedBy { get; set; } // references the user that created the document
        public long ChangedBy { get; set; } // references the user that last edited the document
        public long CarrierID { get; set; }
        //public long VesselID { get; set; } 
        // auto-complete implementaion for Vessel
        public string Vessel { get; set; }
        public string VoyageNumber { get; set; }
        public long IncoTermID { get; set; }
        public string Remark { get; set; }
        public bool ReImportExport { get; set; }
        public bool Unimodal { get; set; }
        public bool Terminated { get; set; }
        public bool Completed { get; set; }

        public Import Import { get; set; }
        public Export Export { get; set; }
        public LC LC { get; set; }
        public Company Consignee { get; set; }
        public List<Item> Items { get; set; }
        public List<ImportExportDoc> Documents { get; set; }
    }

    public enum ImpExpTypeEnum
    {
        Import = 1, Export = 2
    }
}