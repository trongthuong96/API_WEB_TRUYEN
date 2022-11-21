using API.Data;
using API.DataAccess.Repository.IRepository;
using API.Models;
using API.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace API.DataAccess.Repository
{
    public class TickRepository : ITickRepository
    {
        private readonly ApplicationDbContext _db;
        public TickRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool CreateTick(Tick tick)
        {
            _db.Ticks.Add(tick);
            return Save();
        }

        public bool DeleteTick(Tick tick)
        {
            _db.Ticks.Remove(tick);
            return Save();
        }

        public ICollection<Tick> GetTick(string userId)
        {
            return _db.Ticks.Where(t => t.UserId == userId).ToList();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool TickExists(string userId, Guid storyId)
        {
            return _db.Ticks.Any(t => t.UserId == userId && t.StoryId == storyId);
        }
    }
}
