using LMS.Models.InstractourModel;

namespace LMS.Repositories.Interfaces
{
    public interface ILessonRepository
    {
        IEnumerable<Lesson> GetAllLessons(); 
        Lesson GetLessonByID(int id);
        void AddLesson(Lesson Lesson);
        void UpdateLesson(Lesson lesson);  
        void DeleteLessonByID(int id);
        IEnumerable<Lesson> GetLessonsByChaperId(int chaperId);
        IEnumerable<Lesson> GetLessonByCourseId(int courseId); 
        
    }
}
