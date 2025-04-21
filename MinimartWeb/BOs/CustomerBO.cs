using MinimartWeb.DAOs;
using MinimartWeb.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MinimartWeb.BOs
{
    public class CustomerBO
    {
        private readonly CustomerDAO _dao;

        public CustomerBO(CustomerDAO dao)
        {
            _dao = dao;
        }

        public List<Customer> GetAll() => _dao.GetAll();

        public Customer? GetById(int id) => _dao.GetById(id);

        public (bool IsValid, List<string> Errors) Validate(Customer customer, string password, int? id = null)
        {
            var errors = new List<string>();

            // First name and last name
            if (string.IsNullOrWhiteSpace(customer.FirstName) || !Regex.IsMatch(customer.FirstName, @"^[A-Za-z\s]+$"))
                errors.Add("First name must only contain letters and spaces.");
            if (string.IsNullOrWhiteSpace(customer.LastName) || !Regex.IsMatch(customer.LastName, @"^[A-Za-z\s]+$"))
                errors.Add("Last name must only contain letters and spaces.");

            // Email
            if (string.IsNullOrWhiteSpace(customer.Email) || !new EmailAddressAttribute().IsValid(customer.Email))
                errors.Add("Invalid email address.");
            else if (_dao.EmailExists(customer.Email, id))
                errors.Add("Email already exists.");

            // Phone number (10 digits)
            if (string.IsNullOrWhiteSpace(customer.PhoneNumber) || !Regex.IsMatch(customer.PhoneNumber, @"^\d{10}$"))
                errors.Add("Phone number must be exactly 10 digits.");

            // Username
            if (string.IsNullOrWhiteSpace(customer.Username))
                errors.Add("Username is required.");
            else if (_dao.UsernameExists(customer.Username, id))
                errors.Add("Username already exists.");

            // Password
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                errors.Add("Password must be at least 6 characters.");

            return (errors.Count == 0, errors);
        }

        public void Create(Customer customer, string password)
        {
            GeneratePasswordHash(password, out byte[] hash, out byte[] salt);
            customer.PasswordHash = hash;
            customer.Salt = salt;
            _dao.Add(customer);
        }

        public void Update(Customer customer, string password)
        {
            GeneratePasswordHash(password, out byte[] hash, out byte[] salt);
            customer.PasswordHash = hash;
            customer.Salt = salt;
            _dao.Update(customer);
        }

        public void Delete(int id)
        {
            var customer = _dao.GetById(id);
            if (customer != null)
                _dao.Delete(customer);
        }

        private void GeneratePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA256();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}
