using MinimartWeb.DAOs;
using MinimartWeb.Model;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MinimartWeb.BOs
{
    public class EmployeeBO
    {
        private readonly EmployeeDAO _dao;

        private static readonly List<string> ValidGenders = new() { "Male", "Female", "Non-Binary", "Prefer not to say" };

        public EmployeeBO(EmployeeDAO dao)
        {
            _dao = dao;
        }

        public List<Employee> GetAll() => _dao.GetAll();

        public Employee? GetById(int id) => _dao.GetById(id);

        public (bool IsValid, List<string> Errors) ValidateEmployee(Employee employee, bool isEdit = false)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(employee.FirstName) || employee.FirstName.Length > 255)
                errors.Add("First Name is required and must be under 255 characters.");

            if (string.IsNullOrWhiteSpace(employee.LastName) || employee.LastName.Length > 255)
                errors.Add("Last Name is required and must be under 255 characters.");

            if (string.IsNullOrWhiteSpace(employee.Email) || !Regex.IsMatch(employee.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                errors.Add("A valid Email is required.");
            else if (_dao.EmailExists(employee.Email, isEdit ? employee.EmployeeID : null))
                errors.Add("Email already exists.");

            if (string.IsNullOrWhiteSpace(employee.PhoneNumber) || !Regex.IsMatch(employee.PhoneNumber, @"^\d{10}$"))
                errors.Add("Phone Number must be exactly 10 digits.");
            else if (_dao.PhoneExists(employee.PhoneNumber, isEdit ? employee.EmployeeID : null))
                errors.Add("Phone Number already exists.");

            if (string.IsNullOrWhiteSpace(employee.CitizenID))
                errors.Add("Citizen ID is required.");
            else if (_dao.CitizenIDExists(employee.CitizenID, isEdit ? employee.EmployeeID : null))
                errors.Add("Citizen ID already exists.");

            if (!ValidGenders.Contains(employee.Gender))
                errors.Add("Invalid Gender selected.");

            if (!_dao.RoleExists(employee.RoleID))
                errors.Add("The selected Role does not exist.");

            if (employee.BirthDate > DateTime.Now)
                errors.Add("Birthdate cannot be in the future.");

            if (employee.HireDate > DateTime.Now)
                errors.Add("Hire date cannot be in the future.");

            return (errors.Count == 0, errors);
        }

        public (bool IsSuccess, List<string> Errors) Add(Employee employee)
        {
            var (isValid, errors) = ValidateEmployee(employee);
            if (!isValid) return (false, errors);

            _dao.Add(employee);
            return (true, new List<string>());
        }

        public (bool IsSuccess, List<string> Errors) Update(Employee employee)
        {
            var (isValid, errors) = ValidateEmployee(employee, isEdit: true);
            if (!isValid) return (false, errors);

            _dao.Update(employee);
            return (true, new List<string>());
        }

        public void Delete(Employee employee)
        {
            _dao.Delete(employee);
        }
    }
}
