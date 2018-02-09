using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class Quarantined_MappingModelParam
    {
        public Quarantined_MappingModelParam()
        {
            //consturctor
            TableName = "";
            ColumnName = "";
            Op = "";
            Val = "";
            Lop = "";
        }

        //constructor with all prameters
        public Quarantined_MappingModelParam(long _id, string _tablename, string _columname, string _op, string _val,
            string _lop, int _indexposition, long _mappingmodelid)
        {
            Id = _id;
            TableName = _tablename;
            ColumnName = _columname;
            Op = _op;
            Val = _val;
            Lop = _lop;
            IndexPosition = _indexposition;
            MappingModelId = _mappingmodelid;
        }

        public long Id { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string Op { get; set; }
        public string Val { get; set; }
        public string Lop { get; set; }
        public int IndexPosition { get; set; }
        public long MappingModelId { get; set; }


    }

}
