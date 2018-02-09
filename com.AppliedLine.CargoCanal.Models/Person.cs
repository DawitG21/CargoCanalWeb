using System;

namespace com.AppliedLine.CargoCanal.Models
{
    public class Person
    {
        public long ID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public long CompanyID { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PhotoFilename { get; set; }
        public string Photo { get; set; }
    }
}
