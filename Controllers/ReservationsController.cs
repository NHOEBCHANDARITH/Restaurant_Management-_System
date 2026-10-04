using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management__System.Data;
using Restaurant_Management__System.Models;

namespace RestaurantManagementSystem.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var reservations = await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.DiningTable)
                .OrderByDescending(r => r.ReservationDate)
                .ToListAsync();

            return View(reservations);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var reservation = await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.DiningTable)
                .FirstOrDefaultAsync(x => x.ReservationId == id);

            if (reservation == null) return NotFound();

            return View(reservation);
        }

        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Table reservation booked successfully!";
                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns(reservation.CustomerId, reservation.TableId);
            return View(reservation);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return NotFound();

            LoadDropdowns(reservation.CustomerId, reservation.TableId);
            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Reservation reservation)
        {
            if (id != reservation.ReservationId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Reservations.Update(reservation);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Reservation updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.ReservationId))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns(reservation.CustomerId, reservation.TableId);
            return View(reservation);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var reservation = await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.DiningTable)
                .FirstOrDefaultAsync(x => x.ReservationId == id);

            if (reservation == null) return NotFound();

            return View(reservation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Reservation cancelled/removed successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.ReservationId == id);
        }

        private void LoadDropdowns(int? selectedCustomerId = null, int? selectedTableId = null)
        {
            ViewBag.CustomerId = new SelectList(
                _context.Customers.OrderBy(c => c.CustomerName).ToList(),
                "CustomerId",
                "CustomerName",
                selectedCustomerId
            );

            ViewBag.TableId = new SelectList(
                _context.DiningTables.OrderBy(t => t.TableNumber).ToList(),
                "TableId",
                "TableNumber",
                selectedTableId
            );
        }
    }
}