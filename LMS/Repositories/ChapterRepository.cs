using LMS.Data;
using LMS.Models.InstractourModel;
using LMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repositories
{
    public class ChapterRepository : IChapterRepository
    {
        private readonly ApplicationDbContext _context; 
        public ChapterRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddChapter(Chapter chapter)
        {
            _context.Add(chapter);
            await _context.SaveChangesAsync(); 
        }

        public async Task DeleteChapter(int id)
        {
            var chapter = await _context.Chapter.FirstOrDefaultAsync(c => c.ID == id);
            if (chapter != null) {
                _context.Chapter.Remove(chapter);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Chapter> GetChapterById(int id)
        {
            return await _context.Chapter.FirstOrDefaultAsync(c => c.ID == id); 
        }
        public async Task<List<Chapter>> GetChaptersByCourseId(int courseId)
        {
            return await _context.Chapter.Where(c => c.CourseID == courseId).ToListAsync(); 
        }
        public async Task UpdateChapter(Chapter chapter)
        {
            _context.Chapter.Update(chapter);
            await _context.SaveChangesAsync(); 
        }
    }
}
