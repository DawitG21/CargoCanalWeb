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
    public class ConfigController : ApiController
    {
        public IHttpActionResult GetStatsTime()
        {
            var configStatsTime = new DataAccessLayer().GetConfigStatsTime();

            if (configStatsTime == null) return BadRequest();

            return Ok(configStatsTime);
        }

        [HttpPost]
        public IHttpActionResult PostStatsTime(List<ConfigStatsTime> configStatsTimes)
        {
            int result = new DataAccessLayer().UpdateConfigStatsTime(configStatsTimes);

            if (result == 0) return BadRequest();

            return Ok();
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}