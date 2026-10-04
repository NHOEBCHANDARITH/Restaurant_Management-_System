using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management__System.Data;
using Restaurant_Management__System.Models;

namespace RestaurantManagementSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Revenue calculations
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.DiningTable)
                .Include(o => o.User)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            decimal totalRevenue = orders.Sum(o => o.TotalAmount);
            int totalOrders = orders.Count;
            int pendingOrders = orders.Count(o => o.Status == "Pending" || o.Status == "Preparing");
            int completedOrders = orders.Count(o => o.Status == "Completed");

            // Customers
            int totalCustomers = await _context.Customers.CountAsync();

            // Reservations
            var reservations = await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.DiningTable)
                .OrderByDescending(r => r.ReservationDate)
                .ToListAsync();

            int todayReservations = reservations.Count(r => r.ReservationDate.Date == DateTime.Today);

            // Tables
            var tables = await _context.DiningTables.ToListAsync();
            int totalTables = tables.Count;
            int availableTables = tables.Count(t => t.Status == "Available");
            int occupiedTables = tables.Count(t => t.Status == "Occupied");
            int reservedTables = tables.Count(t => t.Status == "Reserved");

            // Inventory
            var inventories = await _context.Inventories
                .Include(i => i.Ingredient)
                .ToListAsync();

            int lowStockCount = inventories.Count(i => i.Quantity < 10);
            int outOfStockCount = inventories.Count(i => i.Quantity <= 0);

            // Menu Items
            int totalMenuItems = await _context.MenuItems.CountAsync();

            // Populate ViewBag for Dashboard view
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.TotalOrders = totalOrders;
            ViewBag.PendingOrders = pendingOrders;
            ViewBag.CompletedOrders = completedOrders;
            ViewBag.TotalCustomers = totalCustomers;
            ViewBag.TodayReservations = todayReservations;
            ViewBag.TotalTables = totalTables;
            ViewBag.AvailableTables = availableTables;
            ViewBag.OccupiedTables = occupiedTables;
            ViewBag.ReservedTables = reservedTables;
            ViewBag.LowStockCount = lowStockCount;
            ViewBag.OutOfStockCount = outOfStockCount;
            ViewBag.TotalMenuItems = totalMenuItems;

            ViewBag.RecentOrders = orders.Take(5).ToList();
            ViewBag.RecentReservations = reservations.Take(5).ToList();
            ViewBag.LowStockItems = inventories.Where(i => i.Quantity < 10).Take(5).ToList();

            return View("~/Views/Dashboard/Dashboard.cshtml");
        }
    }
}
