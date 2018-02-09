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
    public class RegistrationController : ApiController
    {
        [HttpGet]
        [ActionName("GetBlankRegistrationObjects")]
        public RegModel getBlankRegistrationObjects()
        {
            RegModel regObjs = new RegModel();
            //regObjs.Login.UserName = "test@test.com";
            //regObjs.Login.Password = "pw123";

            //regObjs.Person.FirstName = "Abdifatah";
            //regObjs.Person.MiddleName = "Mohamed";
            //regObjs.Person.LastName = "Gabeyre";
            //regObjs.Person.Phone = "0912619282";

            //regObjs.Company.CompanyName = "Apple Inc.";
            return regObjs;

        }

        #region "Save Objects code........"
        // Creates customers
        [HttpPost]
        [ActionName("RegisterCustomer")]
        public IHttpActionResult PostCustomer(RegModel regObjs)
        {
            regObjs.Company = null;
            regObjs.Person.Email = regObjs.Login.UserName;
            string personID = savePersonAndReturnPersonID(regObjs.Person);

            if (!personID.ToLower().Contains("fail"))
            {
                var customer = new Customer()
                {
                    PersonID = personID,
                    RegisteredDate = DateTime.Now
                };

                if (customer.SaveCustomer(customer) > 0)
                {
                    regObjs.Login.PersonID = personID;
                    try
                    {
                        if (regObjs.Login.SaveRecord(regObjs.Login) > 0) return Ok(regObjs);
                    }
                    catch (Exception ex)
                    {
                        return InternalServerError(ex);
                    }
                }
            }

            return BadRequest($"Failed to Add Registration - {personID}!");
        }

        // Creates system administrators
        [HttpPost]
        [ActionName("RegisterAdmin")]
        public IHttpActionResult PostAdminUser(RegModel regObjs)
        {
            regObjs.Person.Email = regObjs.Login.UserName;
            string personID = savePersonAndReturnPersonID(regObjs.Person);

            if (!personID.ToLower().Contains("fail"))
            {
                regObjs.Login.PersonID = personID;
                try
                {
                    if (regObjs.Login.SaveRecord(regObjs.Login) > 0) return Ok();
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }

            return BadRequest($"Failed to Add Registration - {personID}!");
        }

        // Creates customs clearing
        [HttpPost]
        [ActionName("RegisterCustoms")]
        public IHttpActionResult PostCustomsUser(RegModel regObjs)
        {
            regObjs.Person.Email = regObjs.Login.UserName;
            regObjs.Person.CompanyID = "0".PadLeft(10, '0');
            string personID = savePersonAndReturnPersonID(regObjs.Person);

            if (!personID.ToLower().Contains("fail"))
            {
                regObjs.Login.PersonID = personID;
                try
                {
                    if (regObjs.Login.SaveRecord(regObjs.Login) > 0) return Ok();
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }

            return BadRequest($"Failed to Add Registration - {personID}!");
        }

        // Creates agents
        [HttpPost]
        [ActionName("AddNewRegistration")]
        public RegModel AddNewRegistration(RegModel regObjs)
        {
            //if Person works for Company
            string companyID = saveCompanyAndReturnCompanyID(regObjs.Company);
            if (!companyID.Contains("FAIL"))
            {
                regObjs.Person.CompanyID = companyID;
            }

            regObjs.Person.Email = regObjs.Login.UserName;

            //copy the username to the login UserTypes object
            regObjs.UserType.username = regObjs.Person.Email.ToString();

            string personID = savePersonAndReturnPersonID(regObjs.Person);

            if (!personID.Contains("FAIL"))
            {
                regObjs.Login.PersonID = personID;
                try
                {
                    if (regObjs.Login.SaveRecord(regObjs.Login) == 1)
                    {
                        //save usertype
                        if (saveUserType(regObjs.UserType).ToUpper().Contains("FAIL"))
                        {
                            //this means there was an exception adding the Person record
                            regObjs.ErrorMessage = "Failed to Save User Type data";
                        }
                    }
                }
                catch (Exception ex)
                {
                    regObjs.ErrorMessage = "Failed Adding Login Details: " + ex.Message;
                }
            }
            else if (personID.Contains("FAILERROR"))
            {
                //this means there was an exception adding the Person record
                regObjs.ErrorMessage = "Failed to Add Registration! " + personID;
            }

            return regObjs;
        }

        public string saveUserType(UserTypes utObj)
        {
            try
            {
                if (utObj.SaveRecord(utObj) == 1)
                {
                    return utObj.username;
                }
                return "FAILED";
            }
            catch (Exception ex)
            {
                return "FAILEDERROR: " + ex.Message;
            }
        }

        //Save Company and return the company ID
        public string saveCompanyAndReturnCompanyID(Company Company)
        {
            try
            {
                if (Company.SaveRecord(Company) == 1)
                {
                    return Company.CompanyID;
                }
                else
                {
                    return "FAIL";
                }
            }
            catch (Exception ex)
            {
                return "FAIL: " + ex.Message;
            }
        }

        public string savePersonAndReturnPersonID(Person Person)
        {
            try
            {
                if (Person.SaveRecord(Person) == 1) return Person.PersonID;

                return "FAILED";
            }
            catch (Exception ex)
            {
                return "FAILERROR: " + ex.Message;
            }
        }
        #endregion

        #region "Update Objects Code.............."
        [HttpPost]
        [ActionName("UpdateAccount")]
        public int UpdateAccountObjects(RegModel regObjs)
        {
            if (regObjs.Person.updateRecord(
                regObjs.Person) > 0)
            {
                //copy the email to the ownerusername of the default broker object for this person
                regObjs.DefaultBroker.ownerusername = regObjs.Person.Email.ToString();

                //this saves a new default broker record or updates existing record for this person
                updateOrSaveDefaultBroker(regObjs.DefaultBroker);

                if (regObjs.Company.updateRecord(
                    regObjs.Company) > 0)
                {
                    return 1;
                }
            }

            return 0;
        }

        public int updateOrSaveDefaultBroker(DefaultBroker db)
        {
            if (db.getclsdefaultbrokerRecord(db.ownerusername).defaultusername == string.Empty)
            {
                return db.SaveRecord(db);

            }
            else
            {
                return db.updateRecord(db);
            }
        }
        #endregion
    }
}
