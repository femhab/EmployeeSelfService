using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViewModel.Model;
using Web.Helper.Cookie;
using Web.Helper.JWT;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ICookieHelper _cookieHelper;

        public HomeController(IAuthService authService, ICookieHelper cookieHelper)
        {
            _authService = authService;
            _cookieHelper = cookieHelper;
        }

        public IActionResult Login()
        {
            return View();
        }

        [Route("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //action section
        [Route("Signin")]
        [HttpPost("Signin")]
        [AllowAnonymous]
        public async Task<IActionResult> Signin(string email, string password, string returnUrl = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _authService.Login(email, password);
                    if (result.status)
                    {
                        var auth = new ApiTokenModel() { Status = result.status, Message = result.message, Token = result.token, RefreshToken = result.refreshToken };
                        await SignIn(auth);
                        ViewData["Message"] = auth.Message;
                        //await _emailHelper.SendQuickMail(email, "Login Successful", "You just logged in to your account. Contact the support if this is a breach");
                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            return LocalRedirect(returnUrl ?? "/");
                        }

                    }
                    return Json(new
                    {
                        result.status,
                        result.message
                    });
                }


                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task SignIn(ApiTokenModel auth)
        {
            if (auth != null)
            {
                var claims = JwtHelper.GetClaims(auth.Token).ToList();
                var principal = JwtHelper.CreatePrincipal(claims);
                var authProperties = JwtHelper.CreateAuthProperties(auth.Token);

                _cookieHelper.SetAuthCookie(auth.Token, auth.RefreshToken);

                await HttpContext.SignInAsync(principal, authProperties);
            }
        }
    }
}
