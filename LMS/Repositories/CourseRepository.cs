﻿using LMS.Data;
using LMS.Models;
using LMS.Models.InstractourModel;
using LMS.Repositories.Interfaces;
using LMS.ViewModel.Inst;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace LMS.Repositories
{
    public class CourseRepository : Controller, ICourseRepository //valid?
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment; 
        public CourseRepository(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;   
        }
        public async Task AddCourse(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCourse(int id)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if (course != null)
            {
                course.IsDeleted=true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Course>> GetAllCourses()
        {
            return await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .Where(c => c.IsAccepted == true)
                .ToListAsync();
        }
        public async Task<IEnumerable<Course>> GetInstructorCourses(string Id)
        {
            return await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .Where(c => c.IsAccepted == true)
                .Where(c => c.InstructorId == Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetAllRequestedCourses()
        {
            return await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .Where(c => c.IsAccepted == false && c.IsDeleted==false)
                .ToListAsync();
        }
        public async Task<Course> GetCourseById(int id)
        {
            return await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public int GetCourseCount()
        {
            return _context.Courses.Count();
        }

        public async Task<IEnumerable<Course>> GetCoursesByCategoryId(int categoryId)
        {
            return await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .Where(c => c.CategoryId == categoryId).ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetCoursesByInstructorId(string instructorId)
        {
            return await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .Where(c => c.IsAccepted == true)
                .ToListAsync();
        }


        public async Task<IEnumerable<Course>> GetRequestedCoursesByInstructorId(string instructorId)
        {
            return await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .Where(c=>c.InstructorId==instructorId)
                .Where(c => c.IsAccepted == false).ToListAsync();
        }



        public async Task<IEnumerable<Course>> SearchCourseByName(string name)
        {
            return await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .Where(c => c.Name == name).ToListAsync();
        }


        public async Task UpdateCourse(Course course, IFormFile Image)
        {
            try
            {
                if (Image != null && Image.Length > 0)
                {
                    var fileName = Path.GetFileNameWithoutExtension(Image.FileName);
                    var extension = Path.GetExtension(Image.FileName);
                    var uniqueFileName = fileName + "_" + Guid.NewGuid().ToString() + extension;

                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images/courses", uniqueFileName);

                    if (!Directory.Exists(Path.Combine(_webHostEnvironment.WebRootPath, "images/courses")))
                    {
                        Directory.CreateDirectory(Path.Combine(_webHostEnvironment.WebRootPath, "images/courses"));
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(fileStream);
                    }

                    course.Image = uniqueFileName;
                }

                _context.Courses.Update(course);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while uploading the image: {ex.Message}");
            }
        }




        public async Task AcceptCourse(int id)
        {
            var accCourse = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if (accCourse != null)
            {
                accCourse.IsAccepted = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> CreateCourse(CreateCourseViewModel course, IFormFile Image, string InstID)
        {
            try
            {

                if (Image != null && Image.Length > 0)
                {
                    var fileName = Path.GetFileNameWithoutExtension(Image.FileName);
                    var extension = Path.GetExtension(Image.FileName);
                    var uniqueFileName = fileName + "_" + Guid.NewGuid().ToString() + extension;

                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images/courses", uniqueFileName);

                    if (!Directory.Exists(Path.Combine(_webHostEnvironment.WebRootPath, "images/courses")))
                    {
                        Directory.CreateDirectory(Path.Combine(_webHostEnvironment.WebRootPath, "images/courses"));
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(fileStream);
                    }

                    course.Image = uniqueFileName;
                }

                var CreatedCourse = new Course
                {

                    CategoryId = course.CategoryId,
                    Name = course.Name,
                    Description = course.Description,
                    Price = course.Price,
                    Image = course.Image,
                    InstructorId = InstID
                };
                _context.Add(CreatedCourse);
                
                await _context.SaveChangesAsync();

                return CreatedCourse.Id;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred while uploading the image: {ex.Message}");
                return -1;
            }


         
        }

        public async Task<IEnumerable<Course>> GetAllRejectedCourses()
        {
            return await _context.Courses
               .Include(c => c.Category)
               .Include(c => c.Instructor)
               .Where(c => c.IsDeleted == true)
               .ToListAsync();
        }

        //chart
        public IActionResult GetTopCourses()
        {
            var topCourses = _context.studentEnrollCourses
           .GroupBy(e => e.CourseId) 
           .Select(group => new
           {
               CourseId = group.Key,
               TotalStudents = group.Count() ,
               CourseName= (_context.Courses.FirstOrDefault(c=>c.Id==group.Key)).Name,
           })
           .OrderByDescending(c => c.TotalStudents) 
           .Take(6) 
           .ToList();

            return Json(topCourses); 
        }

        public async Task<IEnumerable<CourseIncomeViewModel>> GetCoursesProfit(string instructorId)
        {
            return await _context.Courses
                .Where(c => c.InstructorId == instructorId)
                .Include(c=>c.Category)
                .Include(c=> c.Instructor)
                .Select(c => new CourseIncomeViewModel
                {
                    courseID = c.Id,  
                    CourseName = c.Name,  
                    Amount = c.Purchases.Sum(p => p.AmountPaid), 
                    StudentsCount = c.StudentEnrollments.Count(),
                    CategoryName = c.Category.Name,
                    InstructorName = c.Instructor.Name,
                })
                .ToListAsync();
        }



        public async Task<InstructorIncomeViewModel> GetInstructorCoursesProfit(string instructorId)
        {
            var courses = await _context.Courses
                                       .Where(c => c.InstructorId == instructorId)
                                       .Include(c => c.Purchases) 
                                       .Include(c => c.StudentEnrollments) 
                                       .ToListAsync();
            var totalIncome = courses.Sum(c => c.Purchases.Sum(p => p.AmountPaid)); 
            var coursesCount = courses.Count(); 
            var studentsCount = courses.Sum(c => c.StudentEnrollments.Count());
            var instructorIncome = await _context.Courses
                .Where(c => c.InstructorId == instructorId)
                .Include(c => c.Purchases) 
                .Include(c => c.StudentEnrollments)
                .Select(c => new InstructorIncomeViewModel
                {
                    InstID = instructorId,
                    InstName = c.Instructor.Name,
                    CoursesCount = coursesCount,                  
                    TotalIncome = totalIncome,
                    StudentsCount = studentsCount
                })
                .FirstOrDefaultAsync(); 

            return instructorIncome;
        }
    }
}
