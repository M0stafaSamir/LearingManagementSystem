using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMS.Data;
using LMS.Models;
//added
using System.Security.Claims;
using LMS.Repositories.Interfaces;

namespace LMS.Controllers
{
    public class CategoriesController : Controller
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICategoryRepository _categoryRepo;

        public CategoriesController( ICategoryRepository categoryRepo, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor; // Inject HttpContextAccessor
            _categoryRepo=categoryRepo;
        }

        // loggedIn User ID
        public string GetLoggedInUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            string id = GetLoggedInUserId();//id
            return View(await _categoryRepo.GetAllCategories());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int id)
        {

            var category = await _categoryRepo.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryRepo.Add(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
      
            var category = await _categoryRepo.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _categoryRepo.Update(category);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
         
            var category = await _categoryRepo.GetCategoryById(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _categoryRepo.GetCategoryById(id);
            if (category != null)
            {
                await _categoryRepo.Delete(category);
            }

           
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CategoryExists(int id)
        {
            return await _categoryRepo.GetCategoryById(id) != null;
        }
    }
}
