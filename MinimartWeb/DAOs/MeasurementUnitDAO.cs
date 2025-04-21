using MinimartWeb.Data;
using MinimartWeb.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimartWeb.DAOs
{
    public class MeasurementUnitDAO
    {
        private readonly ApplicationDbContext _context;

        public MeasurementUnitDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MeasurementUnit>> GetAllAsync()
        {
            return await _context.MeasurementUnits.ToListAsync();
        }

        public async Task<MeasurementUnit?> GetByIdAsync(int id)
        {
            return await _context.MeasurementUnits.FindAsync(id);
        }

        public async Task AddAsync(MeasurementUnit unit)
        {
            _context.MeasurementUnits.Add(unit);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MeasurementUnit unit)
        {
            _context.MeasurementUnits.Update(unit);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var unit = await _context.MeasurementUnits.FindAsync(id);
            if (unit != null)
            {
                _context.MeasurementUnits.Remove(unit);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> UnitNameExistsAsync(string unitName, int? excludeId = null)
        {
            return await _context.MeasurementUnits
                .AnyAsync(m => m.UnitName == unitName && (!excludeId.HasValue || m.MeasurementUnitID != excludeId));
        }
    }
}
