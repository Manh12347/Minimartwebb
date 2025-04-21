using MinimartWeb.Data;
using MinimartWeb.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimartWeb.DAOs
{
    public class ProductTypeDAO
    {
        private readonly ApplicationDbContext _context;

        public ProductTypeDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductType>> GetAllAsync()
        {
            return await _context.ProductTypes
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Include(p => p.MeasurementUnit)
                .ToListAsync();
        }

        public async Task<ProductType?> GetByIdAsync(int id)
        {
            return await _context.ProductTypes
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Include(p => p.MeasurementUnit)
                .FirstOrDefaultAsync(p => p.ProductTypeID == id);
        }

        public async Task AddAsync(ProductType productType)
        {
            _context.ProductTypes.Add(productType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductType productType)
        {
            _context.ProductTypes.Update(productType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ProductType productType)
        {
            _context.ProductTypes.Remove(productType);
            await _context.SaveChangesAsync();
        }

        public bool ProductTypeExists(int id)
        {
            return _context.ProductTypes.Any(e => e.ProductTypeID == id);
        }

        public bool ProductNameExists(string name, int? excludeId = null)
        {
            return _context.ProductTypes.Any(e => e.ProductName == name && (excludeId == null || e.ProductTypeID != excludeId));
        }

        public List<Category> GetCategories() => _context.Categories.ToList();
        public List<Supplier> GetSuppliers() => _context.Suppliers.ToList();
        public List<MeasurementUnit> GetMeasurementUnits() => _context.MeasurementUnits.ToList();

        public bool CategoryExists(int categoryId) => _context.Categories.Any(c => c.CategoryID == categoryId);
        public bool SupplierExists(int supplierId) => _context.Suppliers.Any(s => s.SupplierID == supplierId);
        public bool MeasurementUnitExists(int unitId) => _context.MeasurementUnits.Any(u => u.MeasurementUnitID == unitId);
    }
}
