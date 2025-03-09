using LMS.Data;
using LMS.Models;
using LMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context; 
        }
        public async Task Add(Category category)
        {
           await _context.Categories.AddAsync(category);
           await _context.SaveChangesAsync(); 
        }

        public async Task Delete(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync(); 
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _context.Categories
            .Include(cat => cat.Courses)
            .ThenInclude(course => course.Chapters)  
            .ThenInclude(chapter => chapter.Lessons)  
            .Include(cat => cat.Courses)
            .ThenInclude(course => course.Instructor)  
            .ToListAsync();

        }

        public async Task<Category> GetCategoryById(int id)
        {
            return  await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);    
        }

        public int GetCategoryCount()
        {
            return _context.Categories.Count(); 
        }

        public async Task<Category> SearchCategoryByName(string name)
        {
            return  await _context.Categories.FirstOrDefaultAsync(c=>c.Name==name);
        }

        public async Task Update(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync(); 
        }
    }
}
