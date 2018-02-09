using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class Document
    {
        public long ID { get; set; }
        public string DocumentName { get; set; }
        public string Filename { get; set; }
        public string Filepath { get; set; }
        public string FileExtension { get; set; }
        public string FileData { get; set; }
    }
}
