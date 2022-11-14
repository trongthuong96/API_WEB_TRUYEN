using API.Data;
using API.DataAccess.Repository.IRepository;
using API.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace API.DataAccess.Repository
{
    public class CategoryStoryRepository : ICategoryStoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryStoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool CategoryStoryExists(Guid cagoryId, Guid storyId)
        {
            return _db.CategoryStories.Any(c => c.StoryId == storyId && c.CategoryId == cagoryId);
        }

        public bool CreateOrUpdateCategoryStory(CategoryStory categoryStory)
        {
            _db.CategoryStories.Update(categoryStory);
            return Save();
        }

        public bool DeleteCategoryStory(CategoryStory categoryStory)
        {
            _db.CategoryStories.Remove(categoryStory);
            return Save();
        }

        public ICollection<CategoryStory> GetStories(Guid categoryId)
        {
            return _db.CategoryStories.Where(c => c.CategoryId == categoryId).ToList();
        }

        public CategoryStory GetCategoryStory(Guid categoryStoryId, Guid storyId)
        {
            return _db.CategoryStories.FirstOrDefault(c => c.CategoryId == categoryStoryId && c.StoryId == storyId);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public ICollection<CategoryStory> GetCategories(Guid storyId)
        {
            return _db.CategoryStories.Where(c => c.StoryId == storyId).ToList();
        }
    }
}
