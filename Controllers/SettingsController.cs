using Microsoft.AspNetCore.Mvc;

namespace RestaurantManagementSystem.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(string restaurantName, string phone, string address, string currency, string taxRate)
        {
            TempData["SuccessMessage"] = "Restaurant operational settings have been saved successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
