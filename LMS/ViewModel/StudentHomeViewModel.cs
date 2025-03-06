using LMS.Models.InstractourModel;
using LMS.Models.StudentModels;

namespace LMS.ViewModel
{
    public class StudentHomeViewModel
    {
        public IEnumerable<Course> Courses { get; set; } = new List<Course>();
        public IEnumerable<Course> RecommendedCourses { get; set; } = new List<Course>();
        public IEnumerable<StudentEnrollCourse> PurchasedCourses { get; set; } = new List<StudentEnrollCourse>();
        public Dictionary<int, double> CourseProgress { get; set; } = new Dictionary<int, double>(); // CourseId -> Progress
        
    }
}
