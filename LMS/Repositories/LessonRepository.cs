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

        public Task<List<Lesson>> GetLessonByCourseId(int courseId)
        {
            throw new NotImplementedException();
        }

        public Task<Lesson> GetLessonByID(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Lesson>> GetLessonsByChaperId(int chaperId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateLesson(Lesson lesson)
        {
            throw new NotImplementedException();
        }
    }
}
