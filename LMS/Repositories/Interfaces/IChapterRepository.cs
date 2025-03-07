using LMS.Models.InstractourModel;

namespace LMS.Repositories.Interfaces
{
    public interface IChapterRepository
    {
        //IEnumerable<Chapter> GetAllChapters();
        Task<Chapter> GetChapterById(int id);
        Task<int> AddChapter(Chapter chapter);
        Task UpdateChapter(Chapter chapter);
        Task DeleteChapter(int id);
        Task<List<Chapter>> GetChaptersByCourseId(int courseId);
    }
}
