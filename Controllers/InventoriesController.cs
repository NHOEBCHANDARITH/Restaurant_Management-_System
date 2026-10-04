using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var inventories = await _context.Inventories
                .Include(i => i.Ingredient)
                .OrderBy(i => i.Quantity)
                .ToListAsync();

            ViewBag.TotalItems = inventories.Count;
            ViewBag.LowStockCount = inventories.Count(i => i.Quantity < 10);
            ViewBag.OutOfStockCount = inventories.Count(i => i.Quantity <= 0);
            ViewBag.HealthyCount = inventories.Count(i => i.Quantity >= 10);

            return View(inventories);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var inventory = await _context.Inventories
                .Include(i => i.Ingredient)
                .FirstOrDefaultAsync(x => x.InventoryId == id);

            if (inventory == null) return NotFound();

            return View(inventory);
        }

        public async Task<IActionResult> Create()
        {
            await LoadIngredientDropdown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                if (inventory.LastUpdated == default)
                {
                    inventory.LastUpdated = DateTime.Now;
                }

                _context.Inventories.Add(inventory);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Inventory stock record created successfully!";
                return RedirectToAction(nameof(Index));
            }

            await LoadIngredientDropdown(inventory.IngredientId);
            return View(inventory);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var inventory = await _context.Inventories.FindAsync(id);

            if (inventory == null) return NotFound();

            await LoadIngredientDropdown(inventory.IngredientId);
            return View(inventory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Inventory inventory)
        {
            if (id != inventory.InventoryId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    inventory.LastUpdated = DateTime.Now;
                    _context.Inventories.Update(inventory);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Inventory stock updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryExists(inventory.InventoryId))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await LoadIngredientDropdown(inventory.IngredientId);
            return View(inventory);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var inventory = await _context.Inventories
                .Include(i => i.Ingredient)
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
                TempData["SuccessMessage"] = "Inventory record deleted.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool InventoryExists(int id)
        {
            return _context.Inventories.Any(e => e.InventoryId == id);
        }

        private async Task LoadIngredientDropdown(int? selectedIngredientId = null)
        {
            var ingredients = await _context.Ingredients
                .OrderBy(i => i.IngredientName)
                .ToListAsync();

            ViewBag.IngredientId = new SelectList(
                ingredients,
                "IngredientId",
                "IngredientName",
                selectedIngredientId
            );
        }
    }
}