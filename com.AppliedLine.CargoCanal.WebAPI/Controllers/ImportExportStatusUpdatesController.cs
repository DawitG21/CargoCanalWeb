using com.AppliedLine.CargoCanal.Models;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace com.AppliedLine.CargoCanal.WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ImportExportStatusUpdatesController : ApiController
    {
        // POST: api/ImportExportStatusUpdates/
        [HttpPost]
        public IHttpActionResult PostImportExportStatusUpdate(List<StatusUpdate> statusUpdates)
        {
            if (DataAccessLayer.SaveclsImportExportStatusUpdateEntry(statusUpdates, true) > 0)
            {
                // Return all statusupdates with current ImportExportID
                var statuses = DataAccessLayer.getclsImportExportStatusUpdateObj(statusUpdates[0].ImportExportID);
                return Ok(statuses);
            }

            return BadRequest();
        }

        // PUT: api/ImportExportStatusUpdates/
        [HttpPut]
        public IHttpActionResult PutImportExportStatusUpdate(List<StatusUpdate> importExportStatusUpdates)
        {
            if (DataAccessLayer.updateclsImportExportStatusUpdateEntry(importExportStatusUpdates) > 0)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
