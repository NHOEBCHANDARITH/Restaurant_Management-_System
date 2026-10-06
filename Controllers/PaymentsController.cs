using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management__System.Data;
using Restaurant_Management__System.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Controllers
{
    [Authorize] // Requires login for any action in this controller
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        // Mock Exchange Rate (1 USD = 4050 KHR). In production, this would be in appsettings.json or DB
        private const decimal ExchangeRate = 4050m;

        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: admin/payments
        [Route("admin/payments")]
        public async Task<IActionResult> Index(string currency = "All", string status = "All")
        {
            var query = _context.Payments
                .Include(p => p.Order)
                .ThenInclude(o => o.Customer)
                .AsQueryable();

            if (currency != "All")
            {
                query = query.Where(p => p.Currency == currency);
            }

            if (status != "All")
            {
                query = query.Where(p => p.PaymentStatus == status);
            }

            var payments = await query.OrderByDescending(p => p.PaymentDate).ToListAsync();

            ViewBag.CurrentCurrency = currency;
            ViewBag.CurrentStatus = status;

            return View(payments);
        }

        // GET: admin/payments/create/5
        [Route("admin/payments/create/{orderId}")]
        public async Task<IActionResult> Create(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            ViewBag.Order = order;
            ViewBag.ExchangeRate = ExchangeRate;

            return View(new Payment { OrderId = orderId, Amount = order.TotalAmount, Currency = "USD" });
        }

        [HttpPost]
        [Route("admin/payments/create/{orderId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Payment payment)
        {
            if (ModelState.IsValid)
            {
                payment.PaymentDate = DateTime.Now;
                
                // If payment is marked as KHR, we need to convert the order's USD amount (if standard pricing is USD)
                // For this example, we assume the user enters the exact amount in the chosen currency.
                
                _context.Payments.Add(payment);
                
                // Update Order Status
                var order = await _context.Orders.FindAsync(payment.OrderId);
                if (order != null && payment.PaymentStatus == "Paid")
                {
                    order.Status = "Completed";
                    _context.Orders.Update(order);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Receipt), new { id = payment.PaymentId });
            }

            var orderRef = await _context.Orders.Include(o => o.Customer).FirstOrDefaultAsync(o => o.OrderId == payment.OrderId);
            ViewBag.Order = orderRef;
            ViewBag.ExchangeRate = ExchangeRate;
            return View(payment);
        }

        // GET: admin/payments/receipt/5
        [Route("admin/payments/receipt/{id}")]
        public async Task<IActionResult> Receipt(int? id)
        {
            if (id == null) return NotFound();

            var payment = await _context.Payments
                .Include(p => p.Order)
                .ThenInclude(o => o.Customer)
                .FirstOrDefaultAsync(p => p.PaymentId == id);

            if (payment == null) return NotFound();

            ViewBag.OrderItems = await _context.OrderDetails
                .Include(d => d.MenuItem)
                .Where(d => d.OrderId == payment.OrderId)
                .ToListAsync();

            return View(payment);
        }
    }
}