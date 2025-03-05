using LMS.Models;

namespace LMS.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        Task AddAdminUser(AppUser user);
        Task<IEnumerable<AppUser>> GetAllAdmins();
    }
}
