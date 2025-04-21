using System.Text.RegularExpressions;
using MinimartWeb.Model;
using MinimartWeb.DAOs;

namespace MinimartWeb.BOs
{
    public class SupplierBO
    {
        private readonly SupplierDAO _supplierDAO;

        public SupplierBO(SupplierDAO supplierDAO)
        {
            _supplierDAO = supplierDAO;
        }

        public async Task<(bool isValid, List<string> errors)> ValidateAsync(Supplier supplier, bool isEdit = false)
        {
            List<string> errors = new();

            if (string.IsNullOrWhiteSpace(supplier.SupplierName) || supplier.SupplierName.Length > 255)
                errors.Add("Supplier name is required and must be under 255 characters.");

            if (!Regex.IsMatch(supplier.SupplierName ?? "", @"^[A-Za-z0-9\s\-']+$"))
                errors.Add("Supplier name contains invalid characters.");

            if (!string.IsNullOrEmpty(supplier.SupplierEmail) &&
                !Regex.IsMatch(supplier.SupplierEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                errors.Add("Invalid email format.");

            if (!string.IsNullOrEmpty(supplier.SupplierPhoneNumber) &&
                !Regex.IsMatch(supplier.SupplierPhoneNumber, @"^\+?[0-9\s\-]{7,15}$"))
                errors.Add("Phone number must contain only digits, spaces, dashes and optional + sign.");

            if (await _supplierDAO.ExistsByNameAsync(supplier.SupplierName, isEdit ? supplier.SupplierID : null))
                errors.Add("A supplier with this name already exists.");

            if (!string.IsNullOrEmpty(supplier.SupplierEmail) &&
                await _supplierDAO.ExistsByEmailAsync(supplier.SupplierEmail, isEdit ? supplier.SupplierID : null))
                errors.Add("A supplier with this email already exists.");

            return (!errors.Any(), errors);
        }

        public Task<List<Supplier>> GetAllAsync() => _supplierDAO.GetAllAsync();
        public Task<Supplier?> GetByIdAsync(int id) => _supplierDAO.GetByIdAsync(id);
        public Task AddAsync(Supplier supplier) => _supplierDAO.AddAsync(supplier);
        public Task UpdateAsync(Supplier supplier) => _supplierDAO.UpdateAsync(supplier);
        public Task DeleteAsync(Supplier supplier) => _supplierDAO.DeleteAsync(supplier);
    }
}
