using Microsoft.AspNetCore.Mvc;
using MinimartWeb.BOs;
using MinimartWeb.DAOs;
using MinimartWeb.Data;
using MinimartWeb.Model;
using System.Threading.Tasks;

namespace MinimartWeb.Controllers
{
    public class MeasurementUnitsController : Controller
    {
        private readonly MeasurementUnitBO _unitBO;

        public MeasurementUnitsController(ApplicationDbContext context)
        {
            _unitBO = new MeasurementUnitBO(new MeasurementUnitDAO(context));
        }

        public async Task<IActionResult> Index()
        {
            var list = await _unitBO.GetAllAsync();
            return View(list);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var unit = await _unitBO.GetByIdAsync(id.Value);
            if (unit == null) return NotFound();

            return View(unit);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MeasurementUnitID,UnitName,UnitDescription,IsContinuous")] MeasurementUnit unit)
        {
            var (success, errors) = await _unitBO.AddAsync(unit);
            if (success)
                return RedirectToAction(nameof(Index));

            foreach (var error in errors)
                ModelState.AddModelError(string.Empty, error);

            return View(unit);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var unit = await _unitBO.GetByIdAsync(id.Value);
            if (unit == null) return NotFound();

            return View(unit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MeasurementUnitID,UnitName,UnitDescription,IsContinuous")] MeasurementUnit unit)
        {
            if (id != unit.MeasurementUnitID)
                return NotFound();

            var (success, errors) = await _unitBO.UpdateAsync(unit);
            if (success)
                return RedirectToAction(nameof(Index));

            foreach (var error in errors)
                ModelState.AddModelError(string.Empty, error);

            return View(unit);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var unit = await _unitBO.GetByIdAsync(id.Value);
            if (unit == null) return NotFound();

            return View(unit);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitBO.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
