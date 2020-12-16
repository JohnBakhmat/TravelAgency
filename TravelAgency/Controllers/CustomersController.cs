using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Data;
using TravelAgency.Models;

namespace TravelAgency.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index(string searchString="",int order=0) {
            var applicationDbContext = await _context.Customers.ToListAsync();
            if (!string.IsNullOrEmpty(searchString)) {
                var sS = searchString.Split(' ');
                foreach (var parameter in sS) {
                    applicationDbContext = applicationDbContext.Where(c =>
                        c.LastName.Contains(parameter)||
                        c.FirstName.Contains(parameter)||
                        c.FatherName.Contains(parameter)||
                        c.BirthDate.ToString("dd-MMM-yyyy").Contains(parameter)||
                        c.PassportCode.Contains(parameter)||
                        c.Address.Contains(parameter)||
                        c.Phone.Contains(parameter)
                    ).ToList();
                }
            }
            switch (order)
            {
                case 1:
                {
                    applicationDbContext = applicationDbContext.OrderBy(v => v.LastName).ToList();
                    break;
                }
                case 2:
                {
                    applicationDbContext = applicationDbContext.OrderBy(v => v.FirstName).ToList();
                    break;
                }
                case 3:
                {
                    applicationDbContext = applicationDbContext.OrderBy(v => v.FatherName).ToList();
                    break;
                }
                case 4:
                {
                    applicationDbContext = applicationDbContext.OrderBy(v => v.BirthDate).ToList();
                    break;
                }
                case 5:
                {
                    applicationDbContext = applicationDbContext.OrderBy(v => v.PassportCode).ToList();
                    break;
                }
                case 6:
                {
                    applicationDbContext = applicationDbContext.OrderBy(v => v.Address).ToList();
                    break;
                }
                case 7:
                {
                    applicationDbContext = applicationDbContext.OrderBy(v => v.Phone).ToList();
                    break;
                }
                default:
                {
                    applicationDbContext = applicationDbContext.OrderBy(v => v.LastName).ToList();
                    break;
                }
            }
            return View(applicationDbContext);
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.UserId == id);
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,LastName,FirstName,FatherName,BirthDate,PassportCode,Address,Phone")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,LastName,FirstName,FatherName,BirthDate,PassportCode,Address,Phone")] Customer customer)
        {
            if (id != customer.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.UserId))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.UserId == id);
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
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.UserId == id);
        }
    }
}
