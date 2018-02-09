using com.AppliedLine.CargoCanal.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.DAL
{
    public partial class DataAccessLayer
    {
        private List<Quarantined_MappingModel> FillMappingModelParamsForAllMappingModels(List<Quarantined_MappingModel> allMappingModels)
        {
            for (int i = 0; i <= allMappingModels.Count - 1; i++)
            {
                //clsmappingmodel mm = new clsmappingmodel();
                //mm = allMappingModels[i];
                //mm.mappingModelParams = dal.getclsmappingmodelparams(mm.mappingmodelid);

                allMappingModels[i].MappingModelParams = GetMappingModelParams(allMappingModels[i].MappingModelId);
            }

            return allMappingModels;
        }

        public int SaveMappingModelWithParams(Quarantined_MappingModel mappingModel)
        {
            //copy over column mappings list to a string of comma seperated true false elements
            mappingModel.ReadyColumMappingsForSave(mappingModel);

            //assign maxid + 1 from database
            mappingModel.MappingModelId = GetMappingModelNewID();

            //save the parameters of the clsmappingmodelobj before saving itself
            SaveMappingModelParams(mappingModel);

            return SaveMappingModel(mappingModel);
        }

        public int SaveMappingModelParams(Quarantined_MappingModel mappingModel)
        {
            if (mappingModel.MappingModelParams == null) return 1;

            int saveResult = 0;
            for (int i = 0; i < mappingModel.MappingModelParams.Count; i++)
            {
                Quarantined_MappingModelParam mmp = new Quarantined_MappingModelParam();
                mmp = mappingModel.MappingModelParams[i];

                //set indexposition and mappingmodelid foreign key
                mmp.IndexPosition = i;
                mmp.MappingModelId = mappingModel.MappingModelId;

                //save
                //saveResult += mmp.SaveRecord(mmp);
                saveResult += SaveMappingModelParam(mmp);
            }
            return saveResult;
        }

        public int UpdateMappingModelWithParams(Quarantined_MappingModel mappingModel)
        {
            mappingModel.ReadyColumMappingsForSave(mappingModel);
            return UpdateMappingModel(mappingModel);
        }

        public Quarantined_MappingModel GetclsmappingmodelRecord(string mappingmodelid)
        {
            return GetMappingModel(mappingmodelid);
        }

        public List<Quarantined_MappingModel> GetMappingModelsWithParams()
        {
            List<Quarantined_MappingModel> allMappingModels = new List<Quarantined_MappingModel>();
            allMappingModels = GetMappingModels();

            allMappingModels = FillMappingModelParamsForAllMappingModels(allMappingModels);
            return allMappingModels;
        }

        public int DeleteMappingModelWithParams(string mappingmodelid)
        {
            return DeleteMappingModel(mappingmodelid);
        }

        // TODO: Custom Report --> delete this and below as all methods are used by the old Marilog concept
        // You would need to refactor for future implementation of Custom Reports as that would take care of
        // SQL Injection as well as receive the entire query from the UI

        #region"MappingModel"

        private long GetMaxID(string dbTableName)
        {
            using (SqlConnection conx = Connection)
            {
                SqlCommand cmd = new SqlCommand("SELECT MAX(ID) FROM " + dbTableName, conx);
                var ID = cmd.ExecuteScalar();
                if (ID == DBNull.Value)
                {
                    return 1;
                }

                return Convert.ToInt64(ID) + 1;
            }
        }

        /// <summary>
        /// returns maxid + 1 of mappingmodel table
        /// </summary>
        /// <returns></returns>
        public long GetMappingModelNewID()
        {
            return GetMaxID("Quarantine_MappingModel");
        }

        /// <summary>
        /// Please call readyColumnMappings() method of the clsmappingmodel instance by using its SaveRecord instead of this method.
        /// </summary>
        /// <param name="mappingModel"></param>
        /// <returns></returns>
        public int SaveMappingModel(Quarantined_MappingModel mappingModel)
        {

            SqlConnection conx = Connection;
            SqlCommand cmd = new SqlCommand("INSERT INTO Quarantine_MappingModel VALUES('" +
                mappingModel.MappingModelId + "','" +
                mappingModel.Name + "','" +
                mappingModel.ColumnMappingsString + "')");
            cmd.Connection = conx;
            int val = cmd.ExecuteNonQuery();
            conx.Close();
            conx = null;
            return val;
        }

        public Quarantined_MappingModel GetMappingModel(string mappingmodelid)
        {
            Quarantined_MappingModel clsmappingmodelObj = null;

            SqlConnection conx = Connection;
            SqlCommand cmd = new SqlCommand("SELECT * FROM mappingmodel WHERE mappingmodelid = '" + mappingmodelid + "'");
            cmd.Connection = conx;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                clsmappingmodelObj = new Quarantined_MappingModel(reader.GetInt64(0),
                reader.GetInt64(1),
                reader.GetString(2),
                reader.GetString(3));
            }
            conx.Close();
            conx = null;
            return clsmappingmodelObj;
        }

        public List<Quarantined_MappingModel> GetMappingModels()
        {
            List<Quarantined_MappingModel> list = new List<Quarantined_MappingModel>();
            Quarantined_MappingModel mappingModel = null;

            SqlConnection conx = Connection;
            SqlCommand cmd = new SqlCommand("SELECT * FROM Quarantine_MappingModel order by id desc");
            cmd.Connection = conx;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                mappingModel = new Quarantined_MappingModel(reader.GetInt64(0),
                reader.GetInt64(1),
                reader.GetString(2),
                reader.GetString(3));
                list.Add(mappingModel);
            }
            conx.Close();
            conx = null;
            return list;
        }

        public int UpdateMappingModel(Quarantined_MappingModel mappingModel)
        {
            SqlConnection conx = Connection;
            SqlCommand cmd = new SqlCommand("UPDATE mappingmodel SET name = '" + mappingModel.Name
            + "', columnmappings = '" + mappingModel.ColumnMappingsString + "' WHERE mappingmodelid ='" +
            mappingModel.MappingModelId + "'");
            cmd.Connection = conx;
            int val = cmd.ExecuteNonQuery();
            conx.Close();
            conx = null;
            return val;
        }

        public int DeleteMappingModel(string mappingmodelid)
        {
            SqlConnection conx = Connection;
            SqlCommand cmd = new SqlCommand("DELETE FROM mappingmodel WHERE mappingmodelid = '" + mappingmodelid + "'");
            cmd.Connection = conx;
            int val = cmd.ExecuteNonQuery();
            conx.Close();
            return val;
        }
        #endregion

        #region "MappingModelParams"
        public int SaveMappingModelParam(Quarantined_MappingModelParam mappingModelParam)
        {
            SqlConnection conx = Connection;
            SqlCommand cmd = new SqlCommand("INSERT INTO Quarantine_MappingModelParams VALUES('" +
            mappingModelParam.TableName + "','" +
            mappingModelParam.ColumnName + "','" +
            mappingModelParam.Op + "','" +
            mappingModelParam.Val + "','" +
            mappingModelParam.Lop + "','" +
            mappingModelParam.IndexPosition + "','" +
            mappingModelParam.MappingModelId + "')");
            cmd.Connection = conx;
            int val = cmd.ExecuteNonQuery();
            conx.Close();
            conx = null;
            return val;
        }

        public Quarantined_MappingModelParam GetMappingModelParam(string id)
        {
            Quarantined_MappingModelParam mappingModelParam = null;
            SqlConnection conx = Connection;
            SqlCommand cmd = new SqlCommand("SELECT * FROM MappingModelParams WHERE id = '" + id + "'");
            cmd.Connection = conx;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                mappingModelParam = new Quarantined_MappingModelParam(reader.GetInt64(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                reader.GetString(4),
                reader.GetString(5),
                reader.GetInt32(6),
                reader.GetInt64(7));
            }
            conx.Close();
            conx = null;
            return mappingModelParam;
        }

        public List<Quarantined_MappingModelParam> GetMappingModelParams(long mappingModelId)
        {
            List<Quarantined_MappingModelParam> list = new List<Quarantined_MappingModelParam>();
            Quarantined_MappingModelParam mappingModelParam = null;
            SqlConnection conx = Connection;
            SqlCommand cmd = new SqlCommand("SELECT * FROM Quarantine_MappingModelParams WHERE MappingModelId = '" + mappingModelId + "'")
            {
                Connection = conx
            };

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                mappingModelParam = new Quarantined_MappingModelParam(reader.GetInt64(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                reader.GetString(4),
                reader.GetString(5),
                reader.GetInt32(6),
                reader.GetInt64(7));
                list.Add(mappingModelParam);
            }
            conx.Close();
            conx = null;
            return list;
        }

        public int UpdateMappingModelParam(Quarantined_MappingModelParam mappingModelParam)
        {
            SqlConnection conx = Connection;

            SqlCommand cmd = new SqlCommand("UPDATE mappingmodelparams SET tablename = '" + mappingModelParam.TableName
            + "', columname = '" + mappingModelParam.ColumnName
            + "', op = '" + mappingModelParam.Op
            + "', val = '" + mappingModelParam.Val
            + "', lop = '" + mappingModelParam.Lop
            + "', indexposition = '" + mappingModelParam.IndexPosition
            + "', mappingmodelid = '" + mappingModelParam.MappingModelId + "' WHERE id ='" + mappingModelParam.Id + "'");
            cmd.Connection = conx;
            int val = cmd.ExecuteNonQuery();
            conx.Close();
            conx = null;
            return val;
        }

        public int DeleteMappingModelParam(string id)
        {
            SqlConnection conx = Connection;
            SqlCommand cmd = new SqlCommand("DELETE FROM mappingmodelparams WHERE id = '" + id + "'");
            cmd.Connection = conx;
            int val = cmd.ExecuteNonQuery();
            conx.Close();
            return val;
        }
        #endregion

    }
}
