using LMS.Models;
using LMS.Models.InstractourModel;
using LMS.Repositories.Interfaces;
using LMS.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
    public class AdminController : Controller
    {
        private readonly ICourseRepository _courseRepo;
        private readonly IAdminRepository _adminRepo;

        public AdminController(ICourseRepository courseRepo, IAdminRepository adminRepo)
        {
            _courseRepo= courseRepo;
            _adminRepo = adminRepo;
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




        // GET: AdminController/AllAdmins/
        public async Task<IActionResult> AllAdmins()
        {
            return View(await _adminRepo.GetAllAdmins());
        }



        // GET: Create Admin Account
        public async Task<IActionResult> CreateAdmin()
        {
                return View();
        }

        // POST: Create Admin Account
        [HttpPost]
        public async Task<IActionResult> CreateAdmin(CreateAdminViewModel admin)
        {
            try
            {
                await _adminRepo.AddAdminUser(admin);
                return RedirectToAction("AllAdmins");
            }
            catch
            {
                return View("CreateAdmin", admin);
            }
        }




        // GET: Delete Admin Account
        public async Task<IActionResult> DeleteAdmin(string id)
        {
            return View(await _adminRepo.GetAdminById(id));
        }

        // POST: Create Admin Account
        [HttpPost]
        public async Task<IActionResult> RemoveAdmin(string Id)
        {
            try
            {
                await _adminRepo.DeleteAdminUser(Id);
                return RedirectToAction("AllAdmins");
            }
            catch
            {
                return View("DeleteAdmin", Id);
            }
        }

    }
}
