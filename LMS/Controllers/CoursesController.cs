using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMS.Data;
using LMS.Models.InstractourModel;
using LMS.Repositories.Interfaces;
using System.Security.Claims;
using LMS.ViewModel.Inst;

namespace LMS.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICourseRepository _courseRepository;
        private readonly IChapterRepository _chapterRepository;
        private readonly ILessonRepository _lessonRepository; 

        public CoursesController(ApplicationDbContext context
            ,ICourseRepository courseRepository
            , IChapterRepository chapterRepository
            , ILessonRepository lessonRepository)
        {
            _context = context;
            _courseRepository = courseRepository;
            _chapterRepository = chapterRepository;
            _lessonRepository = lessonRepository; 
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var courses = _courseRepository.GetInstructorCourses(userId);   
            return View(await courses);
        }
  
        public async Task<IActionResult> PendingCourses()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var courses = _courseRepository.GetRequestedCoursesByInstructorId(userId);
            return View(await courses);
        }

        public async Task<IActionResult> Details(int id)
        {
            var course = await _courseRepository.GetCourseById(id);
            var Chapters = await _chapterRepository.GetChaptersByCourseId(id); 
            if (course == null)
            {
                return NotFound();
            }
            ViewBag.chapters = Chapters; 
            return View(course);
        }

        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Price,Image,CategoryId")] CreateCourseViewModel CreatedCourse, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                int CourseId= await _courseRepository.CreateCourse(CreatedCourse, Image, userId); 
                //return RedirectToAction(nameof(Details));
                return RedirectToAction(nameof(Details), new {id = CourseId });
            }
         
            return RedirectToAction(nameof(CreateChapter));
        }



        public async Task<IActionResult> CreateChapter(int id)
        {
            //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //var courses = await _courseRepository.GetInstructorCourses(userId);
            //ViewData["CourseID"] = new SelectList(courses, "Id", "Name");
            ViewBag.id = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateChapter([Bind("Name,Title,CourseID")] Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                ViewBag.CourseID = chapter.CourseID; 
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var chapterId= await _chapterRepository.AddChapter(chapter); 
                return RedirectToAction(nameof(CreateLesson), new { chId = chapterId });
            }
            return RedirectToAction(nameof(CreateChapter), chapter);
        }



        //continue
        public async Task<IActionResult> CreateLesson(int chId)
        {
            ViewBag.chapterId= chId;
            var Chapter= await _chapterRepository.GetChapterById(chId);
            ViewBag.CourseId = Chapter.CourseID;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLesson(Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _lessonRepository.AddLesson(lesson);
                return RedirectToAction(nameof(Details), new {id=lesson.CourseId});
            }
            return RedirectToAction(nameof(CreateLesson), lesson);
        }



        public async Task<IActionResult> Edit(int id)
        {
            var chapters = await _chapterRepository.GetChaptersByCourseId(id);
            var course = await _courseRepository.GetCourseById(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", course.CategoryId);
            ViewData["InstructorId"] = new SelectList(_context.Users, "Id", "Id", course.InstructorId);
            ViewBag.chapters = chapters;
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Image,IsAccepted,IsDeleted,CategoryId,InstructorId")] Course course, IFormFile Image)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _courseRepository.UpdateCourse(course, Image);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", course.CategoryId);
            ViewData["InstructorId"] = new SelectList(_context.Users, "Id", "Id", course.InstructorId);
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> DeleteChapter(int id)
        {

            var chapter  = await _chapterRepository.GetChapterById(id);
            var courseid = chapter!=null ? chapter.CourseID : -1 ;
            if (chapter != null)
            {
                await _chapterRepository.DeleteChapter(id);
            }
            else
            {
                return NotFound(); 
            }
            return RedirectToAction("Edit", new { id = courseid });
        }

        public async Task<IActionResult> DeleteLesson(int id)
        {
            var lesson = await _lessonRepository.GetLessonByID(id); 
            var chapterId = lesson!=null ? lesson.CourseId : -1;
            if (lesson != null)
            {
                await _lessonRepository.DeleteLessonByID(id);
            }
            else
            {
                return NotFound(); 
            }

            return RedirectToAction("Edit", new { id = chapterId });   
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
