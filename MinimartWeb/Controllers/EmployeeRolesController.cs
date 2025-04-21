using Microsoft.AspNetCore.Mvc;
using MinimartWeb.BOs;
using MinimartWeb.Data;
using MinimartWeb.DAOs;
using MinimartWeb.Model;

namespace MinimartWeb.Controllers
{
    public class EmployeeRolesController : Controller
    {
        private readonly EmployeeRoleBO _bo;

        public EmployeeRolesController(ApplicationDbContext context)
        {
            var dao = new EmployeeRoleDAO(context);
            _bo = new EmployeeRoleBO(dao);
        }

        public IActionResult Index() => View(_bo.GetAll());

        public IActionResult Details(int id)
        {
            var role = _bo.GetById(id);
            if (role == null) return NotFound();
            return View(role);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeRole role)
        {
            var (isValid, errors) = _bo.ValidateAndAdd(role);
            if (isValid)
                return RedirectToAction(nameof(Index));

            foreach (var error in errors)
                ModelState.AddModelError(string.Empty, error);

            return View(role);
        }

        public IActionResult Edit(int id)
        {
            var role = _bo.GetById(id);
            if (role == null) return NotFound();
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EmployeeRole role)
        {
            if (id != role.RoleID) return NotFound();

            var (isValid, errors) = _bo.ValidateAndUpdate(role);
            if (isValid)
                return RedirectToAction(nameof(Index));

            foreach (var error in errors)
                ModelState.AddModelError(string.Empty, error);

            return View(role);
        }

        public IActionResult Delete(int id)
        {
            var role = _bo.GetById(id);
            if (role == null) return NotFound();
            return View(role);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var role = _bo.GetById(id);
            if (role == null) return NotFound();
            _bo.Delete(role);
            return RedirectToAction(nameof(Index));
        }
    }
}
