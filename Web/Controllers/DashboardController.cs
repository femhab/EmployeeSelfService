using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Employee()
        {
            return View();
        }

        public IActionResult Approval()
        {
            return View();
        }
    }
}