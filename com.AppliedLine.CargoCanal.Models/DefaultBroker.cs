using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{

    public class DefaultBroker
    {
        public DefaultBroker()
        {
            //consturctor
            ownerusername = defaultusername = string.Empty;
        }

        //constructor with all prameters
        public DefaultBroker(long _id, string _ownerusername, string _defaultusername)
        {
            id = _id;
            ownerusername = _ownerusername;
            defaultusername = _defaultusername;
        }
        
        public long id { get; set; }
        
        public string ownerusername { get; set; }
        
        public string defaultusername { get; set; }


        #region " Class Members Code Starts Here ..........."
        // Class Members code....................
        public int SaveRecord(DefaultBroker clsdefaultbrokerObj)
        {
            DataAccessLayer dal = new DataAccessLayer();
            return dal.SaveclsdefaultbrokerEntry(clsdefaultbrokerObj);
        }

        public int updateRecord(DefaultBroker clsdefaultbrokerObj)
        {
            DataAccessLayer dal = new DataAccessLayer();
            return dal.updateclsdefaultbrokerEntry(clsdefaultbrokerObj);
        }

        public DefaultBroker getclsdefaultbrokerRecord(string ownerusername)
        {
            DataAccessLayer dal = new DataAccessLayer();
            return dal.getclsdefaultbrokerObj(ownerusername);
        }

        public int deleteRecord(string ownerusername)
        {
            DataAccessLayer dal = new DataAccessLayer();
            return dal.deleteclsdefaultbrokerEntry(ownerusername);
        }

        // -------------------- end of Class Members Code
        #endregion

    }
}
