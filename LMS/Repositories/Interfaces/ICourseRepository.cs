﻿using LMS.Models.InstractourModel;
using LMS.ViewModel.Inst;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllCourses();
        Task<IEnumerable<Course>> GetInstructorCourses(string Id);
        Task<IEnumerable<Course>> GetAllRequestedCourses();
        Task<IEnumerable<Course>> GetAllRejectedCourses();
        Task<IEnumerable<Course>> SearchCourseByName(string name);
        Task<Course> GetCourseById(int id);
        Task AddCourse(Course course);
        Task UpdateCourse(Course course, IFormFile Image);
        Task AcceptCourse(int id);
        Task DeleteCourse(int id);
        Task<IEnumerable<Course>> GetCoursesByCategoryId(int categoryId);
        Task<IEnumerable<Course>> GetCoursesByInstructorId(string instructorId);
        Task<IEnumerable<Course>> GetRequestedCoursesByInstructorId(string instructorId);
        Task<int> CreateCourse(CreateCourseViewModel course, IFormFile Image,string InstID);
        Task<IEnumerable<CourseIncomeViewModel>> GetCoursesProfit(string instructorId);
        public  Task<InstructorIncomeViewModel> GetInstructorCoursesProfit(string instructorId); 

        int GetCourseCount();

        //new for admin charts
        public IActionResult GetTopCourses();





    }
}
