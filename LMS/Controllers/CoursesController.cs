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

        public async Task<IActionResult> Details(int id)
        {

            var course = await _courseRepository.GetCourseById(id); 
            if (course == null)
            {
                return NotFound();
            }

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

                await _courseRepository.CreateCourse(CreatedCourse, Image, userId); 
                return RedirectToAction(nameof(CreateChapter));
            }
         
            return RedirectToAction(nameof(CreateChapter));
        }



        public async Task<IActionResult> CreateChapter()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var courses = await _courseRepository.GetInstructorCourses(userId);
            ViewData["CourseID"] = new SelectList(courses, "Id", "Name");
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
                await _chapterRepository.AddChapter(chapter); 
                return RedirectToAction(nameof(CreateLesson));
            }
            return RedirectToAction(nameof(Index));
        }



        //continue
        public async Task<IActionResult> CreateLesson()
        {
            dynamic CourseID = ViewBag.CourseID; 
            var chapters = await _chapterRepository.GetChaptersByCourseId(CourseID);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var courses = await _courseRepository.GetInstructorCourses(userId);
            ViewData["CourseID"] = new SelectList(courses, "Id", "Name");
            ViewData["CourseID"] = new SelectList(courses, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLesson([Bind("Name,Title,CourseID")] Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _lessonRepository.AddLesson(lesson);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", course.CategoryId);
            ViewData["InstructorId"] = new SelectList(_context.Users, "Id", "Id", course.InstructorId);
            return View(course);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Image,IsAccepted,IsDeleted,CategoryId,InstructorId")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
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

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
