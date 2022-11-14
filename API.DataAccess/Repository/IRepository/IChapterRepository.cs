using API.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.DataAccess.Repository.IRepository
{
    public interface IChapterRepository
    {
        ICollection<Chapter> GetCategories(Guid storyId);
        Chapter GetCategory(Guid storyId, Guid chapterId);
        bool ChapterExists(Guid id);
        bool ChapterExists(string name);
        bool CreateOrUpdateChapter(Chapter chapter);
        bool DeleteChapter(Chapter chapter);
        bool Save();
    }
}
