using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class AppraisalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Staff()
        {
            return View();
        }
    }
}