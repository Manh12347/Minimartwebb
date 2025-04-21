using MinimartWeb.Data;
using MinimartWeb.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MinimartWeb.DAOs
{
    public class AdminDAO
    {
        private readonly ApplicationDbContext _context;

        public AdminDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Admin> GetAll() => _context.Admins.Include(a => a.Employee).ToList();

        public Admin? GetById(int id) => _context.Admins.Include(a => a.Employee).FirstOrDefault(a => a.AdminID == id);

        public void Add(Admin admin)
        {
            _context.Admins.Add(admin);
            _context.SaveChanges();
        }

        public void Update(Admin admin)
        {
            _context.Admins.Update(admin);
            _context.SaveChanges();
        }

        public void Delete(Admin admin)
        {
            _context.Admins.Remove(admin);
            _context.SaveChanges();
        }

        public bool EmployeeExists(int employeeId) => _context.Employees.Any(e => e.EmployeeID == employeeId);

        public bool UsernameExists(string username, int? excludeId = null)
        {
            return _context.Admins.Any(a => a.Username == username && (!excludeId.HasValue || a.AdminID != excludeId.Value));
        }

        public bool EmployeeAssignedToOtherAdmin(int employeeId, int? excludeAdminId = null)
        {
            var query = _context.Admins.Where(a => a.EmployeeID == employeeId);

            if (excludeAdminId.HasValue)
                query = query.Where(a => a.AdminID != excludeAdminId.Value);

            return query.Any();
        }

    }
}
