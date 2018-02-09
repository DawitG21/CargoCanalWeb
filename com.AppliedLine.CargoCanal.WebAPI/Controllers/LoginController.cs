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
    public class LoginController : ApiController
    {
        [HttpPost]
        [ActionName("LoginNow")]
        public IHttpActionResult getLoginObj(Login login)
        {

            //theLogin object
            Login foundLogin = new Login();

            LoginInfo LoginInfoObj = new LoginInfo();
            //return Ok(LoginInfoObj);

            //if (login.UserName.Length ==0) return Ok(login);
            foundLogin = foundLogin.getclsLoginRecord(login.UserName, login.Password);
            //foundLogin = foundLogin.getclsLoginRecord("agabeyre@gmail.com", "shrinkme");

            if (foundLogin != null)
            {
                //return Ok(foundLogin);
                //complex type to hold Company, Person and Login objects
                //LoginInfo LoginInfoObj = new LoginInfo();

                //return Ok(LoginInfoObj);
                //add the username and create a token - GUID
                LoginInfoObj = Guard.addUserName(foundLogin.UserName);
                LoginInfoObj.Login = foundLogin;

                //now get the Person object from db
                LoginInfoObj.Person = new Person().getclsPersonRecord(foundLogin.PersonID);

                if (LoginInfoObj.Person != null)
                {
                    //get the Company obj from db
                    LoginInfoObj.Company = new Company().getclsCompanyRecord(LoginInfoObj.Person.CompanyID);

                    try
                    {
                        LoginInfoObj.UserType = new UserTypes().getclsusertypesRecord(LoginInfoObj.Login.UserName.ToString());
                        LoginInfoObj.DefaultBroker = new DefaultBroker().getclsdefaultbrokerRecord(LoginInfoObj.Login.UserName.ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    //now return the complete payload and token
                    return Ok(LoginInfoObj);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [ActionName("BlankLogin")]
        public Login getBlankLogin()
        {
            return new Login();
        }
    }
}
