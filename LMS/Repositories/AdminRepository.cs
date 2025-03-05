using LMS.Data;
using LMS.Models;
using LMS.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repositories
{
    public class AdminRepository:IAdminRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AdminRepository(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task AddAdminUser(AppUser user)
        {
            if (user != null)
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                await _userManager.AddToRoleAsync(user, "Admin");
                await _context.SaveChangesAsync();

            }

        }

        public async Task<IEnumerable<AppUser>> GetAllAdmins()
        {
            var Admins = await _userManager.GetUsersInRoleAsync("Admin");
            return Admins.ToList();
        }
    }
}
