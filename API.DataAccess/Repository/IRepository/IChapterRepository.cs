using API.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.DataAccess.Repository.IRepository
{
    public interface IChapterRepository
    {
        ICollection<Chapter> GetChapters(Guid storyId);
        Chapter GetChapter(Guid storyId, Guid chapterId);
        bool ChapterExists(Guid chapterId, Guid storyId);
        bool ChapterExists(string name);
        bool CreateChapter(Chapter chapter);
        bool UpdateChapter(Chapter chapter);
        bool DeleteChapter(Chapter chapter);
        bool Save();
    }
}
