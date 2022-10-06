using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.IRepository
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(Guid categoryId);
        bool CategoryExists(Guid id);
        bool CategoryExists(string name);
        bool CreateOrUpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();
    }
}
