using com.AppliedLine.CargoCanal.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace com.AppliedLine.CargoCanal.WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ExportController : ApiController
    {
        List<Export> allExports = new List<Export>();

        // GET: api/ImportExport
        public IEnumerable<Export> GetAllExports()
        {
            return allExports;
        }

        // GET: api/Export/5
        public IHttpActionResult GetExport(string companyId, long lastID)
        {
            List<Export> chunckExports = DataAccessLayer.GetAllclsExportObj(lastID);

            if (chunckExports.Count > 0) allExports.AddRange(chunckExports);

            var exports = (companyId == "0000000001") ? chunckExports : chunckExports.FindAll(e => e.ReceiverID == companyId);

            if (exports == null || exports.Count == 0) return NotFound();

            return Ok(exports);
        }

        // GET: api/Export/5
        public IHttpActionResult GetExportBySin(string companyId, string sin)
        {
            var exports = (companyId == "0000000001")
                ? allExports.FindAll(e => e.ShippingInstructionNo.ToLower().Contains(sin.ToLower()))
                : allExports.FindAll(e => e.ReceiverID == companyId && e.ShippingInstructionNo.ToLower().Contains(sin.ToLower()));

            if (exports == null) return NotFound();

            return Ok(exports);
        }

        // POST: api/Export/
        [HttpPost]
        public IHttpActionResult PostExport(Export export)
        {
            export.ImpExpTypeID = "2";
            if (DataAccessLayer.SaveclsExportEntry(export) > 0)
            {
                // set the ImportExportID referrences on Item, Cost and ImportExportStatusUpdates
                export.Cost.ImportExportID = export.ImportExportID;
                var dateInserted = DateTime.Now;

                foreach (var importExportStatus in export.ImportExportStatuses)
                {
                    importExportStatus.ImportExportID = export.ImportExportID;
                    importExportStatus.DateInserted = dateInserted;
                }

                foreach (var item in export.Items)
                {
                    item.ImportExportID = export.ImportExportID;
                    item.ImpExpTypeID = export.ImpExpTypeID;
                }

                if (DataAccessLayer.SaveclsItemEntry(export.Items) > 0)
                {
                    if (DataAccessLayer.SaveclsImportExportStatusUpdateEntry(export.ImportExportStatuses, true) > 0
                        && DataAccessLayer.SaveclsCostEntry(export.Cost) > 0)
                    {
                        var dal = new DataAccessLayer();
                        var tracking = dal.createTracking(
                            export.ShippingInstructionNo,
                            export.ImportExportID,
                            export.ConsigneeID,
                            export.CustomerEmail);

                        if (dal.SaveTrackingEntry(tracking) > 0)
                        {
                            // get the new export
                            DataAccessLayer.GetImportExport(export, export.ImportExportID);
                            var exportTracking = new ImportExportAndTracking()
                            {
                                Export = export,
                                Tracking = tracking
                            };

                            // add 'export' to the collection 'allExports'
                            allExports.Insert(0, export);
                            // send an hybrid object with the recent import and tracking
                            return Ok(exportTracking);
                        }
                    }
                }
            }

            return BadRequest();
        }

        // PUT: api/Export/
        [HttpPut]
        public IHttpActionResult PutExport(Export export)
        {
            if (DataAccessLayer.UpdateclsExportEntry(export) > 0)
            {
                return Ok();
            }

            return BadRequest();
        }

        // PUT: api/Export/
        [HttpPut]
        [ActionName("Completed")]
        public IHttpActionResult PutExportCompleted(string importExportID)
        {
            if (DataAccessLayer.UpdateclsImportExportCompleted(importExportID) > 0)
            {
                return Ok();
            }

            return BadRequest();
        }

        // PUT: api/Export/
        [HttpPut]
        [ActionName("Terminated")]
        public IHttpActionResult PutExportTerminated(string importExportID)
        {
            if (DataAccessLayer.UpdateclsImportExportTerminated(importExportID) > 0)
            {
                return Ok();
            }

            return BadRequest();
        }

        // DELETE: api/Export/
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteExport(string importExportID)
        {
            if (await DataAccessLayer.DeleteclsImportExportEntry(importExportID) > 0)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
