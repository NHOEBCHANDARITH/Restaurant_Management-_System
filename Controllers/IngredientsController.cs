using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management__System.Data;
using Restaurant_Management__System.Models;

namespace RestaurantManagementSystem.Controllers
{
    public class IngredientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IngredientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var ingredients = await _context.Ingredients
                .Include(i => i.Supplier)
                .OrderBy(i => i.IngredientName)
                .ToListAsync();

            return View(ingredients);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var ingredient = await _context.Ingredients
                .Include(i => i.Supplier)
                .FirstOrDefaultAsync(x => x.IngredientId == id);

            if (ingredient == null) return NotFound();

            return View(ingredient);
        }

        public async Task<IActionResult> Create()
        {
            await LoadSupplierDropdown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                _context.Ingredients.Add(ingredient);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Ingredient '{ingredient.IngredientName}' added successfully!";
                return RedirectToAction(nameof(Index));
            }

            await LoadSupplierDropdown(ingredient.SupplierId);
            return View(ingredient);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ingredient = await _context.Ingredients.FindAsync(id);

            if (ingredient == null) return NotFound();

            await LoadSupplierDropdown(ingredient.SupplierId);
            return View(ingredient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ingredient ingredient)
        {
            if (id != ingredient.IngredientId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Ingredients.Update(ingredient);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Ingredient '{ingredient.IngredientName}' updated!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngredientExists(ingredient.IngredientId))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await LoadSupplierDropdown(ingredient.SupplierId);
            return View(ingredient);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var ingredient = await _context.Ingredients
                .Include(i => i.Supplier)
                .FirstOrDefaultAsync(x => x.IngredientId == id);

            if (ingredient == null) return NotFound();

            return View(ingredient);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);

            if (ingredient != null)
            {
                _context.Ingredients.Remove(ingredient);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Ingredient '{ingredient.IngredientName}' removed.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool IngredientExists(int id)
        {
            return _context.Ingredients.Any(e => e.IngredientId == id);
        }

        private async Task LoadSupplierDropdown(int? selectedSupplierId = null)
        {
            var suppliers = await _context.Suppliers
                .OrderBy(s => s.SupplierName)
                .ToListAsync();

            ViewBag.SupplierId = new SelectList(
                suppliers,
                "SupplierId",
                "SupplierName",
                selectedSupplierId
            );
        }
    }
}