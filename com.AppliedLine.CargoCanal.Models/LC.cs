using System;

namespace com.AppliedLine.CargoCanal.Models
{
    public class LC
    {
        public LC()
        {
            LCNumber = string.Empty;
        }

        public long ID { get; set; }
        public long ImportExportID { get; set; }
        public string LCNumber { get; set; }
    }
}
