using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{

    public class Consignee
    {
        public Consignee()
        {
            //consturctor
            ConsigneeID = "";
            PersonID = "";
            CompanyID = "";
            IsAgent = "";
        }
        //constructor with all prameters
        public Consignee(long _ID, string _ConsigneeID, string _PersonID, string _CompanyID, string _IsAgent)
        {
            ID = _ID;
            ConsigneeID = _ConsigneeID;
            PersonID = _PersonID;
            CompanyID = _CompanyID;
            IsAgent = _IsAgent;
        }
        long _ID;
        public long ID
        {
            set { _ID = value; }
            get { return _ID; }
        }
        string _ConsigneeID;
        public string ConsigneeID
        {
            set { _ConsigneeID = value; }
            get { return _ConsigneeID; }
        }
        string _PersonID;
        public string PersonID
        {
            set { _PersonID = value; }
            get { return _PersonID; }
        }
        string _CompanyID;
        public string CompanyID
        {
            set { _CompanyID = value; }
            get { return _CompanyID; }
        }
        string _IsAgent;
        public string IsAgent
        {
            set { _IsAgent = value; }
            get { return _IsAgent; }
        }
        

        
        #region " Class Members Code Starts Here ..........."
        // Class Members code....................
        public int SaveRecord(Consignee clsConsigneeObj)
        {
            
            return DataAccessLayer.SaveclsConsigneeEntry(clsConsigneeObj);
        }

        public int updateRecord(Consignee clsConsigneeObj)
        {
            
            return DataAccessLayer.updateclsConsigneeEntry(clsConsigneeObj);
        }

        public Consignee getclsConsigneeRecord(string ConsigneeID)
        {
            
            return DataAccessLayer.getclsConsigneeObj(ConsigneeID);
        }

        public int deleteRecord(string ConsigneeID)
        {
            
            return DataAccessLayer.deleteclsConsigneeEntry(ConsigneeID);
        }

        // -------------------- end of Class Members Code
        #endregion

    }
}
