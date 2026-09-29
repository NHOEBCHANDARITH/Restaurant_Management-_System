using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management__System.Data;
using Restaurant_Management__System.Models;

namespace RestaurantManagementSystem.Controllers
{
    public class InventoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Inventories.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(x => x.InventoryId == id);

            if (inventory == null) return NotFound();

            return View(inventory);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                _context.Inventories.Add(inventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(inventory);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var inventory = await _context.Inventories.FindAsync(id);

            if (inventory == null) return NotFound();

            return View(inventory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Inventory inventory)
        {
            if (id != inventory.InventoryId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Inventories.Update(inventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(inventory);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(x => x.InventoryId == id);

            if (inventory == null) return NotFound();

            return View(inventory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventory = await _context.Inventories.FindAsync(id);

            if (inventory != null)
            {
                _context.Inventories.Remove(inventory);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}