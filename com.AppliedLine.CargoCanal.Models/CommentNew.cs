using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class CommentNew
    {
        public long ImportExportID { get; set; }
        public string CommentText { get; set; }
        public string Tags { get; set; }
        public Guid Token { get; set; }
    }
}
