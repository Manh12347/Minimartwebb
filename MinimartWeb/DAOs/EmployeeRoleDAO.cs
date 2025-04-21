using MinimartWeb.Data;
using MinimartWeb.Model;
using System.Collections.Generic;
using System.Linq;

namespace MinimartWeb.DAOs
{
    public class EmployeeRoleDAO
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRoleDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<EmployeeRole> GetAll() => _context.EmployeeRoles.ToList();
        public EmployeeRole? GetById(int id) => _context.EmployeeRoles.Find(id);
        public void Add(EmployeeRole role)
        {
            _context.EmployeeRoles.Add(role);
            _context.SaveChanges();
        }
        public void Update(EmployeeRole role)
        {
            _context.EmployeeRoles.Update(role);
            _context.SaveChanges();
        }
        public void Delete(EmployeeRole role)
        {
            _context.EmployeeRoles.Remove(role);
            _context.SaveChanges();
        }
        public bool RoleNameExists(string roleName, int? excludeId = null)
        {
            return _context.EmployeeRoles.Any(r => r.RoleName == roleName && (!excludeId.HasValue || r.RoleID != excludeId.Value));
        }
    }
}
