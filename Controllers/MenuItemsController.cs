using Microsoft.AspNetCore.Mvc;
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
            return View(await _context.MenuItems.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var menuItem = await _context.MenuItems
                .FirstOrDefaultAsync(x => x.ItemId == id);

            if (menuItem == null) return NotFound();

            return View(menuItem);
        }

        public IActionResult Create()
        {
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
                return RedirectToAction(nameof(Index));
            }

            return View(menuItem);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var menuItem = await _context.MenuItems.FindAsync(id);

            if (menuItem == null) return NotFound();

            return View(menuItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MenuItem menuItem)
        {
            if (id != menuItem.ItemId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.MenuItems.Update(menuItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(menuItem);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var menuItem = await _context.MenuItems
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
            }

            return RedirectToAction(nameof(Index));
        }
    }
}