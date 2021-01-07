using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TravelAgency.Data;
using TravelAgency.Models;

namespace TravelAgency.Controllers {
    public class CustomersController : Controller {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context) {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index(string searchString = "", int order = 0) {
            var applicationDbContext = await _context.Customers.ToListAsync();
            if (!string.IsNullOrEmpty(searchString)) {
                var sS = searchString.Split(' ');
                foreach (var parameter in sS)
                    applicationDbContext = applicationDbContext.Where(c =>
                        c.LastName.Contains(parameter) ||
                        c.FirstName.Contains(parameter) ||
                        c.FatherName.Contains(parameter) ||
                        c.BirthDate.ToString("dd-MMM-yyyy").Contains(parameter) ||
                        c.PassportCode.Contains(parameter) ||
                        c.Address.Contains(parameter) ||
                        c.Phone.Contains(parameter)
                    ).ToList();
            }

            switch (order) {
                case 1: {
                    applicationDbContext = applicationDbContext.OrderBy(v => v.LastName).ToList();
                    break;
                }
                case 2: {
                    applicationDbContext = applicationDbContext.OrderBy(v => v.FirstName).ToList();
                    break;
                }
                case 3: {
                    applicationDbContext = applicationDbContext.OrderBy(v => v.FatherName).ToList();
                    break;
                }
                case 4: {
                    applicationDbContext = applicationDbContext.OrderBy(v => v.BirthDate).ToList();
                    break;
                }
                case 5: {
                    applicationDbContext = applicationDbContext.OrderBy(v => v.PassportCode).ToList();
                    break;
                }
                case 6: {
                    applicationDbContext = applicationDbContext.OrderBy(v => v.Address).ToList();
                    break;
                }
                case 7: {
                    applicationDbContext = applicationDbContext.OrderBy(v => v.Phone).ToList();
                    break;
                }
                default: {
                    applicationDbContext = applicationDbContext.OrderBy(v => v.LastName).ToList();
                    break;
                }
            }

            return View(applicationDbContext);
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) return NotFound();

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (customer == null) return NotFound();

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create() {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("UserId,LastName,FirstName,FatherName,BirthDate,PassportCode,Address,Phone")]
            Customer customer) {
            if (ModelState.IsValid) {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) return NotFound();

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("UserId,LastName,FirstName,FatherName,BirthDate,PassportCode,Address,Phone")]
            Customer customer) {
            if (id != customer.UserId) return NotFound();

            if (ModelState.IsValid) {
                try {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!CustomerExists(customer.UserId))
                        return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) return NotFound();

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (customer == null) return NotFound();

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id) {
            return _context.Customers.Any(e => e.UserId == id);
        }

        private IEnumerable<CustomerCountyViewModel> GetContent() {
            var query = _context.Vocations
                .Join(_context.Customers, v => v.CustomerId, c => c.UserId, (v, c) => new {v, c})
                .Join(_context.Tours, t1 => t1.v.TourId, t => t.TourId,
                    (t1, t) => new CustomerCountyViewModel {
                        Name = $"{t1.c.LastName} {t1.c.FirstName} {t1.c.FatherName}",
                        Country = t.Country,
                        DaysSpent = t1.v.DaysCount,
                        MoneySpent = t.TransportCost + t.VisaCost + t.OneDayCost * t1.v.DaysCount
                    });
            var group = query.AsEnumerable().GroupBy(item => new {
                item.Name,
                item.Country
            }).Select(grouping => new CustomerCountyViewModel {
                Name = grouping.Key.Name,
                Country = grouping.Key.Country,
                DaysSpent = grouping.Sum(i => i.DaysSpent),
                MoneySpent = grouping.Sum(i => i.MoneySpent)
            });
            return group.OrderBy(g => g.Name).ThenBy(g => g.Country);
        }
        private class CountryValue {
            public string Country { get; }
            public double Value { get; set; }

            public CountryValue(string country, double value) {
                Country = country;
                Value = value;
            }
        }

        // TODO: 1) створювати перехресний запит «Кто сколько где потратил», який повинен містити Прізвище клієнта, Країну та кількість грошей, витрачених тим чи іншим клієнтом у відповідній подорожі(країну). Вся інформація повинна бути організована у вигляді перехресного запиту;
        public FileContentResult Query1() {
            var content = GetContent().ToList();
            var buffer = content.Select(i => i.Country)
                .Distinct()
                .OrderBy(c => c.First())
                .Aggregate("Клиент,", (current, country) => current + $"{country},") + "Всего\n";
            var dictionary = new Dictionary<string, List<CountryValue>>();
            foreach (var item in content)
                if (!dictionary.ContainsKey(item.Name)) {
                    var list = content.Select(i => i.Country)
                        .Distinct()
                        .OrderBy(c => c.First())
                        .Select(country => new CountryValue(country, item.Country == country ? item.DaysSpent : 0))
                        .ToList();
                    dictionary.Add(item.Name, list);
                }
                else {
                    dictionary[item.Name].First(i => i.Country == item.Country).Value = item.DaysSpent;
                }

            return FormCsv(dictionary, buffer, "Трата дней");
        }

        // TODO: 2) створювати перехресний запит «Кол-во дней в странах», який повинен містити Прізвище клієнта, Країну та кількість днів, проведених тим чи іншим клієнтом у відповідній країні. Вся інформація повинна бути організована у вигляді перехресного запиту.
        public FileContentResult Query2() {
            var content = GetContent().ToList();
            var buffer = content.Select(i => i.Country)
                .Distinct()
                .OrderBy(c => c.First())
                .Aggregate("Клиент,", (current, country) => current + $"{country},") + "Всего\n";
            var dictionary = new Dictionary<string, List<CountryValue>>();
            foreach (var item in content)
                if (!dictionary.ContainsKey(item.Name)) {
                    var list = content.Select(i => i.Country)
                        .Distinct()
                        .OrderBy(c => c.First())
                        .Select(country => new CountryValue(country, item.Country == country ? item.MoneySpent : 0))
                        .ToList();
                    dictionary.Add(item.Name, list);
                }
                else {
                    dictionary[item.Name].First(i => i.Country == item.Country).Value = item.MoneySpent;
                }

            return FormCsv(dictionary, buffer, "Трата денег");
        }

        private FileContentResult FormCsv(Dictionary<string, List<CountryValue>> dictionary, string buffer,
            string fileName) {
            foreach (var (key, value) in dictionary) {
                buffer += $"{key},";
                double counter = 0;
                foreach (var countryValue in value) {
                    counter += countryValue.Value;
                    buffer += $"{countryValue.Value},";
                }

                buffer += $"{counter}\n";
            }

            Console.WriteLine(buffer);
            return File(Encoding.UTF8.GetBytes(buffer), "text/csv", fileName + ".csv");
        }
    }
}