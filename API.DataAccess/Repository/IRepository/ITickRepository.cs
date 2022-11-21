using API.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.DataAccess.Repository.IRepository
{
    public interface ITickRepository
    {
        ICollection<Tick> GetTick(string userId);
        bool TickExists(string userId, Guid storyId);
        bool CreateTick(Tick tick);
        bool DeleteTick(Tick tick);
        bool Save();
    }
}
