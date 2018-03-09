using com.AppliedLine.CargoCanal.DAL;
using com.AppliedLine.CargoCanal.Models;
using System;
using System.Collections.Generic;
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
    public class ImportExportController : ApiController
    {
        const string docsDir = "~/Upload/Documents";
        private readonly DataAccessLayer dal;

        public ImportExportController()
        {
            dal = new DataAccessLayer();
        }

        [HttpPost]
        public IHttpActionResult DashboardImportSummary(Login login)
        {
            return Json(dal.DashboardImportExport(login.Token, true));
        }


        [HttpPost]
        public IHttpActionResult DashboardExportSummary(Login login)
        {
            return Json(dal.DashboardImportExport(login.Token, false));
        }


        [HttpPost]
        public IHttpActionResult DashboardShipmentsAnalytics(AnalyticsParams analytics)
        {
            return Json(dal.DashboardShipmentsAnalytics(analytics.Token, analytics.Days));
        }

        [HttpPost]
        public IHttpActionResult DashboardDemurrageAnalytics(AnalyticsParams analytics)
        {
            return Json(dal.DashboardDemurrageAnalytics(analytics.Token, analytics.Days));
        }


        [HttpPost]
        public async Task<IHttpActionResult> PostDocument()
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

                string fileDir = context.Server.MapPath(docsDir);
                string docName = string.Empty;

                Dictionary<string, string> savedFile = new Dictionary<string, string>();
                foreach (var part in multiparts)
                {
                    if (part.Headers.ContentType == null) continue; // not a file e.g. personId
                    docName = part.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
                    savedFile = FileProcessor.SaveFileOnDisc(fileDir, part);
                }

                // save file to database
                long docId = 0;

                if (savedFile.Count > 0)
                {
                    KeyValuePair<string, string> file = savedFile.ToArray()[0];
                    docId = await dal.InsertDocument(new Document { ID = 0, DocumentName = docName, FileData = file.Value, FileExtension = docName.Substring(docName.LastIndexOf(".")).ToLower(), Filename = file.Key });
                }

                //  Delete the existing physical file from the server if the record is successfully saved
                // FileProcessor.DeleteFileOnDisc($"{fileDir}\\{oldfilename}");

                if (docId == 0) return BadRequest();
                return Ok(docId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete]
        public async Task<IHttpActionResult> DeleteDocument(long id)
        {
            string fileDir = HttpContext.Current.Server.MapPath(docsDir);

            Document doc = await dal.SelectDocument(id);
            if (doc == null) return NotFound();

            long result = await dal.DeleteDocument(id);
            FileProcessor.DeleteFileOnDisc($"{fileDir}\\{doc.Filename}");

            if (result == 0) return BadRequest();
            return Ok();
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetImportExportDocs(long id)
        {
            string fileDir = HttpContext.Current.Server.MapPath(docsDir);
            //string host = HttpContext.Current.Request.Url.Authority;

            // iterate docs and create file from FileData if they do not exist
            // clear FileData
            List<Document> docs = await dal.SelectImportExportDocs(id);
            foreach (Document doc in docs)
            {
                FileProcessor.CreateFileFromByteOnDisc(fileDir, doc.Filename, Convert.FromBase64String(doc.FileData));
                doc.Filepath = $"{docsDir.Substring(2)}/{doc.Filename}";
                doc.FileData = string.Empty;
            }

            return Ok(docs);
        }

        [HttpGet]
        public IHttpActionResult GetImportsPending(long id)
        {
            BizLogic biz = new BizLogic(dal, null);
            var imports = biz.GetImportsPendingView(id);

            if (imports == null || imports.Count == 0) return NotFound();

            return Ok(imports);
        }

        [HttpGet]
        public async Task<IHttpActionResult> ImportExportMarkAsDone(long id)
        {
            DataAccessLayer dal = new DataAccessLayer();
            string result = await dal.UpdateImportExportCompleted(id);

            if (result == string.Empty) return Ok();
            return BadRequest(result);
        }
        
        [HttpPost]
        public async Task<IHttpActionResult> ImportExportTerminate(CallParams args)
        {
            DataAccessLayer dal = new DataAccessLayer();
            string result = await dal.UpdateImportExportTerminated(args);

            if (result == string.Empty) return Ok();
            return BadRequest(result);
        }
        

        [HttpGet]
        public IHttpActionResult GetExportsPending(long id)
        {
            BizLogic biz = new BizLogic(dal, null);
            var exports = biz.GetExportsPendingView(id);

            if (exports == null || exports.Count == 0) return NotFound();

            return Ok(exports);
        }

        

        [HttpPut]
        public IHttpActionResult PutLC(LC lc)
        {
            DataAccessLayer dal = new DataAccessLayer();
            long result;

            if (dal.SelectLC(lc.ImportExportID) == null)
            {
                result = dal.InsertLC(lc);
            }
            else
            {
                result = dal.UpdateLC(lc);
            }
            return Ok(lc);
        }

        [HttpPut]
        public IHttpActionResult PutConsigneeTIN(ConsigneeImportExportWithTin consigneeImportExport)
        {
            DataAccessLayer dal = new DataAccessLayer();
            long consigneeId = dal.UpdateConsigneeTIN(consigneeImportExport);
            if (consigneeId > 0) return Ok(dal.SelectCompanyById(consigneeId));

            return BadRequest();
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostImport(ImportExport importExport)
        {
            IDocumentable documentable = new ImportExportRequiredDoc();
            BizLogic biz = new BizLogic(dal, documentable);
            if (await biz.SaveImport(importExport) > 0) return Ok();

            return BadRequest();
        }

        [HttpPut]
        public IHttpActionResult PutImport(ImportExport importExport)
        {
            DataAccessLayer dal = new DataAccessLayer();
            if (dal.UpdateImportExport(importExport) > 0
                && dal.UpdateImport(importExport.Import) > 0) return Ok();

            return BadRequest();
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostExport(ImportExport importExport)
        {
            IDocumentable documentable = new ImportExportRequiredDoc();
            BizLogic biz = new BizLogic(dal, documentable);
            if (await biz.SaveExport(importExport) > 0) return Ok();

            return BadRequest();
        }

        [HttpPut]
        public IHttpActionResult PutExport(ImportExport importExport)
        {
            DataAccessLayer dal = new DataAccessLayer();
            if (dal.UpdateImportExport(importExport) > 0
                && dal.UpdateExport(importExport.Export) > 0) return Ok();

            return BadRequest();
        }

        [HttpPut]
        public IHttpActionResult PutItem(Item item)
        {
            DataAccessLayer dal = new DataAccessLayer();
            if (dal.UpdateItem(item) > 0)
            {
                int detailSaveCount = 0;
                // TODO: perform this as a Trasaction to guarantee 
                //      all item details are saved otherwise ROLLBACK
                foreach (ItemDetail itemDetail in item.ItemDetails)
                {
                    detailSaveCount += dal.UpdateItemDetail(itemDetail);
                }

                return Ok();
            }

            return BadRequest();
        }


        [HttpGet]
        public IHttpActionResult GetCost(long id)
        {
            DataAccessLayer dal = new DataAccessLayer();
            var cost = dal.SelectCost(id);
            if (cost != null) return Ok(cost);

            return BadRequest();
        }

        [HttpPost]
        public IHttpActionResult PostCost(Cost cost)
        {
            if (cost == null || cost.ID > 0) return BadRequest();

            long result;
            DataAccessLayer dal = new DataAccessLayer();

            // insert cost
            result = dal.InsertCost(cost);
            if (result != 0) return Ok(result);

            return BadRequest();
        }

        [HttpPut]
        public IHttpActionResult PutCost(Cost cost)
        {
            if (cost == null || cost.ID == 0) return BadRequest();

            long result;
            DataAccessLayer dal = new DataAccessLayer();

            // update existing cost
            result = dal.UpdateCost(cost);
            if (result != 0) return Ok(result);

            return BadRequest();
        }


        [HttpGet]
        public IHttpActionResult GetProblemUpdates(long id)
        {
            BizLogic biz = new BizLogic(dal, null);
            var problemUpdates = biz.GetProblemUpdateView(id);

            if (problemUpdates == null) return BadRequest();

            return Ok(problemUpdates);
        }

        [HttpPost]
        public IHttpActionResult PostProblemUpdate(ProblemUpdate problemUpdate)
        {
            BizLogic biz = new BizLogic(dal, null);
            if (biz.SaveProblemUpdate(problemUpdate) > 0)
            {
                var problemUpdateView = biz.GetProblemUpdateView(problemUpdate.ImportExportID);
                return Ok(problemUpdateView);
            }

            return BadRequest();
        }

        [HttpPut]
        public IHttpActionResult PutProblemUpdate(ProblemUpdate problemUpdate)
        {
            DataAccessLayer dal = new DataAccessLayer();
            if (dal.UpdateProblemUpdate(problemUpdate) > 0) return Ok();

            return BadRequest();
        }

        [HttpPut]
        public IHttpActionResult PutProblemUpdateResolve(ProblemUpdate problemUpdate)
        {
            DataAccessLayer dal = new DataAccessLayer();
            var i = dal.ResolveProblemUpdate(problemUpdate);
            if (i > 0) return Ok(problemUpdate);

            return BadRequest();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteProblemUpdate(CallParams args)
        {
            DataAccessLayer dal = new DataAccessLayer();
            if (await dal.DeleteProblemUpdate(args) > 0) return Ok();

            return BadRequest();
        }

        [HttpGet]
        public IHttpActionResult GetStatusUpdates(long id)
        {
            BizLogic biz = new BizLogic(dal, null);
            var statusUpdates = biz.GetStatusUpdateView(id);

            if (statusUpdates == null) return BadRequest();

            return Ok(statusUpdates);
        }

        [HttpPost]
        public IHttpActionResult PostStatusUpdate(StatusUpdate statusUpdate)
        {
            DataAccessLayer dal = new DataAccessLayer();
            if (dal.InsertStatusUpdate(statusUpdate) > 0)
            {
                var statusUpdateView = new BizLogic(dal, null).GetStatusUpdateView(statusUpdate.ImportExportID);
                return Ok(statusUpdateView);
            }

            return BadRequest();
        }

        [HttpPut]
        public IHttpActionResult PutStatusUpdate(StatusUpdate statusUpdate)
        {
            DataAccessLayer dal = new DataAccessLayer();
            if (dal.UpdateStatusUpdate(statusUpdate) > 0) return Ok();

            return BadRequest();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteStatusUpdate(CallParams args)
        {
            DataAccessLayer dal = new DataAccessLayer();
            if (await dal.DeleteStatusUpdate(args) > 0) return Ok();

            return BadRequest();
        }

    }
}