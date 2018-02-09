using com.AppliedLine.CargoCanal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace com.AppliedLine.CargoCanal.WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ItemsController : ApiController
    {
        // POST: api/Items/
        [HttpPost]
        public IHttpActionResult PostItem(List<Item> items)
        {
            if (DataAccessLayer.SaveclsItemEntry(items) > 0)
            {
                return Ok();
            }

            return BadRequest();
        }

        // PUT: api/Items/
        [HttpPut]
        public IHttpActionResult PutItem(List<Item> items)
        {
            if (DataAccessLayer.updateclsItemEntry(items) > 0)
            {
                return Ok();
            }

            return BadRequest();
        }

        // DELETE: api/Items/
        [HttpDelete]
        public IHttpActionResult DeleteItem(string itemID)
        {
            if (DataAccessLayer.deleteclsItemEntry(itemID) > 0)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
