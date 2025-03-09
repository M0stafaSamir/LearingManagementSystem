using System.Diagnostics;
using LMS.Models;
using LMS.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{


    //[Authorize(Roles = "Student")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryRepository _categoryRepo;

     

        public HomeController(ILogger<HomeController> logger, ICategoryRepository categoryRepo)
        {
            _logger = logger;
            _categoryRepo = categoryRepo;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _categoryRepo.GetAllCategories());
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
