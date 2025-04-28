using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MinimartWeb.Data;
using MinimartWeb.Model;
using Microsoft.AspNetCore.Hosting;

namespace MinimartWeb.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Inject IWebHostEnvironment via constructor
        public CustomersController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("CustomerID,FirstName,LastName,Email,PhoneNumber,Username")] Customer customer,
            string Password,
            IFormFile ProfileImage)
        {
            // Remove password fields to prevent overposting
            ModelState.Remove(nameof(Customer.PasswordHash));
            ModelState.Remove(nameof(Customer.Salt));

            // Check for existing email, phone number, and username
            if (await _context.Customers.AnyAsync(c => c.Email == customer.Email))
            {
                ModelState.AddModelError("Email", "The email address is already taken.");
            }
            if (await _context.Customers.AnyAsync(c => c.PhoneNumber == customer.PhoneNumber))
            {
                ModelState.AddModelError("PhoneNumber", "The phone number is already in use.");
            }
            if (await _context.Customers.AnyAsync(c => c.Username == customer.Username))
            {
                ModelState.AddModelError("Username", "The username is already taken.");
            }

            // Check if password is provided
            if (string.IsNullOrWhiteSpace(Password))
            {
                ModelState.AddModelError("Password", "Password is required.");
            }

            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "users");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Get the file extension
                var fileExtension = Path.GetExtension(ProfileImage.FileName);

                // Generate a unique file name without the full path
                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfileImage.CopyToAsync(fileStream);
                }

                // Save only the file name in the database
                customer.ImagePath = uniqueFileName;
            }
            else
            {
                customer.ImagePath = "default.png";
            }

            // Hash password and generate salt
            (byte[] passwordHash, byte[] salt) = GeneratePasswordHashAndSalt(Password);
            customer.PasswordHash = passwordHash;
            customer.Salt = salt;

            _context.Add(customer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerID,FirstName,LastName,Email,PhoneNumber,Username")] Customer customer, string Password, IFormFile ProfileImage)
        {
            if (id != customer.CustomerID)
            {
                return NotFound();
            }

            // Remove password fields to prevent overposting
            ModelState.Remove(nameof(Customer.PasswordHash));
            ModelState.Remove(nameof(Customer.Salt));

            // Check for existing email, phone number, and username (excluding current customer's values)
            if (await _context.Customers.AnyAsync(c => c.Email == customer.Email && c.CustomerID != customer.CustomerID))
            {
                ModelState.AddModelError("Email", "The email address is already taken.");
            }

            if (await _context.Customers.AnyAsync(c => c.PhoneNumber == customer.PhoneNumber && c.CustomerID != customer.CustomerID))
            {
                ModelState.AddModelError("PhoneNumber", "The phone number is already in use.");
            }

            if (await _context.Customers.AnyAsync(c => c.Username == customer.Username && c.CustomerID != customer.CustomerID))
            {
                ModelState.AddModelError("Username", "The username is already taken.");
            }

            // Validate uploaded image file (optional)
            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(ProfileImage.FileName).ToLowerInvariant();
                if (!string.IsNullOrEmpty(extension) && !allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImagePath", "Only JPG, JPEG, and PNG image files are allowed.");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            try
            {
                var customerFromDb = await _context.Customers.FindAsync(id);
                if (customerFromDb == null)
                {
                    return NotFound();
                }

                // Update editable fields
                customerFromDb.FirstName = customer.FirstName;
                customerFromDb.LastName = customer.LastName;
                customerFromDb.Email = customer.Email;
                customerFromDb.PhoneNumber = customer.PhoneNumber;
                customerFromDb.Username = customer.Username;

                // Handle image upload
                if (ProfileImage != null && ProfileImage.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "users");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ProfileImage.CopyToAsync(fileStream);
                    }

                    // Save relative path for use in views
                    customerFromDb.ImagePath = uniqueFileName;
                }

                // Only update password if it's provided
                if (!string.IsNullOrWhiteSpace(Password))
                {
                    (byte[] passwordHash, byte[] salt) = GeneratePasswordHashAndSalt(Password);
                    customerFromDb.PasswordHash = passwordHash;
                    customerFromDb.Salt = salt;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(customer.CustomerID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerID == id);
        }

        private (byte[] passwordHash, byte[] salt) GeneratePasswordHashAndSalt(string password)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA256())
            {
                // Create a salt of 16 bytes
                var salt = new byte[16];

                // Use RandomNumberGenerator.Create() to get a concrete implementation
                using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt); // Fill the salt with random bytes
                }

                // Use the salt as the key for HMAC
                hmac.Key = salt;

                // Compute the password hash
                var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                return (passwordHash, salt); // Return both the hash and the salt
            }
        }
    }
}
