using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MinimartWeb.BOs;
using MinimartWeb.Model;
using System.Threading.Tasks;

namespace MinimartWeb.Controllers
{
    public class ProductTypesController : Controller
    {
        private readonly ProductTypeBO _bo;

        public ProductTypesController(ProductTypeBO bo)
        {
            _bo = bo;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _bo.GetAllAsync();
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _bo.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductType productType)
        {
            var result = await _bo.CreateAsync(productType);
            if (result.IsSuccess)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", result.Message);
            PopulateDropdowns();
            return View(productType);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _bo.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            PopulateDropdowns();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductType productType)
        {
            if (id != productType.ProductTypeID)
                return NotFound();

            var result = await _bo.UpdateAsync(id, productType);
            if (result.IsSuccess)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", result.Message);
            PopulateDropdowns();
            return View(productType);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _bo.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _bo.DeleteAsync(id);
            if (!result.IsSuccess)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        private void PopulateDropdowns()
        {
            ViewData["CategoryID"] = new SelectList(_bo.GetCategories(), "CategoryID", "CategoryName");
            ViewData["SupplierID"] = new SelectList(_bo.GetSuppliers(), "SupplierID", "SupplierName");
            ViewData["MeasurementUnitID"] = new SelectList(_bo.GetMeasurementUnits(), "MeasurementUnitID", "UnitName");
        }
    }
}
