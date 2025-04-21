using Microsoft.AspNetCore.Mvc;
using MinimartWeb.BOs;
using MinimartWeb.DAOs;
using MinimartWeb.Data;
using MinimartWeb.Model;

namespace MinimartWeb.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly CategoryBO _categoryBO;

        public CategoriesController(ApplicationDbContext context)
        {
            var dao = new CategoryDAO(context);
            _categoryBO = new CategoryBO(dao);
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryBO.GetAllAsync();
            return View(categories);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _categoryBO.GetByIdAsync(id.Value);
            return category == null ? NotFound() : View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryID,CategoryName,CategoryDescription")] Category category)
        {
            var (success, errors) = await _categoryBO.AddAsync(category);
            if (success)
                return RedirectToAction(nameof(Index));

            foreach (var error in errors)
                ModelState.AddModelError(string.Empty, error);

            return View(category);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _categoryBO.GetByIdAsync(id.Value);
            return category == null ? NotFound() : View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryID,CategoryName,CategoryDescription")] Category category)
        {
            if (id != category.CategoryID)
                return NotFound();

            var (success, errors) = await _categoryBO.UpdateAsync(category);
            if (success)
                return RedirectToAction(nameof(Index));

            foreach (var error in errors)
                ModelState.AddModelError(string.Empty, error);

            return View(category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _categoryBO.GetByIdAsync(id.Value);
            return category == null ? NotFound() : View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoryBO.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
