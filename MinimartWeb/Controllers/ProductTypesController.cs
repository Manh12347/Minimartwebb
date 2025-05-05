using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MinimartWeb.Data;
using MinimartWeb.Model;

namespace MinimartWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProductTypes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProductTypes.Include(p => p.Category).Include(p => p.MeasurementUnit).Include(p => p.Supplier);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProductTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productType = await _context.ProductTypes
                .Include(p => p.Category)
                .Include(p => p.MeasurementUnit)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductTypeID == id);
            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }

        // GET: ProductTypes/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName");
            ViewData["MeasurementUnitID"] = new SelectList(_context.MeasurementUnits, "MeasurementUnitID", "UnitName");
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "SupplierName"); // FIXED: use SupplierName
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductTypeID,ProductName,ProductDescription,CategoryID,SupplierID,Price,StockAmount,MeasurementUnitID,ExpirationDurationDays,IsActive,DateAdded,ImagePath")] ProductType productType, IFormFile ImageUpload)
        {
            if (await _context.ProductTypes.AnyAsync(p => p.ProductName == productType.ProductName))
            {
                ModelState.AddModelError("ProductName", "Product Name already exists.");
            }

            ModelState.Remove("ImagePath");
            ModelState.Remove("Category");
            ModelState.Remove("MeasurementUnit");
            ModelState.Remove("Supplier");

            // Image upload logic
            if (ImageUpload != null && ImageUpload.Length > 0)
            {
                var fileExtension = Path.GetExtension(ImageUpload.FileName);
                var newFileName = $"{Guid.NewGuid()}{fileExtension}";
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products", newFileName);

                using (var fileStream = new FileStream(uploadPath, FileMode.Create))
                {
                    await ImageUpload.CopyToAsync(fileStream);
                }

                productType.ImagePath = newFileName;
            }

            // Only require image if not already set
            if (string.IsNullOrEmpty(productType.ImagePath))
            {
                ModelState.AddModelError("ImagePath", "Product image is required.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(productType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // If model state is not valid, log errors for debugging
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            // If invalid, reload the drop-down lists and show validation errors
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", productType.CategoryID);
            ViewData["MeasurementUnitID"] = new SelectList(_context.MeasurementUnits, "MeasurementUnitID", "UnitName", productType.MeasurementUnitID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "SupplierName", productType.SupplierID);

            return View(productType);
        }

        // GET: ProductTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productType = await _context.ProductTypes.FindAsync(id);
            if (productType == null)
            {
                return NotFound();
            }

            // Match Create action's SelectList naming
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", productType.CategoryID);
            ViewData["MeasurementUnitID"] = new SelectList(_context.MeasurementUnits, "MeasurementUnitID", "UnitName", productType.MeasurementUnitID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "SupplierName", productType.SupplierID); // FIXED: SupplierName instead of SupplierAddress
            return View(productType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductTypeID,ProductName,ProductDescription,CategoryID,SupplierID,Price,StockAmount,MeasurementUnitID,ExpirationDurationDays,IsActive,DateAdded,ImagePath")] ProductType productType, IFormFile ImageUpload)
        {
            if (id != productType.ProductTypeID)
            {
                return NotFound();
            }

            // Match Create action's ModelState.Remove calls
            ModelState.Remove("Category");
            ModelState.Remove("Supplier");
            ModelState.Remove("MeasurementUnit");
            ModelState.Remove("ImageUpload");

            // Duplicate name check (same as Create)
            if (await _context.ProductTypes.AnyAsync(p => p.ProductName == productType.ProductName && p.ProductTypeID != productType.ProductTypeID))
            {
                ModelState.AddModelError("ProductName", "Product Name already exists.");
            }

            // Image upload logic (same as Create)
            if (ImageUpload != null && ImageUpload.Length > 0)
            {
                var fileExtension = Path.GetExtension(ImageUpload.FileName);
                var newFileName = $"{Guid.NewGuid()}{fileExtension}";
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products", newFileName);

                using (var fileStream = new FileStream(uploadPath, FileMode.Create))
                {
                    await ImageUpload.CopyToAsync(fileStream);
                }

                productType.ImagePath = newFileName; // Match Create's path format
            }

            // Validation (same as Create)
            if (string.IsNullOrEmpty(productType.ImagePath))
            {
                ModelState.AddModelError("ImagePath", "Product image is required.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductTypeExists(productType.ProductTypeID))
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

            // Reload dropdowns on error (same as Create)
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", productType.CategoryID);
            ViewData["MeasurementUnitID"] = new SelectList(_context.MeasurementUnits, "MeasurementUnitID", "UnitName", productType.MeasurementUnitID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "SupplierName", productType.SupplierID);

            return View(productType);
        }


        // GET: ProductTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productType = await _context.ProductTypes
                .Include(p => p.Category)
                .Include(p => p.MeasurementUnit)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductTypeID == id);
            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }

        // POST: ProductTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productType = await _context.ProductTypes.FindAsync(id);
            if (productType != null)
            {
                _context.ProductTypes.Remove(productType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductTypeExists(int id)
        {
            return _context.ProductTypes.Any(e => e.ProductTypeID == id);
        }
    }
}
