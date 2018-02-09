using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{

    public class Clearables
    {
        public Clearables()
        {
            //consturctor
            importexportid = "";
            ownerusername = "";
            customsbrokerusername = "";
            cleared = "NO";
        }

        //constructor with all prameters
        public Clearables(long _id, string _importexportid, string _ownerusername, string _customsbrokerusername, string _cleared)
        {
            id = _id;
            importexportid = _importexportid;
            ownerusername = _ownerusername;
            customsbrokerusername = _customsbrokerusername;
            cleared = _cleared;
        }

        public long id { get; set; }

        public string importexportid { get; set; }

        public string ownerusername { get; set; }

        public string customsbrokerusername { get; set; }

        public string cleared { get; set; }


        #region " Class Members Code Starts Here ..........."
        // Class Members code....................
        public int SaveRecord(Clearables clsclearablesObj)
        {
            DataAccessLayer dal = new DataAccessLayer();
            return dal.SaveclsClearablesEntry(clsclearablesObj);
        }

        public int UpdateRecord(Clearables clsclearablesObj)
        {
            DataAccessLayer dal = new DataAccessLayer();
            return dal.updateclsClearablesEntry(clsclearablesObj);
        }

        public Clearables GetClsClearablesRecord(string importExportID)
        {
            DataAccessLayer dal = new DataAccessLayer();
            return dal.getclsClearablesObj(importExportID);
        }
        public List<Clearables> GetClsClearablesRecordsForBroker(string customsbrokerusername)
        {
            DataAccessLayer dal = new DataAccessLayer();
            return dal.getclsClearablesObjForCustomsBroker(customsbrokerusername);
        }
        public int DeleteRecord(string importExportID)
        {
            DataAccessLayer dal = new DataAccessLayer();
            return dal.deleteclsClearablesEntry(importExportID);
        }

        // -------------------- end of Class Members Code
        #endregion

    }
}
