using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{

    public class MappingModelParams
    {
        public MappingModelParams()
        {
            //consturctor

            tablename = "";
            columnname = "";
            op = "";
            val = "";
            lop = "";
            indexposition = "";
            mappingmodelid = "";
        }
        //constructor with all prameters
        public MappingModelParams(long _id, string _tablename, string _columname, string _op, string _val, string _lop, string _indexposition, string _mappingmodelid)
        {
            id = _id;
            tablename = _tablename;
            columnname = _columname;
            op = _op;
            val = _val;
            lop = _lop;
            indexposition = _indexposition;
            mappingmodelid = _mappingmodelid;
        }

        public long id { get; set; }
        public string tablename { get; set; }
        public string columnname { get; set; }
        public string op { get; set; }
        public string val { get; set; }
        public string lop { get; set; }
        public string indexposition { get; set; }
        public string mappingmodelid { get; set; }

        #region " Class Members Code Starts Here ..........."
        // Class Members code....................
        public int SaveRecord(MappingModelParams clsmappingmodelparamsObj)
        {
            DataAccessLayer dal = new DataAccessLayer();
            return dal.SaveclsmappingmodelparamsEntry(clsmappingmodelparamsObj);
        }

        public int updateRecord(MappingModelParams clsmappingmodelparamsObj)
        {
            DataAccessLayer dal = new DataAccessLayer();
            return dal.updateclsmappingmodelparamsEntry(clsmappingmodelparamsObj);
        }

        public MappingModelParams getclsmappingmodelparamsRecord(string id)
        {
            DataAccessLayer dal = new DataAccessLayer();
            return dal.getclsmappingmodelparamsObj(id);
        }
        public List<MappingModelParams> getclsmappingmodelparams(string mappingmodelid)
        {
            DataAccessLayer dal = new DataAccessLayer();
            return dal.getclsmappingmodelparams(mappingmodelid);
        }
        public int deleteRecord(string id)
        {
            DataAccessLayer dal = new DataAccessLayer();
            return dal.deleteclsmappingmodelparamsEntry(id);
        }

        // -------------------- end of Class Members Code
        #endregion

    }
}
