using System.Diagnostics;
using LMS.Models;
using LMS.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{


    //[Authorize(Roles = "Student")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryRepository _categoryRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICourseRepository _courseRepo;

        



        public HomeController(ILogger<HomeController> logger, ICategoryRepository categoryRepo, UserManager<AppUser> userManager, ICourseRepository courseRepo)
        {
            _logger = logger;
            _categoryRepo = categoryRepo;
            _userManager= userManager;
            _courseRepo= courseRepo;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.StudentsCount = (await _userManager.GetUsersInRoleAsync("Student")).Count;
            ViewBag.InstructorsCount = (await _userManager.GetUsersInRoleAsync("Instructor")).Count;
            ViewBag.CoursesCount =  _courseRepo.GetCourseCount();
            return View(await _categoryRepo.GetAllCategories());
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
