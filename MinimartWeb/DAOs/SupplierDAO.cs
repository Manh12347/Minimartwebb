using MinimartWeb.Data;
using MinimartWeb.Model;
using Microsoft.EntityFrameworkCore;

namespace MinimartWeb.DAOs
{
    public class SupplierDAO
    {
        private readonly ApplicationDbContext _context;

        public SupplierDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Supplier>> GetAllAsync()
        {
            return await _context.Suppliers.ToListAsync();
        }

        public async Task<Supplier?> GetByIdAsync(int id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        public async Task AddAsync(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Supplier supplier)
        {
            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Supplier supplier)
        {
            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
        {
            return await _context.Suppliers
                .AnyAsync(s => s.SupplierName == name && (excludeId == null || s.SupplierID != excludeId));
        }

        public async Task<bool> ExistsByEmailAsync(string email, int? excludeId = null)
        {
            return await _context.Suppliers
                .AnyAsync(s => s.SupplierEmail == email && (excludeId == null || s.SupplierID != excludeId));
        }
    }
}
