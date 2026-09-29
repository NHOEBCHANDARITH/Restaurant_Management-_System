using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management__System.Data;
using Restaurant_Management__System.Models;

namespace RestaurantManagementSystem.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OrderDetails
        public async Task<IActionResult> Index()
        {
            var details = await _context.OrderDetails
                .Include(x => x.Order)
                .Include(x => x.MenuItem)
                .ToListAsync();

            return View(details);
        }

        // GET: OrderDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .Include(x => x.Order)
                .Include(x => x.MenuItem)
                .FirstOrDefaultAsync(x => x.DetailId == id);

            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // GET: OrderDetails/Create
        public IActionResult Create()
        {
            ViewBag.OrderId = new SelectList(
                _context.Orders,
                "OrderId",
                "OrderId"
            );

            ViewBag.ItemId = new SelectList(
                _context.MenuItems,
                "ItemId",
                "ItemName"
            );

            return View();
        }

        // POST: OrderDetails/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                // Calculate Subtotal
                orderDetail.Subtotal =
                    orderDetail.Quantity * orderDetail.UnitPrice;

                _context.OrderDetails.Add(orderDetail);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Reload dropdowns if validation fails
            ViewBag.OrderId = new SelectList(
                _context.Orders,
                "OrderId",
                "OrderId",
                orderDetail.OrderId
            );

            ViewBag.ItemId = new SelectList(
                _context.MenuItems,
                "ItemId",
                "ItemName",
                orderDetail.ItemId
            );

            return View(orderDetail);
        }

        // GET: OrderDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .FindAsync(id);

            if (orderDetail == null)
            {
                return NotFound();
            }

            ViewBag.OrderId = new SelectList(
                _context.Orders,
                "OrderId",
                "OrderId",
                orderDetail.OrderId
            );

            ViewBag.ItemId = new SelectList(
                _context.MenuItems,
                "ItemId",
                "ItemName",
                orderDetail.ItemId
            );

            return View(orderDetail);
        }

        // POST: OrderDetails/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            OrderDetail orderDetail)
        {
            if (id != orderDetail.DetailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Calculate Subtotal
                    orderDetail.Subtotal =
                        orderDetail.Quantity * orderDetail.UnitPrice;

                    _context.OrderDetails.Update(orderDetail);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailExists(orderDetail.DetailId))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            // Reload dropdowns if validation fails
            ViewBag.OrderId = new SelectList(
                _context.Orders,
                "OrderId",
                "OrderId",
                orderDetail.OrderId
            );

            ViewBag.ItemId = new SelectList(
                _context.MenuItems,
                "ItemId",
                "ItemName",
                orderDetail.ItemId
            );

            return View(orderDetail);
        }

        // GET: OrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .Include(x => x.Order)
                .Include(x => x.MenuItem)
                .FirstOrDefaultAsync(x => x.DetailId == id);

            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderDetail = await _context.OrderDetails
                .FindAsync(id);

            if (orderDetail != null)
            {
                _context.OrderDetails.Remove(orderDetail);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Check if OrderDetail exists
        private bool OrderDetailExists(int id)
        {
            return _context.OrderDetails
                .Any(e => e.DetailId == id);
        }
    }
}