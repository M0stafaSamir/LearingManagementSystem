using LMS.Data;
using LMS.Models.InstractourModel;
using LMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly ApplicationDbContext _context; 
        public LessonRepository(ApplicationDbContext context)
        {
            _context = context; 
        }
        public async Task AddLesson(Lesson Lesson)
        {

            await _context.Lessons.AddAsync(Lesson); 
            await _context.SaveChangesAsync();  
        }

        public async Task DeleteLessonByID(int id)
        {
            var lesson = await _context.Lessons.FirstOrDefaultAsync(l=>l.Id==id); 
            if(lesson != null)
            {
                _context.Lessons.Remove(lesson); 
                await _context.SaveChangesAsync();
            }
       
        }

        public Task<List<Lesson>> GetAllLessons()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Lesson>> GetLessonByCourseId(int courseId)
        {
            return await _context.Lessons.Include(l => l.Chapter).Where(l => l.Chapter.CourseID == courseId).ToListAsync(); 
        }

        public async Task<Lesson> GetLessonByID(int id)
        {
            return await _context.Lessons.FirstOrDefaultAsync(l => l.Id == id); 
        }

        public async Task<List<Lesson>> GetLessonsByChaperId(int chaperId)
        {
            return await _context.Lessons.Where(l => l.ChapterID == chaperId).ToListAsync(); 
        }

        public async Task UpdateLesson(Lesson lesson)
        {
            _context.Lessons.Update(lesson);
            await _context.SaveChangesAsync(); 
        }
    }
}
