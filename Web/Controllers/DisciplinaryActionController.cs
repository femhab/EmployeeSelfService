using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class DisciplinaryActionController : BaseController
    {
        public IActionResult IssueQuery()
        {
            return View();
        }

        public IActionResult ViewQuery()
        {
            return View();
        }
    }
}