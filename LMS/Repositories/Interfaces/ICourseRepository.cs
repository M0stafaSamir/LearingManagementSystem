using LMS.Models.InstractourModel;
using LMS.ViewModel.Inst;

namespace LMS.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllCourses();
        Task<IEnumerable<Course>> GetInstructorCourses(string Id);
        Task<IEnumerable<Course>> GetAllRequestedCourses();
        Task<IEnumerable<Course>> SearchCourseByName(string name);
        Task<Course> GetCourseById(int id);
        Task AddCourse(Course course);
        Task UpdateCourse(Course course);
        Task AcceptCourse(int id);
        Task DeleteCourse(int id);
        Task<IEnumerable<Course>> GetCoursesByCategoryId(int categoryId);
        Task<IEnumerable<Course>> GetCoursesByInstructorId(string instructorId);
        Task<IEnumerable<Course>> GetRequestedCoursesByInstructorId(string instructorId);
        Task<int> CreateCourse(CreateCourseViewModel course, IFormFile Image,string InstID); 
        int GetCourseCount();


        
    }
}
