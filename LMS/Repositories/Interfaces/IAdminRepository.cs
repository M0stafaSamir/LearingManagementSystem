using LMS.Models;
using LMS.ViewModel;

namespace LMS.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        Task AddAdminUser(CreateAdminViewModel adminUser);
        Task DeleteAdminUser(string Id);
        Task<IEnumerable<AppUser>> GetAllAdmins();
        Task<IEnumerable<AppUser>> GetAllStudnts();
        Task<IEnumerable<AppUser>> GetAllInstructors();
        Task<AppUser> GetAdminById(string Id);

        decimal TotalProfits();
    }
}
