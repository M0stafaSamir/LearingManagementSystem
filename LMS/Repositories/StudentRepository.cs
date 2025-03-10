using LMS.Data;
using LMS.Models;
using LMS.Models.InstractourModel;
using LMS.Models.StudentModels;
using Microsoft.EntityFrameworkCore;
using System;

public class StudentRepository : IStudentRepository
{
    private readonly ApplicationDbContext _context;

    public StudentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Course> GetAllCourses(string search = "", string category = "")
    {
        var query = _context.Courses
            .Where(c=>c.IsAccepted==true)
            .Where(c=>c.IsDeleted==false)
            .Include(c => c.Instructor);

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(c => c.Name.Contains(search)).Include(c=>c.Instructor);
        }

        if (!string.IsNullOrEmpty(category) && category != "All Categories")
        {
            query = query.Where(c => c.Category.Name == category).Include(c => c.Instructor);
        }

        return query.ToList();
    }

    IEnumerable<Category> IStudentRepository.GetAllCategories()
    {
        return _context.Categories.ToList();
    }
    public Course GetCourseDetails(int courseId)
    {
        return _context.Courses
            .Include(c => c.Chapters)
                .ThenInclude(ch => ch.Lessons)
            .Include(c => c.Instructor)
            .Include(c => c.Reviews)
                .ThenInclude(r => r.Student)
            .FirstOrDefault(c => c.Id == courseId);
    }
    public IEnumerable<Course> GetRecommendedCourses(string studentId)
    {
        var enrolledCategories = _context.studentEnrollCourses
            .Where(e => e.StudentId == studentId)
            .Select(e => e.Course.Category)
            .Distinct()
            .ToList();

        return _context.Courses
            .Where(c => enrolledCategories.Contains(c.Category) && !c.StudentEnrollments.Any(e => e.StudentId == studentId)).Include(C=>C.Instructor)
            .ToList();
    }


    public IEnumerable<Course> SearchCoursesByName(string name)
    {
        return _context.Courses.Include(c=>c.Instructor).Where(c => c.Name.Contains(name)).ToList();
    }

    public IEnumerable<Course> SearchCoursesByCategory(Category category)
    {
        return _context.Courses.Where(c => c.Category == category).Include(c=>c.Instructor).ToList();
    }

    public CertificateForStudent GetCertificateForCourse(string studentId, int courseId)
    {
        if (!_context.Certificates.Any(ec => ec.StudentId == studentId && ec.CourseId == courseId))
        {
            var certificate = new CertificateForStudent
            {
                StudentId = studentId,
                CourseId = courseId,
                AcquireDate = DateTime.Now,
                Grade = "Excellent"
            };
            _context.Certificates.Add(certificate);
            _context.SaveChanges();
            return certificate;
        }

        else return null;
    }
    public bool CheckCourseCompletion(string studentId, int courseId)
    {
        int totalLessons = _context.Lessons.Count(l => l.Chapter.CourseID == courseId);
        int completedLessons = _context.StudentStudiedLessons
            .Count(s => s.StudentId == studentId && s.Lesson.Chapter.CourseID == courseId);

        if (completedLessons == totalLessons && totalLessons > 0)
        {
            GetCertificateForCourse(studentId, courseId);
            return true; // Course completed
        }
        return false;
    }

    public IEnumerable<CertificateForStudent> GetAllCertificates(string studentId)
    {
        return _context.Certificates
                                   .Where(c => c.StudentId == studentId)
                                   .Include(c => c.Course)
                                   .ToList();
    }

    public void AddNoteToLesson(string studentId, int lessonId, string content)
    {
        var note = new Note
        {
            StudentId = studentId,
            LessonId = lessonId,
            Content = content,
            CreatedAt = DateTime.Now
        };
        _context.Notes.Add(note);
        _context.SaveChanges();
    }

    public IEnumerable<Note> GetAllNotes(string studentId, int lessonId)
    {
        return _context.Notes.Where(n => n.StudentId == studentId && n.LessonId==lessonId).ToList();
    }

    public void UpdateNote(int noteId, string newContent)
    {
        var note = _context.Notes.Find(noteId);
        if (note != null)
        {
            note.Content = newContent;
            _context.SaveChanges();
        }
    }

    public void DeleteNote(int noteId)
    {
        var note = _context.Notes.Find(noteId);
        if (note != null)
        {
            _context.Notes.Remove(note);
            _context.SaveChanges();
        }
    }

    public void PurchaseCourse(string studentId, int courseId)
    {
        if (!_context.Purchases.Any(ec => ec.StudentId == studentId && ec.CourseId == courseId))
        {
            var payment = new Purchase
            {
                StudentId = studentId,
                CourseId = courseId,
                PurchaseDate = DateTime.Now,
                AmountPaid = _context.Courses.Find(courseId)?.Price ?? 0
            };
            _context.Purchases.Add(payment);
            _context.SaveChanges();

            EnrollInCourse(studentId, courseId);
        }
        
    }

    public IEnumerable<Course> GetAllPurchases(string studentId)
    {
        return _context.Purchases
                   .Where(ec => ec.StudentId == studentId)
                   .Select(ec => ec.Course)
                   .ToList();
    }

    public void AddReview(ReviewedCourse reviewedCourse)
    {
        var review = new ReviewedCourse
        {
            StudentId = reviewedCourse.StudentId,
            CourseId = reviewedCourse.CourseId,
            Comment = reviewedCourse.Comment,
            Rating = reviewedCourse.Rating,
            DateTime = reviewedCourse.DateTime
        };
        _context.ReviewedCourses.Add(review);
        _context.SaveChanges();
    }

    public void UpdateReview(int reviewId, string newReviewText, int newRating)
    {
        var review = _context.ReviewedCourses.Find(reviewId);
        if (review != null)
        {
            review.Comment = newReviewText;
            review.Rating = newRating;
            _context.SaveChanges();
        }
    }

    public void DeleteReview(int reviewId)
    {
        var review = _context.ReviewedCourses.Find(reviewId);
        if (review != null)
        {
            _context.ReviewedCourses.Remove(review);
            _context.SaveChanges();
        }
    }

    public IEnumerable<ReviewedCourse> GetAllReviews(int courseId)
    {
        return _context.ReviewedCourses.Where(r => r.CourseId == courseId).Include(s=>s.Student).ToList();
    }

    public IEnumerable<StudentEnrollCourse> GetEnrolledCourses(string studentId)
    {
        return _context.studentEnrollCourses.Where(e => e.StudentId == studentId).Include(e => e.Course).ThenInclude(c=>c.Instructor).ToList();
    }

    public void EnrollInCourse(string studentId, int courseId)
    {
        
        _context.studentEnrollCourses.Add(new StudentEnrollCourse { StudentId = studentId, CourseId = courseId, EnrollmentDate = DateTime.Now });
        _context.SaveChanges();
        
    }

    public void RemoveEnrollment(string studentId, int courseId)
    {
        var enrollment = _context.studentEnrollCourses.FirstOrDefault(sc => sc.StudentId == studentId && sc.CourseId == courseId);
        if (enrollment != null)
        {
            _context.studentEnrollCourses.Remove(enrollment);
            _context.SaveChanges();
        }
    }
    public bool HasPurchasedCourse(int courseId, string studentId)
    {
        return _context.studentEnrollCourses.Any(e => e.CourseId == courseId && e.StudentId == studentId);
    }
    public IEnumerable<Lesson> GetAllLessonsInCourse(int courseId)
    {
        return _context.Lessons.Where(l => l.ChapterID == courseId).ToList();
    }

    public void MarkLessonAsStudied(string studentId, int lessonId)
    {
        if (!_context.StudentStudiedLessons.Any(s => s.StudentId == studentId && s.LessonId == lessonId))
        {
            _context.StudentStudiedLessons.Add(new StudentStudiedLesson
            {
                StudentId = studentId,
                LessonId = lessonId,
            });
            _context.SaveChanges();
        }

    }
    public Lesson GetLessonDetails(int lessonId, string studentId)
    {
        var lesson = _context.Lessons
            .Where(l => l.Id == lessonId).Include(c=>c.Chapter).ThenInclude(c=>c.Course)
            .Select(l => new Lesson
            {
                Id = l.Id,
                Name = l.Name,
                MediaLink = l.MediaLink,
                ChapterID = l.ChapterID,
                Chapter = l.Chapter,
                Notes = _context.Notes
                    .Where(n => n.LessonId == lessonId && n.StudentId == studentId)
                    .Select(n => new Note
                    {
                        Id = n.Id,
                        Content = n.Content,
                        CreatedAt = n.CreatedAt
                    }).ToList()
            }).FirstOrDefault();
        return lesson;
    }

    public double GetCourseProgress(string studentId, int courseId)
    {
        int totalLessons = _context.Lessons.Count(l => l.Chapter.CourseID == courseId);
        int completedLessons = _context.StudentStudiedLessons
            .Count(s => s.StudentId == studentId && s.Lesson.Chapter.CourseID == courseId);

        return totalLessons == 0 ? 0:(completedLessons / (double)totalLessons) * 100 ;
    }

    public void AddToWishlist(string studentId, int courseId)
    {
        if (!_context.WishLists.Any(w => w.StudentId == studentId && w.WishedCourseId == courseId))
        {
            _context.WishLists.Add(new WishList { StudentId = studentId, WishedCourseId = courseId });
            _context.SaveChanges();
        }
    }

    public IEnumerable<WishList> GetWishlist(string studentId)
    {
        return _context.WishLists.Where(w => w.StudentId == studentId).Include(w => w.WishedCourse).ThenInclude(c=>c.Instructor).ToList();
    }

    public void RemoveFromWishlist(string studentId, int courseId)
    {
        var wishlistItem = _context.WishLists.FirstOrDefault(w => w.StudentId == studentId && w.WishedCourseId == courseId);
        if (wishlistItem != null)
        {
            _context.WishLists.Remove(wishlistItem);
            _context.SaveChanges();
        }
    }

    
}
