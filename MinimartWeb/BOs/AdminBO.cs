using MinimartWeb.DAOs;
using MinimartWeb.Model;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MinimartWeb.BOs
{
    public class AdminBO
    {
        private readonly AdminDAO _adminDAO;

        public AdminBO(AdminDAO adminDAO)
        {
            _adminDAO = adminDAO;
        }

        public List<Admin> GetAll() => _adminDAO.GetAll();

        public Admin? GetById(int id) => _adminDAO.GetById(id);

        public (bool isValid, List<string> errors) ValidateAdmin(Admin admin, string? newPassword, bool isNew)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(admin.Username))
                errors.Add("Username is required.");
            else if (_adminDAO.UsernameExists(admin.Username, isNew ? null : (int?)admin.AdminID))
                errors.Add("Username must be unique.");

            if (!_adminDAO.EmployeeExists(admin.EmployeeID))
                errors.Add("Associated Employee does not exist.");
            else if (_adminDAO.EmployeeAssignedToOtherAdmin(admin.EmployeeID, isNew ? null : (int?)admin.AdminID))
                errors.Add("This employee is already assigned to another admin.");

            if (isNew && string.IsNullOrWhiteSpace(newPassword))
                errors.Add("Password is required for new admins.");

            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                if (newPassword.Length < 8)
                    errors.Add("Password must be at least 8 characters long.");

                if (!newPassword.Any(char.IsUpper))
                    errors.Add("Password must contain at least one uppercase letter.");
                if (!newPassword.Any(char.IsLower))
                    errors.Add("Password must contain at least one lowercase letter.");
                if (!newPassword.Any(char.IsDigit))
                    errors.Add("Password must contain at least one digit.");
                if (!newPassword.Any(c => "!@#$%^&*()_+-=[]{}|;:',.<>?/".Contains(c)))
                    errors.Add("Password must contain at least one special character.");

                if (errors.Count == 0) // Only hash if valid
                {
                    var (hash, salt) = HashPassword(newPassword);
                    admin.PasswordHash = hash;
                    admin.Salt = salt;
                }
            }

            return (errors.Count == 0, errors);
        }


        public (bool isSuccess, List<string> errors) CreateAdmin(Admin admin, string newPassword)
        {
            var (isValid, errors) = ValidateAdmin(admin, newPassword, true);
            if (!isValid) return (false, errors);

            _adminDAO.Add(admin);
            return (true, new List<string>());
        }

        public (bool isSuccess, List<string> errors) UpdateAdmin(Admin admin, string? newPassword)
        {
            var (isValid, errors) = ValidateAdmin(admin, newPassword, false);
            if (!isValid) return (false, errors);

            _adminDAO.Update(admin);
            return (true, new List<string>());
        }

        public void DeleteAdmin(Admin admin) => _adminDAO.Delete(admin);

        private (byte[] hash, byte[] salt) HashPassword(string password)
        {
            using var hmac = new HMACSHA256();
            var salt = hmac.Key;
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return (hash, salt);
        }
    }
}
