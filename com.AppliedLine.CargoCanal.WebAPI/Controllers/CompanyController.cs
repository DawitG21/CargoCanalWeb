using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using com.AppliedLine.CargoCanal.Models;

namespace com.AppliedLine.CargoCanal.WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CompanyController : ApiController

    {
        List<Company> companies = DataAccessLayer.getAllclsCompanyObj();

        ArrayList Companies = new ArrayList();

        [HttpGet]
        [ActionName("AllCompanies")]
        public IEnumerable<Company> GetAllCompanies()
        {
            return companies;
        }

        [HttpGet]
        [ActionName("BlankCompany")]
        public Company getBlankCompany()
        {
            return new Company();
        }

        // GET: Company
        // [Route("Company/GetPersonAndCompany")]
        // [HttpGet]
        public PersonAndCompany GetPersonAndCompany()
        {
            Company companyObj = new Company();
            companyObj.CompanyID = "1";

            PersonAndCompany personAndCompany = new PersonAndCompany();
            personAndCompany.Company = companyObj;

            Person person = new Person();
            person.FirstName = "Abdifatah";
            person.LastName = "Gabeyre";


            personAndCompany.Person = person;

            return personAndCompany;
        }

        [HttpPost]
        [ActionName("AddCompany")]
        public IHttpActionResult AddCompany(Company company)
        {
            //just add it to the collection for testing
            Companies.Add(company);
            if (company.SaveRecord(company) > 0)
            {
                return Ok(company);
            }

            return BadRequest();
        }

        [HttpGet]
        [ActionName("GetCompanyByID")]
        public IHttpActionResult GetCompanyByID(string companyID)
        {
            try
            {
                Company company = new Company();
                if (company.getclsCompanyRecord(companyID) != null)
                {
                    return Ok(company);
                }

                return NotFound();
            }
            catch
            {
                return InternalServerError();
            }
        }
    }
}