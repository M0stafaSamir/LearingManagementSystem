using LMS.Data;
using LMS.Models;
using LMS.Repositories.Interfaces;
using LMS.ViewModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repositories
{
    public class AdminRepository:IAdminRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public AdminRepository(ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager= roleManager;  
        }

        public async Task AddAdminUser(CreateAdminViewModel admin)
        {
            var adminUser = new AppUser
            {
                Name = admin.FullName,
                Email = admin.Email,
                EmailConfirmed = true,
                UserName = admin.UserName,
            };
            var result = await _userManager.CreateAsync(adminUser, admin.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "Admin");


            }
        }

        public async Task DeleteAdminUser(string Id)
        {
            var adminUser = await _userManager.FindByIdAsync(Id);
            if (adminUser != null) 
            {
                var result = await _userManager.DeleteAsync(adminUser);

            }

        }

        public async Task<AppUser> GetAdminById(string Id)
        {
            return await _userManager.FindByIdAsync(Id);
        }

        public async Task<IEnumerable<AppUser>> GetAllAdmins()
        {
            var Admins = await _userManager.GetUsersInRoleAsync("Admin");
            return Admins.ToList();
        }

        public async Task<IEnumerable<AppUser>> GetAllStudnts()
        {
            var Students = await _userManager.GetUsersInRoleAsync("Student");
            return Students.ToList();

        }  public async Task<IEnumerable<AppUser>> GetAllInstructors()
        {
            var Instructors = await _userManager.GetUsersInRoleAsync("Instructor");
            return Instructors.ToList();
        }

        public decimal TotalProfits()
        {
            
            return _context.Purchases.Sum(p => p.AmountPaid); ;
        }

        public async Task<AppUser> GetUserDetails(string Id)
        {
            return await _context.AppUsers.FirstOrDefaultAsync(u=>u.Id==Id);

        }
    }
}
