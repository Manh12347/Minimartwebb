using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MinimartWeb.Data;
using MinimartWeb.Model;

namespace MinimartWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OtpRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OtpRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OtpRequests
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.OtpRequests.Include(o => o.Customer).Include(o => o.EmployeeAccount).Include(o => o.OtpType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: OtpRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var otpRequest = await _context.OtpRequests
                .Include(o => o.Customer)
                .Include(o => o.EmployeeAccount)
                .Include(o => o.OtpType)
                .FirstOrDefaultAsync(m => m.OtpRequestID == id);
            if (otpRequest == null)
            {
                return NotFound();
            }

            return View(otpRequest);
        }

        // GET: OtpRequests/Create
        public IActionResult Create()
        {
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "Email");
            ViewData["EmployeeAccountID"] = new SelectList(_context.EmployeeAccounts, "AccountID", "Username");
            ViewData["OtpTypeID"] = new SelectList(_context.OtpTypes, "OtpTypeID", "OtpTypeName");
            return View();
        }

        // POST: OtpRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OtpRequestID,CustomerID,EmployeeAccountID,OtpTypeID,OtpCode,RequestTime,ExpirationTime,IsUsed,Status")] OtpRequest otpRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(otpRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "Email", otpRequest.CustomerID);
            ViewData["EmployeeAccountID"] = new SelectList(_context.EmployeeAccounts, "AccountID", "Username", otpRequest.EmployeeAccountID);
            ViewData["OtpTypeID"] = new SelectList(_context.OtpTypes, "OtpTypeID", "OtpTypeName", otpRequest.OtpTypeID);
            return View(otpRequest);
        }

        // GET: OtpRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var otpRequest = await _context.OtpRequests.FindAsync(id);
            if (otpRequest == null)
            {
                return NotFound();
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "Email", otpRequest.CustomerID);
            ViewData["EmployeeAccountID"] = new SelectList(_context.EmployeeAccounts, "AccountID", "Username", otpRequest.EmployeeAccountID);
            ViewData["OtpTypeID"] = new SelectList(_context.OtpTypes, "OtpTypeID", "OtpTypeName", otpRequest.OtpTypeID);
            return View(otpRequest);
        }

        // POST: OtpRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OtpRequestID,CustomerID,EmployeeAccountID,OtpTypeID,OtpCode,RequestTime,ExpirationTime,IsUsed,Status")] OtpRequest otpRequest)
        {
            if (id != otpRequest.OtpRequestID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(otpRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OtpRequestExists(otpRequest.OtpRequestID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "Email", otpRequest.CustomerID);
            ViewData["EmployeeAccountID"] = new SelectList(_context.EmployeeAccounts, "AccountID", "Username", otpRequest.EmployeeAccountID);
            ViewData["OtpTypeID"] = new SelectList(_context.OtpTypes, "OtpTypeID", "OtpTypeName", otpRequest.OtpTypeID);
            return View(otpRequest);
        }

        // GET: OtpRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var otpRequest = await _context.OtpRequests
                .Include(o => o.Customer)
                .Include(o => o.EmployeeAccount)
                .Include(o => o.OtpType)
                .FirstOrDefaultAsync(m => m.OtpRequestID == id);
            if (otpRequest == null)
            {
                return NotFound();
            }

            return View(otpRequest);
        }

        // POST: OtpRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var otpRequest = await _context.OtpRequests.FindAsync(id);
            if (otpRequest != null)
            {
                _context.OtpRequests.Remove(otpRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OtpRequestExists(int id)
        {
            return _context.OtpRequests.Any(e => e.OtpRequestID == id);
        }
    }
}
