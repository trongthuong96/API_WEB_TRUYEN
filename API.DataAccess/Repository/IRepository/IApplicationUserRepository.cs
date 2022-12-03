using API.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace API.DataAccess.Repository
{
    public interface IApplicationUserRepository
    {
        ICollection<ApplicationUser> GetUsers();
        ApplicationUser GetUser(string userId);
        ApplicationUser Authenticate(string username, string password);
        bool UserExists(string userName);
        bool EmailExists(string email);
        bool CreateUser(ApplicationUser user);
        bool UpdateUser(ApplicationUser user);
        bool DeleteUser(ApplicationUser user);
        bool Save();
    }
  
}
