using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management__System.Data;
using Restaurant_Management__System.Models;

namespace RestaurantManagementSystem.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET: Orders
        // =========================
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.DiningTable)
                .Include(o => o.User)
                .ToListAsync();

            return View(orders);
        }


        // =========================
        // GET: Orders/Details/5
        // =========================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.DiningTable)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            ViewBag.OrderItems = await _context.OrderDetails
                .Include(d => d.MenuItem)
                .Where(d => d.OrderId == id)
                .ToListAsync();

            ViewBag.Payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.OrderId == id);

            return View(order);
        }


        // =========================
        // GET: Orders/Create
        // =========================
        public IActionResult Create()
        {
            LoadDropdowns();

            return View();
        }


        // =========================
        // POST: Orders/Create
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Orders.Add(order);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Load dropdowns again
            // if validation fails
            LoadDropdowns(
                order.CustomerId,
                order.TableId,
                order.UserId
            );

            return View(order);
        }


        // =========================
        // GET: Orders/Edit/5
        // =========================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            LoadDropdowns(
                order.CustomerId,
                order.TableId,
                order.UserId
            );

            return View(order);
        }


        // =========================
        // POST: Orders/Edit/5
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Orders.Update(order);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            // Load dropdowns again
            // if validation fails
            LoadDropdowns(
                order.CustomerId,
                order.TableId,
                order.UserId
            );

            return View(order);
        }


        // =========================
        // GET: Orders/Delete/5
        // =========================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.DiningTable)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }


        // =========================
        // POST: Orders/Delete/5
        // =========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order != null)
            {
                _context.Orders.Remove(order);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        // =========================
        // Check Order Exists
        // =========================
        private bool OrderExists(int id)
        {
            return _context.Orders
                .Any(e => e.OrderId == id);
        }


        // =========================
        // Dropdown Data
        // =========================
        private void LoadDropdowns(
            int? selectedCustomerId = null,
            int? selectedTableId = null,
            int? selectedUserId = null)
        {
            ViewBag.CustomerId = new SelectList(
                _context.Customers
                    .OrderBy(c => c.CustomerName)
                    .ToList(),
                "CustomerId",
                "CustomerName",
                selectedCustomerId
            );

            ViewBag.TableId = new SelectList(
                _context.DiningTables
                    .OrderBy(t => t.TableNumber)
                    .ToList(),
                "TableId",
                "TableNumber",
                selectedTableId
            );

            ViewBag.UserId = new SelectList(
                _context.Users
                    .OrderBy(u => u.FullName)
                    .ToList(),
                "UserId",
                "FullName",
                selectedUserId
            );
        }
    }
}