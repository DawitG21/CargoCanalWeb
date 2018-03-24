using System;

namespace com.AppliedLine.CargoCanal.Models
{
    public class Company : IEquatable<Company>
    {
        public Company()
        {
            CompanyName = string.Empty;
            IsActive = false;
        }
        
        public long ID { get; set; }
        public string CompanyName { get; set; }
        public long CompanyTypeID { get; set; }
        public long CountryID { get; set; }
        public string Address { get; set; }
        public string TownCity { get; set; }
        public string State { get; set; }
        public string POBox { get; set; }
        public string Wereda { get; set; }
        public string KefleKetema { get; set; }
        public string Kebele { get; set; }
        public string HouseNo { get; set; }
        public string Telephone { get; set; }
        public string ContactName { get; set; }
        public string ContactMobile { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string TIN { get; set; }
        public string LicenseNumber { get; set; }
        public string LicenseUnder { get; set; }
        public DateTime? LastRenewedDate { get; set; }
        public DateTime? LicenseIssuedDate { get; set; }
        public bool IsActive { get; set; }
        public string PhotoFilename { get; set; }
        public string Photo { get; set; }
        public string Filepath { get; set; }

        public bool Equals(Company other)
        {
            // check whether the compared object is null
            if (Object.ReferenceEquals(other, null)) return false;

            // check whether the compared object references the same data
            if (Object.ReferenceEquals(this, other)) return true;

            return CompanyName.Equals(other.CompanyName);
        }

        public override int GetHashCode()
        {

            //Get hash code for the CompanyName field if it is not null. 
            int hashCompanyName = CompanyName == null ? 0 : CompanyName.GetHashCode();

            //Get hash code for the ID field. 
            int hashCompanyID = ID.GetHashCode();

            //Calculate the hash code for the Company ID. 
            return hashCompanyName ^ hashCompanyID;
        }
    }
}
