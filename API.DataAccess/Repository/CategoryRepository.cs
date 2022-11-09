using API.Data;
using API.Models;
using API.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool CategoryExists(Guid id)
        {
            return _db.Categories.Any(c => c.Id == id);
        }

        public bool CategoryExists(string name)
        {
            bool value = _db.Categories.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool CreateOrUpdateCategory(Category category)
        {
            _db.Categories.Update(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _db.Categories.Remove(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return _db.Categories.OrderBy(c => c.Name).ToList();
        }

        public Category GetCategory(Guid categoryId)
        {
            return _db.Categories.FirstOrDefault(c => c.Id == categoryId);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}
