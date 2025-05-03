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
    public class ProductTagsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductTagsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProductTags
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProductTags.Include(p => p.ProductType).Include(p => p.Tag);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProductTags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTag = await _context.ProductTags
                .Include(p => p.ProductType)
                .Include(p => p.Tag)
                .FirstOrDefaultAsync(m => m.ProductTagID == id);
            if (productTag == null)
            {
                return NotFound();
            }

            return View(productTag);
        }

        // GET: ProductTags/Create
        public IActionResult Create()
        {
            ViewData["ProductTypeID"] = new SelectList(_context.ProductTypes, "ProductTypeID", "ProductDescription");
            ViewData["TagID"] = new SelectList(_context.Tags, "TagID", "TagName");
            return View();
        }

        // POST: ProductTags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductTagID,ProductTypeID,TagID")] ProductTag productTag)
        {

            ModelState.Remove(nameof(ProductTag.ProductType));
            ModelState.Remove(nameof(ProductTag.Tag));

            if (ModelState.IsValid)
            {
                // Check if the combination of ProductTypeID and TagID already exists
                var existingProductTag = await _context.ProductTags
                    .FirstOrDefaultAsync(pt => pt.ProductTypeID == productTag.ProductTypeID && pt.TagID == productTag.TagID);

                if (existingProductTag != null)
                {
                    // If it exists, add a model error to notify the user
                    ModelState.AddModelError(string.Empty, "This combination of Product Type and Tag already exists.");
                }
                else
                {
                    _context.Add(productTag);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            // If validation failed or there was an error, rebind the SelectLists for dropdowns
            ViewData["ProductTypeID"] = new SelectList(_context.ProductTypes, "ProductTypeID", "ProductDescription", productTag.ProductTypeID);
            ViewData["TagID"] = new SelectList(_context.Tags, "TagID", "TagName", productTag.TagID);

            return View(productTag);
        }


        // GET: ProductTags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTag = await _context.ProductTags.FindAsync(id);
            if (productTag == null)
            {
                return NotFound();
            }
            ViewData["ProductTypeID"] = new SelectList(_context.ProductTypes, "ProductTypeID", "ProductDescription", productTag.ProductTypeID);
            ViewData["TagID"] = new SelectList(_context.Tags, "TagID", "TagName", productTag.TagID);
            return View(productTag);
        }

        // POST: ProductTags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductTagID,ProductTypeID,TagID")] ProductTag productTag)
        {
            if (id != productTag.ProductTagID)
            {
                return NotFound();
            }

            ModelState.Remove(nameof(ProductTag.ProductType));
            ModelState.Remove(nameof(ProductTag.Tag));

            if (ModelState.IsValid)
            {
                // Check if the combination of ProductTypeID and TagID already exists, excluding the current ProductTagID
                var existingProductTag = await _context.ProductTags
                    .FirstOrDefaultAsync(pt => pt.ProductTypeID == productTag.ProductTypeID && pt.TagID == productTag.TagID && pt.ProductTagID != id);

                if (existingProductTag != null)
                {
                    // If it exists, add a model error to notify the user
                    ModelState.AddModelError(string.Empty, "This combination of Product Type and Tag already exists.");
                }
                else
                {
                    try
                    {
                        _context.Update(productTag);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProductTagExists(productTag.ProductTagID))
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
            }
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            // If validation failed or there was an error, rebind the SelectLists for dropdowns
            ViewData["ProductTypeID"] = new SelectList(_context.ProductTypes, "ProductTypeID", "ProductDescription", productTag.ProductTypeID);
            ViewData["TagID"] = new SelectList(_context.Tags, "TagID", "TagName", productTag.TagID);

            return View(productTag);
        }


        // GET: ProductTags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTag = await _context.ProductTags
                .Include(p => p.ProductType)
                .Include(p => p.Tag)
                .FirstOrDefaultAsync(m => m.ProductTagID == id);
            if (productTag == null)
            {
                return NotFound();
            }

            return View(productTag);
        }

        // POST: ProductTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productTag = await _context.ProductTags.FindAsync(id);
            if (productTag != null)
            {
                _context.ProductTags.Remove(productTag);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductTagExists(int id)
        {
            return _context.ProductTags.Any(e => e.ProductTagID == id);
        }
    }
}
