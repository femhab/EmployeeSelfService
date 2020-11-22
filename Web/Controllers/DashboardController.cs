using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class DashboardController : Controller
    {
        [Route("Dashboard")]
        public IActionResult Employee()
        {
            return View();
        }

        [Route("Approval-Board")]
        public IActionResult Approval()
        {
            return View();
        }
    }
}