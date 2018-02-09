using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class ProblemUpdateMessage
    {
        public long ID { get; set; }
        public long ProblemUpdateID { get; set; }
        public string Message { get; set; }
        public DateTime DateInserted { get; set; }
    }
}
