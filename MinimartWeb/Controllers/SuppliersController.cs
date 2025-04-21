using Microsoft.AspNetCore.Mvc;
using MinimartWeb.BOs;
using MinimartWeb.DAOs;
using MinimartWeb.Data;
using MinimartWeb.Model;

namespace MinimartWeb.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly SupplierBO _supplierBO;

        public SuppliersController(ApplicationDbContext context)
        {
            var dao = new SupplierDAO(context);
            _supplierBO = new SupplierBO(dao);
        }

        public async Task<IActionResult> Index()
        {
            var suppliers = await _supplierBO.GetAllAsync();
            return View(suppliers);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var supplier = await _supplierBO.GetByIdAsync(id.Value);
            return supplier == null ? NotFound() : View(supplier);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SupplierID,SupplierName,SupplierPhoneNumber,SupplierAddress,SupplierEmail")] Supplier supplier)
        {
            var (isValid, errors) = await _supplierBO.ValidateAsync(supplier);

            foreach (var error in errors)
                ModelState.AddModelError(string.Empty, error);

            if (!isValid) return View(supplier);

            await _supplierBO.AddAsync(supplier);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var supplier = await _supplierBO.GetByIdAsync(id.Value);
            return supplier == null ? NotFound() : View(supplier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SupplierID,SupplierName,SupplierPhoneNumber,SupplierAddress,SupplierEmail")] Supplier supplier)
        {
            if (id != supplier.SupplierID) return NotFound();

            var (isValid, errors) = await _supplierBO.ValidateAsync(supplier, isEdit: true);

            foreach (var error in errors)
                ModelState.AddModelError(string.Empty, error);

            if (!isValid) return View(supplier);

            try
            {
                await _supplierBO.UpdateAsync(supplier);
            }
            catch
            {
                return BadRequest("Could not update the supplier.");
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var supplier = await _supplierBO.GetByIdAsync(id.Value);
            return supplier == null ? NotFound() : View(supplier);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplier = await _supplierBO.GetByIdAsync(id);
            if (supplier != null)
                await _supplierBO.DeleteAsync(supplier);

            return RedirectToAction(nameof(Index));
        }
    }
}
