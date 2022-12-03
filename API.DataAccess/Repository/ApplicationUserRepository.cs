using API.Data;
using API.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace API.DataAccess.Repository
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly AppSettings _appSettings;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        public ApplicationUserRepository(ApplicationDbContext db, IOptions<AppSettings> appSettings, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _db = db;
            _appSettings = appSettings.Value;
            _passwordHasher = passwordHasher;
        }

        public ApplicationUser Authenticate(string username, string password)
        {
            // var user = _db.ApplicationUsers.SingleOrDefault(x => x.UserName == username && _passwordHasher.VerifyHashedPassword(x,x.PasswordHash, password));
            return null;
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
            _db.ApplicationUsers.Update(user);
            return Save();
        }

        public bool UserExists(string userName)
        {
            return _db.ApplicationUsers.Any(u => u.UserName == userName);
        }
    }
}
