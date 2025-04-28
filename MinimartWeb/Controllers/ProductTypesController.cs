using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MinimartWeb.Data;
using MinimartWeb.Model;

namespace MinimartWeb.Controllers
{
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

        // POST: ProductTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductTypeID,ProductName,ProductDescription,CategoryID,SupplierID,Price,Tags,StockAmount,MeasurementUnitID,ExpirationDurationDays,IsActive,DateAdded,ImagePath")] ProductType productType, IFormFile ImageUpload)
        {
            ModelState.Remove("Category");
            ModelState.Remove("Supplier");
            ModelState.Remove("MeasurementUnit");

            if (ModelState.IsValid)
            {
                // Handle image upload
                if (ImageUpload != null && ImageUpload.Length > 0)
                {
                    var fileName = Path.GetFileNameWithoutExtension(ImageUpload.FileName);
                    var fileExtension = Path.GetExtension(ImageUpload.FileName);
                    var newFileName = $"{Guid.NewGuid()}{fileExtension}";

                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products", newFileName);

                    using (var fileStream = new FileStream(uploadPath, FileMode.Create))
                    {
                        await ImageUpload.CopyToAsync(fileStream);
                    }

                    productType.ImagePath = $"/images/products/{newFileName}";
                }

                _context.Add(productType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Print model errors if invalid
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

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
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", productType.CategoryID);
            ViewData["MeasurementUnitID"] = new SelectList(_context.MeasurementUnits, "MeasurementUnitID", "UnitName", productType.MeasurementUnitID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "SupplierAddress", productType.SupplierID);
            return View(productType);
        }

        // POST: ProductTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductTypeID,ProductName,ProductDescription,CategoryID,SupplierID,Price,Tags,StockAmount,MeasurementUnitID,ExpirationDurationDays,IsActive,DateAdded,ImagePath")] ProductType productType)
        {
            if (id != productType.ProductTypeID)
            {
                return NotFound();
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
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", productType.CategoryID);
            ViewData["MeasurementUnitID"] = new SelectList(_context.MeasurementUnits, "MeasurementUnitID", "UnitName", productType.MeasurementUnitID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "SupplierAddress", productType.SupplierID);
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
