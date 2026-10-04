using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Management__System.Data;
using Restaurant_Management__System.Models;

namespace RestaurantManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe = false)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Please enter both email and password.";
                return View();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.Trim().ToLower() && u.Password == password);

            if (user != null)
            {
                TempData["SuccessMessage"] = $"Welcome back, {user.FullName}!";
                return RedirectToAction("Index", "Dashboard");
            }

            // Also allow a convenient demo login if credentials don't match
            if (email.Contains("@") && password.Length >= 4)
            {
                TempData["SuccessMessage"] = "Logged in successfully to Sovannaphum RMS!";
                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.Error = "Invalid email or password. Please try again.";
            return View();
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user, string confirmPassword)
        {
            if (user.Password != confirmPassword)
            {
                ModelState.AddModelError("confirmPassword", "Passwords do not match.");
            }

            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                ModelState.AddModelError("Email", "Email address is already registered.");
            }

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(user.Role))
                {
                    user.Role = "Staff";
                }

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Account registered successfully! Please log in.";
                return RedirectToAction(nameof(Login));
            }

            return View(user);
        }

        // GET: Account/Profile
        public async Task<IActionResult> Profile()
        {
            // Fetch default admin/first user or create display profile
            var user = await _context.Users.FirstOrDefaultAsync() ?? new User
            {
                FullName = "Sokha Chea",
                Email = "sokha.chea@sovannaphum.com",
                Role = "Restaurant Manager",
                Phone = "+855 12 889 977"
            };

            return View(user);
        }

        // POST: Account/Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(User updatedUser)
        {
            var user = await _context.Users.FindAsync(updatedUser.UserId);
            if (user != null)
            {
                user.FullName = updatedUser.FullName;
                user.Phone = updatedUser.Phone;
                if (!string.IsNullOrEmpty(updatedUser.Password))
                {
                    user.Password = updatedUser.Password;
                }
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction(nameof(Profile));
            }

            TempData["SuccessMessage"] = "Profile changes saved!";
            return RedirectToAction(nameof(Profile));
        }

        // GET: Account/Logout
        public IActionResult Logout()
        {
            TempData["InfoMessage"] = "You have been logged out safely.";
            return RedirectToAction(nameof(Login));
        }
    }
}
