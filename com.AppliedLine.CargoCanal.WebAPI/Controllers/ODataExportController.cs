using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using com.AppliedLine.CargoCanal.Models;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.OData.Routing;
using com.AppliedLine.CargoCanal.DAL;
using System.Web;

namespace com.AppliedLine.CargoCanal.WebAPI.Controllers
{
    public class ODataExportController : ODataController
    {
        private readonly BizLogic biz;
        public ODataExportController()
        {
            biz = new BizLogic(new DataAccessLayer(), null);
        }

        [HttpPost]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All, PageSize = 20)]
        // post http://localhost:49931/odata/ODataImport(key)/GetImports?id=3&$expand=LC
        public IHttpActionResult GetExports([FromODataUri]int key, ODataActionParameters parameters)
        {
            int dbPageSize = 25; // TODO: make this a global const; value has to be higher than PageSize
            int skip = 0;
            
            // set db rows to skip
            if (parameters != null) int.TryParse(parameters["skip"].ToString(), out skip);
            
            var exports = biz.GetExportsPendingView(Convert.ToInt32(ImpExpTypeEnum.Export), key, skip, dbPageSize);

            if (exports == null || exports.Count == 0) return NotFound();
            return Ok(exports.AsQueryable());
        }

        [HttpPost]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All, PageSize = 20)]
        // post http://localhost:49931/odata/ODataImport(key)/GetImports?id=3&$expand=LC
        public IHttpActionResult SearchExports([FromODataUri]int key, ODataActionParameters parameters)
        {
            int dbPageSize = 25; // TODO: make this a global const; value has to be higher than PageSize
            int skip = 0;
            SearchBill search = new SearchBill();

            // set db rows to skip
            if (parameters != null)
            {
                int.TryParse(parameters["skip"].ToString(), out skip);
                search.SearchText = parameters["searchText"].ToString();
                search.Token = Guid.Parse(parameters["token"].ToString());
            }
            
            var exports = biz.SearchExportsView(Convert.ToInt32(ImpExpTypeEnum.Export), key, skip, dbPageSize, search);

            if (exports == null || exports.Count == 0) return NotFound();
            return Ok(exports.AsQueryable());
        }

    }
}