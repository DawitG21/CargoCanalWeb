using com.AppliedLine.CargoCanal.Models;
using Microsoft.AspNet.SignalR;

namespace com.AppliedLine.CargoCanal.WebAPI.Hubs
{
    [Microsoft.AspNet.SignalR.Hubs.HubName("commentHub")]
    public class CommentHub : Hub<ICommentHub>
    {
        public void Send(Comment comment)
        {
            Clients.All.CommentAdded(comment);
        }
    }
}

