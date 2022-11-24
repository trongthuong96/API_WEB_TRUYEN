using API.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.DataAccess.Repository.IRepository
{
    public interface ICommentRepository
    {
        ICollection<Comment> GetComments(Guid storyId);
        bool CreateComment(Comment comment);
        bool UpdateComment(Comment comment);
        bool DeleteComment(Guid id);
        bool Save();
    }
}
