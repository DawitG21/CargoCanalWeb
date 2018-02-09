using System;

namespace com.AppliedLine.CargoCanal.Models
{
    public class Message
    {
        public Message()
        {
            DateInserted = DateTime.Now;
        }

        public long ID { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string FromPersonID { get; set; }
        public string FromCompanyID { get; set; }
        public string ToCompanyID { get; set; }
        public DateTime DateInserted { get; set; }
        public string Guid { get; set; }
    }
}
