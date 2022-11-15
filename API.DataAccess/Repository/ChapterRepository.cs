using API.Data;
using API.DataAccess.Repository.IRepository;
using API.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace API.DataAccess.Repository
{
    public class ChapterRepository : IChapterRepository
    {
        private readonly ApplicationDbContext _db;
        public ChapterRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool ChapterExists(Guid chapterId, Guid storyId)
        {
            return _db.Chapters.Any(c => c.Id == chapterId && c.StoryId == storyId);
        }

        public bool ChapterExists(string name)
        {
            return _db.Chapters.Any(c => c.Name == name);
        }

        public bool CreateChapter(Chapter chapter)
        {
            _db.Chapters.Add(chapter);
            return Save();
        }

        public bool UpdateChapter(Chapter chapter)
        {
            _db.Chapters.Update(chapter);
            return Save();
        }

        public bool DeleteChapter(Chapter chapter)
        {
            _db.Chapters.Remove(chapter);
            return Save();
        }

        public ICollection<Chapter> GetChapters(Guid storyId)
        {
            return _db.Chapters.Where(c=>c.StoryId == storyId).OrderBy(c => c.NumberChapter).ToList();
        }

        public Chapter GetChapter(Guid storyId, Guid chapterId)
        {
            return _db.Chapters.FirstOrDefault(c => c.StoryId == storyId && c.Id == chapterId);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}
