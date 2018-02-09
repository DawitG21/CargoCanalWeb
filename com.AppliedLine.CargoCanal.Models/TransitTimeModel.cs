using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class TransitTimeModel
    {
        public Import Import { get; set; }
        public string Origin { get; set; }
        public int LoadedToVoyage { get; set; }
        public int VoyageToDischarged { get; set; }
        public int DischargedToUCC { get; set; }
        public int UCCToReadyLoading { get; set; }
        public int ReadyToDispatched { get; set; }
        public int DispatchedToUCI { get; set; }
        public int UCIToDelivered { get; set; }
        public int TotalTransitTime { get; set; }
    }
}
