using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class CallParams
    {
        public long ID { get; set; }
        public string Token { get; set; }
        public string[] Args { get; set; }
    }
}
