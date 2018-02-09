using com.AppliedLine.CargoCanal.Models;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace com.AppliedLine.CargoCanal.WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProblemUpdatesController : ApiController
    {
        List<ProblemUpdate> problemUpdates = DataAccessLayer.GetAllclsProblemUpdateObj();

        public IEnumerable<ProblemUpdate> GetAllProblemUpdates()
        {
            return problemUpdates;
        }

        // GET: api/ProblemUpdates/5
        public IHttpActionResult GetProblemUpdate(string importExportID)
        {
            var problemUpdates = DataAccessLayer.GetclsProblemUpdateObj(importExportID);
            if (problemUpdates == null)
            {
                return NotFound();
            }

            return Ok(problemUpdates);
        }

        // POST: api/ProblemUpdates
        [HttpPost]
        public IHttpActionResult PostProblemUpdate(ProblemUpdate problemUpdate)
        {
            if (DataAccessLayer.SaveclsProblemUpdateEntry(problemUpdate) > 0)
            {
                // Return all problems with current ImportExportID
                var problemUpdates = DataAccessLayer.GetclsProblemUpdateObj(problemUpdate.ImportExportID);
                return Ok(problemUpdates);
            }

            return BadRequest();
        }

        // PUT: api/ProblemUpdates
        [HttpPut]
        [ActionName("Close")]
        public IHttpActionResult PutProblemUpdate(string id)
        {
            if (DataAccessLayer.CloseProblemUpdateEntry(id) > 0)
            {
                // Return all problems with current ImportExportID
                var problem = DataAccessLayer.GetProblemUpdateById(id);
                var problemUpdates = DataAccessLayer.GetclsProblemUpdateObj(problem.ImportExportID);
                return Ok(problemUpdates);
            }

            return BadRequest();
        }

        // PUT: api/ProblemUpdates 
        [HttpPut]
        public IHttpActionResult PutProblemUpdate(ProblemUpdate problemUpdate)
        {
            if (DataAccessLayer.updateclsProblemUpdateEntry(problemUpdate) > 0)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
