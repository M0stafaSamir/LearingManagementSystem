using LMS.Models;
using LMS.Models.InstractourModel;
using Microsoft.AspNetCore.Mvc;
using LMS.ViewModel;
using Microsoft.AspNetCore.Authorization;
using LMS.Models.StudentModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


[Authorize]
public class StudentController : Controller
{
    private readonly IStudentRepository _studentRepo;
    public StudentController(IStudentRepository studentRepo)
    {
        _studentRepo = studentRepo;
    }


    public IActionResult Index(string search, string category)
    {
        var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var courses = _studentRepo.GetAllCourses(search, category);
        var purchasedCourses = _studentRepo.GetEnrolledCourses(studentId)?? new List<StudentEnrollCourse>();

        var courseProgress = new Dictionary<int, double>(); 
        foreach (var course in purchasedCourses)
        {
            courseProgress[course.CourseId] = _studentRepo.GetCourseProgress(studentId, course.CourseId);
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
        var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var course = _studentRepo.GetCourseDetails(courseId);

        if (course == null || studentId == null)
        {
            return NotFound();
        }

        // Check if already enrolled

        var enrollment = new StudentEnrollCourse
        {
            StudentId = studentId,
            CourseId = courseId,
            EnrollmentDate = DateTime.Now
        };

        _studentRepo.EnrollInCourse(studentId,courseId);
        
        return RedirectToAction("Index");
    }


    [HttpPost]
    public IActionResult PurchaseCourse(int courseId)
    {
        var course = _studentRepo.GetCourseDetails(courseId);
        if (course == null) return NotFound();
        //Payment logic and view
        return View(course);
    }

    [HttpPost]
    public IActionResult AddToWishlist(int courseId)
    {
        var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        _studentRepo.AddToWishlist(studentId,courseId );
        TempData["Message"] = "Course added to wishlist!";
        return RedirectToAction("Index");
    }
    public IActionResult Wishlist()
    {
        var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var wishlistItems = _studentRepo.GetWishlist(studentId) ?? new List<WishList>(); 
        return View(wishlistItems);
    }
    [HttpPost]
    public IActionResult RemoveFromWishlist(int courseId)
    {
        var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        _studentRepo.RemoveFromWishlist(studentId, courseId);
        TempData["Message"] = "Course removed from wishlist.";
       
        return RedirectToAction("Wishlist");
    }

    public IActionResult EnrolledCourses()
    {
        var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var enrolledCourses = _studentRepo.GetEnrolledCourses(studentId);
        List<Course> Studentcourses = new();
        foreach(var course in enrolledCourses)
        {
            Studentcourses.Add(_studentRepo.GetCourseDetails(course.CourseId));
        }
        var courseProgress = new Dictionary<int, double>();
        foreach (var course in enrolledCourses)
        {
            courseProgress[course.CourseId] = _studentRepo.GetCourseProgress(studentId, course.CourseId);
        }
        ViewBag.courseProgress = courseProgress;
        return View(Studentcourses);
    }
    [HttpPost]
    public IActionResult UnenrollCourse(int courseId)
    {
        var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        _studentRepo.RemoveEnrollment(studentId, courseId);

        TempData["Message"] = "You have successfully unenrolled from the course.";
        return RedirectToAction("EnrolledCourses");
    }
    public IActionResult CourseDetails(int courseId)
    {
        var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var course = _studentRepo.GetCourseDetails(courseId);

        if (course == null)
        {
            return NotFound();
        }

        var isEnrolled = _studentRepo.HasPurchasedCourse(courseId,studentId);
        var progress = isEnrolled ? _studentRepo.GetCourseProgress(studentId, courseId) : 0;
        var reviews = _studentRepo.GetAllReviews(courseId);

        var viewModel = new CourseDetailsViewModel
        {
            Course = course,
            IsEnrolled = isEnrolled,
            Progress = progress,
            Reviews = reviews
        };

        return View(viewModel);
    }
    public IActionResult AddReview(int courseId)
    {
        var model = new ReviewedCourse { CourseId = courseId ,Course=_studentRepo.GetCourseDetails(courseId)};
        return View(model);
    }
    [HttpPost]
    public IActionResult SubmitReview(ReviewedCourse model)
    {
        if (ModelState.IsValid)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var review = new ReviewedCourse
            {
                StudentId = studentId,
                CourseId = model.CourseId,
                Rating = model.Rating,
                Comment = model.Comment,
                DateTime = DateTime.Now
            };

            _studentRepo.AddReview(review);  
            TempData["Message"] = "Review submitted successfully!";

            return RedirectToAction("CourseDetails", new { courseId = model.CourseId });
        }

        return View("AddReview", model); 
    }

    public IActionResult Certificates()
    {
        var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var enrolledCourses = _studentRepo.GetEnrolledCourses(studentId);

        foreach (var course in enrolledCourses)
        {
            _studentRepo.CheckCourseCompletion(studentId, course.CourseId);
        }

        var certificates = _studentRepo.GetAllCertificates(studentId);
        return View(certificates);
    }

    public IActionResult LessonDetails(int lessonId)
    {
        var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var lesson = _studentRepo.GetLessonDetails(lessonId,studentId);
        if (lesson == null)
        {
            return NotFound();
        }

        var studentNotes = _studentRepo.GetAllNotes(studentId, lessonId);

        _studentRepo.MarkLessonAsStudied(studentId, lessonId);
        _studentRepo.GetCourseProgress(studentId, lesson.Chapter.CourseID);

        var viewModel = new LessonDetailsViewModel
        {
            LessonId = lesson.Id,
            LessonName = lesson.Name,
            MediaLink = lesson.MediaLink,
            Notes = studentNotes
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult SaveNote(int lessonId, int noteId, string noteContent)
    {
        var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (noteId == 0) // Add new note
        {
            _studentRepo.AddNoteToLesson(studentId, lessonId, noteContent);
        }
        else // Update existing note
        {
            _studentRepo.UpdateNote(noteId, noteContent);
        }

        return RedirectToAction("LessonDetails", new { lessonId });
    }

    [HttpPost]
    public IActionResult DeleteNote(int noteId)
    {
        _studentRepo.DeleteNote(noteId);
        return Redirect(Request.Headers["Referer"].ToString()); 
    }

}
