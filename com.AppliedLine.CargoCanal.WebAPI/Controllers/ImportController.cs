using com.AppliedLine.CargoCanal.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace com.AppliedLine.CargoCanal.WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ImportController : ApiController
    {
        //List<clsImport> allImports = DataAccessLayer.GetAllclsImportObj(0);
        List<Import> allImports = new List<Import>();

        // GET: api/Import
        public IEnumerable<Import> GetAllImports()
        {
            return allImports;
        }

        // GET: api/Import/5
        public IHttpActionResult GetImport(string companyId, long lastID)
        {
            List<Import> chunkImports = DataAccessLayer.GetAllclsImportObj(lastID);

            if (chunkImports.Count > 0) allImports.AddRange(chunkImports);

            var imports = (companyId == "0000000001") ?
                chunkImports : chunkImports.FindAll(i => i.ReceiverID == companyId);

            if (imports == null || imports.Count == 0) return NotFound();

            return Ok(imports);
        }

        // GET: api/Import/5
        public IHttpActionResult GetImportByBill(string companyId, string bol)
        {
            var imports = (companyId == "0000000001")
                ? allImports.FindAll(i => i.BillOfLading.ToLower().Contains(bol.ToLower()))
                : allImports.FindAll(i => i.ReceiverID == companyId && i.BillOfLading.ToLower().Contains(bol.ToLower()));

            if (imports == null) return NotFound();

            return Ok(imports);
        }

        // GET: 
        [ActionName("OnVoyageImports")]
        [HttpGet]
        public IHttpActionResult OnVoyageImports()
        {
            var onVoyageImports = new DataAccessLayer().OnVoyageImports();
            if (onVoyageImports == null) return NotFound();

            return Ok(onVoyageImports);
        }

        // GET: 
        [ActionName("DischargedImports")]
        [HttpGet]
        public IHttpActionResult DischargedImports()
        {
            var dischargedImports = new DataAccessLayer().DischargedImports();
            if (dischargedImports == null) return NotFound();

            return Ok(dischargedImports);
        }

        // GET: 
        [ActionName("CustomsClearedImports")]
        [HttpGet]
        public IHttpActionResult CustomsClearedImports()
        {
            var customsClearedImports = new DataAccessLayer().CustomsClearedImports();
            if (customsClearedImports == null) return NotFound();

            return Ok(customsClearedImports);
        }

        // GET: 
        [ActionName("DispatchedImports")]
        [HttpGet]
        public IHttpActionResult DispatchedImports()
        {
            var dispatchedImports = new DataAccessLayer().DispatchedImports();
            if (dispatchedImports == null) return NotFound();

            return Ok(dispatchedImports);
        }

        [HttpPost]
        public IHttpActionResult CustomsDischarged(string importExportID)
        {
            DateTime now = DateTime.Now;

            StatusUpdate cargoDischarged = new StatusUpdate()
            {
                ImportExportID = importExportID,
                StatusDate = now,
                DateInserted = now,
                StatusID = "57"
            };


            int result = DataAccessLayer.SaveclsImportExportStatusUpdateEntry(
                new List<StatusUpdate>() { cargoDischarged }, false);

            if (result == 1) return Ok();

            return BadRequest();
        }

        [HttpPost]
        public IHttpActionResult CustomsCleared(string importExportID)
        {
            DateTime now = DateTime.Now;

            StatusUpdate underCC = new StatusUpdate()
            {
                ImportExportID = importExportID,
                StatusDate = now,
                DateInserted = now,
                StatusID = "8"
            };

            StatusUpdate readyForLoading = new StatusUpdate()
            {
                ImportExportID = importExportID,
                StatusDate = now,
                DateInserted = now,
                StatusID = "10"
            };

            int result = DataAccessLayer.SaveclsImportExportStatusUpdateEntry(
                new List<StatusUpdate>() { underCC, readyForLoading }, false);

            if (result == 1) return Ok();

            return BadRequest();
        }

        [HttpPost]
        public IHttpActionResult CustomsDispatched(string importExportID)
        {
            DateTime now = DateTime.Now;

            StatusUpdate dispatched = new StatusUpdate()
            {
                ImportExportID = importExportID,
                StatusDate = now,
                DateInserted = now,
                StatusID = "11"
            };

            int result = DataAccessLayer.SaveclsImportExportStatusUpdateEntry(
                new List<StatusUpdate>() { dispatched }, false);

            if (result == 1) return Ok();

            return BadRequest();
        }


        [HttpPost]
        public IHttpActionResult CustomsInspected(string importExportID)
        {
            DateTime now = DateTime.Now;

            StatusUpdate dispatched = new StatusUpdate()
            {
                ImportExportID = importExportID,
                StatusDate = now,
                DateInserted = now,
                StatusID = "11"
            };

            StatusUpdate underCI = new StatusUpdate()
            {
                ImportExportID = importExportID,
                StatusDate = now,
                DateInserted = now,
                StatusID = "12"
            };


            int result = DataAccessLayer.SaveclsImportExportStatusUpdateEntry(
                new List<StatusUpdate>() { dispatched, underCI }, false);

            if (result == 1) return Ok();

            return BadRequest();
        }

        // POST: api/Import/
        [HttpPost]
        public IHttpActionResult PostImport(Import import)
        {
            import.ImpExpTypeID = "1";
            if (DataAccessLayer.SaveclsImportEntry(import) > 0)
            {
                // set the ImportExportID referrences on Item, Cost and ImportExportStatusUpdates
                import.Cost.ImportExportID = import.ImportExportID;
                var dateInserted = DateTime.Now;

                foreach (var importExportStatus in import.ImportExportStatuses)
                {
                    importExportStatus.ImportExportID = import.ImportExportID;
                    importExportStatus.DateInserted = dateInserted;
                }

                foreach (var item in import.Items)
                {
                    item.ImportExportID = import.ImportExportID;
                    item.ImpExpTypeID = import.ImpExpTypeID;
                }

                if (DataAccessLayer.SaveclsItemEntry(import.Items) > 0)
                {
                    if (DataAccessLayer.SaveclsImportExportStatusUpdateEntry(import.ImportExportStatuses, true) > 0
                        && DataAccessLayer.SaveclsCostEntry(import.Cost) > 0)
                    {
                        var dal = new DataAccessLayer();
                        var tracking = dal.createTracking(
                            import.BillOfLading,
                            import.ImportExportID,
                            import.ConsigneeID,
                            import.CustomerEmail);

                        if (dal.SaveTrackingEntry(tracking) > 0)
                        {
                            // get the new import
                            DataAccessLayer.GetImportExport(import, import.ImportExportID);
                            var importTracking = new ImportExportAndTracking()
                            {
                                Import = import,
                                Tracking = tracking
                            };

                            // add the import to the collection allImports
                            allImports.Insert(0, import);
                            // send an hybrid object with the recent import and tracking
                            return Ok(importTracking);
                        }
                    }
                }
            }

            return BadRequest();
        }

        // PUT: api/Import/
        [HttpPut]
        public IHttpActionResult PutImport(Import import)
        {
            if (DataAccessLayer.UpdateclsImportEntry(import) > 0)
            {
                return Ok();
            }

            return BadRequest();
        }

        // PUT: api/Import/
        [HttpPut]
        [ActionName("Completed")]
        public IHttpActionResult PutImportCompleted(string importExportID)
        {
            if (DataAccessLayer.UpdateclsImportExportCompleted(importExportID) > 0)
            {
                return Ok();
            }

            return BadRequest();
        }

        // PUT: api/Import/
        [HttpPut]
        [ActionName("Terminated")]
        public IHttpActionResult PutImportTerminated(string importExportID)
        {
            if (DataAccessLayer.UpdateclsImportExportTerminated(importExportID) > 0)
            {
                return Ok();
            }

            return BadRequest();
        }

        // DELETE: api/Import/
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteImport(string importExportID)
        {
            if (await DataAccessLayer.DeleteclsImportExportEntry(importExportID) > 0)
            {
                return Ok();
            }

            return BadRequest();
        }

        //added for CustomsBroker branch
        [HttpPost]
        [ActionName("AddCustomsBrokerForImport")]
        public IHttpActionResult AddCustomsBrokerForImport(Clearables clearableObj)
        {
            if (new Clearables().SaveRecord(clearableObj) == 1)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut]
        [ActionName("UpdateCustomsBrokerForImport")]
        public IHttpActionResult UpdateCustomsBrokerForImport(Clearables clearableObj)
        {
            if (new Clearables().SaveRecord(clearableObj) == 1)
            {
                return Ok();
            }

            return BadRequest();
        }

    }
}
