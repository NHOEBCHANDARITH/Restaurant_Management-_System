using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management__System.Data;
using Restaurant_Management__System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Controllers
{
    // DTOs for POS Submission via JSON payload
    public class POSSubmissionDto
    {
        public int CustomerId { get; set; }
        public int TableId { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<POSItemDto> Items { get; set; }
    }

    public class POSItemDto
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // NEW: POS System (GET)
        // =========================
        public async Task<IActionResult> POS()
        {
            // Fetch everything needed for the POS screen seamlessly
            ViewBag.Categories = await _context.Categories.OrderBy(c => c.CategoryName).ToListAsync();
            
            ViewBag.MenuItems = await _context.MenuItems
                .Where(m => m.Status == "Available")
                .Include(m => m.Category)
                .OrderBy(m => m.Category.CategoryName)
                .ThenBy(m => m.ItemName)
                .ToListAsync();

            ViewBag.Customers = await _context.Customers.OrderBy(c => c.CustomerName).ToListAsync();
            ViewBag.Tables = await _context.DiningTables.OrderBy(t => t.TableNumber).ToListAsync();
            ViewBag.Users = await _context.Users.OrderBy(u => u.FullName).ToListAsync();

            return View();
        }

        // =========================
        // NEW: Submit POS (POST AJAX)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitPOS([FromBody] POSSubmissionDto payload)
        {
            if (payload == null || payload.Items == null || !payload.Items.Any())
            {
                return BadRequest(new { success = false, message = "Invalid payload or empty cart." });
            }

            // Start an Entity Framework Database Transaction
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Create the Order Record
                var order = new Order
                {
                    CustomerId = payload.CustomerId,
                    TableId = payload.TableId,
                    UserId = payload.UserId,
                    OrderDate = DateTime.Now,
                    TotalAmount = payload.TotalAmount,
                    Status = "Pending" // Initial status sent to kitchen
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync(); // Saves to DB and gets the new auto-generated OrderId

                // 2. Create the Order Details Records
                var orderDetails = new List<OrderDetail>();
                foreach (var item in payload.Items)
                {
                    orderDetails.Add(new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Subtotal = item.Subtotal
                    });
                }

                _context.OrderDetails.AddRange(orderDetails);
                await _context.SaveChangesAsync();

                // 3. Update the Dining Table status to "Occupied" automatically
                var table = await _context.DiningTables.FindAsync(payload.TableId);
                if (table != null && table.Status != "Occupied")
                {
                    table.Status = "Occupied";
                    _context.DiningTables.Update(table);
                    await _context.SaveChangesAsync();
                }

                // Commit transaction - if everything succeeded, save permanently
                await transaction.CommitAsync();

                return Ok(new { success = true, orderId = order.OrderId, message = "Order successfully placed and sent to kitchen!" });
            }
            catch (Exception ex)
            {
                // Rollback transaction on failure
                await transaction.RollbackAsync();
                return StatusCode(500, new { success = false, message = "An error occurred while saving the order.", error = ex.Message });
            }
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
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        // =========================
        // GET: Orders/Details/5
        // =========================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.DiningTable)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null) return NotFound();

            ViewBag.OrderItems = await _context.OrderDetails
                .Include(d => d.MenuItem)
                .Where(d => d.OrderId == id)
                .ToListAsync();

            ViewBag.Payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.OrderId == id);

            return View(order);
        }

        // =========================
        // GET: Orders/Create (Legacy form support)
        // =========================
        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

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
            LoadDropdowns(order.CustomerId, order.TableId, order.UserId);
            return View(order);
        }

        // =========================
        // GET: Orders/Edit/5
        // =========================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            LoadDropdowns(order.CustomerId, order.TableId, order.UserId);
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            if (id != order.OrderId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Orders.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            LoadDropdowns(order.CustomerId, order.TableId, order.UserId);
            return View(order);
        }

        // =========================
        // GET: Orders/Delete/5
        // =========================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.DiningTable)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null) return NotFound();

            return View(order);
        }

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

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }

        private void LoadDropdowns(int? selectedCustomerId = null, int? selectedTableId = null, int? selectedUserId = null)
        {
            ViewBag.CustomerId = new SelectList(_context.Customers.OrderBy(c => c.CustomerName).ToList(), "CustomerId", "CustomerName", selectedCustomerId);
            ViewBag.TableId = new SelectList(_context.DiningTables.OrderBy(t => t.TableNumber).ToList(), "TableId", "TableNumber", selectedTableId);
            ViewBag.UserId = new SelectList(_context.Users.OrderBy(u => u.FullName).ToList(), "UserId", "FullName", selectedUserId);
        }
    }
}