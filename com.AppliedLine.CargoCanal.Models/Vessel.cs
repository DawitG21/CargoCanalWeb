using System;

namespace com.AppliedLine.CargoCanal.Models
{
    public class Vessel: IEquatable<Vessel>
    {
        public long ID { get; set; }
        public string VesselName { get; set; }
        public long CarrierID { get; set; }
        public long ModeOfTransportID { get; set; }

        public bool Equals(Vessel other)
        {
            // check whether the compared object is null
            if (Object.ReferenceEquals(other, null)) return false;

            // check whether the compared object references the same data
            if (Object.ReferenceEquals(this, other)) return true;

            return VesselName.Equals(other.VesselName);
        }

        public override int GetHashCode()
        {

            //Get hash code for the field if it is not null. 
            int hashVesselName = VesselName == null ? 0 : VesselName.GetHashCode();

            //Get hash code for the ID field. 
            int hashCarrierID = CarrierID.GetHashCode();

            //Calculate the hash code for the Carrier ID. 
            return hashVesselName;// hashVesselName ^ hashCarrierID;
        }
    }
}
