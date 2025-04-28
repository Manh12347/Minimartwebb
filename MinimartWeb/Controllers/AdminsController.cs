using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MinimartWeb.Data;
using MinimartWeb.Model;

namespace MinimartWeb.Controllers
{
    public class AdminsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admins
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Admins.Include(a => a.Employee);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admins/Create
        public IActionResult Create()
        {
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "CitizenID");
            return View();
        }

        // POST: Admins/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,Username,IsActive")] Admin admin, string Password)
        {
            ModelState.Remove(nameof(Admin.PasswordHash));
            ModelState.Remove(nameof(Admin.Salt));
            ModelState.Remove(nameof(Admin.Employee));

            // First check password manually
            if (string.IsNullOrWhiteSpace(Password))
            {
                ModelState.AddModelError(String.Empty, "Password is required.");
            }

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "CitizenID", admin.EmployeeID);
                return View(admin);
            }

            (byte[] passwordHash, byte[] salt) = GeneratePasswordHashAndSalt(Password);
            admin.PasswordHash = passwordHash;
            admin.Salt = salt;
            admin.CreatedAt = DateTime.Now;
            admin.LastLogin = null;

            try
            {
                _context.Add(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("FK_Admins_Employee") == true)
                {
                    ModelState.AddModelError("EmployeeID", "This employee is already assigned to an admin.");
                }
                else if (ex.InnerException?.Message.Contains("UQ_Admins_Username") == true)
                {
                    ModelState.AddModelError("Username", "This username is already taken.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred while creating the admin.");
                }

                ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "CitizenID", admin.EmployeeID);
                return View(admin);
            }

            //_context.Add(admin);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
        }

        // Utility method to create a password hash and salt
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

        // GET: Admins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }

            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "CitizenID", admin.EmployeeID);
            return View(admin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AdminID,EmployeeID,Username,IsActive")] Admin admin, string Password)
        {
            if (id != admin.AdminID)
            {
                return NotFound();
            }

            ModelState.Remove(nameof(Admin.PasswordHash));
            ModelState.Remove(nameof(Admin.Salt));
            ModelState.Remove(nameof(Admin.Employee));
            ModelState.Remove("Password");

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "CitizenID", admin.EmployeeID);
                return View(admin);
            }

            try
            {
                var adminFromDb = await _context.Admins.FindAsync(id);
                if (adminFromDb == null)
                {
                    return NotFound();
                }

                // 🛑 Check if EmployeeID already assigned to another Admin
                if (await _context.Admins.AnyAsync(a => a.EmployeeID == admin.EmployeeID && a.AdminID != admin.AdminID))
                {
                    ModelState.AddModelError("EmployeeID", "This employee is already assigned to another admin.");
                }

                // 🛑 Check if Username is already taken by another Admin
                if (await _context.Admins.AnyAsync(a => a.Username == admin.Username && a.AdminID != admin.AdminID))
                {
                    ModelState.AddModelError("Username", "This username is already taken.");
                }

                if (!ModelState.IsValid)
                {
                    ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "CitizenID", admin.EmployeeID);
                    return View(admin);
                }

                // Update editable fields
                adminFromDb.EmployeeID = admin.EmployeeID;
                adminFromDb.Username = admin.Username;
                adminFromDb.IsActive = admin.IsActive;

                if (!string.IsNullOrWhiteSpace(Password))
                {
                    (byte[] passwordHash, byte[] salt) = GeneratePasswordHashAndSalt(Password);
                    adminFromDb.PasswordHash = passwordHash;
                    adminFromDb.Salt = salt;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while updating the admin.");
                ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "CitizenID", admin.EmployeeID);
                return View(admin);
            }

        }


        // GET: Admins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(m => m.AdminID == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin != null)
            {
                _context.Admins.Remove(admin);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminExists(int id)
        {
            return _context.Admins.Any(e => e.AdminID == id);
        }
    }
}
