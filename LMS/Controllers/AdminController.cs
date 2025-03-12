using LMS.Models;
using LMS.Models.InstractourModel;
using LMS.Repositories.Interfaces;
using LMS.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace LMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ICourseRepository _courseRepo;
        private readonly IAdminRepository _adminRepo;
        private readonly UserManager<AppUser> _userManager;
        

        public AdminController(ICourseRepository courseRepo, IAdminRepository adminRepo, UserManager<AppUser> userManager)
        {
            _courseRepo= courseRepo;
            _adminRepo = adminRepo;
            _userManager = userManager;
        }

        // GET: AdminController
        [Route("Admin/Dashboard")]
        public async Task<ActionResult> Index()
        {
            ViewBag.TotalCourses =  _courseRepo.GetCourseCount();
            ViewBag.TotalStudents = (await _userManager.GetUsersInRoleAsync("Student")).Count();
            ViewBag.TotalInstructors = (await _userManager.GetUsersInRoleAsync("Instructor")).Count();
            ViewBag.TotalAdmins= (await _userManager.GetUsersInRoleAsync("Admin")).Count();

            string totalProf = _adminRepo.TotalProfits().ToString("C", new CultureInfo("en-EG"));
            string netProf = (_adminRepo.TotalProfits() * 10/100).ToString("C", new CultureInfo("en-EG"));
            
            ViewBag.TotalProfits = totalProf;

            ViewBag.netProfits = netProf;


            return View();
        }

        public async Task<IActionResult> AllStudnts()
        {
            
            return View(await _adminRepo.GetAllStudnts());
        }
        public async Task<IActionResult> AllInstructors()
        {
            
            return View(await _adminRepo.GetAllInstructors());
        }


        public async Task<IActionResult> PendingCourses()
        {
            return View(await _courseRepo.GetAllRequestedCourses());
        }   
        public async Task<IActionResult> RejectedCourses()
        {
            return View(await _courseRepo.GetAllRejectedCourses());
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
        [HttpPost]
        public async Task<IActionResult> RejectPendingCourse(int Id)
        {
            try
            {
                await _courseRepo.DeleteCourse(Id);
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



        //Charts
        public IActionResult GetChartData()
        {
            var data = _courseRepo.GetTopCourses();
            return Json(data);
        }


        public async Task<IActionResult> UserDetails(string Id)
        {
            return View(await _adminRepo.GetUserDetails(Id));
        }


    }
}
