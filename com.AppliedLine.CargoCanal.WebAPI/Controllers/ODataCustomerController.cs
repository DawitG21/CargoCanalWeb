using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using com.AppliedLine.CargoCanal.Models;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using com.AppliedLine.CargoCanal.DAL;

namespace com.AppliedLine.CargoCanal.WebAPI.Controllers
{
    public class ODataCustomerController : ODataController
    {
        private readonly BizLogic biz;
        public ODataCustomerController()
        {
            biz = new BizLogic(new DataAccessLayer(), null);
        }

        [HttpPost]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All, PageSize = 20)]
        // post http://localhost:49931/odata/ODataImport(key)/GetImports?id=3&$expand=LC
        public IHttpActionResult SearchImportExport([FromODataUri]int key, ODataActionParameters parameters)
        {
            int dbPageSize = 20; // TODO: make this a global const
            int skip = 0;
            SearchBill search = new SearchBill();

            // set db rows to skip
            if (parameters != null)
            {
                int.TryParse(parameters["skip"].ToString(), out skip);
                search.SearchText = parameters["searchText"].ToString();
                search.Token = Guid.Parse(parameters["token"].ToString());
            }

            List<CustomerImportExportView> customerImportExport = biz.GetImportWithStatusesByTinLc(key, skip, dbPageSize, search);

            if (customerImportExport == null || customerImportExport.Count == 0) return NotFound();
            return Ok(customerImportExport.AsQueryable());
        }
    }
}