using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management__System.Data;
using Restaurant_Management__System.Models;

namespace RestaurantManagementSystem.Controllers
{
    public class MenuItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var menuItems = await _context.MenuItems
                .Include(m => m.Category)
                .OrderBy(m => m.Category != null ? m.Category.CategoryName : "")
                .ThenBy(m => m.ItemName)
                .ToListAsync();

            ViewBag.Categories = await _context.Categories
                .OrderBy(c => c.CategoryName)
                .ToListAsync();

            return View(menuItems);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var menuItem = await _context.MenuItems
                .Include(m => m.Category)
                .FirstOrDefaultAsync(x => x.ItemId == id);

            if (menuItem == null) return NotFound();

            return View(menuItem);
        }

        public async Task<IActionResult> Create()
        {
            await LoadCategoryDropdown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuItem menuItem)
        {
            if (ModelState.IsValid)
            {
                _context.MenuItems.Add(menuItem);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Menu item '{menuItem.ItemName}' created successfully!";
                return RedirectToAction(nameof(Index));
            }

            await LoadCategoryDropdown(menuItem.CategoryId);
            return View(menuItem);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var menuItem = await _context.MenuItems.FindAsync(id);

            if (menuItem == null) return NotFound();

            await LoadCategoryDropdown(menuItem.CategoryId);
            return View(menuItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MenuItem menuItem)
        {
            if (id != menuItem.ItemId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.MenuItems.Update(menuItem);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Menu item '{menuItem.ItemName}' updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuItemExists(menuItem.ItemId))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await LoadCategoryDropdown(menuItem.CategoryId);
            return View(menuItem);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var menuItem = await _context.MenuItems
                .Include(m => m.Category)
                .FirstOrDefaultAsync(x => x.ItemId == id);

            if (menuItem == null) return NotFound();

            return View(menuItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);

            if (menuItem != null)
            {
                _context.MenuItems.Remove(menuItem);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Menu item '{menuItem.ItemName}' removed.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MenuItemExists(int id)
        {
            return _context.MenuItems.Any(e => e.ItemId == id);
        }

        private async Task LoadCategoryDropdown(int? selectedCategoryId = null)
        {
            var categories = await _context.Categories
                .OrderBy(c => c.CategoryName)
                .ToListAsync();

            ViewBag.CategoryId = new SelectList(
                categories,
                "CategoryId",
                "CategoryName",
                selectedCategoryId
            );
        }
    }
}