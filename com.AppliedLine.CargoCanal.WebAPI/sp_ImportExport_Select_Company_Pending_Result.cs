//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace com.AppliedLine.CargoCanal.WebAPI
{
    using System;
    
    public partial class sp_ImportExport_Select_Company_Pending_Result
    {
        public long ID { get; set; }
        public long CarrierID { get; set; }
        public long ChangedBy { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTimeOffset DateChanged { get; set; }
        public bool Completed { get; set; }
        public Nullable<long> ConsigneeID { get; set; }
        public long CountryID { get; set; }
        public System.DateTimeOffset DateInitiated { get; set; }
        public System.DateTimeOffset DateInserted { get; set; }
        public long CompanyID { get; set; }
        public long ImpExpTypeID { get; set; }
        public long ImportExportReasonID { get; set; }
        public long IncoTermID { get; set; }
        public long ModeOfTransportID { get; set; }
        public long PortOfDischargeID { get; set; }
        public long PortOfLoadingID { get; set; }
        public string ReferenceNo { get; set; }
        public bool ReImportExport { get; set; }
        public string Remark { get; set; }
        public bool Terminated { get; set; }
        public bool Unimodal { get; set; }
        public long VesselID { get; set; }
        public string VoyageNumber { get; set; }
    }
}