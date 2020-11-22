using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class LeaveController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}