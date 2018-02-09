using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{

    public class MappingModel
    {
        public List<MappingModelParams> mappingModelParams { get; set; }
        public List<bool> ColumnMappings { get; set; }
        public long id { get; set; }
        public string mappingmodelid { get; set; }
        public string name { get; set; }
        public string columnmappings { get; set; }

        public MappingModel()
        {
            //consturctor
        }

        //constructor with all prameters
        public MappingModel(long _id, string _mappingmodelid, string _name, string _columnmappings)
        {
            id = _id;
            mappingmodelid = _mappingmodelid;
            name = _name;
            columnmappings = _columnmappings;

            //split the true,false,true... string read from the sql and add it to a List<Boolean> object
            setColumMappingsArray(columnmappings);
        }

        /// <summary>
        /// copies the column mappings true false data from ColumnMappings List to a string and sets columnmappings property of this instance
        /// </summary>
        private void readyColumMappingsForSave(MappingModel mm)
        {
            if (mm.ColumnMappings == null) return;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= mm.ColumnMappings.Count - 1; i++)
            {
                if (i == ColumnMappings.Count - 1)
                {
                    sb.Append(mm.ColumnMappings[i].ToString());
                }
                else
                {
                    sb.Append(mm.ColumnMappings[i].ToString()).Append(",");
                }
            }

            mm.columnmappings = sb.ToString();
        }

        /// <summary>
        /// reads a comma seperated string of true,false data and adds them to a ColumnMappings list property of this instance
        /// </summary>
        /// <param name="columMappingsString"></param>
        private void setColumMappingsArray(string columMappingsString)
        {
            ColumnMappings = new List<bool>();
            string[] columMappingsArray;

            if (string.IsNullOrEmpty(columMappingsString)) {
                ColumnMappings.AddRange(new bool[] { true, true, true, true, true });
                return;
            }

            columMappingsArray = columMappingsString.Split(',');
            for (int i = 0; i <= columMappingsArray.GetUpperBound(0); i++)
            {
                ColumnMappings.Add(Convert.ToBoolean(columMappingsArray[i]));
            }
        }

        #region " Class Members Code Starts Here ..........."
        // Class Members code....................
        public int SaveRecord(MappingModel clsmappingmodelObj)
        {
            DataAccessLayer dal = new DataAccessLayer();

            //copy over column mappings list to a string of comma seperated true false elements
            readyColumMappingsForSave(clsmappingmodelObj);

            //assign maxid + 1 from database
            clsmappingmodelObj.mappingmodelid = dal.getclsmappingmodelNewID();

            //save the parameters of the clsmappingmodelobj before saving itself
            saveMappingModelParams(clsmappingmodelObj.mappingModelParams);

            return dal.SaveclsmappingmodelEntry(clsmappingmodelObj);
        }

        public int saveMappingModelParams(List<MappingModelParams> mappingModelParamsObjsList)
        {
            if (mappingModelParamsObjsList == null) return 1;

            int saveResult = 0;
            for (int i = 0; i < mappingModelParamsObjsList.Count; i++)
            {
                MappingModelParams mmp = new MappingModelParams();
                mmp = mappingModelParamsObjsList[i];

                //set indexposition and mappingmodelid foreign key
                mmp.indexposition = Convert.ToString(i);
                mmp.mappingmodelid = mappingmodelid;

                //save
                saveResult += mmp.SaveRecord(mmp);
            }
            return saveResult;
        }

        public int updateRecord(MappingModel clsmappingmodelObj)
        {
            readyColumMappingsForSave(clsmappingmodelObj);

            DataAccessLayer dal = new DataAccessLayer();
            return dal.updateclsmappingmodelEntry(clsmappingmodelObj);
        }

        public MappingModel getclsmappingmodelRecord(string mappingmodelid)
        {
            DataAccessLayer dal = new DataAccessLayer();
            return dal.getclsmappingmodelObj(mappingmodelid);
        }

        public List<MappingModel> getclsmappingmodels()
        {
            List<MappingModel> AllMappingModels = new List<MappingModel>();

            DataAccessLayer dal = new DataAccessLayer();
            AllMappingModels = dal.getclsmappingmodels();

            AllMappingModels = fillMappingModelParamsForAllMappingModels(AllMappingModels);
            return AllMappingModels;
        }

        private List<MappingModel> fillMappingModelParamsForAllMappingModels(List<MappingModel> allMappingModels)
        {
            DataAccessLayer dal = new DataAccessLayer();

            for (int i = 0; i <= allMappingModels.Count - 1; i++)
            {
                //clsmappingmodel mm = new clsmappingmodel();
                //mm = allMappingModels[i];
                //mm.mappingModelParams = dal.getclsmappingmodelparams(mm.mappingmodelid);

                allMappingModels[i].mappingModelParams =
                    dal.getclsmappingmodelparams(allMappingModels[i].mappingmodelid);
            }

            return allMappingModels;
        }

        public int deleteRecord(string mappingmodelid)
        {
            DataAccessLayer dal = new DataAccessLayer();
            return dal.deleteclsmappingmodelEntry(mappingmodelid);
        }

        // -------------------- end of Class Members Code
        #endregion

    }
}
