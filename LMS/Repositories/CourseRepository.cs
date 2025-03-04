using LMS.Data;
using LMS.Models.InstractourModel;
using LMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _context;   
        public CourseRepository(ApplicationDbContext context)
        {
            _context = context; 
        }
        public async Task AddCourse(Course course)
        {
            _context.Courses.Add(course);  
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCourse(int id)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id); 
            if(course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync(); 
            }
        }

        public async Task<List<Course>> GetAllCourses()
        {
            return await _context.Courses.Where(c=>c.IsAccepted==true).ToListAsync();
        }
        public async Task<List<Course>> GetAllRequestedCourses()
        {
            return await _context.Courses.Where(c => c.IsAccepted == false).ToListAsync();
        }
        public async Task<Course> GetCourseById(int id)
        {
            return await _context.Courses.FirstOrDefaultAsync(c => c.Id == id); 
        }

        public int GetCourseCount()
        {
            return _context.Courses.Count();
        }

        public async Task<List<Course>> GetCoursesByCategoryId(int categoryId)
        {
            return await _context.Courses.Where(c => c.CategoryId == categoryId).ToListAsync(); 
        }

        public async Task<List<Course>> GetCoursesByInstructorId(int instructorId)
        {
            return await _context.Courses.Where(c=>c.IsAccepted==true).ToListAsync();
        }
        public async Task<List<Course>> GetRequestedCoursesByInstructorId(int instructorId)
        {
            return await _context.Courses.Where(c => c.IsAccepted == false).ToListAsync();
        }

        public async Task<List<Course>> SearchCourseByName(string name)
        {
            return await _context.Courses.Where(c=>c.Name==name).ToListAsync();
        }

        public async Task UpdateCourse(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync(); 
        }
    }
}
