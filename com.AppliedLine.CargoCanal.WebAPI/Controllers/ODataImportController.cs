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
    //[ODataRoutePrefix("OnDuct")]
    public class Duct
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }


    public class ODataImportController : ODataController
    {
        CargoCanalDBEntities dbEntities = new CargoCanalDBEntities();
        List<Duct> ducts = new List<Duct>();
        private readonly BizLogic biz;
        public ODataImportController()
        {
            biz = new BizLogic(new DataAccessLayer(), null);
        }

        //[ODataRoute("OnDuct")]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All, PageSize = 1)]
        public IHttpActionResult Get()
        {
            ducts = new List<Duct>(){
                new Duct{ID = 1, ProductName = "Shampoo", Price = 23.5M},
                new Duct{ID = 2, ProductName = "Polish", Price = 13.5M},
                new Duct{ID = 3, ProductName = "Toothpase", Price = 12M}
            };

            return Ok(ducts.AsQueryable());
        }
        

        [ODataRoute("GetMyThings(MyId={mine})")]
        public IHttpActionResult GetMyThings([FromODataUri] int mine)
        {
            return Ok();
        }


        // controller name should reflect entity name in WebApiConfig
        // [ODataRoute("({key})")]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All, PageSize = 1)]
        public IQueryable<sp_ImportExport_Select_Company_Pending_Result> Get([FromODataUri]long key)
        {
            var rval = RequestContext.RouteData.Values;
            var result = dbEntities.sp_ImportExport_Select_Company_Pending(key).AsQueryable();
            return result;
        }


        [HttpPost]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All, PageSize = 20)]
        // post http://localhost:49931/odata/ODataImport(key)/GetImports?id=3&$expand=LC
        public IHttpActionResult GetImports([FromODataUri]int key, ODataActionParameters parameters)
        {
            int dbPageSize = 25; // TODO: make this a global const; value has to be higher than PageSize
            int skip = 0;
            
            // set db rows to skip
            if (parameters != null) int.TryParse(parameters["skip"].ToString(), out skip);

            var imports = biz.GetImportsPendingView(Convert.ToInt32(ImpExpTypeEnum.Import), key, skip, dbPageSize);

            if (imports == null || imports.Count == 0) return NotFound();
            return Ok(imports.AsQueryable());
        }

        [HttpPost]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All, PageSize = 20)]
        // post http://localhost:49931/odata/ODataImport(key)/GetImports?id=3&$expand=LC
        public IHttpActionResult SearchImports([FromODataUri]int key, ODataActionParameters parameters)
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

            var imports = biz.SearchImportsView(Convert.ToInt32(ImpExpTypeEnum.Import), key, skip, dbPageSize, search);

            if (imports == null || imports.Count == 0) return NotFound();
            return Ok(imports.AsQueryable());
        }
    }
}