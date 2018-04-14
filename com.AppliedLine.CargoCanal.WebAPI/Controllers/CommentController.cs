using com.AppliedLine.CargoCanal.DAL;
using com.AppliedLine.CargoCanal.Models;
using com.AppliedLine.CargoCanal.WebAPI.Hubs;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace com.AppliedLine.CargoCanal.WebAPI.Controllers
{
    public class CommentController : ApiController
    {
        private readonly DataAccessLayer _dal;
        private readonly IHubContext<ICommentHub> _context;

        public CommentController()
        {
            _dal = new DataAccessLayer();
        }


        [HttpGet]
        public IHttpActionResult GetComment(long id)
        {
            try
            {
                var result = _dal.SelectComments(id);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostComment([FromBody]CommentNew comment)
        {
            var result = await _dal.InsertComment(comment);
            if (result != null)
            {
                // notify all clients of the new comment
                //var context = GlobalHost.ConnectionManager.GetHubContext<ICommentHub>("commentHub");
                var context = GlobalHost.ConnectionManager.GetHubContext<CommentHub>();
                context.Clients.All.CommentAdded(result);

                //_context.Clients.All.commentAdded(result);
                return Ok(result);
            }

            return BadRequest();
        }

    }
}