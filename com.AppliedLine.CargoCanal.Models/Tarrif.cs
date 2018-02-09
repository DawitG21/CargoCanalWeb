using System;

namespace com.AppliedLine.CargoCanal.Models
{
    public class Tariff
    {
        public long ID { get; set; }
        public string HScode { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public decimal Duty { get; set; }
        public decimal Excise { get; set; }
        public decimal Vat { get; set; }
        public decimal Withholding { get; set; }
        public decimal Sur { get; set; }
        public string SecondSch1 { get; set; }
        public string SecondSch2 { get; set; }
        public decimal ExportTax { get; set; }
        public string SpecialPermission { get; set; }
    }
}
