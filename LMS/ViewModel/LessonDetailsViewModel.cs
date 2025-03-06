using LMS.Models.StudentModels;

namespace LMS.ViewModel
{
    public class LessonDetailsViewModel
    {
        public int LessonId { get; set; }
        public string LessonName { get; set; }
        public string MediaLink { get; set; }
        public IEnumerable<Note> Notes { get; set; }
    }
}
