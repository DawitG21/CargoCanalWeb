using com.AppliedLine.CargoCanal.Models;

namespace com.AppliedLine.CargoCanal.WebAPI.Hubs
{
    public interface ICommentHub
    {
        void CommentAdded(Comment comment);
    }
}
