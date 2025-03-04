using LMS.Models.InstractourModel;

namespace LMS.Repositories.Interfaces
{
    public interface ILessonRepository
    {
        Task<List<Lesson>> GetAllLessons(); 
        Task<Lesson> GetLessonByID(int id);
        Task AddLesson(Lesson Lesson);
        Task UpdateLesson(Lesson lesson);  
        Task DeleteLessonByID(int id);
        Task<List<Lesson>> GetLessonsByChaperId(int chaperId);
        Task<List<Lesson>> GetLessonByCourseId(int courseId); 
        
    }
}
