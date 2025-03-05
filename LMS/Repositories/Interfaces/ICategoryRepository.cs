using LMS.Models;
using LMS.Models.InstractourModel;

namespace LMS.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task Add(Category category);
        Task Update(Category category);
        Task Delete(Category category);
        Task<Category> GetCategoryById(int id);
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category> SearchCategoryByName(string name);
        int GetCategoryCount();
    }
}
