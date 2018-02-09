using com.AppliedLine.CargoCanal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace com.AppliedLine.CargoCanal.WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PersonController : ApiController
    {
        [HttpPost]
        [ActionName("AddPerson")]
        public IHttpActionResult AddPerson(Person p)
        {
            int result = new Person().SaveRecord(p);

            if (result > 1) return Ok(p);

            return BadRequest();
        }

        [HttpPut]
        [ActionName("UpdatePerson")]
        public IHttpActionResult UpdatePerson(Person p)
        {
            Person person = DataAccessLayer.getclsPersonObj(p.PersonID);
            if (person != null)
            {
                if (person.updateRecord(p) > 0) return Ok(p);
                return BadRequest();
            }

            return NotFound();
        }

        [HttpGet]
        [ActionName("GetPerson")]
        public IHttpActionResult GetPerson(string personid)
        {
            Person person = DataAccessLayer.getclsPersonObj(personid);

            if (person != null) return Ok(person);

            return NotFound();
        }

        [HttpGet]
        [ActionName("GetPeople")]
        public Person GetPeople()
        {
            return null;
        }
    }
}
