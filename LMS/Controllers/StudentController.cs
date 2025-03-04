using LMS.Models;
using LMS.Models.InstractourModel;
using Microsoft.AspNetCore.Mvc;
using LMS.ViewModel;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class StudentController : Controller
{
    private readonly IStudentRepository _studentRepo;

    public StudentController(IStudentRepository studentRepo)
    {
        _studentRepo = studentRepo;
    }


    public IActionResult Index(string search, Category category)
    {
        var studentId = User.Identity.Name;

        var courses = _studentRepo.GetAllCourses(search, category);
        var purchasedCourses = _studentRepo.GetAllPurchases(studentId);

        var courseProgress = new Dictionary<int, double>(); 
        foreach (var course in purchasedCourses)
        {
            courseProgress[course.Id] = _studentRepo.GetCourseProgress(studentId, course.Id);
        }

        var model = new StudentHomeViewModel
        {
            Courses = courses,
            RecommendedCourses = _studentRepo.GetRecommendedCourses(studentId),
            PurchasedCourses = purchasedCourses,
            CourseProgress = courseProgress
        };

        ViewBag.Categories = _studentRepo.GetAllCategories();

        return View(model);
    }
    [HttpPost]
    public IActionResult EnrollCourse(int courseId)
    {
        var studentId = User.Identity.Name; 
        _studentRepo.EnrollInCourse(studentId,courseId);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult PurchaseCourse(int courseId)
    {
        var course = _studentRepo.GetCourseDetails(courseId);
        if (course == null) return NotFound();

        return View(course);
    }

    [HttpPost]
    public IActionResult AddToWishlist(int courseId)
    {
        var studentId = User.Identity.Name;
        _studentRepo.AddToWishlist(studentId,courseId );
        TempData["Message"] = "Course added to wishlist!";
        return RedirectToAction("Index");
    }

    public IActionResult CourseDetails(int courseId)
    {
        var course = _studentRepo.GetCourseDetails(courseId);
        if (course == null) return NotFound();
        return View(course);
    }
    public IActionResult LessonDetails(int lessonId)
    {
        var lesson = _studentRepo.GetLessonDetails(lessonId, User.Identity.Name);
        if (lesson == null) return NotFound();
        if (!_studentRepo.HasPurchasedCourse(lesson.Chapter.CourseID, User.Identity.Name))
        {
            return RedirectToAction("CourseDetails", new { courseId = lesson.Chapter.CourseID });
        }
        _studentRepo.MarkLessonAsStudied(User.Identity.Name, lessonId);
        return View(lesson);
    }
    public IActionResult EnrolledCourses()
    {
        var enrolledCourses = _studentRepo.GetEnrolledCourses(User.Identity.Name);
        return View(enrolledCourses);
    }
    public IActionResult Wishlist()
    {
        var wishlist = _studentRepo.GetWishlist(User.Identity.Name);
        return View(wishlist);
    }
    public IActionResult Certificates()
    {
        var certificates = _studentRepo.GetAllCertificates(User.Identity.Name);
        return View(certificates);
    }
}
