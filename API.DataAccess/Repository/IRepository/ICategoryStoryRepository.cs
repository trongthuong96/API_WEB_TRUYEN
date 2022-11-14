using API.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.DataAccess.Repository.IRepository
{
    public interface ICategoryStoryRepository
    {
       // ICollection<CategoryStory> GetCategories();
        CategoryStory GetCategoryStory(Guid categoryStoryId, Guid storyId);
        ICollection<CategoryStory> GetStories(Guid categoryId);
        ICollection<CategoryStory> GetCategories(Guid storyId);
        bool CategoryStoryExists(Guid cagoryId, Guid storyId);
        bool CreateOrUpdateCategoryStory(CategoryStory categoryStory);
        bool DeleteCategoryStory(CategoryStory categoryStory);
        bool Save();
    }
}
