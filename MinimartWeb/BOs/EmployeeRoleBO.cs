using MinimartWeb.DAOs;
using MinimartWeb.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MinimartWeb.BOs
{
    public class EmployeeRoleBO
    {
        private readonly EmployeeRoleDAO _dao;

        public EmployeeRoleBO(EmployeeRoleDAO dao)
        {
            _dao = dao;
        }

        public (bool isValid, List<string> errors) ValidateAndAdd(EmployeeRole role)
        {
            var errors = Validate(role, isNew: true);

            if (errors.Count == 0)
            {
                _dao.Add(role);
                return (true, errors);
            }

            return (false, errors);
        }

        public (bool isValid, List<string> errors) ValidateAndUpdate(EmployeeRole role)
        {
            var errors = Validate(role, isNew: false);

            if (errors.Count == 0)
            {
                _dao.Update(role);
                return (true, errors);
            }

            return (false, errors);
        }

        public List<EmployeeRole> GetAll() => _dao.GetAll();
        public EmployeeRole? GetById(int id) => _dao.GetById(id);
        public void Delete(EmployeeRole role) => _dao.Delete(role);

        private List<string> Validate(EmployeeRole role, bool isNew)
        {
            var errors = new List<string>();

            // Required checks
            if (string.IsNullOrWhiteSpace(role.RoleName))
                errors.Add("Role name is required.");

            // Length checks
            if (!string.IsNullOrWhiteSpace(role.RoleName) && role.RoleName.Length > 255)
                errors.Add("Role name cannot exceed 255 characters.");

            if (!string.IsNullOrWhiteSpace(role.RoleDescription) && role.RoleDescription.Length > 1000)
                errors.Add("Role description is too long.");

            // Uniqueness checks
            if (!string.IsNullOrWhiteSpace(role.RoleName) && _dao.RoleNameExists(role.RoleName, isNew ? null : (int?)role.RoleID))
                errors.Add($"Role name '{role.RoleName}' already exists.");

            return errors;
        }
    }
}
