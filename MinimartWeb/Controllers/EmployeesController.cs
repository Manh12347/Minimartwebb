using Microsoft.AspNetCore.Mvc;
using MinimartWeb.BOs;
using MinimartWeb.DAOs;
using MinimartWeb.Data;
using MinimartWeb.Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MinimartWeb.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeeBO _bo;

        public EmployeesController(ApplicationDbContext context)
        {
            var dao = new EmployeeDAO(context);
            _bo = new EmployeeBO(dao);
        }

        public IActionResult Index()
        {
            var employees = _bo.GetAll();
            return View(employees);
        }

        public IActionResult Details(int id)
        {
            var employee = _bo.GetById(id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        public IActionResult Create()
        {
            PopulateRoles();
            PopulateGenderList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            var (isSuccess, errors) = _bo.Add(employee);

            if (!isSuccess)
            {
                foreach (var error in errors)
                    ModelState.AddModelError(string.Empty, error);

                PopulateRoles();
                PopulateGenderList();
                return View(employee);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var employee = _bo.GetById(id);
            if (employee == null) return NotFound();

            PopulateRoles();
            PopulateGenderList();
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Employee employee)
        {
            if (id != employee.EmployeeID) return NotFound();

            var (isSuccess, errors) = _bo.Update(employee);

            if (!isSuccess)
            {
                foreach (var error in errors)
                    ModelState.AddModelError(string.Empty, error);

                PopulateRoles();
                PopulateGenderList();
                return View(employee);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var employee = _bo.GetById(id);
            if (employee == null) return NotFound();

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var employee = _bo.GetById(id);
            if (employee != null) _bo.Delete(employee);
            return RedirectToAction(nameof(Index));
        }

        private void PopulateRoles()
        {
            var context = HttpContext.RequestServices.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
            ViewData["RoleID"] = new SelectList(context.EmployeeRoles, "RoleID", "RoleName");
        }

        private void PopulateGenderList()
        {
            ViewData["Gender"] = new SelectList(new List<string>
            {
                "Male",
                "Female",
                "Non-Binary",
                "Prefer not to say"
            });
        }
    }
}
