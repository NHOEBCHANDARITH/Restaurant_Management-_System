using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management__System.Data;

namespace RestaurantManagementSystem.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.DiningTable)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var payments = await _context.Payments
                .Include(p => p.Order)
                .ToListAsync();

            var reservations = await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.DiningTable)
                .ToListAsync();

            var inventories = await _context.Inventories
                .Include(i => i.Ingredient)
                .ToListAsync();

            var menuItems = await _context.MenuItems
                .Include(m => m.Category)
                .ToListAsync();

            ViewBag.Orders = orders;
            ViewBag.Payments = payments;
            ViewBag.Reservations = reservations;
            ViewBag.Inventories = inventories;
            ViewBag.MenuItems = menuItems;

            ViewBag.TotalSales = orders.Sum(o => o.TotalAmount);
            ViewBag.TotalCash = payments.Where(p => p.PaymentMethod == "Cash").Sum(p => p.Amount);
            ViewBag.TotalCard = payments.Where(p => p.PaymentMethod == "Card" || p.PaymentMethod == "Credit Card").Sum(p => p.Amount);
            ViewBag.TotalKHQR = payments.Where(p => p.PaymentMethod.Contains("QR") || p.PaymentMethod.Contains("Online") || p.PaymentMethod.Contains("Bakong")).Sum(p => p.Amount);

            return View();
        }
    }
}
