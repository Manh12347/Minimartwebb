using MinimartWeb.Data;
using MinimartWeb.Model;
using System.Collections.Generic;
using System.Linq;

namespace MinimartWeb.DAOs
{
    public class CustomerDAO
    {
        private readonly ApplicationDbContext _context;

        public CustomerDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Customer> GetAll() => _context.Customers.ToList();

        public Customer? GetById(int id) => _context.Customers.Find(id);

        public bool EmailExists(string email, int? excludeId = null)
            => _context.Customers.Any(c => c.Email == email && c.CustomerID != excludeId);

        public bool UsernameExists(string username, int? excludeId = null)
            => _context.Customers.Any(c => c.Username == username && c.CustomerID != excludeId);

        public void Add(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public void Update(Customer customer)
        {
            _context.Customers.Update(customer);
            _context.SaveChanges();
        }

        public void Delete(Customer customer)
        {
            _context.Customers.Remove(customer);
            _context.SaveChanges();
        }
    }
}
