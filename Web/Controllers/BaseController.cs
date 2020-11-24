using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using Web.Models;

namespace Web.Controllers
{
    public class BaseController : Controller
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public BadRequestObjectResult ValidationError()
        {
            return BadRequest(new
            {
                status = false,
                message = "Invalid records. Please check for validation error",
                errors = ModelState
            });
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult ErrorPage(Exception ex)
        {
            var route = RouteParam;
            if (ex.GetType() == typeof(UnauthorizedApiException))
            {
                return Forbid();
                //return Unauthorized(JsonConvert.DeserializeObject(ex.Message));
            }
            return View("Views/Shared/Error.cshtml", new ErrorViewModel { Exception = ex, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private (string Controller, string Action) RouteParam
        {
            get
            {
                var param = ControllerContext.RouteData.Values;
                var result = (param["controller"].ToString(), param["action"].ToString());
                return result;
            }
        }
    }
}