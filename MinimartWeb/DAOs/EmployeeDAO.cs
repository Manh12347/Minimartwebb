using MinimartWeb.Data;
using MinimartWeb.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MinimartWeb.DAOs
{
    public class EmployeeDAO
    {
        private readonly ApplicationDbContext _context;

        public EmployeeDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Employee> GetAll() => _context.Employees.Include(e => e.EmployeeRole).ToList();

        public Employee? GetById(int id) => _context.Employees.Include(e => e.EmployeeRole).FirstOrDefault(e => e.EmployeeID == id);

        public void Add(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public void Update(Employee employee)
        {
            _context.Employees.Update(employee);
            _context.SaveChanges();
        }

        public void Delete(Employee employee)
        {
            _context.Employees.Remove(employee);
            _context.SaveChanges();
        }

        public bool EmailExists(string email, int? excludeId = null)
            => _context.Employees.Any(e => e.Email == email && e.EmployeeID != excludeId);

        public bool PhoneExists(string phone, int? excludeId = null)
            => _context.Employees.Any(e => e.PhoneNumber == phone && e.EmployeeID != excludeId);

        public bool CitizenIDExists(string citizenID, int? excludeId = null)
            => _context.Employees.Any(e => e.CitizenID == citizenID && e.EmployeeID != excludeId);

        public bool RoleExists(int roleId)
            => _context.EmployeeRoles.Any(r => r.RoleID == roleId);
    }
}
