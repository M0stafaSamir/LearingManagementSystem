using LMS.Models.InstractourModel;
using LMS.Repositories.Interfaces;

namespace LMS.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        public void AddLesson(Lesson Lesson)
        {
            throw new NotImplementedException();
        }

        public void DeleteLessonByID(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Lesson> GetAllLessons()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Lesson> GetLessonByCourseId(int courseId)
        {
            throw new NotImplementedException();
        }

        public Lesson GetLessonByID(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Lesson> GetLessonsByChaperId(int chaperId)
        {
            throw new NotImplementedException();
        }

        public void UpdateLesson(Lesson lesson)
        {
            throw new NotImplementedException();
        }
    }
}
