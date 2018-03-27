using com.AppliedLine.CargoCanal.DAL;
using com.AppliedLine.CargoCanal.Models;
using com.AppliedLine.CargoCanal.WebAPI.Hubs;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace com.AppliedLine.CargoCanal.WebAPI.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AccountController : ApiController
    {
        const string profilesDir = "~/Upload/ProfileImages";

        private readonly DataAccessLayer dal;
        private readonly BizLogic biz;

        public AccountController()
        {
            dal = new DataAccessLayer();
            biz = new BizLogic(dal, null);
        }

        [HttpGet]
        public IHttpActionResult GetCompanies(string company)
        {
            try
            {
                var companies = dal.SelectCompaniesLikeTinOrName(company);
                if (companies == null) return NotFound();

                return Ok(companies);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IHttpActionResult ChangeCompanyIsActiveState(Company company)
        {
            try
            {
                // isActive state changed
                if (dal.UpdateCompanyIsActiveState(company.ID, !company.IsActive) > 0)
                    return Ok(!company.IsActive);

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IHttpActionResult GetSubscriptionsCurrent(long id)
        {
            try
            {
                return Ok(dal.SubscriptionHistoriesCurrent(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IHttpActionResult GetSubscriptionTypes()
        {
            try
            {
                return Ok(dal.SubscriptionTypes());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // Subscribe to a service
        [HttpPost]
        public IHttpActionResult Subscribe(Subscribe subscribe)
        {
            try
            {
                var result = dal.InsertSubscriptionHistory(subscribe);
                if (result > 0) return Ok();
                if (result == -1) return BadRequest("ERROR_SUBSCRIPTION_EXISTS");

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IHttpActionResult PutSubscribe(SubscriptionHistoryView subs)
        {
            try
            {
                if (dal.RenewSubscriptionHistory(subs.ID) > 0) return Ok();
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostAllCompanies(User user)
        {
            try
            {
                if (await dal.SelectCompanyByName(user.Company.CompanyName.Trim()) != null) return Conflict();
                if (await dal.SelectCompanyByTin(user.Company.TIN.Trim()) != null) return Conflict();
                if (await dal.SelectLoginEmailExists(user.Person.Email)) return BadRequest("ERROR_EMAIL_CONFLICT");

                biz.SaveAllCompanies(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IHttpActionResult> PostCompany(User user)
        {
            // make sure company exists
            if (user == null
                || user.Company == null
                || string.IsNullOrEmpty(user.Company.CompanyName)
                || string.IsNullOrEmpty(user.Company.TIN))
            {
                return BadRequest();
            }

            try
            {
                if (await dal.SelectCompanyByName(user.Company.CompanyName.Trim()) != null) return Conflict();
                if (await dal.SelectCompanyByTin(user.Company.TIN.Trim()) != null) return Conflict();

                if (await dal.SelectLoginEmailExists(user.Person.Email)) return BadRequest("ERROR_EMAIL_CONFLICT");

                biz.SaveCompany(user);
                return Ok();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IHttpActionResult PutCompany(Company company)
        {
            if (dal.UpdateCompany(company) > 0) return Ok(company);

            return BadRequest();
        }

        [HttpGet]
        // get a user's company information
        public IHttpActionResult GetCompany(long id)
        {
            string fileDir = HttpContext.Current.Server.MapPath(profilesDir);

            // iterate docs and create file from FileData if they do not exist
            // clear FileData
            Company company = dal.SelectCompanyById(id);

            FileProcessor.CreateFileFromByteOnDisc(fileDir, company.PhotoFilename, Convert.FromBase64String(company.Photo));
            company.Filepath = $"{profilesDir.Substring(2)}/{company.PhotoFilename}";
            company.Photo = string.Empty; // don't need to return the raw data
            
            return Ok(company);
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostCompanyPhoto()
        {
            try
            {
                var context = HttpContext.Current;
                var urlReferrer = context.Request.UrlReferrer;

                IEnumerable<HttpContent> multiparts = null;
                Task.Factory.StartNew(
                    () => multiparts = Request.Content.ReadAsMultipartAsync().Result.Contents,
                    CancellationToken.None,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Default).Wait();

                // no file uploaded
                if (multiparts.Count() == 0) return BadRequest();

                string fileDir = context.Server.MapPath(profilesDir);

                var companyId = Convert.ToInt64(context.Request.Form["companyid"].ToString()); // get the company id
                Dictionary<string, string> savedFile = new Dictionary<string, string>();
                foreach (var part in multiparts)
                {
                    if (part.Headers.ContentType == null) continue; // not a file e.g. personId
                    Company company = dal.SelectCompanyById(companyId);
                    //  Delete the existing physical file from the server
                    if (company != null) FileProcessor.DeleteFileOnDisc($"{fileDir}\\{company.PhotoFilename}");

                    savedFile = FileProcessor.SaveFileOnDisc(fileDir, part);
                }

                // save file to database
                var file = savedFile.ToArray()[0];
                await dal.UpdateCompanyPhoto(new Company() { ID = companyId, Photo = file.Value, PhotoFilename = file.Key });

                return Ok(file.Value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult PostLogin(Login login)
        {
            try
            {
                // TODO: decide if users have to be prevented if subscription is in active or expired
                // var lastSubscription = await new DataAccessLayer().SelectSubscriptionHistoryCurrent(login.Token);
                // no subscription history found or last subscription expired
                // if (lastSubscription == null || lastSubscription.Expired) return NotFound();

                return Ok(biz.AuthoriseUser(login));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IHttpActionResult PutPerson(Person person)
        {
            try
            {
                if (dal.UpdatePerson(person) > 0) return Ok(person);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        // get a user's person information
        public IHttpActionResult GetPerson(long id)
        {
            string fileDir = HttpContext.Current.Server.MapPath(profilesDir);

            // iterate docs and create file from FileData if they do not exist
            // clear FileData
            Person person = dal.SelectPerson(id);

            FileProcessor.CreateFileFromByteOnDisc(fileDir, person.PhotoFilename, Convert.FromBase64String(person.Photo));
            person.Filepath = $"{profilesDir.Substring(2)}/{person.PhotoFilename}";
            person.Photo = string.Empty; // don't need to return the raw data

            return Ok(person);
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostProfilePhoto()
        {
            try
            {
                var context = HttpContext.Current;
                var urlReferrer = context.Request.UrlReferrer;
                
                IEnumerable<HttpContent> multiparts = null;
                Task.Factory.StartNew(
                    () => multiparts = Request.Content.ReadAsMultipartAsync().Result.Contents,
                    CancellationToken.None,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Default).Wait();

                
                // no file uploaded
                if (multiparts.Count() == 0) return BadRequest();

                // get the person id
                var personId = Convert.ToInt64(context.Request.Form["personid"].ToString()); 
                string fileDir = context.Server.MapPath(profilesDir);
                Dictionary<string, string> savedFile = new Dictionary<string, string>();
                
                foreach (var part in multiparts)
                {
                   if (part.Headers.ContentType == null) continue; // not a file e.g. personId
                    
                    Person person = dal.SelectPerson(personId);
                    
                    //  Delete the existing physical file from the server
                    if (person != null) FileProcessor.DeleteFileOnDisc($"{fileDir}\\{person.PhotoFilename}");

                    // Save new file on disk
                    savedFile = FileProcessor.SaveFileOnDisc(fileDir, part);
                }

                // save file to database
                var file = savedFile.ToArray()[0];
                await dal.UpdatePersonPhoto(new Person() { ID = personId, Photo = file.Value, PhotoFilename = file.Key });

                return Ok(file.Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Server exception->failed to upload file");
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostPersonForCompany(User user)
        {
            try
            {
                var appSubscription = await dal.GetAppSubscription(user.Login.Token);

                // no subscription history found or last subscription expired
                if (appSubscription == null || !appSubscription.IsActive) return NotFound();

                // save person if subscription unlimited or within subscription limit
                if (appSubscription.MaximumUsers == 0 || appSubscription.MaximumUsers > appSubscription.UsersCount)
                {
                    if (biz.SavePersonWithRole(user) > 0) return Ok();

                    // failed to save
                    return InternalServerError();
                }

                // Subscription limit reached.
                return BadRequest();
            }
            catch
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        public IHttpActionResult GetUsersInCompany(Login login)
        {
            return Ok(dal.SelectUsersInCompany(login.Token));
        }

        [HttpPost]
        public IHttpActionResult GetRolesInCompany(Login login)
        {
            return Ok(dal.SelectRolesInCompanyByPrivilege(login.Token));
        }

        [HttpPut]
        public IHttpActionResult LockOrUnlockUser(UsersInCompanyView userView)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<AccountHub>();
            userView = dal.LockOrUnlockUserLogin(userView);
            context.Clients.All.toggleUserAccess(userView);
            return Ok(userView);
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostRoleAndPermission(RoleAndPermission roleAndPerm)
        {
            try
            {
                var appSubscription = await dal.GetAppSubscription(roleAndPerm.Token);

                // no subscription history found
                if (appSubscription == null || !appSubscription.IsActive) return NotFound();

                // save role if unlimited or within subscription limit
                if (appSubscription.MaximumUsers == 0 || appSubscription.MaximumUsers > appSubscription.UsersCount)
                {
                    if (dal.InsertRole(roleAndPerm.Role) > 0)
                    {
                        roleAndPerm.RolePermission.RoleID = roleAndPerm.Role.ID;
                        if (dal.InsertRolePermission(roleAndPerm.RolePermission) > 0) return Ok();
                    }

                    // failed to save
                    return InternalServerError();
                }

                // Subscription limit reached.
                return BadRequest();
            }
            catch
            {
                return InternalServerError();
            }
        }

        [HttpPut]
        public IHttpActionResult ChangePassword(ChangePasswordView changePassword)
        {
            if (dal.ChangePassword(changePassword) > 0) return Ok();
            return BadRequest();
        }

        [HttpPost]
        public IHttpActionResult PasswordReset(PasswordResetRequest resetRequest)
        {
            if (resetRequest == null) return BadRequest();

            dal.ResetPassword(resetRequest);
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult PasswordResetCheck(PasswordResetChange passwordReset)
        {
            if (passwordReset == null) return BadRequest();

            if (dal.PasswordResetCheck(passwordReset.Uid, passwordReset.Usalt)) return Ok();
            return NotFound();
        }

        [HttpPut]
        public IHttpActionResult PasswordResetChange(PasswordResetChange passwordResetChange)
        {
            if (passwordResetChange == null) return BadRequest();

            if (dal.PasswordResetChange(passwordResetChange)) return Ok();
            return NotFound();
        }

        [HttpPost]
        public IHttpActionResult GetUserPerm(Login login)
        {
            var userPermissions = dal.SelectRolePermissionsByToken(login.Token);

            if (userPermissions == null) return BadRequest();
            return Ok(userPermissions);
        }

        [HttpGet]
        public IHttpActionResult GetMobileAppKey(long id)
        {
            try
            {
                return Ok(dal.SelectMobileApiKey(id));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IHttpActionResult ValidateMobileAppKey(LoginMobile loginMobile)
        {
            var result = dal.ValidateMobileApiKey(loginMobile.LoginID, loginMobile.AppKey);
            return Ok(result);
        }
    }
}