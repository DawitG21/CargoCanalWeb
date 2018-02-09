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
    public class TrackingController : ApiController
    {
        public IHttpActionResult GetTrackingsByNumber(string trackingNumber)
        {
            var dal = new DataAccessLayer();
            var trackings = dal.GetCustomerTrackings(trackingNumber, false);

            if (trackings == null) return NotFound();

            return Ok(trackings);
        }

        public IHttpActionResult GetTrackingsByEmail(string email)
        {
            var dal = new DataAccessLayer();
            var trackings = dal.GetCustomerTrackings(email, true);

            if (trackings == null) return NotFound();

            return Ok(trackings); 
        }

        public IHttpActionResult GetAgentTrackings(string companyID)
        {
            var dal = new DataAccessLayer();
            var trackings = dal.GetTrackingsByCompany(companyID);

            if (trackings == null) return NotFound();

            return Ok(trackings);
        }

        [HttpPut]
        public IHttpActionResult PutTrackingCustomer(Tracking tracking)
        {
            var dal = new DataAccessLayer();

            if (dal.UpdateTrackingEntry(tracking) == 0) return BadRequest();

            return Ok();
        }
    }
}
