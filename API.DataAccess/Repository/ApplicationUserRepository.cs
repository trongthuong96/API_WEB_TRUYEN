using API.Data;
using API.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace API.DataAccess.Repository
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db)
        {
            _db = db;
        }
 

        public bool CreateUser(ApplicationUser user)
        {
            _db.ApplicationUsers.Add(user);
            return Save();
        }

        public bool DeleteUser(ApplicationUser user)
        {
            _db.ApplicationUsers.Remove(user);
            return Save();
        }

        public bool EmailExists(string email)
        {
            return _db.ApplicationUsers.Any(e => e.Email == email);
        }

        public ApplicationUser GetUser(String userId)
        {
            return _db.ApplicationUsers.FirstOrDefault(u => u.Id == userId || u.UserName == userId || u.Email == userId);
        }

        public ICollection<ApplicationUser> GetUsers()
        {
            return _db.ApplicationUsers.ToList();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateUser(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public bool UserExists(string userName)
        {
            return _db.ApplicationUsers.Any(u => u.UserName == userName);
        }
    }
}
