using API.Data;
using API.Models;
using API.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _db;
        public AuthorRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool AuthorExists(Guid id)
        {
            return _db.Authors.Any(c => c.Id == id);
        }

        public bool AuthorExists(string pseudonym)
        {
            bool value = _db.Authors.Any(c => c.pseudonym.ToLower().Trim() == pseudonym.ToLower().Trim());
            return value;
        }

        public bool CreateAuthor(Author author)
        {
            _db.Authors.Update(author);
            return Save();
        }

        public bool DeleteAuthor(Author author)
        {
            _db.Authors.Remove(author);
            return Save();
        }

        public ICollection<Author> GetAuthors()
        {
            return _db.Authors.OrderBy(c => c.pseudonym).ToList();
        }

        public Author GetAuthor(Guid authorId)
        {
            return _db.Authors.FirstOrDefault(c => c.Id == authorId);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateAuthor(Author author)
        {
            _db.Authors.Update(author);
            return Save();
        }

        public Guid AuthorId(string pseudonym)
        {
            return _db.Authors.FirstOrDefault(c => c.pseudonym.ToLower().Trim() == pseudonym.ToLower().Trim()).Id;
        }
    }
}
