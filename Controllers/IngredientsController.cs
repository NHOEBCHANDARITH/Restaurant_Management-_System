using Microsoft.AspNetCore.Mvc;
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
            return View(await _context.Ingredients.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var ingredient = await _context.Ingredients
                .FirstOrDefaultAsync(x => x.IngredientId == id);

            if (ingredient == null) return NotFound();

            return View(ingredient);
        }

        public IActionResult Create()
        {
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
                return RedirectToAction(nameof(Index));
            }

            return View(ingredient);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ingredient = await _context.Ingredients.FindAsync(id);

            if (ingredient == null) return NotFound();

            return View(ingredient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ingredient ingredient)
        {
            if (id != ingredient.IngredientId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Ingredients.Update(ingredient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(ingredient);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var ingredient = await _context.Ingredients
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
            }

            return RedirectToAction(nameof(Index));
        }
    }
}