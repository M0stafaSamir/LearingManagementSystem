using LMS.Models.InstractourModel;
using LMS.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
    public class AdminController : Controller
    {
        private readonly ICourseRepository _courseRepo;

        public AdminController(ICourseRepository courseRepo)
        {
            _courseRepo= courseRepo;
        }

        // GET: AdminController
        public ActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> PendingCourses()
        {
            return View(await _courseRepo.GetAllRequestedCourses());
        }

        // GET: AdminController/PendingCourseDetails/5
        public async Task<IActionResult> PendingCourseDetails(int id)
        {
            return View( await _courseRepo.GetCourseById(id));
        }


        // POST: update course from IsAccepted = false to true
        [HttpPost]
        public async Task<IActionResult> AcceptPendingCourse(int Id)
        {
            try
            {
                await _courseRepo.AcceptCourse(Id);
                return RedirectToAction("PendingCourses");
            }
            catch
            {
                return View("PendingCourseDetails", Id);
            }
        }

    }
}
