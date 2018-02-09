using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public class Quarantined_MappingModel
    {
        public List<Quarantined_MappingModelParam> MappingModelParams { get; set; }
        public List<bool> ColumnMappings { get; set; }
        public long Id { get; set; }
        public long MappingModelId { get; set; }
        public string Name { get; set; }
        public string ColumnMappingsString { get; set; }
        
        public Quarantined_MappingModel() { }

        //constructor with all prameters
        public Quarantined_MappingModel(long _id, long _mappingmodelid, string _name, string _columnmappings)
        {
            Id = _id;
            MappingModelId = _mappingmodelid;
            Name = _name;
            ColumnMappingsString = _columnmappings;

            //split the true,false,true... string read from the sql and add it to a List<Boolean> object
            SetColumMappingsArray(ColumnMappingsString);
        }

        /// <summary>
        /// copies the column mappings true false data from ColumnMappings List to a string and sets columnmappings property of this instance
        /// </summary>
        public void ReadyColumMappingsForSave(Quarantined_MappingModel mm)
        {
            if (mm.ColumnMappings == null) return;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= mm.ColumnMappings.Count - 1; i++)
            {
                if (i == mm.ColumnMappings.Count - 1)
                {
                    sb.Append(mm.ColumnMappings[i].ToString());
                }
                else
                {
                    sb.Append(mm.ColumnMappings[i].ToString()).Append(",");
                }
            }

            mm.ColumnMappingsString = sb.ToString();
        }

        /// <summary>
        /// reads a comma seperated string of true,false data and adds them to a ColumnMappings 
        /// list property of this instance
        /// </summary>
        /// <param name="columMappingsString"></param>
        public void SetColumMappingsArray(string columMappingsString)
        {
            ColumnMappings = new List<bool>();
            string[] columMappingsArray;

            if (string.IsNullOrEmpty(columMappingsString))
            {
                ColumnMappings.AddRange(new bool[] { true, true, true, true, true });
                return;
            }

            columMappingsArray = columMappingsString.Split(',');
            for (int i = 0; i <= columMappingsArray.GetUpperBound(0); i++)
            {
                ColumnMappings.Add(Convert.ToBoolean(columMappingsArray[i]));
            }
        }
        
    }

}
