using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class Comment
    {
        public long ID { get; set; }
        public long ImportExportID { get; set; }
        public long CompanyTypeID { get; set; }
        public string CommentText { get; set; }
        public string Tags { get; set; }
        public string Fullname { get; set; }
        public DateTimeOffset DateInserted { get; set; }
        public Guid Token { get; set; }
    }
}
