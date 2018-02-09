using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using com.AppliedLine.CargoCanal.Models;
namespace com.AppliedLine.CargoCanal.WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CustomsBrokerController : ApiController
    {
        // GET: api/Import/5

        [HttpGet]
        [ActionName("Get")]
        public IHttpActionResult GetClearable(string importExportID)
        {
            Clearables clearableObj = new Clearables();
            clearableObj = clearableObj.GetClsClearablesRecord(importExportID);

            return Ok(new Clearables());
        }

        [HttpPost]
        [ActionName("CustomsBrokerForImportExport")]
        public IHttpActionResult ClearableRecordForImportExport(Import importObj)
        {
            Clearables clearableObj = new Clearables();
            clearableObj = clearableObj.GetClsClearablesRecord(importObj.ImportExportID);

            return Ok(clearableObj);
        }

        [HttpPost]
        [ActionName("UpdateCustomsBrokerForImportExport")]
        public IHttpActionResult UpdateClearableRecordForImportExport(Clearables clearableObj)
        {
            return Ok(clearableObj.UpdateRecord(clearableObj));
        }
    }
}
