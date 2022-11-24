using API.Data;
using API.DataAccess.Repository.IRepository;
using API.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace API.DataAccess.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _db;
        public CommentRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool CreateComment(Comment comment)
        {
            _db.Comments.Add(comment);
            return Save();
        }

        public bool DeleteComment(Guid id)
        {
            _db.Comments.Remove(_db.Comments.Find(id));
            return Save();
        }

        public ICollection<Comment> GetComments(Guid storyId)
        {
            return _db.Comments
                .Where(c => c.StoryId == storyId).ToList();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateComment(Comment comment)
        {
            _db.Comments.Update(comment);
            return Save();
        }
    }
}
