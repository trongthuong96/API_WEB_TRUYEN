using API.Data;
using API.Models;
using API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository
{
    public class StoryRepository : IStoryRepository
    {
        private readonly ApplicationDbContext _db;
        public StoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool StoryExists(Guid id)
        {
            return _db.Stories.Any(c => c.Id == id);
        }

        public bool StoryExists(string name)
        {
            bool value = _db.Stories.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool CreateOrUpdateStory(Story story)
        {
            _db.Stories.Update(story);
            return Save();
        }

        public bool DeleteStory(Story story)
        {
            _db.Stories.Remove(story);
            return Save();
        }

        public ICollection<Story> GetStories()
        {
            return _db.Stories.OrderBy(c => c.Name).ToList();
        }

        public ICollection<Story> GetStoriesToAuthor(string authorName)
        {
            return _db.Stories.Where(c=>c.Author.pseudonym == authorName).ToList();
        }

        public Story GetStory(Guid storyId)
        {
            return _db.Stories.FirstOrDefault(c => c.Id == storyId);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}
