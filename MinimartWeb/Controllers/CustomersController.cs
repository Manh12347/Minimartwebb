using Microsoft.AspNetCore.Mvc;
using MinimartWeb.BOs;
using MinimartWeb.Data;
using MinimartWeb.Model;
using MinimartWeb.DAOs;

namespace MinimartWeb.Controllers
{
    public class CustomersController : Controller
    {
        private readonly CustomerBO _bo;

        public CustomersController(ApplicationDbContext context)
        {
            _bo = new CustomerBO(new CustomerDAO(context));
        }

        public IActionResult Index() => View(_bo.GetAll());

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Customer customer, string password)
        {
            var (isValid, errors) = _bo.Validate(customer, password);
            if (!isValid)
            {
                foreach (var err in errors)
                    ModelState.AddModelError(string.Empty, err);
                return View(customer);
            }

            _bo.Create(customer, password);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var customer = _bo.GetById(id);
            if (customer == null) return NotFound();
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Customer customer, string password)
        {
            if (id != customer.CustomerID) return NotFound();

            var (isValid, errors) = _bo.Validate(customer, password, id);
            if (!isValid)
            {
                foreach (var err in errors)
                    ModelState.AddModelError(string.Empty, err);
                return View(customer);
            }

            _bo.Update(customer, password);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var customer = _bo.GetById(id);
            if (customer == null) return NotFound();
            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _bo.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
