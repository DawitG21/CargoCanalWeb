using com.AppliedLine.CargoCanal.DAL;
using com.AppliedLine.CargoCanal.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace com.AppliedLine.CargoCanal.WebAPI.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ReportsController : ApiController
    {


        [HttpPost]
        public IHttpActionResult GenerateReport(ReportFilters filter)
        {
            try
            {
                DataAccessLayer dal = new DataAccessLayer();
                return Ok(dal.GenerateReport(filter));
            }
            catch(Exception e)
            {
                return InternalServerError(e);
            }
        }


        // TODO: Custom Reports --> delete this as it is taken from the old marilog for custom reports
        [HttpPost]
        [ActionName("Custom")]
        public IHttpActionResult PostCustomReports(CustomReportQuery crQuery)
        {
            var dalReportOld = new DataAccessLayerReports();
            var report = dalReportOld.GetCustomImportsReport(crQuery);

            if (report == null || report.Count == 0) return NotFound();

            return Ok(report);
        }

        // TODO: Custom Reports --> delete this as it is taken from the old marilog for custom reports
        [HttpGet]
        public IHttpActionResult Tables()
        {
            var dalReportOld = new DataAccessLayerReports();
            var tables = dalReportOld.GetTables();
            return Ok(tables);
        }

        // TODO: Custom Reports --> delete this as it is taken from the old marilog for custom reports
        [HttpGet]
        public IHttpActionResult Columns(string tableName)
        {
            var dalReportOld = new DataAccessLayerReports();
            var columns = dalReportOld.GetColumns(tableName);
            return Ok(columns);
        }

        // TODO: Custom Reports --> delete this as it is taken from the old marilog for custom reports
        [HttpGet]
        public IHttpActionResult ColumnHeaders()
        {
            var dalReportOld = new DataAccessLayerReports();
            var columns = dalReportOld.GetColumnHeaders();
            return Ok(columns);
        }

        // TODO: Custom Reports --> delete this as it is taken from the old marilog for custom reports
        public static List<Quarantined_MappingModel> mappingCollection = new List<Quarantined_MappingModel>();

        // TODO: Custom Reports --> delete this as it is taken from the old marilog for custom reports
        [HttpPost]
        [ActionName("AddMapping")]
        public IHttpActionResult PostMapping(Quarantined_MappingModel m)
        {
            DataAccessLayer dal = new DataAccessLayer();
            if (dal.SaveMappingModelWithParams(m) > 0) return Ok();

            return BadRequest();
        }

        // TODO: Custom Reports --> delete this as it is taken from the old marilog for custom reports
        [HttpPut]
        [ActionName("EditMapping")]
        public IHttpActionResult PutMapping(Quarantined_MappingModel m)
        {
            mappingCollection.Remove(mappingCollection.Find(x => x.MappingModelId == m.MappingModelId));
            mappingCollection.Add(m);

            DataAccessLayer dal = new DataAccessLayer();
            // update the database record
            if (dal.UpdateMappingModelWithParams(m) > 0) return Ok();

            return BadRequest();
        }

        // TODO: Custom Reports --> delete this as it is taken from the old marilog for custom reports
        [HttpGet]
        [ActionName("GetMappings")]
        public IHttpActionResult GetMappings()
        {
            var dal = new DataAccessLayer();

            mappingCollection = dal.GetMappingModelsWithParams();

            if (mappingCollection.Count == 0)
            {
                Quarantined_MappingModel mm = new Quarantined_MappingModel();
                mm.Name = "DEFAULT MAPPING";
                mm.ColumnMappings = new List<bool>();

                for (int i = 0; i < 84; i++)
                {
                    mm.ColumnMappings.Add(true);
                }

                mappingCollection.Add(mm);
            }

            return Ok(mappingCollection);
        }

        // TODO: Custom Reports --> delete this as it is taken from the old marilog for custom reports
        [HttpGet]
        [ActionName("GetBlankMapping")]
        public IHttpActionResult GetBlankMapping()
        {
            //returns a blank mapping with all fields shown for editing

            Quarantined_MappingModel mm = new Quarantined_MappingModel()
            {
                Name = "New Mapping",
                ColumnMappings = new List<bool>()
            };

            for (int i = 0; i < 84; i++)
            {
                mm.ColumnMappings.Add(true);
            }

            return Ok(mm);
        }

    }
}