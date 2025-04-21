using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MinimartWeb.BOs;
using MinimartWeb.Data;
using MinimartWeb.DAOs;
using MinimartWeb.Model;

namespace MinimartWeb.Controllers
{
    public class AdminsController : Controller
    {
        private readonly AdminBO _adminBO;
        private readonly ApplicationDbContext _context;

        public AdminsController(ApplicationDbContext context)
        {
            _context = context;
            _adminBO = new AdminBO(new AdminDAO(context));
        }

        public IActionResult Index()
        {
            var admins = _adminBO.GetAll();
            return View(admins);
        }

        public IActionResult Details(int id)
        {
            var admin = _adminBO.GetById(id);
            if (admin == null) return NotFound();
            return View(admin);
        }

        public IActionResult Create()
        {
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "CitizenID");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Admin admin, string newPassword)
        {
            var (isSuccess, errors) = _adminBO.CreateAdmin(admin, newPassword);
            if (!isSuccess)
            {
                foreach (var error in errors)
                    ModelState.AddModelError(string.Empty, error);
                ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "CitizenID", admin.EmployeeID);
                return View(admin);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var admin = _adminBO.GetById(id);
            if (admin == null) return NotFound();

            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "CitizenID", admin.EmployeeID);
            return View(admin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Admin admin, string? newPassword)
        {
            if (id != admin.AdminID) return NotFound();

            var (isSuccess, errors) = _adminBO.UpdateAdmin(admin, newPassword);
            if (!isSuccess)
            {
                foreach (var error in errors)
                    ModelState.AddModelError(string.Empty, error);
                ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "CitizenID", admin.EmployeeID);
                return View(admin);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var admin = _adminBO.GetById(id);
            if (admin == null) return NotFound();
            return View(admin);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var admin = _adminBO.GetById(id);
            if (admin != null) _adminBO.DeleteAdmin(admin);
            return RedirectToAction(nameof(Index));
        }
    }
}
