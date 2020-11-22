using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("Employee")]
        public IActionResult Enroll()
        {
            return View();
        }

        [Route("Profile")]
        public IActionResult Profile()
        {
            return View();
        }

        [Route("Transfer")]
        public IActionResult Transfer()
        {
            return View();
        }
    }
}