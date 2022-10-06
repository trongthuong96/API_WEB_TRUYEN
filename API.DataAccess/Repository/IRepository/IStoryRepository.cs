using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.IRepository
{
    public interface IStoryRepository
    {
        ICollection<Story> GetStories();
        ICollection<Story> GetStoriesToAuthor(string authorName);
        Story GetStory(Guid storyId);
        bool StoryExists(Guid id);
        bool StoryExists(string name);
        bool CreateOrUpdateStory(Story story);
        bool DeleteStory(Story story);
        bool Save();
    }
}
