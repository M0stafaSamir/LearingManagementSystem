using LMS.Models.InstractourModel;

namespace LMS.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetAllCourses();
        Task<List<Course>> GetAllRequestedCourses();
        Task<List<Course>> SearchCourseByName(string name);
        Task<Course> GetCourseById(int id);
        Task AddCourse(Course course);
        Task UpdateCourse(Course course);
        Task DeleteCourse(int id);
        Task<List<Course>> GetCoursesByCategoryId(int categoryId);
        Task<List<Course>> GetCoursesByInstructorId(int instructorId);
        Task<List<Course>> GetRequestedCoursesByInstructorId(int instructorId);

        int GetCourseCount();
    }
}
