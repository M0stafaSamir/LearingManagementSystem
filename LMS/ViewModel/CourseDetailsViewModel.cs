using LMS.Models.InstractourModel;
using LMS.Models.StudentModels;

namespace LMS.ViewModel
{
    public class CourseDetailsViewModel
    {
        public Course Course { get; set; }
        public bool IsEnrolled { get; set; }
        public double Progress { get; set; }
        public IEnumerable<ReviewedCourse> Reviews { get; set; }

    }
}
