using com.AppliedLine.CargoCanal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using com.AppliedLine.CargoCanal.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace com.AppliedLine.CargoCanal.WebAPI.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ValuesController : ApiController
    {
        //private HttpResponseMessage cachedValues(object o)
        //{
        //    var response = Request.CreateResponse(HttpStatusCode.OK);
        //    response.Content = new StringContent(JsonConvert.SerializeObject(o));
        //    response.Headers.CacheControl = new CacheControlHeaderValue();
        //    response.Headers.CacheControl.MaxAge = new TimeSpan(1, 0, 0);
        //    response.Headers.CacheControl.Public = true;
        //    return response;
        //}
        //public HttpResponseMessage GetVessels()
        //{
        //    return cachedValues(DataAccessLayer.getAllclsVesselObj());
        //}

        private readonly DateTime serverDateTime;
        private readonly DataAccessLayer dal;
        public ValuesController()
        {
            serverDateTime = DateTime.UtcNow;
            dal = new DataAccessLayer();
        }

        [HttpGet]
        public IHttpActionResult GetServerTime()
        {
            return Ok(serverDateTime);
        }

        [HttpGet]
        public async Task<IHttpActionResult> QueryVessels(string term)
        {
            DataAccessLayer dal = new DataAccessLayer();
            var vessels = await dal.SelectVessels(term);

            term = term.ToLower();
            var distinctVessels = vessels.Distinct().OrderBy(v => v.VesselName)
                .Select(a => new { value = a.VesselName, label = a.VesselName });
            return Json(distinctVessels);
        }


        [HttpGet]
        public async Task<IHttpActionResult> QueryPreviousConsignee(string term, string extraparams)
        {
            DataAccessLayer dal = new DataAccessLayer();
            List<Company> companies = await dal.SelectCompanyPreviousConsignee(extraparams);

            term = term.ToLower();
            var distinctCompanies = companies.Distinct().OrderBy(c => c.CompanyName)
                .Where(c => c.TIN.ToLower().Contains(term) || c.CompanyName.ToLower().Contains(term))
                .Select(a => new { value = a.TIN, label = $"{a.CompanyName} - {a.TIN}" });
            return Json(distinctCompanies);
        }

        [HttpGet]
        public async Task<IHttpActionResult> ValidateTin(string tin)
        {
            DataAccessLayer dal = new DataAccessLayer();
            Company company = await dal.SelectCompanyByTin(tin);

            if (company == null) return NotFound();
            return Ok(true);
        }

        [HttpGet]
        public IHttpActionResult QueryTransitor(string term)
        {
            DataAccessLayer dal = new DataAccessLayer();
            var companies = dal.SelectCompaniesLikeTinOrName(term).OrderBy(c => c.CompanyName);
            var colCompanies = companies.Select(a => new { value = a.TIN, label = $"{a.CompanyName} - {a.TIN}" });
            return Json(colCompanies);
        }

        [HttpGet]
        public IHttpActionResult QueryCargo(string term)
        {
            DataAccessLayer dal = new DataAccessLayer();
            var cargos = dal.SelectCargos().OrderBy(c => c.CargoName).Where(c => c.CargoName.ToLower().Contains(term.ToLower()));
            var colCargos = cargos.Select(s => new { value = s.CargoName, label = s.CargoName });
            return Json(colCargos);
        }

        [HttpGet]
        public IHttpActionResult QueryCountry(string term)
        {
            DataAccessLayer dal = new DataAccessLayer();
            var countries = dal.SelectCountries().OrderBy(c => c.Name).Where(c => c.Name.ToLower().Contains(term.ToLower()));
            var colCountries = countries.Select(c => new { value = c.Name, label = c.Name });
            return Json(colCountries);
        }

        [HttpGet]
        public IHttpActionResult QueryProblem(string term)
        {
            DataAccessLayer dal = new DataAccessLayer();
            var problems = dal.SelectProblems().OrderBy(p => p.ProblemName).Where(c => c.ProblemName.ToLower().Contains(term.ToLower()));
            var colProblems = problems.Select(p => new { value = p.ProblemName, label = p.ProblemName });
            return Json(colProblems);
        }


        public IHttpActionResult GetCargo(long? id)
        {
            DataAccessLayer dal = new DataAccessLayer();
            if (id != null)
                return Ok(dal.SelectCargo((long)id));

            return Ok(dal.SelectCargos());
        }

        public IHttpActionResult GetCarrier(long? id)
        {
            DataAccessLayer dal = new DataAccessLayer();
            if (id != null)
                return Ok(dal.SelectCarrier((long)id));

            return Ok(dal.SelectCarriers());
        }

        [HttpPost]
        public IHttpActionResult PostCarrier(Carrier carrier)
        {
            try
            {
                if (dal.InsertCarrier(carrier))
                {
                    return Ok(carrier);
                }

                return BadRequest();
            }
            catch (SqlException e)
            {
                if (e.Message.Contains("UNIQUE KEY")) return Conflict();
                return InternalServerError();
            }
        }

        public IHttpActionResult GetCompanyType(long? id)
        {
            DataAccessLayer dal = new DataAccessLayer();
            if (id != null)
                return Ok(dal.SelectCompanyType((long)id));

            return Ok(dal.SelectCompanyTypes());
        }

        public IHttpActionResult GetCountry(long? id)
        {
            DataAccessLayer dal = new DataAccessLayer();
            if (id != null)
                return Ok(dal.SelectCountry((long)id));

            return Ok(dal.SelectCountries());
        }

        public IHttpActionResult GetImpExpType()
        {
            DataAccessLayer dal = new DataAccessLayer();
            return Ok(dal.SelectImpExpTypes());
        }

        public IHttpActionResult GetImportExportReason()
        {
            DataAccessLayer dal = new DataAccessLayer();
            return Ok(dal.SelectImportExportReasons());
        }

        public IHttpActionResult GetIncoTerm(long? id)
        {
            DataAccessLayer dal = new DataAccessLayer();
            if (id != null)
                return Ok(dal.SelectIncoTerm((long)id));

            return Ok(dal.SelectIncoTerms());
        }

        public IHttpActionResult GetLocation(long id)
        {
            DataAccessLayer dal = new DataAccessLayer();
            return Ok(dal.SelectLocation(id));
        }

        public IHttpActionResult GetLocations(long id)
        {
            DataAccessLayer dal = new DataAccessLayer();
            return Ok(dal.SelectLocations(id));
        }

        [HttpPost]
        public IHttpActionResult PostLocation(Location location)
        {
            try
            {
                if (dal.InsertLocation(location))
                {
                    return Ok(location);
                }

                return BadRequest();
            }
            catch (SqlException e)
            {
                if (e.Message.Contains("UNIQUE KEY")) return Conflict();
                return InternalServerError();
            }
        }

        public IHttpActionResult GetModeOfTransport(long? id)
        {
            DataAccessLayer dal = new DataAccessLayer();
            if (id != null)
                return Ok(dal.SelectModeOfTransport((long)id));

            return Ok(dal.SelectModeOfTransports());
        }

        public IHttpActionResult GetPort(long id)
        {
            DataAccessLayer dal = new DataAccessLayer();
            return Ok(dal.SelectPort(id));
        }

        public IHttpActionResult GetPorts(long id)
        {
            DataAccessLayer dal = new DataAccessLayer();
            return Ok(dal.SelectPorts(id));
        }

        [HttpPost]
        public IHttpActionResult PostPort(Port port)
        {
            try
            {
                if (dal.InsertPort(port))
                {
                    return Ok(port);
                }

                return BadRequest();
            }
            catch (SqlException e)
            {
                if (e.Message.Contains("UNIQUE KEY")) return Conflict();
                return InternalServerError();
            }
        }

        public IHttpActionResult GetProblem(long? id)
        {
            DataAccessLayer dal = new DataAccessLayer();
            if (id != null)
                return Ok(dal.SelectProblem((long)id));

            return Ok(dal.SelectProblems());
        }

        public IHttpActionResult GetStatus(long? id)
        {
            DataAccessLayer dal = new DataAccessLayer();
            if (id != null)
                return Ok(dal.SelectStatus((long)id));

            return Ok(dal.SelectStatuses());
        }

        public IHttpActionResult GetStuffMode(long? id)
        {
            DataAccessLayer dal = new DataAccessLayer();
            if (id != null)
                return Ok(dal.SelectStuffMode((long)id));

            return Ok(dal.SelectStuffModes());
        }

        public IHttpActionResult GetSubCargo(long id)
        {
            DataAccessLayer dal = new DataAccessLayer();
            return Ok(dal.SelectSubCargo(id));
        }

        public IHttpActionResult GetSubCargos(long id)
        {
            DataAccessLayer dal = new DataAccessLayer();
            return Ok(dal.SelectSubCargos(id));
        }

        public IHttpActionResult GetTarrif(long? id)
        {
            DataAccessLayer dal = new DataAccessLayer();
            if (id != null)
                return Ok(dal.SelectTarrif((long)id));

            return Ok(dal.SelectTarrifs());
        }

        public IHttpActionResult GetUnit(long? id)
        {
            DataAccessLayer dal = new DataAccessLayer();
            if (id != null)
                return Ok(dal.SelectUnit((long)id));

            return Ok(dal.SelectUnits());
        }

        public IHttpActionResult GetVessel(long id)
        {
            DataAccessLayer dal = new DataAccessLayer();
            return Ok(dal.SelectVessel(id));
        }

        public IHttpActionResult GetVessels(long id)
        {
            DataAccessLayer dal = new DataAccessLayer();
            return Ok(dal.SelectVessels(id));
        }

        [HttpPost]
        public IHttpActionResult PostVessel(Vessel vessel)
        {
            try
            {
                if (dal.InsertVessel(vessel))
                {
                    return Ok(vessel);
                }

                return BadRequest();
            }
            catch (SqlException e)
            {
                if (e.Message.Contains("UNIQUE KEY")) return Conflict();
                return InternalServerError();
            }
        }
    }
}
