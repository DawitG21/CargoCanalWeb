namespace com.AppliedLine.CargoCanal.Models
{
    public class Cost
    {
        public Cost()
        {
            FreightExRate = new CostExRate();
            InsuranceExRate = new CostExRate();
            InlandTransportExRate = new CostExRate();
            DischargePortStorageExRate = new CostExRate();
            InlandStorageExRate = new CostExRate();
            PortHandlingExRate = new CostExRate();
            TruckDetentionExRate = new CostExRate();
            AgentCommisionExRate = new CostExRate();
        }

        public long ID { get; set; }
        public long ImportExportID { get; set; }
        public string BaseCurrency { get; set; }

        public decimal Freight { get; set; }
        public CostExRate FreightExRate { get; set; }

        public decimal Insurance { get; set; }
        public CostExRate InsuranceExRate { get; set; }

        public decimal InlandTransport { get; set; }
        public CostExRate InlandTransportExRate { get; set; }

        public decimal DischargePortStorage { get; set; }
        public CostExRate DischargePortStorageExRate { get; set; }

        public decimal InlandStorage { get; set; }
        public CostExRate InlandStorageExRate { get; set; }

        public decimal PortHandling { get; set; }
        public CostExRate PortHandlingExRate { get; set; }

        public decimal TruckDetention { get; set; }
        public CostExRate TruckDetentionExRate { get; set; }

        public decimal AgentCommision { get; set; }
        public CostExRate AgentCommisionExRate { get; set; }

        public decimal OtherChargesLocal { get; set; }
        public decimal OtherChargesDollar { get; set; }      
    }
}
