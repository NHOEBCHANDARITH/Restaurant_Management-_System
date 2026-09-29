using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management__System.Data;
using Restaurant_Management__System.Models;

namespace RestaurantManagementSystem.Controllers
{
    public class DiningTablesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DiningTablesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.DiningTables.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var table = await _context.DiningTables
                .FirstOrDefaultAsync(x => x.TableId == id);

            if (table == null) return NotFound();

            return View(table);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DiningTable table)
        {
            if (ModelState.IsValid)
            {
                _context.DiningTables.Add(table);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(table);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var table = await _context.DiningTables.FindAsync(id);

            if (table == null) return NotFound();

            return View(table);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DiningTable table)
        {
            if (id != table.TableId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.DiningTables.Update(table);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(table);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var table = await _context.DiningTables
                .FirstOrDefaultAsync(x => x.TableId == id);

            if (table == null) return NotFound();

            return View(table);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var table = await _context.DiningTables.FindAsync(id);

            if (table != null)
            {
                _context.DiningTables.Remove(table);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}